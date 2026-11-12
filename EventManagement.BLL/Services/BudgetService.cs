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
    public class BudgetService : IBudgetService
    {
        // Danh sách sự kiện mẫu kèm ngân sách dự kiến (khớp kịch bản tài trợ và seed data)
        private static readonly List<(string MaSuKien, string TenSuKien, decimal NganSachDuKien)> _events = new()
        {
            ("EVT202601", "Hội nghị Công nghệ Vietnam Tech Summit 2026", 210000000m),
            ("EVT202602", "Workshop Thực chiến GenAI & Prompt Engineering 2026", 80000000m),
            ("EVT202603", "Hội nghị Khoa học Quốc tế về Công nghệ Phần mềm & IoT", 150000000m),
            ("EVT202604", "Diễn đàn Khởi nghiệp & Đổi mới Sáng tạo Quốc gia 2026", 120000000m)
        };

        // Danh sách phiếu chi khởi tạo từ scripts/seed_data_member3.sql
        private static readonly List<ExpenseDto> _expenses = new()
        {
            new ExpenseDto
            {
                MaChiPhi = "CP001",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                KhoanMucChi = "Thuê âm thanh & Ánh sáng sân khấu Grand Ballroom",
                SoTien = 35000000m,
                NgayChi = new DateTime(2026, 10, 15),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Hợp đồng âm thanh sự kiện",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP002",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                KhoanMucChi = "Tiệc trà Teabreak đón tiếp khách mời VIP",
                SoTien = 15000000m,
                NgayChi = new DateTime(2026, 10, 18),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Bao gồm bánh ngọt, trà và cafe",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP003",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                KhoanMucChi = "In ấn Backdrop, Standee & Thẻ đeo đại biểu",
                SoTien = 25000000m,
                NgayChi = new DateTime(2026, 10, 25),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "In ấn ấn phẩm nhận diện thương hiệu",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP004",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                KhoanMucChi = "Thuê thiết bị Lab & Tài khoản thực hành Cloud",
                SoTien = 20000000m,
                NgayChi = new DateTime(2026, 10, 28),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Hạ tầng thực hành cho 100 học viên",
                TrangThai = "Đã duyệt"
            }
        };

        #region 1. CÁC HÀM THUẬT TOÁN BR-14 & BR-15 (TƯƠNG THÍCH UNIT TESTS)

        /// <summary>
        /// Thuật toán BR-14: Thẩm định giá trị khoản chi
        /// Bắt buộc SoTien phải lớn hơn 0 (Khớp ràng buộc CK_ChiPhi_SoTien trong CSDL)
        /// </summary>
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
        /// </summary>
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
        /// </summary>
        public ServiceResult<BudgetAlertDto> CheckBudgetAlert(
            string maSuKien,
            decimal khoanChiMoi,
            string khoanMucChi,
            decimal nganSachDuKien,
            IEnumerable<ChiPhiSuKien> danhSachChiHienTai)
        {
            var amountCheck = ValidateExpenseAmount(khoanChiMoi);
            if (!amountCheck.IsSuccess)
            {
                return ServiceResult<BudgetAlertDto>.Failed(amountCheck.Message, amountCheck.Errors);
            }

            if (string.IsNullOrWhiteSpace(khoanMucChi))
            {
                return ServiceResult<BudgetAlertDto>.Failed("Khoản mục chi không được để trống.");
            }

            decimal tongChiHienTai = danhSachChiHienTai?.Sum(c => c.SoTien) ?? 0m;
            var statusDto = CalculateBudgetStatus(maSuKien, nganSachDuKien, tongChiHienTai, khoanChiMoi);

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

        public decimal CalculateRemainingBudget(decimal nganSachDuKien, decimal tongChiThucTe)
        {
            return nganSachDuKien - tongChiThucTe;
        }

        #endregion

        #region 2. CÁC HÀM CRUD & TÁC NGHIỆP PHỤC VỤ FORM F17

        public List<(string MaSuKien, string TenSuKien, decimal NganSachDuKien)> GetAvailableEvents()
        {
            return _events.ToList();
        }

        public ServiceResult<List<ExpenseDto>> GetExpensesByEvent(string? maSuKien)
        {
            var query = _expenses.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(maSuKien))
            {
                query = query.Where(e => string.Equals(e.MaSuKien, maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            var list = query.OrderByDescending(e => e.NgayChi).Select(CloneDto).ToList();
            return ServiceResult<List<ExpenseDto>>.Success(list);
        }

        public ServiceResult<ExpenseDto> GetExpenseById(string maChiPhi)
        {
            if (string.IsNullOrWhiteSpace(maChiPhi))
                return ServiceResult<ExpenseDto>.Failed("Mã chi phí không được để trống.");

            var item = _expenses.FirstOrDefault(e => string.Equals(e.MaChiPhi, maChiPhi.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item == null)
                return ServiceResult<ExpenseDto>.Failed($"Không tìm thấy phiếu chi có mã '{maChiPhi}'.");

            return ServiceResult<ExpenseDto>.Success(CloneDto(item));
        }

        public ServiceResult<BudgetAlertDto> GetBudgetStatus(string maSuKien, decimal khoanChiSapThem = 0)
        {
            if (string.IsNullOrWhiteSpace(maSuKien))
                return ServiceResult<BudgetAlertDto>.Failed("Vui lòng chọn sự kiện.");

            var evt = _events.FirstOrDefault(e => string.Equals(e.MaSuKien, maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            decimal budget = evt != default ? evt.NganSachDuKien : 0m;

            decimal totalSpent = _expenses
                .Where(e => string.Equals(e.MaSuKien, maSuKien.Trim(), StringComparison.OrdinalIgnoreCase))
                .Sum(e => e.SoTien);

            var status = CalculateBudgetStatus(maSuKien, budget, totalSpent, khoanChiSapThem);
            return ServiceResult<BudgetAlertDto>.Success(status);
        }

        public ServiceResult<BudgetAlertDto> AddExpense(ExpenseDto dto)
        {
            if (dto == null)
                return ServiceResult<BudgetAlertDto>.Failed("Dữ liệu phiếu chi không được để trống.");

            if (string.IsNullOrWhiteSpace(dto.MaSuKien))
                return ServiceResult<BudgetAlertDto>.Failed("Vui lòng chọn sự kiện áp dụng cho khoản chi.");

            if (string.IsNullOrWhiteSpace(dto.KhoanMucChi))
                return ServiceResult<BudgetAlertDto>.Failed("Tên khoản mục chi không được để trống.");

            var amountCheck = ValidateExpenseAmount(dto.SoTien);
            if (!amountCheck.IsSuccess)
                return ServiceResult<BudgetAlertDto>.Failed(amountCheck.Message, amountCheck.Errors);

            if (string.IsNullOrWhiteSpace(dto.MaChiPhi))
                dto.MaChiPhi = GetNextExpenseCode();
            else
                dto.MaChiPhi = dto.MaChiPhi.Trim().ToUpper();

            if (_expenses.Any(e => string.Equals(e.MaChiPhi, dto.MaChiPhi, StringComparison.OrdinalIgnoreCase)))
                return ServiceResult<BudgetAlertDto>.Failed($"Mã phiếu chi '{dto.MaChiPhi}' đã tồn tại trong hệ thống.");

            var evt = _events.FirstOrDefault(e => string.Equals(e.MaSuKien, dto.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            dto.TenSuKien = evt != default ? evt.TenSuKien : dto.MaSuKien;

            if (dto.NgayChi == default)
                dto.NgayChi = DateTime.Now;

            if (string.IsNullOrWhiteSpace(dto.TrangThai))
                dto.TrangThai = "Đã duyệt";

            _expenses.Add(CloneDto(dto));
            DataSyncNotifier.NotifyExpenseChanged(dto.MaSuKien, dto.SoTien);

            var status = GetBudgetStatus(dto.MaSuKien);
            return ServiceResult<BudgetAlertDto>.Success(status.Data!, "Thêm mới phiếu chi thành công!");
        }

        public ServiceResult<BudgetAlertDto> UpdateExpense(ExpenseDto dto)
        {
            if (dto == null)
                return ServiceResult<BudgetAlertDto>.Failed("Dữ liệu phiếu chi không được để trống.");

            if (string.IsNullOrWhiteSpace(dto.MaChiPhi))
                return ServiceResult<BudgetAlertDto>.Failed("Mã phiếu chi không được để trống.");

            var existing = _expenses.FirstOrDefault(e => string.Equals(e.MaChiPhi, dto.MaChiPhi.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return ServiceResult<BudgetAlertDto>.Failed($"Không tìm thấy phiếu chi '{dto.MaChiPhi}' để cập nhật.");

            var amountCheck = ValidateExpenseAmount(dto.SoTien);
            if (!amountCheck.IsSuccess)
                return ServiceResult<BudgetAlertDto>.Failed(amountCheck.Message, amountCheck.Errors);

            existing.KhoanMucChi = dto.KhoanMucChi?.Trim() ?? existing.KhoanMucChi;
            existing.SoTien = dto.SoTien;
            existing.NgayChi = dto.NgayChi != default ? dto.NgayChi : existing.NgayChi;
            existing.NguoiLapPhieu = dto.NguoiLapPhieu?.Trim() ?? existing.NguoiLapPhieu;
            existing.TenNguoiLapPhieu = dto.TenNguoiLapPhieu?.Trim() ?? existing.TenNguoiLapPhieu;
            existing.DienGiai = dto.DienGiai?.Trim();
            if (!string.IsNullOrWhiteSpace(dto.TrangThai))
                existing.TrangThai = dto.TrangThai.Trim();

            var status = GetBudgetStatus(existing.MaSuKien);
            DataSyncNotifier.NotifyExpenseChanged(existing.MaSuKien, dto.SoTien);
            return ServiceResult<BudgetAlertDto>.Success(status.Data!, "Cập nhật phiếu chi thành công!");
        }

        public ServiceResult DeleteExpense(string maChiPhi)
        {
            if (string.IsNullOrWhiteSpace(maChiPhi))
                return ServiceResult.Failed("Mã chi phí không được để trống.");

            var existing = _expenses.FirstOrDefault(e => string.Equals(e.MaChiPhi, maChiPhi.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return ServiceResult.Failed($"Không tìm thấy phiếu chi '{maChiPhi}' để xóa.");

            _expenses.Remove(existing);
            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success("Xóa phiếu chi thành công.");
        }

        public string GetNextExpenseCode()
        {
            int maxId = 0;
            foreach (var item in _expenses)
            {
                if (item.MaChiPhi.StartsWith("CP", StringComparison.OrdinalIgnoreCase)
                    && int.TryParse(item.MaChiPhi.Substring(2), out int num))
                {
                    if (num > maxId) maxId = num;
                }
            }
            return $"CP{(maxId + 1):D3}";
        }

        private static ExpenseDto CloneDto(ExpenseDto src)
        {
            return new ExpenseDto
            {
                MaChiPhi = src.MaChiPhi,
                MaSuKien = src.MaSuKien,
                TenSuKien = src.TenSuKien,
                KhoanMucChi = src.KhoanMucChi,
                DienGiai = src.DienGiai,
                SoTien = src.SoTien,
                NgayChi = src.NgayChi,
                NguoiLapPhieu = src.NguoiLapPhieu,
                TenNguoiLapPhieu = src.TenNguoiLapPhieu,
                TrangThai = src.TrangThai
            };
        }

        #endregion
    }
}
