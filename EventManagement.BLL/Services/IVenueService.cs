using System;
using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện xử lý nghiệp vụ Quản lý địa điểm tổ chức (F07 / UC07)
    /// Hỗ trợ quy tắc BR-04 (Chống trùng lịch địa điểm) và BR-20 (Soft delete).
    /// </summary>
    public interface IVenueService
    {
        ServiceResult<List<VenueDto>> GetAll(string? keyword = null, string? statusFilter = null);
        ServiceResult<VenueDto> GetById(string maDiaDiem);
        ServiceResult Add(VenueDto dto);
        ServiceResult Update(VenueDto dto);
        ServiceResult Delete(string maDiaDiem);
        ServiceResult SoftDelete(string maDiaDiem);
        ServiceResult<bool> CheckVenueAvailability(string maDiaDiem, DateTime startTime, DateTime endTime, string? excludeEventId = null);
    }
}
