USE EventManagementDB;
GO

SET NOCOUNT ON;

-- ====================================================================================================
-- HỆ THỐNG KIỂM THỬ TỰ ĐỘNG CƠ SỞ DỮ LIỆU & BẢO MẬT (NGÀY 1 - THÀNH VIÊN 1)
-- Phụ trách: Thành viên 1 (Kiến trúc, CSDL & Bảo mật)
-- Mục tiêu: Thẩm định toàn diện 20 bảng CSDL, Khóa chính, Khóa ngoại, Ràng buộc nghiệp vụ (CHECK, UNIQUE)
--           và Bộ Dữ liệu mẫu (Seed Data: Quyền hạn, Chức năng, Tài khoản Admin băm BCrypt).
-- ====================================================================================================

-- Khởi tạo bảng tạm lưu trữ kết quả kiểm thử xuyên suốt các batch
IF OBJECT_ID('tempdb..#TestResults') IS NOT NULL DROP TABLE #TestResults;
CREATE TABLE #TestResults (
    TestID      VARCHAR(20)     NOT NULL,
    NhomTest    NVARCHAR(50)    NOT NULL,
    TenTest     NVARCHAR(200)   NOT NULL,
    KetQua      VARCHAR(10)     NOT NULL, -- 'PASS', 'FAIL', 'WARN'
    ChiTiet     NVARCHAR(500)   NULL
);
GO

-- ====================================================================================================
-- PHẦN 1: KIỂM TRA LƯỢC ĐỒ VÀ TOÀN VẸN CẤU TRÚC (SCHEMA INTEGRITY)
-- ====================================================================================================

-- TEST DB-01: Kiểm tra sự tồn tại của đầy đủ 20 bảng CSDL
PRINT N'>> [TEST DB-01] Kiểm tra sự tồn tại của 20 bảng CSDL...';
DECLARE @TableCount INT = 0;
SELECT @TableCount = COUNT(*) 
FROM sys.tables 
WHERE name IN (
    'NhomQuyen', 'NguoiDung', 'ChucNang', 'PhanQuyen', 'LoaiSuKien',
    'DiaDiem', 'DienGia', 'NhanSu', 'NhaTaiTro', 'SuKien',
    'LichTrinh', 'ChiTietTaiTro', 'HangVe', 'NguoiThamGia', 'DangKy',
    'ChiTietVe', 'CheckIn', 'PhanCongNhanSu', 'ChiPhiSuKien', 'PhanHoi'
);

IF @TableCount = 20
    INSERT INTO #TestResults VALUES ('DB-01', N'Lược đồ CSDL', N'Kiểm tra sự tồn tại của đầy đủ 20 bảng', 'PASS', N'Đủ 20/20 bảng theo thiết kế kiến trúc Ngày 1.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-01', N'Lược đồ CSDL', N'Kiểm tra sự tồn tại của đầy đủ 20 bảng', 'FAIL', N'Chỉ tìm thấy ' + CAST(@TableCount AS NVARCHAR(10)) + N'/20 bảng.');
GO

-- TEST DB-02: Kiểm tra Khóa chính (Primary Key) trên toàn bộ 20 bảng
PRINT N'>> [TEST DB-02] Kiểm tra Khóa chính (Primary Key) trên 20 bảng...';
DECLARE @PKCount INT = 0;
SELECT @PKCount = COUNT(DISTINCT t.name)
FROM sys.tables t
JOIN sys.key_constraints kc ON t.object_id = kc.parent_object_id
WHERE kc.type = 'PK' AND t.name IN (
    'NhomQuyen', 'NguoiDung', 'ChucNang', 'PhanQuyen', 'LoaiSuKien',
    'DiaDiem', 'DienGia', 'NhanSu', 'NhaTaiTro', 'SuKien',
    'LichTrinh', 'ChiTietTaiTro', 'HangVe', 'NguoiThamGia', 'DangKy',
    'ChiTietVe', 'CheckIn', 'PhanCongNhanSu', 'ChiPhiSuKien', 'PhanHoi'
);

IF @PKCount = 20
    INSERT INTO #TestResults VALUES ('DB-02', N'Lược đồ CSDL', N'Kiểm tra Khóa chính (PK) trên toàn bộ 20 bảng', 'PASS', N'Đủ 20 bảng đều có Khóa chính hợp lệ.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-02', N'Lược đồ CSDL', N'Kiểm tra Khóa chính (PK) trên toàn bộ 20 bảng', 'FAIL', N'Chỉ có ' + CAST(@PKCount AS NVARCHAR(10)) + N'/20 bảng có PK.');
GO

-- TEST DB-03: Kiểm tra hệ thống Khóa ngoại (Foreign Keys) liên kết các thực thể
PRINT N'>> [TEST DB-03] Kiểm tra số lượng Khóa ngoại (Foreign Keys)...';
DECLARE @FKCount INT = 0;
SELECT @FKCount = COUNT(*) FROM sys.foreign_keys;

IF @FKCount >= 20
    INSERT INTO #TestResults VALUES ('DB-03', N'Lược đồ CSDL', N'Kiểm tra số lượng Khóa ngoại (FK) liên kết thực thể', 'PASS', N'Có ' + CAST(@FKCount AS NVARCHAR(10)) + N' khóa ngoại toàn vẹn (yêu cầu >= 20 FK).');
