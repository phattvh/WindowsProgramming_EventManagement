using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách người dùng trên Form F03
    /// </summary>
    public class UserDto
    {
        public string MaNguoiDung { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; }
        public string MaNhomQuyen { get; set; } = string.Empty;
        public string TenNhomQuyen { get; set; } = string.Empty;
        public string TrangThai { get; set; } = "Hoạt động";
        public int SoLanDangNhapSai { get; set; }
        public DateTime? KhoaDenKhi { get; set; }
        public DateTime? LanDoiMatKhauGanNhat { get; set; }
        public bool IsLocked => KhoaDenKhi.HasValue && KhoaDenKhi.Value > DateTime.Now || TrangThai == "Khóa vĩnh viễn" || TrangThai == "Khóa tạm thời";
    }

    /// <summary>
    /// DTO thêm mới tài khoản
    /// </summary>
    public class CreateUserDto
    {
        public string? MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; }
        public string MaNhomQuyen { get; set; } = "STAFF";
        public string TrangThai { get; set; } = "Hoạt động";
    }

    /// <summary>
    /// DTO cập nhật tài khoản
    /// </summary>
    public class UpdateUserDto
    {
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; }
        public string MaNhomQuyen { get; set; } = string.Empty;
        public string TrangThai { get; set; } = "Hoạt động";
        public string? MatKhauMoi { get; set; } // Nếu để trống thì giữ nguyên mật khẩu cũ
    }
}