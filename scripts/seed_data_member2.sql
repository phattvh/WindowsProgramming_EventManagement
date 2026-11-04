-- ==============================================================================
-- SCRIPT DỮ LIỆU MẪU (SEED DATA) - PHÂN HỆ SỰ KIỆN & DANH MỤC DÙNG CHUNG
-- Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
-- Dự án: Hệ thống Quản lý Sự kiện (Event Management System)
-- Khớp 100% Cấu trúc 01_Database_Schema.sql trên EventManagementDB
-- ==============================================================================

USE EventManagementDB;
GO

PRINT N'>>> BẮT ĐẦU NẠP DỮ LIỆU MẪU PHÂN HỆ THÀNH VIÊN 2 (SỰ KIỆN & DANH MỤC)...';
GO

-- 1. DỮ LIỆU MẪU: LOẠI SỰ KIỆN (LoaiSuKien)
IF NOT EXISTS (SELECT 1 FROM dbo.LoaiSuKien WHERE MaLoaiSK = 'LSK04')
BEGIN
    INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES
    ('LSK04', N'Hội nghị Khoa học (Symposium)', N'Diễn đàn học thuật báo cáo nghiên cứu và công bố công trình khoa học.'),
    ('LSK05', N'Workshop Kỹ năng & Đào tạo', N'Các buổi thực hành chuyên môn ngắn hạn, tương tác trực tiếp.'),
    ('LSK06', N'Ngày hội Việc làm (Career Fair)', N'Kết nối doanh nghiệp tuyển dụng với sinh viên và ứng viên CNTT.');
    PRINT N'-> Đã bổ sung danh mục Loại sự kiện phong phú (LSK04 - LSK06).';
END
GO

-- 2. DỮ LIỆU MẪU: ĐỊA ĐIỂM (DiaDiem)
IF NOT EXISTS (SELECT 1 FROM dbo.DiaDiem WHERE MaDiaDiem = 'DD003')
BEGIN
    INSERT INTO dbo.DiaDiem (MaDiaDiem, TenDiaDiem, DiaChi, SucChuaToiDa, GhiChu) VALUES
    ('DD003', N'Trung tâm Sự kiện GEM Center', N'08 Nguyễn Bỉnh Khiêm, Phường Đa Kao, Quận 1, TP.HCM', 1800, N'Không gian sang trọng, trần cao 8m phù hợp dạ tiệc và lễ ra mắt sản phẩm VIP'),
    ('DD004', N'Trung tâm Triển lãm & Hội chợ SECC', N'799 Nguyễn Văn Linh, Phường Tân Phú, Quận 7, TP.HCM', 5000, N'Hội trường triển lãm tiêu chuẩn quốc tế quy mô lớn nhất miền Nam'),
    ('DD005', N'Hội trường A - ĐH Bách Khoa TP.HCM', N'268 Lý Thường Kiệt, Phường 14, Quận 10, TP.HCM', 1000, N'Hội trường âm học chuẩn cho hội nghị khoa học và hoạt động sinh viên'),
    ('DD006', N'Khách sạn Rex Sài Gòn (Lotus Ballroom)', N'141 Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP.HCM', 800, N'Tọa lạc phố đi bộ Nguyễn Huệ, phù hợp hội nghị ngoại giao và ký kết đối tác');
    PRINT N'-> Đã bổ sung 4 địa điểm tổ chức thực tế lớn tại TP.HCM (DD003 - DD006).';
END
GO

