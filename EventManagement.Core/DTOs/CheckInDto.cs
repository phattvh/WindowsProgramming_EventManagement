using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO gửi từ màn hình F13 khi quét mã QR hoặc nhập mã vé thủ công
    /// </summary>
    public class CheckInRequestDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string MaQRLuocDoc { get; set; } = string.Empty; // Token đọc từ QR hoặc mã vé
        public string NguoiCheckIn { get; set; } = string.Empty; // Mã tài khoản nhân viên
        public string PhuongThuc { get; set; } = "Quét QR Webcam"; // Quét QR Webcam / Nhập mã thủ công
    }

    /// <summary>
    /// DTO kết quả thẩm định và check-in trả về cho màn hình F13 (BR-11, BR-12)
    /// </summary>
    public class CheckInResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string MaVe { get; set; } = string.Empty;
        public string TenNguoiThamGia { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;
        public string TenHangVe { get; set; } = string.Empty;
        public DateTime? ThoiGianCheckIn { get; set; }
        public bool IsWarningDuplicate { get; set; } // True nếu vé đã quét trước đó (chặn quét 1 chiều)
        public DateTime? ThoiGianCheckInTruocDo { get; set; }
    }

    /// <summary>
    /// DTO thống kê tình hình check-in của sự kiện theo thời gian thực
    /// </summary>
    public class CheckInStatsDto
    {
        public string MaSuKien { get; set; } = string.Empty;
        public int TongVePhatHanh { get; set; }
        public int DaCheckIn { get; set; }
        public int ChuaCheckIn => Math.Max(0, TongVePhatHanh - DaCheckIn);
        public double TyLeCheckIn => TongVePhatHanh > 0 ? (double)DaCheckIn / TongVePhatHanh * 100.0 : 0.0;
    }

    /// <summary>
    /// DTO bản ghi lịch sử quét check-in hiển thị trên bảng gần đây
    /// </summary>
    public class CheckInHistoryDto
    {
        public string MaVe { get; set; } = string.Empty;
        public string TenNguoiThamGia { get; set; } = string.Empty;
        public string TenHangVe { get; set; } = string.Empty;
        public DateTime ThoiGian { get; set; } = DateTime.Now;
        public string PhuongThuc { get; set; } = "Webcam";
        public bool ThanhCong { get; set; }
        public string GhiChu { get; set; } = string.Empty;
    }
}
