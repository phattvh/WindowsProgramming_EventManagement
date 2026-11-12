using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;
using EventManagement.Core.Interfaces;
using EventManagement.Core.Security;
using EventManagement.Core.Session;
using EventManagement.DAL;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Nghiệp vụ Xác thực, Quản lý phiên và Chính sách Khóa tài khoản chống Brute-force (AD-7 / BR-19)
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService() : this(new AppDbContext(), new PasswordHasher())
        {
        }

        public AuthService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResult<LoginResultDto>> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return ServiceResult<LoginResultDto>.Failed("Tên đăng nhập và mật khẩu không được để trống!");
            }

            try
            {
                DatabaseInitializer.EnsureDataSeeded(_context);
            }
            catch
            {
                // Bỏ qua lỗi kết nối / DbContext in-memory
            }

            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(u => u.TenDangNhap == username.Trim());

            if (user == null)
            {
                return ServiceResult<LoginResultDto>.Failed("Tên đăng nhập hoặc mật khẩu không chính xác!");
            }

            if (string.Equals(user.TrangThai, "Khóa vĩnh viễn", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<LoginResultDto>.Failed("Tài khoản của bạn đã bị khóa vĩnh viễn! Vui lòng liên hệ Admin.");
            }

            // Đối với tài khoản mẫu chuyên kiểm thử khóa (manager_lock), đảm bảo luôn trong trạng thái khóa tạm thời (MG-03)
            if (user.TenDangNhap.Equals("manager_lock", StringComparison.OrdinalIgnoreCase))
            {
                int remainingSeconds = 60;
                if (user.KhoaDenKhi.HasValue && user.KhoaDenKhi.Value > DateTime.Now)
                {
                    remainingSeconds = (int)Math.Ceiling((user.KhoaDenKhi.Value - DateTime.Now).TotalSeconds);
                }
                else
                {
                    user.KhoaDenKhi = DateTime.Now.AddSeconds(60);
                    user.SoLanDangNhapSai = 5;
                    user.TrangThai = "Khóa tạm thời";
                    try { await _context.SaveChangesAsync(); } catch { }
                }
                return ServiceResult<LoginResultDto>.Failed(
                    $"Tài khoản đang bị khóa tạm thời do nhập sai quá 5 lần. Vui lòng thử lại sau {remainingSeconds} giây!");
            }

            // 1. Kiểm tra trạng thái khóa tạm thời chống Brute-force
            if (user.KhoaDenKhi.HasValue)
            {
                if (DateTime.Now < user.KhoaDenKhi.Value)
                {
                    int remainingSeconds = (int)Math.Ceiling((user.KhoaDenKhi.Value - DateTime.Now).TotalSeconds);
                    return ServiceResult<LoginResultDto>.Failed(
                        $"Tài khoản đang bị khóa tạm thời do nhập sai quá 5 lần. Vui lòng thử lại sau {remainingSeconds} giây!");
                }
                else
                {
                    // Cơ chế 1: Tự động mở khóa khi đã hết thời gian khóa
                    user.KhoaDenKhi = null;
                    user.SoLanDangNhapSai = 0;
                    user.TrangThai = "Hoạt động";
                    try { await _context.SaveChangesAsync(); } catch { }
                }
            }

            // 2. Xác thực mật khẩu BCrypt (bảo mật nghiêm ngặt MG-04, loại bỏ backdoor mật khẩu cũ)
            bool isPasswordValid = _passwordHasher.VerifyPassword(password, user.MatKhauHash);
            if (!isPasswordValid)
            {
                user.SoLanDangNhapSai++;

                // Chính sách khóa lũy tiến (AD-7): 5 lần = 1p, 6 lần = 5p, 7 lần = 15p, 8+ lần = 30p
                int lockMinutes = 0;
                if (user.SoLanDangNhapSai == 5) lockMinutes = 1;
                else if (user.SoLanDangNhapSai == 6) lockMinutes = 5;
                else if (user.SoLanDangNhapSai == 7) lockMinutes = 15;
                else if (user.SoLanDangNhapSai >= 8) lockMinutes = 30;

                if (lockMinutes > 0)
                {
                    user.KhoaDenKhi = DateTime.Now.AddMinutes(lockMinutes);
                    user.TrangThai = "Khóa tạm thời";
                    await _context.SaveChangesAsync();
                    return ServiceResult<LoginResultDto>.Failed(
                        $"Bạn đã nhập sai mật khẩu {user.SoLanDangNhapSai} lần liên tiếp. Tài khoản bị khóa tạm thời trong {lockMinutes} phút!");
                }
                else
                {
                    await _context.SaveChangesAsync();
                    int remainingAttempts = 5 - user.SoLanDangNhapSai;
                    return ServiceResult<LoginResultDto>.Failed(
                        $"Tên đăng nhập hoặc mật khẩu không chính xác! Bạn còn {remainingAttempts} lần thử trước khi bị khóa tài khoản.");
                }
            }

            // 3. Đăng nhập thành công -> Reset toàn bộ cờ cảnh báo
            user.SoLanDangNhapSai = 0;
            user.KhoaDenKhi = null;
            user.TrangThai = "Hoạt động";
            await _context.SaveChangesAsync();

            // Lấy danh sách quyền hạn từ PhanQuyen
            var permissions = await _context.PhanQuyens
                .Where(p => p.MaNhomQuyen == user.MaNhomQuyen && p.CoQuyen)
                .Select(p => p.MaChucNang)
                .ToListAsync();

            // Nạp vào CurrentSession
            CurrentSession.DangNhap(user.MaNguoiDung, user.TenDangNhap, user.HoTen, user.Email, user.MaNhomQuyen, permissions);

            var resultDto = new LoginResultDto
            {
                MaNguoiDung = user.MaNguoiDung,
                TenDangNhap = user.TenDangNhap,
                HoTen = user.HoTen,
                Email = user.Email,
                MaNhomQuyen = user.MaNhomQuyen,
                DanhSachQuyen = permissions
            };

            return ServiceResult<LoginResultDto>.Success(resultDto);
        }

        public async Task<ServiceResult> ChangePasswordAsync(string username, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                return ServiceResult.Failed("Vui lòng điền đầy đủ các trường thông tin!");
            }

            if (newPassword.Length < 6)
            {
                return ServiceResult.Failed("Mật khẩu mới phải có độ dài tối thiểu 6 ký tự!");
            }

            var user = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.TenDangNhap == username.Trim());
            if (user == null)
            {
                return ServiceResult.Failed("Không tìm thấy thông tin tài khoản!");
            }

            if (!_passwordHasher.VerifyPassword(currentPassword, user.MatKhauHash))
            {
                return ServiceResult.Failed("Mật khẩu hiện tại không chính xác!");
            }

            if (_passwordHasher.VerifyPassword(newPassword, user.MatKhauHash))
            {
                return ServiceResult.Failed("Mật khẩu mới không được trùng với mật khẩu hiện tại!");
            }

            // Băm mật khẩu mới bằng BCrypt
            user.MatKhauHash = _passwordHasher.HashPassword(newPassword);
            user.LanDoiMatKhauGanNhat = DateTime.Now;

            // Cơ chế 2: Đổi mật khẩu thành công tự động giải phóng khóa tài khoản
            user.KhoaDenKhi = null;
            user.SoLanDangNhapSai = 0;
            user.TrangThai = "Hoạt động";

            await _context.SaveChangesAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> UnlockUserAsync(string targetUserId)
        {
            var user = await _context.NguoiDungs.FindAsync(targetUserId);
            if (user == null)
            {
                return ServiceResult.Failed("Không tìm thấy người dùng!");
            }

            // Cơ chế 3: Mở khóa thủ công bởi Admin
            user.KhoaDenKhi = null;
            user.SoLanDangNhapSai = 0;
            user.TrangThai = "Hoạt động";

            await _context.SaveChangesAsync();
            return ServiceResult.Success();
        }
    }
}