-- 3. DỮ LIỆU MẪU: DIỄN GIẢ & KHÁCH MỜI (DienGia)
IF NOT EXISTS (SELECT 1 FROM dbo.DienGia WHERE MaDienGia = 'DG003')
BEGIN
    INSERT INTO dbo.DienGia (MaDienGia, HoTen, ChucVu, DonViCongTac, Email, SoDienThoai, TieuSu, HinhAnh) VALUES
    ('DG003', N'ThS. Lê Quang Huy', N'Trưởng phòng An toàn Thông tin', N'Viettel Cyber Security', 'huylq@viettel.com.vn', '0988334455', N'Chuyên gia ứng cứu sự cố và bảo mật hạ tầng trọng yếu, diễn giả quen thuộc tại Security Bootcamp.', 'dg03_le_quang_huy.jpg'),
    ('DG004', N'Bà Phạm Mai Lan', N'Giám đốc Đổi mới Sáng tạo', N'Forbes Under 30 Asia', 'lan.pham@innovate.vn', '0977445566', N'Cố vấn khởi nghiệp công nghệ, đã ươm tạo hơn 40 dự án Tech Startup tại Đông Nam Á.', 'dg04_pham_mai_lan.jpg'),
    ('DG005', N'Ông Michael Harrison', N'Principal Cloud Architect', N'Google Cloud APAC', 'mharrison@google.com', '0933556677', N'Hơn 18 năm tư vấn kiến trúc Cloud Native và bảo mật dữ liệu cấp doanh nghiệp cho các tập đoàn toàn cầu.', 'dg05_michael_harrison.jpg'),
    ('DG006', N'ThS. Đặng Hữu Hoàng', N'Senior Software Architect', N'FPT Software', 'hoangdh@fpt.com', '0966778899', N'Chuyên gia kiến trúc Microservices, Clean Architecture và tối ưu hóa hệ thống .NET Enterprise.', 'dg06_dang_huu_hoang.jpg');
    PRINT N'-> Đã bổ sung 4 chuyên gia diễn giả uy tín (DG003 - DG006).';
END
GO

-- 4. DỮ LIỆU MẪU: NHÂN SỰ BAN TỔ CHỨC (NhanSu)
IF NOT EXISTS (SELECT 1 FROM dbo.NhanSu WHERE MaNhanSu = 'NS004')
BEGIN
    INSERT INTO dbo.NhanSu (MaNhanSu, HoTen, SoDienThoai, Email, ChucVu) VALUES
    ('NS004', N'Vũ Thị Mai', '0909123004', 'mai.vt@eventhub.vn', N'Trưởng ban Lễ tân'),
    ('NS005', N'Phan Đình Tùng', '0909123005', 'tung.pd@eventhub.vn', N'Nhân viên Soát vé QR'),
    ('NS006', N'Bùi Kim Ngân', '0909123006', 'ngan.bk@eventhub.vn', N'Cộng tác viên Hướng dẫn khách'),
    ('NS007', N'Nguyễn Tuấn Anh', '0909123007', 'anh.nt@eventhub.vn', N'Kỹ thuật viên Âm thanh & Ánh sáng'),
    ('NS008', N'Hoàng Minh Châu', '0909123008', 'chau.hm@eventhub.vn', N'Trưởng ban Hậu cần'),
    ('NS009', N'Trần Thị Hương', '0909123009', 'huong.tt@eventhub.vn', N'Tiếp đón Diễn giả VIP'),
    ('NS010', N'Đỗ Văn Khang', '0909123010', 'khang.dv@eventhub.vn', N'Đội trưởng An ninh');
    PRINT N'-> Đã bổ sung 7 nhân sự và cộng tác viên phục vụ vận hành (NS004 - NS010).';
END
GO

-- 5. DỮ LIỆU MẪU: NHÀ TÀI TRỢ (NhaTaiTro)
IF NOT EXISTS (SELECT 1 FROM dbo.NhaTaiTro WHERE MaNhaTaiTro = 'NTT003')
BEGIN
    INSERT INTO dbo.NhaTaiTro (MaNhaTaiTro, TenToChuc, NguoiDaiDien, Email, SoDienThoai, DiaChi) VALUES
    ('NTT003', N'Tập đoàn Công nghiệp - Viễn thông Quân đội (Viettel)', N'Tào Đức Thắng', 'support@viettel.com.vn', '18008098', N'Số 1 Trần Hữu Dực, Nam Từ Liêm, Hà Nội'),
    ('NTT004', N'Công ty Cổ phần Dịch vụ Di động Trực tuyến (Ví MoMo)', N'Nguyễn Bá Diệp', 'hotro@momo.vn', '1900545441', N'Tòa nhà Phú Mỹ Hưng, Quận 7, TP.HCM'),
    ('NTT005', N'Tập đoàn Vingroup (Quỹ Đổi mới sáng tạo VINIF)', N'Phan Thu Hiền', 'vinif@vinbigdata.org', '02439749999', N'Số 7 Đường Bằng Lăng 1, KĐT Vinhomes Riverside, Long Biên, Hà Nội');
    PRINT N'-> Đã bổ sung 3 nhà tài trợ doanh nghiệp lớn (NTT003 - NTT005).';
