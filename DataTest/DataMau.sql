-- ===================================================================================
-- TẬP TIN THIẾT LẬP MÔI TRƯỜNG KIỂM THỬ: DỮ LIỆU MẪU TOÀN DIỆN (DataMau.sql)
-- DỰ ÁN: PHẦN MỀM QUẢN LÝ SỰ KIỆN (EventManagement)
-- MỤC ĐÍCH: Nạp bộ dữ liệu thử nghiệm chuẩn hóa phong phú phục vụ kiểm thử thủ công (Manual Test),
--          kiểm thử tự động, bao quát 12 Sự kiện thực tế, 20 Form (F01 - F20) và 20 Quy tắc (BR-01 - BR-20).
-- ===================================================================================

USE EventManagementDB;
GO

SET NOCOUNT ON;

PRINT N'>>> BẮT ĐẦU NẠP DỮ LIỆU MẪU KIỂM THỬ MỞ RỘNG (DataMau.sql)...';

-- -----------------------------------------------------------------------------------
-- 1. XÓA DỮ LIỆU CŨ THEO THỨ TỰ RÀNG BUỘC KHÓA NGOẠI (CLEANUP)
-- -----------------------------------------------------------------------------------
DELETE FROM dbo.PhanHoi;
DELETE FROM dbo.ChiPhiSuKien;
DELETE FROM dbo.PhanCongNhanSu;
DELETE FROM dbo.CheckIn;
DELETE FROM dbo.ChiTietVe;
DELETE FROM dbo.DangKy;
DELETE FROM dbo.NguoiThamGia;
DELETE FROM dbo.HangVe;
DELETE FROM dbo.ChiTietTaiTro;
DELETE FROM dbo.NhaTaiTro;
DELETE FROM dbo.LichTrinh;
DELETE FROM dbo.SuKien;
DELETE FROM dbo.NhanSu;
DELETE FROM dbo.DienGia;
DELETE FROM dbo.DiaDiem;
DELETE FROM dbo.LoaiSuKien;
GO

-- -----------------------------------------------------------------------------------
-- 2. DANH MỤC LOẠI SỰ KIỆN (F06 - LoaiSuKien)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES
('LSK01', N'Hội thảo & Hội nghị Công nghệ', N'Chuyên đề chia sẻ kiến thức IT, Trí tuệ nhân tạo, Điện toán đám mây và Phần mềm.'),
('LSK02', N'Đại nhạc hội & Lễ hội Âm nhạc', N'Sự kiện văn hóa biểu diễn quy mô lớn ngoài trời và sân vận động.'),
('LSK03', N'Triển lãm Thương mại & Kết nối Doanh nghiệp', N'Không gian trưng bày, kết nối giao thương giữa các tập đoàn và doanh nghiệp.'),
('LSK04', N'Giải thi đấu Thể thao Sinh viên', N'Các giải bóng đá, thể thao điện tử eSports và chạy bộ phong trào.'),
('LSK05', N'Workshop Kỹ năng & Đào tạo Chuyên sâu', N'Khóa đào tạo kỹ năng thực chiến ngắn hạn, tương tác trực tiếp với chuyên gia.'),
('LSK06', N'Ngày hội Việc làm (Career Fair)', N'Ngày hội kết nối tuyển dụng doanh nghiệp công nghệ với sinh viên và kỹ sư trẻ.');
GO

-- -----------------------------------------------------------------------------------
-- 3. DANH MỤC ĐỊA ĐIỂM (F07 - DiaDiem)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.DiaDiem (MaDiaDiem, TenDiaDiem, DiaChi, SucChuaToiDa, GhiChu) VALUES
('DD001', N'Trung tâm Hội nghị White Palace', N'194 Hoàng Văn Thụ, Phường 9, Quận Phú Nhuận, TP.HCM', 1500, N'Hội trường Grand Ballroom đầy đủ màn hình LED, âm thanh P.A chuyên nghiệp.'),
('DD002', N'Nhà thi đấu Thể dục Thể thao Phú Thọ', N'1 Lữ Gia, Phường 15, Quận 11, TP.HCM', 5000, N'Không gian quy mô lớn, phù hợp đại nhạc hội hoặc triển lãm công nghệ cao.'),
('DD003', N'Phòng Hội thảo VIP - Bitexco Financial Tower', N'Tầng 25, Tòa nhà Bitexco, Bến Nghé, Quận 1, TP.HCM', 50, N'Phòng họp VIP sức chứa giới hạn 50 người, view toàn cảnh trung tâm.'),
('DD004', N'Trung tâm Sự kiện Riverside Palace', N'360D Bến Vân Đồn, Phường 1, Quận 4, TP.HCM', 800, N'Hội trường tiệc và hội thảo ven sông, không gian hiện đại.'),
('DD005', N'Trung tâm Triển lãm & Hội nghị Sài Gòn (SECC)', N'799 Nguyễn Văn Linh, Phường Tân Phú, Quận 7, TP.HCM', 6000, N'Hội trường triển lãm tiêu chuẩn quốc tế quy mô lớn nhất miền Nam.'),
('DD006', N'Hội trường Thống Nhất - Dinh Độc Lập', N'135 Nam Kỳ Khởi Nghĩa, Phường Bến Thành, Quận 1, TP.HCM', 1000, N'Không gian lịch sử trang nghiêm, sân khấu ngoài trời rộng rãi.');
GO

