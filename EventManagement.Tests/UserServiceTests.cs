using System;
using System.Threading.Tasks;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
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
    /// Kiểm thử nghiệp vụ UserService và Thao tác Mở khóa Brute-force T1-12, T1-13 (Thành viên 1)
    /// </summary>
    [TestClass]
    public class UserServiceTests
    {
        private readonly IPasswordHasher _hasher = new PasswordHasher();

        [TestCleanup]
        public void CleanupSession()
        {
            CurrentSession.DangXuat();
        }

        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            // Nạp sẵn dữ liệu nhóm quyền
            context.NhomQuyens.AddRange(
                new NhomQuyen { MaNhomQuyen = "ADMIN", TenNhomQuyen = "Quản trị viên" },
                new NhomQuyen { MaNhomQuyen = "MANAGER", TenNhomQuyen = "Quản lý sự kiện" },
                new NhomQuyen { MaNhomQuyen = "STAFF", TenNhomQuyen = "Nhân viên hỗ trợ" }
            );
            context.SaveChanges();

            return context;
        }

        [TestMethod]
        public async Task GetAllAsync_WithKeyword_ReturnsMatchingUsers()
        {
            // Arrange
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.AddRange(
                new NguoiDung { MaNguoiDung = "U01", TenDangNhap = "admin", HoTen = "Nguyen Van Admin", Email = "admin@event.vn", MaNhomQuyen = "ADMIN" },
                new NguoiDung { MaNguoiDung = "U02", TenDangNhap = "staff01", HoTen = "Tran Thi Staff", Email = "staff@event.vn", MaNhomQuyen = "STAFF" }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetAllAsync(keyword: "Staff");

            // Assert
            result.Should().HaveCount(1);
            result[0].TenDangNhap.Should().Be("staff01");
        }

        [TestMethod]
        public async Task CreateAsync_WithDuplicateUsername_ReturnsFailed()
        {
            // Arrange
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.Add(new NguoiDung 
            { 
                MaNguoiDung = "U01", 
                TenDangNhap = "admin", 
                HoTen = "Admin", 
                Email = "admin@event.vn", 
                MaNhomQuyen = "ADMIN" 
            });
            await context.SaveChangesAsync();

            var dto = new CreateUserDto
            {
                TenDangNhap = "admin", // Trùng tên đăng nhập
                MatKhau = "NewPassword@123",
                HoTen = "Another Admin",
                Email = "another@event.vn",
                MaNhomQuyen = "ADMIN"
            };

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("đã tồn tại");
        }

        [TestMethod]
        public async Task CreateAsync_WithValidData_HashesPasswordAndSavesUser()
        {
            // Arrange
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            var dto = new CreateUserDto
            {
                TenDangNhap = "event_staff",
                MatKhau = "Secret@123",
                HoTen = "Pham Nhan Vien",
                Email = "staff@event.vn",
                SoDienThoai = "0988123456",
                MaNhomQuyen = "STAFF"
            };

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.TenDangNhap.Should().Be("event_staff");

            var savedInDb = await context.NguoiDungs.FirstOrDefaultAsync(u => u.TenDangNhap == "event_staff");
            savedInDb.Should().NotBeNull();
            _hasher.VerifyPassword("Secret@123", savedInDb!.MatKhauHash).Should().BeTrue();
        }

        [TestMethod]
        public async Task CreateAsync_WithInvalidEmail_ReturnsFailed()
        {
            // Arrange
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            var dto = new CreateUserDto
            {
                TenDangNhap = "staff_bad_email",
                MatKhau = "Secret@123",
                HoTen = "Pham Nhan Vien",
                Email = "invalid-email-format",
                MaNhomQuyen = "STAFF"
            };

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("email không hợp lệ");
        }

        [TestMethod]
        public async Task CreateAsync_WithEmptyFullName_ReturnsFailed()
        {
            // Arrange
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            var dto = new CreateUserDto
            {
                TenDangNhap = "staff_noname",
                MatKhau = "Secret@123",
                HoTen = "   ",
                Email = "noname@event.vn",
                MaNhomQuyen = "STAFF"
            };

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Họ và tên không được để trống");
        }

        [TestMethod]
        public async Task UpdateAsync_AttemptingToLockLastAdmin_ReturnsFailed()
        {
            // Arrange: Chỉ có đúng 1 tài khoản Admin
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.Add(new NguoiDung
            {
                MaNguoiDung = "U_ADMIN",
                TenDangNhap = "admin",
                HoTen = "Sole Admin",
                Email = "admin@event.vn",
                MaNhomQuyen = "ADMIN",
                TrangThai = "Hoạt động"
            });
            await context.SaveChangesAsync();

            var updateDto = new UpdateUserDto
            {
                HoTen = "Sole Admin",
                Email = "admin@event.vn",
                MaNhomQuyen = "ADMIN",
                TrangThai = "Khóa vĩnh viễn" // Cố tình khóa Admin cuối cùng
            };

            // Act
            var result = await service.UpdateAsync("U_ADMIN", updateDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("cuối cùng");
        }

        [TestMethod]
        public async Task DeleteAsync_LastAdmin_ReturnsFailed()
        {
            // Arrange
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.Add(new NguoiDung
            {
                MaNguoiDung = "U_ADMIN",
                TenDangNhap = "admin",
                HoTen = "Sole Admin",
                Email = "admin@event.vn",
                MaNhomQuyen = "ADMIN"
            });
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteAsync("U_ADMIN");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("cuối cùng");
        }

        [TestMethod]
        public async Task DeleteAsync_NormalUser_DeletesSuccessfully()
        {
            // Arrange
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.Add(new NguoiDung
            {
                MaNguoiDung = "U_STAFF",
                TenDangNhap = "staff99",
                HoTen = "Temporary Staff",
                Email = "staff99@event.vn",
                MaNhomQuyen = "STAFF"
            });
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteAsync("U_STAFF");

            // Assert
            result.IsSuccess.Should().BeTrue();
            var inDb = await context.NguoiDungs.FindAsync("U_STAFF");
            inDb.Should().BeNull();
        }

        [TestMethod]
        public async Task UnlockUserAsync_ResetsFailedAttemptsAndClearsLockout()
        {
            // Arrange: Tài khoản bị khóa do Brute-force
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.Add(new NguoiDung
            {
                MaNguoiDung = "U_LOCKED",
                TenDangNhap = "locked_user",
                HoTen = "Locked User",
                Email = "locked@event.vn",
                MaNhomQuyen = "STAFF",
                SoLanDangNhapSai = 6,
                KhoaDenKhi = DateTime.Now.AddMinutes(15),
                TrangThai = "Khóa tạm thời"
            });
            await context.SaveChangesAsync();

            // Act: Admin nhấn nút Mở khóa tài khoản (Task T1-13)
            var result = await service.UnlockUserAsync("U_LOCKED");

            // Assert
            result.IsSuccess.Should().BeTrue();
            var updated = await context.NguoiDungs.FindAsync("U_LOCKED");
            updated!.SoLanDangNhapSai.Should().Be(0);
            updated.KhoaDenKhi.Should().BeNull();
            updated.TrangThai.Should().Be("Hoạt động");
        }

        [TestMethod]
        public async Task UnlockUserAsync_PermanentlyLockedUser_RestoresToActive()
        {
            // Arrange: Tài khoản bị Admin Khóa vĩnh viễn
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.Add(new NguoiDung
            {
                MaNguoiDung = "U_PERM_LOCKED",
                TenDangNhap = "perm_locked",
                HoTen = "Permanently Locked",
                Email = "perm@event.vn",
                MaNhomQuyen = "STAFF",
                TrangThai = "Khóa vĩnh viễn"
            });
            await context.SaveChangesAsync();

            // Act: Admin bấm mở khóa tài khoản
            var result = await service.UnlockUserAsync("U_PERM_LOCKED");

            // Assert: Trạng thái phải chuyển thành "Hoạt động"
            result.IsSuccess.Should().BeTrue();
            var updated = await context.NguoiDungs.FindAsync("U_PERM_LOCKED");
            updated!.TrangThai.Should().Be("Hoạt động");
            updated.SoLanDangNhapSai.Should().Be(0);
            updated.KhoaDenKhi.Should().BeNull();
        }

        [TestMethod]
        public async Task ToggleLockAsync_WhenActive_LocksUser()
        {
            // Arrange
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.Add(new NguoiDung
            {
                MaNguoiDung = "U_TOGGLE",
                TenDangNhap = "toggle_user",
                HoTen = "Toggle User",
                Email = "toggle@event.vn",
                MaNhomQuyen = "STAFF",
                TrangThai = "Hoạt động"
            });
            await context.SaveChangesAsync();

            // Act
            var result = await service.ToggleLockAsync("U_TOGGLE");

            // Assert
            result.IsSuccess.Should().BeTrue();
            var updated = await context.NguoiDungs.FindAsync("U_TOGGLE");
            updated!.TrangThai.Should().Be("Khóa vĩnh viễn");
        }

        [TestMethod]
        public async Task CreateAsync_AutoGeneratesUsrPrefixedCode_Sequentially()
        {
            // Arrange (AD-09, AD-10, AD-11): Đã có USR001..USR011
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            for (int i = 1; i <= 11; i++)
            {
                context.NguoiDungs.Add(new NguoiDung
                {
                    MaNguoiDung = $"USR{i:D3}",
                    TenDangNhap = $"user{i:D2}",
                    HoTen = $"User {i}",
                    Email = $"user{i}@event.vn",
                    MaNhomQuyen = i == 1 ? "ADMIN" : "STAFF"
                });
            }
            await context.SaveChangesAsync();

            // Act
            var res12 = await service.CreateAsync(new CreateUserDto
            {
                TenDangNhap = "staff_new",
                MatKhau = "Staff@123",
                HoTen = "Hoang Van Nam",
                Email = "nam@event.vn",
                MaNhomQuyen = "STAFF"
            });

            var res13 = await service.CreateAsync(new CreateUserDto
            {
                TenDangNhap = "mgr_new",
                MatKhau = "Manager@123",
                HoTen = "Hoa Phat",
                Email = "phat@event.vn",
                MaNhomQuyen = "MANAGER"
            });

            // Assert
            res12.IsSuccess.Should().BeTrue();
            res12.Data!.MaNguoiDung.Should().Be("USR012");
            res13.IsSuccess.Should().BeTrue();
            res13.Data!.MaNguoiDung.Should().Be("USR013");
        }

        [TestMethod]
        public async Task DeleteAsync_LastActiveAdmin_WhenOnlyLockedAdminExists_ReturnsFailed()
        {
            // Arrange (AD-25): 1 Admin Hoạt động + 1 Admin Khóa tạm thời
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.AddRange(
                new NguoiDung
                {
                    MaNguoiDung = "USR001",
                    TenDangNhap = "admin",
                    HoTen = "Active Admin",
                    Email = "admin@event.vn",
                    MaNhomQuyen = "ADMIN",
                    TrangThai = "Hoạt động"
                },
                new NguoiDung
                {
                    MaNguoiDung = "USR005",
                    TenDangNhap = "admin_lock",
                    HoTen = "Locked Admin",
                    Email = "admin.lock@event.vn",
                    MaNhomQuyen = "ADMIN",
                    TrangThai = "Khóa tạm thời"
                });
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteAsync("USR001");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("cuối cùng");
            (await context.NguoiDungs.FindAsync("USR001")).Should().NotBeNull();
        }

        [TestMethod]
        public async Task DeleteAsync_CurrentlyLoggedInUser_ReturnsFailed()
        {
            // Arrange (AD-43): Đang đăng nhập USR001, hệ thống vẫn còn USR004 (ADMIN khác)
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.AddRange(
                new NguoiDung
                {
                    MaNguoiDung = "USR001",
                    TenDangNhap = "admin",
                    HoTen = "Primary Admin",
                    Email = "admin@event.vn",
                    MaNhomQuyen = "ADMIN",
                    TrangThai = "Hoạt động"
                },
                new NguoiDung
                {
                    MaNguoiDung = "USR004",
                    TenDangNhap = "admin2",
                    HoTen = "Backup Admin",
                    Email = "admin2@event.vn",
                    MaNhomQuyen = "ADMIN",
                    TrangThai = "Hoạt động"
                });
            await context.SaveChangesAsync();

            CurrentSession.DangNhap("USR001", "admin", "Primary Admin", "admin@event.vn", "ADMIN", new[] { "UC03" });

            // Act
            var result = await service.DeleteAsync("USR001");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("đang sử dụng");
            (await context.NguoiDungs.FindAsync("USR001")).Should().NotBeNull();
        }

        [TestMethod]
        public async Task UpdateAndToggleLockAsync_SelfAccount_ReturnsFailed()
        {
            // Arrange: Admin đang đăng nhập không được tự khóa hoặc tự hạ quyền chính mình dù còn Admin khác
            using var context = CreateContext();
            var service = new UserService(context, _hasher);

            context.NguoiDungs.AddRange(
                new NguoiDung
                {
                    MaNguoiDung = "USR001",
                    TenDangNhap = "admin",
                    HoTen = "Primary Admin",
                    Email = "admin@event.vn",
                    MaNhomQuyen = "ADMIN",
                    TrangThai = "Hoạt động"
                },
                new NguoiDung
                {
                    MaNguoiDung = "USR004",
                    TenDangNhap = "admin2",
                    HoTen = "Backup Admin",
                    Email = "admin2@event.vn",
                    MaNhomQuyen = "ADMIN",
                    TrangThai = "Hoạt động"
                });
            await context.SaveChangesAsync();

            CurrentSession.DangNhap("USR001", "admin", "Primary Admin", "admin@event.vn", "ADMIN", new[] { "UC03" });

            // Act
            var demoteResult = await service.UpdateAsync("USR001", new UpdateUserDto
            {
                HoTen = "Primary Admin",
                Email = "admin@event.vn",
                MaNhomQuyen = "STAFF",
                TrangThai = "Hoạt động"
            });
            var lockResult = await service.ToggleLockAsync("USR001");

            // Assert
            demoteResult.IsSuccess.Should().BeFalse();
            demoteResult.Message.Should().Contain("đang sử dụng");
            lockResult.IsSuccess.Should().BeFalse();
            lockResult.Message.Should().Contain("đang sử dụng");
        }
    }
}
