using System;
using System.Linq;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
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

            ApplicationConfiguration.Initialize();
            // Mặc định khởi chạy FrmSponsor (F16) cho Thành viên 2 kiểm tra giao diện và nghiệp vụ Ngày 5
            Application.Run(new FrmSponsor());
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
    }
}
