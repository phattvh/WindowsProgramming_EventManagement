using System;
using System.Drawing;
using System.Windows.Forms;

namespace EventManagement.GUI.Forms
{
    partial class FrmEventDetail
    {
        private System.ComponentModel.IContainer components = null;

        // Top Header
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblChonSuKien;
        private ComboBox cboChonSuKien;
        private Button btnLamMoiHeader;

        // KPI Summary Panel
        private Panel pnlKPICards;
        private Panel pnlCardVe;
        private Label lblCardVeTitle;
        private Label lblCardVeValue;
        private Panel pnlCardCheckIn;
        private Label lblCardCheckInTitle;
        private Label lblCardCheckInValue;
        private Panel pnlCardDoanhThu;
        private Label lblCardDoanhThuTitle;
        private Label lblCardDoanhThuValue;
        private Panel pnlCardChiPhi;
        private Label lblCardChiPhiTitle;
        private Label lblCardChiPhiValue;
        private Panel pnlCardNganSach;
        private Label lblCardNganSachTitle;
        private Label lblCardNganSachValue;

        // Main TabControl
        private TabControl tabMain;

        // Tab 1: Thong tin co ban
        private TabPage tabThongTin;
        private GroupBox grpThongTinChung;
        private Label lblMaSuKien;
        private TextBox txtMaSuKien;
        private Label lblTenSuKien;
        private TextBox txtTenSuKien;
        private Label lblLoaiSK;
        private ComboBox cboLoaiSK;
        private Label lblDiaDiem;
        private ComboBox cboDiaDiem;
        private Label lblNguoiPhuTrach;
        private TextBox txtNguoiPhuTrach;
        private Label lblTrangThai;
        private ComboBox cboTrangThai;

        private GroupBox grpThoiGianNganSach;
        private Label lblNgayBatDau;
        private DateTimePicker dtpNgayBatDau;
        private Label lblNgayKetThuc;
        private DateTimePicker dtpNgayKetThuc;
        private Label lblSucChua;
        private NumericUpDown nudSucChua;
        private Label lblNganSach;
        private NumericUpDown nudNganSach;
        private Label lblMoTa;
        private TextBox txtMoTa;

        private Panel pnlTab1Actions;
        private Button btnSuaThongTin;
        private Button btnLuuThongTin;
        private Button btnHuyThongTin;

        // Tab 2: Lich trinh
        private TabPage tabLichTrinh;
        private Panel pnlTab2Toolbar;
        private Label lblTab2Header;
        private Button btnMoF08;
        private DataGridView dgvLichTrinh;
        private DataGridViewTextBoxColumn colLT_Ma;
        private DataGridViewTextBoxColumn colLT_Ten;
        private DataGridViewTextBoxColumn colLT_BatDau;
        private DataGridViewTextBoxColumn colLT_KetThuc;
        private DataGridViewTextBoxColumn colLT_DienGia;
        private DataGridViewTextBoxColumn colLT_Phong;
        private DataGridViewTextBoxColumn colLT_GhiChu;

        // Tab 3: Dien gia
        private TabPage tabDienGia;
        private Panel pnlTab3Toolbar;
        private Label lblTab3Header;
        private DataGridView dgvDienGia;
        private DataGridViewTextBoxColumn colDG_Ma;
        private DataGridViewTextBoxColumn colDG_Ten;
        private DataGridViewTextBoxColumn colDG_ChucVu;
        private DataGridViewTextBoxColumn colDG_DonVi;
        private DataGridViewTextBoxColumn colDG_Email;
        private DataGridViewTextBoxColumn colDG_SDT;
        private DataGridViewTextBoxColumn colDG_TietMuc;
        private DataGridViewTextBoxColumn colDG_TrangThai;

        // Tab 4: Nguoi tham gia
        private TabPage tabNguoiThamGia;
        private Panel pnlTab4Toolbar;
        private Label lblTab4Header;
        private Label lblTimNguoiThamGia;
        private TextBox txtTimNguoiThamGia;
        private DataGridView dgvNguoiThamGia;
        private DataGridViewTextBoxColumn colNTG_MaVe;
        private DataGridViewTextBoxColumn colNTG_Ten;
        private DataGridViewTextBoxColumn colNTG_HangVe;
        private DataGridViewTextBoxColumn colNTG_Email;
        private DataGridViewTextBoxColumn colNTG_MaQR;
        private DataGridViewTextBoxColumn colNTG_NgayDangKy;
        private DataGridViewTextBoxColumn colNTG_TrangThai;

        // Tab 5: Hang ve
        private TabPage tabVe;
        private Panel pnlTab5Toolbar;
        private Label lblTab5Header;
        private DataGridView dgvHangVe;
        private DataGridViewTextBoxColumn colHV_Ma;
        private DataGridViewTextBoxColumn colHV_Ten;
        private DataGridViewTextBoxColumn colHV_Gia;
        private DataGridViewTextBoxColumn colHV_PhatHanh;
        private DataGridViewTextBoxColumn colHV_DaBan;
        private DataGridViewTextBoxColumn colHV_ConLai;
        private DataGridViewTextBoxColumn colHV_TyLe;
        private DataGridViewTextBoxColumn colHV_DoanhThu;

        // Tab 6: Nhan su
        private TabPage tabNhanSu;
        private Panel pnlTab6Toolbar;
        private Label lblTab6Header;
        private Button btnMoF15;
        private DataGridView dgvNhanSu;
        private DataGridViewTextBoxColumn colNS_MaPhanCong;
        private DataGridViewTextBoxColumn colNS_MaNhanSu;
        private DataGridViewTextBoxColumn colNS_Ten;
        private DataGridViewTextBoxColumn colNS_PhongBan;
        private DataGridViewTextBoxColumn colNS_NhiemVu;
        private DataGridViewTextBoxColumn colNS_BatDau;
        private DataGridViewTextBoxColumn colNS_KetThuc;
        private DataGridViewTextBoxColumn colNS_TrangThai;

        // Tab 7: Nha tai tro
        private TabPage tabTaiTro;
        private Panel pnlTab7Toolbar;
        private Label lblTab7Header;
        private Button btnMoF16;
        private DataGridView dgvTaiTro;
        private DataGridViewTextBoxColumn colTT_Ma;
        private DataGridViewTextBoxColumn colTT_Ten;
        private DataGridViewTextBoxColumn colTT_Goi;
        private DataGridViewTextBoxColumn colTT_GiaTri;
        private DataGridViewTextBoxColumn colTT_HinhThuc;
        private DataGridViewTextBoxColumn colTT_DaiDien;
        private DataGridViewTextBoxColumn colTT_SDT;
        private DataGridViewTextBoxColumn colTT_TrangThai;

        // Tab 8: Chi phi & Ngan sach
        private TabPage tabChiPhi;
        private Panel pnlTab8Toolbar;
        private Label lblTab8Header;
        private Label lblThanhDoNganSach;
        private ProgressBar prgNganSach;
        private Label lblNganSachInfo;
        private Button btnMoF17;
        private DataGridView dgvChiPhi;
        private DataGridViewTextBoxColumn colCP_Ma;
        private DataGridViewTextBoxColumn colCP_KhoanMuc;
        private DataGridViewTextBoxColumn colCP_SoTien;
        private DataGridViewTextBoxColumn colCP_NgayChi;
        private DataGridViewTextBoxColumn colCP_NguoiLap;
        private DataGridViewTextBoxColumn colCP_TrangThai;
        private DataGridViewTextBoxColumn colCP_DienGiai;

        // Tab 9: Phan hoi & Danh gia
        private TabPage tabDanhGia;
        private Panel pnlTab9Toolbar;
        private Label lblTab9Header;
        private Label lblDiemTrungBinh;
        private Label lblTyLeHaiLong;
        private Button btnMoF18;
        private DataGridView dgvDanhGia;
        private DataGridViewTextBoxColumn colDG_MaPhanHoi;
        private DataGridViewTextBoxColumn colDG_NguoiGui;
        private DataGridViewTextBoxColumn colDG_Diem;
        private DataGridViewTextBoxColumn colDG_NgayGui;
        private DataGridViewTextBoxColumn colDG_NoiDung;

        // Bottom Action Bar
        private Panel pnlBottomActions;
        private Button btnDong;
        private Button btnLamMoi;
        private Button btnXuatBaoCao;

        private ErrorProvider errBaoLoi;

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

            // 1. Instantiate Controls
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.lblChonSuKien = new Label();
            this.cboChonSuKien = new ComboBox();
            this.btnLamMoiHeader = new Button();

            this.pnlKPICards = new Panel();
            this.pnlCardVe = new Panel();
            this.lblCardVeTitle = new Label();
            this.lblCardVeValue = new Label();
            this.pnlCardCheckIn = new Panel();
            this.lblCardCheckInTitle = new Label();
            this.lblCardCheckInValue = new Label();
            this.pnlCardDoanhThu = new Panel();
            this.lblCardDoanhThuTitle = new Label();
            this.lblCardDoanhThuValue = new Label();
            this.pnlCardChiPhi = new Panel();
            this.lblCardChiPhiTitle = new Label();
            this.lblCardChiPhiValue = new Label();
            this.pnlCardNganSach = new Panel();
            this.lblCardNganSachTitle = new Label();
            this.lblCardNganSachValue = new Label();

            this.tabMain = new TabControl();

            // Tab 1
            this.tabThongTin = new TabPage();
            this.grpThongTinChung = new GroupBox();
            this.lblMaSuKien = new Label();
            this.txtMaSuKien = new TextBox();
            this.lblTenSuKien = new Label();
            this.txtTenSuKien = new TextBox();
            this.lblLoaiSK = new Label();
            this.cboLoaiSK = new ComboBox();
            this.lblDiaDiem = new Label();
            this.cboDiaDiem = new ComboBox();
            this.lblNguoiPhuTrach = new Label();
            this.txtNguoiPhuTrach = new TextBox();
            this.lblTrangThai = new Label();
            this.cboTrangThai = new ComboBox();

            this.grpThoiGianNganSach = new GroupBox();
            this.lblNgayBatDau = new Label();
            this.dtpNgayBatDau = new DateTimePicker();
            this.lblNgayKetThuc = new Label();
            this.dtpNgayKetThuc = new DateTimePicker();
            this.lblSucChua = new Label();
            this.nudSucChua = new NumericUpDown();
            this.lblNganSach = new Label();
            this.nudNganSach = new NumericUpDown();
            this.lblMoTa = new Label();
            this.txtMoTa = new TextBox();

            this.pnlTab1Actions = new Panel();
            this.btnSuaThongTin = new Button();
            this.btnLuuThongTin = new Button();
            this.btnHuyThongTin = new Button();

            // Tab 2
            this.tabLichTrinh = new TabPage();
            this.pnlTab2Toolbar = new Panel();
            this.lblTab2Header = new Label();
            this.btnMoF08 = new Button();
            this.dgvLichTrinh = new DataGridView();
            this.colLT_Ma = new DataGridViewTextBoxColumn();
            this.colLT_Ten = new DataGridViewTextBoxColumn();
            this.colLT_BatDau = new DataGridViewTextBoxColumn();
            this.colLT_KetThuc = new DataGridViewTextBoxColumn();
            this.colLT_DienGia = new DataGridViewTextBoxColumn();
            this.colLT_Phong = new DataGridViewTextBoxColumn();
            this.colLT_GhiChu = new DataGridViewTextBoxColumn();

