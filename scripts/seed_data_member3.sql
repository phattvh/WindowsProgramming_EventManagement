-- ==============================================================================
-- SCRIPT DỮ LIỆU MẪU (SEED DATA) - PHÂN HỆ VÉ, ĐIỂM DANH, TÀI CHÍNH & BÁO CÁO
-- Phụ trách: Thành viên 3
-- Dự án: Hệ thống Quản lý Sự kiện (Event Management System)
-- ==============================================================================

USE EventManagementDB;
GO

PRINT N'>>> BẮT ĐẦU NẠP DỮ LIỆU MẪU PHÂN HỆ THÀNH VIÊN 3...';
GO

-- 1. DỮ LIỆU MẪU: HẠNG VÉ (HangVe)
-- Giả định Sự kiện SK001 (Hội thảo Công nghệ 2026) có ngân sách 100.000.000 VNĐ
IF NOT EXISTS (SELECT 1 FROM HangVe WHERE MaHangVe = 'HV001')
BEGIN
    INSERT INTO HangVe (MaHangVe, MaSuKien, TenHangVe, DonGia, SoLuongPhatHanh, SoLuongDaBan, ThoiGianMoBan, ThoiGianDongBan, TrangThai)
    VALUES 
    ('HV001', 'SK001', N'Vé VIP', 500000, 50, 5, '2026-10-01 08:00:00', '2026-11-15 17:00:00', N'Đang mở bán'),
    ('HV002', 'SK001', N'Vé Tiêu chuẩn (Standard)', 200000, 200, 25, '2026-10-01 08:00:00', '2026-11-15 17:00:00', N'Đang mở bán'),
    ('HV003', 'SK001', N'Vé Sinh viên (Student)', 100000, 100, 50, '2026-10-05 08:00:00', '2026-11-10 17:00:00', N'Đang mở bán');
    PRINT N'-> Đã nạp 3 hạng vé mẫu cho sự kiện SK001.';
END
GO

-- 2. DỮ LIỆU MẪU: NGƯỜI THAM GIA (NguoiThamGia)
IF NOT EXISTS (SELECT 1 FROM NguoiThamGia WHERE MaNguoiThamGia = 'NTG001')
BEGIN
    INSERT INTO NguoiThamGia (MaNguoiThamGia, HoTen, GioiTinh, NgaySinh, SoDienThoai, Email, DonVi, DiaChi, TrangThai)
    VALUES 
    ('NTG001', N'Nguyễn Văn An', N'Nam', '1998-05-12', '0912345678', 'an.nguyen@gmail.com', N'Đại học Bách Khoa', N'Quận 10, TP.HCM', N'Hoạt động'),
    ('NTG002', N'Trần Thị Bích', N'Nữ', '2001-08-22', '0987654321', 'bich.tran@gmail.com', N'FPT Software', N'Thành phố Thủ Đức, TP.HCM', N'Hoạt động'),
    ('NTG003', N'Lê Hoàng Nam', N'Nam', '1995-11-30', '0903112233', 'nam.le@techcorp.vn', N'TechCorp VN', N'Quận 3, TP.HCM', N'Hoạt động'),
    ('NTG004', N'Phạm Minh Tuấn', N'Nam', '2000-03-15', '0934556677', 'tuan.pham@student.edu.vn', N'Đại học CNTT', N'Dĩ An, Bình Dương', N'Hoạt động'),
    ('NTG005', N'Hoàng Thu Thảo', N'Nữ', '1999-09-09', '0977889900', 'thao.hoang@gmail.com', N'VNG Corporation', N'Quận 7, TP.HCM', N'Hoạt động');
    PRINT N'-> Đã nạp 5 hồ sơ người tham gia mẫu.';
END
GO

