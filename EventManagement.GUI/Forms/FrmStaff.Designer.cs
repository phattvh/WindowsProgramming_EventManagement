namespace EventManagement.GUI.Forms
{
    partial class FrmStaff
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Label lblLocPhongBan;
        private System.Windows.Forms.ComboBox cboLocPhongBan;
        private System.Windows.Forms.Label lblLocTrangThai;
        private System.Windows.Forms.ComboBox cboLocTrangThai;
        private System.Windows.Forms.Button btnLamMoi;

        private System.Windows.Forms.DataGridView dgvNhanSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaNhanSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoDienThoai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChucDanh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhongBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoCaTruc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;

        private System.Windows.Forms.GroupBox grbThongTin;
        private System.Windows.Forms.Label lblMaNhanSu;
        private System.Windows.Forms.TextBox txtMaNhanSu;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.Label lblSoDienThoai;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblChucDanh;
        private System.Windows.Forms.ComboBox cboChucDanh;
        private System.Windows.Forms.Label lblPhongBan;
        private System.Windows.Forms.ComboBox cboPhongBan;

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
            this.lblLocPhongBan = new System.Windows.Forms.Label();
            this.cboLocPhongBan = new System.Windows.Forms.ComboBox();
            this.lblLocTrangThai = new System.Windows.Forms.Label();
            this.cboLocTrangThai = new System.Windows.Forms.ComboBox();
            this.btnLamMoi = new System.Windows.Forms.Button();

            this.dgvNhanSu = new System.Windows.Forms.DataGridView();
            this.colMaNhanSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoDienThoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChucDanh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhongBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoCaTruc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.grbThongTin = new System.Windows.Forms.GroupBox();
            this.lblMaNhanSu = new System.Windows.Forms.Label();
            this.txtMaNhanSu = new System.Windows.Forms.TextBox();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblSoDienThoai = new System.Windows.Forms.Label();
            this.txtSoDienThoai = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblChucDanh = new System.Windows.Forms.Label();
            this.cboChucDanh = new System.Windows.Forms.ComboBox();
            this.lblPhongBan = new System.Windows.Forms.Label();
            this.cboPhongBan = new System.Windows.Forms.ComboBox();

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
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanSu)).BeginInit();
            this.grbThongTin.SuspendLayout();
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
            this.pnlHeader.Size = new System.Drawing.Size(1060, 56);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(370, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "👥 QUẢN LÝ NHÂN SỰ BAN TỔ CHỨC";

            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.btnLamMoi);
            this.pnlFilter.Controls.Add(this.cboLocTrangThai);
            this.pnlFilter.Controls.Add(this.lblLocTrangThai);
            this.pnlFilter.Controls.Add(this.cboLocPhongBan);
            this.pnlFilter.Controls.Add(this.lblLocPhongBan);
            this.pnlFilter.Controls.Add(this.txtTimKiem);
            this.pnlFilter.Controls.Add(this.lblTimKiem);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 56);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1060, 50);
            this.pnlFilter.TabIndex = 1;

            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTimKiem.Location = new System.Drawing.Point(15, 15);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(79, 17);
            this.lblTimKiem.TabIndex = 0;
            this.lblTimKiem.Text = "🔍 Tìm kiếm:";

            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTimKiem.Location = new System.Drawing.Point(120, 12);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Nhập mã, họ tên, số điện thoại, chức danh...";
            this.txtTimKiem.Size = new System.Drawing.Size(225, 24);
            this.txtTimKiem.TabIndex = 1;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);

            // 
            // lblLocPhongBan
            // 
            this.lblLocPhongBan.AutoSize = true;
            this.lblLocPhongBan.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocPhongBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocPhongBan.Location = new System.Drawing.Point(365, 15);
            this.lblLocPhongBan.Name = "lblLocPhongBan";
            this.lblLocPhongBan.Size = new System.Drawing.Size(78, 17);
            this.lblLocPhongBan.TabIndex = 2;
            this.lblLocPhongBan.Text = "Phòng ban:";

            // 
            // cboLocPhongBan
            // 
            this.cboLocPhongBan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocPhongBan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocPhongBan.FormattingEnabled = true;
            this.cboLocPhongBan.Items.AddRange(new object[] {
            "Tất cả",
            "Lễ tân",
            "Kỹ thuật",
            "Hậu cần",
            "An ninh",
            "Điều phối",
            "Truyền thông"});
            this.cboLocPhongBan.Location = new System.Drawing.Point(455, 12);
            this.cboLocPhongBan.Name = "cboLocPhongBan";
            this.cboLocPhongBan.Size = new System.Drawing.Size(150, 24);
            this.cboLocPhongBan.TabIndex = 3;
            this.cboLocPhongBan.SelectedIndexChanged += new System.EventHandler(this.cboLocPhongBan_SelectedIndexChanged);

            // 
            // lblLocTrangThai
            // 
            this.lblLocTrangThai.AutoSize = true;
            this.lblLocTrangThai.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocTrangThai.Location = new System.Drawing.Point(625, 15);
            this.lblLocTrangThai.Name = "lblLocTrangThai";
            this.lblLocTrangThai.Size = new System.Drawing.Size(71, 17);
            this.lblLocTrangThai.TabIndex = 4;
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
            "Nghỉ việc"});
            this.cboLocTrangThai.Location = new System.Drawing.Point(705, 12);
            this.cboLocTrangThai.Name = "cboLocTrangThai";
            this.cboLocTrangThai.Size = new System.Drawing.Size(150, 24);
            this.cboLocTrangThai.TabIndex = 5;
            this.cboLocTrangThai.SelectedIndexChanged += new System.EventHandler(this.cboLocTrangThai_SelectedIndexChanged);

            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLamMoi.Location = new System.Drawing.Point(910, 10);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(110, 30);
            this.btnLamMoi.TabIndex = 6;
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);

            // 
            // dgvNhanSu
            // 
            this.dgvNhanSu.AllowUserToAddRows = false;
            this.dgvNhanSu.AllowUserToDeleteRows = false;
            this.dgvNhanSu.AllowUserToResizeColumns = true;
            this.dgvNhanSu.AllowUserToResizeRows = false;
            this.dgvNhanSu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvNhanSu.BackgroundColor = System.Drawing.Color.White;
            this.dgvNhanSu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvNhanSu.ScrollBars = System.Windows.Forms.ScrollBars.Both;

            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.White;
            dgvHeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            dgvHeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvHeaderStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNhanSu.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvNhanSu.ColumnHeadersHeight = 36;
            this.dgvNhanSu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dgvNhanSu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaNhanSu,
            this.colHoTen,
            this.colSoDienThoai,
            this.colEmail,
            this.colChucDanh,
            this.colPhongBan,
            this.colSoCaTruc,
            this.colTrangThai});

            dgvRowStyle.BackColor = System.Drawing.Color.White;
            dgvRowStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dgvRowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dgvRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dgvRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.dgvNhanSu.DefaultCellStyle = dgvRowStyle;

            dgvAltRowStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            dgvAltRowStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dgvAltRowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dgvAltRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dgvAltRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.dgvNhanSu.AlternatingRowsDefaultCellStyle = dgvAltRowStyle;

            this.dgvNhanSu.EnableHeadersVisualStyles = false;
            this.dgvNhanSu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNhanSu.MultiSelect = false;
            this.dgvNhanSu.Name = "dgvNhanSu";
            this.dgvNhanSu.ReadOnly = true;
            this.dgvNhanSu.RowHeadersVisible = false;
            this.dgvNhanSu.RowTemplate.Height = 28;
            this.dgvNhanSu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNhanSu.TabIndex = 2;
            this.dgvNhanSu.SelectionChanged += new System.EventHandler(this.dgvNhanSu_SelectionChanged);

            // 
            // colMaNhanSu
            // 
            this.colMaNhanSu.DataPropertyName = "MaNhanSu";
            this.colMaNhanSu.HeaderText = "Mã NS";
            this.colMaNhanSu.Name = "colMaNhanSu";
            this.colMaNhanSu.ReadOnly = true;
            this.colMaNhanSu.Width = 95;

            // 
            // colHoTen
            // 
            this.colHoTen.DataPropertyName = "HoTen";
            this.colHoTen.HeaderText = "Họ và tên nhân sự";
            this.colHoTen.Name = "colHoTen";
            this.colHoTen.ReadOnly = true;
            this.colHoTen.Width = 240;

            // 
            // colSoDienThoai
            // 
            this.colSoDienThoai.DataPropertyName = "SoDienThoai";
            this.colSoDienThoai.HeaderText = "Số điện thoại";
            this.colSoDienThoai.Name = "colSoDienThoai";
            this.colSoDienThoai.ReadOnly = true;
            this.colSoDienThoai.Width = 140;

            // 
            // colEmail
            // 
            this.colEmail.DataPropertyName = "Email";
            this.colEmail.HeaderText = "Hòm thư điện tử";
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            this.colEmail.Width = 260;

            // 
            // colChucDanh
            // 
            this.colChucDanh.DataPropertyName = "ChucDanh";
            this.colChucDanh.HeaderText = "Chức danh";
            this.colChucDanh.Name = "colChucDanh";
            this.colChucDanh.ReadOnly = true;
            this.colChucDanh.Width = 260;

            // 
            // colPhongBan
            // 
            this.colPhongBan.DataPropertyName = "PhongBan";
            this.colPhongBan.HeaderText = "Phòng ban";
            this.colPhongBan.Name = "colPhongBan";
            this.colPhongBan.ReadOnly = true;
            this.colPhongBan.Width = 160;

            // 
            // colSoCaTruc
            // 
            this.colSoCaTruc.DataPropertyName = "SoCaTrucThamGia";
            this.colSoCaTruc.HeaderText = "Số ca trực";
            this.colSoCaTruc.Name = "colSoCaTruc";
            this.colSoCaTruc.ReadOnly = true;
            this.colSoCaTruc.Width = 130;

            // 
            // colTrangThai
            // 
            this.colTrangThai.DataPropertyName = "TrangThai";
            this.colTrangThai.HeaderText = "Trạng thái";
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            this.colTrangThai.Width = 140;

            // 
            // grbThongTin
            // 
            this.grbThongTin.BackColor = System.Drawing.Color.White;
            this.grbThongTin.Controls.Add(this.cboPhongBan);
            this.grbThongTin.Controls.Add(this.lblPhongBan);
            this.grbThongTin.Controls.Add(this.cboChucDanh);
            this.grbThongTin.Controls.Add(this.lblChucDanh);
            this.grbThongTin.Controls.Add(this.txtEmail);
            this.grbThongTin.Controls.Add(this.lblEmail);
            this.grbThongTin.Controls.Add(this.txtSoDienThoai);
            this.grbThongTin.Controls.Add(this.lblSoDienThoai);
            this.grbThongTin.Controls.Add(this.cboTrangThai);
            this.grbThongTin.Controls.Add(this.lblTrangThai);
            this.grbThongTin.Controls.Add(this.txtHoTen);
            this.grbThongTin.Controls.Add(this.lblHoTen);
            this.grbThongTin.Controls.Add(this.txtMaNhanSu);
            this.grbThongTin.Controls.Add(this.lblMaNhanSu);
            this.grbThongTin.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grbThongTin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grbThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.grbThongTin.Location = new System.Drawing.Point(0, 395);
            this.grbThongTin.Name = "grbThongTin";
            this.grbThongTin.Size = new System.Drawing.Size(1060, 185);
            this.grbThongTin.TabIndex = 3;
            this.grbThongTin.TabStop = false;
            this.grbThongTin.Text = "HỒ SƠ CHI TIẾT NHÂN SỰ BAN TỔ CHỨC";

            // 
            // lblMaNhanSu
            // 
            this.lblMaNhanSu.AutoSize = true;
            this.lblMaNhanSu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaNhanSu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblMaNhanSu.Location = new System.Drawing.Point(20, 35);
            this.lblMaNhanSu.Name = "lblMaNhanSu";
            this.lblMaNhanSu.Size = new System.Drawing.Size(73, 15);
            this.lblMaNhanSu.TabIndex = 0;
            this.lblMaNhanSu.Text = "Mã nhân sự:";

            // 
            // txtMaNhanSu
            // 
            this.txtMaNhanSu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMaNhanSu.Location = new System.Drawing.Point(115, 32);
            this.txtMaNhanSu.Name = "txtMaNhanSu";
            this.txtMaNhanSu.Size = new System.Drawing.Size(120, 23);
            this.txtMaNhanSu.TabIndex = 1;

            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHoTen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblHoTen.Location = new System.Drawing.Point(265, 35);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(78, 15);
            this.lblHoTen.TabIndex = 2;
            this.lblHoTen.Text = "Họ tên (*):";

            // 
            // txtHoTen
            // 
            this.txtHoTen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtHoTen.Location = new System.Drawing.Point(355, 32);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(320, 23);
            this.txtHoTen.TabIndex = 3;

            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTrangThai.Location = new System.Drawing.Point(705, 35);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(62, 15);
            this.lblTrangThai.TabIndex = 4;
            this.lblTrangThai.Text = "Trạng thái:";

            // 
            // cboTrangThai
            // 
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboTrangThai.FormattingEnabled = true;
            this.cboTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Nghỉ việc"});
            this.cboTrangThai.Location = new System.Drawing.Point(790, 32);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(240, 23);
            this.cboTrangThai.TabIndex = 5;

            // 
            // lblSoDienThoai
            // 
            this.lblSoDienThoai.AutoSize = true;
            this.lblSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSoDienThoai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblSoDienThoai.Location = new System.Drawing.Point(20, 83);
            this.lblSoDienThoai.Name = "lblSoDienThoai";
            this.lblSoDienThoai.Size = new System.Drawing.Size(95, 15);
            this.lblSoDienThoai.TabIndex = 6;
            this.lblSoDienThoai.Text = "Số điện thoại (*):";

            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSoDienThoai.Location = new System.Drawing.Point(145, 80);
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.Size = new System.Drawing.Size(195, 23);
            this.txtSoDienThoai.TabIndex = 7;

            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblEmail.Location = new System.Drawing.Point(365, 83);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(95, 15);
            this.lblEmail.TabIndex = 8;
            this.lblEmail.Text = "Hòm thư điện tử:";

            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtEmail.Location = new System.Drawing.Point(495, 80);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(535, 23);
            this.txtEmail.TabIndex = 9;

            // 
            // lblChucDanh
            // 
            this.lblChucDanh.AutoSize = true;
            this.lblChucDanh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblChucDanh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblChucDanh.Location = new System.Drawing.Point(20, 131);
            this.lblChucDanh.Name = "lblChucDanh";
            this.lblChucDanh.Size = new System.Drawing.Size(68, 15);
            this.lblChucDanh.TabIndex = 10;
            this.lblChucDanh.Text = "Chức danh:";

            // 
            // cboChucDanh
            // 
            this.cboChucDanh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboChucDanh.FormattingEnabled = true;
            this.cboChucDanh.Items.AddRange(new object[] {
            "Trưởng ban Hậu cần",
            "Trưởng ban Lễ tân",
            "Kỹ thuật viên Âm thanh",
            "Kỹ thuật viên Ánh sáng",
            "Nhân viên Đón tiếp",
            "Nhân viên Soát vé QR",
            "Hướng dẫn khách mời",
            "Điều phối viên Sự kiện",
            "Tiếp đón Diễn giả VIP",
            "Đội trưởng An ninh",
            "Cộng tác viên",
            "Tình nguyện viên"});
            this.cboChucDanh.Location = new System.Drawing.Point(130, 128);
            this.cboChucDanh.Name = "cboChucDanh";
            this.cboChucDanh.Size = new System.Drawing.Size(320, 23);
            this.cboChucDanh.TabIndex = 11;

            // 
            // lblPhongBan
            // 
            this.lblPhongBan.AutoSize = true;
            this.lblPhongBan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPhongBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblPhongBan.Location = new System.Drawing.Point(475, 131);
            this.lblPhongBan.Name = "lblPhongBan";
            this.lblPhongBan.Size = new System.Drawing.Size(95, 15);
            this.lblPhongBan.TabIndex = 12;
            this.lblPhongBan.Text = "Phòng ban / Ban:";

            // 
            // cboPhongBan
            // 
            this.cboPhongBan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboPhongBan.FormattingEnabled = true;
            this.cboPhongBan.Items.AddRange(new object[] {
            "Lễ tân",
            "Kỹ thuật",
            "Hậu cần",
            "An ninh",
            "Điều phối",
            "Truyền thông"});
            this.cboPhongBan.Location = new System.Drawing.Point(590, 128);
            this.cboPhongBan.Name = "cboPhongBan";
            this.cboPhongBan.Size = new System.Drawing.Size(440, 23);
            this.cboPhongBan.TabIndex = 13;

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
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 580);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1060, 60);
            this.pnlButtons.TabIndex = 4;

            // 
            // btnThem
            // 
            this.btnThem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnThem.Location = new System.Drawing.Point(20, 11);
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
            this.btnSua.Location = new System.Drawing.Point(150, 11);
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
            this.btnXoa.Location = new System.Drawing.Point(280, 11);
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
            this.btnXuatExcel.Location = new System.Drawing.Point(410, 11);
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
            this.btnLuu.Location = new System.Drawing.Point(760, 11);
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
            this.btnHuy.Location = new System.Drawing.Point(900, 11);
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
            // FrmStaff
            // 
            this.ClientSize = new System.Drawing.Size(1060, 640);
            this.Controls.Add(this.dgvNhanSu);
            this.Controls.Add(this.grbThongTin);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.Name = "FrmStaff";
            this.Text = "Quản lý nhân sự ban tổ chức";
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmStaff_KeyDown);
            this.Load += new System.EventHandler(this.FrmStaff_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanSu)).EndInit();
            this.grbThongTin.ResumeLayout(false);
            this.grbThongTin.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
