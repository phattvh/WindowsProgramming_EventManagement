namespace EventManagement.GUI.Forms
{
    partial class FrmEventList
    {
        private System.ComponentModel.IContainer components = null;

        // Header Panel
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        // Filter Panel
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Label lblLocLoaiSK;
        private System.Windows.Forms.ComboBox cboLocLoaiSK;
        private System.Windows.Forms.Label lblLocTrangThai;
        private System.Windows.Forms.ComboBox cboLocTrangThai;
        private System.Windows.Forms.CheckBox chkLocThoiGian;
        private System.Windows.Forms.DateTimePicker dtpLocTuNgay;
        private System.Windows.Forms.Label lblDenNgay;
        private System.Windows.Forms.DateTimePicker dtpLocDenNgay;
        private System.Windows.Forms.Button btnLamMoi;

        // DataGridView Panel
        private System.Windows.Forms.DataGridView dgvSuKien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaSuKien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSuKien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenLoaiSK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenDiaDiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayBatDau;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayKetThuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSucChua;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNganSachDuKien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNguoiPhuTrach;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoVeDaBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTyLeCheckIn;

        // Summary Bar
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblTongSo;
        private System.Windows.Forms.Label lblDangDienRa;
        private System.Windows.Forms.Label lblSapDienRa;
        private System.Windows.Forms.Label lblChuanBi;
        private System.Windows.Forms.Label lblDaHoanThanh;
        private System.Windows.Forms.Label lblDaHuy;

        // Detail GroupBox
        private System.Windows.Forms.GroupBox grbThongTin;
        private System.Windows.Forms.Label lblMaSuKien;
        private System.Windows.Forms.TextBox txtMaSuKien;
        private System.Windows.Forms.Label lblTenSuKien;
        private System.Windows.Forms.TextBox txtTenSuKien;
        private System.Windows.Forms.Label lblLoaiSK;
        private System.Windows.Forms.ComboBox cboLoaiSK;
        private System.Windows.Forms.Label lblDiaDiem;
        private System.Windows.Forms.ComboBox cboDiaDiem;
        private System.Windows.Forms.Label lblNgayBatDau;
        private System.Windows.Forms.DateTimePicker dtpNgayBatDau;
        private System.Windows.Forms.Label lblNgayKetThuc;
        private System.Windows.Forms.DateTimePicker dtpNgayKetThuc;
        private System.Windows.Forms.Label lblSucChua;
        private System.Windows.Forms.NumericUpDown nudSucChua;
        private System.Windows.Forms.Label lblNganSach;
        private System.Windows.Forms.NumericUpDown nudNganSach;
        private System.Windows.Forms.Label lblNguoiPhuTrach;
        private System.Windows.Forms.ComboBox cboNguoiPhuTrach;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.TextBox txtMoTa;

        // Buttons Panel
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnHuySuKien;
        private System.Windows.Forms.Button btnChiTiet;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();

            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();

            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.lblLocLoaiSK = new System.Windows.Forms.Label();
            this.cboLocLoaiSK = new System.Windows.Forms.ComboBox();
            this.lblLocTrangThai = new System.Windows.Forms.Label();
            this.cboLocTrangThai = new System.Windows.Forms.ComboBox();
            this.chkLocThoiGian = new System.Windows.Forms.CheckBox();
            this.dtpLocTuNgay = new System.Windows.Forms.DateTimePicker();
            this.lblDenNgay = new System.Windows.Forms.Label();
            this.dtpLocDenNgay = new System.Windows.Forms.DateTimePicker();
            this.btnLamMoi = new System.Windows.Forms.Button();

            this.dgvSuKien = new System.Windows.Forms.DataGridView();
            this.colMaSuKien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSuKien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenLoaiSK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenDiaDiem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayBatDau = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayKetThuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSucChua = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNganSachDuKien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNguoiPhuTrach = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoVeDaBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTyLeCheckIn = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlSummary = new System.Windows.Forms.Panel();
            this.lblTongSo = new System.Windows.Forms.Label();
            this.lblDangDienRa = new System.Windows.Forms.Label();
            this.lblSapDienRa = new System.Windows.Forms.Label();
            this.lblChuanBi = new System.Windows.Forms.Label();
            this.lblDaHoanThanh = new System.Windows.Forms.Label();
            this.lblDaHuy = new System.Windows.Forms.Label();

            this.grbThongTin = new System.Windows.Forms.GroupBox();
            this.lblMaSuKien = new System.Windows.Forms.Label();
            this.txtMaSuKien = new System.Windows.Forms.TextBox();
            this.lblTenSuKien = new System.Windows.Forms.Label();
            this.txtTenSuKien = new System.Windows.Forms.TextBox();
            this.lblLoaiSK = new System.Windows.Forms.Label();
            this.cboLoaiSK = new System.Windows.Forms.ComboBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();

            this.lblDiaDiem = new System.Windows.Forms.Label();
            this.cboDiaDiem = new System.Windows.Forms.ComboBox();
            this.lblNgayBatDau = new System.Windows.Forms.Label();
            this.dtpNgayBatDau = new System.Windows.Forms.DateTimePicker();
            this.lblNgayKetThuc = new System.Windows.Forms.Label();
            this.dtpNgayKetThuc = new System.Windows.Forms.DateTimePicker();

            this.lblSucChua = new System.Windows.Forms.Label();
            this.nudSucChua = new System.Windows.Forms.NumericUpDown();
            this.lblNganSach = new System.Windows.Forms.Label();
            this.nudNganSach = new System.Windows.Forms.NumericUpDown();
            this.lblNguoiPhuTrach = new System.Windows.Forms.Label();
            this.cboNguoiPhuTrach = new System.Windows.Forms.ComboBox();

            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnHuySuKien = new System.Windows.Forms.Button();
            this.btnChiTiet = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();

            this.errBaoLoi = new System.Windows.Forms.ErrorProvider(this.components);

            this.pnlHeader.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuKien)).BeginInit();
            this.pnlSummary.SuspendLayout();
            this.grbThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSucChua)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNganSach)).BeginInit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1184, 58);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1184, 58);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📅 QUẢN LÝ DANH SÁCH SỰ KIỆN TỔNG THỂ";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.lblSubtitle.Location = new System.Drawing.Point(16, 33);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(510, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "";
            this.lblSubtitle.Visible = false;

            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.lblTimKiem);
            this.pnlFilter.Controls.Add(this.txtTimKiem);
            this.pnlFilter.Controls.Add(this.lblLocLoaiSK);
            this.pnlFilter.Controls.Add(this.cboLocLoaiSK);
            this.pnlFilter.Controls.Add(this.lblLocTrangThai);
            this.pnlFilter.Controls.Add(this.cboLocTrangThai);
            this.pnlFilter.Controls.Add(this.chkLocThoiGian);
            this.pnlFilter.Controls.Add(this.dtpLocTuNgay);
            this.pnlFilter.Controls.Add(this.lblDenNgay);
            this.pnlFilter.Controls.Add(this.dtpLocDenNgay);
            this.pnlFilter.Controls.Add(this.btnLamMoi);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 58);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1204, 52);
            this.pnlFilter.TabIndex = 1;

            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTimKiem.Location = new System.Drawing.Point(15, 16);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(59, 15);
            this.lblTimKiem.TabIndex = 0;
            this.lblTimKiem.Text = "Tìm kiếm:";

            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTimKiem.Location = new System.Drawing.Point(92, 13);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Mã, tên, phụ trách...";
            this.txtTimKiem.Size = new System.Drawing.Size(130, 23);
            this.txtTimKiem.TabIndex = 1;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);

            // 
            // lblLocLoaiSK
            // 
            this.lblLocLoaiSK.AutoSize = true;
            this.lblLocLoaiSK.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocLoaiSK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocLoaiSK.Location = new System.Drawing.Point(238, 16);
            this.lblLocLoaiSK.Name = "lblLocLoaiSK";
            this.lblLocLoaiSK.Size = new System.Drawing.Size(73, 15);
            this.lblLocLoaiSK.TabIndex = 2;
            this.lblLocLoaiSK.Text = "Loại sự kiện:";

            // 
            // cboLocLoaiSK
            // 
            this.cboLocLoaiSK.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocLoaiSK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocLoaiSK.FormattingEnabled = true;
            this.cboLocLoaiSK.Location = new System.Drawing.Point(325, 13);
            this.cboLocLoaiSK.Name = "cboLocLoaiSK";
            this.cboLocLoaiSK.Size = new System.Drawing.Size(140, 23);
            this.cboLocLoaiSK.TabIndex = 3;
            this.cboLocLoaiSK.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);

            // 
            // lblLocTrangThai
            // 
            this.lblLocTrangThai.AutoSize = true;
            this.lblLocTrangThai.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocTrangThai.Location = new System.Drawing.Point(485, 16);
            this.lblLocTrangThai.Name = "lblLocTrangThai";
            this.lblLocTrangThai.Size = new System.Drawing.Size(63, 15);
            this.lblLocTrangThai.TabIndex = 4;
            this.lblLocTrangThai.Text = "Trạng thái:";

            // 
            // cboLocTrangThai
            // 
            this.cboLocTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocTrangThai.FormattingEnabled = true;
            this.cboLocTrangThai.Location = new System.Drawing.Point(560, 13);
            this.cboLocTrangThai.Name = "cboLocTrangThai";
            this.cboLocTrangThai.Size = new System.Drawing.Size(130, 23);
            this.cboLocTrangThai.TabIndex = 5;
            this.cboLocTrangThai.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);

            // 
            // chkLocThoiGian
            // 
            this.chkLocThoiGian.AutoSize = true;
            this.chkLocThoiGian.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkLocThoiGian.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.chkLocThoiGian.Location = new System.Drawing.Point(710, 15);
            this.chkLocThoiGian.Name = "chkLocThoiGian";
            this.chkLocThoiGian.Size = new System.Drawing.Size(77, 19);
            this.chkLocThoiGian.TabIndex = 6;
            this.chkLocThoiGian.Text = "Thời gian:";
            this.chkLocThoiGian.UseVisualStyleBackColor = true;
            this.chkLocThoiGian.CheckedChanged += new System.EventHandler(this.chkLocThoiGian_CheckedChanged);

            // 
            // dtpLocTuNgay
            // 
            this.dtpLocTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpLocTuNgay.Enabled = false;
            this.dtpLocTuNgay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpLocTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLocTuNgay.Location = new System.Drawing.Point(800, 13);
            this.dtpLocTuNgay.Name = "dtpLocTuNgay";
            this.dtpLocTuNgay.Size = new System.Drawing.Size(105, 23);
            this.dtpLocTuNgay.TabIndex = 7;
            this.dtpLocTuNgay.ValueChanged += new System.EventHandler(this.Filter_Changed);

            // 
            // lblDenNgay
            // 
            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDenNgay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblDenNgay.Location = new System.Drawing.Point(915, 16);
            this.lblDenNgay.Name = "lblDenNgay";
            this.lblDenNgay.Size = new System.Drawing.Size(12, 15);
            this.lblDenNgay.TabIndex = 8;
            this.lblDenNgay.Text = "-";

            // 
            // dtpLocDenNgay
            // 
            this.dtpLocDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpLocDenNgay.Enabled = false;
            this.dtpLocDenNgay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpLocDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLocDenNgay.Location = new System.Drawing.Point(935, 13);
            this.dtpLocDenNgay.Name = "dtpLocDenNgay";
            this.dtpLocDenNgay.Size = new System.Drawing.Size(105, 23);
            this.dtpLocDenNgay.TabIndex = 9;
            this.dtpLocDenNgay.ValueChanged += new System.EventHandler(this.Filter_Changed);

            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLamMoi.Location = new System.Drawing.Point(1065, 9);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(105, 32);
            this.btnLamMoi.TabIndex = 10;
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);

            // 
            // dgvSuKien
            // 
            this.dgvSuKien.AllowUserToAddRows = false;
            this.dgvSuKien.AllowUserToDeleteRows = false;
            this.dgvSuKien.AllowUserToResizeRows = false;
            this.dgvSuKien.BackgroundColor = System.Drawing.Color.White;
            this.dgvSuKien.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSuKien.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSuKien.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSuKien.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSuKien.ColumnHeadersHeight = 36;
            this.dgvSuKien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSuKien.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaSuKien,
            this.colTenSuKien,
            this.colTenLoaiSK,
            this.colTenDiaDiem,
            this.colNgayBatDau,
            this.colNgayKetThuc,
            this.colSucChua,
            this.colNganSachDuKien,
            this.colNguoiPhuTrach,
            this.colTrangThai,
            this.colSoVeDaBan,
            this.colTyLeCheckIn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSuKien.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSuKien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSuKien.EnableHeadersVisualStyles = false;
            this.dgvSuKien.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.dgvSuKien.Location = new System.Drawing.Point(0, 110);
            this.dgvSuKien.MultiSelect = false;
            this.dgvSuKien.Name = "dgvSuKien";
            this.dgvSuKien.ReadOnly = true;
            this.dgvSuKien.RowHeadersVisible = false;
            this.dgvSuKien.RowHeadersWidth = 51;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dgvSuKien.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSuKien.RowTemplate.Height = 32;
            this.dgvSuKien.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuKien.Size = new System.Drawing.Size(1184, 305);
            this.dgvSuKien.TabIndex = 2;
            this.dgvSuKien.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSuKien_CellFormatting);
            this.dgvSuKien.SelectionChanged += new System.EventHandler(this.dgvSuKien_SelectionChanged);
            this.dgvSuKien.DoubleClick += new System.EventHandler(this.dgvSuKien_DoubleClick);

            // 
            // colMaSuKien
            // 
            this.colMaSuKien.DataPropertyName = "MaSuKien";
            this.colMaSuKien.HeaderText = "Mã Sự Kiện";
            this.colMaSuKien.MinimumWidth = 95;
            this.colMaSuKien.Name = "colMaSuKien";
            this.colMaSuKien.ReadOnly = true;
            this.colMaSuKien.Width = 100;

            // 
            // colTenSuKien
            // 
            this.colTenSuKien.DataPropertyName = "TenSuKien";
            this.colTenSuKien.HeaderText = "Tên Sự Kiện";
            this.colTenSuKien.MinimumWidth = 200;
            this.colTenSuKien.Name = "colTenSuKien";
            this.colTenSuKien.ReadOnly = true;
            this.colTenSuKien.Width = 240;

            // 
            // colTenLoaiSK
            // 
            this.colTenLoaiSK.DataPropertyName = "TenLoaiSK";
            this.colTenLoaiSK.HeaderText = "Loại Sự Kiện";
            this.colTenLoaiSK.MinimumWidth = 130;
            this.colTenLoaiSK.Name = "colTenLoaiSK";
            this.colTenLoaiSK.ReadOnly = true;
            this.colTenLoaiSK.Width = 145;

            // 
            // colTenDiaDiem
            // 
            this.colTenDiaDiem.DataPropertyName = "TenDiaDiem";
            this.colTenDiaDiem.HeaderText = "Địa Điểm Tổ Chức";
            this.colTenDiaDiem.MinimumWidth = 150;
            this.colTenDiaDiem.Name = "colTenDiaDiem";
            this.colTenDiaDiem.ReadOnly = true;
            this.colTenDiaDiem.Width = 175;

            // 
            // colNgayBatDau
            // 
            this.colNgayBatDau.DataPropertyName = "NgayBatDau";
            this.colNgayBatDau.HeaderText = "Bắt Đầu";
            this.colNgayBatDau.MinimumWidth = 120;
            this.colNgayBatDau.Name = "colNgayBatDau";
            this.colNgayBatDau.ReadOnly = true;
            this.colNgayBatDau.Width = 125;

            // 
            // colNgayKetThuc
            // 
            this.colNgayKetThuc.DataPropertyName = "NgayKetThuc";
            this.colNgayKetThuc.HeaderText = "Kết Thúc";
            this.colNgayKetThuc.MinimumWidth = 120;
            this.colNgayKetThuc.Name = "colNgayKetThuc";
            this.colNgayKetThuc.ReadOnly = true;
            this.colNgayKetThuc.Width = 125;

            // 
            // colSucChua
            // 
            this.colSucChua.DataPropertyName = "SucChua";
            this.colSucChua.HeaderText = "Sức Chứa";
            this.colSucChua.MinimumWidth = 75;
            this.colSucChua.Name = "colSucChua";
            this.colSucChua.ReadOnly = true;
            this.colSucChua.Width = 80;

            // 
            // colNganSachDuKien
            // 
            this.colNganSachDuKien.DataPropertyName = "NganSachDuKien";
            this.colNganSachDuKien.HeaderText = "Ngân Sách (VNĐ)";
            this.colNganSachDuKien.MinimumWidth = 115;
            this.colNganSachDuKien.Name = "colNganSachDuKien";
            this.colNganSachDuKien.ReadOnly = true;
            this.colNganSachDuKien.Width = 120;

            // 
            // colNguoiPhuTrach
            // 
            this.colNguoiPhuTrach.DataPropertyName = "NguoiPhuTrach";
            this.colNguoiPhuTrach.HeaderText = "Người Phụ Trách";
            this.colNguoiPhuTrach.MinimumWidth = 125;
            this.colNguoiPhuTrach.Name = "colNguoiPhuTrach";
            this.colNguoiPhuTrach.ReadOnly = true;
            this.colNguoiPhuTrach.Width = 135;

            // 
            // colTrangThai
            // 
            this.colTrangThai.DataPropertyName = "TrangThai";
            this.colTrangThai.HeaderText = "Trạng Thái";
            this.colTrangThai.MinimumWidth = 100;
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            this.colTrangThai.Width = 105;

            // 
            // colSoVeDaBan
            // 
            this.colSoVeDaBan.DataPropertyName = "SoVeDaBan";
            this.colSoVeDaBan.HeaderText = "Vé Đã Bán";
            this.colSoVeDaBan.MinimumWidth = 80;
            this.colSoVeDaBan.Name = "colSoVeDaBan";
            this.colSoVeDaBan.ReadOnly = true;
            this.colSoVeDaBan.Width = 85;

            // 
            // colTyLeCheckIn
            // 
            this.colTyLeCheckIn.DataPropertyName = "TyLeCheckIn";
            this.colTyLeCheckIn.HeaderText = "Check-in (%)";
            this.colTyLeCheckIn.MinimumWidth = 85;
            this.colTyLeCheckIn.Name = "colTyLeCheckIn";
            this.colTyLeCheckIn.ReadOnly = true;
            this.colTyLeCheckIn.Width = 90;

            // 
            // pnlSummary
            // 
            this.pnlSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.pnlSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSummary.Controls.Add(this.lblTongSo);
            this.pnlSummary.Controls.Add(this.lblDangDienRa);
            this.pnlSummary.Controls.Add(this.lblSapDienRa);
            this.pnlSummary.Controls.Add(this.lblChuanBi);
            this.pnlSummary.Controls.Add(this.lblDaHoanThanh);
            this.pnlSummary.Controls.Add(this.lblDaHuy);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSummary.Location = new System.Drawing.Point(0, 415);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlSummary.Size = new System.Drawing.Size(1184, 38);
            this.pnlSummary.TabIndex = 3;

            // 
            // lblTongSo
            // 
            this.lblTongSo.AutoSize = true;
            this.lblTongSo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTongSo.Location = new System.Drawing.Point(12, 6);
            this.lblTongSo.Name = "lblTongSo";
            this.lblTongSo.Size = new System.Drawing.Size(115, 15);
            this.lblTongSo.TabIndex = 0;
            this.lblTongSo.Text = "Tổng số: 0 sự kiện";

            // 
            // lblDangDienRa
            // 
            this.lblDangDienRa.AutoSize = true;
            this.lblDangDienRa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblDangDienRa.Location = new System.Drawing.Point(165, 6);
            this.lblDangDienRa.Name = "lblDangDienRa";
            this.lblDangDienRa.Size = new System.Drawing.Size(95, 15);
            this.lblDangDienRa.TabIndex = 1;
            this.lblDangDienRa.Text = "Đang diễn ra: 0";

            // 
            // lblSapDienRa
            // 
            this.lblSapDienRa.AutoSize = true;
            this.lblSapDienRa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblSapDienRa.Location = new System.Drawing.Point(295, 6);
            this.lblSapDienRa.Name = "lblSapDienRa";
            this.lblSapDienRa.Size = new System.Drawing.Size(87, 15);
            this.lblSapDienRa.TabIndex = 2;
            this.lblSapDienRa.Text = "Sắp diễn ra: 0";

            // 
            // lblChuanBi
            // 
            this.lblChuanBi.AutoSize = true;
            this.lblChuanBi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblChuanBi.Location = new System.Drawing.Point(415, 6);
            this.lblChuanBi.Name = "lblChuanBi";
            this.lblChuanBi.Size = new System.Drawing.Size(73, 15);
            this.lblChuanBi.TabIndex = 3;
            this.lblChuanBi.Text = "Chuẩn bị: 0";

            // 
            // lblDaHoanThanh
            // 
            this.lblDaHoanThanh.AutoSize = true;
            this.lblDaHoanThanh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblDaHoanThanh.Location = new System.Drawing.Point(525, 6);
            this.lblDaHoanThanh.Name = "lblDaHoanThanh";
            this.lblDaHoanThanh.Size = new System.Drawing.Size(107, 15);
            this.lblDaHoanThanh.TabIndex = 4;
            this.lblDaHoanThanh.Text = "Đã hoàn thành: 0";

            // 
            // lblDaHuy
            // 
            this.lblDaHuy.AutoSize = true;
            this.lblDaHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblDaHuy.Location = new System.Drawing.Point(665, 6);
            this.lblDaHuy.Name = "lblDaHuy";
            this.lblDaHuy.Size = new System.Drawing.Size(63, 15);
            this.lblDaHuy.TabIndex = 5;
            this.lblDaHuy.Text = "Đã hủy: 0";

            // 
            // grbThongTin
            // 
            this.grbThongTin.BackColor = System.Drawing.Color.White;
            this.grbThongTin.Controls.Add(this.lblMaSuKien);
            this.grbThongTin.Controls.Add(this.txtMaSuKien);
            this.grbThongTin.Controls.Add(this.lblTenSuKien);
            this.grbThongTin.Controls.Add(this.txtTenSuKien);
            this.grbThongTin.Controls.Add(this.lblLoaiSK);
            this.grbThongTin.Controls.Add(this.cboLoaiSK);
            this.grbThongTin.Controls.Add(this.lblTrangThai);
            this.grbThongTin.Controls.Add(this.cboTrangThai);
            this.grbThongTin.Controls.Add(this.lblDiaDiem);
            this.grbThongTin.Controls.Add(this.cboDiaDiem);
            this.grbThongTin.Controls.Add(this.lblNgayBatDau);
            this.grbThongTin.Controls.Add(this.dtpNgayBatDau);
            this.grbThongTin.Controls.Add(this.lblNgayKetThuc);
            this.grbThongTin.Controls.Add(this.dtpNgayKetThuc);
            this.grbThongTin.Controls.Add(this.lblSucChua);
            this.grbThongTin.Controls.Add(this.nudSucChua);
            this.grbThongTin.Controls.Add(this.lblNganSach);
            this.grbThongTin.Controls.Add(this.nudNganSach);
            this.grbThongTin.Controls.Add(this.lblNguoiPhuTrach);
            this.grbThongTin.Controls.Add(this.cboNguoiPhuTrach);
            this.grbThongTin.Controls.Add(this.lblMoTa);
            this.grbThongTin.Controls.Add(this.txtMoTa);
            this.grbThongTin.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grbThongTin.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grbThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.grbThongTin.Location = new System.Drawing.Point(0, 438);
            this.grbThongTin.Name = "grbThongTin";
            this.grbThongTin.Size = new System.Drawing.Size(1204, 220);
            this.grbThongTin.TabIndex = 4;
            this.grbThongTin.TabStop = false;
            this.grbThongTin.Text = "  THÔNG TIN CHI TIẾT SỰ KIỆN  ";

            // 
            // lblMaSuKien
            // 
            this.lblMaSuKien.AutoSize = true;
            this.lblMaSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblMaSuKien.Location = new System.Drawing.Point(15, 28);
            this.lblMaSuKien.Name = "lblMaSuKien";
            this.lblMaSuKien.Size = new System.Drawing.Size(68, 15);
            this.lblMaSuKien.TabIndex = 0;
            this.lblMaSuKien.Text = "Mã sự kiện:";

            // 
            // txtMaSuKien
            // 
            this.txtMaSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMaSuKien.Location = new System.Drawing.Point(105, 25);
            this.txtMaSuKien.Name = "txtMaSuKien";
            this.txtMaSuKien.ReadOnly = true;
            this.txtMaSuKien.Size = new System.Drawing.Size(95, 23);
            this.txtMaSuKien.TabIndex = 1;

            // 
            // lblTenSuKien
            // 
            this.lblTenSuKien.AutoSize = true;
            this.lblTenSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTenSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTenSuKien.Location = new System.Drawing.Point(215, 28);
            this.lblTenSuKien.Name = "lblTenSuKien";
            this.lblTenSuKien.Size = new System.Drawing.Size(69, 15);
            this.lblTenSuKien.TabIndex = 2;
            this.lblTenSuKien.Text = "Tên sự kiện (*):";

            // 
            // txtTenSuKien
            // 
            this.txtTenSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTenSuKien.Location = new System.Drawing.Point(330, 25);
            this.txtTenSuKien.Name = "txtTenSuKien";
            this.txtTenSuKien.Size = new System.Drawing.Size(240, 23);
            this.txtTenSuKien.TabIndex = 3;

            // 
            // lblLoaiSK
            // 
            this.lblLoaiSK.AutoSize = true;
            this.lblLoaiSK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLoaiSK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblLoaiSK.Location = new System.Drawing.Point(585, 28);
            this.lblLoaiSK.Name = "lblLoaiSK";
            this.lblLoaiSK.Size = new System.Drawing.Size(73, 15);
            this.lblLoaiSK.TabIndex = 4;
            this.lblLoaiSK.Text = "Loại sự kiện (*):";

            // 
            // cboLoaiSK
            // 
            this.cboLoaiSK.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiSK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLoaiSK.FormattingEnabled = true;
            this.cboLoaiSK.Location = new System.Drawing.Point(685, 25);
            this.cboLoaiSK.Name = "cboLoaiSK";
            this.cboLoaiSK.Size = new System.Drawing.Size(165, 23);
            this.cboLoaiSK.TabIndex = 5;

            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTrangThai.Location = new System.Drawing.Point(880, 28);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(62, 15);
            this.lblTrangThai.TabIndex = 6;
            this.lblTrangThai.Text = "Trạng thái (*):";

            // 
            // cboTrangThai
            // 
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboTrangThai.FormattingEnabled = true;
            this.cboTrangThai.Location = new System.Drawing.Point(975, 25);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(200, 23);
            this.cboTrangThai.TabIndex = 7;

            // 
            // lblDiaDiem
            // 
            this.lblDiaDiem.AutoSize = true;
            this.lblDiaDiem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDiaDiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblDiaDiem.Location = new System.Drawing.Point(15, 63);
            this.lblDiaDiem.Name = "lblDiaDiem";
            this.lblDiaDiem.Size = new System.Drawing.Size(57, 15);
            this.lblDiaDiem.TabIndex = 8;
            this.lblDiaDiem.Text = "Địa điểm (*):";

            // 
            // cboDiaDiem
            // 
            this.cboDiaDiem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDiaDiem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboDiaDiem.FormattingEnabled = true;
            this.cboDiaDiem.Location = new System.Drawing.Point(105, 60);
            this.cboDiaDiem.Name = "cboDiaDiem";
            this.cboDiaDiem.Size = new System.Drawing.Size(290, 23);
            this.cboDiaDiem.TabIndex = 9;

            // 
            // lblNgayBatDau
            // 
            this.lblNgayBatDau.AutoSize = true;
            this.lblNgayBatDau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNgayBatDau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblNgayBatDau.Location = new System.Drawing.Point(415, 63);
            this.lblNgayBatDau.Name = "lblNgayBatDau";
            this.lblNgayBatDau.Size = new System.Drawing.Size(81, 15);
            this.lblNgayBatDau.TabIndex = 10;
            this.lblNgayBatDau.Text = "Ngày bắt đầu (*):";

            // 
            // dtpNgayBatDau
            // 
            this.dtpNgayBatDau.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpNgayBatDau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpNgayBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayBatDau.Location = new System.Drawing.Point(530, 60);
            this.dtpNgayBatDau.Name = "dtpNgayBatDau";
            this.dtpNgayBatDau.Size = new System.Drawing.Size(165, 23);
            this.dtpNgayBatDau.TabIndex = 11;

            // 
            // lblNgayKetThuc
            // 
            this.lblNgayKetThuc.AutoSize = true;
            this.lblNgayKetThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNgayKetThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblNgayKetThuc.Location = new System.Drawing.Point(715, 63);
            this.lblNgayKetThuc.Name = "lblNgayKetThuc";
            this.lblNgayKetThuc.Size = new System.Drawing.Size(84, 15);
            this.lblNgayKetThuc.TabIndex = 12;
            this.lblNgayKetThuc.Text = "Ngày kết thúc (*):";

            // 
            // dtpNgayKetThuc
            // 
            this.dtpNgayKetThuc.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpNgayKetThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpNgayKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayKetThuc.Location = new System.Drawing.Point(835, 60);
            this.dtpNgayKetThuc.Name = "dtpNgayKetThuc";
            this.dtpNgayKetThuc.Size = new System.Drawing.Size(165, 23);
            this.dtpNgayKetThuc.TabIndex = 13;

            // 
            // lblSucChua
            // 
            this.lblSucChua.AutoSize = true;
            this.lblSucChua.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSucChua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblSucChua.Location = new System.Drawing.Point(15, 98);
            this.lblSucChua.Name = "lblSucChua";
            this.lblSucChua.Size = new System.Drawing.Size(58, 15);
            this.lblSucChua.TabIndex = 14;
            this.lblSucChua.Text = "Sức chứa (*):";

            // 
            // nudSucChua
            // 
            this.nudSucChua.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nudSucChua.Location = new System.Drawing.Point(105, 95);
            this.nudSucChua.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.nudSucChua.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudSucChua.Name = "nudSucChua";
            this.nudSucChua.Size = new System.Drawing.Size(100, 23);
            this.nudSucChua.TabIndex = 15;
            this.nudSucChua.ThousandsSeparator = true;
            this.nudSucChua.Value = new decimal(new int[] { 500, 0, 0, 0 });

            // 
            // lblNganSach
            // 
            this.lblNganSach.AutoSize = true;
            this.lblNganSach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNganSach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblNganSach.Location = new System.Drawing.Point(220, 98);
            this.lblNganSach.Name = "lblNganSach";
            this.lblNganSach.Size = new System.Drawing.Size(107, 15);
            this.lblNganSach.TabIndex = 16;
            this.lblNganSach.Text = "Ngân sách dự kiến:";

            // 
            // nudNganSach
            // 
            this.nudNganSach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nudNganSach.Increment = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.nudNganSach.Location = new System.Drawing.Point(355, 95);
            this.nudNganSach.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            this.nudNganSach.Name = "nudNganSach";
            this.nudNganSach.Size = new System.Drawing.Size(160, 23);
            this.nudNganSach.TabIndex = 17;
            this.nudNganSach.ThousandsSeparator = true;

            // 
            // lblNguoiPhuTrach
            // 
            this.lblNguoiPhuTrach.AutoSize = true;
            this.lblNguoiPhuTrach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNguoiPhuTrach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblNguoiPhuTrach.Location = new System.Drawing.Point(540, 98);
            this.lblNguoiPhuTrach.Name = "lblNguoiPhuTrach";
            this.lblNguoiPhuTrach.Size = new System.Drawing.Size(96, 15);
            this.lblNguoiPhuTrach.TabIndex = 18;
            this.lblNguoiPhuTrach.Text = "Người phụ trách (*):";

            // 
            // txtNguoiPhuTrach
            // 
            this.cboNguoiPhuTrach.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboNguoiPhuTrach.Location = new System.Drawing.Point(680, 95);
            this.cboNguoiPhuTrach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboNguoiPhuTrach.Name = "cboNguoiPhuTrach";
            this.cboNguoiPhuTrach.Size = new System.Drawing.Size(230, 23);
            this.cboNguoiPhuTrach.TabIndex = 19;

            // 
            // lblMoTa
            // 
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMoTa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblMoTa.Location = new System.Drawing.Point(15, 133);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(83, 15);
            this.lblMoTa.TabIndex = 20;
            this.lblMoTa.Text = "Mô tả sự kiện:";

            // 
            // txtMoTa
            // 
            this.txtMoTa.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMoTa.Location = new System.Drawing.Point(125, 130);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMoTa.Size = new System.Drawing.Size(1055, 75);
            this.txtMoTa.TabIndex = 21;

            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlButtons.Controls.Add(this.btnThem);
            this.pnlButtons.Controls.Add(this.btnSua);
            this.pnlButtons.Controls.Add(this.btnXoa);
            this.pnlButtons.Controls.Add(this.btnHuySuKien);
            this.pnlButtons.Controls.Add(this.btnChiTiet);
            this.pnlButtons.Controls.Add(this.btnXuatExcel);
            this.pnlButtons.Controls.Add(this.btnLuu);
            this.pnlButtons.Controls.Add(this.btnHuy);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 658);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1204, 52);
            this.pnlButtons.TabIndex = 5;

            // 
            // btnThem
            // 
            this.btnThem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnThem.Location = new System.Drawing.Point(15, 8);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 36);
            this.btnThem.TabIndex = 0;
            this.btnThem.Text = "➕ Thêm (F2)";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            // 
            // btnSua
            // 
            this.btnSua.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSua.Location = new System.Drawing.Point(145, 8);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(115, 36);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "✏️ Sửa (F3)";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);

            // 
            // btnXoa
            // 
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnXoa.Location = new System.Drawing.Point(270, 8);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(115, 36);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "🗑️ Xóa (Del)";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            // 
            // btnHuySuKien
            // 
            this.btnHuySuKien.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHuySuKien.Location = new System.Drawing.Point(395, 8);
            this.btnHuySuKien.Name = "btnHuySuKien";
            this.btnHuySuKien.Size = new System.Drawing.Size(130, 36);
            this.btnHuySuKien.TabIndex = 3;
            this.btnHuySuKien.Text = "🚫 Hủy sự kiện";
            this.btnHuySuKien.UseVisualStyleBackColor = true;
            this.btnHuySuKien.Click += new System.EventHandler(this.btnHuySuKien_Click);

            // 
            // btnChiTiet
            // 
            this.btnChiTiet.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnChiTiet.Location = new System.Drawing.Point(535, 8);
            this.btnChiTiet.Name = "btnChiTiet";
            this.btnChiTiet.Size = new System.Drawing.Size(130, 36);
            this.btnChiTiet.TabIndex = 4;
            this.btnChiTiet.Text = "👁️ Xem chi tiết";
            this.btnChiTiet.UseVisualStyleBackColor = true;
            this.btnChiTiet.Click += new System.EventHandler(this.btnChiTiet_Click);

            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnXuatExcel.Location = new System.Drawing.Point(675, 8);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(125, 36);
            this.btnXuatExcel.TabIndex = 5;
            this.btnXuatExcel.Text = "📊 Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);

            // 
            // btnLuu
            // 
            this.btnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLuu.Location = new System.Drawing.Point(945, 8);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(120, 36);
            this.btnLuu.TabIndex = 6;
            this.btnLuu.Text = "💾 Lưu (Ctrl+S)";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            // 
            // btnHuy
            // 
            this.btnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHuy.Location = new System.Drawing.Point(1075, 8);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(110, 36);
            this.btnHuy.TabIndex = 7;
            this.btnHuy.Text = "❌ Hủy (Esc)";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // 
            // errBaoLoi
            // 
            this.errBaoLoi.ContainerControl = this;

            // 
            // FrmEventList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 710);
            this.Controls.Add(this.dgvSuKien);
            this.Controls.Add(this.pnlSummary);
            this.Controls.Add(this.grbThongTin);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(1200, 720);
            this.Name = "FrmEventList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmEventList_KeyDown);
            this.Load += new System.EventHandler(this.FrmEventList_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuKien)).EndInit();
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            this.grbThongTin.ResumeLayout(false);
            this.grbThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSucChua)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNganSach)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
