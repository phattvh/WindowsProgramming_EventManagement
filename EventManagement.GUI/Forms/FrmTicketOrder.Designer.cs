namespace EventManagement.GUI.Forms
{
    partial class FrmTicketOrder
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
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.grpOrderInfo = new System.Windows.Forms.GroupBox();
            this.lblSuKien = new System.Windows.Forms.Label();
            this.cboSuKien = new System.Windows.Forms.ComboBox();
            this.lblHangVe = new System.Windows.Forms.Label();
            this.cboHangVe = new System.Windows.Forms.ComboBox();
            this.lblTierInfo = new System.Windows.Forms.Label();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.nudSoLuong = new System.Windows.Forms.NumericUpDown();
            this.lblSoLuongHint = new System.Windows.Forms.Label();
            this.pnlTotalMoney = new System.Windows.Forms.Panel();
            this.lblTotalTitle = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.grpBuyer = new System.Windows.Forms.GroupBox();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.lblSoDienThoai = new System.Windows.Forms.Label();
            this.txtSoDienThoai = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblDonVi = new System.Windows.Forms.Label();
            this.txtDonVi = new System.Windows.Forms.TextBox();
            this.btnXacNhan = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.scRight = new System.Windows.Forms.SplitContainer();
            this.grpAttendees = new System.Windows.Forms.GroupBox();
            this.dgvNguoiDiCung = new System.Windows.Forms.DataGridView();
            this.colSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenNguoiThamDu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpIssuedTickets = new System.Windows.Forms.GroupBox();
            this.dgvIssuedTickets = new System.Windows.Forms.DataGridView();
            this.colMaVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttendeeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaQR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHangVeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlRightBottom = new System.Windows.Forms.Panel();
            this.btnInVe = new System.Windows.Forms.Button();
            this.lblTicketCountSummary = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlTopHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.grpOrderInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).BeginInit();
            this.pnlTotalMoney.SuspendLayout();
            this.grpBuyer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scRight)).BeginInit();
            this.scRight.Panel1.SuspendLayout();
            this.scRight.Panel2.SuspendLayout();
            this.scRight.SuspendLayout();
            this.grpAttendees.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNguoiDiCung)).BeginInit();
            this.grpIssuedTickets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIssuedTickets)).BeginInit();
            this.pnlRightBottom.SuspendLayout();
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
            this.lblTitle.Size = new System.Drawing.Size(433, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "ĐĂNG KÝ VÉ SỰ KIỆN";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(255)))));
            this.lblSubTitle.Location = new System.Drawing.Point(18, 37);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(569, 15);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Cho phép mua 1–10 vé (BR-10), kiểm soát kho vé (BR-09), tự sinh mã QR GUID (BR-17) & Transaction (BR-18)";
            this.lblSubTitle.Visible = false;
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.Location = new System.Drawing.Point(0, 65);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.grpOrderInfo);
            this.scMain.Panel1.Padding = new System.Windows.Forms.Padding(12, 10, 6, 10);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.scRight);
            this.scMain.Panel2.Padding = new System.Windows.Forms.Padding(6, 10, 12, 10);
            this.scMain.Size = new System.Drawing.Size(1160, 655);
            this.scMain.SplitterDistance = 470;
            this.scMain.TabIndex = 1;
            // 
            // grpOrderInfo
            // 
            this.grpOrderInfo.BackColor = System.Drawing.Color.White;
            this.grpOrderInfo.Controls.Add(this.lblSuKien);
            this.grpOrderInfo.Controls.Add(this.cboSuKien);
            this.grpOrderInfo.Controls.Add(this.lblHangVe);
            this.grpOrderInfo.Controls.Add(this.cboHangVe);
            this.grpOrderInfo.Controls.Add(this.lblTierInfo);
            this.grpOrderInfo.Controls.Add(this.lblSoLuong);
            this.grpOrderInfo.Controls.Add(this.nudSoLuong);
            this.grpOrderInfo.Controls.Add(this.lblSoLuongHint);
            this.grpOrderInfo.Controls.Add(this.pnlTotalMoney);
            this.grpOrderInfo.Controls.Add(this.grpBuyer);
            this.grpOrderInfo.Controls.Add(this.btnXacNhan);
            this.grpOrderInfo.Controls.Add(this.btnLamMoi);
            this.grpOrderInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpOrderInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpOrderInfo.Location = new System.Drawing.Point(12, 10);
            this.grpOrderInfo.Name = "grpOrderInfo";
            this.grpOrderInfo.Padding = new System.Windows.Forms.Padding(12);
            this.grpOrderInfo.Size = new System.Drawing.Size(452, 635);
            this.grpOrderInfo.TabIndex = 0;
            this.grpOrderInfo.TabStop = false;
            this.grpOrderInfo.Text = "THÔNG TIN ĐẶT VÉ";
            // 
            // lblSuKien
            // 
            this.lblSuKien.AutoSize = true;
            this.lblSuKien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSuKien.Location = new System.Drawing.Point(12, 28);
            this.lblSuKien.Name = "lblSuKien";
            this.lblSuKien.Size = new System.Drawing.Size(95, 15);
            this.lblSuKien.TabIndex = 0;
            this.lblSuKien.Text = "Chọn sự kiện (*):";
            // 
            // cboSuKien
            // 
            this.cboSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSuKien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboSuKien.FormattingEnabled = true;
            this.cboSuKien.Location = new System.Drawing.Point(12, 46);
            this.cboSuKien.Name = "cboSuKien";
            this.cboSuKien.Size = new System.Drawing.Size(425, 23);
            this.cboSuKien.TabIndex = 1;
            this.cboSuKien.SelectedIndexChanged += new System.EventHandler(this.cboSuKien_SelectedIndexChanged);
            // 
            // lblHangVe
            // 
            this.lblHangVe.AutoSize = true;
            this.lblHangVe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHangVe.Location = new System.Drawing.Point(12, 80);
            this.lblHangVe.Name = "lblHangVe";
            this.lblHangVe.Size = new System.Drawing.Size(100, 15);
            this.lblHangVe.TabIndex = 2;
            this.lblHangVe.Text = "Chọn hạng vé (*):";
            // 
            // cboHangVe
            // 
            this.cboHangVe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHangVe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboHangVe.FormattingEnabled = true;
            this.cboHangVe.Location = new System.Drawing.Point(12, 98);
            this.cboHangVe.Name = "cboHangVe";
            this.cboHangVe.Size = new System.Drawing.Size(425, 23);
            this.cboHangVe.TabIndex = 3;
            this.cboHangVe.SelectedIndexChanged += new System.EventHandler(this.cboHangVe_SelectedIndexChanged);
            // 
            // lblTierInfo
            // 
            this.lblTierInfo.AutoSize = true;
            this.lblTierInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblTierInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(101)))), ((int)(((byte)(52)))));
            this.lblTierInfo.Location = new System.Drawing.Point(12, 126);
            this.lblTierInfo.Name = "lblTierInfo";
            this.lblTierInfo.Size = new System.Drawing.Size(262, 15);
            this.lblTierInfo.TabIndex = 4;
            this.lblTierInfo.Text = "Đơn giá: 0 VNĐ | Khả dụng trong kho: 0 vé";
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSoLuong.Location = new System.Drawing.Point(12, 154);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(102, 15);
            this.lblSoLuong.TabIndex = 5;
            this.lblSoLuong.Text = "Số lượng mua (*):";
            // 
            // nudSoLuong
            // 
            this.nudSoLuong.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.nudSoLuong.Location = new System.Drawing.Point(120, 150);
            this.nudSoLuong.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSoLuong.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSoLuong.Name = "nudSoLuong";
            this.nudSoLuong.Size = new System.Drawing.Size(80, 24);
            this.nudSoLuong.TabIndex = 6;
            this.nudSoLuong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSoLuong.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSoLuong.ValueChanged += new System.EventHandler(this.nudSoLuong_ValueChanged);
            // 
            // lblSoLuongHint
            // 
            this.lblSoLuongHint.AutoSize = true;
            this.lblSoLuongHint.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSoLuongHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSoLuongHint.Location = new System.Drawing.Point(210, 154);
            this.lblSoLuongHint.Name = "lblSoLuongHint";
            this.lblSoLuongHint.Size = new System.Drawing.Size(183, 15);
            this.lblSoLuongHint.TabIndex = 7;
            this.lblSoLuongHint.Text = "(Tối đa 1–10 vé/đơn)";
            // 
            // pnlTotalMoney
            // 
            this.pnlTotalMoney.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.pnlTotalMoney.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTotalMoney.Controls.Add(this.lblTotalTitle);
            this.pnlTotalMoney.Controls.Add(this.lblTongTien);
            this.pnlTotalMoney.Location = new System.Drawing.Point(12, 186);
            this.pnlTotalMoney.Name = "pnlTotalMoney";
            this.pnlTotalMoney.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlTotalMoney.Size = new System.Drawing.Size(425, 62);
            this.pnlTotalMoney.TabIndex = 8;
            // 
            // lblTotalTitle
            // 
            this.lblTotalTitle.AutoSize = true;
            this.lblTotalTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTotalTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblTotalTitle.Location = new System.Drawing.Point(12, 8);
            this.lblTotalTitle.Name = "lblTotalTitle";
            this.lblTotalTitle.Size = new System.Drawing.Size(155, 15);
            this.lblTotalTitle.TabIndex = 0;
            this.lblTotalTitle.Text = "TỔNG TIỀN THANH TOÁN:";
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblTongTien.Location = new System.Drawing.Point(10, 27);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(68, 25);
            this.lblTongTien.TabIndex = 1;
            this.lblTongTien.Text = "0 VNĐ";
            // 
            // grpBuyer
            // 
            this.grpBuyer.Controls.Add(this.lblHoTen);
            this.grpBuyer.Controls.Add(this.txtHoTen);
            this.grpBuyer.Controls.Add(this.lblSoDienThoai);
            this.grpBuyer.Controls.Add(this.txtSoDienThoai);
            this.grpBuyer.Controls.Add(this.lblEmail);
            this.grpBuyer.Controls.Add(this.txtEmail);
            this.grpBuyer.Controls.Add(this.lblDonVi);
            this.grpBuyer.Controls.Add(this.txtDonVi);
            this.grpBuyer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpBuyer.Location = new System.Drawing.Point(12, 260);
            this.grpBuyer.Name = "grpBuyer";
            this.grpBuyer.Padding = new System.Windows.Forms.Padding(10);
            this.grpBuyer.Size = new System.Drawing.Size(425, 230);
            this.grpBuyer.TabIndex = 9;
            this.grpBuyer.TabStop = false;
            this.grpBuyer.Text = "Người đại diện mua vé (Thanh toán)";
            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHoTen.Location = new System.Drawing.Point(12, 26);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(76, 15);
            this.lblHoTen.TabIndex = 0;
            this.lblHoTen.Text = "Họ và tên (*):";
            // 
            // txtHoTen
            // 
            this.txtHoTen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtHoTen.Location = new System.Drawing.Point(12, 44);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.PlaceholderText = "Ví dụ: Nguyễn Văn An";
            this.txtHoTen.Size = new System.Drawing.Size(400, 23);
            this.txtHoTen.TabIndex = 1;
            this.txtHoTen.TextChanged += new System.EventHandler(this.txtHoTen_TextChanged);
            // 
            // lblSoDienThoai
            // 
            this.lblSoDienThoai.AutoSize = true;
            this.lblSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSoDienThoai.Location = new System.Drawing.Point(12, 75);
            this.lblSoDienThoai.Name = "lblSoDienThoai";
            this.lblSoDienThoai.Size = new System.Drawing.Size(94, 15);
            this.lblSoDienThoai.TabIndex = 2;
            this.lblSoDienThoai.Text = "Số điện thoại (*):";
            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSoDienThoai.Location = new System.Drawing.Point(12, 93);
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.PlaceholderText = "Ví dụ: 0901234567";
            this.txtSoDienThoai.Size = new System.Drawing.Size(400, 23);
            this.txtSoDienThoai.TabIndex = 3;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEmail.Location = new System.Drawing.Point(12, 125);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(126, 15);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Email nhận vé điện tử:";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEmail.Location = new System.Drawing.Point(12, 143);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PlaceholderText = "an.nguyen@company.com";
            this.txtEmail.Size = new System.Drawing.Size(400, 23);
            this.txtEmail.TabIndex = 5;
            // 
            // lblDonVi
            // 
            this.lblDonVi.AutoSize = true;
            this.lblDonVi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDonVi.Location = new System.Drawing.Point(12, 175);
            this.lblDonVi.Name = "lblDonVi";
            this.lblDonVi.Size = new System.Drawing.Size(95, 15);
            this.lblDonVi.TabIndex = 6;
            this.lblDonVi.Text = "Đơn vị / Cơ quan:";
            // 
            // txtDonVi
            // 
            this.txtDonVi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDonVi.Location = new System.Drawing.Point(12, 193);
            this.txtDonVi.Name = "txtDonVi";
            this.txtDonVi.PlaceholderText = "Công ty TNHH...";
            this.txtDonVi.Size = new System.Drawing.Size(400, 23);
            this.txtDonVi.TabIndex = 7;
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXacNhan.ForeColor = System.Drawing.Color.White;
            this.btnXacNhan.Location = new System.Drawing.Point(12, 505);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(425, 42);
            this.btnXacNhan.TabIndex = 10;
            this.btnXacNhan.Text = "XÁC NHẬN BÁN VÉ & THANH TOÁN (Ctrl+S)";
            this.btnXacNhan.UseVisualStyleBackColor = false;
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(12, 555);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(425, 32);
            this.btnLamMoi.TabIndex = 11;
            this.btnLamMoi.Text = "Làm Mới Đơn Đặt (Ctrl+N)";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // scRight
            // 
            this.scRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scRight.Location = new System.Drawing.Point(6, 10);
            this.scRight.Name = "scRight";
            this.scRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scRight.Panel1
            // 
            this.scRight.Panel1.Controls.Add(this.grpAttendees);
            // 
            // scRight.Panel2
            // 
            this.scRight.Panel2.Controls.Add(this.grpIssuedTickets);
            this.scRight.Panel2.Controls.Add(this.pnlRightBottom);
            this.scRight.Size = new System.Drawing.Size(668, 635);
            this.scRight.SplitterDistance = 240;
            this.scRight.TabIndex = 0;
            // 
            // grpAttendees
            // 
            this.grpAttendees.BackColor = System.Drawing.Color.White;
            this.grpAttendees.Controls.Add(this.dgvNguoiDiCung);
            this.grpAttendees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAttendees.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpAttendees.Location = new System.Drawing.Point(0, 0);
            this.grpAttendees.Name = "grpAttendees";
            this.grpAttendees.Padding = new System.Windows.Forms.Padding(8);
            this.grpAttendees.Size = new System.Drawing.Size(668, 240);
            this.grpAttendees.TabIndex = 0;
            this.grpAttendees.TabStop = false;
            this.grpAttendees.Text = "DANH SÁCH NGƯỜI SỬ DỤNG VÉ";
            // 
            // dgvNguoiDiCung
            // 
            this.dgvNguoiDiCung.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNguoiDiCung.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSTT,
            this.colTenNguoiThamDu});
            this.dgvNguoiDiCung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNguoiDiCung.Location = new System.Drawing.Point(8, 25);
            this.dgvNguoiDiCung.Name = "dgvNguoiDiCung";
            this.dgvNguoiDiCung.Size = new System.Drawing.Size(652, 207);
            this.dgvNguoiDiCung.TabIndex = 0;
            // 
            // colSTT
            // 
            this.colSTT.HeaderText = "Vé Số";
            this.colSTT.Name = "colSTT";
            this.colSTT.ReadOnly = true;
            this.colSTT.Width = 60;
            // 
            // colTenNguoiThamDu
            // 
            this.colTenNguoiThamDu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTenNguoiThamDu.HeaderText = "Họ và tên người sử dụng vé";
            this.colTenNguoiThamDu.Name = "colTenNguoiThamDu";
            // 
            // grpIssuedTickets
            // 
            this.grpIssuedTickets.BackColor = System.Drawing.Color.White;
            this.grpIssuedTickets.Controls.Add(this.dgvIssuedTickets);
            this.grpIssuedTickets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpIssuedTickets.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpIssuedTickets.Location = new System.Drawing.Point(0, 0);
            this.grpIssuedTickets.Name = "grpIssuedTickets";
            this.grpIssuedTickets.Padding = new System.Windows.Forms.Padding(8);
            this.grpIssuedTickets.Size = new System.Drawing.Size(668, 345);
            this.grpIssuedTickets.TabIndex = 0;
            this.grpIssuedTickets.TabStop = false;
            this.grpIssuedTickets.Text = "DANH SÁCH VÉ ĐIỆN TỬ";
            // 
            // dgvIssuedTickets
            // 
            this.dgvIssuedTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIssuedTickets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaVe,
            this.colAttendeeName,
            this.colMaQR,
            this.colHangVeName,
            this.colGiaVe});
            this.dgvIssuedTickets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIssuedTickets.Location = new System.Drawing.Point(8, 25);
            this.dgvIssuedTickets.Name = "dgvIssuedTickets";
            this.dgvIssuedTickets.Size = new System.Drawing.Size(652, 312);
            this.dgvIssuedTickets.TabIndex = 0;
            // 
            // colMaVe
            // 
            this.colMaVe.DataPropertyName = "MaVe";
            this.colMaVe.HeaderText = "Mã Vé";
            this.colMaVe.Name = "colMaVe";
            this.colMaVe.Width = 100;
            // 
            // colAttendeeName
            // 
            this.colAttendeeName.DataPropertyName = "TenNguoiThamGia";
            this.colAttendeeName.HeaderText = "Người Tham Gia";
            this.colAttendeeName.Name = "colAttendeeName";
            this.colAttendeeName.Width = 160;
            // 
            // colMaQR
            // 
            this.colMaQR.DataPropertyName = "MaQR";
            this.colMaQR.HeaderText = "Mã QR (Token)";
            this.colMaQR.Name = "colMaQR";
            this.colMaQR.Width = 170;
            // 
            // colHangVeName
            // 
            this.colHangVeName.DataPropertyName = "TenHangVe";
            this.colHangVeName.HeaderText = "Hạng Vé";
            this.colHangVeName.Name = "colHangVeName";
            this.colHangVeName.Width = 120;
            // 
            // colGiaVe
            // 
            this.colGiaVe.DataPropertyName = "GiaVe";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            this.colGiaVe.DefaultCellStyle = dataGridViewCellStyle1;
            this.colGiaVe.HeaderText = "Đơn Giá (VNĐ)";
            this.colGiaVe.Name = "colGiaVe";
            this.colGiaVe.Width = 110;
            // 
            // pnlRightBottom
            // 
            this.pnlRightBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlRightBottom.Controls.Add(this.btnInVe);
            this.pnlRightBottom.Controls.Add(this.lblTicketCountSummary);
            this.pnlRightBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlRightBottom.Location = new System.Drawing.Point(0, 345);
            this.pnlRightBottom.Name = "pnlRightBottom";
            this.pnlRightBottom.Padding = new System.Windows.Forms.Padding(8);
            this.pnlRightBottom.Size = new System.Drawing.Size(668, 46);
            this.pnlRightBottom.TabIndex = 1;
            // 
            // btnInVe
            // 
            this.btnInVe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInVe.Location = new System.Drawing.Point(498, 8);
            this.btnInVe.Name = "btnInVe";
            this.btnInVe.Size = new System.Drawing.Size(160, 30);
            this.btnInVe.TabIndex = 0;
            this.btnInVe.Text = "In Vé Điện Tử (RDLC)";
            this.btnInVe.UseVisualStyleBackColor = true;
            this.btnInVe.Click += new System.EventHandler(this.btnInVe_Click);
            // 
            // lblTicketCountSummary
            // 
            this.lblTicketCountSummary.AutoSize = true;
            this.lblTicketCountSummary.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTicketCountSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTicketCountSummary.Location = new System.Drawing.Point(12, 16);
            this.lblTicketCountSummary.Name = "lblTicketCountSummary";
            this.lblTicketCountSummary.Size = new System.Drawing.Size(155, 15);
            this.lblTicketCountSummary.TabIndex = 1;
            this.lblTicketCountSummary.Text = "Tổng số vé đã phát hành: 0";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // FrmTicketOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 720);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.pnlTopHeader);
            this.MinimumSize = new System.Drawing.Size(1100, 680);
            this.Name = "FrmTicketOrder";
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.pnlTopHeader.ResumeLayout(false);
            this.pnlTopHeader.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.grpOrderInfo.ResumeLayout(false);
            this.grpOrderInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuong)).EndInit();
            this.pnlTotalMoney.ResumeLayout(false);
            this.pnlTotalMoney.PerformLayout();
            this.grpBuyer.ResumeLayout(false);
            this.grpBuyer.PerformLayout();
            this.scRight.Panel1.ResumeLayout(false);
            this.scRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scRight)).EndInit();
            this.scRight.ResumeLayout(false);
            this.grpAttendees.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNguoiDiCung)).EndInit();
            this.grpIssuedTickets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIssuedTickets)).EndInit();
            this.pnlRightBottom.ResumeLayout(false);
            this.pnlRightBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.GroupBox grpOrderInfo;
        private System.Windows.Forms.Label lblSuKien;
        private System.Windows.Forms.ComboBox cboSuKien;
        private System.Windows.Forms.Label lblHangVe;
        private System.Windows.Forms.ComboBox cboHangVe;
        private System.Windows.Forms.Label lblTierInfo;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.NumericUpDown nudSoLuong;
        private System.Windows.Forms.Label lblSoLuongHint;
        private System.Windows.Forms.Panel pnlTotalMoney;
        private System.Windows.Forms.Label lblTotalTitle;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.GroupBox grpBuyer;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblSoDienThoai;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblDonVi;
        private System.Windows.Forms.TextBox txtDonVi;
        private System.Windows.Forms.Button btnXacNhan;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.SplitContainer scRight;
        private System.Windows.Forms.GroupBox grpAttendees;
        private System.Windows.Forms.DataGridView dgvNguoiDiCung;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenNguoiThamDu;
        private System.Windows.Forms.GroupBox grpIssuedTickets;
        private System.Windows.Forms.DataGridView dgvIssuedTickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaVe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttendeeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaQR;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHangVeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaVe;
        private System.Windows.Forms.Panel pnlRightBottom;
        private System.Windows.Forms.Button btnInVe;
        private System.Windows.Forms.Label lblTicketCountSummary;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
