using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;
using EventManagement.Core.Security;
using EventManagement.Core.Session;
using EventManagement.DAL;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Triển khai dịch vụ Quản lý nhóm quyền & Phân quyền
    /// </summary>
    public class RolePermissionService : IRolePermissionService
    {
        private readonly AppDbContext _context;

        public RolePermissionService() : this(new AppDbContext())
        {
        }

        public RolePermissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await _context.NhomQuyens
                .Select(r => new RoleDto
                {
                    MaNhomQuyen = r.MaNhomQuyen,
                    TenNhomQuyen = r.TenNhomQuyen,
                    MoTa = r.MoTa,
                    SoLuongNguoiDung = r.NguoiDungs.Count()
                })
                .OrderBy(r => r.MaNhomQuyen)
                .ToListAsync();
        }

        public async Task<List<ChucNangDto>> GetAllFunctionsAsync()
        {
            return await _context.ChucNangs
                .OrderBy(c => c.MaChucNang)
                .Select(c => new ChucNangDto
                {
                    MaChucNang = c.MaChucNang,
                    TenChucNang = c.TenChucNang,
                    MaForm = c.MaForm
                })
                .ToListAsync();
        }

        public async Task<List<PhanQuyenMatrixDto>> GetPermissionMatrixAsync(string roleId)
        {
            var allFunctions = await _context.ChucNangs
                .OrderBy(c => c.MaChucNang)
                .ToListAsync();

            var currentPermissions = await _context.PhanQuyens
                .Where(p => p.MaNhomQuyen == roleId)
                .ToDictionaryAsync(p => p.MaChucNang, p => p.CoQuyen);

            var matrix = new List<PhanQuyenMatrixDto>();
            foreach (var func in allFunctions)
            {
                bool hasPermission = false;
                if (currentPermissions.TryGetValue(func.MaChucNang, out bool coQuyen))
                {
                    hasPermission = coQuyen;
                }
                else if (roleId == "ADMIN")
                {
                    hasPermission = true;
                }

                matrix.Add(new PhanQuyenMatrixDto
                {
                    MaNhomQuyen = roleId,
                    MaChucNang = func.MaChucNang,
                    TenChucNang = func.TenChucNang,
                    MaForm = func.MaForm,
                    CoQuyen = hasPermission
                });
            }

            return matrix;
        }

        public async Task<ServiceResult> SavePermissionMatrixAsync(string roleId, List<PhanQuyenMatrixDto> matrix)
        {
            if (CurrentSession.IsLoggedIn) SecurityGuard.YeuCauQuyen("UC20");
            var role = await _context.NhomQuyens.FindAsync(roleId);
            if (role == null)
            {
                return ServiceResult.Failed($"Không tìm thấy nhóm quyền '{roleId}' trong hệ thống!");
            }

            // Ràng buộc an toàn: Không được thu hồi quyền của nhóm ADMIN
            if (roleId == "ADMIN")
            {
                bool hasRevoked = matrix.Any(m => !m.CoQuyen);
                if (hasRevoked)
                {
                    return ServiceResult.Failed("Không thể thu hồi quyền hạn cốt lõi của Nhóm Quản trị viên (ADMIN)!");
                }
            }

            var existingPermissions = await _context.PhanQuyens
                .Where(p => p.MaNhomQuyen == roleId)
                .ToListAsync();

            foreach (var item in matrix)
            {
                var existing = existingPermissions.FirstOrDefault(p => p.MaChucNang == item.MaChucNang);
                if (existing != null)
                {
                    existing.CoQuyen = item.CoQuyen;
                }
                else
                {
                    _context.PhanQuyens.Add(new PhanQuyen
                    {
                        MaNhomQuyen = roleId,
                        MaChucNang = item.MaChucNang,
                        CoQuyen = item.CoQuyen
                    });
                }
            }

            await _context.SaveChangesAsync();
            return ServiceResult.Success($"Cập nhật ma trận phân quyền cho vai trò '{role.TenNhomQuyen}' thành công! Quyền hạn mới sẽ có hiệu lực trong phiên đăng nhập tiếp theo.");
        }

        public async Task<ServiceResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
        {
            if (CurrentSession.IsLoggedIn) SecurityGuard.YeuCauQuyen("UC20");
            if (string.IsNullOrWhiteSpace(dto.MaNhomQuyen))
            {
                return ServiceResult<RoleDto>.Failed("Mã nhóm quyền không được để trống!");
            }

            string cleanCode = dto.MaNhomQuyen.Trim().ToUpper();
            if (!Regex.IsMatch(cleanCode, "^[A-Z0-9_]{3,20}$"))
            {
                return ServiceResult<RoleDto>.Failed("Mã nhóm quyền phải từ 3 đến 20 ký tự (chữ hoa không dấu, số hoặc gạch dưới)!");
            }

            if (string.IsNullOrWhiteSpace(dto.TenNhomQuyen))
            {
                return ServiceResult<RoleDto>.Failed("Tên nhóm quyền không được để trống!");
            }

            bool exists = await _context.NhomQuyens.AnyAsync(r => r.MaNhomQuyen == cleanCode);
            if (exists)
            {
                return ServiceResult<RoleDto>.Failed($"Mã nhóm quyền '{cleanCode}' đã tồn tại!");
            }

            var newRole = new NhomQuyen
            {
                MaNhomQuyen = cleanCode,
                TenNhomQuyen = dto.TenNhomQuyen.Trim(),
                MoTa = dto.MoTa?.Trim()
            };

            await _context.NhomQuyens.AddAsync(newRole);

            // Tự động khởi tạo bản ghi phân quyền mặc định (CoQuyen = false) cho tất cả chức năng
            var functions = await _context.ChucNangs.ToListAsync();
            foreach (var f in functions)
            {
                _context.PhanQuyens.Add(new PhanQuyen
                {
                    MaNhomQuyen = cleanCode,
                    MaChucNang = f.MaChucNang,
                    CoQuyen = false
                });
            }

            await _context.SaveChangesAsync();

            var roleDto = new RoleDto
            {
                MaNhomQuyen = newRole.MaNhomQuyen,
                TenNhomQuyen = newRole.TenNhomQuyen,
                MoTa = newRole.MoTa,
                SoLuongNguoiDung = 0
            };

            return ServiceResult<RoleDto>.Success(roleDto, $"Tạo mới nhóm quyền '{newRole.TenNhomQuyen}' thành công!");
        }

        public async Task<ServiceResult> DeleteRoleAsync(string roleId)
        {
            if (CurrentSession.IsLoggedIn) SecurityGuard.YeuCauQuyen("UC20");
            var role = await _context.NhomQuyens
                .Include(r => r.NguoiDungs)
                .Include(r => r.PhanQuyens)
                .FirstOrDefaultAsync(r => r.MaNhomQuyen == roleId);

            if (role == null)
            {
                return ServiceResult.Failed($"Không tìm thấy nhóm quyền '{roleId}'!");
            }

            // Chặn xóa các nhóm quyền hệ thống
            if (roleId is "ADMIN" or "MANAGER" or "STAFF")
            {
                return ServiceResult.Failed($"Không thể xóa các nhóm quyền mặc định của hệ thống ({roleId})!");
            }

            // Chặn xóa nếu còn người dùng gán vai trò này
            if (role.NguoiDungs.Any())
            {
                return ServiceResult.Failed($"Không thể xóa nhóm quyền '{role.TenNhomQuyen}' vì đang có {role.NguoiDungs.Count} người dùng thuộc vai trò này!");
            }

            // Xóa các liên kết phân quyền trước
            _context.PhanQuyens.RemoveRange(role.PhanQuyens);
            _context.NhomQuyens.Remove(role);

            await _context.SaveChangesAsync();
            return ServiceResult.Success($"Đã xóa nhóm quyền '{role.TenNhomQuyen}' thành công!");
        }

        /// <summary>
        /// Logic thẩm định dữ liệu đầu vào khi tạo nhóm quyền mới
        /// </summary>
        public static (bool IsValid, string? ErrorMessage) ValidateRoleInput(string roleCode, string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleCode))
            {
                return (false, "Mã nhóm quyền không được để trống!");
            }

            string cleanCode = roleCode.Trim().ToUpper();
            if (!Regex.IsMatch(cleanCode, "^[A-Z0-9_]{3,20}$"))
            {
                return (false, "Mã nhóm quyền phải từ 3 đến 20 ký tự (chữ hoa không dấu, số hoặc gạch dưới)!");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                return (false, "Tên nhóm quyền không được để trống!");
            }

            return (true, null);
        }
    }
}