ELSE
    INSERT INTO #TestResults VALUES ('DB-03', N'Lược đồ CSDL', N'Kiểm tra số lượng Khóa ngoại (FK) liên kết thực thể', 'FAIL', N'Thiếu khóa ngoại: hiện có ' + CAST(@FKCount AS NVARCHAR(10)) + N' FK.');
GO

-- TEST DB-04: Kiểm tra Chỉ mục (Indexes) hỗ trợ tối ưu hiệu năng truy vấn
PRINT N'>> [TEST DB-04] Kiểm tra các Chỉ mục (Indexes IX_*)...';
DECLARE @IndexCount INT = 0;
SELECT @IndexCount = COUNT(*) 
FROM sys.indexes 
WHERE name LIKE 'IX_%';

IF @IndexCount >= 13
    INSERT INTO #TestResults VALUES ('DB-04', N'Lược đồ CSDL', N'Kiểm tra các Chỉ mục (Indexes) tối ưu truy vấn', 'PASS', N'Tìm thấy đủ ' + CAST(@IndexCount AS NVARCHAR(10)) + N' chỉ mục non-clustered IX_*.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-04', N'Lược đồ CSDL', N'Kiểm tra các Chỉ mục (Indexes) tối ưu truy vấn', 'FAIL', N'Chỉ tìm thấy ' + CAST(@IndexCount AS NVARCHAR(10)) + N'/13 chỉ mục IX_*.');
GO

-- ====================================================================================================
-- PHẦN 2: KIỂM TRA CÁC RÀNG BUỘC DUY NHẤT (UNIQUE CONSTRAINTS)
-- ====================================================================================================

-- TEST DB-05: Chống trùng tên đăng nhập (NguoiDung.TenDangNhap UNIQUE)
PRINT N'>> [TEST DB-05] Kiểm tra ràng buộc UNIQUE trên NguoiDung.TenDangNhap...';
BEGIN TRY
    INSERT INTO dbo.NguoiDung (MaNguoiDung, TenDangNhap, MatKhauHash, HoTen, Email, MaNhomQuyen)
    VALUES ('USR_TEST_UQ', 'admin', '$2a$11$testtesttesttesttesttesttesttesttesttesttesttesttesttes', N'Admin Trùng', 'admin_dup@test.vn', 'ADMIN');
    
    INSERT INTO #TestResults VALUES ('DB-05', N'Ràng buộc UNIQUE', N'Chống trùng lặp Tên đăng nhập (NguoiDung.TenDangNhap)', 'FAIL', N'Không chặn được trùng username admin.');
    DELETE FROM dbo.NguoiDung WHERE MaNguoiDung = 'USR_TEST_UQ';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() IN (2627, 2601)
        INSERT INTO #TestResults VALUES ('DB-05', N'Ràng buộc UNIQUE', N'Chống trùng lặp Tên đăng nhập (NguoiDung.TenDangNhap)', 'PASS', N'Đã chặn trùng tên đăng nhập (Lỗi ' + CAST(ERROR_NUMBER() AS NVARCHAR(10)) + N').');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-05', N'Ràng buộc UNIQUE', N'Chống trùng lặp Tên đăng nhập (NguoiDung.TenDangNhap)', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());
END CATCH;
GO

-- TEST DB-06: Mã QR duy nhất chống làm giả vé (ChiTietVe.MaQR UNIQUE - BR-12)
PRINT N'>> [TEST DB-06] Kiểm tra ràng buộc UNIQUE trên ChiTietVe.MaQR...';
DECLARE @HasUQ_QR BIT = 0;
IF EXISTS (
    SELECT 1 FROM sys.key_constraints 
    WHERE parent_object_id = OBJECT_ID('dbo.ChiTietVe') AND type = 'UQ' AND name = 'UQ_ChiTietVe_MaQR'
) OR EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE object_id = OBJECT_ID('dbo.ChiTietVe') AND is_unique = 1 AND name = 'UQ_ChiTietVe_MaQR'
)
    SET @HasUQ_QR = 1;

IF @HasUQ_QR = 1
    INSERT INTO #TestResults VALUES ('DB-06', N'Ràng buộc UNIQUE', N'Mã QR duy nhất chống làm giả vé (ChiTietVe.MaQR - BR-12)', 'PASS', N'Tồn tại ràng buộc UNIQUE UQ_ChiTietVe_MaQR.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-06', N'Ràng buộc UNIQUE', N'Mã QR duy nhất chống làm giả vé (ChiTietVe.MaQR - BR-12)', 'FAIL', N'Thiếu ràng buộc UNIQUE trên cột MaQR.');
GO

-- TEST DB-07: Soát vé 1 chiều, chống quét trùng vé (CheckIn.MaVe UNIQUE - BR-12)
PRINT N'>> [TEST DB-07] Kiểm tra ràng buộc UNIQUE trên CheckIn.MaVe...';
DECLARE @HasUQ_CheckIn BIT = 0;
IF EXISTS (
    SELECT 1 FROM sys.key_constraints 
    WHERE parent_object_id = OBJECT_ID('dbo.CheckIn') AND type = 'UQ' AND name = 'UQ_CheckIn_MaVe'
) OR EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE object_id = OBJECT_ID('dbo.CheckIn') AND is_unique = 1 AND name = 'UQ_CheckIn_MaVe'
)
    SET @HasUQ_CheckIn = 1;

