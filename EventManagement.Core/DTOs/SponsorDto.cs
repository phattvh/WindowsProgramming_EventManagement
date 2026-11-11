using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object cho Nhà tài trợ và gói tài trợ (Form F16)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// </summary>
    public class SponsorDto
    {
        public string MaNhaTaiTro { get; set; } = string.Empty;
        public string TenToChuc { get; set; } = string.Empty;
        public string? NguoiDaiDien { get; set; }
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? GhiChu { get; set; }

        // Thông tin gói và giá trị tài trợ
        public string GoiTaiTro { get; set; } = "Đồng"; // Kim Cương, Vàng, Bạc, Đồng, Đồng hành
        public decimal TongGiaTriTaiTro { get; set; } = 0;
        public string HinhThucTaiTro { get; set; } = "Tiền mặt"; // Tiền mặt, Hiện vật, Dịch vụ...
        public string TrangThai { get; set; } = "Đang hợp tác"; // Đang hợp tác, Tiềm năng, Ngừng hợp tác
        public int SoSuKienTaiTro { get; set; } = 0;
        public string? MaSuKien { get; set; } = "EVT202601";
        public string? TenSuKien { get; set; }

        /// <summary>
        /// Hiển thị giá trị tiền tệ định dạng VNĐ
        /// </summary>
        public string GiaTriFormatted => TongGiaTriTaiTro.ToString("#,##0") + " VNĐ";
    }
}
