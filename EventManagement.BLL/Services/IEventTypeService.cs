using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện xử lý nghiệp vụ Quản lý loại sự kiện (F06 / UC06)
    /// Tuân thủ quy tắc nghiệp vụ BR-07 và BR-20.
    /// </summary>
    public interface IEventTypeService
    {
        ServiceResult<List<EventTypeDto>> GetAll(string? keyword = null, string? statusFilter = null);
        ServiceResult<EventTypeDto> GetById(string maLoaiSK);
        ServiceResult Add(EventTypeDto dto);
        ServiceResult Update(EventTypeDto dto);
        ServiceResult Delete(string maLoaiSK);
        ServiceResult SoftDelete(string maLoaiSK);
    }
}
