using System.Collections.Generic;

namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng 3: ChucNang - Danh mục chức năng / màn hình hệ thống (UC01 - UC21)
    /// </summary>
    public class ChucNang
    {
        public string MaChucNang { get; set; } = string.Empty;
        public string TenChucNang { get; set; } = string.Empty;
        public string MaForm { get; set; } = string.Empty;

        public virtual ICollection<PhanQuyen> PhanQuyens { get; set; } = new List<PhanQuyen>();
    }
}
