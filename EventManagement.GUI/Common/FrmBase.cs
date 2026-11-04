using System;
using System.Drawing;
using System.Windows.Forms;

namespace EventManagement.GUI.Common
{
    /// <summary>
    /// Form cơ sở chuẩn mực cho toàn hệ thống (Áp dụng theo 08. Quy Tắc Thiết Kế Giao Diện)
    /// </summary>
    public class FrmBase : Form
    {
        // Bảng màu chuẩn doanh nghiệp (Corporate Blue Theme)
        public static readonly Color ColorPrimary = Color.FromArgb(30, 58, 138);      // Xanh Navy chính #1E3A8A
        public static readonly Color ColorPrimaryLight = Color.FromArgb(37, 99, 235); // Xanh biển tươi #2563EB
        public static readonly Color ColorBackground = Color.FromArgb(248, 250, 252); // Xám sáng nền #F8FAFC
        public static readonly Color ColorSurface = Color.FromArgb(255, 255, 255);    // Trắng bề mặt #FFFFFF
        public static readonly Color ColorText = Color.FromArgb(15, 23, 42);          // Xám đen chữ #0F172A
        public static readonly Color ColorTextMuted = Color.FromArgb(100, 116, 139);  // Xám nhạt chú thích #64748B
        public static readonly Color ColorBorder = Color.FromArgb(226, 232, 240);     // Xám viền #E2E8F0
        public static readonly Color ColorSuccess = Color.FromArgb(22, 163, 74);      // Xanh lá #16A34A
        public static readonly Color ColorWarning = Color.FromArgb(217, 119, 6);      // Vàng cam #D97706
        public static readonly Color ColorDanger = Color.FromArgb(220, 38, 38);       // Đỏ cảnh báo #DC2626

        public FrmBase()
        {
            // Chuẩn hóa Font Segoe UI 9pt
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = ColorBackground;
            this.ForeColor = ColorText;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AutoScaleMode = AutoScaleMode.Font;
        }

        /// <summary>
        /// Chuẩn hóa giao diện DataGridView theo 5 quy tắc chuẩn trong docs 08
        /// </summary>
        protected void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = ColorSurface;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = ColorBorder;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Header Style
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ColorText;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row Style & Zebra Striping
            dgv.RowTemplate.Height = 32;
            dgv.DefaultCellStyle.BackColor = ColorSurface;
            dgv.DefaultCellStyle.ForeColor = ColorText;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254); // Xanh nhạt khi chọn
            dgv.DefaultCellStyle.SelectionForeColor = ColorText;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        }

        /// <summary>
        /// Tạo nút bấm chuẩn giao diện
        /// </summary>
        protected Button CreateStyledButton(string text, Color backColor, Color foreColor, int width = 90, int height = 32)
        {
            return new Button
            {
                Text = text,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(width, height),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }
    }
}
