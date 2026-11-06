using System;
using System.IO;
using EventManagement.DAL;
using EventManagement.GUI.Common;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class InfrastructureAndErrorHandlingTests
    {
        [TestMethod]
        [Description("Kiểm tra DatabaseConfigHelper trả về chuỗi kết nối dự phòng (Fallback) khi cấu hình trống hoặc chạy môi trường thử nghiệm.")]
        public void GetConnectionString_WhenNotConfigured_ReturnsValidFallbackString()
        {
            // Act
            var connStr = DatabaseConfigHelper.GetConnectionString("NonExistentKey_XYZ");

            // Assert
            connStr.Should().NotBeNullOrWhiteSpace();
            connStr.Should().Contain("EventManagementDb");
            connStr.Should().Contain("Server=");
        }
        // Mô tả: Test này đảm bảo cơ chế AD-6 không bao giờ trả về null/rỗng ngay cả khi file App.config bị thiếu khóa chuỗi kết nối, giúp ứng dụng không bị crash khi nạp DbContext.

        [TestMethod]
        [Description("Kiểm tra xây dựng chuỗi kết nối SQL Server bằng Windows Authentication (IntegratedSecurity = true).")]
        public void BuildConnectionString_WithWindowsAuth_SetsIntegratedSecurityTrue()
        {
            // Act
            var connStr = DatabaseConfigHelper.BuildConnectionString(
                server: "localhost",
                database: "TestEventDb",
                useWindowsAuth: true);

            // Assert
            connStr.Should().Contain("Data Source=localhost");
            connStr.Should().Contain("Initial Catalog=TestEventDb");
            connStr.Should().Contain("Integrated Security=True");
        }
        // Mô tả: Test này thẩm định tính đúng đắn khi quản trị viên chọn chế độ Windows Authentication trong hộp thoại FrmServerConfig, chuỗi kết nối sẽ bật cờ Integrated Security.

        [TestMethod]
        [Description("Kiểm tra xây dựng chuỗi kết nối SQL Server bằng tài khoản SQL Server (sa / mật khẩu).")]
        public void BuildConnectionString_WithSqlAuth_SetsUserAndPassword()
        {
            // Act
            var connStr = DatabaseConfigHelper.BuildConnectionString(
                server: "192.168.1.100",
                database: "EventDb",
                useWindowsAuth: false,
                username: "sa",
                password: "SecurePassword123!");

            // Assert
            connStr.Should().Contain("Data Source=192.168.1.100");
            connStr.Should().Contain("Initial Catalog=EventDb");
            connStr.Should().Contain("User ID=sa");
            connStr.Should().Contain("Password=SecurePassword123!");
            connStr.Should().NotContain("Integrated Security=True");
        }
        // Mô tả: Test này thẩm định khả năng cấu hình máy chủ từ xa dùng SQL Authentication (phù hợp khi máy chấm thi dùng SQL Server với user sa).

        [TestMethod]
        [Description("Kiểm tra xây dựng chuỗi kết nối với tên server rỗng sẽ ném ArgumentException hợp lệ.")]
        public void BuildConnectionString_WithEmptyServer_ThrowsArgumentException()
        {
            // Act
            Action act = () => DatabaseConfigHelper.BuildConnectionString(
                server: "   ",
                database: "EventDb",
                useWindowsAuth: true);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Tên máy chủ*");
        }
        // Mô tả: Test này ngăn ngừa việc nhập máy chủ rỗng gây lỗi ngoại lệ không mong muốn ở tầng dữ liệu.

        [TestMethod]
        [Description("Kiểm tra TestConnection với chuỗi rỗng sẽ trả về kết quả thất bại an toàn, không ném exception văng ứng dụng.")]
        public void TestConnection_WithEmptyConnectionString_ReturnsFailureGracefully()
        {
            // Act
            var (success, message) = DatabaseConfigHelper.TestConnection(string.Empty, timeoutSeconds: 1);

            // Assert
            success.Should().BeFalse();
            message.Should().Contain("rỗng");
        }
        // Mô tả: Thẩm định khả năng kiểm soát lỗi kết nối (AD-6) không làm sập ứng dụng khi người dùng test chuỗi kết nối không hợp lệ.

        [TestMethod]
        [Description("Kiểm tra GlobalExceptionHandler ghi log chi tiết lỗi ra file Error.log và tạo thư mục tự động.")]
        public void LogException_WritesExceptionDetailsAndStackTraceToFile()
        {
            // Arrange
            var testLogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestLogs");
            var testLogFile = Path.Combine(testLogDir, $"TestError_{Guid.NewGuid():N}.log");

            try
            {
                var sampleException = new InvalidOperationException("Mô phỏng sự cố hệ thống nghiêm trọng tại tầng UI");

                // Act
                GlobalExceptionHandler.LogException(sampleException, testLogFile);

                // Assert
                File.Exists(testLogFile).Should().BeTrue();
                var content = File.ReadAllText(testLogFile);
                content.Should().Contain("InvalidOperationException");
                content.Should().Contain("Mô phỏng sự cố hệ thống nghiêm trọng");
                content.Should().Contain("[THỜI GIAN]:");
            }
            finally
            {
                if (File.Exists(testLogFile))
                {
                    File.Delete(testLogFile);
                }
            }
        }
        // Mô tả: Test này bảo đảm tầng xử lý lỗi cấp 3 hoạt động đúng đặc tả Mục 9 của tài liệu Kiến trúc hệ thống: tự động ghi nhận nhật ký lỗi vào file văn bản mà không để sót thông tin chẩn đoán.

        [TestMethod]
        [Description("Kiểm tra GlobalExceptionHandler ghi nhận đầy đủ cả thông tin InnerException.")]
        public void LogException_WithInnerException_WritesInnerExceptionDetails()
        {
            // Arrange
            var testLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Inner_{Guid.NewGuid():N}.log");

            try
            {
                var innerEx = new TimeoutException("Hết thời gian chờ kết nối máy chủ CSDL");
                var outerEx = new ApplicationException("Lỗi xử lý nghiệp vụ bán vé", innerEx);

                // Act
                GlobalExceptionHandler.LogException(outerEx, testLogFile);

                // Assert
                File.Exists(testLogFile).Should().BeTrue();
                var content = File.ReadAllText(testLogFile);
                content.Should().Contain("Lỗi xử lý nghiệp vụ bán vé");
                content.Should().Contain("--- INNER EXCEPTION ---");
                content.Should().Contain("Hết thời gian chờ kết nối");
            }
            finally
            {
                if (File.Exists(testLogFile))
                {
                    File.Delete(testLogFile);
                }
            }
        }
        // Mô tả: Test này kiểm tra việc bóc tách chuỗi ngoại lệ lồng nhau (InnerException), đặc biệt quan trọng khi EntityFramework hoặc SqlClient bọc lỗi tầng sâu.

        [TestMethod]
        [Description("Kiểm tra khởi tạo form FrmServerConfig và phương thức GetConstructedConnectionString.")]
        public void FrmServerConfig_InitializesWithDefaultValuesAndConstructsString()
        {
            // Arrange & Act
            using var form = new FrmServerConfig();
            var connStr = form.GetConstructedConnectionString();

            // Assert
            form.Text.Should().Contain("Cấu hình Máy chủ");
            connStr.Should().NotBeNullOrWhiteSpace();
            connStr.Should().Contain("EventManagementDb");
        }
        // Mô tả: Đảm bảo giao diện cấu hình server AD-6 khởi tạo đúng chuẩn kế thừa FrmBase, nạp giá trị mặc định sẵn sàng cho người dùng thao tác.
    }
}
