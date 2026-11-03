using System;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng ChiTietTaiTro - Hợp đồng tài trợ theo sự kiện
    /// </summary>
    public class ChiTietTaiTro
    {
        public string MaTaiTro { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string MaNhaTaiTro { get; set; } = string.Empty;
        public string HangTaiTro { get; set; } = "Đồng"; // Kim cương, Vàng, Bạc, Đồng
        public decimal GiaTri { get; set; }
        public string HinhThucTaiTro { get; set; } = "Tiền mặt"; // Tiền mặt, Hiện vật, Dịch vụ
        public string? QuyenLoiTruyenThong { get; set; }
        public string? GhiChu { get; set; }

        // Navigation properties
        public virtual SuKien? SuKien { get; set; }
        public virtual NhaTaiTro? NhaTaiTro { get; set; }
    }
}