-- -----------------------------------------------------------------------------------
-- 4. DANH MỤC DIỄN GIẢ / KHÁCH MỜI (F09 - DienGia)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.DienGia (MaDienGia, HoTen, ChucVu, DonViCongTac, Email, SoDienThoai, TieuSu, HinhAnh) VALUES
('DG001', N'TS. Nguyễn Văn Hùng', N'Chuyên gia Trí tuệ Nhân tạo', N'Viện Nghiên cứu Đổi mới Sáng tạo AI', 'hung.nguyen@ai-institute.vn', '0912345678', N'Hơn 15 năm nghiên cứu Machine Learning, LLM và Agentic Coding.', 'dg01_nguyen_van_an.jpg'),
('DG002', N'ThS. Lê Hoàng Mai', N'Giám đốc Kỹ thuật (CTO)', N'TechCorp Asia Group', 'mai.le@techcorp.com', '0987654321', N'Chuyên gia kiến trúc hệ thống phân tán, vi dịch vụ và bảo mật Cloud.', 'dg02_tran_thi_bich.jpg'),
('DG003', N'Bà Trần Thị Kim Cương', N'Giám đốc Phát triển Đối tác', N'Quỹ Đầu tư Khởi nghiệp NextGen', 'cuong.tran@nextgenvc.com', '0933221100', N'Cố vấn chiến lược gọi vốn cho hơn 30 doanh nghiệp công nghệ.', 'dg03_le_quang_huy.jpg'),
('DG004', N'ThS. Vũ Hải Đăng', N'Kiến trúc sư Giải pháp Phần mềm', N'Global Software Solutions', 'dang.vu@gss.com.vn', '0944556677', N'Diễn giả khách mời chuyên ngành Clean Architecture và DevOps CI/CD.', 'dg04_pham_mai_lan.jpg'),
('DG005', N'Michael Harrison', N'Giám đốc An ninh mạng Khu vực', N'CyberShield Global Inc', 'michael.harrison@cybershield.com', '0918998877', N'Chuyên gia ứng phó sự cố mã độc tống tiền (Ransomware) và Zero-Trust.', 'dg05_michael_harrison.jpg'),
('DG006', N'TS. Đặng Hữu Hoàng', N'Trưởng Bộ môn Hệ thống Thông tin', N'Đại học Khoa học Tự nhiên TP.HCM', 'hoang.dang@hcmus.edu.vn', '0903887766', N'Chuyên gia cơ sở dữ liệu lớn, phân tích dữ liệu nghiệp vụ và khai phá dữ liệu.', 'dg06_dang_huu_hoang.jpg');
GO

-- -----------------------------------------------------------------------------------
-- 5. DANH MỤC NHÂN SỰ VẬN HÀNH (F14 - NhanSu)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.NhanSu (MaNhanSu, HoTen, SoDienThoai, Email, ChucVu) VALUES
('NS001', N'Trần Minh Tâm', '0933111222', 'tam.tran@eventops.vn', N'Trưởng nhóm điều phối'),
('NS002', N'Lê Thị Thu Thảo', '0944333444', 'thao.le@eventops.vn', N'Điều phối viên Check-in'),
('NS003', N'Nguyễn Quốc Bảo', '0966555666', 'bao.nguyen@eventops.vn', N'Kỹ thuật viên Âm thanh - Ánh sáng'),
('NS004', N'Phạm Quỳnh Như', '0977888999', 'nhu.pham@eventops.vn', N'Lễ tân đón tiếp đại biểu'),
('NS005', N'Đỗ Hải Đăng', '0911223344', 'dang.do@eventops.vn', N'Cộng tác viên hiện trường'),
('NS006', N'Hoàng Văn Tuấn', '0922334455', 'tuan.hoang@eventops.vn', N'Kỹ thuật viên Mạng & Hạ tầng IT'),
('NS007', N'Vũ Mai Lan', '0933445566', 'lan.vu@eventops.vn', N'Điều phối viên Hậu cần & Teabreak');
GO

