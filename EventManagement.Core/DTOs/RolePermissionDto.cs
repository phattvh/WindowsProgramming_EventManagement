using System;
using System.Collections.Generic;

namespace EventManagement.Core.DTOs
{
    /// <summary>
    /// DTO thông tin Nhóm quyền (Vai trò)
    /// </summary>
    public class RoleDto
    {
        public string MaNhomQuyen { get; set; } = string.Empty;
        public string TenNhomQuyen { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public int SoLuongNguoiDung { get; set; }
        public bool IsSystemRole => MaNhomQuyen is "ADMIN" or "MANAGER" or "STAFF";
    }

    /// <summary>
    /// DTO thông tin Chức năng hệ thống
    /// </summary>
    public class ChucNangDto
    {
        public string MaChucNang { get; set; } = string.Empty;
        public string TenChucNang { get; set; } = string.Empty;
        public string MaForm { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO dòng ma trận phân quyền trên Form F20
    /// </summary>
    public class PhanQuyenMatrixDto
    {
        public string MaNhomQuyen { get; set; } = string.Empty;
        public string MaChucNang { get; set; } = string.Empty;
        public string TenChucNang { get; set; } = string.Empty;
        public string MaForm { get; set; } = string.Empty;
        public bool CoQuyen { get; set; }
    }

    /// <summary>
    /// DTO tạo mới nhóm quyền
    /// </summary>
    public class CreateRoleDto
    {
        public string MaNhomQuyen { get; set; } = string.Empty;
        public string TenNhomQuyen { get; set; } = string.Empty;
        public string? MoTa { get; set; }
    }
}
