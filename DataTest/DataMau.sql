-- ===================================================================================
-- TẬP TIN THIẾT LẬP MÔI TRƯỜNG KIỂM THỬ: DỮ LIỆU MẪU TOÀN DIỆN (DataMau.sql)
-- DỰ ÁN: PHẦN MỀM QUẢN LÝ SỰ KIỆN (EventManagement)
-- MỤC ĐÍCH: Nạp bộ dữ liệu thử nghiệm chuẩn hóa phục vụ kiểm thử thủ công (Manual Test)
--          bao quát 20 Form (F01 - F20) và 20 Quy tắc nghiệp vụ (BR-01 - BR-20).
-- ===================================================================================

USE EventManagementDB;
GO

SET NOCOUNT ON;

PRINT N'>>> BẮT ĐẦU NẠP DỮ LIỆU MẪU KIỂM THỬ (DataMau.sql)...';

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
-- Ghi chú kiểm thử: LSK01, LSK02, LSK03 có sự kiện liên kết (dùng test chặn xóa BR-07).
--                   LSK04 chưa có sự kiện nào (dùng test xóa thành công).
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.LoaiSuKien (MaLoaiSK, TenLoaiSK, MoTa) VALUES
('LSK01', N'Hội thảo & Hội nghị Công nghệ', N'Chuyên đề chia sẻ kiến thức IT, Trí tuệ nhân tạo và Phần mềm.'),
('LSK02', N'Đại nhạc hội & Lễ hội Âm nhạc', N'Sự kiện văn hóa biểu diễn quy mô lớn ngoài trời.'),
('LSK03', N'Triển lãm Thương mại & Kết nối Doanh nghiệp', N'Không gian trưng bày, kết nối giao thương giữa các doanh nghiệp.'),
('LSK04', N'Giải thi đấu Thể thao Sinh viên', N'Các giải bóng đá, chạy bộ phong trào (Danh mục mẫu để test Thêm/Sửa/Xóa).');
GO

-- -----------------------------------------------------------------------------------
-- 3. DANH MỤC ĐỊA ĐIỂM (F07 - DiaDiem)
-- Ghi chú kiểm thử: DD003 sức chứa nhỏ (50 chỗ) dùng test bán vé vượt sức chứa.
--                   DD001, DD002 có sự kiện xếp lịch (test chống trùng lịch BR-04).
--                   DD004 địa điểm tự do (test thêm/sửa/xóa).
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.DiaDiem (MaDiaDiem, TenDiaDiem, DiaChi, SucChuaToiDa, GhiChu) VALUES
('DD001', N'Trung tâm Hội nghị White Palace', N'194 Hoàng Văn Thụ, Phường 9, Quận Phú Nhuận, TP.HCM', 1500, N'Hội trường Grand Ballroom đầy đủ màn hình LED, âm thanh P.A chuyên nghiệp.'),
('DD002', N'Nhà thi đấu Thể dục Thể thao Phú Thọ', N'1 Lữ Gia, Phường 15, Quận 11, TP.HCM', 5000, N'Không gian quy mô lớn, phù hợp đại nhạc hội hoặc triển lãm công nghệ cao.'),
('DD003', N'Phòng Hội thảo VIP - Bitexco Financial Tower', N'Tầng 25, Tòa nhà Bitexco, Bến Nghé, Quận 1, TP.HCM', 50, N'Phòng họp VIP sức chứa giới hạn 50 người, view toàn cảnh trung tâm.'),
('DD004', N'Trung tâm Sự kiện Riverside Palace', N'360D Bến Vân Đồn, Phường 1, Quận 4, TP.HCM', 800, N'Hội trường tiệc và hội thảo ven sông, không gian hiện đại.');
GO

