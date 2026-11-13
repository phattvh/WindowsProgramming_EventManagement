using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using SkiaSharp;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình F19 - Báo cáo & Thống kê tổng hợp sự kiện
    /// Phụ trách: Thành viên 3 (Thuật toán lõi, Bán vé, Ngân sách & Báo cáo - Task T3-23)
    /// Tuân thủ: Mẫu bố cục 4 (Archetype 4), Kiến trúc báo cáo kép RDLC + Native GDI+ PrintDocument (AD-5).
    /// </summary>
    public partial class FrmReportStatistics : FrmBase
    {
        private readonly IReportService _reportService;
        private readonly PrintDocument _printDoc = new();

        private SettlementDto? _currentSettlement;
        private List<TicketReceiptDto> _currentTickets = new();
        private List<EventSummaryKpiDto> _currentKpis = new();

        private int _selectedReportMode = 1; // 1: Quyết toán, 2: Vé điện tử, 3: Thống kê KPI

        public FrmReportStatistics() : this(new ReportService())
        {
        }

        public FrmReportStatistics(IReportService reportService)
        {
            InitializeComponent();
            _reportService = reportService ?? new ReportService();

            InitCustomStylesAndBindings();
        }

        private void InitCustomStylesAndBindings()
        {
            // Tiêu đề form và kích thước toàn màn hình
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.WindowState = FormWindowState.Maximized;
            lblBannerTitle.Text = "BÁO CÁO THỐNG KÊ";
            lblBannerSubtitle.Visible = false;
            lblBannerSubtitle.Text = string.Empty;

            // Định kiểu các nút theo FrmBase chuẩn Design Rules
            StyleButton(btnXem, ButtonRole.Primary);
            StyleButton(btnIn, ButtonRole.Secondary);
            StyleButton(btnXuatExcel, ButtonRole.Success);
            StyleButton(btnLamMoi, ButtonRole.Secondary);
            StyleButton(btnPrintFromPreview, ButtonRole.Primary);

            foreach (var btn in new[] { btnXem, btnIn, btnXuatExcel, btnLamMoi, btnPrintFromPreview })
            {
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.UseCompatibleTextRendering = false;
            }

            radQuyetToan.UseMnemonic = false;
            radVeDienTu.UseMnemonic = false;
            radTongHop.UseMnemonic = false;

            StyleDataGridView(dgvReportData);
            dgvReportData.BorderStyle = BorderStyle.FixedSingle;
            dgvReportData.RowTemplate.Height = 30;
            dgvReportData.ColumnHeadersHeight = 36;
            dgvReportData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvReportData.EnableHeadersVisualStyles = false;
            dgvReportData.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgvReportData.ColumnHeadersDefaultCellStyle.SelectionForeColor = ColorText;
            dgvReportData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReportData.AutoGenerateColumns = false;

            pnlKpiBottom.Resize += (s, e) => LayoutKpiBottom();
            LayoutKpiBottom();

            // Thiết lập ngày mặc định (đầu tháng đến cuối tháng hiện tại)
            var now = DateTime.Today;
            dtpTuNgay.Value = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            dtpDenNgay.Value = new DateTime(now.Year, now.Month, 1).AddMonths(2).AddDays(-1);

            // Nạp danh sách sự kiện vào ComboBox
            LoadEventDropdown();

            // Zoom Preview ComboBox
            cboZoom.SelectedIndex = 2; // 100%

            // Setup PrintDocument
            _printDoc.DocumentName = "BaoCaoSuKien";
            _printDoc.PrintPage += PrintDoc_PrintPage;
            printPreviewControl.Document = _printDoc;

            // Đăng ký sự kiện
            btnXem.Click += (s, e) => ExecuteLoadReport();
            btnLamMoi.Click += (s, e) => ResetFilterAndReload();
            btnIn.Click += (s, e) => ExecutePrint();
            btnPrintFromPreview.Click += (s, e) => ExecutePrint();
            btnXuatExcel.Click += (s, e) => ExecuteExportCsv();

            radQuyetToan.CheckedChanged += (s, e) => { if (radQuyetToan.Checked) { _selectedReportMode = 1; ExecuteLoadReport(); } };
            radVeDienTu.CheckedChanged += (s, e) => { if (radVeDienTu.Checked) { _selectedReportMode = 2; ExecuteLoadReport(); } };
            radTongHop.CheckedChanged += (s, e) => { if (radTongHop.Checked) { _selectedReportMode = 3; ExecuteLoadReport(); } };

            cboZoom.SelectedIndexChanged += CboZoom_SelectedIndexChanged;
            tabReport.SelectedIndexChanged += TabReport_SelectedIndexChanged;

            // Phím tắt
            this.KeyDown += FrmReportStatistics_KeyDown;

            // Tải dữ liệu mặc định ban đầu
            ExecuteLoadReport();

            DataSyncNotifier.GeneralDataChanged += OnDataSyncChanged;
            this.FormClosed += (s, e) => DataSyncNotifier.GeneralDataChanged -= OnDataSyncChanged;
        }

        private void OnDataSyncChanged()
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            try
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(new Action(() => ExecuteLoadReport()));
                else
                    ExecuteLoadReport();
            }
            catch { }
        }

        private void LayoutKpiBottom()
        {
            int totalW = pnlKpiBottom.ClientSize.Width;
            if (totalW < 400) return;
            int colW = totalW / 4;
            lblTongThu.Location = new Point(15, 13);
            lblTongThu.Width = colW - 20;
            lblTongChi.Location = new Point(colW + 10, 13);
            lblTongChi.Width = colW - 20;
            lblChenhLech.Location = new Point(colW * 2 + 10, 13);
            lblChenhLech.Width = colW - 20;
            lblTrangThai.Location = new Point(colW * 3 + 10, 13);
            lblTrangThai.Width = colW - 25;
            lblTrangThai.TextAlign = ContentAlignment.MiddleRight;
        }

        private void LoadEventDropdown()
        {
            cboSuKien.Items.Clear();
            cboSuKien.Items.Add(new ComboBoxItem("ALL", "-- Tất cả sự kiện --"));

            try
            {
                var kpis = _reportService.GenerateAllEventSummaries();
                foreach (var kpi in kpis)
                {
                    cboSuKien.Items.Add(new ComboBoxItem(kpi.MaSuKien, $"{kpi.MaSuKien} - {kpi.TenSuKien}"));
                }
            }
            catch
            {
                // Fallback nếu chưa có kết nối DB
                cboSuKien.Items.Add(new ComboBoxItem("EVT001", "EVT001 - Hội Thảo Công Nghệ AI 2026"));
                cboSuKien.Items.Add(new ComboBoxItem("EVT002", "EVT002 - Lễ Hội Âm Nhạc Mùa Hè"));
            }

            cboSuKien.SelectedIndex = 0;
        }

        private void ExecuteLoadReport()
        {
            string? selectedEventId = null;
            if (cboSuKien.SelectedItem is ComboBoxItem item && item.Value != "ALL")
            {
                selectedEventId = item.Value;
            }

            var tuNgay = dtpTuNgay.Value.Date;
            var denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

            switch (_selectedReportMode)
            {
                case 1:
                    LoadSettlementReport(selectedEventId, tuNgay, denNgay);
                    break;
                case 2:
                    LoadTicketReport(selectedEventId);
                    break;
                case 3:
                    LoadSummaryKpiReport(selectedEventId);
                    break;
            }

            // Cập nhật lại khung PrintPreview nếu tab xem trước đang mở
            if (tabReport.SelectedTab == tabPreview)
            {
                printPreviewControl.InvalidatePreview();
            }
        }

        #region Báo Cáo 1: Quyết Toán Thu Chi (F17 / RDLC BaoCaoQuyetToan)
        private void LoadSettlementReport(string? maSuKien, DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                _currentSettlement = _reportService.GenerateFinancialSettlement(maSuKien, tuNgay, denNgay);
                SetupSettlementGridColumns();

                dgvReportData.Rows.Clear();
                int stt = 1;
                foreach (var chiPhi in _currentSettlement.DanhSachChiPhi)
                {
                    dgvReportData.Rows.Add(
                        stt++,
                        chiPhi.MaChiPhi,
                        chiPhi.KhoanMucChi,
                        chiPhi.DienGiai ?? "",
                        chiPhi.NgayChi.ToString("dd/MM/yyyy"),
                        (!string.IsNullOrWhiteSpace(chiPhi.TenNguoiLapPhieu) ? $"{chiPhi.NguoiLapPhieu} - {chiPhi.TenNguoiLapPhieu}" : chiPhi.NguoiLapPhieu),
                        chiPhi.SoTien.ToString("#,##0") + " VNĐ",
                        chiPhi.TrangThai ?? ""
                    );
                }

                // Cập nhật thanh KPI phía dưới
                lblTongThu.Text = $"Tổng nguồn thu: {_currentSettlement.TongThu:#,##0} VNĐ";
                lblTongChi.Text = $"Tổng thực chi: {_currentSettlement.TongThucChi:#,##0} VNĐ";
                lblChenhLech.Text = $"Chênh lệch: {_currentSettlement.ChenhLechThuChi:#,##0} VNĐ";

                if (_currentSettlement.ChenhLechThuChi >= 0)
                {
                    lblTrangThai.ForeColor = ColorSuccess;
                    lblTrangThai.Text = $"Kết luận: {_currentSettlement.DanhGiaQuyetToan} (+{_currentSettlement.ChenhLechThuChi:#,##0} VNĐ)";
                }
                else
                {
                    lblTrangThai.ForeColor = ColorDanger;
                    lblTrangThai.Text = $"Kết luận: {_currentSettlement.DanhGiaQuyetToan} ({_currentSettlement.ChenhLechThuChi:#,##0} VNĐ)";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể tải dữ liệu báo cáo quyết toán. Vui lòng thử lại sau.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupSettlementGridColumns()
        {
            dgvReportData.Columns.Clear();
            var colStt = new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 45, MinimumWidth = 40, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Resizable = DataGridViewTriState.False };
            var colMa = new DataGridViewTextBoxColumn { HeaderText = "Mã chi phí", Width = 105, MinimumWidth = 95, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colKhoanMuc = new DataGridViewTextBoxColumn { HeaderText = "Khoản mục chi", Width = 320, MinimumWidth = 240, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 125 };
            var colNoiDung = new DataGridViewTextBoxColumn { HeaderText = "Nội dung", Width = 260, MinimumWidth = 200, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 95 };
            var colNgay = new DataGridViewTextBoxColumn { HeaderText = "Ngày chi", Width = 95, MinimumWidth = 85, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colNguoi = new DataGridViewTextBoxColumn { HeaderText = "Người chi", Width = 200, MinimumWidth = 160, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colTien = new DataGridViewTextBoxColumn { HeaderText = "Số tiền", Width = 130, MinimumWidth = 110, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colTrangThai = new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", Width = 95, MinimumWidth = 85, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };

            dgvReportData.Columns.AddRange(colStt, colMa, colKhoanMuc, colNoiDung, colNgay, colNguoi, colTien, colTrangThai);

            foreach (DataGridViewColumn col in dgvReportData.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            colStt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colMa.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNguoi.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colNgay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTien.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNgay.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNguoi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        #endregion

        #region Báo Cáo 2: Vé Điện Tử & Doanh Thu (F11 / RDLC VeDienTu)
        private void LoadTicketReport(string? maSuKien)
        {
            try
            {
                _currentTickets = _reportService.GenerateTicketReceipts(maSuKien);
                SetupTicketGridColumns();

                dgvReportData.Rows.Clear();
                int stt = 1;
                decimal tongDoanhThu = 0;
                foreach (var ticket in _currentTickets)
                {
                    tongDoanhThu += ticket.DonGia;
                    dgvReportData.Rows.Add(
                        stt++,
                        ticket.MaVe,
                        ticket.TenSuKien,
                        ticket.TenHangVe,
                        ticket.DonGia.ToString("#,##0") + " VNĐ",
                        ticket.TenNguoiThamGia,
                        ticket.EmailNguoiThamGia ?? "",
                        ticket.MaQR,
                        ticket.NgayXuatVe.ToString("dd/MM/yyyy HH:mm")
                    );
                }

                lblTongThu.Text = $"Tổng doanh thu vé: {tongDoanhThu:#,##0} VNĐ";
                lblTongChi.Text = $"Tổng số vé phát hành: {_currentTickets.Count:#,##0} vé";
                lblChenhLech.Text = $"Giá vé trung bình: {(_currentTickets.Count > 0 ? (tongDoanhThu / _currentTickets.Count) : 0):#,##0} VNĐ";
                lblTrangThai.ForeColor = ColorPrimaryLight;
                lblTrangThai.Text = "Trạng thái: Vé điện tử hợp lệ tích hợp QR";
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể tải dữ liệu báo cáo vé điện tử. Vui lòng thử lại sau.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupTicketGridColumns()
        {
            dgvReportData.Columns.Clear();
            var colStt = new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 45, MinimumWidth = 40, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Resizable = DataGridViewTriState.False };
            var colMa = new DataGridViewTextBoxColumn { HeaderText = "Mã vé", Width = 80, MinimumWidth = 70, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colSuKien = new DataGridViewTextBoxColumn { HeaderText = "Tên sự kiện", Width = 320, MinimumWidth = 220, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 140 };
            var colHangVe = new DataGridViewTextBoxColumn { HeaderText = "Hạng vé", Width = 90, MinimumWidth = 80, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colDonGia = new DataGridViewTextBoxColumn { HeaderText = "Đơn giá", Width = 110, MinimumWidth = 100, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colNguoiThamGia = new DataGridViewTextBoxColumn { HeaderText = "Người tham gia", Width = 150, MinimumWidth = 130, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colEmail = new DataGridViewTextBoxColumn { HeaderText = "Email", Width = 200, MinimumWidth = 160, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 90 };
            var colMaQR = new DataGridViewTextBoxColumn { HeaderText = "Mã QR Check-in", Width = 120, MinimumWidth = 110, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colNgay = new DataGridViewTextBoxColumn { HeaderText = "Ngày xuất vé", Width = 120, MinimumWidth = 110, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };

            dgvReportData.Columns.AddRange(colStt, colMa, colSuKien, colHangVe, colDonGia, colNguoiThamGia, colEmail, colMaQR, colNgay);

            foreach (DataGridViewColumn col in dgvReportData.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            colStt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colMa.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNguoiThamGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            colHangVe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDonGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colDonGia.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colMaQR.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNgay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        #endregion

        #region Báo Cáo 3: Thống Kê Tổng Hợp KPI Sự Kiện
        private void LoadSummaryKpiReport(string? maSuKien)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(maSuKien))
                {
                    _currentKpis = new List<EventSummaryKpiDto> { _reportService.GenerateEventSummary(maSuKien) };
                }
                else
                {
                    _currentKpis = _reportService.GenerateAllEventSummaries();
                }

                SetupKpiGridColumns();

                dgvReportData.Rows.Clear();
                int stt = 1;
                decimal tongDoanhThu = 0;
                int tongVeBan = 0;
                int tongCheckIn = 0;

                foreach (var kpi in _currentKpis)
                {
                    tongDoanhThu += kpi.TongDoanhThu;
                    tongVeBan += kpi.TongVeDaBan;
                    tongCheckIn += kpi.TongLuotCheckIn;

                    dgvReportData.Rows.Add(
                        stt++,
                        kpi.MaSuKien,
                        kpi.TenSuKien,
                        kpi.DiaDiem,
                        kpi.ThoiGianBatDau.ToString("dd/MM/yyyy HH:mm"),
                        kpi.TongVePhatHanh,
                        kpi.TongVeDaBan,
                        kpi.TongLuotCheckIn,
                        $"{kpi.TyLeBanVe:0.0}%",
                        $"{kpi.TyLeThamDu:0.0}%",
                        kpi.TongDoanhThu.ToString("#,##0") + " VNĐ"
                    );
                }

                lblTongThu.Text = $"Tổng doanh thu toàn bộ: {tongDoanhThu:#,##0} VNĐ";
                lblTongChi.Text = $"Tổng vé bán ra: {tongVeBan:#,##0} vé";
                lblChenhLech.Text = $"Tổng lượt check-in: {tongCheckIn:#,##0} lượt";
                lblTrangThai.ForeColor = ColorSuccess;
                lblTrangThai.Text = "Chỉ số KPI đạt tiến độ chuẩn";
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể tải dữ liệu thống kê KPI. Vui lòng thử lại sau.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupKpiGridColumns()
        {
            dgvReportData.Columns.Clear();
            var colStt = new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 45, MinimumWidth = 40, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Resizable = DataGridViewTriState.False };
            var colMa = new DataGridViewTextBoxColumn { HeaderText = "Mã SK", Width = 80 };
            var colSuKien = new DataGridViewTextBoxColumn { HeaderText = "Tên sự kiện", Width = 320, MinimumWidth = 220, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 140 };
            var colDiaDiem = new DataGridViewTextBoxColumn { HeaderText = "Địa điểm", Width = 180, MinimumWidth = 150, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 85 };
            var colBatDau = new DataGridViewTextBoxColumn { HeaderText = "Thời gian bắt đầu", Width = 135 };
            var colPhatHanh = new DataGridViewTextBoxColumn { HeaderText = "Phát hành", Width = 85 };
            var colDaBan = new DataGridViewTextBoxColumn { HeaderText = "Đã bán", Width = 80 };
            var colCheckIn = new DataGridViewTextBoxColumn { HeaderText = "Check-in", Width = 80 };
            var colTyLeBan = new DataGridViewTextBoxColumn { HeaderText = "% Bán vé", Width = 85 };
            var colTyLeThamDu = new DataGridViewTextBoxColumn { HeaderText = "% Tham dự", Width = 90 };
            var colDoanhThu = new DataGridViewTextBoxColumn { HeaderText = "Doanh thu", Width = 140, MinimumWidth = 125 };

            dgvReportData.Columns.AddRange(colStt, colMa, colSuKien, colDiaDiem, colBatDau, colPhatHanh, colDaBan, colCheckIn, colTyLeBan, colTyLeThamDu, colDoanhThu);

            foreach (DataGridViewColumn col in dgvReportData.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            colStt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colMa.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colBatDau.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colPhatHanh.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colDaBan.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCheckIn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTyLeBan.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTyLeThamDu.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colDoanhThu.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colDoanhThu.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        #endregion

        #region In Ấn (Native PrintDocument & Preview Engine - Kiến Trúc AD-5)
        private void TabReport_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabReport.SelectedTab == tabPreview)
            {
                printPreviewControl.InvalidatePreview();
            }
        }

        private void CboZoom_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (cboZoom.SelectedIndex)
            {
                case 0: printPreviewControl.Zoom = 0.5; break;
                case 1: printPreviewControl.Zoom = 0.75; break;
                case 2: printPreviewControl.Zoom = 1.0; break;
                case 3: printPreviewControl.Zoom = 1.25; break;
                case 4: printPreviewControl.Zoom = 1.5; break;
            }
        }

        private void ExecutePrint()
        {
            using var sfd = new SaveFileDialog
            {
                Title = "Xuất Báo Cáo Ra Định Dạng PDF / In Báo Cáo",
                Filter = "Tệp PDF (*.pdf)|*.pdf|In qua máy in vật lý (*.*)|*.*",
                FileName = _selectedReportMode switch
                {
                    1 => $"BaoCaoQuyetToan_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    2 => $"BaoCaoVeDienTu_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    _ => $"ThongKeKpi_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                }
            };

            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            string targetFile = sfd.FileName;
            if (Path.GetExtension(targetFile).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    GenerateRealTextPdfReport(targetFile);

                    MessageBox.Show(
                        $"Xuất báo cáo PDF thành công!\n\nĐường dẫn: {targetFile}\nĐịnh dạng: Chuẩn trang A4, văn bản Vector (True Text) sắc nét, có thể chọn/sao chép/chỉnh sửa nội dung, bảng biểu cân đối không bị chồng chéo.",
                        "Xuất PDF Thành Công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tạo tệp PDF: {ex.Message}", "Lỗi Xuất Báo Cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                using var printDialog = new PrintDialog();
                printDialog.Document = _printDoc;
                printDialog.UseEXDialog = true;
                if (printDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        _printDoc.Print();
                    }
                    catch
                    {
                        MessageBox.Show("Không thể thực hiện in ấn. Vui lòng kiểm tra lại thiết bị máy in.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        public void GenerateRealTextPdfReport(string filePath)
        {
            using var stream = File.Create(filePath);
            using var doc = SKDocument.CreatePdf(stream);
            using var canvas = doc.BeginPage(595, 842); // Khổ A4 tiêu chuẩn quốc tế: 595 x 842 pt

            using var typefaceRegular = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Normal)
                                        ?? SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal)
                                        ?? SKTypeface.Default;
            using var typefaceBold = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Bold)
                                     ?? SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
                                     ?? SKTypeface.Default;
            using var typefaceItalic = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Italic)
                                       ?? SKTypeface.FromFamilyName("Arial", SKFontStyle.Italic)
                                       ?? SKTypeface.Default;

            using var paintTitle = new SKPaint { Typeface = typefaceBold, TextSize = 15f, Color = new SKColor(30, 58, 138), IsAntialias = true };
            using var paintSubTitle = new SKPaint { Typeface = typefaceBold, TextSize = 10f, Color = new SKColor(30, 41, 59), IsAntialias = true };
            using var paintItalic = new SKPaint { Typeface = typefaceItalic, TextSize = 8.5f, Color = new SKColor(100, 116, 139), IsAntialias = true };
            using var paintSectionHeader = new SKPaint { Typeface = typefaceBold, TextSize = 10f, Color = new SKColor(30, 58, 138), IsAntialias = true };
            using var paintTh = new SKPaint { Typeface = typefaceBold, TextSize = 8.5f, Color = new SKColor(30, 41, 59), IsAntialias = true };
            using var paintTd = new SKPaint { Typeface = typefaceRegular, TextSize = 8.5f, Color = new SKColor(30, 41, 59), IsAntialias = true };
            using var paintSuccess = new SKPaint { Typeface = typefaceBold, TextSize = 9f, Color = new SKColor(22, 163, 74), IsAntialias = true };
            using var paintDanger = new SKPaint { Typeface = typefaceBold, TextSize = 9f, Color = new SKColor(220, 38, 38), IsAntialias = true };

            using var paintBorder = new SKPaint { Color = new SKColor(226, 232, 240), Style = SKPaintStyle.Stroke, StrokeWidth = 1f, IsAntialias = true };
            using var paintKpiBorder = new SKPaint { Color = new SKColor(203, 213, 225), Style = SKPaintStyle.Stroke, StrokeWidth = 1f, IsAntialias = true };
            using var paintBoxBg = new SKPaint { Color = new SKColor(248, 250, 252), Style = SKPaintStyle.Fill, IsAntialias = true };
            using var paintHeaderBg = new SKPaint { Color = new SKColor(241, 245, 249), Style = SKPaintStyle.Fill, IsAntialias = true };
            using var paintZebraBg = new SKPaint { Color = new SKColor(248, 250, 252), Style = SKPaintStyle.Fill, IsAntialias = true };
            using var paintWhiteBg = new SKPaint { Color = SKColors.White, Style = SKPaintStyle.Fill, IsAntialias = true };

            float left = 36f;
            float top = 36f;
            float width = 523f;
            float y = top;

            canvas.DrawRect(0, 0, 595, 842, paintWhiteBg);

            if (_selectedReportMode == 1 && _currentSettlement != null)
            {
                // Báo Cáo Quyết Toán Tài Chính Sự Kiện
                string title = "BÁO CÁO QUYẾT TOÁN TÀI CHÍNH SỰ KIỆN";
                float titleW = paintTitle.MeasureText(title);
                canvas.DrawText(title, left + (width - titleW) / 2f, y + 16, paintTitle);
                y += 28;

                string subTitle = $"Sự kiện: {_currentSettlement.TenSuKien} (Mã SK: {_currentSettlement.MaSuKien})";
                float subW = paintSubTitle.MeasureText(subTitle);
                canvas.DrawText(subTitle, left + (width - subW) / 2f, y + 12, paintSubTitle);
                y += 20;

                string period = $"Kỳ quyết toán: Từ {_currentSettlement.TuNgay:dd/MM/yyyy} đến {_currentSettlement.DenNgay:dd/MM/yyyy}";
                float perW = paintItalic.MeasureText(period);
                canvas.DrawText(period, left + (width - perW) / 2f, y + 10, paintItalic);
                y += 22;

                // Khung tổng hợp KPI
                float kpiH = 70f;
                canvas.DrawRoundRect(left, y, width, kpiH, 4, 4, paintBoxBg);
                canvas.DrawRoundRect(left, y, width, kpiH, 4, 4, paintKpiBorder);

                float halfW = width / 2f;
                DrawSkiaCellText(canvas, $"Ngân sách dự kiến: {_currentSettlement.NganSachDuKien:#,##0} VNĐ", left + 12, halfW - 20, y + 8, 16, paintTd, StringAlignment.Near, 0);
                DrawSkiaCellText(canvas, $"Doanh thu bán vé: {_currentSettlement.TongDoanhThuBanVe:#,##0} VNĐ", left + 12, halfW - 20, y + 26, 16, paintTd, StringAlignment.Near, 0);

                DrawSkiaCellText(canvas, $"Tiền tài trợ tiếp nhận: {_currentSettlement.TongTienTaiTro:#,##0} VNĐ", left + halfW + 12, halfW - 20, y + 8, 16, paintTd, StringAlignment.Near, 0);
                DrawSkiaCellText(canvas, $"TỔNG NGUỒN THU: {_currentSettlement.TongThu:#,##0} VNĐ", left + halfW + 12, halfW - 20, y + 26, 16, paintSuccess, StringAlignment.Near, 0);

                canvas.DrawLine(left + 12, y + 46, left + width - 12, y + 46, paintBorder);

                var paintDiff = _currentSettlement.ChenhLechThuChi >= 0 ? paintSuccess : paintDanger;
                string bottomSummary = $"TỔNG THỰC CHI: {_currentSettlement.TongThucChi:#,##0} VNĐ   |   CHÊNH LỆCH: {_currentSettlement.ChenhLechThuChi:#,##0} VNĐ";
                DrawSkiaCellText(canvas, bottomSummary, left + 12, width - 24, y + 48, 18, paintDiff, StringAlignment.Near, 0);

                y += kpiH + 18;

                // Tiêu đề bảng
                canvas.DrawText("DANH SÁCH CÁC KHOẢN CHI THỰC TẾ", left, y + 11, paintSectionHeader);
                y += 18;

                // Cấu trúc cột bảng (tổng chiều rộng 523pt hoàn toàn vừa vặn khổ A4)
                float[] colW = { 35f, 155f, 183f, 65f, 85f };
                string[] headers = { "STT", "Khoản mục chi", "Diễn giải / Ghi chú", "Ngày chi", "Số tiền (VNĐ)" };
                StringAlignment[] aligns = { StringAlignment.Center, StringAlignment.Near, StringAlignment.Near, StringAlignment.Center, StringAlignment.Far };

                // Header bảng
                float headerH = 24f;
                canvas.DrawRect(left, y, width, headerH, paintHeaderBg);
                canvas.DrawRect(left, y, width, headerH, paintBorder);

                float curX = left;
                for (int i = 0; i < colW.Length; i++)
                {
                    if (i > 0) canvas.DrawLine(curX, y, curX, y + headerH, paintBorder);
                    DrawSkiaCellText(canvas, headers[i], curX, colW[i], y, headerH, paintTh, aligns[i]);
                    curX += colW[i];
                }
                y += headerH;

                // Dữ liệu bảng chi phí
                float rowH = 22f;
                int index = 1;
                var listChi = _currentSettlement.DanhSachChiPhi.Take(15).ToList();
                for (int r = 0; r < listChi.Count; r++)
                {
                    var chi = listChi[r];
                    var rowBg = (r % 2 == 1) ? paintZebraBg : paintWhiteBg;
                    canvas.DrawRect(left, y, width, rowH, rowBg);
                    canvas.DrawRect(left, y, width, rowH, paintBorder);

                    string[] cells = {
                        index++.ToString(),
                        chi.KhoanMucChi,
                        chi.DienGiai ?? "",
                        chi.NgayChi.ToString("dd/MM/yyyy"),
                        chi.SoTien.ToString("#,##0")
                    };

                    curX = left;
                    for (int i = 0; i < colW.Length; i++)
                    {
                        if (i > 0) canvas.DrawLine(curX, y, curX, y + rowH, paintBorder);
                        DrawSkiaCellText(canvas, cells[i], curX, colW[i], y, rowH, paintTd, aligns[i]);
                        curX += colW[i];
                    }
                    y += rowH;
                }

                // Dòng tổng cộng thực chi
                float sumH = 24f;
                canvas.DrawRect(left, y, width, sumH, paintHeaderBg);
                canvas.DrawRect(left, y, width, sumH, paintBorder);

                float sumLabelW = colW[0] + colW[1] + colW[2] + colW[3];
                canvas.DrawLine(left + sumLabelW, y, left + sumLabelW, y + sumH, paintBorder);
                DrawSkiaCellText(canvas, "TỔNG CỘNG THỰC CHI:", left, sumLabelW, y, sumH, paintTh, StringAlignment.Far, 8);
                DrawSkiaCellText(canvas, $"{_currentSettlement.TongThucChi:#,##0} VNĐ", left + sumLabelW, colW[4], y, sumH, paintDanger, StringAlignment.Far, 5);
                y += sumH + 35;

                // Khối chữ ký 3 bên
                float sigColW = width / 3f;
                string[] sigTitles = { "NGƯỜI LẬP BIỂU", "KẾ TOÁN TRƯỞNG", "TRƯỞNG BAN TỔ CHỨC" };
                string[] sigNotes = { "(Ký, ghi rõ họ tên)", "(Ký, ghi rõ họ tên)", "(Ký, đóng dấu)" };

                for (int i = 0; i < 3; i++)
                {
                    float sx = left + i * sigColW;
                    DrawSkiaCellText(canvas, sigTitles[i], sx, sigColW, y, 16, paintTh, StringAlignment.Center, 0);
                    DrawSkiaCellText(canvas, sigNotes[i], sx, sigColW, y + 14, 16, paintItalic, StringAlignment.Center, 0);
                }
            }
            else if (_selectedReportMode == 2)
            {
                // Báo Cáo Vé Điện Tử & Doanh Thu
                string title = "BÁO CÁO TỔNG HỢP VÉ ĐIỆN TỬ & DOANH THU";
                float titleW = paintTitle.MeasureText(title);
                canvas.DrawText(title, left + (width - titleW) / 2f, y + 16, paintTitle);
                y += 28;

                string summary = $"Tổng số vé: {_currentTickets.Count} vé   |   Tổng doanh thu: {_currentTickets.Sum(t => t.DonGia):#,##0} VNĐ";
                float sumW = paintSubTitle.MeasureText(summary);
                canvas.DrawText(summary, left + (width - sumW) / 2f, y + 12, paintSubTitle);
                y += 26;

                float[] colW = { 65f, 100f, 125f, 145f, 88f };
                string[] headers = { "Mã vé", "Hạng vé", "Người tham gia", "Mã QR", "Đơn giá (VNĐ)" };
                StringAlignment[] aligns = { StringAlignment.Center, StringAlignment.Near, StringAlignment.Near, StringAlignment.Center, StringAlignment.Far };

                float headerH = 24f;
                canvas.DrawRect(left, y, width, headerH, paintHeaderBg);
                canvas.DrawRect(left, y, width, headerH, paintBorder);

                float curX = left;
                for (int i = 0; i < colW.Length; i++)
                {
                    if (i > 0) canvas.DrawLine(curX, y, curX, y + headerH, paintBorder);
                    DrawSkiaCellText(canvas, headers[i], curX, colW[i], y, headerH, paintTh, aligns[i]);
                    curX += colW[i];
                }
                y += headerH;

                float rowH = 22f;
                var listTickets = _currentTickets.Take(25).ToList();
                for (int r = 0; r < listTickets.Count; r++)
                {
                    var ticket = listTickets[r];
                    var rowBg = (r % 2 == 1) ? paintZebraBg : paintWhiteBg;
                    canvas.DrawRect(left, y, width, rowH, rowBg);
                    canvas.DrawRect(left, y, width, rowH, paintBorder);

                    string[] cells = {
                        ticket.MaVe,
                        ticket.TenHangVe,
                        ticket.TenNguoiThamGia,
                        ticket.MaQR,
                        ticket.DonGia.ToString("#,##0")
                    };

                    curX = left;
                    for (int i = 0; i < colW.Length; i++)
                    {
                        if (i > 0) canvas.DrawLine(curX, y, curX, y + rowH, paintBorder);
                        DrawSkiaCellText(canvas, cells[i], curX, colW[i], y, rowH, paintTd, aligns[i]);
                        curX += colW[i];
                    }
                    y += rowH;
                }
            }
            else
            {
                // Báo Cáo Thống Kê Chỉ Số Hoạt Động (KPI) Sự Kiện
                string title = "BÁO CÁO THỐNG KÊ CHỈ SỐ HOẠT ĐỘNG (KPI) SỰ KIỆN";
                float titleW = paintTitle.MeasureText(title);
                canvas.DrawText(title, left + (width - titleW) / 2f, y + 16, paintTitle);
                y += 28;

                float[] colW = { 65f, 195f, 55f, 55f, 58f, 95f };
                string[] headers = { "Mã SK", "Tên sự kiện", "Vé bán", "Check-in", "% Bán vé", "Doanh thu (VNĐ)" };
                StringAlignment[] aligns = { StringAlignment.Center, StringAlignment.Near, StringAlignment.Center, StringAlignment.Center, StringAlignment.Center, StringAlignment.Far };

                float headerH = 24f;
                canvas.DrawRect(left, y, width, headerH, paintHeaderBg);
                canvas.DrawRect(left, y, width, headerH, paintBorder);

                float curX = left;
                for (int i = 0; i < colW.Length; i++)
                {
                    if (i > 0) canvas.DrawLine(curX, y, curX, y + headerH, paintBorder);
                    DrawSkiaCellText(canvas, headers[i], curX, colW[i], y, headerH, paintTh, aligns[i]);
                    curX += colW[i];
                }
                y += headerH;

                float rowH = 22f;
                var listKpis = _currentKpis.Take(25).ToList();
                for (int r = 0; r < listKpis.Count; r++)
                {
                    var kpi = listKpis[r];
                    var rowBg = (r % 2 == 1) ? paintZebraBg : paintWhiteBg;
                    canvas.DrawRect(left, y, width, rowH, rowBg);
                    canvas.DrawRect(left, y, width, rowH, paintBorder);

                    string[] cells = {
                        kpi.MaSuKien,
                        kpi.TenSuKien,
                        kpi.TongVeDaBan.ToString(),
                        kpi.TongLuotCheckIn.ToString(),
                        $"{kpi.TyLeBanVe:0.0}%",
                        kpi.TongDoanhThu.ToString("#,##0")
                    };

                    curX = left;
                    for (int i = 0; i < colW.Length; i++)
                    {
                        if (i > 0) canvas.DrawLine(curX, y, curX, y + rowH, paintBorder);
                        DrawSkiaCellText(canvas, cells[i], curX, colW[i], y, rowH, paintTd, aligns[i]);
                        curX += colW[i];
                    }
                    y += rowH;
                }
            }

            doc.EndPage();
            doc.Close();
        }

        private static string TruncateSkiaText(string text, SKPaint paint, float maxWidth)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (paint.MeasureText(text) <= maxWidth) return text;

            string ellipsis = "...";
            float ellipsisWidth = paint.MeasureText(ellipsis);
            if (ellipsisWidth >= maxWidth) return string.Empty;

            int len = text.Length;
            while (len > 0 && paint.MeasureText(text.Substring(0, len)) + ellipsisWidth > maxWidth)
            {
                len--;
            }
            return text.Substring(0, len) + ellipsis;
        }

        private static void DrawSkiaCellText(SKCanvas canvas, string text, float cellX, float cellWidth, float cellY, float cellHeight, SKPaint paint, StringAlignment alignment = StringAlignment.Near, float padding = 5f)
        {
            float maxTextWidth = Math.Max(0, cellWidth - padding * 2);
            string truncated = TruncateSkiaText(text, paint, maxTextWidth);
            float textWidth = paint.MeasureText(truncated);

            float textX = alignment switch
            {
                StringAlignment.Center => cellX + (cellWidth - textWidth) / 2f,
                StringAlignment.Far => cellX + cellWidth - padding - textWidth,
                _ => cellX + padding
            };

            float textY = cellY + (cellHeight / 2f) - ((paint.FontMetrics.Ascent + paint.FontMetrics.Descent) / 2f);
            canvas.DrawText(truncated, textX, textY, paint);
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            if (g == null) return;

            using var fontTitle = new Font("Segoe UI", 16F, FontStyle.Bold);
            using var fontHeader = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var fontTh = new Font("Segoe UI", 9F, FontStyle.Bold);
            using var fontBody = new Font("Segoe UI", 9F, FontStyle.Regular);
            using var fontItalic = new Font("Segoe UI", 8.5F, FontStyle.Italic);

            using var brushTitle = new SolidBrush(ColorPrimary);
            using var brushText = new SolidBrush(ColorText);
            using var brushSuccess = new SolidBrush(ColorSuccess);
            using var brushDanger = new SolidBrush(ColorDanger);
            using var penBorder = new Pen(ColorBorder);

            using var sfLeft = new StringFormat(StringFormatFlags.NoWrap) { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
            using var sfCenter = new StringFormat(StringFormatFlags.NoWrap) { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
            using var sfRight = new StringFormat(StringFormatFlags.NoWrap) { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };

            int left = 40;
            int y = 40;
            int width = e.PageBounds.Width - 80;

            if (_selectedReportMode == 1 && _currentSettlement != null)
            {
                // In Báo Cáo Quyết Toán Tài Chính
                string title = "BÁO CÁO QUYẾT TOÁN TÀI CHÍNH SỰ KIỆN";
                var titleSize = g.MeasureString(title, fontTitle);
                g.DrawString(title, fontTitle, brushTitle, left + (width - titleSize.Width) / 2, y);
                y += 35;

                string subTitle = $"Sự kiện: {_currentSettlement.TenSuKien} (Mã SK: {_currentSettlement.MaSuKien})";
                var subSize = g.MeasureString(subTitle, fontHeader);
                g.DrawString(subTitle, fontHeader, brushText, left + (width - subSize.Width) / 2, y);
                y += 22;

                string period = $"Kỳ quyết toán: Từ {_currentSettlement.TuNgay:dd/MM/yyyy} đến {_currentSettlement.DenNgay:dd/MM/yyyy}";
                var perSize = g.MeasureString(period, fontItalic);
                g.DrawString(period, fontItalic, brushText, left + (width - perSize.Width) / 2, y);
                y += 30;

                // Khung tổng hợp nguồn thu & dự toán
                g.FillRectangle(new SolidBrush(Color.FromArgb(248, 250, 252)), left, y, width, 85);
                g.DrawRectangle(penBorder, left, y, width, 85);

                g.DrawString($"Ngân sách dự kiến: {_currentSettlement.NganSachDuKien:#,##0} VNĐ", fontBody, brushText, left + 15, y + 10);
                g.DrawString($"Doanh thu bán vé: {_currentSettlement.TongDoanhThuBanVe:#,##0} VNĐ", fontBody, brushText, left + 15, y + 32);

                g.DrawString($"Tiền tài trợ tiếp nhận: {_currentSettlement.TongTienTaiTro:#,##0} VNĐ", fontBody, brushText, left + width / 2, y + 10);
                g.DrawString($"TỔNG NGUỒN THU: {_currentSettlement.TongThu:#,##0} VNĐ", fontHeader, brushSuccess, left + width / 2, y + 32);

                g.DrawString($"TỔNG THỰC CHI: {_currentSettlement.TongThucChi:#,##0} VNĐ  |  CHÊNH LỆCH: {_currentSettlement.ChenhLechThuChi:#,##0} VNĐ", fontHeader,
                    _currentSettlement.ChenhLechThuChi >= 0 ? brushSuccess : brushDanger, left + 15, y + 58);
                y += 105;

                // Tiêu đề bảng chi phí
                g.DrawString("DANH SÁCH CÁC KHOẢN CHI THỰC TẾ", fontHeader, brushTitle, left, y);
                y += 22;

                int[] colW = { 45, 210, 250, 90, width - 45 - 210 - 250 - 90 };
                string[] headers = { "STT", "Khoản mục chi", "Diễn giải / Ghi chú", "Ngày chi", "Số tiền (VNĐ)" };
                StringFormat[] aligns = { sfCenter, sfLeft, sfLeft, sfCenter, sfRight };

                int headerH = 26;
                g.FillRectangle(new SolidBrush(Color.FromArgb(241, 245, 249)), left, y, width, headerH);
                g.DrawRectangle(penBorder, left, y, width, headerH);

                int curX = left;
                for (int i = 0; i < colW.Length; i++)
                {
                    if (i > 0) g.DrawLine(penBorder, curX, y, curX, y + headerH);
                    g.DrawString(headers[i], fontTh, brushText, new RectangleF(curX + 4, y, colW[i] - 8, headerH), aligns[i]);
                    curX += colW[i];
                }
                y += headerH;

                int rowH = 24;
                int index = 1;
                foreach (var chi in _currentSettlement.DanhSachChiPhi.Take(15))
                {
                    g.DrawRectangle(penBorder, left, y, width, rowH);
                    string[] cells = {
                        index++.ToString(),
                        chi.KhoanMucChi,
                        chi.DienGiai ?? "",
                        chi.NgayChi.ToString("dd/MM/yyyy"),
                        chi.SoTien.ToString("#,##0")
                    };

                    curX = left;
                    for (int i = 0; i < colW.Length; i++)
                    {
                        if (i > 0) g.DrawLine(penBorder, curX, y, curX, y + rowH);
                        g.DrawString(cells[i], fontBody, brushText, new RectangleF(curX + 4, y, colW[i] - 8, rowH), aligns[i]);
                        curX += colW[i];
                    }
                    y += rowH;
                }

                // Dòng tổng cộng
                int sumH = 26;
                g.FillRectangle(new SolidBrush(Color.FromArgb(248, 250, 252)), left, y, width, sumH);
                g.DrawRectangle(penBorder, left, y, width, sumH);
                int sumLabelW = colW[0] + colW[1] + colW[2] + colW[3];
                g.DrawLine(penBorder, left + sumLabelW, y, left + sumLabelW, y + sumH);
                g.DrawString("TỔNG CỘNG THỰC CHI:", fontHeader, brushText, new RectangleF(left, y, sumLabelW - 8, sumH), sfRight);
                g.DrawString($"{_currentSettlement.TongThucChi:#,##0} VNĐ", fontHeader, brushDanger, new RectangleF(left + sumLabelW, y, colW[4] - 8, sumH), sfRight);
                y += sumH + 40;

                // Chữ ký 3 bên
                int sigColWidth = width / 3;
                g.DrawString("NGƯỜI LẬP BIỂU", fontHeader, brushText, new RectangleF(left, y, sigColWidth, 20), sfCenter);
                g.DrawString("KẾ TOÁN TRƯỞNG", fontHeader, brushText, new RectangleF(left + sigColWidth, y, sigColWidth, 20), sfCenter);
                g.DrawString("TRƯỞNG BAN TỔ CHỨC", fontHeader, brushText, new RectangleF(left + sigColWidth * 2, y, sigColWidth, 20), sfCenter);

                y += 20;
                g.DrawString("(Ký, ghi rõ họ tên)", fontItalic, brushText, new RectangleF(left, y, sigColWidth, 18), sfCenter);
                g.DrawString("(Ký, ghi rõ họ tên)", fontItalic, brushText, new RectangleF(left + sigColWidth, y, sigColWidth, 18), sfCenter);
                g.DrawString("(Ký, đóng dấu)", fontItalic, brushText, new RectangleF(left + sigColWidth * 2, y, sigColWidth, 18), sfCenter);
            }
            else if (_selectedReportMode == 2)
            {
                // In Báo Cáo Vé Điện Tử
                string title = "BÁO CÁO TỔNG HỢP VÉ ĐIỆN TỬ & DOANH THU";
                var titleSize = g.MeasureString(title, fontTitle);
                g.DrawString(title, fontTitle, brushTitle, left + (width - titleSize.Width) / 2, y);
                y += 35;

                g.DrawString($"Tổng số vé: {_currentTickets.Count} vé | Doanh thu: {_currentTickets.Sum(t => t.DonGia):#,##0} VNĐ", fontHeader, brushText, left, y);
                y += 30;

                int[] colW = { 80, 130, 160, 200, width - 80 - 130 - 160 - 200 };
                string[] headers = { "Mã vé", "Hạng vé", "Người tham gia", "Mã QR", "Đơn giá (VNĐ)" };
                StringFormat[] aligns = { sfCenter, sfLeft, sfLeft, sfCenter, sfRight };

                int headerH = 26;
                g.FillRectangle(new SolidBrush(Color.FromArgb(241, 245, 249)), left, y, width, headerH);
                g.DrawRectangle(penBorder, left, y, width, headerH);

                int curX = left;
                for (int i = 0; i < colW.Length; i++)
                {
                    if (i > 0) g.DrawLine(penBorder, curX, y, curX, y + headerH);
                    g.DrawString(headers[i], fontTh, brushText, new RectangleF(curX + 4, y, colW[i] - 8, headerH), aligns[i]);
                    curX += colW[i];
                }
                y += headerH;

                int rowH = 24;
                foreach (var ticket in _currentTickets.Take(25))
                {
                    g.DrawRectangle(penBorder, left, y, width, rowH);
                    string[] cells = {
                        ticket.MaVe,
                        ticket.TenHangVe,
                        ticket.TenNguoiThamGia,
                        ticket.MaQR,
                        ticket.DonGia.ToString("#,##0")
                    };

                    curX = left;
                    for (int i = 0; i < colW.Length; i++)
                    {
                        if (i > 0) g.DrawLine(penBorder, curX, y, curX, y + rowH);
                        g.DrawString(cells[i], fontBody, brushText, new RectangleF(curX + 4, y, colW[i] - 8, rowH), aligns[i]);
                        curX += colW[i];
                    }
                    y += rowH;
                }
            }
            else
            {
                // In Báo Cáo Thống Kê Tổng Hợp KPI
                string title = "BÁO CÁO THỐNG KÊ CHỈ SỐ HOẠT ĐỘNG (KPI) SỰ KIỆN";
                var titleSize = g.MeasureString(title, fontTitle);
                g.DrawString(title, fontTitle, brushTitle, left + (width - titleSize.Width) / 2, y);
                y += 35;

                int[] colW = { 80, 250, 75, 75, 80, width - 80 - 250 - 75 - 75 - 80 };
                string[] headers = { "Mã SK", "Tên sự kiện", "Vé bán", "Check-in", "% Bán vé", "Doanh thu (VNĐ)" };
                StringFormat[] aligns = { sfCenter, sfLeft, sfCenter, sfCenter, sfCenter, sfRight };

                int headerH = 26;
                g.FillRectangle(new SolidBrush(Color.FromArgb(241, 245, 249)), left, y, width, headerH);
                g.DrawRectangle(penBorder, left, y, width, headerH);

                int curX = left;
                for (int i = 0; i < colW.Length; i++)
                {
                    if (i > 0) g.DrawLine(penBorder, curX, y, curX, y + headerH);
                    g.DrawString(headers[i], fontTh, brushText, new RectangleF(curX + 4, y, colW[i] - 8, headerH), aligns[i]);
                    curX += colW[i];
                }
                y += headerH;

                int rowH = 24;
                foreach (var kpi in _currentKpis.Take(25))
                {
                    g.DrawRectangle(penBorder, left, y, width, rowH);
                    string[] cells = {
                        kpi.MaSuKien,
                        kpi.TenSuKien,
                        kpi.TongVeDaBan.ToString(),
                        kpi.TongLuotCheckIn.ToString(),
                        $"{kpi.TyLeBanVe:0.0}%",
                        kpi.TongDoanhThu.ToString("#,##0")
                    };

                    curX = left;
                    for (int i = 0; i < colW.Length; i++)
                    {
                        if (i > 0) g.DrawLine(penBorder, curX, y, curX, y + rowH);
                        g.DrawString(cells[i], fontBody, brushText, new RectangleF(curX + 4, y, colW[i] - 8, rowH), aligns[i]);
                        curX += colW[i];
                    }
                    y += rowH;
                }
            }

            e.HasMorePages = false;
        }
        #endregion

        #region Xuất Dữ Liệu CSV / Excel (AD-5 Fallback Export)
        private void ExecuteExportCsv()
        {
            using var sfd = new SaveFileDialog();
            sfd.Filter = "Tệp Excel CSV (*.csv)|*.csv|Tất cả tệp (*.*)|*.*";
            sfd.Title = "Xuất Dữ Liệu Báo Cáo Ra Excel / CSV";
            sfd.FileName = _selectedReportMode switch
            {
                1 => $"BaoCaoQuyetToan_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                2 => $"BaoCaoVeDienTu_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                _ => $"ThongKeKpi_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    if (_selectedReportMode == 1 && _currentSettlement != null)
                    {
                        _reportService.ExportSettlementToCsv(_currentSettlement, sfd.FileName);
                    }
                    else if (_selectedReportMode == 2 && _currentTickets.Count > 0)
                    {
                        _reportService.ExportTicketsToCsv(_currentTickets, sfd.FileName);
                    }
                    else
                    {
                        // Export trực tiếp từ DataGridView
                        var sb = new StringBuilder();
                        sb.Append('\uFEFF'); // Thêm UTF-8 BOM để Excel hiển thị đúng tiếng Việt
                        sb.AppendLine(string.Join(",", dgvReportData.Columns.Cast<DataGridViewColumn>().Select(c => $"\"{c.HeaderText}\"")));
                        foreach (DataGridViewRow row in dgvReportData.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                sb.AppendLine(string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(c => $"\"{c.Value?.ToString()?.Replace("\"", "\"\"")}\"")));
                            }
                        }
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    }

                    MessageBox.Show($"Xuất dữ liệu báo cáo thành công ra file:\n{sfd.FileName}", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Có lỗi xảy ra trong quá trình xuất dữ liệu. Vui lòng thử lại sau.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        #endregion

        private void ResetFilterAndReload()
        {
            cboSuKien.SelectedIndex = 0;
            var now = DateTime.Today;
            dtpTuNgay.Value = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            dtpDenNgay.Value = new DateTime(now.Year, now.Month, 1).AddMonths(2).AddDays(-1);
            radQuyetToan.Checked = true;
            _selectedReportMode = 1;
            ExecuteLoadReport();
        }

        private void FrmReportStatistics_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
            {
                e.Handled = true;
                ExecutePrint();
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                e.Handled = true;
                ExecuteExportCsv();
            }
            else if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                ResetFilterAndReload();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

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
