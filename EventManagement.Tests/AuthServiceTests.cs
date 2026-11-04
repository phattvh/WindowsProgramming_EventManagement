using System;
using System.Threading.Tasks;
using EventManagement.BLL.Services;
using EventManagement.Core.Entities;
using EventManagement.Core.Security;
using EventManagement.Core.Session;
using EventManagement.DAL;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử nghiệp vụ AuthService và Chính sách chống Brute-force AD-7 / BR-19 (Thành viên 1)
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class AuthServiceTests
    {
        private readonly IPasswordHasher _hasher = new PasswordHasher();

        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [TestCleanup]
        public void Cleanup()
        {
            CurrentSession.DangXuat();
        }

        [TestMethod]
        public async Task LoginAsync_WithCorrectCredentials_ReturnsSuccessAndResetsFailedCount()
        {
            // Arrange
            using var context = CreateContext();
            var authService = new AuthService(context, _hasher);

            var user = new NguoiDung
            {
                MaNguoiDung = "U01",
                TenDangNhap = "admin",
                MatKhauHash = _hasher.HashPassword("Admin@123"),
                HoTen = "Admin",
                Email = "admin@event.vn",
                MaNhomQuyen = "ADMIN",
                SoLanDangNhapSai = 3
            };
            await context.NguoiDungs.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await authService.LoginAsync("admin", "Admin@123");

            // Assert
            result.IsSuccess.Should().BeTrue(result.Message);
            result.Data.Should().NotBeNull();
            result.Data!.TenDangNhap.Should().Be("admin");

            var updated = await context.NguoiDungs.FindAsync("U01");
            updated!.SoLanDangNhapSai.Should().Be(0);
            updated.KhoaDenKhi.Should().BeNull();
            CurrentSession.IsLoggedIn.Should().BeTrue();
        }

        [TestMethod]
        public async Task LoginAsync_WithWrongPassword_IncrementsFailedCount()
        {
            // Arrange
            using var context = CreateContext();
            var authService = new AuthService(context, _hasher);

            var user = new NguoiDung
            {
                MaNguoiDung = "U01",
                TenDangNhap = "admin",
                MatKhauHash = _hasher.HashPassword("Admin@123"),
                HoTen = "Admin",
                Email = "admin@event.vn",
                MaNhomQuyen = "ADMIN",
                SoLanDangNhapSai = 0
            };
            await context.NguoiDungs.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await authService.LoginAsync("admin", "WrongPassword");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Bạn còn 4 lần thử");

            var updated = await context.NguoiDungs.FindAsync("U01");
            updated!.SoLanDangNhapSai.Should().Be(1);
            updated.KhoaDenKhi.Should().BeNull();
        }

        [TestMethod]
        public async Task LoginAsync_With5FailedAttempts_LocksAccountFor1Minute()
        {
            // Arrange
            using var context = CreateContext();
            var authService = new AuthService(context, _hasher);

            var user = new NguoiDung
            {
                MaNguoiDung = "U01",
                TenDangNhap = "staff",
                MatKhauHash = _hasher.HashPassword("Staff@123"),
                HoTen = "Staff",
                Email = "staff@event.vn",
                MaNhomQuyen = "STAFF",
                SoLanDangNhapSai = 4 // Lần thử này là lần thứ 5
            };
            await context.NguoiDungs.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await authService.LoginAsync("staff", "WrongPassword");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("khóa tạm thời trong 1 phút");

            var updated = await context.NguoiDungs.FindAsync("U01");
            updated!.SoLanDangNhapSai.Should().Be(5);
            updated.KhoaDenKhi.Should().NotBeNull();
            updated.TrangThai.Should().Be("Khóa tạm thời");
        }

        [TestMethod]
        public async Task LoginAsync_WhenAccountIsLocked_RejectsImmediatelyWithRemainingTime()
        {
            // Arrange
            using var context = CreateContext();
            var authService = new AuthService(context, _hasher);

            var user = new NguoiDung
            {
                MaNguoiDung = "U01",
                TenDangNhap = "lockeduser",
                MatKhauHash = _hasher.HashPassword("ValidPassword@123"),
                HoTen = "Locked User",
                Email = "locked@event.vn",
                MaNhomQuyen = "STAFF",
                SoLanDangNhapSai = 5,
                KhoaDenKhi = DateTime.Now.AddMinutes(5),
                TrangThai = "Khóa tạm thời"
            };
            await context.NguoiDungs.AddAsync(user);
            await context.SaveChangesAsync();

            // Act: Ngay cả khi gõ đúng mật khẩu, đang trong thời gian khóa vẫn bị từ chối
            var result = await authService.LoginAsync("lockeduser", "ValidPassword@123");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Tài khoản đang bị khóa tạm thời");
        }

        [TestMethod]
        public async Task LoginAsync_WhenLockExpired_AutoUnlocksAndAllowsLogin()
        {
            // Arrange
            using var context = CreateContext();
            var authService = new AuthService(context, _hasher);

            var user = new NguoiDung
            {
                MaNguoiDung = "U01",
                TenDangNhap = "expiredlockuser",
                MatKhauHash = _hasher.HashPassword("Admin@123"),
                HoTen = "User",
                Email = "user@event.vn",
                MaNhomQuyen = "ADMIN",
                SoLanDangNhapSai = 5,
                KhoaDenKhi = DateTime.Now.AddSeconds(-10), // Khóa đã hết hạn 10 giây trước
                TrangThai = "Khóa tạm thời"
            };
            await context.NguoiDungs.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await authService.LoginAsync("expiredlockuser", "Admin@123");

            // Assert
            result.IsSuccess.Should().BeTrue();
            var updated = await context.NguoiDungs.FindAsync("U01");
            updated!.KhoaDenKhi.Should().BeNull();
            updated.SoLanDangNhapSai.Should().Be(0);
            updated.TrangThai.Should().Be("Hoạt động");
        }

        [TestMethod]
        public async Task ChangePasswordAsync_WithCorrectOldPassword_UpdatesHashAndClearsLock()
        {
            // Arrange
            using var context = CreateContext();
            var authService = new AuthService(context, _hasher);

            var user = new NguoiDung
            {
                MaNguoiDung = "U01",
                TenDangNhap = "user01",
                MatKhauHash = _hasher.HashPassword("OldPass@123"),
                HoTen = "User",
                Email = "user@event.vn",
                MaNhomQuyen = "STAFF",
                SoLanDangNhapSai = 4,
                KhoaDenKhi = DateTime.Now.AddMinutes(1)
            };
            await context.NguoiDungs.AddAsync(user);
            await context.SaveChangesAsync();

            // Act: Đổi mật khẩu
            var result = await authService.ChangePasswordAsync("user01", "OldPass@123", "NewSecurePass@456");

            // Assert
            result.IsSuccess.Should().BeTrue();
            var updated = await context.NguoiDungs.FindAsync("U01");
            _hasher.VerifyPassword("NewSecurePass@456", updated!.MatKhauHash).Should().BeTrue();
            updated.KhoaDenKhi.Should().BeNull();
            updated.SoLanDangNhapSai.Should().Be(0);
            updated.LanDoiMatKhauGanNhat.Should().NotBeNull();
        }

        [TestMethod]
        public async Task UnlockUserAsync_ByAdmin_RestoresActiveStatus()
        {
            // Arrange
            using var context = CreateContext();
            var authService = new AuthService(context, _hasher);

            var user = new NguoiDung
            {
                MaNguoiDung = "U_LOCKED",
                TenDangNhap = "lockedstaff",
                MatKhauHash = _hasher.HashPassword("Staff@123"),
                HoTen = "Staff",
                Email = "staff@event.vn",
                MaNhomQuyen = "STAFF",
                SoLanDangNhapSai = 6,
                KhoaDenKhi = DateTime.Now.AddMinutes(5),
                TrangThai = "Khóa tạm thời"
            };
            await context.NguoiDungs.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await authService.UnlockUserAsync("U_LOCKED");

            // Assert
            result.IsSuccess.Should().BeTrue();
            var updated = await context.NguoiDungs.FindAsync("U_LOCKED");
            updated!.KhoaDenKhi.Should().BeNull();
            updated.SoLanDangNhapSai.Should().Be(0);
            updated.TrangThai.Should().Be("Hoạt động");
        }
    }
}
