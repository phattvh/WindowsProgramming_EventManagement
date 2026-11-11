-- ==============================================================================
-- DỰ ÁN: HỆ THỐNG QUẢN LÝ SỰ KIỆN (EVENT MANAGEMENT SYSTEM)
-- SCRIPT 04: NẠP DỮ LIỆU MẪU TOÀN DIỆN CHO NGÀY BẢO VỆ ĐỒ ÁN (DAY 9 / TASK T3-25)
-- Chuẩn bị cho 6 Kịch bản Kiểm thử Tích hợp End-to-End (E2E) Ngày 10
-- Tương thích 100% Schema: 01_Database_Schema.sql
-- ==============================================================================

USE EventManagementDB;
GO

PRINT N'==============================================================================';
PRINT N'>>> BẮT ĐẦU NẠP DỮ LIỆU MẪU ĐỒ ÁN TOÀN DIỆN (04_FinalDemoData.sql)...';
PRINT N'==============================================================================';
GO

-- 1. DANH MỤC NHÓM QUYỀN (NhomQuyen)
IF NOT EXISTS (SELECT 1 FROM dbo.NhomQuyen WHERE MaNhomQuyen = 'ADMIN')
    INSERT INTO dbo.NhomQuyen (MaNhomQuyen, TenNhomQuyen, MoTa) VALUES
    ('ADMIN',   N'Quản trị viên',   N'Toàn quyền quản trị hệ thống, quản lý tài khoản người dùng, phân quyền và cấu hình an ninh.');

IF NOT EXISTS (SELECT 1 FROM dbo.NhomQuyen WHERE MaNhomQuyen = 'MANAGER')
    INSERT INTO dbo.NhomQuyen (MaNhomQuyen, TenNhomQuyen, MoTa) VALUES
    ('MANAGER', N'Quản lý sự kiện', N'Quản lý vòng đời sự kiện, danh mục địa điểm, diễn giả, lịch trình, nhân sự và dự toán ngân sách.');

IF NOT EXISTS (SELECT 1 FROM dbo.NhomQuyen WHERE MaNhomQuyen = 'STAFF')
    INSERT INTO dbo.NhomQuyen (MaNhomQuyen, TenNhomQuyen, MoTa) VALUES
    ('STAFF',   N'Nhân viên',       N'Thực thi tác nghiệp: tiếp nhận đăng ký, bán vé, soát vé check-in webcam và ghi nhận khảo sát.');

PRINT N'-> Đã đảm bảo 3 nhóm quyền cốt lõi (ADMIN, MANAGER, STAFF).';
GO

-- 2. DANH MỤC CHỨC NĂNG (ChucNang)
MERGE dbo.ChucNang AS target
USING (VALUES
    ('UC01', N'Đăng nhập hệ thống',                      'F01'),
    ('UC02', N'Xem Dashboard / Trang chủ',                'F02'),
    ('UC03', N'Quản lý người dùng & Khóa tài khoản',     'F03'),
    ('UC04', N'Quản lý danh sách sự kiện',               'F04'),
    ('UC05', N'Xem & Điều phối chi tiết sự kiện (9 Tabs)','F05'),
    ('UC06', N'Quản lý loại sự kiện',                    'F06'),
    ('UC07', N'Quản lý địa điểm & hội trường',           'F07'),
    ('UC08', N'Quản lý lịch trình & timeline',           'F08'),
    ('UC09', N'Quản lý diễn giả & khách mời',            'F09'),
    ('UC10', N'Quản lý hồ sơ người tham gia',            'F10'),
    ('UC11', N'Tiếp nhận đăng ký & Bán vé',              'F11'),
    ('UC12', N'Quản lý các hạng vé & mở bán',            'F12'),
    ('UC13', N'Soát vé Check-in (Webcam QR Code)',       'F13'),
    ('UC14', N'Quản lý danh mục nhân sự',                'F14'),
    ('UC15', N'Phân công ca trực nhân sự',               'F15'),
    ('UC16', N'Quản lý nhà tài trợ & gói tài trợ',       'F16'),
    ('UC17', N'Quản lý ngân sách & chi phí phát sinh',   'F17'),
    ('UC19', N'Báo cáo thống kê & Xuất Excel / PDF',     'F19'),
    ('UC20', N'Cấu hình hệ thống & Ma trận phân quyền',  'F20'),
    ('UC21', N'Đổi mật khẩu cá nhân',                    'UC21')
) AS source (MaChucNang, TenChucNang, MaForm)
ON target.MaChucNang = source.MaChucNang
WHEN MATCHED THEN
    UPDATE SET TenChucNang = source.TenChucNang, MaForm = source.MaForm