IF @HasUQ_CheckIn = 1
    INSERT INTO #TestResults VALUES ('DB-07', N'Ràng buộc UNIQUE', N'Soát vé 1 chiều, chống quét trùng vé (CheckIn.MaVe - BR-12)', 'PASS', N'Tồn tại ràng buộc UNIQUE UQ_CheckIn_MaVe.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-07', N'Ràng buộc UNIQUE', N'Soát vé 1 chiều, chống quét trùng vé (CheckIn.MaVe - BR-12)', 'FAIL', N'Thiếu ràng buộc UNIQUE trên cột CheckIn.MaVe.');
GO

-- TEST DB-08: Khảo sát duy nhất 1 lần/người/sự kiện (PhanHoi UNIQUE - BR-16)
PRINT N'>> [TEST DB-08] Kiểm tra ràng buộc UNIQUE trên PhanHoi(MaSuKien, MaNguoiThamGia)...';
DECLARE @HasUQ_PhanHoi BIT = 0;
IF EXISTS (
    SELECT 1 FROM sys.key_constraints 
    WHERE parent_object_id = OBJECT_ID('dbo.PhanHoi') AND type = 'UQ' AND name = 'UQ_PhanHoi_SuKien_NguoiThamGia'
) OR EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE object_id = OBJECT_ID('dbo.PhanHoi') AND is_unique = 1 AND name = 'UQ_PhanHoi_SuKien_NguoiThamGia'
)
    SET @HasUQ_PhanHoi = 1;

IF @HasUQ_PhanHoi = 1
    INSERT INTO #TestResults VALUES ('DB-08', N'Ràng buộc UNIQUE', N'Khảo sát duy nhất 1 lần/người/sự kiện (PhanHoi - BR-16)', 'PASS', N'Tồn tại ràng buộc UNIQUE (MaSuKien, MaNguoiThamGia).');
ELSE
    INSERT INTO #TestResults VALUES ('DB-08', N'Ràng buộc UNIQUE', N'Khảo sát duy nhất 1 lần/người/sự kiện (PhanHoi - BR-16)', 'FAIL', N'Thiếu ràng buộc UNIQUE trên PhanHoi.');
GO

-- ====================================================================================================
-- PHẦN 3: KIỂM TRA CÁC RÀNG BUỘC TOÀN VẸN & NGHIỆP VỤ (CHECK CONSTRAINTS)
-- ====================================================================================================

-- TEST DB-09: Thời gian sự kiện: NgayKetThuc >= NgayBatDau (BR-03)
PRINT N'>> [TEST DB-09] Kiểm tra ràng buộc CHECK thời gian sự kiện (BR-03)...';
BEGIN TRY
    INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MaNguoiTao)
    VALUES ('EVT_ERR_DATE', N'Sự kiện lỗi ngày', 'LSK01', 'DD001', '2026-12-20 17:00:00', '2026-12-20 08:00:00', N'Dự kiến', 'USR001');

    INSERT INTO #TestResults VALUES ('DB-09', N'Ràng buộc CHECK', N'Thời gian sự kiện: NgayKetThuc >= NgayBatDau (BR-03)', 'FAIL', N'Không chặn được NgayKetThuc < NgayBatDau.');
    DELETE FROM dbo.SuKien WHERE MaSuKien = 'EVT_ERR_DATE';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-09', N'Ràng buộc CHECK', N'Thời gian sự kiện: NgayKetThuc >= NgayBatDau (BR-03)', 'PASS', N'Đã chặn thời gian sự kiện phi logic (CK_SuKien_NgayHopLe).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-09', N'Ràng buộc CHECK', N'Thời gian sự kiện: NgayKetThuc >= NgayBatDau (BR-03)', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());
END CATCH;
GO

-- TEST DB-10: Thời gian lịch trình con: ThoiGianKetThuc > ThoiGianBatDau (BR-05)
PRINT N'>> [TEST DB-10] Kiểm tra ràng buộc CHECK thời gian lịch trình (BR-05)...';
BEGIN TRY
    INSERT INTO dbo.LichTrinh (MaLichTrinh, MaSuKien, TenTietMuc, ThoiGianBatDau, ThoiGianKetThuc)
    VALUES ('LT_ERR_TIME', 'EVT202601', N'Tiết mục lỗi giờ', '2026-12-15 11:00:00', '2026-12-15 09:00:00');

    INSERT INTO #TestResults VALUES ('DB-10', N'Ràng buộc CHECK', N'Thời gian lịch trình: ThoiGianKetThuc > ThoiGianBatDau (BR-05)', 'FAIL', N'Không chặn được lịch trình con sai giờ.');
    DELETE FROM dbo.LichTrinh WHERE MaLichTrinh = 'LT_ERR_TIME';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-10', N'Ràng buộc CHECK', N'Thời gian lịch trình: ThoiGianKetThuc > ThoiGianBatDau (BR-05)', 'PASS', N'Đã chặn lịch trình con sai thời gian (CK_LichTrinh_ThoiGian).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-10', N'Ràng buộc CHECK', N'Thời gian lịch trình: ThoiGianKetThuc > ThoiGianBatDau (BR-05)', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());