-- 3. DỮ LIỆU MẪU: ĐĂNG KÝ VÉ (DangKy)
-- Giao dịch DK001: Nguyễn Văn An mua 3 vé Tiêu chuẩn (200.000 x 3 = 600.000 VNĐ)
IF NOT EXISTS (SELECT 1 FROM DangKy WHERE MaDangKy = 'DK001')
BEGIN
    INSERT INTO DangKy (MaDangKy, MaSuKien, MaNguoiThamGia, MaHangVe, SoLuong, TongTien, NgayDangKy, TrangThaiThanhToan)
    VALUES 
    ('DK001', 'SK001', 'NTG001', 'HV002', 3, 600000, '2026-10-20 14:30:00', N'Đã thanh toán'),
    ('DK002', 'SK001', 'NTG002', 'HV001', 1, 500000, '2026-10-21 09:15:00', N'Đã thanh toán'),
    ('DK003', 'SK001', 'NTG003', 'HV002', 2, 400000, '2026-10-22 16:45:00', N'Chờ thanh toán');
    PRINT N'-> Đã nạp 3 đơn đăng ký mua vé mẫu.';
END
GO

-- 4. DỮ LIỆU MẪU: CHI TIẾT VÉ (ChiTietVe)
-- Phát hành 3 vé con cho đơn DK001 (mỗi vé có mã QR token GUID độc lập)
IF NOT EXISTS (SELECT 1 FROM ChiTietVe WHERE MaVe = 'VE001')
BEGIN
    INSERT INTO ChiTietVe (MaVe, MaDangKy, MaQR, TenNguoiThamGia, Email, TrangThaiVe)
    VALUES 
    ('VE001', 'DK001', 'A1B2C3D4E5F678901122334455667788', N'Nguyễn Văn An', 'an.nguyen@gmail.com', N'Đã check-in'),
    ('VE002', 'DK001', 'B2C3D4E5F6A178901122334455667799', N'Lê Thị Mai (Khách đi cùng)', 'an.nguyen@gmail.com', N'Chưa sử dụng'),
    ('VE003', 'DK001', 'C3D4E5F6A1B278901122334455667700', N'Nguyễn Hoàng Long (Khách đi cùng)', 'an.nguyen@gmail.com', N'Chưa sử dụng'),
    ('VE004', 'DK002', 'D4E5F6A1B2C378901122334455667711', N'Trần Thị Bích', 'bich.tran@gmail.com', N'Chưa sử dụng');
    PRINT N'-> Đã nạp 4 chi tiết vé phát hành kèm mã QR.';
END
GO

-- 5. DỮ LIỆU MẪU: CHECK-IN SỰ KIỆN (CheckIn)
-- Vé VE001 đã check-in thành công qua Webcam
IF NOT EXISTS (SELECT 1 FROM CheckIn WHERE MaCheckIn = 'CI001')
BEGIN
    INSERT INTO CheckIn (MaCheckIn, MaVe, ThoiGianCheckIn, NguoiCheckIn, PhuongThuc)
    VALUES 
    ('CI001', 'VE001', '2026-11-01 07:45:12', 'USR002', N'Quét QR Webcam');
    PRINT N'-> Đã nạp 1 lượt check-in thành công cho vé VE001.';
END
GO