            // Tab 3
            this.tabDienGia = new TabPage();
            this.pnlTab3Toolbar = new Panel();
            this.lblTab3Header = new Label();
            this.dgvDienGia = new DataGridView();
            this.colDG_Ma = new DataGridViewTextBoxColumn();
            this.colDG_Ten = new DataGridViewTextBoxColumn();
            this.colDG_ChucVu = new DataGridViewTextBoxColumn();
            this.colDG_DonVi = new DataGridViewTextBoxColumn();
            this.colDG_Email = new DataGridViewTextBoxColumn();
            this.colDG_SDT = new DataGridViewTextBoxColumn();
            this.colDG_TietMuc = new DataGridViewTextBoxColumn();
            this.colDG_TrangThai = new DataGridViewTextBoxColumn();

            // Tab 4
            this.tabNguoiThamGia = new TabPage();
            this.pnlTab4Toolbar = new Panel();
            this.lblTab4Header = new Label();
            this.lblTimNguoiThamGia = new Label();
            this.txtTimNguoiThamGia = new TextBox();
            this.dgvNguoiThamGia = new DataGridView();
            this.colNTG_MaVe = new DataGridViewTextBoxColumn();
            this.colNTG_Ten = new DataGridViewTextBoxColumn();
            this.colNTG_HangVe = new DataGridViewTextBoxColumn();
            this.colNTG_Email = new DataGridViewTextBoxColumn();
            this.colNTG_MaQR = new DataGridViewTextBoxColumn();
            this.colNTG_NgayDangKy = new DataGridViewTextBoxColumn();
            this.colNTG_TrangThai = new DataGridViewTextBoxColumn();

            // Tab 5
            this.tabVe = new TabPage();
            this.pnlTab5Toolbar = new Panel();
            this.lblTab5Header = new Label();
            this.dgvHangVe = new DataGridView();
            this.colHV_Ma = new DataGridViewTextBoxColumn();
            this.colHV_Ten = new DataGridViewTextBoxColumn();
            this.colHV_Gia = new DataGridViewTextBoxColumn();
            this.colHV_PhatHanh = new DataGridViewTextBoxColumn();
            this.colHV_DaBan = new DataGridViewTextBoxColumn();
            this.colHV_ConLai = new DataGridViewTextBoxColumn();
            this.colHV_TyLe = new DataGridViewTextBoxColumn();
            this.colHV_DoanhThu = new DataGridViewTextBoxColumn();

            // Tab 6
            this.tabNhanSu = new TabPage();
            this.pnlTab6Toolbar = new Panel();
            this.lblTab6Header = new Label();
            this.btnMoF15 = new Button();
            this.dgvNhanSu = new DataGridView();
            this.colNS_MaPhanCong = new DataGridViewTextBoxColumn();
            this.colNS_MaNhanSu = new DataGridViewTextBoxColumn();
            this.colNS_Ten = new DataGridViewTextBoxColumn();
            this.colNS_PhongBan = new DataGridViewTextBoxColumn();
            this.colNS_NhiemVu = new DataGridViewTextBoxColumn();
            this.colNS_BatDau = new DataGridViewTextBoxColumn();
            this.colNS_KetThuc = new DataGridViewTextBoxColumn();
            this.colNS_TrangThai = new DataGridViewTextBoxColumn();

            // Tab 7
            this.tabTaiTro = new TabPage();
            this.pnlTab7Toolbar = new Panel();
            this.lblTab7Header = new Label();
            this.btnMoF16 = new Button();
            this.dgvTaiTro = new DataGridView();
            this.colTT_Ma = new DataGridViewTextBoxColumn();
            this.colTT_Ten = new DataGridViewTextBoxColumn();
            this.colTT_Goi = new DataGridViewTextBoxColumn();
            this.colTT_GiaTri = new DataGridViewTextBoxColumn();
            this.colTT_HinhThuc = new DataGridViewTextBoxColumn();
            this.colTT_DaiDien = new DataGridViewTextBoxColumn();
            this.colTT_SDT = new DataGridViewTextBoxColumn();
            this.colTT_TrangThai = new DataGridViewTextBoxColumn();

            // Tab 8
            this.tabChiPhi = new TabPage();
            this.pnlTab8Toolbar = new Panel();
            this.lblTab8Header = new Label();
            this.lblThanhDoNganSach = new Label();
            this.prgNganSach = new ProgressBar();
            this.lblNganSachInfo = new Label();
            this.btnMoF17 = new Button();
            this.dgvChiPhi = new DataGridView();
            this.colCP_Ma = new DataGridViewTextBoxColumn();
            this.colCP_KhoanMuc = new DataGridViewTextBoxColumn();
            this.colCP_SoTien = new DataGridViewTextBoxColumn();
            this.colCP_NgayChi = new DataGridViewTextBoxColumn();
            this.colCP_NguoiLap = new DataGridViewTextBoxColumn();
            this.colCP_TrangThai = new DataGridViewTextBoxColumn();
            this.colCP_DienGiai = new DataGridViewTextBoxColumn();

            // Tab 9
            this.tabDanhGia = new TabPage();
            this.pnlTab9Toolbar = new Panel();
            this.lblTab9Header = new Label();
            this.lblDiemTrungBinh = new Label();
            this.lblTyLeHaiLong = new Label();
            this.btnMoF18 = new Button();
            this.dgvDanhGia = new DataGridView();
            this.colDG_MaPhanHoi = new DataGridViewTextBoxColumn();
            this.colDG_NguoiGui = new DataGridViewTextBoxColumn();
            this.colDG_Diem = new DataGridViewTextBoxColumn();
            this.colDG_NgayGui = new DataGridViewTextBoxColumn();
            this.colDG_NoiDung = new DataGridViewTextBoxColumn();

            // Bottom
            this.pnlBottomActions = new Panel();
            this.btnDong = new Button();
            this.btnLamMoi = new Button();
            this.btnXuatBaoCao = new Button();

            this.errBaoLoi = new ErrorProvider(this.components);