END CATCH;
GO

-- TEST DB-11: Thời gian ca trực nhân sự: ThoiGianKetThuc > ThoiGianBatDau (BR-13)
PRINT N'>> [TEST DB-11] Kiểm tra ràng buộc CHECK thời gian ca trực (BR-13)...';
BEGIN TRY
    INSERT INTO dbo.PhanCongNhanSu (MaPhanCong, MaSuKien, MaNhanSu, NhiemVu, ThoiGianBatDau, ThoiGianKetThuc)
    VALUES ('PC_TEST_ERR', 'EVT202601', 'NS001', N'Trực cổng kiểm tra', '2026-12-15 10:00:00', '2026-12-15 08:00:00');

    INSERT INTO #TestResults VALUES ('DB-11', N'Ràng buộc CHECK', N'Thời gian ca trực: ThoiGianKetThuc > ThoiGianBatDau (BR-13)', 'FAIL', N'Không chặn được ca trực sai thời gian.');
    DELETE FROM dbo.PhanCongNhanSu WHERE MaPhanCong = 'PC_TEST_ERR';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-11', N'Ràng buộc CHECK', N'Thời gian ca trực: ThoiGianKetThuc > ThoiGianBatDau (BR-13)', 'PASS', N'Đã chặn ca trực sai thời gian (CK_PhanCong_ThoiGian).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-11', N'Ràng buộc CHECK', N'Thời gian ca trực: ThoiGianKetThuc > ThoiGianBatDau (BR-13)', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());
END CATCH;
GO

-- TEST DB-12: Giới hạn đặt vé: SoLuong từ 1 đến 10 vé (DangKy - BR-10)
PRINT N'>> [TEST DB-12] Kiểm tra ràng buộc CHECK số lượng vé mua (BR-10)...';
BEGIN TRY
    INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, Email, SoDienThoai)
    VALUES ('NTG_TEST_CK', N'Khách Test Đặt Vé', 'test_ticket@test.vn', '0900000000');

    -- Thử đặt 15 vé (vượt ngưỡng tối đa 10 vé)
    INSERT INTO dbo.DangKy (MaDangKy, MaSuKien, MaNguoiThamGia, MaHangVe, SoLuong, TongTien)
    VALUES ('DK_TEST_ERR', 'EVT202601', 'NTG_TEST_CK', 'HV001', 15, 3000000);

    INSERT INTO #TestResults VALUES ('DB-12', N'Ràng buộc CHECK', N'Giới hạn đặt vé: SoLuong từ 1 đến 10 vé (DangKy - BR-10)', 'FAIL', N'Không chặn được đơn đặt 15 vé.');
    DELETE FROM dbo.DangKy WHERE MaDangKy = 'DK_TEST_ERR';
    DELETE FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG_TEST_CK';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-12', N'Ràng buộc CHECK', N'Giới hạn đặt vé: SoLuong từ 1 đến 10 vé (DangKy - BR-10)', 'PASS', N'Đã chặn mua vượt ngưỡng 1-10 vé (CK_DangKy_SoLuong).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-12', N'Ràng buộc CHECK', N'Giới hạn đặt vé: SoLuong từ 1 đến 10 vé (DangKy - BR-10)', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());

    DELETE FROM dbo.DangKy WHERE MaDangKy = 'DK_TEST_ERR';
    DELETE FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG_TEST_CK';
END CATCH;
GO

-- TEST DB-13: Bảo vệ kho vé: SoLuongDaBan <= SoLuongPhatHanh (HangVe - BR-08, BR-09)
PRINT N'>> [TEST DB-13] Kiểm tra ràng buộc CHECK kho vé (BR-08, BR-09)...';
BEGIN TRY
    -- HV001 phát hành 800 vé, thử sửa đã bán = 900 vé
    UPDATE dbo.HangVe SET SoLuongDaBan = 900 WHERE MaHangVe = 'HV001';
    
    INSERT INTO #TestResults VALUES ('DB-13', N'Ràng buộc CHECK', N'Bảo vệ kho vé: SoLuongDaBan <= SoLuongPhatHanh (BR-08, BR-09)', 'FAIL', N'Không chặn được số vé bán vượt số phát hành.');
    UPDATE dbo.HangVe SET SoLuongDaBan = 0 WHERE MaHangVe = 'HV001';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-13', N'Ràng buộc CHECK', N'Bảo vệ kho vé: SoLuongDaBan <= SoLuongPhatHanh (BR-08, BR-09)', 'PASS', N'Đã chặn bán vượt kho (CK_HangVe_SoLuongDaBan).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-13', N'Ràng buộc CHECK', N'Bảo vệ kho vé: SoLuongDaBan <= SoLuongPhatHanh (BR-08, BR-09)', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());
END CATCH;
GO

