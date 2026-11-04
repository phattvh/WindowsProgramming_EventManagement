using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO kiểm tra ngân sách và cảnh báo bội chi 80% (BR-14, BR-15) cho màn hình F17
    /// </summary>
    public class BudgetAlertDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public decimal NganSachDuKien { get; set; }
        public decimal TongThucChiHienTai { get; set; }
        public decimal KhoanChiSapThem { get; set; }
        public decimal TongChiSauKhiThem => TongThucChiHienTai + KhoanChiSapThem;
        
        // Tỷ lệ sử dụng ngân sách (%)
        public double TyLeSuDung => NganSachDuKien > 0 ? (double)(TongChiSauKhiThem / NganSachDuKien) * 100 : 0;

        // Cảnh báo mềm (80% - 100% đổi màu vàng cam)
        public bool IsWarning80 => TyLeSuDung >= 80.0 && TyLeSuDung <= 100.0;

        // Cảnh báo vượt trần (> 100% đổi màu đỏ cảnh báo bội chi)
        public bool IsOverBudget => TyLeSuDung > 100.0;

        public string ThongBaoTrangThai
        {
            get
            {
                if (IsOverBudget)
                    return $"BỘI CHI! Tổng chi phí đã vượt ngân sách dự kiến ({TyLeSuDung:F1}%).";
                if (IsWarning80)
                    return $"CẢNH BÁO: Chi phí đã chạm mốc {TyLeSuDung:F1}% ngân sách dự kiến!";
                return $"Ngân sách trong ngưỡng an toàn ({TyLeSuDung:F1}%).";
            }
        }
    }

    /// <summary>
    /// DTO hiển thị danh sách phiếu chi trên Form F17
    /// </summary>
    public class ExpenseDto
    {
        public string MaChiPhi { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public string KhoanMucChi { get; set; } = string.Empty;
        public string? DienGiai { get; set; }
        public decimal SoTien { get; set; }
        public DateTime NgayChi { get; set; }
        public string NguoiLapPhieu { get; set; } = string.Empty;
        public string? TenNguoiLapPhieu { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}
