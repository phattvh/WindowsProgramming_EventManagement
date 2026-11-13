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
        private static readonly List<(string MaSuKien, string TenSuKien, decimal NganSachDuKien)> _events = new();
        private static readonly List<ExpenseDto> _expenses = new();

        private static readonly Dictionary<string, decimal> _defaultEventBudgets = new(StringComparer.OrdinalIgnoreCase)
        {
            ["EVT202601"] = 210000000m,
            ["EVT202602"] = 80000000m,
            ["EVT202603"] = 150000000m,
            ["EVT202604"] = 120000000m,
            ["EVT202605"] = 120000000m,
            ["EVT202606"] = 160000000m,
            ["EVT202607"] = 95000000m,
            ["EVT202608"] = 100000000m,
            ["EVT202609"] = 150000000m,
            ["EVT202610"] = 100000000m
        };

        private static decimal ResolveDefaultBudget(string maSuKien, decimal existingBudget)
        {
            if (existingBudget > 0) return existingBudget;
            if (_defaultEventBudgets.TryGetValue(maSuKien, out decimal defaultVal)) return defaultVal;
            return 100000000m; // Ngân sách cơ bản 100 triệu mặc định cho mỗi sự kiện
        }

        private void EnsureLoadedFromDb()
        {
            if (_expenses.Count == 0 && _events.Count == 0)
            {
                try
                {
                    using var db = new EventManagement.DAL.AppDbContext();
                    var evts = db.SuKiens.Select(s => new { s.MaSuKien, s.TenSuKien, s.NganSachDuKien }).ToList();
                    foreach (var e in evts)
                    {
                        if (!_events.Any(x => x.MaSuKien == e.MaSuKien))
                        {
                            decimal budget = ResolveDefaultBudget(e.MaSuKien, e.NganSachDuKien);
                            _events.Add((e.MaSuKien, e.TenSuKien, budget));
                        }
                    }

                    var exps = db.ChiPhiSuKiens.Select(c => new ExpenseDto
                    {
                        MaChiPhi = c.MaChiPhi,
                        MaSuKien = c.MaSuKien,
                        TenSuKien = c.SuKien != null ? c.SuKien.TenSuKien : c.MaSuKien,
                        KhoanMucChi = c.KhoanMucChi,
                        SoTien = c.SoTien,
                        NgayChi = c.NgayChi,
                        NguoiLapPhieu = c.NguoiDuyet ?? "admin",
                        TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                        DienGiai = c.GhiChu,
                        TrangThai = "Đã duyệt"
                    }).ToList();

                    if (exps.Any())
                    {
                        _expenses.AddRange(exps);
                    }
                }
                catch { }

                SeedDefaultExpensesForEvents();
            }
        }

        private static void SeedDefaultExpensesForEvents()
        {
            var demoEvents = new (string Ma, string Ten, decimal NganSach)[]
            {
                ("EVT202601", "Hội nghị Công nghệ Vietnam Tech Summit 2026", 210000000m),
                ("EVT202602", "Đại nhạc hội Sinh viên YouthWave 2026", 80000000m),
                ("EVT202603", "Hội thảo Khởi nghiệp Trẻ & Đổi mới Sáng tạo 2026", 150000000m),
                ("EVT202604", "Triển lãm Thiết bị & Trải nghiệm AI Expo 2026", 120000000m),
                ("EVT202605", "Workshop Thực chiến GenAI & Agentic Coding 2026", 120000000m),
                ("EVT202606", "Giải thi đấu Thể thao Điện tử Sinh viên Toàn quốc 2026", 160000000m),
                ("EVT202607", "Ngày hội Việc làm & Kết nối Doanh nghiệp IT Career Fair 2026", 95000000m),
                ("EVT202608", "Diễn đàn An ninh mạng Quốc gia Cybershield 2026", 100000000m),
                ("EVT202609", "Hội nghị Khoa học Quốc tế về Trí tuệ Nhân tạo & Bán dẫn 2026", 150000000m),
                ("EVT202610", "Lễ hội Văn hóa Ẩm thực & Âm nhạc Mùa Thu 2026", 100000000m),
                ("EVT202611", "Diễn đàn Chuyển đổi số Doanh nghiệp Vừa và Nhỏ 2026", 100000000m),
                ("EVT202612", "Giải Marathon Sinh viên Vì Cộng đồng Green Run 2026", 100000000m)
            };

            foreach (var d in demoEvents)
            {
                if (!_events.Any(x => x.MaSuKien == d.Ma))
                {
                    _events.Add((d.Ma, d.Ten, d.NganSach));
                }
            }

            int nextId = 100;
            foreach (var evt in _events.ToList())
            {
                if (!_expenses.Any(x => x.MaSuKien == evt.MaSuKien))
                {
                    decimal budget = evt.NganSachDuKien > 0 ? evt.NganSachDuKien : 100000000m;
                    decimal spend1 = Math.Round(budget * 0.25m, 0);
                    decimal spend2 = Math.Round(budget * 0.15m, 0);

                    _expenses.Add(new ExpenseDto
                    {
                        MaChiPhi = $"CP{nextId++}",
                        MaSuKien = evt.MaSuKien,
                        TenSuKien = evt.TenSuKien,
                        KhoanMucChi = "Thuê địa điểm & Hội trường tổ chức",
                        SoTien = spend1,
                        NgayChi = DateTime.Now.AddDays(-15),
                        NguoiLapPhieu = "admin",
                        TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                        TrangThai = "Đã duyệt",
                        DienGiai = "Thanh toán trọn gói chi phí mặt bằng và hội trường"
                    });

                    _expenses.Add(new ExpenseDto
                    {
                        MaChiPhi = $"CP{nextId++}",
                        MaSuKien = evt.MaSuKien,
                        TenSuKien = evt.TenSuKien,
                        KhoanMucChi = "In ấn Backdrop, Standee & Truyền thông",
                        SoTien = spend2,
                        NgayChi = DateTime.Now.AddDays(-7),
                        NguoiLapPhieu = "admin",
                        TenNguoiLapPhieu = "Quản trị viên Hệ thống",
                        TrangThai = "Đã duyệt",
                        DienGiai = "In ấn ấn phẩm và thiết kế tài liệu cho người tham dự"
                    });
                }
            }
        }

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
            EnsureLoadedFromDb();
            return _events.ToList();
        }

        public ServiceResult<List<ExpenseDto>> GetExpensesByEvent(string? maSuKien)
        {
            EnsureLoadedFromDb();
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
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maChiPhi))
                return ServiceResult<ExpenseDto>.Failed("Mã chi phí không được để trống.");

            var item = _expenses.FirstOrDefault(e => string.Equals(e.MaChiPhi, maChiPhi.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item == null)
                return ServiceResult<ExpenseDto>.Failed($"Không tìm thấy phiếu chi có mã '{maChiPhi}'.");

            return ServiceResult<ExpenseDto>.Success(CloneDto(item));
        }

        public ServiceResult<BudgetAlertDto> GetBudgetStatus(string maSuKien, decimal khoanChiSapThem = 0)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maSuKien))
                return ServiceResult<BudgetAlertDto>.Failed("Vui lòng chọn sự kiện.");

            var evt = _events.FirstOrDefault(e => string.Equals(e.MaSuKien, maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            decimal budget = evt != default && evt.NganSachDuKien > 0 ? evt.NganSachDuKien : ResolveDefaultBudget(maSuKien, 0m);

            decimal totalSpent = _expenses
                .Where(e => string.Equals(e.MaSuKien, maSuKien.Trim(), StringComparison.OrdinalIgnoreCase))
                .Sum(e => e.SoTien);

            var status = CalculateBudgetStatus(maSuKien, budget, totalSpent, khoanChiSapThem);
            return ServiceResult<BudgetAlertDto>.Success(status);
        }

        public ServiceResult<BudgetAlertDto> AddExpense(ExpenseDto dto)
        {
            EnsureLoadedFromDb();
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
            EnsureLoadedFromDb();
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
            EnsureLoadedFromDb();
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
            EnsureLoadedFromDb();
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
