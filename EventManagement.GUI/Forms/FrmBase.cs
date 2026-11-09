using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.Core.Session;
using EventManagement.GUI.Common;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Form cơ sở (FrmBase) chuẩn hóa toàn bộ hệ thống giao diện 20 Form
    /// Tuân thủ quy định tại docs/08. Quy Tắc Thiết Kế Giao Diện.md:
    /// - Phông chữ mặc định: Segoe UI 9pt
    /// - Bảng màu chuẩn doanh nghiệp (Corporate Blue Theme & Neutral palette)
    /// - Canh giữa màn hình khi khởi chạy
    /// - Tích hợp thanh MenuStrip điều hướng toàn cục đa năng kèm phím tắt (Hotkeys/Mnemonics)
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

        private MenuStrip? _navMenuStrip;

        /// <summary>
        /// Cho phép bật/tắt thanh Menu điều hướng chung trên Form. Mặc định là true cho các màn hình nghiệp vụ.
        /// </summary>
        public bool EnableGlobalNavigationMenu { get; set; } = true;

        /// <summary>
        /// Truy cập điều khiển MenuStrip dùng chung của Form.
        /// </summary>
        public MenuStrip? NavigationMenuStrip => _navMenuStrip;

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

            if (this is not FrmLogin)
            {
                this.MinimumSize = new Size(1024, 680);
                this.MaximizeBox = true;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.MinimumSize = new Size(420, 345);
                this.MaximumSize = new Size(436, 384);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.WindowState = FormWindowState.Normal;
            }

            // Tự động tắt menu điều hướng trên các hộp thoại nhỏ/đăng nhập
            if (this is FrmLogin || this is FrmChangePassword || this is FrmServerConfig)
            {
                EnableGlobalNavigationMenu = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode && this is not FrmLogin)
            {
                this.MaximizeBox = true;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Maximized;

                if (EnableGlobalNavigationMenu)
                {
                    InitializeGlobalNavigationMenu();
                }

                ApplyResponsiveLayout(this);
            }
            else if (!DesignMode && this is FrmLogin)
            {
                this.WindowState = FormWindowState.Normal;
                this.ClientSize = new Size(420, 345);
                this.CenterToScreen();
            }
        }

        /// <summary>
        /// Khởi tạo thanh MenuStrip dùng chung cho toàn bộ các Form nghiệp vụ trong hệ thống.
        /// Bố cục chuẩn gồm 7 menu chính với phím tắt (Hotkeys) và phím truy cập nhanh (Mnemonics).
        /// </summary>
        public virtual void InitializeGlobalNavigationMenu()
        {
            if (_navMenuStrip != null || !EnableGlobalNavigationMenu) return;
            if (this is FrmLogin || this is FrmChangePassword || this is FrmServerConfig) return;

            _navMenuStrip = new MenuStrip
            {
                Name = "navMenuStrip",
                Dock = DockStyle.Top,
                BackColor = ColorSurface,
                ForeColor = ColorText,
                Font = new Font("Segoe UI", 9.25F, FontStyle.Regular, GraphicsUnit.Point),
                Padding = new Padding(6, 2, 0, 2),
                ShowItemToolTips = true,
                Renderer = new ToolStripProfessionalRenderer(new CorporateMenuColorTable())
            };

            // 1. MENU HỆ THỐNG (&H)
            var mnuSystem = new ToolStripMenuItem("&Hệ thống");
            mnuSystem.DropDownItems.Add(CreateMenuItem("Trang chủ", Keys.Control | Keys.Shift | Keys.H, "UC02", (s, e) => NavigateTo("UC02", () => new FrmDashboard(), "Trang chủ")));
            mnuSystem.DropDownItems.Add(CreateMenuItem("Cấu hình máy chủ", Keys.None, "UC00", (s, e) => NavigateTo("UC00", () => new FrmServerConfig(), "Cấu hình máy chủ")));
            mnuSystem.DropDownItems.Add(CreateMenuItem("Đổi mật khẩu", Keys.Control | Keys.Shift | Keys.W, "UC21", (s, e) => NavigateTo("UC21", () => new FrmChangePassword(), "Đổi mật khẩu")));
            mnuSystem.DropDownItems.Add(new ToolStripSeparator());
            mnuSystem.DropDownItems.Add(CreateMenuItem("Đăng xuất", Keys.Control | Keys.Shift | Keys.L, null, (s, e) => PerformLogout()));
            mnuSystem.DropDownItems.Add(CreateMenuItem("Thoát", Keys.Alt | Keys.F4, null, (s, e) => PerformExit()));

            // 2. MENU SỰ KIỆN (&S)
            var mnuEvents = new ToolStripMenuItem("&Sự kiện");
            mnuEvents.DropDownItems.Add(CreateMenuItem("Danh sách sự kiện", Keys.Control | Keys.Shift | Keys.E, "UC04", (s, e) => NavigateTo("UC04", () => new FrmEventList(), "Danh sách sự kiện")));
            mnuEvents.DropDownItems.Add(CreateMenuItem("Chi tiết sự kiện", Keys.None, "UC05", (s, e) => NavigateTo("UC05", () => new FrmEventDetail(), "Chi tiết sự kiện")));
            mnuEvents.DropDownItems.Add(CreateMenuItem("Lịch trình", Keys.Control | Keys.Shift | Keys.S, "UC08", (s, e) => NavigateTo("UC08", () => new FrmSchedule(), "Lịch trình")));
            mnuEvents.DropDownItems.Add(CreateMenuItem("Loại sự kiện", Keys.None, "UC06", (s, e) => NavigateTo("UC06", () => new FrmEventType(), "Loại sự kiện")));
            mnuEvents.DropDownItems.Add(CreateMenuItem("Địa điểm tổ chức", Keys.None, "UC07", (s, e) => NavigateTo("UC07", () => new FrmVenue(), "Địa điểm tổ chức")));

            // 3. MENU VÉ & SOÁT VÉ (&V)
            var mnuTickets = new ToolStripMenuItem("&Vé && Soát vé");
            mnuTickets.DropDownItems.Add(CreateMenuItem("Đăng ký && Bán vé", Keys.Control | Keys.Shift | Keys.T, "UC11", (s, e) => NavigateTo("UC11", () => new FrmTicketOrder(), "Đăng ký & Bán vé")));
            mnuTickets.DropDownItems.Add(CreateMenuItem("Hạng vé", Keys.None, "UC12", (s, e) => NavigateTo("UC12", () => new FrmTicketTier(), "Hạng vé")));
            mnuTickets.DropDownItems.Add(CreateMenuItem("Soát vé QR", Keys.Control | Keys.Shift | Keys.K, "UC13", (s, e) => NavigateTo("UC13", () => new FrmCheckInCamera(), "Soát vé QR")));

            // 4. MENU NHÂN SỰ & ĐỐI TÁC (&N)
            var mnuStaff = new ToolStripMenuItem("&Nhân sự && Đối tác");
            mnuStaff.DropDownItems.Add(CreateMenuItem("Danh mục nhân sự", Keys.None, "UC14", (s, e) => NavigateTo("UC14", () => new FrmStaff(), "Danh mục nhân sự")));
            mnuStaff.DropDownItems.Add(CreateMenuItem("Phân công ca trực", Keys.None, "UC15", (s, e) => NavigateTo("UC15", () => new FrmStaffAssignment(), "Phân công ca trực")));
            mnuStaff.DropDownItems.Add(CreateMenuItem("Diễn giả && Khách mời", Keys.Control | Keys.Shift | Keys.J, "UC09", (s, e) => NavigateTo("UC09", () => new FrmSpeaker(), "Diễn giả & Khách mời")));
            mnuStaff.DropDownItems.Add(CreateMenuItem("Hồ sơ người tham dự", Keys.None, "UC10", (s, e) => NavigateTo("UC10", () => new FrmAttendee(), "Hồ sơ người tham dự")));
            mnuStaff.DropDownItems.Add(CreateMenuItem("Nhà tài trợ", Keys.None, "UC16", (s, e) => NavigateTo("UC16", () => new FrmSponsor(), "Nhà tài trợ")));
            mnuStaff.DropDownItems.Add(CreateMenuItem("Khảo sát && Phản hồi", Keys.None, "UC18", (s, e) => NavigateTo("UC18", () => new FrmFeedback(), "Khảo sát & Phản hồi")));

            // 5. MENU TÀI CHÍNH & BÁO CÁO (&T)
            var mnuFinance = new ToolStripMenuItem("&Tài chính && BC");
            mnuFinance.DropDownItems.Add(CreateMenuItem("Ngân sách && Chi phí", Keys.Control | Keys.Shift | Keys.B, "UC17", (s, e) => NavigateTo("UC17", () => new FrmBudgetExpense(), "Ngân sách & Chi phí")));
            mnuFinance.DropDownItems.Add(CreateMenuItem("Báo cáo && Thống kê", Keys.Control | Keys.Shift | Keys.R, "UC19", (s, e) => NavigateTo("UC19", () => new FrmReportStatistics(), "Báo cáo & Thống kê")));

            // 6. MENU QUẢN TRỊ NGƯỜI DÙNG (&Q)
            var mnuAdmin = new ToolStripMenuItem("&Quản trị");
            mnuAdmin.DropDownItems.Add(CreateMenuItem("Quản lý người dùng", Keys.Control | Keys.Shift | Keys.U, "UC03", (s, e) => NavigateTo("UC03", () => new FrmUserManagement(), "Quản lý người dùng")));

            // 7. MENU TRỢ GIÚP (&G)
            var mnuHelp = new ToolStripMenuItem("&Trợ giúp");
            mnuHelp.DropDownItems.Add(CreateMenuItem("Hướng dẫn phím tắt", Keys.F1, null, (s, e) => ShowShortcutsHelpDialog()));
            mnuHelp.DropDownItems.Add(new ToolStripSeparator());
            mnuHelp.DropDownItems.Add(CreateMenuItem("Giới thiệu phần mềm", Keys.None, null, (s, e) => ShowAboutDialog()));

            _navMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                mnuSystem,
                mnuEvents,
                mnuTickets,
                mnuStaff,
                mnuFinance,
                mnuAdmin,
                mnuHelp
            });

            this.MainMenuStrip = _navMenuStrip;
            this.Controls.Add(_navMenuStrip);

            // Gửi menu về đáy của Z-order để nó dock ở vị trí ngoài cùng mép trên (Top = 0)
            // Nhờ đó các Panel header/top bên dưới sẽ tự động dời xuống đúng bằng chiều cao của menu,
            // bảo đảm 100% không bị tình trạng chữ đè khung hay khung đè chữ.
            _navMenuStrip.SendToBack();

            RefreshMenuPermissions();
        }

        /// <summary>
        /// Tạo nhanh một ToolStripMenuItem chuẩn hóa giao diện và phím tắt
        /// </summary>
        private static ToolStripMenuItem CreateMenuItem(
            string text,
            Keys shortcut = Keys.None,
            string? tag = null,
            EventHandler? onClick = null,
            string? toolTip = null)
        {
            var item = new ToolStripMenuItem(text)
            {
                ShowShortcutKeys = shortcut != Keys.None,
                ShortcutKeys = shortcut,
                Tag = tag,
                ToolTipText = toolTip ?? text.Replace("&", string.Empty)
            };

            if (onClick != null)
            {
                item.Click += onClick;
            }

            return item;
        }

        /// <summary>
        /// Đồng bộ phân quyền hiển thị và làm mờ các mục menu theo phiên đăng nhập hiện tại
        /// </summary>
        public void RefreshMenuPermissions()
        {
            if (_navMenuStrip != null && CurrentSession.IsLoggedIn)
            {
                PermissionHelper.ApplyToolStripAuthorization(_navMenuStrip, hideInsteadOfDisable: false);
            }
        }

        /// <summary>
        /// Điều hướng an toàn giữa các màn hình, ngăn chặn mở đúp màn hình trùng lặp
        /// </summary>
        protected virtual void NavigateTo<TTargetForm>(string maChucNang, Func<TTargetForm> factory, string tenChucNang) where TTargetForm : Form
        {
            // 1. Nếu đang đứng ngay trên Form đích, chỉ cần kích hoạt lại Form
            if (this is TTargetForm)
            {
                this.Activate();
                return;
            }

            // 2. Nếu Form đích là Dashboard, đóng Form hiện tại để trở về Dashboard gốc bên dưới
            if (typeof(TTargetForm) == typeof(FrmDashboard))
            {
                if (this is not FrmDashboard)
                {
                    this.Close();
                    return;
                }
            }

            // 3. Nếu Form chủ (Owner) mở ra form này chính là Form đích, đóng form hiện tại để quay về
            if (this.Owner != null && this.Owner.GetType() == typeof(TTargetForm))
            {
                this.Close();
                return;
            }

            // 4. Khởi chạy thông qua cơ chế kiểm tra phân quyền 2 lớp (AD-4)
            PermissionHelper.SafeLaunchDialog(this, maChucNang, factory, tenChucNang);
        }

        private void PerformLogout()
        {
            var res = MessageBox.Show(
                this,
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                CurrentSession.DangXuat();
                this.DialogResult = DialogResult.Retry;
                this.Close();
            }
        }

        private void PerformExit()
        {
            var res = MessageBox.Show(
                this,
                "Bạn có chắc chắn muốn thoát toàn bộ ứng dụng?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ShowShortcutsHelpDialog()
        {
            string msg =
                "====================================================\n" +
                "         HỆ THỐNG PHÍM TẮT & ĐIỀU HƯỚNG NHANH       \n" +
                "====================================================\n\n" +
                "1. PHÍM TẮT ĐIỀU HƯỚNG TRANG (TỔ HỢP PHÍM):\n" +
                "   • Ctrl + Shift + H : Trang chủ / Dashboard\n" +
                "   • Ctrl + Shift + E : Danh sách sự kiện tổng thể\n" +
                "   • Ctrl + Shift + S : Lịch trình sự kiện\n" +
                "   • Ctrl + Shift + T : Đăng ký & Bán vé\n" +
                "   • Ctrl + Shift + K : Soát vé qua Webcam QR\n" +
                "   • Ctrl + Shift + J : Quản lý diễn giả\n" +
                "   • Ctrl + Shift + B : Quản lý ngân sách & Chi phí\n" +
                "   • Ctrl + Shift + R : Báo cáo & Thống kê tài chính\n" +
                "   • Ctrl + Shift + U : Quản lý người dùng & Mở khóa\n" +
                "   • Ctrl + Shift + Q : Phân quyền & Nhóm quyền\n" +
                "   • Ctrl + Shift + W : Đổi mật khẩu tài khoản\n" +
                "   • Ctrl + Shift + L : Đăng xuất tài khoản\n" +
                "   • Alt + F4         : Thoát ứng dụng\n" +
                "   • F1               : Mở hướng dẫn phím tắt này\n\n" +
                "2. PHÍM TRUY CẬP NHANH MENU (ALT + KÝ TỰ GẠCH CHÂN):\n" +
                "   • Alt + H : Mở menu Hệ thống\n" +
                "   • Alt + S : Mở menu Sự kiện\n" +
                "   • Alt + V : Mở menu Vé & Soát vé\n" +
                "   • Alt + N : Mở menu Nhân sự & Khách\n" +
                "   • Alt + T : Mở menu Tài chính & Báo cáo\n" +
                "   • Alt + Q : Mở menu Quản trị người dùng\n" +
                "   • Alt + G : Mở menu Trợ giúp\n\n" +
                "3. PHÍM THAO TÁC DỮ LIỆU CHUẨN TẠI MỖI MÀN HÌNH:\n" +
                "   • Ctrl + N           : Thêm mới bản ghi\n" +
                "   • Ctrl + E (hoặc F3) : Chỉnh sửa bản ghi đang chọn\n" +
                "   • Delete             : Xóa bản ghi (có xác nhận)\n" +
                "   • Ctrl + S           : Lưu dữ liệu khi đang Thêm/Sửa\n" +
                "   • Escape             : Hủy thao tác nhập liệu\n" +
                "   • F5                 : Nạp lại danh sách dữ liệu\n";

            MessageBox.Show(this, msg, "Danh Mục Phím Tắt Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowAboutDialog()
        {
            string msg =
                "HỆ THỐNG QUẢN LÝ SỰ KIỆN (EVENT MANAGEMENT SYSTEM)\n" +
                "Đồ án môn Lập trình Windows Cuối kỳ\n\n" +
                "• Phiên bản: 1.0.0 (Windows Forms .NET 8.0)\n" +
                "• Chuẩn thiết kế: Corporate Blue Theme & UI/UX Standards\n" +
                "• Tính năng: MenuStrip điều hướng toàn cục + Lối tắt phím nhanh (Hotkeys)\n" +
                "• Bảo mật: Kiểm soát phân quyền 2 lớp (AD-4 / Role-Based Access Control)\n" +
                "• Bản quyền: © 2026 Nhóm sinh viên thực hiện.";

            MessageBox.Show(this, msg, "Giới Thiệu Phần Mềm", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Bảng màu chuyên nghiệp cho MenuStrip chuẩn phong cách doanh nghiệp hiện đại
        /// </summary>
        private class CorporateMenuColorTable : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => Color.White;
            public override Color MenuStripGradientEnd => Color.White;
            public override Color MenuItemSelected => Color.FromArgb(239, 246, 255);       // Blue 50
            public override Color MenuItemBorder => Color.FromArgb(191, 219, 254);         // Blue 200
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(239, 246, 255);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(239, 246, 255);
            public override Color MenuItemPressedGradientBegin => Color.FromArgb(219, 234, 254);
            public override Color MenuItemPressedGradientEnd => Color.FromArgb(219, 234, 254);
            public override Color ToolStripDropDownBackground => Color.White;
            public override Color ImageMarginGradientBegin => Color.FromArgb(248, 250, 252);
            public override Color ImageMarginGradientMiddle => Color.FromArgb(248, 250, 252);
            public override Color ImageMarginGradientEnd => Color.FromArgb(248, 250, 252);
            public override Color MenuBorder => Color.FromArgb(226, 232, 240);
            public override Color SeparatorDark => Color.FromArgb(226, 232, 240);
            public override Color SeparatorLight => Color.White;
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