WHEN NOT MATCHED THEN
    INSERT (MaChucNang, TenChucNang, MaForm)
    VALUES (source.MaChucNang, source.TenChucNang, source.MaForm);

PRINT N'-> Đã đồng bộ 21 chức năng Use Cases (UC01 - UC21).';
GO

-- 3. MA TRẬN PHÂN QUYỀN (PhanQuyen)
INSERT INTO dbo.PhanQuyen (MaNhomQuyen, MaChucNang, CoQuyen)
SELECT 'ADMIN', c.MaChucNang, 1
FROM dbo.ChucNang c
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.PhanQuyen p 
    WHERE p.MaNhomQuyen = 'ADMIN' AND p.MaChucNang = c.MaChucNang
);

INSERT INTO dbo.PhanQuyen (MaNhomQuyen, MaChucNang, CoQuyen)
SELECT 'MANAGER', c.MaChucNang, 1
FROM dbo.ChucNang c
WHERE c.MaChucNang IN ('UC01','UC02','UC04','UC05','UC06','UC07','UC08','UC09','UC10','UC11','UC12','UC13','UC14','UC15','UC16','UC17','UC19','UC21')
  AND NOT EXISTS (
    SELECT 1 FROM dbo.PhanQuyen p 
    WHERE p.MaNhomQuyen = 'MANAGER' AND p.MaChucNang = c.MaChucNang
);

INSERT INTO dbo.PhanQuyen (MaNhomQuyen, MaChucNang, CoQuyen)
SELECT 'STAFF', c.MaChucNang, 1
FROM dbo.ChucNang c
WHERE c.MaChucNang IN ('UC01','UC02','UC08','UC10','UC11','UC13','UC21')
  AND NOT EXISTS (
    SELECT 1 FROM dbo.PhanQuyen p 
    WHERE p.MaNhomQuyen = 'STAFF' AND p.MaChucNang = c.MaChucNang
);

PRINT N'-> Đã hoàn thiện ma trận phân quyền 3 tầng người dùng.';
GO

-- 4. TÀI KHOẢN NGƯỜI DÙNG DEMO (NguoiDung - BCrypt Hash)
IF NOT EXISTS (SELECT 1 FROM dbo.NguoiDung WHERE TenDangNhap = 'admin')
    INSERT INTO dbo.NguoiDung (MaNguoiDung, TenDangNhap, MatKhauHash, HoTen, Email, SoDienThoai, MaNhomQuyen, TrangThai, SoLanDangNhapSai) VALUES
    ('USR001', 'admin', '$2a$11$Vn9HCdEIZew5LLR7UcYHH.0eTE9n3Jj928xQ7TJq8mQauMf9sOe0y', N'Quản trị viên Hệ thống', 'admin@eventhub.vn', '0909123456', 'ADMIN', N'Hoạt động', 0);

IF NOT EXISTS (SELECT 1 FROM dbo.NguoiDung WHERE TenDangNhap = 'manager1')
    INSERT INTO dbo.NguoiDung (MaNguoiDung, TenDangNhap, MatKhauHash, HoTen, Email, SoDienThoai, MaNhomQuyen, TrangThai, SoLanDangNhapSai) VALUES
    ('USR002', 'manager1', '$2a$11$Vn9HCdEIZew5LLR7UcYHH.0eTE9n3Jj928xQ7TJq8mQauMf9sOe0y', N'Trần Minh Tâm (Trưởng BTC)', 'tam.tran@eventhub.vn', '0918765432', 'MANAGER', N'Hoạt động', 0);