-- -----------------------------------------------------------------------------------
-- 6. DANH MỤC NHÀ TÀI TRỢ (F16 - NhaTaiTro)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.NhaTaiTro (MaNhaTaiTro, TenToChuc, NguoiDaiDien, Email, SoDienThoai, DiaChi) VALUES
('NTT001', N'Tập đoàn Viễn thông & Công nghệ VietTech', N'Ông Phạm Nhật Long', 'contact@viettech.vn', '0283899999', N'Tòa nhà VietTech Tower, Quận 1, TP.HCM'),
('NTT002', N'Ngân hàng Số FinPay Việt Nam', N'Bà Võ Kim Oanh', 'partnership@finpay.vn', '0283788888', N'Khu Đô thị Sala, TP. Thủ Đức, TP.HCM'),
('NTT003', N'Công ty Cổ phần Nước uống Tinh khiết PureWater', N'Ông Nguyễn Tuấn Tú', 'sales@purewater.vn', '0283511111', N'Khu Chế xuất Tân Thuận, Quận 7, TP.HCM'),
('NTT004', N'Doanh nghiệp Khởi nghiệp Công nghệ EcoLife', N'Bà Mai Phương Lan', 'contact@ecolife.vn', '0283622222', N'Công viên Phần mềm Quang Trung, Quận 12, TP.HCM'),
('NTT005', N'Tập đoàn Công nghệ Đám mây CloudVn', N'Ông Lê Quốc Hưng', 'partner@cloudvn.com', '0283912345', N'Tòa nhà Landmark 81, Bình Thạnh, TP.HCM'),
('NTT006', N'Công ty Nước giải khát RedBull Việt Nam', N'Bà Hoàng Mỹ Linh', 'marketing@redbull.vn', '0283865432', N'Khu Công nghiệp Sóng Thần, Dĩ An, Bình Dương');
GO

-- -----------------------------------------------------------------------------------
-- 7. DANH MỤC SỰ KIỆN MỞ RỘNG (12 SỰ KIỆN - ĐỦ LOẠI VÀ TRẠNG THÁI) (F04, F05 - SuKien)
-- Lưu ý: TrangThai tuân thủ ràng buộc CHECK CK_SuKien_TrangThai:
--        IN (N'Dự kiến', N'Mở đăng ký', N'Đang diễn ra', N'Đã kết thúc', N'Hủy')
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES
('EVT202601', N'Hội nghị Công nghệ Vietnam Tech Summit 2026', 'LSK01', 'DD001', 
 '2026-12-15 08:00:00', '2026-12-15 17:30:00', N'Mở đăng ký', 
 N'Hội nghị công nghệ thường niên quy tụ hơn 1,000 chuyên gia, kỹ sư phần mềm và lãnh đạo doanh nghiệp hàng đầu.', 
 'banner_techsummit2026.png', 'USR002'),

('EVT202602', N'Đại nhạc hội Sinh viên Youth Wave Fest 2026', 'LSK02', 'DD002', 
 '2026-12-25 18:00:00', '2026-12-25 23:00:00', N'Dự kiến', 
 N'Đêm nhạc hội giao lưu văn nghệ dành cho sinh viên các trường đại học với dàn nghệ sĩ trẻ nổi tiếng.', 
 'banner_musicfest2026.png', 'USR002'),

('EVT202603', N'Hội thảo Khởi nghiệp Trẻ & Đổi mới Sáng tạo 2026', 'LSK01', 'DD003', 
 '2026-01-10 08:30:00', '2026-01-10 12:00:00', N'Đã kết thúc', 
 N'Chia sẻ kinh nghiệm xây dựng sản phẩm và gọi vốn khởi nghiệp trong kỷ nguyên số.', 
 'banner_startup2026.png', 'USR002'),

('EVT202604', N'Triển lãm Thiết bị & Trải nghiệm AR/VR', 'LSK03', 'DD001', 
 '2026-05-20 09:00:00', '2026-05-21 17:00:00', N'Hủy', 
 N'Khu trưng bày kính thực tế ảo và mô phỏng vũ trụ ảo (Đã hủy do thay đổi kế hoạch đối tác).', 
 'banner_arvr2026.png', 'USR006'),

('EVT202605', N'Workshop Thực chiến GenAI & Xây dựng AI Agent 2026', 'LSK05', 'DD003', 
 '2026-11-28 08:30:00', '2026-11-28 17:00:00', N'Đang diễn ra', 
 N'Đào tạo kỹ năng thực chiến ứng dụng Large Language Models và xây dựng AI Agent tự động hóa doanh nghiệp.', 
 'banner_genai2026.png', 'USR002'),

('EVT202606', N'Giải thi đấu Thể thao Điện tử Sinh viên Toàn quốc (eSports Cup)', 'LSK04', 'DD002', 
 '2026-12-05 08:00:00', '2026-12-07 21:00:00', N'Mở đăng ký', 
 N'Tranh tài các bộ môn thể thao điện tử hàng đầu giữa các trường đại học toàn quốc với giải thưởng lớn.', 
 'banner_esports2026.png', 'USR002'),

('EVT202607', N'Ngày hội Việc làm & Kết nối Doanh nghiệp IT Career Fair 2026', 'LSK06', 'DD005', 
 '2026-12-18 08:00:00', '2026-12-18 17:00:00', N'Mở đăng ký', 
 N'Kết nối tuyển dụng hơn 60 doanh nghiệp công nghệ hàng đầu với sinh viên và kỹ sư phần mềm trẻ.', 
 'banner_careerfair2026.png', 'USR002'),

