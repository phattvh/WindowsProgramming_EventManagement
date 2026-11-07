using System;
using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện xử lý nghiệp vụ Quản lý sự kiện tổng thể (F04 / UC04)
    /// Đảm bảo các quy tắc nghiệp vụ:
    /// - BR-02: Ràng buộc xóa sự kiện (chỉ xóa khi chưa có dữ liệu liên kết, ngược lại chuyển 'Đã hủy').
    /// - BR-03: Kiểm tra logic thời gian sự kiện (NgayKetThuc >= NgayBatDau).
    /// - BR-04: Chống trùng lịch địa điểm giữa các sự kiện trong cùng khoảng thời gian giao thoa.
    /// - BR-17: Đề xuất mã sự kiện tự động theo chuẩn quy ước.
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-12 / Ngày 7)
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Lấy danh sách sự kiện có lọc theo bộ tiêu chí DTO
        /// </summary>
        ServiceResult<List<EventListDto>> GetAll(EventFilterDto filter);

        /// <summary>
        /// Lấy danh sách sự kiện với các tham số tìm kiếm đa điều kiện
        /// </summary>
        ServiceResult<List<EventListDto>> GetAll(
            string? keyword = null,
            string? maLoaiSK = null,
            string? trangThai = null,
            string? maDiaDiem = null,
            DateTime? tuNgay = null,
            DateTime? denNgay = null);

        /// <summary>
        /// Lấy thông tin chi tiết một sự kiện theo mã
        /// </summary>
        ServiceResult<EventListDto> GetById(string maSuKien);

        /// <summary>
        /// Thêm mới một sự kiện vào hệ thống (thẩm định BR-03, BR-04)
        /// </summary>
        ServiceResult Add(EventListDto dto);

        /// <summary>
        /// Cập nhật thông tin sự kiện (thẩm định BR-03, BR-04)
        /// </summary>
        ServiceResult Update(EventListDto dto);

        /// <summary>
        /// Xóa sự kiện khỏi hệ thống (thẩm định BR-02)
        /// </summary>
        ServiceResult Delete(string maSuKien);

        /// <summary>
        /// Hủy sự kiện: chuyển trạng thái sang 'Đã hủy'
        /// </summary>
        ServiceResult CancelEvent(string maSuKien);

        /// <summary>
        /// Đề xuất mã sự kiện tiếp theo tự động
        /// </summary>
        string SuggestNextCode();

        /// <summary>
        /// Lấy danh sách loại sự kiện khả dụng để đổ ComboBox
        /// </summary>
        List<EventTypeDto> GetAvailableEventTypes();

        /// <summary>
        /// Lấy danh sách địa điểm khả dụng để đổ ComboBox
        /// </summary>
        List<VenueDto> GetAvailableVenues();
    }
}
