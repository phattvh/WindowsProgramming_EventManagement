using System.Drawing;
using System.Windows.Forms;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Form cơ sở (FrmBase) chuẩn hóa toàn bộ hệ thống giao diện
    /// Tuân thủ quy định tại docs/08. Quy Tắc Thiết Kế Giao Diện.md:
    /// - Phông chữ mặc định: Segoe UI 9pt
    /// - Màu nền form: Neutral Light #F3F4F6
    /// - Màu chữ chính: Text Dark #1F2937
    /// - Canh giữa màn hình khi khởi chạy
    /// </summary>
    public class FrmBase : Form
    {
        public FrmBase()
        {
            InitializeBaseSettings();
        }

        private void InitializeBaseSettings()
        {
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = Color.FromArgb(243, 244, 246); // #F3F4F6
            this.ForeColor = Color.FromArgb(31, 41, 55);    // #1F2937
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Tạo nút bấm chuẩn theo quy chuẩn bảng màu thiết kế
        /// </summary>
        public static void StyleButton(Button btn, ButtonRole role)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.Cursor = Cursors.Hand;
            btn.Height = 34;

            switch (role)
            {
                case ButtonRole.Primary:
                    btn.BackColor = Color.FromArgb(37, 99, 235);   // Action Blue #2563EB
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(29, 78, 216);
                    break;

                case ButtonRole.Secondary:
                    btn.BackColor = Color.White;
                    btn.ForeColor = Color.FromArgb(55, 65, 81);     // Text Dark #374151
                    btn.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219); // Border Gray #D1D5DB
                    break;

                case ButtonRole.Danger:
                    btn.BackColor = Color.FromArgb(239, 68, 68);    // Danger Red #EF4444
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(220, 38, 38);
                    break;

                case ButtonRole.Success:
                    btn.BackColor = Color.FromArgb(16, 185, 129);  // Success Green #10B981
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(5, 150, 105);
                    break;
            }
        }
    }

    public enum ButtonRole
    {
        Primary,
        Secondary,
        Danger,
        Success
    }

    public enum FormMode
    {
        View,
        Add,
        Edit
    }
}
