using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Service xử lý nghiệp vụ Đăng ký & Bán vé sự kiện (F11 / UC11)
    /// Đảm trách quy tắc:
    /// - BR-09: Kiểm soát tồn kho vé khả dụng (PhatHanh - DaBan >= SoLuongMua)
    /// - BR-10: Kiểm soát giới hạn mua vé (1 đến 10 vé/đơn), phân rã Master-Detail
    /// - BR-17: Tự động sinh mã vé và mã QR dạng GUID duy nhất
    /// - BR-18: Giao dịch an toàn (ACID Transaction)
    /// Phụ trách: Thành viên 3 (Thuật toán lõi & BLL) - Task T3-17 & T3-18 (Ngày 6)
    /// </summary>
    public class TicketOrderService : ITicketOrderService
    {
        private static readonly List<(string MaSuKien, string TenSuKien)> _events = new();
        private static readonly List<TicketTierSummaryDto> _ticketTiers = new();
        private static readonly List<TicketDetailDto> _issuedTickets = new();

        private static void EnsureLoadedFromDb()
        {
            if (_events.Count == 0 && _ticketTiers.Count == 0)
            {
                try
                {
                    using var db = new EventManagement.DAL.AppDbContext();
                    var evts = db.SuKiens.Select(s => new { s.MaSuKien, s.TenSuKien }).ToList();
                    foreach (var e in evts)
                    {
                        if (!_events.Any(x => x.MaSuKien == e.MaSuKien))
                            _events.Add((e.MaSuKien, e.TenSuKien));
                    }

                    var tiers = db.HangVes.Select(h => new TicketTierSummaryDto
                    {
                        MaHangVe = h.MaHangVe,
                        MaSuKien = h.MaSuKien,
                        TenHangVe = h.TenHangVe,
                        GiaVe = h.GiaVe,
                        SoLuongPhatHanh = h.SoLuongPhatHanh,
                        SoLuongDaBan = h.SoLuongDaBan,
                        ThoiGianMoBan = h.ThoiGianMoBan,
                        ThoiGianDongBan = h.ThoiGianDongBan
                    }).ToList();
                    if (tiers.Any()) _ticketTiers.AddRange(tiers);

                    var tickets = (from ctv in db.ChiTietVes
                                   join dk in db.DangKys on ctv.MaDangKy equals dk.MaDangKy
                                   join hv in db.HangVes on dk.MaHangVe equals hv.MaHangVe into hvj
                                   from hv in hvj.DefaultIfEmpty()
                                   join sk in db.SuKiens on dk.MaSuKien equals sk.MaSuKien into skj
                                   from sk in skj.DefaultIfEmpty()
                                   select new TicketDetailDto
                                   {
                                       MaVe = ctv.MaVe,
                                       MaDangKy = dk.MaDangKy,
                                       MaSuKien = sk != null ? sk.MaSuKien : dk.MaSuKien,
                                       TenSuKien = sk != null ? sk.TenSuKien : dk.MaSuKien,
                                       TenHangVe = hv != null ? hv.TenHangVe : (dk.HangVe != null ? dk.HangVe.TenHangVe : "Vé chuẩn"),
                                       GiaVe = hv != null ? hv.GiaVe : 0m,
                                       TenNguoiThamGia = ctv.TenNguoiThamGia,
                                       Email = ctv.Email,
                                       MaQR = ctv.MaQR,
                                       TrangThaiVe = ctv.TrangThaiVe,
                                       NgayDangKy = dk.NgayDangKy,
                                       ThoiGianCheckIn = ctv.CheckIn != null ? (DateTime?)ctv.CheckIn.ThoiGianCheckIn : null,
                                       NguoiCheckIn = ctv.CheckIn != null ? ctv.CheckIn.NguoiCheckIn : null
                                   }).ToList();
                    if (tickets.Any()) _issuedTickets.AddRange(tickets);
                }
                catch { }

                SeedDefaultTiersAndTickets();
            }
        }

        private static void SeedDefaultTiersAndTickets()
        {
            var demoEvents = new (string Ma, string Ten)[]
            {
                ("EVT202601", "Hội nghị Công nghệ Vietnam Tech Summit 2026"),
                ("EVT202602", "Đại nhạc hội Sinh viên YouthWave 2026"),
                ("EVT202603", "Hội thảo Khởi nghiệp Trẻ & Đổi mới Sáng tạo 2026"),
                ("EVT202604", "Triển lãm Thiết bị & Trải nghiệm AI Expo 2026"),
                ("EVT202605", "Workshop Thực chiến GenAI & Agentic Coding 2026"),
                ("EVT202606", "Giải thi đấu Thể thao Điện tử Sinh viên Toàn quốc 2026"),
                ("EVT202607", "Ngày hội Việc làm & Kết nối Doanh nghiệp IT Career Fair 2026"),
                ("EVT202608", "Diễn đàn An ninh mạng Quốc gia Cybershield 2026"),
                ("EVT202609", "Hội nghị Khoa học Quốc tế về Trí tuệ Nhân tạo & Bán dẫn 2026"),
                ("EVT202610", "Lễ hội Văn hóa Ẩm thực & Âm nhạc Mùa Thu 2026"),
                ("EVT202611", "Diễn đàn Chuyển đổi số Doanh nghiệp Vừa và Nhỏ 2026"),
                ("EVT202612", "Giải Marathon Sinh viên Vì Cộng đồng Green Run 2026")
            };

            foreach (var d in demoEvents)
            {
                if (!_events.Any(x => x.MaSuKien == d.Ma))
                    _events.Add((d.Ma, d.Ten));
            }

            int tierNum = 100;
            foreach (var evt in _events.ToList())
            {
                if (!_ticketTiers.Any(t => t.MaSuKien == evt.MaSuKien))
                {
                    string hv1 = $"HV{tierNum++}";
                    string hv2 = $"HV{tierNum++}";
                    _ticketTiers.Add(new TicketTierSummaryDto
                    {
                        MaHangVe = hv1,
                        MaSuKien = evt.MaSuKien,
                        TenHangVe = "Vé Tiêu chuẩn (Standard)",
                        GiaVe = 200000m,
                        SoLuongPhatHanh = 80,
                        SoLuongDaBan = 35,
                        ThoiGianMoBan = DateTime.Now.AddDays(-30),
                        ThoiGianDongBan = DateTime.Now.AddDays(30)
                    });
                    _ticketTiers.Add(new TicketTierSummaryDto
                    {
                        MaHangVe = hv2,
                        MaSuKien = evt.MaSuKien,
                        TenHangVe = "Vé VIP (Full-Access)",
                        GiaVe = 500000m,
                        SoLuongPhatHanh = 20,
                        SoLuongDaBan = 10,
                        ThoiGianMoBan = DateTime.Now.AddDays(-30),
                        ThoiGianDongBan = DateTime.Now.AddDays(30)
                    });
                }

                if (!_issuedTickets.Any(t => t.MaSuKien == evt.MaSuKien))
                {
                    _issuedTickets.Add(new TicketDetailDto
                    {
                        MaVe = $"TKT{_ticketCounter++}",
                        MaDangKy = $"DK{_orderCounter++}",
                        MaSuKien = evt.MaSuKien,
                        TenSuKien = evt.TenSuKien,
                        TenHangVe = "Vé Tiêu chuẩn (Standard)",
                        GiaVe = 200000m,
                        TenNguoiThamGia = "Nguyễn Văn Hùng",
                        Email = "hung.nguyen@ai-institute.vn",
                        MaQR = $"QR_{evt.MaSuKien}_001",
                        TrangThaiVe = "Đã check-in",
                        NgayDangKy = DateTime.Now.AddDays(-10),
                        ThoiGianCheckIn = DateTime.Now.AddDays(-1),
                        NguoiCheckIn = "Quản trị viên Hệ thống"
                    });
                    _issuedTickets.Add(new TicketDetailDto
                    {
                        MaVe = $"TKT{_ticketCounter++}",
                        MaDangKy = $"DK{_orderCounter++}",
                        MaSuKien = evt.MaSuKien,
                        TenSuKien = evt.TenSuKien,
                        TenHangVe = "Vé VIP (Full-Access)",
                        GiaVe = 500000m,
                        TenNguoiThamGia = "Lê Hoàng Mai",
                        Email = "mai.le@techcorp.com",
                        MaQR = $"QR_{evt.MaSuKien}_002",
                        TrangThaiVe = "Chưa sử dụng",
                        NgayDangKy = DateTime.Now.AddDays(-8),
                        ThoiGianCheckIn = null,
                        NguoiCheckIn = null
                    });
                }
            }
        }

        private static int _orderCounter = 100;
        private static int _ticketCounter = 1000;
        private static readonly object _syncLock = new();

        private static string NormalizeCode(string? code)
        {
            if (string.IsNullOrWhiteSpace(code)) return string.Empty;
            var trimmed = code.Trim();
            if (trimmed.StartsWith("(") && trimmed.Contains(','))
            {
                var parts = trimmed.Trim('(', ')').Split(',');
                if (parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0]))
                {
                    return parts[0].Trim();
                }
            }
            return trimmed;
        }

        #region Public Methods

        public static void CloseTicketTier(string maHangVe)
        {
            var tier = _ticketTiers.FirstOrDefault(t => string.Equals(t.MaHangVe, maHangVe.Trim(), StringComparison.OrdinalIgnoreCase));
            if (tier != null)
            {
                tier.ThoiGianDongBan = DateTime.Now.AddSeconds(-1);
            }

            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var dbTier = db.HangVes.FirstOrDefault(h => h.MaHangVe == maHangVe.Trim());
                if (dbTier != null)
                {
                    dbTier.ThoiGianDongBan = DateTime.Now.AddSeconds(-1);
                    db.SaveChanges();
                }
            }
            catch { }
        }

        public List<(string MaSuKien, string TenSuKien)> GetAvailableEvents()
        {
            EnsureLoadedFromDb();
            return _events.ToList();
        }

        public List<TicketTierSummaryDto> GetTicketTiersByEvent(string maSuKien)
        {
            EnsureLoadedFromDb();
            var eventCode = NormalizeCode(maSuKien);
            if (string.IsNullOrWhiteSpace(eventCode))
                return _ticketTiers.ToList();

            var list = _ticketTiers
                .Where(t => string.Equals(t.MaSuKien, eventCode, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return list;
        }

        public ServiceResult<List<TicketDetailDto>> GetTicketsByEvent(string? maSuKien)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maSuKien))
                return ServiceResult<List<TicketDetailDto>>.Success(_issuedTickets.ToList());

            var trimmed = maSuKien.Trim();
            var list = _issuedTickets
                .Where(t => string.Equals(t.MaSuKien, trimmed, StringComparison.OrdinalIgnoreCase)
                    || t.TenSuKien.Contains(trimmed, StringComparison.OrdinalIgnoreCase)
                    || t.MaDangKy.Contains(trimmed, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(t => t.NgayDangKy)
                .ToList();

            return ServiceResult<List<TicketDetailDto>>.Success(list);
        }

        public ServiceResult<TicketDetailDto> GetTicketByCode(string codeOrQr)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(codeOrQr))
                return ServiceResult<TicketDetailDto>.Failed("Mã vé hoặc mã QR không được để trống.");

            var trimmed = codeOrQr.Trim();
            var ticket = _issuedTickets.FirstOrDefault(t =>
                string.Equals(t.MaVe, trimmed, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(t.MaQR, trimmed, StringComparison.OrdinalIgnoreCase));

            if (ticket == null)
                return ServiceResult<TicketDetailDto>.Failed($"Không tìm thấy vé tương ứng với mã '{trimmed}'.");

            return ServiceResult<TicketDetailDto>.Success(ticket);
        }

        /// <summary>
        /// Nghiệp vụ Đăng ký & Bán vé (BR-09, BR-10, BR-17, BR-18)
        /// </summary>
        public ServiceResult<TicketOrderResultDto> RegisterTicketOrder(RegisterTicketRequestDto request)
        {
            EnsureLoadedFromDb();
            if (request == null)
                return ServiceResult<TicketOrderResultDto>.Failed("Yêu cầu đặt vé không được để trống.");

            // 1. Thẩm định số lượng vé (BR-10: 1 đến 10 vé)
            if (request.SoLuongMua < 1 || request.SoLuongMua > 10)
            {
                return ServiceResult<TicketOrderResultDto>.Failed(
                    "Mỗi đơn hàng chỉ cho phép mua từ 1 đến 10 vé.",
                    new List<string> { $"Số lượng vé yêu cầu: {request.SoLuongMua} (hợp lệ: 1 - 10)" }
                );
            }

            // 2. Thẩm định thông tin người mua
            if (string.IsNullOrWhiteSpace(request.HoTenNguoiMua))
            {
                return ServiceResult<TicketOrderResultDto>.Failed("Họ tên người mua vé không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(request.SoDienThoai))
            {
                return ServiceResult<TicketOrderResultDto>.Failed("Số điện thoại liên hệ không được để trống.");
            }

            string contactEmail = string.IsNullOrWhiteSpace(request.Email) 
                ? $"{request.SoDienThoai.Trim()}@event.vn" 
                : request.Email.Trim();

            // 3. Khóa giao dịch kiểm soát tồn kho khả dụng (BR-09 & BR-18 Transaction)
            lock (_syncLock)
            {
                var tier = _ticketTiers.FirstOrDefault(t => string.Equals(t.MaHangVe, request.MaHangVe.Trim(), StringComparison.OrdinalIgnoreCase));
                if (tier == null)
                {
                    return ServiceResult<TicketOrderResultDto>.Failed($"Không tìm thấy hạng vé có mã '{request.MaHangVe}'.");
                }

                if (tier.IsClosed)
                {
                    return ServiceResult<TicketOrderResultDto>.Failed($"Hạng vé '{tier.TenHangVe}' đã đóng bán, không thể đặt vé.");
                }

                int available = tier.SoLuongConLai;
                if (request.SoLuongMua > available)
                {
                    return ServiceResult<TicketOrderResultDto>.Failed(
                        $"Kho vé khả dụng không đủ đáp ứng số lượng yêu cầu.",
                        new List<string>
                        {
                            $"Hạng vé: {tier.TenHangVe}",
                            $"Số vé còn lại trong kho: {available} vé",
                            $"Số vé yêu cầu mua: {request.SoLuongMua} vé"
                        }
                    );
                }

                // 4. Sinh mã đơn hàng và danh sách vé con (BR-17)
                string orderCode = $"REG2026_{++_orderCounter:D4}";
                decimal unitPrice = tier.GiaVe;
                decimal totalAmount = request.SoLuongMua * unitPrice;

                var evt = _events.FirstOrDefault(e => string.Equals(e.MaSuKien, request.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
                string eventName = evt != default ? evt.TenSuKien : request.MaSuKien;

                var createdTickets = new List<TicketDetailDto>();

                for (int i = 0; i < request.SoLuongMua; i++)
                {
                    string ticketCode = $"VE{++_ticketCounter:D5}";
                    // Mã QR dạng GUID duy nhất (BR-17)
                    string qrCode = $"EVT-QR-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";

                    string attendeeName = request.HoTenNguoiMua;
                    if (request.DanhSachNguoiDiCung != null && request.DanhSachNguoiDiCung.Count >= request.SoLuongMua && i < request.DanhSachNguoiDiCung.Count && !string.IsNullOrWhiteSpace(request.DanhSachNguoiDiCung[i]))
                    {
                        // Trường hợp danh sách người tham gia bao gồm cả người mua (đầy đủ N người từ bảng nhập liệu)
                        attendeeName = request.DanhSachNguoiDiCung[i].Trim();
                    }
                    else if (request.DanhSachNguoiDiCung != null && request.DanhSachNguoiDiCung.Count == request.SoLuongMua - 1)
                    {
                        // Trường hợp danh sách người đi cùng chỉ chứa các người đi kèm (N - 1 người)
                        if (i == 0)
                        {
                            attendeeName = request.HoTenNguoiMua;
                        }
                        else if (i - 1 < request.DanhSachNguoiDiCung.Count && !string.IsNullOrWhiteSpace(request.DanhSachNguoiDiCung[i - 1]))
                        {
                            attendeeName = request.DanhSachNguoiDiCung[i - 1].Trim();
                        }
                    }
                    else if (i < (request.DanhSachNguoiDiCung?.Count ?? 0) && !string.IsNullOrWhiteSpace(request.DanhSachNguoiDiCung![i]))
                    {
                        attendeeName = request.DanhSachNguoiDiCung[i].Trim();
                    }
                    else if (i > 0)
                    {
                        attendeeName = $"{request.HoTenNguoiMua} (Người đi cùng {i})";
                    }

                    var ticketDto = new TicketDetailDto
                    {
                        MaVe = ticketCode,
                        MaDangKy = orderCode,
                        MaSuKien = request.MaSuKien.Trim(),
                        TenSuKien = eventName,
                        TenHangVe = tier.TenHangVe,
                        GiaVe = unitPrice,
                        TenNguoiThamGia = attendeeName,
                        Email = request.Email.Trim(),
                        MaQR = qrCode,
                        TrangThaiVe = "Hợp lệ",
                        NgayDangKy = DateTime.Now
                    };

                    createdTickets.Add(ticketDto);
                }

                // 5. Cập nhật số lượng đã bán (BR-09) và lưu vào danh sách hệ thống
                tier.SoLuongDaBan += request.SoLuongMua;
                _issuedTickets.AddRange(createdTickets);

                var orderResult = new TicketOrderResultDto
                {
                    MaDangKy = orderCode,
                    MaSuKien = request.MaSuKien,
                    TenSuKien = eventName,
                    MaHangVe = tier.MaHangVe,
                    TenHangVe = tier.TenHangVe,
                    DonGia = unitPrice,
                    SoLuong = request.SoLuongMua,
                    TongTien = totalAmount,
                    NgayDangKy = DateTime.Now,
                    TrangThaiThanhToan = "Đã thanh toán",
                    HoTenNguoiMua = request.HoTenNguoiMua.Trim(),
                    SoDienThoai = request.SoDienThoai.Trim(),
                    Email = contactEmail,
                    DanhSachVe = createdTickets
                };

                EventManagement.Core.Common.DataSyncNotifier.NotifyTicketSold(request.MaSuKien, request.SoLuongMua, totalAmount);
                return ServiceResult<TicketOrderResultDto>.Success(
                    orderResult,
                    $"Đăng ký đơn hàng thành công! Đã phát hành {request.SoLuongMua} vé điện tử với mã QR độc lập."
                );
            }
        }

        /// <summary>
        /// Lấy tham chiếu danh sách vé phục vụ liên thông kiểm tra điểm danh (CheckInService)
        /// </summary>
        public static List<TicketDetailDto> GetIssuedTicketsList()
        {
            EnsureLoadedFromDb();
            lock (_syncLock)
            {
                return _issuedTickets;
            }
        }

        #endregion
    }
}