('EVT202608', N'Diễn đàn An ninh mạng Quốc gia & Phòng chống Ransomware 2026', 'LSK01', 'DD006', 
 '2026-11-15 08:30:00', '2026-11-15 16:30:00', N'Đã kết thúc', 
 N'Tọa đàm chuyên sâu về giải pháp bảo vệ hạ tầng trọng yếu, ứng phó sự cố tấn công dữ liệu tống tiền.', 
 'banner_security2026.png', 'USR002'),

('EVT202609', N'Hội nghị Khoa học Quốc tế về Công nghệ Phần mềm & IoT (ICSE 2026)', 'LSK01', 'DD004', 
 '2026-12-28 08:00:00', '2026-12-29 17:30:00', N'Dự kiến', 
 N'Diễn đàn học thuật công bố các nghiên cứu mới về Clean Architecture, Microservices và Hệ thống phân tán.', 
 'banner_icse2026.png', 'USR002'),

('EVT202610', N'Lễ hội Văn hóa Ẩm thực & Âm nhạc Đường phố Sài Gòn 2026', 'LSK02', 'DD002', 
 '2026-11-08 16:00:00', '2026-11-09 22:30:00', N'Đã kết thúc', 
 N'Sự kiện giao lưu văn hóa ẩm thực truyền thống kết hợp biểu diễn nghệ thuật âm nhạc đường phố.', 
 'banner_streetfood2026.png', 'USR002'),

('EVT202611', N'Diễn đàn Chuyển đổi số Doanh nghiệp Bán lẻ & E-Commerce 2026', 'LSK03', 'DD001', 
 '2026-12-20 08:30:00', '2026-12-20 17:00:00', N'Mở đăng ký', 
 N'Giải pháp thúc đẩy chuyển đổi số bán lẻ đa kênh, tự động hóa chuỗi cung ứng và thanh toán số.', 
 'banner_ecommerce2026.png', 'USR002'),

('EVT202612', N'Giải Marathon Sinh viên Vì Môi trường Xanh (Green Run 2026)', 'LSK04', 'DD002', 
 '2026-12-27 05:30:00', '2026-12-27 11:30:00', N'Dự kiến', 
 N'Giải chạy việt dã phong trào quy tụ hơn 3000 sinh viên nhằm gây quỹ trồng cây bảo vệ môi trường.', 
 'banner_greenrun2026.png', 'USR002');
GO

-- -----------------------------------------------------------------------------------
-- 8. LỊCH TRÌNH TIẾT MỤC (F08 - LichTrinh)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.LichTrinh (MaLichTrinh, MaSuKien, TenTietMuc, ThoiGianBatDau, ThoiGianKetThuc, MaDienGia, DiaDiemChiTiet, GhiChu) VALUES
('LT001', 'EVT202601', N'Khai mạc & Keynote: Xu hướng GenAI & Agentic Coding 2026', '2026-12-15 08:30:00', '2026-12-15 10:00:00', 'DG001', N'Hội trường chính Grand Ballroom', N'Trình chiếu bài phát biểu khai mạc và demo trực tiếp'),
('LT002', 'EVT202601', N'Tọa đàm: Bảo mật Đám mây & Kiến trúc Microservices', '2026-12-15 10:15:00', '2026-12-15 11:45:00', 'DG002', N'Phòng Hội thảo Lotus 2', N'Phiên trao đổi thảo luận bàn tròn cùng khách mời'),
('LT003', 'EVT202601', N'Workshop: Ứng dụng AI xây dựng ứng dụng Desktop Hiện đại', '2026-12-15 13:30:00', '2026-12-15 15:30:00', 'DG001', N'Hội trường chính Grand Ballroom', N'Thực hành tương tác cùng người tham dự'),

('LT004', 'EVT202605', N'Lý thuyết Nền tảng: LLM Architecture & Prompt Patterns', '2026-11-28 09:00:00', '2026-11-28 11:30:00', 'DG001', N'Phòng VIP Bitexco', N'Phân tích cơ chế Attention và Prompt Optimization'),
('LT005', 'EVT202605', N'Thực hành Lab: Xây dựng AI Assistant với Tool Calling & RAG', '2026-11-28 13:30:00', '2026-11-28 16:30:00', 'DG001', N'Phòng VIP Bitexco', N'Học viên thực hành trực tiếp trên máy tính cá nhân'),

('LT006', 'EVT202606', N'Lễ Khai mạc & Vòng Bảng Giải eSports Sinh viên', '2026-12-05 08:30:00', '2026-12-05 18:00:00', NULL, N'Sân khấu Trung tâm Nhà thi đấu', N'Lễ diễu hành các đội tuyển và thi đấu bảng A, B'),
('LT007', 'EVT202606', N'Trận Chung kết Tổng & Lễ Trao Cúp Vô địch', '2026-12-07 15:00:00', '2026-12-07 20:30:00', NULL, N'Sân khấu Trung tâm Nhà thi đấu', N'Truyền hình trực tiếp trên các nền tảng mạng xã hội'),

