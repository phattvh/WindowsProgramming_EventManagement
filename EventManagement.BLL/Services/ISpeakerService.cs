using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện xử lý nghiệp vụ Quản lý diễn giả / khách mời (F09 / UC09)
    /// Hỗ trợ quy tắc BR-06 (Chống trùng diễn giả) và BR-20 (Soft delete).
    /// </summary>
    public interface ISpeakerService
    {
        ServiceResult<List<SpeakerDto>> GetAll(string? keyword = null, string? statusFilter = null);
        ServiceResult<SpeakerDto> GetById(string maDienGia);
        ServiceResult Add(SpeakerDto dto);
        ServiceResult Update(SpeakerDto dto);
        ServiceResult Delete(string maDienGia);
        ServiceResult SoftDelete(string maDienGia);
    }
}
