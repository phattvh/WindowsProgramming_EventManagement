using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;
using EventManagement.Core.Security;
using EventManagement.Core.Session;
using EventManagement.DAL;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Triển khai dịch vụ quản lý người dùng F03 & Mở khóa Brute-force T1-12, T1-13 (Thành viên 1)
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex PhoneRegex = new(@"^(0|\+84)[0-9]{9,10}$", RegexOptions.Compiled);

        public UserService() : this(new AppDbContext(), new PasswordHasher())
        {
        }

        public UserService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<UserDto>> GetAllAsync(string? keyword = null, string? roleId = null, string? status = null)
        {
            var query = _context.NguoiDungs
                .Include(u => u.NhomQuyen)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(u => u.TenDangNhap.ToLower().Contains(kw)
                                      || u.HoTen.ToLower().Contains(kw)
                                      || u.Email.ToLower().Contains(kw)
                                      || (u.SoDienThoai != null && u.SoDienThoai.Contains(kw)));
            }

            if (!string.IsNullOrWhiteSpace(roleId) && roleId != "Tất cả")
            {
                query = query.Where(u => u.MaNhomQuyen == roleId);
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "Tất cả")
            {
                query = query.Where(u => u.TrangThai == status);
            }

            return await query
                .OrderBy(u => u.MaNguoiDung)
                .Select(u => new UserDto
                {
                    MaNguoiDung = u.MaNguoiDung,
                    TenDangNhap = u.TenDangNhap,
                    HoTen = u.HoTen,
                    Email = u.Email,
                    SoDienThoai = u.SoDienThoai,
                    MaNhomQuyen = u.MaNhomQuyen,
                    TenNhomQuyen = u.NhomQuyen != null ? u.NhomQuyen.TenNhomQuyen : u.MaNhomQuyen,
                    TrangThai = u.TrangThai,
                    SoLanDangNhapSai = u.SoLanDangNhapSai,
                    KhoaDenKhi = u.KhoaDenKhi,
                    LanDoiMatKhauGanNhat = u.LanDoiMatKhauGanNhat
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetByIdAsync(string userId)
        {
            var user = await _context.NguoiDungs
                .Include(u => u.NhomQuyen)
                .FirstOrDefaultAsync(u => u.MaNguoiDung == userId);

            if (user == null) return null;

            return new UserDto
            {
                MaNguoiDung = user.MaNguoiDung,
                TenDangNhap = user.TenDangNhap,
                HoTen = user.HoTen,
                Email = user.Email,
                SoDienThoai = user.SoDienThoai,
                MaNhomQuyen = user.MaNhomQuyen,
                TenNhomQuyen = user.NhomQuyen != null ? user.NhomQuyen.TenNhomQuyen : user.MaNhomQuyen,
                TrangThai = user.TrangThai,
                SoLanDangNhapSai = user.SoLanDangNhapSai,
                KhoaDenKhi = user.KhoaDenKhi,
                LanDoiMatKhauGanNhat = user.LanDoiMatKhauGanNhat
            };
        }

        public async Task<ServiceResult<UserDto>> CreateAsync(CreateUserDto dto)
        {
            if (CurrentSession.IsLoggedIn) SecurityGuard.YeuCauQuyen("UC03");
            if (string.IsNullOrWhiteSpace(dto.TenDangNhap))
            {
                return ServiceResult<UserDto>.Failed("Tên đăng nhập không được để trống!");
            }

            if (dto.TenDangNhap.Trim().Length < 3)
            {
                return ServiceResult<UserDto>.Failed("Tên đăng nhập phải có ít nhất 3 ký tự!");
            }

            if (string.IsNullOrWhiteSpace(dto.MatKhau) || dto.MatKhau.Length < 6)
            {
                return ServiceResult<UserDto>.Failed("Mật khẩu phải có độ dài tối thiểu 6 ký tự!");
            }

            if (string.IsNullOrWhiteSpace(dto.HoTen))
            {
                return ServiceResult<UserDto>.Failed("Họ và tên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return ServiceResult<UserDto>.Failed("Email không được để trống!");
            }

            if (!EmailRegex.IsMatch(dto.Email.Trim()))
            {
                return ServiceResult<UserDto>.Failed("Định dạng email không hợp lệ (ví dụ: user@example.com)!");
            }

            if (!string.IsNullOrWhiteSpace(dto.SoDienThoai) && !PhoneRegex.IsMatch(dto.SoDienThoai.Trim()))
            {
                return ServiceResult<UserDto>.Failed("Số điện thoại không hợp lệ (cần 10 số, bắt đầu bằng 0 hoặc +84)!");
            }

            string cleanUsername = dto.TenDangNhap.Trim();
            bool isDuplicate = await _context.NguoiDungs.AnyAsync(u => u.TenDangNhap.ToLower() == cleanUsername.ToLower());
            if (isDuplicate)
            {
                return ServiceResult<UserDto>.Failed($"Tên đăng nhập '{cleanUsername}' đã tồn tại trong hệ thống!");
            }

            // Tự động sinh mã người dùng nếu chưa có
            string newId = dto.MaNguoiDung?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(newId))
            {
                newId = await GenerateNextUserIdAsync();
            }
            else if (await _context.NguoiDungs.AnyAsync(u => u.MaNguoiDung == newId))
            {
                return ServiceResult<UserDto>.Failed($"Mã người dùng '{newId}' đã tồn tại!");
            }

            var newUser = new NguoiDung
            {
                MaNguoiDung = newId,
                TenDangNhap = cleanUsername,
                MatKhauHash = _passwordHasher.HashPassword(dto.MatKhau),
                HoTen = dto.HoTen.Trim(),
                Email = dto.Email.Trim(),
                SoDienThoai = dto.SoDienThoai?.Trim(),
                MaNhomQuyen = dto.MaNhomQuyen,
                TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "Hoạt động" : dto.TrangThai,
                SoLanDangNhapSai = 0,
                KhoaDenKhi = null,
                LanDoiMatKhauGanNhat = DateTime.Now
            };

            await _context.NguoiDungs.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var resultDto = new UserDto
            {
                MaNguoiDung = newUser.MaNguoiDung,
                TenDangNhap = newUser.TenDangNhap,
                HoTen = newUser.HoTen,
                Email = newUser.Email,
                SoDienThoai = newUser.SoDienThoai,
                MaNhomQuyen = newUser.MaNhomQuyen,
                TenNhomQuyen = newUser.MaNhomQuyen,
                TrangThai = newUser.TrangThai,
                SoLanDangNhapSai = 0,
                KhoaDenKhi = null,
                LanDoiMatKhauGanNhat = newUser.LanDoiMatKhauGanNhat
            };

            return ServiceResult<UserDto>.Success(resultDto, "Thêm tài khoản người dùng thành công!");
        }

        public async Task<ServiceResult> UpdateAsync(string userId, UpdateUserDto dto)
        {
            if (CurrentSession.IsLoggedIn) SecurityGuard.YeuCauQuyen("UC03");
            var user = await _context.NguoiDungs.FindAsync(userId);
            if (user == null)
            {
                return ServiceResult.Failed("Không tìm thấy thông tin tài khoản!");
            }

            if (string.IsNullOrWhiteSpace(dto.HoTen))
            {
                return ServiceResult.Failed("Họ và tên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return ServiceResult.Failed("Email không được để trống!");
            }

            if (!EmailRegex.IsMatch(dto.Email.Trim()))
            {
                return ServiceResult.Failed("Định dạng email không hợp lệ (ví dụ: user@example.com)!");
            }

            if (!string.IsNullOrWhiteSpace(dto.SoDienThoai) && !PhoneRegex.IsMatch(dto.SoDienThoai.Trim()))
            {
                return ServiceResult.Failed("Số điện thoại không hợp lệ (cần 10 số, bắt đầu bằng 0 hoặc +84)!");
            }

            // Ràng buộc an toàn: Không tự khóa/hạ quyền chính mình hoặc khóa/hạ quyền Admin duy nhất
            bool isSelfLockOrDemote = IsCurrentSessionUser(userId)
                && (dto.TrangThai != "Hoạt động" || (user.MaNhomQuyen == "ADMIN" && dto.MaNhomQuyen != "ADMIN"));
            if (isSelfLockOrDemote)
            {
                return ServiceResult.Failed("Không thể khóa hoặc hạ quyền tài khoản bạn đang sử dụng trong phiên làm việc hiện tại!");
            }

            if (user.MaNhomQuyen == "ADMIN")
            {
                bool isDemotingOrLocking = dto.MaNhomQuyen != "ADMIN" || dto.TrangThai != "Hoạt động";
                if (isDemotingOrLocking && !await HasOtherActiveAdminAsync(userId))
                {
                    return ServiceResult.Failed("Không thể khóa hoặc hạ quyền Quản trị viên (ADMIN) cuối cùng đang hoạt động!");
                }
            }

            user.HoTen = dto.HoTen.Trim();
            user.Email = dto.Email.Trim();
            user.SoDienThoai = dto.SoDienThoai?.Trim();
            user.MaNhomQuyen = dto.MaNhomQuyen;
            user.TrangThai = dto.TrangThai;

            // Nếu có đổi mật khẩu mới
            if (!string.IsNullOrWhiteSpace(dto.MatKhauMoi))
            {
                if (dto.MatKhauMoi.Length < 6)
                {
                    return ServiceResult.Failed("Mật khẩu mới phải có tối thiểu 6 ký tự!");
                }
                user.MatKhauHash = _passwordHasher.HashPassword(dto.MatKhauMoi);
                user.LanDoiMatKhauGanNhat = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return ServiceResult.Success("Cập nhật thông tin tài khoản thành công!");
        }

        public async Task<ServiceResult> DeleteAsync(string userId)
        {
            if (CurrentSession.IsLoggedIn) SecurityGuard.YeuCauQuyen("UC03");
            var user = await _context.NguoiDungs.FindAsync(userId);
            if (user == null)
            {
                return ServiceResult.Failed("Không tìm thấy tài khoản cần xóa!");
            }

            // Ràng buộc an ninh: Không cho phép tự xóa chính tài khoản đang đăng nhập
            if (IsCurrentSessionUser(user.MaNguoiDung))
            {
                return ServiceResult.Failed("Không thể xóa tài khoản bạn đang sử dụng trong phiên làm việc hiện tại!");
            }

            // Ràng buộc an toàn: Không được xóa Admin cuối cùng đang hoạt động
            if (user.MaNhomQuyen == "ADMIN" && !await HasOtherActiveAdminAsync(userId))
            {
                return ServiceResult.Failed("Không được phép xóa tài khoản Quản trị viên (Admin) cuối cùng đang hoạt động trong hệ thống!");
            }

            try
            {
                // Xóa người dùng
                _context.NguoiDungs.Remove(user);
                await _context.SaveChangesAsync();
                return ServiceResult.Success("Xóa tài khoản thành công!");
            }
            catch (DbUpdateException)
            {
                return ServiceResult.Failed("Không thể xóa tài khoản này do đã có dữ liệu ràng buộc trong hệ thống (Sự kiện, Điểm danh hoặc Chi phí). Vui lòng chuyển trạng thái sang 'Khóa vĩnh viễn' thay vì xóa!");
            }
        }

        /// <summary>
        /// Mở khóa tức thì cho tài khoản bị khóa do Brute-force hoặc do Admin (Task T1-13 / AD-7 / BR-19)
        /// </summary>
        public async Task<ServiceResult> UnlockUserAsync(string userId)
        {
            if (CurrentSession.IsLoggedIn) SecurityGuard.YeuCauQuyen("UC03");
            var user = await _context.NguoiDungs.FindAsync(userId);
            if (user == null)
            {
                return ServiceResult.Failed("Không tìm thấy tài khoản!");
            }

            user.SoLanDangNhapSai = 0;
            user.KhoaDenKhi = null;
            user.TrangThai = "Hoạt động";

            await _context.SaveChangesAsync();
            return ServiceResult.Success($"Đã mở khóa và khôi phục trạng thái hoạt động cho tài khoản '{user.TenDangNhap}' thành công!");
        }

        public async Task<ServiceResult> ToggleLockAsync(string userId)
        {
            if (CurrentSession.IsLoggedIn) SecurityGuard.YeuCauQuyen("UC03");
            var user = await _context.NguoiDungs.FindAsync(userId);
            if (user == null)
            {
                return ServiceResult.Failed("Không tìm thấy tài khoản!");
            }

            if (user.TrangThai == "Hoạt động")
            {
                if (IsCurrentSessionUser(userId))
                {
                    return ServiceResult.Failed("Không thể khóa tài khoản bạn đang sử dụng trong phiên làm việc hiện tại!");
                }

                // Khóa tài khoản -> kiểm tra admin cuối cùng đang hoạt động
                if (user.MaNhomQuyen == "ADMIN" && !await HasOtherActiveAdminAsync(userId))
                {
                    return ServiceResult.Failed("Không thể khóa Quản trị viên (ADMIN) cuối cùng đang hoạt động!");
                }

                user.TrangThai = "Khóa vĩnh viễn";
            }
            else
            {
                // Mở khóa tài khoản
                user.TrangThai = "Hoạt động";
                user.SoLanDangNhapSai = 0;
                user.KhoaDenKhi = null;
            }

            await _context.SaveChangesAsync();
            return ServiceResult.Success($"Cập nhật trạng thái tài khoản '{user.TenDangNhap}' thành '{user.TrangThai}' thành công!");
        }

        private static bool IsCurrentSessionUser(string userId)
            => CurrentSession.IsLoggedIn
               && string.Equals(userId, CurrentSession.MaNguoiDung, StringComparison.OrdinalIgnoreCase);

        private async Task<bool> HasOtherActiveAdminAsync(string excludedUserId)
            => await _context.NguoiDungs
                .AnyAsync(u => u.MaNhomQuyen == "ADMIN"
                            && u.TrangThai == "Hoạt động"
                            && u.MaNguoiDung != excludedUserId);

        private async Task<string> GenerateNextUserIdAsync()
        {
            var existingIds = await _context.NguoiDungs
                .Select(u => u.MaNguoiDung)
                .ToListAsync();

            int maxIndex = 0;
            foreach (var id in existingIds)
            {
                if (id.StartsWith("USR", StringComparison.OrdinalIgnoreCase)
                    && int.TryParse(id.Substring(3), out int num)
                    && num > maxIndex)
                {
                    maxIndex = num;
                }
            }

            int nextIndex = maxIndex + 1;
            string candidate = $"USR{nextIndex:D3}";
            while (existingIds.Contains(candidate, StringComparer.OrdinalIgnoreCase))
            {
                nextIndex++;
                candidate = $"USR{nextIndex:D3}";
            }
            return candidate;
        }
    }
}
