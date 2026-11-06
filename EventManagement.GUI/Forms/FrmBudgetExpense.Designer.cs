namespace EventManagement.GUI.Forms
{
    partial class FrmBudgetExpense
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTopHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.pnlFilterBar = new System.Windows.Forms.Panel();
            this.lblEventFilter = new System.Windows.Forms.Label();
            this.cboFilterSuKien = new System.Windows.Forms.ComboBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.pnlBudgetCards = new System.Windows.Forms.Panel();
            this.pnlCardBudget = new System.Windows.Forms.Panel();
            this.lblCardBudgetTitle = new System.Windows.Forms.Label();
            this.lblNganSachDuKien = new System.Windows.Forms.Label();
            this.pnlCardSpent = new System.Windows.Forms.Panel();
            this.lblCardSpentTitle = new System.Windows.Forms.Label();
            this.lblTongThucChi = new System.Windows.Forms.Label();
            this.pnlCardRemaining = new System.Windows.Forms.Panel();
            this.lblCardRemainingTitle = new System.Windows.Forms.Label();
            this.lblNganSachConLai = new System.Windows.Forms.Label();
            this.pnlCardPercent = new System.Windows.Forms.Panel();
            this.lblCardPercentTitle = new System.Windows.Forms.Label();
            this.lblTyLeSuDung = new System.Windows.Forms.Label();
            this.pnlAlertBanner = new System.Windows.Forms.Panel();
            this.lblAlertIcon = new System.Windows.Forms.Label();
            this.lblAlertStatus = new System.Windows.Forms.Label();
            this.scContent = new System.Windows.Forms.SplitContainer();
            this.grpList = new System.Windows.Forms.GroupBox();
            this.dgvChiPhi = new System.Windows.Forms.DataGridView();
            this.colMaChiPhi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKhoanMucChi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayChi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNguoiLap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDienGiai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.lblMaChiPhi = new System.Windows.Forms.Label();
            this.txtMaChiPhi = new System.Windows.Forms.TextBox();
            this.lblFormSuKien = new System.Windows.Forms.Label();
            this.cboFormSuKien = new System.Windows.Forms.ComboBox();
            this.lblKhoanMuc = new System.Windows.Forms.Label();
            this.txtKhoanMucChi = new System.Windows.Forms.TextBox();
            this.lblSoTien = new System.Windows.Forms.Label();
            this.nudSoTien = new System.Windows.Forms.NumericUpDown();
            this.lblNgayChi = new System.Windows.Forms.Label();
            this.dtpNgayChi = new System.Windows.Forms.DateTimePicker();
            this.lblNguoiLap = new System.Windows.Forms.Label();
            this.txtNguoiLapPhieu = new System.Windows.Forms.TextBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblDienGiai = new System.Windows.Forms.Label();
            this.txtDienGiai = new System.Windows.Forms.TextBox();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlTopHeader.SuspendLayout();
            this.pnlFilterBar.SuspendLayout();
            this.pnlBudgetCards.SuspendLayout();
            this.pnlCardBudget.SuspendLayout();
            this.pnlCardSpent.SuspendLayout();
            this.pnlCardRemaining.SuspendLayout();
            this.pnlCardPercent.SuspendLayout();
            this.pnlAlertBanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scContent)).BeginInit();
            this.scContent.Panel1.SuspendLayout();
            this.scContent.Panel2.SuspendLayout();
            this.scContent.SuspendLayout();
            this.grpList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiPhi)).BeginInit();
            this.grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoTien)).BeginInit();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTopHeader
            // 
            this.pnlTopHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.pnlTopHeader.Controls.Add(this.lblTitle);
            this.pnlTopHeader.Controls.Add(this.lblSubTitle);
            this.pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlTopHeader.Name = "pnlTopHeader";
            this.pnlTopHeader.Size = new System.Drawing.Size(1160, 65);
            this.pnlTopHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(490, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ CHI PHÍ & NGÂN SÁCH SỰ KIỆN (FORM F17)";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.lblSubTitle.Location = new System.Drawing.Point(18, 37);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(534, 15);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Kiểm soát chi tiêu, định mức tài chính và tự động cảnh báo bội chi đa tầng theo quy tắc BR-14, BR-15";
            // 
            // pnlFilterBar
            // 
            this.pnlFilterBar.BackColor = System.Drawing.Color.White;
            this.pnlFilterBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilterBar.Controls.Add(this.lblEventFilter);
            this.pnlFilterBar.Controls.Add(this.cboFilterSuKien);
            this.pnlFilterBar.Controls.Add(this.lblSearch);
            this.pnlFilterBar.Controls.Add(this.txtSearch);
            this.pnlFilterBar.Controls.Add(this.btnLamMoi);
            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterBar.Location = new System.Drawing.Point(0, 65);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlFilterBar.Size = new System.Drawing.Size(1160, 48);
            this.pnlFilterBar.TabIndex = 1;
            // 
            // lblEventFilter
            // 
            this.lblEventFilter.AutoSize = true;
            this.lblEventFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEventFilter.Location = new System.Drawing.Point(12, 14);
            this.lblEventFilter.Name = "lblEventFilter";
            this.lblEventFilter.Size = new System.Drawing.Size(95, 17);
            this.lblEventFilter.TabIndex = 0;
            this.lblEventFilter.Text = "Chọn sự kiện:";
            // 
            // cboFilterSuKien
            // 
            this.cboFilterSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterSuKien.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboFilterSuKien.FormattingEnabled = true;
            this.cboFilterSuKien.Location = new System.Drawing.Point(113, 10);
            this.cboFilterSuKien.Name = "cboFilterSuKien";
            this.cboFilterSuKien.Size = new System.Drawing.Size(420, 25);
            this.cboFilterSuKien.TabIndex = 1;
            this.cboFilterSuKien.SelectedIndexChanged += new System.EventHandler(this.cboFilterSuKien_SelectedIndexChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(555, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(59, 15);
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Tìm kiếm:";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSearch.Location = new System.Drawing.Point(620, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Nhập khoản mục chi, người lập...";
            this.txtSearch.Size = new System.Drawing.Size(260, 24);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(890, 8);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(95, 28);
            this.btnLamMoi.TabIndex = 4;
            this.btnLamMoi.Text = "Làm Mới (F5)";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // pnlBudgetCards
            // 
            this.pnlBudgetCards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlBudgetCards.Controls.Add(this.pnlCardBudget);
            this.pnlBudgetCards.Controls.Add(this.pnlCardSpent);
            this.pnlBudgetCards.Controls.Add(this.pnlCardRemaining);
            this.pnlBudgetCards.Controls.Add(this.pnlCardPercent);
            this.pnlBudgetCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBudgetCards.Location = new System.Drawing.Point(0, 113);
            this.pnlBudgetCards.Name = "pnlBudgetCards";
            this.pnlBudgetCards.Padding = new System.Windows.Forms.Padding(12, 10, 12, 6);
            this.pnlBudgetCards.Size = new System.Drawing.Size(1160, 85);
            this.pnlBudgetCards.TabIndex = 2;
            // 
            // pnlCardBudget
            // 
            this.pnlCardBudget.BackColor = System.Drawing.Color.White;
            this.pnlCardBudget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCardBudget.Controls.Add(this.lblCardBudgetTitle);
            this.pnlCardBudget.Controls.Add(this.lblNganSachDuKien);
            this.pnlCardBudget.Location = new System.Drawing.Point(12, 10);
            this.pnlCardBudget.Name = "pnlCardBudget";
            this.pnlCardBudget.Size = new System.Drawing.Size(265, 66);
            this.pnlCardBudget.TabIndex = 0;
            // 
            // lblCardBudgetTitle
            // 
            this.lblCardBudgetTitle.AutoSize = true;
            this.lblCardBudgetTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCardBudgetTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCardBudgetTitle.Location = new System.Drawing.Point(12, 8);
            this.lblCardBudgetTitle.Name = "lblCardBudgetTitle";
            this.lblCardBudgetTitle.Size = new System.Drawing.Size(125, 15);
            this.lblCardBudgetTitle.TabIndex = 0;
            this.lblCardBudgetTitle.Text = "NGÂN SÁCH DỰ KIẾN";
            // 
            // lblNganSachDuKien
            // 
            this.lblNganSachDuKien.AutoSize = true;
            this.lblNganSachDuKien.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            this.lblNganSachDuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblNganSachDuKien.Location = new System.Drawing.Point(11, 28);
            this.lblNganSachDuKien.Name = "lblNganSachDuKien";
            this.lblNganSachDuKien.Size = new System.Drawing.Size(62, 25);
            this.lblNganSachDuKien.TabIndex = 1;
            this.lblNganSachDuKien.Text = "0 VNĐ";
            // 
            // pnlCardSpent
            // 
            this.pnlCardSpent.BackColor = System.Drawing.Color.White;
            this.pnlCardSpent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCardSpent.Controls.Add(this.lblCardSpentTitle);
            this.pnlCardSpent.Controls.Add(this.lblTongThucChi);
            this.pnlCardSpent.Location = new System.Drawing.Point(292, 10);
            this.pnlCardSpent.Name = "pnlCardSpent";
            this.pnlCardSpent.Size = new System.Drawing.Size(265, 66);
            this.pnlCardSpent.TabIndex = 1;
            // 
            // lblCardSpentTitle
            // 
            this.lblCardSpentTitle.AutoSize = true;
            this.lblCardSpentTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCardSpentTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCardSpentTitle.Location = new System.Drawing.Point(12, 8);
            this.lblCardSpentTitle.Name = "lblCardSpentTitle";
            this.lblCardSpentTitle.Size = new System.Drawing.Size(126, 15);
            this.lblCardSpentTitle.TabIndex = 0;
            this.lblCardSpentTitle.Text = "TỔNG THỰC CHI HIỆN TẠI";
            // 
            // lblTongThucChi
            // 
            this.lblTongThucChi.AutoSize = true;
            this.lblTongThucChi.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            this.lblTongThucChi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTongThucChi.Location = new System.Drawing.Point(11, 28);
            this.lblTongThucChi.Name = "lblTongThucChi";
            this.lblTongThucChi.Size = new System.Drawing.Size(62, 25);
            this.lblTongThucChi.TabIndex = 1;
            this.lblTongThucChi.Text = "0 VNĐ";
            // 
            // pnlCardRemaining
            // 
            this.pnlCardRemaining.BackColor = System.Drawing.Color.White;
            this.pnlCardRemaining.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCardRemaining.Controls.Add(this.lblCardRemainingTitle);
            this.pnlCardRemaining.Controls.Add(this.lblNganSachConLai);
            this.pnlCardRemaining.Location = new System.Drawing.Point(572, 10);
            this.pnlCardRemaining.Name = "pnlCardRemaining";
            this.pnlCardRemaining.Size = new System.Drawing.Size(265, 66);
            this.pnlCardRemaining.TabIndex = 2;
            // 
            // lblCardRemainingTitle
            // 
            this.lblCardRemainingTitle.AutoSize = true;
            this.lblCardRemainingTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCardRemainingTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCardRemainingTitle.Location = new System.Drawing.Point(12, 8);
            this.lblCardRemainingTitle.Name = "lblCardRemainingTitle";
            this.lblCardRemainingTitle.Size = new System.Drawing.Size(126, 15);
            this.lblCardRemainingTitle.TabIndex = 0;
            this.lblCardRemainingTitle.Text = "NGÂN SÁCH CÒN LẠI";
            // 
            // lblNganSachConLai
            // 
            this.lblNganSachConLai.AutoSize = true;
            this.lblNganSachConLai.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            this.lblNganSachConLai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblNganSachConLai.Location = new System.Drawing.Point(11, 28);
            this.lblNganSachConLai.Name = "lblNganSachConLai";
            this.lblNganSachConLai.Size = new System.Drawing.Size(62, 25);
            this.lblNganSachConLai.TabIndex = 1;
            this.lblNganSachConLai.Text = "0 VNĐ";
            // 
            // pnlCardPercent
            // 
            this.pnlCardPercent.BackColor = System.Drawing.Color.White;
            this.pnlCardPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCardPercent.Controls.Add(this.lblCardPercentTitle);
            this.pnlCardPercent.Controls.Add(this.lblTyLeSuDung);
            this.pnlCardPercent.Location = new System.Drawing.Point(852, 10);
            this.pnlCardPercent.Name = "pnlCardPercent";
            this.pnlCardPercent.Size = new System.Drawing.Size(295, 66);
            this.pnlCardPercent.TabIndex = 3;
            // 
            // lblCardPercentTitle
            // 
            this.lblCardPercentTitle.AutoSize = true;
            this.lblCardPercentTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCardPercentTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCardPercentTitle.Location = new System.Drawing.Point(12, 8);
            this.lblCardPercentTitle.Name = "lblCardPercentTitle";
            this.lblCardPercentTitle.Size = new System.Drawing.Size(147, 15);
            this.lblCardPercentTitle.TabIndex = 0;
            this.lblCardPercentTitle.Text = "TỶ LỆ SỬ DỤNG NGÂN SÁCH";
            // 
            // lblTyLeSuDung
            // 
            this.lblTyLeSuDung.AutoSize = true;
            this.lblTyLeSuDung.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            this.lblTyLeSuDung.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblTyLeSuDung.Location = new System.Drawing.Point(11, 28);
            this.lblTyLeSuDung.Name = "lblTyLeSuDung";
            this.lblTyLeSuDung.Size = new System.Drawing.Size(58, 25);
            this.lblTyLeSuDung.TabIndex = 1;
            this.lblTyLeSuDung.Text = "0.0 %";
            // 
            // pnlAlertBanner
            // 
            this.pnlAlertBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(252)))), ((int)(((byte)(231)))));
            this.pnlAlertBanner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAlertBanner.Controls.Add(this.lblAlertIcon);
            this.pnlAlertBanner.Controls.Add(this.lblAlertStatus);
            this.pnlAlertBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAlertBanner.Location = new System.Drawing.Point(0, 198);
            this.pnlAlertBanner.Name = "pnlAlertBanner";
            this.pnlAlertBanner.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.pnlAlertBanner.Size = new System.Drawing.Size(1160, 42);
            this.pnlAlertBanner.TabIndex = 3;
            // 
            // lblAlertIcon
            // 
            this.lblAlertIcon.AutoSize = true;
            this.lblAlertIcon.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAlertIcon.Location = new System.Drawing.Point(14, 10);
            this.lblAlertIcon.Name = "lblAlertIcon";
            this.lblAlertIcon.Size = new System.Drawing.Size(26, 20);
            this.lblAlertIcon.TabIndex = 0;
            this.lblAlertIcon.Text = "✅";
            // 
            // lblAlertStatus
            // 
            this.lblAlertStatus.AutoSize = true;
            this.lblAlertStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAlertStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(101)))), ((int)(((byte)(52)))));
            this.lblAlertStatus.Location = new System.Drawing.Point(46, 12);
            this.lblAlertStatus.Name = "lblAlertStatus";
            this.lblAlertStatus.Size = new System.Drawing.Size(378, 17);
            this.lblAlertStatus.TabIndex = 1;
            this.lblAlertStatus.Text = "Ngân sách sự kiện trong ngưỡng an toàn (Chưa chạm mốc 80%).";
            // 
            // scContent
            // 
            this.scContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scContent.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scContent.Location = new System.Drawing.Point(0, 240);
            this.scContent.Name = "scContent";
            // 
            // scContent.Panel1
            // 
            this.scContent.Panel1.Controls.Add(this.grpList);
            this.scContent.Panel1.Padding = new System.Windows.Forms.Padding(12, 8, 6, 8);
            // 
            // scContent.Panel2
            // 
            this.scContent.Panel2.Controls.Add(this.grpDetails);
            this.scContent.Panel2.Padding = new System.Windows.Forms.Padding(6, 8, 12, 8);
            this.scContent.Size = new System.Drawing.Size(1160, 480);
            this.scContent.SplitterDistance = 710;
            this.scContent.TabIndex = 4;
            // 
            // grpList
            // 
            this.grpList.Controls.Add(this.dgvChiPhi);
            this.grpList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpList.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpList.Location = new System.Drawing.Point(12, 8);
            this.grpList.Name = "grpList";
            this.grpList.Padding = new System.Windows.Forms.Padding(8);
            this.grpList.Size = new System.Drawing.Size(692, 464);
            this.grpList.TabIndex = 0;
            this.grpList.TabStop = false;
            this.grpList.Text = "DANH SÁCH CÁC PHIẾU CHI CỦA SỰ KIỆN";
            // 
            // dgvChiPhi
            // 
            this.dgvChiPhi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiPhi.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaChiPhi,
            this.colKhoanMucChi,
            this.colSoTien,
            this.colNgayChi,
            this.colNguoiLap,
            this.colTrangThai,
            this.colDienGiai});
            this.dgvChiPhi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiPhi.Location = new System.Drawing.Point(8, 25);
            this.dgvChiPhi.Name = "dgvChiPhi";
            this.dgvChiPhi.Size = new System.Drawing.Size(676, 431);
            this.dgvChiPhi.TabIndex = 0;
            this.dgvChiPhi.SelectionChanged += new System.EventHandler(this.dgvChiPhi_SelectionChanged);
            // 
            // colMaChiPhi
            // 
            this.colMaChiPhi.DataPropertyName = "MaChiPhi";
            this.colMaChiPhi.HeaderText = "Mã Phiếu";
            this.colMaChiPhi.Name = "colMaChiPhi";
            this.colMaChiPhi.Width = 90;
            // 
            // colKhoanMucChi
            // 
            this.colKhoanMucChi.DataPropertyName = "KhoanMucChi";
            this.colKhoanMucChi.HeaderText = "Khoản Mục Chi";
            this.colKhoanMucChi.Name = "colKhoanMucChi";
            this.colKhoanMucChi.Width = 190;
            // 
            // colSoTien
            // 
            this.colSoTien.DataPropertyName = "SoTien";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            this.colSoTien.DefaultCellStyle = dataGridViewCellStyle1;
            this.colSoTien.HeaderText = "Số Tiền (VNĐ)";
            this.colSoTien.Name = "colSoTien";
            this.colSoTien.Width = 120;
            // 
            // colNgayChi
            // 
            this.colNgayChi.DataPropertyName = "NgayChi";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Format = "dd/MM/yyyy";
            this.colNgayChi.DefaultCellStyle = dataGridViewCellStyle2;
            this.colNgayChi.HeaderText = "Ngày Chi";
            this.colNgayChi.Name = "colNgayChi";
            this.colNgayChi.Width = 100;
            // 
            // colNguoiLap
            // 
            this.colNguoiLap.DataPropertyName = "TenNguoiLapPhieu";
            this.colNguoiLap.HeaderText = "Người Lập";
            this.colNguoiLap.Name = "colNguoiLap";
            this.colNguoiLap.Width = 120;
            // 
            // colTrangThai
            // 
            this.colTrangThai.DataPropertyName = "TrangThai";
            this.colTrangThai.HeaderText = "Trạng Thái";
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.Width = 100;
            // 
            // colDienGiai
            // 
            this.colDienGiai.DataPropertyName = "DienGiai";
            this.colDienGiai.HeaderText = "Diễn Giải";
            this.colDienGiai.Name = "colDienGiai";
            this.colDienGiai.Width = 150;
            // 
            // grpDetails
            // 
            this.grpDetails.BackColor = System.Drawing.Color.White;
            this.grpDetails.Controls.Add(this.lblMaChiPhi);
            this.grpDetails.Controls.Add(this.txtMaChiPhi);
            this.grpDetails.Controls.Add(this.lblFormSuKien);
            this.grpDetails.Controls.Add(this.cboFormSuKien);
            this.grpDetails.Controls.Add(this.lblKhoanMuc);
            this.grpDetails.Controls.Add(this.txtKhoanMucChi);
            this.grpDetails.Controls.Add(this.lblSoTien);
            this.grpDetails.Controls.Add(this.nudSoTien);
            this.grpDetails.Controls.Add(this.lblNgayChi);
            this.grpDetails.Controls.Add(this.dtpNgayChi);
            this.grpDetails.Controls.Add(this.lblNguoiLap);
            this.grpDetails.Controls.Add(this.txtNguoiLapPhieu);
            this.grpDetails.Controls.Add(this.lblTrangThai);
            this.grpDetails.Controls.Add(this.cboTrangThai);
            this.grpDetails.Controls.Add(this.lblDienGiai);
            this.grpDetails.Controls.Add(this.txtDienGiai);
            this.grpDetails.Controls.Add(this.pnlActions);
            this.grpDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetails.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpDetails.Location = new System.Drawing.Point(6, 8);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Padding = new System.Windows.Forms.Padding(12);
            this.grpDetails.Size = new System.Drawing.Size(428, 464);
            this.grpDetails.TabIndex = 0;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "THÔNG TIN PHIẾU CHI";
            // 
            // lblMaChiPhi
            // 
            this.lblMaChiPhi.AutoSize = true;
            this.lblMaChiPhi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMaChiPhi.Location = new System.Drawing.Point(12, 28);
            this.lblMaChiPhi.Name = "lblMaChiPhi";
            this.lblMaChiPhi.Size = new System.Drawing.Size(60, 15);
            this.lblMaChiPhi.TabIndex = 0;
            this.lblMaChiPhi.Text = "Mã phiếu:";
            // 
            // txtMaChiPhi
            // 
            this.txtMaChiPhi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMaChiPhi.Location = new System.Drawing.Point(120, 25);
            this.txtMaChiPhi.Name = "txtMaChiPhi";
            this.txtMaChiPhi.ReadOnly = true;
            this.txtMaChiPhi.Size = new System.Drawing.Size(280, 23);
            this.txtMaChiPhi.TabIndex = 1;
            // 
            // lblFormSuKien
            // 
            this.lblFormSuKien.AutoSize = true;
            this.lblFormSuKien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFormSuKien.Location = new System.Drawing.Point(12, 60);
            this.lblFormSuKien.Name = "lblFormSuKien";
            this.lblFormSuKien.Size = new System.Drawing.Size(51, 15);
            this.lblFormSuKien.TabIndex = 2;
            this.lblFormSuKien.Text = "Sự kiện:";
            // 
            // cboFormSuKien
            // 
            this.cboFormSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormSuKien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboFormSuKien.FormattingEnabled = true;
            this.cboFormSuKien.Location = new System.Drawing.Point(120, 56);
            this.cboFormSuKien.Name = "cboFormSuKien";
            this.cboFormSuKien.Size = new System.Drawing.Size(280, 23);
            this.cboFormSuKien.TabIndex = 3;
            // 
            // lblKhoanMuc
            // 
            this.lblKhoanMuc.AutoSize = true;
            this.lblKhoanMuc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKhoanMuc.Location = new System.Drawing.Point(12, 92);
            this.lblKhoanMuc.Name = "lblKhoanMuc";
            this.lblKhoanMuc.Size = new System.Drawing.Size(91, 15);
            this.lblKhoanMuc.TabIndex = 4;
            this.lblKhoanMuc.Text = "Khoản mục (*):";
            // 
            // txtKhoanMucChi
            // 
            this.txtKhoanMucChi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtKhoanMucChi.Location = new System.Drawing.Point(120, 88);
            this.txtKhoanMucChi.Name = "txtKhoanMucChi";
            this.txtKhoanMucChi.PlaceholderText = "Ví dụ: Thuê âm thanh, Teabreak...";
            this.txtKhoanMucChi.Size = new System.Drawing.Size(280, 23);
            this.txtKhoanMucChi.TabIndex = 5;
            // 
            // lblSoTien
            // 
            this.lblSoTien.AutoSize = true;
            this.lblSoTien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSoTien.Location = new System.Drawing.Point(12, 124);
            this.lblSoTien.Name = "lblSoTien";
            this.lblSoTien.Size = new System.Drawing.Size(95, 15);
            this.lblSoTien.TabIndex = 6;
            this.lblSoTien.Text = "Số tiền VNĐ (*):";
            // 
            // nudSoTien
            // 
            this.nudSoTien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudSoTien.Location = new System.Drawing.Point(120, 120);
            this.nudSoTien.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nudSoTien.Name = "nudSoTien";
            this.nudSoTien.Size = new System.Drawing.Size(280, 23);
            this.nudSoTien.TabIndex = 7;
            this.nudSoTien.ThousandsSeparator = true;
            this.nudSoTien.ValueChanged += new System.EventHandler(this.nudSoTien_ValueChanged);
            // 
            // lblNgayChi
            // 
            this.lblNgayChi.AutoSize = true;
            this.lblNgayChi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNgayChi.Location = new System.Drawing.Point(12, 156);
            this.lblNgayChi.Name = "lblNgayChi";
            this.lblNgayChi.Size = new System.Drawing.Size(57, 15);
            this.lblNgayChi.TabIndex = 8;
            this.lblNgayChi.Text = "Ngày chi:";
            // 
            // dtpNgayChi
            // 
            this.dtpNgayChi.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayChi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpNgayChi.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayChi.Location = new System.Drawing.Point(120, 152);
            this.dtpNgayChi.Name = "dtpNgayChi";
            this.dtpNgayChi.Size = new System.Drawing.Size(280, 23);
            this.dtpNgayChi.TabIndex = 9;
            // 
            // lblNguoiLap
            // 
            this.lblNguoiLap.AutoSize = true;
            this.lblNguoiLap.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNguoiLap.Location = new System.Drawing.Point(12, 188);
            this.lblNguoiLap.Name = "lblNguoiLap";
            this.lblNguoiLap.Size = new System.Drawing.Size(62, 15);
            this.lblNguoiLap.TabIndex = 10;
            this.lblNguoiLap.Text = "Người lập:";
            // 
            // txtNguoiLapPhieu
            // 
            this.txtNguoiLapPhieu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNguoiLapPhieu.Location = new System.Drawing.Point(120, 184);
            this.txtNguoiLapPhieu.Name = "txtNguoiLapPhieu";
            this.txtNguoiLapPhieu.Size = new System.Drawing.Size(280, 23);
            this.txtNguoiLapPhieu.TabIndex = 11;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTrangThai.Location = new System.Drawing.Point(12, 220);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(64, 15);
            this.lblTrangThai.TabIndex = 12;
            this.lblTrangThai.Text = "Trạng thái:";
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboTrangThai.FormattingEnabled = true;
            this.cboTrangThai.Items.AddRange(new object[] {
            "Đã duyệt",
            "Chờ duyệt",
            "Đã hủy"});
            this.cboTrangThai.Location = new System.Drawing.Point(120, 216);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(280, 23);
            this.cboTrangThai.TabIndex = 13;
            // 
            // lblDienGiai
            // 
            this.lblDienGiai.AutoSize = true;
            this.lblDienGiai.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDienGiai.Location = new System.Drawing.Point(12, 252);
            this.lblDienGiai.Name = "lblDienGiai";
            this.lblDienGiai.Size = new System.Drawing.Size(56, 15);
            this.lblDienGiai.TabIndex = 14;
            this.lblDienGiai.Text = "Diễn giải:";
            // 
            // txtDienGiai
            // 
            this.txtDienGiai.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDienGiai.Location = new System.Drawing.Point(120, 248);
            this.txtDienGiai.Multiline = true;
            this.txtDienGiai.Name = "txtDienGiai";
            this.txtDienGiai.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDienGiai.Size = new System.Drawing.Size(280, 68);
            this.txtDienGiai.TabIndex = 15;
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnThem);
            this.pnlActions.Controls.Add(this.btnSua);
            this.pnlActions.Controls.Add(this.btnXoa);
            this.pnlActions.Controls.Add(this.btnLuu);
            this.pnlActions.Controls.Add(this.btnHuy);
            this.pnlActions.Controls.Add(this.btnXuatExcel);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Location = new System.Drawing.Point(12, 335);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(404, 117);
            this.pnlActions.TabIndex = 16;
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(8, 12);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 34);
            this.btnThem.TabIndex = 0;
            this.btnThem.Text = "Thêm (Ctrl+N)";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(142, 12);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(120, 34);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(276, 12);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(120, 34);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "Xóa (Ctrl+D)";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(8, 62);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(120, 34);
            this.btnLuu.TabIndex = 3;
            this.btnLuu.Text = "Lưu (Ctrl+S)";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(142, 62);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(120, 34);
            this.btnHuy.TabIndex = 4;
            this.btnHuy.Text = "Hủy (Esc)";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Location = new System.Drawing.Point(276, 62);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(120, 34);
            this.btnXuatExcel.TabIndex = 5;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // FrmBudgetExpense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 720);
            this.Controls.Add(this.scContent);
            this.Controls.Add(this.pnlAlertBanner);
            this.Controls.Add(this.pnlBudgetCards);
            this.Controls.Add(this.pnlFilterBar);
            this.Controls.Add(this.pnlTopHeader);
            this.MinimumSize = new System.Drawing.Size(1100, 680);
            this.Name = "FrmBudgetExpense";
            this.Text = "Quản Lý Chi Phí & Ngân Sách Sự Kiện (F17)";
            this.pnlTopHeader.ResumeLayout(false);
            this.pnlTopHeader.PerformLayout();
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            this.pnlBudgetCards.ResumeLayout(false);
            this.pnlCardBudget.ResumeLayout(false);
            this.pnlCardBudget.PerformLayout();
            this.pnlCardSpent.ResumeLayout(false);
            this.pnlCardSpent.PerformLayout();
            this.pnlCardRemaining.ResumeLayout(false);
            this.pnlCardRemaining.PerformLayout();
            this.pnlCardPercent.ResumeLayout(false);
            this.pnlCardPercent.PerformLayout();
            this.pnlAlertBanner.ResumeLayout(false);
            this.pnlAlertBanner.PerformLayout();
            this.scContent.Panel1.ResumeLayout(false);
            this.scContent.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scContent)).EndInit();
            this.scContent.ResumeLayout(false);
            this.grpList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiPhi)).EndInit();
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoTien)).EndInit();
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Panel pnlFilterBar;
        private System.Windows.Forms.Label lblEventFilter;
        private System.Windows.Forms.ComboBox cboFilterSuKien;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Panel pnlBudgetCards;
        private System.Windows.Forms.Panel pnlCardBudget;
        private System.Windows.Forms.Label lblCardBudgetTitle;
        private System.Windows.Forms.Label lblNganSachDuKien;
        private System.Windows.Forms.Panel pnlCardSpent;
        private System.Windows.Forms.Label lblCardSpentTitle;
        private System.Windows.Forms.Label lblTongThucChi;
        private System.Windows.Forms.Panel pnlCardRemaining;
        private System.Windows.Forms.Label lblCardRemainingTitle;
        private System.Windows.Forms.Label lblNganSachConLai;
        private System.Windows.Forms.Panel pnlCardPercent;
        private System.Windows.Forms.Label lblCardPercentTitle;
        private System.Windows.Forms.Label lblTyLeSuDung;
        private System.Windows.Forms.Panel pnlAlertBanner;
        private System.Windows.Forms.Label lblAlertIcon;
        private System.Windows.Forms.Label lblAlertStatus;
        private System.Windows.Forms.SplitContainer scContent;
        private System.Windows.Forms.GroupBox grpList;
        private System.Windows.Forms.DataGridView dgvChiPhi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaChiPhi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKhoanMucChi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayChi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNguoiLap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDienGiai;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblMaChiPhi;
        private System.Windows.Forms.TextBox txtMaChiPhi;
        private System.Windows.Forms.Label lblFormSuKien;
        private System.Windows.Forms.ComboBox cboFormSuKien;
        private System.Windows.Forms.Label lblKhoanMuc;
        private System.Windows.Forms.TextBox txtKhoanMucChi;
        private System.Windows.Forms.Label lblSoTien;
        private System.Windows.Forms.NumericUpDown nudSoTien;
        private System.Windows.Forms.Label lblNgayChi;
        private System.Windows.Forms.DateTimePicker dtpNgayChi;
        private System.Windows.Forms.Label lblNguoiLap;
        private System.Windows.Forms.TextBox txtNguoiLapPhieu;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.Label lblDienGiai;
        private System.Windows.Forms.TextBox txtDienGiai;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
