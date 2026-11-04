namespace EventManagement.GUI.Common
{
    /// <summary>
    /// Ba trạng thái chuẩn mực của Form nghiệp vụ (Theo Quy tắc 08. Quy Tắc Thiết Kế Giao Diện)
    /// </summary>
    public enum FormMode
    {
        View, // Xem danh sách (mặc định - khóa ô nhập, bật Thêm/Sửa/Xóa, tắt Lưu/Hủy)
        Add,  // Thêm mới (mở ô nhập, xóa trắng, tắt Thêm/Sửa/Xóa, bật Lưu/Hủy)
        Edit  // Chỉnh sửa (mở ô nhập trừ khóa chính, tắt Thêm/Sửa/Xóa, bật Lưu/Hủy)
    }
}
