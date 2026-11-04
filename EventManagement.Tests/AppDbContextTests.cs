using System;
using System.Threading.Tasks;
using EventManagement.Core.Entities;
using EventManagement.DAL;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử hạ tầng AppDbContext với EF Core InMemory (Thành viên 1)
    /// </summary>
    [TestClass]
    public class AppDbContextTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [TestMethod]
        public async Task AppDbContext_CanSaveAndRetrieveNguoiDungWithNhomQuyen()
        {
            // Arrange
            using var context = CreateContext();
            var role = new NhomQuyen
            {
                MaNhomQuyen = "ADMIN",
                TenNhomQuyen = "Quản trị viên",
                MoTa = "Toàn quyền quản trị"
            };
            var user = new NguoiDung
            {
                MaNguoiDung = "USR001",
                TenDangNhap = "admin",
                MatKhauHash = "$2a$11$samplehashstringfortestingpurposeonly00000000000000",
                HoTen = "Quản trị viên Hệ thống",
                Email = "admin@event.vn",
                MaNhomQuyen = "ADMIN",
                TrangThai = "Hoạt động",
                SoLanDangNhapSai = 0
            };

            // Act
            await context.NhomQuyens.AddAsync(role);
            await context.NguoiDungs.AddAsync(user);
            int saved = await context.SaveChangesAsync();

            // Assert
            saved.Should().Be(2);

            var retrievedUser = await context.NguoiDungs
                .Include(u => u.NhomQuyen)
                .FirstOrDefaultAsync(u => u.TenDangNhap == "admin");

            retrievedUser.Should().NotBeNull();
            retrievedUser!.HoTen.Should().Be("Quản trị viên Hệ thống");
            retrievedUser.NhomQuyen.Should().NotBeNull();
            retrievedUser.NhomQuyen!.TenNhomQuyen.Should().Be("Quản trị viên");
        }

        [TestMethod]
        public async Task AppDbContext_CanSavePermissionMatrix()
        {
            // Arrange
            using var context = CreateContext();
            var role = new NhomQuyen { MaNhomQuyen = "STAFF", TenNhomQuyen = "Nhân viên" };
            var func = new ChucNang { MaChucNang = "UC13", TenChucNang = "Soát vé Check-in", MaForm = "F13" };
            var permission = new PhanQuyen { MaNhomQuyen = "STAFF", MaChucNang = "UC13", CoQuyen = true };

            // Act
            await context.NhomQuyens.AddAsync(role);
            await context.ChucNangs.AddAsync(func);
            await context.PhanQuyens.AddAsync(permission);
            await context.SaveChangesAsync();

            // Assert
            var retrieved = await context.PhanQuyens.FindAsync("STAFF", "UC13");
            retrieved.Should().NotBeNull();
            retrieved!.CoQuyen.Should().BeTrue();
        }
    }
}
