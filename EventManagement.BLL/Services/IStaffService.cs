using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện xử lý nghiệp vụ Quản lý nhân sự ban tổ chức (F14 / UC14)
    /// Hỗ trợ quy tắc BR-13 (Chống trùng ca trực) và BR-20 (Soft delete).
    /// </summary>
    public interface IStaffService
    {
        ServiceResult<List<StaffDto>> GetAll(string? keyword = null, string? statusFilter = null, string? departmentFilter = null);
        ServiceResult<StaffDto> GetById(string maNhanSu);
        ServiceResult Add(StaffDto dto);
        ServiceResult Update(StaffDto dto);
        ServiceResult Delete(string maNhanSu);
        ServiceResult SoftDelete(string maNhanSu);
    }
}
