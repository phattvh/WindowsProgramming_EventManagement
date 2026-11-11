using System;
using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng HangVe - Hạng vé mở bán cho sự kiện
    /// </summary>
    public class HangVe
    {
        public string MaHangVe { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string TenHangVe { get; set; } = string.Empty;
        public decimal GiaVe { get; set; }
        public decimal DonGia
        {
            get => GiaVe;
            set => GiaVe = value;
        }
        public int SoLuongPhatHanh { get; set; }
        public int SoLuongDaBan { get; set; }
        public DateTime? ThoiGianMoBan { get; set; }
        public DateTime? ThoiGianDongBan { get; set; }

        // Thuộc tính tiện ích tính toán tồn kho (BR-09)
        public int SoLuongConLai => Math.Max(0, SoLuongPhatHanh - SoLuongDaBan);
        public bool ConVe => SoLuongConLai > 0;

        /// <summary>
        /// Trạng thái kho vé phục vụ hiển thị trực quan trên giao diện
        /// </summary>
        public string TrangThaiKho
        {
            get
            {
                if (ThoiGianDongBan.HasValue && DateTime.Now > ThoiGianDongBan.Value)
                    return "Đã đóng";
                if (ThoiGianMoBan.HasValue && DateTime.Now < ThoiGianMoBan.Value)
                    return "Chưa mở bán";
                if (SoLuongConLai <= 0)
                    return "Hết vé";
                if (SoLuongConLai <= 10)
                    return "Sắp hết vé";
                return "Đang mở bán";
            }
        }

        public string TrangThai => TrangThaiKho;

        // Navigation properties
        public virtual SuKien? SuKien { get; set; }
        public virtual ICollection<DangKy> DangKys { get; set; } = new List<DangKy>();
    }
}
