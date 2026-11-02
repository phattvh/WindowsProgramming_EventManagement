USE EventManagementDB;
GO

-- test DB-01: kiem tra 20 bang --
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
    PRINT N'>> [PASS] DB-01: Đủ 20 bảng trong CSDL EventManagementDB.'
ELSE
    PRINT N'>> [FAIL] DB-01: Chỉ tìm thấy ' + CAST(@TableCount AS NVARCHAR(10)) + N'/20 bảng.';
GO

-- test DB-02: rang buoc CHECK --
PRINT N'>> [TEST DB-02] Kiểm tra ràng buộc CHECK trên bảng PhanCongNhanSu...';
BEGIN TRY
    INSERT INTO dbo.PhanCongNhanSu (MaPhanCong, MaSuKien, MaNhanSu, NhiemVu, ThoiGianBatDau, ThoiGianKetThuc)
    VALUES ('PC_TEST_ERR', 'EVT202601', 'NS001', N'Trực cổng kiểm tra', '2026-12-15 10:00:00', '2026-12-15 08:00:00');
    
    PRINT N'>> [FAIL] DB-02: Ràng buộc CHECK không chặn được ThoiGianKetThuc < ThoiGianBatDau!';
    DELETE FROM dbo.PhanCongNhanSu WHERE MaPhanCong = 'PC_TEST_ERR';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        PRINT N'>> [PASS] DB-02: Ràng buộc CK_PhanCong_ThoiGian đã chặn thành công (' + ERROR_MESSAGE() + N').'
    ELSE
        PRINT N'>> [WARN] DB-02: Lỗi khác: ' + ERROR_MESSAGE();
END CATCH;
GO

-- test DB-03: khoa ngoai FK --
PRINT N'>> [TEST DB-03] Kiểm tra ràng buộc Khóa ngoại FK trên bảng SuKien...';
BEGIN TRY
    INSERT INTO dbo.SuKien (MaSuKien, TenSuKien, MaLoaiSK, MaDiaDiem, NgayBatDau, NgayKetThuc, TrangThai, MaNguoiTao)
    VALUES ('EVT_FK_ERR', N'Sự kiện test FK', 'LSK01', 'DD_KHONG_TON_TAI', '2026-12-20 08:00:00', '2026-12-20 17:00:00', N'Dự kiến', 'USR001');

    PRINT N'>> [FAIL] DB-03: Khóa ngoại không chặn được MaDiaDiem không tồn tại!';
    DELETE FROM dbo.SuKien WHERE MaSuKien = 'EVT_FK_ERR';
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 547
        PRINT N'>> [PASS] DB-03: Khóa ngoại FK_SuKien_DiaDiem đã chặn thành công (' + ERROR_MESSAGE() + N').'
    ELSE
        PRINT N'>> [WARN] DB-03: Lỗi khác: ' + ERROR_MESSAGE();
END CATCH;
GO

-- test DB-04: kiem tra seed Admin BCrypt --
PRINT N'>> [TEST DB-04] Kiểm tra tài khoản Admin và định dạng BCrypt trong SeedData...';
DECLARE @AdminHash VARCHAR(255);
SELECT @AdminHash = MatKhauHash FROM dbo.NguoiDung WHERE TenDangNhap = 'admin';

IF @AdminHash IS NULL
    PRINT N'>> [FAIL] DB-04: Không tìm thấy tài khoản admin trong bảng NguoiDung!'
ELSE IF @AdminHash LIKE '$2a$%' OR @AdminHash LIKE '$2b$%'
    PRINT N'>> [PASS] DB-04: Tài khoản admin tồn tại, mật khẩu băm chuẩn BCrypt hợp lệ (' + @AdminHash + N').'
ELSE
    PRINT N'>> [FAIL] DB-04: Mật khẩu chưa được băm chuẩn BCrypt! Giá trị hiện tại: ' + @AdminHash;
GO
