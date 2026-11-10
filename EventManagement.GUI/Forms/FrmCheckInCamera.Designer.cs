namespace EventManagement.GUI.Forms
{
    partial class FrmCheckInCamera
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            StopCamera();
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
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.btnTatCamera = new System.Windows.Forms.Button();
            this.btnBatCamera = new System.Windows.Forms.Button();
            this.cboCamera = new System.Windows.Forms.ComboBox();
            this.lblCamera = new System.Windows.Forms.Label();
            this.cboSuKien = new System.Windows.Forms.ComboBox();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.grpCamera = new System.Windows.Forms.GroupBox();
            this.picCameraFeed = new System.Windows.Forms.PictureBox();
            this.grpManual = new System.Windows.Forms.GroupBox();
            this.lblManualHint = new System.Windows.Forms.Label();
            this.btnCheckInManual = new System.Windows.Forms.Button();
            this.txtMaVeManual = new System.Windows.Forms.TextBox();
            this.lblMaVeManual = new System.Windows.Forms.Label();
            this.pnlKpis = new System.Windows.Forms.TableLayoutPanel();
            this.pnlKpi1 = new System.Windows.Forms.Panel();
            this.lblKpiTongVe = new System.Windows.Forms.Label();
            this.lblKpiTitle1 = new System.Windows.Forms.Label();
            this.pnlKpi2 = new System.Windows.Forms.Panel();
            this.lblKpiDaCheckIn = new System.Windows.Forms.Label();
            this.lblKpiTitle2 = new System.Windows.Forms.Label();
            this.pnlKpi3 = new System.Windows.Forms.Panel();
            this.lblKpiChuaCheckIn = new System.Windows.Forms.Label();
            this.lblKpiTitle3 = new System.Windows.Forms.Label();
            this.pnlKpi4 = new System.Windows.Forms.Panel();
            this.lblKpiTyLe = new System.Windows.Forms.Label();
            this.lblKpiTitle4 = new System.Windows.Forms.Label();
            this.pnlStatusBanner = new System.Windows.Forms.Panel();
            this.lblStatusDetail = new System.Windows.Forms.Label();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.grpRecent = new System.Windows.Forms.GroupBox();
            this.dgvRecentCheckIns = new System.Windows.Forms.DataGridView();
            this.colThoiGian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenNguoi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHangVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhuongThuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKetQua = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnResetDemo = new System.Windows.Forms.Button();
            this.lblFooterStatus = new System.Windows.Forms.Label();
            this.btnDong = new System.Windows.Forms.Button();
            this.pnlTopHeader.SuspendLayout();
            this.pnlTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.grpCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCameraFeed)).BeginInit();
            this.grpManual.SuspendLayout();
            this.pnlKpis.SuspendLayout();
            this.pnlKpi1.SuspendLayout();
            this.pnlKpi2.SuspendLayout();
            this.pnlKpi3.SuspendLayout();
            this.pnlKpi4.SuspendLayout();
            this.pnlStatusBanner.SuspendLayout();
            this.grpRecent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentCheckIns)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopHeader
            // 
            this.pnlTopHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.pnlTopHeader.Controls.Add(this.lblSubTitle);
            this.pnlTopHeader.Controls.Add(this.lblTitle);
            this.pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlTopHeader.Name = "pnlTopHeader";
            this.pnlTopHeader.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.pnlTopHeader.Size = new System.Drawing.Size(1200, 72);
            this.pnlTopHeader.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.lblSubTitle.Location = new System.Drawing.Point(18, 42);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(595, 15);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Thẩm định QR Token theo thời gian thực • Kiểm soát vào cổng một chiều chống quét trùng lặp";
            this.lblSubTitle.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(360, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "SOÁT VÉ & ĐIỂM DANH SỰ KIỆN";
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.White;
            this.pnlTopBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTopBar.Controls.Add(this.btnTatCamera);
            this.pnlTopBar.Controls.Add(this.btnBatCamera);
            this.pnlTopBar.Controls.Add(this.cboCamera);
            this.pnlTopBar.Controls.Add(this.lblCamera);
            this.pnlTopBar.Controls.Add(this.cboSuKien);
            this.pnlTopBar.Controls.Add(this.lblSuKien);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 72);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Padding = new System.Windows.Forms.Padding(12);
            this.pnlTopBar.Size = new System.Drawing.Size(1200, 56);
            this.pnlTopBar.TabIndex = 1;
            // 
            // btnTatCamera
            // 
            this.btnTatCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnTatCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTatCamera.Enabled = false;
            this.btnTatCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTatCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTatCamera.ForeColor = System.Drawing.Color.White;
            this.btnTatCamera.Location = new System.Drawing.Point(1040, 11);
            this.btnTatCamera.Name = "btnTatCamera";
            this.btnTatCamera.Size = new System.Drawing.Size(125, 32);
            this.btnTatCamera.TabIndex = 5;
            this.btnTatCamera.Text = "■ Tắt Camera";
            this.btnTatCamera.UseVisualStyleBackColor = false;
            // 
            // btnBatCamera
            // 
            this.btnBatCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnBatCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBatCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBatCamera.ForeColor = System.Drawing.Color.White;
            this.btnBatCamera.Location = new System.Drawing.Point(905, 11);
            this.btnBatCamera.Name = "btnBatCamera";
            this.btnBatCamera.Size = new System.Drawing.Size(125, 32);
            this.btnBatCamera.TabIndex = 4;
            this.btnBatCamera.Text = "▶ Bật Camera";
            this.btnBatCamera.UseVisualStyleBackColor = false;
            // 
            // cboCamera
            // 
            this.cboCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCamera.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboCamera.FormattingEnabled = true;
            this.cboCamera.Location = new System.Drawing.Point(630, 16);
            this.cboCamera.Name = "cboCamera";
            this.cboCamera.Size = new System.Drawing.Size(260, 23);
            this.cboCamera.TabIndex = 3;
            // 
            // lblCamera
            // 
            this.lblCamera.AutoSize = true;
            this.lblCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCamera.Location = new System.Drawing.Point(525, 20);
            this.lblCamera.Name = "lblCamera";
            this.lblCamera.Size = new System.Drawing.Size(95, 15);
            this.lblCamera.TabIndex = 2;
            this.lblCamera.Text = "Thiết bị Video:";
            // 
            // cboSuKien
            // 
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboSuKien.FormattingEnabled = true;
            this.cboSuKien.Location = new System.Drawing.Point(140, 16);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(365, 23);
            this.cboSuKien.TabIndex = 1;
            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSuKien.Location = new System.Drawing.Point(18, 20);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(115, 15);
            this.lblSuKien.TabIndex = 0;
            this.lblSuKien.Text = "Sự kiện kiểm tra:";
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.Location = new System.Drawing.Point(0, 128);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.grpCamera);
            this.scMain.Panel1.Controls.Add(this.grpManual);
            this.scMain.Panel1.Padding = new System.Windows.Forms.Padding(12);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.grpRecent);
            this.scMain.Panel2.Controls.Add(this.pnlStatusBanner);
            this.scMain.Panel2.Controls.Add(this.pnlKpis);
            this.scMain.Panel2.Padding = new System.Windows.Forms.Padding(12);
            this.scMain.Size = new System.Drawing.Size(1200, 572);
            this.scMain.SplitterDistance = 460;
            this.scMain.TabIndex = 2;
            // 
            // grpCamera
            // 
            this.grpCamera.Controls.Add(this.picCameraFeed);
            this.grpCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCamera.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCamera.Location = new System.Drawing.Point(12, 12);
            this.grpCamera.Name = "grpCamera";
            this.grpCamera.Padding = new System.Windows.Forms.Padding(10);
            this.grpCamera.Size = new System.Drawing.Size(436, 428);
            this.grpCamera.TabIndex = 0;
            this.grpCamera.TabStop = false;
            this.grpCamera.Text = "Khung quét Camera QR";
            // 
            // picCameraFeed
            // 
            this.picCameraFeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.picCameraFeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCameraFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCameraFeed.Location = new System.Drawing.Point(10, 26);
            this.picCameraFeed.Name = "picCameraFeed";
            this.picCameraFeed.Size = new System.Drawing.Size(416, 392);
            this.picCameraFeed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCameraFeed.TabIndex = 0;
            this.picCameraFeed.TabStop = false;
            // 
            // grpManual
            // 
            this.grpManual.Controls.Add(this.lblManualHint);
            this.grpManual.Controls.Add(this.btnCheckInManual);
            this.grpManual.Controls.Add(this.txtMaVeManual);
            this.grpManual.Controls.Add(this.lblMaVeManual);
            this.grpManual.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpManual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpManual.Location = new System.Drawing.Point(12, 440);
            this.grpManual.Name = "grpManual";
            this.grpManual.Padding = new System.Windows.Forms.Padding(10);
            this.grpManual.Size = new System.Drawing.Size(436, 120);
            this.grpManual.TabIndex = 1;
            this.grpManual.TabStop = false;
            this.grpManual.Text = "Nhập mã vé / Token thủ công (Dự phòng)";
            // 
            // lblManualHint
            // 
            this.lblManualHint.AutoSize = true;
            this.lblManualHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblManualHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblManualHint.Location = new System.Drawing.Point(10, 93);
            this.lblManualHint.Name = "lblManualHint";
            this.lblManualHint.Size = new System.Drawing.Size(347, 13);
            this.lblManualHint.TabIndex = 3;
            this.lblManualHint.Text = "Nhập mã vé (VD: VE00001) hoặc chuỗi QR token để soát vé thủ công.";
            // 
            // btnCheckInManual
            // 
            this.btnCheckInManual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnCheckInManual.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheckInManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckInManual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCheckInManual.ForeColor = System.Drawing.Color.White;
            this.btnCheckInManual.Location = new System.Drawing.Point(300, 48);
            this.btnCheckInManual.Name = "btnCheckInManual";
            this.btnCheckInManual.Size = new System.Drawing.Size(126, 32);
            this.btnCheckInManual.TabIndex = 2;
            this.btnCheckInManual.Text = "Xác nhận";
            this.btnCheckInManual.UseVisualStyleBackColor = false;
            // 
            // txtMaVeManual
            // 
            this.txtMaVeManual.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMaVeManual.Location = new System.Drawing.Point(10, 51);
            this.txtMaVeManual.Name = "txtMaVeManual";
            this.txtMaVeManual.Size = new System.Drawing.Size(280, 27);
            this.txtMaVeManual.TabIndex = 1;
            // 
            // lblMaVeManual
            // 
            this.lblMaVeManual.AutoSize = true;
            this.lblMaVeManual.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblMaVeManual.Location = new System.Drawing.Point(10, 28);
            this.lblMaVeManual.Name = "lblMaVeManual";
            this.lblMaVeManual.Size = new System.Drawing.Size(129, 15);
            this.lblMaVeManual.TabIndex = 0;
            this.lblMaVeManual.Text = "Mã vé hoặc Token QR:";
            // 
            // pnlKpis
            // 
            this.pnlKpis.ColumnCount = 4;
            this.pnlKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlKpis.Controls.Add(this.pnlKpi4, 3, 0);
            this.pnlKpis.Controls.Add(this.pnlKpi3, 2, 0);
            this.pnlKpis.Controls.Add(this.pnlKpi2, 1, 0);
            this.pnlKpis.Controls.Add(this.pnlKpi1, 0, 0);
            this.pnlKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpis.Location = new System.Drawing.Point(12, 12);
            this.pnlKpis.Name = "pnlKpis";
            this.pnlKpis.RowCount = 1;
            this.pnlKpis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlKpis.Size = new System.Drawing.Size(712, 70);
            this.pnlKpis.TabIndex = 0;
            // 
            // pnlKpi1
            // 
            this.pnlKpi1.BackColor = System.Drawing.Color.White;
            this.pnlKpi1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi1.Controls.Add(this.lblKpiTongVe);
            this.pnlKpi1.Controls.Add(this.lblKpiTitle1);
            this.pnlKpi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi1.Location = new System.Drawing.Point(3, 3);
            this.pnlKpi1.Name = "pnlKpi1";
            this.pnlKpi1.Padding = new System.Windows.Forms.Padding(8);
            this.pnlKpi1.Size = new System.Drawing.Size(172, 64);
            this.pnlKpi1.TabIndex = 0;
            // 
            // lblKpiTongVe
            // 
            this.lblKpiTongVe.AutoSize = true;
            this.lblKpiTongVe.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblKpiTongVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblKpiTongVe.Location = new System.Drawing.Point(8, 25);
            this.lblKpiTongVe.Name = "lblKpiTongVe";
            this.lblKpiTongVe.Size = new System.Drawing.Size(26, 30);
            this.lblKpiTongVe.TabIndex = 1;
            this.lblKpiTongVe.Text = "0";
            // 
            // lblKpiTitle1
            // 
            this.lblKpiTitle1.AutoSize = true;
            this.lblKpiTitle1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblKpiTitle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblKpiTitle1.Location = new System.Drawing.Point(8, 8);
            this.lblKpiTitle1.Name = "lblKpiTitle1";
            this.lblKpiTitle1.Size = new System.Drawing.Size(99, 13);
            this.lblKpiTitle1.TabIndex = 0;
            this.lblKpiTitle1.Text = "TỔNG VÉ ĐÃ BÁN";
            // 
            // pnlKpi2
            // 
            this.pnlKpi2.BackColor = System.Drawing.Color.White;
            this.pnlKpi2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi2.Controls.Add(this.lblKpiDaCheckIn);
            this.pnlKpi2.Controls.Add(this.lblKpiTitle2);
            this.pnlKpi2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi2.Location = new System.Drawing.Point(181, 3);
            this.pnlKpi2.Name = "pnlKpi2";
            this.pnlKpi2.Padding = new System.Windows.Forms.Padding(8);
            this.pnlKpi2.Size = new System.Drawing.Size(172, 64);
            this.pnlKpi2.TabIndex = 1;
            // 
            // lblKpiDaCheckIn
            // 
            this.lblKpiDaCheckIn.AutoSize = true;
            this.lblKpiDaCheckIn.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblKpiDaCheckIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblKpiDaCheckIn.Location = new System.Drawing.Point(8, 25);
            this.lblKpiDaCheckIn.Name = "lblKpiDaCheckIn";
            this.lblKpiDaCheckIn.Size = new System.Drawing.Size(26, 30);
            this.lblKpiDaCheckIn.TabIndex = 1;
            this.lblKpiDaCheckIn.Text = "0";
            // 
            // lblKpiTitle2
            // 
            this.lblKpiTitle2.AutoSize = true;
            this.lblKpiTitle2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblKpiTitle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblKpiTitle2.Location = new System.Drawing.Point(8, 8);
            this.lblKpiTitle2.Name = "lblKpiTitle2";
            this.lblKpiTitle2.Size = new System.Drawing.Size(78, 13);
            this.lblKpiTitle2.TabIndex = 0;
            this.lblKpiTitle2.Text = "ĐÃ CHECK-IN";
            // 
            // pnlKpi3
            // 
            this.pnlKpi3.BackColor = System.Drawing.Color.White;
            this.pnlKpi3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi3.Controls.Add(this.lblKpiChuaCheckIn);
            this.pnlKpi3.Controls.Add(this.lblKpiTitle3);
            this.pnlKpi3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi3.Location = new System.Drawing.Point(359, 3);
            this.pnlKpi3.Name = "pnlKpi3";
            this.pnlKpi3.Padding = new System.Windows.Forms.Padding(8);
            this.pnlKpi3.Size = new System.Drawing.Size(172, 64);
            this.pnlKpi3.TabIndex = 2;
            // 
            // lblKpiChuaCheckIn
            // 
            this.lblKpiChuaCheckIn.AutoSize = true;
            this.lblKpiChuaCheckIn.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblKpiChuaCheckIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblKpiChuaCheckIn.Location = new System.Drawing.Point(8, 25);
            this.lblKpiChuaCheckIn.Name = "lblKpiChuaCheckIn";
            this.lblKpiChuaCheckIn.Size = new System.Drawing.Size(26, 30);
            this.lblKpiChuaCheckIn.TabIndex = 1;
            this.lblKpiChuaCheckIn.Text = "0";
            // 
            // lblKpiTitle3
            // 
            this.lblKpiTitle3.AutoSize = true;
            this.lblKpiTitle3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblKpiTitle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblKpiTitle3.Location = new System.Drawing.Point(8, 8);
            this.lblKpiTitle3.Name = "lblKpiTitle3";
            this.lblKpiTitle3.Size = new System.Drawing.Size(92, 13);
            this.lblKpiTitle3.TabIndex = 0;
            this.lblKpiTitle3.Text = "CHƯA CHECK-IN";
            // 
            // pnlKpi4
            // 
            this.pnlKpi4.BackColor = System.Drawing.Color.White;
            this.pnlKpi4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi4.Controls.Add(this.lblKpiTyLe);
            this.pnlKpi4.Controls.Add(this.lblKpiTitle4);
            this.pnlKpi4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi4.Location = new System.Drawing.Point(537, 3);
            this.pnlKpi4.Name = "pnlKpi4";
            this.pnlKpi4.Padding = new System.Windows.Forms.Padding(8);
            this.pnlKpi4.Size = new System.Drawing.Size(172, 64);
            this.pnlKpi4.TabIndex = 3;
            // 
            // lblKpiTyLe
            // 
            this.lblKpiTyLe.AutoSize = true;
            this.lblKpiTyLe.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblKpiTyLe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblKpiTyLe.Location = new System.Drawing.Point(8, 25);
            this.lblKpiTyLe.Name = "lblKpiTyLe";
            this.lblKpiTyLe.Size = new System.Drawing.Size(63, 30);
            this.lblKpiTyLe.TabIndex = 1;
            this.lblKpiTyLe.Text = "0.0%";
            // 
            // lblKpiTitle4
            // 
            this.lblKpiTitle4.AutoSize = true;
            this.lblKpiTitle4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblKpiTitle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblKpiTitle4.Location = new System.Drawing.Point(8, 8);
            this.lblKpiTitle4.Name = "lblKpiTitle4";
            this.lblKpiTitle4.Size = new System.Drawing.Size(95, 13);
            this.lblKpiTitle4.TabIndex = 0;
            this.lblKpiTitle4.Text = "TỶ LỆ VÀO CỔNG";
            // 
            // pnlStatusBanner
            // 
            this.pnlStatusBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.pnlStatusBanner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStatusBanner.Controls.Add(this.lblStatusDetail);
            this.pnlStatusBanner.Controls.Add(this.lblStatusTitle);
            this.pnlStatusBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatusBanner.Location = new System.Drawing.Point(12, 82);
            this.pnlStatusBanner.Name = "pnlStatusBanner";
            this.pnlStatusBanner.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.pnlStatusBanner.Size = new System.Drawing.Size(712, 85);
            this.pnlStatusBanner.TabIndex = 1;
            // 
            // lblStatusDetail
            // 
            this.lblStatusDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatusDetail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatusDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblStatusDetail.Location = new System.Drawing.Point(16, 38);
            this.lblStatusDetail.Name = "lblStatusDetail";
            this.lblStatusDetail.Size = new System.Drawing.Size(678, 33);
            this.lblStatusDetail.TabIndex = 1;
            this.lblStatusDetail.Text = "Đưa mã QR trên vé hoặc điện thoại vào trước camera để quét tự động";
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatusTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblStatusTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblStatusTitle.Location = new System.Drawing.Point(16, 12);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(678, 26);
            this.lblStatusTitle.TabIndex = 0;
            this.lblStatusTitle.Text = "SẴN SÀNG QUÉT MÃ QR";
            // 
            // grpRecent
            // 
            this.grpRecent.Controls.Add(this.dgvRecentCheckIns);
            this.grpRecent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRecent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpRecent.Location = new System.Drawing.Point(12, 167);
            this.grpRecent.Name = "grpRecent";
            this.grpRecent.Padding = new System.Windows.Forms.Padding(10);
            this.grpRecent.Size = new System.Drawing.Size(712, 393);
            this.grpRecent.TabIndex = 2;
            this.grpRecent.TabStop = false;
            this.grpRecent.Text = "Lịch sử quét check-in gần đây";
            // 
            // dgvRecentCheckIns
            // 
            this.dgvRecentCheckIns.AllowUserToAddRows = false;
            this.dgvRecentCheckIns.AllowUserToDeleteRows = false;
            this.dgvRecentCheckIns.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecentCheckIns.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecentCheckIns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecentCheckIns.ColumnHeadersHeight = 32;
            this.dgvRecentCheckIns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colThoiGian,
            this.colMaVe,
            this.colTenNguoi,
            this.colHangVe,
            this.colPhuongThuc,
            this.colKetQua});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecentCheckIns.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecentCheckIns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecentCheckIns.Location = new System.Drawing.Point(10, 26);
            this.dgvRecentCheckIns.Name = "dgvRecentCheckIns";
            this.dgvRecentCheckIns.ReadOnly = true;
            this.dgvRecentCheckIns.RowHeadersVisible = false;
            this.dgvRecentCheckIns.RowTemplate.Height = 28;
            this.dgvRecentCheckIns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecentCheckIns.Size = new System.Drawing.Size(692, 357);
            this.dgvRecentCheckIns.TabIndex = 0;
            // 
            // colThoiGian
            // 
            this.colThoiGian.HeaderText = "Thời gian";
            this.colThoiGian.Name = "colThoiGian";
            this.colThoiGian.ReadOnly = true;
            this.colThoiGian.Width = 110;
            // 
            // colMaVe
            // 
            this.colMaVe.HeaderText = "Mã vé";
            this.colMaVe.Name = "colMaVe";
            this.colMaVe.ReadOnly = true;
            this.colMaVe.Width = 90;
            // 
            // colTenNguoi
            // 
            this.colTenNguoi.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTenNguoi.HeaderText = "Người tham gia";
            this.colTenNguoi.Name = "colTenNguoi";
            this.colTenNguoi.ReadOnly = true;
            // 
            // colHangVe
            // 
            this.colHangVe.HeaderText = "Hạng vé";
            this.colHangVe.Name = "colHangVe";
            this.colHangVe.ReadOnly = true;
            this.colHangVe.Width = 120;
            // 
            // colPhuongThuc
            // 
            this.colPhuongThuc.HeaderText = "Phương thức";
            this.colPhuongThuc.Name = "colPhuongThuc";
            this.colPhuongThuc.ReadOnly = true;
            this.colPhuongThuc.Width = 100;
            // 
            // colKetQua
            // 
            this.colKetQua.HeaderText = "Ghi chú";
            this.colKetQua.Name = "colKetQua";
            this.colKetQua.ReadOnly = true;
            this.colKetQua.Width = 130;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottom.Controls.Add(this.btnResetDemo);
            this.pnlBottom.Controls.Add(this.lblFooterStatus);
            this.pnlBottom.Controls.Add(this.btnDong);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 700);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlBottom.Size = new System.Drawing.Size(1200, 48);
            this.pnlBottom.TabIndex = 3;
            // 
            // btnResetDemo
            // 
            this.btnResetDemo.BackColor = System.Drawing.Color.White;
            this.btnResetDemo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResetDemo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetDemo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnResetDemo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnResetDemo.Location = new System.Drawing.Point(12, 8);
            this.btnResetDemo.Name = "btnResetDemo";
            this.btnResetDemo.Size = new System.Drawing.Size(180, 30);
            this.btnResetDemo.TabIndex = 2;
            this.btnResetDemo.Text = "↺ Đặt lại vé mẫu để test";
            this.btnResetDemo.UseVisualStyleBackColor = false;
            // 
            // lblFooterStatus
            // 
            this.lblFooterStatus.AutoSize = true;
            this.lblFooterStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFooterStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblFooterStatus.Location = new System.Drawing.Point(210, 16);
            this.lblFooterStatus.Name = "lblFooterStatus";
            this.lblFooterStatus.Size = new System.Drawing.Size(262, 15);
            this.lblFooterStatus.TabIndex = 1;
            this.lblFooterStatus.Text = "Trạng thái camera: Chưa kết nối thiết bị hình ảnh.";
            // 
            // btnDong
            // 
            this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDong.BackColor = System.Drawing.Color.White;
            this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.btnDong.Location = new System.Drawing.Point(1090, 8);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(96, 30);
            this.btnDong.TabIndex = 0;
            this.btnDong.Text = "Đóng (Esc)";
            this.btnDong.UseVisualStyleBackColor = false;
            // 
            // FrmCheckInCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 748);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTopBar);
            this.Controls.Add(this.pnlTopHeader);
            this.MinimumSize = new System.Drawing.Size(1000, 680);
            this.Name = "FrmCheckInCamera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.pnlTopHeader.ResumeLayout(false);
            this.pnlTopHeader.PerformLayout();
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.grpCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCameraFeed)).EndInit();
            this.grpManual.ResumeLayout(false);
            this.grpManual.PerformLayout();
            this.pnlKpis.ResumeLayout(false);
            this.pnlKpi1.ResumeLayout(false);
            this.pnlKpi1.PerformLayout();
            this.pnlKpi2.ResumeLayout(false);
            this.pnlKpi2.PerformLayout();
            this.pnlKpi3.ResumeLayout(false);
            this.pnlKpi3.PerformLayout();
            this.pnlKpi4.ResumeLayout(false);
            this.pnlKpi4.PerformLayout();
            this.pnlStatusBanner.ResumeLayout(false);
            this.grpRecent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentCheckIns)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Button btnTatCamera;
        private System.Windows.Forms.Button btnBatCamera;
        private System.Windows.Forms.ComboBox cboCamera;
        private System.Windows.Forms.Label lblCamera;
        private System.Windows.Forms.ComboBox cboSuKien;
        private System.Windows.Forms.Label lblSuKien;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.GroupBox grpCamera;
        private System.Windows.Forms.PictureBox picCameraFeed;
        private System.Windows.Forms.GroupBox grpManual;
        private System.Windows.Forms.Label lblManualHint;
        private System.Windows.Forms.Button btnCheckInManual;
        private System.Windows.Forms.TextBox txtMaVeManual;
        private System.Windows.Forms.Label lblMaVeManual;
        private System.Windows.Forms.TableLayoutPanel pnlKpis;
        private System.Windows.Forms.Panel pnlKpi1;
        private System.Windows.Forms.Label lblKpiTongVe;
        private System.Windows.Forms.Label lblKpiTitle1;
        private System.Windows.Forms.Panel pnlKpi2;
        private System.Windows.Forms.Label lblKpiDaCheckIn;
        private System.Windows.Forms.Label lblKpiTitle2;
        private System.Windows.Forms.Panel pnlKpi3;
        private System.Windows.Forms.Label lblKpiChuaCheckIn;
        private System.Windows.Forms.Label lblKpiTitle3;
        private System.Windows.Forms.Panel pnlKpi4;
        private System.Windows.Forms.Label lblKpiTyLe;
        private System.Windows.Forms.Label lblKpiTitle4;
        private System.Windows.Forms.Panel pnlStatusBanner;
        private System.Windows.Forms.Label lblStatusDetail;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.GroupBox grpRecent;
        private System.Windows.Forms.DataGridView dgvRecentCheckIns;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGian;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenNguoi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHangVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhuongThuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKetQua;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnResetDemo;
        private System.Windows.Forms.Label lblFooterStatus;
        private System.Windows.Forms.Button btnDong;
    }
}
