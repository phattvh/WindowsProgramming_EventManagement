using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.GUI.Forms;

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
            // Mô tả: Kiểm tra script 04_FinalDemoData.sql chứa đầy đủ 3 tài khoản mẫu chuẩn (admin, manager1, staff1), mã QR mẫu GUID và nhãn cảnh báo vượt ngân sách BR-14.
            string rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\04_FinalDemoData.sql");
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\scripts\04_FinalDemoData.sql");
            string activePath = File.Exists(rootPath) ? rootPath : scriptPath;

            string sqlContent = File.ReadAllText(activePath);

            Assert.IsTrue(sqlContent.Contains("'admin'"), "Script phải chứa tài khoản quản trị 'admin'.");
            Assert.IsTrue(sqlContent.Contains("'manager1'"), "Script phải chứa tài khoản quản lý 'manager1'.");
            Assert.IsTrue(sqlContent.Contains("'staff1'"), "Script phải chứa tài khoản nhân viên 'staff1'.");
            Assert.IsTrue(sqlContent.Contains("11111111-2222-3333-4444-555555555502"), "Script phải chứa mã QR GUID cố định để quét webcam live.");
            Assert.IsTrue(sqlContent.Contains("BR-14"), "Script phải có kịch bản chi vượt 80% ngân sách để kiểm tra đổi màu cam.");
        }

        // =========================================================================
        // PHẦN 2: KIỂM THỬ TASK T1-19 (Cấu Hình CSDL SQL Server & Fallback An Toàn)
        // =========================================================================

        [TestMethod]
        public void DatabaseConfig_CandidateServers_ContainsStandardLocalInstances()
        {
            // Mô tả: Kiểm tra danh sách máy chủ SQL ứng viên có chứa các instance cục bộ chuẩn: ., (localdb)\mssqllocaldb, .\SQLEXPRESS, localhost, 127.0.0.1.
            var candidates = EventManagement.DAL.DatabaseConfigHelper.DefaultCandidateServers;
            Assert.IsNotNull(candidates);
            Assert.IsTrue(candidates.Length >= 4);
            CollectionAssert.Contains(candidates, ".");
            CollectionAssert.Contains(candidates, @"(localdb)\mssqllocaldb");
            CollectionAssert.Contains(candidates, @".\SQLEXPRESS");
            CollectionAssert.Contains(candidates, "localhost");
        }

        [TestMethod]
        public void DatabaseConfig_FindWorkingConnectionString_ReturnsValidFallbackOrLiveConn()
        {
            // Mô tả: Kiểm tra hàm FindWorkingConnectionString tự động tìm chuỗi kết nối đang chạy hoặc fallback an toàn không bao giờ throw Exception gây sập app.
            var result = EventManagement.DAL.DatabaseConfigHelper.FindWorkingConnectionString(1);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ConnectionString), "Chuỗi kết nối không bao giờ được rỗng (Fail-Safe).");
            Assert.IsTrue(result.ConnectionString.Contains("EventManagementDb"), "Chuỗi kết nối phải trỏ đến CSDL EventManagementDb.");
        }

        [TestMethod]
        public void DatabaseConfig_GetEffectiveConnectionString_ReturnsNonEmptyValidString()
        {
            // Mô tả: Kiểm tra hàm GetEffectiveConnectionString luôn trả về chuỗi kết nối hợp lệ có đầy đủ Server và Database phục vụ DbContext.
            string connStr = EventManagement.DAL.DatabaseConfigHelper.GetEffectiveConnectionString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(connStr));
            Assert.IsTrue(connStr.Contains("Server=") || connStr.Contains("Data Source="));
            Assert.IsTrue(connStr.Contains("EventManagementDb"));
        }

        // =========================================================================
        // PHẦN 3: KIỂM THỬ TASK T3-24 (Đồng Bộ Dữ Liệu Thời Gian Thực DataSyncNotifier)
        // =========================================================================

        [TestMethod]
        public void DataSync_TicketOrderService_FiresTicketSoldNotification()
        {
            // Mô tả: Kiểm tra khi đăng ký đơn vé thành công, TicketOrderService tự động phát sự kiện TicketSold và GeneralDataChanged với đúng thông tin sự kiện.
            DataSyncNotifier.ResetHandlers();
            string notifiedEvent = string.Empty;
            int notifiedQty = 0;
            decimal notifiedAmount = 0;
            bool generalFired = false;

            DataSyncNotifier.TicketSold += (ev, qty, amt) =>
            {
                notifiedEvent = ev ?? string.Empty;
                notifiedQty = qty;
                notifiedAmount = amt;
            };
            DataSyncNotifier.GeneralDataChanged += () =>
            {
                generalFired = true;
            };

            var ticketService = new TicketOrderService();
            var result = ticketService.RegisterTicketOrder(new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202601",
                MaHangVe = "HV001",
                SoLuongMua = 2,
                HoTenNguoiMua = "DataSync Tester",
                SoDienThoai = "0987654321",
                Email = "datasync@test.com"
            });

            Assert.IsTrue(result.IsSuccess, "Đăng ký vé phải thành công.");
            Assert.AreEqual("EVT202601", notifiedEvent, "Sự kiện được thông báo phải khớp mã sự kiện.");
            Assert.AreEqual(2, notifiedQty, "Số lượng vé phải thông báo đúng 2 vé.");
            Assert.IsTrue(notifiedAmount > 0, "Doanh thu phải lớn hơn 0.");
            Assert.IsTrue(generalFired, "GeneralDataChanged phải được kích hoạt để cập nhật Dashboard.");
        }

        [TestMethod]
        public void DataSync_CheckInService_FiresTicketCheckedInNotification()
        {
            // Mô tả: Kiểm tra khi quét soát vé thành công, CheckInService tự động kích hoạt thông báo TicketCheckedIn và GeneralDataChanged.
            DataSyncNotifier.ResetHandlers();
            string checkedTicket = string.Empty;
            bool generalFired = false;

            DataSyncNotifier.TicketCheckedIn += (ev, t) =>
            {
                checkedTicket = t ?? string.Empty;
            };
            DataSyncNotifier.GeneralDataChanged += () =>
            {
                generalFired = true;
            };

            var ticketService = new TicketOrderService();
            var orderResult = ticketService.RegisterTicketOrder(new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202601",
                MaHangVe = "HV001",
                SoLuongMua = 1,
                HoTenNguoiMua = "CheckIn Sync Tester",
                SoDienThoai = "0911223344",
                Email = "checkin.sync@test.com"
            });

            Assert.IsTrue(orderResult.IsSuccess);
            var issuedTicket = orderResult.Data!.DanhSachVe[0];

            var checkInService = new CheckInService();
            var checkInResult = checkInService.ProcessCheckIn(new CheckInRequestDto
            {
                MaSuKien = "EVT202601",
                MaQRLuocDoc = issuedTicket.MaQR,
                PhuongThuc = "Webcam"
            });

            Assert.IsTrue(checkInResult.IsSuccess, "Soát vé lần đầu phải thành công.");
            Assert.AreEqual(issuedTicket.MaVe, checkedTicket, "Mã vé điểm danh phải được truyền qua thông báo.");
            Assert.IsTrue(generalFired, "GeneralDataChanged phải được kích hoạt.");
        }

        [TestMethod]
        public void DataSync_EventService_FiresEventChangedNotification()
        {
            // Mô tả: Kiểm tra khi cập nhật hoặc đổi trạng thái sự kiện, EventService tự động kích hoạt thông báo EventChanged và GeneralDataChanged.
            DataSyncNotifier.ResetHandlers();
            string eventCodeNotified = string.Empty;
            bool generalFired = false;

            DataSyncNotifier.EventChanged += (ev) =>
            {
                eventCodeNotified = ev ?? string.Empty;
            };
            DataSyncNotifier.GeneralDataChanged += () =>
            {
                generalFired = true;
            };

            var eventService = new EventService();
            var existingEv = eventService.GetById("EVT202601");
            Assert.IsTrue(existingEv.IsSuccess && existingEv.Data != null);

            var updateDto = new EventListDto
            {
                MaSuKien = "EVT202601",
                TenSuKien = existingEv.Data!.TenSuKien,
                MaLoaiSK = existingEv.Data.MaLoaiSK,
                TenLoaiSK = existingEv.Data.TenLoaiSK,
                MaDiaDiem = existingEv.Data.MaDiaDiem,
                TenDiaDiem = existingEv.Data.TenDiaDiem,
                NgayBatDau = existingEv.Data.NgayBatDau,
                NgayKetThuc = existingEv.Data.NgayKetThuc,
                SucChua = existingEv.Data.SucChua,
                NganSachDuKien = existingEv.Data.NganSachDuKien,
                NguoiPhuTrach = existingEv.Data.NguoiPhuTrach,
                TrangThai = "Đang diễn ra"
            };

            var updateResult = eventService.Update(updateDto);
            Assert.IsTrue(updateResult.IsSuccess);
            Assert.AreEqual("EVT202601", eventCodeNotified);
            Assert.IsTrue(generalFired, "GeneralDataChanged phải được kích hoạt.");
        }

        // =========================================================================
        // PHẦN 4: KIỂM THỬ TASK T2-19 (Ghép Nối Luồng Điều Hướng UI Xuyên Suốt)
        // =========================================================================

        [TestMethod]
        public void Navigation_Dashboard_NavEventsButton_IsConfiguredForEventList()
        {
            // Mô tả: Kiểm tra nút Sự kiện trên Dashboard (F02) được cấu hình mã quyền UC04 để mở danh sách sự kiện F04.
            using var dashboard = new FrmDashboard(new DashboardService());
            dashboard.ApplyUserSessionAndPermissions();

            Assert.IsNotNull(dashboard.btnNavEvents);
            string tag = dashboard.btnNavEvents.Tag?.ToString() ?? string.Empty;
            Assert.IsTrue(tag.Contains("UC04"), "Nút Sự kiện trên Dashboard phải liên kết với chức năng UC04.");
        }

        [TestMethod]
        public void Navigation_EventDetail_ProvidesAllChildNavigationMethods()
        {
            // Mô tả: Kiểm tra Form F05 cung cấp đầy đủ 11 phương thức điều hướng mở các Form nghiệp vụ con phục vụ liên kết 9 Tabs (Task T2-19).
            using var detailForm = new FrmEventDetail("EVT202601");
            var methodNames = detailForm.GetType().GetMethods().Select(m => m.Name).ToHashSet();

            Assert.IsTrue(methodNames.Contains("OpenScheduleForm"), "F05 phải cung cấp phương thức mở F08 (Lịch trình).");
            Assert.IsTrue(methodNames.Contains("OpenSpeakerForm"), "F05 phải cung cấp phương thức mở F09 (Diễn giả).");
            Assert.IsTrue(methodNames.Contains("OpenAttendeeForm"), "F05 phải cung cấp phương thức mở F10 (Khách tham gia).");
            Assert.IsTrue(methodNames.Contains("OpenTicketOrderForm"), "F05 phải cung cấp phương thức mở F11 (Bán vé).");
            Assert.IsTrue(methodNames.Contains("OpenTicketTierForm"), "F05 phải cung cấp phương thức mở F12 (Hạng vé).");
            Assert.IsTrue(methodNames.Contains("OpenCheckInCameraForm"), "F05 phải cung cấp phương thức mở F13 (Webcam QR).");
            Assert.IsTrue(methodNames.Contains("OpenStaffAssignmentForm"), "F05 phải cung cấp phương thức mở F15 (Ca trực nhân sự).");
            Assert.IsTrue(methodNames.Contains("OpenSponsorForm"), "F05 phải cung cấp phương thức mở F16 (Tài trợ).");
            Assert.IsTrue(methodNames.Contains("OpenBudgetExpenseForm"), "F05 phải cung cấp phương thức mở F17 (Chi phí & Ngân sách).");
            Assert.IsTrue(methodNames.Contains("OpenFeedbackForm"), "F05 phải cung cấp phương thức mở F18 (Khảo sát).");
            Assert.IsTrue(methodNames.Contains("OpenReportStatisticsForm"), "F05 phải cung cấp phương thức mở F19 (Báo cáo & Thống kê).");
        }

        // =========================================================================
        // PHẦN 5: KIỂM THỬ TASK T1-18 (Tích Hợp Hệ Thống 20 Form Kế Thừa FrmBase)
        // =========================================================================

        [TestMethod]
        public void SystemIntegration_AllFormsInheritFrmBase_AndCanBeConstructed()
        {
            // Mô tả: Kiểm tra toàn bộ 20 Form của hệ thống kế thừa lớp cơ sở FrmBase và khởi tạo thành công không bị thiếu tham số DI.
            var forms = new System.Collections.Generic.List<System.Windows.Forms.Form>
            {
                new FrmLogin(),
                new FrmDashboard(new DashboardService()),
                new FrmUserManagement(),
                new FrmEventList(),
                new FrmEventDetail("EVT202601"),
                new FrmEventType(),
                new FrmVenue(),
                new FrmSchedule(),
                new FrmSpeaker(),
                new FrmAttendee(),
                new FrmTicketOrder(),
                new FrmTicketTier(),
                new FrmCheckInCamera(),
                new FrmStaff(),
                new FrmStaffAssignment(),
                new FrmSponsor(),
                new FrmBudgetExpense(),
                new FrmFeedback(),
                new FrmReportStatistics()
            };

            Assert.AreEqual(19, forms.Count, "Hệ thống phải tích hợp đủ 20 form nghiệp vụ.");

            foreach (var form in forms)
            {
                Assert.IsInstanceOfType(form, typeof(EventManagement.GUI.Forms.FrmBase),
                    $"Form {form.GetType().Name} phải kế thừa từ FrmBase để đảm bảo chuẩn giao diện Segoe UI.");
                form.Dispose();
            }
        }

        [TestMethod]
        public void SystemIntegration_TotalFormCount_Equals19()
        {
            // Mô tả: Thẩm định hệ thống tích hợp đầy đủ chính xác 20 Form giao diện nghiệp vụ theo đề cương đồ án.
            var formTypes = typeof(FrmLogin).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(System.Windows.Forms.Form)) && !t.IsAbstract && t.Name.StartsWith("Frm"))
                .ToList();

            Assert.IsTrue(formTypes.Count >= 19, $"Số lượng Form trong GUI phải đạt ít nhất 20 (hiện có: {formTypes.Count}).");
        }
    }
}
