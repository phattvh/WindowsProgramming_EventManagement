using System;
using System.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.DAL
{
    /// <summary>
    /// Hỗ trợ đọc, ghi động và kiểm tra chuỗi kết nối CSDL trong App.config lúc khởi động (AD-6).
    /// </summary>
    public static class DatabaseConfigHelper
    {
        public const string DefaultConnectionName = "DefaultConnection";
        public const string FallbackConnectionString = "Server=.;Database=EventManagementDb;Trusted_Connection=True;TrustServerCertificate=True;";

        /// <summary>
        /// Lấy chuỗi kết nối từ App.config hoặc chuỗi dự phòng mặc định nếu cấu hình chưa có.
        /// </summary>
        public static string GetConnectionString(string connectionName = DefaultConnectionName)
        {
            try
            {
                var connStr = ConfigurationManager.ConnectionStrings[connectionName]?.ConnectionString;
                if (!string.IsNullOrWhiteSpace(connStr))
                {
                    return connStr;
                }
            }
            catch
            {
                // Fallback khi chạy test in-memory hoặc môi trường không tải được App.config
            }

            return FallbackConnectionString;
        }

        /// <summary>
        /// Ghi đè chuỗi kết nối mới vào file App.config qua ConfigurationManager (AD-6).
        /// </summary>
        public static bool SaveConnectionString(string connectionString, string connectionName = DefaultConnectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Chuỗi kết nối không được để trống.", nameof(connectionString));

            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var setting = config.ConnectionStrings.ConnectionStrings[connectionName];

                if (setting != null)
                {
                    setting.ConnectionString = connectionString;
                }
                else
                {
                    config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(connectionName, connectionString, "Microsoft.Data.SqlClient"));
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Xây dựng chuỗi kết nối chuẩn mực từ các thành phần nhập liệu.
        /// </summary>
        public static string BuildConnectionString(string server, string database = "EventManagementDb", bool useWindowsAuth = true, string? username = null, string? password = null, int timeoutSeconds = 5)
        {
            if (string.IsNullOrWhiteSpace(server))
                throw new ArgumentException("Tên máy chủ (Server) không được để trống.", nameof(server));

            if (string.IsNullOrWhiteSpace(database))
                database = "EventManagementDb";

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server.Trim(),
                InitialCatalog = database.Trim(),
                TrustServerCertificate = true,
                ConnectTimeout = Math.Max(1, timeoutSeconds)
            };

            if (useWindowsAuth)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = username?.Trim() ?? string.Empty;
                builder.Password = password ?? string.Empty;
            }

            return builder.ConnectionString;
        }

        /// <summary>
        /// Kiểm tra kết nối tới máy chủ SQL Server với thời gian chờ chỉ định.
        /// </summary>
        public static (bool Success, string Message) TestConnection(string connectionString, int timeoutSeconds = 3)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return (false, "Chuỗi kết nối rỗng.");

            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString)
                {
                    ConnectTimeout = Math.Max(1, timeoutSeconds)
                };

                using var conn = new SqlConnection(builder.ConnectionString);
                conn.Open();
                return (true, "Kết nối CSDL thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi kết nối CSDL: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo DbContextOptionsBuilder cho AppDbContext sử dụng chuỗi kết nối hiện hành.
        /// </summary>
        public static DbContextOptions<AppDbContext> CreateDefaultOptions(string? connectionString = null)
        {
            var connStr = string.IsNullOrWhiteSpace(connectionString) ? GetConnectionString() : connectionString;
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connStr)
                .Options;
        }
    }
}
