using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện nghiệp vụ Đăng ký & Bán vé sự kiện (F11)
    /// Đảm trách quy tắc: BR-09 (Tồn kho vé), BR-10 (Mua 1-10 vé), BR-17 (Mã GUID QR), BR-18 (SqlTransaction)
    /// </summary>
    public interface ITicketOrderService
    {
        List<(string MaSuKien, string TenSuKien)> GetAvailableEvents();
        List<TicketTierSummaryDto> GetTicketTiersByEvent(string maSuKien);
        ServiceResult<TicketOrderResultDto> RegisterTicketOrder(RegisterTicketRequestDto request);
        ServiceResult<List<TicketDetailDto>> GetTicketsByEvent(string? maSuKien);
        ServiceResult<TicketDetailDto> GetTicketByCode(string codeOrQr);
    }
}
