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
        public const string FallbackConnectionString = "Server=.;Database=EventManagementDb;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=2;";

        /// <summary>
        /// Danh sách máy chủ SQL Server ứng viên cục bộ phổ biến cho môi trường chạy máy chấm thi / bảo vệ đồ án.
        /// </summary>
        public static readonly string[] DefaultCandidateServers = new[]
        {
            @".\SQLEXPRESS",
            ".",
            @"(localdb)\mssqllocaldb",
            "localhost",
            "127.0.0.1"
        };

        private static string? _cachedEffectiveConnectionString;
        private static readonly object _connLock = new();

        /// <summary>
        /// Xóa cache chuỗi kết nối khi có sự thay đổi cấu hình máy chủ.
        /// </summary>
        public static void InvalidateCache()
        {
            lock (_connLock)
            {
                _cachedEffectiveConnectionString = null;
            }
        }

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
            InvalidateCache();
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
        /// Tự động dò tìm một chuỗi kết nối SQL Server đang hoạt động trong danh sách ứng viên (T1-19).
        /// Sử dụng non-blocking timeout để không làm treo giao diện.
        /// </summary>
        public static (bool Found, string ConnectionString, string ServerName) FindWorkingConnectionString(int timeoutSecondsPerServer = 1)
        {
            // 1. Thử chuỗi kết nối từ App.config trước tiên
            var configuredConn = GetConnectionString();
            if (!string.IsNullOrWhiteSpace(configuredConn))
            {
                var configuredTest = TestConnection(configuredConn, timeoutSecondsPerServer);
                if (configuredTest.Success)
                {
                    try
                    {
                        var b = new SqlConnectionStringBuilder(configuredConn);
                        return (true, configuredConn, b.DataSource);
                    }
                    catch
                    {
                        return (true, configuredConn, "ConfiguredServer");
                    }
                }
            }

            // 2. Thử lần lượt các máy chủ cục bộ ứng viên
            foreach (var server in DefaultCandidateServers)
            {
                var candidateConn = BuildConnectionString(server, "EventManagementDb", true, timeoutSeconds: timeoutSecondsPerServer);
                var test = TestConnection(candidateConn, timeoutSecondsPerServer);
                if (test.Success)
                {
                    return (true, candidateConn, server);
                }
            }

            // 3. Fallback an toàn về chuỗi mặc định, không bao giờ ném Exception làm sập app (Fail-Safe)
            return (false, FallbackConnectionString, "FallbackServer");
        }

        /// <summary>
        /// Lấy chuỗi kết nối hiệu dụng tối ưu: ưu tiên kết nối sống hoặc fallback an toàn (T1-19).
        /// </summary>
        public static string GetEffectiveConnectionString()
        {
            if (!string.IsNullOrWhiteSpace(_cachedEffectiveConnectionString))
            {
                return _cachedEffectiveConnectionString;
            }

            lock (_connLock)
            {
                if (!string.IsNullOrWhiteSpace(_cachedEffectiveConnectionString))
                {
                    return _cachedEffectiveConnectionString;
                }

                var result = FindWorkingConnectionString(1);
                _cachedEffectiveConnectionString = !string.IsNullOrWhiteSpace(result.ConnectionString)
                    ? result.ConnectionString
                    : FallbackConnectionString;

                return _cachedEffectiveConnectionString;
            }
        }

        /// <summary>
        /// Tạo DbContextOptionsBuilder cho AppDbContext sử dụng chuỗi kết nối hiện hành.
        /// </summary>
        public static DbContextOptions<AppDbContext> CreateDefaultOptions(string? connectionString = null)
        {
            var connStr = string.IsNullOrWhiteSpace(connectionString) ? GetEffectiveConnectionString() : connectionString;
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connStr)
                .Options;
        }
    }
}
