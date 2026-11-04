using System.Collections.Generic;
using EventManagement.Core.Session;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử cơ chế quản lý phiên làm việc CurrentSession (Thành viên 1)
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class CurrentSessionTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            CurrentSession.DangXuat();
        }

        [TestMethod]
        public void DangNhap_ShouldSetSessionDataCorrectly()
        {
            // Arrange
            var permissions = new List<string> { "UC01", "UC02", "UC10" };

            // Act
            CurrentSession.DangNhap("USR001", "staff01", "Nguyễn Văn A", "staff@event.vn", "STAFF", permissions);

            // Assert
            CurrentSession.IsLoggedIn.Should().BeTrue();
            CurrentSession.MaNguoiDung.Should().Be("USR001");
            CurrentSession.TenDangNhap.Should().Be("staff01");
            CurrentSession.HoTen.Should().Be("Nguyễn Văn A");
            CurrentSession.Email.Should().Be("staff@event.vn");
            CurrentSession.MaNhomQuyen.Should().Be("STAFF");
            CurrentSession.ThoiDiemDangNhap.Should().NotBeNull();
            CurrentSession.CoQuyen("UC01").Should().BeTrue();
            CurrentSession.CoQuyen("UC10").Should().BeTrue();
            CurrentSession.CoQuyen("UC03").Should().BeFalse();
        }

        [TestMethod]
        public void DangXuat_ShouldClearAllSessionData()
        {
            // Arrange
            CurrentSession.DangNhap("USR001", "staff01", "Nguyễn Văn A", "staff@event.vn", "STAFF");

            // Act
            CurrentSession.DangXuat();

            // Assert
            CurrentSession.IsLoggedIn.Should().BeFalse();
            CurrentSession.MaNguoiDung.Should().BeNull();
            CurrentSession.TenDangNhap.Should().BeNull();
            CurrentSession.DanhSachQuyen.Should().BeEmpty();
        }

        [TestMethod]
        public void CoQuyen_AdminRole_ShouldAlwaysHaveAllPermissions()
        {
            // Arrange
            CurrentSession.DangNhap("USR_ADM", "admin", "Quản trị viên", "admin@root.vn", "ADMIN");

            // Act & Assert
            CurrentSession.CoQuyen("UC01").Should().BeTrue();
            CurrentSession.CoQuyen("UC20").Should().BeTrue();
            CurrentSession.CoQuyen("UC_ANYTHING").Should().BeTrue();
        }
    }
}
