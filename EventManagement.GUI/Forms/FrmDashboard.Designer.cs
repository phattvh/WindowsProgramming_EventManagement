namespace EventManagement.GUI.Forms
{
    partial class FrmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.pnlUserInfo = new System.Windows.Forms.Panel();
            this.lblUserGreeting = new System.Windows.Forms.Label();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.tblKpiCards = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCard1 = new System.Windows.Forms.Panel();
            this.lblCard1Title = new System.Windows.Forms.Label();
            this.lblCard1Value = new System.Windows.Forms.Label();
            this.lblCard1Sub = new System.Windows.Forms.Label();
            this.pnlCard2 = new System.Windows.Forms.Panel();
            this.lblCard2Title = new System.Windows.Forms.Label();
            this.lblCard2Value = new System.Windows.Forms.Label();
            this.lblCard2Sub = new System.Windows.Forms.Label();
            this.pnlCard3 = new System.Windows.Forms.Panel();
            this.lblCard3Title = new System.Windows.Forms.Label();
            this.lblCard3Value = new System.Windows.Forms.Label();
            this.lblCard3Sub = new System.Windows.Forms.Label();
            this.pnlCard4 = new System.Windows.Forms.Panel();
            this.lblCard4Title = new System.Windows.Forms.Label();
            this.lblCard4Value = new System.Windows.Forms.Label();
            this.lblCard4Sub = new System.Windows.Forms.Label();
            this.pnlQuickAccess = new System.Windows.Forms.Panel();
            this.lblQuickAccess = new System.Windows.Forms.Label();
            this.flpQuickButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNavEvents = new System.Windows.Forms.Button();
            this.btnNavTickets = new System.Windows.Forms.Button();
            this.btnNavCheckIn = new System.Windows.Forms.Button();
            this.btnNavTicketTiers = new System.Windows.Forms.Button();
            this.btnNavExpenses = new System.Windows.Forms.Button();
            this.btnNavReports = new System.Windows.Forms.Button();
            this.btnNavUsers = new System.Windows.Forms.Button();
            this.btnNavRoles = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlChartsContainer = new System.Windows.Forms.Panel();
            this.tblCharts = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRevenueChart = new System.Windows.Forms.Panel();
            this.lblRevenueChartTitle = new System.Windows.Forms.Label();
            this.chartRevenue = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            this.pnlCheckInChart = new System.Windows.Forms.Panel();
            this.lblCheckInChartTitle = new System.Windows.Forms.Label();
            this.chartCheckIn = new LiveChartsCore.SkiaSharpView.WinForms.PieChart();
            this.grpUpcoming = new System.Windows.Forms.GroupBox();
            this.dgvUpcoming = new System.Windows.Forms.DataGridView();
            this.colMaSK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayBatDau = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayKetThuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiaDiem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckIn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTyLe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlUpcomingFooter = new System.Windows.Forms.Panel();
            this.lblUpcomingSummary = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlUserInfo.SuspendLayout();
            this.tblKpiCards.SuspendLayout();
            this.pnlCard1.SuspendLayout();
            this.pnlCard2.SuspendLayout();
            this.pnlCard3.SuspendLayout();
            this.pnlCard4.SuspendLayout();
            this.pnlQuickAccess.SuspendLayout();
            this.flpQuickButtons.SuspendLayout();
            this.pnlChartsContainer.SuspendLayout();
            this.tblCharts.SuspendLayout();
            this.pnlRevenueChart.SuspendLayout();
            this.pnlCheckInChart.SuspendLayout();
            this.grpUpcoming.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpcoming)).BeginInit();
            this.pnlUpcomingFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(34, 49, 63);
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);
            this.pnlHeader.Controls.Add(this.pnlUserInfo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1134, 65);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(15, 14);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(200, 37);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "TRANG CHỦ";
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.Silver;
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(17, 39);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(0, 20);
            this.lblHeaderSubtitle.TabIndex = 1;
            this.lblHeaderSubtitle.Text = "";
            this.lblHeaderSubtitle.Visible = false;
            // 
            // pnlUserInfo
            // 
            this.pnlUserInfo.Controls.Add(this.lblUserGreeting);
            this.pnlUserInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlUserInfo.Location = new System.Drawing.Point(634, 0);
            this.pnlUserInfo.Name = "pnlUserInfo";
            this.pnlUserInfo.Size = new System.Drawing.Size(500, 65);
            this.pnlUserInfo.TabIndex = 2;
            // 
            // lblUserGreeting
            // 
            this.lblUserGreeting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUserGreeting.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUserGreeting.ForeColor = System.Drawing.Color.White;
            this.lblUserGreeting.Location = new System.Drawing.Point(0, 0);
            this.lblUserGreeting.Name = "lblUserGreeting";
            this.lblUserGreeting.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.lblUserGreeting.Size = new System.Drawing.Size(500, 65);
            this.lblUserGreeting.TabIndex = 0;
            this.lblUserGreeting.Text = "Xin chào: Quản trị viên (Admin)";
            this.lblUserGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnChangePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangePassword.FlatAppearance.BorderSize = 0;
            this.btnChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePassword.ForeColor = System.Drawing.Color.White;
            this.btnChangePassword.Location = new System.Drawing.Point(295, 18);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(95, 30);
            this.btnChangePassword.TabIndex = 1;
            this.btnChangePassword.Text = "Đổi MK";
            this.btnChangePassword.UseVisualStyleBackColor = false;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(398, 18);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(90, 30);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // tblKpiCards
            // 
            this.tblKpiCards.ColumnCount = 4;
            this.tblKpiCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpiCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpiCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpiCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpiCards.Controls.Add(this.pnlCard1, 0, 0);
            this.tblKpiCards.Controls.Add(this.pnlCard2, 1, 0);
            this.tblKpiCards.Controls.Add(this.pnlCard3, 2, 0);
            this.tblKpiCards.Controls.Add(this.pnlCard4, 3, 0);
            this.tblKpiCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblKpiCards.Location = new System.Drawing.Point(0, 65);
            this.tblKpiCards.Name = "tblKpiCards";
            this.tblKpiCards.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.tblKpiCards.RowCount = 1;
            this.tblKpiCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKpiCards.Size = new System.Drawing.Size(1134, 115);
            this.tblKpiCards.TabIndex = 1;
            // 
            // pnlCard1
            // 
            this.pnlCard1.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.pnlCard1.Controls.Add(this.lblCard1Title);
            this.pnlCard1.Controls.Add(this.lblCard1Value);
            this.pnlCard1.Controls.Add(this.lblCard1Sub);
            this.pnlCard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard1.Location = new System.Drawing.Point(15, 13);
            this.pnlCard1.Name = "pnlCard1";
            this.pnlCard1.Padding = new System.Windows.Forms.Padding(10);
            this.pnlCard1.Size = new System.Drawing.Size(271, 89);
            this.pnlCard1.TabIndex = 0;
            // 
            // lblCard1Title
            // 
            this.lblCard1Title.AutoSize = true;
            this.lblCard1Title.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCard1Title.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblCard1Title.Location = new System.Drawing.Point(8, 8);
            this.lblCard1Title.Name = "lblCard1Title";
            this.lblCard1Title.Size = new System.Drawing.Size(130, 21);
            this.lblCard1Title.TabIndex = 0;
            this.lblCard1Title.Text = "📅 TỔNG SỰ KIỆN";
            // 
            // lblCard1Value
            // 
            this.lblCard1Value.AutoSize = true;
            this.lblCard1Value.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCard1Value.ForeColor = System.Drawing.Color.White;
            this.lblCard1Value.Location = new System.Drawing.Point(6, 26);
            this.lblCard1Value.Name = "lblCard1Value";
            this.lblCard1Value.Size = new System.Drawing.Size(39, 38);
            this.lblCard1Value.TabIndex = 1;
            this.lblCard1Value.Text = "0";
            // 
            // lblCard1Sub
            // 
            this.lblCard1Sub.AutoSize = true;
            this.lblCard1Sub.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCard1Sub.ForeColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.lblCard1Sub.Location = new System.Drawing.Point(8, 66);
            this.lblCard1Sub.Name = "lblCard1Sub";
            this.lblCard1Sub.Size = new System.Drawing.Size(161, 19);
            this.lblCard1Sub.TabIndex = 2;
            this.lblCard1Sub.Text = "Chuẩn bị: 0 | Sắp diễn: 0";
            // 
            // pnlCard2
            // 
            this.pnlCard2.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.pnlCard2.Controls.Add(this.lblCard2Title);
            this.pnlCard2.Controls.Add(this.lblCard2Value);
            this.pnlCard2.Controls.Add(this.lblCard2Sub);
            this.pnlCard2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard2.Location = new System.Drawing.Point(292, 13);
            this.pnlCard2.Name = "pnlCard2";
            this.pnlCard2.Padding = new System.Windows.Forms.Padding(10);
            this.pnlCard2.Size = new System.Drawing.Size(271, 89);
            this.pnlCard2.TabIndex = 1;
            // 
            // lblCard2Title
            // 
            this.lblCard2Title.AutoSize = true;
            this.lblCard2Title.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCard2Title.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblCard2Title.Location = new System.Drawing.Point(8, 8);
            this.lblCard2Title.Name = "lblCard2Title";
            this.lblCard2Title.Size = new System.Drawing.Size(155, 21);
            this.lblCard2Title.TabIndex = 0;
            this.lblCard2Title.Text = "🎟️ TỔNG VÉ ĐÃ BÁN";
            // 
            // lblCard2Value
            // 
            this.lblCard2Value.AutoSize = true;
            this.lblCard2Value.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCard2Value.ForeColor = System.Drawing.Color.White;
            this.lblCard2Value.Location = new System.Drawing.Point(6, 26);
            this.lblCard2Value.Name = "lblCard2Value";
            this.lblCard2Value.Size = new System.Drawing.Size(39, 38);
            this.lblCard2Value.TabIndex = 1;
            this.lblCard2Value.Text = "0";
            // 
            // lblCard2Sub
            // 
            this.lblCard2Sub.AutoSize = true;
            this.lblCard2Sub.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCard2Sub.ForeColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.lblCard2Sub.Location = new System.Drawing.Point(8, 66);
            this.lblCard2Sub.Name = "lblCard2Sub";
            this.lblCard2Sub.Size = new System.Drawing.Size(126, 19);
            this.lblCard2Sub.TabIndex = 2;
            this.lblCard2Sub.Text = "Người tham gia: 0";
            // 
            // pnlCard3
            // 
            this.pnlCard3.BackColor = System.Drawing.Color.FromArgb(211, 84, 0);
            this.pnlCard3.Controls.Add(this.lblCard3Title);
            this.pnlCard3.Controls.Add(this.lblCard3Value);
            this.pnlCard3.Controls.Add(this.lblCard3Sub);
            this.pnlCard3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard3.Location = new System.Drawing.Point(569, 13);
            this.pnlCard3.Name = "pnlCard3";
            this.pnlCard3.Padding = new System.Windows.Forms.Padding(10);
            this.pnlCard3.Size = new System.Drawing.Size(271, 89);
            this.pnlCard3.TabIndex = 2;
            // 
            // lblCard3Title
            // 
            this.lblCard3Title.AutoSize = true;
            this.lblCard3Title.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCard3Title.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblCard3Title.Location = new System.Drawing.Point(8, 8);
            this.lblCard3Title.Name = "lblCard3Title";
            this.lblCard3Title.Size = new System.Drawing.Size(160, 21);
            this.lblCard3Title.TabIndex = 0;
            this.lblCard3Title.Text = "💰 TỔNG DOANH THU";
            // 
            // lblCard3Value
            // 
            this.lblCard3Value.AutoSize = true;
            this.lblCard3Value.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCard3Value.ForeColor = System.Drawing.Color.White;
            this.lblCard3Value.Location = new System.Drawing.Point(6, 26);
            this.lblCard3Value.Name = "lblCard3Value";
            this.lblCard3Value.Size = new System.Drawing.Size(63, 38);
            this.lblCard3Value.TabIndex = 1;
            this.lblCard3Value.Text = "0 đ";
            // 
            // lblCard3Sub
            // 
            this.lblCard3Sub.AutoSize = true;
            this.lblCard3Sub.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCard3Sub.ForeColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.lblCard3Sub.Location = new System.Drawing.Point(8, 66);
            this.lblCard3Sub.Name = "lblCard3Sub";
            this.lblCard3Sub.Size = new System.Drawing.Size(127, 19);
            this.lblCard3Sub.TabIndex = 2;
            this.lblCard3Sub.Text = "Chi phí thực tế: 0 đ";
            // 
            // pnlCard4
            // 
            this.pnlCard4.BackColor = System.Drawing.Color.FromArgb(142, 68, 173);
            this.pnlCard4.Controls.Add(this.lblCard4Title);
            this.pnlCard4.Controls.Add(this.lblCard4Value);
            this.pnlCard4.Controls.Add(this.lblCard4Sub);
            this.pnlCard4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard4.Location = new System.Drawing.Point(846, 13);
            this.pnlCard4.Name = "pnlCard4";
            this.pnlCard4.Padding = new System.Windows.Forms.Padding(10);
            this.pnlCard4.Size = new System.Drawing.Size(273, 89);
            this.pnlCard4.TabIndex = 3;
            // 
            // lblCard4Title
            // 
            this.lblCard4Title.AutoSize = true;
            this.lblCard4Title.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCard4Title.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblCard4Title.Location = new System.Drawing.Point(8, 8);
            this.lblCard4Title.Name = "lblCard4Title";
            this.lblCard4Title.Size = new System.Drawing.Size(147, 21);
            this.lblCard4Title.TabIndex = 0;
            this.lblCard4Title.Text = "🎯 TỶ LỆ CHECK-IN";
            // 
            // lblCard4Value
            // 
            this.lblCard4Value.AutoSize = true;
            this.lblCard4Value.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCard4Value.ForeColor = System.Drawing.Color.White;
            this.lblCard4Value.Location = new System.Drawing.Point(6, 26);
            this.lblCard4Value.Name = "lblCard4Value";
            this.lblCard4Value.Size = new System.Drawing.Size(107, 38);
            this.lblCard4Value.TabIndex = 1;
            this.lblCard4Value.Text = "0.0 %";
            // 
            // lblCard4Sub
            // 
            this.lblCard4Sub.AutoSize = true;
            this.lblCard4Sub.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCard4Sub.ForeColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.lblCard4Sub.Location = new System.Drawing.Point(8, 66);
            this.lblCard4Sub.Name = "lblCard4Sub";
            this.lblCard4Sub.Size = new System.Drawing.Size(126, 19);
            this.lblCard4Sub.TabIndex = 2;
            this.lblCard4Sub.Text = "Đã điểm danh: 0 vé";
            // 
            // pnlQuickAccess
            // 
            this.pnlQuickAccess.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlQuickAccess.Controls.Add(this.flpQuickButtons);
            this.pnlQuickAccess.Controls.Add(this.lblQuickAccess);
            this.pnlQuickAccess.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuickAccess.Location = new System.Drawing.Point(0, 180);
            this.pnlQuickAccess.Name = "pnlQuickAccess";
            this.pnlQuickAccess.Padding = new System.Windows.Forms.Padding(15, 6, 15, 6);
            this.pnlQuickAccess.Size = new System.Drawing.Size(1134, 48);
            this.pnlQuickAccess.TabIndex = 2;
            // 
            // lblQuickAccess
            // 
            this.lblQuickAccess.AutoSize = true;
            this.lblQuickAccess.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblQuickAccess.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblQuickAccess.Location = new System.Drawing.Point(12, 13);
            this.lblQuickAccess.Name = "lblQuickAccess";
            this.lblQuickAccess.Size = new System.Drawing.Size(95, 21);
            this.lblQuickAccess.TabIndex = 0;
            this.lblQuickAccess.Text = "Điều hướng:";
            // 
            // flpQuickButtons
            // 
            this.flpQuickButtons.Controls.Add(this.btnNavEvents);
            this.flpQuickButtons.Controls.Add(this.btnNavTickets);
            this.flpQuickButtons.Controls.Add(this.btnNavCheckIn);
            this.flpQuickButtons.Controls.Add(this.btnNavTicketTiers);
            this.flpQuickButtons.Controls.Add(this.btnNavExpenses);
            this.flpQuickButtons.Controls.Add(this.btnNavReports);
            this.flpQuickButtons.Controls.Add(this.btnNavUsers);
            this.flpQuickButtons.Controls.Add(this.btnNavRoles);
            this.flpQuickButtons.Controls.Add(this.btnRefresh);
            this.flpQuickButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpQuickButtons.Location = new System.Drawing.Point(115, 6);
            this.flpQuickButtons.Name = "flpQuickButtons";
            this.flpQuickButtons.Size = new System.Drawing.Size(1004, 36);
            this.flpQuickButtons.TabIndex = 1;
            // 
            // btnNavEvents
            // 
            this.btnNavEvents.Location = new System.Drawing.Point(3, 3);
            this.btnNavEvents.Name = "btnNavEvents";
            this.btnNavEvents.Size = new System.Drawing.Size(95, 30);
            this.btnNavEvents.TabIndex = 0;
            this.btnNavEvents.Tag = "UC04,UC08";
            this.btnNavEvents.Text = "📅 Sự kiện";
            this.btnNavEvents.UseVisualStyleBackColor = true;
            this.btnNavEvents.Click += new System.EventHandler(this.btnNavEvents_Click);
            // 
            // btnNavTickets
            // 
            this.btnNavTickets.Location = new System.Drawing.Point(104, 3);
            this.btnNavTickets.Name = "btnNavTickets";
            this.btnNavTickets.Size = new System.Drawing.Size(95, 30);
            this.btnNavTickets.TabIndex = 1;
            this.btnNavTickets.Tag = "UC11";
            this.btnNavTickets.Text = "🎟️ Bán vé";
            this.btnNavTickets.UseVisualStyleBackColor = true;
            this.btnNavTickets.Click += new System.EventHandler(this.btnNavTickets_Click);
            // 
            // btnNavCheckIn
            // 
            this.btnNavCheckIn.Location = new System.Drawing.Point(205, 3);
            this.btnNavCheckIn.Name = "btnNavCheckIn";
            this.btnNavCheckIn.Size = new System.Drawing.Size(105, 30);
            this.btnNavCheckIn.TabIndex = 2;
            this.btnNavCheckIn.Tag = "UC13";
            this.btnNavCheckIn.Text = "📷 Soát vé QR";
            this.btnNavCheckIn.UseVisualStyleBackColor = true;
            this.btnNavCheckIn.Click += new System.EventHandler(this.btnNavCheckIn_Click);
            // 
            // btnNavTicketTiers
            // 
            this.btnNavTicketTiers.Location = new System.Drawing.Point(316, 3);
            this.btnNavTicketTiers.Name = "btnNavTicketTiers";
            this.btnNavTicketTiers.Size = new System.Drawing.Size(95, 30);
            this.btnNavTicketTiers.TabIndex = 3;
            this.btnNavTicketTiers.Tag = "UC12";
            this.btnNavTicketTiers.Text = "🏷️ Hạng vé";
            this.btnNavTicketTiers.UseVisualStyleBackColor = true;
            this.btnNavTicketTiers.Click += new System.EventHandler(this.btnNavTicketTiers_Click);
            // 
            // btnNavExpenses
            // 
            this.btnNavExpenses.Location = new System.Drawing.Point(417, 3);
            this.btnNavExpenses.Name = "btnNavExpenses";
            this.btnNavExpenses.Size = new System.Drawing.Size(95, 30);
            this.btnNavExpenses.TabIndex = 4;
            this.btnNavExpenses.Tag = "UC17";
            this.btnNavExpenses.Text = "💵 Chi phí";
            this.btnNavExpenses.UseVisualStyleBackColor = true;
            this.btnNavExpenses.Click += new System.EventHandler(this.btnNavExpenses_Click);
            // 
            // btnNavReports
            // 
            this.btnNavReports.Location = new System.Drawing.Point(518, 3);
            this.btnNavReports.Name = "btnNavReports";
            this.btnNavReports.Size = new System.Drawing.Size(95, 30);
            this.btnNavReports.TabIndex = 5;
            this.btnNavReports.Tag = "UC19";
            this.btnNavReports.Text = "📊 Báo cáo";
            this.btnNavReports.UseVisualStyleBackColor = true;
            this.btnNavReports.Click += new System.EventHandler(this.btnNavReports_Click);
            // 
            // btnNavUsers
            // 
            this.btnNavUsers.Location = new System.Drawing.Point(619, 3);
            this.btnNavUsers.Name = "btnNavUsers";
            this.btnNavUsers.Size = new System.Drawing.Size(105, 30);
            this.btnNavUsers.TabIndex = 6;
            this.btnNavUsers.Tag = "UC03";
            this.btnNavUsers.Text = "👥 Người dùng";
            this.btnNavUsers.UseVisualStyleBackColor = true;
            this.btnNavUsers.Click += new System.EventHandler(this.btnNavUsers_Click);
            // 
            // btnNavRoles
            // 
            this.btnNavRoles.Location = new System.Drawing.Point(730, 3);
            this.btnNavRoles.Name = "btnNavRoles";
            this.btnNavRoles.Size = new System.Drawing.Size(105, 30);
            this.btnNavRoles.TabIndex = 7;
            this.btnNavRoles.Tag = "UC20";
            this.btnNavRoles.Text = "🛡️ Phân quyền";
            this.btnNavRoles.UseVisualStyleBackColor = true;
            this.btnNavRoles.Click += new System.EventHandler(this.btnNavRoles_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(841, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(110, 30);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "🔄 Làm mới (F5)";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.Location = new System.Drawing.Point(815, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 30);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "🔄 Làm mới (F5)";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnlChartsContainer
            // 
            this.pnlChartsContainer.Controls.Add(this.tblCharts);
            this.pnlChartsContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChartsContainer.Location = new System.Drawing.Point(0, 228);
            this.pnlChartsContainer.Name = "pnlChartsContainer";
            this.pnlChartsContainer.Size = new System.Drawing.Size(1134, 230);
            this.pnlChartsContainer.TabIndex = 3;
            // 
            // tblCharts
            // 
            this.tblCharts.ColumnCount = 2;
            this.tblCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tblCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tblCharts.Controls.Add(this.pnlRevenueChart, 0, 0);
            this.tblCharts.Controls.Add(this.pnlCheckInChart, 1, 0);
            this.tblCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblCharts.Location = new System.Drawing.Point(0, 0);
            this.tblCharts.Name = "tblCharts";
            this.tblCharts.Padding = new System.Windows.Forms.Padding(12, 5, 12, 5);
            this.tblCharts.RowCount = 1;
            this.tblCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblCharts.Size = new System.Drawing.Size(1134, 230);
            this.tblCharts.TabIndex = 0;
            // 
            // pnlRevenueChart
            // 
            this.pnlRevenueChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRevenueChart.Controls.Add(this.chartRevenue);
            this.pnlRevenueChart.Controls.Add(this.lblRevenueChartTitle);
            this.pnlRevenueChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRevenueChart.Location = new System.Drawing.Point(15, 8);
            this.pnlRevenueChart.Name = "pnlRevenueChart";
            this.pnlRevenueChart.Size = new System.Drawing.Size(682, 214);
            this.pnlRevenueChart.TabIndex = 0;
            // 
            // lblRevenueChartTitle
            // 
            this.lblRevenueChartTitle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.lblRevenueChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRevenueChartTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRevenueChartTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblRevenueChartTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRevenueChartTitle.Name = "lblRevenueChartTitle";
            this.lblRevenueChartTitle.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblRevenueChartTitle.Size = new System.Drawing.Size(680, 26);
            this.lblRevenueChartTitle.TabIndex = 0;
            this.lblRevenueChartTitle.Text = "📊 SO SÁNH DOANH THU & CHI PHÍ THEO SỰ KIỆN (VNĐ)";
            this.lblRevenueChartTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chartRevenue
            // 
            this.chartRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartRevenue.Location = new System.Drawing.Point(0, 26);
            this.chartRevenue.Name = "chartRevenue";
            this.chartRevenue.Size = new System.Drawing.Size(680, 186);
            this.chartRevenue.TabIndex = 1;
            // 
            // pnlCheckInChart
            // 
            this.pnlCheckInChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCheckInChart.Controls.Add(this.chartCheckIn);
            this.pnlCheckInChart.Controls.Add(this.lblCheckInChartTitle);
            this.pnlCheckInChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCheckInChart.Location = new System.Drawing.Point(703, 8);
            this.pnlCheckInChart.Name = "pnlCheckInChart";
            this.pnlCheckInChart.Size = new System.Drawing.Size(416, 214);
            this.pnlCheckInChart.TabIndex = 1;
            // 
            // lblCheckInChartTitle
            // 
            this.lblCheckInChartTitle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.lblCheckInChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCheckInChartTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCheckInChartTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblCheckInChartTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCheckInChartTitle.Name = "lblCheckInChartTitle";
            this.lblCheckInChartTitle.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblCheckInChartTitle.Size = new System.Drawing.Size(414, 26);
            this.lblCheckInChartTitle.TabIndex = 0;
            this.lblCheckInChartTitle.Text = "🎯 TỶ LỆ CHECK-IN THỰC TẾ (%)";
            this.lblCheckInChartTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chartCheckIn
            // 
            this.chartCheckIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCheckIn.Location = new System.Drawing.Point(0, 26);
            this.chartCheckIn.Name = "chartCheckIn";
            this.chartCheckIn.Size = new System.Drawing.Size(414, 186);
            this.chartCheckIn.TabIndex = 1;
            // 
            // grpUpcoming
            // 
            this.grpUpcoming.Controls.Add(this.dgvUpcoming);
            this.grpUpcoming.Controls.Add(this.pnlUpcomingFooter);
            this.grpUpcoming.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpUpcoming.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpUpcoming.Location = new System.Drawing.Point(0, 458);
            this.grpUpcoming.Name = "grpUpcoming";
            this.grpUpcoming.Padding = new System.Windows.Forms.Padding(10);
            this.grpUpcoming.Size = new System.Drawing.Size(1134, 252);
            this.grpUpcoming.TabIndex = 4;
            this.grpUpcoming.TabStop = false;
            this.grpUpcoming.Text = "📅 CÁC SỰ KIỆN SẮP DIỄN RA TRONG 7 NGÀY TỚI";
            // 
            // dgvUpcoming
            // 
            this.dgvUpcoming.AllowUserToAddRows = false;
            this.dgvUpcoming.AllowUserToDeleteRows = false;
            this.dgvUpcoming.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUpcoming.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUpcoming.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUpcoming.ColumnHeadersHeight = 30;
            this.dgvUpcoming.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaSK,
            this.colTenSK,
            this.colNgayBatDau,
            this.colNgayKetThuc,
            this.colDiaDiem,
            this.colSoVe,
            this.colCheckIn,
            this.colTyLe,
            this.colTrangThai});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUpcoming.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUpcoming.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUpcoming.Location = new System.Drawing.Point(10, 30);
            this.dgvUpcoming.MultiSelect = false;
            this.dgvUpcoming.Name = "dgvUpcoming";
            this.dgvUpcoming.ReadOnly = true;
            this.dgvUpcoming.RowHeadersVisible = false;
            this.dgvUpcoming.RowHeadersWidth = 51;
            this.dgvUpcoming.RowTemplate.Height = 29;
            this.dgvUpcoming.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUpcoming.Size = new System.Drawing.Size(1114, 182);
            this.dgvUpcoming.TabIndex = 0;
            // 
            // colMaSK
            // 
            this.colMaSK.DataPropertyName = "MaSuKien";
            this.colMaSK.FillWeight = 55F;
            this.colMaSK.HeaderText = "Mã SK";
            this.colMaSK.MinimumWidth = 6;
            this.colMaSK.Name = "colMaSK";
            this.colMaSK.ReadOnly = true;
            // 
            // colTenSK
            // 
            this.colTenSK.DataPropertyName = "TenSuKien";
            this.colTenSK.FillWeight = 140F;
            this.colTenSK.HeaderText = "Tên Sự Kiện";
            this.colTenSK.MinimumWidth = 6;
            this.colTenSK.Name = "colTenSK";
            this.colTenSK.ReadOnly = true;
            // 
            // colNgayBatDau
            // 
            this.colNgayBatDau.DataPropertyName = "NgayBatDau";
            this.colNgayBatDau.FillWeight = 85F;
            this.colNgayBatDau.HeaderText = "Bắt Đầu";
            this.colNgayBatDau.MinimumWidth = 6;
            this.colNgayBatDau.Name = "colNgayBatDau";
            this.colNgayBatDau.ReadOnly = true;
            // 
            // colNgayKetThuc
            // 
            this.colNgayKetThuc.DataPropertyName = "NgayKetThuc";
            this.colNgayKetThuc.FillWeight = 85F;
            this.colNgayKetThuc.HeaderText = "Kết Thúc";
            this.colNgayKetThuc.MinimumWidth = 6;
            this.colNgayKetThuc.Name = "colNgayKetThuc";
            this.colNgayKetThuc.ReadOnly = true;
            // 
            // colDiaDiem
            // 
            this.colDiaDiem.DataPropertyName = "DiaDiem";
            this.colDiaDiem.FillWeight = 90F;
            this.colDiaDiem.HeaderText = "Địa Điểm";
            this.colDiaDiem.MinimumWidth = 6;
            this.colDiaDiem.Name = "colDiaDiem";
            this.colDiaDiem.ReadOnly = true;
            // 
            // colSoVe
            // 
            this.colSoVe.DataPropertyName = "SoVeDaBan";
            this.colSoVe.FillWeight = 60F;
            this.colSoVe.HeaderText = "Vé Bán";
            this.colSoVe.MinimumWidth = 6;
            this.colSoVe.Name = "colSoVe";
            this.colSoVe.ReadOnly = true;
            // 
            // colCheckIn
            // 
            this.colCheckIn.DataPropertyName = "SoNguoiCheckIn";
            this.colCheckIn.FillWeight = 60F;
            this.colCheckIn.HeaderText = "Check-in";
            this.colCheckIn.MinimumWidth = 6;
            this.colCheckIn.Name = "colCheckIn";
            this.colCheckIn.ReadOnly = true;
            // 
            // colTyLe
            // 
            this.colTyLe.DataPropertyName = "TyLeCheckIn";
            this.colTyLe.FillWeight = 60F;
            this.colTyLe.HeaderText = "Tỷ Lệ (%)";
            this.colTyLe.MinimumWidth = 6;
            this.colTyLe.Name = "colTyLe";
            this.colTyLe.ReadOnly = true;
            // 
            // colTrangThai
            // 
            this.colTrangThai.DataPropertyName = "TrangThai";
            this.colTrangThai.FillWeight = 75F;
            this.colTrangThai.HeaderText = "Trạng Thái";
            this.colTrangThai.MinimumWidth = 6;
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            // 
            // pnlUpcomingFooter
            // 
            this.pnlUpcomingFooter.Controls.Add(this.lblUpcomingSummary);
            this.pnlUpcomingFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUpcomingFooter.Location = new System.Drawing.Point(10, 212);
            this.pnlUpcomingFooter.Name = "pnlUpcomingFooter";
            this.pnlUpcomingFooter.Size = new System.Drawing.Size(1114, 30);
            this.pnlUpcomingFooter.TabIndex = 1;
            // 
            // lblUpcomingSummary
            // 
            this.lblUpcomingSummary.AutoSize = true;
            this.lblUpcomingSummary.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblUpcomingSummary.ForeColor = System.Drawing.Color.DimGray;
            this.lblUpcomingSummary.Location = new System.Drawing.Point(3, 6);
            this.lblUpcomingSummary.Name = "lblUpcomingSummary";
            this.lblUpcomingSummary.Size = new System.Drawing.Size(271, 20);
            this.lblUpcomingSummary.TabIndex = 0;
            this.lblUpcomingSummary.Text = "Hiển thị các sự kiện bắt đầu trong 7 ngày tới.";
            // 
            // FrmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1134, 710);
            this.Controls.Add(this.grpUpcoming);
            this.Controls.Add(this.pnlChartsContainer);
            this.Controls.Add(this.pnlQuickAccess);
            this.Controls.Add(this.tblKpiCards);
            this.Controls.Add(this.pnlHeader);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(950, 600);
            this.Name = "FrmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.Load += new System.EventHandler(this.FrmDashboard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmDashboard_KeyDown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlUserInfo.ResumeLayout(false);
            this.tblKpiCards.ResumeLayout(false);
            this.pnlCard1.ResumeLayout(false);
            this.pnlCard1.PerformLayout();
            this.pnlCard2.ResumeLayout(false);
            this.pnlCard2.PerformLayout();
            this.pnlCard3.ResumeLayout(false);
            this.pnlCard3.PerformLayout();
            this.pnlCard4.ResumeLayout(false);
            this.pnlCard4.PerformLayout();
            this.pnlQuickAccess.ResumeLayout(false);
            this.pnlQuickAccess.PerformLayout();
            this.flpQuickButtons.ResumeLayout(false);
            this.pnlChartsContainer.ResumeLayout(false);
            this.tblCharts.ResumeLayout(false);
            this.pnlRevenueChart.ResumeLayout(false);
            this.pnlCheckInChart.ResumeLayout(false);
            this.grpUpcoming.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpcoming)).EndInit();
            this.pnlUpcomingFooter.ResumeLayout(false);
            this.pnlUpcomingFooter.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Panel pnlUserInfo;
        private System.Windows.Forms.Label lblUserGreeting;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Button btnLogout;

        private System.Windows.Forms.TableLayoutPanel tblKpiCards;
        private System.Windows.Forms.Panel pnlCard1;
        private System.Windows.Forms.Label lblCard1Title;
        private System.Windows.Forms.Label lblCard1Value;
        private System.Windows.Forms.Label lblCard1Sub;

        private System.Windows.Forms.Panel pnlCard2;
        private System.Windows.Forms.Label lblCard2Title;
        private System.Windows.Forms.Label lblCard2Value;
        private System.Windows.Forms.Label lblCard2Sub;

        private System.Windows.Forms.Panel pnlCard3;
        private System.Windows.Forms.Label lblCard3Title;
        private System.Windows.Forms.Label lblCard3Value;
        private System.Windows.Forms.Label lblCard3Sub;

        private System.Windows.Forms.Panel pnlCard4;
        private System.Windows.Forms.Label lblCard4Title;
        private System.Windows.Forms.Label lblCard4Value;
        private System.Windows.Forms.Label lblCard4Sub;

        private System.Windows.Forms.Panel pnlQuickAccess;
        private System.Windows.Forms.Label lblQuickAccess;
        public System.Windows.Forms.FlowLayoutPanel flpQuickButtons;
        public System.Windows.Forms.Button btnNavEvents;
        public System.Windows.Forms.Button btnNavTickets;
        public System.Windows.Forms.Button btnNavCheckIn;
        public System.Windows.Forms.Button btnNavTicketTiers;
        public System.Windows.Forms.Button btnNavExpenses;
        public System.Windows.Forms.Button btnNavReports;
        public System.Windows.Forms.Button btnNavUsers;
        public System.Windows.Forms.Button btnNavRoles;
        public System.Windows.Forms.Button btnRefresh;

        protected System.Windows.Forms.Panel pnlChartsContainer;
        private System.Windows.Forms.TableLayoutPanel tblCharts;
        private System.Windows.Forms.Panel pnlRevenueChart;
        private System.Windows.Forms.Label lblRevenueChartTitle;
        public LiveChartsCore.SkiaSharpView.WinForms.CartesianChart chartRevenue;
        private System.Windows.Forms.Panel pnlCheckInChart;
        private System.Windows.Forms.Label lblCheckInChartTitle;
        public LiveChartsCore.SkiaSharpView.WinForms.PieChart chartCheckIn;

        private System.Windows.Forms.GroupBox grpUpcoming;
        private System.Windows.Forms.DataGridView dgvUpcoming;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaSK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayBatDau;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayKetThuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiaDiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTyLe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.Panel pnlUpcomingFooter;
        private System.Windows.Forms.Label lblUpcomingSummary;
    }
}
