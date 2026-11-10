using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO thông tin lịch sử tham gia sự kiện của người tham dự (Form F10)
    /// </summary>
    public class AttendeeEventHistoryDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public string TenHangVe { get; set; } = string.Empty;
        public DateTime NgayDangKy { get; set; }
        public int SoLuong { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThaiThanhToan { get; set; } = "Đã thanh toán";
        public string TrangThaiCheckIn { get; set; } = "Chưa check-in";
        public DateTime? ThoiGianCheckIn { get; set; }
    }
}
