using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Service quản lý chi phí, ngân sách và đối soát tài chính sự kiện
    /// Đảm trách quy tắc nghiệp vụ: BR-14 (Ràng buộc chi phí > 0) và BR-15 (Cảnh báo bội chi đa tầng 80% & 100%)
    /// Phụ trách: Thành viên 3 (Thuật toán lõi & BLL) - Phục vụ màn hình F17 và Tab 7 Form F05
    /// </summary>
    public class BudgetService
    {
        /// <summary>
        /// Thuật toán BR-14: Thẩm định giá trị khoản chi
        /// Bắt buộc SoTien phải lớn hơn 0 (Khớp ràng buộc CK_ChiPhi_SoTien trong CSDL)
        /// </summary>
        /// <param name="soTien">Số tiền đề xuất chi</param>
        /// <returns>ServiceResult xác nhận tính hợp lệ</returns>
        public ServiceResult<bool> ValidateExpenseAmount(decimal soTien)
        {
            if (soTien <= 0)
            {
                return ServiceResult<bool>.Failed(
                    "Số tiền chi phí bắt buộc phải lớn hơn 0 theo quy tắc BR-14.",
                    new List<string> { $"Số tiền nhập vào: {soTien:N0} VNĐ <= 0" }
                );
            }

            return ServiceResult<bool>.Success(true, "Số tiền chi phí hợp lệ.");
        }

        /// <summary>
        /// Thuật toán BR-15: Tính toán tỷ lệ sử dụng ngân sách và trạng thái cảnh báo bội chi
        /// Công thức: TyLeSuDung = ((TongThucChiHienTai + KhoanChiSapThem) / NganSachDuKien) * 100%
        /// - Dưới 80%: Ngưỡng an toàn (xanh lá)
        /// - Từ 80% đến 100%: Cảnh báo chạm ngưỡng (vàng/cam)
        /// - Lớn hơn 100%: Cảnh báo bội chi ngân sách (đỏ)
        /// </summary>
        /// <param name="maSuKien">Mã sự kiện</param>
        /// <param name="nganSachDuKien">Ngân sách dự kiến được phê duyệt</param>
        /// <param name="tongChiHienTai">Tổng chi phí thực tế đã chi</param>
        /// <param name="khoanChiSapThem">Khoản chi dự kiến sắp bổ sung (mặc định 0)</param>
        /// <returns>Đối tượng BudgetAlertDto mang đầy đủ thông số hiển thị và trạng thái cảnh báo</returns>
        public BudgetAlertDto CalculateBudgetStatus(
            string maSuKien,
            decimal nganSachDuKien,
            decimal tongChiHienTai,
            decimal khoanChiSapThem = 0)
        {
            return new BudgetAlertDto
            {
                MaSuKien = maSuKien,
                NganSachDuKien = nganSachDuKien,
                TongThucChiHienTai = tongChiHienTai,
                KhoanChiSapThem = khoanChiSapThem
            };
        }

        /// <summary>
        /// Thẩm định toàn diện trước khi thêm một khoản chi mới cho sự kiện
        /// Áp dụng cơ chế cảnh báo mềm (Soft Warning) theo tài liệu thiết kế:
        /// - Nếu số tiền <= 0 -> Chặn cứng (Lỗi)
        /// - Nếu chạm ngưỡng 80% hoặc vượt 100% -> Vẫn cho phép ghi nhận nhưng gắn cờ cảnh báo
        /// </summary>
        /// <param name="maSuKien">Mã sự kiện</param>
        /// <param name="khoanChiMoi">Số tiền của khoản chi mới</param>
        /// <param name="khoanMucChi">Tên khoản mục chi</param>
        /// <param name="nganSachDuKien">Ngân sách dự kiến của sự kiện</param>
        /// <param name="danhSachChiHienTai">Danh sách các khoản chi hiện có của sự kiện</param>
        /// <returns>ServiceResult chứa BudgetAlertDto kèm cờ cảnh báo tương ứng</returns>
        public ServiceResult<BudgetAlertDto> CheckBudgetAlert(
            string maSuKien,
            decimal khoanChiMoi,
            string khoanMucChi,
            decimal nganSachDuKien,
            IEnumerable<ChiPhiSuKien> danhSachChiHienTai)
        {
            // 1. Thẩm định BR-14 (Số tiền chi > 0)
            var amountCheck = ValidateExpenseAmount(khoanChiMoi);
            if (!amountCheck.IsSuccess)
            {
                return ServiceResult<BudgetAlertDto>.Failed(amountCheck.Message, amountCheck.Errors);
            }

            if (string.IsNullOrWhiteSpace(khoanMucChi))
            {
                return ServiceResult<BudgetAlertDto>.Failed("Khoản mục chi không được để trống.");
            }

            // 2. Tính toán tổng chi
            decimal tongChiHienTai = danhSachChiHienTai?.Sum(c => c.SoTien) ?? 0m;
            var statusDto = CalculateBudgetStatus(maSuKien, nganSachDuKien, tongChiHienTai, khoanChiMoi);

            // 3. Phân tầng thông báo theo ngưỡng BR-15
            if (statusDto.IsOverBudget)
            {
                return ServiceResult<BudgetAlertDto>.Success(
                    statusDto,
                    $"[CẢNH BÁO BỘI CHI] {statusDto.ThongBaoTrangThai}"
                );
            }

            if (statusDto.IsWarning80)
            {
                return ServiceResult<BudgetAlertDto>.Success(
                    statusDto,
                    $"[CẢNH BÁO TIỆM CẬN NGÂN SÁCH] {statusDto.ThongBaoTrangThai}"
                );
            }

            return ServiceResult<BudgetAlertDto>.Success(statusDto, statusDto.ThongBaoTrangThai);
        }

        /// <summary>
        /// Tính toán tổng ngân sách còn lại của sự kiện theo công thức:
        /// NganSachConLai = NganSachDuKien - TongChiThucTe
        /// </summary>
        public decimal CalculateRemainingBudget(decimal nganSachDuKien, decimal tongChiThucTe)
        {
            return nganSachDuKien - tongChiThucTe;
        }
    }
}
