using System;
using System.Collections.Generic;

namespace EventManagement.Core.DTOs
{
    public class LoginResultDto
    {
        public string MaNguoiDung { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MaNhomQuyen { get; set; } = string.Empty;
        public List<string> DanhSachQuyen { get; set; } = new();
    }
}
