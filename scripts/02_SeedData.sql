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
       CASE WHEN MaChucNang IN ('UC01', 'UC02', 'UC10', 'UC11', 'UC13', 'UC21') THEN 1 ELSE 0 END
FROM dbo.ChucNang;
GO

-- seed: NguoiDung (3 tai khoan mau: admin, manager1, staff1) --
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
),
(
    'USR002', 
    'manager1', 
    '$2a$11$Vn9HCdEIZew5LLR7UcYHH.0eTE9n3Jj928xQ7TJq8mQauMf9sOe0y', 
    N'Trần Minh Tâm (Trưởng BTC)', 
    'tam.tran@eventhub.vn', 
    '0918765432', 
    'MANAGER', 
    N'Hoạt động', 
    GETDATE(), 
    0, 
    NULL
),
(
    'USR003', 
    'staff1', 
    '$2a$11$Vn9HCdEIZew5LLR7UcYHH.0eTE9n3Jj928xQ7TJq8mQauMf9sOe0y', 
    N'Lê Thị Hương (Soát vé)', 
    'huong.le@eventhub.vn', 
    '0987112233', 
    'STAFF', 
    N'Hoạt động', 
    GETDATE(), 
    0, 
    NULL
);
GO

-- seed: DuLieuMau --
INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES
('LSK01', N'Hội thảo Công nghệ', N'Các sự kiện chia sẻ kiến thức chuyên đề IT, AI và phần mềm.'),
('LSK02', N'Đại nhạc hội', N'Sự kiện âm nhạc ngoài trời quy mô lớn.'),
('LSK03', N'Triển lãm Thương mại', N'Không gian trưng bày, kết nối giao thương giữa các doanh nghiệp.');
GO

INSERT INTO dbo.DiaDiem (MaDiaDiem, TenDiaDiem, DiaChi, SucChuaToiDa, GhiChu) VALUES
('DD001', N'Trung tâm Hội nghị White Palace', N'194 Hoàng Văn Thụ, Phường 9, Phú Nhuận, TP.HCM', 1500, N'Hội trường Grand Ballroom đầy đủ màn hình LED và âm thanh P.A.'),
('DD002', N'Nhà thi đấu Phú Thọ', N'1 Lữ Gia, Phường 15, Quận 11, TP.HCM', 5000, N'Sức chứa lớn, phù hợp đại nhạc hội hoặc triển lãm công nghệ.');
GO

INSERT INTO dbo.DienGia (MaDienGia, HoTen, ChucVu, DonViCongTac, Email, SoDienThoai, TieuSu, HinhAnh) VALUES
('DG001', N'TS. Nguyễn Văn Hùng', N'Chuyên gia Trí tuệ nhân tạo', N'Viện Nghiên cứu Đổi mới Sáng tạo', 'hung.nguyen@ai-institute.vn', '0912345678', N'Hơn 15 năm nghiên cứu Machine Learning và GenAI.', 'dg01_nguyen_van_an.jpg'),
('DG002', N'ThS. Lê Hoàng Mai', N'Giám đốc Kỹ thuật (CTO)', N'TechCorp Asia', 'mai.le@techcorp.com', '0987654321', N'Chuyên gia kiến trúc hệ thống phân tán và bảo mật đám mây.', 'dg02_tran_thi_bich.jpg');
GO

INSERT INTO dbo.NhanSu (MaNhanSu, HoTen, SoDienThoai, Email, ChucVu) VALUES
('NS001', N'Trần Minh Tâm', '0933111222', 'tam.tran@eventops.vn', N'Trưởng nhóm điều phối'),
('NS002', N'Lê Thị Thu Thảo', '0944333444', 'thao.le@eventops.vn', N'Điều phối viên Check-in');
GO

INSERT INTO dbo.NhaTaiTro (MaNhaTaiTro, TenToChuc, NguoiDaiDien, Email, SoDienThoai, DiaChi) VALUES
('NTT001', N'Tập đoàn Viễn thông & Công nghệ VietTech', N'Ông Phạm Nhật Long', 'contact@viettech.vn', '0283899999', N'Tòa nhà VietTech Tower, Quận 1, TP.HCM'),
('NTT002', N'Ngân hàng Số FinPay', N'Bà Võ Kim Oanh', 'partnership@finpay.vn', '0283788888', N'Khu Đô thị Sala, TP. Thủ Đức, TP.HCM');
GO

INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES
('EVT202601', N'Hội nghị Công nghệ Vietnam Tech Summit 2026', 'LSK01', 'DD001', '2026-12-15 08:00:00', '2026-12-15 17:30:00', N'Mở đăng ký', N'Sự kiện công nghệ thường niên quy tụ hơn 1000 chuyên gia, kỹ sư và lãnh đạo công nghệ.', 'banner_techsummit2026.png', 'USR001');
GO

INSERT INTO dbo.HangVe (MaHangVe, MaSuKien, TenHangVe, GiaVe, SoLuongPhatHanh, SoLuongDaBan, ThoiGianMoBan, ThoiGianDongBan) VALUES
('HV001', 'EVT202601', N'Standard',   200000, 800, 159, '2026-11-01 00:00:00', '2026-12-14 23:59:59'),
('HV002', 'EVT202601', N'VIP',        800000, 200, 50,  '2026-11-01 00:00:00', '2026-12-14 23:59:59'),
('HV005', 'EVT202601', N'Early Bird', 150000, 150, 150, '2026-10-15 00:00:00', '2026-10-31 23:59:59');
GO

INSERT INTO dbo.LichTrinh (MaLichTrinh, MaSuKien, TenTietMuc, ThoiGianBatDau, ThoiGianKetThuc, MaDienGia, DiaDiemChiTiet, GhiChu) VALUES
('LT001', 'EVT202601', N'Khai mạc & Keynote: Xu hướng GenAI & Agentic Coding 2026', '2026-12-15 08:30:00', '2026-12-15 10:00:00', 'DG001', N'Hội trường chính Grand Ballroom', N'Trình chiếu keynote khai mạc'),
('LT002', 'EVT202601', N'Tọa đàm: Bảo mật thông tin & Kiến trúc Cloud-Native', '2026-12-15 10:15:00', '2026-12-15 11:45:00', 'DG002', N'Phòng Hội thảo 2', N'Phiên hỏi đáp Q&A cùng khán giả');
GO

INSERT INTO dbo.ChiTietTaiTro (MaTaiTro, MaSuKien, MaNhaTaiTro, GoiTaiTro, GiaTriTaiTro, HinhThucTaiTro, GhiChu) VALUES
('TT001', 'EVT202601', 'NTT001', N'Kim Cương', 150000000, N'Tiền mặt', N'Tài trợ độc quyền tiệc Gala Dinner'),
('TT002', 'EVT202601', 'NTT002', N'Vàng',       60000000,  N'Tiền mặt', N'Tài trợ toàn bộ hệ thống quà tặng công nghệ');
GO
