using System;
using System.Collections.Generic;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO kết quả sau khi đăng ký & bán vé thành công (F11 - BR-10, BR-18)
    /// </summary>
    public class TicketOrderResultDto
    {
        public string MaDangKy { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public string MaHangVe { get; set; } = string.Empty;
        public string TenHangVe { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public decimal TongTien { get; set; }
        public DateTime NgayDangKy { get; set; } = DateTime.Now;
        public string TrangThaiThanhToan { get; set; } = "Đã thanh toán";
        public string HoTenNguoiMua { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<TicketDetailDto> DanhSachVe { get; set; } = new List<TicketDetailDto>();
    }

    /// <summary>
    /// DTO tóm tắt hạng vé mở bán trên Form F11
    /// </summary>
    public class TicketTierSummaryDto
    {
        public string MaHangVe { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string TenHangVe { get; set; } = string.Empty;
        public decimal GiaVe { get; set; }
        public int SoLuongPhatHanh { get; set; }
        public int SoLuongDaBan { get; set; }
        public int SoLuongConLai => Math.Max(0, SoLuongPhatHanh - SoLuongDaBan);
        public bool ConVe => SoLuongConLai > 0;
        public DateTime? ThoiGianMoBan { get; set; }
        public DateTime? ThoiGianDongBan { get; set; }
    }
}