('LT008', 'EVT202607', N'Khai mạc & Tọa đàm Bí quyết Chinh phục Nhà tuyển dụng IT', '2026-12-18 08:30:00', '2026-12-18 10:30:00', 'DG003', N'Khu vực Hội thảo Sảnh A - SECC', N'Diễn giả chia sẻ xu hướng tuyển dụng ngành phần mềm'),
('LT009', 'EVT202607', N'Phiên Phỏng vấn Tuyển dụng & Thử việc Nhanh (Fast-Track)', '2026-12-18 10:30:00', '2026-12-18 16:30:00', NULL, N'Gian hàng các Doanh nghiệp', N'Hơn 60 gian hàng phỏng vấn ứng viên trực tiếp'),

('LT010', 'EVT202608', N'Báo cáo Toàn cảnh: Nguy cơ Mã độc Tống tiền và Chiến lược Phòng thủ', '2026-11-15 09:00:00', '2026-11-15 11:30:00', 'DG005', N'Hội trường chính Thống Nhất', N'Báo cáo dữ liệu điều tra thực tế từ các vụ rò rỉ năm 2026'),

('LT011', 'EVT202611', N'Keynote: Đột phá Doanh thu Bán lẻ với Thương mại Đa kênh Omni-Channel', '2026-12-20 09:00:00', '2026-12-20 11:00:00', 'DG004', N'Sảnh Grand Ballroom White Palace', N'Chia sẻ case-study thành công của chuỗi bán lẻ hiện đại');
GO

-- -----------------------------------------------------------------------------------
-- 9. GÓI TÀI TRỢ SỰ KIỆN (F16 - ChiTietTaiTro)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.ChiTietTaiTro (MaTaiTro, MaSuKien, MaNhaTaiTro, GoiTaiTro, GiaTriTaiTro, HinhThucTaiTro, GhiChu) VALUES
('TT001', 'EVT202601', 'NTT001', N'Kim Cương', 150000000, N'Tiền mặt', N'Tài trợ độc quyền tiệc Gala Dinner và quà tặng công nghệ'),
('TT002', 'EVT202601', 'NTT002', N'Vàng',       60000000,  N'Tiền mặt', N'Tài trợ truyền thông và cổng thanh toán trực tuyến'),
('TT003', 'EVT202601', 'NTT003', N'Đồng',       15000000,  N'Hiện vật', N'Tài trợ toàn bộ nước uống đóng chai cho sự kiện'),
('TT004', 'EVT202606', 'NTT006', N'Kim Cương', 100000000, N'Hiện vật', N'Tài trợ nước tăng lực RedBull và hiện kim giải thưởng eSports'),
('TT005', 'EVT202606', 'NTT002', N'Vàng',       50000000,  N'Tiền mặt', N'Tài trợ cổng ví điện tử và giải thưởng phụ'),
('TT006', 'EVT202607', 'NTT001', N'Kim Cương',  80000000,  N'Tiền mặt', N'Tài trợ không gian tuyển dụng và sân khấu chính'),
('TT007', 'EVT202611', 'NTT005', N'Vàng',       50000000,  N'Hiện vật', N'Tài trợ hạ tầng máy chủ Cloud Server phục vụ demo');
GO

-- -----------------------------------------------------------------------------------
-- 10. HẠNG VÉ & MỞ BÁN (F12 - HangVe)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.HangVe (MaHangVe, MaSuKien, TenHangVe, GiaVe, SoLuongPhatHanh, SoLuongDaBan, ThoiGianMoBan, ThoiGianDongBan) VALUES
('HV001', 'EVT202601', N'Vé Tiêu chuẩn (Standard)', 200000, 500, 5,  '2026-11-01 00:00:00', '2026-12-14 23:59:59'),
('HV002', 'EVT202601', N'Vé VIP (Full-access)',     800000, 100, 2,  '2026-11-01 00:00:00', '2026-12-14 23:59:59'),
('HV003', 'EVT202601', N'Vé Ưu đãi Sinh viên',      50000,  50,  50, '2026-11-01 00:00:00', '2026-12-10 23:59:59'),
('HV004', 'EVT202601', N'Vé Miễn phí Khách mời VIP',0,       30,  0,  '2026-11-01 00:00:00', '2026-12-14 23:59:59'),

('HV005', 'EVT202605', N'Vé Tham dự Workshop GenAI',350000, 40,  1,  '2026-11-01 00:00:00', '2026-11-27 23:59:59'),
('HV006', 'EVT202605', N'Vé VIP Mentoring 1-1',     800000, 10,  0,  '2026-11-01 00:00:00', '2026-11-27 23:59:59'),

