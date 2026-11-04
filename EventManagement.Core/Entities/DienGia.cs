using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng DienGia - Danh sách diễn giả và khách mời chuyên môn
    /// Phụ trách: Thành viên 2 (Màn hình F09 và Tab 3 Form F05)
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

        // Navigation properties: 1 Diễn giả có thể trình bày ở nhiều Tiết mục/Lịch trình
        public virtual ICollection<LichTrinh> LichTrinhs { get; set; } = new List<LichTrinh>();
    }
}
