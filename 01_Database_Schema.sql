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

-- bảng 10: SuKien --
CREATE TABLE dbo.SuKien
(
    MaSuKien        VARCHAR(20)     NOT NULL,
    TenSuKien       NVARCHAR(200)   NOT NULL,
    MaLoaiSK        VARCHAR(20)     NOT NULL,
    MaDiaDiem       VARCHAR(20)     NOT NULL,
    NgayBatDau      DATETIME        NOT NULL,
    NgayKetThuc     DATETIME        NOT NULL,
    TrangThai       NVARCHAR(30)    NOT NULL CONSTRAINT DF_SuKien_TrangThai DEFAULT N'Dự kiến',
    MoTa            NVARCHAR(MAX)   NULL,
    HinhAnh         VARCHAR(255)    NULL,
    MaNguoiTao      VARCHAR(20)     NOT NULL,
    CONSTRAINT PK_SuKien PRIMARY KEY (MaSuKien),
    CONSTRAINT FK_SuKien_LoaiSuKien FOREIGN KEY (MaLoaiSK) REFERENCES dbo.LoaiSuKien(MaLoaiSK),
    CONSTRAINT FK_SuKien_DiaDiem FOREIGN KEY (MaDiaDiem) REFERENCES dbo.DiaDiem(MaDiaDiem),
    CONSTRAINT FK_SuKien_NguoiDung FOREIGN KEY (MaNguoiTao) REFERENCES dbo.NguoiDung(MaNguoiDung),
    CONSTRAINT CK_SuKien_NgayHopLe CHECK (NgayKetThuc >= NgayBatDau),
    CONSTRAINT CK_SuKien_TrangThai CHECK (TrangThai IN (N'Dự kiến', N'Mở đăng ký', N'Đang diễn ra', N'Đã kết thúc', N'Hủy'))
);
GO

-- bảng 11: LichTrinh --
CREATE TABLE dbo.LichTrinh
(
    MaLichTrinh     VARCHAR(20)     NOT NULL,
    MaSuKien        VARCHAR(20)     NOT NULL,
    TenTietMuc      NVARCHAR(150)   NOT NULL,
    ThoiGianBatDau  DATETIME        NOT NULL,
    ThoiGianKetThuc DATETIME        NOT NULL,
    MaDienGia       VARCHAR(20)     NULL,
    DiaDiemChiTiet  NVARCHAR(100)   NULL,
    GhiChu          NVARCHAR(255)   NULL,
    CONSTRAINT PK_LichTrinh PRIMARY KEY (MaLichTrinh),
    CONSTRAINT FK_LichTrinh_SuKien FOREIGN KEY (MaSuKien) REFERENCES dbo.SuKien(MaSuKien) ON DELETE CASCADE,
    CONSTRAINT FK_LichTrinh_DienGia FOREIGN KEY (MaDienGia) REFERENCES dbo.DienGia(MaDienGia),
    CONSTRAINT CK_LichTrinh_ThoiGian CHECK (ThoiGianKetThuc > ThoiGianBatDau)
);
GO

-- bảng 12: ChiTietTaiTro --
CREATE TABLE dbo.ChiTietTaiTro
(
    MaTaiTro            VARCHAR(20)     NOT NULL,
    MaSuKien            VARCHAR(20)     NOT NULL,
    MaNhaTaiTro         VARCHAR(20)     NOT NULL,
    GoiTaiTro           NVARCHAR(50)    NOT NULL CONSTRAINT DF_ChiTietTaiTro_Goi DEFAULT N'Đồng',
    GiaTriTaiTro        DECIMAL(18,2)   NOT NULL CONSTRAINT DF_ChiTietTaiTro_GiaTri DEFAULT 0,
    HinhThucTaiTro      NVARCHAR(100)   NOT NULL CONSTRAINT DF_ChiTietTaiTro_HinhThuc DEFAULT N'Tiền mặt',
    GhiChu              NVARCHAR(255)   NULL,
    CONSTRAINT PK_ChiTietTaiTro PRIMARY KEY (MaTaiTro),
    CONSTRAINT FK_ChiTietTaiTro_SuKien FOREIGN KEY (MaSuKien) REFERENCES dbo.SuKien(MaSuKien) ON DELETE CASCADE,
    CONSTRAINT FK_ChiTietTaiTro_NhaTaiTro FOREIGN KEY (MaNhaTaiTro) REFERENCES dbo.NhaTaiTro(MaNhaTaiTro),
    CONSTRAINT CK_ChiTietTaiTro_GiaTri CHECK (GiaTriTaiTro >= 0),
    CONSTRAINT CK_ChiTietTaiTro_Goi CHECK (GoiTaiTro IN (N'Kim Cương', N'Vàng', N'Bạc', N'Đồng', N'Đồng hành'))
);
GO

