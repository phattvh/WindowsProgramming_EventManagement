using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;
using EventManagement.DAL;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử nghiệp vụ RolePermissionService và Form F20 Quản lý nhóm quyền & Ma trận phân quyền động (Task T1-14 - Thành viên 1)
    /// </summary>
    [TestClass]
    public class RolePermissionTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            // Seed Roles
            context.NhomQuyens.AddRange(
                new NhomQuyen { MaNhomQuyen = "ADMIN", TenNhomQuyen = "Quản trị viên", MoTa = "Toàn quyền hệ thống" },
                new NhomQuyen { MaNhomQuyen = "MANAGER", TenNhomQuyen = "Quản lý sự kiện", MoTa = "Quản lý nội dung" },
                new NhomQuyen { MaNhomQuyen = "STAFF", TenNhomQuyen = "Nhân viên hỗ trợ", MoTa = "Tác nghiệp bán vé" }
            );

            // Seed Functions (UC01 - UC04)
            context.ChucNangs.AddRange(
                new ChucNang { MaChucNang = "UC01", TenChucNang = "Đăng nhập hệ thống", MaForm = "F01" },
                new ChucNang { MaChucNang = "UC02", TenChucNang = "Xem Dashboard", MaForm = "F02" },
                new ChucNang { MaChucNang = "UC03", TenChucNang = "Quản lý người dùng", MaForm = "F03" },
                new ChucNang { MaChucNang = "UC20", TenChucNang = "Quản lý nhóm quyền & phân quyền", MaForm = "F20" }
            );

            // Seed initial matrix
            context.PhanQuyens.AddRange(
                new PhanQuyen { MaNhomQuyen = "ADMIN", MaChucNang = "UC01", CoQuyen = true },
                new PhanQuyen { MaNhomQuyen = "ADMIN", MaChucNang = "UC02", CoQuyen = true },
                new PhanQuyen { MaNhomQuyen = "ADMIN", MaChucNang = "UC03", CoQuyen = true },
                new PhanQuyen { MaNhomQuyen = "ADMIN", MaChucNang = "UC20", CoQuyen = true },
                new PhanQuyen { MaNhomQuyen = "STAFF", MaChucNang = "UC01", CoQuyen = true },
                new PhanQuyen { MaNhomQuyen = "STAFF", MaChucNang = "UC02", CoQuyen = false },
                new PhanQuyen { MaNhomQuyen = "STAFF", MaChucNang = "UC03", CoQuyen = false },
                new PhanQuyen { MaNhomQuyen = "STAFF", MaChucNang = "UC20", CoQuyen = false }
            );

            context.SaveChanges();
            return context;
        }

        [TestMethod]
        public async Task GetAllRolesAsync_ReturnsSystemRoles()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            // Act
            var roles = await service.GetAllRolesAsync();

            // Assert
            roles.Should().HaveCount(3);
            roles.Select(r => r.MaNhomQuyen).Should().Contain(new[] { "ADMIN", "MANAGER", "STAFF" });
        }

        [TestMethod]
        public async Task GetAllFunctionsAsync_ReturnsAllSystemFunctions()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            // Act
            var funcs = await service.GetAllFunctionsAsync();

            // Assert
            funcs.Should().HaveCount(4);
            funcs.Select(f => f.MaChucNang).Should().Contain(new[] { "UC01", "UC02", "UC03", "UC20" });
        }

        [TestMethod]
        public async Task GetPermissionMatrixAsync_ForAdmin_DefaultsToAllTrue()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            // Act
            var matrix = await service.GetPermissionMatrixAsync("ADMIN");

            // Assert
            matrix.Should().HaveCount(4);
            matrix.All(m => m.CoQuyen).Should().BeTrue();
        }

        [TestMethod]
        public async Task GetPermissionMatrixAsync_ForStaff_ReturnsConfiguredPermissions()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            // Act
            var matrix = await service.GetPermissionMatrixAsync("STAFF");

            // Assert
            matrix.Should().HaveCount(4);
            var uc01 = matrix.First(m => m.MaChucNang == "UC01");
            var uc03 = matrix.First(m => m.MaChucNang == "UC03");

            uc01.CoQuyen.Should().BeTrue();
            uc03.CoQuyen.Should().BeFalse();
        }

        [TestMethod]
        public async Task SavePermissionMatrixAsync_ForStaff_UpdatesPermissionsSuccessfully()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            var matrix = await service.GetPermissionMatrixAsync("STAFF");
            // Cấp thêm quyền UC02 (Dashboard) cho STAFF
            matrix.First(m => m.MaChucNang == "UC02").CoQuyen = true;

            // Act
            var result = await service.SavePermissionMatrixAsync("STAFF", matrix);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var updatedPq = await context.PhanQuyens.FirstAsync(p => p.MaNhomQuyen == "STAFF" && p.MaChucNang == "UC02");
            updatedPq.CoQuyen.Should().BeTrue();
        }

        [TestMethod]
        public async Task SavePermissionMatrixAsync_ForAdmin_WhenRevokingPermission_ReturnsFailed()
        {
            // Arrange: Cố tình bỏ quyền của ADMIN
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            var matrix = await service.GetPermissionMatrixAsync("ADMIN");
            matrix.First(m => m.MaChucNang == "UC20").CoQuyen = false; // Cố tình bỏ quyền F20 của ADMIN

            // Act
            var result = await service.SavePermissionMatrixAsync("ADMIN", matrix);

            // Assert: Hệ thống phải từ chối
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("ADMIN");
            result.Message.Should().Contain("thu hồi");
        }

        [TestMethod]
        public async Task CreateRoleAsync_WithValidData_CreatesRoleAndSeedsDefaultPermissions()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            var dto = new CreateRoleDto
            {
                MaNhomQuyen = "ACCOUNTANT",
                TenNhomQuyen = "Kế toán viên",
                MoTa = "Quản lý ngân sách và hóa đơn"
            };

            // Act
            var result = await service.CreateRoleAsync(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var inDb = await context.NhomQuyens.FindAsync("ACCOUNTANT");
            inDb.Should().NotBeNull();
            inDb!.TenNhomQuyen.Should().Be("Kế toán viên");

            // Phải tự động tạo đủ 4 dòng PhanQuyen cho vai trò mới
            var pqs = await context.PhanQuyens.Where(p => p.MaNhomQuyen == "ACCOUNTANT").ToListAsync();
            pqs.Should().HaveCount(4);
            pqs.All(p => !p.CoQuyen).Should().BeTrue(); // Mặc định false
        }

        [TestMethod]
        public async Task CreateRoleAsync_WithDuplicateCode_ReturnsFailed()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            var dto = new CreateRoleDto
            {
                MaNhomQuyen = "STAFF", // Đã tồn tại
                TenNhomQuyen = "Nhân viên trùng lặp"
            };

            // Act
            var result = await service.CreateRoleAsync(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("đã tồn tại");
        }

        [TestMethod]
        public async Task CreateRoleAsync_WithInvalidCodeFormat_ReturnsFailed()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            var dto = new CreateRoleDto
            {
                MaNhomQuyen = "a b-c!", // Ký tự đặc biệt, khoảng trắng
                TenNhomQuyen = "Role không hợp lệ"
            };

            // Act
            var result = await service.CreateRoleAsync(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("từ 3 đến 20 ký tự");
        }

        [TestMethod]
        public async Task DeleteRoleAsync_ForSystemRoles_ReturnsFailed()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            // Act: Cố xóa vai trò ADMIN
            var result = await service.DeleteRoleAsync("ADMIN");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("mặc định của hệ thống");
        }

        [TestMethod]
        public async Task DeleteRoleAsync_WhenRoleHasUsers_ReturnsFailed()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            // Tạo custom role
            context.NhomQuyens.Add(new NhomQuyen { MaNhomQuyen = "VOLUNTEER", TenNhomQuyen = "Tình nguyện viên" });
            // Gán 1 user vào role này
            context.NguoiDungs.Add(new NguoiDung
            {
                MaNguoiDung = "U_VOL",
                TenDangNhap = "vol01",
                MatKhauHash = "hash",
                HoTen = "Tran Tinh Nguyen",
                Email = "vol@event.vn",
                MaNhomQuyen = "VOLUNTEER"
            });
            await context.SaveChangesAsync();

            // Act: Cố xóa VOLUNTEER khi đang có người dùng
            var result = await service.DeleteRoleAsync("VOLUNTEER");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("người dùng");
        }

        [TestMethod]
        public async Task DeleteRoleAsync_CustomRoleWithoutUsers_DeletesSuccessfully()
        {
            // Arrange
            using var context = CreateContext();
            var service = new RolePermissionService(context);

            context.NhomQuyens.Add(new NhomQuyen { MaNhomQuyen = "SECURITY", TenNhomQuyen = "An ninh" });
            context.PhanQuyens.Add(new PhanQuyen { MaNhomQuyen = "SECURITY", MaChucNang = "UC01", CoQuyen = true });
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteRoleAsync("SECURITY");

            // Assert
            result.IsSuccess.Should().BeTrue();
            var inDb = await context.NhomQuyens.FindAsync("SECURITY");
            inDb.Should().BeNull();
            var pqs = await context.PhanQuyens.Where(p => p.MaNhomQuyen == "SECURITY").ToListAsync();
            pqs.Should().BeEmpty();
        }

        [TestMethod]
        public void ValidateRoleInput_EmptyCode_ReturnsError()
        {
            var result = RolePermissionService.ValidateRoleInput("", "Tên vai trò");
            result.IsValid.Should().BeFalse();
            result.ErrorMessage.Should().Contain("không được để trống");
        }

        [TestMethod]
        public void ValidateRoleInput_InvalidCodeFormat_ReturnsError()
        {
            var result = RolePermissionService.ValidateRoleInput("a", "Tên vai trò");
            result.IsValid.Should().BeFalse();
            result.ErrorMessage.Should().Contain("3 đến 20");
        }

        [TestMethod]
        public void ValidateRoleInput_EmptyName_ReturnsError()
        {
            var result = RolePermissionService.ValidateRoleInput("AUDITOR", "");
            result.IsValid.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Tên nhóm quyền không được để trống");
        }

        [TestMethod]
        public void ValidateRoleInput_ValidData_ReturnsSuccess()
        {
            var result = RolePermissionService.ValidateRoleInput("AUDITOR", "Kiểm toán viên");
            result.IsValid.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
        }
    }
}

