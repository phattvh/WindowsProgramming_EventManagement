using System;
using System.Windows.Forms;
using EventManagement.GUI.Forms;

namespace EventManagement.GUI
{
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi chạy chính của ứng dụng Windows Forms.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            // Khởi chạy mặc định Form F06 (hoặc F07) trong giai đoạn phát triển phân hệ Thành viên 2
            Application.Run(new FrmEventType());
        }
    }
}
