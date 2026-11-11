using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.GUI.Common;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình F05 - Chi tiết sự kiện & Điều phối trung tâm (9 Tabs chức năng)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-14 / Ngày 8)
    /// Tuân thủ: Mẫu bố cục 3 (Archetype 3 - TabControl), Corporate Navy Blue Theme (#1E3A8A)
    /// 9 Tabs: Thông tin, Lịch trình, Diễn giả, Người tham gia, Hạng vé, Nhân sự, Tài trợ, Chi phí, Khảo sát.
    /// </summary>
    public partial class FrmEventDetail : FrmBase
    {
        private readonly IEventService _eventService;
        private readonly IScheduleService _scheduleService;
        private readonly ISpeakerService _speakerService;
        private readonly ITicketOrderService _ticketOrderService;
        private readonly IStaffAssignmentService _staffAssignmentService;
        private readonly ISponsorService _sponsorService;
        private readonly IBudgetService _budgetService;
        private readonly IFeedbackService _feedbackService;
        private readonly Action _onSyncDataChanged;

        private string _maSuKien = string.Empty;
        private EventListDto? _currentEvent;
        private bool _isEditingTab1 = false;
        private bool _isLoading = false;

        private List<EventListDto> _allEvents = new();
        private List<EventTypeDto> _eventTypes = new();
        private List<VenueDto> _venues = new();

        // Data caches for tabs
        private List<ScheduleItemDto> _scheduleCache = new();
        private List<SpeakerDto> _speakerCache = new();
        private List<TicketDetailDto> _attendeeTicketsCache = new();
        private List<TicketTierSummaryDto> _ticketTiersCache = new();
        private List<StaffAssignmentDto> _staffAssignmentsCache = new();
        private List<SponsorDto> _sponsorsCache = new();
        private List<ExpenseDto> _expensesCache = new();
        private List<FeedbackDto> _feedbackCache = new();

        private Button? btnMoF09;
        private Button? btnMoF12;
        private Label? lblTab2Footer;
        private Label? lblTab3Footer;
        private Label? lblTab4Footer;
        private Label? lblTab5Footer;
        private Label? lblTab6Footer;
        private Label? lblTab7Footer;
        private Label? lblTab8Footer;
        private Label? lblTab9Footer;

        private void DistributeKpiCards()
        {
            if (pnlKPICards == null) return;
            var cards = new[] { pnlCardVe, pnlCardCheckIn, pnlCardDoanhThu, pnlCardChiPhi, pnlCardNganSach };
            int padding = 12;
            int gap = 10;
            int totalW = pnlKPICards.ClientSize.Width - (padding * 2) - (gap * (cards.Length - 1));
            if (totalW <= 0) return;
            int cardW = totalW / cards.Length;
            int curLeft = padding;
            for (int i = 0; i < cards.Length; i++)
            {
                int w = (i == cards.Length - 1) ? (pnlKPICards.ClientSize.Width - padding - curLeft) : cardW;
                cards[i].Location = new Point(curLeft, 7);
                cards[i].Size = new Size(Math.Max(100, w), 56);
                curLeft += w + gap;
            }
        }

        private void DistributeTab1()
        {
            if (tabThongTin == null || grpThongTinChung == null || grpThoiGianNganSach == null || pnlTab1Actions == null) return;
            int pad = 15;
            int gap = 15;
            int w = tabThongTin.ClientSize.Width - (pad * 2) - gap;
            int halfW = Math.Max(300, w / 2);
            int h = Math.Max(380, tabThongTin.ClientSize.Height - 75);

            grpThongTinChung.Location = new Point(pad, pad);
            grpThongTinChung.Size = new Size(halfW, h);

            grpThoiGianNganSach.Location = new Point(pad + halfW + gap, pad);
            grpThoiGianNganSach.Size = new Size(Math.Max(300, tabThongTin.ClientSize.Width - pad - (pad + halfW + gap)), h);

            pnlTab1Actions.Location = new Point(pad, tabThongTin.ClientSize.Height - 55);
            pnlTab1Actions.Size = new Size(tabThongTin.ClientSize.Width - (pad * 2), 48);

            int leftInputW = halfW - 25 - txtTenSuKien.Left;
            if (leftInputW > 150)
            {
                txtTenSuKien.Width = leftInputW;
                cboLoaiSK.Width = Math.Min(350, leftInputW);
                cboDiaDiem.Width = leftInputW;
                txtNguoiPhuTrach.Width = Math.Min(350, leftInputW);
                cboTrangThai.Width = Math.Min(220, leftInputW);
            }

            int rightInputW = grpThoiGianNganSach.Width - 25 - txtMoTa.Left;
            if (rightInputW > 150)
            {
                txtMoTa.Width = rightInputW;
                txtMoTa.Height = Math.Max(100, h - txtMoTa.Top - 20);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            DistributeKpiCards();
            DistributeTab1();
            AlignHeaderSelector();
        }

        private void AlignHeaderSelector()
        {
            if (lblChonSuKien == null || cboChonSuKien == null) return;
            lblChonSuKien.Left = cboChonSuKien.Left - lblChonSuKien.Width - 10;
        }

        private Panel CreateTabFooter(TabPage tab, out Label lblFooter)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 32,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(12, 6, 12, 6)
            };
            lblFooter = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(71, 85, 105),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = string.Empty
            };
            pnl.Controls.Add(lblFooter);
            tab.Controls.Add(pnl);
            pnl.SendToBack();
            return pnl;
        }

        public FrmEventDetail() : this(string.Empty)
        {
        }

        public FrmEventDetail(string? maSuKien) : this(
            maSuKien,
            new EventService(),
            new ScheduleService(),
            new SpeakerService(),
            new TicketOrderService(),
            new StaffAssignmentService(),
            new SponsorService(),
            new BudgetService(),
            new FeedbackService())
        {
        }

        public FrmEventDetail(
            string? maSuKien,
            IEventService eventService,
            IScheduleService scheduleService,
            ISpeakerService speakerService,
            ITicketOrderService ticketOrderService,
            IStaffAssignmentService staffAssignmentService,
            ISponsorService sponsorService,
            IBudgetService budgetService,
            IFeedbackService feedbackService)
        {
            InitializeComponent();

            _maSuKien = maSuKien?.Trim() ?? string.Empty;
            _eventService = eventService ?? new EventService();
            _scheduleService = scheduleService ?? new ScheduleService();
            _speakerService = speakerService ?? new SpeakerService();
            _ticketOrderService = ticketOrderService ?? new TicketOrderService();
            _staffAssignmentService = staffAssignmentService ?? new StaffAssignmentService();
            _sponsorService = sponsorService ?? new SponsorService();
            _budgetService = budgetService ?? new BudgetService();
            _feedbackService = feedbackService ?? new FeedbackService();

            // Đăng ký nhận thông báo đồng bộ dữ liệu thời gian thực (Task T3-24)
            _onSyncDataChanged = () =>
            {
                if (IsHandleCreated && !IsDisposed && !string.IsNullOrEmpty(_maSuKien))
                {
                    try
                    {
                        BeginInvoke(new Action(() => ReloadAllTabs()));
                    }
                    catch
                    {
                        // Bỏ qua khi form đang đóng
                    }
                }
            };
            DataSyncNotifier.GeneralDataChanged += _onSyncDataChanged;
            this.FormClosed += (s, e) =>
            {
                DataSyncNotifier.GeneralDataChanged -= _onSyncDataChanged;
            };
        }

        private void FrmEventDetail_Load(object sender, EventArgs e)
        {
            _isLoading = true;
            try
            {
                ApplyCustomStyles();
                LoadLookups();
                LoadEventSelector();

                if (!string.IsNullOrWhiteSpace(_maSuKien))
                {
                    SelectEventInCombo(_maSuKien);
                }
                else if (cboChonSuKien.Items.Count > 0)
                {
                    cboChonSuKien.SelectedIndex = 0;
                }

                ReloadAllTabs();
            }
            finally
            {
                _isLoading = false;
            }
        }

        #region Styling & Lookups

        private void ApplyCustomStyles()
        {
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            lblTitle.Text = "CHI TIẾT SỰ KIỆN";
            lblSubtitle.Visible = false;
            lblTitle.Location = new Point(16, 19);
            AlignHeaderSelector();

            tabMain.SizeMode = TabSizeMode.Normal;
            tabMain.Padding = new Point(14, 8);

            lblTab2Header.UseMnemonic = false;
            lblTab3Header.UseMnemonic = false;
            lblTab4Header.UseMnemonic = false;
            lblTab5Header.UseMnemonic = false;
            lblTab6Header.UseMnemonic = false;
            lblTab7Header.UseMnemonic = false;
            lblTab8Header.UseMnemonic = false;
            lblTab9Header.UseMnemonic = false;

            // Button Roles & Styling
            StyleButton(btnSuaThongTin, ButtonRole.Primary);
            StyleButton(btnLuuThongTin, ButtonRole.Success);
            StyleButton(btnHuyThongTin, ButtonRole.Secondary);

            StyleButton(btnMoF08, ButtonRole.Primary);
            btnMoF08.Text = "Quản lý lịch trình";

            StyleButton(btnMoF15, ButtonRole.Primary);
            btnMoF15.Text = "Quản lý phân công";

            StyleButton(btnMoF16, ButtonRole.Primary);
            btnMoF16.Text = "Quản lý tài trợ";

            StyleButton(btnMoF17, ButtonRole.Primary);
            btnMoF17.Text = "Quản lý chi phí";

            StyleButton(btnMoF18, ButtonRole.Primary);
            btnMoF18.Text = "Quản lý phản hồi";
            btnMoF18.Visible = false; // Form 18 (Quản lý phản hồi) đã được lược bỏ khỏi hệ thống

            StyleButton(btnDong, ButtonRole.Secondary);
            StyleButton(btnLamMoi, ButtonRole.Secondary);
            StyleButton(btnXuatBaoCao, ButtonRole.Primary);

            // Tab 3 Button
            if (btnMoF09 == null)
            {
                btnMoF09 = new Button
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                    Location = new Point(pnlTab3Toolbar.Width - 225, 5),
                    Size = new Size(215, 34),
                    Text = "Quản lý diễn giả & khách mời"
                };
                StyleButton(btnMoF09, ButtonRole.Primary);
                btnMoF09.Click += (s, e) => OpenSpeakerForm();
                pnlTab3Toolbar.Controls.Add(btnMoF09);
            }

            // Tab 5 Button
            if (btnMoF12 == null)
            {
                btnMoF12 = new Button
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                    Location = new Point(pnlTab5Toolbar.Width - 190, 5),
                    Size = new Size(180, 34),
                    Text = "Quản lý hạng vé"
                };
                StyleButton(btnMoF12, ButtonRole.Primary);
                btnMoF12.Click += (s, e) => OpenTicketTierForm();
                pnlTab5Toolbar.Controls.Add(btnMoF12);
            }

            // Create Footers
            if (lblTab2Footer == null) CreateTabFooter(tabLichTrinh, out lblTab2Footer);
            if (lblTab3Footer == null) CreateTabFooter(tabDienGia, out lblTab3Footer);
            if (lblTab4Footer == null) CreateTabFooter(tabNguoiThamGia, out lblTab4Footer);
            if (lblTab5Footer == null) CreateTabFooter(tabVe, out lblTab5Footer);
            if (lblTab6Footer == null) CreateTabFooter(tabNhanSu, out lblTab6Footer);
            if (lblTab7Footer == null) CreateTabFooter(tabTaiTro, out lblTab7Footer);
            if (lblTab8Footer == null) CreateTabFooter(tabChiPhi, out lblTab8Footer);
            if (lblTab9Footer == null) CreateTabFooter(tabDanhGia, out lblTab9Footer);

            // Send toolbars and footers to back so they dock to Top/Bottom edges first
            pnlTab2Toolbar.SendToBack();
            pnlTab3Toolbar.SendToBack();
            pnlTab4Toolbar.SendToBack();
            pnlTab5Toolbar.SendToBack();
            pnlTab6Toolbar.SendToBack();
            pnlTab7Toolbar.SendToBack();
            pnlTab8Toolbar.SendToBack();
            pnlTab9Toolbar.SendToBack();

            // Bring DataGridViews to front so Dock.Fill occupies only the space between Top toolbar and Bottom footer
            dgvLichTrinh.BringToFront();
            dgvDienGia.BringToFront();
            dgvNguoiThamGia.BringToFront();
            dgvHangVe.BringToFront();
            dgvNhanSu.BringToFront();
            dgvTaiTro.BringToFront();
            dgvChiPhi.BringToFront();
            dgvDanhGia.BringToFront();

            // Style all 8 DataGridViews
            DataGridView[] allGrids = { dgvLichTrinh, dgvDienGia, dgvNguoiThamGia, dgvHangVe, dgvNhanSu, dgvTaiTro, dgvChiPhi, dgvDanhGia };
            foreach (var dgv in allGrids)
            {
                dgv.AutoGenerateColumns = false;
                StyleDataGridView(dgv);
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersHeight = 36;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgv.RowHeadersVisible = false;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            // Wire double-click drill-down navigation
            dgvLichTrinh.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenScheduleForm(); };
            dgvDienGia.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenSpeakerForm(); };
            dgvNguoiThamGia.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenAttendeeForm(); };
            dgvHangVe.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenTicketTierForm(); };
            dgvNhanSu.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenStaffAssignmentForm(); };
            dgvTaiTro.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenSponsorForm(); };
            dgvChiPhi.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenBudgetExpenseForm(); };
            dgvDanhGia.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenFeedbackForm(); };

            // Tab 2
            colLT_Ma.HeaderText = "Mã Lịch Trình";
            colLT_Ten.HeaderText = "Tên Tiết Mục / Hoạt Động";
            colLT_BatDau.HeaderText = "Thời Gian Bắt Đầu";
            colLT_KetThuc.HeaderText = "Thời Gian Kết Thúc";
            colLT_DienGia.HeaderText = "Diễn Giả Đảm Nhận";
            colLT_Phong.Visible = false;
            colLT_GhiChu.Visible = false;
            colLT_Ma.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colLT_BatDau.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colLT_KetThuc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colLT_BatDau.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colLT_KetThuc.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

            // Tab 3
            colDG_Ma.HeaderText = "Mã Diễn Giả"; colDG_Ma.Width = 100;
            colDG_Ten.HeaderText = "Họ và Tên"; colDG_Ten.Width = 200;
            colDG_ChucVu.HeaderText = "Chức Vụ"; colDG_ChucVu.Width = 180;
            colDG_DonVi.HeaderText = "Đơn Vị Công Tác"; colDG_DonVi.Width = 220;
            colDG_Email.HeaderText = "Email"; colDG_Email.Width = 190;
            colDG_SDT.HeaderText = "Số Điện Thoại"; colDG_SDT.Width = 110;
            colDG_TietMuc.HeaderText = "Tiết Mục Đảm Nhận"; colDG_TietMuc.Width = 130;
            colDG_TrangThai.HeaderText = "Trạng Thái";
            colDG_Ma.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDG_SDT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDG_TietMuc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDG_TrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Tab 4
            colNTG_MaVe.HeaderText = "Mã Vé";
            colNTG_Ten.HeaderText = "Họ và Tên";
            colNTG_HangVe.HeaderText = "Hạng Vé";
            colNTG_Email.HeaderText = "Email";
            colNTG_MaQR.HeaderText = "Mã QR";
            colNTG_NgayDangKy.HeaderText = "Ngày Đăng Ký";
            colNTG_TrangThai.HeaderText = "Trạng Thái Vé";
            colNTG_MaVe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNTG_MaQR.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNTG_NgayDangKy.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNTG_NgayDangKy.DefaultCellStyle.Format = "dd/MM/yyyy";
            colNTG_TrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Tab 5
            colHV_Ma.HeaderText = "Mã Hạng Vé";
            colHV_Ten.HeaderText = "Tên Hạng Vé";
            colHV_Gia.HeaderText = "Giá Vé";
            colHV_PhatHanh.HeaderText = "Phát Hành";
            colHV_DaBan.HeaderText = "Đã Bán";
            colHV_ConLai.HeaderText = "Còn Lại";
            colHV_TyLe.HeaderText = "Tỷ Lệ Bán";
            colHV_DoanhThu.HeaderText = "Doanh Thu Dự Kiến";
            colHV_Ma.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHV_Gia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colHV_PhatHanh.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colHV_DaBan.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colHV_ConLai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colHV_TyLe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colHV_DoanhThu.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Tab 6
            colNS_MaPhanCong.HeaderText = "Mã Phân Công"; colNS_MaPhanCong.Width = 120;
            colNS_MaNhanSu.HeaderText = "Mã Nhân Sự"; colNS_MaNhanSu.Width = 110;
            colNS_Ten.HeaderText = "Họ và Tên Nhân Sự"; colNS_Ten.Width = 220;
            colNS_PhongBan.HeaderText = "Phòng Ban"; colNS_PhongBan.Width = 150;
            colNS_NhiemVu.HeaderText = "Nhiệm Vụ Phân Công"; colNS_NhiemVu.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNS_BatDau.HeaderText = "Thời Gian Bắt Đầu"; colNS_BatDau.Width = 150;
            colNS_KetThuc.HeaderText = "Thời Gian Kết Thúc"; colNS_KetThuc.Width = 150;
            colNS_TrangThai.Visible = false;
            colNS_MaPhanCong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNS_MaNhanSu.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNS_BatDau.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNS_KetThuc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNS_BatDau.DefaultCellStyle.Format = "HH:mm dd/MM/yyyy";
            colNS_KetThuc.DefaultCellStyle.Format = "HH:mm dd/MM/yyyy";

            // Tab 7
            colTT_Ma.HeaderText = "Mã NTT"; colTT_Ma.Width = 85;
            colTT_Ten.HeaderText = "Tên Tổ Chức Tài Trợ"; colTT_Ten.Width = 250;
            colTT_DaiDien.HeaderText = "Người Đại Diện"; colTT_DaiDien.Width = 170;
            colTT_GiaTri.HeaderText = "Giá Trị Tài Trợ"; colTT_GiaTri.Width = 140;
            colTT_HinhThuc.HeaderText = "Hình Thức"; colTT_HinhThuc.Width = 100;
            colTT_SDT.HeaderText = "Số Điện Thoại"; colTT_SDT.Width = 105;
            colTT_Goi.Visible = false;
            colTT_TrangThai.Visible = false;
            colTT_Ma.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTT_GiaTri.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTT_SDT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (dgvTaiTro.Columns["colTT_Email"] == null)
            {
                var colTT_Email = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Email",
                    HeaderText = "Email",
                    Width = 180,
                    Name = "colTT_Email"
                };
                colTT_Email.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTaiTro.Columns.Add(colTT_Email);
            }
            if (dgvTaiTro.Columns["colTT_DiaChi"] == null)
            {
                var colTT_DiaChi = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "DiaChi",
                    HeaderText = "Địa Chỉ",
                    Width = 200,
                    Name = "colTT_DiaChi"
                };
                colTT_DiaChi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTaiTro.Columns.Add(colTT_DiaChi);
            }
            if (dgvTaiTro.Columns["colTT_GhiChu"] == null)
            {
                var colTT_GhiChu = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "GhiChu",
                    HeaderText = "Ghi Chú",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    Name = "colTT_GhiChu"
                };
                colTT_GhiChu.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTaiTro.Columns.Add(colTT_GhiChu);
            }

            // Tab 8
            lblTab8Header.Text = "TIẾN ĐỘ NGÂN SÁCH";
            lblTab8Header.Location = new Point(10, 12);
            lblThanhDoNganSach.Location = new Point(190, 14);
            prgNganSach.Location = new Point(310, 12); prgNganSach.Size = new Size(200, 20);
            lblNganSachInfo.Location = new Point(520, 14);
            btnMoF17.Location = new Point(pnlTab8Toolbar.Width - 190, 6);
            pnlTab8Toolbar.Height = 45;

            colCP_Ma.HeaderText = "Mã Chi Phí";
            colCP_KhoanMuc.HeaderText = "Khoản Mục Chi";
            colCP_SoTien.HeaderText = "Số Tiền Chi";
            colCP_NgayChi.HeaderText = "Ngày Chi";
            colCP_NguoiLap.HeaderText = "Người Đề Xuất / Duyệt";
            colCP_TrangThai.HeaderText = "Trạng Thái";
            colCP_DienGiai.HeaderText = "Diễn Giải Chi Tiết";
            colCP_Ma.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCP_SoTien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCP_NgayChi.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCP_NgayChi.DefaultCellStyle.Format = "dd/MM/yyyy";
            colCP_TrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Tab 9
            lblTab9Header.Text = "Ý KIẾN ĐÓNG GÓP";
            lblTab9Header.Location = new Point(10, 8);
            lblDiemTrungBinh.Location = new Point(10, 32);
            lblTyLeHaiLong.Location = new Point(230, 32);
            btnMoF18.Location = new Point(pnlTab9Toolbar.Width - 190, 10);
            pnlTab9Toolbar.Height = 58;

            colDG_MaPhanHoi.HeaderText = "Mã Phản Hồi";
            colDG_NguoiGui.HeaderText = "Người Tham Gia";
            colDG_Diem.HeaderText = "Điểm Đánh Giá";
            colDG_NgayGui.HeaderText = "Ngày Đánh Giá";
            colDG_NoiDung.HeaderText = "Nội Dung Ý Kiến";
            colDG_MaPhanHoi.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDG_Diem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDG_NgayGui.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Wire responsive events
            pnlKPICards.Resize += (s, e) => DistributeKpiCards();
            tabThongTin.Resize += (s, e) => DistributeTab1();
            DistributeKpiCards();
            DistributeTab1();

            SetTab1EditMode(false);
        }


        private void LoadLookups()
        {
            // Load Event Types
            _eventTypes = _eventService.GetAvailableEventTypes() ?? new List<EventTypeDto>();
            cboLoaiSK.Items.Clear();
            foreach (var t in _eventTypes)
            {
                cboLoaiSK.Items.Add(new ComboBoxItem(t.MaLoaiSK, t.TenLoaiSK));
            }

            // Load Venues
            _venues = _eventService.GetAvailableVenues() ?? new List<VenueDto>();
            cboDiaDiem.Items.Clear();
            foreach (var v in _venues)
            {
                cboDiaDiem.Items.Add(new ComboBoxItem(v.MaDiaDiem, $"{v.TenDiaDiem} ({v.SucChuaToiDa:N0} chỗ)"));
            }
        }

        private void LoadEventSelector()
        {
            var res = _eventService.GetAll();
            _allEvents = res.IsSuccess && res.Data != null ? res.Data : new List<EventListDto>();

            cboChonSuKien.Items.Clear();
            foreach (var ev in _allEvents)
            {
                cboChonSuKien.Items.Add(new ComboBoxItem(ev.MaSuKien, $"{ev.MaSuKien} - {ev.TenSuKien}"));
            }
        }

        private void SelectEventInCombo(string maSuKien)
        {
            for (int i = 0; i < cboChonSuKien.Items.Count; i++)
            {
                if (cboChonSuKien.Items[i] is ComboBoxItem item &&
                    string.Equals(item.Value?.ToString(), maSuKien, StringComparison.OrdinalIgnoreCase))
                {
                    cboChonSuKien.SelectedIndex = i;
                    return;
                }
            }

            if (cboChonSuKien.Items.Count > 0)
            {
                cboChonSuKien.SelectedIndex = 0;
            }
        }

        #endregion

        #region Master Event Loading & KPI Calculation

        private void cboChonSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            if (cboChonSuKien.SelectedItem is ComboBoxItem item && item.Value != null)
            {
                _maSuKien = item.Value.ToString()!;
                ReloadAllTabs();
            }
        }

        public void ReloadAllTabs()
        {
            if (string.IsNullOrWhiteSpace(_maSuKien))
            {
                var firstEv = _allEvents.FirstOrDefault();
                if (firstEv != null)
                {
                    _maSuKien = firstEv.MaSuKien;
                }
            }

            var res = _eventService.GetById(_maSuKien);
            if (!res.IsSuccess || res.Data == null)
            {
                _currentEvent = _allEvents.FirstOrDefault(x => x.MaSuKien.Equals(_maSuKien, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                _currentEvent = res.Data;
            }

            if (_currentEvent == null)
            {
                return;
            }

            // Tab 1: Basic Info
            PopulateTab1(_currentEvent);
            SetTab1EditMode(false);

            // Tab 2: Schedule
            LoadTab2_LichTrinh(_maSuKien);

            // Tab 3: Speakers
            LoadTab3_DienGia(_maSuKien);

            // Tab 4: Attendees
            LoadTab4_NguoiThamGia(_maSuKien);

            // Tab 5: Ticket Tiers
            LoadTab5_HangVe(_maSuKien);

            // Tab 6: Staff Shift Assignments
            LoadTab6_NhanSu(_maSuKien);

            // Tab 7: Sponsors
            LoadTab7_TaiTro(_maSuKien);

            // Tab 8: Budget & Expenses
            LoadTab8_ChiPhi(_maSuKien);

            // Calculate and display top KPI Cards
            UpdateKPICards();
        }

        private void UpdateKPICards()
        {
            if (_currentEvent == null) return;

            // 1. Ve Da Ban / Suc Chua
            int totalTicketsSold = _currentEvent.SoVeDaBan;
            if (_ticketTiersCache.Count > 0)
            {
                totalTicketsSold = _ticketTiersCache.Sum(t => t.SoLuongDaBan);
            }
            int capacity = _currentEvent.SucChua > 0 ? _currentEvent.SucChua : 1;
            double percentTickets = Math.Round((double)totalTicketsSold / capacity * 100, 1);
            lblCardVeValue.Text = $"{totalTicketsSold:N0} / {capacity:N0} ({percentTickets}%)";

            // 2. Ty Le Check-in
            int checkInCount = _currentEvent.SoKhachCheckIn;
            double percentCheckIn = totalTicketsSold > 0 ? Math.Round((double)checkInCount / totalTicketsSold * 100, 1) : 0;
            lblCardCheckInValue.Text = $"{percentCheckIn}% ({checkInCount:N0} khách)";

            // 3. Doanh Thu Ve
            decimal totalRevenue = 0;
            if (_ticketTiersCache.Count > 0)
            {
                totalRevenue = _ticketTiersCache.Sum(t => t.SoLuongDaBan * t.GiaVe);
            }
            else
            {
                totalRevenue = totalTicketsSold * 250000m;
            }
            lblCardDoanhThuValue.Text = totalRevenue.ToString("#,##0") + " đ";

            // 4. Tong Thuc Chi
            decimal totalSpent = _expensesCache.Sum(x => x.SoTien);
            lblCardChiPhiValue.Text = totalSpent.ToString("#,##0") + " đ";

            // 5. Tien Do Ngan Sach
            decimal budget = _currentEvent.NganSachDuKien;
            decimal remaining = budget - totalSpent;
            double percentBudgetUsed = budget > 0 ? Math.Round((double)totalSpent / (double)budget * 100, 1) : 0;
            double percentRemaining = Math.Max(0, 100.0 - percentBudgetUsed);

            if (percentBudgetUsed > 100)
            {
                lblCardNganSachValue.Text = $"BỘI CHI! (-{Math.Abs(remaining):N0} đ)";
                lblCardNganSachValue.ForeColor = ColorDanger;
            }
            else if (percentBudgetUsed >= 80)
            {
                lblCardNganSachValue.Text = $"Cảnh báo: {remaining:N0} đ ({percentRemaining}%)";
                lblCardNganSachValue.ForeColor = ColorWarning;
            }
            else
            {
                lblCardNganSachValue.Text = $"Còn: {remaining:N0} đ ({percentRemaining}%)";
                lblCardNganSachValue.ForeColor = ColorSuccess;
            }
        }

        #endregion

        #region Tab 1: Thong Tin Co Ban (Basic Info)

        private void PopulateTab1(EventListDto ev)
        {
            txtMaSuKien.Text = ev.MaSuKien;
            txtTenSuKien.Text = ev.TenSuKien;
            txtNguoiPhuTrach.Text = ev.NguoiPhuTrach;
            cboTrangThai.SelectedItem = ev.TrangThai;

            // Select Event Type
            for (int i = 0; i < cboLoaiSK.Items.Count; i++)
            {
                if (cboLoaiSK.Items[i] is ComboBoxItem it &&
                    (string.Equals(it.Value?.ToString(), ev.MaLoaiSK, StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(it.Text, ev.TenLoaiSK, StringComparison.OrdinalIgnoreCase)))
                {
                    cboLoaiSK.SelectedIndex = i;
                    break;
                }
            }

            // Select Venue
            for (int i = 0; i < cboDiaDiem.Items.Count; i++)
            {
                if (cboDiaDiem.Items[i] is ComboBoxItem it &&
                    (string.Equals(it.Value?.ToString(), ev.MaDiaDiem, StringComparison.OrdinalIgnoreCase) ||
                     it.Text.StartsWith(ev.TenDiaDiem, StringComparison.OrdinalIgnoreCase)))
                {
                    cboDiaDiem.SelectedIndex = i;
                    break;
                }
            }

            if (ev.NgayBatDau >= dtpNgayBatDau.MinDate && ev.NgayBatDau <= dtpNgayBatDau.MaxDate)
            {
                dtpNgayBatDau.Value = ev.NgayBatDau;
            }

            if (ev.NgayKetThuc >= dtpNgayKetThuc.MinDate && ev.NgayKetThuc <= dtpNgayKetThuc.MaxDate)
            {
                dtpNgayKetThuc.Value = ev.NgayKetThuc;
            }

            nudSucChua.Value = Math.Clamp(ev.SucChua, nudSucChua.Minimum, nudSucChua.Maximum);
            nudNganSach.Value = Math.Clamp(ev.NganSachDuKien, nudNganSach.Minimum, nudNganSach.Maximum);
            txtMoTa.Text = ev.MoTa ?? string.Empty;
        }

        private void SetTab1EditMode(bool isEditing)
        {
            _isEditingTab1 = isEditing;

            txtTenSuKien.ReadOnly = !isEditing;
            cboLoaiSK.Enabled = isEditing;
            cboDiaDiem.Enabled = isEditing;
            txtNguoiPhuTrach.ReadOnly = !isEditing;
            cboTrangThai.Enabled = isEditing;

            dtpNgayBatDau.Enabled = isEditing;
            dtpNgayKetThuc.Enabled = isEditing;
            nudSucChua.Enabled = isEditing;
            nudNganSach.Enabled = isEditing;
            txtMoTa.ReadOnly = !isEditing;

            Color back = isEditing ? Color.White : Color.FromArgb(248, 250, 252);
            txtTenSuKien.BackColor = back;
            txtNguoiPhuTrach.BackColor = back;
            txtMoTa.BackColor = back;

            btnSuaThongTin.Visible = !isEditing;
            btnLuuThongTin.Visible = isEditing;
            btnHuyThongTin.Visible = isEditing;

            errBaoLoi.Clear();
        }

        private void btnSuaThongTin_Click(object sender, EventArgs e)
        {
            if (_currentEvent == null) return;
            SetTab1EditMode(true);
            txtTenSuKien.Focus();
        }

        private void btnLuuThongTin_Click(object sender, EventArgs e)
        {
            if (_currentEvent == null) return;
            errBaoLoi.Clear();

            string tenSuKien = txtTenSuKien.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenSuKien))
            {
                errBaoLoi.SetError(txtTenSuKien, "Tên sự kiện không được để trống!");
                txtTenSuKien.Focus();
                return;
            }

            if (dtpNgayKetThuc.Value < dtpNgayBatDau.Value)
            {
                errBaoLoi.SetError(dtpNgayKetThuc, "Thời gian kết thúc phải lớn hơn hoặc bằng thời gian bắt đầu (BR-03)!");
                dtpNgayKetThuc.Focus();
                return;
            }

            string maLoai = (cboLoaiSK.SelectedItem as ComboBoxItem)?.Value?.ToString() ?? _currentEvent.MaLoaiSK;
            string tenLoai = (cboLoaiSK.SelectedItem as ComboBoxItem)?.Text ?? _currentEvent.TenLoaiSK;
            string maDiaDiem = (cboDiaDiem.SelectedItem as ComboBoxItem)?.Value?.ToString() ?? _currentEvent.MaDiaDiem;
            string tenDiaDiem = (cboDiaDiem.SelectedItem as ComboBoxItem)?.Text ?? _currentEvent.TenDiaDiem;

            var updatedDto = new EventListDto
            {
                MaSuKien = _currentEvent.MaSuKien,
                TenSuKien = tenSuKien,
                MaLoaiSK = maLoai,
                TenLoaiSK = tenLoai,
                MaDiaDiem = maDiaDiem,
                TenDiaDiem = tenDiaDiem,
                NgayBatDau = dtpNgayBatDau.Value,
                NgayKetThuc = dtpNgayKetThuc.Value,
                SucChua = (int)nudSucChua.Value,
                NganSachDuKien = nudNganSach.Value,
                NguoiPhuTrach = txtNguoiPhuTrach.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? _currentEvent.TrangThai,
                MoTa = txtMoTa.Text.Trim(),
                SoVeDaBan = _currentEvent.SoVeDaBan,
                SoKhachCheckIn = _currentEvent.SoKhachCheckIn
            };

            var saveRes = _eventService.Update(updatedDto);
            if (saveRes.IsSuccess)
            {
                MessageBox.Show("Cập nhật thông tin sự kiện thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _currentEvent = updatedDto;
                SetTab1EditMode(false);
                LoadEventSelector();
                SelectEventInCombo(updatedDto.MaSuKien);
                UpdateKPICards();
            }
            else
            {
                string msg = saveRes.Message;
                if (saveRes.Errors != null && saveRes.Errors.Count > 0)
                {
                    msg += "\n" + string.Join("\n", saveRes.Errors);
                }
                MessageBox.Show(msg, "Lỗi cập nhật sự kiện", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHuyThongTin_Click(object sender, EventArgs e)
        {
            if (_currentEvent != null)
            {
                PopulateTab1(_currentEvent);
            }
            SetTab1EditMode(false);
        }

        #endregion

        #region Tab 2: Lich Trinh (Schedule)

        private void LoadTab2_LichTrinh(string maSuKien)
        {
            var res = _scheduleService.GetAll(maSuKien: maSuKien);
            _scheduleCache = res.IsSuccess && res.Data != null ? res.Data : new List<ScheduleItemDto>();
            dgvLichTrinh.AutoGenerateColumns = false;
            dgvLichTrinh.DataSource = null;
            dgvLichTrinh.DataSource = _scheduleCache;
            lblTab2Header.Text = "DANH SÁCH CÁC TIẾT MỤC / NỘI DUNG CHƯƠNG TRÌNH SỰ KIỆN";
            if (lblTab2Footer != null)
                lblTab2Footer.Text = $"Số tiết mục đang có: {_scheduleCache.Count} tiết mục";
        }

        private void btnMoF08_Click(object sender, EventArgs e)
        {
            using var frm = new FrmSchedule(_maSuKien);
            frm.ShowDialog(this);
            LoadTab2_LichTrinh(_maSuKien);
        }

        #endregion

        #region Tab 3: Dien Gia (Speakers)

        private void LoadTab3_DienGia(string maSuKien)
        {
            var res = _speakerService.GetAll();
            var allSpeakers = res.IsSuccess && res.Data != null ? res.Data : new List<SpeakerDto>();

            var speakerIdsInEvent = _scheduleCache
                .Where(s => !string.IsNullOrEmpty(s.MaDienGia))
                .Select(s => s.MaDienGia!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (speakerIdsInEvent.Count > 0)
            {
                _speakerCache = allSpeakers.Where(s => speakerIdsInEvent.Contains(s.MaDienGia)).ToList();
            }
            else
            {
                _speakerCache = allSpeakers.Take(5).ToList();
            }

            dgvDienGia.AutoGenerateColumns = false;
            dgvDienGia.DataSource = null;
            dgvDienGia.DataSource = _speakerCache;
            lblTab3Header.Text = "DANH SÁCH DIỄN GIẢ & KHÁCH MỜI THAM GIA";
            if (lblTab3Footer != null)
                lblTab3Footer.Text = $"Số diễn giả tham gia: {_speakerCache.Count} diễn giả";
        }

        #endregion

        #region Tab 4: Nguoi Tham Gia (Attendees)

        private void LoadTab4_NguoiThamGia(string maSuKien)
        {
            var res = _ticketOrderService.GetTicketsByEvent(maSuKien);
            _attendeeTicketsCache = res.IsSuccess && res.Data != null ? res.Data : new List<TicketDetailDto>();
            FilterAttendeeGrid();
        }

        private void txtTimNguoiThamGia_TextChanged(object sender, EventArgs e)
        {
            FilterAttendeeGrid();
        }

        private void FilterAttendeeGrid()
        {
            string kw = txtTimNguoiThamGia.Text.Trim().ToLower();
            var filtered = _attendeeTicketsCache;
            if (!string.IsNullOrEmpty(kw))
            {
                filtered = _attendeeTicketsCache.Where(t =>
                    (t.TenNguoiThamGia != null && t.TenNguoiThamGia.ToLower().Contains(kw)) ||
                    (t.Email != null && t.Email.ToLower().Contains(kw)) ||
                    (t.MaVe != null && t.MaVe.ToLower().Contains(kw)) ||
                    (t.MaQR != null && t.MaQR.ToLower().Contains(kw))
                ).ToList();
            }

            dgvNguoiThamGia.AutoGenerateColumns = false;
            dgvNguoiThamGia.DataSource = null;
            dgvNguoiThamGia.DataSource = filtered;
            lblTab4Header.Text = "DANH SÁCH ĐĂNG KÝ VÉ & TÌNH TRẠNG THAM DỰ";
            if (lblTab4Footer != null)
                lblTab4Footer.Text = $"Số người tham gia: {filtered.Count} người";
        }

        #endregion

        #region Tab 5: Hang Ve (Ticket Tiers)

        private void LoadTab5_HangVe(string maSuKien)
        {
            _ticketTiersCache = _ticketOrderService.GetTicketTiersByEvent(maSuKien) ?? new List<TicketTierSummaryDto>();

            var viewItems = _ticketTiersCache.Select(t => new
            {
                t.MaHangVe,
                t.TenHangVe,
                GiaVeFormatted = t.GiaVe.ToString("#,##0") + " VNĐ",
                t.SoLuongPhatHanh,
                t.SoLuongDaBan,
                t.SoLuongConLai,
                TyLeBanFormatted = t.SoLuongPhatHanh > 0 ? $"{(double)t.SoLuongDaBan / t.SoLuongPhatHanh * 100:0.0}%" : "0%",
                DoanhThuFormatted = (t.SoLuongDaBan * t.GiaVe).ToString("#,##0") + " VNĐ"
            }).ToList();

            dgvHangVe.AutoGenerateColumns = false;
            dgvHangVe.DataSource = null;
            dgvHangVe.DataSource = viewItems;
            lblTab5Header.Text = "CƠ CẤU HẠNG VÉ, GIÁ PHÁT HÀNH & TỒN KHO";
            if (lblTab5Footer != null)
                lblTab5Footer.Text = $"Số hạng vé: {viewItems.Count} hạng vé";
        }

        #endregion

        #region Tab 6: Nhan Su (Staff Shift Assignments)

        private void LoadTab6_NhanSu(string maSuKien)
        {
            var res = _staffAssignmentService.GetAll(maSuKien: maSuKien);
            _staffAssignmentsCache = res.IsSuccess && res.Data != null ? res.Data : new List<StaffAssignmentDto>();
            dgvNhanSu.AutoGenerateColumns = false;
            dgvNhanSu.DataSource = null;
            dgvNhanSu.DataSource = _staffAssignmentsCache;
            lblTab6Header.Text = "DANH SÁCH NHÂN SỰ & CA TRỰC";
            if (lblTab6Footer != null)
                lblTab6Footer.Text = $"Số việc phân công: {_staffAssignmentsCache.Count} lượt phân công";
        }

        private void btnMoF15_Click(object sender, EventArgs e)
        {
            using var frm = new FrmStaffAssignment();
            frm.ShowDialog(this);
            LoadTab6_NhanSu(_maSuKien);
        }

        #endregion

        #region Tab 7: Nha Tai Tro (Sponsors)

        private void LoadTab7_TaiTro(string maSuKien)
        {
            var res = _sponsorService.GetAll();
            _sponsorsCache = res.IsSuccess && res.Data != null ? res.Data : new List<SponsorDto>();
            dgvTaiTro.AutoGenerateColumns = false;
            dgvTaiTro.DataSource = null;
            dgvTaiTro.DataSource = _sponsorsCache;
            lblTab7Header.Text = "DANH SÁCH NHÀ TÀI TRỢ & ĐỐI TÁC";
            if (lblTab7Footer != null)
                lblTab7Footer.Text = $"Số nhà tài trợ: {_sponsorsCache.Count} đơn vị";
        }

        private void btnMoF16_Click(object sender, EventArgs e)
        {
            using var frm = new FrmSponsor();
            frm.ShowDialog(this);
            LoadTab7_TaiTro(_maSuKien);
        }

        #endregion

        #region Tab 8: Chi Phi & Ngan Sach (Budget & Expenses)

        private void LoadTab8_ChiPhi(string maSuKien)
        {
            var res = _budgetService.GetExpensesByEvent(maSuKien);
            _expensesCache = res.IsSuccess && res.Data != null ? res.Data : new List<ExpenseDto>();

            var viewExpenses = _expensesCache.Select(x => new
            {
                x.MaChiPhi,
                x.KhoanMucChi,
                SoTienFormatted = x.SoTien.ToString("#,##0") + " VNĐ",
                NgayChi = x.NgayChi.ToString("dd/MM/yyyy"),
                x.NguoiLapPhieu,
                x.TrangThai,
                x.DienGiai
            }).ToList();

            dgvChiPhi.AutoGenerateColumns = false;
            dgvChiPhi.DataSource = null;
            dgvChiPhi.DataSource = viewExpenses;

            // Budget Progress & Alert (BR-14, BR-15)
            var alertRes = _budgetService.GetBudgetStatus(maSuKien);
            if (alertRes.IsSuccess && alertRes.Data != null)
            {
                var alert = alertRes.Data;
                int progressVal = (int)Math.Clamp(Math.Round(alert.TyLeSuDung), 0, 100);
                prgNganSach.Value = progressVal;

                if (alert.IsOverBudget)
                {
                    lblNganSachInfo.Text = $"🚨 BỘI CHI: {alert.TyLeSuDung:F1}% định mức! (Chi {alert.TongThucChiHienTai:N0} đ / Ngân sách {alert.NganSachDuKien:N0} đ)";
                    lblNganSachInfo.ForeColor = ColorDanger;
                }
                else if (alert.IsWarning80)
                {
                    lblNganSachInfo.Text = $"⚠️ CẢNH BÁO: Chi phí đã đạt {alert.TyLeSuDung:F1}% ngân sách! (Chạm ngưỡng 80% BR-15)";
                    lblNganSachInfo.ForeColor = ColorWarning;
                }
                else
                {
                    lblNganSachInfo.Text = $"✅ Ngân sách an toàn: {alert.TyLeSuDung:F1}% định mức (Còn lại {(alert.NganSachDuKien - alert.TongThucChiHienTai):N0} đ)";
                    lblNganSachInfo.ForeColor = ColorSuccess;
                }
            }
            lblTab8Header.Text = "TIẾN ĐỘ NGÂN SÁCH";
            if (lblTab8Footer != null)
                lblTab8Footer.Text = $"Tổng số khoản chi: {_expensesCache.Count} khoản mục";
        }

        private void btnMoF17_Click(object sender, EventArgs e)
        {
            using var frm = new FrmBudgetExpense();
            frm.ShowDialog(this);
            LoadTab8_ChiPhi(_maSuKien);
            UpdateKPICards();
        }

        #endregion

        #region Tab 9: Phan Hoi & Danh Gia (Feedback & Ratings)

        private void LoadTab9_DanhGia(string maSuKien)
        {
            var res = _feedbackService.GetAll(maSuKien: maSuKien);
            _feedbackCache = res.IsSuccess && res.Data != null ? res.Data : new List<FeedbackDto>();

            var viewFeedback = _feedbackCache.Select(f => new
            {
                f.MaPhanHoi,
                TenNguoiGui = !string.IsNullOrEmpty(f.TenNguoiThamGia) ? f.TenNguoiThamGia : "Khách mời ẩn danh",
                DiemDanhGiaFormatted = f.SoSaoText,
                NgayGui = f.NgayDanhGiaFormatted,
                f.NoiDungPhanHoi
            }).ToList();

            dgvDanhGia.AutoGenerateColumns = false;
            dgvDanhGia.DataSource = null;
            dgvDanhGia.DataSource = viewFeedback;

            var sumRes = _feedbackService.GetSummary(maSuKien);
            if (sumRes.IsSuccess && sumRes.Data != null)
            {
                var sum = sumRes.Data;
                lblDiemTrungBinh.Text = $"⭐ Điểm trung bình: {sum.DiemTrungBinh:F1} / 5.0 ({sum.TongSoPhanHoi} phản hồi)";
                lblTyLeHaiLong.Text = $"👍 Tỷ lệ hài lòng: {sum.TyLeHaiLong}% ({sum.SoLuong5Sao + sum.SoLuong4Sao} đánh giá tích cực)";
            }
            else
            {
                lblDiemTrungBinh.Text = $"⭐ Điểm trung bình: 0.0 / 5.0 (0 phản hồi)";
                lblTyLeHaiLong.Text = $"👍 Tỷ lệ hài lòng: 0% (0 đánh giá tích cực)";
            }

            lblTab9Header.Text = "Ý KIẾN ĐÓNG GÓP";
            if (lblTab9Footer != null)
                lblTab9Footer.Text = $"Số ý kiến đóng góp: {_feedbackCache.Count} phản hồi";
        }

        private void btnMoF18_Click(object sender, EventArgs e)
        {
            OpenFeedbackForm();
        }


        #endregion

        #region Bottom Actions & Shortcuts

        #region Public Cross-Form Navigation Methods (Task T2-19)

        public void OpenScheduleForm()
        {
            using var frm = new FrmSchedule(_maSuKien);
            frm.ShowDialog(this);
            LoadTab2_LichTrinh(_maSuKien);
        }

        public void OpenSpeakerForm()
        {
            using var frm = new FrmSpeaker();
            frm.ShowDialog(this);
            LoadTab3_DienGia(_maSuKien);
        }

        public void OpenAttendeeForm()
        {
            using var frm = new FrmAttendee();
            frm.ShowDialog(this);
            LoadTab4_NguoiThamGia(_maSuKien);
        }

        public void OpenTicketOrderForm()
        {
            using var frm = new FrmTicketOrder();
            frm.ShowDialog(this);
            LoadTab4_NguoiThamGia(_maSuKien);
            LoadTab5_HangVe(_maSuKien);
            UpdateKPICards();
        }

        public void OpenTicketTierForm()
        {
            using var frm = new FrmTicketTier();
            frm.ShowDialog(this);
            LoadTab5_HangVe(_maSuKien);
            UpdateKPICards();
        }

        public void OpenStaffForm()
        {
            using var frm = new FrmStaff();
            frm.ShowDialog(this);
        }

        public void OpenStaffAssignmentForm()
        {
            using var frm = new FrmStaffAssignment();
            frm.ShowDialog(this);
            LoadTab6_NhanSu(_maSuKien);
        }

        public void OpenSponsorForm()
        {
            using var frm = new FrmSponsor();
            frm.ShowDialog(this);
            LoadTab7_TaiTro(_maSuKien);
        }

        public void OpenBudgetExpenseForm()
        {
            using var frm = new FrmBudgetExpense();
            frm.ShowDialog(this);
            LoadTab8_ChiPhi(_maSuKien);
            UpdateKPICards();
        }

        public void OpenFeedbackForm()
        {
            MessageBox.Show("Chức năng Quản lý phản hồi (Form 18) đã được lược bỏ khỏi hệ thống theo yêu cầu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void OpenCheckInCameraForm()
        {
            using var frm = new FrmCheckInCamera();
            frm.ShowDialog(this);
            LoadTab4_NguoiThamGia(_maSuKien);
            UpdateKPICards();
        }

        public void OpenReportStatisticsForm()
        {
            using var frm = new FrmReportStatistics();
            frm.ShowDialog(this);
        }

        #endregion

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ReloadAllTabs();
            MessageBox.Show("Đã làm mới dữ liệu sự kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            if (_currentEvent == null)
            {
                MessageBox.Show("Vui lòng chọn sự kiện hợp lệ để xuất báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("==========================================================================");
            sb.AppendLine("                 BÁO CÁO TỔNG KẾT ĐIỀU PHỐI SỰ KIỆN");
            sb.AppendLine("==========================================================================");
            sb.AppendLine($"Mã sự kiện:      {_currentEvent.MaSuKien}");
            sb.AppendLine($"Tên sự kiện:     {_currentEvent.TenSuKien}");
            sb.AppendLine($"Loại sự kiện:    {_currentEvent.TenLoaiSK}");
            sb.AppendLine($"Địa điểm:        {_currentEvent.TenDiaDiem}");
            sb.AppendLine($"Thời gian:       {_currentEvent.NgayBatDau:dd/MM/yyyy HH:mm} - {_currentEvent.NgayKetThuc:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Người phụ trách: {_currentEvent.NguoiPhuTrach}");
            sb.AppendLine($"Trạng thái:      {_currentEvent.TrangThai}");
            sb.AppendLine("--------------------------------------------------------------------------");
            sb.AppendLine("CHỈ SỐ TỔNG HỢP (9 PHÂN HỆ):");
            sb.AppendLine($"- Sức chứa tối đa:             {_currentEvent.SucChua:N0} người");
            sb.AppendLine($"- Số vé đã bán ra:             {_currentEvent.SoVeDaBan:N0} vé ({lblCardVeValue.Text})");
            sb.AppendLine($"- Tỷ lệ check-in thực tế:      {lblCardCheckInValue.Text}");
            sb.AppendLine($"- Tổng doanh thu vé:           {lblCardDoanhThuValue.Text}");
            sb.AppendLine($"- Ngân sách dự kiến:           {_currentEvent.NganSachDuKien:N0} VNĐ");
            sb.AppendLine($"- Tổng chi phí thực tế:        {lblCardChiPhiValue.Text}");
            sb.AppendLine($"- Tình trạng ngân sách:        {lblCardNganSachValue.Text}");
            sb.AppendLine($"- Số lượng tiết mục lịch trình: {_scheduleCache.Count} tiết mục");
            sb.AppendLine($"- Số lượng diễn giả tham gia:   {_speakerCache.Count} diễn giả");
            sb.AppendLine($"- Số lượng nhân sự điều phối:   {_staffAssignmentsCache.Count} ca trực");
            sb.AppendLine($"- Số nhà tài trợ đồng hành:     {_sponsorsCache.Count} đơn vị");
            sb.AppendLine($"- Điểm đánh giá chất lượng:    {lblDiemTrungBinh.Text} | {lblTyLeHaiLong.Text}");
            sb.AppendLine("==========================================================================");

            using var sfd = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                FileName = $"BaoCao_DieuPhoi_{_currentEvent.MaSuKien}_{DateTime.Now:yyyyMMdd}.txt",
                Title = "Lưu báo cáo tổng hợp điều phối sự kiện"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show($"Xuất báo cáo tổng kết sự kiện thành công!\nĐường dẫn: {sfd.FileName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FrmEventDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnLamMoi_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F3)
            {
                if (!_isEditingTab1)
                {
                    btnSuaThongTin_Click(sender, e);
                    e.Handled = true;
                }
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (_isEditingTab1)
                {
                    btnLuuThongTin_Click(sender, e);
                    e.Handled = true;
                }
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                btnXuatBaoCao_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (_isEditingTab1)
                {
                    btnHuyThongTin_Click(sender, e);
                    e.Handled = true;
                }
                else
                {
                    btnDong_Click(sender, e);
                    e.Handled = true;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                btnLamMoi_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == Keys.F3 && !_isEditingTab1)
            {
                btnSuaThongTin_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == (Keys.Control | Keys.S) && _isEditingTab1)
            {
                btnLuuThongTin_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == (Keys.Control | Keys.P))
            {
                btnXuatBaoCao_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == Keys.Escape)
            {
                if (_isEditingTab1)
                {
                    btnHuyThongTin_Click(this, EventArgs.Empty);
                }
                else
                {
                    btnDong_Click(this, EventArgs.Empty);
                }
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        private class ComboBoxItem
        {
            public string Value { get; set; }
            public string Text { get; set; }

            public ComboBoxItem(string value, string text)
            {
                Value = value;
                Text = text;
            }

            public override string ToString() => Text;
        }
    }
}