            // Suspend Layouts
            this.pnlHeader.SuspendLayout();
            this.pnlKPICards.SuspendLayout();
            this.pnlCardVe.SuspendLayout();
            this.pnlCardCheckIn.SuspendLayout();
            this.pnlCardDoanhThu.SuspendLayout();
            this.pnlCardChiPhi.SuspendLayout();
            this.pnlCardNganSach.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabThongTin.SuspendLayout();
            this.grpThongTinChung.SuspendLayout();
            this.grpThoiGianNganSach.SuspendLayout();
            this.pnlTab1Actions.SuspendLayout();
            this.tabLichTrinh.SuspendLayout();
            this.pnlTab2Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichTrinh)).BeginInit();
            this.tabDienGia.SuspendLayout();
            this.pnlTab3Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDienGia)).BeginInit();
            this.tabNguoiThamGia.SuspendLayout();
            this.pnlTab4Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNguoiThamGia)).BeginInit();
            this.tabVe.SuspendLayout();
            this.pnlTab5Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHangVe)).BeginInit();
            this.tabNhanSu.SuspendLayout();
            this.pnlTab6Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanSu)).BeginInit();
            this.tabTaiTro.SuspendLayout();
            this.pnlTab7Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaiTro)).BeginInit();
            this.tabChiPhi.SuspendLayout();
            this.pnlTab8Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiPhi)).BeginInit();
            this.tabDanhGia.SuspendLayout();
            this.pnlTab9Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhGia)).BeginInit();
            this.pnlBottomActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSucChua)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNganSach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).BeginInit();
            this.SuspendLayout();

            // ==========================================
            // pnlHeader
            // ==========================================
            this.pnlHeader.BackColor = Color.FromArgb(30, 58, 138); // Primary Blue
            this.pnlHeader.Controls.Add(this.btnLamMoiHeader);
            this.pnlHeader.Controls.Add(this.cboChonSuKien);
            this.pnlHeader.Controls.Add(this.lblChonSuKien);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Location = new Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new Size(1244, 62);
            this.pnlHeader.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(16, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(420, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CHI TIẾT SỰ KIỆN";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new Font("Segoe UI", 8.5F);
            this.lblSubtitle.ForeColor = Color.FromArgb(219, 234, 254);
            this.lblSubtitle.Location = new Point(18, 37);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new Size(580, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Trung tâm điều phối tập trung 9 phân hệ: Lịch trình, Hạng vé, Người tham gia, Nhân sự, Tài trợ, Chi phí, Đánh giá";

            // lblChonSuKien
            this.lblChonSuKien.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblChonSuKien.AutoSize = true;
            this.lblChonSuKien.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblChonSuKien.ForeColor = Color.White;
            this.lblChonSuKien.Location = new Point(720, 22);
            this.lblChonSuKien.Name = "lblChonSuKien";
            this.lblChonSuKien.Size = new Size(82, 15);
            this.lblChonSuKien.TabIndex = 2;
            this.lblChonSuKien.Text = "Chọn sự kiện:";

            // cboChonSuKien
            this.cboChonSuKien.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.cboChonSuKien.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboChonSuKien.Font = new Font("Segoe UI", 9F);
            this.cboChonSuKien.FormattingEnabled = true;
            this.cboChonSuKien.Location = new Point(810, 18);
            this.cboChonSuKien.Name = "cboChonSuKien";
            this.cboChonSuKien.Size = new Size(310, 23);
            this.cboChonSuKien.TabIndex = 3;
            this.cboChonSuKien.SelectedIndexChanged += new EventHandler(this.cboChonSuKien_SelectedIndexChanged);

            // btnLamMoiHeader
            this.btnLamMoiHeader.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnLamMoiHeader.BackColor = Color.FromArgb(37, 99, 235);
            this.btnLamMoiHeader.FlatAppearance.BorderSize = 0;
            this.btnLamMoiHeader.FlatStyle = FlatStyle.Flat;
            this.btnLamMoiHeader.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnLamMoiHeader.ForeColor = Color.White;
            this.btnLamMoiHeader.Location = new Point(1130, 16);
            this.btnLamMoiHeader.Name = "btnLamMoiHeader";
            this.btnLamMoiHeader.Size = new Size(95, 28);
            this.btnLamMoiHeader.TabIndex = 4;
            this.btnLamMoiHeader.Text = "🔄 Nạp lại";
            this.btnLamMoiHeader.UseVisualStyleBackColor = false;
            this.btnLamMoiHeader.Click += new EventHandler(this.btnLamMoi_Click);

            // ==========================================
            // pnlKPICards
            // ==========================================
            this.pnlKPICards.BackColor = Color.FromArgb(241, 245, 249);
            this.pnlKPICards.BorderStyle = BorderStyle.FixedSingle;
            this.pnlKPICards.Controls.Add(this.pnlCardVe);
            this.pnlKPICards.Controls.Add(this.pnlCardCheckIn);
            this.pnlKPICards.Controls.Add(this.pnlCardDoanhThu);
            this.pnlKPICards.Controls.Add(this.pnlCardChiPhi);
            this.pnlKPICards.Controls.Add(this.pnlCardNganSach);
            this.pnlKPICards.Dock = DockStyle.Top;
            this.pnlKPICards.Location = new Point(0, 62);
            this.pnlKPICards.Name = "pnlKPICards";
            this.pnlKPICards.Padding = new Padding(12, 6, 12, 6);
            this.pnlKPICards.Size = new Size(1244, 72);
            this.pnlKPICards.TabIndex = 1;

            // Card 1: Ve
            this.pnlCardVe.BackColor = Color.White;
            this.pnlCardVe.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCardVe.Controls.Add(this.lblCardVeTitle);
            this.pnlCardVe.Controls.Add(this.lblCardVeValue);
            this.pnlCardVe.Location = new Point(12, 7);
            this.pnlCardVe.Name = "pnlCardVe";
            this.pnlCardVe.Size = new Size(234, 56);
            this.pnlCardVe.TabIndex = 0;

            this.lblCardVeTitle.AutoSize = true;
            this.lblCardVeTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            this.lblCardVeTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblCardVeTitle.Location = new Point(10, 6);
            this.lblCardVeTitle.Name = "lblCardVeTitle";
            this.lblCardVeTitle.Size = new Size(119, 13);
            this.lblCardVeTitle.TabIndex = 0;
            this.lblCardVeTitle.Text = "VÉ ĐÃ BÁN / SỨC CHỨA";

            this.lblCardVeValue.AutoSize = true;
            this.lblCardVeValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblCardVeValue.ForeColor = Color.FromArgb(30, 58, 138);
            this.lblCardVeValue.Location = new Point(9, 26);
            this.lblCardVeValue.Name = "lblCardVeValue";
            this.lblCardVeValue.Size = new Size(130, 21);
            this.lblCardVeValue.TabIndex = 1;
            this.lblCardVeValue.Text = "0 / 0 vé (0%)";

            // Card 2: Check-in
            this.pnlCardCheckIn.BackColor = Color.White;
            this.pnlCardCheckIn.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCardCheckIn.Controls.Add(this.lblCardCheckInTitle);
            this.pnlCardCheckIn.Controls.Add(this.lblCardCheckInValue);
            this.pnlCardCheckIn.Location = new Point(256, 7);
            this.pnlCardCheckIn.Name = "pnlCardCheckIn";
            this.pnlCardCheckIn.Size = new Size(234, 56);
            this.pnlCardCheckIn.TabIndex = 1;

            this.lblCardCheckInTitle.AutoSize = true;
            this.lblCardCheckInTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            this.lblCardCheckInTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblCardCheckInTitle.Location = new Point(10, 6);
            this.lblCardCheckInTitle.Name = "lblCardCheckInTitle";
            this.lblCardCheckInTitle.Size = new Size(112, 13);
            this.lblCardCheckInTitle.TabIndex = 0;
            this.lblCardCheckInTitle.Text = "TỶ LỆ CHECK-IN THỰC";

            this.lblCardCheckInValue.AutoSize = true;
            this.lblCardCheckInValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblCardCheckInValue.ForeColor = Color.FromArgb(22, 163, 74);
            this.lblCardCheckInValue.Location = new Point(9, 26);
            this.lblCardCheckInValue.Name = "lblCardCheckInValue";
            this.lblCardCheckInValue.Size = new Size(118, 21);
            this.lblCardCheckInValue.TabIndex = 1;
            this.lblCardCheckInValue.Text = "0% (0 khách)";

            // Card 3: Doanh thu
            this.pnlCardDoanhThu.BackColor = Color.White;
            this.pnlCardDoanhThu.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCardDoanhThu.Controls.Add(this.lblCardDoanhThuTitle);
            this.pnlCardDoanhThu.Controls.Add(this.lblCardDoanhThuValue);
            this.pnlCardDoanhThu.Location = new Point(500, 7);
            this.pnlCardDoanhThu.Name = "pnlCardDoanhThu";
            this.pnlCardDoanhThu.Size = new Size(234, 56);
            this.pnlCardDoanhThu.TabIndex = 2;

            this.lblCardDoanhThuTitle.AutoSize = true;
            this.lblCardDoanhThuTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            this.lblCardDoanhThuTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblCardDoanhThuTitle.Location = new Point(10, 6);
            this.lblCardDoanhThuTitle.Name = "lblCardDoanhThuTitle";
            this.lblCardDoanhThuTitle.Size = new Size(112, 13);
            this.lblCardDoanhThuTitle.TabIndex = 0;
            this.lblCardDoanhThuTitle.Text = "TỔNG DOANH THU VÉ";

            this.lblCardDoanhThuValue.AutoSize = true;
            this.lblCardDoanhThuValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblCardDoanhThuValue.ForeColor = Color.FromArgb(2, 132, 199);
            this.lblCardDoanhThuValue.Location = new Point(9, 26);
            this.lblCardDoanhThuValue.Name = "lblCardDoanhThuValue";
            this.lblCardDoanhThuValue.Size = new Size(59, 21);
            this.lblCardDoanhThuValue.TabIndex = 1;
            this.lblCardDoanhThuValue.Text = "0 VNĐ";

            // Card 4: Chi phi
            this.pnlCardChiPhi.BackColor = Color.White;
            this.pnlCardChiPhi.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCardChiPhi.Controls.Add(this.lblCardChiPhiTitle);
            this.pnlCardChiPhi.Controls.Add(this.lblCardChiPhiValue);
            this.pnlCardChiPhi.Location = new Point(744, 7);
            this.pnlCardChiPhi.Name = "pnlCardChiPhi";
            this.pnlCardChiPhi.Size = new Size(234, 56);
            this.pnlCardChiPhi.TabIndex = 3;

            this.lblCardChiPhiTitle.AutoSize = true;
            this.lblCardChiPhiTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            this.lblCardChiPhiTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblCardChiPhiTitle.Location = new Point(10, 6);
            this.lblCardChiPhiTitle.Name = "lblCardChiPhiTitle";
            this.lblCardChiPhiTitle.Size = new Size(116, 13);
            this.lblCardChiPhiTitle.TabIndex = 0;
            this.lblCardChiPhiTitle.Text = "TỔNG THỰC CHI";

            this.lblCardChiPhiValue.AutoSize = true;
            this.lblCardChiPhiValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblCardChiPhiValue.ForeColor = Color.FromArgb(217, 119, 6);
            this.lblCardChiPhiValue.Location = new Point(9, 26);
            this.lblCardChiPhiValue.Name = "lblCardChiPhiValue";
            this.lblCardChiPhiValue.Size = new Size(59, 21);
            this.lblCardChiPhiValue.TabIndex = 1;
            this.lblCardChiPhiValue.Text = "0 VNĐ";

            // Card 5: Ngan sach
            this.pnlCardNganSach.BackColor = Color.White;
            this.pnlCardNganSach.BorderStyle = BorderStyle.FixedSingle;
            this.pnlCardNganSach.Controls.Add(this.lblCardNganSachTitle);
            this.pnlCardNganSach.Controls.Add(this.lblCardNganSachValue);
            this.pnlCardNganSach.Location = new Point(988, 7);
            this.pnlCardNganSach.Name = "pnlCardNganSach";
            this.pnlCardNganSach.Size = new Size(240, 56);
            this.pnlCardNganSach.TabIndex = 4;

            this.lblCardNganSachTitle.AutoSize = true;
            this.lblCardNganSachTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            this.lblCardNganSachTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblCardNganSachTitle.Location = new Point(10, 6);
            this.lblCardNganSachTitle.Name = "lblCardNganSachTitle";
            this.lblCardNganSachTitle.Size = new Size(122, 13);
            this.lblCardNganSachTitle.TabIndex = 0;
            this.lblCardNganSachTitle.Text = "TIẾN ĐỘ NGÂN SÁCH";

            this.lblCardNganSachValue.AutoSize = true;
            this.lblCardNganSachValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblCardNganSachValue.ForeColor = Color.FromArgb(22, 163, 74);
            this.lblCardNganSachValue.Location = new Point(9, 26);
            this.lblCardNganSachValue.Name = "lblCardNganSachValue";
            this.lblCardNganSachValue.Size = new Size(130, 21);
            this.lblCardNganSachValue.TabIndex = 1;
            this.lblCardNganSachValue.Text = "Còn: 0 đ (100%)";

            // ==========================================
            // tabMain
            // ==========================================
            this.tabMain.Controls.Add(this.tabThongTin);
            this.tabMain.Controls.Add(this.tabLichTrinh);
            this.tabMain.Controls.Add(this.tabDienGia);
            this.tabMain.Controls.Add(this.tabNguoiThamGia);
            this.tabMain.Controls.Add(this.tabVe);
            this.tabMain.Controls.Add(this.tabNhanSu);
            this.tabMain.Controls.Add(this.tabTaiTro);
            this.tabMain.Controls.Add(this.tabChiPhi);
            this.tabMain.Controls.Add(this.tabDanhGia);
            this.tabMain.Dock = DockStyle.Fill;
            this.tabMain.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.tabMain.ItemSize = new Size(132, 34);
            this.tabMain.Location = new Point(0, 134);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new Size(1244, 654);
            this.tabMain.SizeMode = TabSizeMode.Fixed;
            this.tabMain.TabIndex = 2;

            // ------------------------------------------
            // TAB 1: THONG TIN CO BAN
            // ------------------------------------------
            this.tabThongTin.BackColor = Color.FromArgb(248, 250, 252);
            this.tabThongTin.Controls.Add(this.pnlTab1Actions);
            this.tabThongTin.Controls.Add(this.grpThoiGianNganSach);
            this.tabThongTin.Controls.Add(this.grpThongTinChung);
            this.tabThongTin.Location = new Point(4, 38);
            this.tabThongTin.Name = "tabThongTin";
            this.tabThongTin.Padding = new Padding(15);
            this.tabThongTin.Size = new Size(1236, 612);
            this.tabThongTin.TabIndex = 0;
            this.tabThongTin.Text = "📋 Thông tin sự kiện";

            // grpThongTinChung
            this.grpThongTinChung.BackColor = Color.White;
            this.grpThongTinChung.Controls.Add(this.lblMaSuKien);
            this.grpThongTinChung.Controls.Add(this.txtMaSuKien);
            this.grpThongTinChung.Controls.Add(this.lblTenSuKien);
            this.grpThongTinChung.Controls.Add(this.txtTenSuKien);
            this.grpThongTinChung.Controls.Add(this.lblLoaiSK);
            this.grpThongTinChung.Controls.Add(this.cboLoaiSK);
            this.grpThongTinChung.Controls.Add(this.lblDiaDiem);
            this.grpThongTinChung.Controls.Add(this.cboDiaDiem);
            this.grpThongTinChung.Controls.Add(this.lblNguoiPhuTrach);
            this.grpThongTinChung.Controls.Add(this.txtNguoiPhuTrach);
            this.grpThongTinChung.Controls.Add(this.lblTrangThai);
            this.grpThongTinChung.Controls.Add(this.cboTrangThai);
            this.grpThongTinChung.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpThongTinChung.ForeColor = Color.FromArgb(30, 58, 138);
            this.grpThongTinChung.Location = new Point(15, 15);
            this.grpThongTinChung.Name = "grpThongTinChung";
            this.grpThongTinChung.Size = new Size(585, 470);
            this.grpThongTinChung.TabIndex = 0;
            this.grpThongTinChung.TabStop = false;
            this.grpThongTinChung.Text = "THÔNG TIN CƠ BẢN SỰ KIỆN";

            // lblMaSuKien
            this.lblMaSuKien.AutoSize = true;
            this.lblMaSuKien.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblMaSuKien.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblMaSuKien.Location = new Point(20, 38);
            this.lblMaSuKien.Name = "lblMaSuKien";
            this.lblMaSuKien.Size = new Size(71, 15);
            this.lblMaSuKien.TabIndex = 0;
            this.lblMaSuKien.Text = "Mã sự kiện:";

            // txtMaSuKien
            this.txtMaSuKien.BackColor = Color.FromArgb(241, 245, 249);
            this.txtMaSuKien.Font = new Font("Segoe UI", 9F);
            this.txtMaSuKien.Location = new Point(150, 35);
            this.txtMaSuKien.Name = "txtMaSuKien";
            this.txtMaSuKien.ReadOnly = true;
            this.txtMaSuKien.Size = new Size(220, 23);
            this.txtMaSuKien.TabIndex = 1;

            // lblTenSuKien
            this.lblTenSuKien.AutoSize = true;
            this.lblTenSuKien.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblTenSuKien.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblTenSuKien.Location = new Point(20, 82);
            this.lblTenSuKien.Name = "lblTenSuKien";
            this.lblTenSuKien.Size = new Size(74, 15);
            this.lblTenSuKien.TabIndex = 2;
            this.lblTenSuKien.Text = "Tên sự kiện:";

            // txtTenSuKien
            this.txtTenSuKien.Font = new Font("Segoe UI", 9F);
            this.txtTenSuKien.Location = new Point(150, 79);
            this.txtTenSuKien.Name = "txtTenSuKien";
            this.txtTenSuKien.Size = new Size(410, 23);
            this.txtTenSuKien.TabIndex = 3;

            // lblLoaiSK
            this.lblLoaiSK.AutoSize = true;
            this.lblLoaiSK.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblLoaiSK.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblLoaiSK.Location = new Point(20, 126);
            this.lblLoaiSK.Name = "lblLoaiSK";
            this.lblLoaiSK.Size = new Size(76, 15);
            this.lblLoaiSK.TabIndex = 4;
            this.lblLoaiSK.Text = "Loại sự kiện:";

            // cboLoaiSK
            this.cboLoaiSK.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboLoaiSK.Font = new Font("Segoe UI", 9F);
            this.cboLoaiSK.FormattingEnabled = true;
            this.cboLoaiSK.Location = new Point(150, 123);
            this.cboLoaiSK.Name = "cboLoaiSK";
            this.cboLoaiSK.Size = new Size(310, 23);
            this.cboLoaiSK.TabIndex = 5;

            // lblDiaDiem
            this.lblDiaDiem.AutoSize = true;
            this.lblDiaDiem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblDiaDiem.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblDiaDiem.Location = new Point(20, 170);
            this.lblDiaDiem.Name = "lblDiaDiem";
            this.lblDiaDiem.Size = new Size(59, 15);
            this.lblDiaDiem.TabIndex = 6;
            this.lblDiaDiem.Text = "Địa điểm:";

            // cboDiaDiem
            this.cboDiaDiem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboDiaDiem.Font = new Font("Segoe UI", 9F);
            this.cboDiaDiem.FormattingEnabled = true;
            this.cboDiaDiem.Location = new Point(150, 167);
            this.cboDiaDiem.Name = "cboDiaDiem";
            this.cboDiaDiem.Size = new Size(410, 23);
            this.cboDiaDiem.TabIndex = 7;

            // lblNguoiPhuTrach
            this.lblNguoiPhuTrach.AutoSize = true;
            this.lblNguoiPhuTrach.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblNguoiPhuTrach.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblNguoiPhuTrach.Location = new Point(20, 214);
            this.lblNguoiPhuTrach.Name = "lblNguoiPhuTrach";
            this.lblNguoiPhuTrach.Size = new Size(99, 15);
            this.lblNguoiPhuTrach.TabIndex = 8;
            this.lblNguoiPhuTrach.Text = "Người phụ trách:";

            // txtNguoiPhuTrach
            this.txtNguoiPhuTrach.Font = new Font("Segoe UI", 9F);
            this.txtNguoiPhuTrach.Location = new Point(150, 211);
            this.txtNguoiPhuTrach.Name = "txtNguoiPhuTrach";
            this.txtNguoiPhuTrach.Size = new Size(310, 23);
            this.txtNguoiPhuTrach.TabIndex = 9;

            // lblTrangThai
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblTrangThai.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblTrangThai.Location = new Point(20, 258);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new Size(65, 15);
            this.lblTrangThai.TabIndex = 10;
            this.lblTrangThai.Text = "Trạng thái:";

            // cboTrangThai
            this.cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboTrangThai.Font = new Font("Segoe UI", 9F);
            this.cboTrangThai.FormattingEnabled = true;
            this.cboTrangThai.Items.AddRange(new object[] {
                "Chuẩn bị",
                "Sắp diễn ra",
                "Đang diễn ra",
                "Đã hoàn thành",
                "Đã hủy"
            });
            this.cboTrangThai.Location = new Point(150, 255);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new Size(220, 23);
            this.cboTrangThai.TabIndex = 11;

            // grpThoiGianNganSach
            this.grpThoiGianNganSach.BackColor = Color.White;
            this.grpThoiGianNganSach.Controls.Add(this.lblNgayBatDau);
            this.grpThoiGianNganSach.Controls.Add(this.dtpNgayBatDau);
            this.grpThoiGianNganSach.Controls.Add(this.lblNgayKetThuc);
            this.grpThoiGianNganSach.Controls.Add(this.dtpNgayKetThuc);
            this.grpThoiGianNganSach.Controls.Add(this.lblSucChua);
            this.grpThoiGianNganSach.Controls.Add(this.nudSucChua);
            this.grpThoiGianNganSach.Controls.Add(this.lblNganSach);
            this.grpThoiGianNganSach.Controls.Add(this.nudNganSach);
            this.grpThoiGianNganSach.Controls.Add(this.lblMoTa);
            this.grpThoiGianNganSach.Controls.Add(this.txtMoTa);
            this.grpThoiGianNganSach.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpThoiGianNganSach.ForeColor = Color.FromArgb(30, 58, 138);
            this.grpThoiGianNganSach.Location = new Point(620, 15);
            this.grpThoiGianNganSach.Name = "grpThoiGianNganSach";
            this.grpThoiGianNganSach.Size = new Size(600, 470);
            this.grpThoiGianNganSach.TabIndex = 1;
            this.grpThoiGianNganSach.TabStop = false;
            this.grpThoiGianNganSach.Text = "THỜI GIAN, ĐỊNH MỨC & MÔ TẢ";

            // lblNgayBatDau
            this.lblNgayBatDau.AutoSize = true;
            this.lblNgayBatDau.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblNgayBatDau.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblNgayBatDau.Location = new Point(20, 38);
            this.lblNgayBatDau.Name = "lblNgayBatDau";
            this.lblNgayBatDau.Size = new Size(110, 15);
            this.lblNgayBatDau.TabIndex = 0;
            this.lblNgayBatDau.Text = "Thời gian bắt đầu:";

            // dtpNgayBatDau
            this.dtpNgayBatDau.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpNgayBatDau.Font = new Font("Segoe UI", 9F);
            this.dtpNgayBatDau.Format = DateTimePickerFormat.Custom;
            this.dtpNgayBatDau.Location = new Point(165, 35);
            this.dtpNgayBatDau.Name = "dtpNgayBatDau";
            this.dtpNgayBatDau.Size = new Size(200, 23);
            this.dtpNgayBatDau.TabIndex = 1;

            // lblNgayKetThuc
            this.lblNgayKetThuc.AutoSize = true;
            this.lblNgayKetThuc.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblNgayKetThuc.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblNgayKetThuc.Location = new Point(20, 82);
            this.lblNgayKetThuc.Name = "lblNgayKetThuc";
            this.lblNgayKetThuc.Size = new Size(111, 15);
            this.lblNgayKetThuc.TabIndex = 2;
            this.lblNgayKetThuc.Text = "Thời gian kết thúc:";

            // dtpNgayKetThuc
            this.dtpNgayKetThuc.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpNgayKetThuc.Font = new Font("Segoe UI", 9F);
            this.dtpNgayKetThuc.Format = DateTimePickerFormat.Custom;
            this.dtpNgayKetThuc.Location = new Point(165, 79);
            this.dtpNgayKetThuc.Name = "dtpNgayKetThuc";
            this.dtpNgayKetThuc.Size = new Size(200, 23);
            this.dtpNgayKetThuc.TabIndex = 3;

            // lblSucChua
            this.lblSucChua.AutoSize = true;
            this.lblSucChua.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblSucChua.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblSucChua.Location = new Point(20, 126);
            this.lblSucChua.Name = "lblSucChua";
            this.lblSucChua.Size = new Size(95, 15);
            this.lblSucChua.TabIndex = 4;
            this.lblSucChua.Text = "Sức chứa tối đa:";

            // nudSucChua
            this.nudSucChua.Font = new Font("Segoe UI", 9F);
            this.nudSucChua.Location = new Point(165, 123);
            this.nudSucChua.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.nudSucChua.Name = "nudSucChua";
            this.nudSucChua.Size = new Size(180, 23);
            this.nudSucChua.TabIndex = 5;

            // lblNganSach
            this.lblNganSach.AutoSize = true;
            this.lblNganSach.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblNganSach.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblNganSach.Location = new Point(20, 170);
            this.lblNganSach.Name = "lblNganSach";
            this.lblNganSach.Size = new Size(116, 15);
            this.lblNganSach.TabIndex = 6;
            this.lblNganSach.Text = "Ngân sách dự kiến:";

            // nudNganSach
            this.nudNganSach.Font = new Font("Segoe UI", 9F);
            this.nudNganSach.Increment = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.nudNganSach.Location = new Point(165, 167);
            this.nudNganSach.Maximum = new decimal(new int[] { 1410065407, 2, 0, 0 });
            this.nudNganSach.Name = "nudNganSach";
            this.nudNganSach.Size = new Size(240, 23);
            this.nudNganSach.TabIndex = 7;
            this.nudNganSach.ThousandsSeparator = true;

            // lblMoTa
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblMoTa.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblMoTa.Location = new Point(20, 214);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new Size(82, 15);
            this.lblMoTa.TabIndex = 8;
            this.lblMoTa.Text = "Mô tả sự kiện:";

            // txtMoTa
            this.txtMoTa.Font = new Font("Segoe UI", 9F);
            this.txtMoTa.Location = new Point(165, 211);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.ScrollBars = ScrollBars.Vertical;
            this.txtMoTa.Size = new Size(415, 130);
            this.txtMoTa.TabIndex = 9;

            // pnlTab1Actions
            this.pnlTab1Actions.Controls.Add(this.btnHuyThongTin);
            this.pnlTab1Actions.Controls.Add(this.btnLuuThongTin);
            this.pnlTab1Actions.Controls.Add(this.btnSuaThongTin);
            this.pnlTab1Actions.Location = new Point(15, 500);
            this.pnlTab1Actions.Name = "pnlTab1Actions";
            this.pnlTab1Actions.Size = new Size(1205, 50);
            this.pnlTab1Actions.TabIndex = 2;

            // btnSuaThongTin
            this.btnSuaThongTin.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnSuaThongTin.Location = new Point(0, 8);
            this.btnSuaThongTin.Name = "btnSuaThongTin";
            this.btnSuaThongTin.Size = new Size(150, 36);
            this.btnSuaThongTin.TabIndex = 0;
            this.btnSuaThongTin.Text = "✏️ Chỉnh sửa (F3)";
            this.btnSuaThongTin.UseVisualStyleBackColor = true;
            this.btnSuaThongTin.Click += new EventHandler(this.btnSuaThongTin_Click);

            // btnLuuThongTin
            this.btnLuuThongTin.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnLuuThongTin.Location = new Point(165, 8);
            this.btnLuuThongTin.Name = "btnLuuThongTin";
            this.btnLuuThongTin.Size = new Size(160, 36);
            this.btnLuuThongTin.TabIndex = 1;
            this.btnLuuThongTin.Text = "💾 Lưu thay đổi (Ctrl+S)";
            this.btnLuuThongTin.UseVisualStyleBackColor = true;
            this.btnLuuThongTin.Click += new EventHandler(this.btnLuuThongTin_Click);

            // btnHuyThongTin
            this.btnHuyThongTin.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnHuyThongTin.Location = new Point(340, 8);
            this.btnHuyThongTin.Name = "btnHuyThongTin";
            this.btnHuyThongTin.Size = new Size(150, 36);
            this.btnHuyThongTin.TabIndex = 2;
            this.btnHuyThongTin.Text = "❌ Hủy thay đổi (Esc)";
            this.btnHuyThongTin.UseVisualStyleBackColor = true;
            this.btnHuyThongTin.Click += new EventHandler(this.btnHuyThongTin_Click);

            // ------------------------------------------
            // TAB 2: LICH CHUONG TRINH
            // ------------------------------------------
            this.tabLichTrinh.BackColor = Color.White;
            this.tabLichTrinh.Controls.Add(this.dgvLichTrinh);
            this.tabLichTrinh.Controls.Add(this.pnlTab2Toolbar);
            this.tabLichTrinh.Location = new Point(4, 38);
            this.tabLichTrinh.Name = "tabLichTrinh";
            this.tabLichTrinh.Padding = new Padding(12);
            this.tabLichTrinh.Size = new Size(1236, 612);
            this.tabLichTrinh.TabIndex = 1;
            this.tabLichTrinh.Text = "🗓️ Lịch chương trình";

            this.pnlTab2Toolbar.Controls.Add(this.btnMoF08);
            this.pnlTab2Toolbar.Controls.Add(this.lblTab2Header);
            this.pnlTab2Toolbar.Dock = DockStyle.Top;
            this.pnlTab2Toolbar.Location = new Point(12, 12);
            this.pnlTab2Toolbar.Name = "pnlTab2Toolbar";
            this.pnlTab2Toolbar.Size = new Size(1212, 45);
            this.pnlTab2Toolbar.TabIndex = 0;

            this.lblTab2Header.AutoSize = true;
            this.lblTab2Header.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTab2Header.ForeColor = Color.FromArgb(30, 58, 138);
            this.lblTab2Header.Location = new Point(0, 12);
            this.lblTab2Header.Name = "lblTab2Header";
            this.lblTab2Header.Size = new Size(420, 19);
            this.lblTab2Header.TabIndex = 0;
            this.lblTab2Header.Text = "DANH SÁCH CÁC TIẾT MỤC / NỘI DUNG CHƯƠNG TRÌNH SỰ KIỆN";

            this.btnMoF08.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMoF08.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnMoF08.Location = new Point(1020, 5);
            this.btnMoF08.Name = "btnMoF08";
            this.btnMoF08.Size = new Size(190, 34);
            this.btnMoF08.TabIndex = 1;
            this.btnMoF08.Text = "Quản lý lịch trình";
            this.btnMoF08.UseVisualStyleBackColor = true;
            this.btnMoF08.Click += new EventHandler(this.btnMoF08_Click);

            this.dgvLichTrinh.Columns.AddRange(new DataGridViewColumn[] {
                this.colLT_Ma,
                this.colLT_Ten,
                this.colLT_BatDau,
                this.colLT_KetThuc,
                this.colLT_DienGia,
                this.colLT_Phong,
                this.colLT_GhiChu
            });
            this.dgvLichTrinh.Dock = DockStyle.Fill;
            this.dgvLichTrinh.Location = new Point(12, 57);
            this.dgvLichTrinh.Name = "dgvLichTrinh";
            this.dgvLichTrinh.Size = new Size(1212, 543);
            this.dgvLichTrinh.TabIndex = 1;

            this.colLT_Ma.DataPropertyName = "MaLichTrinh";
            this.colLT_Ma.HeaderText = "Mã lịch";
            this.colLT_Ma.Name = "colLT_Ma";
            this.colLT_Ma.Width = 100;

            this.colLT_Ten.DataPropertyName = "TenTietMuc";
            this.colLT_Ten.HeaderText = "Tên tiết mục / Nội dung";
            this.colLT_Ten.Name = "colLT_Ten";
            this.colLT_Ten.Width = 280;

            this.colLT_BatDau.DataPropertyName = "ThoiGianBatDau";
            this.colLT_BatDau.HeaderText = "Bắt đầu";
            this.colLT_BatDau.Name = "colLT_BatDau";
            this.colLT_BatDau.Width = 140;

            this.colLT_KetThuc.DataPropertyName = "ThoiGianKetThuc";
            this.colLT_KetThuc.HeaderText = "Kết thúc";
            this.colLT_KetThuc.Name = "colLT_KetThuc";
            this.colLT_KetThuc.Width = 140;

            this.colLT_DienGia.DataPropertyName = "TenDienGia";
            this.colLT_DienGia.HeaderText = "Diễn giả phụ trách";
            this.colLT_DienGia.Name = "colLT_DienGia";
            this.colLT_DienGia.Width = 180;

            this.colLT_Phong.DataPropertyName = "PhongChucNang";
            this.colLT_Phong.HeaderText = "Phòng / Khu vực";
            this.colLT_Phong.Name = "colLT_Phong";
            this.colLT_Phong.Width = 140;

            this.colLT_GhiChu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colLT_GhiChu.DataPropertyName = "MoTa";
            this.colLT_GhiChu.HeaderText = "Ghi chú & Chi tiết";
            this.colLT_GhiChu.Name = "colLT_GhiChu";

            // ------------------------------------------
            // TAB 3: DIEN GIA
            // ------------------------------------------
            this.tabDienGia.BackColor = Color.White;
            this.tabDienGia.Controls.Add(this.dgvDienGia);
            this.tabDienGia.Controls.Add(this.pnlTab3Toolbar);
            this.tabDienGia.Location = new Point(4, 38);
            this.tabDienGia.Name = "tabDienGia";
            this.tabDienGia.Padding = new Padding(12);
            this.tabDienGia.Size = new Size(1236, 612);
            this.tabDienGia.TabIndex = 2;
            this.tabDienGia.Text = "🎤 Diễn giả & Khách mời";

            this.pnlTab3Toolbar.Controls.Add(this.lblTab3Header);
            this.pnlTab3Toolbar.Dock = DockStyle.Top;
            this.pnlTab3Toolbar.Location = new Point(12, 12);
            this.pnlTab3Toolbar.Name = "pnlTab3Toolbar";
            this.pnlTab3Toolbar.Size = new Size(1212, 45);
            this.pnlTab3Toolbar.TabIndex = 0;

            this.lblTab3Header.AutoSize = true;
            this.lblTab3Header.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTab3Header.ForeColor = Color.FromArgb(30, 58, 138);
            this.lblTab3Header.Location = new Point(0, 12);
            this.lblTab3Header.Name = "lblTab3Header";
            this.lblTab3Header.Size = new Size(390, 19);
            this.lblTab3Header.TabIndex = 0;
            this.lblTab3Header.Text = "DANH SÁCH DIỄN GIẢ & KHÁCH MỜI THAM GIA CHIA SẺ";

            this.dgvDienGia.Columns.AddRange(new DataGridViewColumn[] {
                this.colDG_Ma,
                this.colDG_Ten,
                this.colDG_ChucVu,
                this.colDG_DonVi,
                this.colDG_Email,
                this.colDG_SDT,
                this.colDG_TietMuc,
                this.colDG_TrangThai
            });
            this.dgvDienGia.Dock = DockStyle.Fill;
            this.dgvDienGia.Location = new Point(12, 57);
            this.dgvDienGia.Name = "dgvDienGia";
            this.dgvDienGia.Size = new Size(1212, 543);
            this.dgvDienGia.TabIndex = 1;

            this.colDG_Ma.DataPropertyName = "MaDienGia";
            this.colDG_Ma.HeaderText = "Mã";
            this.colDG_Ma.Name = "colDG_Ma";
            this.colDG_Ma.Width = 90;

            this.colDG_Ten.DataPropertyName = "HoTen";
            this.colDG_Ten.HeaderText = "Họ và tên";
            this.colDG_Ten.Name = "colDG_Ten";
            this.colDG_Ten.Width = 190;

            this.colDG_ChucVu.DataPropertyName = "ChucVu";
            this.colDG_ChucVu.HeaderText = "Chức vụ / Học vị";
            this.colDG_ChucVu.Name = "colDG_ChucVu";
            this.colDG_ChucVu.Width = 170;

            this.colDG_DonVi.DataPropertyName = "DonViCongTac";
            this.colDG_DonVi.HeaderText = "Đơn vị công tác";
            this.colDG_DonVi.Name = "colDG_DonVi";
            this.colDG_DonVi.Width = 210;

            this.colDG_Email.DataPropertyName = "Email";
            this.colDG_Email.HeaderText = "Email liên hệ";
            this.colDG_Email.Name = "colDG_Email";
            this.colDG_Email.Width = 180;

            this.colDG_SDT.DataPropertyName = "SoDienThoai";
            this.colDG_SDT.HeaderText = "Số điện thoại";
            this.colDG_SDT.Name = "colDG_SDT";
            this.colDG_SDT.Width = 120;

            this.colDG_TietMuc.DataPropertyName = "SoTietMucThamGia";
            this.colDG_TietMuc.HeaderText = "Số tiết mục";
            this.colDG_TietMuc.Name = "colDG_TietMuc";
            this.colDG_TietMuc.Width = 110;

            this.colDG_TrangThai.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colDG_TrangThai.DataPropertyName = "TrangThai";
            this.colDG_TrangThai.HeaderText = "Trạng thái";
            this.colDG_TrangThai.Name = "colDG_TrangThai";

            // ------------------------------------------
            // TAB 4: NGUOI THAM GIA
            // ------------------------------------------
            this.tabNguoiThamGia.BackColor = Color.White;
            this.tabNguoiThamGia.Controls.Add(this.dgvNguoiThamGia);
            this.tabNguoiThamGia.Controls.Add(this.pnlTab4Toolbar);
            this.tabNguoiThamGia.Location = new Point(4, 38);
            this.tabNguoiThamGia.Name = "tabNguoiThamGia";
            this.tabNguoiThamGia.Padding = new Padding(12);
            this.tabNguoiThamGia.Size = new Size(1236, 612);
            this.tabNguoiThamGia.TabIndex = 3;
            this.tabNguoiThamGia.Text = "👥 Người tham gia";

            this.pnlTab4Toolbar.Controls.Add(this.txtTimNguoiThamGia);
            this.pnlTab4Toolbar.Controls.Add(this.lblTimNguoiThamGia);
            this.pnlTab4Toolbar.Controls.Add(this.lblTab4Header);
            this.pnlTab4Toolbar.Dock = DockStyle.Top;
            this.pnlTab4Toolbar.Location = new Point(12, 12);
            this.pnlTab4Toolbar.Name = "pnlTab4Toolbar";
            this.pnlTab4Toolbar.Size = new Size(1212, 45);
            this.pnlTab4Toolbar.TabIndex = 0;

            this.lblTab4Header.AutoSize = true;
            this.lblTab4Header.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTab4Header.ForeColor = Color.FromArgb(30, 58, 138);
            this.lblTab4Header.Location = new Point(0, 12);
            this.lblTab4Header.Name = "lblTab4Header";
            this.lblTab4Header.Size = new Size(400, 19);
            this.lblTab4Header.TabIndex = 0;
            this.lblTab4Header.Text = "DANH SÁCH ĐĂNG KÝ VÉ & TÌNH TRẠNG NGƯỜI THAM GIA";

            this.lblTimNguoiThamGia.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblTimNguoiThamGia.AutoSize = true;
            this.lblTimNguoiThamGia.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblTimNguoiThamGia.Location = new Point(880, 14);
            this.lblTimNguoiThamGia.Name = "lblTimNguoiThamGia";
            this.lblTimNguoiThamGia.Size = new Size(59, 15);
            this.lblTimNguoiThamGia.TabIndex = 1;
            this.lblTimNguoiThamGia.Text = "Tìm kiếm:";

            this.txtTimNguoiThamGia.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.txtTimNguoiThamGia.Font = new Font("Segoe UI", 9F);
            this.txtTimNguoiThamGia.Location = new Point(950, 11);
            this.txtTimNguoiThamGia.Name = "txtTimNguoiThamGia";
            this.txtTimNguoiThamGia.PlaceholderText = "Tên, email, mã vé...";
            this.txtTimNguoiThamGia.Size = new Size(250, 23);
            this.txtTimNguoiThamGia.TabIndex = 2;
            this.txtTimNguoiThamGia.TextChanged += new EventHandler(this.txtTimNguoiThamGia_TextChanged);

            this.dgvNguoiThamGia.Columns.AddRange(new DataGridViewColumn[] {
                this.colNTG_MaVe,
                this.colNTG_Ten,
                this.colNTG_HangVe,
                this.colNTG_Email,
                this.colNTG_MaQR,
                this.colNTG_NgayDangKy,
                this.colNTG_TrangThai
            });
            this.dgvNguoiThamGia.Dock = DockStyle.Fill;
            this.dgvNguoiThamGia.Location = new Point(12, 57);
            this.dgvNguoiThamGia.Name = "dgvNguoiThamGia";
            this.dgvNguoiThamGia.Size = new Size(1212, 543);
            this.dgvNguoiThamGia.TabIndex = 1;

            this.colNTG_MaVe.DataPropertyName = "MaVe";
            this.colNTG_MaVe.HeaderText = "Mã vé";
            this.colNTG_MaVe.Name = "colNTG_MaVe";
            this.colNTG_MaVe.Width = 100;

            this.colNTG_Ten.DataPropertyName = "TenNguoiThamGia";
            this.colNTG_Ten.HeaderText = "Họ tên người tham gia";
            this.colNTG_Ten.Name = "colNTG_Ten";
            this.colNTG_Ten.Width = 200;

            this.colNTG_HangVe.DataPropertyName = "TenHangVe";
            this.colNTG_HangVe.HeaderText = "Hạng vé";
            this.colNTG_HangVe.Name = "colNTG_HangVe";
            this.colNTG_HangVe.Width = 160;

            this.colNTG_Email.DataPropertyName = "Email";
            this.colNTG_Email.HeaderText = "Email";
            this.colNTG_Email.Name = "colNTG_Email";
            this.colNTG_Email.Width = 200;

            this.colNTG_MaQR.DataPropertyName = "MaQR";
            this.colNTG_MaQR.HeaderText = "Mã vé QR";
            this.colNTG_MaQR.Name = "colNTG_MaQR";
            this.colNTG_MaQR.Width = 160;

            this.colNTG_NgayDangKy.DataPropertyName = "NgayDangKy";
            this.colNTG_NgayDangKy.HeaderText = "Ngày đặt vé";
            this.colNTG_NgayDangKy.Name = "colNTG_NgayDangKy";
            this.colNTG_NgayDangKy.Width = 130;

            this.colNTG_TrangThai.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colNTG_TrangThai.DataPropertyName = "TrangThaiVe";
            this.colNTG_TrangThai.HeaderText = "Trạng thái vé";
            this.colNTG_TrangThai.Name = "colNTG_TrangThai";

            // ------------------------------------------
            // TAB 5: HANG VE
            // ------------------------------------------
            this.tabVe.BackColor = Color.White;
            this.tabVe.Controls.Add(this.dgvHangVe);
            this.tabVe.Controls.Add(this.pnlTab5Toolbar);
            this.tabVe.Location = new Point(4, 38);
            this.tabVe.Name = "tabVe";
            this.tabVe.Padding = new Padding(12);
            this.tabVe.Size = new Size(1236, 612);
            this.tabVe.TabIndex = 4;
            this.tabVe.Text = "🎟️ Hạng vé & Kho vé";

            this.pnlTab5Toolbar.Controls.Add(this.lblTab5Header);
            this.pnlTab5Toolbar.Dock = DockStyle.Top;
            this.pnlTab5Toolbar.Location = new Point(12, 12);
            this.pnlTab5Toolbar.Name = "pnlTab5Toolbar";
            this.pnlTab5Toolbar.Size = new Size(1212, 45);
            this.pnlTab5Toolbar.TabIndex = 0;

            this.lblTab5Header.AutoSize = true;
            this.lblTab5Header.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTab5Header.ForeColor = Color.FromArgb(30, 58, 138);
            this.lblTab5Header.Location = new Point(0, 12);
            this.lblTab5Header.Name = "lblTab5Header";
            this.lblTab5Header.Size = new Size(490, 19);
            this.lblTab5Header.TabIndex = 0;
            this.lblTab5Header.Text = "QUẢN LÝ CƠ CẤU HẠNG VÉ, GIÁ PHÁT HÀNH & TỒN KHO KHẢ DỤNG (BR-08, BR-09)";

            this.dgvHangVe.Columns.AddRange(new DataGridViewColumn[] {
                this.colHV_Ma,
                this.colHV_Ten,
                this.colHV_Gia,
                this.colHV_PhatHanh,
                this.colHV_DaBan,
                this.colHV_ConLai,
                this.colHV_TyLe,
                this.colHV_DoanhThu
            });
            this.dgvHangVe.Dock = DockStyle.Fill;
            this.dgvHangVe.Location = new Point(12, 57);
            this.dgvHangVe.Name = "dgvHangVe";
            this.dgvHangVe.Size = new Size(1212, 543);
            this.dgvHangVe.TabIndex = 1;

            this.colHV_Ma.DataPropertyName = "MaHangVe";
            this.colHV_Ma.HeaderText = "Mã hạng vé";
            this.colHV_Ma.Name = "colHV_Ma";
            this.colHV_Ma.Width = 110;

            this.colHV_Ten.DataPropertyName = "TenHangVe";
            this.colHV_Ten.HeaderText = "Tên hạng vé";
            this.colHV_Ten.Name = "colHV_Ten";
            this.colHV_Ten.Width = 240;

            this.colHV_Gia.DataPropertyName = "GiaVeFormatted";
            this.colHV_Gia.HeaderText = "Giá vé";
            this.colHV_Gia.Name = "colHV_Gia";
            this.colHV_Gia.Width = 140;

            this.colHV_PhatHanh.DataPropertyName = "SoLuongPhatHanh";
            this.colHV_PhatHanh.HeaderText = "Phát hành";
            this.colHV_PhatHanh.Name = "colHV_PhatHanh";
            this.colHV_PhatHanh.Width = 120;

            this.colHV_DaBan.DataPropertyName = "SoLuongDaBan";
            this.colHV_DaBan.HeaderText = "Đã bán";
            this.colHV_DaBan.Name = "colHV_DaBan";
            this.colHV_DaBan.Width = 120;

            this.colHV_ConLai.DataPropertyName = "SoLuongConLai";
            this.colHV_ConLai.HeaderText = "Còn lại";
            this.colHV_ConLai.Name = "colHV_ConLai";
            this.colHV_ConLai.Width = 120;

            this.colHV_TyLe.DataPropertyName = "TyLeBanFormatted";
            this.colHV_TyLe.HeaderText = "Tỷ lệ bán";
            this.colHV_TyLe.Name = "colHV_TyLe";
            this.colHV_TyLe.Width = 120;

            this.colHV_DoanhThu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colHV_DoanhThu.DataPropertyName = "DoanhThuFormatted";
            this.colHV_DoanhThu.HeaderText = "Tổng thu dự kiến";
            this.colHV_DoanhThu.Name = "colHV_DoanhThu";

            // ------------------------------------------
            // TAB 6: NHAN SU
            // ------------------------------------------
            this.tabNhanSu.BackColor = Color.White;
            this.tabNhanSu.Controls.Add(this.dgvNhanSu);
            this.tabNhanSu.Controls.Add(this.pnlTab6Toolbar);
            this.tabNhanSu.Location = new Point(4, 38);
            this.tabNhanSu.Name = "tabNhanSu";
            this.tabNhanSu.Padding = new Padding(12);
            this.tabNhanSu.Size = new Size(1236, 612);
            this.tabNhanSu.TabIndex = 5;
            this.tabNhanSu.Text = "👷 Nhân sự tổ chức";

            this.pnlTab6Toolbar.Controls.Add(this.btnMoF15);
            this.pnlTab6Toolbar.Controls.Add(this.lblTab6Header);
            this.pnlTab6Toolbar.Dock = DockStyle.Top;
            this.pnlTab6Toolbar.Location = new Point(12, 12);
            this.pnlTab6Toolbar.Name = "pnlTab6Toolbar";
            this.pnlTab6Toolbar.Size = new Size(1212, 45);
            this.pnlTab6Toolbar.TabIndex = 0;

            this.lblTab6Header.AutoSize = true;
            this.lblTab6Header.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTab6Header.ForeColor = Color.FromArgb(30, 58, 138);
            this.lblTab6Header.Location = new Point(0, 12);
            this.lblTab6Header.Name = "lblTab6Header";
            this.lblTab6Header.Size = new Size(470, 19);
            this.lblTab6Header.TabIndex = 0;
            this.lblTab6Header.Text = "DANH SÁCH NHÂN SỰ BAN TỔ CHỨC & CA TRỰC PHÂN CÔNG (BR-13)";

            this.btnMoF15.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMoF15.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnMoF15.Location = new Point(1010, 5);
            this.btnMoF15.Name = "btnMoF15";
            this.btnMoF15.Size = new Size(200, 34);
            this.btnMoF15.TabIndex = 1;
            this.btnMoF15.Text = "Quản lý phân công";
            this.btnMoF15.UseVisualStyleBackColor = true;
            this.btnMoF15.Click += new EventHandler(this.btnMoF15_Click);

            this.dgvNhanSu.Columns.AddRange(new DataGridViewColumn[] {
                this.colNS_MaPhanCong,
                this.colNS_MaNhanSu,
                this.colNS_Ten,
                this.colNS_PhongBan,
                this.colNS_NhiemVu,
                this.colNS_BatDau,
                this.colNS_KetThuc,
                this.colNS_TrangThai
            });
            this.dgvNhanSu.Dock = DockStyle.Fill;
            this.dgvNhanSu.Location = new Point(12, 57);
            this.dgvNhanSu.Name = "dgvNhanSu";
            this.dgvNhanSu.Size = new Size(1212, 543);
            this.dgvNhanSu.TabIndex = 1;

            this.colNS_MaPhanCong.DataPropertyName = "MaPhanCong";
            this.colNS_MaPhanCong.HeaderText = "Mã phân công";
            this.colNS_MaPhanCong.Name = "colNS_MaPhanCong";
            this.colNS_MaPhanCong.Width = 110;

            this.colNS_MaNhanSu.DataPropertyName = "MaNhanSu";
            this.colNS_MaNhanSu.HeaderText = "Mã NS";
            this.colNS_MaNhanSu.Name = "colNS_MaNhanSu";
            this.colNS_MaNhanSu.Width = 90;

            this.colNS_Ten.DataPropertyName = "HoTen";
            this.colNS_Ten.HeaderText = "Họ tên nhân sự";
            this.colNS_Ten.Name = "colNS_Ten";
            this.colNS_Ten.Width = 180;

            this.colNS_PhongBan.DataPropertyName = "PhongBan";
            this.colNS_PhongBan.HeaderText = "Phòng ban";
            this.colNS_PhongBan.Name = "colNS_PhongBan";
            this.colNS_PhongBan.Width = 140;

            this.colNS_NhiemVu.DataPropertyName = "NhiemVu";
            this.colNS_NhiemVu.HeaderText = "Nhiệm vụ phụ trách";
            this.colNS_NhiemVu.Name = "colNS_NhiemVu";
            this.colNS_NhiemVu.Width = 190;

            this.colNS_BatDau.DataPropertyName = "ThoiGianBatDau";
            this.colNS_BatDau.HeaderText = "Bắt đầu ca";
            this.colNS_BatDau.Name = "colNS_BatDau";
            this.colNS_BatDau.Width = 130;

            this.colNS_KetThuc.DataPropertyName = "ThoiGianKetThuc";
            this.colNS_KetThuc.HeaderText = "Kết thúc ca";
            this.colNS_KetThuc.Name = "colNS_KetThuc";
            this.colNS_KetThuc.Width = 130;

            this.colNS_TrangThai.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colNS_TrangThai.DataPropertyName = "TrangThai";
            this.colNS_TrangThai.HeaderText = "Trạng thái";
            this.colNS_TrangThai.Name = "colNS_TrangThai";

            // ------------------------------------------
            // TAB 7: NHA TAI TRO
            // ------------------------------------------
            this.tabTaiTro.BackColor = Color.White;
            this.tabTaiTro.Controls.Add(this.dgvTaiTro);
            this.tabTaiTro.Controls.Add(this.pnlTab7Toolbar);
            this.tabTaiTro.Location = new Point(4, 38);
            this.tabTaiTro.Name = "tabTaiTro";
            this.tabTaiTro.Padding = new Padding(12);
            this.tabTaiTro.Size = new Size(1236, 612);
            this.tabTaiTro.TabIndex = 6;
            this.tabTaiTro.Text = "🤝 Nhà tài trợ";

            this.pnlTab7Toolbar.Controls.Add(this.btnMoF16);
            this.pnlTab7Toolbar.Controls.Add(this.lblTab7Header);
            this.pnlTab7Toolbar.Dock = DockStyle.Top;
            this.pnlTab7Toolbar.Location = new Point(12, 12);
            this.pnlTab7Toolbar.Name = "pnlTab7Toolbar";
            this.pnlTab7Toolbar.Size = new Size(1212, 45);
            this.pnlTab7Toolbar.TabIndex = 0;

            this.lblTab7Header.AutoSize = true;
            this.lblTab7Header.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTab7Header.ForeColor = Color.FromArgb(30, 58, 138);
            this.lblTab7Header.Location = new Point(0, 12);
            this.lblTab7Header.Name = "lblTab7Header";
            this.lblTab7Header.Size = new Size(420, 19);
            this.lblTab7Header.TabIndex = 0;
            this.lblTab7Header.Text = "DANH SÁCH CÁC ĐƠN VỊ ĐỒNG HÀNH & GÓI TÀI TRỢ SỰ KIỆN";

            this.btnMoF16.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMoF16.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnMoF16.Location = new Point(1030, 5);
            this.btnMoF16.Name = "btnMoF16";
            this.btnMoF16.Size = new Size(180, 34);
            this.btnMoF16.TabIndex = 1;
            this.btnMoF16.Text = "Quản lý tài trợ";
            this.btnMoF16.UseVisualStyleBackColor = true;
            this.btnMoF16.Click += new EventHandler(this.btnMoF16_Click);

            this.dgvTaiTro.Columns.AddRange(new DataGridViewColumn[] {
                this.colTT_Ma,
                this.colTT_Ten,
                this.colTT_Goi,
                this.colTT_GiaTri,
                this.colTT_HinhThuc,
                this.colTT_DaiDien,
                this.colTT_SDT,
                this.colTT_TrangThai
            });
            this.dgvTaiTro.Dock = DockStyle.Fill;
            this.dgvTaiTro.Location = new Point(12, 57);
            this.dgvTaiTro.Name = "dgvTaiTro";
            this.dgvTaiTro.Size = new Size(1212, 543);
            this.dgvTaiTro.TabIndex = 1;

            this.colTT_Ma.DataPropertyName = "MaNhaTaiTro";
            this.colTT_Ma.HeaderText = "Mã";
            this.colTT_Ma.Name = "colTT_Ma";
            this.colTT_Ma.Width = 90;

            this.colTT_Ten.DataPropertyName = "TenToChuc";
            this.colTT_Ten.HeaderText = "Tên tổ chức / Doanh nghiệp";
            this.colTT_Ten.Name = "colTT_Ten";
            this.colTT_Ten.Width = 230;

            this.colTT_Goi.DataPropertyName = "GoiTaiTro";
            this.colTT_Goi.HeaderText = "Gói tài trợ";
            this.colTT_Goi.Name = "colTT_Goi";
            this.colTT_Goi.Width = 130;

            this.colTT_GiaTri.DataPropertyName = "GiaTriFormatted";
            this.colTT_GiaTri.HeaderText = "Tổng giá trị";
            this.colTT_GiaTri.Name = "colTT_GiaTri";
            this.colTT_GiaTri.Width = 150;

            this.colTT_HinhThuc.DataPropertyName = "HinhThucTaiTro";
            this.colTT_HinhThuc.HeaderText = "Hình thức";
            this.colTT_HinhThuc.Name = "colTT_HinhThuc";
            this.colTT_HinhThuc.Width = 130;

            this.colTT_DaiDien.DataPropertyName = "NguoiDaiDien";
            this.colTT_DaiDien.HeaderText = "Người đại diện";
            this.colTT_DaiDien.Name = "colTT_DaiDien";
            this.colTT_DaiDien.Width = 160;

            this.colTT_SDT.DataPropertyName = "SoDienThoai";
            this.colTT_SDT.HeaderText = "Điện thoại";
            this.colTT_SDT.Name = "colTT_SDT";
            this.colTT_SDT.Width = 120;

            this.colTT_TrangThai.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colTT_TrangThai.DataPropertyName = "TrangThai";
            this.colTT_TrangThai.HeaderText = "Trạng thái";
            this.colTT_TrangThai.Name = "colTT_TrangThai";

            // ------------------------------------------
            // TAB 8: CHI PHI & NGAN SACH
            // ------------------------------------------
            this.tabChiPhi.BackColor = Color.White;
            this.tabChiPhi.Controls.Add(this.dgvChiPhi);
            this.tabChiPhi.Controls.Add(this.pnlTab8Toolbar);
            this.tabChiPhi.Location = new Point(4, 38);
            this.tabChiPhi.Name = "tabChiPhi";
            this.tabChiPhi.Padding = new Padding(12);
            this.tabChiPhi.Size = new Size(1236, 612);
            this.tabChiPhi.TabIndex = 7;
            this.tabChiPhi.Text = "💰 Ngân sách & Chi phí";

            this.pnlTab8Toolbar.BackColor = Color.FromArgb(248, 250, 252);
            this.pnlTab8Toolbar.BorderStyle = BorderStyle.FixedSingle;
            this.pnlTab8Toolbar.Controls.Add(this.btnMoF17);
            this.pnlTab8Toolbar.Controls.Add(this.lblNganSachInfo);
            this.pnlTab8Toolbar.Controls.Add(this.prgNganSach);
            this.pnlTab8Toolbar.Controls.Add(this.lblThanhDoNganSach);
            this.pnlTab8Toolbar.Controls.Add(this.lblTab8Header);
            this.pnlTab8Toolbar.Dock = DockStyle.Top;
            this.pnlTab8Toolbar.Location = new Point(12, 12);
            this.pnlTab8Toolbar.Name = "pnlTab8Toolbar";
            this.pnlTab8Toolbar.Size = new Size(1212, 60);
            this.pnlTab8Toolbar.TabIndex = 0;

            this.lblTab8Header.AutoSize = true;
            this.lblTab8Header.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTab8Header.ForeColor = Color.FromArgb(30, 58, 138);
            this.lblTab8Header.Location = new Point(10, 8);
            this.lblTab8Header.Name = "lblTab8Header";
            this.lblTab8Header.Size = new Size(330, 19);
            this.lblTab8Header.TabIndex = 0;
            this.lblTab8Header.Text = "TIẾN ĐỘ NGÂN SÁCH";

            this.lblThanhDoNganSach.AutoSize = true;
            this.lblThanhDoNganSach.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.lblThanhDoNganSach.ForeColor = Color.FromArgb(71, 85, 105);
            this.lblThanhDoNganSach.Location = new Point(10, 32);
            this.lblThanhDoNganSach.Name = "lblThanhDoNganSach";
            this.lblThanhDoNganSach.Size = new Size(101, 15);
            this.lblThanhDoNganSach.TabIndex = 1;
            this.lblThanhDoNganSach.Text = "Tiến độ tiêu ngân:";

            this.prgNganSach.Location = new Point(115, 31);
            this.prgNganSach.Name = "prgNganSach";
            this.prgNganSach.Size = new Size(300, 18);
            this.prgNganSach.TabIndex = 2;

            this.lblNganSachInfo.AutoSize = true;
            this.lblNganSachInfo.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblNganSachInfo.ForeColor = Color.FromArgb(22, 163, 74);
            this.lblNganSachInfo.Location = new Point(425, 32);
            this.lblNganSachInfo.Name = "lblNganSachInfo";
            this.lblNganSachInfo.Size = new Size(185, 15);
            this.lblNganSachInfo.TabIndex = 3;
            this.lblNganSachInfo.Text = "Ngân sách an toàn (<80% định mức)";

            this.btnMoF17.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMoF17.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnMoF17.Location = new Point(1020, 12);
            this.btnMoF17.Name = "btnMoF17";
            this.btnMoF17.Size = new Size(180, 34);
            this.btnMoF17.TabIndex = 4;
            this.btnMoF17.Text = "Quản lý chi phí";
            this.btnMoF17.UseVisualStyleBackColor = true;
            this.btnMoF17.Click += new EventHandler(this.btnMoF17_Click);

            this.dgvChiPhi.Columns.AddRange(new DataGridViewColumn[] {
                this.colCP_Ma,
                this.colCP_KhoanMuc,
                this.colCP_SoTien,
                this.colCP_NgayChi,
                this.colCP_NguoiLap,
                this.colCP_TrangThai,
                this.colCP_DienGiai
            });
            this.dgvChiPhi.Dock = DockStyle.Fill;
            this.dgvChiPhi.Location = new Point(12, 72);
            this.dgvChiPhi.Name = "dgvChiPhi";
            this.dgvChiPhi.Size = new Size(1212, 528);
            this.dgvChiPhi.TabIndex = 1;

            this.colCP_Ma.DataPropertyName = "MaChiPhi";
            this.colCP_Ma.HeaderText = "Mã chi";
            this.colCP_Ma.Name = "colCP_Ma";
            this.colCP_Ma.Width = 100;

            this.colCP_KhoanMuc.DataPropertyName = "KhoanMucChi";
            this.colCP_KhoanMuc.HeaderText = "Khoản mục chi";
            this.colCP_KhoanMuc.Name = "colCP_KhoanMuc";
            this.colCP_KhoanMuc.Width = 240;

            this.colCP_SoTien.DataPropertyName = "SoTienFormatted";
            this.colCP_SoTien.HeaderText = "Số tiền chi";
            this.colCP_SoTien.Name = "colCP_SoTien";
            this.colCP_SoTien.Width = 150;

            this.colCP_NgayChi.DataPropertyName = "NgayChi";
            this.colCP_NgayChi.HeaderText = "Ngày chi";
            this.colCP_NgayChi.Name = "colCP_NgayChi";
            this.colCP_NgayChi.Width = 120;

            this.colCP_NguoiLap.DataPropertyName = "NguoiLapPhieu";
            this.colCP_NguoiLap.HeaderText = "Người lập";
            this.colCP_NguoiLap.Name = "colCP_NguoiLap";
            this.colCP_NguoiLap.Width = 160;

            this.colCP_TrangThai.DataPropertyName = "TrangThai";
            this.colCP_TrangThai.HeaderText = "Trạng thái";
            this.colCP_TrangThai.Name = "colCP_TrangThai";
            this.colCP_TrangThai.Width = 130;

            this.colCP_DienGiai.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colCP_DienGiai.DataPropertyName = "DienGiai";
            this.colCP_DienGiai.HeaderText = "Diễn giải chi tiết";
            this.colCP_DienGiai.Name = "colCP_DienGiai";

            // ------------------------------------------
            // TAB 9: PHAN HOI & DANH GIA
            // ------------------------------------------
            this.tabDanhGia.BackColor = Color.White;
            this.tabDanhGia.Controls.Add(this.dgvDanhGia);
            this.tabDanhGia.Controls.Add(this.pnlTab9Toolbar);
            this.tabDanhGia.Location = new Point(4, 38);
            this.tabDanhGia.Name = "tabDanhGia";
            this.tabDanhGia.Padding = new Padding(12);
            this.tabDanhGia.Size = new Size(1236, 612);
            this.tabDanhGia.TabIndex = 8;
            this.tabDanhGia.Text = "⭐ Khảo sát & Phản hồi";

            this.pnlTab9Toolbar.BackColor = Color.FromArgb(248, 250, 252);
            this.pnlTab9Toolbar.BorderStyle = BorderStyle.FixedSingle;
            this.pnlTab9Toolbar.Controls.Add(this.btnMoF18);
            this.pnlTab9Toolbar.Controls.Add(this.lblTyLeHaiLong);
            this.pnlTab9Toolbar.Controls.Add(this.lblDiemTrungBinh);
            this.pnlTab9Toolbar.Controls.Add(this.lblTab9Header);
            this.pnlTab9Toolbar.Dock = DockStyle.Top;
            this.pnlTab9Toolbar.Location = new Point(12, 12);
            this.pnlTab9Toolbar.Name = "pnlTab9Toolbar";
            this.pnlTab9Toolbar.Size = new Size(1212, 55);
            this.pnlTab9Toolbar.TabIndex = 0;

            this.lblTab9Header.AutoSize = true;
            this.lblTab9Header.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTab9Header.ForeColor = Color.FromArgb(30, 58, 138);
            this.lblTab9Header.Location = new Point(10, 8);
            this.lblTab9Header.Name = "lblTab9Header";
            this.lblTab9Header.Size = new Size(420, 19);
            this.lblTab9Header.TabIndex = 0;
            this.lblTab9Header.Text = "Ý KIẾN ĐÓNG GÓP";

            this.lblDiemTrungBinh.AutoSize = true;
            this.lblDiemTrungBinh.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblDiemTrungBinh.ForeColor = Color.FromArgb(217, 119, 6);
            this.lblDiemTrungBinh.Location = new Point(10, 31);
            this.lblDiemTrungBinh.Name = "lblDiemTrungBinh";
            this.lblDiemTrungBinh.Size = new Size(150, 15);
            this.lblDiemTrungBinh.TabIndex = 1;
            this.lblDiemTrungBinh.Text = "⭐ Điểm trung bình: 0.0 / 5.0";

            this.lblTyLeHaiLong.AutoSize = true;
            this.lblTyLeHaiLong.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblTyLeHaiLong.ForeColor = Color.FromArgb(22, 163, 74);
            this.lblTyLeHaiLong.Location = new Point(220, 31);
            this.lblTyLeHaiLong.Name = "lblTyLeHaiLong";
            this.lblTyLeHaiLong.Size = new Size(140, 15);
            this.lblTyLeHaiLong.TabIndex = 2;
            this.lblTyLeHaiLong.Text = "👍 Tỷ lệ hài lòng: 0% (0 ý kiến)";

            this.btnMoF18.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMoF18.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnMoF18.Location = new Point(1020, 10);
            this.btnMoF18.Name = "btnMoF18";
            this.btnMoF18.Size = new Size(180, 34);
            this.btnMoF18.TabIndex = 3;
            this.btnMoF18.Text = "Quản lý phản hồi";
            this.btnMoF18.UseVisualStyleBackColor = true;
            this.btnMoF18.Click += new EventHandler(this.btnMoF18_Click);

            this.dgvDanhGia.Columns.AddRange(new DataGridViewColumn[] {
                this.colDG_MaPhanHoi,
                this.colDG_NguoiGui,
                this.colDG_Diem,
                this.colDG_NgayGui,
                this.colDG_NoiDung
            });
            this.dgvDanhGia.Dock = DockStyle.Fill;
            this.dgvDanhGia.Location = new Point(12, 67);
            this.dgvDanhGia.Name = "dgvDanhGia";
            this.dgvDanhGia.Size = new Size(1212, 533);
            this.dgvDanhGia.TabIndex = 1;

            this.colDG_MaPhanHoi.DataPropertyName = "MaPhanHoi";
            this.colDG_MaPhanHoi.HeaderText = "Mã phản hồi";
            this.colDG_MaPhanHoi.Name = "colDG_MaPhanHoi";
            this.colDG_MaPhanHoi.Width = 110;

            this.colDG_NguoiGui.DataPropertyName = "TenNguoiGui";
            this.colDG_NguoiGui.HeaderText = "Người gửi đánh giá";
            this.colDG_NguoiGui.Name = "colDG_NguoiGui";
            this.colDG_NguoiGui.Width = 200;

            this.colDG_Diem.DataPropertyName = "DiemDanhGiaFormatted";
            this.colDG_Diem.HeaderText = "Đánh giá";
            this.colDG_Diem.Name = "colDG_Diem";
            this.colDG_Diem.Width = 130;

            this.colDG_NgayGui.DataPropertyName = "NgayGui";
            this.colDG_NgayGui.HeaderText = "Thời điểm gửi";
            this.colDG_NgayGui.Name = "colDG_NgayGui";
            this.colDG_NgayGui.Width = 140;

            this.colDG_NoiDung.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colDG_NoiDung.DataPropertyName = "NoiDungPhanHoi";
            this.colDG_NoiDung.HeaderText = "Nội dung đóng góp ý kiến";
            this.colDG_NoiDung.Name = "colDG_NoiDung";

            // ==========================================
            // pnlBottomActions
            // ==========================================
            this.pnlBottomActions.BackColor = Color.White;
            this.pnlBottomActions.BorderStyle = BorderStyle.FixedSingle;
            this.pnlBottomActions.Controls.Add(this.btnXuatBaoCao);
            this.pnlBottomActions.Controls.Add(this.btnLamMoi);
            this.pnlBottomActions.Controls.Add(this.btnDong);
            this.pnlBottomActions.Dock = DockStyle.Bottom;
            this.pnlBottomActions.Location = new Point(0, 788);
            this.pnlBottomActions.Name = "pnlBottomActions";
            this.pnlBottomActions.Padding = new Padding(16, 8, 16, 8);
            this.pnlBottomActions.Size = new Size(1244, 52);
            this.pnlBottomActions.TabIndex = 3;

            // btnDong
            this.btnDong.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnDong.Location = new Point(16, 8);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new Size(120, 36);
            this.btnDong.TabIndex = 0;
            this.btnDong.Text = "🚪 Đóng (Esc)";
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new EventHandler(this.btnDong_Click);

            // btnLamMoi
            this.btnLamMoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnLamMoi.Location = new Point(145, 8);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new Size(130, 36);
            this.btnLamMoi.TabIndex = 1;
            this.btnLamMoi.Text = "🔄 Làm mới (F5)";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new EventHandler(this.btnLamMoi_Click);

            // btnXuatBaoCao
            this.btnXuatBaoCao.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnXuatBaoCao.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnXuatBaoCao.Location = new Point(1040, 8);
            this.btnXuatBaoCao.Name = "btnXuatBaoCao";
            this.btnXuatBaoCao.Size = new Size(190, 36);
            this.btnXuatBaoCao.TabIndex = 2;
            this.btnXuatBaoCao.Text = "📄 Xuất báo cáo (Ctrl+P)";
            this.btnXuatBaoCao.UseVisualStyleBackColor = true;
            this.btnXuatBaoCao.Click += new EventHandler(this.btnXuatBaoCao_Click);

            // ==========================================
            // errBaoLoi
            // ==========================================
            this.errBaoLoi.ContainerControl = this;

            // ==========================================
            // Form Setup
            // ==========================================
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1244, 840);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.pnlBottomActions);
            this.Controls.Add(this.pnlKPICards);
            this.Controls.Add(this.pnlHeader);
            this.KeyPreview = true;
            this.MinimumSize = new Size(1200, 780);
            this.Name = "FrmEventDetail";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.Load += new EventHandler(this.FrmEventDetail_Load);
            this.KeyDown += new KeyEventHandler(this.FrmEventDetail_KeyDown);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlKPICards.ResumeLayout(false);
            this.pnlCardVe.ResumeLayout(false);
            this.pnlCardVe.PerformLayout();
            this.pnlCardCheckIn.ResumeLayout(false);
            this.pnlCardCheckIn.PerformLayout();
            this.pnlCardDoanhThu.ResumeLayout(false);
            this.pnlCardDoanhThu.PerformLayout();
            this.pnlCardChiPhi.ResumeLayout(false);
            this.pnlCardChiPhi.PerformLayout();
            this.pnlCardNganSach.ResumeLayout(false);
            this.pnlCardNganSach.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabThongTin.ResumeLayout(false);
            this.grpThongTinChung.ResumeLayout(false);
            this.grpThongTinChung.PerformLayout();
            this.grpThoiGianNganSach.ResumeLayout(false);
            this.grpThoiGianNganSach.PerformLayout();
            this.pnlTab1Actions.ResumeLayout(false);
            this.tabLichTrinh.ResumeLayout(false);
            this.pnlTab2Toolbar.ResumeLayout(false);
            this.pnlTab2Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichTrinh)).EndInit();
            this.tabDienGia.ResumeLayout(false);
            this.pnlTab3Toolbar.ResumeLayout(false);
            this.pnlTab3Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDienGia)).EndInit();
            this.tabNguoiThamGia.ResumeLayout(false);
            this.pnlTab4Toolbar.ResumeLayout(false);
            this.pnlTab4Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNguoiThamGia)).EndInit();
            this.tabVe.ResumeLayout(false);
            this.pnlTab5Toolbar.ResumeLayout(false);
            this.pnlTab5Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHangVe)).EndInit();
            this.tabNhanSu.ResumeLayout(false);
            this.pnlTab6Toolbar.ResumeLayout(false);
            this.pnlTab6Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanSu)).EndInit();
            this.tabTaiTro.ResumeLayout(false);
            this.pnlTab7Toolbar.ResumeLayout(false);
            this.pnlTab7Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaiTro)).EndInit();
            this.tabChiPhi.ResumeLayout(false);
            this.pnlTab8Toolbar.ResumeLayout(false);
            this.pnlTab8Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiPhi)).EndInit();
            this.tabDanhGia.ResumeLayout(false);
            this.pnlTab9Toolbar.ResumeLayout(false);
            this.pnlTab9Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhGia)).EndInit();
            this.pnlBottomActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSucChua)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNganSach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errBaoLoi)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
