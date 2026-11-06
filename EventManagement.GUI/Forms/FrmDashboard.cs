using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
using EventManagement.Core.Session;

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
            ApplyUserSessionAndPermissions();
            await RefreshDashboardAsync();
        }

        public bool CanAccessAdminFeatures { get; private set; }

        public void ApplyUserSessionAndPermissions()
        {
            if (CurrentSession.IsLoggedIn)
            {
                var hoTen = CurrentSession.HoTen ?? "Người dùng";
                var role = CurrentSession.MaNhomQuyen ?? "Khách";
                lblUserGreeting.Text = $"Xin chào: {hoTen}\n({role})";

                // Phân quyền hiển thị các nút quản trị hệ thống
                bool isAdmin = string.Equals(role, "ADMIN", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(role, "NQ_ADMIN", StringComparison.OrdinalIgnoreCase)
                    || CurrentSession.CoQuyen("F03")
                    || CurrentSession.CoQuyen("ALL");

                CanAccessAdminFeatures = isAdmin;
                btnNavUsers.Visible = isAdmin;
                btnNavRoles.Visible = isAdmin;
            }
            else
            {
                lblUserGreeting.Text = "Xin chào: Quản trị viên (Khách)";
                CanAccessAdminFeatures = true;
                btnNavUsers.Visible = true;
                btnNavRoles.Visible = true;
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
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // --- Điều hướng các lối tắt nghiệp vụ ---

        private void btnNavEvents_Click(object sender, EventArgs e)
        {
            // Mở màn hình sự kiện / lịch trình
            try
            {
                using var frm = new FrmSchedule();
                frm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chức năng Sự kiện: {ex.Message}", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnNavTickets_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Màn hình Đăng ký & Bán vé (F11) đang được Thành viên 3 hoàn thiện.\nVui lòng mở chức năng Quản lý hạng vé (F12) để xem tồn kho.",
                "Điều Hướng Tác Nghiệp (F11)",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnNavCheckIn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Màn hình Soát vé Webcam QR (F13) đang được Thành viên 3 kết nối camera.\nHệ thống sẽ sẵn sàng trong Ngày 7.",
                "Điều Hướng Tác Nghiệp (F13)",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnNavTicketTiers_Click(object sender, EventArgs e)
        {
            try
            {
                using var frm = new FrmTicketTier();
                frm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở màn hình Hạng vé: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNavReports_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Màn hình Báo cáo & Thống kê RDLC/Excel (F19) được phân công cho Thành viên 3 tại Ngày 8.",
                "Điều Hướng Báo Cáo (F19)",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnNavUsers_Click(object sender, EventArgs e)
        {
            try
            {
                using var frm = new FrmUserManagement();
                frm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở màn hình Quản lý người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNavRoles_Click(object sender, EventArgs e)
        {
            try
            {
                using var frm = new FrmRolePermission();
                frm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở màn hình Phân quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
