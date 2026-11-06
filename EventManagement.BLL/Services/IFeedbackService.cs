using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện dịch vụ nghiệp vụ Quản lý phản hồi sau sự kiện (Form F18)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// </summary>
    public interface IFeedbackService
    {
        ServiceResult<List<FeedbackDto>> GetAll(string? keyword = null, string? maSuKien = null, int? ratingFilter = null);
        ServiceResult<FeedbackDto> GetById(string maPhanHoi);
        ServiceResult Add(FeedbackDto dto);
        ServiceResult Update(FeedbackDto dto);
        ServiceResult Delete(string maPhanHoi);
        ServiceResult<FeedbackSummaryDto> GetSummary(string? maSuKien = null);
        string SuggestNextCode();
        List<KeyValuePair<string, string>> GetCompletedEvents();
        List<KeyValuePair<string, string>> GetEligibleAttendees(string? maSuKien = null);
    }
}
