using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng 1: NhomQuyen - Định nghĩa nhóm quyền / vai trò (ADMIN, MANAGER, STAFF)
    /// </summary>
    public class NhomQuyen
    {
        public string MaNhomQuyen { get; set; } = string.Empty;
        public string TenNhomQuyen { get; set; } = string.Empty;
        public string? MoTa { get; set; }

        public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
        public virtual ICollection<PhanQuyen> PhanQuyens { get; set; } = new List<PhanQuyen>();
    }
}
