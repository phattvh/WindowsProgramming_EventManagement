-- ===================================================================================
-- TẬP TIN THIẾT LẬP MÔI TRƯỜNG KIỂM THỬ: TÀI KHOẢN MẪU & PHÂN QUYỀN (TaiKhoanMau.sql)
-- DỰ ÁN: PHẦN MỀM QUẢN LÝ SỰ KIỆN (EventManagement)
-- MỤC ĐÍCH: Khởi tạo đầy đủ các nhóm quyền, phân quyền chức năng và danh sách tài khoản
--          kiểm thử với các vai trò (ADMIN, MANAGER, STAFF) và trạng thái khác nhau.
-- ===================================================================================

USE EventManagementDB;
GO

SET NOCOUNT ON;

PRINT N'>>> BẮT ĐẦU CẤU HÌNH TÀI KHOẢN & PHÂN QUYỀN KIỂM THỬ...';

-- -----------------------------------------------------------------------------------
-- 1. CẬP NHẬT DANH MỤC NHÓM QUYỀN (NhomQuyen)
-- -----------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.NhomQuyen WHERE MaNhomQuyen = 'ADMIN')
    INSERT INTO dbo.NhomQuyen (MaNhomQuyen, TenNhomQuyen, MoTa)
    VALUES ('ADMIN', N'Quản trị viên', N'Toàn quyền quản trị hệ thống, quản lý tài khoản người dùng, phân quyền và cấu hình an ninh.');

IF NOT EXISTS (SELECT 1 FROM dbo.NhomQuyen WHERE MaNhomQuyen = 'MANAGER')
    INSERT INTO dbo.NhomQuyen (MaNhomQuyen, TenNhomQuyen, MoTa)
    VALUES ('MANAGER', N'Quản lý sự kiện', N'Quản lý vòng đời sự kiện, danh mục địa điểm, diễn giả, lịch trình, nhân sự và dự toán ngân sách.');

IF NOT EXISTS (SELECT 1 FROM dbo.NhomQuyen WHERE MaNhomQuyen = 'STAFF')
    INSERT INTO dbo.NhomQuyen (MaNhomQuyen, TenNhomQuyen, MoTa)
    VALUES ('STAFF', N'Nhân viên', N'Thực thi tác nghiệp: tiếp nhận đăng ký, bán vé, soát vé check-in webcam.');

-- -----------------------------------------------------------------------------------
-- 2. CẬP NHẬT TOÀN BỘ 21 CHỨC NĂNG HỆ THỐNG (ChucNang)
-- -----------------------------------------------------------------------------------
MERGE dbo.ChucNang AS target
USING (VALUES
    ('UC01', N'Đăng nhập hệ thống',                     'F01'),
    ('UC02', N'Xem Dashboard / Trang chủ',               'F02'),
    ('UC03', N'Quản lý người dùng & Khóa tài khoản',    'F03'),
    ('UC04', N'Quản lý danh sách sự kiện',              'F04'),
    ('UC05', N'Xem & Điều phối chi tiết sự kiện (9 Tabs)','F05'),
    ('UC06', N'Quản lý loại sự kiện',                   'F06'),
    ('UC07', N'Quản lý địa điểm & hội trường',          'F07'),
    ('UC08', N'Quản lý lịch trình & timeline',          'F08'),
    ('UC09', N'Quản lý diễn giả & khách mời',           'F09'),
    ('UC10', N'Quản lý hồ sơ người tham gia',           'F10'),
    ('UC11', N'Tiếp nhận đăng ký & Bán vé',             'F11'),
    ('UC12', N'Quản lý các hạng vé & mở bán',           'F12'),
    ('UC13', N'Soát vé Check-in (Webcam QR Code)',      'F13'),
    ('UC14', N'Quản lý danh mục nhân sự',               'F14'),
    ('UC15', N'Phân công ca trực nhân sự',              'F15'),
    ('UC16', N'Quản lý nhà tài trợ & gói tài trợ',      'F16'),
    ('UC17', N'Quản lý ngân sách & chi phí phát sinh',  'F17'),
    ('UC19', N'Báo cáo thống kê & Xuất Excel / PDF',    'F19'),
    ('UC20', N'Cấu hình hệ thống & Ma trận phân quyền', 'F20'),
    ('UC21', N'Đổi mật khẩu cá nhân',                   'UC21')
) AS source (MaChucNang, TenChucNang, MaForm)
ON target.MaChucNang = source.MaChucNang
WHEN MATCHED THEN
    UPDATE SET TenChucNang = source.TenChucNang, MaForm = source.MaForm
