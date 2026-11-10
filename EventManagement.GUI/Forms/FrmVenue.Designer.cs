namespace EventManagement.GUI.Forms
{
    partial class FrmVenue
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Label lblLocTrangThai;
        private System.Windows.Forms.ComboBox cboLocTrangThai;
        private System.Windows.Forms.Button btnLamMoi;

        private System.Windows.Forms.DataGridView dgvDiaDiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaDiaDiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenDiaDiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiaChi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSucChuaToiDa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNguoiQuanLy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoDienThoai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoSuKienToChuc;

        private System.Windows.Forms.GroupBox grbThongTin;
        private System.Windows.Forms.Label lblMaDiaDiem;
        private System.Windows.Forms.TextBox txtMaDiaDiem;
        private System.Windows.Forms.Label lblTenDiaDiem;
        private System.Windows.Forms.TextBox txtTenDiaDiem;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblSucChuaToiDa;
        private System.Windows.Forms.NumericUpDown nudSucChuaToiDa;
        private System.Windows.Forms.Label lblNguoiQuanLy;
        private System.Windows.Forms.TextBox txtNguoiQuanLy;
        private System.Windows.Forms.Label lblSoDienThoai;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.TextBox txtGhiChu;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;

        private System.Windows.Forms.ErrorProvider errBaoLoi;

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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dgvHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvRowStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvAltRowStyle = new System.Windows.Forms.DataGridViewCellStyle();

            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();

            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.lblLocTrangThai = new System.Windows.Forms.Label();
            this.cboLocTrangThai = new System.Windows.Forms.ComboBox();
            this.btnLamMoi = new System.Windows.Forms.Button();

            this.dgvDiaDiem = new System.Windows.Forms.DataGridView();
            this.colMaDiaDiem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenDiaDiem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiaChi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSucChuaToiDa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNguoiQuanLy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoDienThoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoSuKienToChuc = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.grbThongTin = new System.Windows.Forms.GroupBox();
            this.lblMaDiaDiem = new System.Windows.Forms.Label();
            this.txtMaDiaDiem = new System.Windows.Forms.TextBox();
            this.lblTenDiaDiem = new System.Windows.Forms.Label();
            this.txtTenDiaDiem = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblSucChuaToiDa = new System.Windows.Forms.Label();
            this.nudSucChuaToiDa = new System.Windows.Forms.NumericUpDown();
            this.lblNguoiQuanLy = new System.Windows.Forms.Label();
            this.txtNguoiQuanLy = new System.Windows.Forms.TextBox();
            this.lblSoDienThoai = new System.Windows.Forms.Label();
            this.txtSoDienThoai = new System.Windows.Forms.TextBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.txtGhiChu = new System.Windows.Forms.TextBox();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();

            this.errBaoLoi = new System.Windows.Forms.ErrorProvider(this.components);

            this.pnlHeader.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiaDiem)).BeginInit();
            this.grbThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSucChuaToiDa)).BeginInit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 52);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(306, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ ĐỊA ĐIỂM TỔ CHỨC";

            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.btnLamMoi);
            this.pnlFilter.Controls.Add(this.cboLocTrangThai);
            this.pnlFilter.Controls.Add(this.lblLocTrangThai);
            this.pnlFilter.Controls.Add(this.txtTimKiem);
            this.pnlFilter.Controls.Add(this.lblTimKiem);
            this.pnlFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFilter.Location = new System.Drawing.Point(16, 64);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(968, 48);
            this.pnlFilter.TabIndex = 1;

            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTimKiem.Location = new System.Drawing.Point(12, 14);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(66, 17);
            this.lblTimKiem.TabIndex = 0;
            this.lblTimKiem.Text = "Tìm kiếm:";

            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTimKiem.Location = new System.Drawing.Point(84, 11);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Nhập tên địa điểm, địa chỉ, người quản lý, số điện thoại...";
            this.txtTimKiem.Size = new System.Drawing.Size(420, 24);
            this.txtTimKiem.TabIndex = 1;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);

            // 
            // lblLocTrangThai
            // 
            this.lblLocTrangThai.AutoSize = true;
            this.lblLocTrangThai.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocTrangThai.Location = new System.Drawing.Point(530, 14);
            this.lblLocTrangThai.Name = "lblLocTrangThai";
            this.lblLocTrangThai.Size = new System.Drawing.Size(71, 17);
            this.lblLocTrangThai.TabIndex = 2;
            this.lblLocTrangThai.Text = "Trạng thái:";

            // 
            // cboLocTrangThai
            // 
            this.cboLocTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocTrangThai.FormattingEnabled = true;
            this.cboLocTrangThai.Items.AddRange(new object[] {
            "Tất cả",
            "Hoạt động",
            "Bảo trì",
            "Ngừng sử dụng"});
            this.cboLocTrangThai.Location = new System.Drawing.Point(607, 11);
            this.cboLocTrangThai.Name = "cboLocTrangThai";
            this.cboLocTrangThai.Size = new System.Drawing.Size(180, 24);
            this.cboLocTrangThai.TabIndex = 3;
            this.cboLocTrangThai.SelectedIndexChanged += new System.EventHandler(this.cboLocTrangThai_SelectedIndexChanged);

            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLamMoi.Location = new System.Drawing.Point(820, 9);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(130, 28);
            this.btnLamMoi.TabIndex = 4;
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);

            // 
            // dgvDiaDiem
            // 
            this.dgvDiaDiem.AllowUserToAddRows = false;
            this.dgvDiaDiem.AllowUserToDeleteRows = false;
            this.dgvDiaDiem.AllowUserToResizeRows = false;
            this.dgvDiaDiem.BackgroundColor = System.Drawing.Color.White;
            this.dgvDiaDiem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dgvHeaderStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dgvHeaderStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvDiaDiem.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvDiaDiem.ColumnHeadersHeight = 34;
            this.dgvDiaDiem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dgvDiaDiem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaDiaDiem,
            this.colTenDiaDiem,
            this.colDiaChi,
            this.colSucChuaToiDa,
            this.colNguoiQuanLy,
            this.colSoDienThoai,
            this.colTrangThai,
            this.colSoSuKienToChuc});

            dgvRowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRowStyle.BackColor = System.Drawing.Color.White;
            dgvRowStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dgvRowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dgvRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dgvRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.dgvDiaDiem.DefaultCellStyle = dgvRowStyle;

            dgvAltRowStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.dgvDiaDiem.AlternatingRowsDefaultCellStyle = dgvAltRowStyle;

            this.dgvDiaDiem.EnableHeadersVisualStyles = false;
            this.dgvDiaDiem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiaDiem.Location = new System.Drawing.Point(16, 120);
            this.dgvDiaDiem.MultiSelect = false;
            this.dgvDiaDiem.Name = "dgvDiaDiem";
            this.dgvDiaDiem.ReadOnly = true;
            this.dgvDiaDiem.RowHeadersVisible = false;
            this.dgvDiaDiem.RowTemplate.Height = 30;
            this.dgvDiaDiem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDiaDiem.Size = new System.Drawing.Size(968, 230);
            this.dgvDiaDiem.TabIndex = 2;
            this.dgvDiaDiem.SelectionChanged += new System.EventHandler(this.dgvDiaDiem_SelectionChanged);

            // 
            // colMaDiaDiem
            // 
            this.colMaDiaDiem.DataPropertyName = "MaDiaDiem";
            this.colMaDiaDiem.HeaderText = "Mã ĐĐ";
            this.colMaDiaDiem.Name = "colMaDiaDiem";
            this.colMaDiaDiem.ReadOnly = true;
            this.colMaDiaDiem.Width = 90;
            this.colMaDiaDiem.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colTenDiaDiem
            // 
            this.colTenDiaDiem.DataPropertyName = "TenDiaDiem";
            this.colTenDiaDiem.HeaderText = "Tên địa điểm / Hội trường";
            this.colTenDiaDiem.Name = "colTenDiaDiem";
            this.colTenDiaDiem.ReadOnly = true;
            this.colTenDiaDiem.Width = 220;

            // 
            // colDiaChi
            // 
            this.colDiaChi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDiaChi.DataPropertyName = "DiaChi";
            this.colDiaChi.HeaderText = "Địa chỉ chi tiết";
            this.colDiaChi.Name = "colDiaChi";
            this.colDiaChi.ReadOnly = true;

            // 
            // colSucChuaToiDa
            // 
            this.colSucChuaToiDa.DataPropertyName = "SucChuaToiDa";
            this.colSucChuaToiDa.HeaderText = "Sức chứa";
            this.colSucChuaToiDa.Name = "colSucChuaToiDa";
            this.colSucChuaToiDa.ReadOnly = true;
            this.colSucChuaToiDa.Width = 100;
            this.colSucChuaToiDa.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSucChuaToiDa.DefaultCellStyle.Format = "N0";

            // 
            // colNguoiQuanLy
            // 
            this.colNguoiQuanLy.DataPropertyName = "NguoiQuanLy";
            this.colNguoiQuanLy.HeaderText = "Người quản lý";
            this.colNguoiQuanLy.Name = "colNguoiQuanLy";
            this.colNguoiQuanLy.ReadOnly = true;
            this.colNguoiQuanLy.Width = 140;

            // 
            // colSoDienThoai
            // 
            this.colSoDienThoai.DataPropertyName = "SoDienThoai";
            this.colSoDienThoai.HeaderText = "Số điện thoại";
            this.colSoDienThoai.Name = "colSoDienThoai";
            this.colSoDienThoai.ReadOnly = true;
            this.colSoDienThoai.Width = 110;
            this.colSoDienThoai.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colTrangThai
            // 
            this.colTrangThai.DataPropertyName = "TrangThai";
            this.colTrangThai.HeaderText = "Trạng thái";
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            this.colTrangThai.Width = 110;
            this.colTrangThai.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colSoSuKienToChuc
            // 
            this.colSoSuKienToChuc.DataPropertyName = "SoSuKienToChuc";
            this.colSoSuKienToChuc.HeaderText = "Sự kiện";
            this.colSoSuKienToChuc.Name = "colSoSuKienToChuc";
            this.colSoSuKienToChuc.ReadOnly = true;
            this.colSoSuKienToChuc.Width = 80;
            this.colSoSuKienToChuc.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // grbThongTin
            // 
            this.grbThongTin.BackColor = System.Drawing.Color.White;
            this.grbThongTin.Controls.Add(this.txtGhiChu);
            this.grbThongTin.Controls.Add(this.lblGhiChu);
            this.grbThongTin.Controls.Add(this.cboTrangThai);
            this.grbThongTin.Controls.Add(this.lblTrangThai);
            this.grbThongTin.Controls.Add(this.txtSoDienThoai);
            this.grbThongTin.Controls.Add(this.lblSoDienThoai);
            this.grbThongTin.Controls.Add(this.txtNguoiQuanLy);
            this.grbThongTin.Controls.Add(this.lblNguoiQuanLy);
            this.grbThongTin.Controls.Add(this.nudSucChuaToiDa);
            this.grbThongTin.Controls.Add(this.lblSucChuaToiDa);
            this.grbThongTin.Controls.Add(this.txtDiaChi);
            this.grbThongTin.Controls.Add(this.lblDiaChi);
            this.grbThongTin.Controls.Add(this.txtTenDiaDiem);
            this.grbThongTin.Controls.Add(this.lblTenDiaDiem);
            this.grbThongTin.Controls.Add(this.txtMaDiaDiem);
            this.grbThongTin.Controls.Add(this.lblMaDiaDiem);
            this.grbThongTin.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grbThongTin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grbThongTin.Location = new System.Drawing.Point(16, 360);
            this.grbThongTin.Name = "grbThongTin";
            this.grbThongTin.Size = new System.Drawing.Size(968, 175);
            this.grbThongTin.TabIndex = 3;
            this.grbThongTin.TabStop = false;
            this.grbThongTin.Text = "Thông tin chi tiết địa điểm tổ chức";

            // 
            // lblMaDiaDiem
            // 
            this.lblMaDiaDiem.AutoSize = true;
            this.lblMaDiaDiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaDiaDiem.Location = new System.Drawing.Point(20, 32);
            this.lblMaDiaDiem.Name = "lblMaDiaDiem";
            this.lblMaDiaDiem.Size = new System.Drawing.Size(107, 17);
            this.lblMaDiaDiem.TabIndex = 0;
            this.lblMaDiaDiem.Text = "Mã địa điểm (*):";

            // 
            // txtMaDiaDiem
            // 
            this.txtMaDiaDiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMaDiaDiem.Location = new System.Drawing.Point(135, 29);
            this.txtMaDiaDiem.Name = "txtMaDiaDiem";
            this.txtMaDiaDiem.Size = new System.Drawing.Size(180, 24);
            this.txtMaDiaDiem.TabIndex = 1;

            // 
            // lblTenDiaDiem
            // 
            this.lblTenDiaDiem.AutoSize = true;
            this.lblTenDiaDiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTenDiaDiem.Location = new System.Drawing.Point(340, 32);
            this.lblTenDiaDiem.Name = "lblTenDiaDiem";
            this.lblTenDiaDiem.Size = new System.Drawing.Size(110, 17);
            this.lblTenDiaDiem.TabIndex = 2;
            this.lblTenDiaDiem.Text = "Tên địa điểm (*):";

            // 
            // txtTenDiaDiem
            // 
            this.txtTenDiaDiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTenDiaDiem.Location = new System.Drawing.Point(455, 29);
            this.txtTenDiaDiem.Name = "txtTenDiaDiem";
            this.txtTenDiaDiem.Size = new System.Drawing.Size(490, 24);
            this.txtTenDiaDiem.TabIndex = 3;

            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDiaChi.Location = new System.Drawing.Point(20, 68);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(78, 17);
            this.lblDiaChi.TabIndex = 4;
            this.lblDiaChi.Text = "Địa chỉ (*):";

            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDiaChi.Location = new System.Drawing.Point(135, 65);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(480, 24);
            this.txtDiaChi.TabIndex = 5;

            // 
            // lblSucChuaToiDa
            // 
            this.lblSucChuaToiDa.AutoSize = true;
            this.lblSucChuaToiDa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSucChuaToiDa.Location = new System.Drawing.Point(645, 68);
            this.lblSucChuaToiDa.Name = "lblSucChuaToiDa";
            this.lblSucChuaToiDa.Size = new System.Drawing.Size(91, 17);
            this.lblSucChuaToiDa.TabIndex = 6;
            this.lblSucChuaToiDa.Text = "Sức chứa (*):";

            // 
            // nudSucChuaToiDa
            // 
            this.nudSucChuaToiDa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nudSucChuaToiDa.Location = new System.Drawing.Point(740, 65);
            this.nudSucChuaToiDa.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.nudSucChuaToiDa.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudSucChuaToiDa.Name = "nudSucChuaToiDa";
            this.nudSucChuaToiDa.Size = new System.Drawing.Size(205, 24);
            this.nudSucChuaToiDa.TabIndex = 7;
            this.nudSucChuaToiDa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudSucChuaToiDa.ThousandsSeparator = true;
            this.nudSucChuaToiDa.Value = new decimal(new int[] { 100, 0, 0, 0 });

            // 
            // lblNguoiQuanLy
            // 
            this.lblNguoiQuanLy.AutoSize = true;
            this.lblNguoiQuanLy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNguoiQuanLy.Location = new System.Drawing.Point(20, 104);
            this.lblNguoiQuanLy.Name = "lblNguoiQuanLy";
            this.lblNguoiQuanLy.Size = new System.Drawing.Size(94, 17);
            this.lblNguoiQuanLy.TabIndex = 8;
            this.lblNguoiQuanLy.Text = "Người quản lý:";

            // 
            // txtNguoiQuanLy
            // 
            this.txtNguoiQuanLy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNguoiQuanLy.Location = new System.Drawing.Point(135, 101);
            this.txtNguoiQuanLy.Name = "txtNguoiQuanLy";
            this.txtNguoiQuanLy.Size = new System.Drawing.Size(180, 24);
            this.txtNguoiQuanLy.TabIndex = 9;

            // 
            // lblSoDienThoai
            // 
            this.lblSoDienThoai.AutoSize = true;
            this.lblSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSoDienThoai.Location = new System.Drawing.Point(340, 104);
            this.lblSoDienThoai.Name = "lblSoDienThoai";
            this.lblSoDienThoai.Size = new System.Drawing.Size(88, 17);
            this.lblSoDienThoai.TabIndex = 10;
            this.lblSoDienThoai.Text = "Số điện thoại:";

            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSoDienThoai.Location = new System.Drawing.Point(455, 101);
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.Size = new System.Drawing.Size(160, 24);
            this.txtSoDienThoai.TabIndex = 11;

            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTrangThai.Location = new System.Drawing.Point(645, 104);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(71, 17);
            this.lblTrangThai.TabIndex = 12;
            this.lblTrangThai.Text = "Trạng thái:";

            // 
            // cboTrangThai
            // 
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboTrangThai.FormattingEnabled = true;
            this.cboTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Bảo trì",
            "Ngừng sử dụng"});
            this.cboTrangThai.Location = new System.Drawing.Point(740, 101);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(205, 24);
            this.cboTrangThai.TabIndex = 13;

            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGhiChu.Location = new System.Drawing.Point(20, 140);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(54, 17);
            this.lblGhiChu.TabIndex = 14;
            this.lblGhiChu.Text = "Ghi chú:";

            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGhiChu.Location = new System.Drawing.Point(135, 137);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(810, 24);
            this.txtGhiChu.TabIndex = 15;

            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.White;
            this.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlButtons.Controls.Add(this.btnHuy);
            this.pnlButtons.Controls.Add(this.btnLuu);
            this.pnlButtons.Controls.Add(this.btnXoa);
            this.pnlButtons.Controls.Add(this.btnSua);
            this.pnlButtons.Controls.Add(this.btnThem);
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.Location = new System.Drawing.Point(16, 545);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(968, 56);
            this.pnlButtons.TabIndex = 4;

            // 
            // btnThem
            // 
            this.btnThem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnThem.Location = new System.Drawing.Point(20, 10);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 36);
            this.btnThem.TabIndex = 0;
            this.btnThem.Text = "➕ Thêm (F2)";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            // 
            // btnSua
            // 
            this.btnSua.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSua.Location = new System.Drawing.Point(155, 10);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(120, 36);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "✏️ Sửa (F3)";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);

            // 
            // btnXoa
            // 
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnXoa.Location = new System.Drawing.Point(290, 10);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(120, 36);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "🗑️ Xóa (Del)";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            // 
            // btnLuu
            // 
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLuu.Location = new System.Drawing.Point(425, 10);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(130, 36);
            this.btnLuu.TabIndex = 3;
            this.btnLuu.Text = "💾 Lưu (Ctrl+S)";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            // 
            // btnHuy
            // 
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHuy.Location = new System.Drawing.Point(565, 10);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(130, 36);
            this.btnHuy.TabIndex = 4;
            this.btnHuy.Text = "❌ Hủy (Esc)";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // 
            // errBaoLoi
            // 
            this.errBaoLoi.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errBaoLoi.ContainerControl = this;

            // 
            // FrmVenue
            // 
            this.ClientSize = new System.Drawing.Size(1000, 615);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.grbThongTin);
            this.Controls.Add(this.dgvDiaDiem);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.Name = "FrmVenue";
            this.Text = "Quản lý địa điểm tổ chức";
            this.KeyPreview = true;
            this.Load += new System.EventHandler(this.FrmVenue_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmVenue_KeyDown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiaDiem)).EndInit();
            this.grbThongTin.ResumeLayout(false);
            this.grbThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSucChuaToiDa)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
