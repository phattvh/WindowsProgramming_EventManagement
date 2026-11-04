using System;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO chứa các tiêu chí lọc/tìm kiếm đa điều kiện cho Form F04
    /// Phụ trách: Thành viên 2
    /// </summary>
    public class EventFilterDto
    {
        public string? TuKhoa { get; set; }
        public string? MaLoaiSK { get; set; }
        public string? MaDiaDiem { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
    }
}
