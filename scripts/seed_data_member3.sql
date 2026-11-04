-- ==============================================================================
-- SCRIPT DỮ LIỆU MẪU (SEED DATA) - PHÂN HỆ VÉ, ĐIỂM DANH, TÀI CHÍNH & BÁO CÁO
-- Phụ trách: Thành viên 3 (Thuật toán lõi, Bán vé & Báo cáo)
-- Dự án: Hệ thống Quản lý Sự kiện (Event Management System)
-- Khớp 100% CSDL 01_Database_Schema.sql và liên kết trực tiếp với EVT202601, EVT202602
-- ==============================================================================

USE EventManagementDB;
GO

PRINT N'>>> BẮT ĐẦU NẠP DỮ LIỆU MẪU PHÂN HỆ THÀNH VIÊN 3 (VÉ, CHECK-IN & TÀI CHÍNH)...';
GO

-- 1. DỮ LIỆU MẪU: HẠNG VÉ (HangVe)
-- Bổ sung hạng vé cho sự kiện EVT202602 (Workshop GenAI - Đang diễn ra) để test Check-in F13
IF NOT EXISTS (SELECT 1 FROM dbo.HangVe WHERE MaHangVe = 'HV003')
BEGIN
    INSERT INTO dbo.HangVe (MaHangVe, MaSuKien, TenHangVe, GiaVe, SoLuongPhatHanh, SoLuongDaBan, ThoiGianMoBan, ThoiGianDongBan)
    VALUES 
    ('HV003', 'EVT202602', N'Vé Thực hành Lab (Standard)', 300000, 100, 20, '2026-10-01 00:00:00', '2026-11-02 23:59:59'),
    ('HV004', 'EVT202602', N'Vé VIP Mentoring 1-1',       600000,  50, 10, '2026-10-01 00:00:00', '2026-11-02 23:59:59');
    PRINT N'-> Đã nạp 2 hạng vé mẫu (HV003, HV004) cho sự kiện đang diễn ra EVT202602.';
END
GO

-- 2. DỮ LIỆU MẪU: NGƯỜI THAM GIA (NguoiThamGia)
IF NOT EXISTS (SELECT 1 FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG003')
BEGIN
    INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, GioiTinh, Email, SoDienThoai, DonViCongTac)
    VALUES 
    ('NTG003', N'Lê Hoàng Nam',   N'Nam', 'nam.le@techcorp.vn',       '0903112233', N'TechCorp VN'),
    ('NTG004', N'Phạm Minh Tuấn', N'Nam', 'tuan.pham@student.edu.vn', '0934556677', N'Đại học CNTT - ĐHQG'),
    ('NTG005', N'Hoàng Thu Thảo', N'Nữ',  'thao.hoang@vng.com.vn',    '0977889900', N'VNG Corporation');
    PRINT N'-> Đã nạp bổ sung 3 hồ sơ người tham gia mẫu (NTG003 - NTG005).';
END
GO

-- 3. DỮ LIỆU MẪU: ĐĂNG KÝ VÉ (DangKy) - HỖ TRỢ MUA TỪ 1 ĐẾN 10 VÉ (BR-10)
-- Đơn DK001: Nguyễn Văn An mua 3 vé Thực hành Lab (300.000 x 3 = 900.000 VNĐ)
IF NOT EXISTS (SELECT 1 FROM dbo.DangKy WHERE MaDangKy = 'DK001')
BEGIN
    INSERT INTO dbo.DangKy (MaDangKy, MaSuKien, MaNguoiThamGia, MaHangVe, NgayDangKy, SoLuong, TongTien, TrangThaiThanhToan, PhuongThucThanhToan)
    VALUES 
    ('DK001', 'EVT202602', 'NTG001', 'HV003', '2026-10-25 14:30:00', 3, 900000, N'Đã thanh toán', N'Chuyển khoản'),
    ('DK002', 'EVT202602', 'NTG002', 'HV004', '2026-10-26 09:15:00', 1, 600000, N'Đã thanh toán', N'Ví MoMo'),
    ('DK003', 'EVT202602', 'NTG003', 'HV003', '2026-10-28 16:45:00', 2, 600000, N'Chưa thanh toán', N'Tiền mặt');
    PRINT N'-> Đã nạp 3 đơn đăng ký mua vé mẫu cho sự kiện EVT202602.';
END
GO