IF NOT EXISTS (SELECT 1 FROM dbo.NguoiDung WHERE TenDangNhap = 'staff1')
    INSERT INTO dbo.NguoiDung (MaNguoiDung, TenDangNhap, MatKhauHash, HoTen, Email, SoDienThoai, MaNhomQuyen, TrangThai, SoLanDangNhapSai) VALUES
    ('USR003', 'staff1', '$2a$11$Vn9HCdEIZew5LLR7UcYHH.0eTE9n3Jj928xQ7TJq8mQauMf9sOe0y', N'Lê Thị Hương (Soát vé)', 'huong.le@eventhub.vn', '0987112233', 'STAFF', N'Hoạt động', 0);


PRINT N'-> Đã sẵn sàng 3 tài khoản người dùng mẫu (admin, manager1, staff1) phục vụ phân quyền.';
GO

-- 5. DANH MỤC LOẠI SỰ KIỆN (LoaiSuKien)
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiSuKien WHERE MaLoaiSK = 'LSK01') INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES ('LSK01', N'Hội thảo Công nghệ', N'Các hội nghị, diễn đàn công nghệ lớn có diễn giả quốc tế.');
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiSuKien WHERE MaLoaiSK = 'LSK02') INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES ('LSK02', N'Đại nhạc hội / Festival', N'Chương trình biểu diễn âm nhạc nghệ thuật quy mô lớn ngoài trời.');
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiSuKien WHERE MaLoaiSK = 'LSK03') INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES ('LSK03', N'Giải đấu Thể thao Điện tử', N'Các giải đấu eSports, thể thao cho sinh viên và giới trẻ.');
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiSuKien WHERE MaLoaiSK = 'LSK04') INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES ('LSK04', N'Hội nghị Khoa học (Symposium)', N'Diễn đàn học thuật báo cáo nghiên cứu và công bố công trình khoa học.');
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiSuKien WHERE MaLoaiSK = 'LSK05') INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES ('LSK05', N'Workshop Kỹ năng & Đào tạo', N'Các buổi thực hành chuyên môn ngắn hạn, tương tác trực tiếp.');
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiSuKien WHERE MaLoaiSK = 'LSK06') INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES ('LSK06', N'Ngày hội Việc làm (Career Fair)', N'Kết nối doanh nghiệp tuyển dụng với sinh viên và ứng viên CNTT.');
PRINT N'-> Đã cập nhật 6 loại sự kiện phong phú.';
GO

-- 6. DANH MỤC ĐỊA ĐIỂM (DiaDiem)
IF NOT EXISTS (SELECT 1 FROM dbo.DiaDiem WHERE MaDiaDiem = 'DD001') INSERT INTO dbo.DiaDiem (MaDiaDiem, TenDiaDiem, DiaChi, SucChuaToiDa, GhiChu) VALUES ('DD001', N'Trung tâm Hội nghị White Palace', N'194 Hoàng Văn Thụ, Phường 9, Phú Nhuận, TP.HCM', 1200, N'Hội trường Alpas hiện đại, hệ thống âm thanh vòm Line Array');
IF NOT EXISTS (SELECT 1 FROM dbo.DiaDiem WHERE MaDiaDiem = 'DD002') INSERT INTO dbo.DiaDiem (MaDiaDiem, TenDiaDiem, DiaChi, SucChuaToiDa, GhiChu) VALUES ('DD002', N'Hội trường Thống Nhất (Dinh Độc Lập)', N'135 Nam Kỳ Khởi Nghĩa, Bến Thành, Quận 1, TP.HCM', 800, N'Không gian lịch sử trang nghiêm, sân khấu ngoài trời rộng rãi');
IF NOT EXISTS (SELECT 1 FROM dbo.DiaDiem WHERE MaDiaDiem = 'DD003') INSERT INTO dbo.DiaDiem (MaDiaDiem, TenDiaDiem, DiaChi, SucChuaToiDa, GhiChu) VALUES ('DD003', N'Trung tâm Sự kiện GEM Center', N'08 Nguyễn Bỉnh Khiêm, Phường Đa Kao, Quận 1, TP.HCM', 1800, N'Không gian sang trọng trần cao 8m phù hợp dạ tiệc và lễ ra mắt VIP');
IF NOT EXISTS (SELECT 1 FROM dbo.DiaDiem WHERE MaDiaDiem = 'DD004') INSERT INTO dbo.DiaDiem (MaDiaDiem, TenDiaDiem, DiaChi, SucChuaToiDa, GhiChu) VALUES ('DD004', N'Trung tâm Triển lãm & Hội chợ SECC', N'799 Nguyễn Văn Linh, Phường Tân Phú, Quận 7, TP.HCM', 5000, N'Hội trường triển lãm tiêu chuẩn quốc tế quy mô lớn nhất miền Nam');
PRINT N'-> Đã cập nhật 4 địa điểm tổ chức thực tế.';
GO