-- -----------------------------------------------------------------------------------
-- 4. DANH MỤC DIỄN GIẢ / KHÁCH MỜI (F09 - DienGia)
-- Ghi chú: DG001, DG002 đã gắn vào lịch trình sự kiện EVT202601 (test chống trùng lịch BR-06).
--          DG004 diễn giả độc lập (test thêm/sửa/xóa).
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.DienGia (MaDienGia, HoTen, ChucVu, DonViCongTac, Email, SoDienThoai, TieuSu, HinhAnh) VALUES
('DG001', N'TS. Nguyễn Văn Hùng', N'Chuyên gia Trí tuệ Nhân tạo', N'Viện Nghiên cứu Đổi mới Sáng tạo AI', 'hung.nguyen@ai-institute.vn', '0912345678', N'Hơn 15 năm nghiên cứu Machine Learning, LLM và Agentic Coding.', 'dg01_nguyen_van_an.jpg'),
('DG002', N'ThS. Lê Hoàng Mai', N'Giám đốc Kỹ thuật (CTO)', N'TechCorp Asia Group', 'mai.le@techcorp.com', '0987654321', N'Chuyên gia kiến trúc hệ thống phân tán, vi dịch vụ và bảo mật Cloud.', 'dg02_tran_thi_bich.jpg'),
('DG003', N'Bà Trần Thị Kim Cương', N'Giám đốc Phát triển Đối tác', N'Quỹ Đầu tư Khởi nghiệp NextGen', 'cuong.tran@nextgenvc.com', '0933221100', N'Cố vấn chiến lược gọi vốn cho hơn 30 doanh nghiệp công nghệ.', 'dg03_le_hoang_nam.jpg'),
('DG004', N'ThS. Vũ Hải Đăng', N'Kiến trúc sư Giải pháp Phần mềm', N'Global Software Solutions', 'dang.vu@gss.com.vn', '0944556677', N'Diễn giả khách mời (Dùng kiểm thử thao tác thêm, cập nhật, xóa diễn giả).', NULL);
GO

-- -----------------------------------------------------------------------------------
-- 5. DANH MỤC NHÂN SỰ VẬN HÀNH (F14 - NhanSu)
-- Ghi chú: NS001, NS002, NS004 đã được phân công ca trực (dùng test trùng ca trực BR-13).
--          NS005 nhân sự mới chưa phân công (test phân công mới).
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.NhanSu (MaNhanSu, HoTen, SoDienThoai, Email, ChucVu) VALUES
('NS001', N'Trần Minh Tâm', '0933111222', 'tam.tran@eventops.vn', N'Trưởng nhóm điều phối'),
('NS002', N'Lê Thị Thu Thảo', '0944333444', 'thao.le@eventops.vn', N'Điều phối viên Check-in'),
('NS003', N'Nguyễn Quốc Bảo', '0966555666', 'bao.nguyen@eventops.vn', N'Kỹ thuật viên Âm thanh - Ánh sáng'),
('NS004', N'Phạm Quỳnh Như', '0977888999', 'nhu.pham@eventops.vn', N'Lễ tân đón tiếp đại biểu'),
('NS005', N'Đỗ Hải Đăng', '0911223344', 'dang.do@eventops.vn', N'Cộng tác viên hiện trường');
GO

-- -----------------------------------------------------------------------------------
-- 6. DANH MỤC NHÀ TÀI TRỢ (F16 - NhaTaiTro)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.NhaTaiTro (MaNhaTaiTro, TenToChuc, NguoiDaiDien, Email, SoDienThoai, DiaChi) VALUES
('NTT001', N'Tập đoàn Viễn thông & Công nghệ VietTech', N'Ông Phạm Nhật Long', 'contact@viettech.vn', '0283899999', N'Tòa nhà VietTech Tower, Quận 1, TP.HCM'),
('NTT002', N'Ngân hàng Số FinPay Việt Nam', N'Bà Võ Kim Oanh', 'partnership@finpay.vn', '0283788888', N'Khu Đô thị Sala, TP. Thủ Đức, TP.HCM'),
('NTT003', N'Công ty Cổ phần Nước uống Tinh khiết PureWater', N'Ông Nguyễn Tuấn Tú', 'sales@purewater.vn', '0283511111', N'Khu Chế xuất Tân Thuận, Quận 7, TP.HCM'),
('NTT004', N'Doanh nghiệp Khởi nghiệp Công nghệ EcoLife', N'Bà Mai Phương Lan', 'contact@ecolife.vn', '0283622222', N'Công viên Phần mềm Quang Trung, Quận 12, TP.HCM');
GO

-- -----------------------------------------------------------------------------------
-- 7. DANH MỤC SỰ KIỆN ĐA TRẠNG THÁI (F04, F05 - SuKien)
-- Ghi chú kiểm thử:
--   - EVT202601: "Mở đăng ký" -> Sự kiện chính để test bán vé (F11), soát vé (F13), 9 Tabs (F05).
--   - EVT202602: "Dự kiến" -> Đang chuẩn bị, chưa mở bán vé, test chuyển trạng thái.
--   - EVT202603: "Đã kết thúc" -> Test cấm hủy (BR-02), cấm sửa vé, xem báo cáo (F19).
--   - EVT202604: "Hủy" -> Sự kiện đã bị hủy bỏ.
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MoTa, HinhAnh, MaNguoiTao) VALUES
('EVT202601', N'Hội nghị Công nghệ Vietnam Tech Summit 2026', 'LSK01', 'DD001', 
 '2026-12-15 08:00:00', '2026-12-15 17:30:00', N'Mở đăng ký', 
 N'Hội nghị công nghệ thường niên quy tụ hơn 1000 chuyên gia, kỹ sư và lãnh đạo công nghệ hàng đầu.', 
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
 'banner_arvr2026.png', 'USR006');
