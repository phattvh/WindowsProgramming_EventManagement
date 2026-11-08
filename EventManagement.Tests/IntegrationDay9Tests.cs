using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Bộ kiểm thử tích hợp hệ thống Ngày 9 (Integration Tests Phase 3).
    /// </summary>
    [TestClass]
    public class IntegrationDay9Tests
    {
        // =========================================================================
        // PHẦN 1: KIỂM THỬ TASK T3-25 (Script SQL Dữ Liệu Mẫu Bảo Vệ 04_FinalDemoData.sql)
        // =========================================================================

        [TestMethod]
        public void DemoScript_FinalDemoDataFile_ExistsAndNonEmpty()
        {
            // Mô tả: Kiểm tra file script 04_FinalDemoData.sql tồn tại trong thư mục gốc hoặc thư mục scripts và có kích thước lớn hơn 1000 bytes.
            string rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\04_FinalDemoData.sql");
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\scripts\04_FinalDemoData.sql");

            bool exists = File.Exists(rootPath) || File.Exists(scriptPath);
            Assert.IsTrue(exists, "File 04_FinalDemoData.sql phải tồn tại để nạp CSDL bảo vệ đồ án.");

            string activePath = File.Exists(rootPath) ? rootPath : scriptPath;
            var fileInfo = new FileInfo(activePath);
            Assert.IsTrue(fileInfo.Length > 1000, "Script dữ liệu mẫu phải có nội dung đầy đủ cho 20 bảng.");
        }

        [TestMethod]
        public void DemoScript_ContainsRequiredAccounts_AndScenarioKeys()
        {
            // Mô tả: Kiểm tra script 04_FinalDemoData.sql chứa đầy đủ 4 tài khoản (admin, manager1, staff1, locked_user), mã QR mẫu GUID và nhãn cảnh báo vượt ngân sách BR-14.
            string rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\04_FinalDemoData.sql");
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\scripts\04_FinalDemoData.sql");
            string activePath = File.Exists(rootPath) ? rootPath : scriptPath;

            string sqlContent = File.ReadAllText(activePath);

            Assert.IsTrue(sqlContent.Contains("'admin'"), "Script phải chứa tài khoản quản trị 'admin'.");
            Assert.IsTrue(sqlContent.Contains("'manager1'"), "Script phải chứa tài khoản quản lý 'manager1'.");
            Assert.IsTrue(sqlContent.Contains("'staff1'"), "Script phải chứa tài khoản nhân viên 'staff1'.");
            Assert.IsTrue(sqlContent.Contains("'locked_user'"), "Script phải chứa tài khoản bị khóa 'locked_user' để kiểm thử BR-01.");
            Assert.IsTrue(sqlContent.Contains("11111111-2222-3333-4444-555555555502"), "Script phải chứa mã QR GUID cố định để quét webcam live.");
            Assert.IsTrue(sqlContent.Contains("BR-14"), "Script phải có kịch bản chi vượt 80% ngân sách để kiểm tra đổi màu cam.");
        }
    }
}