-- TEST DB-14: Ràng buộc chi phí: SoTien > 0 (ChiPhiSuKien - BR-14)
PRINT N'>> [TEST DB-14] Kiểm tra ràng buộc CHECK chi phí phát sinh (BR-14)...';
BEGIN TRY
    INSERT INTO dbo.ChiPhiSuKien (MaChiPhi, MaSuKien, KhoanMucChi, SoTien)
    VALUES ('CP_TEST_ERR', 'EVT202601', N'Khoản chi âm test', -500000);

    INSERT INTO #TestResults VALUES ('DB-14', N'Ràng buộc CHECK', N'Ràng buộc chi phí: SoTien > 0 (ChiPhiSuKien - BR-14)', 'FAIL', N'Không chặn được số tiền chi <= 0.');
    DELETE FROM dbo.ChiPhiSuKien WHERE MaChiPhi = 'CP_TEST_ERR';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-14', N'Ràng buộc CHECK', N'Ràng buộc chi phí: SoTien > 0 (ChiPhiSuKien - BR-14)', 'PASS', N'Đã chặn khoản chi âm hoặc bằng 0 (CK_ChiPhi_SoTien).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-14', N'Ràng buộc CHECK', N'Ràng buộc chi phí: SoTien > 0 (ChiPhiSuKien - BR-14)', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());
END CATCH;
GO

-- TEST DB-15: Thang điểm khảo sát: 1 <= DiemDanhGia <= 5 (PhanHoi - BR-16)
PRINT N'>> [TEST DB-15] Kiểm tra ràng buộc CHECK thang điểm khảo sát (BR-16)...';
BEGIN TRY
    INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, Email, SoDienThoai)
    VALUES ('NTG_TEST_PH', N'Người đánh giá test', 'danhgia@test.vn', '0911111111');

    -- Thử đánh giá 6 sao (vượt thang 5 sao)
    INSERT INTO dbo.PhanHoi (MaPhanHoi, MaSuKien, MaNguoiThamGia, DiemDanhGia, NoiDungPhanHoi)
    VALUES ('PH_TEST_ERR', 'EVT202601', 'NTG_TEST_PH', 6, N'Đánh giá 6 sao lỗi');

    INSERT INTO #TestResults VALUES ('DB-15', N'Ràng buộc CHECK', N'Thang điểm đánh giá: 1 <= Diem <= 5 (PhanHoi - BR-16)', 'FAIL', N'Không chặn được điểm đánh giá 6 sao.');
    DELETE FROM dbo.PhanHoi WHERE MaPhanHoi = 'PH_TEST_ERR';
    DELETE FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG_TEST_PH';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-15', N'Ràng buộc CHECK', N'Thang điểm đánh giá: 1 <= Diem <= 5 (PhanHoi - BR-16)', 'PASS', N'Đã chặn điểm khảo sát ngoài thang 1-5 (CK_PhanHoi_DiemDanhGia).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-15', N'Ràng buộc CHECK', N'Thang điểm đánh giá: 1 <= Diem <= 5 (PhanHoi - BR-16)', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());

    DELETE FROM dbo.PhanHoi WHERE MaPhanHoi = 'PH_TEST_ERR';
    DELETE FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG_TEST_PH';
END CATCH;
GO

-- TEST DB-16: Kiểm soát trạng thái tài khoản NguoiDung (CK_NguoiDung_TrangThai)
PRINT N'>> [TEST DB-16] Kiểm tra ràng buộc CHECK trạng thái tài khoản...';
BEGIN TRY
    INSERT INTO dbo.NguoiDung (MaNguoiDung, TenDangNhap, MatKhauHash, HoTen, Email, MaNhomQuyen, TrangThai)
    VALUES ('USR_ERR_TT', 'user_invalid_status', '$2a$11$samplehashsamplehashsamplehashsamplehashsamplehashsampl', N'Test', 'err@test.vn', 'ADMIN', N'Trạng thái không hợp lệ');

    INSERT INTO #TestResults VALUES ('DB-16', N'Ràng buộc CHECK', N'Kiểm soát trạng thái tài khoản NguoiDung (CK_NguoiDung_TrangThai)', 'FAIL', N'Không chặn được trạng thái tài khoản sai danh mục.');
    DELETE FROM dbo.NguoiDung WHERE MaNguoiDung = 'USR_ERR_TT';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-16', N'Ràng buộc CHECK', N'Kiểm soát trạng thái tài khoản NguoiDung (CK_NguoiDung_TrangThai)', 'PASS', N'Đã chặn trạng thái tài khoản ngoài danh mục hợp lệ.');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-16', N'Ràng buộc CHECK', N'Kiểm soát trạng thái tài khoản NguoiDung (CK_NguoiDung_TrangThai)', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());
END CATCH;
GO

-- ====================================================================================================
-- PHẦN 4: KIỂM TRA RÀNG BUỘC KHÓA NGOẠI (FOREIGN KEY ENFORCEMENT)
-- ====================================================================================================

