using System;
using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng NguoiDung - Tài khoản người dùng hệ thống
    /// </summary>
    public class NguoiDung
    {
        public string MaNguoiDung { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhauHash { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; }
        public string MaNhomQuyen { get; set; } = string.Empty;
        public string TrangThai { get; set; } = "Hoạt động"; // Hoạt động, Khóa tạm thời, Khóa vĩnh viễn
        public DateTime? LanDoiMatKhauGanNhat { get; set; }
        public int SoLanDangNhapSai { get; set; } = 0;
        public DateTime? KhoaDenKhi { get; set; }

        // Navigation properties liên kết tới phân hệ của Thành viên 3
        public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
        public virtual ICollection<ChiPhiSuKien> ChiPhiSuKiens { get; set; } = new List<ChiPhiSuKien>();
    }
}
