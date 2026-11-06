namespace EventManagement.GUI.Forms
{
    partial class FrmSchedule
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblLocSuKien;
        private System.Windows.Forms.ComboBox cboLocSuKien;
        private System.Windows.Forms.Label lblLocDienGia;
        private System.Windows.Forms.ComboBox cboLocDienGia;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnLamMoi;

        private System.Windows.Forms.DataGridView dgvLichTrinh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaLichTrinh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSuKien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenTietMuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayDienRa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKhungGio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiaDiemChiTiet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenDienGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGhiChu;

        private System.Windows.Forms.GroupBox grbThongTin;
        private System.Windows.Forms.Label lblMaLichTrinh;
        private System.Windows.Forms.TextBox txtMaLichTrinh;
        private System.Windows.Forms.Label lblSuKien;
        private System.Windows.Forms.ComboBox cboSuKien;
        private System.Windows.Forms.Label lblTenTietMuc;
        private System.Windows.Forms.TextBox txtTenTietMuc;
        private System.Windows.Forms.Label lblBatDau;
        private System.Windows.Forms.DateTimePicker dtpBatDau;
        private System.Windows.Forms.Label lblKetThuc;
        private System.Windows.Forms.DateTimePicker dtpKetThuc;
        private System.Windows.Forms.Label lblDienGia;
        private System.Windows.Forms.ComboBox cboDienGia;
        private System.Windows.Forms.Label lblDiaDiemChiTiet;
        private System.Windows.Forms.TextBox txtDiaDiemChiTiet;
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
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblLocSuKien = new System.Windows.Forms.Label();
            this.cboLocSuKien = new System.Windows.Forms.ComboBox();
            this.lblLocDienGia = new System.Windows.Forms.Label();
            this.cboLocDienGia = new System.Windows.Forms.ComboBox();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnLamMoi = new System.Windows.Forms.Button();

            this.dgvLichTrinh = new System.Windows.Forms.DataGridView();
            this.colMaLichTrinh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSuKien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenTietMuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayDienRa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKhungGio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiaDiemChiTiet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenDienGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.grbThongTin = new System.Windows.Forms.GroupBox();
            this.lblMaLichTrinh = new System.Windows.Forms.Label();
            this.txtMaLichTrinh = new System.Windows.Forms.TextBox();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.cboSuKien = new System.Windows.Forms.ComboBox();
            this.lblTenTietMuc = new System.Windows.Forms.Label();
            this.txtTenTietMuc = new System.Windows.Forms.TextBox();
            this.lblBatDau = new System.Windows.Forms.Label();
            this.dtpBatDau = new System.Windows.Forms.DateTimePicker();
            this.lblKetThuc = new System.Windows.Forms.Label();
            this.dtpKetThuc = new System.Windows.Forms.DateTimePicker();
            this.lblDienGia = new System.Windows.Forms.Label();
            this.cboDienGia = new System.Windows.Forms.ComboBox();
            this.lblDiaDiemChiTiet = new System.Windows.Forms.Label();
            this.txtDiaDiemChiTiet = new System.Windows.Forms.TextBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichTrinh)).BeginInit();
            this.grbThongTin.SuspendLayout();
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
            this.pnlHeader.Size = new System.Drawing.Size(1100, 56);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(465, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📅 QUẢN LÝ LỊCH TRÌNH TIẾT MỤC SỰ KIỆN (F08)";

            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.lblSubtitle.Location = new System.Drawing.Point(20, 34);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(530, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Thiết lập timeline chi tiết, kiểm soát xung đột phòng chức năng và phân công diễn giả (BR-05 & BR-06)";

            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.btnLamMoi);
            this.pnlFilter.Controls.Add(this.txtTimKiem);
            this.pnlFilter.Controls.Add(this.lblTimKiem);
            this.pnlFilter.Controls.Add(this.cboLocDienGia);
            this.pnlFilter.Controls.Add(this.lblLocDienGia);
            this.pnlFilter.Controls.Add(this.cboLocSuKien);
            this.pnlFilter.Controls.Add(this.lblLocSuKien);
            this.pnlFilter.Location = new System.Drawing.Point(20, 66);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1060, 46);
            this.pnlFilter.TabIndex = 1;

            // 
            // lblLocSuKien
            // 
            this.lblLocSuKien.AutoSize = true;
            this.lblLocSuKien.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocSuKien.Location = new System.Drawing.Point(12, 14);
            this.lblLocSuKien.Name = "lblLocSuKien";
            this.lblLocSuKien.Size = new System.Drawing.Size(53, 15);
            this.lblLocSuKien.TabIndex = 0;
            this.lblLocSuKien.Text = "Sự kiện:";

            // 
            // cboLocSuKien
            // 
            this.cboLocSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocSuKien.FormattingEnabled = true;
            this.cboLocSuKien.Location = new System.Drawing.Point(70, 11);
            this.cboLocSuKien.Name = "cboLocSuKien";
            this.cboLocSuKien.Size = new System.Drawing.Size(240, 23);
            this.cboLocSuKien.TabIndex = 1;
            this.cboLocSuKien.SelectedIndexChanged += new System.EventHandler(this.cboLocSuKien_SelectedIndexChanged);

            // 
            // lblLocDienGia
            // 
            this.lblLocDienGia.AutoSize = true;
            this.lblLocDienGia.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocDienGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocDienGia.Location = new System.Drawing.Point(325, 14);
            this.lblLocDienGia.Name = "lblLocDienGia";
            this.lblLocDienGia.Size = new System.Drawing.Size(54, 15);
            this.lblLocDienGia.TabIndex = 2;
            this.lblLocDienGia.Text = "Diễn giả:";

            // 
            // cboLocDienGia
            // 
            this.cboLocDienGia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocDienGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocDienGia.FormattingEnabled = true;
            this.cboLocDienGia.Location = new System.Drawing.Point(385, 11);
            this.cboLocDienGia.Name = "cboLocDienGia";
            this.cboLocDienGia.Size = new System.Drawing.Size(190, 23);
            this.cboLocDienGia.TabIndex = 3;
            this.cboLocDienGia.SelectedIndexChanged += new System.EventHandler(this.cboLocDienGia_SelectedIndexChanged);

            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTimKiem.Location = new System.Drawing.Point(590, 14);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(59, 15);
            this.lblTimKiem.TabIndex = 4;
            this.lblTimKiem.Text = "Tìm kiếm:";

            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTimKiem.Location = new System.Drawing.Point(655, 11);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Nhập tên tiết mục, phòng, diễn giả...";
            this.txtTimKiem.Size = new System.Drawing.Size(280, 23);
            this.txtTimKiem.TabIndex = 5;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);

            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLamMoi.Location = new System.Drawing.Point(950, 7);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(95, 30);
            this.btnLamMoi.TabIndex = 6;
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);

            // 
            // dgvLichTrinh
            // 
            this.dgvLichTrinh.AllowUserToAddRows = false;
            this.dgvLichTrinh.AllowUserToDeleteRows = false;
            this.dgvLichTrinh.AllowUserToResizeRows = false;
            this.dgvLichTrinh.BackgroundColor = System.Drawing.Color.White;
            this.dgvLichTrinh.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLichTrinh.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLichTrinh.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvHeaderStyle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dgvHeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dgvHeaderStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvHeaderStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLichTrinh.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvLichTrinh.ColumnHeadersHeight = 36;
            this.dgvLichTrinh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLichTrinh.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaLichTrinh,
            this.colTenSuKien,
            this.colTenTietMuc,
            this.colNgayDienRa,
            this.colKhungGio,
            this.colDiaDiemChiTiet,
            this.colTenDienGia,
            this.colGhiChu});
            dgvRowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRowStyle.BackColor = System.Drawing.Color.White;
            dgvRowStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dgvRowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvRowStyle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dgvRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dgvRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvRowStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLichTrinh.DefaultCellStyle = dgvRowStyle;
            this.dgvLichTrinh.EnableHeadersVisualStyles = false;
            this.dgvLichTrinh.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.dgvLichTrinh.Location = new System.Drawing.Point(20, 120);
            this.dgvLichTrinh.MultiSelect = false;
            this.dgvLichTrinh.Name = "dgvLichTrinh";
            this.dgvLichTrinh.ReadOnly = true;
            this.dgvLichTrinh.RowHeadersVisible = false;
            this.dgvLichTrinh.RowHeadersWidth = 51;
            dgvAltRowStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dgvLichTrinh.RowsDefaultCellStyle = dgvAltRowStyle;
            this.dgvLichTrinh.RowTemplate.Height = 32;
            this.dgvLichTrinh.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLichTrinh.Size = new System.Drawing.Size(1060, 260);
            this.dgvLichTrinh.TabIndex = 2;
            this.dgvLichTrinh.SelectionChanged += new System.EventHandler(this.dgvLichTrinh_SelectionChanged);

            // 
            // colMaLichTrinh
            // 
            this.colMaLichTrinh.DataPropertyName = "MaLichTrinh";
            this.colMaLichTrinh.HeaderText = "Mã lịch";
            this.colMaLichTrinh.Name = "colMaLichTrinh";
            this.colMaLichTrinh.ReadOnly = true;
            this.colMaLichTrinh.Width = 80;

            // 
            // colTenSuKien
            // 
            this.colTenSuKien.DataPropertyName = "TenSuKien";
            this.colTenSuKien.HeaderText = "Sự kiện";
            this.colTenSuKien.Name = "colTenSuKien";
            this.colTenSuKien.ReadOnly = true;
            this.colTenSuKien.Width = 200;

            // 
            // colTenTietMuc
            // 
            this.colTenTietMuc.DataPropertyName = "TenTietMuc";
            this.colTenTietMuc.HeaderText = "Tên chương trình / tiết mục";
            this.colTenTietMuc.Name = "colTenTietMuc";
            this.colTenTietMuc.ReadOnly = true;
            this.colTenTietMuc.Width = 230;

            // 
            // colNgayDienRa
            // 
            this.colNgayDienRa.DataPropertyName = "NgayDienRa";
            this.colNgayDienRa.HeaderText = "Ngày";
            this.colNgayDienRa.Name = "colNgayDienRa";
            this.colNgayDienRa.ReadOnly = true;
            this.colNgayDienRa.Width = 90;

            // 
            // colKhungGio
            // 
            this.colKhungGio.DataPropertyName = "KhungGio";
            this.colKhungGio.HeaderText = "Khung giờ";
            this.colKhungGio.Name = "colKhungGio";
            this.colKhungGio.ReadOnly = true;
            this.colKhungGio.Width = 100;

            // 
            // colDiaDiemChiTiet
            // 
            this.colDiaDiemChiTiet.DataPropertyName = "DiaDiemChiTiet";
            this.colDiaDiemChiTiet.HeaderText = "Phòng / Hội trường";
            this.colDiaDiemChiTiet.Name = "colDiaDiemChiTiet";
            this.colDiaDiemChiTiet.ReadOnly = true;
            this.colDiaDiemChiTiet.Width = 160;

            // 
            // colTenDienGia
            // 
            this.colTenDienGia.DataPropertyName = "TenDienGia";
            this.colTenDienGia.HeaderText = "Diễn giả";
            this.colTenDienGia.Name = "colTenDienGia";
            this.colTenDienGia.ReadOnly = true;
            this.colTenDienGia.Width = 150;

            // 
            // colGhiChu
            // 
            this.colGhiChu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGhiChu.DataPropertyName = "GhiChu";
            this.colGhiChu.HeaderText = "Ghi chú / Mô tả";
            this.colGhiChu.Name = "colGhiChu";
            this.colGhiChu.ReadOnly = true;

            // 
            // grbThongTin
            // 
            this.grbThongTin.BackColor = System.Drawing.Color.White;
            this.grbThongTin.Controls.Add(this.lblKiemTraStatus);
            this.grbThongTin.Controls.Add(this.btnKiemTraXungDot);
            this.grbThongTin.Controls.Add(this.txtGhiChu);
            this.grbThongTin.Controls.Add(this.lblGhiChu);
            this.grbThongTin.Controls.Add(this.txtDiaDiemChiTiet);
            this.grbThongTin.Controls.Add(this.lblDiaDiemChiTiet);
            this.grbThongTin.Controls.Add(this.cboDienGia);
            this.grbThongTin.Controls.Add(this.lblDienGia);
            this.grbThongTin.Controls.Add(this.dtpKetThuc);
            this.grbThongTin.Controls.Add(this.lblKetThuc);
            this.grbThongTin.Controls.Add(this.dtpBatDau);
            this.grbThongTin.Controls.Add(this.lblBatDau);
            this.grbThongTin.Controls.Add(this.txtTenTietMuc);
            this.grbThongTin.Controls.Add(this.lblTenTietMuc);
            this.grbThongTin.Controls.Add(this.cboSuKien);
            this.grbThongTin.Controls.Add(this.lblSuKien);
            this.grbThongTin.Controls.Add(this.txtMaLichTrinh);
            this.grbThongTin.Controls.Add(this.lblMaLichTrinh);
            this.grbThongTin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grbThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.grbThongTin.Location = new System.Drawing.Point(20, 390);
            this.grbThongTin.Name = "grbThongTin";
            this.grbThongTin.Size = new System.Drawing.Size(1060, 240);
            this.grbThongTin.TabIndex = 3;
            this.grbThongTin.TabStop = false;
            this.grbThongTin.Text = "  CHI TIẾT TIẾT MỤC / LỊCH TRÌNH HOẠT ĐỘNG  ";

            // 
            // lblMaLichTrinh
            // 
            this.lblMaLichTrinh.AutoSize = true;
            this.lblMaLichTrinh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaLichTrinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblMaLichTrinh.Location = new System.Drawing.Point(20, 28);
            this.lblMaLichTrinh.Name = "lblMaLichTrinh";
            this.lblMaLichTrinh.Size = new System.Drawing.Size(77, 15);
            this.lblMaLichTrinh.TabIndex = 0;
            this.lblMaLichTrinh.Text = "Mã lịch trình:";

            // 
            // txtMaLichTrinh
            // 
            this.txtMaLichTrinh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMaLichTrinh.Location = new System.Drawing.Point(120, 25);
            this.txtMaLichTrinh.Name = "txtMaLichTrinh";
            this.txtMaLichTrinh.PlaceholderText = "VD: LT009";
            this.txtMaLichTrinh.Size = new System.Drawing.Size(140, 23);
            this.txtMaLichTrinh.TabIndex = 1;

            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblSuKien.Location = new System.Drawing.Point(280, 28);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(99, 15);
            this.lblSuKien.TabIndex = 2;
            this.lblSuKien.Text = "Sự kiện áp dụng:*";

            // 
            // cboSuKien
            // 
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboSuKien.FormattingEnabled = true;
            this.cboSuKien.Location = new System.Drawing.Point(385, 25);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(655, 23);
            this.cboSuKien.TabIndex = 3;

            // 
            // lblTenTietMuc
            // 
            this.lblTenTietMuc.AutoSize = true;
            this.lblTenTietMuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTenTietMuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTenTietMuc.Location = new System.Drawing.Point(20, 63);
            this.lblTenTietMuc.Name = "lblTenTietMuc";
            this.lblTenTietMuc.Size = new System.Drawing.Size(86, 15);
            this.lblTenTietMuc.TabIndex = 4;
            this.lblTenTietMuc.Text = "Tên tiết mục:*";

            // 
            // txtTenTietMuc
            // 
            this.txtTenTietMuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTenTietMuc.Location = new System.Drawing.Point(120, 60);
            this.txtTenTietMuc.Name = "txtTenTietMuc";
            this.txtTenTietMuc.PlaceholderText = "Nhập tiêu đề phiên, hội thảo, keynote hoặc tiết mục hoạt động...";
            this.txtTenTietMuc.Size = new System.Drawing.Size(920, 23);
            this.txtTenTietMuc.TabIndex = 5;

            // 
            // lblBatDau
            // 
            this.lblBatDau.AutoSize = true;
            this.lblBatDau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblBatDau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblBatDau.Location = new System.Drawing.Point(20, 98);
            this.lblBatDau.Name = "lblBatDau";
            this.lblBatDau.Size = new System.Drawing.Size(80, 15);
            this.lblBatDau.TabIndex = 6;
            this.lblBatDau.Text = "Thời gian từ:*";

            // 
            // dtpBatDau
            // 
            this.dtpBatDau.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpBatDau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBatDau.Location = new System.Drawing.Point(120, 95);
            this.dtpBatDau.Name = "dtpBatDau";
            this.dtpBatDau.Size = new System.Drawing.Size(180, 23);
            this.dtpBatDau.TabIndex = 7;

            // 
            // lblKetThuc
            // 
            this.lblKetThuc.AutoSize = true;
            this.lblKetThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKetThuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblKetThuc.Location = new System.Drawing.Point(315, 98);
            this.lblKetThuc.Name = "lblKetThuc";
            this.lblKetThuc.Size = new System.Drawing.Size(61, 15);
            this.lblKetThuc.TabIndex = 8;
            this.lblKetThuc.Text = "Đến ngày:*";

            // 
            // dtpKetThuc
            // 
            this.dtpKetThuc.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpKetThuc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpKetThuc.Location = new System.Drawing.Point(385, 95);
            this.dtpKetThuc.Name = "dtpKetThuc";
            this.dtpKetThuc.Size = new System.Drawing.Size(180, 23);
            this.dtpKetThuc.TabIndex = 9;

            // 
            // lblDienGia
            // 
            this.lblDienGia.AutoSize = true;
            this.lblDienGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDienGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblDienGia.Location = new System.Drawing.Point(585, 98);
            this.lblDienGia.Name = "lblDienGia";
            this.lblDienGia.Size = new System.Drawing.Size(54, 15);
            this.lblDienGia.TabIndex = 10;
            this.lblDienGia.Text = "Diễn giả:";

            // 
            // cboDienGia
            // 
            this.cboDienGia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDienGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboDienGia.FormattingEnabled = true;
            this.cboDienGia.Location = new System.Drawing.Point(650, 95);
            this.cboDienGia.Name = "cboDienGia";
            this.cboDienGia.Size = new System.Drawing.Size(390, 23);
            this.cboDienGia.TabIndex = 11;

            // 
            // lblDiaDiemChiTiet
            // 
            this.lblDiaDiemChiTiet.AutoSize = true;
            this.lblDiaDiemChiTiet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDiaDiemChiTiet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblDiaDiemChiTiet.Location = new System.Drawing.Point(20, 133);
            this.lblDiaDiemChiTiet.Name = "lblDiaDiemChiTiet";
            this.lblDiaDiemChiTiet.Size = new System.Drawing.Size(95, 15);
            this.lblDiaDiemChiTiet.TabIndex = 12;
            this.lblDiaDiemChiTiet.Text = "Phòng/Hội trường:";

            // 
            // txtDiaDiemChiTiet
            // 
            this.txtDiaDiemChiTiet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDiaDiemChiTiet.Location = new System.Drawing.Point(120, 130);
            this.txtDiaDiemChiTiet.Name = "txtDiaDiemChiTiet";
            this.txtDiaDiemChiTiet.PlaceholderText = "VD: Hội trường Grand Ballroom, Phòng Lab Kỹ thuật A...";
            this.txtDiaDiemChiTiet.Size = new System.Drawing.Size(445, 23);
            this.txtDiaDiemChiTiet.TabIndex = 13;

            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblGhiChu.Location = new System.Drawing.Point(585, 133);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(51, 15);
            this.lblGhiChu.TabIndex = 14;
            this.lblGhiChu.Text = "Ghi chú:";

            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGhiChu.Location = new System.Drawing.Point(650, 130);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.PlaceholderText = "Ghi chú nội dung, đạo cụ hoặc chuẩn bị kỹ thuật...";
            this.txtGhiChu.Size = new System.Drawing.Size(390, 23);
            this.txtGhiChu.TabIndex = 15;

            // 
            // btnKiemTraXungDot
            // 
            this.btnKiemTraXungDot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnKiemTraXungDot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKiemTraXungDot.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnKiemTraXungDot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.btnKiemTraXungDot.Location = new System.Drawing.Point(120, 168);
            this.btnKiemTraXungDot.Name = "btnKiemTraXungDot";
            this.btnKiemTraXungDot.Size = new System.Drawing.Size(210, 32);
            this.btnKiemTraXungDot.TabIndex = 16;
            this.btnKiemTraXungDot.Text = "⚡ Kiểm tra xung đột BR-05 & BR-06";
            this.btnKiemTraXungDot.UseVisualStyleBackColor = false;
            this.btnKiemTraXungDot.Click += new System.EventHandler(this.btnKiemTraXungDot_Click);

            // 
            // lblKiemTraStatus
            // 
            this.lblKiemTraStatus.AutoSize = true;
            this.lblKiemTraStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblKiemTraStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblKiemTraStatus.Location = new System.Drawing.Point(340, 176);
            this.lblKiemTraStatus.Name = "lblKiemTraStatus";
            this.lblKiemTraStatus.Size = new System.Drawing.Size(316, 15);
            this.lblKiemTraStatus.TabIndex = 17;
            this.lblKiemTraStatus.Text = "Bấm để kiểm tra giao thoa phòng & trùng lịch diễn giả tức thì";

            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlButtons.Controls.Add(this.btnHuy);
            this.pnlButtons.Controls.Add(this.btnLuu);
            this.pnlButtons.Controls.Add(this.btnXuatExcel);
            this.pnlButtons.Controls.Add(this.btnXoa);
            this.pnlButtons.Controls.Add(this.btnSua);
            this.pnlButtons.Controls.Add(this.btnThem);
            this.pnlButtons.Location = new System.Drawing.Point(20, 642);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1060, 48);
            this.pnlButtons.TabIndex = 4;

            // 
            // btnThem
            // 
            this.btnThem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnThem.Location = new System.Drawing.Point(0, 6);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(110, 36);
            this.btnThem.TabIndex = 0;
            this.btnThem.Text = "➕ Thêm (F2)";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            // 
            // btnSua
            // 
            this.btnSua.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSua.Location = new System.Drawing.Point(120, 6);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(110, 36);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "✏️ Sửa (F3)";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);

            // 
            // btnXoa
            // 
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnXoa.Location = new System.Drawing.Point(240, 6);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(110, 36);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "🗑️ Xóa (Del)";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnXuatExcel.Location = new System.Drawing.Point(360, 6);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(120, 36);
            this.btnXuatExcel.TabIndex = 3;
            this.btnXuatExcel.Text = "📊 Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);

            // 
            // btnLuu
            // 
            this.btnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLuu.Location = new System.Drawing.Point(825, 6);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(110, 36);
            this.btnLuu.TabIndex = 4;
            this.btnLuu.Text = "💾 Lưu (Ctrl+S)";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            // 
            // btnHuy
            // 
            this.btnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHuy.Location = new System.Drawing.Point(945, 6);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(110, 36);
            this.btnHuy.TabIndex = 5;
            this.btnHuy.Text = "❌ Hủy (Esc)";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // 
            // errBaoLoi
            // 
            this.errBaoLoi.ContainerControl = this;

            // 
            // FrmSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 705);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.grbThongTin);
            this.Controls.Add(this.dgvLichTrinh);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmSchedule";
            this.Text = "F08 – Quản lý lịch trình / chương trình tiết mục";
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSchedule_KeyDown);
            this.Load += new System.EventHandler(this.FrmSchedule_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichTrinh)).EndInit();
            this.grbThongTin.ResumeLayout(false);
            this.grbThongTin.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