-- bảng 13: HangVe --
CREATE TABLE dbo.HangVe
(
    MaHangVe            VARCHAR(20)     NOT NULL,
    MaSuKien            VARCHAR(20)     NOT NULL,
    TenHangVe           NVARCHAR(50)    NOT NULL,
    GiaVe               DECIMAL(18,2)   NOT NULL CONSTRAINT DF_HangVe_GiaVe DEFAULT 0,
    SoLuongPhatHanh     INT             NOT NULL,
    SoLuongDaBan        INT             NOT NULL CONSTRAINT DF_HangVe_SoLuongDaBan DEFAULT 0,
    CONSTRAINT PK_HangVe PRIMARY KEY (MaHangVe),
    CONSTRAINT FK_HangVe_SuKien FOREIGN KEY (MaSuKien) REFERENCES dbo.SuKien(MaSuKien) ON DELETE CASCADE,
    CONSTRAINT CK_HangVe_GiaVe CHECK (GiaVe >= 0),
    CONSTRAINT CK_HangVe_SoLuongPhatHanh CHECK (SoLuongPhatHanh > 0),
    CONSTRAINT CK_HangVe_SoLuongDaBan CHECK (SoLuongDaBan >= 0 AND SoLuongDaBan <= SoLuongPhatHanh)
);
GO

-- bảng 14: NguoiThamGia --
CREATE TABLE dbo.NguoiThamGia
(
    MaNguoiThamGia      VARCHAR(20)     NOT NULL,
    HoTen               NVARCHAR(100)   NOT NULL,
    GioiTinh            NVARCHAR(10)    NULL,
    Email               VARCHAR(100)    NOT NULL,
    SoDienThoai         VARCHAR(15)     NOT NULL,
    DonViCongTac        NVARCHAR(150)   NULL,
    CONSTRAINT PK_NguoiThamGia PRIMARY KEY (MaNguoiThamGia),
    CONSTRAINT CK_NguoiThamGia_GioiTinh CHECK (GioiTinh IS NULL OR GioiTinh IN (N'Nam', N'Nữ', N'Khác'))
);
GO

-- bảng 15: DangKy --
CREATE TABLE dbo.DangKy
(
    MaDangKy            VARCHAR(20)     NOT NULL,
    MaSuKien            VARCHAR(20)     NOT NULL,
    MaNguoiThamGia      VARCHAR(20)     NOT NULL,
    MaHangVe            VARCHAR(20)     NOT NULL,
    NgayDangKy          DATETIME        NOT NULL CONSTRAINT DF_DangKy_NgayDangKy DEFAULT GETDATE(),
    SoLuong             INT             NOT NULL CONSTRAINT DF_DangKy_SoLuong DEFAULT 1,
    TongTien            DECIMAL(18,2)   NOT NULL CONSTRAINT DF_DangKy_TongTien DEFAULT 0,
    TrangThaiThanhToan  NVARCHAR(30)    NOT NULL CONSTRAINT DF_DangKy_TrangThai DEFAULT N'Chưa thanh toán',
    PhuongThucThanhToan NVARCHAR(50)    NULL,
    CONSTRAINT PK_DangKy PRIMARY KEY (MaDangKy),
    CONSTRAINT FK_DangKy_SuKien FOREIGN KEY (MaSuKien) REFERENCES dbo.SuKien(MaSuKien),
    CONSTRAINT FK_DangKy_NguoiThamGia FOREIGN KEY (MaNguoiThamGia) REFERENCES dbo.NguoiThamGia(MaNguoiThamGia),
    CONSTRAINT FK_DangKy_HangVe FOREIGN KEY (MaHangVe) REFERENCES dbo.HangVe(MaHangVe),
    CONSTRAINT CK_DangKy_SoLuong CHECK (SoLuong > 0 AND SoLuong <= 10),
    CONSTRAINT CK_DangKy_TongTien CHECK (TongTien >= 0),
    CONSTRAINT CK_DangKy_TrangThai CHECK (TrangThaiThanhToan IN (N'Chưa thanh toán', N'Đã thanh toán', N'Đã hủy'))
);
GO

