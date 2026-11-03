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
}
