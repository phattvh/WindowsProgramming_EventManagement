using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách vé trên DataGridView (F12) hoặc danh sách vé đã mua (F11)
    /// </summary>
    public class TicketDetailDto
    {
        public string MaVe { get; set; } = string.Empty;
        public string MaDangKy { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public string TenHangVe { get; set; } = string.Empty;
        public decimal GiaVe { get; set; }
        public string TenNguoiThamGia { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string MaQR { get; set; } = string.Empty;
        public string TrangThaiVe { get; set; } = string.Empty;
        public DateTime NgayDangKy { get; set; }
        public DateTime? ThoiGianCheckIn { get; set; }
    }
}
