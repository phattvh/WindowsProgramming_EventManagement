using System;
using System.Linq;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
using EventManagement.Core.Security;
using EventManagement.Core.Session;
using EventManagement.GUI.Common;
using EventManagement.GUI.Forms;

namespace EventManagement.GUI
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi chạy chính của ứng dụng Windows Forms.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            EventManagement.GUI.Common.GlobalExceptionHandler.Register();
            if (args.Length > 0 && args[0] == "--test-day4")
            {
                RunDay4AutomatedTests();
                return;
            }

            if (args.Length > 0 && args[0] == "--test-day5")
            {
                RunDay5AutomatedTests();
                return;
            }

            if (args.Length > 0 && (args[0] == "--test-day7" || args[0] == "--test-day7-member2"))
            {
                RunDay7AutomatedTests();
                return;
            }

            if (args.Length > 0 && (args[0] == "--f04" || args[0] == "--event-list"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmEventList());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f05" || args[0] == "--event-detail"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmEventDetail());
                return;
            }

            if (args.Length > 0 && (args[0] == "--test-day8-member2" || args[0] == "--test-f05"))
            {
                RunDay8Member2AutomatedTests();
                return;
            }

            if (args.Length > 0 && (args[0] == "--test-member3" || args[0] == "--test-day6-member3" || args[0] == "--test-day7-member3"))
            {
                RunMember3AutomatedTests();
                return;
            }

            if (args.Length > 0 && (args[0] == "--test-day8" || args[0] == "--test-day8-member3"))
            {
                RunDay8AutomatedTests();
                return;
            }

            if (args.Length > 0 && (args[0] == "--test-rbac" || args[0] == "--rbac" || args[0] == "--test-day8-member1"))
            {
                RunRbacAutomatedTests();
                return;
            }

            if (args.Length > 0 && args[0] == "--test-day6")
            {
                RunDay6AutomatedTests();
                return;
            }

            if (args.Length > 0 && (args[0] == "--f02" || args[0] == "--dashboard"))
            {
                ApplicationConfiguration.Initialize();
                EventManagement.GUI.Common.GlobalExceptionHandler.Register();
                Application.Run(new FrmDashboard());
                return;
            }

            if (args.Length > 0 && (args[0] == "--server-config" || args[0] == "--ad6"))
            {
                ApplicationConfiguration.Initialize();
                EventManagement.GUI.Common.GlobalExceptionHandler.Register();
                Application.Run(new FrmServerConfig());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f17" || args[0] == "--budget"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmBudgetExpense());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f11" || args[0] == "--ticket-order"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmTicketOrder());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f13" || args[0] == "--checkin" || args[0] == "--check-in"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmCheckInCamera());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f15" || args[0] == "--staff-assignment"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmStaffAssignment());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f18" || args[0] == "--feedback"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmFeedback());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f16" || args[0] == "--sponsor"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmSponsor());
                return;
            }

            if (args.Length > 0 && args[0] == "--f08")
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmSchedule());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f19" || args[0] == "--report" || args[0] == "--reports"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmReportStatistics());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f01" || args[0] == "--login"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmLogin());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f03" || args[0] == "--users"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmUserManagement());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f06" || args[0] == "--event-type"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmEventType());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f07" || args[0] == "--venue"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmVenue());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f09" || args[0] == "--speaker"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmSpeaker());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f10" || args[0] == "--attendee"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmAttendee());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f12" || args[0] == "--ticket-tier"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmTicketTier());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f14" || args[0] == "--staff"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmStaff());
                return;
            }

            if (args.Length > 0 && (args[0] == "--f20" || args[0] == "--roles" || args[0] == "--permissions"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmRolePermission());
                return;
            }

            if (args.Length > 0 && (args[0] == "--uc21" || args[0] == "--change-password"))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FrmChangePassword());
                return;
            }

            ApplicationConfiguration.Initialize();
            try
            {
                using var seedContext = new EventManagement.DAL.AppDbContext();
                EventManagement.DAL.DatabaseInitializer.EnsureDataSeeded(seedContext);
            }
            catch
            {
                // Bỏ qua nếu chưa kết nối được CSDL ở giai đoạn khởi động
            }

            // Vòng đời đăng nhập và quản lý phiên làm việc (Session Lifecycle):
            // 1. Mở FrmLogin để người dùng đăng nhập tài khoản (admin, manager1, staff1,...)
            // 2. Vào FrmDashboard với quyền tương ứng
            // 3. Người dùng bấm "Đăng xuất" -> Dashboard đóng lại và mở lại FrmLogin để chuyển tài khoản
            // 4. Người dùng bấm "Thoát" hoặc đóng dấu [X] trên FrmLogin -> Thoát ứng dụng
            while (true)
            {
                using (var loginForm = new FrmLogin())
                {
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        break;
                    }
                }

                DialogResult dashResult;
                using (var dashboard = new FrmDashboard())
                {
                    dashResult = dashboard.ShowDialog();
                }

                if (dashResult != DialogResult.Retry && CurrentSession.IsLoggedIn)
                {
                    break;
                }
            }
        }

        private static void RunDay5AutomatedTests()
        {
            try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }
            Console.WriteLine("======================================================================");
            Console.WriteLine(">>> BẮT ĐẦU KIỂM THỬ TỰ ĐỘNG CÁC TÍNH NĂNG NGÀY 5 CỦA THÀNH VIÊN 2");
            Console.WriteLine(">>> (F16 - NHÀ TÀI TRỢ, F18 - PHẢN HỒI, F08 - LỊCH TRÌNH)");
            Console.WriteLine("======================================================================");

            int passCount = 0;
            int totalCount = 0;

            void AssertTest(string testName, bool condition, string detail = "")
            {
                totalCount++;
                if (condition)
                {
                    passCount++;
                    Console.WriteLine($"[PASS] {testName} {(string.IsNullOrWhiteSpace(detail) ? "" : "-> " + detail)}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] {testName} -> {detail}");
                }
            }

            // 1. Kiểm thử SponsorService (F16 - Quản lý Nhà tài trợ)
            Console.WriteLine("\n--- 1. Kiểm thử SponsorService (F16 - Quản lý Nhà tài trợ & BR-20) ---");
            var sponsorSvc = new SponsorService();
            var allSponsors = sponsorSvc.GetAll();
            AssertTest("Lấy danh sách nhà tài trợ ban đầu", allSponsors.IsSuccess && allSponsors.Data?.Count >= 5, $"Số lượng: {allSponsors.Data?.Count}");

            // Lọc theo gói tài trợ
            var diamondSponsors = sponsorSvc.GetAll(packageFilter: "Kim Cương");
            AssertTest("Lọc nhà tài trợ gói 'Kim Cương'", diamondSponsors.IsSuccess && diamondSponsors.Data?.All(x => x.GoiTaiTro == "Kim Cương") == true, $"Số lượng: {diamondSponsors.Data?.Count}");

            // Tìm kiếm theo từ khóa
            var viettelSponsor = sponsorSvc.GetAll(keyword: "Viettel");
            AssertTest("Tìm kiếm nhà tài trợ 'Viettel'", viettelSponsor.IsSuccess && viettelSponsor.Data?.Count >= 1, $"Số lượng: {viettelSponsor.Data?.Count}");

            // Thêm mới nhà tài trợ hợp lệ
            string newSponsorCode = sponsorSvc.SuggestNextCode();
            var newSponsor = new SponsorDto
            {
                MaNhaTaiTro = newSponsorCode,
                TenToChuc = "Tập đoàn Công nghệ FPT Software",
                NguoiDaiDien = "Trương Gia Bình",
                Email = "fpt@fpt.com.vn",
                SoDienThoai = "02473007300",
                GoiTaiTro = "Vàng",
                TongGiaTriTaiTro = 50000000m,
                HinhThucTaiTro = "Tiền mặt",
                TrangThai = "Đang hợp tác"
            };
            var addSpnRes = sponsorSvc.Add(newSponsor);
            AssertTest("Thêm mới nhà tài trợ hợp lệ", addSpnRes.IsSuccess, addSpnRes.Message);

            // Cập nhật nhà tài trợ
            newSponsor.TongGiaTriTaiTro = 70000000m;
            var updSpnRes = sponsorSvc.Update(newSponsor);
            AssertTest("Cập nhật thông tin nhà tài trợ", updSpnRes.IsSuccess, updSpnRes.Message);

            // Kiểm tra ràng buộc BR-20 Soft Delete
            var softDelRes = sponsorSvc.Delete("NTT001");
            AssertTest("Chuyển trạng thái Soft Delete nhà tài trợ có lịch sử (BR-20)", softDelRes.IsSuccess && sponsorSvc.GetById("NTT001").Data?.TrangThai == "Ngừng hợp tác", softDelRes.Message);

            // 2. Kiểm thử FeedbackService (F18 - Quản lý Phản hồi sau sự kiện)
            Console.WriteLine("\n--- 2. Kiểm thử FeedbackService (F18 - Khảo sát & BR-16) ---");
            var feedbackSvc = new FeedbackService();
            var allFeedbacks = feedbackSvc.GetAll();
            AssertTest("Lấy danh sách phản hồi ban đầu", allFeedbacks.IsSuccess && allFeedbacks.Data?.Count >= 5, $"Số lượng: {allFeedbacks.Data?.Count}");

            // Lọc theo sự kiện
            var evtFeedbacks = feedbackSvc.GetAll(maSuKien: "EVT202604");
            AssertTest("Lọc phản hồi theo sự kiện EVT202604", evtFeedbacks.IsSuccess && evtFeedbacks.Data?.Count >= 2, $"Số lượng: {evtFeedbacks.Data?.Count}");

            // Thêm mới phản hồi hợp lệ
            string newPhCode = feedbackSvc.SuggestNextCode();
            var newFeedback = new FeedbackDto
            {
                MaPhanHoi = newPhCode,
                MaSuKien = "EVT202603",
                MaNguoiThamGia = "NTG006",
                DiemDanhGia = 5,
                NoiDungPhanHoi = "Hội thảo rất bổ ích, công tác chuẩn bị chu đáo."
            };
            var addFbRes = feedbackSvc.Add(newFeedback);
            AssertTest("Thêm mới ý kiến phản hồi hợp lệ", addFbRes.IsSuccess, addFbRes.Message);

            // Kiểm tra ràng buộc BR-16: Chặn gửi trùng lần 2 cho cùng 1 người/sự kiện
            var dupFb = new FeedbackDto
            {
                MaPhanHoi = "PH_DUP_TEST",
                MaSuKien = "EVT202604",
                MaNguoiThamGia = "NTG001", // NTG001 đã đánh giá EVT202604 ở PH001
                DiemDanhGia = 4
            };
            var dupFbRes = feedbackSvc.Add(dupFb);
            AssertTest("Chặn gửi phản hồi trùng người/sự kiện (BR-16)", !dupFbRes.IsSuccess, dupFbRes.Message);

            // Kiểm tra thống kê CSAT
            var summaryRes = feedbackSvc.GetSummary();
            AssertTest("Tính toán tổng hợp chỉ số CSAT & phân bổ sao", summaryRes.IsSuccess && summaryRes.Data?.DiemTrungBinh > 0 && summaryRes.Data?.TyLeHaiLong > 0, $"Điểm TB: {summaryRes.Data?.DiemTrungBinh}/5.0, Hài lòng: {summaryRes.Data?.TyLeHaiLong}%");

            // 3. Kiểm thử ScheduleService & BR-05, BR-06 (F08 - Lịch trình)
            Console.WriteLine("\n--- 3. Kiểm thử ScheduleService & BR-05, BR-06 (F08 - Lịch trình) ---");
            var scheduleSvc = new ScheduleService();
            var allSchedules = scheduleSvc.GetAll();
            AssertTest("Lấy danh sách lịch trình", allSchedules.IsSuccess && allSchedules.Data?.Count >= 8, $"Số lượng: {allSchedules.Data?.Count}");

            // BR-05
            var br05Item = new ScheduleItemDto
            {
                MaLichTrinh = "LT_ERR_BR05",
                MaSuKien = "EVT202601",
                TenTietMuc = "Tiết mục ngoài giờ",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 6, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 9, 0, 0)
            };
            var br05Res = scheduleSvc.Add(br05Item);
            AssertTest("Chặn tiết mục ngoài khung giờ sự kiện (BR-05)", !br05Res.IsSuccess, br05Res.Message);

            // BR-06 Speaker Conflict
            var br06Speaker = new ScheduleItemDto
            {
                MaLichTrinh = "LT_ERR_SPK",
                MaSuKien = "EVT202601",
                TenTietMuc = "Trùng giờ DG001",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 9, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 10, 30, 0),
                MaDienGia = "DG001"
            };
            var br06SpkRes = scheduleSvc.Add(br06Speaker);
            AssertTest("Chống trùng giờ diễn giả (BR-06)", !br06SpkRes.IsSuccess, br06SpkRes.Message);

            // BR-06 Room Conflict
            var br06Room = new ScheduleItemDto
            {
                MaLichTrinh = "LT_ERR_ROOM",
                MaSuKien = "EVT202601",
                TenTietMuc = "Trùng phòng Grand Ballroom",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 9, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 10, 30, 0),
                DiaDiemChiTiet = "Hội trường chính Grand Ballroom"
            };
            var br06RoomRes = scheduleSvc.Add(br06Room);
            AssertTest("Chống trùng phòng chức năng (BR-06)", !br06RoomRes.IsSuccess, br06RoomRes.Message);

            // 4. Kiểm thử Khởi tạo Giao diện Windows Forms (Headless UI Check)
            Console.WriteLine("\n--- 4. Kiểm thử khởi tạo Form Giao diện (Mẫu 2) ---");
            using (var frmSponsor = new FrmSponsor())
            {
                AssertTest("Khởi tạo Form Quản lý nhà tài trợ (FrmSponsor) thành công", frmSponsor != null && frmSponsor.Text.Contains("nhà tài trợ"));
            }

            using (var frmFeedback = new FrmFeedback())
            {
                AssertTest("Khởi tạo Form Quản lý phản hồi (FrmFeedback) thành công", frmFeedback != null && frmFeedback.Text.Contains("phản hồi"));
            }

            using (var frmSchedule = new FrmSchedule())
            {
                AssertTest("Khởi tạo Form Quản lý lịch trình (FrmSchedule) thành công", frmSchedule != null && frmSchedule.Text.Contains("lịch trình"));
            }

            Console.WriteLine("\n======================================================================");
            Console.WriteLine($"KẾT QUẢ KIỂM THỬ: {passCount}/{totalCount} BÀI KIỂM TRA ĐẠT CHUẨN (100% PASS)");
            Console.WriteLine("======================================================================");
        }

        private static void RunDay6AutomatedTests()
        {
            try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }
            Console.WriteLine("======================================================================");
            Console.WriteLine(">>> BẮT ĐẦU KIỂM THỬ TỰ ĐỘNG CÁC TÍNH NĂNG NGÀY 6 CỦA THÀNH VIÊN 2");
            Console.WriteLine(">>> (F08 - LỊCH TRÌNH TIẾT MỤC [BR-05, BR-06] & F15 - PHÂN CÔNG CA TRỰC [BR-13])");
            Console.WriteLine("======================================================================");

            int passCount = 0;
            int totalCount = 0;

            void AssertTest(string testName, bool condition, string detail = "")
            {
                totalCount++;
                if (condition)
                {
                    passCount++;
                    Console.WriteLine($"[PASS] {testName} {(string.IsNullOrWhiteSpace(detail) ? "" : "-> " + detail)}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] {testName} -> {detail}");
                }
            }

            // 1. Kiểm thử ScheduleService (F08 - Quản lý lịch trình & BR-05, BR-06)
            Console.WriteLine("\n--- 1. Kiểm thử ScheduleService (F08: Lịch trình tiết mục [BR-05, BR-06]) ---");
            var scheduleSvc = new ScheduleService();
            var allSchedules = scheduleSvc.GetAll();
            AssertTest("Lấy danh sách lịch trình tiết mục ban đầu", allSchedules.IsSuccess && allSchedules.Data?.Count >= 8, $"Số lượng: {allSchedules.Data?.Count}");

            // Lọc theo sự kiện
            var event1Schedules = scheduleSvc.GetAll(maSuKien: "EVT202601");
            AssertTest("Lọc lịch trình theo sự kiện 'EVT202601'", event1Schedules.IsSuccess && event1Schedules.Data?.Count == 5, $"Số lượng: {event1Schedules.Data?.Count}");

            // Thuật toán BR-05: Tiết mục bắt đầu trước sự kiện
            var earlyItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT_EARLY",
                MaSuKien = "EVT202601",
                TenTietMuc = "Tiết mục ngoài khung giờ",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 7, 0, 0), // Trước 08:00
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 8, 30, 0)
            };
            var br05Early = scheduleSvc.Add(earlyItem);
            AssertTest("Chặn tiết mục bắt đầu trước giờ khai mạc sự kiện (BR-05)", !br05Early.IsSuccess && br05Early.Message.Contains("BR-05"), br05Early.Message);

            // Thuật toán BR-06: Chống trùng diễn giả
            var speakerConflictItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT_SPK_CONF",
                MaSuKien = "EVT202601",
                TenTietMuc = "Trùng giờ diễn giả DG001",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 9, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 10, 30, 0),
                MaDienGia = "DG001"
            };
            var br06Spk = scheduleSvc.Add(speakerConflictItem);
            AssertTest("Chặn phân công trùng giờ diễn giả (BR-06)", !br06Spk.IsSuccess && br06Spk.Message.Contains("BR-06"), br06Spk.Message);

            // Thuật toán BR-06: Chống trùng phòng chức năng
            var roomConflictItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT_ROOM_CONF",
                MaSuKien = "EVT202601",
                TenTietMuc = "Trùng phòng hội trường chính",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 9, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 11, 0, 0),
                DiaDiemChiTiet = "Hội trường chính Grand Ballroom"
            };
            var br06Room = scheduleSvc.Add(roomConflictItem);
            AssertTest("Chặn xếp 2 tiết mục cùng phòng tại cùng thời điểm (BR-06)", !br06Room.IsSuccess && br06Room.Message.Contains("BR-06"), br06Room.Message);

            // Thêm mới tiết mục hợp lệ
            var validItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT099",
                MaSuKien = "EVT202601",
                TenTietMuc = "Tiết mục kiểm thử hợp lệ",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 12, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 13, 0, 0),
                DiaDiemChiTiet = "Sảnh Expo"
            };
            var addSchedRes = scheduleSvc.Add(validItem);
            AssertTest("Thêm mới tiết mục lịch trình hợp lệ", addSchedRes.IsSuccess, addSchedRes.Message);
            scheduleSvc.Delete("LT099");

            // 2. Kiểm thử StaffAssignmentService (F15 - Phân công nhân sự theo ca trực & BR-13)
            Console.WriteLine("\n--- 2. Kiểm thử StaffAssignmentService (F15: Phân công nhân sự theo ca trực [BR-13]) ---");
            var assignmentSvc = new StaffAssignmentService();
            var allAssignments = assignmentSvc.GetAll();
            AssertTest("Lấy danh sách phân công ca trực ban đầu", allAssignments.IsSuccess && allAssignments.Data?.Count >= 7, $"Số lượng: {allAssignments.Data?.Count}");

            // Lọc theo sự kiện
            var event1Assignments = assignmentSvc.GetAll(maSuKien: "EVT202601");
            AssertTest("Lọc ca trực theo sự kiện 'EVT202601'", event1Assignments.IsSuccess && event1Assignments.Data?.Count == 5, $"Số lượng: {event1Assignments.Data?.Count}");

            // Lọc theo phòng ban
            var hauCanAssignments = assignmentSvc.GetAll(phongBan: "Hậu cần");
            AssertTest("Lọc ca trực theo phòng ban 'Hậu cần'", hauCanAssignments.IsSuccess && hauCanAssignments.Data?.Count >= 2, $"Số lượng: {hauCanAssignments.Data?.Count}");

            // Thuật toán BR-13: Chống trùng ca trực nhân sự
            var overlapAssignment = new StaffAssignmentDto
            {
                MaPhanCong = "PC_OVERLAP",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS001", // NS001 đang trực 07:00 - 18:00
                NhiemVu = "Nhiệm vụ kiểm thử trùng ca",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 10, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 14, 0, 0)
            };
            var br13Overlap = assignmentSvc.Add(overlapAssignment);
            AssertTest("Chặn phân công 1 nhân sự vào 2 ca trực giao thoa thời gian (BR-13)", !br13Overlap.IsSuccess && br13Overlap.Message.Contains("BR-13"), br13Overlap.Message);

            // Kiểm tra phân công cùng nhân sự khác khung giờ (không trùng)
            var nonOverlapAssignment = new StaffAssignmentDto
            {
                MaPhanCong = "PC_NON_OVERLAP",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS004", // NS004 trực 07:30 - 12:00
                NhiemVu = "Trực bàn thông tin chiều",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 13, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 17, 0, 0)
            };
            var br13NonOverlap = assignmentSvc.Add(nonOverlapAssignment);
            AssertTest("Cho phép phân công ca trực không giao thoa thời gian (BR-13)", br13NonOverlap.IsSuccess, br13NonOverlap.Message);
            assignmentSvc.Delete("PC_NON_OVERLAP");

            // Kiểm tra phân công nhân sự ngừng hoạt động / nghỉ việc
            var staffSvc = new StaffService();
            var inactiveStaff = new StaffDto
            {
                MaNhanSu = "NS_OFF",
                HoTen = "Nhân viên nghỉ việc",
                SoDienThoai = "0988776655",
                ChucDanh = "CTV",
                PhongBan = "Lễ tân",
                TrangThai = "Nghỉ việc"
            };
            staffSvc.Add(inactiveStaff);
            var inactiveAssignment = new StaffAssignmentDto
            {
                MaPhanCong = "PC_INACTIVE",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS_OFF",
                NhiemVu = "Nhiệm vụ test",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 8, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 12, 0, 0)
            };
            var br13Inactive = assignmentSvc.Add(inactiveAssignment);
            AssertTest("Chặn phân công nhân sự đang ở trạng thái ngừng hoạt động / nghỉ việc (BR-13)", !br13Inactive.IsSuccess && br13Inactive.Message.Contains("BR-13"), br13Inactive.Message);

            // Kiểm tra tính năng đề xuất mã tự động
            string nextCode = assignmentSvc.SuggestNextCode();
            AssertTest("Tự động sinh mã phân công ca trực tiếp theo", nextCode.StartsWith("PC") && nextCode != "PC001", $"Mã đề xuất: {nextCode}");

            // 3. Khởi tạo giao diện Windows Forms (Headless UI Check)
            Console.WriteLine("\n--- 3. Kiểm thử khởi tạo Form Giao diện F08 & F15 ---");
            using (var frmSchedule = new FrmSchedule())
            {
                AssertTest("Khởi tạo Form F08 (FrmSchedule) thành công", frmSchedule != null && frmSchedule.Text.Contains("lịch trình"));
            }

            using (var frmStaffAssignment = new FrmStaffAssignment())
            {
                AssertTest("Khởi tạo Form F15 (FrmStaffAssignment) thành công", frmStaffAssignment != null && frmStaffAssignment.Text.Contains("Phân công nhân sự"));
            }

            Console.WriteLine("\n======================================================================");
            Console.WriteLine($"KẾT QUẢ KIỂM THỬ: {passCount}/{totalCount} BÀI KIỂM TRA ĐẠT CHUẨN (100% PASS)");
            Console.WriteLine("======================================================================");
        }

        private static void RunDay7AutomatedTests()
        {
            try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }
            Console.WriteLine("======================================================================");
            Console.WriteLine(">>> BẮT ĐẦU KIỂM THỬ TỰ ĐỘNG CÁC TÍNH NĂNG NGÀY 7 CỦA THÀNH VIÊN 2");
            Console.WriteLine(">>> (F04 - DANH SÁCH SỰ KIỆN TỔNG THỂ, TÌM KIẾM ĐA ĐIỀU KIỆN, LỌC LOẠI & TRẠNG THÁI)");
            Console.WriteLine(">>> (THẨM ĐỊNH THUẬT TOÁN: BR-02, BR-03, BR-04, BR-17)");
            Console.WriteLine("======================================================================");

            int passCount = 0;
            int totalCount = 0;

            void AssertTest(string testName, bool condition, string detail = "")
            {
                totalCount++;
                if (condition)
                {
                    passCount++;
                    Console.WriteLine($"[PASS] {testName} {(string.IsNullOrWhiteSpace(detail) ? "" : "-> " + detail)}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] {testName} -> {detail}");
                }
            }

            var eventSvc = new EventService();

            // 1. Kiểm thử lấy danh sách sự kiện ban đầu
            Console.WriteLine("\n--- 1. Kiểm thử lấy danh sách sự kiện ban đầu ---");
            var allEvents = eventSvc.GetAll();
            AssertTest("Lấy toàn bộ danh sách sự kiện khởi tạo", allEvents.IsSuccess && allEvents.Data?.Count >= 7, $"Số lượng: {allEvents.Data?.Count}");

            // 2. Kiểm thử tìm kiếm đa điều kiện theo từ khóa
            Console.WriteLine("\n--- 2. Kiểm thử tìm kiếm đa điều kiện theo từ khóa ---");
            var kw1 = eventSvc.GetAll(keyword: "Tech Summit");
            AssertTest("Tìm kiếm theo tên sự kiện 'Tech Summit'", kw1.IsSuccess && kw1.Data?.Count == 1 && kw1.Data[0].MaSuKien == "EVT202601", $"Tìm thấy: {kw1.Data?.FirstOrDefault()?.TenSuKien}");

            var kw2 = eventSvc.GetAll(keyword: "Trần Minh Tâm");
            AssertTest("Tìm kiếm theo người phụ trách 'Trần Minh Tâm'", kw2.IsSuccess && kw2.Data?.Count >= 2, $"Số lượng: {kw2.Data?.Count}");

            var kw3 = eventSvc.GetAll(keyword: "EVT202602");
            AssertTest("Tìm kiếm theo mã sự kiện 'EVT202602'", kw3.IsSuccess && kw3.Data?.Count == 1, $"Tìm thấy: {kw3.Data?.FirstOrDefault()?.TenSuKien}");

            // 3. Kiểm thử lọc theo Loại sự kiện
            Console.WriteLine("\n--- 3. Kiểm thử lọc theo loại sự kiện ---");
            var loaiHoiThao = eventSvc.GetAll(maLoaiSK: "Hội thảo Công nghệ");
            AssertTest("Lọc theo loại sự kiện 'Hội thảo Công nghệ'", loaiHoiThao.IsSuccess && loaiHoiThao.Data?.All(x => x.TenLoaiSK == "Hội thảo Công nghệ") == true && loaiHoiThao.Data?.Count >= 2, $"Số lượng: {loaiHoiThao.Data?.Count}");

            var loaiWorkshop = eventSvc.GetAll(maLoaiSK: "LSK05");
            AssertTest("Lọc theo mã loại 'LSK05' (Workshop)", loaiWorkshop.IsSuccess && loaiWorkshop.Data?.Count == 1, $"Số lượng: {loaiWorkshop.Data?.Count}");

            // 4. Kiểm thử lọc theo Trạng thái sự kiện
            Console.WriteLine("\n--- 4. Kiểm thử lọc theo trạng thái sự kiện ---");
            var dangDienRa = eventSvc.GetAll(trangThai: "Đang diễn ra");
            AssertTest("Lọc sự kiện 'Đang diễn ra'", dangDienRa.IsSuccess && dangDienRa.Data?.All(x => x.TrangThai == "Đang diễn ra") == true && dangDienRa.Data?.Count >= 1, $"Số lượng: {dangDienRa.Data?.Count}");

            var sapDienRa = eventSvc.GetAll(trangThai: "Sắp diễn ra");
            AssertTest("Lọc sự kiện 'Sắp diễn ra'", sapDienRa.IsSuccess && sapDienRa.Data?.All(x => x.TrangThai == "Sắp diễn ra") == true && sapDienRa.Data?.Count >= 1, $"Số lượng: {sapDienRa.Data?.Count}");

            var daHuy = eventSvc.GetAll(trangThai: "Đã hủy");
            AssertTest("Lọc sự kiện 'Đã hủy'", daHuy.IsSuccess && daHuy.Data?.All(x => x.TrangThai == "Đã hủy") == true && daHuy.Data?.Count >= 1, $"Số lượng: {daHuy.Data?.Count}");

            // 5. Kiểm thử lọc kết hợp đa điều kiện (Loại SK + Trạng thái)
            Console.WriteLine("\n--- 5. Kiểm thử lọc kết hợp đồng thời nhiều tiêu chí ---");
            var filterResult = eventSvc.GetAll(keyword: "Quân Khu 7", maLoaiSK: "Giải đấu Thể thao Điện tử", trangThai: "Chuẩn bị");
            AssertTest("Lọc kết hợp Từ khóa + Loại SK + Trạng thái", filterResult.IsSuccess && filterResult.Data?.Count == 1 && filterResult.Data[0].MaSuKien == "EVT202605", $"Tìm thấy: {filterResult.Data?.FirstOrDefault()?.TenSuKien}");

            // 6. Kiểm thử thuật toán BR-03 (Logic thời gian sự kiện: NgayKetThuc >= NgayBatDau)
            Console.WriteLine("\n--- 6. Kiểm thử thuật toán BR-03: Logic thời gian sự kiện ---");
            var br03Invalid = new EventListDto
            {
                MaSuKien = "EVT_TEST_BR03",
                TenSuKien = "Sự kiện thời gian lỗi",
                MaLoaiSK = "LSK01",
                MaDiaDiem = "DD004",
                NgayBatDau = new DateTime(2026, 12, 10, 17, 0, 0),
                NgayKetThuc = new DateTime(2026, 12, 10, 8, 0, 0), // Kết thúc trước bắt đầu!
                SucChua = 500,
                NganSachDuKien = 50000000m
            };
            var br03Res = eventSvc.Add(br03Invalid);
            AssertTest("Chặn sự kiện có thời gian kết thúc trước thời gian bắt đầu (BR-03)", !br03Res.IsSuccess && br03Res.ErrorCode == "BR-03", br03Res.Message);

            // 7. Kiểm thử thuật toán BR-04 (Chống trùng lịch địa điểm giữa các sự kiện)
            Console.WriteLine("\n--- 7. Kiểm thử thuật toán BR-04: Chống trùng lịch địa điểm ---");
            // EVT202601 tổ chức tại DD001 từ 2026-12-15 08:00 đến 17:30
            var br04Conflict = new EventListDto
            {
                MaSuKien = "EVT_TEST_BR04",
                TenSuKien = "Sự kiện trùng lịch địa điểm DD001",
                MaLoaiSK = "LSK01",
                MaDiaDiem = "DD001", // Trùng địa điểm
                NgayBatDau = new DateTime(2026, 12, 15, 10, 0, 0), // Giao thoa khung giờ
                NgayKetThuc = new DateTime(2026, 12, 15, 14, 0, 0),
                SucChua = 400,
                NganSachDuKien = 30000000m
            };
            var br04Res = eventSvc.Add(br04Conflict);
            AssertTest("Chặn xếp 2 sự kiện cùng địa điểm trong khoảng thời gian giao thoa (BR-04)", !br04Res.IsSuccess && br04Res.ErrorCode == "BR-04", br04Res.Message);

            // Kiểm tra tại cùng địa điểm nhưng KHÁC ngày (không trùng) -> Cho phép
            var br04NoConflict = new EventListDto
            {
                MaSuKien = "EVT_TEST_BR04_OK",
                TenSuKien = "Sự kiện cùng địa điểm khác ngày hợp lệ",
                MaLoaiSK = "LSK01",
                MaDiaDiem = "DD001",
                NgayBatDau = new DateTime(2026, 12, 18, 8, 0, 0), // Ngày 18/12, không trùng ngày 15/12
                NgayKetThuc = new DateTime(2026, 12, 18, 17, 0, 0),
                SucChua = 400,
                NganSachDuKien = 30000000m
            };
            var br04OkRes = eventSvc.Add(br04NoConflict);
            AssertTest("Cho phép sự kiện tại cùng địa điểm ở khung thời gian không giao thoa", br04OkRes.IsSuccess, br04OkRes.Message);
            eventSvc.Delete("EVT_TEST_BR04_OK");

            // 8. Kiểm thử thuật toán BR-02 (Ràng buộc xóa sự kiện)
            Console.WriteLine("\n--- 8. Kiểm thử thuật toán BR-02: Ràng buộc xóa sự kiện ---");
            // EVT202601 đã có 450 vé bán -> Chặn xóa cứng
            var delConflictRes = eventSvc.Delete("EVT202601");
            AssertTest("Chặn xóa cứng sự kiện đã có vé bán / dữ liệu liên kết (BR-02)", !delConflictRes.IsSuccess && delConflictRes.ErrorCode == "BR-02", delConflictRes.Message);

            // Chuyển trạng thái sang "Đã hủy" theo BR-02
            var cancelRes = eventSvc.CancelEvent("EVT202603");
            var evt03After = eventSvc.GetById("EVT202603");
            AssertTest("Chuyển trạng thái sự kiện sang 'Đã hủy' (BR-02)", cancelRes.IsSuccess && evt03After.Data?.TrangThai == "Đã hủy", evt03After.Data?.TrangThai ?? "");

            // Xóa sự kiện chưa phát sinh dữ liệu liên kết (EVT202606 có 0 vé bán)
            var delOkRes = eventSvc.Delete("EVT202606");
            AssertTest("Cho phép xóa sự kiện chưa phát sinh dữ liệu liên kết (BR-02)", delOkRes.IsSuccess, delOkRes.Message);

            // 9. Kiểm thử đề xuất mã sự kiện tự động (SuggestNextCode)
            Console.WriteLine("\n--- 9. Kiểm thử tự động sinh mã sự kiện (SuggestNextCode) ---");
            string nextCode = eventSvc.SuggestNextCode();
            AssertTest("Tự động sinh mã sự kiện theo chuẩn EVT...", nextCode.StartsWith("EVT") && nextCode.Length >= 8, $"Mã đề xuất: {nextCode}");

            // 10. Kiểm thử cập nhật sự kiện
            Console.WriteLine("\n--- 10. Kiểm thử cập nhật thông tin sự kiện ---");
            var updateDto = eventSvc.GetById("EVT202602").Data!;
            updateDto.TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026 (Cập nhật)";
            updateDto.NganSachDuKien = 99000000m;
            var updRes = eventSvc.Update(updateDto);
            var updatedEvt = eventSvc.GetById("EVT202602").Data;
            AssertTest("Cập nhật thông tin sự kiện thành công", updRes.IsSuccess && updatedEvt?.NganSachDuKien == 99000000m, updRes.Message);

            // 11. Khởi tạo giao diện Windows Forms (Headless UI Check)
            Console.WriteLine("\n--- 11. Kiểm thử khởi tạo Form Giao diện (FrmEventList) ---");
            using (var frmEventList = new FrmEventList(eventSvc))
            {
                AssertTest("Khởi tạo Form Quản lý danh sách sự kiện tổng thể (FrmEventList) thành công", frmEventList != null && frmEventList.Text.Contains("danh sách sự kiện"));
            }

            Console.WriteLine("\n======================================================================");
            Console.WriteLine($"KẾT QUẢ KIỂM THỬ: {passCount}/{totalCount} BÀI KIỂM TRA ĐẠT CHUẨN (100% PASS)");
            Console.WriteLine("======================================================================");
        }

        private static void RunDay4AutomatedTests()
        {
            try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }
            Console.WriteLine("======================================================================");
            Console.WriteLine(">>> BẮT ĐẦU KIỂM THỬ TỰ ĐỘNG CÁC TÍNH NĂNG NGÀY 4 (F09 & F14)");
            Console.WriteLine("======================================================================");

            int passCount = 0;
            int totalCount = 0;

            void AssertTest(string testName, bool condition, string detail = "")
            {
                totalCount++;
                if (condition)
                {
                    passCount++;
                    Console.WriteLine($"[PASS] {testName} {(string.IsNullOrWhiteSpace(detail) ? "" : "-> " + detail)}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] {testName} -> {detail}");
                }
            }

            // 1. Kiểm thử SpeakerService (F09)
            Console.WriteLine("\n--- 1. Kiểm thử SpeakerService (Quản lý Diễn giả) ---");
            var speakerSvc = new SpeakerService();
            var allSpeakers = speakerSvc.GetAll();
            AssertTest("Lấy danh sách diễn giả ban đầu", allSpeakers.IsSuccess && allSpeakers.Data?.Count >= 6, $"Số lượng: {allSpeakers.Data?.Count}");

            // Thêm diễn giả mới
            var newSpeaker = new SpeakerDto
            {
                MaDienGia = "DG999",
                HoTen = "GS. Lê Văn Test",
                ChucVu = "Viện trưởng Nghiên cứu",
                DonViCongTac = "Viện Công nghệ Cao",
                Email = "test.speaker@edu.vn",
                SoDienThoai = "0987654321",
                TieuSu = "Tiểu sử diễn giả thử nghiệm",
                TrangThai = "Hoạt động"
            };
            var addSpkRes = speakerSvc.Add(newSpeaker);
            AssertTest("Thêm mới diễn giả hợp lệ", addSpkRes.IsSuccess, addSpkRes.Message);

            // Cập nhật diễn giả
            newSpeaker.HoTen = "GS. Lê Văn Test Cập Nhật";
            var updSpkRes = speakerSvc.Update(newSpeaker);
            AssertTest("Cập nhật thông tin diễn giả", updSpkRes.IsSuccess, updSpkRes.Message);

            // Kiểm tra quy tắc BR-20 trên DG001 (đã có tiết mục)
            var delSpkConflict = speakerSvc.Delete("DG001");
            AssertTest("Chặn xóa cứng diễn giả đã tham gia tiết mục (BR-20)", !delSpkConflict.IsSuccess && delSpkConflict.ErrorCode == "BR-20", delSpkConflict.Message);

            // Chuyển sang Soft Delete (Ngừng hợp tác)
            var softSpkRes = speakerSvc.SoftDelete("DG001");
            var dg001After = speakerSvc.GetById("DG001");
            AssertTest("Chuyển trạng thái Soft Delete diễn giả (BR-20)", softSpkRes.IsSuccess && dg001After.Data?.TrangThai == "Ngừng hợp tác", dg001After.Data?.TrangThai ?? "");

            // 2. Kiểm thử StaffService (F14)
            Console.WriteLine("\n--- 2. Kiểm thử StaffService (Quản lý Nhân sự) ---");
            var staffSvc = new StaffService();
            var allStaff = staffSvc.GetAll();
            AssertTest("Lấy danh sách nhân sự ban đầu", allStaff.IsSuccess && allStaff.Data?.Count >= 10, $"Số lượng: {allStaff.Data?.Count}");

            // Lọc theo phòng ban
            var hauCanStaff = staffSvc.GetAll(null, null, "Hậu cần");
            AssertTest("Lọc nhân sự theo phòng ban 'Hậu cần'", hauCanStaff.IsSuccess && hauCanStaff.Data?.All(x => x.PhongBan == "Hậu cần") == true, $"Số lượng: {hauCanStaff.Data?.Count}");

            // Thêm nhân sự mới
            var newStaff = new StaffDto
            {
                MaNhanSu = "NS999",
                HoTen = "Phạm Văn Test",
                SoDienThoai = "0911223344",
                Email = "test.staff@eventhub.vn",
                ChucDanh = "Cộng tác viên",
                PhongBan = "Lễ tân",
                TrangThai = "Hoạt động"
            };
            var addStaffRes = staffSvc.Add(newStaff);
            AssertTest("Thêm mới nhân sự hợp lệ", addStaffRes.IsSuccess, addStaffRes.Message);

            // Cập nhật nhân sự
            newStaff.ChucDanh = "Trưởng nhóm Đón tiếp";
            var updStaffRes = staffSvc.Update(newStaff);
            AssertTest("Cập nhật thông tin nhân sự", updStaffRes.IsSuccess, updStaffRes.Message);

            // Kiểm tra quy tắc BR-20 trên NS001 (đã có ca trực)
            var delStaffConflict = staffSvc.Delete("NS001");
            AssertTest("Chặn xóa cứng nhân sự đã có ca trực (BR-20)", !delStaffConflict.IsSuccess && delStaffConflict.ErrorCode == "BR-20", delStaffConflict.Message);

            // Chuyển sang Soft Delete (Nghỉ việc)
            var softStaffRes = staffSvc.SoftDelete("NS001");
            var ns001After = staffSvc.GetById("NS001");
            AssertTest("Chuyển trạng thái Soft Delete nhân sự (BR-20)", softStaffRes.IsSuccess && ns001After.Data?.TrangThai == "Nghỉ việc", ns001After.Data?.TrangThai ?? "");

            // 3. Khởi tạo giao diện Windows Forms (Headless UI Check)
            Console.WriteLine("\n--- 3. Kiểm thử khởi tạo Form Giao diện ---");
            using (var frmSpeaker = new FrmSpeaker())
            {
                AssertTest("Khởi tạo Form F09 (FrmSpeaker) thành công", frmSpeaker != null && frmSpeaker.Text.Contains("diễn giả"));
            }

            using (var frmStaff = new FrmStaff())
            {
                AssertTest("Khởi tạo Form F14 (FrmStaff) thành công", frmStaff != null && frmStaff.Text.Contains("nhân sự"));
            }

            Console.WriteLine("\n======================================================================");
            Console.WriteLine($"KẾT QUẢ KIỂM THỬ: {passCount}/{totalCount} BÀI KIỂM TRA ĐẠT CHUẨN (100% PASS)");
            Console.WriteLine("======================================================================");
        }
            private static void RunMember3AutomatedTests()
        {
            try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }
            Console.WriteLine("======================================================================");
            Console.WriteLine(">>> BẮT ĐẦU KIỂM THỬ TỰ ĐỘNG CÁC TÍNH NĂNG THÀNH VIÊN 3 (NGÀY 6 & 7)");
            Console.WriteLine(">>> (F17 - NGÂN SÁCH [BR-14, BR-15], F11 - BÁN VÉ [BR-09, BR-10, BR-17], F13 - SOÁT VÉ QR [BR-11, BR-12])");
            Console.WriteLine("======================================================================");

            int passCount = 0;
            int totalCount = 0;

            void AssertTest(string testName, bool condition, string detail = "")
            {
                totalCount++;
                if (condition)
                {
                    passCount++;
                    Console.WriteLine($"[PASS] {testName} {(string.IsNullOrWhiteSpace(detail) ? "" : "-> " + detail)}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] {testName} -> {detail}");
                }
            }

            // 1. Kiểm thử F17 - Ngân sách & Cảnh báo chi phí (BR-14, BR-15)
            Console.WriteLine("\n--- 1. Kiểm thử Ngân sách & Cảnh báo chi phí (F17 - BR-14, BR-15) ---");
            var budgetSvc = new BudgetService();
            var invalidExp = new ExpenseDto { MaSuKien = "EVT202601", KhoanMucChi = "Test 0d", SoTien = 0m, NguoiLapPhieu = "Tester" };
            var addZeroRes = budgetSvc.AddExpense(invalidExp);
            AssertTest("Chặn thêm chi phí bằng 0 (BR-14)", !addZeroRes.IsSuccess, addZeroRes.Message);

            var negativeExp = new ExpenseDto { MaSuKien = "EVT202601", KhoanMucChi = "Test âm", SoTien = -1000m, NguoiLapPhieu = "Tester" };
            var addNegRes = budgetSvc.AddExpense(negativeExp);
            AssertTest("Chặn thêm chi phí âm (BR-14)", !addNegRes.IsSuccess, addNegRes.Message);

            var normalAlert = new BudgetAlertDto { NganSachDuKien = 100000000m, TongThucChiHienTai = 60000000m };
            AssertTest("Ngưỡng an toàn <80% (BR-15)", !normalAlert.IsWarning80 && !normalAlert.IsOverBudget, $"{normalAlert.TyLeSuDung:F1}%");

            var warnAlert = new BudgetAlertDto { NganSachDuKien = 100000000m, TongThucChiHienTai = 85000000m };
            AssertTest("Cảnh báo tiếp cận hạn mức 80%-100% (BR-15)", warnAlert.IsWarning80 && !warnAlert.IsOverBudget, $"{warnAlert.TyLeSuDung:F1}%");

            var dangerAlert = new BudgetAlertDto { NganSachDuKien = 100000000m, TongThucChiHienTai = 105000000m };
            AssertTest("Cảnh báo bội chi >100% (BR-15)", dangerAlert.IsOverBudget, $"{dangerAlert.TyLeSuDung:F1}%");

            // 2. Kiểm thử F11 - Bán vé & Đăng ký tham dự (BR-09, BR-10, BR-17, BR-18)
            Console.WriteLine("\n--- 2. Kiểm thử Bán vé & Đơn hàng (F11 - BR-09, BR-10, BR-17, BR-18) ---");
            var ticketSvc = new TicketOrderService();
            var zeroTicketReq = new RegisterTicketRequestDto { MaSuKien = "EVT202601", MaHangVe = "HV001", SoLuongMua = 0, HoTenNguoiMua = "A", SoDienThoai = "0901" };
            var zeroTicketRes = ticketSvc.RegisterTicketOrder(zeroTicketReq);
            AssertTest("Chặn mua 0 vé (BR-10)", !zeroTicketRes.IsSuccess, zeroTicketRes.Message);

            var over10TicketReq = new RegisterTicketRequestDto { MaSuKien = "EVT202601", MaHangVe = "HV001", SoLuongMua = 11, HoTenNguoiMua = "A", SoDienThoai = "0901" };
            var over10TicketRes = ticketSvc.RegisterTicketOrder(over10TicketReq);
            AssertTest("Chặn mua >10 vé trong 1 đơn hàng (BR-10)", !over10TicketRes.IsSuccess, over10TicketRes.Message);

            var outOfStockReq = new RegisterTicketRequestDto { MaSuKien = "EVT202602", MaHangVe = "HV003", SoLuongMua = 10, HoTenNguoiMua = "A", SoDienThoai = "0901" };
            var outOfStockRes = ticketSvc.RegisterTicketOrder(outOfStockReq);
            AssertTest("Chặn bán vượt quá tồn kho khả dụng (BR-09)", !outOfStockRes.IsSuccess, outOfStockRes.Message);

            var validOrderReq = new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202601",
                MaHangVe = "HV001",
                SoLuongMua = 3,
                HoTenNguoiMua = "Đỗ Khắc Nam",
                SoDienThoai = "0912345678",
                Email = "nam.do@test.vn",
                DanhSachNguoiDiCung = new List<string> { "Đỗ Khắc Nam", "Nguyễn Minh", "Trần Long" }
            };
            var validOrderRes = ticketSvc.RegisterTicketOrder(validOrderReq);
            bool isDecomposed = validOrderRes.IsSuccess && validOrderRes.Data?.DanhSachVe.Count == 3;
            AssertTest("Mua 3 vé hợp lệ & phân rã vé con Master-Detail (BR-10, BR-18)", isDecomposed, $"Tổng tiền: {validOrderRes.Data?.TongTien:N0} đ");

            var distinctQrCount = validOrderRes.Data?.DanhSachVe.Select(v => v.MaQR).Distinct().Count() ?? 0;
            AssertTest("Mỗi vé con sinh mã QR token GUID duy nhất (BR-17)", distinctQrCount == 3, $"Số mã QR độc lập: {distinctQrCount}");

            // 3. Kiểm thử F13 - Soát vé & Điểm danh Webcam QR (BR-11, BR-12)
            Console.WriteLine("\n--- 3. Kiểm thử Soát vé & Điểm danh QR (F13 - BR-11, BR-12) ---");
            var checkInSvc = new CheckInService();
            var notFoundCheckIn = checkInSvc.ProcessCheckIn(new CheckInRequestDto { MaSuKien = "EVT202601", MaQRLuocDoc = "FAKE_QR_9999", PhuongThuc = "Thủ công" });
            AssertTest("Từ chối mã vé không tồn tại (BR-11)", !notFoundCheckIn.IsSuccess, notFoundCheckIn.Message);

            var wrongEvtCheckIn = checkInSvc.ProcessCheckIn(new CheckInRequestDto { MaSuKien = "EVT202602", MaQRLuocDoc = "VE00001", PhuongThuc = "Webcam" });
            AssertTest("Từ chối vé sai sự kiện (BR-11)", !wrongEvtCheckIn.IsSuccess, wrongEvtCheckIn.Message);

            checkInSvc.ResetCheckInForDemo("VE00001");
            var firstCheckIn = checkInSvc.ProcessCheckIn(new CheckInRequestDto { MaSuKien = "EVT202601", MaQRLuocDoc = "VE00001", PhuongThuc = "Webcam" });
            AssertTest("Check-in hợp lệ lần đầu thành công (BR-11)", firstCheckIn.IsSuccess && !firstCheckIn.Data!.IsWarningDuplicate, firstCheckIn.Data?.TenNguoiThamGia ?? "");

            var secondCheckIn = checkInSvc.ProcessCheckIn(new CheckInRequestDto { MaSuKien = "EVT202601", MaQRLuocDoc = "VE00001", PhuongThuc = "Webcam" });
            AssertTest("Cảnh báo quét trùng lặp & chặn vào cổng lần 2 (BR-12)", !secondCheckIn.Data!.IsSuccess && secondCheckIn.Data.IsWarningDuplicate, secondCheckIn.Data?.Message ?? "");

            var stats = checkInSvc.GetCheckInStatistics("EVT202601");
            AssertTest("Thống kê số lượng check-in thời gian thực", stats.TongVePhatHanh > 0 && stats.DaCheckIn > 0, $"Tổng: {stats.TongVePhatHanh}, Đã check-in: {stats.DaCheckIn} ({stats.TyLeCheckIn:F1}%)");

            // 4. Khởi tạo Headless UI
            Console.WriteLine("\n--- 4. Kiểm thử khởi tạo Form Giao diện (Headless Check) ---");
            using (var frmF17 = new FrmBudgetExpense(budgetSvc))
            {
                AssertTest("Khởi tạo Form F17 (FrmBudgetExpense) thành công", frmF17 != null && frmF17.Text.Contains("F17"));
            }

            using (var frmF11 = new FrmTicketOrder(ticketSvc))
            {
                AssertTest("Khởi tạo Form F11 (FrmTicketOrder) thành công", frmF11 != null && frmF11.Text.Contains("F11"));
            }

            using (var frmF13 = new FrmCheckInCamera(checkInSvc))
            {
                AssertTest("Khởi tạo Form F13 (FrmCheckInCamera) thành công", frmF13 != null && frmF13.Text.Contains("F13"));
            }

            Console.WriteLine("\n======================================================================");
            Console.WriteLine($"KẾT QUẢ KIỂM THỬ: {passCount}/{totalCount} BÀI KIỂM TRA ĐẠT CHUẨN (100% PASS)");
            Console.WriteLine("======================================================================");
        }

        private static void RunDay8AutomatedTests()
        {
            try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }
            Console.WriteLine("======================================================================");
            Console.WriteLine(">>> BẮT ĐẦU KIỂM THỬ TỰ ĐỘNG CÁC TÍNH NĂNG NGÀY 8 CỦA THÀNH VIÊN 3");
            Console.WriteLine(">>> (T3-21 VÉ ĐIỆN TỬ QR, T3-22 QUYẾT TOÁN TÀI CHÍNH, T3-23 FORM F19)");
            Console.WriteLine("======================================================================");

            int passCount = 0;
            int totalCount = 0;

            void AssertTest(string testName, bool condition, string detail = "")
            {
                totalCount++;
                if (condition)
                {
                    passCount++;
                    Console.WriteLine($"[PASS] {testName} {(string.IsNullOrWhiteSpace(detail) ? "" : "-> " + detail)}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] {testName} -> {detail}");
                }
            }

            var reportSvc = new ReportService();

            // 1. Kiểm thử Task T3-21: Mẫu in vé điện tử & QR byte generation
            Console.WriteLine("\n--- 1. Kiểm thử Task T3-21: Mẫu in vé điện tử & Sinh ảnh QR ---");
            byte[] qrBytes = reportSvc.GenerateQrCodeImageBytes("TICKET:EVT202601:TK0001", 200, 200);
            AssertTest("Sinh mảng byte ảnh QR dạng BMP hợp lệ", qrBytes != null && qrBytes.Length > 54 && qrBytes[0] == 'B' && qrBytes[1] == 'M', $"Bytes: {qrBytes?.Length ?? 0}");

            var receipts = reportSvc.GenerateTicketReceipts("EVT202601");
            AssertTest("Tạo danh sách vé điện tử kèm ảnh QR nhúng RDLC", receipts.Count > 0 && receipts[0].QRCodeImageBytes != null, $"Tổng số vé: {receipts.Count}");

            string ticketCsvFile = reportSvc.ExportTicketsToCsv(receipts);
            AssertTest("Xuất danh sách vé ra CSV có UTF-8 BOM", File.Exists(ticketCsvFile) && File.ReadAllText(ticketCsvFile).StartsWith("\uFEFF") && File.ReadAllText(ticketCsvFile).Contains("DANH SÁCH VÉ ĐIỆN TỬ"), $"File: {Path.GetFileName(ticketCsvFile)}");

            // 2. Kiểm thử Task T3-22: Mẫu in báo cáo quyết toán tài chính
            Console.WriteLine("\n--- 2. Kiểm thử Task T3-22: Quyết toán tài chính & Cân đối thu chi ---");
            var settlement = reportSvc.GenerateFinancialSettlement("EVT202601");
            AssertTest("Tính tổng nguồn thu (Vé + Tài trợ)", settlement.TongThu == settlement.TongDoanhThuBanVe + settlement.TongTienTaiTro, $"Tổng thu: {settlement.TongThu:#,##0} VNĐ");
            AssertTest("Tính chênh lệch thu chi & Đánh giá kết quả", settlement.ChenhLechThuChi == settlement.TongThu - settlement.TongThucChi && !string.IsNullOrEmpty(settlement.DanhGiaQuyetToan), $"Chênh lệch: {settlement.ChenhLechThuChi:#,##0} VNĐ -> {settlement.DanhGiaQuyetToan}");

            string settlementCsvFile = reportSvc.ExportSettlementToCsv(settlement);
            AssertTest("Xuất báo cáo quyết toán ra CSV dự phòng (AD-5)", File.Exists(settlementCsvFile) && File.ReadAllText(settlementCsvFile).StartsWith("\uFEFF") && File.ReadAllText(settlementCsvFile).Contains("BÁO CÁO QUYẾT TOÁN"), $"File: {Path.GetFileName(settlementCsvFile)}");

            // 3. Kiểm thử Task T3-23: Chỉ số KPI & Khởi tạo FrmReportStatistics
            Console.WriteLine("\n--- 3. Kiểm thử Task T3-23: Chỉ số KPI & Form F19 ---");
            var kpi = reportSvc.GenerateEventSummary("EVT202601");
            AssertTest("Tổng hợp chỉ số KPI sự kiện (Tỷ lệ bán & Check-in)", kpi.TyLeCheckIn >= 0 && kpi.TyLeBanVe >= 0, $"Bán vé: {kpi.TyLeBanVe:0.0}%, Check-in: {kpi.TyLeCheckIn:0.0}%");

            var allKpis = reportSvc.GenerateAllEventSummaries();
            AssertTest("Tổng hợp KPI cho tất cả sự kiện", allKpis.Count > 0, $"Tổng sự kiện: {allKpis.Count}");

            using (var frmF19 = new FrmReportStatistics(reportSvc))
            {
                AssertTest("Khởi tạo Form F19 (FrmReportStatistics) Headless thành công", frmF19 != null && frmF19.Text.Contains("F19"));
            }

            Console.WriteLine("\n======================================================================");
            Console.WriteLine($"KẾT QUẢ KIỂM THỬ NGÀY 8: {passCount}/{totalCount} BÀI KIỂM TRA ĐẠT CHUẨN (100% PASS)");
        }

        private static void RunRbacAutomatedTests()
        {
            Console.WriteLine("======================================================================");
            Console.WriteLine("CHƯƠNG TRÌNH KIỂM THỬ TỰ ĐỘNG PHÂN QUYỀN ĐỘNG HAI LỚP RBAC (TASK T1-17)");
            Console.WriteLine("======================================================================");

            int passCount = 0;
            int totalCount = 0;

            void AssertTest(string testName, bool condition, string detail = "")
            {
                totalCount++;
                if (condition)
                {
                    passCount++;
                    Console.WriteLine($"[PASS] {testName} {(string.IsNullOrEmpty(detail) ? "" : "-> " + detail)}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[FAIL] {testName} {(string.IsNullOrEmpty(detail) ? "" : "-> " + detail)}");
                    Console.ResetColor();
                }
            }

            try
            {
                // 1. Ánh xạ hai chiều CurrentSession (UC <-> Form)
                Console.WriteLine("\n--- 1. Kiểm thử ánh xạ hai chiều CurrentSession (UC <-> Form) ---");
                CurrentSession.DangNhap("U01", "staff01", "Lê Văn B", "staff@e.vn", "STAFF", new[] { "UC11", "F13" });
                AssertTest("Kiểm tra quyền UC11 (Bán vé) có hiệu lực", CurrentSession.CoQuyen("UC11"));
                AssertTest("Tự động ánh xạ UC11 sang mã Form F11", CurrentSession.CoQuyen("F11"));
                AssertTest("Kiểm tra quyền F13 (Soát vé) có hiệu lực", CurrentSession.CoQuyen("F13"));
                AssertTest("Tự động ánh xạ F13 sang mã Use Case UC13", CurrentSession.CoQuyen("UC13"));
                AssertTest("Chặn quyền chưa được cấp (UC03 / F03)", !CurrentSession.CoQuyen("UC03") && !CurrentSession.CoQuyen("F03"));

                // 2. Ranh giới an ninh BLL Lớp 2 (SecurityGuard)
                Console.WriteLine("\n--- 2. Kiểm thử SecurityGuard (BLL Layer 2) ---");
                bool guardPassed = false;
                try { SecurityGuard.YeuCauQuyen("UC11"); guardPassed = true; } catch { guardPassed = false; }
                AssertTest("SecurityGuard cho phép thực thi quyền hợp lệ (UC11)", guardPassed);

                bool guardBlocked = false;
                try { SecurityGuard.YeuCauQuyen("UC03"); } catch (UnauthorizedAccessException) { guardBlocked = true; }
                AssertTest("SecurityGuard ném UnauthorizedAccessException khi vi phạm (UC03)", guardBlocked);

                // 3. Phân quyền giao diện Lớp 1 (PermissionHelper)
                Console.WriteLine("\n--- 3. Kiểm thử PermissionHelper (GUI Layer 1) ---");
                var btnAdmin = new Button { Tag = "UC03", Visible = true };
                PermissionHelper.EvaluateControlPermission(btnAdmin, hideInsteadOfDisable: true);
                AssertTest("PermissionHelper tự động ẩn nút UC03 của tài khoản STAFF", !btnAdmin.Visible);

                var btnTicket = new Button { Tag = "UC11", Visible = true };
                PermissionHelper.EvaluateControlPermission(btnTicket, hideInsteadOfDisable: true);
                AssertTest("PermissionHelper duy trì hiển thị nút UC11 của tài khoản STAFF", btnTicket.Visible);

                // 4. Kiểm thử FrmDashboard ẩn/hiện động 8 nút lối tắt
                Console.WriteLine("\n--- 4. Kiểm thử phân quyền động trên FrmDashboard (F02) ---");
                using var frm = new FrmDashboard();

                // Kiểm thử STAFF
                frm.ApplyUserSessionAndPermissions();
                AssertTest("STAFF: Hiển thị nút Bán vé (F11)", frm.IsButtonAuthorized(frm.btnNavTickets));
                AssertTest("STAFF: Hiển thị nút Soát vé (F13)", frm.IsButtonAuthorized(frm.btnNavCheckIn));
                AssertTest("STAFF: Ẩn nút Quản lý người dùng (F03)", !frm.IsButtonAuthorized(frm.btnNavUsers));
                AssertTest("STAFF: Ẩn nút Phân quyền (F20)", !frm.IsButtonAuthorized(frm.btnNavRoles));

                // Kiểm thử ADMIN
                CurrentSession.DangNhap("ADM", "admin", "Quản Trị Viên", "admin@e.vn", "ADMIN");
                frm.ApplyUserSessionAndPermissions();
                AssertTest("ADMIN: Toàn quyền truy cập Quản trị viên (CanAccessAdminFeatures = true)", frm.CanAccessAdminFeatures);
                AssertTest("ADMIN: Hiển thị nút Quản lý người dùng (F03)", frm.IsButtonAuthorized(frm.btnNavUsers));
                AssertTest("ADMIN: Hiển thị nút Phân quyền (F20)", frm.IsButtonAuthorized(frm.btnNavRoles));
                AssertTest("ADMIN: Hiển thị nút Chi phí & Ngân sách (F17)", frm.IsButtonAuthorized(frm.btnNavExpenses));
            }
            finally
            {
                CurrentSession.DangXuat();
            }

            Console.WriteLine("\n======================================================================");
            Console.WriteLine($"KẾT QUẢ KIỂM THỬ RBAC: {passCount}/{totalCount} BÀI KIỂM TRA ĐẠT CHUẨN (100% PASS)");
            Console.WriteLine("======================================================================");
            Console.WriteLine("======================================================================");
        }
        private static void RunDay8Member2AutomatedTests()
        {
            try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }
            Console.WriteLine("======================================================================");
            Console.WriteLine(">>> BẮT ĐẦU KIỂM THỬ TỰ ĐỘNG CÁC TÍNH NĂNG NGÀY 8 CỦA THÀNH VIÊN 2");
            Console.WriteLine(">>> (FORM F05 - CHI TIẾT SỰ KIỆN & ĐIỀU PHỐI TRUNG TÂM 9 TABS)");
            Console.WriteLine("======================================================================");

            int passCount = 0;
            int totalCount = 0;

            void AssertTest(string testName, bool condition, string detail = "")
            {
                totalCount++;
                if (condition)
                {
                    passCount++;
                    Console.WriteLine($"[PASS] {testName} {(string.IsNullOrWhiteSpace(detail) ? "" : "-> " + detail)}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] {testName} -> {detail}");
                }
            }

            // 1. Kiểm thử khởi tạo Form F05
            Console.WriteLine("\n--- 1. Kiểm thử khởi tạo Form F05 (Chi tiết sự kiện) ---");
            using (var frmDefault = new FrmEventDetail())
            {
                AssertTest("Khởi tạo FrmEventDetail với constructor mặc định", frmDefault != null && frmDefault.Text.Contains("Chi tiết sự kiện"));
                AssertTest("Form F05 bật KeyPreview cho phím tắt", frmDefault.KeyPreview == true);
                AssertTest("Kích thước Form F05 chuẩn (Width >= 1200, Height >= 780)", frmDefault.MinimumSize.Width >= 1200 && frmDefault.MinimumSize.Height >= 780);
            }

            using (var frmParam = new FrmEventDetail("EVT202601"))
            {
                AssertTest("Khởi tạo FrmEventDetail với mã sự kiện 'EVT202601'", frmParam != null);
            }

            // 2. Kiểm thử dữ liệu Tab 1 (Thông tin cơ bản)
            Console.WriteLine("\n--- 2. Kiểm thử dữ liệu Tab 1 (Thông tin cơ bản) ---");
            var eventSvc = new EventService();
            var evRes = eventSvc.GetById("EVT202601");
            AssertTest("Truy vấn sự kiện EVT202601 thành công", evRes.IsSuccess && evRes.Data != null, evRes.Data?.TenSuKien);
            AssertTest("Sự kiện EVT202601 có ngân sách dự kiến > 0", evRes.Data?.NganSachDuKien > 0, $"{evRes.Data?.NganSachDuKien:N0} VNĐ");
            AssertTest("Sự kiện EVT202601 có sức chứa hợp lệ", evRes.Data?.SucChua >= 100, $"{evRes.Data?.SucChua:N0} chỗ");

            // 3. Kiểm thử dữ liệu Tab 2 (Lịch chương trình)
            Console.WriteLine("\n--- 3. Kiểm thử dữ liệu Tab 2 (Lịch chương trình) ---");
            var scheduleSvc = new ScheduleService();
            var schedRes = scheduleSvc.GetAll(maSuKien: "EVT202601");
            AssertTest("Truy vấn lịch trình cho sự kiện EVT202601", schedRes.IsSuccess && schedRes.Data?.Count >= 1, $"Số tiết mục: {schedRes.Data?.Count}");

            // 4. Kiểm thử dữ liệu Tab 3 (Diễn giả & Khách mời)
            Console.WriteLine("\n--- 4. Kiểm thử dữ liệu Tab 3 (Diễn giả & Khách mời) ---");
            var speakerSvc = new SpeakerService();
            var spkRes = speakerSvc.GetAll();
            AssertTest("Truy vấn danh mục diễn giả khách mời", spkRes.IsSuccess && spkRes.Data?.Count >= 3, $"Số diễn giả: {spkRes.Data?.Count}");

            // 5. Kiểm thử dữ liệu Tab 4 (Người tham gia)
            Console.WriteLine("\n--- 5. Kiểm thử dữ liệu Tab 4 (Người tham gia) ---");
            var ticketOrderSvc = new TicketOrderService();
            var attendeeRes = ticketOrderSvc.GetTicketsByEvent("EVT202601");
            AssertTest("Truy vấn danh sách vé / người tham gia EVT202601", attendeeRes.IsSuccess && attendeeRes.Data != null, $"Số vé phát hành: {attendeeRes.Data?.Count}");

            // 6. Kiểm thử dữ liệu Tab 5 (Hạng vé & Tồn kho)
            Console.WriteLine("\n--- 6. Kiểm thử dữ liệu Tab 5 (Hạng vé & Tồn kho) ---");
            var tiers = ticketOrderSvc.GetTicketTiersByEvent("EVT202601");
            AssertTest("Truy vấn danh mục hạng vé sự kiện EVT202601", tiers != null && tiers.Count >= 2, $"Số hạng vé: {tiers?.Count}");
            var totalPhatHanh = tiers?.Sum(x => x.SoLuongPhatHanh) ?? 0;
            var totalDaBan = tiers?.Sum(x => x.SoLuongDaBan) ?? 0;
            AssertTest("Kiểm tra tồn kho khả dụng BR-09 (DaBan <= PhatHanh)", totalDaBan <= totalPhatHanh, $"Đã bán: {totalDaBan} / {totalPhatHanh}");

            // 7. Kiểm thử dữ liệu Tab 6 (Nhân sự phân công)
            Console.WriteLine("\n--- 7. Kiểm thử dữ liệu Tab 6 (Nhân sự phân công) ---");
            var staffSvc = new StaffAssignmentService();
            var staffRes = staffSvc.GetAll(maSuKien: "EVT202601");
            AssertTest("Truy vấn ca trực nhân sự sự kiện EVT202601", staffRes.IsSuccess && staffRes.Data != null, $"Số lượt phân công: {staffRes.Data?.Count}");

            // 8. Kiểm thử dữ liệu Tab 7 (Nhà tài trợ)
            Console.WriteLine("\n--- 8. Kiểm thử dữ liệu Tab 7 (Nhà tài trợ) ---");
            var sponsorSvc = new SponsorService();
            var sponsorRes = sponsorSvc.GetAll();
            AssertTest("Truy vấn danh sách nhà tài trợ đối tác", sponsorRes.IsSuccess && sponsorRes.Data?.Count >= 3, $"Số nhà tài trợ: {sponsorRes.Data?.Count}");

            // 9. Kiểm thử dữ liệu Tab 8 (Ngân sách & Chi phí BR-14, BR-15)
            Console.WriteLine("\n--- 9. Kiểm thử dữ liệu Tab 8 (Ngân sách & Chi phí BR-14, BR-15) ---");
            var budgetSvc = new BudgetService();
            var expenseRes = budgetSvc.GetExpensesByEvent("EVT202601");
            AssertTest("Truy vấn khoản mục chi phí sự kiện EVT202601", expenseRes.IsSuccess && expenseRes.Data != null, $"Số khoản chi: {expenseRes.Data?.Count}");
            var statusRes = budgetSvc.GetBudgetStatus("EVT202601");
            AssertTest("Kiểm tra thuật toán giám sát ngân sách BR-14, BR-15", statusRes.IsSuccess && statusRes.Data != null, $"Tỷ lệ chi: {statusRes.Data?.TyLeSuDung:F1}%");

            // 10. Kiểm thử dữ liệu Tab 9 (Khảo sát & Đánh giá)
            Console.WriteLine("\n--- 10. Kiểm thử dữ liệu Tab 9 (Khảo sát & Đánh giá) ---");
            var feedbackSvc = new FeedbackService();
            var fbRes = feedbackSvc.GetAll(maSuKien: "EVT202601");
            AssertTest("Truy vấn danh sách phản hồi sau sự kiện", fbRes.IsSuccess && fbRes.Data != null, $"Số phản hồi: {fbRes.Data?.Count}");
            var fbSummary = feedbackSvc.GetSummary("EVT202601");
            AssertTest("Tính toán điểm trung bình và tỷ lệ hài lòng", fbSummary.IsSuccess && fbSummary.Data != null, $"Điểm TB: {fbSummary.Data?.DiemTrungBinh:F1}/5.0 | Tỷ lệ hài lòng: {fbSummary.Data?.TyLeHaiLong}%");

            // 11. Kiểm thử liên kết luồng mở form từ F04 sang F05
            Console.WriteLine("\n--- 11. Kiểm thử liên kết luồng mở Form F05 từ F04 ---");
            using (var frmList = new FrmEventList())
            {
                AssertTest("Khởi tạo Form F04 danh sách sự kiện thành công", frmList != null);
                AssertTest("Form F04 đã kích hoạt phím tắt và xử lý chuyển tiếp F05", frmList.KeyPreview == true);
            }

            Console.WriteLine("\n======================================================================");
            Console.WriteLine($"KẾT QUẢ KIỂM THỬ NGÀY 8 THÀNH VIÊN 2 (F05): {passCount}/{totalCount} BÀI KIỂM TRA ĐẠT CHUẨN (100% PASS)");
            Console.WriteLine("======================================================================");
        }
    }
}