-- 6. DỮ LIỆU MẪU: NHÀ TÀI TRỢ & CHI TIẾT TÀI TRỢ (NhaTaiTro, ChiTietTaiTro)
IF NOT EXISTS (SELECT 1 FROM NhaTaiTro WHERE MaNhaTaiTro = 'NTT001')
BEGIN
    INSERT INTO NhaTaiTro (MaNhaTaiTro, TenToChuc, NguoiDaiDien, Email, SoDienThoai, DiaChi, GhiChu)
    VALUES 
    ('NTT001', N'Tập đoàn Công nghệ FPT', N'Ông Trương Gia Bình', 'contact@fpt.com.vn', '02473007300', N'Cầu Giấy, Hà Nội', N'Đối tác chiến lược đào tạo & tuyển dụng'),
    ('NTT002', N'Công ty Cổ phần VNG', N'Bà Lê Hồng Minh', 'info@vng.com.vn', '02839623888', N'Quận 7, TP.HCM', N'Tài trợ nền tảng điện toán đám mây');

    INSERT INTO ChiTietTaiTro (MaTaiTro, MaSuKien, MaNhaTaiTro, HangTaiTro, GiaTri, HinhThucTaiTro, QuyenLoiTruyenThong, GhiChu)
    VALUES 
    ('TT001', 'SK001', 'NTT001', N'Kim cương (Diamond)', 50000000, N'Tiền mặt', N'Đặt logo vị trí trung tâm backdrop, phát biểu 10 phút khai mạc', N'Đã giải ngân đợt 1'),
    ('TT002', 'SK001', 'NTT002', N'Vàng (Gold)', 30000000, N'Tiền mặt + Server', N'Đặt logo trên vé điện tử và website sự kiện', N'Cam kết tài trợ hạ tầng');
    PRINT N'-> Đã nạp 2 nhà tài trợ và hợp đồng tài trợ cho SK001.';
END
GO

-- 7. DỮ LIỆU MẪU: CHI PHÍ SỰ KIỆN (ChiPhiSuKien)
-- Ngân sách SK001: 100.000.000 VNĐ. Hiện tại đã chi 35tr + 15tr + 25tr = 75.000.000 VNĐ (75%).
-- Sẵn sàng để test nhập thêm khoản 10.000.000 VNĐ chạm mốc 85% (>80%) kích hoạt cảnh báo đổi màu giao diện (BR-15).
IF NOT EXISTS (SELECT 1 FROM ChiPhiSuKien WHERE MaChiPhi = 'CP001')
BEGIN
    INSERT INTO ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, DienGiai, SoTien, NgayChi, NguoiLapPhieu, DuongDanChungTu, TrangThai)
    VALUES 
    ('CP001', 'SK001', N'Thuê âm thanh & Ánh sáng sân khấu', N'Hệ thống âm thanh hội trường 500 chỗ và dàn đèn LED', 35000000, '2026-10-15', 'USR001', N'/docs/invoices/inv_001.pdf', N'Đã duyệt'),
    ('CP002', 'SK001', N'Tiệc trà Teabreak giữa giờ', N'Phục vụ trà, cà phê, bánh ngọt cho 300 khách', 15000000, '2026-10-18', 'USR001', N'/docs/invoices/inv_002.pdf', N'Đã duyệt'),
    ('CP003', 'SK001', N'In ấn Banner, Backdrop & Thẻ đeo', N'In ấn ấn phẩm truyền thông sự kiện', 25000000, '2026-10-25', 'USR002', N'/docs/invoices/inv_003.pdf', N'Đã duyệt');
    PRINT N'-> Đã nạp 3 phiếu chi sự kiện (Tổng chi hiện tại: 75.000.000 VNĐ = 75% ngân sách).';
END
GO

-- 8. DỮ LIỆU MẪU: PHẢN HỒI ĐÁNH GIÁ (PhanHoi)
IF NOT EXISTS (SELECT 1 FROM PhanHoi WHERE MaPhanHoi = 'PH001')
BEGIN
    INSERT INTO PhanHoi (MaPhanHoi, MaSuKien, MaNguoiThamGia, DiemNoiDung, DiemDienGia, DiemCoSoVatChat, DiemToChuc, DiemDichVu, NhanXet, NgayGui)
    VALUES 
    ('PH001', 'SK001', 'NTG001', 5, 5, 4, 5, 4, N'Sự kiện tổ chức rất chuyên nghiệp, diễn giả chia sẻ nhiều kiến thức thực tế bổ ích!', '2026-11-01 17:30:00');
    PRINT N'-> Đã nạp 1 phản hồi đánh giá mẫu của NTG001.';
END
GO

PRINT N'>>> HOÀN TẤT NẠP DỮ LIỆU MẪU PHÂN HỆ THÀNH VIÊN 3 THÀNH CÔNG!';
GO
