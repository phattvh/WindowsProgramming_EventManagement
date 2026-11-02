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
