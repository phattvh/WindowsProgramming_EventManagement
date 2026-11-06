using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object cho Ý kiến đánh giá / Phản hồi sau sự kiện (Form F18)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// </summary>
    public class FeedbackDto
    {
        public string MaPhanHoi { get; set; } = string.Empty;
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public string MaNguoiThamGia { get; set; } = string.Empty;
        public string TenNguoiThamGia { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }

        public int DiemDanhGia { get; set; } = 5; // 1 đến 5 sao
        public string? NoiDungPhanHoi { get; set; }
        public DateTime NgayDanhGia { get; set; } = DateTime.Now;

        /// <summary>
        /// Hiển thị chuỗi sao trực quan (ví dụ: ★★★★★ (5/5))
        /// </summary>
        public string SoSaoText
        {
            get
            {
                int stars = Math.Clamp(DiemDanhGia, 1, 5);
                return new string('★', stars) + new string('☆', 5 - stars) + $" ({stars}/5)";
            }
        }

        public string NgayDanhGiaFormatted => NgayDanhGia.ToString("dd/MM/yyyy HH:mm");
    }

    /// <summary>
    /// Thống kê tổng hợp ý kiến phản hồi sự kiện
    /// </summary>
    public class FeedbackSummaryDto
    {
        public int TongSoPhanHoi { get; set; }
        public double DiemTrungBinh { get; set; }
        public int SoLuong5Sao { get; set; }
        public int SoLuong4Sao { get; set; }
        public int SoLuong3Sao { get; set; }
        public int SoLuong2Sao { get; set; }
        public int SoLuong1Sao { get; set; }

        /// <summary>
        /// Tỷ lệ hài lòng (đánh giá từ 4 sao trở lên)
        /// </summary>
        public double TyLeHaiLong => TongSoPhanHoi > 0 
            ? Math.Round((double)(SoLuong5Sao + SoLuong4Sao) / TongSoPhanHoi * 100.0, 1) 
            : 0;
    }
}
