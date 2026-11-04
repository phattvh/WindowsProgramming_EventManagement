using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng DiaDiem - Cơ sở vật chất và địa điểm tổ chức sự kiện
    /// Phụ trách: Thành viên 2 (Màn hình F07)
    /// </summary>
    public class DiaDiem
    {
        public string MaDiaDiem { get; set; } = string.Empty;
        public string TenDiaDiem { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public int SucChuaToiDa { get; set; } = 100;
        public string? GhiChu { get; set; }

        // Navigation properties: 1 Địa điểm có thể tổ chức nhiều Sự kiện
        public virtual ICollection<SuKien> SuKiens { get; set; } = new List<SuKien>();
    }
}
