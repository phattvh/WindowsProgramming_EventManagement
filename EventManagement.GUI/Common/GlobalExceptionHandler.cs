using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EventManagement.GUI.Common
{
    /// <summary>
    /// Bộ bắt lỗi toàn cục (Global Exception Handler) cấp 3 của hệ thống.
    /// Ngăn chặn ứng dụng crash đột ngột, ghi log kỹ thuật vào Logs/Error.log
    /// và hiển thị thông báo lịch sự tới người dùng.
    /// </summary>
    public static class GlobalExceptionHandler
    {
        private static bool _isRegistered = false;
        private static readonly object _logLock = new();

        public static string DefaultLogDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        public static string DefaultLogFilePath => Path.Combine(DefaultLogDirectory, "Error.log");

        /// <summary>
        /// Đăng ký bắt lỗi toàn cục trên luồng UI và mọi tiến trình con.
        /// </summary>
        public static void Register()
        {
            if (_isRegistered) return;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += HandleThreadException;
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

            _isRegistered = true;
        }

        private static void HandleThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex);
            }
            else
            {
                LogText($"Unhandled non-exception object: {e.ExceptionObject}");
            }
        }

        public static void HandleException(Exception ex, bool showDialog = true)
        {
            try
            {
                LogException(ex);
            }
            catch
            {
                // Tránh loop exception khi ghi file log thất bại
            }

            if (showDialog)
            {
                ShowFriendlyError(ex);
            }
        }

        /// <summary>
        /// Ghi chi tiết lỗi kỹ thuật vào tệp log.
        /// </summary>
        public static void LogException(Exception ex, string? customFilePath = null)
        {
            if (ex == null) return;

            var sb = new StringBuilder();
            sb.AppendLine("================================================================================");
            sb.AppendLine($"[THỜI GIAN]: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            sb.AppendLine($"[LOẠI LỖI ]: {ex.GetType().FullName}");
            sb.AppendLine($"[THÔNG BÁO]: {ex.Message}");
            sb.AppendLine($"[CALLSTACK]:");
            sb.AppendLine(ex.StackTrace ?? "Không có StackTrace");

            if (ex.InnerException != null)
            {
                sb.AppendLine($"--- INNER EXCEPTION ---");
                sb.AppendLine($"[INNER TYPE   ]: {ex.InnerException.GetType().FullName}");
                sb.AppendLine($"[INNER MESSAGE]: {ex.InnerException.Message}");
                sb.AppendLine($"[INNER STACK  ]: {ex.InnerException.StackTrace}");
            }
            sb.AppendLine("================================================================================");
            sb.AppendLine();

            LogText(sb.ToString(), customFilePath);
        }

        public static void LogText(string content, string? customFilePath = null)
        {
            try
            {
                var filePath = string.IsNullOrWhiteSpace(customFilePath) ? DefaultLogFilePath : customFilePath;
                var dir = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                lock (_logLock)
                {
                    File.AppendAllText(filePath, content, Encoding.UTF8);
                }
            }
            catch
            {
                // Im lặng nếu không có quyền ghi đĩa
            }
        }

        /// <summary>
        /// Hiển thị thông báo thân thiện cấp 3 tới người dùng.
        /// </summary>
        public static void ShowFriendlyError(Exception ex, string? customMessage = null)
        {
            var displayMessage = customMessage ??
                "Hệ thống gặp sự cố gián đoạn tạm thời hoặc mất kết nối CSDL.\n" +
                "Chi tiết lỗi đã được tự động ghi nhận vào tệp nhật ký Logs/Error.log.\n\n" +
                $"Thông điệp: {ex.Message}\n\n" +
                "Vui lòng thử lại hoặc liên hệ quản trị viên hệ thống để được hỗ trợ.";

            MessageBox.Show(
                displayMessage,
                "Thông Báo Sự Cố Hệ Thống",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