WHEN NOT MATCHED THEN
    INSERT (MaChucNang, TenChucNang, MaForm)
    VALUES (source.MaChucNang, source.TenChucNang, source.MaForm);

-- -----------------------------------------------------------------------------------
-- 3. CẤU HÌNH MA TRẬN PHÂN QUYỀN MẶC ĐỊNH (PhanQuyen)
-- -----------------------------------------------------------------------------------
DELETE FROM dbo.PhanQuyen;

-- ADMIN: Có toàn bộ 21 quyền
INSERT INTO dbo.PhanQuyen (MaNhomQuyen, MaChucNang, CoQuyen)
SELECT 'ADMIN', MaChucNang, 1 FROM dbo.ChucNang;

-- MANAGER: Không có quyền F03 (Quản trị User) và F20 (Cấu hình quyền), còn lại có đủ
INSERT INTO dbo.PhanQuyen (MaNhomQuyen, MaChucNang, CoQuyen)
SELECT 'MANAGER', MaChucNang,
       CASE WHEN MaChucNang IN ('UC03', 'UC20') THEN 0 ELSE 1 END
FROM dbo.ChucNang;

-- STAFF: Chỉ có quyền F01, F02, F10, F11, F13, UC21
INSERT INTO dbo.PhanQuyen (MaNhomQuyen, MaChucNang, CoQuyen)
SELECT 'STAFF', MaChucNang,
       CASE WHEN MaChucNang IN ('UC01', 'UC02', 'UC10', 'UC11', 'UC13', 'UC21') THEN 1 ELSE 0 END
FROM dbo.ChucNang;

-- -----------------------------------------------------------------------------------
-- 4. DANH SÁCH TÀI KHOẢN KIỂM THỬ (NguoiDung)
-- Mật khẩu chung chuẩn: Admin@123 (BCrypt cost 11: $2a$11$Vn9HCdEIZew5LLR7UcYHH.0eTE9n3Jj928xQ7TJq8mQauMf9sOe0y)
-- Hệ thống hỗ trợ đăng nhập bằng: "Admin@123", "123456" hoặc chính tên đăng nhập.
-- Mã PIN / OTP xác thực đổi mật khẩu chung: 123456
-- -----------------------------------------------------------------------------------

DECLARE @MatKhauBCrypt VARCHAR(255) = '$2a$11$Vn9HCdEIZew5LLR7UcYHH.0eTE9n3Jj928xQ7TJq8mQauMf9sOe0y';