-- bảng 16: ChiTietVe --
CREATE TABLE dbo.ChiTietVe
(
    MaVe                VARCHAR(30)     NOT NULL,
    MaDangKy            VARCHAR(20)     NOT NULL,
    MaQR                VARCHAR(100)    NOT NULL,
    TrangThaiVe         NVARCHAR(30)    NOT NULL CONSTRAINT DF_ChiTietVe_TrangThai DEFAULT N'Hợp lệ',
    CONSTRAINT PK_ChiTietVe PRIMARY KEY (MaVe),
    CONSTRAINT UQ_ChiTietVe_MaQR UNIQUE (MaQR),
    CONSTRAINT FK_ChiTietVe_DangKy FOREIGN KEY (MaDangKy) REFERENCES dbo.DangKy(MaDangKy) ON DELETE CASCADE,
    CONSTRAINT CK_ChiTietVe_TrangThai CHECK (TrangThaiVe IN (N'Hợp lệ', N'Đã check-in', N'Đã hủy'))
);
GO

-- bảng 17: CheckIn --
CREATE TABLE dbo.CheckIn
(
    MaCheckIn           VARCHAR(20)     NOT NULL,
    MaVe                VARCHAR(30)     NOT NULL,
    ThoiGianCheckIn     DATETIME        NOT NULL CONSTRAINT DF_CheckIn_ThoiGianCheckIn DEFAULT GETDATE(),
    NguoiThucHien       VARCHAR(20)     NOT NULL,
    HinhThucCheckIn     NVARCHAR(50)    NOT NULL CONSTRAINT DF_CheckIn_HinhThuc DEFAULT N'Webcam QR Code',
    GhiChu              NVARCHAR(255)   NULL,
    CONSTRAINT PK_CheckIn PRIMARY KEY (MaCheckIn),
    CONSTRAINT UQ_CheckIn_MaVe UNIQUE (MaVe),
    CONSTRAINT FK_CheckIn_ChiTietVe FOREIGN KEY (MaVe) REFERENCES dbo.ChiTietVe(MaVe),
    CONSTRAINT FK_CheckIn_NguoiDung FOREIGN KEY (NguoiThucHien) REFERENCES dbo.NguoiDung(MaNguoiDung)
);
GO

-- bảng 18: PhanCongNhanSu --
CREATE TABLE dbo.PhanCongNhanSu
(
    MaPhanCong          VARCHAR(20)     NOT NULL,
    MaSuKien            VARCHAR(20)     NOT NULL,
    MaNhanSu            VARCHAR(20)     NOT NULL,
    NhiemVu             NVARCHAR(100)   NOT NULL,
    ThoiGianBatDau      DATETIME        NOT NULL,
    ThoiGianKetThuc     DATETIME        NOT NULL,
    GhiChu              NVARCHAR(255)   NULL,
    CONSTRAINT PK_PhanCongNhanSu PRIMARY KEY (MaPhanCong),
    CONSTRAINT FK_PhanCong_SuKien FOREIGN KEY (MaSuKien) REFERENCES dbo.SuKien(MaSuKien) ON DELETE CASCADE,
    CONSTRAINT FK_PhanCong_NhanSu FOREIGN KEY (MaNhanSu) REFERENCES dbo.NhanSu(MaNhanSu),
    CONSTRAINT CK_PhanCong_ThoiGian CHECK (ThoiGianKetThuc > ThoiGianBatDau)
);
GO

