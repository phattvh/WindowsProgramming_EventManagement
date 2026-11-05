using EventManagement.Core.DTOs;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử thẩm định dữ liệu và quy tắc nghiệp vụ Form F03 Quản lý tài khoản (Task T1-12 & T1-13)
    /// </summary>
    [TestClass]
    public class UserManagementFormTests
    {
        [TestMethod]
        public void ValidateUserInput_EmptyUsername_ReturnsErrorOnUsername()
        {
            // Act
            var result = FrmUserManagement.ValidateUserInput("", "Nguyen Van A", "a@event.vn", "0901234567", "Pass@123", isNewUser: true);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtTenDangNhap");
            result.ErrorMessage.Should().Contain("không được để trống");
        }

        [TestMethod]
        public void ValidateUserInput_ShortUsername_ReturnsErrorOnUsername()
        {
            // Act: Tên đăng nhập dưới 3 ký tự
            var result = FrmUserManagement.ValidateUserInput("ab", "Nguyen Van A", "a@event.vn", "0901234567", "Pass@123", isNewUser: true);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtTenDangNhap");
            result.ErrorMessage.Should().Contain("ít nhất 3 ký tự");
        }

        [TestMethod]
        public void ValidateUserInput_EmptyFullName_ReturnsErrorOnFullName()
        {
            // Act
            var result = FrmUserManagement.ValidateUserInput("admin01", "", "a@event.vn", "0901234567", "Pass@123", isNewUser: true);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtHoTen");
            result.ErrorMessage.Should().Contain("không được để trống");
        }

        [TestMethod]
        public void ValidateUserInput_InvalidEmail_ReturnsErrorOnEmail()
        {
            // Act: Email không có @ hoặc domain
            var result = FrmUserManagement.ValidateUserInput("admin01", "Nguyen Van A", "invalid-email-format", "0901234567", "Pass@123", isNewUser: true);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtEmail");
            result.ErrorMessage.Should().Contain("email không hợp lệ");
        }

        [TestMethod]
        public void ValidateUserInput_InvalidPhone_ReturnsErrorOnPhone()
        {
            // Act: Số điện thoại chứa chữ hoặc sai độ dài
            var result = FrmUserManagement.ValidateUserInput("admin01", "Nguyen Van A", "a@event.vn", "12345abc", "Pass@123", isNewUser: true);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtSoDienThoai");
            result.ErrorMessage.Should().Contain("Số điện thoại không hợp lệ");
        }

        [TestMethod]
        public void ValidateUserInput_EmptyPasswordWhenNewUser_ReturnsErrorOnPassword()
        {
            // Act: Tạo mới nhưng không nhập mật khẩu
            var result = FrmUserManagement.ValidateUserInput("admin01", "Nguyen Van A", "a@event.vn", "0901234567", "", isNewUser: true);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtMatKhau");
            result.ErrorMessage.Should().Contain("Mật khẩu không được để trống");
        }

        [TestMethod]
        public void ValidateUserInput_ShortPasswordWhenNewUser_ReturnsErrorOnPassword()
        {
            // Act: Mật khẩu dưới 6 ký tự
            var result = FrmUserManagement.ValidateUserInput("admin01", "Nguyen Van A", "a@event.vn", "0901234567", "12345", isNewUser: true);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtMatKhau");
            result.ErrorMessage.Should().Contain("tối thiểu 6 ký tự");
        }

        [TestMethod]
        public void ValidateUserInput_ValidNewUserData_ReturnsSuccess()
        {
            // Act: Dữ liệu thêm mới hợp lệ
            var result = FrmUserManagement.ValidateUserInput("manager01", "Tran Thi Quan Ly", "manager@event.vn", "0912345678", "Secret@123", isNewUser: true);

            // Assert
            result.IsValid.Should().BeTrue();
            result.ErrorField.Should().BeNull();
            result.ErrorMessage.Should().BeNull();
        }

        [TestMethod]
        public void ValidateUserInput_ValidEditDataWithoutPasswordChange_ReturnsSuccess()
        {
            // Act: Chế độ Sửa (isNewUser = false) và để trống mật khẩu (không đổi)
            var result = FrmUserManagement.ValidateUserInput("manager01", "Tran Thi Quan Ly Updated", "manager_updated@event.vn", "0912345678", "", isNewUser: false);

            // Assert
            result.IsValid.Should().BeTrue();
            result.ErrorField.Should().BeNull();
            result.ErrorMessage.Should().BeNull();
        }

        [TestMethod]
        public void UserDto_IsLockedProperty_DetectsLockedStatusCorrectly()
        {
            // Case 1: Active
            var activeUser = new UserDto { TrangThai = "Hoạt động", KhoaDenKhi = null };
            activeUser.IsLocked.Should().BeFalse();

            // Case 2: Permanent Lock
            var permLockedUser = new UserDto { TrangThai = "Khóa vĩnh viễn" };
            permLockedUser.IsLocked.Should().BeTrue();

            // Case 3: Temporary Lock until future
            var tempLockedUser = new UserDto { TrangThai = "Khóa tạm thời", KhoaDenKhi = DateTime.Now.AddMinutes(5) };
            tempLockedUser.IsLocked.Should().BeTrue();

            // Case 4: Lock expired in the past
            var expiredLockUser = new UserDto { TrangThai = "Hoạt động", KhoaDenKhi = DateTime.Now.AddMinutes(-5) };
            expiredLockUser.IsLocked.Should().BeFalse();
        }
    }
}