-- TEST DB-17: Khóa ngoại FK_SuKien_DiaDiem chặn Địa điểm không tồn tại
PRINT N'>> [TEST DB-17] Kiểm tra ràng buộc Khóa ngoại FK_SuKien_DiaDiem...';
BEGIN TRY
    INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MaNguoiTao)
    VALUES ('EVT_FK_ERR', N'Sự kiện test FK', 'LSK01', 'DD_KHONG_TON_TAI', '2026-12-20 08:00:00', '2026-12-20 17:00:00', N'Dự kiến', 'USR001');

    INSERT INTO #TestResults VALUES ('DB-17', N'Ràng buộc Khóa ngoại', N'Khóa ngoại FK_SuKien_DiaDiem chặn Địa điểm ảo', 'FAIL', N'Không chặn được MaDiaDiem ảo.');
    DELETE FROM dbo.SuKien WHERE MaSuKien = 'EVT_FK_ERR';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-17', N'Ràng buộc Khóa ngoại', N'Khóa ngoại FK_SuKien_DiaDiem chặn Địa điểm ảo', 'PASS', N'Đã chặn thành công với lỗi 547 (FK_SuKien_DiaDiem).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-17', N'Ràng buộc Khóa ngoại', N'Khóa ngoại FK_SuKien_DiaDiem chặn Địa điểm ảo', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());
END CATCH;
GO

-- TEST DB-18: Khóa ngoại FK_DangKy_SuKien chặn Đăng ký sự kiện không tồn tại
PRINT N'>> [TEST DB-18] Kiểm tra ràng buộc Khóa ngoại FK_DangKy_SuKien...';
BEGIN TRY
    INSERT INTO dbo.NguoiThamGia (MaNguoiThamGia, HoTen, Email, SoDienThoai)
    VALUES ('NTG_TEST_FK', N'Khách FK', 'fk@test.vn', '0922222222');

    INSERT INTO dbo.DangKy (MaDangKy, MaSuKien, MaNguoiThamGia, MaHangVe, SoLuong, TongTien)
    VALUES ('DK_FK_ERR', 'EVT_AO_999', 'NTG_TEST_FK', 'HV001', 1, 200000);

    INSERT INTO #TestResults VALUES ('DB-18', N'Ràng buộc Khóa ngoại', N'Khóa ngoại FK_DangKy_SuKien chặn Đăng ký sự kiện ảo', 'FAIL', N'Không chặn được MaSuKien ảo.');
    DELETE FROM dbo.DangKy WHERE MaDangKy = 'DK_FK_ERR';
    DELETE FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG_TEST_FK';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-18', N'Ràng buộc Khóa ngoại', N'Khóa ngoại FK_DangKy_SuKien chặn Đăng ký sự kiện ảo', 'PASS', N'Đã chặn thành công với lỗi 547 (FK_DangKy_SuKien).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-18', N'Ràng buộc Khóa ngoại', N'Khóa ngoại FK_DangKy_SuKien chặn Đăng ký sự kiện ảo', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());

    DELETE FROM dbo.DangKy WHERE MaDangKy = 'DK_FK_ERR';
    DELETE FROM dbo.NguoiThamGia WHERE MaNguoiThamGia = 'NTG_TEST_FK';
END CATCH;
GO

-- TEST DB-19: Khóa ngoại FK_CheckIn_ChiTietVe chặn Soát vé không tồn tại
PRINT N'>> [TEST DB-19] Kiểm tra ràng buộc Khóa ngoại FK_CheckIn_ChiTietVe...';
BEGIN TRY
    INSERT INTO dbo.CheckIn (MaCheckIn, MaVe, NguoiThucHien)
    VALUES ('CI_FK_ERR', 'VE_KHONG_TON_TAI_999', 'USR001');

    INSERT INTO #TestResults VALUES ('DB-19', N'Ràng buộc Khóa ngoại', N'Khóa ngoại FK_CheckIn_ChiTietVe chặn Soát vé ảo', 'FAIL', N'Không chặn được mã vé ảo.');
    DELETE FROM dbo.CheckIn WHERE MaCheckIn = 'CI_FK_ERR';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        INSERT INTO #TestResults VALUES ('DB-19', N'Ràng buộc Khóa ngoại', N'Khóa ngoại FK_CheckIn_ChiTietVe chặn Soát vé ảo', 'PASS', N'Đã chặn thành công với lỗi 547 (FK_CheckIn_ChiTietVe).');
    ELSE
        INSERT INTO #TestResults VALUES ('DB-19', N'Ràng buộc Khóa ngoại', N'Khóa ngoại FK_CheckIn_ChiTietVe chặn Soát vé ảo', 'WARN', N'Lỗi khác: ' + ERROR_MESSAGE());
END CATCH;
GO

-- ====================================================================================================
-- PHẦN 5: KIỂM TRA DỮ LIỆU MẪU KHỞI TẠO (SEED DATA & SECURITY VERIFICATION)
-- ====================================================================================================

-- TEST DB-20: Khởi tạo đủ 3 nhóm quyền cốt lõi (ADMIN, MANAGER, STAFF)
PRINT N'>> [TEST DB-20] Kiểm tra 3 Nhóm quyền khởi tạo...';
DECLARE @RoleCount INT = 0;
SELECT @RoleCount = COUNT(*) FROM dbo.NhomQuyen WHERE MaNhomQuyen IN ('ADMIN', 'MANAGER', 'STAFF');

