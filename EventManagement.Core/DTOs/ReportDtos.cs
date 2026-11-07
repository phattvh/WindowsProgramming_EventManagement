using System;
using System.Collections.Generic;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO cấp nguồn dữ liệu cho mẫu in vé điện tử VeDienTu.rdlc (F11 / AD-5)
    /// </summary>
    public class TicketReceiptDto
    {
        public string MaVe { get; set; } = string.Empty;
        public string MaDangKy { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string TenDiaDiem { get; set; } = string.Empty;
        public string DiaChiDiaDiem { get; set; } = string.Empty;
        public string TenHangVe { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public string TenNguoiThamGia { get; set; } = string.Empty; // Người cầm vé
        public string? EmailNguoiThamGia { get; set; }
        public string MaQR { get; set; } = string.Empty;
        public byte[]? QRCodeImageBytes { get; set; } // Mảng byte ảnh QR do ZXing.Net sinh ra để nhúng vào RDLC
        public DateTime NgayXuatVe { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// DTO cấp nguồn dữ liệu cho mẫu in báo cáo quyết toán tài chính BaoCaoQuyetToan.rdlc (F17 / F19)
    /// </summary>
    public class SettlementDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public decimal NganSachDuKien { get; set; }
        public decimal TongDoanhThuBanVe { get; set; }
        public decimal TongTienTaiTro { get; set; }
        public decimal TongThu => TongDoanhThuBanVe + TongTienTaiTro;
        public decimal TongThucChi { get; set; }
        public decimal ChenhLechThuChi => TongThu - TongThucChi;
        public string DanhGiaQuyetToan => ChenhLechThuChi >= 0 ? "Thặng dư tài chính" : "Thâm hụt tài chính";

        // Danh sách chi tiết các khoản chi
        public List<ExpenseDto> DanhSachChiPhi { get; set; } = new List<ExpenseDto>();
    }

    /// <summary>
    /// DTO tổng hợp chỉ số KPI sự kiện phục vụ báo cáo tổng hợp (F19)
    /// </summary>
    public class EventSummaryKpiDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string DiaDiem { get; set; } = string.Empty;
        public int TongVePhatHanh { get; set; }
        public int TongVeDaBan { get; set; }
        public int TongLuotCheckIn { get; set; }
        public double TyLeCheckIn => TongVeDaBan > 0 ? (double)TongLuotCheckIn / TongVeDaBan * 100 : 0.0;
        public double TyLeThamDu => TyLeCheckIn;
        public double TyLeBanVe => TongVePhatHanh > 0 ? (double)TongVeDaBan / TongVePhatHanh * 100 : 0.0;
        public decimal TongDoanhThuVe { get; set; }
        public decimal TongDoanhThu => TongDoanhThuVe;
        public decimal TongTienTaiTro { get; set; }
        public decimal TongChiPhiThucTe { get; set; }
        public decimal LoiNhuanRong => (TongDoanhThuVe + TongTienTaiTro) - TongChiPhiThucTe;
    }
}
