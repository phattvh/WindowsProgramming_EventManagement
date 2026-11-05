using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện dịch vụ quản lý người dùng (F03) - Phụ trách: Thành viên 1
    /// </summary>
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync(string? keyword = null, string? roleId = null, string? status = null);
        Task<UserDto?> GetByIdAsync(string userId);
        Task<ServiceResult<UserDto>> CreateAsync(CreateUserDto dto);
        Task<ServiceResult> UpdateAsync(string userId, UpdateUserDto dto);
        Task<ServiceResult> DeleteAsync(string userId);
        Task<ServiceResult> UnlockUserAsync(string userId);
        Task<ServiceResult> ToggleLockAsync(string userId);
    }
}