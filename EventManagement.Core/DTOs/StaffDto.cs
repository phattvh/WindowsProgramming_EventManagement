namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO quản lý hồ sơ nhân sự ban tổ chức (Form F14, phân công ca trực F15, Tab 6 Form F05)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// </summary>
    public class StaffDto
    {
        public string MaNhanSu { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string ChucDanh { get; set; } = "Cộng tác viên";
        public string PhongBan { get; set; } = "Hậu cần";
        public string TrangThai { get; set; } = "Hoạt động";

        // Thống kê số lượng ca trực nhân sự này đã được phân công tham gia
        public int SoCaTrucThamGia { get; set; }
    }
}
