using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách phân công nhân sự theo ca trực (Form F15, Tab 6 Form F05)
    /// Phụ trách: Thành viên 2
    /// </summary>
    public class StaffAssignmentDto
    {
        public string MaPhanCong { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string MaNhanSu { get; set; } = string.Empty;
        public string TenNhanSu { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string ChucVuNhanSu { get; set; } = string.Empty;
        public string NhiemVu { get; set; } = string.Empty;
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }

        // Khung giờ hiển thị định dạng HH:mm - HH:mm (dd/MM)
        public string KhungGio => $"{ThoiGianBatDau:HH:mm} - {ThoiGianKetThuc:HH:mm} ({ThoiGianBatDau:dd/MM})";

        public string? GhiChu { get; set; }
    }
}