-- 4. DỮ LIỆU MẪU: CHI TIẾT VÉ (ChiTietVe) - MỖI VÉ 1 MÃ QR ĐỘC LẬP
-- Đơn DK001 sinh ra 3 vé con: VE001 (đã check-in), VE002 và VE003 (chưa sử dụng, sẵn sàng quét webcam QR)
IF NOT EXISTS (SELECT 1 FROM dbo.ChiTietVe WHERE MaVe = 'VE001')
BEGIN
    INSERT INTO dbo.ChiTietVe (MaVe, MaDangKy, MaQR, TenNguoiThamGia, Email, TrangThaiVe)
    VALUES 
    ('VE001', 'DK001', 'EVT2-QR-AN-001',   N'Nguyễn Văn An',         'an.nguyen@gmail.com',  N'Đã check-in'),
    ('VE002', 'DK001', 'EVT2-QR-MAI-002',  N'Lê Thị Mai (Khách kèm)', 'an.nguyen@gmail.com',  N'Hợp lệ'),
    ('VE003', 'DK001', 'EVT2-QR-LONG-003', N'Nguyễn Hoàng Long',     'long.nguyen@gmail.com', N'Hợp lệ'),
    ('VE004', 'DK002', 'EVT2-QR-BICH-004', N'Trần Thị Bích',         'bich.tran@gmail.com',  N'Hợp lệ');
    PRINT N'-> Đã nạp 4 chi tiết vé phát hành kèm mã QR token độc lập (phục vụ test Webcam F13).';
END
GO

-- 5. DỮ LIỆU MẪU: CHECK-IN SỰ KIỆN (CheckIn - BR-11, BR-12)
-- Vé VE001 đã check-in thành công qua Webcam, nếu quét lại sẽ báo vé gian lận đã sử dụng
IF NOT EXISTS (SELECT 1 FROM dbo.CheckIn WHERE MaCheckIn = 'CI001')
BEGIN
    INSERT INTO dbo.CheckIn (MaCheckIn, MaVe, ThoiGianCheckIn, NguoiThucHien, HinhThucCheckIn, GhiChu)
    VALUES 
    ('CI001', 'VE001', '2026-11-03 08:15:00', 'USR001', N'Webcam QR Code', N'Check-in cửa chính phòng Lab A');
    PRINT N'-> Đã nạp 1 lượt check-in mẫu cho vé VE001 (kiểm tra chống quét vé lần 2 BR-12).';
END
GO

-- 6. DỮ LIỆU MẪU: CHI PHÍ SỰ KIỆN (ChiPhiSuKien) - PHỤC VỤ TEST CẢNH BÁO CHI 80% (BR-15)
-- Dự toán tài trợ EVT202601: 210.000.000 VNĐ.
-- Hiện tại đã chi: 35tr + 15tr + 25tr = 75.000.000 VNĐ.
-- Sẵn sàng để demo nhập thêm khoản chi chạm ngưỡng 80% để thanh đo chuyển sang màu vàng cam!
IF NOT EXISTS (SELECT 1 FROM dbo.ChiPhiSuKien WHERE MaChiPhi = 'CP001')
BEGIN
    INSERT INTO dbo.ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, SoTien, NgayChi, NguoiDuyet, GhiChu)
    VALUES 
    ('CP001', 'EVT202601', N'Thuê âm thanh & Ánh sáng sân khấu Grand Ballroom', 35000000, '2026-10-15', 'USR001', N'Hợp đồng âm thanh sự kiện'),
    ('CP002', 'EVT202601', N'Tiệc trà Teabreak đón tiếp khách mời VIP',        15000000, '2026-10-18', 'USR001', N'Bao gồm bánh ngọt, trà và cafe'),
    ('CP003', 'EVT202601', N'In ấn Backdrop, Standee & Thẻ đeo đại biểu',      25000000, '2026-10-25', 'USR001', N'In ấn ấn phẩm nhận diện thương hiệu');
    PRINT N'-> Đã nạp 3 phiếu chi sự kiện cho EVT202601 (phục vụ test cảnh báo ngân sách BR-15).';
END
GO

PRINT N'==============================================================================';
PRINT N'>> [THÀNH CÔNG] ĐÃ NẠP TOÀN BỘ SEED DATA PHÂN HỆ VÉ & TÀI CHÍNH CỦA THÀNH VIÊN 3!';
PRINT N'>> Mã QR mẫu sẵn sàng quét webcam: EVT2-QR-MAI-002, EVT2-QR-LONG-003, EVT2-QR-BICH-004';
PRINT N'==============================================================================';
GO
