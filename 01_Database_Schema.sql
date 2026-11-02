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

-- bảng 2: NguoiDung --
CREATE TABLE dbo.NguoiDung
(
    MaNguoiDung         VARCHAR(20)     NOT NULL,
    TenDangNhap         VARCHAR(50)     NOT NULL,
    MatKhauHash         VARCHAR(255)    NOT NULL,
    HoTen               NVARCHAR(100)   NOT NULL,
    Email               VARCHAR(100)    NOT NULL,
    SoDienThoai         VARCHAR(15)     NULL,
    MaNhomQuyen         VARCHAR(20)     NOT NULL,
    TrangThai           NVARCHAR(30)    NOT NULL CONSTRAINT DF_NguoiDung_TrangThai DEFAULT N'Hoạt động',
    LanDoiMatKhauGanNhat DATETIME       NULL,
    SoLanDangNhapSai    INT             NOT NULL CONSTRAINT DF_NguoiDung_SoLanDangNhapSai DEFAULT 0,
    KhoaDenKhi          DATETIME        NULL,
    CONSTRAINT PK_NguoiDung PRIMARY KEY (MaNguoiDung),
    CONSTRAINT UQ_NguoiDung_TenDangNhap UNIQUE (TenDangNhap),
    CONSTRAINT FK_NguoiDung_NhomQuyen FOREIGN KEY (MaNhomQuyen) REFERENCES dbo.NhomQuyen(MaNhomQuyen),
    CONSTRAINT CK_NguoiDung_TrangThai CHECK (TrangThai IN (N'Hoạt động', N'Khóa tạm thời', N'Khóa vĩnh viễn'))
);
GO

-- bảng 3: ChucNang --
CREATE TABLE dbo.ChucNang
(
    MaChucNang      VARCHAR(20)     NOT NULL,
    TenChucNang     NVARCHAR(100)   NOT NULL,
    MaForm          VARCHAR(20)     NOT NULL,
    CONSTRAINT PK_ChucNang PRIMARY KEY (MaChucNang)
);
GO

-- bảng 4: PhanQuyen --
CREATE TABLE dbo.PhanQuyen
(
    MaNhomQuyen     VARCHAR(20)     NOT NULL,
    MaChucNang      VARCHAR(20)     NOT NULL,
    CoQuyen         BIT             NOT NULL CONSTRAINT DF_PhanQuyen_CoQuyen DEFAULT 1,
    CONSTRAINT PK_PhanQuyen PRIMARY KEY (MaNhomQuyen, MaChucNang),
    CONSTRAINT FK_PhanQuyen_NhomQuyen FOREIGN KEY (MaNhomQuyen) REFERENCES dbo.NhomQuyen(MaNhomQuyen),
    CONSTRAINT FK_PhanQuyen_ChucNang FOREIGN KEY (MaChucNang) REFERENCES dbo.ChucNang(MaChucNang)
);
GO

-- bảng 5: LoaiSuKien --
CREATE TABLE dbo.LoaiSuKien
(
    MaLoaiSK        VARCHAR(20)     NOT NULL,
    TenLoaiSK       NVARCHAR(100)   NOT NULL,
    MoTa            NVARCHAR(255)   NULL,
    CONSTRAINT PK_LoaiSuKien PRIMARY KEY (MaLoaiSK)
);
GO

-- bảng 6: DiaDiem --
CREATE TABLE dbo.DiaDiem
(
    MaDiaDiem       VARCHAR(20)     NOT NULL,
    TenDiaDiem      NVARCHAR(150)   NOT NULL,
    DiaChi          NVARCHAR(255)   NOT NULL,
    SucChuaToiDa    INT             NOT NULL CONSTRAINT DF_DiaDiem_SucChuaToiDa DEFAULT 100,
    GhiChu          NVARCHAR(255)   NULL,
    CONSTRAINT PK_DiaDiem PRIMARY KEY (MaDiaDiem),
    CONSTRAINT CK_DiaDiem_SucChuaToiDa CHECK (SucChuaToiDa > 0)
);
GO

-- bảng 7: DienGia --
CREATE TABLE dbo.DienGia
(
    MaDienGia       VARCHAR(20)     NOT NULL,
    HoTen           NVARCHAR(100)   NOT NULL,
    ChucVu          NVARCHAR(100)   NULL,
    DonViCongTac    NVARCHAR(150)   NULL,
    Email           VARCHAR(100)    NULL,
    SoDienThoai     VARCHAR(15)     NULL,
    TieuSu          NVARCHAR(500)   NULL,
    CONSTRAINT PK_DienGia PRIMARY KEY (MaDienGia)
);
GO

-- bảng 8: NhanSu --
CREATE TABLE dbo.NhanSu
(
    MaNhanSu        VARCHAR(20)     NOT NULL,
    HoTen           NVARCHAR(100)   NOT NULL,
    SoDienThoai     VARCHAR(15)     NOT NULL,
    Email           VARCHAR(100)    NULL,
    ChucVu          NVARCHAR(50)    NOT NULL CONSTRAINT DF_NhanSu_ChucVu DEFAULT N'Cộng tác viên',
    CONSTRAINT PK_NhanSu PRIMARY KEY (MaNhanSu)
);
GO

-- bảng 9: NhaTaiTro --
CREATE TABLE dbo.NhaTaiTro
(
    MaNhaTaiTro     VARCHAR(20)     NOT NULL,
    TenToChuc       NVARCHAR(150)   NOT NULL,
    NguoiDaiDien    NVARCHAR(100)   NULL,
    Email           VARCHAR(100)    NULL,
    SoDienThoai     VARCHAR(15)     NULL,
    DiaChi          NVARCHAR(255)   NULL,
    CONSTRAINT PK_NhaTaiTro PRIMARY KEY (MaNhaTaiTro)
);
GO
