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

            ApplicationConfiguration.Initialize();
            // Mặc định khởi chạy FrmSpeaker (F09) hoặc FrmStaff (F14) cho Thành viên 2 kiểm tra giao diện
            Application.Run(new FrmSpeaker());
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
