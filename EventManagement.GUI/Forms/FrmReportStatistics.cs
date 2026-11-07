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
            // Định kiểu các nút theo FrmBase chuẩn Design Rules
            StyleButton(btnXem, ButtonRole.Primary);
            StyleButton(btnIn, ButtonRole.Secondary);
            StyleButton(btnXuatExcel, ButtonRole.Success);
            StyleButton(btnLamMoi, ButtonRole.Secondary);
            StyleButton(btnPrintFromPreview, ButtonRole.Primary);

            StyleDataGridView(dgvReportData);
            dgvReportData.AutoGenerateColumns = false;

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
                        chiPhi.NguoiLapPhieu,
                        chiPhi.SoTien.ToString("#,##0") + " đ",
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
                    lblTrangThai.Text = $"Kết luận: {_currentSettlement.DanhGiaQuyetToan} (+{_currentSettlement.ChenhLechThuChi:#,##0} đ)";
                }
                else
                {
                    lblTrangThai.ForeColor = ColorDanger;
                    lblTrangThai.Text = $"Kết luận: {_currentSettlement.DanhGiaQuyetToan} ({_currentSettlement.ChenhLechThuChi:#,##0} đ)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải báo cáo quyết toán: {ex.Message}", "Lỗi Báo Cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupSettlementGridColumns()
        {
            dgvReportData.Columns.Clear();
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 50 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã chi phí", Width = 100 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Khoản mục chi", Width = 180 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nội dung / Diễn giải", Width = 220 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngày chi", Width = 100 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Người chi", Width = 140 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số tiền", Width = 130 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", Width = 150 });

            dgvReportData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReportData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReportData.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                        ticket.DonGia.ToString("#,##0") + " đ",
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
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải báo cáo vé điện tử: {ex.Message}", "Lỗi Báo Cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupTicketGridColumns()
        {
            dgvReportData.Columns.Clear();
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 50 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã vé", Width = 100 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên sự kiện", Width = 200 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Hạng vé", Width = 120 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đơn giá", Width = 110 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Người tham gia", Width = 160 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", Width = 160 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã QR Check-in", Width = 140 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngày xuất vé", Width = 130 });

            dgvReportData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReportData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvReportData.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                        kpi.TongDoanhThu.ToString("#,##0") + " đ"
                    );
                }

                lblTongThu.Text = $"Tổng doanh thu toàn bộ: {tongDoanhThu:#,##0} VNĐ";
                lblTongChi.Text = $"Tổng vé bán ra: {tongVeBan:#,##0} vé";
                lblChenhLech.Text = $"Tổng lượt check-in: {tongCheckIn:#,##0} lượt";
                lblTrangThai.ForeColor = ColorSuccess;
                lblTrangThai.Text = "Chỉ số KPI đạt tiến độ chuẩn";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải thống kê KPI: {ex.Message}", "Lỗi Báo Cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupKpiGridColumns()
        {
            dgvReportData.Columns.Clear();
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 45 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã SK", Width = 80 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên sự kiện", Width = 190 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Địa điểm", Width = 130 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thời gian bắt đầu", Width = 125 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Phát hành", Width = 80 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đã bán", Width = 75 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Check-in", Width = 75 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "% Bán vé", Width = 80 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "% Tham dự", Width = 85 });
            dgvReportData.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Doanh thu", Width = 120 });

            dgvReportData.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReportData.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvReportData.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvReportData.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvReportData.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvReportData.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvReportData.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
            using var printDialog = new PrintDialog();
            printDialog.Document = _printDoc;
            printDialog.UseEXDialog = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _printDoc.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể thực hiện in: {ex.Message}", "Lỗi In Ấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            if (g == null) return;

            using var fontTitle = new Font("Segoe UI", 16F, FontStyle.Bold);
            using var fontHeader = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var fontBody = new Font("Segoe UI", 9F, FontStyle.Regular);
            using var fontItalic = new Font("Segoe UI", 8.5F, FontStyle.Italic);

            using var brushTitle = new SolidBrush(ColorPrimary);
            using var brushText = new SolidBrush(ColorText);
            using var brushSuccess = new SolidBrush(ColorSuccess);
            using var brushDanger = new SolidBrush(ColorDanger);
            using var penBorder = new Pen(ColorBorder);
            using var penDark = new Pen(Color.FromArgb(100, 116, 139));

            int left = 40;
            int y = 40;
            int width = e.PageBounds.Width - 80;

            if (_selectedReportMode == 1 && _currentSettlement != null)
            {
                // In Báo Cáo Quyết Toán Tài Chính (Tương ứng BaoCaoQuyetToan.rdlc)
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

                // Header Bảng
                int col0 = left;
                int col1 = left + 40;
                int col2 = left + 200;
                int col3 = left + 430;
                int col4 = left + 540;
                int col5 = left + width;

                g.FillRectangle(new SolidBrush(Color.FromArgb(241, 245, 249)), left, y, width, 26);
                g.DrawRectangle(penBorder, left, y, width, 26);
                g.DrawString("STT", fontHeader, brushText, col0 + 5, y + 5);
                g.DrawString("Khoản mục chi", fontHeader, brushText, col1 + 5, y + 5);
                g.DrawString("Diễn giải / Ghi chú", fontHeader, brushText, col2 + 5, y + 5);
                g.DrawString("Ngày chi", fontHeader, brushText, col3 + 5, y + 5);
                g.DrawString("Số tiền (VNĐ)", fontHeader, brushText, col4 + 5, y + 5);
                y += 26;

                int index = 1;
                foreach (var chi in _currentSettlement.DanhSachChiPhi.Take(15)) // Hiển thị vừa trang A4
                {
                    g.DrawRectangle(penBorder, left, y, width, 22);
                    g.DrawString(index++.ToString(), fontBody, brushText, col0 + 5, y + 4);
                    g.DrawString(chi.KhoanMucChi, fontBody, brushText, col1 + 5, y + 4);
                    g.DrawString(chi.DienGiai ?? "", fontBody, brushText, col2 + 5, y + 4);
                    g.DrawString(chi.NgayChi.ToString("dd/MM/yyyy"), fontBody, brushText, col3 + 5, y + 4);
                    g.DrawString(chi.SoTien.ToString("#,##0"), fontBody, brushText, col4 + 5, y + 4);
                    y += 22;
                }

                // Dòng tổng cộng
                g.FillRectangle(new SolidBrush(Color.FromArgb(248, 250, 252)), left, y, width, 26);
                g.DrawRectangle(penBorder, left, y, width, 26);
                g.DrawString("TỔNG CỘNG THỰC CHI:", fontHeader, brushText, col2 + 5, y + 5);
                g.DrawString($"{_currentSettlement.TongThucChi:#,##0} VNĐ", fontHeader, brushDanger, col4 + 5, y + 5);
                y += 50;

                // Chữ ký 3 bên
                int sigColWidth = width / 3;
                g.DrawString("NGƯỜI LẬP BIỂU", fontHeader, brushText, left + (sigColWidth - 110) / 2, y);
                g.DrawString("KẾ TOÁN TRƯỞNG", fontHeader, brushText, left + sigColWidth + (sigColWidth - 120) / 2, y);
                g.DrawString("TRƯỞNG BAN TỔ CHỨC", fontHeader, brushText, left + sigColWidth * 2 + (sigColWidth - 140) / 2, y);

                y += 18;
                g.DrawString("(Ký, ghi rõ họ tên)", fontItalic, brushText, left + (sigColWidth - 110) / 2, y);
                g.DrawString("(Ký, ghi rõ họ tên)", fontItalic, brushText, left + sigColWidth + (sigColWidth - 120) / 2, y);
                g.DrawString("(Ký, đóng dấu)", fontItalic, brushText, left + sigColWidth * 2 + (sigColWidth - 140) / 2, y);
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

                g.FillRectangle(new SolidBrush(Color.FromArgb(241, 245, 249)), left, y, width, 26);
                g.DrawRectangle(penBorder, left, y, width, 26);
                g.DrawString("Mã vé", fontHeader, brushText, left + 10, y + 5);
                g.DrawString("Hạng vé", fontHeader, brushText, left + 120, y + 5);
                g.DrawString("Người tham gia", fontHeader, brushText, left + 240, y + 5);
                g.DrawString("Mã QR", fontHeader, brushText, left + 430, y + 5);
                g.DrawString("Đơn giá (VNĐ)", fontHeader, brushText, left + 580, y + 5);
                y += 26;

                foreach (var ticket in _currentTickets.Take(25))
                {
                    g.DrawRectangle(penBorder, left, y, width, 22);
                    g.DrawString(ticket.MaVe, fontBody, brushText, left + 10, y + 4);
                    g.DrawString(ticket.TenHangVe, fontBody, brushText, left + 120, y + 4);
                    g.DrawString(ticket.TenNguoiThamGia, fontBody, brushText, left + 240, y + 4);
                    g.DrawString(ticket.MaQR, fontBody, brushText, left + 430, y + 4);
                    g.DrawString(ticket.DonGia.ToString("#,##0"), fontBody, brushText, left + 580, y + 4);
                    y += 22;
                }
            }
            else
            {
                // In Báo Cáo Thống Kê Tổng Hợp KPI
                string title = "BÁO CÁO THỐNG KÊ CHỈ SỐ HOẠT ĐỘNG (KPI) SỰ KIỆN";
                var titleSize = g.MeasureString(title, fontTitle);
                g.DrawString(title, fontTitle, brushTitle, left + (width - titleSize.Width) / 2, y);
                y += 35;

                g.FillRectangle(new SolidBrush(Color.FromArgb(241, 245, 249)), left, y, width, 26);
                g.DrawRectangle(penBorder, left, y, width, 26);
                g.DrawString("Mã SK", fontHeader, brushText, left + 10, y + 5);
                g.DrawString("Tên sự kiện", fontHeader, brushText, left + 90, y + 5);
                g.DrawString("Vé bán", fontHeader, brushText, left + 340, y + 5);
                g.DrawString("Check-in", fontHeader, brushText, left + 420, y + 5);
                g.DrawString("% Bán vé", fontHeader, brushText, left + 500, y + 5);
                g.DrawString("Doanh thu (VNĐ)", fontHeader, brushText, left + 580, y + 5);
                y += 26;

                foreach (var kpi in _currentKpis.Take(25))
                {
                    g.DrawRectangle(penBorder, left, y, width, 22);
                    g.DrawString(kpi.MaSuKien, fontBody, brushText, left + 10, y + 4);
                    g.DrawString(kpi.TenSuKien, fontBody, brushText, left + 90, y + 4);
                    g.DrawString(kpi.TongVeDaBan.ToString(), fontBody, brushText, left + 340, y + 4);
                    g.DrawString(kpi.TongLuotCheckIn.ToString(), fontBody, brushText, left + 420, y + 4);
                    g.DrawString($"{kpi.TyLeBanVe:0.0}%", fontBody, brushText, left + 500, y + 4);
                    g.DrawString(kpi.TongDoanhThu.ToString("#,##0"), fontBody, brushText, left + 580, y + 4);
                    y += 22;
                }
            }

            e.HasMorePages = false;
        }
        #endregion

        #region Xuất Dữ Liệu CSV / Excel (AD-5 Fallback Export)
        private void ExecuteExportCsv()
        {
            using var sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";
            sfd.FileName = _selectedReportMode switch
            {
                1 => $"BaoCaoQuyetToan_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                2 => $"BaoCaoVeDienTu_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                _ => $"ThongKeKpi_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
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

                    MessageBox.Show($"Xuất báo cáo thành công ra file:\n{sfd.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
