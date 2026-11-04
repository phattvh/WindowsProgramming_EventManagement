namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách loại sự kiện và nguồn dữ liệu ComboBox (Form F06, F04, F05)
    /// Phụ trách: Thành viên 2
    /// </summary>
    public class EventTypeDto
    {
        public string MaLoaiSK { get; set; } = string.Empty;
        public string TenLoaiSK { get; set; } = string.Empty;
        public string? MoTa { get; set; }

        /// <summary>
        /// Số sự kiện hiện đang áp dụng loại này (phục vụ kiểm tra BR-07)
        /// </summary>
        public int SoSuKien { get; set; }

        /// <summary>
        /// Trạng thái hoạt động: 'Hoạt động' hoặc 'Ngừng áp dụng'
        /// </summary>
        public string TrangThai { get; set; } = "Hoạt động";

        /// <summary>
        /// Chuỗi hiển thị định dạng cho ComboBox
        /// </summary>
        public string TenHienThi => TenLoaiSK;
    }
}
