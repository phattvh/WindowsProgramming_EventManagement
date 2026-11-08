using System;
using System.Drawing;
using System.Windows.Forms;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Form cơ sở (FrmBase) chuẩn hóa toàn bộ hệ thống giao diện 20 Form
    /// Tuân thủ quy định tại docs/08. Quy Tắc Thiết Kế Giao Diện.md:
    /// - Phông chữ mặc định: Segoe UI 9pt
    /// - Bảng màu chuẩn doanh nghiệp (Corporate Blue Theme & Neutral palette)
    /// - Canh giữa màn hình khi khởi chạy
    /// </summary>
    public class FrmBase : Form
    {
        // Bảng màu chuẩn doanh nghiệp (Corporate Blue Theme - Docs 08)
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
            ApplyDefaultSettings();
        }

        private void ApplyDefaultSettings()
        {
            // Chuẩn hóa Font Segoe UI 9pt theo tài liệu UI/UX thiết kế
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = ColorBackground;
            this.ForeColor = ColorText;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.DoubleBuffered = true;
            this.MinimumSize = new Size(1024, 680);

            if (!(this is FrmLogin))
            {
                this.MaximizeBox = true;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode && !(this is FrmLogin))
            {
                this.MaximizeBox = true;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Maximized;
                ApplyResponsiveLayout(this);
            }
        }

        /// <summary>
        /// Tự động thích ứng giao diện toàn màn hình cho mọi độ phân giải (1920x1080, 1366x768, 2K, 4K)
        /// Tự động canh chỉnh Anchor và Column Fill cho các DataGridView và Container.
        /// </summary>
        protected virtual void ApplyResponsiveLayout(Control container)
        {
            if (container == null) return;

            foreach (Control c in container.Controls)
            {
                if (c is DataGridView dgv)
                {
                    if (dgv.Dock == DockStyle.None)
                    {
                        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    }
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                else if (c is GroupBox grp && grp.Dock == DockStyle.None)
                {
                    if (grp.Top >= container.ClientSize.Height * 0.4)
                    {
                        grp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    }
                }
                else if (c is Panel pnl && pnl.Dock == DockStyle.None)
                {
                    string name = pnl.Name.ToLowerInvariant();
                    if (name.Contains("button") || pnl.Top >= container.ClientSize.Height * 0.75)
                    {
                        pnl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    }
                    else if (name.Contains("filter") || name.Contains("search") || name.Contains("kpi") || pnl.Top < container.ClientSize.Height * 0.4)
                    {
                        pnl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    }
                }
                else if (c is TabControl tab && tab.Dock == DockStyle.None)
                {
                    tab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                }
            }
        }

        protected virtual void ApplyStandardTheme()
        {
            // Có thể ghi đè ở các Form con để áp dụng theme tùy biến
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

        /// <summary>
        /// Định kiểu nút bấm chuẩn theo quy chuẩn vai trò nút (ButtonRole)
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
}