('HV007', 'EVT202606', N'Vé Khán giả eSports Cup',   80000,  1500, 1, '2026-11-15 00:00:00', '2026-12-04 23:59:59'),
('HV008', 'EVT202606', N'Vé VIP Khán đài A',        250000, 200,  0,  '2026-11-15 00:00:00', '2026-12-04 23:59:59'),

('HV009', 'EVT202607', N'Vé Miễn phí Tham quan',    0,      2000, 1,  '2026-11-20 00:00:00', '2026-12-17 23:59:59'),
('HV010', 'EVT202607', N'Vé Fast-Track Phỏng vấn',  50000,  300,  0,  '2026-11-20 00:00:00', '2026-12-17 23:59:59'),

('HV011', 'EVT202608', N'Vé Đại biểu An ninh mạng', 500000, 400,  1,  '2026-10-15 00:00:00', '2026-11-14 23:59:59'),
('HV012', 'EVT202610', N'Vé Vào cổng Lễ hội Ẩm thực',100000,2000, 1, '2026-10-01 00:00:00', '2026-11-07 23:59:59'),
('HV013', 'EVT202611', N'Vé Tham dự Doanh nghiệp',  350000, 500,  0,  '2026-11-20 00:00:00', '2026-12-19 23:59:59');
GO

-- -----------------------------------------------------------------------------------
-- 11. HỒ SƠ NGƯỜI THAM GIA (F10 - NguoiThamGia)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, GioiTinh, Email, SoDienThoai, DonViCongTac) VALUES
('NTG001', N'Nguyễn Văn An',    N'Nam', 'an.nguyen@fpt.com',       '0908111222', N'Công ty Cổ phần Viễn thông FPT'),
('NTG002', N'Lê Thị Mai',       N'Nữ',  'mai.le@techstart.vn',     '0908222333', N'TechStart Vietnam Hub'),
('NTG003', N'Trần Quốc Tuấn',   N'Nam', 'tuan.tran@vng.com.vn',    '0908333444', N'Tập đoàn VNG'),
('NTG004', N'Phạm Thu Trang',   N'Nữ',  'trang.pham@vnpay.vn',     '0908444555', N'Công ty CP Giải pháp Thanh toán VNPAY'),
('NTG005', N'Hoàng Minh Đức',   N'Nam', 'duc.hoang@student.vn',    '0908555666', N'Trường Đại học Bách Khoa TP.HCM'),
('NTG006', N'Vũ Ngọc Ánh',      N'Nữ',  'anh.vu@tiki.vn',          '0908666777', N'Tiki Corporation'),
('NTG007', N'Đặng Đình Toàn',   N'Nam', 'toan.dang@shopee.vn',     '0908777888', N'Shopee Logistics Vietnam'),
('NTG008', N'Bùi Thanh Hà',     N'Nữ',  'ha.bui@chotot.vn',        '0908888999', N'Chợ Tốt (Carousell Group)'),
('NTG009', N'Trương Công Định', N'Nam', 'dinh.truong@ai.edu.vn',   '0908999000', N'Viện Trí tuệ Nhân tạo Ứng dụng'),
('NTG010', N'Ngô Bảo Trân',     N'Nữ',  'tran.ngo@fintech.vn',     '0908000111', N'Fintech Solutions JSC'),
('NTG011', N'Đinh Quốc Huy',    N'Nam', 'huy.dinh@security.vn',    '0908222444', N'Trung tâm An ninh Mạng Quốc gia'),
('NTG012', N'Lý Mỹ Duyên',      N'Nữ',  'duyen.ly@foody.vn',       '0908333555', N'Hiệp hội Ẩm thực Sài Gòn');
GO

-- -----------------------------------------------------------------------------------
-- 12. ĐĂNG KÝ MUA VÉ (F11 - DangKy)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.DangKy (MaDangKy, MaSuKien, MaNguoiThamGia, MaHangVe, NgayDangKy, SoLuong, TongTien, TrangThaiThanhToan, PhuongThucThanhToan) VALUES
('DK001', 'EVT202601', 'NTG001', 'HV001', '2026-11-10 09:15:00', 2, 400000, N'Đã thanh toán',   N'Tiền mặt tại quầy'),
('DK002', 'EVT202601', 'NTG002', 'HV002', '2026-11-11 14:20:00', 1, 800000, N'Đã thanh toán',   N'Chuyển khoản QR'),
('DK003', 'EVT202601', 'NTG003', 'HV002', '2026-11-12 10:05:00', 1, 800000, N'Đã thanh toán',   N'Chuyển khoản QR'),
('DK004', 'EVT202601', 'NTG004', 'HV001', '2026-11-15 16:45:00', 2, 400000, N'Đã thanh toán',   N'Thẻ ngân hàng'),
('DK005', 'EVT202601', 'NTG005', 'HV001', '2026-11-20 11:30:00', 1, 200000, N'Chưa thanh toán', N'Chưa thanh toán'),

