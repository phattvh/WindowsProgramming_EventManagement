USE EventManagementDB;
GO

-- seed: NhomQuyen --
DELETE FROM dbo.PhanQuyen;
DELETE FROM dbo.NguoiDung;
DELETE FROM dbo.NhomQuyen;
GO

INSERT INTO dbo.NhomQuyen (MaNhomQuyen, TenNhomQuyen, MoTa) VALUES
('ADMIN',   N'Quản trị viên',   N'Toàn quyền quản trị hệ thống, quản lý tài khoản người dùng, phân quyền và cấu hình an ninh.'),
('MANAGER', N'Quản lý sự kiện', N'Quản lý vòng đời sự kiện, danh mục địa điểm, diễn giả, lịch trình, nhân sự và dự toán ngân sách.'),
('STAFF',   N'Nhân viên',       N'Thực thi tác nghiệp: tiếp nhận đăng ký, bán vé, soát vé check-in webcam và ghi nhận khảo sát.');
GO

-- seed: ChucNang --
DELETE FROM dbo.ChucNang;
GO

INSERT INTO dbo.ChucNang (MaChucNang, TenChucNang, MaForm) VALUES
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
('UC18', N'Quản lý khảo sát & đánh giá phản hồi',   'F18'),
('UC19', N'Báo cáo thống kê & Xuất Excel / PDF',    'F19'),
('UC20', N'Cấu hình hệ thống & Ma trận phân quyền', 'F20'),
('UC21', N'Đổi mật khẩu cá nhân',                   'UC21');
GO

-- seed: PhanQuyen --
INSERT INTO dbo.PhanQuyen (MaNhomQuyen, MaChucNang, CoQuyen)
SELECT 'ADMIN', MaChucNang, 1 FROM dbo.ChucNang;
GO

INSERT INTO dbo.PhanQuyen (MaNhomQuyen, MaChucNang, CoQuyen)
SELECT 'MANAGER', MaChucNang, 
       CASE WHEN MaChucNang IN ('UC03', 'UC20') THEN 0 ELSE 1 END
FROM dbo.ChucNang;
GO

INSERT INTO dbo.PhanQuyen (MaNhomQuyen, MaChucNang, CoQuyen)
SELECT 'STAFF', MaChucNang, 
       CASE WHEN MaChucNang IN ('UC01', 'UC02', 'UC10', 'UC11', 'UC13', 'UC18', 'UC21') THEN 1 ELSE 0 END
FROM dbo.ChucNang;
GO

-- seed: NguoiDung Admin --
INSERT INTO dbo.NguoiDung 
(
    MaNguoiDung, TenDangNhap, MatKhauHash, HoTen, Email, SoDienThoai, MaNhomQuyen, TrangThai, LanDoiMatKhauGanNhat, SoLanDangNhapSai, KhoaDenKhi
)
VALUES
(
    'USR001', 
    'admin', 
    '$2a$11$Vn9HCdEIZew5LLR7UcYHH.0eTE9n3Jj928xQ7TJq8mQauMf9sOe0y', 
    N'Quản trị viên Hệ thống', 
    'admin@eventmanagement.vn', 
    '0901234567', 
    'ADMIN', 
    N'Hoạt động', 
    GETDATE(), 
    0, 
    NULL
);
GO
