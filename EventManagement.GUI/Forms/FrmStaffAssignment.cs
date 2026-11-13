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
    /// Màn hình F15 - Phân công nhân sự theo ca trực sự kiện
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-13/Day 6)
    /// Tuân thủ: Mẫu bố cục 2 (Archetype 2), 3 trạng thái màn hình (Xem - Thêm - Sửa), quy tắc BR-13 (Chống trùng ca trực).
    /// </summary>
    public partial class FrmStaffAssignment : FrmBase
    {
        private readonly IStaffAssignmentService _assignmentService;
        private bool _isLoading = true;
        private FormMode _currentMode = FormMode.View;
        private List<StaffAssignmentDto> _currentList = new();
        private List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> _eventList = new();
        private List<StaffDto> _staffList = new();

        public Button BtnThem => btnThem;
        public Button BtnSua => btnSua;
        public Button BtnXoa => btnXoa;
        public Button BtnLuu => btnLuu;
        public Button BtnHuy => btnHuy;
        public Button BtnLamMoi => btnLamMoi;
        public Button BtnKiemTraXungDot => btnKiemTraXungDot;
        public Label LblKiemTraStatus => lblKiemTraStatus;
        public DataGridView DgvPhanCong => dgvPhanCong;
        public GroupBox GrbThongTin => grbThongTin;
        public Panel PnlFilter => pnlFilter;

        public FrmStaffAssignment() : this(new StaffAssignmentService())
        {
        }

        public FrmStaffAssignment(IStaffAssignmentService assignmentService)
        {
            InitializeComponent();
            _assignmentService = assignmentService ?? new StaffAssignmentService();

            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            btnKiemTraXungDot.Visible = false;

            ApplyCustomStyles();

            // Lắng nghe sự kiện để tự động kiểm tra trùng ca trực
            dtpBatDau.ValueChanged += (s, e) => AutoCheckConflict();
            dtpKetThuc.ValueChanged += (s, e) => AutoCheckConflict();
            cboNhanSu.SelectedIndexChanged += (s, e) => AutoCheckConflict();
            cboSuKien.SelectedIndexChanged += (s, e) => AutoCheckConflict();

            // Co giãn giao diện responsive
            grbThongTin.Resize += (s, e) => AdjustInputLayout();
            pnlButtons.Resize += (s, e) => AdjustButtonLayout();
            pnlFilter.Resize += (s, e) => AdjustFilterLayout();
            this.Resize += (s, e) => { AdjustInputLayout(); AdjustButtonLayout(); AdjustFilterLayout(); };
        }

        private void ApplyCustomStyles()
        {
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            btnKiemTraXungDot.Visible = false;

            StyleButton(btnThem, ButtonRole.Success);
            StyleButton(btnSua, ButtonRole.Primary);
            StyleButton(btnXoa, ButtonRole.Danger);
            StyleButton(btnXuatExcel, ButtonRole.Secondary);
            StyleButton(btnLuu, ButtonRole.Primary);
            StyleButton(btnHuy, ButtonRole.Secondary);
            StyleButton(btnLamMoi, ButtonRole.Secondary);

            StyleDataGridView(dgvPhanCong);
            dgvPhanCong.AutoGenerateColumns = false;

            // Đổi tên cột và tinh chỉnh độ rộng hiển thị theo yêu cầu
            colTenSuKien.HeaderText = "Sự kiện";
            colTenNhanSu.HeaderText = "Họ và tên";

            colMaPhanCong.FillWeight = 50;
            colTenNhanSu.FillWeight = 90;
            colPhongBan.FillWeight = 75;
            colChucVuNhanSu.FillWeight = 75;
            colSoDienThoai.FillWeight = 65;
            colNgayTruc.FillWeight = 60;
            colKhungGio.FillWeight = 75;
            colTenSuKien.FillWeight = 160;
            colNhiemVu.FillWeight = 150;
            colGhiChu.FillWeight = 130;

            colMaPhanCong.MinimumWidth = 65;
            colTenNhanSu.MinimumWidth = 120;
            colPhongBan.MinimumWidth = 135;
            colChucVuNhanSu.MinimumWidth = 135;
            colSoDienThoai.MinimumWidth = 105;
            colNgayTruc.MinimumWidth = 95;
            colKhungGio.MinimumWidth = 135;
            colTenSuKien.MinimumWidth = 240;
            colNhiemVu.MinimumWidth = 190;
            colGhiChu.MinimumWidth = 180;

            dgvPhanCong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhanCong.ScrollBars = ScrollBars.Both;

            foreach (DataGridViewColumn col in dgvPhanCong.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            LoadAuxiliaryData();
            AdjustInputLayout();
            AdjustButtonLayout();
            AdjustFilterLayout();
        }

        /// <summary>
        /// Tự động dàn đều 2 cột thông tin chi tiết trên toàn màn hình (Responsive 1920x1080)
        /// </summary>
        private void AdjustInputLayout()
        {
            if (grbThongTin == null) return;
            int clientW = grbThongTin.ClientSize.Width;
            int margin = 20;
            int gap = 30;
            int colWidth = (clientW - (margin * 2) - gap) / 2;
            if (colWidth < 250) colWidth = 250;

            int col1X = margin;
            int col2X = margin + colWidth + gap;
            int labelW = 145;
            int inputW = colWidth - labelW - 10;

            // Dòng 1: Mã ca | Sự kiện
            lblMaPhanCong.Location = new Point(col1X, 31);
            txtMaPhanCong.Location = new Point(col1X + labelW, 28);
            txtMaPhanCong.Size = new Size(inputW, 23);

            lblSuKien.Location = new Point(col2X, 31);
            cboSuKien.Location = new Point(col2X + labelW, 28);
            cboSuKien.Size = new Size(inputW, 23);

            // Dòng 2: Nhân sự | Ghi chú
            lblNhanSu.Location = new Point(col1X, 65);
            cboNhanSu.Location = new Point(col1X + labelW, 62);
            cboNhanSu.Size = new Size(inputW, 23);

            lblGhiChu.Location = new Point(col2X, 65);
            txtGhiChu.Location = new Point(col2X + labelW, 62);
            txtGhiChu.Size = new Size(inputW, 23);

            // Dòng 3: Bắt đầu | Kết thúc
            lblBatDau.Location = new Point(col1X, 99);
            dtpBatDau.Location = new Point(col1X + labelW, 96);
            dtpBatDau.Size = new Size(inputW, 23);

            lblKetThuc.Location = new Point(col2X, 99);
            dtpKetThuc.Location = new Point(col2X + labelW, 96);
            dtpKetThuc.Size = new Size(inputW, 23);

            // Dòng 4: Nhiệm vụ
            lblNhiemVu.Location = new Point(col1X, 133);
            txtNhiemVu.Location = new Point(col1X + labelW, 130);
            txtNhiemVu.Size = new Size(clientW - (margin * 2) - labelW, 23);

            // Dòng 5: Nhãn trạng thái kiểm tra tự động góc trái dưới
            lblKiemTraStatus.Location = new Point(col1X, 168);
            lblKiemTraStatus.Size = new Size(clientW - (margin * 2), 24);
        }

        private void AdjustButtonLayout()
        {
            if (pnlButtons == null) return;
            int rightEdge = pnlButtons.ClientSize.Width - 15;
            btnHuy.Location = new Point(rightEdge - btnHuy.Width, 10);
            btnLuu.Location = new Point(btnHuy.Left - btnLuu.Width - 10, 10);
        }

        private void AdjustFilterLayout()
        {
            if (pnlFilter == null) return;
            btnLamMoi.Location = new Point(pnlFilter.ClientSize.Width - btnLamMoi.Width - 15, 8);
            int searchWidth = btnLamMoi.Left - 20 - txtTimKiem.Left;
            if (searchWidth > 150)
            {
                txtTimKiem.Width = searchWidth;
            }
        }

        /// <summary>
        /// Tự động kiểm tra xung đột trùng ca trực của nhân sự
        /// </summary>
        private void AutoCheckConflict()
        {
            if (_isLoading) return;
            if (_currentMode == FormMode.View)
            {
                lblKiemTraStatus.Text = "✅ Ca trực hợp lệ, không có xung đột";
                lblKiemTraStatus.ForeColor = ColorSuccess;
                return;
            }

            var eventItem = cboSuKien.SelectedItem as ComboBoxItem;
            var staffItem = cboNhanSu.SelectedItem as ComboBoxItem;
            if (eventItem == null || string.IsNullOrWhiteSpace(eventItem.Value) ||
                staffItem == null || string.IsNullOrWhiteSpace(staffItem.Value))
            {
                lblKiemTraStatus.Text = "ℹ️ Vui lòng chọn sự kiện và nhân sự để kiểm tra ca trực.";
                lblKiemTraStatus.ForeColor = ColorTextMuted;
                return;
            }

            if (StaffAssignmentService.IsEventEnded(eventItem.Value))
            {
                lblKiemTraStatus.Text = "❌ Không thể phân công nhân sự vào sự kiện đã kết thúc!";
                lblKiemTraStatus.ForeColor = ColorDanger;
                return;
            }

            if (dtpKetThuc.Value <= dtpBatDau.Value)
            {
                lblKiemTraStatus.Text = "❌ Thời gian kết thúc phải lớn hơn thời gian bắt đầu!";
                lblKiemTraStatus.ForeColor = ColorDanger;
                return;
            }

            var tempDto = new StaffAssignmentDto
            {
                MaPhanCong = txtMaPhanCong.Text.Trim(),
                MaSuKien = eventItem.Value,
                TenSuKien = eventItem.Text,
                MaNhanSu = staffItem.Value,
                NhiemVu = txtNhiemVu.Text.Trim(),
                ThoiGianBatDau = dtpBatDau.Value,
                ThoiGianKetThuc = dtpKetThuc.Value,
                GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim()
            };

            string? excludeId = _currentMode == FormMode.Edit ? tempDto.MaPhanCong : null;
            var checkRes = _assignmentService.CheckConflict(tempDto, excludeId);
            if (checkRes.IsSuccess)
            {
                lblKiemTraStatus.Text = "✅ Ca trực hợp lệ, không có xung đột";
                lblKiemTraStatus.ForeColor = ColorSuccess;
            }
            else
            {
                lblKiemTraStatus.Text = $"❌ {StripBrTags(checkRes.Message)}";
                lblKiemTraStatus.ForeColor = ColorDanger;
            }
        }

        private void LoadAuxiliaryData()
        {
            // 1. Nạp danh mục Sự kiện
            _eventList = _assignmentService.GetAvailableEvents();

            cboLocSuKien.Items.Clear();
            cboLocSuKien.Items.Add("Tất cả sự kiện");
            foreach (var ev in _eventList)
            {
                cboLocSuKien.Items.Add($"{ev.MaSuKien} - {ev.TenSuKien}");
            }
            cboLocSuKien.SelectedIndex = 0;

            cboSuKien.Items.Clear();
            foreach (var ev in _eventList)
            {
                cboSuKien.Items.Add(new ComboBoxItem(ev.MaSuKien, $"{ev.MaSuKien} - {ev.TenSuKien}"));
            }
            if (cboSuKien.Items.Count > 0) cboSuKien.SelectedIndex = 0;

            // 2. Nạp danh mục Nhân sự từ F14
            _staffList = _assignmentService.GetAvailableStaff();

            cboNhanSu.Items.Clear();
            foreach (var st in _staffList)
            {
                string statusSuffix = st.TrangThai == "Hoạt động" ? "" : $" [{st.TrangThai}]";
                cboNhanSu.Items.Add(new ComboBoxItem(st.MaNhanSu, $"{st.MaNhanSu} - {st.HoTen} ({st.PhongBan} / {st.ChucDanh}){statusSuffix}"));
            }
            if (cboNhanSu.Items.Count > 0) cboNhanSu.SelectedIndex = 0;

            // 3. Nạp danh mục Phòng ban cho bộ lọc
            cboLocPhongBan.Items.Clear();
            cboLocPhongBan.Items.Add("Tất cả phòng ban");
            var depts = _staffList.Select(x => x.PhongBan).Distinct().OrderBy(x => x);
            foreach (var dp in depts)
            {
                if (!string.IsNullOrWhiteSpace(dp))
                {
                    cboLocPhongBan.Items.Add(dp);
                }
            }
            cboLocPhongBan.SelectedIndex = 0;
        }

        private void FrmStaffAssignment_Load(object sender, EventArgs e)
        {
            _isLoading = false;
            LoadData();
            SetFormState(FormMode.View);
        }

        private void LoadData()
        {
            string? selectedEventId = GetSelectedFilterEventId();
            string? selectedDept = cboLocPhongBan.SelectedIndex > 0 ? cboLocPhongBan.SelectedItem?.ToString() : null;
            string? keyword = txtTimKiem.Text.Trim();

            var result = _assignmentService.GetAll(selectedEventId, keyword, selectedDept);
            if (result.IsSuccess && result.Data != null)
            {
                _currentList = result.Data;
                dgvPhanCong.DataSource = null;
                dgvPhanCong.DataSource = _currentList;

                if (_currentList.Count > 0)
                {
                    dgvPhanCong.Rows[0].Selected = true;
                    DisplaySelectedRow(_currentList[0]);
                }
                else
                {
                    ClearInputs();
                }
            }
            else
            {
                MessageBox.Show(StripBrTags(result.Message), "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string? GetSelectedFilterEventId()
        {
            if (cboLocSuKien.SelectedIndex <= 0) return null;
            string text = cboLocSuKien.SelectedItem?.ToString() ?? string.Empty;
            int dashIdx = text.IndexOf('-');
            return dashIdx > 0 ? text.Substring(0, dashIdx).Trim() : text;
        }

        private void DisplaySelectedRow(StaffAssignmentDto dto)
        {
            if (_currentMode != FormMode.View || dto == null) return;

            txtMaPhanCong.Text = dto.MaPhanCong;
            txtNhiemVu.Text = dto.NhiemVu;
            txtGhiChu.Text = dto.GhiChu ?? string.Empty;

            // Đồng bộ combobox Sự kiện
            for (int i = 0; i < cboSuKien.Items.Count; i++)
            {
                if (cboSuKien.Items[i] is ComboBoxItem item &&
                    item.Value.Equals(dto.MaSuKien, StringComparison.OrdinalIgnoreCase))
                {
                    cboSuKien.SelectedIndex = i;
                    break;
                }
            }

            // Đồng bộ combobox Nhân sự
            for (int i = 0; i < cboNhanSu.Items.Count; i++)
            {
                if (cboNhanSu.Items[i] is ComboBoxItem item &&
                    item.Value.Equals(dto.MaNhanSu, StringComparison.OrdinalIgnoreCase))
                {
                    cboNhanSu.SelectedIndex = i;
                    break;
                }
            }

            dtpBatDau.Value = dto.ThoiGianBatDau >= dtpBatDau.MinDate && dto.ThoiGianBatDau <= dtpBatDau.MaxDate
                ? dto.ThoiGianBatDau
                : DateTime.Now;

            dtpKetThuc.Value = dto.ThoiGianKetThuc >= dtpKetThuc.MinDate && dto.ThoiGianKetThuc <= dtpKetThuc.MaxDate
                ? dto.ThoiGianKetThuc
                : DateTime.Now.AddHours(4);

            lblKiemTraStatus.Text = "✅ Ca trực hợp lệ, không có xung đột";
            lblKiemTraStatus.ForeColor = ColorSuccess;
            errBaoLoi.Clear();
        }

        private void ClearInputs()
        {
            txtMaPhanCong.Clear();
            txtNhiemVu.Clear();
            txtGhiChu.Clear();
            if (cboSuKien.Items.Count > 0) cboSuKien.SelectedIndex = 0;
            if (cboNhanSu.Items.Count > 0) cboNhanSu.SelectedIndex = 0;

            var currentEvent = GetCurrentSelectedEvent();
            if (currentEvent.HasValue)
            {
                dtpBatDau.Value = currentEvent.Value.BatDau;
                dtpKetThuc.Value = currentEvent.Value.BatDau.AddHours(4);
            }
            else
            {
                dtpBatDau.Value = DateTime.Now;
                dtpKetThuc.Value = DateTime.Now.AddHours(4);
            }

            lblKiemTraStatus.Text = "✅ Ca trực hợp lệ, không có xung đột";
            lblKiemTraStatus.ForeColor = ColorSuccess;
            errBaoLoi.Clear();
        }

        private (string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)? GetCurrentSelectedEvent()
        {
            if (cboSuKien.SelectedItem is ComboBoxItem item)
            {
                var ev = _eventList.FirstOrDefault(e => e.MaSuKien.Equals(item.Value, StringComparison.OrdinalIgnoreCase));
                if (ev != default) return ev;
            }
            return null;
        }

        /// <summary>
        /// Quản lý 3 trạng thái màn hình (Xem - Thêm - Sửa) theo chuẩn Archetype 2 docs/08
        /// Nút Lưu và Hủy chỉ hiển thị khi Thêm hoặc Sửa
        /// </summary>
        private void SetFormState(FormMode mode)
        {
            _currentMode = mode;
            errBaoLoi.Clear();

            switch (mode)
            {
                case FormMode.View:
                    txtMaPhanCong.ReadOnly = true;
                    cboSuKien.Enabled = false;
                    cboNhanSu.Enabled = false;
                    txtNhiemVu.ReadOnly = true;
                    dtpBatDau.Enabled = false;
                    dtpKetThuc.Enabled = false;
                    txtGhiChu.ReadOnly = true;
                    btnKiemTraXungDot.Visible = false;

                    btnThem.Enabled = true;
                    bool hasRows = dgvPhanCong.CurrentRow != null;
                    btnSua.Enabled = hasRows;
                    btnXoa.Enabled = hasRows;
                    btnLuu.Visible = false;
                    btnHuy.Visible = false;

                    dgvPhanCong.Enabled = true;
                    pnlFilter.Enabled = true;

                    if (dgvPhanCong.CurrentRow != null && dgvPhanCong.CurrentRow.DataBoundItem is StaffAssignmentDto selected)
                    {
                        DisplaySelectedRow(selected);
                    }
                    break;

                case FormMode.Add:
                    txtMaPhanCong.ReadOnly = false;
                    cboSuKien.Enabled = true;
                    cboNhanSu.Enabled = true;
                    txtNhiemVu.ReadOnly = false;
                    dtpBatDau.Enabled = true;
                    dtpKetThuc.Enabled = true;
                    txtGhiChu.ReadOnly = false;
                    btnKiemTraXungDot.Visible = false;

                    ClearInputs();
                    txtMaPhanCong.Text = _assignmentService.SuggestNextCode();

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Visible = true;
                    btnHuy.Visible = true;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvPhanCong.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtNhiemVu.Focus();
                    AutoCheckConflict();
                    break;

                case FormMode.Edit:
                    txtMaPhanCong.ReadOnly = true; // Khóa cứng khóa chính
                    cboSuKien.Enabled = true;
                    cboNhanSu.Enabled = true;
                    txtNhiemVu.ReadOnly = false;
                    dtpBatDau.Enabled = true;
                    dtpKetThuc.Enabled = true;
                    txtGhiChu.ReadOnly = false;
                    btnKiemTraXungDot.Visible = false;

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Visible = true;
                    btnHuy.Visible = true;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvPhanCong.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtNhiemVu.Focus();
                    AutoCheckConflict();
                    break;
            }
        }

        private void dgvPhanCong_SelectionChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View &&
                dgvPhanCong.CurrentRow != null &&
                dgvPhanCong.CurrentRow.DataBoundItem is StaffAssignmentDto selected)
            {
                DisplaySelectedRow(selected);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.Add);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvPhanCong.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một ca trực nhân sự cần chỉnh sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SetFormState(FormMode.Edit);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvPhanCong.CurrentRow == null || !(dgvPhanCong.CurrentRow.DataBoundItem is StaffAssignmentDto selected))
            {
                MessageBox.Show("Vui lòng chọn ca trực nhân sự cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa ca trực [{selected.MaPhanCong}] của nhân sự '{selected.TenNhanSu}' (Nhiệm vụ: {selected.NhiemVu})?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                var result = _assignmentService.Delete(selected.MaPhanCong);
                if (result.IsSuccess)
                {
                    MessageBox.Show(StripBrTags(result.Message), "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    SetFormState(FormMode.View);
                }
                else
                {
                    MessageBox.Show(StripBrTags(result.Message), "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out var dto)) return;

            ServiceResult result;
            if (_currentMode == FormMode.Add)
            {
                result = _assignmentService.Add(dto);
            }
            else
            {
                result = _assignmentService.Update(dto);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show(StripBrTags(result.Message), "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);

                // Chọn dòng vừa thêm/sửa
                for (int i = 0; i < dgvPhanCong.Rows.Count; i++)
                {
                    if (dgvPhanCong.Rows[i].DataBoundItem is StaffAssignmentDto row &&
                        row.MaPhanCong.Equals(dto.MaPhanCong, StringComparison.OrdinalIgnoreCase))
                    {
                        dgvPhanCong.Rows[i].Selected = true;
                        DisplaySelectedRow(row);
                        break;
                    }
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(StripBrTags(result.Message));
                if (result.Errors != null && result.Errors.Count > 0)
                {
                    sb.AppendLine("\nChi tiết cảnh báo:");
                    foreach (var err in result.Errors)
                    {
                        sb.AppendLine($"  • {StripBrTags(err)}");
                    }
                }
                MessageBox.Show(sb.ToString(), "Cảnh báo trùng ca trực", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.View);
        }

        private void btnKiemTraXungDot_Click(object sender, EventArgs e)
        {
            AutoCheckConflict();
        }

        private bool ValidateInputs(out StaffAssignmentDto dto)
        {
            dto = new StaffAssignmentDto();
            errBaoLoi.Clear();
            bool isValid = true;

            string maPhanCong = txtMaPhanCong.Text.Trim();
            if (string.IsNullOrWhiteSpace(maPhanCong))
            {
                errBaoLoi.SetError(txtMaPhanCong, "Mã phân công không được để trống.");
                isValid = false;
            }

            var eventItem = cboSuKien.SelectedItem as ComboBoxItem;
            if (eventItem == null || string.IsNullOrWhiteSpace(eventItem.Value))
            {
                errBaoLoi.SetError(cboSuKien, "Vui lòng chọn sự kiện áp dụng.");
                isValid = false;
            }
            else if (StaffAssignmentService.IsEventEnded(eventItem.Value))
            {
                errBaoLoi.SetError(cboSuKien, "Không thể phân công nhân sự vào sự kiện đã kết thúc!");
                MessageBox.Show("Không thể phân công nhân sự vào sự kiện đã kết thúc!", "Lỗi phân công", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var staffItem = cboNhanSu.SelectedItem as ComboBoxItem;
            if (staffItem == null || string.IsNullOrWhiteSpace(staffItem.Value))
            {
                errBaoLoi.SetError(cboNhanSu, "Vui lòng chọn nhân sự được phân công.");
                isValid = false;
            }

            string nhiemVu = txtNhiemVu.Text.Trim();
            if (string.IsNullOrWhiteSpace(nhiemVu))
            {
                errBaoLoi.SetError(txtNhiemVu, "Nhiệm vụ phân công không được để trống.");
                isValid = false;
            }

            DateTime batDau = dtpBatDau.Value;
            DateTime ketThuc = dtpKetThuc.Value;
            if (ketThuc <= batDau)
            {
                errBaoLoi.SetError(dtpKetThuc, "Thời gian kết thúc ca trực phải lớn hơn thời gian bắt đầu.");
                isValid = false;
            }

            if (!isValid) return false;

            dto = new StaffAssignmentDto
            {
                MaPhanCong = maPhanCong,
                MaSuKien = eventItem!.Value,
                TenSuKien = eventItem!.Text,
                MaNhanSu = staffItem!.Value,
                NhiemVu = nhiemVu,
                ThoiGianBatDau = batDau,
                ThoiGianKetThuc = ketThuc,
                GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim()
            };

            return true;
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (_currentList == null || _currentList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu phân công để xuất báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Tệp CSV (*.csv)|*.csv",
                FileName = $"PhanCong_NhanSu_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Mã ca,Mã sự kiện,Sự kiện,Mã nhân sự,Họ tên,Phòng ban,Chức vụ,Số ĐT,Nhiệm vụ,Ngày trực,Khung giờ,Ghi chú");

                    foreach (var item in _currentList)
                    {
                        sb.AppendLine($"\"{EscapeCsv(item.MaPhanCong)}\",\"{EscapeCsv(item.MaSuKien)}\",\"{EscapeCsv(item.TenSuKien)}\",\"{EscapeCsv(item.MaNhanSu)}\",\"{EscapeCsv(item.TenNhanSu)}\",\"{EscapeCsv(item.PhongBan)}\",\"{EscapeCsv(item.ChucVuNhanSu)}\",\"{EscapeCsv(item.SoDienThoai)}\",\"{EscapeCsv(item.NhiemVu)}\",\"{item.NgayTruc}\",\"{item.KhungGio}\",\"{EscapeCsv(item.GhiChu)}\"");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show($"Xuất dữ liệu phân công nhân sự thành công!\nĐường dẫn: {sfd.FileName}", "Xuất Excel thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboLocSuKien.SelectedIndex = 0;
            cboLocPhongBan.SelectedIndex = 0;
            LoadData();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            LoadData();
        }

        private void cboLocSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            LoadData();
        }

        private void cboLocPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            LoadData();
        }

        private void cboSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading || _currentMode != FormMode.Add) return;

            var currentEvent = GetCurrentSelectedEvent();
            if (currentEvent.HasValue)
            {
                dtpBatDau.Value = currentEvent.Value.BatDau;
                dtpKetThuc.Value = currentEvent.Value.BatDau.AddHours(4);
            }
        }

        private void FrmStaffAssignment_KeyDown(object sender, KeyEventArgs e)
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
