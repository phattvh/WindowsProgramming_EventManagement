using System;
using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng NhaTaiTro - Hồ sơ đối tác tài trợ
    /// </summary>
    public class NhaTaiTro
    {
        public string MaNhaTaiTro { get; set; } = string.Empty;
        public string TenToChuc { get; set; } = string.Empty;
        public string? NguoiDaiDien { get; set; }
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? GhiChu { get; set; }

        // Navigation properties
        public virtual ICollection<ChiTietTaiTro> ChiTietTaiTros { get; set; } = new List<ChiTietTaiTro>();
    }
}