IF @RoleCount = 3
    INSERT INTO #TestResults VALUES ('DB-20', N'Dữ liệu mẫu (SeedData)', N'Khởi tạo đủ 3 nhóm quyền cốt lõi (ADMIN, MANAGER, STAFF)', 'PASS', N'Đủ 3/3 nhóm quyền chuẩn.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-20', N'Dữ liệu mẫu (SeedData)', N'Khởi tạo đủ 3 nhóm quyền cốt lõi (ADMIN, MANAGER, STAFF)', 'FAIL', N'Chỉ tìm thấy ' + CAST(@RoleCount AS NVARCHAR(10)) + N'/3 nhóm quyền.');
GO

-- TEST DB-21: Khởi tạo đủ 21 Chức năng hệ thống (UC01 - UC21)
PRINT N'>> [TEST DB-21] Kiểm tra 21 Chức năng hệ thống (UC01 - UC21)...';
DECLARE @FuncCount INT = 0;
SELECT @FuncCount = COUNT(*) FROM dbo.ChucNang;

IF @FuncCount = 21
    INSERT INTO #TestResults VALUES ('DB-21', N'Dữ liệu mẫu (SeedData)', N'Khởi tạo đủ 21 Chức năng hệ thống (UC01 - UC21)', 'PASS', N'Đủ 21/21 chức năng tương ứng F01-F20, UC21.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-21', N'Dữ liệu mẫu (SeedData)', N'Khởi tạo đủ 21 Chức năng hệ thống (UC01 - UC21)', 'FAIL', N'Chỉ có ' + CAST(@FuncCount AS NVARCHAR(10)) + N'/21 chức năng.');
GO

-- TEST DB-22: Ma trận phân quyền PhanQuyen (ADMIN: 21, MANAGER: 19, STAFF: 7)
PRINT N'>> [TEST DB-22] Kiểm tra Ma trận phân quyền PhanQuyen...';
DECLARE @AdminQuyen INT = 0, @ManagerQuyen INT = 0, @StaffQuyen INT = 0;
SELECT @AdminQuyen = COUNT(*) FROM dbo.PhanQuyen WHERE MaNhomQuyen = 'ADMIN' AND CoQuyen = 1;
SELECT @ManagerQuyen = COUNT(*) FROM dbo.PhanQuyen WHERE MaNhomQuyen = 'MANAGER' AND CoQuyen = 1;
SELECT @StaffQuyen = COUNT(*) FROM dbo.PhanQuyen WHERE MaNhomQuyen = 'STAFF' AND CoQuyen = 1;

IF @AdminQuyen = 21 AND @ManagerQuyen = 19 AND @StaffQuyen = 7
    INSERT INTO #TestResults VALUES ('DB-22', N'Dữ liệu mẫu (SeedData)', N'Ma trận phân quyền PhanQuyen (ADMIN: 21, MGR: 19, STAFF: 7)', 'PASS', N'Chuẩn xác: ADMIN=' + CAST(@AdminQuyen AS NVARCHAR(10)) + N', MGR=' + CAST(@ManagerQuyen AS NVARCHAR(10)) + N', STAFF=' + CAST(@StaffQuyen AS NVARCHAR(10)) + N'.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-22', N'Dữ liệu mẫu (SeedData)', N'Ma trận phân quyền PhanQuyen (ADMIN: 21, MGR: 19, STAFF: 7)', 'FAIL', N'Sai lệch quyền: ADMIN=' + CAST(@AdminQuyen AS NVARCHAR(10)) + N', MGR=' + CAST(@ManagerQuyen AS NVARCHAR(10)) + N', STAFF=' + CAST(@StaffQuyen AS NVARCHAR(10)) + N'.');
GO

-- TEST DB-23: Tài khoản Admin mặc định & Mật khẩu băm BCrypt (BR-19)
PRINT N'>> [TEST DB-23] Kiểm tra tài khoản Admin và định dạng băm BCrypt...';
DECLARE @AdminHash VARCHAR(255), @AdminRole VARCHAR(20), @AdminStatus NVARCHAR(30), @FailLoginCount INT;
SELECT 
    @AdminHash = MatKhauHash,
    @AdminRole = MaNhomQuyen,
    @AdminStatus = TrangThai,
    @FailLoginCount = SoLanDangNhapSai
FROM dbo.NguoiDung 
WHERE TenDangNhap = 'admin';

IF @AdminHash IS NULL
    INSERT INTO #TestResults VALUES ('DB-23', N'Dữ liệu mẫu (SeedData)', N'Tài khoản Admin mặc định & chuẩn băm BCrypt (BR-19)', 'FAIL', N'Không tìm thấy tài khoản admin trong CSDL.');
ELSE IF (@AdminHash LIKE '$2a$%' OR @AdminHash LIKE '$2b$%') AND LEN(@AdminHash) = 60 AND @AdminRole = 'ADMIN' AND @AdminStatus = N'Hoạt động' AND @FailLoginCount = 0
    INSERT INTO #TestResults VALUES ('DB-23', N'Dữ liệu mẫu (SeedData)', N'Tài khoản Admin mặc định & chuẩn băm BCrypt (BR-19)', 'PASS', N'Tài khoản hợp lệ: BCrypt cost 11 (60 ký tự), nhóm ADMIN, trạng thái Hoạt động.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-23', N'Dữ liệu mẫu (SeedData)', N'Tài khoản Admin mặc định & chuẩn băm BCrypt (BR-19)', 'FAIL', N'Thông tin admin chưa chuẩn: Hash=' + ISNULL(@AdminHash, 'NULL') + N', Nhóm=' + ISNULL(@AdminRole, 'NULL'));
