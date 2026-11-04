namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách địa điểm và nguồn dữ liệu cho ComboBox chọn địa điểm (Form F07, F04, F05)
    /// Phụ trách: Thành viên 2
    /// </summary>
    public class VenueDto
    {
        public string MaDiaDiem { get; set; } = string.Empty;
        public string TenDiaDiem { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public int SucChuaToiDa { get; set; }
        public string? GhiChu { get; set; }

        // Thống kê số sự kiện đã/đang tổ chức tại địa điểm này
        public int SoSuKienToChuc { get; set; }

        // Chuỗi hiển thị định dạng cho ComboBox
        public string TenHienThi => $"{TenDiaDiem} (Sức chứa: {SucChuaToiDa:N0} người)";
    }
}
