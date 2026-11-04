using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng LoaiSuKien - Chuẩn hóa các danh mục loại hình sự kiện (Hội thảo, Workshop, Hội nghị...)
    /// Phụ trách: Thành viên 2 (Màn hình F06)
    /// </summary>
    public class LoaiSuKien
    {
        public string MaLoaiSK { get; set; } = string.Empty;
        public string TenLoaiSK { get; set; } = string.Empty;
        public string? MoTa { get; set; }

        // Navigation properties: 1 Loại sự kiện có thể áp dụng cho nhiều Sự kiện
        public virtual ICollection<SuKien> SuKiens { get; set; } = new List<SuKien>();
    }
}
