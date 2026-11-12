using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng CheckIn - Lịch sử điểm danh & Soát vé (BR-11, BR-12)
    /// MaVe là UNIQUE: Mỗi vé chỉ được phép check-in thành công 1 lần duy nhất
    /// </summary>
    public class CheckIn
    {
        public string MaCheckIn { get; set; } = string.Empty;
        public string MaVe { get; set; } = string.Empty; // UNIQUE
        public DateTime ThoiGianCheckIn { get; set; } = DateTime.Now;
        public string NguoiThucHien { get; set; } = string.Empty; // Mã tài khoản nhân viên soát vé (FK NguoiDung)

        [NotMapped]
        public string NguoiCheckIn
        {
            get => NguoiThucHien;
            set => NguoiThucHien = value;
        }

        public string HinhThucCheckIn { get; set; } = "Webcam QR Code"; // Webcam QR Code, Thủ công

        [NotMapped]
        public string PhuongThuc
        {
            get => HinhThucCheckIn;
            set => HinhThucCheckIn = value;
        }
        public string? GhiChu { get; set; }

        // Navigation properties
        public virtual ChiTietVe? ChiTietVe { get; set; }
        public virtual NguoiDung? NguoiDung { get; set; }
    }
}
