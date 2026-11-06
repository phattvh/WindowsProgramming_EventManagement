using System;
using System.Collections.Generic;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO chứa 4 nhóm chỉ số KPI thống kê tổng quan trên Dashboard (Form F02).
    /// </summary>
    public class DashboardStatsDto
    {
        // Nhóm 1: Sự kiện
        public int TongSoSuKien { get; set; }
        public int SuKienChuanBi { get; set; }
        public int SuKienSapDienRa { get; set; }
        public int SuKienDangDienRa { get; set; }
        public int SuKienDaHoanThanh { get; set; }
        public int SuKienDaHuy { get; set; }

        // Nhóm 2: Bán vé & Người tham gia
        public int TongVeDaBan { get; set; }
        public int TongNguoiThamGia { get; set; }

        // Nhóm 3: Tài chính
        public decimal TongDoanhThuVe { get; set; }
        public decimal TongChiPhiThucTe { get; set; }

        // Nhóm 4: Check-in
        public int TongNguoiCheckIn { get; set; }
        public double TyLeCheckIn { get; set; } // % từ 0 đến 100

        // Bổ trợ khác
        public int TongNhaTaiTro { get; set; }
    }

    /// <summary>
    /// DTO hiển thị danh sách sự kiện sắp diễn ra trong 7 ngày tới trên F02.
    /// </summary>
    public class UpcomingEventDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string DiaDiem { get; set; } = string.Empty;
        public int SoVeDaBan { get; set; }
        public int SoNguoiCheckIn { get; set; }
        public double TyLeCheckIn { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO phục vụ biểu đồ so sánh Doanh thu vs Chi phí theo sự kiện (LiveCharts2).
    /// </summary>
    public class EventRevenueChartDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public decimal DoanhThu { get; set; }
        public decimal ChiPhi { get; set; }
    }

    /// <summary>
    /// DTO phục vụ biểu đồ phân bổ số lượng sự kiện theo tháng trong năm.
    /// </summary>
    public class MonthlyEventCountDto
    {
        public int Thang { get; set; }
        public int SoLuongSuKien { get; set; }
    }

    /// <summary>
    /// DTO phục vụ biểu đồ tròn tỷ lệ Check-in thực tế.
    /// </summary>
    public class CheckInRatioDto
    {
        public int DaCheckIn { get; set; }
        public int ChuaCheckIn { get; set; }
        public double TyLePhanTram { get; set; }
    }
}