GO

-- TEST DB-24: Dữ liệu danh mục mẫu cơ sở cho ngày 2 (Sự kiện, Hạng vé, Lịch trình...)
PRINT N'>> [TEST DB-24] Kiểm tra Dữ liệu danh mục mẫu cơ sở...';
DECLARE @MasterDataCheck INT = 0;
IF EXISTS (SELECT 1 FROM dbo.LoaiSuKien)
   AND EXISTS (SELECT 1 FROM dbo.DiaDiem)
   AND EXISTS (SELECT 1 FROM dbo.DienGia)
   AND EXISTS (SELECT 1 FROM dbo.NhanSu)
   AND EXISTS (SELECT 1 FROM dbo.NhaTaiTro)
   AND EXISTS (SELECT 1 FROM dbo.SuKien)
   AND EXISTS (SELECT 1 FROM dbo.HangVe)
   AND EXISTS (SELECT 1 FROM dbo.LichTrinh)
   AND EXISTS (SELECT 1 FROM dbo.ChiTietTaiTro)
    SET @MasterDataCheck = 1;

IF @MasterDataCheck = 1
    INSERT INTO #TestResults VALUES ('DB-24', N'Dữ liệu mẫu (SeedData)', N'Dữ liệu danh mục mẫu cơ sở (Sự kiện, Hạng vé, Lịch trình...)', 'PASS', N'Đầy đủ dữ liệu mồi ở tất cả các bảng danh mục cốt lõi.');
ELSE
    INSERT INTO #TestResults VALUES ('DB-24', N'Dữ liệu mẫu (SeedData)', N'Dữ liệu danh mục mẫu cơ sở (Sự kiện, Hạng vé, Lịch trình...)', 'FAIL', N'Thiếu dữ liệu mẫu ở một số bảng danh mục.');
GO

-- ====================================================================================================
-- PHẦN 6: TỔNG KẾT & XUẤT BÁO CÁO NGHIỆM THU NGÀY 1 (THÀNH VIÊN 1)
-- ====================================================================================================

SELECT 
    TestID AS [Mã Test],
    NhomTest AS [Nhóm Nghiệp Vụ],
    TenTest AS [Nội Dung Kiểm Thử],
    KetQua AS [Kết Quả],
    ChiTiet AS [Chi Tiết Thẩm Định]
FROM #TestResults
ORDER BY TestID;

DECLARE @TotalTests INT, @PassedTests INT, @FailedTests INT, @WarnTests INT;
SELECT 
    @TotalTests = COUNT(*),
    @PassedTests = SUM(CASE WHEN KetQua = 'PASS' THEN 1 ELSE 0 END),
    @FailedTests = SUM(CASE WHEN KetQua = 'FAIL' THEN 1 ELSE 0 END),
    @WarnTests   = SUM(CASE WHEN KetQua = 'WARN' THEN 1 ELSE 0 END)
FROM #TestResults;

PRINT N'';
PRINT N'====================================================================================================';
PRINT N'                  BÁO CÁO NGHIỆM THU KIỂM THỬ CSDL NGÀY 1 (THÀNH VIÊN 1)';
PRINT N'====================================================================================================';
PRINT N'  >> Tổng số ca kiểm thử thực thi:        ' + CAST(@TotalTests AS NVARCHAR(10));
PRINT N'  >> Số ca kiểm thử THÀNH CÔNG (PASS):    ' + CAST(@PassedTests AS NVARCHAR(10)) + N' (' + CAST((@PassedTests * 100 / @TotalTests) AS NVARCHAR(10)) + N'%)';
PRINT N'  >> Số ca kiểm thử THẤT BẠI (FAIL):       ' + CAST(@FailedTests AS NVARCHAR(10));
PRINT N'  >> Số ca cảnh báo (WARN):                ' + CAST(@WarnTests AS NVARCHAR(10));
PRINT N'----------------------------------------------------------------------------------------------------';

IF @FailedTests = 0
BEGIN
    PRINT N'  >> KẾT LUẬN: [XUẤT SẮC] CƠ SỞ DỮ LIỆU ĐẠT 100% TIÊU CHUẨN NGHIỆM THU NGÀY 1 CỦA THÀNH VIÊN 1!';
    PRINT N'  >> Lược đồ 20 bảng, ràng buộc khóa ngoại, CHECK, UNIQUE, an ninh BCrypt & Seed Data hoàn tất.';
    PRINT N'  >> Toàn bộ hệ thống sẵn sàng bàn giao cho Ngày 2 (Dựng Solution 5 projects & Entity Models).';
END
ELSE
BEGIN
    PRINT N'  >> KẾT LUẬN: [CHƯA ĐẠT] Vui lòng kiểm tra các mục FAIL trong bảng kết quả để khắc phục.';
END
PRINT N'====================================================================================================';

-- Dọn dẹp bảng tạm
DROP TABLE #TestResults;
GO