('DK006', 'EVT202605', 'NTG009', 'HV005', '2026-11-22 09:00:00', 1, 350000, N'Đã thanh toán',   N'Chuyển khoản QR'),
('DK007', 'EVT202606', 'NTG005', 'HV007', '2026-11-25 15:00:00', 1,  80000, N'Đã thanh toán',   N'Ví điện tử MoMo'),
('DK008', 'EVT202607', 'NTG006', 'HV009', '2026-11-26 10:30:00', 1,      0, N'Đã thanh toán',   N'Miễn phí'),
('DK009', 'EVT202608', 'NTG011', 'HV011', '2026-11-01 14:00:00', 1, 500000, N'Đã thanh toán',   N'Chuyển khoản QR'),
('DK010', 'EVT202610', 'NTG012', 'HV012', '2026-10-25 18:30:00', 1, 100000, N'Đã thanh toán',   N'Tiền mặt tại quầy');
GO

-- -----------------------------------------------------------------------------------
-- 13. CHI TIẾT VÉ & MÃ QR CODE (F11, F13 - ChiTietVe)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.ChiTietVe (MaVe, MaDangKy, MaQR, TenNguoiThamGia, Email, TrangThaiVe) VALUES
('VE001_1', 'DK001', 'QR_EVT202601_001_1', N'Nguyễn Văn An',  'an.nguyen@fpt.com',    N'Hợp lệ'),
('VE001_2', 'DK001', 'QR_EVT202601_001_2', N'Nguyễn Văn Bình', 'binh.nguyen@fpt.com',  N'Hợp lệ'),
('VE002_1', 'DK002', 'QR_EVT202601_002_1', N'Lê Thị Mai',     'mai.le@techstart.vn',  N'Hợp lệ'),
('VE003_1', 'DK003', 'QR_EVT202601_003_1', N'Trần Quốc Tuấn', 'tuan.tran@vng.com.vn', N'Đã check-in'),
('VE004_1', 'DK004', 'QR_EVT202601_004_1', N'Phạm Thu Trang', 'trang.pham@vnpay.vn',  N'Hợp lệ'),
('VE004_2', 'DK004', 'QR_EVT202601_004_2', N'Đỗ Minh Quang',  'quang.do@vnpay.vn',    N'Hợp lệ'),

('VE005_1', 'DK006', 'QR_EVT202605_001_1', N'Trương Công Định','dinh.truong@ai.edu.vn',N'Đã check-in'),
('VE006_1', 'DK007', 'QR_EVT202606_001_1', N'Hoàng Minh Đức',  'duc.hoang@student.vn', N'Hợp lệ'),
('VE007_1', 'DK008', 'QR_EVT202607_001_1', N'Vũ Ngọc Ánh',     'anh.vu@tiki.vn',       N'Hợp lệ'),
('VE008_1', 'DK009', 'QR_EVT202608_001_1', N'Đinh Quốc Huy',   'huy.dinh@security.vn', N'Đã check-in'),
('VE010_1', 'DK010', 'QR_EVT202610_001_1', N'Lý Mỹ Duyên',     'duyen.ly@foody.vn',    N'Đã check-in');
GO

-- -----------------------------------------------------------------------------------
-- 14. SOÁT VÉ CHECK-IN (F13 - CheckIn)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.CheckIn (MaCheckIn, MaVe, ThoiGianCheckIn, NguoiThucHien, HinhThucCheckIn, GhiChu) VALUES
('CK001', 'VE003_1', '2026-12-15 08:15:00', 'USR003', N'Webcam QR Code', N'Check-in hợp lệ tại Cổng chính A'),
('CK002', 'VE005_1', '2026-11-28 08:20:00', 'USR003', N'Webcam QR Code', N'Check-in Workshop GenAI đúng giờ'),
('CK003', 'VE008_1', '2026-11-15 08:10:00', 'USR003', N'Nhập mã thủ công', N'Đại biểu VIP check-in thủ công'),
('CK004', 'VE010_1', '2026-11-08 16:15:00', 'USR003', N'File ảnh QR',    N'Quét thành công từ vé điện tử trên điện thoại');
GO

-- -----------------------------------------------------------------------------------
-- 15. PHÂN CÔNG NHÂN SỰ (F15 - PhanCongNhanSu)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.PhanCongNhanSu (MaPhanCong, MaSuKien, MaNhanSu, NhiemVu, ThoiGianBatDau, ThoiGianKetThuc, GhiChu) VALUES
('PC001', 'EVT202601', 'NS001', N'Trưởng nhóm điều phối hiện trường', '2026-12-15 07:00:00', '2026-12-15 18:00:00', N'Phụ trách tổng thể điều hành khu vực hội trường chính'),
('PC002', 'EVT202601', 'NS002', N'Soát vé Check-in Cổng A',          '2026-12-15 07:30:00', '2026-12-15 12:00:00', N'Trực thiết bị quét mã QR cổng vào chính'),
('PC003', 'EVT202601', 'NS004', N'Lễ tân đón tiếp khách mời VIP',    '2026-12-15 08:00:00', '2026-12-15 12:00:00', N'Đón tiếp diễn giả và trao thẻ đại biểu'),

