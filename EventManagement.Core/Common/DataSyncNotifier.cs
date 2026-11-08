using System;

namespace EventManagement.Core.Common
{
    /// <summary>
    /// Bộ điều phối đồng bộ luồng dữ liệu thời gian thực giữa các phân hệ (Task T3-24).
    /// Hỗ trợ luồng dữ liệu tự động: Bán vé (F11) -> Soát vé (F13) -> Cập nhật tức thì Dashboard (F02) và Chi tiết (F05).
    /// </summary>
    public static class DataSyncNotifier
    {
        public static event Action<string?, int, decimal>? TicketSold;
        public static event Action<string?, string?>? TicketCheckedIn;
        public static event Action<string?, decimal>? ExpenseChanged;
        public static event Action<string?>? EventChanged;
        public static event Action? GeneralDataChanged;

        /// <summary>
        /// Phát thông báo khi một đơn đặt vé mới được xác nhận thành công trên Form F11.
        /// </summary>
        public static void NotifyTicketSold(string? maSuKien, int soLuong, decimal tongTien)
        {
            try
            {
                TicketSold?.Invoke(maSuKien, soLuong, tongTien);
                GeneralDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi phát sự kiện TicketSold: {ex.Message}");
            }
        }

        /// <summary>
        /// Phát thông báo khi một vé được quét và điểm danh thành công trên Form F13.
        /// </summary>
        public static void NotifyTicketCheckedIn(string? maSuKien, string? maVe)
        {
            try
            {
                TicketCheckedIn?.Invoke(maSuKien, maVe);
                GeneralDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi phát sự kiện TicketCheckedIn: {ex.Message}");
            }
        }

        /// <summary>
        /// Phát thông báo khi có khoản mục chi phí/ngân sách phát sinh trên Form F17.
        /// </summary>
        public static void NotifyExpenseChanged(string? maSuKien, decimal soTien)
        {
            try
            {
                ExpenseChanged?.Invoke(maSuKien, soTien);
                GeneralDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi phát sự kiện ExpenseChanged: {ex.Message}");
            }
        }

        /// <summary>
        /// Phát thông báo khi thông tin sự kiện được thêm mới/sửa đổi/hủy trên Form F04 / F05.
        /// </summary>
        public static void NotifyEventChanged(string? maSuKien, string? action = null)
        {
            try
            {
                EventChanged?.Invoke(maSuKien);
                GeneralDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi phát sự kiện EventChanged: {ex.Message}");
            }
        }

        /// <summary>
        /// Phát thông báo cập nhật dữ liệu chung toàn hệ thống.
        /// </summary>
        public static void NotifyDataChanged()
        {
            try
            {
                GeneralDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi phát sự kiện GeneralDataChanged: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa sạch các bộ lắng nghe sự kiện (dùng cho teardown unit test).
        /// </summary>
        public static void ResetHandlers()
        {
            TicketSold = null;
            TicketCheckedIn = null;
            ExpenseChanged = null;
            EventChanged = null;
            GeneralDataChanged = null;
        }
    }
}
