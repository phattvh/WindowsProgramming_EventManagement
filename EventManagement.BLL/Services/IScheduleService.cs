using System;
using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện xử lý nghiệp vụ Quản lý lịch trình / chương trình tiết mục (F08 / UC08)
    /// Đảm bảo các quy tắc nghiệp vụ:
    /// - BR-05: Khung giờ tiết mục con phải nằm hoàn toàn trong thời gian diễn ra sự kiện.
    /// - BR-06: Chống trùng diễn giả & chống trùng phòng chức năng trong cùng thời điểm.
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-11)
    /// </summary>
    public interface IScheduleService
    {
        ServiceResult<List<ScheduleItemDto>> GetAll(string? maSuKien = null, string? keyword = null, string? maDienGia = null);
        ServiceResult<ScheduleItemDto> GetById(string maLichTrinh);
        ServiceResult Add(ScheduleItemDto dto);
        ServiceResult Update(ScheduleItemDto dto);
        ServiceResult Delete(string maLichTrinh);
        ServiceResult<bool> CheckConflict(ScheduleItemDto dto, string? excludeMaLichTrinh = null);

        // Danh mục phụ trợ phục vụ ComboBox trên giao diện F08
        List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> GetAvailableEvents();
        List<(string MaDienGia, string HoTen)> GetAvailableSpeakers();
    }
}
