using System;
using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng NguoiThamGia - Hồ sơ người tham dự sự kiện
    /// </summary>
    public class NguoiThamGia
    {
        public string MaNguoiThamGia { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string GioiTinh { get; set; } = "Nam"; // Nam, Nữ, Khác
        public DateTime? NgaySinh { get; set; }
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DonVi { get; set; }
        public string? DonViCongTac { get => DonVi; set => DonVi = value; }
        public string? DiaChi { get; set; }
        public string TrangThai { get; set; } = "Hoạt động"; // Hoạt động, Khóa

        // Navigation properties
        public virtual ICollection<DangKy> DangKys { get; set; } = new List<DangKy>();
        public virtual ICollection<PhanHoi> PhanHois { get; set; } = new List<PhanHoi>();
    }
}
