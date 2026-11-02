USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'EventManagementDB')
BEGIN
    CREATE DATABASE EventManagementDB;
END
GO

USE EventManagementDB;
GO

IF OBJECT_ID(N'dbo.PhanHoi', N'U') IS NOT NULL DROP TABLE dbo.PhanHoi;
IF OBJECT_ID(N'dbo.ChiPhiSuKien', N'U') IS NOT NULL DROP TABLE dbo.ChiPhiSuKien;
IF OBJECT_ID(N'dbo.PhanCongNhanSu', N'U') IS NOT NULL DROP TABLE dbo.PhanCongNhanSu;
IF OBJECT_ID(N'dbo.CheckIn', N'U') IS NOT NULL DROP TABLE dbo.CheckIn;
IF OBJECT_ID(N'dbo.ChiTietVe', N'U') IS NOT NULL DROP TABLE dbo.ChiTietVe;
IF OBJECT_ID(N'dbo.DangKy', N'U') IS NOT NULL DROP TABLE dbo.DangKy;
IF OBJECT_ID(N'dbo.NguoiThamGia', N'U') IS NOT NULL DROP TABLE dbo.NguoiThamGia;
IF OBJECT_ID(N'dbo.HangVe', N'U') IS NOT NULL DROP TABLE dbo.HangVe;
IF OBJECT_ID(N'dbo.ChiTietTaiTro', N'U') IS NOT NULL DROP TABLE dbo.ChiTietTaiTro;
IF OBJECT_ID(N'dbo.NhaTaiTro', N'U') IS NOT NULL DROP TABLE dbo.NhaTaiTro;
IF OBJECT_ID(N'dbo.LichTrinh', N'U') IS NOT NULL DROP TABLE dbo.LichTrinh;
IF OBJECT_ID(N'dbo.SuKien', N'U') IS NOT NULL DROP TABLE dbo.SuKien;
IF OBJECT_ID(N'dbo.NhanSu', N'U') IS NOT NULL DROP TABLE dbo.NhanSu;
IF OBJECT_ID(N'dbo.DienGia', N'U') IS NOT NULL DROP TABLE dbo.DienGia;
IF OBJECT_ID(N'dbo.DiaDiem', N'U') IS NOT NULL DROP TABLE dbo.DiaDiem;
IF OBJECT_ID(N'dbo.LoaiSuKien', N'U') IS NOT NULL DROP TABLE dbo.LoaiSuKien;
IF OBJECT_ID(N'dbo.PhanQuyen', N'U') IS NOT NULL DROP TABLE dbo.PhanQuyen;
IF OBJECT_ID(N'dbo.ChucNang', N'U') IS NOT NULL DROP TABLE dbo.ChucNang;
IF OBJECT_ID(N'dbo.NguoiDung', N'U') IS NOT NULL DROP TABLE dbo.NguoiDung;
IF OBJECT_ID(N'dbo.NhomQuyen', N'U') IS NOT NULL DROP TABLE dbo.NhomQuyen;
GO

-- bảng 1: NhomQuyen --
CREATE TABLE dbo.NhomQuyen
(
    MaNhomQuyen     VARCHAR(20)     NOT NULL,
    TenNhomQuyen    NVARCHAR(50)    NOT NULL,
    MoTa            NVARCHAR(255)   NULL,
    CONSTRAINT PK_NhomQuyen PRIMARY KEY (MaNhomQuyen)
);
GO
