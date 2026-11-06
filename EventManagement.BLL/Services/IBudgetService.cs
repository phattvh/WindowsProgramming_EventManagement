using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện dịch vụ quản lý chi phí & ngân sách sự kiện (F17 - BR-14, BR-15)
    /// </summary>
    public interface IBudgetService
    {
        List<(string MaSuKien, string TenSuKien, decimal NganSachDuKien)> GetAvailableEvents();
        ServiceResult<List<ExpenseDto>> GetExpensesByEvent(string? maSuKien);
        ServiceResult<ExpenseDto> GetExpenseById(string maChiPhi);
        ServiceResult<BudgetAlertDto> GetBudgetStatus(string maSuKien, decimal khoanChiSapThem = 0);
        ServiceResult<BudgetAlertDto> AddExpense(ExpenseDto dto);
        ServiceResult<BudgetAlertDto> UpdateExpense(ExpenseDto dto);
        ServiceResult DeleteExpense(string maChiPhi);
        string GetNextExpenseCode();
    }
}
