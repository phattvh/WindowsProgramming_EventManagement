using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Service xử lý nghiệp vụ Soát vé & Điểm danh sự kiện (F13 / UC13)
    /// Đảm trách quy tắc:
    /// - BR-11: Thẩm định tính hợp lệ của vé (Tồn tại, đúng sự kiện, đã thanh toán, không bị hủy)
    /// - BR-12: Điểm danh một chiều & chặn quét trùng lặp (Duplicate scan prevention)
    /// Phụ trách: Thành viên 3 (Task T3-19 & T3-20 / Day 7)
    /// </summary>
    public class CheckInService : ICheckInService
    {
        private static readonly List<(string MaSuKien, string TenSuKien)> _events = new()
        {
            ("EVT202601", "Hội nghị Công nghệ Vietnam Tech Summit 2026"),
            ("EVT202602", "Workshop Thực chiến GenAI & Prompt Engineering 2026"),
            ("EVT202603", "Hội nghị Khoa học Quốc tế về Công nghệ Phần mềm & IoT")
        };

        private static readonly List<CheckInHistoryDto> _recentLogs = new()
        {
            new CheckInHistoryDto
            {
                MaVe = "VE00000",
                TenNguoiThamGia = "Trần Đình Khôi (Khách mời)",
                TenHangVe = "Vé VIP",
                ThoiGian = DateTime.Now.AddMinutes(-25),
                PhuongThuc = "Webcam QR",
                ThanhCong = true,
                GhiChu = "Check-in thành công lúc mở cửa"
            }
        };

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

        public List<(string MaSuKien, string TenSuKien)> GetActiveEvents()
        {
            return _events.ToList();
        }

        public ServiceResult<CheckInResultDto> ProcessCheckIn(CheckInRequestDto request)
        {
            if (request == null)
                return ServiceResult<CheckInResultDto>.Failed("Dữ liệu yêu cầu check-in không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.MaQRLuocDoc))
                return ServiceResult<CheckInResultDto>.Failed("Mã QR hoặc mã vé không được để trống.");

            string token = request.MaQRLuocDoc.Trim();
            string selectedEventId = NormalizeCode(request.MaSuKien);

            lock (_syncLock)
            {
                var tickets = TicketOrderService.GetIssuedTicketsList();

                // 1. Tìm vé theo MaVe hoặc MaQR token (BR-11)
                var ticket = tickets.FirstOrDefault(t =>
                    string.Equals(t.MaVe, token, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(t.MaQR, token, StringComparison.OrdinalIgnoreCase));

                if (ticket == null)
                {
                    LogHistory("UNKNOWN", "Không rõ", "N/A", request.PhuongThuc, false, $"Mã không tồn tại: {token}");
                    return ServiceResult<CheckInResultDto>.Failed("Mã vé không tồn tại trong hệ thống!");
                }

                // 2. Kiểm tra sự kiện tương ứng (BR-11: Đúng sự kiện)
                var currentEvent = _events.FirstOrDefault(e => e.MaSuKien == selectedEventId);
                string expectedEventName = currentEvent.TenSuKien ?? string.Empty;

                if (!string.IsNullOrEmpty(expectedEventName) &&
                    !string.Equals(ticket.TenSuKien, expectedEventName, StringComparison.OrdinalIgnoreCase))
                {
                    LogHistory(ticket.MaVe, ticket.TenNguoiThamGia, ticket.TenHangVe, request.PhuongThuc, false,
                        $"Sai sự kiện! Vé thuộc: {ticket.TenSuKien}");

                    return ServiceResult<CheckInResultDto>.Failed(
                        $"Vé '{ticket.MaVe}' thuộc sự kiện '{ticket.TenSuKien}', không thể check-in tại '{expectedEventName}' (BR-11).");
                }

                // 3. Kiểm tra trạng thái hủy/chưa thanh toán
                if (string.Equals(ticket.TrangThaiVe, "Đã hủy", StringComparison.OrdinalIgnoreCase))
                {
                    LogHistory(ticket.MaVe, ticket.TenNguoiThamGia, ticket.TenHangVe, request.PhuongThuc, false, "Vé đã bị hủy");
                    return ServiceResult<CheckInResultDto>.Failed($"Vé '{ticket.MaVe}' đã bị hủy trên hệ thống.");
                }

                // 4. Kiểm tra quét trùng lặp / Check-in 1 chiều (BR-12)
                if (ticket.ThoiGianCheckIn != null || string.Equals(ticket.TrangThaiVe, "Đã check-in", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime previousScanTime = ticket.ThoiGianCheckIn ?? DateTime.Now;
                    string prevChecker = !string.IsNullOrWhiteSpace(ticket.NguoiCheckIn) ? ticket.NguoiCheckIn : "Nhân viên";
                    string warnMsg = $"CẢNH BÁO: Vé này đã check-in vào lúc {previousScanTime:HH:mm:ss dd/MM/yyyy} bởi {prevChecker}!";

                    LogHistory(ticket.MaVe, ticket.TenNguoiThamGia, ticket.TenHangVe, request.PhuongThuc, false,
                        $"Quét trùng lặp lúc {previousScanTime:HH:mm:ss}");

                    var duplicateResult = new CheckInResultDto
                    {
                        IsSuccess = false,
                        IsWarningDuplicate = true,
                        ThoiGianCheckInTruocDo = previousScanTime,
                        NguoiCheckInTruocDo = prevChecker,
                        MaVe = ticket.MaVe,
                        TenNguoiThamGia = ticket.TenNguoiThamGia,
                        TenSuKien = ticket.TenSuKien,
                        TenHangVe = ticket.TenHangVe,
                        Message = warnMsg
                    };

                    return ServiceResult<CheckInResultDto>.Success(duplicateResult, warnMsg);
                }

                // 5. Check-in thành công (Hợp lệ lần đầu)
                ticket.ThoiGianCheckIn = DateTime.Now;
                ticket.NguoiCheckIn = string.IsNullOrWhiteSpace(request.NguoiCheckIn) ? "STAFF" : request.NguoiCheckIn.Trim();
                ticket.TrangThaiVe = "Đã check-in";

                string successMsg = $"Check-in thành công: {ticket.TenNguoiThamGia} - Hạng: {ticket.TenHangVe}";
                LogHistory(ticket.MaVe, ticket.TenNguoiThamGia, ticket.TenHangVe, request.PhuongThuc, true, "Check-in thành công");

                var successResult = new CheckInResultDto
                {
                    IsSuccess = true,
                    IsWarningDuplicate = false,
                    ThoiGianCheckIn = ticket.ThoiGianCheckIn,
                    MaVe = ticket.MaVe,
                    TenNguoiThamGia = ticket.TenNguoiThamGia,
                    TenSuKien = ticket.TenSuKien,
                    TenHangVe = ticket.TenHangVe,
                    Message = successMsg
                };

                EventManagement.Core.Common.DataSyncNotifier.NotifyTicketCheckedIn(ticket.MaSuKien, ticket.MaVe);
                return ServiceResult<CheckInResultDto>.Success(successResult, successMsg);
            }
        }

        public CheckInStatsDto GetCheckInStatistics(string maSuKien)
        {
            lock (_syncLock)
            {
                var eventCode = NormalizeCode(maSuKien);
                var tickets = TicketOrderService.GetIssuedTicketsList();
                var currentEvent = _events.FirstOrDefault(e => string.Equals(e.MaSuKien, eventCode, StringComparison.OrdinalIgnoreCase));
                string eventName = currentEvent.TenSuKien ?? string.Empty;

                var eventTickets = string.IsNullOrWhiteSpace(eventCode) && string.IsNullOrWhiteSpace(eventName)
                    ? tickets
                    : tickets.Where(t => 
                        (!string.IsNullOrWhiteSpace(eventCode) && string.Equals(t.MaSuKien, eventCode, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(eventName) && string.Equals(t.TenSuKien, eventName, StringComparison.OrdinalIgnoreCase))
                    ).ToList();

                int total = eventTickets.Count;
                int checkedIn = eventTickets.Count(t => t.ThoiGianCheckIn != null || string.Equals(t.TrangThaiVe, "Đã check-in", StringComparison.OrdinalIgnoreCase));

                return new CheckInStatsDto
                {
                    MaSuKien = eventCode,
                    TongVePhatHanh = total,
                    DaCheckIn = checkedIn
                };
            }
        }

        public List<CheckInHistoryDto> GetRecentCheckIns(string maSuKien, int count = 20)
        {
            lock (_syncLock)
            {
                return _recentLogs
                    .OrderByDescending(x => x.ThoiGian)
                    .Take(count)
                    .ToList();
            }
        }

        public void ResetCheckInForDemo(string maVe)
        {
            lock (_syncLock)
            {
                var tickets = TicketOrderService.GetIssuedTicketsList();
                var ticket = tickets.FirstOrDefault(t => string.Equals(t.MaVe, maVe, StringComparison.OrdinalIgnoreCase));
                if (ticket != null)
                {
                    ticket.ThoiGianCheckIn = null;
                    ticket.TrangThaiVe = "Hợp lệ";
                }
            }
        }

        private static void LogHistory(string maVe, string tenNguoi, string hangVe, string phuongThuc, bool thanhCong, string ghiChu)
        {
            _recentLogs.Add(new CheckInHistoryDto
            {
                MaVe = maVe,
                TenNguoiThamGia = tenNguoi,
                TenHangVe = hangVe,
                ThoiGian = DateTime.Now,
                PhuongThuc = phuongThuc,
                ThanhCong = thanhCong,
                GhiChu = ghiChu
            });
        }
    }
}
