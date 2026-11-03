using System;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng ChiTietVe - Từng chiếc vé thực tế phát hành (Detail)
    /// Mỗi vé sở hữu 1 mã QR GUID duy nhất và định danh người sử dụng vé
    /// </summary>
    public class ChiTietVe
    {
        public string MaVe { get; set; } = string.Empty; // Mã vé tự sinh VE_...
        public string MaDangKy { get; set; } = string.Empty;
        public string MaQR { get; set; } = string.Empty; // Chuỗi token duy nhất in trên mã QR
        public string TenNguoiThamGia { get; set; } = "Khách tham dự"; // Người thực tế sử dụng vé (BR-10)
        public string? Email { get; set; } // Email nhận vé điện tử
        public string TrangThaiVe { get; set; } = "Hợp lệ"; // Hợp lệ, Đã check-in, Đã hủy

        // Navigation properties
        public virtual DangKy? DangKy { get; set; }
        public virtual CheckIn? CheckIn { get; set; }
    }
}
