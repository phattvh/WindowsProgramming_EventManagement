using System;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng LichTrinh - Lịch trình chi tiết các tiết mục/phiên thảo luận của sự kiện
    /// Phụ trách: Thành viên 2 (Màn hình F08 và Tab 2 Form F05)
    /// </summary>
    public class LichTrinh
    {
        public string MaLichTrinh { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string TenTietMuc { get; set; } = string.Empty;
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string? MaDienGia { get; set; }
        public string? DiaDiemChiTiet { get; set; }
        public string? GhiChu { get; set; }

        // Navigation properties
        public virtual SuKien? SuKien { get; set; }
        public virtual DienGia? DienGia { get; set; }
    }
}