-- 7. DANH SÁCH DIỄN GIẢ (DienGia)
IF NOT EXISTS (SELECT 1 FROM dbo.DienGia WHERE MaDienGia = 'DG001') INSERT INTO dbo.DienGia (MaDienGia, HoTen, ChucVu, DonViCongTac, Email, SoDienThoai, TieuSu) VALUES ('DG001', N'TS. Lê Thành Long', N'Principal AI Researcher', 'Google DeepMind', 'long.le@deepmind.com', '0901234567', N'Chuyên gia hàng đầu về mô hình ngôn ngữ lớn (LLM) và hệ thống Multi-Agent.');
IF NOT EXISTS (SELECT 1 FROM dbo.DienGia WHERE MaDienGia = 'DG002') INSERT INTO dbo.DienGia (MaDienGia, HoTen, ChucVu, DonViCongTac, Email, SoDienThoai, TieuSu) VALUES ('DG002', N'ThS. Hoàng Phương Nam', N'Solutions Architect Director', 'AWS Vietnam', 'nam.hoang@aws.com', '0912345678', N'15 năm kinh nghiệm kiến trúc hạ tầng Cloud quy mô lớn và Serverless.');
IF NOT EXISTS (SELECT 1 FROM dbo.DienGia WHERE MaDienGia = 'DG003') INSERT INTO dbo.DienGia (MaDienGia, HoTen, ChucVu, DonViCongTac, Email, SoDienThoai, TieuSu) VALUES ('DG003', N'Bà Đặng Mai Khanh', N'VP of Engineering', 'VNG Corporation', 'khanh.dang@vng.com.vn', '0923456789', N'Lãnh đạo đội ngũ kỹ thuật phát triển nền tảng Zalo với hơn 75 triệu người dùng.');
IF NOT EXISTS (SELECT 1 FROM dbo.DienGia WHERE MaDienGia = 'DG004') INSERT INTO dbo.DienGia (MaDienGia, HoTen, ChucVu, DonViCongTac, Email, SoDienThoai, TieuSu) VALUES ('DG004', N'PGS.TS. Trần Đình Quân', N'Trưởng Khoa CNTT', 'ĐH Bách Khoa TP.HCM', 'quan.tran@hcmut.edu.vn', '0934567890', N'Nhà khoa học uy tín với nhiều công bố quốc tế về Mạng nơ-ron và Thị giác máy tính.');
IF NOT EXISTS (SELECT 1 FROM dbo.DienGia WHERE MaDienGia = 'DG005') INSERT INTO dbo.DienGia (MaDienGia, HoTen, ChucVu, DonViCongTac, Email, SoDienThoai, TieuSu) VALUES ('DG005', N'Ông Johnathan Miller', N'Chief Security Officer', 'CyberGuard Singapore', 'j.miller@cyberguard.sg', '+6591234567', N'Chuyên gia an ninh mạng quốc tế, cố vấn bảo mật cho các định chế tài chính ASEAN.');
PRINT N'-> Đã cập nhật 5 diễn giả danh tiếng.';
GO