('PC004', 'EVT202605', 'NS002', N'Trực check-in và hướng dẫn phòng Lab','2026-11-28 08:00:00', '2026-11-28 17:30:00', N'Hỗ trợ học viên kết nối Wifi và máy chủ GPU'),
('PC005', 'EVT202606', 'NS003', N'Giám sát hệ thống mạng thi đấu',    '2026-12-05 07:30:00', '2026-12-07 21:30:00', N'Đảm bảo đường truyền internet cáp quang ổn định'),
('PC006', 'EVT202607', 'NS005', N'Điều phối luồng sinh viên tham quan','2026-12-18 07:30:00', '2026-12-18 17:30:00', N'Hướng dẫn vị trí sơ đồ các gian hàng tuyển dụng');
GO

-- -----------------------------------------------------------------------------------
-- 16. NGÂN SÁCH & CHI PHÍ THỰC TẾ (F17 - ChiPhiSuKien)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, SoTien, NgayChi, NguoiDuyet, GhiChu) VALUES
('CP001', 'EVT202601', N'Thuê mặt bằng Hội trường Grand Ballroom', 45000000, '2026-11-05 10:00:00', 'USR002', N'Thanh toán đợt 1 tiền thuê địa điểm White Palace'),
('CP002', 'EVT202601', N'Hệ thống Âm thanh, Ánh sáng & Màn LED P3', 20000000, '2026-11-20 15:30:00', 'USR002', N'Trọn gói dàn âm thanh biểu diễn chuyên nghiệp'),
('CP003', 'EVT202601', N'In ấn Backdrop, Standee & Thẻ đeo sự kiện', 10000000, '2026-12-01 09:15:00', 'USR002', N'In ấn vật phẩm nhận diện thương hiệu'),

('CP004', 'EVT202602', N'Thuê hệ thống sân khấu ngoài trời & Loa D&B',35000000, '2026-12-01 11:00:00', 'USR002', N'Đặt cọc thiết bị âm thanh biểu diễn đại nhạc hội'),
('CP005', 'EVT202605', N'Thuê phòng hội thảo VIP Bitexco & Teabreak', 15000000, '2026-11-20 14:00:00', 'USR002', N'Chi phí trọn gói phòng hội thảo cao cấp'),
('CP006', 'EVT202606', N'Dàn máy tính thi đấu cấu hình cao i7/RTX4080',40000000, '2026-11-25 16:30:00', 'USR002', N'Thuê 20 máy tính thi đấu chuyên nghiệp'),
('CP007', 'EVT202607', N'Dựng gian hàng tiêu chuẩn & Bạt che SECC',    30000000, '2026-12-05 09:00:00', 'USR002', N'Lắp đặt 60 gian hàng hội chợ việc làm'),
('CP008', 'EVT202608', N'Tiệc Buffet trưa & Teabreak đại biểu VIP',   25000000, '2026-11-10 10:30:00', 'USR002', N'Phục vụ ăn trưa 200 chuyên gia an ninh mạng'),
('CP009', 'EVT202610', N'Trang trí không gian ẩm thực & Cổng chào',   18000000, '2026-10-30 14:15:00', 'USR002', N'Chi phí decor lễ hội đường phố');
GO

-- -----------------------------------------------------------------------------------
-- 17. KHẢO SÁT & ĐÁNH GIÁ PHẢN HỒI (PhanHoi - Phục vụ Tab 9 Form F05)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.PhanHoi (MaPhanHoi, MaSuKien, MaNguoiThamGia, DiemDanhGia, NoiDungPhanHoi, NgayDanhGia) VALUES
('PH001', 'EVT202601', 'NTG003', 5, N'Hội thảo rất quy mô, nội dung diễn giả TS. Nguyễn Văn Hùng chia sẻ cực kỳ thực tế và sâu sắc!', '2026-12-15 17:00:00'),
('PH002', 'EVT202608', 'NTG011', 5, N'Chuyên đề An ninh mạng rất bổ ích, phân tích các vụ tấn công Ransomware thực tế mang tính cảnh tỉnh cao.', '2026-11-15 17:00:00'),
('PH003', 'EVT202610', 'NTG012', 4, N'Không gian lễ hội náo nhiệt, món ăn phong phú và âm nhạc đường phố rất sôi động!', '2026-11-09 22:00:00');
GO

PRINT N'>>> HOÀN THÀNH NẠP DỮ LIỆU MẪU KIỂM THỬ MỞ RỘNG (DataMau.sql) THÀNH CÔNG 100%!';
PRINT N'>>> ĐÃ BỔ SUNG 12 SỰ KIỆN PHONG PHÚ, BAO QUÁT TẤT CẢ CÁC LOẠI HÌNH VÀ TRẠNG THÁI!';
GO
