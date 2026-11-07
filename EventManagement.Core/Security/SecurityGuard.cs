using System;
using EventManagement.Core.Session;

namespace EventManagement.Core.Security
{
    /// <summary>
    /// Ranh giới an ninh Tầng Nghiệp vụ (BLL Security Guard - AD-4 Lớp 2).
    /// Đảm bảo mọi lời gọi phương thức nhạy cảm đều phải qua kiểm soát phân quyền.
    /// Ném UnauthorizedAccessException khi tài khoản không thỏa mãn yêu cầu quyền hạn.
    /// </summary>
    public static class SecurityGuard
    {
        /// <summary>
        /// Yêu cầu tài khoản đăng nhập phải có quyền thực hiện chức năng chỉ định.
        /// </summary>
        /// <param name="maChucNang">Mã chức năng (UC01-UC21) hoặc mã Form (F01-F20)</param>
        /// <param name="customMessage">Thông điệp lỗi tùy chỉnh</param>
        public static void YeuCauQuyen(string maChucNang, string? customMessage = null)
        {
            if (string.IsNullOrWhiteSpace(maChucNang))
                throw new ArgumentNullException(nameof(maChucNang), "Mã chức năng yêu cầu không được rỗng!");

            if (!CurrentSession.IsLoggedIn)
            {
                throw new UnauthorizedAccessException("Phiên làm việc chưa được khởi tạo. Vui lòng đăng nhập hệ thống trước khi thao tác!");
            }

            if (!CurrentSession.CoQuyen(maChucNang))
            {
                var user = CurrentSession.TenDangNhap ?? "Khách";
                var role = CurrentSession.MaNhomQuyen ?? "Chưa phân quyền";
                throw new UnauthorizedAccessException(customMessage ??
                    $"Truy cập bị từ chối: Tài khoản '{user}' (Nhóm: {role}) không có quyền '{maChucNang}' để thực hiện nghiệp vụ này!");
            }
        }

        /// <summary>
        /// Yêu cầu tài khoản đăng nhập phải có ít nhất một trong các quyền chỉ định (Toán tử OR).
        /// </summary>
        public static void YeuCauQuyenBatKy(params string[] maChucNangs)
        {
            if (maChucNangs == null || maChucNangs.Length == 0)
                throw new ArgumentException("Danh sách mã chức năng yêu cầu không được rỗng!", nameof(maChucNangs));

            if (!CurrentSession.IsLoggedIn)
            {
                throw new UnauthorizedAccessException("Phiên làm việc chưa được khởi tạo. Vui lòng đăng nhập hệ thống!");
            }

            if (!CurrentSession.CoQuyenBatKy(maChucNangs))
            {
                var user = CurrentSession.TenDangNhap ?? "Khách";
                var role = CurrentSession.MaNhomQuyen ?? "Chưa phân quyền";
                var codes = string.Join(", ", maChucNangs);
                throw new UnauthorizedAccessException(
                    $"Truy cập bị từ chối: Tài khoản '{user}' (Nhóm: {role}) không có bất kỳ quyền nào trong nhóm [{codes}] để thực hiện nghiệp vụ này!");
            }
        }

        /// <summary>
        /// Yêu cầu tài khoản đăng nhập phải có đầy đủ tất cả các quyền chỉ định (Toán tử AND).
        /// </summary>
        public static void YeuCauDuQuyen(params string[] maChucNangs)
        {
            if (maChucNangs == null || maChucNangs.Length == 0)
                throw new ArgumentException("Danh sách mã chức năng yêu cầu không được rỗng!", nameof(maChucNangs));

            if (!CurrentSession.IsLoggedIn)
            {
                throw new UnauthorizedAccessException("Phiên làm việc chưa được khởi tạo. Vui lòng đăng nhập hệ thống!");
            }

            if (!CurrentSession.CoDuQuyen(maChucNangs))
            {
                var user = CurrentSession.TenDangNhap ?? "Khách";
                var role = CurrentSession.MaNhomQuyen ?? "Chưa phân quyền";
                var codes = string.Join(", ", maChucNangs);
                throw new UnauthorizedAccessException(
                    $"Truy cập bị từ chối: Tài khoản '{user}' (Nhóm: {role}) không có đủ tất cả các quyền trong nhóm [{codes}] để thực hiện nghiệp vụ này!");
            }
        }

        /// <summary>
        /// Yêu cầu tài khoản đăng nhập phải mang vai trò Quản trị viên hệ thống (ADMIN).
        /// </summary>
        public static void YeuCauAdmin(string? customMessage = null)
        {
            if (!CurrentSession.IsLoggedIn)
            {
                throw new UnauthorizedAccessException("Phiên làm việc chưa được khởi tạo. Vui lòng đăng nhập hệ thống!");
            }

            if (!CurrentSession.IsAdmin)
            {
                var user = CurrentSession.TenDangNhap ?? "Khách";
                var role = CurrentSession.MaNhomQuyen ?? "Chưa phân quyền";
                throw new UnauthorizedAccessException(customMessage ??
                    $"Yêu cầu quyền Quản trị viên: Tài khoản '{user}' (Nhóm: {role}) không có quyền quản trị tối cao (ADMIN)!");
            }
        }
    }

    /// <summary>
    /// Bí danh Guard ngắn gọn theo đặc tả tại docs/07. Bảo Mật & Phân Quyền.md (§4.2).
    /// </summary>
    public static class Guard
    {
        public static void YeuCauQuyen(string maChucNang, string? customMessage = null) 
            => SecurityGuard.YeuCauQuyen(maChucNang, customMessage);

        public static void YeuCauQuyenBatKy(params string[] maChucNangs) 
            => SecurityGuard.YeuCauQuyenBatKy(maChucNangs);

        public static void YeuCauDuQuyen(params string[] maChucNangs) 
            => SecurityGuard.YeuCauDuQuyen(maChucNangs);

        public static void YeuCauAdmin(string? customMessage = null) 
            => SecurityGuard.YeuCauAdmin(customMessage);
    }
}
