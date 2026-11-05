namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách diễn giả và khách mời chuyên môn (Form F09, Tab 3 Form F05)
    /// Phụ trách: Thành viên 2
    /// </summary>
    public class SpeakerDto
    {
        public string MaDienGia { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string? ChucVu { get; set; }
        public string? DonViCongTac { get; set; }
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public string? TieuSu { get; set; }

        // Đường dẫn tương đối tới file ảnh chân dung trong thư mục Images/Speakers/
        public string? HinhAnh { get; set; }

        // Thống kê số lượng tiết mục diễn giả này tham gia
        public int SoTietMucThamGia { get; set; }

        // Trạng thái hoạt động: "Hoạt động" hoặc "Ngừng hợp tác" (BR-20)
        public string TrangThai { get; set; } = "Hoạt động";
    }
}
