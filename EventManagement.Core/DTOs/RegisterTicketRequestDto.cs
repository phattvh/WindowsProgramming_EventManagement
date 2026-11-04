using System;
using System.Collections.Generic;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO yêu cầu đăng ký & mua vé từ màn hình F11
    /// Hỗ trợ mua từ 1 đến 10 vé trong một đơn hàng (BR-10)
    /// </summary>
    public class RegisterTicketRequestDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string MaHangVe { get; set; } = string.Empty;
        public int SoLuongMua { get; set; } = 1; // 1 đến 10 vé
        
        // Thông tin người đại diện mua vé
        public string HoTenNguoiMua { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DonVi { get; set; }
        public string? DiaChi { get; set; }

        // Danh sách họ tên người thực tế sử dụng vé (khi mua nhiều vé)
        public List<string> DanhSachNguoiDiCung { get; set; } = new List<string>();
    }
}
