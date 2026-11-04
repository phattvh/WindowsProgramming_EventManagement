using System;
using System.Windows.Forms;
using EventManagement.GUI.Forms;

namespace EventManagement.GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            // Khởi chạy với Form F10 (Quản lý người tham gia)
            Application.Run(new FrmAttendee());
        }    
    }
}
