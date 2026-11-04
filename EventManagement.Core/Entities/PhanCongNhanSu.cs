using System;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng PhanCongNhanSu - Phân công nhân sự theo ca trực và nhiệm vụ cụ thể
    /// Phụ trách: Thành viên 2 (Màn hình F15 và Tab 6 Form F05)
    /// </summary>
    public class PhanCongNhanSu
    {
        public string MaPhanCong { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string MaNhanSu { get; set; } = string.Empty;
        public string NhiemVu { get; set; } = string.Empty;
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string? GhiChu { get; set; }

        // Navigation properties
        public virtual SuKien? SuKien { get; set; }
        public virtual NhanSu? NhanSu { get; set; }
    }
}