END
GO

-- 6. DỮ LIỆU MẪU: SỰ KIỆN (SuKien) - PHỦ KÍN CÁC TRẠNG THÁI THEO CK_SuKien_TrangThai
-- Ràng buộc: TrangThai IN (N'Dự kiến', N'Mở đăng ký', N'Đang diễn ra', N'Đã kết thúc', N'Hủy')
IF NOT EXISTS (SELECT 1 FROM dbo.SuKien WHERE MaSuKien = 'EVT202602')
BEGIN
    DECLARE @NguoiTao VARCHAR(20) = (SELECT TOP 1 MaNguoiDung FROM dbo.NguoiDung WHERE MaNhomQuyen = 'ADMIN');
    IF @NguoiTao IS NULL SET @NguoiTao = 'USR001';

    INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES
    -- Sự kiện 2: Đang diễn ra (để test Check-in F13)
    ('EVT202602', N'Workshop Thực chiến GenAI & Prompt Engineering 2026', 'LSK05', 'DD003', '2026-11-03 08:30:00', '2026-11-04 17:00:00', N'Đang diễn ra', N'Chuỗi đào tạo 2 ngày thực hành xây dựng AI Assistant và Agentic Workflows.', 'banner_genai2026.png', @NguoiTao),
    
    -- Sự kiện 3: Dự kiến (để test Sửa/Xóa sự kiện F04)
    ('EVT202603', N'Hội nghị Khoa học Quốc tế về Công nghệ Phần mềm & IoT', 'LSK04', 'DD004', '2026-12-25 08:00:00', '2026-12-26 18:00:00', N'Dự kiến', N'Diễn đàn học thuật công bố các bài báo khoa học quốc tế về kỹ thuật phần mềm.', 'banner_iot2026.png', @NguoiTao),
    
    -- Sự kiện 4: Đã kết thúc (để test Khảo sát phản hồi F18 & Báo cáo F19)
    ('EVT202604', N'Đêm Nhạc Gây Quỹ Sinh Viên - Vòng Tay Bè Bạn 2026', 'LSK02', 'DD005', '2026-10-15 18:00:00', '2026-10-15 22:30:00', N'Đã kết thúc', N'Đại nhạc hội nghệ thuật kết nối sinh viên nhằm gây quỹ học bổng vượt khó.', 'banner_music2026.png', @NguoiTao),
    
    -- Sự kiện 5: Hủy (để test ràng buộc BR-02 chặn bán vé khi đã hủy)
    ('EVT202605', N'Triển lãm Bất động sản Nghỉ dưỡng Mùa thu', 'LSK03', 'DD006', '2026-09-20 09:00:00', '2026-09-22 17:00:00', N'Hủy', N'Hội chợ bất động sản du lịch (sự kiện đã hủy theo thông báo ban tổ chức).', 'banner_expo2026.png', @NguoiTao);

    PRINT N'-> Đã nạp 4 sự kiện thực tế phủ kín các trạng thái (EVT202602 - EVT202605).';
END
GO