-- 8. DANH SÁCH SỰ KIỆN (SuKien)
IF NOT EXISTS (SELECT 1 FROM dbo.SuKien WHERE MaSuKien = 'EVT202601') INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES ('EVT202601', N'Hội nghị Công nghệ Vietnam Tech Summit 2026', 'LSK01', 'DD003', '2026-12-15 08:00:00', '2026-12-15 17:30:00', N'Đang diễn ra', N'Sự kiện thường niên lớn nhất quy tụ hơn 1,000 chuyên gia công nghệ, kỹ sư phần mềm và lãnh đạo doanh nghiệp hàng đầu.', 'tech_summit_2026.png', 'USR001');
IF NOT EXISTS (SELECT 1 FROM dbo.SuKien WHERE MaSuKien = 'EVT202602') INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES ('EVT202602', N'Workshop Thực chiến GenAI & Prompt Engineering 2026', 'LSK05', 'DD002', '2026-11-20 08:30:00', '2026-11-20 16:30:00', N'Mở đăng ký', N'Khóa đào tạo chuyên sâu hướng dẫn kỹ sư ứng dụng Gemini 1.5 Pro và Cursor xây dựng sản phẩm tự động.', 'genai_workshop.png', 'USR001');
IF NOT EXISTS (SELECT 1 FROM dbo.SuKien WHERE MaSuKien = 'EVT202603') INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES ('EVT202603', N'Hội nghị Khoa học Quốc tế về Công nghệ Phần mềm & IoT', 'LSK04', 'DD004', '2026-12-28 09:00:00', '2026-12-29 17:00:00', N'Dự kiến', N'Diễn đàn học thuật công bố các nghiên cứu mới về Clean Architecture, Microservices và Hệ thống phân tán.', 'iot_symposium.png', 'USR001');
IF NOT EXISTS (SELECT 1 FROM dbo.SuKien WHERE MaSuKien = 'EVT202604') INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES ('EVT202604', N'Đại nhạc hội Sinh viên Youth Fest 2026', 'LSK02', 'DD001', '2026-10-15 18:00:00', '2026-10-15 22:30:00', N'Đã kết thúc', N'Chương trình nghệ thuật chào đón tân sinh viên với dàn nghệ sĩ trẻ thịnh hành.', 'youth_fest.png', 'USR001');
IF NOT EXISTS (SELECT 1 FROM dbo.SuKien WHERE MaSuKien = 'EVT202606') INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES ('EVT202606', N'Ngày hội Việc làm & Kết nối Doanh nghiệp IT Career Fair', 'LSK06', 'DD001', '2026-12-30 08:00:00', '2026-12-30 16:00:00', N'Dự kiến', N'Sự kiện tuyển dụng quy tụ 40 công ty công nghệ lớn (dùng kiểm thử xóa sự kiện BR-02).', 'career_fair.png', 'USR001');
IF NOT EXISTS (SELECT 1 FROM dbo.SuKien WHERE MaSuKien = 'EVT202607') INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES ('EVT202607', N'Triển lãm Công nghệ Xanh GreenTech Expo', 'LSK04', 'DD004', '2026-11-05 08:00:00', '2026-11-07 17:00:00', N'Hủy', N'Sự kiện đã bị hủy theo kế hoạch kiểm thử trạng thái.', 'greentech.png', 'USR001');
PRINT N'-> Đã nạp danh sách sự kiện đầy đủ các trạng thái thực tế.';
GO

-- 9. DANH MỤC HẠNG VÉ (HangVe)
IF NOT EXISTS (SELECT 1 FROM dbo.HangVe WHERE MaHangVe = 'HV001') INSERT INTO dbo.HangVe (MaHangVe, MaSuKien, TenHangVe, GiaVe, SoLuongPhatHanh, SoLuongDaBan, ThoiGianMoBan, ThoiGianDongBan) VALUES ('HV001', 'EVT202601', N'Vé Tiêu chuẩn (Standard)', 200000, 800, 150, '2026-10-01 00:00:00', '2026-12-14 23:59:59');
IF NOT EXISTS (SELECT 1 FROM dbo.HangVe WHERE MaHangVe = 'HV002') INSERT INTO dbo.HangVe (MaHangVe, MaSuKien, TenHangVe, GiaVe, SoLuongPhatHanh, SoLuongDaBan, ThoiGianMoBan, ThoiGianDongBan) VALUES ('HV002', 'EVT202601', N'Vé VIP (Full-access & Tiệc trưa)', 800000, 200, 50, '2026-10-01 00:00:00', '2026-12-14 23:59:59');
IF NOT EXISTS (SELECT 1 FROM dbo.HangVe WHERE MaHangVe = 'HV003') INSERT INTO dbo.HangVe (MaHangVe, MaSuKien, TenHangVe, GiaVe, SoLuongPhatHanh, SoLuongDaBan, ThoiGianMoBan, ThoiGianDongBan) VALUES ('HV003', 'EVT202602', N'Vé Thực hành Lab (Standard)', 300000, 100, 20, '2026-10-15 00:00:00', '2026-11-19 23:59:59');
IF NOT EXISTS (SELECT 1 FROM dbo.HangVe WHERE MaHangVe = 'HV004') INSERT INTO dbo.HangVe (MaHangVe, MaSuKien, TenHangVe, GiaVe, SoLuongPhatHanh, SoLuongDaBan, ThoiGianMoBan, ThoiGianDongBan) VALUES ('HV004', 'EVT202602', N'Vé VIP Mentoring 1-1', 600000, 50, 10, '2026-10-15 00:00:00', '2026-11-19 23:59:59');
PRINT N'-> Đã nạp 4 hạng vé cho sự kiện Tech Summit và Workshop GenAI.';
GO

