using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện dịch vụ phân tích dữ liệu tổng quan và KPI cho Dashboard (Form F02).
    /// </summary>
    public interface IDashboardService
    {
        Task<ServiceResult<DashboardStatsDto>> GetDashboardStatsAsync();
        ServiceResult<DashboardStatsDto> GetDashboardStats();

        Task<ServiceResult<List<UpcomingEventDto>>> GetUpcomingEventsAsync(int daysAhead = 7);
        ServiceResult<List<UpcomingEventDto>> GetUpcomingEvents(int daysAhead = 7);

        Task<ServiceResult<List<UpcomingEventDto>>> GetAllEventsAsync();
        ServiceResult<List<UpcomingEventDto>> GetAllEvents();

        Task<ServiceResult<List<EventRevenueChartDto>>> GetEventRevenueComparisonAsync(int limit = 5);
        ServiceResult<List<EventRevenueChartDto>> GetEventRevenueComparison(int limit = 5);

        Task<ServiceResult<List<MonthlyEventCountDto>>> GetMonthlyEventDistributionAsync(int year);
        ServiceResult<List<MonthlyEventCountDto>> GetMonthlyEventDistribution(int year);

        Task<ServiceResult<CheckInRatioDto>> GetCheckInRatioAsync(string? maSuKien = null);
        ServiceResult<CheckInRatioDto> GetCheckInRatio(string? maSuKien = null);
    }
}
