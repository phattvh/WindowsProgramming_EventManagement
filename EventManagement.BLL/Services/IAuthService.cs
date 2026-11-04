using System.Threading.Tasks;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<LoginResultDto>> LoginAsync(string username, string password);
        Task<ServiceResult> ChangePasswordAsync(string username, string currentPassword, string newPassword);
        Task<ServiceResult> UnlockUserAsync(string targetUserId);
    }
}
