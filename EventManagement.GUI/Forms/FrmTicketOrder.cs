using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Màn hình F11 - Bán vé & Đăng ký tham dự sự kiện
    /// Phụ trách: Thành viên 3 (Task T3-17 / Day 6)
    /// Nghiệp vụ:
    /// - BR-09: Kiểm tra số lượng vé còn lại trước khi xác nhận.
    /// - BR-10: 1 đơn hàng được mua từ 1 đến 10 vé, nhập danh sách người đi cùng.
    /// - BR-17: Sinh mã QR token ngẫu nhiên (GUID duy nhất) cho mỗi vé.
    /// - BR-18: Đảm bảo tính toàn vẹn dữ liệu đơn hàng và vé (SqlTransaction mô phỏng).
    /// </summary>
    public partial class FrmTicketOrder : FrmBase
    {
        private readonly ITicketOrderService _ticketOrderService;
        private bool _isLoading = true;
        private List<(string MaSuKien, string TenSuKien)> _events = new();
        private List<TicketTierSummaryDto> _currentTiers = new();
        private List<TicketDetailDto> _lastIssuedTickets = new();

        public Button BtnXacNhan => btnXacNhan;
        public Button BtnLamMoi => btnLamMoi;
        public ComboBox CboSuKien => cboSuKien;
        public ComboBox CboHangVe => cboHangVe;
        public DataGridView DgvNguoiDiCung => dgvNguoiDiCung;
        public DataGridView DgvIssuedTickets => dgvIssuedTickets;
        public GroupBox GrpOrderInfo => grpOrderInfo;
        public GroupBox GrpIssuedTickets => grpIssuedTickets;
        public Label LblSubTitle => lblSubTitle;

        public FrmTicketOrder() : this(new TicketOrderService())
        {
        }

        public FrmTicketOrder(ITicketOrderService ticketOrderService)
        {
            InitializeComponent();
            _ticketOrderService = ticketOrderService ?? new TicketOrderService();
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            StyleButton(btnXacNhan, ButtonRole.Success);
            StyleButton(btnLamMoi, ButtonRole.Secondary);
            StyleButton(btnInVe, ButtonRole.Primary);
            btnInVe.Text = "In vé điện tử";

            StyleDataGridView(dgvNguoiDiCung);
            StyleDataGridView(dgvIssuedTickets);
            dgvNguoiDiCung.AllowUserToAddRows = false;
            dgvNguoiDiCung.AllowUserToDeleteRows = false;

            // Bảng danh sách người sử dụng vé: Căn giữa tiêu đề và cột STT
            dgvNguoiDiCung.EnableHeadersVisualStyles = false;
            dgvNguoiDiCung.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSTT.HeaderText = "STT";
            colSTT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTenNguoiThamDu.HeaderText = "Họ và tên người sử dụng vé";
            foreach (DataGridViewColumn col in dgvNguoiDiCung.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Bảng danh sách vé điện tử: Căn giữa toàn bộ tiêu đề cột, chỉ viết hoa chữ cái đầu
            dgvIssuedTickets.EnableHeadersVisualStyles = false;
            dgvIssuedTickets.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvIssuedTickets.AutoGenerateColumns = false;
            dgvIssuedTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            colMaVe.HeaderText = "Mã vé";
            colMaVe.FillWeight = 14;
            colMaVe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            colAttendeeName.HeaderText = "Người tham gia";
            colAttendeeName.FillWeight = 24;
            colAttendeeName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            colMaQR.HeaderText = "Mã QR (token)";
            colMaQR.FillWeight = 30;
            colMaQR.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            colHangVeName.HeaderText = "Hạng vé";
            colHangVeName.FillWeight = 16;
            colHangVeName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            colGiaVe.HeaderText = "Đơn giá (VNĐ)";
            colGiaVe.FillWeight = 16;
            colGiaVe.DefaultCellStyle.Format = "N0";
            colGiaVe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            foreach (DataGridViewColumn col in dgvIssuedTickets.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgvIssuedTickets.CellDoubleClick -= dgvIssuedTickets_CellDoubleClick;
            dgvIssuedTickets.CellDoubleClick += dgvIssuedTickets_CellDoubleClick;

            // Anchor controls inside grpOrderInfo for full-screen layout
            cboSuKien.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboHangVe.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlTotalMoney.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpBuyer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtHoTen.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSoDienThoai.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDonVi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnXacNhan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            this.Resize += (s, e) =>
            {
                if (scMain != null && scMain.Width > 800)
                {
                    scMain.SplitterDistance = Math.Max(520, (int)(scMain.Width * 0.42));
                }
            };
            if (scMain != null && scMain.Width > 800)
            {
                scMain.SplitterDistance = Math.Max(520, (int)(scMain.Width * 0.42));
            }

            _isLoading = true;
            LoadEvents();
            _isLoading = false;

            if (cboSuKien.Items.Count > 0)
            {
                cboSuKien.SelectedIndex = 0;
                string maSuKien = GetSelectedEventCode();
                if (!string.IsNullOrEmpty(maSuKien))
                {
                    LoadTicketTiers(maSuKien);
                }
            }

            SyncAttendeeGridRows((int)nudSoLuong.Value);
            RecalculateTotal();

            DataSyncNotifier.GeneralDataChanged += OnDataSyncChanged;
            this.FormClosed += (s, e) => DataSyncNotifier.GeneralDataChanged -= OnDataSyncChanged;
        }

        private void OnDataSyncChanged()
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            try
            {
                string currentEvent = GetSelectedEventCode();
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        _isLoading = true;
                        LoadEvents();
                        _isLoading = false;
                        if (!string.IsNullOrEmpty(currentEvent))
                        {
                            for (int i = 0; i < cboSuKien.Items.Count; i++)
                            {
                                if (cboSuKien.Items[i] is EventLookupItem item && item.MaSuKien == currentEvent)
                                { cboSuKien.SelectedIndex = i; break; }
                            }
                        }
                    }));
                }
                else
                {
                    _isLoading = true;
                    LoadEvents();
                    _isLoading = false;
                }
            }
            catch { }
        }

        private string GetSelectedEventCode()
        {
            if (cboSuKien.SelectedItem is EventLookupItem item)
            {
                return item.MaSuKien;
            }
            if (cboSuKien.SelectedValue is string code && !string.IsNullOrEmpty(code))
            {
                return code;
            }
            return string.Empty;
        }

        private void LoadEvents()
        {
            try
            {
                var eventTuples = _ticketOrderService.GetAvailableEvents();
                var items = eventTuples.Select(e => new EventLookupItem(e.MaSuKien, e.TenSuKien)).ToList();
                cboSuKien.DataSource = null;
                cboSuKien.DisplayMember = nameof(EventLookupItem.TenSuKien);
                cboSuKien.ValueMember = nameof(EventLookupItem.MaSuKien);
                cboSuKien.DataSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sự kiện: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            string maSuKien = GetSelectedEventCode();
            if (!string.IsNullOrEmpty(maSuKien))
            {
                LoadTicketTiers(maSuKien);
            }
        }

        private void LoadTicketTiers(string maSuKien)
        {
            try
            {
                _currentTiers = _ticketOrderService.GetTicketTiersByEvent(maSuKien);
                cboHangVe.DataSource = null;
                cboHangVe.DisplayMember = "DisplayName";
                cboHangVe.ValueMember = "MaHangVe";
                cboHangVe.DataSource = _currentTiers;

                if (_currentTiers.Count > 0)
                {
                    cboHangVe.SelectedIndex = 0;
                }
                UpdateTierInfoDisplay();
                RecalculateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp hạng vé: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboHangVe_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTierInfoDisplay();
            RecalculateTotal();
        }

        private TicketTierSummaryDto? GetSelectedTier()
        {
            if (cboHangVe.SelectedItem is TicketTierSummaryDto dto) return dto;
            if (cboHangVe.SelectedValue != null)
            {
                string maHangVe = cboHangVe.SelectedValue.ToString() ?? string.Empty;
                return _currentTiers.FirstOrDefault(t => t.MaHangVe == maHangVe);
            }
            return null;
        }

        private void UpdateTierInfoDisplay()
        {
            var tier = GetSelectedTier();
            if (tier != null)
            {
                if (tier.IsClosed)
                {
                    lblTierInfo.Text = $"Đơn giá: {tier.GiaVe:N0} đ | HẠNG VÉ ĐÃ ĐÓNG BÁN (KHÔNG CHO PHÉP MUA)";
                    lblTierInfo.ForeColor = Color.FromArgb(185, 28, 28);
                    btnXacNhan.Enabled = false;
                    return;
                }

                btnXacNhan.Enabled = true;
                lblTierInfo.Text = $"Đơn giá: {tier.GiaVe:N0} đ | Còn lại: {tier.SoLuongConLai}/{tier.SoLuongPhatHanh} vé";
                if (tier.SoLuongConLai <= 0)
                {
                    lblTierInfo.ForeColor = Color.Red;
                    lblTierInfo.Text += " (HẾT VÉ)";
                }
                else if (tier.SoLuongConLai <= 10)
                {
                    lblTierInfo.ForeColor = Color.DarkOrange;
                    lblTierInfo.Text += " (SẮP HẾT)";
                }
                else
                {
                    lblTierInfo.ForeColor = Color.FromArgb(22, 101, 52);
                }
            }
            else
            {
                btnXacNhan.Enabled = true;
                lblTierInfo.Text = "Chưa có thông tin hạng vé";
                lblTierInfo.ForeColor = Color.Gray;
            }
        }

        private void nudSoLuong_ValueChanged(object sender, EventArgs e)
        {
            int qty = (int)nudSoLuong.Value;
            SyncAttendeeGridRows(qty);
            RecalculateTotal();
        }

        private void txtHoTen_TextChanged(object sender, EventArgs e)
        {
            if (dgvNguoiDiCung.Rows.Count > 0)
            {
                string buyer = string.IsNullOrWhiteSpace(txtHoTen.Text) ? "Người mua vé (Chính)" : txtHoTen.Text.Trim();
                dgvNguoiDiCung.Rows[0].Cells[1].Value = buyer;
            }
        }

        private void SyncAttendeeGridRows(int count)
        {
            // BR-10: 1 - 10 vé
            if (count < 1) count = 1;
            if (count > 10) count = 10;

            int currentCount = dgvNguoiDiCung.Rows.Count;
            if (currentCount < count)
            {
                for (int i = currentCount; i < count; i++)
                {
                    int stt = i + 1;
                    string defaultName = i == 0 
                        ? (string.IsNullOrWhiteSpace(txtHoTen.Text) ? "Người mua vé (Chính)" : txtHoTen.Text.Trim())
                        : $"Người tham dự #{stt}";
                    dgvNguoiDiCung.Rows.Add(stt.ToString(), defaultName);
                }
            }
            else if (currentCount > count)
            {
                for (int i = currentCount - 1; i >= count; i--)
                {
                    dgvNguoiDiCung.Rows.RemoveAt(i);
                }
            }

            if (dgvNguoiDiCung.Rows.Count > 0)
            {
                dgvNguoiDiCung.Rows[0].Cells[1].ReadOnly = false;
            }
        }

        private void RecalculateTotal()
        {
            var tier = GetSelectedTier();
            if (tier != null)
            {
                decimal total = (decimal)nudSoLuong.Value * tier.GiaVe;
                lblTongTien.Text = $"{total:N0} đ";
            }
            else
            {
                lblTongTien.Text = "0 đ";
            }
        }

        private bool ValidateOrderInput()
        {
            errorProvider.Clear();
            bool isValid = true;

            if (string.IsNullOrEmpty(GetSelectedEventCode()))
            {
                errorProvider.SetError(cboSuKien, "Vui lòng chọn sự kiện.");
                isValid = false;
            }

            var tier = GetSelectedTier();
            if (tier == null)
            {
                errorProvider.SetError(cboHangVe, "Vui lòng chọn hạng vé.");
                isValid = false;
            }
            else if (tier.IsClosed)
            {
                errorProvider.SetError(cboHangVe, $"Hạng vé '{tier.TenHangVe}' đã đóng bán, không thể đặt mua!");
                MessageBox.Show($"Hạng vé '{tier.TenHangVe}' đã đóng bán, không thể đặt mua!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            int qty = (int)nudSoLuong.Value;
            if (qty < 1 || qty > 10)
            {
                errorProvider.SetError(nudSoLuong, "Số lượng vé mỗi đơn hàng phải từ 1 đến 10 vé.");
                isValid = false;
            }

            // Kiểm tra số lượng tồn
            if (tier != null && tier.SoLuongConLai < qty)
            {
                errorProvider.SetError(nudSoLuong, $"Hạng vé chỉ còn lại {tier.SoLuongConLai} vé, không đủ đáp ứng số lượng {qty}.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errorProvider.SetError(txtHoTen, "Vui lòng nhập họ tên người mua vé.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                errorProvider.SetError(txtSoDienThoai, "Vui lòng nhập số điện thoại liên hệ.");
                isValid = false;
            }

            return isValid;
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (!ValidateOrderInput())
            {
                return;
            }

            var tier = GetSelectedTier();
            string maSuKien = GetSelectedEventCode();
            if (tier == null || string.IsNullOrEmpty(maSuKien)) return;

            // maSuKien already obtained
            string maHangVe = tier.MaHangVe;
            int qty = (int)nudSoLuong.Value;

            // Thu thập danh sách người tham dự (đúng chuẩn 1-đối-1 theo từng vị trí vé)
            var attendees = new List<string>();
            for (int i = 0; i < qty; i++)
            {
                string name = string.Empty;
                if (i < dgvNguoiDiCung.Rows.Count)
                {
                    name = dgvNguoiDiCung.Rows[i].Cells[1].Value?.ToString() ?? string.Empty;
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = (i == 0) ? txtHoTen.Text.Trim() : $"Người tham dự #{i + 1}";
                }
                attendees.Add(name.Trim());
            }

            var request = new RegisterTicketRequestDto
            {
                MaSuKien = maSuKien,
                MaHangVe = maHangVe,
                SoLuongMua = qty,
                HoTenNguoiMua = txtHoTen.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DonVi = txtDonVi.Text.Trim(),
                DanhSachNguoiDiCung = attendees
            };

            var confirmResult = MessageBox.Show(
                $"Xác nhận lập đơn hàng:\n- Sự kiện: {cboSuKien.Text}\n- Hạng vé: {tier.TenHangVe}\n- Số lượng: {qty} vé\n- Tổng tiền: {lblTongTien.Text}\n- Người mua: {request.HoTenNguoiMua}\n\nBạn có chắc chắn muốn xuất vé không?",
                "Xác nhận đặt vé",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var result = _ticketOrderService.RegisterTicketOrder(request);
                if (result.IsSuccess && result.Data != null)
                {
                    _lastIssuedTickets = result.Data.DanhSachVe ?? new List<TicketDetailDto>();
                    BindIssuedTickets(_lastIssuedTickets);

                    MessageBox.Show(
                        $"Đặt vé thành công!\nMã đơn đăng ký: {result.Data.MaDangKy}\nTổng cộng: {result.Data.SoLuong} vé đã được phát hành kèm mã QR hợp lệ.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Cập nhật lại tồn kho hạng vé trên giao diện
                    LoadTicketTiers(maSuKien);
                    cboHangVe.SelectedValue = maHangVe;
                    UpdateTierInfoDisplay();
                }
                else
                {
                    MessageBox.Show("Không thể tạo đơn hàng: " + (result.Message ?? "Lỗi không xác định"), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý đặt vé: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindIssuedTickets(List<TicketDetailDto> tickets)
        {
            dgvIssuedTickets.Rows.Clear();
            foreach (var t in tickets)
            {
                dgvIssuedTickets.Rows.Add(
                    t.MaVe,
                    t.TenNguoiThamGia,
                    t.MaQR,
                    t.TenHangVe,
                    t.GiaVe.ToString("N0") + " đ"
                );
            }

            lblTicketCountSummary.Text = $"Tổng cộng: {tickets.Count} vé đã xuất thành công";
            btnInVe.Enabled = tickets.Count > 0;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            txtDonVi.Clear();
            nudSoLuong.Value = 1;
            dgvIssuedTickets.Rows.Clear();
            lblTicketCountSummary.Text = "Tổng cộng: 0 vé đã xuất";
            btnInVe.Enabled = false;
            errorProvider.Clear();

            if (cboSuKien.SelectedValue != null)
            {
                LoadTicketTiers(GetSelectedEventCode());
            }

            SyncAttendeeGridRows(1);
            RecalculateTotal();
            txtHoTen.Focus();
        }

        private void btnInVe_Click(object sender, EventArgs e)
        {
            if (_lastIssuedTickets == null || _lastIssuedTickets.Count == 0)
            {
                MessageBox.Show("Chưa có danh sách vé vừa xuất để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = 0;
            if (dgvIssuedTickets.SelectedRows.Count > 0)
            {
                selectedIndex = dgvIssuedTickets.SelectedRows[0].Index;
            }

            using var dlg = new FrmTicketPrintDialog(
                cboSuKien.Text,
                txtHoTen.Text.Trim(),
                txtSoDienThoai.Text.Trim(),
                _lastIssuedTickets,
                selectedIndex);
            dlg.ShowDialog(this);
        }

        private void dgvIssuedTickets_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (_lastIssuedTickets == null || _lastIssuedTickets.Count == 0) return;
            if (e.RowIndex >= 0 && e.RowIndex < _lastIssuedTickets.Count)
            {
                using var dlg = new FrmTicketPrintDialog(
                    cboSuKien.Text,
                    txtHoTen.Text.Trim(),
                    txtSoDienThoai.Text.Trim(),
                    _lastIssuedTickets,
                    e.RowIndex);
                dlg.ShowDialog(this);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                btnXacNhan.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.N))
            {
                btnLamMoi.PerformClick();
                return true;
            }
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    /// <summary>
    /// Lớp bọc DTO dữ liệu nguồn cho ComboBox Sự kiện (WinForms ComboBox data binding yêu cầu property).
    /// </summary>
    public class EventLookupItem
    {
        public string MaSuKien { get; set; } = string.Empty;
        public string TenSuKien { get; set; } = string.Empty;

        public EventLookupItem() { }

        public EventLookupItem(string maSuKien, string tenSuKien)
        {
            MaSuKien = maSuKien;
            TenSuKien = tenSuKien;
        }

        public override string ToString() => TenSuKien;
    }
}
