using EventManagement.Core.Security;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử thuật toán băm và xác thực mật khẩu BCrypt.Net-Next (Thành viên 1)
    /// </summary>
    [TestClass]
    public class PasswordHasherTests
    {
        private readonly IPasswordHasher _hasher;

        public PasswordHasherTests()
        {
            _hasher = new PasswordHasher();
        }

        [TestMethod]
        public void Hash_Password_ReturnsValidBcryptString()
        {
            // Arrange
            string rawPassword = "Admin@123";

            // Act
            string hash = _hasher.HashPassword(rawPassword);

            // Assert
            hash.Should().NotBeNullOrWhiteSpace();
            hash.Length.Should().Be(60);
            hash.Should().MatchRegex(@"^\$2[ab]\$11\$[A-Za-z0-9./]{53}$");
        }

        [TestMethod]
        public void Verify_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string rawPassword = "Admin@123";
            string hash = _hasher.HashPassword(rawPassword);

            // Act
            bool isValid = _hasher.VerifyPassword(rawPassword, hash);

            // Assert
            isValid.Should().BeTrue();
        }

        [TestMethod]
        public void Verify_WrongPassword_ReturnsFalse()
        {
            // Arrange
            string rawPassword = "Admin@123";
            string wrongPassword = "WrongPassword@999";
            string hash = _hasher.HashPassword(rawPassword);

            // Act
            bool isValid = _hasher.VerifyPassword(wrongPassword, hash);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}
