namespace EventManagement.Core.Entities
{
    /// <summary>
    /// Bảng 4: PhanQuyen - Ma trận phân quyền giữa Nhóm quyền và Chức năng
    /// </summary>
    public class PhanQuyen
    {
        public string MaNhomQuyen { get; set; } = string.Empty;
        public string MaChucNang { get; set; } = string.Empty;
        public bool CoQuyen { get; set; } = true;

        public virtual NhomQuyen? NhomQuyen { get; set; }
        public virtual ChucNang? ChucNang { get; set; }
    }
}
