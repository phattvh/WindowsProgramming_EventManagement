-- ===================================================================================
-- SCRIPT: LÀM TRẮNG DỮ LIỆU CƠ SỞ DỮ LIỆU (00_ClearData.sql)
-- DỰ ÁN: HỆ THỐNG QUẢN LÝ SỰ KIỆN (EventManagement)
-- MỤC ĐÍCH: Xóa sạch toàn bộ dữ liệu nghiệp vụ (Sự kiện, Hạng vé, Vé, Check-in, Chi phí, v.v.),
--          đưa CSDL về trạng thái trắng tinh (0 dòng) sẵn sàng nạp DataMau.sql hoặc nhập mới từ đầu.
--          Giữ lại cấu hình hệ thống cốt lõi: NhomQuyen, ChucNang, PhanQuyen và 3 tài khoản chuẩn.
-- ===================================================================================

USE EventManagementDB;
GO

SET NOCOUNT ON;

PRINT N'>>> BẮT ĐẦU LÀM TRẮNG TOÀN BỘ DỮ LIỆU NGHIỆP VỤ CSDL...';

-- 1. Xóa toàn bộ dữ liệu nghiệp vụ theo thứ tự ràng buộc khóa ngoại
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

-- 2. Dọn dẹp tài khoản kiểm thử phụ, chỉ giữ lại 3 tài khoản chuẩn hệ thống
DELETE FROM dbo.NguoiDung WHERE MaNguoiDung NOT IN ('USR001', 'USR002', 'USR003');
GO

-- 3. Reset trạng thái hoạt động và mở khóa cho 3 tài khoản chuẩn
UPDATE dbo.NguoiDung 
SET TrangThai = N'Hoạt động', 
    SoLanDangNhapSai = 0, 
    KhoaDenKhi = NULL;
GO

PRINT N'>>> [THÀNH CÔNG] CSDL EventManagementDB ĐÃ ĐƯỢC LÀM TRẮNG HOÀN TOÀN!';
PRINT N'>>> Các bảng nghiệp vụ: SuKien, HangVe, ChiTietVe, CheckIn, ChiPhi, LichTrinh,... = 0 dòng.';
PRINT N'>>> Sẵn sàng để nạp DataMau.sql hoặc nhập liệu mới qua giao diện phần mềm.';
GO
