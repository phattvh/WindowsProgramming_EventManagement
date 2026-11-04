using System;
using System.Collections.Generic;

namespace EventManagement.Core.Session
{
    /// <summary>
    /// Lưu giữ thông tin phiên làm việc của người dùng hiện tại (Task T1-11 / AD-4)
    /// </summary>
    public static class CurrentSession
    {
        public static string? MaNguoiDung { get; private set; }
        public static string? TenDangNhap { get; private set; }
        public static string? HoTen { get; private set; }
        public static string? Email { get; private set; }
        public static string? MaNhomQuyen { get; private set; }
        public static DateTime? ThoiDiemDangNhap { get; private set; }
        public static HashSet<string> DanhSachQuyen { get; private set; } = new(StringComparer.OrdinalIgnoreCase);

        public static bool IsLoggedIn => !string.IsNullOrEmpty(MaNguoiDung);

        public static void DangNhap(string maNguoiDung, string tenDangNhap, string hoTen, string email, string maNhomQuyen, IEnumerable<string>? danhSachQuyen = null)
        {
            MaNguoiDung = maNguoiDung;
            TenDangNhap = tenDangNhap;
            HoTen = hoTen;
            Email = email;
            MaNhomQuyen = maNhomQuyen;
            ThoiDiemDangNhap = DateTime.Now;

            DanhSachQuyen.Clear();
            if (danhSachQuyen != null)
            {
                foreach (var q in danhSachQuyen)
                {
                    DanhSachQuyen.Add(q);
                }
            }
        }

        public static void DangXuat()
        {
            MaNguoiDung = null;
            TenDangNhap = null;
            HoTen = null;
            Email = null;
            MaNhomQuyen = null;
            ThoiDiemDangNhap = null;
            DanhSachQuyen.Clear();
        }

        public static bool CoQuyen(string maChucNang)
        {
            if (string.Equals(MaNhomQuyen, "ADMIN", StringComparison.OrdinalIgnoreCase))
                return true;

            return DanhSachQuyen.Contains(maChucNang);
        }
    }
}
