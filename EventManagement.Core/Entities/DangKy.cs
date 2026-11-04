using System;
using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng DangKy - Đơn hàng đăng ký vé (Master)
    /// Hỗ trợ 1 người đại diện mua từ 1 đến 10 vé trong 1 đơn hàng (BR-10)
    /// </summary>
    public class DangKy
    {
        public string MaDangKy { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string MaNguoiThamGia { get; set; } = string.Empty; // Người đại diện thanh toán
        public string MaHangVe { get; set; } = string.Empty;
        public int SoLuong { get; set; } // Số lượng vé đặt mua (1 đến 10 vé)
        public decimal TongTien { get; set; } // Tổng tiền = Đơn giá * Số lượng
        public DateTime NgayDangKy { get; set; } = DateTime.Now;
        public string TrangThaiThanhToan { get; set; } = "Chưa thanh toán"; // Chưa thanh toán, Đã thanh toán, Đã hủy
        public string? PhuongThucThanhToan { get; set; } // Tiền mặt, Chuyển khoản, Thẻ tín dụng, Ví điện tử

        // Navigation properties
        public virtual SuKien? SuKien { get; set; }
        public virtual NguoiThamGia? NguoiThamGia { get; set; }
        public virtual HangVe? HangVe { get; set; }
        public virtual ICollection<ChiTietVe> ChiTietVes { get; set; } = new List<ChiTietVe>();
    }
}
