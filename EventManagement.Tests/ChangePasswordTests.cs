using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử thẩm định dữ liệu cho Dialog UC21 Đổi mật khẩu tài khoản (FrmChangePassword)
    /// </summary>
    [TestClass]
    public class ChangePasswordTests
    {
        [TestMethod]
        public void ValidateInputs_WithEmptyCurrentPassword_ReturnsErrorOnCurrentPassword()
        {
            // Act
            var result = FrmChangePassword.ValidateInputs("", "NewPass@123", "NewPass@123");

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtCurrentPassword");
            result.ErrorMessage.Should().Contain("nhập mật khẩu hiện tại");
        }

        [TestMethod]
        public void ValidateInputs_WithEmptyNewPassword_ReturnsErrorOnNewPassword()
        {
            // Act
            var result = FrmChangePassword.ValidateInputs("CurrentPass@123", "", "");

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtNewPassword");
            result.ErrorMessage.Should().Contain("nhập mật khẩu mới");
        }

        [TestMethod]
        public void ValidateInputs_WithShortNewPassword_ReturnsErrorMinimum6Chars()
        {
            // Act: Mật khẩu mới dưới 6 ký tự
            var result = FrmChangePassword.ValidateInputs("CurrentPass@123", "12345", "12345");

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtNewPassword");
            result.ErrorMessage.Should().Contain("ít nhất 6 ký tự");
        }

        [TestMethod]
        public void ValidateInputs_WithMismatchedConfirmPassword_ReturnsErrorMismatched()
        {
            // Act: Mật khẩu mới và xác nhận mật khẩu không khớp
            var result = FrmChangePassword.ValidateInputs("CurrentPass@123", "NewPass@123", "DifferentPass@456");

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtConfirmPassword");
            result.ErrorMessage.Should().Contain("không khớp");
        }

        [TestMethod]
        public void ValidateInputs_WithSameAsCurrentPassword_ReturnsErrorSamePassword()
        {
            // Act: Mật khẩu mới trùng với mật khẩu hiện tại
            var result = FrmChangePassword.ValidateInputs("OldPass@123", "OldPass@123", "OldPass@123");

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtNewPassword");
            result.ErrorMessage.Should().Contain("không được trùng");
        }

        [TestMethod]
        public void ValidateInputs_WithValidData_ReturnsSuccess()
        {
            // Act: Dữ liệu hợp lệ
            var result = FrmChangePassword.ValidateInputs("OldPass@123", "NewPass@456", "NewPass@456");

            // Assert
            result.IsValid.Should().BeTrue();
            result.ErrorField.Should().BeNull();
            result.ErrorMessage.Should().BeNull();
        }
    }
}