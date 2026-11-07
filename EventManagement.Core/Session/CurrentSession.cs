using System;
using System.Collections.Generic;
using System.Linq;

namespace EventManagement.Core.Session
{
    /// <summary>
    /// Lưu giữ thông tin phiên làm việc và quản lý thẩm định quyền hạn (Task T1-11, T1-17 / AD-4).
    /// Hỗ trợ tra cứu ánh xạ hai chiều giữa mã nghiệp vụ (UC01-UC21) và mã màn hình (F01-F20).
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

        // Ánh xạ chuẩn giữa Use Case (UC) và Màn hình (Form) theo tài liệu thiết kế hệ thống
        private static readonly Dictionary<string, string> UcToFormMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "UC01", "F01" }, { "UC02", "F02" }, { "UC03", "F03" }, { "UC04", "F04" },
            { "UC05", "F05" }, { "UC06", "F06" }, { "UC07", "F07" }, { "UC08", "F08" },
            { "UC09", "F09" }, { "UC10", "F10" }, { "UC11", "F11" }, { "UC12", "F12" },
            { "UC13", "F13" }, { "UC14", "F14" }, { "UC15", "F15" }, { "UC16", "F16" },
            { "UC17", "F17" }, { "UC18", "F18" }, { "UC19", "F19" }, { "UC20", "F20" },
            { "UC21", "UC21" }
        };

        private static readonly Dictionary<string, string> FormToUcMap = new(StringComparer.OrdinalIgnoreCase);

        static CurrentSession()
        {
            foreach (var kvp in UcToFormMap)
            {
                if (!FormToUcMap.ContainsKey(kvp.Value))
                {
                    FormToUcMap[kvp.Value] = kvp.Key;
                }
            }
        }

        public static bool IsLoggedIn => !string.IsNullOrEmpty(MaNguoiDung);

        public static bool IsAdmin => string.Equals(MaNhomQuyen, "ADMIN", StringComparison.OrdinalIgnoreCase)
                                   || string.Equals(MaNhomQuyen, "NQ_ADMIN", StringComparison.OrdinalIgnoreCase);

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
                    if (string.IsNullOrWhiteSpace(q)) continue;
                    DanhSachQuyen.Add(q);

                    // Tự động nạp ánh xạ chéo (UC <-> Form)
                    if (UcToFormMap.TryGetValue(q, out var formCode))
                    {
                        DanhSachQuyen.Add(formCode);
                    }
                    else if (FormToUcMap.TryGetValue(q, out var ucCode))
                    {
                        DanhSachQuyen.Add(ucCode);
                    }
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

        /// <summary>
        /// Thêm quyền bổ sung vào phiên làm việc hiện tại (dùng cho testing hoặc phân quyền mở rộng).
        /// </summary>
        public static void ThemQuyen(params string[] quyenHans)
        {
            if (quyenHans == null) return;
            foreach (var q in quyenHans)
            {
                if (string.IsNullOrWhiteSpace(q)) continue;
                DanhSachQuyen.Add(q);
                if (UcToFormMap.TryGetValue(q, out var formCode)) DanhSachQuyen.Add(formCode);
                else if (FormToUcMap.TryGetValue(q, out var ucCode)) DanhSachQuyen.Add(ucCode);
            }
        }

        /// <summary>
        /// Kiểm tra xem người dùng hiện tại có thẩm quyền đối với chức năng hoặc màn hình chỉ định hay không.
        /// Quản trị viên (ADMIN) mặc định có toàn bộ quyền trong hệ thống.
        /// </summary>
        public static bool CoQuyen(string? maChucNang)
        {
            if (string.IsNullOrWhiteSpace(maChucNang)) return false;

            if (IsAdmin) return true;

            if (DanhSachQuyen.Contains(maChucNang)) return true;

            // Kiểm tra mã ánh xạ chéo (nếu truyền F03 mà danh sách lưu UC03, hoặc ngược lại)
            if (UcToFormMap.TryGetValue(maChucNang, out var formCode) && DanhSachQuyen.Contains(formCode))
                return true;

            if (FormToUcMap.TryGetValue(maChucNang, out var ucCode) && DanhSachQuyen.Contains(ucCode))
                return true;

            return false;
        }

        /// <summary>
        /// Kiểm tra người dùng có ít nhất MỘT trong các quyền được chỉ định (Toán tử HOẶC / OR).
        /// </summary>
        public static bool CoQuyenBatKy(params string[] maChucNangs)
        {
            if (maChucNangs == null || maChucNangs.Length == 0) return false;
            if (IsAdmin) return true;

            return maChucNangs.Any(CoQuyen);
        }

        /// <summary>
        /// Kiểm tra người dùng có ĐỦ TẤT CẢ các quyền được chỉ định (Toán tử VÀ / AND).
        /// </summary>
        public static bool CoDuQuyen(params string[] maChucNangs)
        {
            if (maChucNangs == null || maChucNangs.Length == 0) return false;
            if (IsAdmin) return true;

            return maChucNangs.All(CoQuyen);
        }

        /// <summary>
        /// Lấy mã Form tương ứng từ mã Use Case (ví dụ: UC03 -> F03).
        /// </summary>
        public static string? LayMaFormTuUc(string maUc)
        {
            return UcToFormMap.TryGetValue(maUc, out var form) ? form : null;
        }

        /// <summary>
        /// Lấy mã Use Case tương ứng từ mã Form (ví dụ: F03 -> UC03).
        /// </summary>
        public static string? LayMaUcTuForm(string maForm)
        {
            return FormToUcMap.TryGetValue(maForm, out var uc) ? uc : null;
        }
    }
}
