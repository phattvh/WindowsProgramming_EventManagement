using EventManagement.Core.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử các thực thể Phân quyền & Quản trị hệ thống (Thành viên 1)
    /// </summary>
    [TestClass]
    public class AuthorizationEntitiesTests
    {
        [TestMethod]
        public void NhomQuyen_ShouldInitializeCorrectlyWithDefaultCollections()
        {
            // Arrange & Act
            var role = new NhomQuyen
            {
                MaNhomQuyen = "ADMIN",
                TenNhomQuyen = "Quản trị viên",
                MoTa = "Toàn quyền hệ thống"
            };

            // Assert
            role.MaNhomQuyen.Should().Be("ADMIN");
            role.TenNhomQuyen.Should().Be("Quản trị viên");
            role.MoTa.Should().Be("Toàn quyền hệ thống");
            role.NguoiDungs.Should().NotBeNull().And.BeEmpty();
            role.PhanQuyens.Should().NotBeNull().And.BeEmpty();
        }

        [TestMethod]
        public void ChucNang_ShouldStoreFormAndCodeCorrectly()
        {
            // Arrange & Act
            var func = new ChucNang
            {
                MaChucNang = "UC01",
                TenChucNang = "Đăng nhập hệ thống",
                MaForm = "F01"
            };

            // Assert
            func.MaChucNang.Should().Be("UC01");
            func.TenChucNang.Should().Be("Đăng nhập hệ thống");
            func.MaForm.Should().Be("F01");
            func.PhanQuyens.Should().NotBeNull().And.BeEmpty();
        }

        [TestMethod]
        public void PhanQuyen_ShouldDefaultCoQuyenToTrue()
        {
            // Arrange & Act
            var permission = new PhanQuyen
            {
                MaNhomQuyen = "ADMIN",
                MaChucNang = "UC01"
            };

            // Assert
            permission.MaNhomQuyen.Should().Be("ADMIN");
            permission.MaChucNang.Should().Be("UC01");
            permission.CoQuyen.Should().BeTrue();
        }

        [TestMethod]
        public void NguoiDung_ShouldAssociateWithNhomQuyen()
        {
            // Arrange
            var role = new NhomQuyen { MaNhomQuyen = "ADMIN", TenNhomQuyen = "Quản trị viên" };
            var user = new NguoiDung
            {
                MaNguoiDung = "USR001",
                TenDangNhap = "admin",
                MatKhauHash = "$2a$11$samplehash",
                HoTen = "Admin User",
                Email = "admin@event.vn",
                MaNhomQuyen = "ADMIN",
                NhomQuyen = role
            };

            // Assert
            user.MaNhomQuyen.Should().Be("ADMIN");
            user.NhomQuyen.Should().NotBeNull();
            user.NhomQuyen!.TenNhomQuyen.Should().Be("Quản trị viên");
            user.TrangThai.Should().Be("Hoạt động");
            user.SoLanDangNhapSai.Should().Be(0);
        }
    }
}