-- 10. HỒ SƠ NGƯỜI THAM GIA (NguoiThamGia)
IF NOT EXISTS (SELECT 1 FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG001') INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, GioiTinh, Email, SoDienThoai, DonViCongTac) VALUES ('NTG001', N'Nguyễn Văn An',   N'Nam', 'an.nguyen@gmail.com',       '0912345678', N'Đại học Bách Khoa TP.HCM');
IF NOT EXISTS (SELECT 1 FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG002') INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, GioiTinh, Email, SoDienThoai, DonViCongTac) VALUES ('NTG002', N'Trần Thị Bích',   N'Nữ',  'bich.tran@fpt.com',         '0987654321', N'FPT Software');
IF NOT EXISTS (SELECT 1 FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG003') INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, GioiTinh, Email, SoDienThoai, DonViCongTac) VALUES ('NTG003', N'Lê Hoàng Nam',    N'Nam', 'nam.le@techcorp.vn',        '0903112233', N'TechCorp VN');
IF NOT EXISTS (SELECT 1 FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG004') INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, GioiTinh, Email, SoDienThoai, DonViCongTac) VALUES ('NTG004', N'Phạm Minh Tuấn',  N'Nam', 'tuan.pham@student.edu.vn',  '0934556677', N'ĐH Công nghệ Thông tin - ĐHQG');
IF NOT EXISTS (SELECT 1 FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG005') INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, GioiTinh, Email, SoDienThoai, DonViCongTac) VALUES ('NTG005', N'Hoàng Thu Thảo',  N'Nữ',  'thao.hoang@vng.com.vn',     '0977889900', N'VNG Corporation');
PRINT N'-> Đã nạp 5 hồ sơ người tham gia đại biểu mẫu.';
GO

-- 11. ĐƠN ĐĂNG KÝ VÉ (DangKy)
IF NOT EXISTS (SELECT 1 FROM dbo.DangKy WHERE MaDangKy = 'DK001') INSERT INTO dbo.DangKy (MaDangKy, MaSuKien, MaNguoiThamGia, MaHangVe, NgayDangKy, SoLuong, TongTien, TrangThaiThanhToan, PhuongThucThanhToan) VALUES ('DK001', 'EVT202601', 'NTG001', 'HV001', '2026-11-01 09:30:00', 2, 400000, N'Đã thanh toán', N'Chuyển khoản QR VNPay');
IF NOT EXISTS (SELECT 1 FROM dbo.DangKy WHERE MaDangKy = 'DK002') INSERT INTO dbo.DangKy (MaDangKy, MaSuKien, MaNguoiThamGia, MaHangVe, NgayDangKy, SoLuong, TongTien, TrangThaiThanhToan, PhuongThucThanhToan) VALUES ('DK002', 'EVT202601', 'NTG002', 'HV002', '2026-11-02 14:15:00', 1, 800000, N'Đã thanh toán', N'Thẻ tín dụng Visa');
IF NOT EXISTS (SELECT 1 FROM dbo.DangKy WHERE MaDangKy = 'DK003') INSERT INTO dbo.DangKy (MaDangKy, MaSuKien, MaNguoiThamGia, MaHangVe, NgayDangKy, SoLuong, TongTien, TrangThaiThanhToan, PhuongThucThanhToan) VALUES ('DK003', 'EVT202602', 'NTG003', 'HV003', '2026-11-03 10:00:00', 1, 300000, N'Đã thanh toán', N'Ví điện tử MoMo');
PRINT N'-> Đã nạp 3 đơn đặt vé mẫu thành công.';
GO

-- 12. CHI TIẾT VÉ & MÃ QR TOKEN CHUẨN (ChiTietVe - GUID)
IF NOT EXISTS (SELECT 1 FROM dbo.ChiTietVe WHERE MaVe = 'VE-EVT202601-0001') INSERT INTO dbo.ChiTietVe (MaVe, MaDangKy, MaQR, TenNguoiThamGia, Email, TrangThaiVe) VALUES ('VE-EVT202601-0001', 'DK001', '11111111-2222-3333-4444-555555555501', N'Nguyễn Văn An',   'an.nguyen@gmail.com', N'Đã check-in');
IF NOT EXISTS (SELECT 1 FROM dbo.ChiTietVe WHERE MaVe = 'VE-EVT202601-0002') INSERT INTO dbo.ChiTietVe (MaVe, MaDangKy, MaQR, TenNguoiThamGia, Email, TrangThaiVe) VALUES ('VE-EVT202601-0002', 'DK001', '11111111-2222-3333-4444-555555555502', N'Đỗ Hữu Nghĩa',    'nghia.do@gmail.com',   N'Hợp lệ');
IF NOT EXISTS (SELECT 1 FROM dbo.ChiTietVe WHERE MaVe = 'VE-EVT202601-0003') INSERT INTO dbo.ChiTietVe (MaVe, MaDangKy, MaQR, TenNguoiThamGia, Email, TrangThaiVe) VALUES ('VE-EVT202601-0003', 'DK002', '11111111-2222-3333-4444-555555555503', N'Trần Thị Bích',   'bich.tran@fpt.com',   N'Hợp lệ');
IF NOT EXISTS (SELECT 1 FROM dbo.ChiTietVe WHERE MaVe = 'VE-EVT202602-0001') INSERT INTO dbo.ChiTietVe (MaVe, MaDangKy, MaQR, TenNguoiThamGia, Email, TrangThaiVe) VALUES ('VE-EVT202602-0001', 'DK003', '22222222-3333-4444-5555-666666666601', N'Lê Hoàng Nam',    'nam.le@techcorp.vn',  N'Hợp lệ');
PRINT N'-> Đã sinh 4 vé điện tử kèm mã QR GUID chuẩn sẵn sàng in và quét webcam.';
GO

-- 13. DỮ LIỆU SOÁT VÉ (CheckIn) - BR-12
IF NOT EXISTS (SELECT 1 FROM dbo.CheckIn WHERE MaCheckIn = 'CI001') INSERT INTO dbo.CheckIn (MaCheckIn, MaVe, ThoiGianCheckIn, NguoiThucHien, HinhThucCheckIn, GhiChu) VALUES ('CI001', 'VE-EVT202601-0001', '2026-12-15 08:15:20', 'USR003', N'Webcam QR Code', N'Đại biểu đến đúng giờ, check-in QR thành công');
PRINT N'-> Đã ghi nhận lượt điểm danh mẫu cho VE-EVT202601-0001.';
GO

-- 14. DỰ TOÁN & CHI PHÍ THỰC TẾ (ChiPhiSuKien) - THỎA MÃN BR-14 (CẢNH BÁO > 80%)
IF NOT EXISTS (SELECT 1 FROM dbo.ChiPhiSuKien WHERE MaChiPhi = 'CP001') INSERT INTO dbo.ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, SoTien, NgayChi, NguoiDuyet, GhiChu) VALUES ('CP001', 'EVT202601', N'Thuê sảnh Grand Ballroom GEM Center', 120000000, '2026-11-10 10:00:00', 'USR001', N'Đã thanh toán đợt 1 (50%)');
IF NOT EXISTS (SELECT 1 FROM dbo.ChiPhiSuKien WHERE MaChiPhi = 'CP002') INSERT INTO dbo.ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, SoTien, NgayChi, NguoiDuyet, GhiChu) VALUES ('CP002', 'EVT202601', N'Dàn âm thanh ánh sáng & màn hình LED 4K', 45000000, '2026-11-12 15:30:00', 'USR001', N'Bàn giao trước sự kiện 1 ngày');
IF NOT EXISTS (SELECT 1 FROM dbo.ChiPhiSuKien WHERE MaChiPhi = 'CP003') INSERT INTO dbo.ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, SoTien, NgayChi, NguoiDuyet, GhiChu) VALUES ('CP003', 'EVT202601', N'Tiệc Teabreak cao cấp sáng và chiều', 35000000, '2026-11-14 09:00:00', 'USR001', N'Suất phục vụ cho 1000 khách');
IF NOT EXISTS (SELECT 1 FROM dbo.ChiPhiSuKien WHERE MaChiPhi = 'CP004') INSERT INTO dbo.ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, SoTien, NgayChi, NguoiDuyet, GhiChu) VALUES ('CP004', 'EVT202602', N'Thuê phòng máy tính lab cấu hình cao', 32000000, '2026-11-05 14:00:00', 'USR001', N'Đạt 64% ngân sách dự kiến (BR-14)');
IF NOT EXISTS (SELECT 1 FROM dbo.ChiPhiSuKien WHERE MaChiPhi = 'CP005') INSERT INTO dbo.ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, SoTien, NgayChi, NguoiDuyet, GhiChu) VALUES ('CP005', 'EVT202602', N'Tài khoản Cloud GPU Cluster phục vụ thực hành', 12000000, '2026-11-06 16:30:00', 'USR001', N'Tổng chi đạt 44/50 triệu (88% ngân sách) -> KÍCH HOẠT CẢNH BÁO MÀU CAM BR-14!');
PRINT N'-> Đã nạp 5 khoản chi phí thực tế (kèm kịch bản kích hoạt BR-14 vượt 80% ngân sách).';
GO

