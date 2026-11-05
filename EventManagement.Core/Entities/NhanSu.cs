using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng NhanSu - Quản lý danh sách nhân viên và cộng tác viên phục vụ sự kiện
    /// Phụ trách: Thành viên 2 (Màn hình F14 và Tab 6 Form F05)
    /// </summary>
    public class NhanSu
    {
        public string MaNhanSu { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string ChucVu { get; set; } = "Cộng tác viên";
        public string PhongBan { get; set; } = "Hậu cần";
        public string TrangThai { get; set; } = "Hoạt động";

        // Navigation properties: 1 Nhân sự có thể được phân công trực ở nhiều sự kiện/ca trực
        public virtual ICollection<PhanCongNhanSu> PhanCongNhanSus { get; set; } = new List<PhanCongNhanSu>();
    }
}
