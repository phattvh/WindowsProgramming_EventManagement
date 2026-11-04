using System;
using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng SuKien - Thông tin tổng thể sự kiện
    /// </summary>
    public class SuKien
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public string MaLoaiSK { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string MaDiaDiem { get; set; } = string.Empty;
        public int SucChua { get; set; }
        public decimal NganSachDuKien { get; set; }
        public string NguoiPhuTrach { get; set; } = string.Empty;
        public string TrangThai { get; set; } = "Chuẩn bị"; // Chuẩn bị, Sắp diễn ra, Đang diễn ra, Đã hoàn thành, Đã hủy

        // Navigation properties phân hệ Sự kiện & Danh mục (Thành viên 2)
        public virtual LoaiSuKien? LoaiSuKien { get; set; }
        public virtual DiaDiem? DiaDiem { get; set; }
        public virtual ICollection<LichTrinh> LichTrinhs { get; set; } = new List<LichTrinh>();
        public virtual ICollection<PhanCongNhanSu> PhanCongNhanSus { get; set; } = new List<PhanCongNhanSu>();

        // Navigation properties liên kết tới phân hệ của Thành viên 3
        public virtual ICollection<HangVe> HangVes { get; set; } = new List<HangVe>();
        public virtual ICollection<DangKy> DangKys { get; set; } = new List<DangKy>();
        public virtual ICollection<ChiPhiSuKien> ChiPhiSuKiens { get; set; } = new List<ChiPhiSuKien>();
        public virtual ICollection<ChiTietTaiTro> ChiTietTaiTros { get; set; } = new List<ChiTietTaiTro>();
        public virtual ICollection<PhanHoi> PhanHois { get; set; } = new List<PhanHoi>();
    }
}
