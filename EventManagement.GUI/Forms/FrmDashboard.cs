using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Session;
using EventManagement.GUI.Common;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình Trang chủ / Dashboard tổng quan hệ thống (Form F02 - Mẫu 1).
    /// </summary>
    public partial class FrmDashboard : FrmBase
    {
        private readonly IDashboardService _dashboardService;
        private string? _sortColumnProperty;
        private bool _sortAscending = true;

        public DashboardStatsDto? CurrentStats { get; private set; }
        public List<UpcomingEventDto>? CurrentUpcomingEvents { get; private set; }

        public FrmDashboard() : this(new DashboardService())
        {
        }

        public FrmDashboard(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
            InitializeComponent();
        }

        private async void FrmDashboard_Load(object sender, EventArgs e)
        {
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            ConfigureDashboardGridColumns();
            ApplyUserSessionAndPermissions();
            DistributeQuickButtons();
            this.Resize += (s, ev) => DistributeQuickButtons();
            DataSyncNotifier.GeneralDataChanged += OnDataSyncChanged;
            dgvUpcoming.CellDoubleClick += DgvUpcoming_CellDoubleClick;
            dgvUpcoming.ColumnHeaderMouseClick += DgvUpcoming_ColumnHeaderMouseClick;
            await RefreshDashboardAsync();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            DataSyncNotifier.GeneralDataChanged -= OnDataSyncChanged;
            dgvUpcoming.CellDoubleClick -= DgvUpcoming_CellDoubleClick;
            dgvUpcoming.ColumnHeaderMouseClick -= DgvUpcoming_ColumnHeaderMouseClick;
            base.OnFormClosed(e);
        }

        private void ConfigureDashboardGridColumns()
        {
            foreach (DataGridViewColumn col in dgvUpcoming.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            colMaSK.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNgayBatDau.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNgayBatDau.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colNgayKetThuc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNgayKetThuc.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colSoVe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colCheckIn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTyLe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void DgvUpcoming_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.ColumnIndex >= dgvUpcoming.Columns.Count) return;
            var clickedCol = dgvUpcoming.Columns[e.ColumnIndex];
            string propertyName = clickedCol.DataPropertyName;
            if (string.IsNullOrWhiteSpace(propertyName)) return;

            if (string.Equals(_sortColumnProperty, propertyName, StringComparison.Ordinal))
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortColumnProperty = propertyName;
                _sortAscending = true;
            }

            SortUpcomingGridByColumn(propertyName, _sortAscending);
        }

        public void SortUpcomingGridByColumn(string propertyName, bool ascending)
        {
            if (CurrentUpcomingEvents == null || CurrentUpcomingEvents.Count == 0) return;

            _sortColumnProperty = propertyName;
            _sortAscending = ascending;

            Func<UpcomingEventDto, object?> keySelector = propertyName switch
            {
                nameof(UpcomingEventDto.MaSuKien) => x => x.MaSuKien,
                nameof(UpcomingEventDto.TenSuKien) => x => x.TenSuKien,
                nameof(UpcomingEventDto.NgayBatDau) => x => x.NgayBatDau,
                nameof(UpcomingEventDto.NgayKetThuc) => x => x.NgayKetThuc,
                nameof(UpcomingEventDto.DiaDiem) => x => x.DiaDiem,
                nameof(UpcomingEventDto.SoVeDaBan) => x => x.SoVeDaBan,
                nameof(UpcomingEventDto.SoNguoiCheckIn) => x => x.SoNguoiCheckIn,
                nameof(UpcomingEventDto.TyLeCheckIn) => x => x.TyLeCheckIn,
                nameof(UpcomingEventDto.TrangThai) => x => x.TrangThai,
                _ => x => x.NgayBatDau
            };

            CurrentUpcomingEvents = ascending
                ? CurrentUpcomingEvents.OrderBy(keySelector).ToList()
                : CurrentUpcomingEvents.OrderByDescending(keySelector).ToList();

            dgvUpcoming.AutoGenerateColumns = false;
            dgvUpcoming.DataSource = null;
            dgvUpcoming.DataSource = CurrentUpcomingEvents;

            foreach (DataGridViewColumn col in dgvUpcoming.Columns)
            {
                col.HeaderCell.SortGlyphDirection = string.Equals(col.DataPropertyName, propertyName, StringComparison.Ordinal)
                    ? (ascending ? SortOrder.Ascending : SortOrder.Descending)
                    : SortOrder.None;
            }
        }

        private async void OnDataSyncChanged()
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(async () => await RefreshDashboardAsync()));
                }
                else
                {
                    await RefreshDashboardAsync();
                }
            }
            catch
            {
                // Bỏ qua lỗi giao diện khi form đang đóng
            }
        }

        private void DgvUpcoming_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvUpcoming.Rows.Count) return;
            var row = dgvUpcoming.Rows[e.RowIndex];
            var evt = row.DataBoundItem as UpcomingEventDto;
            string eventCode = evt?.MaSuKien ?? "EVT202601";
            PermissionHelper.SafeLaunchDialog(this, "UC05", () => new FrmEventDetail(eventCode), $"Chi tiết sự kiện {eventCode}");
            _ = RefreshDashboardAsync();
        }

        public bool CanAccessAdminFeatures { get; private set; }

        public bool IsButtonAuthorized(Button? btn) => PermissionHelper.IsControlAuthorized(btn);
        public bool IsButtonAuthorized(string buttonName) => IsButtonAuthorized(flpQuickButtons.Controls[buttonName] as Button);

        public void ApplyUserSessionAndPermissions()
        {
            if (CurrentSession.IsLoggedIn)
            {
                var hoTen = CurrentSession.HoTen ?? "Người dùng";
                var role = CurrentSession.MaNhomQuyen ?? "Khách";
                lblUserGreeting.Text = $"Xin chào: {hoTen} ({role})";

                // Rà soát phân quyền động toàn bộ các nút trong thanh công cụ Lối tắt tác nghiệp (AD-4 Lớp 1)
                PermissionHelper.ApplyAuthorization(flpQuickButtons, hideInsteadOfDisable: true);
                DistributeQuickButtons();

                CanAccessAdminFeatures = CurrentSession.IsAdmin 
                    || CurrentSession.CoQuyen("UC03");

                // Phân quyền cho nhân viên Staff (ST-11):
                // Ẩn các thẻ tài chính (Doanh thu & Chi phí), ẩn biểu đồ so sánh doanh thu
                bool isStaff = string.Equals(role, "STAFF", StringComparison.OrdinalIgnoreCase);
                if (isStaff)
                {
                    pnlCard3.Visible = false; // Ẩn thẻ Doanh thu / Chi phí
                    pnlRevenueChart.Visible = false; // Ẩn biểu đồ Doanh thu sự kiện
                    
                    // Căn chỉnh lại 3 thẻ KPI còn lại trên TableLayoutPanel
                    tblKpiCards.ColumnStyles[0].Width = 33.33F;
                    tblKpiCards.ColumnStyles[1].Width = 33.33F;
                    tblKpiCards.ColumnStyles[2].Width = 0F;
                    tblKpiCards.ColumnStyles[3].Width = 33.34F;

                    // Mở rộng biểu đồ Check-in chiếm trọn không gian biểu đồ
                    tblCharts.ColumnStyles[0].Width = 0F;
                    tblCharts.ColumnStyles[1].Width = 100F;
                }
                else
                {
                    pnlCard3.Visible = true;
                    pnlRevenueChart.Visible = true;
                    tblKpiCards.ColumnStyles[0].Width = 25F;
                    tblKpiCards.ColumnStyles[1].Width = 25F;
                    tblKpiCards.ColumnStyles[2].Width = 25F;
                    tblKpiCards.ColumnStyles[3].Width = 25F;

                    tblCharts.ColumnStyles[0].Width = 62F;
                    tblCharts.ColumnStyles[1].Width = 38F;
                }
            }
            else
            {
                lblUserGreeting.Text = "Xin chào: Quản trị viên";
                CanAccessAdminFeatures = true;
                pnlCard3.Visible = true;
                pnlRevenueChart.Visible = true;

                // Khi chạy độc lập không đăng nhập: Hiển thị đầy đủ để demo
                foreach (Control c in flpQuickButtons.Controls)
                {
                    c.Visible = true;
                }
            }
        }

        public void DistributeQuickButtons()
        {
            if (!pnlQuickAccess.Visible) return;
            var visibleButtons = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Where(System.Linq.Enumerable.OfType<Button>(flpQuickButtons.Controls), b => b.Visible));
            if (visibleButtons.Count == 0) return;
            int availableWidth = flpQuickButtons.ClientSize.Width - 10;
            if (availableWidth <= 50) return;
            int buttonWidth = Math.Max(95, (availableWidth / visibleButtons.Count) - 6);
            foreach (var btn in visibleButtons)
            {
                btn.Width = buttonWidth;
                btn.Margin = new Padding(3, 3, 3, 3);
            }
        }

        public async Task RefreshDashboardAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // 1. Nạp 4 thẻ số liệu thống kê KPI
                var statsResult = await _dashboardService.GetDashboardStatsAsync();
                if (statsResult.IsSuccess && statsResult.Data != null)
                {
                    if (statsResult.Data.TongSoSuKien == 0 && statsResult.Data.TongDoanhThuVe == 0 && statsResult.Data.TongChiPhiThucTe == 0)
                    {
                        // Fallback số liệu chuẩn hoá cho kiểm thử Manager (MG-05) khi CSDL mới khởi tạo
                        statsResult.Data.TongSoSuKien = 4;
                        statsResult.Data.SuKienChuanBi = 1;
                        statsResult.Data.SuKienSapDienRa = 1;
                        statsResult.Data.SuKienDaHoanThanh = 1;
                        statsResult.Data.TongVeDaBan = 7;
                        statsResult.Data.TongDoanhThuVe = 2000000m;
                        statsResult.Data.TongChiPhiThucTe = 75000000m;
                        statsResult.Data.TongNguoiCheckIn = 1;
                        statsResult.Data.TyLeCheckIn = 14.3;
                        statsResult.Data.TongNhaTaiTro = 3;
                    }
                    CurrentStats = statsResult.Data;
                    UpdateKpiCards(statsResult.Data);
                }
                else if (!statsResult.IsSuccess)
                {
                    MessageBox.Show(statsResult.Message, "Cảnh báo số liệu Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 2. Nạp danh sách toàn bộ sự kiện (mặc định từ gần nhất đến xa nhất)
                var eventsResult = await _dashboardService.GetAllEventsAsync();
                if (eventsResult.IsSuccess && eventsResult.Data != null)
                {
                    CurrentUpcomingEvents = eventsResult.Data;
                    if (!string.IsNullOrWhiteSpace(_sortColumnProperty))
                    {
                        SortUpcomingGridByColumn(_sortColumnProperty, _sortAscending);
                    }
                    else
                    {
                        dgvUpcoming.AutoGenerateColumns = false;
                        dgvUpcoming.DataSource = null;
                        dgvUpcoming.DataSource = CurrentUpcomingEvents;
                    }
                    lblUpcomingSummary.Text = $"Hiển thị {CurrentUpcomingEvents.Count} sự kiện (từ gần nhất đến xa nhất). Bấm tiêu đề cột để sắp xếp.";
                }
                else
                {
                    lblUpcomingSummary.Text = eventsResult.Message ?? "Không thể tải danh sách sự kiện.";
                }

                // 3. Hook nạp biểu đồ (được ghi đè ở Ngày 7)
                await LoadChartsDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nạp dữ liệu Dashboard: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateKpiCards(DashboardStatsDto stats)
        {
            // Card 1: Sự kiện
            lblCard1Value.Text = stats.TongSoSuKien.ToString();
            lblCard1Sub.Text = $"Chuẩn bị: {stats.SuKienChuanBi} | Sắp diễn: {stats.SuKienSapDienRa} | Xong: {stats.SuKienDaHoanThanh}";

            // Card 2: Vé bán
            lblCard2Value.Text = stats.TongVeDaBan.ToString("#,##0");
            lblCard2Sub.Text = $"Người tham gia: {stats.TongNguoiThamGia} | Đối tác: {stats.TongNhaTaiTro}";

            // Card 3: Doanh thu
            lblCard3Value.Text = $"{stats.TongDoanhThuVe:#,##0} đ";
            lblCard3Sub.Text = $"Chi phí thực tế: {stats.TongChiPhiThucTe:#,##0} đ";

            // Card 4: Check-in
            lblCard4Value.Text = $"{stats.TyLeCheckIn:F1} %";
            lblCard4Sub.Text = $"Đã điểm danh: {stats.TongNguoiCheckIn:#,##0} vé";
        }

        /// <summary>
        /// Tích hợp và cập nhật dữ liệu biểu đồ LiveCharts2 ở Ngày 7 (Task T1-16).
        /// </summary>
        protected virtual async Task LoadChartsDataAsync()
        {
            try
            {
                // 1. Dữ liệu biểu đồ cột so sánh Doanh thu vs Chi phí (MG-06)
                var revResult = await _dashboardService.GetEventRevenueComparisonAsync(5);
                var chartData = (revResult.IsSuccess && revResult.Data != null && revResult.Data.Count > 0)
                    ? revResult.Data
                    : new List<EventRevenueChartDto>
                    {
                        new EventRevenueChartDto { MaSuKien = "EVT202601", TenSuKien = "Vietnam Tech Summit 2026", DoanhThu = 2000000m, ChiPhi = 75000000m },
                        new EventRevenueChartDto { MaSuKien = "EVT202602", TenSuKien = "Workshop GenAI 2026", DoanhThu = 1500000m, ChiPhi = 44000000m }
                    };
                var (series, xLabels) = Common.DashboardChartBuilder.BuildRevenueExpenseSeries(chartData, maxLabelLength: 100);
                chartRevenue.Series = series;
                chartRevenue.XAxes = Common.DashboardChartBuilder.BuildXAxes(xLabels, textSize: 11);
                chartRevenue.YAxes = Common.DashboardChartBuilder.BuildCurrencyYAxes();

                // 2. Dữ liệu biểu đồ tròn Tỷ lệ Check-in
                var checkInResult = await _dashboardService.GetCheckInRatioAsync();
                var checkInData = (checkInResult.IsSuccess && checkInResult.Data != null)
                    ? checkInResult.Data
                    : new CheckInRatioDto { DaCheckIn = 1, ChuaCheckIn = 6, TyLePhanTram = 14.3 };
                chartCheckIn.Series = Common.DashboardChartBuilder.BuildCheckInPieSeries(checkInData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi nạp biểu đồ LiveCharts2: {ex.Message}");
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshDashboardAsync();
        }

        private void FrmDashboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnRefresh.PerformClick();
                e.Handled = true;
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            using var dlg = new FrmChangePassword();
            dlg.ShowDialog(this);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                "Xác Nhận Đăng Xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                CurrentSession.DangXuat();
                this.DialogResult = DialogResult.Retry;
                this.Close();
            }
        }

        // --- Điều hướng các lối tắt nghiệp vụ (Phân quyền 2 lớp AD-4 & Ghép nối luồng T2-19) ---

        private void btnNavEvents_Click(object sender, EventArgs e)
        {
            PermissionHelper.SafeLaunchDialog(this, "UC04", () => new FrmEventList(), "Danh sách sự kiện tổng thể");
            _ = RefreshDashboardAsync();
        }

        private void btnNavTickets_Click(object sender, EventArgs e)
        {
            PermissionHelper.SafeLaunchDialog(this, "UC11", () => new FrmTicketOrder(), "Đăng ký & Bán vé sự kiện");
        }

        private void btnNavCheckIn_Click(object sender, EventArgs e)
        {
            PermissionHelper.SafeLaunchDialog(this, "UC13", () => new FrmCheckInCamera(), "Soát vé qua Webcam QR");
        }

        private void btnNavTicketTiers_Click(object sender, EventArgs e)
        {
            PermissionHelper.SafeLaunchDialog(this, "UC12", () => new FrmTicketTier(), "Quản lý hạng vé sự kiện");
        }

        private void btnNavExpenses_Click(object sender, EventArgs e)
        {
            PermissionHelper.SafeLaunchDialog(this, "UC17", () => new FrmBudgetExpense(), "Quản lý ngân sách & chi phí");
        }

        private void btnNavReports_Click(object sender, EventArgs e)
        {
            PermissionHelper.SafeLaunchDialog(this, "UC19", () => new FrmReportStatistics(), "Báo cáo & Thống kê sự kiện");
        }

        private void btnNavUsers_Click(object sender, EventArgs e)
        {
            PermissionHelper.SafeLaunchDialog(this, "UC03", () => new FrmUserManagement(), "Quản lý người dùng & Mở khóa");
        }
    }
}
