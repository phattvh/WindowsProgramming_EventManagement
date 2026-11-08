using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử thẩm định và logic đếm ngược màn hình Đăng nhập F01 (FrmLogin)
    /// </summary>
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public void ValidateLoginInputs_WithEmptyUsername_ReturnsErrorOnUsername()
        {
            // Act
            var result = FrmLogin.ValidateLoginInputs("", "ValidPass@123");

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtUsername");
            result.ErrorMessage.Should().Contain("tên đăng nhập");
        }

        [TestMethod]
        public void ValidateLoginInputs_WithEmptyPassword_ReturnsErrorOnPassword()
        {
            // Act
            var result = FrmLogin.ValidateLoginInputs("admin", "");

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtPassword");
            result.ErrorMessage.Should().Contain("mật khẩu");
        }

        [TestMethod]
        public void ValidateLoginInputs_WithWhitespace_ReturnsError()
        {
            // Act
            var result = FrmLogin.ValidateLoginInputs("   ", "   ");

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorField.Should().Be("txtUsername");
        }

        [TestMethod]
        public void ValidateLoginInputs_WithValidCredentials_ReturnsSuccess()
        {
            // Act
            var result = FrmLogin.ValidateLoginInputs("admin", "Admin@123");

            // Assert
            result.IsValid.Should().BeTrue();
            result.ErrorField.Should().BeNull();
            result.ErrorMessage.Should().BeNull();
        }

        [TestMethod]
        public void ExtractLockoutSeconds_FromSecondsMessage_ReturnsCorrectSeconds()
        {
            // Arrange
            string msg = "Tài khoản đang bị khóa tạm thời do nhập sai quá 5 lần. Vui lòng thử lại sau 45 giây!";

            // Act
            int seconds = FrmLogin.ExtractLockoutSeconds(msg);

            // Assert
            seconds.Should().Be(45);
        }

        [TestMethod]
        public void ExtractLockoutSeconds_FromMinutesMessage_ReturnsCorrectSeconds()
        {
            // Arrange
            string msg = "Bạn đã nhập sai mật khẩu 5 lần liên tiếp. Tài khoản bị khóa tạm thời trong 5 phút!";

            // Act
            int seconds = FrmLogin.ExtractLockoutSeconds(msg);

            // Assert
            seconds.Should().Be(300); // 5 phút = 300 giây
        }

        [TestMethod]
        public void ExtractLockoutSeconds_FromNormalMessage_ReturnsZero()
        {
            // Arrange
            string msg = "Tên đăng nhập hoặc mật khẩu không chính xác! Bạn còn 3 lần thử.";

            // Act
            int seconds = FrmLogin.ExtractLockoutSeconds(msg);

            // Assert
            seconds.Should().Be(0);
        }
        [TestMethod]
        public void FrmLogin_LayoutAndDimensions_AreProperlyProportioned_WithoutSubTitle()
        {
            // Mô tả: Kiểm tra Form Đăng nhập F01 không còn hiển thị subtitle 'Hệ Thống Quản Lý Sự Kiện & Hội Nghị', có kích thước dialog cân đối (<= 450x400) và không bị ép full màn hình.
            using var form = new FrmLogin();

            form.Text.Should().Be("Hệ thống quản lý sự kiện", "Thanh tiêu đề Form đăng nhập phải là 'Hệ thống quản lý sự kiện' theo chuẩn hóa");
            form.MaximizeBox.Should().BeFalse("Form Đăng nhập phải ở dạng hộp thoại cố định, không cho phóng to toàn màn hình.");
            form.WindowState.Should().Be(System.Windows.Forms.FormWindowState.Normal, "Form Đăng nhập phải hiển thị ở chế độ Normal giữa màn hình.");
            form.ClientSize.Width.Should().BeInRange(380, 460, "Chiều rộng form đăng nhập phải cân đối.");
            form.ClientSize.Height.Should().BeInRange(320, 390, "Chiều cao form đăng nhập phải gọn gàng, vừa vặn.");

            // Kiểm tra không còn control nào chứa chữ phụ đề dài
            foreach (System.Windows.Forms.Control c in form.Controls)
            {
                c.Text.Should().NotContain("Hệ Thống Quản Lý Sự Kiện & Hội Nghị", "Đã lược bỏ dòng phụ đề dài theo yêu cầu giao diện.");
            }
        }
    }
}