GO

-- -----------------------------------------------------------------------------------
-- 8. LỊCH TRÌNH TIẾT MỤC (F08 - LichTrinh)
-- Ghi chú: Khung giờ nằm trong sự kiện EVT202601 (08:00 - 17:30 ngày 2026-12-15) - BR-05.
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.LichTrinh (MaLichTrinh, MaSuKien, TenTietMuc, ThoiGianBatDau, ThoiGianKetThuc, MaDienGia, DiaDiemChiTiet, GhiChu) VALUES
('LT001', 'EVT202601', N'Khai mạc & Keynote: Xu hướng GenAI & Agentic Coding 2026', '2026-12-15 08:30:00', '2026-12-15 10:00:00', 'DG001', N'Hội trường chính Grand Ballroom', N'Trình chiếu bài phát biểu khai mạc và demo trực tiếp'),
('LT002', 'EVT202601', N'Tọa đàm: Bảo mật Đám mây & Kiến trúc Microservices', '2026-12-15 10:15:00', '2026-12-15 11:45:00', 'DG002', N'Phòng Hội thảo Lotus 2', N'Phiên trao đổi thảo luận bàn tròn cùng khách mời'),
('LT003', 'EVT202601', N'Workshop: Ứng dụng AI xây dựng ứng dụng Desktop Hiện đại', '2026-12-15 13:30:00', '2026-12-15 15:30:00', 'DG001', N'Hội trường chính Grand Ballroom', N'Thực hành tương tác cùng người tham dự');
GO

-- -----------------------------------------------------------------------------------
-- 9. GÓI TÀI TRỢ SỰ KIỆN (F16 - ChiTietTaiTro)
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.ChiTietTaiTro (MaTaiTro, MaSuKien, MaNhaTaiTro, GoiTaiTro, GiaTriTaiTro, HinhThucTaiTro, GhiChu) VALUES
('TT001', 'EVT202601', 'NTT001', N'Kim Cương', 150000000, N'Tiền mặt', N'Tài trợ độc quyền tiệc Gala Dinner và quà tặng công nghệ'),
('TT002', 'EVT202601', 'NTT002', N'Vàng',       60000000,  N'Tiền mặt', N'Tài trợ truyền thông và cổng thanh toán trực tuyến'),
('TT003', 'EVT202601', 'NTT003', N'Đồng',       15000000,  N'Hiện vật', N'Tài trợ toàn bộ nước uống đóng chai cho sự kiện');
GO

-- -----------------------------------------------------------------------------------
-- 10. HẠNG VÉ & MỞ BÁN (F12 - HangVe)
-- Ghi chú kiểm thử:
--   - HV001: Vé Tiêu chuẩn (200k) -> Phát hành 500, đã bán 5, còn lại 495.
--   - HV002: Vé VIP (800k) -> Phát hành 100, đã bán 2, còn lại 98.
--   - HV003: Vé Sinh viên (50k) -> Phát hành 50, đã bán 50 -> CÒN LẠI 0 (HẾT VÉ / SOLD OUT, dùng test chặn bán vé).
--   - HV004: Vé Miễn phí (0đ) -> Phát hành 30, đã bán 0, còn lại 30.
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.HangVe (MaHangVe, MaSuKien, TenHangVe, GiaVe, SoLuongPhatHanh, SoLuongDaBan, ThoiGianMoBan, ThoiGianDongBan) VALUES
('HV001', 'EVT202601', N'Vé Tiêu chuẩn (Standard)', 200000, 500, 5,  '2026-11-01 00:00:00', '2026-12-14 23:59:59'),
('HV002', 'EVT202601', N'Vé VIP (Full-access)',     800000, 100, 2,  '2026-11-01 00:00:00', '2026-12-14 23:59:59'),
('HV003', 'EVT202601', N'Vé Ưu đãi Sinh viên',      50000,  50,  50, '2026-11-01 00:00:00', '2026-12-10 23:59:59'),
('HV004', 'EVT202601', N'Vé Miễn phí Khách mời VIP',0,       30,  0,  '2026-11-01 00:00:00', '2026-12-14 23:59:59');
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
('NTG008', N'Bùi Thanh Hà',     N'Nữ',  'ha.bui@chotot.vn',        '0908888999', N'Chợ Tốt (Carousell Group)');
GO