-- 7. DỮ LIỆU MẪU: LỊCH TRÌNH TIẾT MỤC (LichTrinh) - THỎA MÃN BR-05 & BR-06
IF NOT EXISTS (SELECT 1 FROM dbo.LichTrinh WHERE MaLichTrinh = 'LT003')
BEGIN
    INSERT INTO dbo.LichTrinh (MaLichTrinh, MaSuKien, TenTietMuc, ThoiGianBatDau, ThoiGianKetThuc, MaDienGia, DiaDiemChiTiet, GhiChu) VALUES
    -- Lịch trình cho EVT202601 (Vietnam Tech Summit)
    ('LT003', 'EVT202601', N'Tiệc Teabreak & Kết nối Doanh nghiệp B2B', '2026-12-15 10:00:00', '2026-12-15 10:30:00', NULL, N'Sảnh Expo Lounge', N'Thưởng thức trà, cafe và giao lưu kết nối mạng lưới'),
    ('LT004', 'EVT202601', N'Kiến trúc Hệ thống Phân tán Siêu quy mô (Hyperscale Systems)', '2026-12-15 13:30:00', '2026-12-15 15:00:00', 'DG005', N'Hội trường Grand Ballroom', N'Phiên chuyên sâu về hạ tầng Cloud quy mô lớn'),
    ('LT005', 'EVT202601', N'Panel Discussion: Tương lai Lập trình viên trong kỷ nguyên AI', '2026-12-15 15:15:00', '2026-12-15 16:30:00', 'DG006', N'Hội trường Grand Ballroom', N'Thảo luận bàn tròn cùng các CTO đầu ngành'),

    -- Lịch trình cho EVT202602 (Workshop GenAI Bootcamp)
    ('LT006', 'EVT202602', N'Khai mạc & Hướng dẫn Cài đặt Môi trường Lab', '2026-11-03 08:30:00', '2026-11-03 09:30:00', 'DG006', N'Phòng Lab Kỹ thuật A', N'Cấp tài khoản Cloud và khởi tạo workspace'),
    ('LT007', 'EVT202602', N'Thực chiến Prompt Engineering & Kỹ thuật RAG Nâng cao', '2026-11-03 09:30:00', '2026-11-03 12:00:00', 'DG003', N'Phòng Lab Kỹ thuật A', N'Bài tập lab tương tác trực tiếp 1-1'),
    ('LT008', 'EVT202602', N'Tích hợp AI Agents vào Phần mềm C# Windows Forms', '2026-11-03 13:30:00', '2026-11-03 17:00:00', 'DG006', N'Phòng Lab Kỹ thuật A', N'Hướng dẫn gọi API và streaming response lên UI');

    PRINT N'-> Đã nạp 6 tiết mục timeline chi tiết cho EVT202601 và EVT202602 (thỏa mãn BR-05 & BR-06).';
END
GO

-- 8. DỮ LIỆU MẪU: CHI TIẾT TÀI TRỢ (ChiTietTaiTro) - RÀNG BUỘC CK_ChiTietTaiTro_Goi
IF NOT EXISTS (SELECT 1 FROM dbo.ChiTietTaiTro WHERE MaTaiTro = 'TT003')
BEGIN
    INSERT INTO dbo.ChiTietTaiTro (MaTaiTro, MaSuKien, MaNhaTaiTro, GoiTaiTro, GiaTriTaiTro, HinhThucTaiTro, GhiChu) VALUES
    ('TT003', 'EVT202601', 'NTT003', N'Bạc',       30000000, N'Dịch vụ viễn thông', N'Tài trợ đường truyền Internet cáp quang tốc độ cao 1Gbps'),
    ('TT004', 'EVT202601', 'NTT004', N'Đồng',      15000000, N'Hiện vật / Voucher', N'Tài trợ cổng thanh toán MoMo miễn phí giao dịch vé'),
    ('TT005', 'EVT202602', 'NTT005', N'Kim Cương', 80000000, N'Học bổng & Cloud GPU', N'Cấp 300 tài khoản GPU Cloud cho học viên xuất sắc');
    PRINT N'-> Đã nạp 3 bản ghi gói tài trợ mới (TT003 - TT005).';
END
GO

