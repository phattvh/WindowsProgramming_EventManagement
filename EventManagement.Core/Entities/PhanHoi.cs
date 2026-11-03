using System;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng PhanHoi - Khảo sát ý kiến người tham gia sau sự kiện (BR-16)
    /// </summary>
    public class PhanHoi
    {
        public string MaPhanHoi { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string MaNguoiThamGia { get; set; } = string.Empty;
        public int DiemDanhGia { get; set; } // 1 - 5 sao (CK_PhanHoi_DiemDanhGia)
        public string? NoiDungPhanHoi { get; set; }
        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual SuKien? SuKien { get; set; }
        public virtual NguoiThamGia? NguoiThamGia { get; set; }
    }
}