-- -----------------------------------------------------------------------------------
-- 12. ĐĂNG KÝ MUA VÉ (F11 - DangKy)
-- Ghi chú kiểm thử:
--   - DK001: Mua 2 vé HV001 (Đã thanh toán) -> Sinh VE001_1 và VE001_2.
--   - DK002: Mua 1 vé HV002 (Đã thanh toán) -> Sinh VE002_1.
--   - DK003: Mua 1 vé HV002 (Đã thanh toán) -> Sinh VE003_1 (ĐÃ CHECK-IN).
--   - DK004: Mua 2 vé HV001 (Đã thanh toán) -> Sinh VE004_1 và VE004_2.
--   - DK005: Mua 1 vé HV001 (Chưa thanh toán) -> Test lọc đơn chưa thanh toán.
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.DangKy (MaDangKy, MaSuKien, MaNguoiThamGia, MaHangVe, NgayDangKy, SoLuong, TongTien, TrangThaiThanhToan, PhuongThucThanhToan) VALUES
('DK001', 'EVT202601', 'NTG001', 'HV001', '2026-11-10 09:15:00', 2, 400000, N'Đã thanh toán',   N'Tiền mặt tại quầy'),
('DK002', 'EVT202601', 'NTG002', 'HV002', '2026-11-11 14:20:00', 1, 800000, N'Đã thanh toán',   N'Chuyển khoản QR'),
('DK003', 'EVT202601', 'NTG003', 'HV002', '2026-11-12 10:05:00', 1, 800000, N'Đã thanh toán',   N'Chuyển khoản QR'),
('DK004', 'EVT202601', 'NTG004', 'HV001', '2026-11-15 16:45:00', 2, 400000, N'Đã thanh toán',   N'Thẻ ngân hàng'),
('DK005', 'EVT202601', 'NTG005', 'HV001', '2026-11-20 11:30:00', 1, 200000, N'Chưa thanh toán', N'Chưa thanh toán');
GO

-- -----------------------------------------------------------------------------------
-- 13. CHI TIẾT VÉ & MÃ QR CODE (F11, F13 - ChiTietVe)
-- Ghi chú kiểm thử:
--   - VE001_1, VE001_2, VE002_1, VE004_1, VE004_2: Trạng thái "Hợp lệ" (Sẵn sàng quét Check-in).
--   - VE003_1: Trạng thái "Đã check-in" -> DÙNG ĐỂ TEST QUÉT LẠI MÃ QR ĐÃ CHECK-IN (BÁO ĐỎ THEO BR-12).
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.ChiTietVe (MaVe, MaDangKy, MaQR, TenNguoiThamGia, Email, TrangThaiVe) VALUES
('VE001_1', 'DK001', 'QR_EVT202601_001_1', N'Nguyễn Văn An',  'an.nguyen@fpt.com',    N'Hợp lệ'),
('VE001_2', 'DK001', 'QR_EVT202601_001_2', N'Nguyễn Văn Bình', 'binh.nguyen@fpt.com',  N'Hợp lệ'),
('VE002_1', 'DK002', 'QR_EVT202601_002_1', N'Lê Thị Mai',     'mai.le@techstart.vn',  N'Hợp lệ'),
('VE003_1', 'DK003', 'QR_EVT202601_003_1', N'Trần Quốc Tuấn', 'tuan.tran@vng.com.vn', N'Đã check-in'),
('VE004_1', 'DK004', 'QR_EVT202601_004_1', N'Phạm Thu Trang', 'trang.pham@vnpay.vn',  N'Hợp lệ'),
('VE004_2', 'DK004', 'QR_EVT202601_004_2', N'Đỗ Minh Quang',  'quang.do@vnpay.vn',    N'Hợp lệ');
GO

-- -----------------------------------------------------------------------------------
-- 14. SOÁT VÉ CHECK-IN (F13 - CheckIn)
-- Ghi chú: Bản ghi này chứng minh vé VE003_1 đã check-in lúc 08:15:00 bởi nhân viên staff1 (USR003).
--          Khi người test quét lại mã QR_EVT202601_003_1, phần mềm sẽ hiển thị cảnh báo:
--          "Vé này đã được check-in vào lúc 2026-12-15 08:15:00 bởi staff1" (BR-12).
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.CheckIn (MaCheckIn, MaVe, ThoiGianCheckIn, NguoiThucHien, HinhThucCheckIn, GhiChu) VALUES
('CK001', 'VE003_1', '2026-12-15 08:15:00', 'USR003', N'Webcam QR Code', N'Check-in hợp lệ tại Cổng chính A');
GO