-- 15. ĐÁNH GIÁ KHẢO SÁT HÀI LÒNG (PhanHoi - Phục vụ Tab 9 Form F05)
IF NOT EXISTS (SELECT 1 FROM dbo.PhanHoi WHERE MaPhanHoi = 'PH001') INSERT INTO dbo.PhanHoi (MaPhanHoi, MaSuKien, MaNguoiThamGia, DiemDanhGia, NoiDungPhanHoi, NgayDanhGia) VALUES ('PH001', 'EVT202601', 'NTG001', 5, N'Chương trình rất sôi động và ý nghĩa, các tiết mục chuẩn bị chuyên nghiệp. Rất mong duy trì hàng năm!', '2026-10-16 09:00:00');
IF NOT EXISTS (SELECT 1 FROM dbo.PhanHoi WHERE MaPhanHoi = 'PH002') INSERT INTO dbo.PhanHoi (MaPhanHoi, MaSuKien, MaNguoiThamGia, DiemDanhGia, NoiDungPhanHoi, NgayDanhGia) VALUES ('PH002', 'EVT202601', 'NTG002', 4, N'Khâu soát vé check-in QR qua webcam rất nhanh, không phải chờ đợi lâu. Âm thanh ánh sáng tuyệt vời!', '2026-10-16 09:30:00');
IF NOT EXISTS (SELECT 1 FROM dbo.PhanHoi WHERE MaPhanHoi = 'PH003') INSERT INTO dbo.PhanHoi (MaPhanHoi, MaSuKien, MaNguoiThamGia, DiemDanhGia, NoiDungPhanHoi, NgayDanhGia) VALUES ('PH003', 'EVT202601', 'NTG003', 5, N'Hội thảo có nội dung thực chiến cao, các diễn giả chia sẻ rất sâu về kiến trúc phần mềm và AI.', '2026-12-15 17:00:00');
PRINT N'-> Đã nạp 3 phiếu khảo sát đánh giá chất lượng sự kiện.';
GO

PRINT N'==============================================================================';
PRINT N'>> [HOÀN TẤT 100%] ĐÃ NẠP TOÀN BỘ DỮ LIỆU MẪU SẴN SÀNG CHO BUỔI BẢO VỆ ĐỒ ÁN!';
PRINT N'>> Mã QR sẵn sàng test webcam F13:  11111111-2222-3333-4444-555555555502';
PRINT N'>> Tài khoản đăng nhập F01:';
PRINT N'   - admin / Admin@123';
PRINT N'   - manager1 / Admin@123';
PRINT N'   - staff1 / Admin@123';
PRINT N'==============================================================================';
GO