-- 9. DỮ LIỆU MẪU: PHÂN CÔNG NHÂN SỰ (PhanCongNhanSu) - THỎA MÃN BR-13
IF NOT EXISTS (SELECT 1 FROM dbo.PhanCongNhanSu WHERE MaPhanCong = 'PC001')
BEGIN
    INSERT INTO dbo.PhanCongNhanSu (MaPhanCong, MaSuKien, MaNhanSu, NhiemVu, ThoiGianBatDau, ThoiGianKetThuc, GhiChu) VALUES
    ('PC001', 'EVT202601', 'NS001', N'Điều phối chung toàn bộ sự kiện', '2026-12-15 07:00:00', '2026-12-15 18:00:00', N'Bao quát khu vực lễ tân và sân khấu chính'),
    ('PC002', 'EVT202601', 'NS004', N'Trực bàn đón tiếp khách VIP và đại biểu', '2026-12-15 07:30:00', '2026-12-15 12:00:00', N'Đón tiếp các diễn giả và lãnh đạo doanh nghiệp'),
    ('PC003', 'EVT202601', 'NS005', N'Soát vé và quét mã QR Webcam cửa 1', '2026-12-15 07:30:00', '2026-12-15 12:00:00', N'Sử dụng laptop quét mã QR trên vé in hoặc điện thoại'),
    ('PC004', 'EVT202601', 'NS007', N'Vận hành âm thanh ánh sáng hội trường', '2026-12-15 07:00:00', '2026-12-15 17:30:00', N'Trực bàn điều khiển kỹ thuật P.A.'),
    ('PC005', 'EVT202601', 'NS010', N'Bảo đảm an ninh trật tự khu vực sảnh', '2026-12-15 06:30:00', '2026-12-15 18:30:00', N'Phối hợp với bảo vệ tòa nhà phân luồng xe'),
    ('PC006', 'EVT202602', 'NS006', N'Điểm danh học viên và phát tài liệu lab', '2026-11-03 08:00:00', '2026-11-03 12:00:00', N'Phát thẻ đeo và tài liệu thực hành'),
    ('PC007', 'EVT202602', 'NS007', N'Hỗ trợ kỹ thuật mạng phòng máy tính', '2026-11-03 08:00:00', '2026-11-03 17:00:00', N'Xử lý sự cố kết nối Cloud cho học viên');
    PRINT N'-> Đã nạp 7 ca trực nhân sự điều phối (thỏa mãn BR-13 không trùng ca).';
END
GO

-- 10. DỮ LIỆU MẪU: ĐÁNH GIÁ PHẢN HỒI (PhanHoi - Form F18) - THỎA MÃN BR-16
-- Nạp phản hồi cho sự kiện ĐÃ HOÀN THÀNH (EVT202604)
IF NOT EXISTS (SELECT 1 FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG001')
BEGIN
    INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, GioiTinh, Email, SoDienThoai, DonViCongTac) VALUES
    ('NTG001', N'Nguyễn Văn An', N'Nam', 'an.nguyen@gmail.com', '0912345678', N'Đại học Bách Khoa TP.HCM'),
    ('NTG002', N'Trần Thị Bích', N'Nữ', 'bich.tran@gmail.com', '0987654321', N'FPT Software');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.PhanHoi WHERE MaPhanHoi = 'PH001')
BEGIN
    INSERT INTO dbo.PhanHoi (MaPhanHoi, MaSuKien, MaNguoiThamGia, DiemDanhGia, NoiDungPhanHoi, NgayDanhGia) VALUES
    ('PH001', 'EVT202604', 'NTG001', 5, N'Chương trình ca nhạc rất sôi động và ý nghĩa, các tiết mục chuẩn bị chuyên nghiệp. Rất mong duy trì hàng năm!', '2026-10-16 09:00:00'),
    ('PH002', 'EVT202604', 'NTG002', 4, N'Khâu soát vé check-in QR rất nhanh, không phải chờ đợi lâu. Âm thanh ánh sáng tuyệt vời!', '2026-10-16 09:30:00');
    PRINT N'-> Đã nạp 2 phản hồi khảo sát mẫu cho sự kiện đã kết thúc (EVT202604).';
END
GO

PRINT N'==============================================================================';
PRINT N'>> [THÀNH CÔNG] ĐÃ NẠP TOÀN BỘ SEED DATA PHÂN HỆ SỰ KIỆN CỦA THÀNH VIÊN 2!';
PRINT N'>> Khớp 100% Schema EventManagementDB, sẵn sàng bàn giao cho toàn đội ngũ!';
PRINT N'==============================================================================';
GO