-- -----------------------------------------------------------------------------------
-- 15. PHÂN CÔNG NHÂN SỰ (F15 - PhanCongNhanSu)
-- Ghi chú kiểm thử: NS001 trực cả ngày (07:00 - 18:00); NS002 trực buổi sáng (07:30 - 12:00).
--                   Dùng test gán thêm ca trùng giờ cho NS002 (hệ thống phải chặn theo BR-13).
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.PhanCongNhanSu (MaPhanCong, MaSuKien, MaNhanSu, NhiemVu, ThoiGianBatDau, ThoiGianKetThuc, GhiChu) VALUES
('PC001', 'EVT202601', 'NS001', N'Trưởng nhóm điều phối hiện trường', '2026-12-15 07:00:00', '2026-12-15 18:00:00', N'Phụ trách tổng thể điều hành khu vực hội trường chính'),
('PC002', 'EVT202601', 'NS002', N'Soát vé Check-in Cổng A',          '2026-12-15 07:30:00', '2026-12-15 12:00:00', N'Trực thiết bị quét mã QR cổng vào chính'),
('PC003', 'EVT202601', 'NS004', N'Lễ tân đón tiếp khách mời VIP',    '2026-12-15 08:00:00', '2026-12-15 12:00:00', N'Đón tiếp diễn giả và trao thẻ đại biểu');
GO

-- -----------------------------------------------------------------------------------
-- 16. NGÂN SÁCH & CHI PHÍ THỰC TẾ (F17 - ChiPhiSuKien)
-- Ghi chú kiểm thử: Ngân sách dự kiến của EVT202601 là 100,000,000đ.
--   - CP001: 45,000,000đ (Thuê mặt bằng)
--   - CP002: 20,000,000đ (Âm thanh, ánh sáng, màn hình LED)
--   - CP003: 10,000,000đ (In ấn Backdrop, Standee, thẻ đeo)
--   => TỔNG CHI HIỆN TẠI = 75,000,000đ (Chiếm đúng 75% ngân sách, mức AN TOÀN).
--   => Khi người test vào F17 thêm một khoản chi >= 5,000,000đ (ví dụ 6tr -> tổng 81tr = 81%),
--      THANH TIẾN ĐỘ VÀ GIAO DIỆN SẼ LẬP TỨC ĐỔI MÀU VÀNG CẢNH BÁO BỘI CHI (BR-15)!
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, SoTien, NgayChi, NguoiDuyet, GhiChu) VALUES
('CP001', 'EVT202601', N'Thuê mặt bằng Hội trường Grand Ballroom', 45000000, '2026-11-05 10:00:00', 'USR002', N'Thanh toán đợt 1 tiền thuê địa điểm White Palace'),
('CP002', 'EVT202601', N'Hệ thống Âm thanh, Ánh sáng & Màn LED P3', 20000000, '2026-11-20 15:30:00', 'USR002', N'Trọn gói dàn âm thanh biểu diễn chuyên nghiệp'),
('CP003', 'EVT202601', N'In ấn Backdrop, Standee & Thẻ đeo sự kiện', 10000000, '2026-12-01 09:15:00', 'USR002', N'In ấn vật phẩm nhận diện thương hiệu');
GO

-- -----------------------------------------------------------------------------------
-- 17. KHẢO SÁT & ĐÁNH GIÁ PHẢN HỒI (F18 - PhanHoi)
-- Ghi chú kiểm thử:
--   - NTG003 (Trần Quốc Tuấn) ĐÃ CHECK-IN -> Đã gửi đánh giá 5 sao.
--   - Dùng để test: NTG003 gửi thêm phản hồi lần 2 sẽ bị chặn (BR-16).
--   - Dùng để test: NTG001 (chưa check-in) cố tình gửi phản hồi sẽ bị chặn (BR-16).
-- -----------------------------------------------------------------------------------
INSERT INTO dbo.PhanHoi (MaPhanHoi, MaSuKien, MaNguoiThamGia, DiemDanhGia, NoiDungPhanHoi, NgayDanhGia) VALUES
('PH001', 'EVT202601', 'NTG003', 5, N'Hội thảo rất quy mô, nội dung diễn giả TS. Nguyễn Văn Hùng chia sẻ cực kỳ thực tế và sâu sắc!', '2026-12-15 17:00:00');
GO

PRINT N'>>> HOÀN THÀNH NẠP DỮ LIỆU MẪU KIỂM THỬ (DataMau.sql) THÀNH CÔNG 100%!';
GO
