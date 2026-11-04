using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách sự kiện trên DataGridView của Form F04 và F02
    /// Phụ trách: Thành viên 2
    /// </summary>
    public class EventListDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public string MaLoaiSK { get; set; } = string.Empty;
        public string TenLoaiSK { get; set; } = string.Empty;
        public string MaDiaDiem { get; set; } = string.Empty;
        public string TenDiaDiem { get; set; } = string.Empty;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int SucChua { get; set; }
        public decimal NganSachDuKien { get; set; }
        public string NguoiPhuTrach { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;

        // Các chỉ số tổng hợp nhanh phục vụ hiển thị
        public int SoVeDaBan { get; set; }
        public int SoKhachCheckIn { get; set; }
        public double TyLeCheckIn => SoVeDaBan > 0 ? Math.Round((double)SoKhachCheckIn / SoVeDaBan * 100, 1) : 0;
    }
}
