using System;
using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng DienGia - Thông tin diễn giả & chuyên gia khách mời (F09)
    /// </summary>
    public class DienGia
    {
        public string MaDienGia { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string? ChucVu { get; set; }
        public string? DonViCongTac { get; set; }
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public string? TieuSu { get; set; }
        public string? HinhAnh { get; set; } // Tên file ảnh chân dung (ví dụ: dg01_nguyen_van_an.jpg)
    }
}