MERGE dbo.NguoiDung AS target
USING (VALUES
    -- 1. NHÓM ADMIN (Quản trị hệ thống)
    ('USR001', 'admin',         @MatKhauBCrypt, N'Quản trị viên Hệ thống (Chính)', 'admin@eventmanagement.vn',      '0901234567', 'ADMIN',   N'Hoạt động',      0, NULL),
    ('USR004', 'admin2',        @MatKhauBCrypt, N'Quản trị viên Dự phòng',         'admin2@eventmanagement.vn',     '0901234568', 'ADMIN',   N'Hoạt động',      0, NULL),
    ('USR005', 'admin_lock',    @MatKhauBCrypt, N'Admin Test Khóa Brute-force',    'admin.lock@eventmanagement.vn', '0901234569', 'ADMIN',   N'Khóa tạm thời',  5, DATEADD(MINUTE, 30, GETDATE())),

    -- 2. NHÓM MANAGER (Quản lý & Điều phối sự kiện)
    ('USR002', 'manager1',      @MatKhauBCrypt, N'Trần Minh Tâm (Trưởng BTC)',     'tam.tran@eventhub.vn',          '0918765432', 'MANAGER', N'Hoạt động',      0, NULL),
    ('USR006', 'manager2',      @MatKhauBCrypt, N'Nguyễn Hoàng Nam (Phó BTC)',     'nam.nguyen@eventhub.vn',        '0918765433', 'MANAGER', N'Hoạt động',      0, NULL),
    ('USR007', 'manager_lock',  @MatKhauBCrypt, N'Manager Test Khóa Brute-force',  'mgr.lock@eventhub.vn',          '0918765434', 'MANAGER', N'Khóa tạm thời',  5, DATEADD(MINUTE, 15, GETDATE())),
    ('USR008', 'mgr_inactive',  @MatKhauBCrypt, N'Quản lý Đã nghỉ việc',           'mgr.off@eventhub.vn',           '0918765435', 'MANAGER', N'Khóa vĩnh viễn', 0, NULL),

    -- 3. NHÓM STAFF (Nhân viên tác nghiệp hiện trường)
    ('USR003', 'staff1',        @MatKhauBCrypt, N'Lê Thị Hương (Soát vé & Bán vé)','huong.le@eventhub.vn',          '0987112233', 'STAFF',   N'Hoạt động',      0, NULL),
    ('USR009', 'staff2',        @MatKhauBCrypt, N'Phạm Tuấn Anh (Hỗ trợ Check-in)','tuananh.pham@eventhub.vn',      '0987112234', 'STAFF',   N'Hoạt động',      0, NULL),
    ('USR010', 'staff_lock',    @MatKhauBCrypt, N'Staff Test Khóa Đăng nhập',      'staff.lock@eventhub.vn',        '0987112235', 'STAFF',   N'Khóa tạm thời',  5, DATEADD(MINUTE, 5, GETDATE())),
    ('USR011', 'staff_noperm',  @MatKhauBCrypt, N'Nhân viên Giới hạn Quyền',       'staff.noperm@eventhub.vn',      '0987112236', 'STAFF',   N'Hoạt động',      0, NULL)
) AS source (MaNguoiDung, TenDangNhap, MatKhauHash, HoTen, Email, SoDienThoai, MaNhomQuyen, TrangThai, SoLanDangNhapSai, KhoaDenKhi)
ON target.TenDangNhap = source.TenDangNhap
WHEN MATCHED THEN
    UPDATE SET 
        MatKhauHash = source.MatKhauHash,
        HoTen = source.HoTen,
        Email = source.Email,
        SoDienThoai = source.SoDienThoai,
        MaNhomQuyen = source.MaNhomQuyen,
        TrangThai = source.TrangThai,
        SoLanDangNhapSai = source.SoLanDangNhapSai,
        KhoaDenKhi = source.KhoaDenKhi,
        LanDoiMatKhauGanNhat = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (MaNguoiDung, TenDangNhap, MatKhauHash, HoTen, Email, SoDienThoai, MaNhomQuyen, TrangThai, LanDoiMatKhauGanNhat, SoLanDangNhapSai, KhoaDenKhi)
    VALUES (source.MaNguoiDung, source.TenDangNhap, source.MatKhauHash, source.HoTen, source.Email, source.SoDienThoai, source.MaNhomQuyen, source.TrangThai, GETDATE(), source.SoLanDangNhapSai, source.KhoaDenKhi);

PRINT N'>>> HOÀN THÀNH THIẾT LẬP TÀI KHOẢN & PHÂN QUYỀN MẪU! (Tổng cộng 11 tài khoản)';
GO
