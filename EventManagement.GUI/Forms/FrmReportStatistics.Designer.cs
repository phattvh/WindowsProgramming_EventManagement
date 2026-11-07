namespace EventManagement.GUI.Forms
{
    partial class FrmReportStatistics
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
            this.pnlTopBanner = new System.Windows.Forms.Panel();
            this.lblBannerSubtitle = new System.Windows.Forms.Label();
            this.lblBannerTitle = new System.Windows.Forms.Label();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.pnlReportType = new System.Windows.Forms.Panel();
            this.radTongHop = new System.Windows.Forms.RadioButton();
            this.radVeDienTu = new System.Windows.Forms.RadioButton();
            this.radQuyetToan = new System.Windows.Forms.RadioButton();
            this.lblLoaiBaoCao = new System.Windows.Forms.Label();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnXem = new System.Windows.Forms.Button();
            this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
            this.lblDenNgay = new System.Windows.Forms.Label();
            this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
            this.lblTuNgay = new System.Windows.Forms.Label();
            this.cboSuKien = new System.Windows.Forms.ComboBox();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.pnlKpiBottom = new System.Windows.Forms.Panel();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.lblChenhLech = new System.Windows.Forms.Label();
            this.lblTongChi = new System.Windows.Forms.Label();
            this.lblTongThu = new System.Windows.Forms.Label();
            this.tabReport = new System.Windows.Forms.TabControl();
            this.tabData = new System.Windows.Forms.TabPage();
            this.dgvReportData = new System.Windows.Forms.DataGridView();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.pnlPreviewToolbar = new System.Windows.Forms.Panel();
            this.btnPrintFromPreview = new System.Windows.Forms.Button();
            this.cboZoom = new System.Windows.Forms.ComboBox();
            this.lblZoom = new System.Windows.Forms.Label();
            this.printPreviewControl = new System.Windows.Forms.PrintPreviewControl();
            this.pnlTopBanner.SuspendLayout();
            this.grpFilter.SuspendLayout();
            this.pnlReportType.SuspendLayout();
            this.pnlKpiBottom.SuspendLayout();
            this.tabReport.SuspendLayout();
            this.tabData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportData)).BeginInit();
            this.tabPreview.SuspendLayout();
            this.pnlPreviewToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBanner
            // 
            this.pnlTopBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.pnlTopBanner.Controls.Add(this.lblBannerSubtitle);
            this.pnlTopBanner.Controls.Add(this.lblBannerTitle);
            this.pnlTopBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBanner.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBanner.Name = "pnlTopBanner";
            this.pnlTopBanner.Size = new System.Drawing.Size(1104, 65);
            this.pnlTopBanner.TabIndex = 0;
            // 
            // lblBannerSubtitle
            // 
            this.lblBannerSubtitle.AutoSize = true;
            this.lblBannerSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblBannerSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.lblBannerSubtitle.Location = new System.Drawing.Point(16, 38);
            this.lblBannerSubtitle.Name = "lblBannerSubtitle";
            this.lblBannerSubtitle.Size = new System.Drawing.Size(465, 15);
            this.lblBannerSubtitle.TabIndex = 1;
            this.lblBannerSubtitle.Text = "Hệ thống tổng hợp quyết toán tài chính, quản lý vé điện tử và chỉ số hoạt động sự kiện";
            // 
            // lblBannerTitle
            // 
            this.lblBannerTitle.AutoSize = true;
            this.lblBannerTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblBannerTitle.ForeColor = System.Drawing.Color.White;
            this.lblBannerTitle.Location = new System.Drawing.Point(15, 9);
            this.lblBannerTitle.Name = "lblBannerTitle";
            this.lblBannerTitle.Size = new System.Drawing.Size(438, 25);
            this.lblBannerTitle.TabIndex = 0;
            this.lblBannerTitle.Text = "F19 - BÁO CÁO & THỐNG KÊ TỔNG HỢP SỰ KIỆN";
            // 
            // grpFilter
            // 
            this.grpFilter.BackColor = System.Drawing.Color.White;
            this.grpFilter.Controls.Add(this.pnlReportType);
            this.grpFilter.Controls.Add(this.lblLoaiBaoCao);
            this.grpFilter.Controls.Add(this.btnLamMoi);
            this.grpFilter.Controls.Add(this.btnXuatExcel);
            this.grpFilter.Controls.Add(this.btnIn);
            this.grpFilter.Controls.Add(this.btnXem);
            this.grpFilter.Controls.Add(this.dtpDenNgay);
            this.grpFilter.Controls.Add(this.lblDenNgay);
            this.grpFilter.Controls.Add(this.dtpTuNgay);
            this.grpFilter.Controls.Add(this.lblTuNgay);
            this.grpFilter.Controls.Add(this.cboSuKien);
            this.grpFilter.Controls.Add(this.lblSuKien);
            this.grpFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.grpFilter.Location = new System.Drawing.Point(0, 65);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(1104, 100);
            this.grpFilter.TabIndex = 1;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "Bộ lọc & Tùy chọn báo cáo";
            // 
            // pnlReportType
            // 
            this.pnlReportType.Controls.Add(this.radTongHop);
            this.pnlReportType.Controls.Add(this.radVeDienTu);
            this.pnlReportType.Controls.Add(this.radQuyetToan);
            this.pnlReportType.Location = new System.Drawing.Point(90, 60);
            this.pnlReportType.Name = "pnlReportType";
            this.pnlReportType.Size = new System.Drawing.Size(560, 30);
            this.pnlReportType.TabIndex = 11;
            // 
            // radTongHop
            // 
            this.radTongHop.AutoSize = true;
            this.radTongHop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radTongHop.Location = new System.Drawing.Point(365, 5);
            this.radTongHop.Name = "radTongHop";
            this.radTongHop.Size = new System.Drawing.Size(161, 19);
            this.radTongHop.TabIndex = 2;
            this.radTongHop.Text = "3. Thống kê tổng hợp KPI";
            this.radTongHop.UseVisualStyleBackColor = true;
            // 
            // radVeDienTu
            // 
            this.radVeDienTu.AutoSize = true;
            this.radVeDienTu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radVeDienTu.Location = new System.Drawing.Point(185, 5);
            this.radVeDienTu.Name = "radVeDienTu";
            this.radVeDienTu.Size = new System.Drawing.Size(160, 19);
            this.radVeDienTu.TabIndex = 1;
            this.radVeDienTu.Text = "2. Vé điện tử & Doanh thu";
            this.radVeDienTu.UseVisualStyleBackColor = true;
            // 
            // radQuyetToan
            // 
            this.radQuyetToan.AutoSize = true;
            this.radQuyetToan.Checked = true;
            this.radQuyetToan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.radQuyetToan.Location = new System.Drawing.Point(5, 5);
            this.radQuyetToan.Name = "radQuyetToan";
            this.radQuyetToan.Size = new System.Drawing.Size(162, 19);
            this.radQuyetToan.TabIndex = 0;
            this.radQuyetToan.TabStop = true;
            this.radQuyetToan.Text = "1. Quyết toán thu chi (F17)";
            this.radQuyetToan.UseVisualStyleBackColor = true;
            // 
            // lblLoaiBaoCao
            // 
            this.lblLoaiBaoCao.AutoSize = true;
            this.lblLoaiBaoCao.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLoaiBaoCao.Location = new System.Drawing.Point(15, 67);
            this.lblLoaiBaoCao.Name = "lblLoaiBaoCao";
            this.lblLoaiBaoCao.Size = new System.Drawing.Size(53, 15);
            this.lblLoaiBaoCao.TabIndex = 10;
            this.lblLoaiBaoCao.Text = "Loại BC:";
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLamMoi.Location = new System.Drawing.Point(995, 23);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(95, 30);
            this.btnLamMoi.TabIndex = 9;
            this.btnLamMoi.Text = "Làm mới (F5)";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnXuatExcel.Location = new System.Drawing.Point(885, 23);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(104, 30);
            this.btnXuatExcel.TabIndex = 8;
            this.btnXuatExcel.Text = "Xuất CSV (Ctrl+E)";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            // 
            // btnIn
            // 
            this.btnIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnIn.Location = new System.Drawing.Point(785, 23);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(94, 30);
            this.btnIn.TabIndex = 7;
            this.btnIn.Text = "In ấn (Ctrl+P)";
            this.btnIn.UseVisualStyleBackColor = true;
            // 
            // btnXem
            // 
            this.btnXem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnXem.Location = new System.Drawing.Point(680, 23);
            this.btnXem.Name = "btnXem";
            this.btnXem.Size = new System.Drawing.Size(99, 30);
            this.btnXem.TabIndex = 6;
            this.btnXem.Text = "Xem báo cáo";
            this.btnXem.UseVisualStyleBackColor = true;
            // 
            // dtpDenNgay
            // 
            this.dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpDenNgay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDenNgay.Location = new System.Drawing.Point(525, 26);
            this.dtpDenNgay.Name = "dtpDenNgay";
            this.dtpDenNgay.Size = new System.Drawing.Size(125, 23);
            this.dtpDenNgay.TabIndex = 5;
            // 
            // lblDenNgay
            // 
            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDenNgay.Location = new System.Drawing.Point(460, 30);
            this.lblDenNgay.Name = "lblDenNgay";
            this.lblDenNgay.Size = new System.Drawing.Size(60, 15);
            this.lblDenNgay.TabIndex = 4;
            this.lblDenNgay.Text = "Đến ngày:";
            // 
            // dtpTuNgay
            // 
            this.dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpTuNgay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTuNgay.Location = new System.Drawing.Point(325, 26);
            this.dtpTuNgay.Name = "dtpTuNgay";
            this.dtpTuNgay.Size = new System.Drawing.Size(125, 23);
            this.dtpTuNgay.TabIndex = 3;
            // 
            // lblTuNgay
            // 
            this.lblTuNgay.AutoSize = true;
            this.lblTuNgay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTuNgay.Location = new System.Drawing.Point(270, 30);
            this.lblTuNgay.Name = "lblTuNgay";
            this.lblTuNgay.Size = new System.Drawing.Size(52, 15);
            this.lblTuNgay.TabIndex = 2;
            this.lblTuNgay.Text = "Từ ngày:";
            // 
            // cboSuKien
            // 
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboSuKien.FormattingEnabled = true;
            this.cboSuKien.Location = new System.Drawing.Point(75, 26);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(185, 23);
            this.cboSuKien.TabIndex = 1;
            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSuKien.Location = new System.Drawing.Point(15, 30);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(49, 15);
            this.lblSuKien.TabIndex = 0;
            this.lblSuKien.Text = "Sự kiện:";
            // 
            // pnlKpiBottom
            // 
            this.pnlKpiBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.pnlKpiBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpiBottom.Controls.Add(this.lblTrangThai);
            this.pnlKpiBottom.Controls.Add(this.lblChenhLech);
            this.pnlKpiBottom.Controls.Add(this.lblTongChi);
            this.pnlKpiBottom.Controls.Add(this.lblTongThu);
            this.pnlKpiBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlKpiBottom.Location = new System.Drawing.Point(0, 565);
            this.pnlKpiBottom.Name = "pnlKpiBottom";
            this.pnlKpiBottom.Size = new System.Drawing.Size(1104, 45);
            this.pnlKpiBottom.TabIndex = 2;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblTrangThai.Location = new System.Drawing.Point(820, 10);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(265, 22);
            this.lblTrangThai.TabIndex = 3;
            this.lblTrangThai.Text = "Kết luận: Thặng dư tài chính";
            this.lblTrangThai.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChenhLech
            // 
            this.lblChenhLech.AutoSize = true;
            this.lblChenhLech.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblChenhLech.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblChenhLech.Location = new System.Drawing.Point(540, 13);
            this.lblChenhLech.Name = "lblChenhLech";
            this.lblChenhLech.Size = new System.Drawing.Size(161, 17);
            this.lblChenhLech.TabIndex = 2;
            this.lblChenhLech.Text = "Chênh lệch thu chi: 0 VNĐ";
            // 
            // lblTongChi
            // 
            this.lblTongChi.AutoSize = true;
            this.lblTongChi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTongChi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblTongChi.Location = new System.Drawing.Point(280, 13);
            this.lblTongChi.Name = "lblTongChi";
            this.lblTongChi.Size = new System.Drawing.Size(147, 17);
            this.lblTongChi.TabIndex = 1;
            this.lblTongChi.Text = "Tổng thực chi: 0 VNĐ";
            // 
            // lblTongThu
            // 
            this.lblTongThu.AutoSize = true;
            this.lblTongThu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTongThu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblTongThu.Location = new System.Drawing.Point(20, 13);
            this.lblTongThu.Name = "lblTongThu";
            this.lblTongThu.Size = new System.Drawing.Size(150, 17);
            this.lblTongThu.TabIndex = 0;
            this.lblTongThu.Text = "Tổng nguồn thu: 0 VNĐ";
            // 
            // tabReport
            // 
            this.tabReport.Controls.Add(this.tabData);
            this.tabReport.Controls.Add(this.tabPreview);
            this.tabReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabReport.Location = new System.Drawing.Point(0, 165);
            this.tabReport.Name = "tabReport";
            this.tabReport.SelectedIndex = 0;
            this.tabReport.Size = new System.Drawing.Size(1104, 400);
            this.tabReport.TabIndex = 3;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.dgvReportData);
            this.tabData.Location = new System.Drawing.Point(4, 24);
            this.tabData.Name = "tabData";
            this.tabData.Padding = new System.Windows.Forms.Padding(5);
            this.tabData.Size = new System.Drawing.Size(1096, 372);
            this.tabData.TabIndex = 0;
            this.tabData.Text = "Bảng số liệu chi tiết";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // dgvReportData
            // 
            this.dgvReportData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReportData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReportData.Location = new System.Drawing.Point(5, 5);
            this.dgvReportData.Name = "dgvReportData";
            this.dgvReportData.RowTemplate.Height = 25;
            this.dgvReportData.Size = new System.Drawing.Size(1086, 362);
            this.dgvReportData.TabIndex = 0;
            // 
            // tabPreview
            // 
            this.tabPreview.Controls.Add(this.printPreviewControl);
            this.tabPreview.Controls.Add(this.pnlPreviewToolbar);
            this.tabPreview.Location = new System.Drawing.Point(4, 24);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPreview.Size = new System.Drawing.Size(1096, 372);
            this.tabPreview.TabIndex = 1;
            this.tabPreview.Text = "Xem trước bản in (Print Preview)";
            this.tabPreview.UseVisualStyleBackColor = true;
            // 
            // pnlPreviewToolbar
            // 
            this.pnlPreviewToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlPreviewToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPreviewToolbar.Controls.Add(this.btnPrintFromPreview);
            this.pnlPreviewToolbar.Controls.Add(this.cboZoom);
            this.pnlPreviewToolbar.Controls.Add(this.lblZoom);
            this.pnlPreviewToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreviewToolbar.Location = new System.Drawing.Point(3, 3);
            this.pnlPreviewToolbar.Name = "pnlPreviewToolbar";
            this.pnlPreviewToolbar.Size = new System.Drawing.Size(1090, 38);
            this.pnlPreviewToolbar.TabIndex = 0;
            // 
            // btnPrintFromPreview
            // 
            this.btnPrintFromPreview.Location = new System.Drawing.Point(220, 5);
            this.btnPrintFromPreview.Name = "btnPrintFromPreview";
            this.btnPrintFromPreview.Size = new System.Drawing.Size(110, 26);
            this.btnPrintFromPreview.TabIndex = 2;
            this.btnPrintFromPreview.Text = "In ngay (Print)";
            this.btnPrintFromPreview.UseVisualStyleBackColor = true;
            // 
            // cboZoom
            // 
            this.cboZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboZoom.FormattingEnabled = true;
            this.cboZoom.Items.AddRange(new object[] {
            "50%",
            "75%",
            "100%",
            "125%",
            "150%"});
            this.cboZoom.Location = new System.Drawing.Point(85, 7);
            this.cboZoom.Name = "cboZoom";
            this.cboZoom.Size = new System.Drawing.Size(90, 23);
            this.cboZoom.TabIndex = 1;
            // 
            // lblZoom
            // 
            this.lblZoom.AutoSize = true;
            this.lblZoom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblZoom.Location = new System.Drawing.Point(12, 10);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(67, 15);
            this.lblZoom.TabIndex = 0;
            this.lblZoom.Text = "Phóng to:";
            // 
            // printPreviewControl
            // 
            this.printPreviewControl.AutoZoom = false;
            this.printPreviewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl.Location = new System.Drawing.Point(3, 41);
            this.printPreviewControl.Name = "printPreviewControl";
            this.printPreviewControl.Size = new System.Drawing.Size(1090, 328);
            this.printPreviewControl.TabIndex = 1;
            this.printPreviewControl.Zoom = 1D;
            // 
            // FrmReportStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 610);
            this.Controls.Add(this.tabReport);
            this.Controls.Add(this.pnlKpiBottom);
            this.Controls.Add(this.grpFilter);
            this.Controls.Add(this.pnlTopBanner);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(950, 550);
            this.Name = "FrmReportStatistics";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F19 - Báo Cáo & Thống Kê Tổng Hợp Sự Kiện";
            this.pnlTopBanner.ResumeLayout(false);
            this.pnlTopBanner.PerformLayout();
            this.grpFilter.ResumeLayout(false);
            this.grpFilter.PerformLayout();
            this.pnlReportType.ResumeLayout(false);
            this.pnlReportType.PerformLayout();
            this.pnlKpiBottom.ResumeLayout(false);
            this.pnlKpiBottom.PerformLayout();
            this.tabReport.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportData)).EndInit();
            this.tabPreview.ResumeLayout(false);
            this.pnlPreviewToolbar.ResumeLayout(false);
            this.pnlPreviewToolbar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopBanner;
        private System.Windows.Forms.Label lblBannerSubtitle;
        private System.Windows.Forms.Label lblBannerTitle;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.Label lblSuKien;
        private System.Windows.Forms.ComboBox cboSuKien;
        private System.Windows.Forms.Label lblTuNgay;
        private System.Windows.Forms.DateTimePicker dtpTuNgay;
        private System.Windows.Forms.Label lblDenNgay;
        private System.Windows.Forms.DateTimePicker dtpDenNgay;
        private System.Windows.Forms.Button btnXem;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Panel pnlReportType;
        private System.Windows.Forms.RadioButton radTongHop;
        private System.Windows.Forms.RadioButton radVeDienTu;
        private System.Windows.Forms.RadioButton radQuyetToan;
        private System.Windows.Forms.Label lblLoaiBaoCao;
        private System.Windows.Forms.Panel pnlKpiBottom;
        private System.Windows.Forms.Label lblTongThu;
        private System.Windows.Forms.Label lblTongChi;
        private System.Windows.Forms.Label lblChenhLech;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.TabControl tabReport;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.TabPage tabPreview;
        private System.Windows.Forms.DataGridView dgvReportData;
        private System.Windows.Forms.Panel pnlPreviewToolbar;
        private System.Windows.Forms.Button btnPrintFromPreview;
        private System.Windows.Forms.ComboBox cboZoom;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.PrintPreviewControl printPreviewControl;
    }
}
