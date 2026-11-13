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
            ("EVT202604", "Diễn đàn Khởi nghiệp & Đổi mới Sáng tạo Quốc gia 2026", 120000000m),
            ("EVT202605", "Giải đấu Thể thao Điện tử Sinh viên Toàn quốc (eSports Cup)", 120000000m),
            ("EVT202606", "Ngày hội Việc làm & Kết nối Doanh nghiệp IT Career Fair", 160000000m),
            ("EVT202607", "Hội thảo Bảo mật Đám mây & Ứng phó Sự cố An ninh mạng", 95000000m)
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
            },
            new ExpenseDto
            {
                MaChiPhi = "CP005",
                MaSuKien = "EVT202607",
                TenSuKien = "Hội thảo Bảo mật Đám mây & Ứng phó Sự cố An ninh mạng",
                KhoanMucChi = "Đặt cọc hội trường Hội trường Thống Nhất",
                SoTien = 18000000m,
                NgayChi = new DateTime(2026, 09, 20),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Chi phí đặt cọc giữ chỗ trước khi sự kiện hoãn",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP006",
                MaSuKien = "EVT202607",
                TenSuKien = "Hội thảo Bảo mật Đám mây & Ứng phó Sự cố An ninh mạng",
                KhoanMucChi = "Thiết kế ấn phẩm truyền thông & Website hội thảo",
                SoTien = 8000000m,
                NgayChi = new DateTime(2026, 09, 25),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Hợp đồng thiết kế bộ nhận diện trực tuyến",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP015",
                MaSuKien = "EVT202607",
                TenSuKien = "Hội thảo Bảo mật Đám mây & Ứng phó Sự cố An ninh mạng",
                KhoanMucChi = "Thuê hệ thống dịch cabin & tai nghe đa ngôn ngữ",
                SoTien = 12000000m,
                NgayChi = new DateTime(2026, 09, 28),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Hệ thống tai nghe không dây cho 200 đại biểu",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP016",
                MaSuKien = "EVT202607",
                TenSuKien = "Hội thảo Bảo mật Đám mây & Ứng phó Sự cố An ninh mạng",
                KhoanMucChi = "Tiệc trà & Teabreak cao cấp phiên sáng chiều",
                SoTien = 9500000m,
                NgayChi = new DateTime(2026, 10, 01),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Phục vụ đại biểu và chuyên gia bảo mật",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP017",
                MaSuKien = "EVT202607",
                TenSuKien = "Hội thảo Bảo mật Đám mây & Ứng phó Sự cố An ninh mạng",
                KhoanMucChi = "Quà tặng lưu niệm & Kỷ niệm chương diễn giả",
                SoTien = 6500000m,
                NgayChi = new DateTime(2026, 10, 02),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Bộ kỷ niệm chương pha lê và quà công nghệ",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP018",
                MaSuKien = "EVT202607",
                TenSuKien = "Hội thảo Bảo mật Đám mây & Ứng phó Sự cố An ninh mạng",
                KhoanMucChi = "Băng rôn, Backdrop sân khấu & Standee đón khách",
                SoTien = 7000000m,
                NgayChi = new DateTime(2026, 10, 03),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Thi công trang trí sảnh chính và hội trường",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP019",
                MaSuKien = "EVT202607",
                TenSuKien = "Hội thảo Bảo mật Đám mây & Ứng phó Sự cố An ninh mạng",
                KhoanMucChi = "Bản quyền tài liệu bảo mật & học liệu số",
                SoTien = 10000000m,
                NgayChi = new DateTime(2026, 10, 04),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Phát hành tài liệu chuyên đề số cho khách mời",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP007",
                MaSuKien = "EVT202603",
                TenSuKien = "Triển lãm Tranh & Công nghệ số \"Nghệ thuật tương lai\"",
                KhoanMucChi = "Thuê khung gian trưng bày & Hệ thống đèn chiếu",
                SoTien = 35000000m,
                NgayChi = new DateTime(2026, 10, 15),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Thi công không gian triển lãm nghệ thuật số",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP008",
                MaSuKien = "EVT202603",
                TenSuKien = "Triển lãm Tranh & Công nghệ số \"Nghệ thuật tương lai\"",
                KhoanMucChi = "In ấn Catalogue & Thẻ đeo khách VIP",
                SoTien = 15000000m,
                NgayChi = new DateTime(2026, 10, 20),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Bộ tài liệu giới thiệu tác phẩm nghệ sĩ",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP009",
                MaSuKien = "EVT202604",
                TenSuKien = "Lễ Ra Mắt Sản Phẩm Mới: Nền Tảng Cloud AI 2026",
                KhoanMucChi = "Thuê màn hình LED P2.5 & Âm thanh biểu diễn",
                SoTien = 25000000m,
                NgayChi = new DateTime(2026, 11, 01),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Hệ thống trình chiếu Demo sản phẩm công nghệ",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP010",
                MaSuKien = "EVT202604",
                TenSuKien = "Lễ Ra Mắt Sản Phẩm Mới: Nền Tảng Cloud AI 2026",
                KhoanMucChi = "Tiệc Teabreak & Quà tặng khách mời ra mắt",
                SoTien = 12000000m,
                NgayChi = new DateTime(2026, 11, 05),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Phục vụ 250 khách mời và báo chí",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP011",
                MaSuKien = "EVT202605",
                TenSuKien = "Hackathon Lập trình Web & AI \"Sáng tạo tương lai 2026\"",
                KhoanMucChi = "Giải thưởng cuộc thi Hackathon 48h",
                SoTien = 40000000m,
                NgayChi = new DateTime(2026, 11, 10),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Trao giải Nhất, Nhì, Ba cho các đội thi lập trình",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP012",
                MaSuKien = "EVT202605",
                TenSuKien = "Hackathon Lập trình Web & AI \"Sáng tạo tương lai 2026\"",
                KhoanMucChi = "Suất ăn & Đồ uống 48h cho thí sinh và mentor",
                SoTien = 20000000m,
                NgayChi = new DateTime(2026, 11, 12),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Phục vụ 200 lập trình viên thức đêm thi đấu",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP013",
                MaSuKien = "EVT202606",
                TenSuKien = "Diễn Đàn Việc Làm & Khởi Nghiệp Công Nghệ Trẻ 2026",
                KhoanMucChi = "Dựng 50 gian hàng phỏng vấn tuyển dụng",
                SoTien = 50000000m,
                NgayChi = new DateTime(2026, 11, 15),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Lắp đặt khung booth tiêu chuẩn cho doanh nghiệp",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP014",
                MaSuKien = "EVT202606",
                TenSuKien = "Diễn Đàn Việc Làm & Khởi Nghiệp Công Nghệ Trẻ 2026",
                KhoanMucChi = "Truyền thông đa kênh & Livestream sự kiện",
                SoTien = 30000000m,
                NgayChi = new DateTime(2026, 11, 18),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Chi phí quảng bá tiếp cận 10.000 sinh viên",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP020",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                KhoanMucChi = "Tài khoản thực hành API OpenAI & Claude",
                SoTien = 6000000m,
                NgayChi = new DateTime(2026, 10, 26),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Cấp token API cho học viên thực hành tại lớp",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP021",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                KhoanMucChi = "Tiệc nhẹ Teabreak & Cà phê cho học viên",
                SoTien = 4000000m,
                NgayChi = new DateTime(2026, 10, 27),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Phục vụ 2 buổi học cuối tuần",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP022",
                MaSuKien = "EVT202603",
                TenSuKien = "Triển lãm Tranh & Công nghệ số \"Nghệ thuật tương lai\"",
                KhoanMucChi = "Bảo hiểm tác phẩm nghệ thuật số & Thiết bị",
                SoTien = 8000000m,
                NgayChi = new DateTime(2026, 10, 18),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Bảo hiểm rủi ro trong suốt thời gian triển lãm",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP023",
                MaSuKien = "EVT202604",
                TenSuKien = "Lễ Ra Mắt Sản Phẩm Mới: Nền Tảng Cloud AI 2026",
                KhoanMucChi = "Quà tặng công nghệ trải nghiệm cho khách VIP",
                SoTien = 7500000m,
                NgayChi = new DateTime(2026, 11, 02),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Bộ quà tặng cao cấp kỷ niệm lễ ra mắt",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP024",
                MaSuKien = "EVT202605",
                TenSuKien = "Hackathon Lập trình Web & AI \"Sáng tạo tương lai 2026\"",
                KhoanMucChi = "Áo phông đồng phục & Balo thí sinh Hackathon",
                SoTien = 9000000m,
                NgayChi = new DateTime(2026, 11, 11),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Đồng phục cho 200 thí sinh và tình nguyện viên",
                TrangThai = "Đã duyệt"
            },
            new ExpenseDto
            {
                MaChiPhi = "CP025",
                MaSuKien = "EVT202606",
                TenSuKien = "Diễn Đàn Việc Làm & Khởi Nghiệp Công Nghệ Trẻ 2026",
                KhoanMucChi = "Hệ thống âm thanh hội trường & Sân khấu tuyển dụng",
                SoTien = 15000000m,
                NgayChi = new DateTime(2026, 11, 16),
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                DienGiai = "Trang thiết bị sân khấu tọa đàm doanh nghiệp",
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
                    "Số tiền chi phí bắt buộc phải lớn hơn 0 VNĐ.",
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
