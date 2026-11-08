namespace EventManagement.GUI.Forms
{
    partial class FrmSponsor
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Label lblLocGoi;
        private System.Windows.Forms.ComboBox cboLocGoiTaiTro;
        private System.Windows.Forms.Label lblLocTrangThai;
        private System.Windows.Forms.ComboBox cboLocTrangThai;
        private System.Windows.Forms.Button btnLamMoi;

        private System.Windows.Forms.DataGridView dgvNhaTaiTro;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaNhaTaiTro;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenToChuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNguoiDaiDien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoDienThoai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGoiTaiTro;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaTriTaiTro;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHinhThucTaiTro;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;

        private System.Windows.Forms.GroupBox grbThongTin;
        private System.Windows.Forms.Label lblMaNhaTaiTro;
        private System.Windows.Forms.TextBox txtMaNhaTaiTro;
        private System.Windows.Forms.Label lblTenToChuc;
        private System.Windows.Forms.TextBox txtTenToChuc;
        private System.Windows.Forms.Label lblNguoiDaiDien;
        private System.Windows.Forms.TextBox txtNguoiDaiDien;

        private System.Windows.Forms.Label lblSoDienThoai;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;

        private System.Windows.Forms.Label lblGoiTaiTro;
        private System.Windows.Forms.ComboBox cboGoiTaiTro;
        private System.Windows.Forms.Label lblGiaTriTaiTro;
        private System.Windows.Forms.NumericUpDown nudGiaTriTaiTro;
        private System.Windows.Forms.Label lblHinhThuc;
        private System.Windows.Forms.ComboBox cboHinhThuc;

        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.TextBox txtGhiChu;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnXuatExcel;
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
            this.lblLocGoi = new System.Windows.Forms.Label();
            this.cboLocGoiTaiTro = new System.Windows.Forms.ComboBox();
            this.lblLocTrangThai = new System.Windows.Forms.Label();
            this.cboLocTrangThai = new System.Windows.Forms.ComboBox();
            this.btnLamMoi = new System.Windows.Forms.Button();

            this.dgvNhaTaiTro = new System.Windows.Forms.DataGridView();
            this.colMaNhaTaiTro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenToChuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNguoiDaiDien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoDienThoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGoiTaiTro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaTriTaiTro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHinhThucTaiTro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.grbThongTin = new System.Windows.Forms.GroupBox();
            this.lblMaNhaTaiTro = new System.Windows.Forms.Label();
            this.txtMaNhaTaiTro = new System.Windows.Forms.TextBox();
            this.lblTenToChuc = new System.Windows.Forms.Label();
            this.txtTenToChuc = new System.Windows.Forms.TextBox();
            this.lblNguoiDaiDien = new System.Windows.Forms.Label();
            this.txtNguoiDaiDien = new System.Windows.Forms.TextBox();

            this.lblSoDienThoai = new System.Windows.Forms.Label();
            this.txtSoDienThoai = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();

            this.lblGoiTaiTro = new System.Windows.Forms.Label();
            this.cboGoiTaiTro = new System.Windows.Forms.ComboBox();
            this.lblGiaTriTaiTro = new System.Windows.Forms.Label();
            this.nudGiaTriTaiTro = new System.Windows.Forms.NumericUpDown();
            this.lblHinhThuc = new System.Windows.Forms.Label();
            this.cboHinhThuc = new System.Windows.Forms.ComboBox();

            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.txtGhiChu = new System.Windows.Forms.TextBox();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();

            this.errBaoLoi = new System.Windows.Forms.ErrorProvider(this.components);

            this.pnlHeader.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhaTaiTro)).BeginInit();
            this.grbThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGiaTriTaiTro)).BeginInit();
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
            this.pnlHeader.Size = new System.Drawing.Size(1060, 52);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🤝 QUẢN LÝ NHÀ TÀI TRỢ VÀ ĐỐI TÁC";

            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.btnLamMoi);
            this.pnlFilter.Controls.Add(this.cboLocTrangThai);
            this.pnlFilter.Controls.Add(this.lblLocTrangThai);
            this.pnlFilter.Controls.Add(this.cboLocGoiTaiTro);
            this.pnlFilter.Controls.Add(this.lblLocGoi);
            this.pnlFilter.Controls.Add(this.txtTimKiem);
            this.pnlFilter.Controls.Add(this.lblTimKiem);
            this.pnlFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFilter.Location = new System.Drawing.Point(20, 62);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1020, 48);
            this.pnlFilter.TabIndex = 1;

            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTimKiem.Location = new System.Drawing.Point(15, 14);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(60, 15);
            this.lblTimKiem.TabIndex = 0;
            this.lblTimKiem.Text = "Tìm kiếm:";

            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTimKiem.Location = new System.Drawing.Point(95, 11);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Mã, tên, email, sđt...";
            this.txtTimKiem.Size = new System.Drawing.Size(165, 23);
            this.txtTimKiem.TabIndex = 1;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);

            // 
            // lblLocGoi
            // 
            this.lblLocGoi.AutoSize = true;
            this.lblLocGoi.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocGoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocGoi.Location = new System.Drawing.Point(280, 14);
            this.lblLocGoi.Name = "lblLocGoi";
            this.lblLocGoi.Size = new System.Drawing.Size(64, 15);
            this.lblLocGoi.TabIndex = 2;
            this.lblLocGoi.Text = "Gói tài trợ:";

            // 
            // cboLocGoiTaiTro
            // 
            this.cboLocGoiTaiTro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocGoiTaiTro.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocGoiTaiTro.FormattingEnabled = true;
            this.cboLocGoiTaiTro.Items.AddRange(new object[] {
            "Tất cả",
            "Kim Cương",
            "Vàng",
            "Bạc",
            "Đồng",
            "Đồng hành"});
            this.cboLocGoiTaiTro.Location = new System.Drawing.Point(370, 11);
            this.cboLocGoiTaiTro.Name = "cboLocGoiTaiTro";
            this.cboLocGoiTaiTro.Size = new System.Drawing.Size(115, 23);
            this.cboLocGoiTaiTro.TabIndex = 3;
            this.cboLocGoiTaiTro.SelectedIndexChanged += new System.EventHandler(this.cboLocGoiTaiTro_SelectedIndexChanged);

            // 
            // lblLocTrangThai
            // 
            this.lblLocTrangThai.AutoSize = true;
            this.lblLocTrangThai.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocTrangThai.Location = new System.Drawing.Point(505, 14);
            this.lblLocTrangThai.Name = "lblLocTrangThai";
            this.lblLocTrangThai.Size = new System.Drawing.Size(65, 15);
            this.lblLocTrangThai.TabIndex = 4;
            this.lblLocTrangThai.Text = "Trạng thái:";

            // 
            // cboLocTrangThai
            // 
            this.cboLocTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocTrangThai.FormattingEnabled = true;
            this.cboLocTrangThai.Items.AddRange(new object[] {
            "Tất cả",
            "Đang hợp tác",
            "Tiềm năng",
            "Ngừng hợp tác"});
            this.cboLocTrangThai.Location = new System.Drawing.Point(595, 11);
            this.cboLocTrangThai.Name = "cboLocTrangThai";
            this.cboLocTrangThai.Size = new System.Drawing.Size(115, 23);
            this.cboLocTrangThai.TabIndex = 5;
            this.cboLocTrangThai.SelectedIndexChanged += new System.EventHandler(this.cboLocTrangThai_SelectedIndexChanged);

            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLamMoi.Location = new System.Drawing.Point(885, 8);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(120, 30);
            this.btnLamMoi.TabIndex = 6;
            this.btnLamMoi.Text = "🔄 Làm mới (F5)";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);

            // 
            // dgvNhaTaiTro
            // 
            this.dgvNhaTaiTro.AllowUserToAddRows = false;
            this.dgvNhaTaiTro.AllowUserToDeleteRows = false;
            this.dgvNhaTaiTro.AllowUserToResizeRows = false;
            this.dgvNhaTaiTro.BackgroundColor = System.Drawing.Color.White;
            this.dgvNhaTaiTro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvNhaTaiTro.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvNhaTaiTro.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvHeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dgvHeaderStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.dgvNhaTaiTro.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvNhaTaiTro.ColumnHeadersHeight = 34;
            this.dgvNhaTaiTro.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dgvNhaTaiTro.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaNhaTaiTro,
            this.colTenToChuc,
            this.colNguoiDaiDien,
            this.colSoDienThoai,
            this.colEmail,
            this.colGoiTaiTro,
            this.colGiaTriTaiTro,
            this.colHinhThucTaiTro,
            this.colTrangThai});

            dgvRowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRowStyle.BackColor = System.Drawing.Color.White;
            dgvRowStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dgvRowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dgvRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.dgvNhaTaiTro.DefaultCellStyle = dgvRowStyle;

            dgvAltRowStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            dgvAltRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dgvAltRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.dgvNhaTaiTro.AlternatingRowsDefaultCellStyle = dgvAltRowStyle;

            this.dgvNhaTaiTro.EnableHeadersVisualStyles = false;
            this.dgvNhaTaiTro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvNhaTaiTro.Location = new System.Drawing.Point(20, 118);
            this.dgvNhaTaiTro.MultiSelect = false;
            this.dgvNhaTaiTro.Name = "dgvNhaTaiTro";
            this.dgvNhaTaiTro.ReadOnly = true;
            this.dgvNhaTaiTro.RowHeadersVisible = false;
            this.dgvNhaTaiTro.RowTemplate.Height = 30;
            this.dgvNhaTaiTro.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNhaTaiTro.Size = new System.Drawing.Size(1020, 220);
            this.dgvNhaTaiTro.TabIndex = 2;
            this.dgvNhaTaiTro.SelectionChanged += new System.EventHandler(this.dgvNhaTaiTro_SelectionChanged);

            // 
            // colMaNhaTaiTro
            // 
            this.colMaNhaTaiTro.DataPropertyName = "MaNhaTaiTro";
            this.colMaNhaTaiTro.HeaderText = "Mã NTT";
            this.colMaNhaTaiTro.Name = "colMaNhaTaiTro";
            this.colMaNhaTaiTro.ReadOnly = true;
            this.colMaNhaTaiTro.Width = 90;
            this.colMaNhaTaiTro.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colTenToChuc
            // 
            this.colTenToChuc.DataPropertyName = "TenToChuc";
            this.colTenToChuc.HeaderText = "Tên tổ chức / Doanh nghiệp";
            this.colTenToChuc.Name = "colTenToChuc";
            this.colTenToChuc.ReadOnly = true;
            this.colTenToChuc.Width = 240;

            // 
            // colNguoiDaiDien
            // 
            this.colNguoiDaiDien.DataPropertyName = "NguoiDaiDien";
            this.colNguoiDaiDien.HeaderText = "Người đại diện";
            this.colNguoiDaiDien.Name = "colNguoiDaiDien";
            this.colNguoiDaiDien.ReadOnly = true;
            this.colNguoiDaiDien.Width = 140;

            // 
            // colSoDienThoai
            // 
            this.colSoDienThoai.DataPropertyName = "SoDienThoai";
            this.colSoDienThoai.HeaderText = "Điện thoại";
            this.colSoDienThoai.Name = "colSoDienThoai";
            this.colSoDienThoai.ReadOnly = true;
            this.colSoDienThoai.Width = 105;
            this.colSoDienThoai.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colEmail
            // 
            this.colEmail.DataPropertyName = "Email";
            this.colEmail.HeaderText = "Email liên hệ";
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            this.colEmail.Width = 150;

            // 
            // colGoiTaiTro
            // 
            this.colGoiTaiTro.DataPropertyName = "GoiTaiTro";
            this.colGoiTaiTro.HeaderText = "Gói tài trợ";
            this.colGoiTaiTro.Name = "colGoiTaiTro";
            this.colGoiTaiTro.ReadOnly = true;
            this.colGoiTaiTro.Width = 100;
            this.colGoiTaiTro.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colGiaTriTaiTro
            // 
            this.colGiaTriTaiTro.DataPropertyName = "GiaTriFormatted";
            this.colGiaTriTaiTro.HeaderText = "Tổng giá trị";
            this.colGiaTriTaiTro.Name = "colGiaTriTaiTro";
            this.colGiaTriTaiTro.ReadOnly = true;
            this.colGiaTriTaiTro.Width = 120;
            this.colGiaTriTaiTro.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // 
            // colHinhThucTaiTro
            // 
            this.colHinhThucTaiTro.DataPropertyName = "HinhThucTaiTro";
            this.colHinhThucTaiTro.HeaderText = "Hình thức";
            this.colHinhThucTaiTro.Name = "colHinhThucTaiTro";
            this.colHinhThucTaiTro.ReadOnly = true;
            this.colHinhThucTaiTro.Width = 100;
            this.colHinhThucTaiTro.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

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
            // grbThongTin
            // 
            this.grbThongTin.BackColor = System.Drawing.Color.White;
            this.grbThongTin.Controls.Add(this.txtGhiChu);
            this.grbThongTin.Controls.Add(this.lblGhiChu);
            this.grbThongTin.Controls.Add(this.txtDiaChi);
            this.grbThongTin.Controls.Add(this.lblDiaChi);
            this.grbThongTin.Controls.Add(this.cboHinhThuc);
            this.grbThongTin.Controls.Add(this.lblHinhThuc);
            this.grbThongTin.Controls.Add(this.nudGiaTriTaiTro);
            this.grbThongTin.Controls.Add(this.lblGiaTriTaiTro);
            this.grbThongTin.Controls.Add(this.cboGoiTaiTro);
            this.grbThongTin.Controls.Add(this.lblGoiTaiTro);
            this.grbThongTin.Controls.Add(this.cboTrangThai);
            this.grbThongTin.Controls.Add(this.lblTrangThai);
            this.grbThongTin.Controls.Add(this.txtEmail);
            this.grbThongTin.Controls.Add(this.lblEmail);
            this.grbThongTin.Controls.Add(this.txtSoDienThoai);
            this.grbThongTin.Controls.Add(this.lblSoDienThoai);
            this.grbThongTin.Controls.Add(this.txtNguoiDaiDien);
            this.grbThongTin.Controls.Add(this.lblNguoiDaiDien);
            this.grbThongTin.Controls.Add(this.txtTenToChuc);
            this.grbThongTin.Controls.Add(this.lblTenToChuc);
            this.grbThongTin.Controls.Add(this.txtMaNhaTaiTro);
            this.grbThongTin.Controls.Add(this.lblMaNhaTaiTro);
            this.grbThongTin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grbThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.grbThongTin.Location = new System.Drawing.Point(20, 348);
            this.grbThongTin.Name = "grbThongTin";
            this.grbThongTin.Size = new System.Drawing.Size(1020, 205);
            this.grbThongTin.TabIndex = 3;
            this.grbThongTin.TabStop = false;
            this.grbThongTin.Text = "THÔNG TIN CHI TIẾT NHÀ TÀI TRỢ";

            // 
            // lblMaNhaTaiTro
            // 
            this.lblMaNhaTaiTro.AutoSize = true;
            this.lblMaNhaTaiTro.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaNhaTaiTro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblMaNhaTaiTro.Location = new System.Drawing.Point(20, 32);
            this.lblMaNhaTaiTro.Name = "lblMaNhaTaiTro";
            this.lblMaNhaTaiTro.Size = new System.Drawing.Size(99, 15);
            this.lblMaNhaTaiTro.TabIndex = 0;
            this.lblMaNhaTaiTro.Text = "Mã nhà tài trợ (*):";

            // 
            // txtMaNhaTaiTro
            // 
            this.txtMaNhaTaiTro.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMaNhaTaiTro.Location = new System.Drawing.Point(145, 29);
            this.txtMaNhaTaiTro.Name = "txtMaNhaTaiTro";
            this.txtMaNhaTaiTro.Size = new System.Drawing.Size(110, 23);
            this.txtMaNhaTaiTro.TabIndex = 1;

            // 
            // lblTenToChuc
            // 
            this.lblTenToChuc.AutoSize = true;
            this.lblTenToChuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTenToChuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTenToChuc.Location = new System.Drawing.Point(280, 32);
            this.lblTenToChuc.Name = "lblTenToChuc";
            this.lblTenToChuc.Size = new System.Drawing.Size(89, 15);
            this.lblTenToChuc.TabIndex = 2;
            this.lblTenToChuc.Text = "Tên tổ chức (*):";

            // 
            // txtTenToChuc
            // 
            this.txtTenToChuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTenToChuc.Location = new System.Drawing.Point(390, 29);
            this.txtTenToChuc.Name = "txtTenToChuc";
            this.txtTenToChuc.Size = new System.Drawing.Size(300, 23);
            this.txtTenToChuc.TabIndex = 3;

            // 
            // lblNguoiDaiDien
            // 
            this.lblNguoiDaiDien.AutoSize = true;
            this.lblNguoiDaiDien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNguoiDaiDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblNguoiDaiDien.Location = new System.Drawing.Point(715, 32);
            this.lblNguoiDaiDien.Name = "lblNguoiDaiDien";
            this.lblNguoiDaiDien.Size = new System.Drawing.Size(88, 15);
            this.lblNguoiDaiDien.TabIndex = 4;
            this.lblNguoiDaiDien.Text = "Người đại diện:";

            // 
            // txtNguoiDaiDien
            // 
            this.txtNguoiDaiDien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNguoiDaiDien.Location = new System.Drawing.Point(820, 29);
            this.txtNguoiDaiDien.Name = "txtNguoiDaiDien";
            this.txtNguoiDaiDien.Size = new System.Drawing.Size(180, 23);
            this.txtNguoiDaiDien.TabIndex = 5;

            // 
            // lblSoDienThoai
            // 
            this.lblSoDienThoai.AutoSize = true;
            this.lblSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSoDienThoai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblSoDienThoai.Location = new System.Drawing.Point(20, 70);
            this.lblSoDienThoai.Name = "lblSoDienThoai";
            this.lblSoDienThoai.Size = new System.Drawing.Size(79, 15);
            this.lblSoDienThoai.TabIndex = 6;
            this.lblSoDienThoai.Text = "Số điện thoại:";

            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSoDienThoai.Location = new System.Drawing.Point(145, 67);
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.Size = new System.Drawing.Size(110, 23);
            this.txtSoDienThoai.TabIndex = 7;

            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblEmail.Location = new System.Drawing.Point(280, 70);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(78, 15);
            this.lblEmail.TabIndex = 8;
            this.lblEmail.Text = "Email liên hệ:";

            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtEmail.Location = new System.Drawing.Point(390, 67);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(300, 23);
            this.txtEmail.TabIndex = 9;

            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTrangThai.Location = new System.Drawing.Point(715, 70);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(65, 15);
            this.lblTrangThai.TabIndex = 10;
            this.lblTrangThai.Text = "Trạng thái:";

            // 
            // cboTrangThai
            // 
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboTrangThai.FormattingEnabled = true;
            this.cboTrangThai.Items.AddRange(new object[] {
            "Đang hợp tác",
            "Tiềm năng",
            "Ngừng hợp tác"});
            this.cboTrangThai.Location = new System.Drawing.Point(820, 67);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(180, 23);
            this.cboTrangThai.TabIndex = 11;

            // 
            // lblGoiTaiTro
            // 
            this.lblGoiTaiTro.AutoSize = true;
            this.lblGoiTaiTro.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGoiTaiTro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblGoiTaiTro.Location = new System.Drawing.Point(20, 108);
            this.lblGoiTaiTro.Name = "lblGoiTaiTro";
            this.lblGoiTaiTro.Size = new System.Drawing.Size(64, 15);
            this.lblGoiTaiTro.TabIndex = 12;
            this.lblGoiTaiTro.Text = "Gói tài trợ:";

            // 
            // cboGoiTaiTro
            // 
            this.cboGoiTaiTro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGoiTaiTro.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboGoiTaiTro.FormattingEnabled = true;
            this.cboGoiTaiTro.Items.AddRange(new object[] {
            "Kim Cương",
            "Vàng",
            "Bạc",
            "Đồng",
            "Đồng hành"});
            this.cboGoiTaiTro.Location = new System.Drawing.Point(145, 105);
            this.cboGoiTaiTro.Name = "cboGoiTaiTro";
            this.cboGoiTaiTro.Size = new System.Drawing.Size(110, 23);
            this.cboGoiTaiTro.TabIndex = 13;

            // 
            // lblGiaTriTaiTro
            // 
            this.lblGiaTriTaiTro.AutoSize = true;
            this.lblGiaTriTaiTro.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGiaTriTaiTro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblGiaTriTaiTro.Location = new System.Drawing.Point(280, 108);
            this.lblGiaTriTaiTro.Name = "lblGiaTriTaiTro";
            this.lblGiaTriTaiTro.Size = new System.Drawing.Size(76, 15);
            this.lblGiaTriTaiTro.TabIndex = 14;
            this.lblGiaTriTaiTro.Text = "Giá trị (VNĐ):";

            // 
            // nudGiaTriTaiTro
            // 
            this.nudGiaTriTaiTro.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nudGiaTriTaiTro.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudGiaTriTaiTro.Location = new System.Drawing.Point(390, 105);
            this.nudGiaTriTaiTro.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.nudGiaTriTaiTro.Name = "nudGiaTriTaiTro";
            this.nudGiaTriTaiTro.Size = new System.Drawing.Size(170, 23);
            this.nudGiaTriTaiTro.TabIndex = 15;
            this.nudGiaTriTaiTro.ThousandsSeparator = true;

            // 
            // lblHinhThuc
            // 
            this.lblHinhThuc.AutoSize = true;
            this.lblHinhThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHinhThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblHinhThuc.Location = new System.Drawing.Point(590, 108);
            this.lblHinhThuc.Name = "lblHinhThuc";
            this.lblHinhThuc.Size = new System.Drawing.Size(62, 15);
            this.lblHinhThuc.TabIndex = 16;
            this.lblHinhThuc.Text = "Hình thức:";

            // 
            // cboHinhThuc
            // 
            this.cboHinhThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHinhThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboHinhThuc.FormattingEnabled = true;
            this.cboHinhThuc.Items.AddRange(new object[] {
            "Tiền mặt",
            "Hiện vật",
            "Dịch vụ"});
            this.cboHinhThuc.Location = new System.Drawing.Point(670, 105);
            this.cboHinhThuc.Name = "cboHinhThuc";
            this.cboHinhThuc.Size = new System.Drawing.Size(130, 23);
            this.cboHinhThuc.TabIndex = 17;

            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDiaChi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblDiaChi.Location = new System.Drawing.Point(20, 146);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(46, 15);
            this.lblDiaChi.TabIndex = 18;
            this.lblDiaChi.Text = "Địa chỉ:";

            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDiaChi.Location = new System.Drawing.Point(145, 143);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(415, 23);
            this.txtDiaChi.TabIndex = 19;

            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblGhiChu.Location = new System.Drawing.Point(590, 146);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(51, 15);
            this.lblGhiChu.TabIndex = 20;
            this.lblGhiChu.Text = "Ghi chú:";

            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGhiChu.Location = new System.Drawing.Point(670, 143);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(330, 23);
            this.txtGhiChu.TabIndex = 21;

            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.White;
            this.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlButtons.Controls.Add(this.btnHuy);
            this.pnlButtons.Controls.Add(this.btnLuu);
            this.pnlButtons.Controls.Add(this.btnXuatExcel);
            this.pnlButtons.Controls.Add(this.btnXoa);
            this.pnlButtons.Controls.Add(this.btnSua);
            this.pnlButtons.Controls.Add(this.btnThem);
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.Location = new System.Drawing.Point(20, 565);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1020, 58);
            this.pnlButtons.TabIndex = 4;

            // 
            // btnThem
            // 
            this.btnThem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnThem.Location = new System.Drawing.Point(15, 10);
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
            this.btnSua.Location = new System.Drawing.Point(145, 10);
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
            this.btnXoa.Location = new System.Drawing.Point(275, 10);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(120, 36);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "🗑️ Xóa (Del)";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnXuatExcel.Location = new System.Drawing.Point(405, 10);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(135, 36);
            this.btnXuatExcel.TabIndex = 3;
            this.btnXuatExcel.Text = "📊 Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);

            // 
            // btnLuu
            // 
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLuu.Location = new System.Drawing.Point(730, 10);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(130, 36);
            this.btnLuu.TabIndex = 4;
            this.btnLuu.Text = "💾 Lưu (Ctrl+S)";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            // 
            // btnHuy
            // 
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHuy.Location = new System.Drawing.Point(870, 10);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(130, 36);
            this.btnHuy.TabIndex = 5;
            this.btnHuy.Text = "❌ Hủy (Esc)";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // 
            // errBaoLoi
            // 
            this.errBaoLoi.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errBaoLoi.ContainerControl = this;

            // 
            // FrmSponsor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 640);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.grbThongTin);
            this.Controls.Add(this.dgvNhaTaiTro);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.KeyPreview = true;
            this.MaximizeBox = true;
            this.Name = "FrmSponsor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý nhà tài trợ và đối tác";
            this.Load += new System.EventHandler(this.FrmSponsor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSponsor_KeyDown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhaTaiTro)).EndInit();
            this.grbThongTin.ResumeLayout(false);
            this.grbThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGiaTriTaiTro)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
