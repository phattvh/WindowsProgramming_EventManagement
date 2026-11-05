using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện dịch vụ Quản lý nhóm quyền & Ma trận phân quyền động F20 (Task T1-14 - Thành viên 1)
    /// </summary>
    public interface IRolePermissionService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<List<ChucNangDto>> GetAllFunctionsAsync();
        Task<List<PhanQuyenMatrixDto>> GetPermissionMatrixAsync(string roleId);
        Task<ServiceResult> SavePermissionMatrixAsync(string roleId, List<PhanQuyenMatrixDto> matrix);
        Task<ServiceResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto);
        Task<ServiceResult> DeleteRoleAsync(string roleId);
    }
}
