using System;
using System.Linq;
using System.Threading.Tasks;
using EventManagement.Core.Entities;
using EventManagement.DAL;
using EventManagement.DAL.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử GenericRepository<T> và UnitOfWork (Thành viên 1)
    /// </summary>
    [TestClass]
    public class GenericRepositoryTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [TestMethod]
        public async Task AddAsync_ShouldPersistEntity()
        {
            // Arrange
            using var context = CreateContext();
            var repo = new GenericRepository<NhomQuyen>(context);
            var role = new NhomQuyen { MaNhomQuyen = "MANAGER", TenNhomQuyen = "Quản lý sự kiện" };

            // Act
            await repo.AddAsync(role);
            await repo.SaveChangesAsync();

            // Assert
            var result = await repo.GetByIdAsync("MANAGER");
            result.Should().NotBeNull();
            result!.TenNhomQuyen.Should().Be("Quản lý sự kiện");
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            using var context = CreateContext();
            var repo = new GenericRepository<ChucNang>(context);
            await repo.AddAsync(new ChucNang { MaChucNang = "UC01", TenChucNang = "Đăng nhập", MaForm = "F01" });
            await repo.AddAsync(new ChucNang { MaChucNang = "UC02", TenChucNang = "Dashboard", MaForm = "F02" });
            await repo.SaveChangesAsync();

            // Act
            var all = await repo.GetAllAsync();

            // Assert
            all.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task Update_ShouldModifyEntityValues()
        {
            // Arrange
            using var context = CreateContext();
            var repo = new GenericRepository<NhomQuyen>(context);
            var role = new NhomQuyen { MaNhomQuyen = "STAFF", TenNhomQuyen = "Tên cũ" };
            await repo.AddAsync(role);
            await repo.SaveChangesAsync();

            // Act
            role.TenNhomQuyen = "Nhân viên mới";
            repo.Update(role);
            await repo.SaveChangesAsync();

            // Assert
            var updated = await repo.GetByIdAsync("STAFF");
            updated!.TenNhomQuyen.Should().Be("Nhân viên mới");
        }

        [TestMethod]
        public async Task Delete_ShouldRemoveEntityFromDatabase()
        {
            // Arrange
            using var context = CreateContext();
            var repo = new GenericRepository<ChucNang>(context);
            var func = new ChucNang { MaChucNang = "UC99", TenChucNang = "Xóa thử", MaForm = "F99" };
            await repo.AddAsync(func);
            await repo.SaveChangesAsync();

            // Act
            repo.Delete(func);
            await repo.SaveChangesAsync();

            // Assert
            var deleted = await repo.GetByIdAsync("UC99");
            deleted.Should().BeNull();
        }

        [TestMethod]
        public async Task UnitOfWork_ShouldManageRepositoriesAndSave()
        {
            // Arrange
            using var context = CreateContext();
            using var uow = new UnitOfWork(context);

            var userRepo = uow.Repository<NguoiDung>();
            var roleRepo = uow.Repository<NhomQuyen>();

            var role = new NhomQuyen { MaNhomQuyen = "ADMIN", TenNhomQuyen = "Quản trị" };
            var user = new NguoiDung
            {
                MaNguoiDung = "U01",
                TenDangNhap = "superadmin",
                MatKhauHash = "hash123",
                HoTen = "Super Admin",
                Email = "admin@root.vn",
                MaNhomQuyen = "ADMIN"
            };

            // Act
            await roleRepo.AddAsync(role);
            await userRepo.AddAsync(user);
            int rows = await uow.SaveChangesAsync();

            // Assert
            rows.Should().Be(2);
            var found = await userRepo.GetByIdAsync("U01");
            found.Should().NotBeNull();
            found!.TenDangNhap.Should().Be("superadmin");
        }
    }
}
