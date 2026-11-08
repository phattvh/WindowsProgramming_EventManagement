using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Màn hình F17 - Quản lý chi phí & ngân sách sự kiện
    /// Phụ trách: Thành viên 3 (Thuật toán lõi, Bán vé, Ngân sách & Báo cáo - Task T3-16)
    /// Tuân thủ: Mẫu bố cục 2 (Archetype 2), 3 trạng thái màn hình (Xem - Thêm - Sửa), quy tắc BR-14 (Chi phí > 0) và BR-15 (Cảnh báo bội chi 80%).
    /// </summary>
    public partial class FrmBudgetExpense : FrmBase
    {
        private readonly IBudgetService _budgetService;
        private bool _isLoading = true;
        private FormMode _currentMode = FormMode.View;
        private List<ExpenseDto> _currentList = new();
        private List<(string MaSuKien, string TenSuKien, decimal NganSachDuKien)> _events = new();

        public Button BtnThem => btnThem;
        public Button BtnSua => btnSua;
        public Button BtnXoa => btnXoa;
        public Button BtnLuu => btnLuu;
        public Button BtnHuy => btnHuy;
        public Button BtnLamMoi => btnLamMoi;
        public DataGridView DgvChiPhi => dgvChiPhi;
        public Panel PnlBudgetCards => pnlBudgetCards;
        public Panel PnlCardBudget => pnlCardBudget;
        public Panel PnlCardSpent => pnlCardSpent;
        public Panel PnlCardRemaining => pnlCardRemaining;
        public Panel PnlCardPercent => pnlCardPercent;
        public Label LblSubTitle => lblSubTitle;

        public FrmBudgetExpense() : this(new BudgetService())
        {
        }

        public FrmBudgetExpense(IBudgetService budgetService)
        {
            InitializeComponent();
            _budgetService = budgetService ?? new BudgetService();
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            StyleButton(btnThem, ButtonRole.Success);
            StyleButton(btnSua, ButtonRole.Primary);
            StyleButton(btnXoa, ButtonRole.Danger);
            StyleButton(btnLuu, ButtonRole.Primary);
            StyleButton(btnHuy, ButtonRole.Secondary);
            StyleButton(btnLamMoi, ButtonRole.Secondary);
            StyleButton(btnXuatExcel, ButtonRole.Secondary);

            StyleDataGridView(dgvChiPhi);
            dgvChiPhi.AutoGenerateColumns = false;

            foreach (DataGridViewColumn col in dgvChiPhi.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgvChiPhi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            colMaChiPhi.FillWeight = 10;
            colKhoanMucChi.FillWeight = 22;
            colSoTien.FillWeight = 15;
            colNgayChi.FillWeight = 11;
            colNguoiLap.FillWeight = 14;
            colTrangThai.FillWeight = 10;
            colDienGiai.FillWeight = 18;

            // Anchor filter bar & details
            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboFilterSuKien.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            txtMaChiPhi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboFormSuKien.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtKhoanMucChi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nudSoTien.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dtpNgayChi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNguoiLapPhieu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboTrangThai.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDienGiai.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            pnlBudgetCards.Resize += (s, e) => LayoutBudgetCards();
            this.Resize += (s, e) =>
            {
                if (scContent != null && scContent.Width > 800)
                {
                    scContent.SplitterDistance = (int)(scContent.Width * 0.62);
                }
            };
            LayoutBudgetCards();

            LoadEventDropdowns();
            _isLoading = false;

            // Nạp dữ liệu ban đầu
            LoadExpensesAndBudgetSummary();
            SetFormMode(FormMode.View);
        }

        private void LayoutBudgetCards()
        {
            if (pnlBudgetCards == null) return;
            int pad = 12;
            int gap = 12;
            int totalWidth = pnlBudgetCards.ClientSize.Width - pad * 2;
            if (totalWidth <= 0) return;
            int cardWidth = (totalWidth - gap * 3) / 4;
            if (cardWidth < 180) cardWidth = 180;

            pnlCardBudget.Location = new Point(pad, 10);
            pnlCardBudget.Width = cardWidth;

            pnlCardSpent.Location = new Point(pad + cardWidth + gap, 10);
            pnlCardSpent.Width = cardWidth;

            pnlCardRemaining.Location = new Point(pad + (cardWidth + gap) * 2, 10);
            pnlCardRemaining.Width = cardWidth;

            pnlCardPercent.Location = new Point(pad + (cardWidth + gap) * 3, 10);
            pnlCardPercent.Width = Math.Max(cardWidth, totalWidth - (cardWidth + gap) * 3);
        }

        private void LoadEventDropdowns()
        {
            _events = _budgetService.GetAvailableEvents();

            cboFilterSuKien.Items.Clear();
            cboFilterSuKien.Items.Add("-- Tất cả sự kiện --");
            foreach (var ev in _events)
            {
                cboFilterSuKien.Items.Add($"{ev.MaSuKien} - {ev.TenSuKien}");
            }

            cboFormSuKien.Items.Clear();
            foreach (var ev in _events)
            {
                cboFormSuKien.Items.Add($"{ev.MaSuKien} - {ev.TenSuKien}");
            }

            if (cboFilterSuKien.Items.Count > 1)
            {
                cboFilterSuKien.SelectedIndex = 1; // Mặc định chọn EVT202601
            }
            else
            {
                cboFilterSuKien.SelectedIndex = 0;
            }

            if (cboFormSuKien.Items.Count > 0)
            {
                cboFormSuKien.SelectedIndex = 0;
            }

            cboTrangThai.SelectedIndex = 0;
        }

        private string? GetSelectedFilterEventId()
        {
            if (cboFilterSuKien.SelectedIndex <= 0) return null;
            var text = cboFilterSuKien.SelectedItem?.ToString() ?? "";
            var parts = text.Split(" - ");
            return parts.Length > 0 ? parts[0].Trim() : null;
        }

        private string GetSelectedFormEventId()
        {
            var text = cboFormSuKien.SelectedItem?.ToString() ?? "";
            var parts = text.Split(" - ");
            return parts.Length > 0 ? parts[0].Trim() : "";
        }

        private void SelectFormEventById(string maSuKien)
        {
            for (int i = 0; i < cboFormSuKien.Items.Count; i++)
            {
                if (cboFormSuKien.Items[i].ToString()?.StartsWith(maSuKien, StringComparison.OrdinalIgnoreCase) == true)
                {
                    cboFormSuKien.SelectedIndex = i;
                    return;
                }
            }
        }

        private void LoadExpensesAndBudgetSummary()
        {
            string? selectedEvent = GetSelectedFilterEventId();

            var expResult = _budgetService.GetExpensesByEvent(selectedEvent);
            if (expResult.IsSuccess && expResult.Data != null)
            {
                _currentList = expResult.Data;
            }
            else
            {
                _currentList = new List<ExpenseDto>();
            }

            ApplySearchFilter();
            UpdateBudgetDisplay(selectedEvent);
        }

        private void ApplySearchFilter()
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            var filtered = _currentList.Where(e =>
                string.IsNullOrEmpty(keyword) ||
                e.MaChiPhi.ToLower().Contains(keyword) ||
                e.KhoanMucChi.ToLower().Contains(keyword) ||
                (e.TenNguoiLapPhieu != null && e.TenNguoiLapPhieu.ToLower().Contains(keyword)) ||
                (e.DienGiai != null && e.DienGiai.ToLower().Contains(keyword))
            ).ToList();

            dgvChiPhi.DataSource = null;
            dgvChiPhi.DataSource = filtered;

            if (filtered.Count > 0 && _currentMode == FormMode.View)
            {
                BindRowToInputs(filtered[0]);
            }
            else if (filtered.Count == 0 && _currentMode == FormMode.View)
            {
                ClearInputs();
            }
        }

        private void UpdateBudgetDisplay(string? maSuKien, decimal additionalAmount = 0)
        {
            if (string.IsNullOrWhiteSpace(maSuKien))
            {
                // Khi xem tất cả sự kiện
                decimal totalBudget = _events.Sum(e => e.NganSachDuKien);
                decimal totalSpent = _currentList.Sum(e => e.SoTien) + additionalAmount;
                decimal remaining = totalBudget - totalSpent;
                double ratio = totalBudget > 0 ? (double)(totalSpent / totalBudget) * 100 : 0;

                lblNganSachDuKien.Text = $"{totalBudget:N0} VNĐ";
                lblTongThucChi.Text = $"{totalSpent:N0} VNĐ";
                lblNganSachConLai.Text = $"{remaining:N0} VNĐ";
                lblNganSachConLai.ForeColor = remaining >= 0 ? ColorSuccess : ColorDanger;
                lblTyLeSuDung.Text = $"{ratio:F1} %";

                ApplyAlertBanner(ratio, remaining < 0);
                return;
            }

            var statusResult = _budgetService.GetBudgetStatus(maSuKien, additionalAmount);
            if (statusResult.IsSuccess && statusResult.Data != null)
            {
                var s = statusResult.Data;
                lblNganSachDuKien.Text = $"{s.NganSachDuKien:N0} VNĐ";
                lblTongThucChi.Text = $"{s.TongChiSauKhiThem:N0} VNĐ";
                decimal rem = s.NganSachDuKien - s.TongChiSauKhiThem;
                lblNganSachConLai.Text = $"{rem:N0} VNĐ";
                lblNganSachConLai.ForeColor = rem >= 0 ? ColorSuccess : ColorDanger;
                lblTyLeSuDung.Text = $"{s.TyLeSuDung:F1} %";

                ApplyAlertBanner(s.TyLeSuDung, s.IsOverBudget);
            }
        }

        private void ApplyAlertBanner(double ratio, bool isOverBudget)
        {
            if (isOverBudget)
            {
                pnlAlertBanner.BackColor = Color.FromArgb(254, 226, 226); // Đỏ nhạt
                lblAlertIcon.Text = "🚨";
                lblAlertStatus.ForeColor = ColorDanger;
                lblAlertStatus.Text = $"CẢNH BÁO BỘI CHI: Chi phí đã vượt quá 100% ngân sách được duyệt ({ratio:F1}%).";
            }
            else if (ratio >= 80.0)
            {
                pnlAlertBanner.BackColor = Color.FromArgb(254, 243, 199); // Vàng cam nhạt
                lblAlertIcon.Text = "⚠️";
                lblAlertStatus.ForeColor = ColorWarning;
                lblAlertStatus.Text = $"CẢNH BÁO TIỆM CẬN: Chi phí đã chạm ngưỡng {ratio:F1}% ngân sách dự kiến (≥ 80%).";
            }
            else
            {
                pnlAlertBanner.BackColor = Color.FromArgb(220, 252, 231); // Xanh lá nhạt
                lblAlertIcon.Text = "✅";
                lblAlertStatus.ForeColor = Color.FromArgb(22, 101, 52);
                lblAlertStatus.Text = $"Ngân sách trong ngưỡng an toàn ({ratio:F1}%).";
            }
        }

        private void BindRowToInputs(ExpenseDto dto)
        {
            txtMaChiPhi.Text = dto.MaChiPhi;
            SelectFormEventById(dto.MaSuKien);
            txtKhoanMucChi.Text = dto.KhoanMucChi;
            nudSoTien.Value = Math.Min(nudSoTien.Maximum, Math.Max(0, dto.SoTien));
            dtpNgayChi.Value = dto.NgayChi != default ? dto.NgayChi : DateTime.Today;
            txtNguoiLapPhieu.Text = !string.IsNullOrWhiteSpace(dto.TenNguoiLapPhieu) ? dto.TenNguoiLapPhieu : dto.NguoiLapPhieu;
            cboTrangThai.SelectedItem = dto.TrangThai ?? "Đã duyệt";
            txtDienGiai.Text = dto.DienGiai ?? "";
        }

        private void ClearInputs()
        {
            txtMaChiPhi.Text = _budgetService.GetNextExpenseCode();
            txtKhoanMucChi.Text = "";
            nudSoTien.Value = 0;
            dtpNgayChi.Value = DateTime.Today;
            txtNguoiLapPhieu.Text = "Quản trị viên Hệ thống";
            cboTrangThai.SelectedIndex = 0;
            txtDienGiai.Text = "";
            errorProvider.Clear();
        }

        private void SetFormMode(FormMode mode)
        {
            _currentMode = mode;

            bool isEditing = mode == FormMode.Add || mode == FormMode.Edit;

            cboFormSuKien.Enabled = isEditing;
            txtKhoanMucChi.ReadOnly = !isEditing;
            nudSoTien.Enabled = isEditing;
            dtpNgayChi.Enabled = isEditing;
            txtNguoiLapPhieu.ReadOnly = !isEditing;
            cboTrangThai.Enabled = isEditing;
            txtDienGiai.ReadOnly = !isEditing;

            btnThem.Visible = !isEditing;
            btnSua.Visible = !isEditing;
            btnXoa.Visible = !isEditing;
            btnXuatExcel.Visible = !isEditing;
            btnThem.Enabled = !isEditing;
            btnSua.Enabled = !isEditing && dgvChiPhi.CurrentRow != null;
            btnXoa.Enabled = !isEditing && dgvChiPhi.CurrentRow != null;

            btnLuu.Visible = isEditing;
            btnHuy.Visible = isEditing;
            btnLuu.Enabled = isEditing;
            btnHuy.Enabled = isEditing;

            if (isEditing)
            {
                btnLuu.Location = new Point(8, 12);
                btnHuy.Location = new Point(138, 12);
            }
            else
            {
                btnThem.Location = new Point(8, 12);
                btnSua.Location = new Point(138, 12);
                btnXoa.Location = new Point(268, 12);
                btnXuatExcel.Location = new Point(8, 56);
            }

            dgvChiPhi.Enabled = !isEditing;
            cboFilterSuKien.Enabled = !isEditing;
            txtSearch.Enabled = !isEditing;

            if (mode == FormMode.Add)
            {
                ClearInputs();
                txtKhoanMucChi.Focus();
            }
            else if (mode == FormMode.Edit)
            {
                txtKhoanMucChi.Focus();
            }
        }

        #region Event Handlers

        private void cboFilterSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            LoadExpensesAndBudgetSummary();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            ApplySearchFilter();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadExpensesAndBudgetSummary();
            SetFormMode(FormMode.View);
        }

        private void dgvChiPhi_SelectionChanged(object sender, EventArgs e)
        {
            if (_isLoading || _currentMode != FormMode.View) return;

            if (dgvChiPhi.CurrentRow?.DataBoundItem is ExpenseDto dto)
            {
                BindRowToInputs(dto);
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
            else
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
        }

        private void nudSoTien_ValueChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.Add)
            {
                string eventId = GetSelectedFormEventId();
                if (!string.IsNullOrWhiteSpace(eventId))
                {
                    UpdateBudgetDisplay(eventId, nudSoTien.Value);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SetFormMode(FormMode.Add);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvChiPhi.CurrentRow?.DataBoundItem is ExpenseDto)
            {
                SetFormMode(FormMode.Edit);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            SetFormMode(FormMode.View);
            if (dgvChiPhi.CurrentRow?.DataBoundItem is ExpenseDto dto)
            {
                BindRowToInputs(dto);
            }
            UpdateBudgetDisplay(GetSelectedFilterEventId());
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvChiPhi.CurrentRow?.DataBoundItem is not ExpenseDto dto) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phiếu chi [{dto.MaChiPhi} - {dto.KhoanMucChi}] với số tiền {dto.SoTien:N0} VNĐ không?",
                "Xác Nhận Xóa Phiếu Chi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var delRes = _budgetService.DeleteExpense(dto.MaChiPhi);
                if (delRes.IsSuccess)
                {
                    MessageBox.Show(delRes.Message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadExpensesAndBudgetSummary();
                    SetFormMode(FormMode.View);
                }
                else
                {
                    MessageBox.Show(delRes.Message, "Lỗi Xóa Phiếu Chi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            // 1. Thẩm định dữ liệu đầu vào
            string khoanMuc = txtKhoanMucChi.Text.Trim();
            if (string.IsNullOrWhiteSpace(khoanMuc))
            {
                errorProvider.SetError(txtKhoanMucChi, "Tên khoản mục chi không được để trống.");
                txtKhoanMucChi.Focus();
                return;
            }

            decimal soTien = nudSoTien.Value;
            if (soTien <= 0)
            {
                errorProvider.SetError(nudSoTien, "Số tiền chi phí phải lớn hơn 0 VNĐ.");
                nudSoTien.Focus();
                return;
            }

            string maSuKien = GetSelectedFormEventId();
            if (string.IsNullOrWhiteSpace(maSuKien))
            {
                errorProvider.SetError(cboFormSuKien, "Vui lòng chọn sự kiện áp dụng cho khoản chi.");
                cboFormSuKien.Focus();
                return;
            }

            var dto = new ExpenseDto
            {
                MaChiPhi = txtMaChiPhi.Text.Trim(),
                MaSuKien = maSuKien,
                KhoanMucChi = khoanMuc,
                SoTien = soTien,
                NgayChi = dtpNgayChi.Value,
                NguoiLapPhieu = "USR001",
                TenNguoiLapPhieu = string.IsNullOrWhiteSpace(txtNguoiLapPhieu.Text) ? "Quản trị viên Hệ thống" : txtNguoiLapPhieu.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Đã duyệt",
                DienGiai = txtDienGiai.Text.Trim()
            };

            ServiceResult<BudgetAlertDto> saveRes;
            if (_currentMode == FormMode.Add)
            {
                saveRes = _budgetService.AddExpense(dto);
            }
            else
            {
                saveRes = _budgetService.UpdateExpense(dto);
            }

            if (saveRes.IsSuccess)
            {
                var alert = saveRes.Data;
                string notice = saveRes.Message;

                if (alert != null && alert.IsOverBudget)
                {
                    MessageBox.Show(
                        $"{notice}\n\n🚨 CẢNH BÁO BỘI CHI: Tổng chi phí đã vượt quá ngân sách dự kiến ({alert.TyLeSuDung:F1}%).",
                        "Cảnh Báo Bội Chi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else if (alert != null && alert.IsWarning80)
                {
                    MessageBox.Show(
                        $"{notice}\n\n⚠️ CẢNH BÁO: Khoản chi này làm tổng chi đạt {alert.TyLeSuDung:F1}% ngân sách dự kiến (đạt ngưỡng 80%).",
                        "Cảnh Báo Ngân Sách",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(notice, "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadExpensesAndBudgetSummary();
                SetFormMode(FormMode.View);
            }
            else
            {
                string errMsg = saveRes.Message;
                if (saveRes.Errors != null && saveRes.Errors.Count > 0)
                {
                    errMsg += "\n- " + string.Join("\n- ", saveRes.Errors);
                }
                MessageBox.Show(errMsg, "Lỗi Nghiệp Vụ Ngân Sách", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using var sfd = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv",
                    FileName = $"BaoCaoChiPhi_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                    Title = "Xuất Báo Cáo Chi Phí Sự Kiện"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Mã Phiếu,Mã Sự Kiện,Khoản Mục Chi,Số Tiền,Ngày Chi,Người Lập,Trạng Thái,Diễn Giải");
                    foreach (var item in _currentList)
                    {
                        sb.AppendLine($"\"{item.MaChiPhi}\",\"{item.MaSuKien}\",\"{item.KhoanMucChi}\",{item.SoTien},\"{item.NgayChi:dd/MM/yyyy}\",\"{item.TenNguoiLapPhieu}\",\"{item.TrangThai}\",\"{item.DienGiai}\"");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show($"Xuất báo cáo chi phí thành công ra tệp:\n{sfd.FileName}", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.N))
            {
                if (btnThem.Enabled) btnThem.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                if (btnLuu.Enabled) btnLuu.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.D))
            {
                if (btnXoa.Enabled) btnXoa.PerformClick();
                return true;
            }
            if (keyData == Keys.Escape)
            {
                if (btnHuy.Enabled) btnHuy.PerformClick();
                else this.Close();
                return true;
            }
            if (keyData == Keys.F5)
            {
                btnLamMoi.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}
