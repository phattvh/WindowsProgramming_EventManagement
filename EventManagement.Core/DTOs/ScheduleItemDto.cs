using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách lịch trình hoạt động chi tiết (Form F08, Tab 2 Form F05)
    /// Phụ trách: Thành viên 2
    /// </summary>
    public class ScheduleItemDto
    {
        public string MaLichTrinh { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string TenTietMuc { get; set; } = string.Empty;
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }

        // Khung giờ hiển thị định dạng HH:mm - HH:mm
        public string KhungGio => $"{ThoiGianBatDau:HH:mm} - {ThoiGianKetThuc:HH:mm}";

        public string? MaDienGia { get; set; }
        public string? TenDienGia { get; set; }
        public string? DiaDiemChiTiet { get; set; }
        public string? GhiChu { get; set; }
    }
}
