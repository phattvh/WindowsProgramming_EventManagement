namespace EventManagement.GUI.Forms
{
    partial class FrmStaffAssignment
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblLocSuKien;
        private System.Windows.Forms.ComboBox cboLocSuKien;
        private System.Windows.Forms.Label lblLocPhongBan;
        private System.Windows.Forms.ComboBox cboLocPhongBan;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnLamMoi;

        private System.Windows.Forms.DataGridView dgvPhanCong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaPhanCong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSuKien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenNhanSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhongBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChucVuNhanSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoDienThoai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNhiemVu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayTruc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKhungGio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGhiChu;

        private System.Windows.Forms.GroupBox grbThongTin;
        private System.Windows.Forms.Label lblMaPhanCong;
        private System.Windows.Forms.TextBox txtMaPhanCong;
        private System.Windows.Forms.Label lblSuKien;
        private System.Windows.Forms.ComboBox cboSuKien;
        private System.Windows.Forms.Label lblNhanSu;
        private System.Windows.Forms.ComboBox cboNhanSu;
        private System.Windows.Forms.Label lblNhiemVu;
        private System.Windows.Forms.TextBox txtNhiemVu;
        private System.Windows.Forms.Label lblBatDau;
        private System.Windows.Forms.DateTimePicker dtpBatDau;
        private System.Windows.Forms.Label lblKetThuc;
        private System.Windows.Forms.DateTimePicker dtpKetThuc;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Button btnKiemTraXungDot;
        private System.Windows.Forms.Label lblKiemTraStatus;

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
            this.lblLocSuKien = new System.Windows.Forms.Label();
            this.cboLocSuKien = new System.Windows.Forms.ComboBox();
            this.lblLocPhongBan = new System.Windows.Forms.Label();
            this.cboLocPhongBan = new System.Windows.Forms.ComboBox();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnLamMoi = new System.Windows.Forms.Button();

            this.dgvPhanCong = new System.Windows.Forms.DataGridView();
            this.colMaPhanCong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSuKien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenNhanSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhongBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChucVuNhanSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoDienThoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhiemVu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayTruc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKhungGio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.grbThongTin = new System.Windows.Forms.GroupBox();
            this.lblMaPhanCong = new System.Windows.Forms.Label();
            this.txtMaPhanCong = new System.Windows.Forms.TextBox();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.cboSuKien = new System.Windows.Forms.ComboBox();
            this.lblNhanSu = new System.Windows.Forms.Label();
            this.cboNhanSu = new System.Windows.Forms.ComboBox();
            this.lblNhiemVu = new System.Windows.Forms.Label();
            this.txtNhiemVu = new System.Windows.Forms.TextBox();
            this.lblBatDau = new System.Windows.Forms.Label();
            this.dtpBatDau = new System.Windows.Forms.DateTimePicker();
            this.lblKetThuc = new System.Windows.Forms.Label();
            this.dtpKetThuc = new System.Windows.Forms.DateTimePicker();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.btnKiemTraXungDot = new System.Windows.Forms.Button();
            this.lblKiemTraStatus = new System.Windows.Forms.Label();

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
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhanCong)).BeginInit();
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
            this.pnlHeader.Size = new System.Drawing.Size(1084, 52);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(460, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📋 PHÂN CÔNG NHÂN SỰ THEO CA TRỰC";

            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.Controls.Add(this.btnLamMoi);
            this.pnlFilter.Controls.Add(this.txtTimKiem);
            this.pnlFilter.Controls.Add(this.lblTimKiem);
            this.pnlFilter.Controls.Add(this.cboLocPhongBan);
            this.pnlFilter.Controls.Add(this.lblLocPhongBan);
            this.pnlFilter.Controls.Add(this.cboLocSuKien);
            this.pnlFilter.Controls.Add(this.lblLocSuKien);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 52);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1084, 48);
            this.pnlFilter.TabIndex = 1;

            // 
            // lblLocSuKien
            // 
            this.lblLocSuKien.AutoSize = true;
            this.lblLocSuKien.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocSuKien.Location = new System.Drawing.Point(15, 15);
            this.lblLocSuKien.Name = "lblLocSuKien";
            this.lblLocSuKien.Size = new System.Drawing.Size(51, 15);
            this.lblLocSuKien.TabIndex = 0;
            this.lblLocSuKien.Text = "Sự kiện:";

            // 
            // cboLocSuKien
            // 
            this.cboLocSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocSuKien.FormattingEnabled = true;
            this.cboLocSuKien.Location = new System.Drawing.Point(85, 11);
            this.cboLocSuKien.Name = "cboLocSuKien";
            this.cboLocSuKien.Size = new System.Drawing.Size(215, 23);
            this.cboLocSuKien.TabIndex = 1;
            this.cboLocSuKien.SelectedIndexChanged += new System.EventHandler(this.cboLocSuKien_SelectedIndexChanged);

            // 
            // lblLocPhongBan
            // 
            this.lblLocPhongBan.AutoSize = true;
            this.lblLocPhongBan.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocPhongBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocPhongBan.Location = new System.Drawing.Point(320, 15);
            this.lblLocPhongBan.Name = "lblLocPhongBan";
            this.lblLocPhongBan.Size = new System.Drawing.Size(69, 15);
            this.lblLocPhongBan.TabIndex = 2;
            this.lblLocPhongBan.Text = "Phòng ban:";

            // 
            // cboLocPhongBan
            // 
            this.cboLocPhongBan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocPhongBan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocPhongBan.FormattingEnabled = true;
            this.cboLocPhongBan.Location = new System.Drawing.Point(415, 11);
            this.cboLocPhongBan.Name = "cboLocPhongBan";
            this.cboLocPhongBan.Size = new System.Drawing.Size(145, 23);
            this.cboLocPhongBan.TabIndex = 3;
            this.cboLocPhongBan.SelectedIndexChanged += new System.EventHandler(this.cboLocPhongBan_SelectedIndexChanged);

            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTimKiem.Location = new System.Drawing.Point(580, 15);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(59, 15);
            this.lblTimKiem.TabIndex = 4;
            this.lblTimKiem.Text = "Tìm kiếm:";

            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTimKiem.Location = new System.Drawing.Point(660, 11);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Tìm theo tên nhân sự, mã ca, nhiệm vụ, số ĐT...";
            this.txtTimKiem.Size = new System.Drawing.Size(270, 23);
            this.txtTimKiem.TabIndex = 5;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);

            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLamMoi.Location = new System.Drawing.Point(945, 8);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(120, 30);
            this.btnLamMoi.TabIndex = 6;
            this.btnLamMoi.Text = "🔄 Làm mới (F5)";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);

            // 
            // dgvPhanCong
            // 
            this.dgvPhanCong.AllowUserToAddRows = false;
            this.dgvPhanCong.AllowUserToDeleteRows = false;
            this.dgvPhanCong.AllowUserToResizeRows = false;
            this.dgvPhanCong.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPhanCong.BackgroundColor = System.Drawing.Color.White;
            this.dgvPhanCong.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPhanCong.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPhanCong.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvHeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dgvHeaderStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvHeaderStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPhanCong.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvPhanCong.ColumnHeadersHeight = 36;
            this.dgvPhanCong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPhanCong.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaPhanCong,
            this.colTenSuKien,
            this.colTenNhanSu,
            this.colPhongBan,
            this.colChucVuNhanSu,
            this.colSoDienThoai,
            this.colNhiemVu,
            this.colNgayTruc,
            this.colKhungGio,
            this.colGhiChu});
            dgvRowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRowStyle.BackColor = System.Drawing.Color.White;
            dgvRowStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dgvRowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dgvRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvRowStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPhanCong.DefaultCellStyle = dgvRowStyle;
            this.dgvPhanCong.EnableHeadersVisualStyles = false;
            this.dgvPhanCong.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.dgvPhanCong.Location = new System.Drawing.Point(15, 116);
            this.dgvPhanCong.MultiSelect = false;
            this.dgvPhanCong.Name = "dgvPhanCong";
            this.dgvPhanCong.ReadOnly = true;
            this.dgvPhanCong.RowHeadersVisible = false;
            this.dgvPhanCong.RowTemplate.Height = 32;
            this.dgvPhanCong.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhanCong.Size = new System.Drawing.Size(1054, 275);
            this.dgvPhanCong.TabIndex = 2;
            this.dgvPhanCong.SelectionChanged += new System.EventHandler(this.dgvPhanCong_SelectionChanged);

            // 
            // colMaPhanCong
            // 
            this.colMaPhanCong.DataPropertyName = "MaPhanCong";
            this.colMaPhanCong.HeaderText = "Mã ca";
            this.colMaPhanCong.Name = "colMaPhanCong";
            this.colMaPhanCong.ReadOnly = true;
            this.colMaPhanCong.Width = 75;

            // 
            // colTenSuKien
            // 
            this.colTenSuKien.DataPropertyName = "TenSuKien";
            this.colTenSuKien.HeaderText = "Sự kiện";
            this.colTenSuKien.Name = "colTenSuKien";
            this.colTenSuKien.ReadOnly = true;
            this.colTenSuKien.Width = 190;

            // 
            // colTenNhanSu
            // 
            this.colTenNhanSu.DataPropertyName = "TenNhanSu";
            this.colTenNhanSu.HeaderText = "Họ và tên nhân sự";
            this.colTenNhanSu.Name = "colTenNhanSu";
            this.colTenNhanSu.ReadOnly = true;
            this.colTenNhanSu.Width = 145;

            // 
            // colPhongBan
            // 
            this.colPhongBan.DataPropertyName = "PhongBan";
            this.colPhongBan.HeaderText = "Phòng ban";
            this.colPhongBan.Name = "colPhongBan";
            this.colPhongBan.ReadOnly = true;
            this.colPhongBan.Width = 95;

            // 
            // colChucVuNhanSu
            // 
            this.colChucVuNhanSu.DataPropertyName = "ChucVuNhanSu";
            this.colChucVuNhanSu.HeaderText = "Chức vụ";
            this.colChucVuNhanSu.Name = "colChucVuNhanSu";
            this.colChucVuNhanSu.ReadOnly = true;
            this.colChucVuNhanSu.Width = 125;

            // 
            // colSoDienThoai
            // 
            this.colSoDienThoai.DataPropertyName = "SoDienThoai";
            this.colSoDienThoai.HeaderText = "Số điện thoại";
            this.colSoDienThoai.Name = "colSoDienThoai";
            this.colSoDienThoai.ReadOnly = true;
            this.colSoDienThoai.Width = 105;

            // 
            // colNhiemVu
            // 
            this.colNhiemVu.DataPropertyName = "NhiemVu";
            this.colNhiemVu.HeaderText = "Nhiệm vụ phân công";
            this.colNhiemVu.Name = "colNhiemVu";
            this.colNhiemVu.ReadOnly = true;
            this.colNhiemVu.Width = 185;

            // 
            // colNgayTruc
            // 
            this.colNgayTruc.DataPropertyName = "NgayTruc";
            this.colNgayTruc.HeaderText = "Ngày trực";
            this.colNgayTruc.Name = "colNgayTruc";
            this.colNgayTruc.ReadOnly = true;
            this.colNgayTruc.Width = 90;

            // 
            // colKhungGio
            // 
            this.colKhungGio.DataPropertyName = "KhungGio";
            this.colKhungGio.HeaderText = "Khung giờ";
            this.colKhungGio.Name = "colKhungGio";
            this.colKhungGio.ReadOnly = true;
            this.colKhungGio.Width = 120;

            // 
            // colGhiChu
            // 
            this.colGhiChu.DataPropertyName = "GhiChu";
            this.colGhiChu.HeaderText = "Ghi chú";
            this.colGhiChu.Name = "colGhiChu";
            this.colGhiChu.ReadOnly = true;
            this.colGhiChu.Width = 140;

            // 
            // grbThongTin
            // 
            this.grbThongTin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbThongTin.Controls.Add(this.lblKiemTraStatus);
            this.grbThongTin.Controls.Add(this.btnKiemTraXungDot);
            this.grbThongTin.Controls.Add(this.txtGhiChu);
            this.grbThongTin.Controls.Add(this.lblGhiChu);
            this.grbThongTin.Controls.Add(this.dtpKetThuc);
            this.grbThongTin.Controls.Add(this.lblKetThuc);
            this.grbThongTin.Controls.Add(this.dtpBatDau);
            this.grbThongTin.Controls.Add(this.lblBatDau);
            this.grbThongTin.Controls.Add(this.txtNhiemVu);
            this.grbThongTin.Controls.Add(this.lblNhiemVu);
            this.grbThongTin.Controls.Add(this.cboNhanSu);
            this.grbThongTin.Controls.Add(this.lblNhanSu);
            this.grbThongTin.Controls.Add(this.cboSuKien);
            this.grbThongTin.Controls.Add(this.lblSuKien);
            this.grbThongTin.Controls.Add(this.txtMaPhanCong);
            this.grbThongTin.Controls.Add(this.lblMaPhanCong);
            this.grbThongTin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grbThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.grbThongTin.Location = new System.Drawing.Point(15, 397);
            this.grbThongTin.Name = "grbThongTin";
            this.grbThongTin.Size = new System.Drawing.Size(1054, 208);
            this.grbThongTin.TabIndex = 3;
            this.grbThongTin.TabStop = false;
            this.grbThongTin.Text = "THÔNG TIN CHI TIẾT PHÂN CÔNG CA TRỰC";

            // 
            // lblMaPhanCong
            // 
            this.lblMaPhanCong.AutoSize = true;
            this.lblMaPhanCong.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaPhanCong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblMaPhanCong.Location = new System.Drawing.Point(20, 28);
            this.lblMaPhanCong.Name = "lblMaPhanCong";
            this.lblMaPhanCong.Size = new System.Drawing.Size(89, 15);
            this.lblMaPhanCong.TabIndex = 0;
            this.lblMaPhanCong.Text = "Mã phân công:";

            // 
            // txtMaPhanCong
            // 
            this.txtMaPhanCong.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMaPhanCong.Location = new System.Drawing.Point(125, 25);
            this.txtMaPhanCong.Name = "txtMaPhanCong";
            this.txtMaPhanCong.ReadOnly = true;
            this.txtMaPhanCong.Size = new System.Drawing.Size(155, 23);
            this.txtMaPhanCong.TabIndex = 1;

            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblSuKien.Location = new System.Drawing.Point(315, 28);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(64, 15);
            this.lblSuKien.TabIndex = 2;
            this.lblSuKien.Text = "Sự kiện (*):";

            // 
            // cboSuKien
            // 
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboSuKien.FormattingEnabled = true;
            this.cboSuKien.Location = new System.Drawing.Point(415, 25);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(615, 23);
            this.cboSuKien.TabIndex = 3;
            this.cboSuKien.SelectedIndexChanged += new System.EventHandler(this.cboSuKien_SelectedIndexChanged);

            // 
            // lblNhanSu
            // 
            this.lblNhanSu.AutoSize = true;
            this.lblNhanSu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNhanSu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblNhanSu.Location = new System.Drawing.Point(20, 63);
            this.lblNhanSu.Name = "lblNhanSu";
            this.lblNhanSu.Size = new System.Drawing.Size(70, 15);
            this.lblNhanSu.TabIndex = 4;
            this.lblNhanSu.Text = "Nhân sự (*):";

            // 
            // cboNhanSu
            // 
            this.cboNhanSu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhanSu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboNhanSu.FormattingEnabled = true;
            this.cboNhanSu.Location = new System.Drawing.Point(125, 60);
            this.cboNhanSu.Name = "cboNhanSu";
            this.cboNhanSu.Size = new System.Drawing.Size(380, 23);
            this.cboNhanSu.TabIndex = 5;

            // 
            // lblNhiemVu
            // 
            this.lblNhiemVu.AutoSize = true;
            this.lblNhiemVu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNhiemVu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblNhiemVu.Location = new System.Drawing.Point(535, 63);
            this.lblNhiemVu.Name = "lblNhiemVu";
            this.lblNhiemVu.Size = new System.Drawing.Size(76, 15);
            this.lblNhiemVu.TabIndex = 6;
            this.lblNhiemVu.Text = "Nhiệm vụ (*):";

            // 
            // txtNhiemVu
            // 
            this.txtNhiemVu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNhiemVu.Location = new System.Drawing.Point(640, 60);
            this.txtNhiemVu.Name = "txtNhiemVu";
            this.txtNhiemVu.PlaceholderText = "Ví dụ: Soát vé cửa 1, Vận hành âm thanh, Đón tiếp VIP...";
            this.txtNhiemVu.Size = new System.Drawing.Size(390, 23);
            this.txtNhiemVu.TabIndex = 7;

            // 
            // lblBatDau
            // 
            this.lblBatDau.AutoSize = true;
            this.lblBatDau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblBatDau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblBatDau.Location = new System.Drawing.Point(20, 98);
            this.lblBatDau.Name = "lblBatDau";
            this.lblBatDau.Size = new System.Drawing.Size(87, 15);
            this.lblBatDau.TabIndex = 8;
            this.lblBatDau.Text = "Giờ bắt đầu (*):";

            // 
            // dtpBatDau
            // 
            this.dtpBatDau.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpBatDau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBatDau.Location = new System.Drawing.Point(125, 95);
            this.dtpBatDau.Name = "dtpBatDau";
            this.dtpBatDau.Size = new System.Drawing.Size(155, 23);
            this.dtpBatDau.TabIndex = 9;

            // 
            // lblKetThuc
            // 
            this.lblKetThuc.AutoSize = true;
            this.lblKetThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKetThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblKetThuc.Location = new System.Drawing.Point(305, 98);
            this.lblKetThuc.Name = "lblKetThuc";
            this.lblKetThuc.Size = new System.Drawing.Size(91, 15);
            this.lblKetThuc.TabIndex = 10;
            this.lblKetThuc.Text = "Giờ kết thúc (*):";

            // 
            // dtpKetThuc
            // 
            this.dtpKetThuc.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpKetThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpKetThuc.Location = new System.Drawing.Point(435, 95);
            this.dtpKetThuc.Name = "dtpKetThuc";
            this.dtpKetThuc.Size = new System.Drawing.Size(155, 23);
            this.dtpKetThuc.TabIndex = 11;

            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblGhiChu.Location = new System.Drawing.Point(615, 98);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(51, 15);
            this.lblGhiChu.TabIndex = 12;
            this.lblGhiChu.Text = "Ghi chú:";

            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGhiChu.Location = new System.Drawing.Point(680, 95);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.PlaceholderText = "Ghi chú phân công, bộ đàm, vị trí trực...";
            this.txtGhiChu.Size = new System.Drawing.Size(350, 23);
            this.txtGhiChu.TabIndex = 13;

            // 
            // btnKiemTraXungDot
            // 
            this.btnKiemTraXungDot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.btnKiemTraXungDot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKiemTraXungDot.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(210)))), ((int)(((byte)(254)))));
            this.btnKiemTraXungDot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKiemTraXungDot.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnKiemTraXungDot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.btnKiemTraXungDot.Location = new System.Drawing.Point(125, 142);
            this.btnKiemTraXungDot.Name = "btnKiemTraXungDot";
            this.btnKiemTraXungDot.Size = new System.Drawing.Size(210, 34);
            this.btnKiemTraXungDot.TabIndex = 14;
            this.btnKiemTraXungDot.Text = "⚡ Kiểm tra ca trực";
            this.btnKiemTraXungDot.UseVisualStyleBackColor = false;
            this.btnKiemTraXungDot.Visible = false;
            this.btnKiemTraXungDot.Click += new System.EventHandler(this.btnKiemTraXungDot_Click);

            // 
            // lblKiemTraStatus
            // 
            this.lblKiemTraStatus.AutoSize = true;
            this.lblKiemTraStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblKiemTraStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblKiemTraStatus.Location = new System.Drawing.Point(350, 151);
            this.lblKiemTraStatus.Name = "lblKiemTraStatus";
            this.lblKiemTraStatus.Size = new System.Drawing.Size(220, 15);
            this.lblKiemTraStatus.TabIndex = 15;
            this.lblKiemTraStatus.Text = "Bấm để kiểm tra tính hợp lệ của ca trực";

            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.White;
            this.pnlButtons.Controls.Add(this.btnHuy);
            this.pnlButtons.Controls.Add(this.btnLuu);
            this.pnlButtons.Controls.Add(this.btnXuatExcel);
            this.pnlButtons.Controls.Add(this.btnXoa);
            this.pnlButtons.Controls.Add(this.btnSua);
            this.pnlButtons.Controls.Add(this.btnThem);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 620);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1084, 50);
            this.pnlButtons.TabIndex = 4;

            // 
            // btnThem
            // 
            this.btnThem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnThem.Location = new System.Drawing.Point(15, 7);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(125, 36);
            this.btnThem.TabIndex = 0;
            this.btnThem.Text = "➕ Thêm (F2)";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            // 
            // btnSua
            // 
            this.btnSua.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSua.Location = new System.Drawing.Point(150, 7);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(120, 36);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "✏️ Sửa (F3)";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);

            // 
            // btnXoa
            // 
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnXoa.Location = new System.Drawing.Point(280, 7);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(130, 36);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "🗑️ Xóa (Delete)";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnXuatExcel.Location = new System.Drawing.Point(420, 7);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(125, 36);
            this.btnXuatExcel.TabIndex = 3;
            this.btnXuatExcel.Text = "📊 Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);

            // 
            // btnLuu
            // 
            this.btnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLuu.Location = new System.Drawing.Point(800, 7);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(135, 36);
            this.btnLuu.TabIndex = 4;
            this.btnLuu.Text = "💾 Lưu (Ctrl+S)";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            // 
            // btnHuy
            // 
            this.btnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHuy.Location = new System.Drawing.Point(945, 7);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(120, 36);
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
            // FrmStaffAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1084, 670);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.grbThongTin);
            this.Controls.Add(this.dgvPhanCong);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.Name = "FrmStaffAssignment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmStaffAssignment_KeyDown);
            this.Load += new System.EventHandler(this.FrmStaffAssignment_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhanCong)).EndInit();
            this.grbThongTin.ResumeLayout(false);
            this.grbThongTin.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
