using System;
using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện xử lý nghiệp vụ Phân công nhân sự theo ca trực (F15 / UC15)
    /// Đảm bảo các quy tắc nghiệp vụ:
    /// - BR-13: Chống trùng ca trực nhân sự theo khung giờ làm việc giao thoa nhau.
    /// - Không phân công nhân sự đang ở trạng thái ngừng hoạt động / nghỉ việc.
    /// - Thẩm định thời gian ca trực hợp lệ.
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-13/Day 6)
    /// </summary>
    public interface IStaffAssignmentService
    {
        ServiceResult<List<StaffAssignmentDto>> GetAll(string? maSuKien = null, string? keyword = null, string? maNhanSu = null, string? phongBan = null);
        ServiceResult<StaffAssignmentDto> GetById(string maPhanCong);
        ServiceResult Add(StaffAssignmentDto dto);
        ServiceResult Update(StaffAssignmentDto dto);
        ServiceResult Delete(string maPhanCong);
        ServiceResult<bool> CheckConflict(StaffAssignmentDto dto, string? excludeMaPhanCong = null);

        // Danh mục phụ trợ phục vụ ComboBox trên giao diện F15
        List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> GetAvailableEvents();
        List<StaffDto> GetAvailableStaff();
        string SuggestNextCode();
    }
}
