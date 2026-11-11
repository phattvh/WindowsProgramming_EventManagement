using System;
using System.Linq;
using EventManagement.Core.Entities;

namespace EventManagement.DAL
{
    /// <summary>
    /// Tự động kiểm tra và nạp dữ liệu hạt giống (Seed Data) cho CSDL nếu chưa có (AD-7 / T1-12)
    /// Đảm bảo luôn có 3 tài khoản mẫu chuẩn: admin, manager1, staff1
    /// </summary>
    public static class DatabaseInitializer
    {
        // Hash BCrypt chuẩn cho mật khẩu "Admin@123"
        public const string DefaultPasswordHash = "$2a$11$Vn9HCdEIZew5LLR7UcYHH.0eTE9n3Jj928xQ7TJq8mQauMf9sOe0y";

        public static void EnsureDataSeeded(AppDbContext context)
        {
            try
            {
                // 1. Nhóm quyền
                if (!context.NhomQuyens.Any())
                {
                    context.NhomQuyens.AddRange(
                        new NhomQuyen { MaNhomQuyen = "ADMIN", TenNhomQuyen = "Quản trị viên", MoTa = "Toàn quyền quản trị hệ thống" },
                        new NhomQuyen { MaNhomQuyen = "MANAGER", TenNhomQuyen = "Quản lý sự kiện", MoTa = "Quản lý vòng đời sự kiện và điều phối" },
                        new NhomQuyen { MaNhomQuyen = "STAFF", TenNhomQuyen = "Nhân viên", MoTa = "Thực thi tác nghiệp bán vé, soát vé" }
                    );
                    context.SaveChanges();
                }

                // 2. Chức năng (UC01 - UC21)
                if (!context.ChucNangs.Any())
                {
                    var functions = new[]
                    {
                        ("UC01", "Đăng nhập hệ thống", "F01"),
                        ("UC02", "Xem Dashboard / Trang chủ", "F02"),
                        ("UC03", "Quản lý người dùng & Khóa tài khoản", "F03"),
                        ("UC04", "Quản lý danh sách sự kiện", "F04"),
                        ("UC05", "Xem & Điều phối chi tiết sự kiện (9 Tabs)", "F05"),
                        ("UC06", "Quản loại sự kiện", "F06"),
                        ("UC07", "Quản lý địa điểm & hội trường", "F07"),
                        ("UC08", "Quản lý lịch trình & timeline", "F08"),
                        ("UC09", "Quản lý diễn giả & khách mời", "F09"),
                        ("UC10", "Quản lý hồ sơ người tham gia", "F10"),
                        ("UC11", "Tiếp nhận đăng ký & Bán vé", "F11"),
                        ("UC12", "Quản lý các hạng vé & mở bán", "F12"),
                        ("UC13", "Soát vé Check-in (Webcam QR Code)", "F13"),
                        ("UC14", "Quản lý danh mục nhân sự", "F14"),
                        ("UC15", "Phân công ca trực nhân sự", "F15"),
                        ("UC16", "Quản lý nhà tài trợ & gói tài trợ", "F16"),
                        ("UC17", "Quản lý ngân sách & chi phí phát sinh", "F17"),
                        ("UC19", "Báo cáo thống kê & Xuất Excel / PDF", "F19"),
                        ("UC20", "Cấu hình hệ thống & Ma trận phân quyền", "F20"),
                        ("UC21", "Đổi mật khẩu cá nhân", "UC21")
                    };

                    foreach (var (code, name, form) in functions)
                    {
                        context.ChucNangs.Add(new ChucNang { MaChucNang = code, TenChucNang = name, MaForm = form });
                    }
                    context.SaveChanges();
                }

                // 3. Phân quyền
                if (!context.PhanQuyens.Any())
                {
                    var allFuncs = context.ChucNangs.Select(c => c.MaChucNang).ToList();
                    // ADMIN có tất cả
                    foreach (var f in allFuncs)
                    {
                        context.PhanQuyens.Add(new PhanQuyen { MaNhomQuyen = "ADMIN", MaChucNang = f, CoQuyen = true });
                    }
                    // MANAGER
                    foreach (var f in allFuncs.Where(x => x != "UC03" && x != "UC20"))
                    {
                        context.PhanQuyens.Add(new PhanQuyen { MaNhomQuyen = "MANAGER", MaChucNang = f, CoQuyen = true });
                    }
                    // STAFF
                    var staffFuncs = new[] { "UC01", "UC02", "UC08", "UC10", "UC11", "UC13", "UC21" };
                    foreach (var f in staffFuncs)
                    {
                        context.PhanQuyens.Add(new PhanQuyen { MaNhomQuyen = "STAFF", MaChucNang = f, CoQuyen = true });
                    }
                    context.SaveChanges();
                }

                // 4. Ba tài khoản người dùng mẫu
                bool hasChanged = false;
                if (!context.NguoiDungs.Any(u => u.TenDangNhap == "admin"))
                {
                    context.NguoiDungs.Add(new NguoiDung
                    {
                        MaNguoiDung = "USR001",
                        TenDangNhap = "admin",
                        MatKhauHash = DefaultPasswordHash,
                        HoTen = "Quản trị viên Hệ thống",
                        Email = "admin@eventmanagement.vn",
                        SoDienThoai = "0901234567",
                        MaNhomQuyen = "ADMIN",
                        TrangThai = "Hoạt động",
                        SoLanDangNhapSai = 0,
                        LanDoiMatKhauGanNhat = DateTime.Now
                    });
                    hasChanged = true;
                }

                if (!context.NguoiDungs.Any(u => u.TenDangNhap == "manager1"))
                {
                    context.NguoiDungs.Add(new NguoiDung
                    {
                        MaNguoiDung = "USR002",
                        TenDangNhap = "manager1",
                        MatKhauHash = DefaultPasswordHash,
                        HoTen = "Trần Minh Tâm (Trưởng BTC)",
                        Email = "tam.tran@eventhub.vn",
                        SoDienThoai = "0918765432",
                        MaNhomQuyen = "MANAGER",
                        TrangThai = "Hoạt động",
                        SoLanDangNhapSai = 0,
                        LanDoiMatKhauGanNhat = DateTime.Now
                    });
                    hasChanged = true;
                }

                if (!context.NguoiDungs.Any(u => u.TenDangNhap == "staff1"))
                {
                    context.NguoiDungs.Add(new NguoiDung
                    {
                        MaNguoiDung = "USR003",
                        TenDangNhap = "staff1",
                        MatKhauHash = DefaultPasswordHash,
                        HoTen = "Lê Thị Hương (Soát vé)",
                        Email = "huong.le@eventhub.vn",
                        SoDienThoai = "0987112233",
                        MaNhomQuyen = "STAFF",
                        TrangThai = "Hoạt động",
                        SoLanDangNhapSai = 0,
                        LanDoiMatKhauGanNhat = DateTime.Now
                    });
                    hasChanged = true;
                }

                // Bổ sung tài khoản kiểm thử cho Manager (MG-02, MG-03, MG-14)
                if (!context.NguoiDungs.Any(u => u.TenDangNhap == "manager2"))
                {
                    context.NguoiDungs.Add(new NguoiDung
                    {
                        MaNguoiDung = "USR006",
                        TenDangNhap = "manager2",
                        MatKhauHash = DefaultPasswordHash,
                        HoTen = "Nguyễn Hoàng Nam (Phó BTC)",
                        Email = "nam.nguyen@eventhub.vn",
                        SoDienThoai = "0918765433",
                        MaNhomQuyen = "MANAGER",
                        TrangThai = "Hoạt động",
                        SoLanDangNhapSai = 0,
                        LanDoiMatKhauGanNhat = DateTime.Now
                    });
                    hasChanged = true;
                }

                var mgrLock = context.NguoiDungs.FirstOrDefault(u => u.TenDangNhap == "manager_lock");
                if (mgrLock == null)
                {
                    context.NguoiDungs.Add(new NguoiDung
                    {
                        MaNguoiDung = "USR007",
                        TenDangNhap = "manager_lock",
                        MatKhauHash = DefaultPasswordHash,
                        HoTen = "Manager Test Khóa Brute-force",
                        Email = "mgr.lock@eventhub.vn",
                        SoDienThoai = "0918765434",
                        MaNhomQuyen = "MANAGER",
                        TrangThai = "Khóa tạm thời",
                        SoLanDangNhapSai = 5,
                        KhoaDenKhi = DateTime.Now.AddMinutes(15),
                        LanDoiMatKhauGanNhat = DateTime.Now
                    });
                    hasChanged = true;
                }
                else
                {
                    // Luôn duy trì trạng thái khóa cho tài khoản kiểm thử manager_lock
                    if (!mgrLock.KhoaDenKhi.HasValue || mgrLock.KhoaDenKhi.Value <= DateTime.Now)
                    {
                        mgrLock.TrangThai = "Khóa tạm thời";
                        mgrLock.SoLanDangNhapSai = 5;
                        mgrLock.KhoaDenKhi = DateTime.Now.AddMinutes(15);
                        hasChanged = true;
                    }
                }

                var mgrInactive = context.NguoiDungs.FirstOrDefault(u => u.TenDangNhap == "mgr_inactive");
                if (mgrInactive == null)
                {
                    context.NguoiDungs.Add(new NguoiDung
                    {
                        MaNguoiDung = "USR008",
                        TenDangNhap = "mgr_inactive",
                        MatKhauHash = DefaultPasswordHash,
                        HoTen = "Quản lý Đã nghỉ việc",
                        Email = "mgr.off@eventhub.vn",
                        SoDienThoai = "0918765435",
                        MaNhomQuyen = "MANAGER",
                        TrangThai = "Khóa vĩnh viễn",
                        SoLanDangNhapSai = 0,
                        LanDoiMatKhauGanNhat = DateTime.Now
                    });
                    hasChanged = true;
                }
                else if (mgrInactive.TrangThai != "Khóa vĩnh viễn")
                {
                    mgrInactive.TrangThai = "Khóa vĩnh viễn";
                    hasChanged = true;
                }

                if (hasChanged)
                {
                    context.SaveChanges();
                }
            }
            catch
            {
                // Bỏ qua lỗi seed khi CSDL đang bận hoặc chạy test Mock/In-Memory
            }
        }
    }
}
