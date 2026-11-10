namespace EventManagement.GUI.Forms
{
    partial class FrmFeedback
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel pnlKpi;
        private System.Windows.Forms.Label lblKpiDiemTbTitle;
        private System.Windows.Forms.Label lblKpiDiemTbValue;
        private System.Windows.Forms.Label lblKpiTongPhTitle;
        private System.Windows.Forms.Label lblKpiTongPhValue;
        private System.Windows.Forms.Label lblKpiHaiLongTitle;
        private System.Windows.Forms.Label lblKpiHaiLongValue;
        private System.Windows.Forms.Label lblKpiDanhGiaXauTitle;
        private System.Windows.Forms.Label lblKpiDanhGiaXauValue;

        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblLocSuKien;
        private System.Windows.Forms.ComboBox cboLocSuKien;
        private System.Windows.Forms.Label lblLocMucSao;
        private System.Windows.Forms.ComboBox cboLocMucSao;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnLamMoi;

        private System.Windows.Forms.DataGridView dgvPhanHoi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaPhanHoi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSuKien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenNguoiThamGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoSaoText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNoiDungPhanHoi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayDanhGia;

        private System.Windows.Forms.GroupBox grbThongTin;
        private System.Windows.Forms.Label lblMaPhanHoi;
        private System.Windows.Forms.TextBox txtMaPhanHoi;
        private System.Windows.Forms.Label lblSuKien;
        private System.Windows.Forms.ComboBox cboSuKien;
        private System.Windows.Forms.Label lblNguoiThamGia;
        private System.Windows.Forms.ComboBox cboNguoiThamGia;

        private System.Windows.Forms.Label lblDiemDanhGia;
        private System.Windows.Forms.ComboBox cboDiemDanhGia;
        private System.Windows.Forms.Label lblNgayDanhGia;
        private System.Windows.Forms.DateTimePicker dtpNgayDanhGia;

        private System.Windows.Forms.Label lblNoiDung;
        private System.Windows.Forms.TextBox txtNoiDungPhanHoi;

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

            this.pnlKpi = new System.Windows.Forms.Panel();
            this.lblKpiDiemTbTitle = new System.Windows.Forms.Label();
            this.lblKpiDiemTbValue = new System.Windows.Forms.Label();
            this.lblKpiTongPhTitle = new System.Windows.Forms.Label();
            this.lblKpiTongPhValue = new System.Windows.Forms.Label();
            this.lblKpiHaiLongTitle = new System.Windows.Forms.Label();
            this.lblKpiHaiLongValue = new System.Windows.Forms.Label();
            this.lblKpiDanhGiaXauTitle = new System.Windows.Forms.Label();
            this.lblKpiDanhGiaXauValue = new System.Windows.Forms.Label();

            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblLocSuKien = new System.Windows.Forms.Label();
            this.cboLocSuKien = new System.Windows.Forms.ComboBox();
            this.lblLocMucSao = new System.Windows.Forms.Label();
            this.cboLocMucSao = new System.Windows.Forms.ComboBox();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnLamMoi = new System.Windows.Forms.Button();

            this.dgvPhanHoi = new System.Windows.Forms.DataGridView();
            this.colMaPhanHoi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenSuKien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenNguoiThamGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoSaoText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNoiDungPhanHoi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayDanhGia = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.grbThongTin = new System.Windows.Forms.GroupBox();
            this.lblMaPhanHoi = new System.Windows.Forms.Label();
            this.txtMaPhanHoi = new System.Windows.Forms.TextBox();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.cboSuKien = new System.Windows.Forms.ComboBox();
            this.lblNguoiThamGia = new System.Windows.Forms.Label();
            this.cboNguoiThamGia = new System.Windows.Forms.ComboBox();

            this.lblDiemDanhGia = new System.Windows.Forms.Label();
            this.cboDiemDanhGia = new System.Windows.Forms.ComboBox();
            this.lblNgayDanhGia = new System.Windows.Forms.Label();
            this.dtpNgayDanhGia = new System.Windows.Forms.DateTimePicker();

            this.lblNoiDung = new System.Windows.Forms.Label();
            this.txtNoiDungPhanHoi = new System.Windows.Forms.TextBox();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();

            this.errBaoLoi = new System.Windows.Forms.ErrorProvider(this.components);

            this.pnlHeader.SuspendLayout();
            this.pnlKpi.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhanHoi)).BeginInit();
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
            this.lblTitle.Location = new System.Drawing.Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(420, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "⭐ QUẢN LÝ ĐÁNH GIÁ VÀ PHẢN HỒI";

            // 
            // pnlKpi
            // 
            this.pnlKpi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.pnlKpi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi.Controls.Add(this.lblKpiDanhGiaXauValue);
            this.pnlKpi.Controls.Add(this.lblKpiDanhGiaXauTitle);
            this.pnlKpi.Controls.Add(this.lblKpiHaiLongValue);
            this.pnlKpi.Controls.Add(this.lblKpiHaiLongTitle);
            this.pnlKpi.Controls.Add(this.lblKpiTongPhValue);
            this.pnlKpi.Controls.Add(this.lblKpiTongPhTitle);
            this.pnlKpi.Controls.Add(this.lblKpiDiemTbValue);
            this.pnlKpi.Controls.Add(this.lblKpiDiemTbTitle);
            this.pnlKpi.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpi.Location = new System.Drawing.Point(0, 56);
            this.pnlKpi.Name = "pnlKpi";
            this.pnlKpi.Size = new System.Drawing.Size(1060, 44);
            this.pnlKpi.TabIndex = 1;

            // 
            // lblKpiDiemTbTitle
            // 
            this.lblKpiDiemTbTitle.AutoSize = true;
            this.lblKpiDiemTbTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKpiDiemTbTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblKpiDiemTbTitle.Location = new System.Drawing.Point(15, 13);
            this.lblKpiDiemTbTitle.Name = "lblKpiDiemTbTitle";
            this.lblKpiDiemTbTitle.Size = new System.Drawing.Size(142, 15);
            this.lblKpiDiemTbTitle.TabIndex = 0;
            this.lblKpiDiemTbTitle.Text = "Điểm đánh giá TB (CSAT):";

            // 
            // lblKpiDiemTbValue
            // 
            this.lblKpiDiemTbValue.AutoSize = true;
            this.lblKpiDiemTbValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpiDiemTbValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblKpiDiemTbValue.Location = new System.Drawing.Point(165, 10);
            this.lblKpiDiemTbValue.Name = "lblKpiDiemTbValue";
            this.lblKpiDiemTbValue.Size = new System.Drawing.Size(81, 20);
            this.lblKpiDiemTbValue.TabIndex = 1;
            this.lblKpiDiemTbValue.Text = "4.7 / 5.0 ★";

            // 
            // lblKpiTongPhTitle
            // 
            this.lblKpiTongPhTitle.AutoSize = true;
            this.lblKpiTongPhTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKpiTongPhTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblKpiTongPhTitle.Location = new System.Drawing.Point(275, 13);
            this.lblKpiTongPhTitle.Name = "lblKpiTongPhTitle";
            this.lblKpiTongPhTitle.Size = new System.Drawing.Size(117, 15);
            this.lblKpiTongPhTitle.TabIndex = 2;
            this.lblKpiTongPhTitle.Text = "Tổng lượt khảo sát:";

            // 
            // lblKpiTongPhValue
            // 
            this.lblKpiTongPhValue.AutoSize = true;
            this.lblKpiTongPhValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpiTongPhValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblKpiTongPhValue.Location = new System.Drawing.Point(400, 10);
            this.lblKpiTongPhValue.Name = "lblKpiTongPhValue";
            this.lblKpiTongPhValue.Size = new System.Drawing.Size(73, 20);
            this.lblKpiTongPhValue.TabIndex = 3;
            this.lblKpiTongPhValue.Text = "5 ý kiến";

            // 
            // lblKpiHaiLongTitle
            // 
            this.lblKpiHaiLongTitle.AutoSize = true;
            this.lblKpiHaiLongTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKpiHaiLongTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblKpiHaiLongTitle.Location = new System.Drawing.Point(500, 13);
            this.lblKpiHaiLongTitle.Name = "lblKpiHaiLongTitle";
            this.lblKpiHaiLongTitle.Size = new System.Drawing.Size(147, 15);
            this.lblKpiHaiLongTitle.TabIndex = 4;
            this.lblKpiHaiLongTitle.Text = "Tỷ lệ hài lòng (4 - 5 sao):";

            // 
            // lblKpiHaiLongValue
            // 
            this.lblKpiHaiLongValue.AutoSize = true;
            this.lblKpiHaiLongValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpiHaiLongValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblKpiHaiLongValue.Location = new System.Drawing.Point(655, 10);
            this.lblKpiHaiLongValue.Name = "lblKpiHaiLongValue";
            this.lblKpiHaiLongValue.Size = new System.Drawing.Size(59, 20);
            this.lblKpiHaiLongValue.TabIndex = 5;
            this.lblKpiHaiLongValue.Text = "100 %";

            // 
            // lblKpiDanhGiaXauTitle
            // 
            this.lblKpiDanhGiaXauTitle.AutoSize = true;
            this.lblKpiDanhGiaXauTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblKpiDanhGiaXauTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblKpiDanhGiaXauTitle.Location = new System.Drawing.Point(740, 13);
            this.lblKpiDanhGiaXauTitle.Name = "lblKpiDanhGiaXauTitle";
            this.lblKpiDanhGiaXauTitle.Size = new System.Drawing.Size(155, 15);
            this.lblKpiDanhGiaXauTitle.TabIndex = 6;
            this.lblKpiDanhGiaXauTitle.Text = "Tỷ lệ đánh giá xấu (1 - 2 sao):";

            // 
            // lblKpiDanhGiaXauValue
            // 
            this.lblKpiDanhGiaXauValue.AutoSize = true;
            this.lblKpiDanhGiaXauValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKpiDanhGiaXauValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblKpiDanhGiaXauValue.Location = new System.Drawing.Point(905, 10);
            this.lblKpiDanhGiaXauValue.Name = "lblKpiDanhGiaXauValue";
            this.lblKpiDanhGiaXauValue.Size = new System.Drawing.Size(45, 20);
            this.lblKpiDanhGiaXauValue.TabIndex = 7;
            this.lblKpiDanhGiaXauValue.Text = "0 %";

            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.btnLamMoi);
            this.pnlFilter.Controls.Add(this.txtTimKiem);
            this.pnlFilter.Controls.Add(this.lblTimKiem);
            this.pnlFilter.Controls.Add(this.cboLocMucSao);
            this.pnlFilter.Controls.Add(this.lblLocMucSao);
            this.pnlFilter.Controls.Add(this.cboLocSuKien);
            this.pnlFilter.Controls.Add(this.lblLocSuKien);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 100);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1060, 50);
            this.pnlFilter.TabIndex = 2;

            // 
            // lblLocSuKien
            // 
            this.lblLocSuKien.AutoSize = true;
            this.lblLocSuKien.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocSuKien.Location = new System.Drawing.Point(12, 15);
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
            this.cboLocSuKien.Location = new System.Drawing.Point(75, 12);
            this.cboLocSuKien.Name = "cboLocSuKien";
            this.cboLocSuKien.Size = new System.Drawing.Size(205, 23);
            this.cboLocSuKien.TabIndex = 1;
            this.cboLocSuKien.SelectedIndexChanged += new System.EventHandler(this.cboLocSuKien_SelectedIndexChanged);

            // 
            // lblLocMucSao
            // 
            this.lblLocMucSao.AutoSize = true;
            this.lblLocMucSao.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLocMucSao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblLocMucSao.Location = new System.Drawing.Point(295, 15);
            this.lblLocMucSao.Name = "lblLocMucSao";
            this.lblLocMucSao.Size = new System.Drawing.Size(56, 15);
            this.lblLocMucSao.TabIndex = 2;
            this.lblLocMucSao.Text = "Mức sao:";

            // 
            // cboLocMucSao
            // 
            this.cboLocMucSao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocMucSao.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLocMucSao.FormattingEnabled = true;
            this.cboLocMucSao.Items.AddRange(new object[] {
            "Tất cả",
            "5 sao (★★★★★)",
            "4 sao (★★★★☆)",
            "3 sao (★★★☆☆)",
            "2 sao (★★☆☆☆)",
            "1 sao (★☆☆☆☆)"});
            this.cboLocMucSao.Location = new System.Drawing.Point(365, 12);
            this.cboLocMucSao.Name = "cboLocMucSao";
            this.cboLocMucSao.Size = new System.Drawing.Size(125, 23);
            this.cboLocMucSao.TabIndex = 3;
            this.cboLocMucSao.SelectedIndexChanged += new System.EventHandler(this.cboLocMucSao_SelectedIndexChanged);

            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTimKiem.Location = new System.Drawing.Point(505, 15);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(60, 15);
            this.lblTimKiem.TabIndex = 4;
            this.lblTimKiem.Text = "Tìm kiếm:";

            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTimKiem.Location = new System.Drawing.Point(575, 12);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Mã, người tham gia, nội dung...";
            this.txtTimKiem.Size = new System.Drawing.Size(195, 23);
            this.txtTimKiem.TabIndex = 5;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);

            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLamMoi.Location = new System.Drawing.Point(915, 9);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(125, 30);
            this.btnLamMoi.TabIndex = 6;
            this.btnLamMoi.Text = "🔄 Làm mới (F5)";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(120, 30);
            this.btnLamMoi.TabIndex = 6;
            this.btnLamMoi.Text = "🔄 Làm mới (F5)";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);

            // 
            // dgvPhanHoi
            // 
            this.dgvPhanHoi.AllowUserToAddRows = false;
            this.dgvPhanHoi.AllowUserToDeleteRows = false;
            this.dgvPhanHoi.AllowUserToResizeRows = false;
            this.dgvPhanHoi.BackgroundColor = System.Drawing.Color.White;
            this.dgvPhanHoi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvPhanHoi.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPhanHoi.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvHeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dgvHeaderStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.dgvPhanHoi.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvPhanHoi.ColumnHeadersHeight = 34;
            this.dgvPhanHoi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dgvPhanHoi.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaPhanHoi,
            this.colTenSuKien,
            this.colTenNguoiThamGia,
            this.colEmail,
            this.colSoSaoText,
            this.colNoiDungPhanHoi,
            this.colNgayDanhGia});

            dgvRowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRowStyle.BackColor = System.Drawing.Color.White;
            dgvRowStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dgvRowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dgvRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dgvRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.dgvPhanHoi.DefaultCellStyle = dgvRowStyle;

            dgvAltRowStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            dgvAltRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dgvAltRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.dgvPhanHoi.AlternatingRowsDefaultCellStyle = dgvAltRowStyle;

            this.dgvPhanHoi.EnableHeadersVisualStyles = false;
            this.dgvPhanHoi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPhanHoi.Location = new System.Drawing.Point(20, 186);
            this.dgvPhanHoi.MultiSelect = false;
            this.dgvPhanHoi.Name = "dgvPhanHoi";
            this.dgvPhanHoi.ReadOnly = true;
            this.dgvPhanHoi.RowHeadersVisible = false;
            this.dgvPhanHoi.RowTemplate.Height = 30;
            this.dgvPhanHoi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhanHoi.Size = new System.Drawing.Size(1020, 152);
            this.dgvPhanHoi.TabIndex = 3;
            this.dgvPhanHoi.SelectionChanged += new System.EventHandler(this.dgvPhanHoi_SelectionChanged);

            // 
            // colMaPhanHoi
            // 
            this.colMaPhanHoi.DataPropertyName = "MaPhanHoi";
            this.colMaPhanHoi.HeaderText = "Mã PH";
            this.colMaPhanHoi.Name = "colMaPhanHoi";
            this.colMaPhanHoi.ReadOnly = true;
            this.colMaPhanHoi.Width = 85;
            this.colMaPhanHoi.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colTenSuKien
            // 
            this.colTenSuKien.DataPropertyName = "TenSuKien";
            this.colTenSuKien.HeaderText = "Sự kiện";
            this.colTenSuKien.Name = "colTenSuKien";
            this.colTenSuKien.ReadOnly = true;
            this.colTenSuKien.Width = 220;

            // 
            // colTenNguoiThamGia
            // 
            this.colTenNguoiThamGia.DataPropertyName = "TenNguoiThamGia";
            this.colTenNguoiThamGia.HeaderText = "Người đánh giá";
            this.colTenNguoiThamGia.Name = "colTenNguoiThamGia";
            this.colTenNguoiThamGia.ReadOnly = true;
            this.colTenNguoiThamGia.Width = 150;

            // 
            // colEmail
            // 
            this.colEmail.DataPropertyName = "Email";
            this.colEmail.HeaderText = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            this.colEmail.Width = 150;

            // 
            // colSoSaoText
            // 
            this.colSoSaoText.DataPropertyName = "SoSaoText";
            this.colSoSaoText.HeaderText = "Mức đánh giá";
            this.colSoSaoText.Name = "colSoSaoText";
            this.colSoSaoText.ReadOnly = true;
            this.colSoSaoText.Width = 120;
            this.colSoSaoText.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colNoiDungPhanHoi
            // 
            this.colNoiDungPhanHoi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNoiDungPhanHoi.DataPropertyName = "NoiDungPhanHoi";
            this.colNoiDungPhanHoi.HeaderText = "Ý kiến góp ý / Nhận xét";
            this.colNoiDungPhanHoi.Name = "colNoiDungPhanHoi";
            this.colNoiDungPhanHoi.ReadOnly = true;

            // 
            // colNgayDanhGia
            // 
            this.colNgayDanhGia.DataPropertyName = "NgayDanhGiaFormatted";
            this.colNgayDanhGia.HeaderText = "Thời gian gửi";
            this.colNgayDanhGia.Name = "colNgayDanhGia";
            this.colNgayDanhGia.ReadOnly = true;
            this.colNgayDanhGia.Width = 125;
            this.colNgayDanhGia.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // grbThongTin
            // 
            this.grbThongTin.BackColor = System.Drawing.Color.White;
            this.grbThongTin.Controls.Add(this.txtNoiDungPhanHoi);
            this.grbThongTin.Controls.Add(this.lblNoiDung);
            this.grbThongTin.Controls.Add(this.dtpNgayDanhGia);
            this.grbThongTin.Controls.Add(this.lblNgayDanhGia);
            this.grbThongTin.Controls.Add(this.cboDiemDanhGia);
            this.grbThongTin.Controls.Add(this.lblDiemDanhGia);
            this.grbThongTin.Controls.Add(this.cboNguoiThamGia);
            this.grbThongTin.Controls.Add(this.lblNguoiThamGia);
            this.grbThongTin.Controls.Add(this.cboSuKien);
            this.grbThongTin.Controls.Add(this.lblSuKien);
            this.grbThongTin.Controls.Add(this.txtMaPhanHoi);
            this.grbThongTin.Controls.Add(this.lblMaPhanHoi);
            this.grbThongTin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grbThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.grbThongTin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grbThongTin.Location = new System.Drawing.Point(20, 348);
            this.grbThongTin.Name = "grbThongTin";
            this.grbThongTin.Size = new System.Drawing.Size(1020, 205);
            this.grbThongTin.TabIndex = 4;
            this.grbThongTin.TabStop = false;
            this.grbThongTin.Text = "NỘI DUNG CHI TIẾT KHẢO SÁT & ĐÁNH GIÁ";

            // 
            // lblMaPhanHoi
            // 
            this.lblMaPhanHoi.AutoSize = true;
            this.lblMaPhanHoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaPhanHoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblMaPhanHoi.Location = new System.Drawing.Point(20, 32);
            this.lblMaPhanHoi.Name = "lblMaPhanHoi";
            this.lblMaPhanHoi.Size = new System.Drawing.Size(91, 15);
            this.lblMaPhanHoi.TabIndex = 0;
            this.lblMaPhanHoi.Text = "Mã phản hồi (*):";

            // 
            // txtMaPhanHoi
            // 
            this.txtMaPhanHoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMaPhanHoi.Location = new System.Drawing.Point(150, 29);
            this.txtMaPhanHoi.Name = "txtMaPhanHoi";
            this.txtMaPhanHoi.Size = new System.Drawing.Size(110, 23);
            this.txtMaPhanHoi.TabIndex = 1;

            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblSuKien.Location = new System.Drawing.Point(275, 32);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(65, 15);
            this.lblSuKien.TabIndex = 2;
            this.lblSuKien.Text = "Sự kiện (*):";

            // 
            // cboSuKien
            // 
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboSuKien.FormattingEnabled = true;
            this.cboSuKien.Location = new System.Drawing.Point(355, 29);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(300, 23);
            this.cboSuKien.TabIndex = 3;

            // 
            // lblNguoiThamGia
            // 
            this.lblNguoiThamGia.AutoSize = true;
            this.lblNguoiThamGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNguoiThamGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblNguoiThamGia.Location = new System.Drawing.Point(675, 32);
            this.lblNguoiThamGia.Name = "lblNguoiThamGia";
            this.lblNguoiThamGia.Size = new System.Drawing.Size(107, 15);
            this.lblNguoiThamGia.TabIndex = 4;
            this.lblNguoiThamGia.Text = "Người đánh giá (*):";

            // 
            // cboNguoiThamGia
            // 
            this.cboNguoiThamGia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNguoiThamGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboNguoiThamGia.FormattingEnabled = true;
            this.cboNguoiThamGia.Location = new System.Drawing.Point(805, 29);
            this.cboNguoiThamGia.Name = "cboNguoiThamGia";
            this.cboNguoiThamGia.Size = new System.Drawing.Size(195, 23);
            this.cboNguoiThamGia.TabIndex = 5;

            // 
            // lblDiemDanhGia
            // 
            this.lblDiemDanhGia.AutoSize = true;
            this.lblDiemDanhGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDiemDanhGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblDiemDanhGia.Location = new System.Drawing.Point(20, 70);
            this.lblDiemDanhGia.Name = "lblDiemDanhGia";
            this.lblDiemDanhGia.Size = new System.Drawing.Size(95, 15);
            this.lblDiemDanhGia.TabIndex = 6;
            this.lblDiemDanhGia.Text = "Mức độ hài lòng:";

            // 
            // cboDiemDanhGia
            // 
            this.cboDiemDanhGia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDiemDanhGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboDiemDanhGia.FormattingEnabled = true;
            this.cboDiemDanhGia.Items.AddRange(new object[] {
            "★★★★★ (5 sao - Rất hài lòng)",
            "★★★★☆ (4 sao - Hài lòng)",
            "★★★☆☆ (3 sao - Bình thường)",
            "★★☆☆☆ (2 sao - Chưa hài lòng)",
            "★☆☆☆☆ (1 sao - Thất vọng)"});
            this.cboDiemDanhGia.Location = new System.Drawing.Point(135, 67);
            this.cboDiemDanhGia.Name = "cboDiemDanhGia";
            this.cboDiemDanhGia.Size = new System.Drawing.Size(235, 23);
            this.cboDiemDanhGia.TabIndex = 7;

            // 
            // lblNgayDanhGia
            // 
            this.lblNgayDanhGia.AutoSize = true;
            this.lblNgayDanhGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNgayDanhGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblNgayDanhGia.Location = new System.Drawing.Point(395, 70);
            this.lblNgayDanhGia.Name = "lblNgayDanhGia";
            this.lblNgayDanhGia.Size = new System.Drawing.Size(78, 15);
            this.lblNgayDanhGia.TabIndex = 8;
            this.lblNgayDanhGia.Text = "Thời gian gửi:";

            // 
            // dtpNgayDanhGia
            // 
            this.dtpNgayDanhGia.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpNgayDanhGia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpNgayDanhGia.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayDanhGia.Location = new System.Drawing.Point(495, 67);
            this.dtpNgayDanhGia.Name = "dtpNgayDanhGia";
            this.dtpNgayDanhGia.Size = new System.Drawing.Size(160, 23);
            this.dtpNgayDanhGia.TabIndex = 9;

            // 
            // lblNoiDung
            // 
            this.lblNoiDung.AutoSize = true;
            this.lblNoiDung.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNoiDung.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblNoiDung.Location = new System.Drawing.Point(20, 108);
            this.lblNoiDung.Name = "lblNoiDung";
            this.lblNoiDung.Size = new System.Drawing.Size(107, 15);
            this.lblNoiDung.TabIndex = 10;
            this.lblNoiDung.Text = "Ý kiến góp ý:";

            // 
            // txtNoiDungPhanHoi
            // 
            this.txtNoiDungPhanHoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNoiDungPhanHoi.Location = new System.Drawing.Point(135, 105);
            this.txtNoiDungPhanHoi.Multiline = true;
            this.txtNoiDungPhanHoi.Name = "txtNoiDungPhanHoi";
            this.txtNoiDungPhanHoi.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNoiDungPhanHoi.Size = new System.Drawing.Size(865, 85);
            this.txtNoiDungPhanHoi.TabIndex = 11;

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
            this.pnlButtons.TabIndex = 5;

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
            // FrmFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 640);
            this.Controls.Add(this.dgvPhanHoi);
            this.Controls.Add(this.grbThongTin);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlKpi);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.KeyPreview = true;
            this.MaximizeBox = true;
            this.Name = "FrmFeedback";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.Load += new System.EventHandler(this.FrmFeedback_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmFeedback_KeyDown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlKpi.ResumeLayout(false);
            this.pnlKpi.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhanHoi)).EndInit();
            this.grbThongTin.ResumeLayout(false);
            this.grbThongTin.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
