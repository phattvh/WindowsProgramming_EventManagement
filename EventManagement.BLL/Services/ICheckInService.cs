using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện nghiệp vụ Soát vé & Điểm danh sự kiện qua Webcam QR / Mã vé (F13)
    /// Đảm trách quy tắc:
    /// - BR-11: Thẩm định vé (Tồn tại, đúng sự kiện, đã thanh toán, không bị hủy)
    /// - BR-12: Điểm danh một chiều & chặn quét trùng lặp (Duplicate scan prevention)
    /// Phụ trách: Thành viên 3 (Task T3-19 & T3-20 / Day 7)
    /// </summary>
    public interface ICheckInService
    {
        /// <summary>
        /// Lấy danh sách sự kiện đang diễn ra để phục vụ điểm danh
        /// </summary>
        List<(string MaSuKien, string TenSuKien)> GetActiveEvents();

        /// <summary>
        /// Thực hiện thẩm định và check-in vé (BR-11, BR-12)
        /// </summary>
        ServiceResult<CheckInResultDto> ProcessCheckIn(CheckInRequestDto request);

        /// <summary>
        /// Lấy thống kê số lượng điểm danh theo thời gian thực của sự kiện
        /// </summary>
        CheckInStatsDto GetCheckInStatistics(string maSuKien);

        /// <summary>
        /// Lấy danh sách các lượt check-in gần nhất của sự kiện
        /// </summary>
        List<CheckInHistoryDto> GetRecentCheckIns(string maSuKien, int count = 20);

        /// <summary>
        /// Đặt lại trạng thái check-in (cho mục đích demo/kiểm thử)
        /// </summary>
        void ResetCheckInForDemo(string maVe);
    }
}
