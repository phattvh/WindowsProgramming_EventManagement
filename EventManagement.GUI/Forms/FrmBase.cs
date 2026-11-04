using System.Drawing;
using System.Windows.Forms;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Form cơ sở FrmBase - Chuẩn hóa giao diện Segoe UI 9pt cho toàn bộ 20 Form (Task T1-07)
    /// </summary>
    public class FrmBase : Form
    {
        public FrmBase()
        {
            ApplyDefaultSettings();
        }

        private void ApplyDefaultSettings()
        {
            // Chuẩn hóa Font Segoe UI 9pt theo tài liệu UI/UX thiết kế
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = Color.White;
        }

        protected virtual void ApplyStandardTheme()
        {
            // Có thể ghi đè ở các Form con để áp dụng theme tùy biến
        }
    }
}
