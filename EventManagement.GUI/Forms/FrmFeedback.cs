using System;
using System.Collections.Generic;
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
    /// Màn hình F18 – Quản lý khảo sát & ý kiến đánh giá sau sự kiện
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// Tuân thủ: Mẫu bố cục 2 (Danh sách kết hợp Nhập liệu), 3 trạng thái Form (Xem - Thêm - Sửa), BR-16.
    /// </summary>
    public partial class FrmFeedback : FrmBase
    {
        private readonly IFeedbackService _feedbackService;
        private FormMode _currentMode = FormMode.View;
        private List<FeedbackDto> _currentList = new();
        private bool _isLoading = true;

        public Button BtnThem => btnThem;
        public Button BtnSua => btnSua;
        public Button BtnXoa => btnXoa;
        public Button BtnLuu => btnLuu;
        public Button BtnHuy => btnHuy;
        public Button BtnLamMoi => btnLamMoi;
        public DataGridView DgvPhanHoi => dgvPhanHoi;
        public Panel PnlKpi => pnlKpi;
        public Label LblKpiDanhGiaXauValue => lblKpiDanhGiaXauValue;
        public Label LblKpiHaiLongValue => lblKpiHaiLongValue;
        public Label LblKpiDiemTbValue => lblKpiDiemTbValue;
        public Label LblKpiTongPhValue => lblKpiTongPhValue;

        public FrmFeedback() : this(new FeedbackService())
        {
        }

        public FrmFeedback(IFeedbackService feedbackService)
        {
            InitializeComponent();
            _feedbackService = feedbackService;
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            StyleButton(btnThem, ButtonRole.Success);
            StyleButton(btnSua, ButtonRole.Primary);
            StyleButton(btnXoa, ButtonRole.Danger);
            StyleButton(btnXuatExcel, ButtonRole.Secondary);
            StyleButton(btnLuu, ButtonRole.Primary);
            StyleButton(btnHuy, ButtonRole.Secondary);
            StyleButton(btnLamMoi, ButtonRole.Secondary);

            dgvPhanHoi.AutoGenerateColumns = false;

            foreach (DataGridViewColumn col in dgvPhanHoi.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgvPhanHoi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            colMaPhanHoi.FillWeight = 10;
            colTenSuKien.FillWeight = 20;
            colTenNguoiThamGia.FillWeight = 15;
            colEmail.FillWeight = 15;
            colSoSaoText.FillWeight = 12;
            colNoiDungPhanHoi.FillWeight = 20;
            colNgayDanhGia.FillWeight = 12;

            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            pnlKpi.Resize += (s, e) => LayoutKpiCards();
            LayoutKpiCards();
        }

        private void LayoutKpiCards()
        {
            if (pnlKpi == null) return;
            int totalWidth = pnlKpi.ClientSize.Width;
            if (totalWidth <= 0) return;
            int sectionWidth = totalWidth / 4;

            int x0 = 15;
            lblKpiDiemTbTitle.Location = new System.Drawing.Point(x0, 13);
            lblKpiDiemTbValue.Location = new System.Drawing.Point(x0 + lblKpiDiemTbTitle.Width + 6, 10);

            int x1 = sectionWidth + 10;
            lblKpiTongPhTitle.Location = new System.Drawing.Point(x1, 13);
            lblKpiTongPhValue.Location = new System.Drawing.Point(x1 + lblKpiTongPhTitle.Width + 6, 10);

            int x2 = sectionWidth * 2 + 10;
            lblKpiHaiLongTitle.Location = new System.Drawing.Point(x2, 13);
            lblKpiHaiLongValue.Location = new System.Drawing.Point(x2 + lblKpiHaiLongTitle.Width + 6, 10);

            int x3 = sectionWidth * 3 + 10;
            lblKpiDanhGiaXauTitle.Location = new System.Drawing.Point(x3, 13);
            lblKpiDanhGiaXauValue.Location = new System.Drawing.Point(x3 + lblKpiDanhGiaXauTitle.Width + 6, 10);
        }

        private void FrmFeedback_Load(object sender, EventArgs e)
        {
            _isLoading = true;
            LoadAuxiliaryData();
            _isLoading = false;

            LoadData();
            SetFormState(FormMode.View);
        }

        private void LoadAuxiliaryData()
        {
            // Nạp sự kiện cho bộ lọc
            cboLocSuKien.Items.Clear();
            cboLocSuKien.Items.Add(new ComboBoxItem("Tất cả", "Tất cả"));

            var events = _feedbackService.GetCompletedEvents();
            foreach (var evt in events)
            {
                cboLocSuKien.Items.Add(new ComboBoxItem(evt.Key, $"{evt.Key} - {evt.Value}"));
            }
            cboLocSuKien.SelectedIndex = 0;

            // Nạp sự kiện cho GroupBox nhập liệu
            cboSuKien.Items.Clear();
            foreach (var evt in events)
            {
                cboSuKien.Items.Add(new ComboBoxItem(evt.Key, $"{evt.Key} - {evt.Value}"));
            }
            if (cboSuKien.Items.Count > 0) cboSuKien.SelectedIndex = 0;

            // Nạp danh sách người tham gia
            cboNguoiThamGia.Items.Clear();
            var attendees = _feedbackService.GetEligibleAttendees();
            foreach (var att in attendees)
            {
                cboNguoiThamGia.Items.Add(new ComboBoxItem(att.Key, $"{att.Key} - {att.Value}"));
            }
            if (cboNguoiThamGia.Items.Count > 0) cboNguoiThamGia.SelectedIndex = 0;

            cboLocMucSao.SelectedIndex = 0; // "Tất cả"
            cboDiemDanhGia.SelectedIndex = 0; // "5 sao"
        }

        private void LoadData()
        {
            if (_isLoading) return;

            string? keyword = txtTimKiem.Text.Trim();

            string? selectedEventId = null;
            if (cboLocSuKien.SelectedItem is ComboBoxItem item && item.Value != "Tất cả")
            {
                selectedEventId = item.Value;
            }

            int? ratingFilter = null;
            if (cboLocMucSao.SelectedIndex > 0)
            {
                // Index 1 = 5 sao, Index 2 = 4 sao, Index 3 = 3 sao, Index 4 = 2 sao, Index 5 = 1 sao
                ratingFilter = 6 - cboLocMucSao.SelectedIndex;
            }

            var result = _feedbackService.GetAll(keyword, selectedEventId, ratingFilter);
            if (result.IsSuccess && result.Data != null)
            {
                _currentList = result.Data;
                dgvPhanHoi.DataSource = null;
                dgvPhanHoi.DataSource = _currentList;

                UpdateKpiCards(selectedEventId);

                if (_currentList.Count > 0)
                {
                    dgvPhanHoi.Rows[0].Selected = true;
                    DisplaySelectedRow(_currentList[0]);
                }
                else
                {
                    ClearInputs();
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateKpiCards(string? selectedEventId)
        {
            var summaryResult = _feedbackService.GetSummary(selectedEventId);
            if (summaryResult.IsSuccess && summaryResult.Data != null)
            {
                var s = summaryResult.Data;
                lblKpiDiemTbValue.Text = $"{s.DiemTrungBinh:0.0} / 5.0 ★";
                lblKpiTongPhValue.Text = $"{s.TongSoPhanHoi} ý kiến";
                lblKpiHaiLongValue.Text = $"{s.TyLeHaiLong:0.#} %";

                double tyLeDanhGiaXau = s.TongSoPhanHoi > 0
                    ? Math.Round((double)(s.SoLuong1Sao + s.SoLuong2Sao) / s.TongSoPhanHoi * 100.0, 1)
                    : 0;
                lblKpiDanhGiaXauValue.Text = $"{tyLeDanhGiaXau:0.#} %";
                lblKpiDanhGiaXauValue.ForeColor = tyLeDanhGiaXau > 0
                    ? System.Drawing.Color.FromArgb(220, 38, 38)
                    : System.Drawing.Color.FromArgb(71, 85, 105);

                // Tô màu sinh động theo chỉ số hài lòng
                if (s.DiemTrungBinh >= 4.5)
                    lblKpiDiemTbValue.ForeColor = System.Drawing.Color.FromArgb(22, 163, 74); // Xanh lá
                else if (s.DiemTrungBinh >= 3.5)
                    lblKpiDiemTbValue.ForeColor = System.Drawing.Color.FromArgb(217, 119, 6);  // Vàng cam
                else
                    lblKpiDiemTbValue.ForeColor = System.Drawing.Color.FromArgb(220, 38, 38);  // Đỏ
            }
        }

        private void DisplaySelectedRow(FeedbackDto dto)
        {
            if (_currentMode != FormMode.View) return;

            txtMaPhanHoi.Text = dto.MaPhanHoi;

            // Chọn sự kiện
            for (int i = 0; i < cboSuKien.Items.Count; i++)
            {
                if (cboSuKien.Items[i] is ComboBoxItem item && string.Equals(item.Value, dto.MaSuKien, StringComparison.OrdinalIgnoreCase))
                {
                    cboSuKien.SelectedIndex = i;
                    break;
                }
            }

            // Chọn người tham gia
            for (int i = 0; i < cboNguoiThamGia.Items.Count; i++)
            {
                if (cboNguoiThamGia.Items[i] is ComboBoxItem item && string.Equals(item.Value, dto.MaNguoiThamGia, StringComparison.OrdinalIgnoreCase))
                {
                    cboNguoiThamGia.SelectedIndex = i;
                    break;
                }
            }

            // Chọn mức sao (5 sao = index 0, 4 sao = index 1, ...)
            int starIndex = Math.Clamp(5 - dto.DiemDanhGia, 0, 4);
            cboDiemDanhGia.SelectedIndex = starIndex;

            dtpNgayDanhGia.Value = dto.NgayDanhGia;
            txtNoiDungPhanHoi.Text = dto.NoiDungPhanHoi ?? string.Empty;

            errBaoLoi.Clear();
        }

        private void SetFormState(FormMode mode)
        {
            _currentMode = mode;

            bool isEditing = mode == FormMode.Add || mode == FormMode.Edit;

            // Vùng nhập liệu
            txtMaPhanHoi.ReadOnly = mode != FormMode.Add;
            cboSuKien.Enabled = mode == FormMode.Add; // Khóa sự kiện khi sửa
            cboNguoiThamGia.Enabled = mode == FormMode.Add; // Khóa người đánh giá khi sửa (BR-16)
            cboDiemDanhGia.Enabled = isEditing;
            dtpNgayDanhGia.Enabled = isEditing;
            txtNoiDungPhanHoi.ReadOnly = !isEditing;

            // Nút bấm
            btnThem.Visible = !isEditing;
            btnSua.Visible = !isEditing;
            btnXoa.Visible = !isEditing;
            btnXuatExcel.Visible = !isEditing;

            btnLuu.Visible = isEditing;
            btnHuy.Visible = isEditing;

            btnThem.Enabled = !isEditing;
            btnSua.Enabled = !isEditing && _currentList.Count > 0;
            btnXoa.Enabled = !isEditing && _currentList.Count > 0;
            btnLuu.Enabled = isEditing;
            btnHuy.Enabled = isEditing;

            if (isEditing)
            {
                btnLuu.Location = new System.Drawing.Point(15, 10);
                btnHuy.Location = new System.Drawing.Point(155, 10);
            }
            else
            {
                btnThem.Location = new System.Drawing.Point(15, 10);
                btnSua.Location = new System.Drawing.Point(145, 10);
                btnXoa.Location = new System.Drawing.Point(275, 10);
                btnXuatExcel.Location = new System.Drawing.Point(405, 10);
            }

            // Danh sách & bộ lọc
            dgvPhanHoi.Enabled = !isEditing;
            pnlFilter.Enabled = !isEditing;

            errBaoLoi.Clear();
        }

        private void ClearInputs()
        {
            txtMaPhanHoi.Text = string.Empty;
            if (cboSuKien.Items.Count > 0) cboSuKien.SelectedIndex = 0;
            if (cboNguoiThamGia.Items.Count > 0) cboNguoiThamGia.SelectedIndex = 0;
            cboDiemDanhGia.SelectedIndex = 0;
            dtpNgayDanhGia.Value = DateTime.Now;
            txtNoiDungPhanHoi.Text = string.Empty;
            errBaoLoi.Clear();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.Add);
            ClearInputs();
            txtMaPhanHoi.Text = _feedbackService.SuggestNextCode();
            txtNoiDungPhanHoi.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvPhanHoi.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phản hồi cần chỉnh sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SetFormState(FormMode.Edit);
            txtNoiDungPhanHoi.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvPhanHoi.CurrentRow?.DataBoundItem is not FeedbackDto selected)
            {
                MessageBox.Show("Vui lòng chọn phản hồi cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phản hồi [{selected.MaPhanHoi}] của người tham gia [{selected.TenNguoiThamGia}] không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var result = _feedbackService.Delete(selected.MaPhanHoi);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi khi xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var selectedEvent = cboSuKien.SelectedItem as ComboBoxItem;
            var selectedAttendee = cboNguoiThamGia.SelectedItem as ComboBoxItem;

            int rating = 5 - cboDiemDanhGia.SelectedIndex; // Index 0 -> 5 sao, Index 4 -> 1 sao

            var dto = new FeedbackDto
            {
                MaPhanHoi = txtMaPhanHoi.Text.Trim(),
                MaSuKien = selectedEvent?.Value ?? string.Empty,
                TenSuKien = selectedEvent?.Text?.Split('-')[1].Trim() ?? string.Empty,
                MaNguoiThamGia = selectedAttendee?.Value ?? string.Empty,
                TenNguoiThamGia = selectedAttendee?.Text?.Split('-')[1].Trim().Split('(')[0].Trim() ?? string.Empty,
                DiemDanhGia = rating,
                NoiDungPhanHoi = string.IsNullOrWhiteSpace(txtNoiDungPhanHoi.Text) ? null : txtNoiDungPhanHoi.Text.Trim(),
                NgayDanhGia = dtpNgayDanhGia.Value
            };

            ServiceResult result;
            if (_currentMode == FormMode.Add)
            {
                result = _feedbackService.Add(dto);
            }
            else
            {
                result = _feedbackService.Update(dto);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetFormState(FormMode.View);
                LoadData();
                SelectRowByCode(dto.MaPhanHoi);
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi lưu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            errBaoLoi.Clear();
            SetFormState(FormMode.View);
            if (dgvPhanHoi.CurrentRow?.DataBoundItem is FeedbackDto selected)
            {
                DisplaySelectedRow(selected);
            }
            else
            {
                LoadData();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            _isLoading = true;
            txtTimKiem.Clear();
            cboLocSuKien.SelectedIndex = 0;
            cboLocMucSao.SelectedIndex = 0;
            _isLoading = false;

            LoadData();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (_currentList == null || _currentList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu phản hồi để xuất file.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Tệp CSV (Excel UTF-8) (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = $"KhaoSatPhanHoi_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Mã PH,Sự kiện,Người tham gia,Email,Điểm sao,Mức đánh giá,Ý kiến góp ý,Thời gian gửi");

                    foreach (var p in _currentList)
                    {
                        sb.AppendLine($"\"{p.MaPhanHoi}\",\"{EscapeCsv(p.TenSuKien)}\",\"{EscapeCsv(p.TenNguoiThamGia)}\",\"{p.Email}\",{p.DiemDanhGia},\"{p.SoSaoText}\",\"{EscapeCsv(p.NoiDungPhanHoi)}\",\"{p.NgayDanhGiaFormatted}\"");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất danh sách phản hồi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Replace("\"", "\"\"");
        }

        private bool ValidateInputs()
        {
            errBaoLoi.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtMaPhanHoi.Text))
            {
                errBaoLoi.SetError(txtMaPhanHoi, "Mã phản hồi không được để trống.");
                isValid = false;
            }

            if (cboSuKien.SelectedItem == null)
            {
                errBaoLoi.SetError(cboSuKien, "Vui lòng chọn sự kiện được đánh giá.");
                isValid = false;
            }

            if (cboNguoiThamGia.SelectedItem == null)
            {
                errBaoLoi.SetError(cboNguoiThamGia, "Vui lòng chọn người tham gia đánh giá.");
                isValid = false;
            }

            return isValid;
        }

        private void SelectRowByCode(string code)
        {
            foreach (DataGridViewRow row in dgvPhanHoi.Rows)
            {
                if (row.DataBoundItem is FeedbackDto dto && string.Equals(dto.MaPhanHoi, code, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    DisplaySelectedRow(dto);
                    break;
                }
            }
        }

        private void dgvPhanHoi_SelectionChanged(object sender, EventArgs e)
        {
            if (_isLoading || _currentMode != FormMode.View) return;

            if (dgvPhanHoi.CurrentRow?.DataBoundItem is FeedbackDto selected)
            {
                DisplaySelectedRow(selected);
            }
        }

        private void cboLocSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboLocMucSao_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void FrmFeedback_KeyDown(object sender, KeyEventArgs e)
        {
            if (_currentMode == FormMode.View)
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnThem.PerformClick();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F3)
                {
                    btnSua.PerformClick();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Delete && !(ActiveControl is TextBoxBase))
                {
                    btnXoa.PerformClick();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F5)
                {
                    btnLamMoi.PerformClick();
                    e.Handled = true;
                }
            }
            else
            {
                if (e.Control && e.KeyCode == Keys.S)
                {
                    btnLuu.PerformClick();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    btnHuy.PerformClick();
                    e.Handled = true;
                }
            }
        }

        private class ComboBoxItem
        {
            public string Value { get; }
            public string Text { get; }

            public ComboBoxItem(string value, string text)
            {
                Value = value;
                Text = text;
            }

            public override string ToString() => Text;
        }
    }
}
