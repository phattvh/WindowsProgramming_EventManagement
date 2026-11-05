using System;

namespace EventManagement.Core.Security
{
    /// <summary>
    /// Tiện ích băm và xác thực mật khẩu chuẩn BCrypt (Cost factor = 11)
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 11;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Mật khẩu không được để trống!", nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WorkFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
}
