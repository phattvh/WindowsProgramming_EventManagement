using System;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng ChiPhiSuKien - Khoản mục chi phí thực tế phát sinh (BR-14, BR-15)
    /// </summary>
    public class ChiPhiSuKien
    {
        public string MaChiPhi { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string KhoanMucChi { get; set; } = string.Empty;
        public decimal SoTien { get; set; } // Phải > 0 (CK_ChiPhi_SoTien)
        public DateTime NgayChi { get; set; } = DateTime.Now;
        public string? NguoiDuyet { get; set; } // Mã người dùng duyệt chi (FK NguoiDung)
        public string? GhiChu { get; set; }

        // Navigation properties
        public virtual SuKien? SuKien { get; set; }
        public virtual NguoiDung? NguoiDung { get; set; }
    }
}
