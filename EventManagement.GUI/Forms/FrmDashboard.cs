using System;
using System.Collections.Generic;
using System.Drawing;
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
            ApplyUserSessionAndPermissions();
            DistributeQuickButtons();
            this.Resize += (s, ev) => DistributeQuickButtons();
            DataSyncNotifier.GeneralDataChanged += OnDataSyncChanged;
            dgvUpcoming.CellDoubleClick += DgvUpcoming_CellDoubleClick;
            await RefreshDashboardAsync();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            DataSyncNotifier.GeneralDataChanged -= OnDataSyncChanged;
            dgvUpcoming.CellDoubleClick -= DgvUpcoming_CellDoubleClick;
            base.OnFormClosed(e);
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
            }
            else
            {
                lblUserGreeting.Text = "Xin chào: Quản trị viên";
                CanAccessAdminFeatures = true;

                // Khi chạy độc lập không đăng nhập: Hiển thị đầy đủ để demo
                foreach (Control c in flpQuickButtons.Controls)
                {
                    c.Visible = true;
                }
            }
        }

        public void DistributeQuickButtons()
        {
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
                    CurrentStats = statsResult.Data;
                    UpdateKpiCards(statsResult.Data);
                }

                // 2. Nạp danh sách sự kiện sắp diễn ra trong 7 ngày
                var upcomingResult = await _dashboardService.GetUpcomingEventsAsync(7);
                if (upcomingResult.IsSuccess && upcomingResult.Data != null)
                {
                    CurrentUpcomingEvents = upcomingResult.Data;
                    dgvUpcoming.AutoGenerateColumns = false;
                    dgvUpcoming.DataSource = upcomingResult.Data;
                    lblUpcomingSummary.Text = $"Tìm thấy {upcomingResult.Data.Count} sự kiện sẽ diễn ra trong vòng 7 ngày tới.";
                }
                else
                {
                    lblUpcomingSummary.Text = "Hiện không có sự kiện nào diễn ra trong 7 ngày tới.";
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
                // 1. Dữ liệu biểu đồ cột so sánh Doanh thu vs Chi phí
                var revResult = await _dashboardService.GetEventRevenueComparisonAsync(5);
                if (revResult.IsSuccess && revResult.Data != null)
                {
                    var (series, xLabels) = Common.DashboardChartBuilder.BuildRevenueExpenseSeries(revResult.Data);
                    chartRevenue.Series = series;
                    chartRevenue.XAxes = Common.DashboardChartBuilder.BuildXAxes(xLabels);
                    chartRevenue.YAxes = Common.DashboardChartBuilder.BuildCurrencyYAxes();
                }

                // 2. Dữ liệu biểu đồ tròn Tỷ lệ Check-in
                var checkInResult = await _dashboardService.GetCheckInRatioAsync();
                if (checkInResult.IsSuccess && checkInResult.Data != null)
                {
                    chartCheckIn.Series = Common.DashboardChartBuilder.BuildCheckInPieSeries(checkInResult.Data);
                }
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