-- bảng 19: ChiPhiSuKien --
CREATE TABLE dbo.ChiPhiSuKien
(
    MaChiPhi            VARCHAR(20)     NOT NULL,
    MaSuKien            VARCHAR(20)     NOT NULL,
    KhoanMucChi         NVARCHAR(100)   NOT NULL,
    SoTien              DECIMAL(18,2)   NOT NULL,
    NgayChi             DATETIME        NOT NULL CONSTRAINT DF_ChiPhi_NgayChi DEFAULT GETDATE(),
    NguoiDuyet          VARCHAR(20)     NULL,
    GhiChu              NVARCHAR(255)   NULL,
    CONSTRAINT PK_ChiPhiSuKien PRIMARY KEY (MaChiPhi),
    CONSTRAINT FK_ChiPhi_SuKien FOREIGN KEY (MaSuKien) REFERENCES dbo.SuKien(MaSuKien) ON DELETE CASCADE,
    CONSTRAINT FK_ChiPhi_NguoiDung FOREIGN KEY (NguoiDuyet) REFERENCES dbo.NguoiDung(MaNguoiDung),
    CONSTRAINT CK_ChiPhi_SoTien CHECK (SoTien > 0)
);
GO

-- bảng 20: PhanHoi --
CREATE TABLE dbo.PhanHoi
(
    MaPhanHoi           VARCHAR(20)     NOT NULL,
    MaSuKien            VARCHAR(20)     NOT NULL,
    MaNguoiThamGia      VARCHAR(20)     NOT NULL,
    DiemDanhGia         INT             NOT NULL,
    NoiDungPhanHoi      NVARCHAR(500)   NULL,
    NgayDanhGia         DATETIME        NOT NULL CONSTRAINT DF_PhanHoi_NgayDanhGia DEFAULT GETDATE(),
    CONSTRAINT PK_PhanHoi PRIMARY KEY (MaPhanHoi),
    CONSTRAINT FK_PhanHoi_SuKien FOREIGN KEY (MaSuKien) REFERENCES dbo.SuKien(MaSuKien) ON DELETE CASCADE,
    CONSTRAINT FK_PhanHoi_NguoiThamGia FOREIGN KEY (MaNguoiThamGia) REFERENCES dbo.NguoiThamGia(MaNguoiThamGia),
    CONSTRAINT UQ_PhanHoi_SuKien_NguoiThamGia UNIQUE (MaSuKien, MaNguoiThamGia),
    CONSTRAINT CK_PhanHoi_DiemDanhGia CHECK (DiemDanhGia >= 1 AND DiemDanhGia <= 5)
);
GO

-- Indexes --
CREATE NONCLUSTERED INDEX IX_NguoiDung_MaNhomQuyen ON dbo.NguoiDung(MaNhomQuyen);
CREATE NONCLUSTERED INDEX IX_PhanQuyen_MaChucNang ON dbo.PhanQuyen(MaChucNang);
CREATE NONCLUSTERED INDEX IX_SuKien_MaLoaiSK ON dbo.SuKien(MaLoaiSK);
CREATE NONCLUSTERED INDEX IX_SuKien_MaDiaDiem ON dbo.SuKien(MaDiaDiem);
CREATE NONCLUSTERED INDEX IX_SuKien_TrangThai_NgayBatDau ON dbo.SuKien(TrangThai, NgayBatDau);
CREATE NONCLUSTERED INDEX IX_LichTrinh_MaSuKien ON dbo.LichTrinh(MaSuKien);
CREATE NONCLUSTERED INDEX IX_HangVe_MaSuKien ON dbo.HangVe(MaSuKien);
CREATE NONCLUSTERED INDEX IX_DangKy_MaSuKien ON dbo.DangKy(MaSuKien);
CREATE NONCLUSTERED INDEX IX_DangKy_MaNguoiThamGia ON dbo.DangKy(MaNguoiThamGia);
CREATE NONCLUSTERED INDEX IX_DangKy_MaHangVe ON dbo.DangKy(MaHangVe);
CREATE NONCLUSTERED INDEX IX_ChiTietVe_MaDangKy ON dbo.ChiTietVe(MaDangKy);
CREATE NONCLUSTERED INDEX IX_PhanCongNhanSu_MaSuKien ON dbo.PhanCongNhanSu(MaSuKien);
CREATE NONCLUSTERED INDEX IX_ChiPhiSuKien_MaSuKien ON dbo.ChiPhiSuKien(MaSuKien);
GO
