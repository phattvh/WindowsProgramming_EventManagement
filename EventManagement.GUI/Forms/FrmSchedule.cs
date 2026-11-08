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
    /// Màn hình F08 – Quản lý lịch trình / chương trình tiết mục sự kiện
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-11)
    /// Tuân thủ: Mẫu bố cục 2 (Archetype 2), 3 trạng thái màn hình (Xem - Thêm - Sửa), quy tắc BR-05 & BR-06.
    /// </summary>
    public partial class FrmSchedule : FrmBase
    {
        private readonly IScheduleService _scheduleService;
        private bool _isLoading = true;
        private FormMode _currentMode = FormMode.View;
        private List<ScheduleItemDto> _currentList = new();
        private List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> _eventList = new();
        private List<(string MaDienGia, string HoTen)> _speakerList = new();

                private string? _initialMaSuKien;

        public FrmSchedule() : this(new ScheduleService(), null)
        {
        }

        public FrmSchedule(string? maSuKien) : this(new ScheduleService(), maSuKien)
        {
        }

        public FrmSchedule(IScheduleService scheduleService) : this(scheduleService, null)
        {
        }

        public FrmSchedule(IScheduleService scheduleService, string? maSuKien)
        {
            _initialMaSuKien = maSuKien;
            InitializeComponent();
            _scheduleService = scheduleService ?? new ScheduleService();
            ApplyCustomStyles();

            if (!string.IsNullOrWhiteSpace(_initialMaSuKien))
            {
                for (int i = 0; i < cboLocSuKien.Items.Count; i++)
                {
                    if (cboLocSuKien.Items[i]?.ToString()?.StartsWith(_initialMaSuKien, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        cboLocSuKien.SelectedIndex = i;
                        break;
                    }
                }
            }
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

            StyleDataGridView(dgvLichTrinh);
            dgvLichTrinh.AutoGenerateColumns = false;
            dgvLichTrinh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvLichTrinh.ScrollBars = ScrollBars.Both;

            LoadAuxiliaryData();
        }

        private void LoadAuxiliaryData()
        {
            // 1. Nạp danh mục Sự kiện
            _eventList = _scheduleService.GetAvailableEvents();

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

            // 2. Nạp danh mục Diễn giả
            _speakerList = _scheduleService.GetAvailableSpeakers();

            cboLocDienGia.Items.Clear();
            cboLocDienGia.Items.Add("Tất cả diễn giả");
            foreach (var sp in _speakerList)
            {
                cboLocDienGia.Items.Add($"{sp.MaDienGia} - {sp.HoTen}");
            }
            cboLocDienGia.SelectedIndex = 0;

            cboDienGia.Items.Clear();
            cboDienGia.Items.Add(new ComboBoxItem(string.Empty, "(Không phân công / Chưa có)"));
            foreach (var sp in _speakerList)
            {
                cboDienGia.Items.Add(new ComboBoxItem(sp.MaDienGia, $"{sp.MaDienGia} - {sp.HoTen}"));
            }
            cboDienGia.SelectedIndex = 0;
        }

        private void FrmSchedule_Load(object sender, EventArgs e)
        {
            _isLoading = false;
            LoadData();
            SetFormState(FormMode.View);
        }

        private void LoadData()
        {
            string? selectedEventId = GetSelectedFilterEventId();
            string? selectedSpeakerId = GetSelectedFilterSpeakerId();
            string? keyword = txtTimKiem.Text.Trim();

            var result = _scheduleService.GetAll(selectedEventId, keyword, selectedSpeakerId);
            if (result.IsSuccess && result.Data != null)
            {
                _currentList = result.Data;
                dgvLichTrinh.DataSource = null;
                dgvLichTrinh.DataSource = _currentList;

                if (_currentList.Count > 0)
                {
                    dgvLichTrinh.Rows[0].Selected = true;
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

        private string? GetSelectedFilterEventId()
        {
            if (cboLocSuKien.SelectedIndex <= 0) return null;
            string text = cboLocSuKien.SelectedItem?.ToString() ?? string.Empty;
            int dashIdx = text.IndexOf('-');
            return dashIdx > 0 ? text.Substring(0, dashIdx).Trim() : text;
        }

        private string? GetSelectedFilterSpeakerId()
        {
            if (cboLocDienGia.SelectedIndex <= 0) return null;
            string text = cboLocDienGia.SelectedItem?.ToString() ?? string.Empty;
            int dashIdx = text.IndexOf('-');
            return dashIdx > 0 ? text.Substring(0, dashIdx).Trim() : text;
        }

        private void DisplaySelectedRow(ScheduleItemDto dto)
        {
            if (_currentMode != FormMode.View || dto == null) return;

            txtMaLichTrinh.Text = dto.MaLichTrinh;
            txtTenTietMuc.Text = dto.TenTietMuc;

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

            // Đồng bộ combobox Diễn giả
            if (cboDienGia.Items.Count > 0) cboDienGia.SelectedIndex = 0;
            if (!string.IsNullOrWhiteSpace(dto.MaDienGia))
            {
                for (int i = 0; i < cboDienGia.Items.Count; i++)
                {
                    if (cboDienGia.Items[i] is ComboBoxItem item &&
                        item.Value.Equals(dto.MaDienGia, StringComparison.OrdinalIgnoreCase))
                    {
                        cboDienGia.SelectedIndex = i;
                        break;
                    }
                }
            }

            dtpBatDau.Value = dto.ThoiGianBatDau >= dtpBatDau.MinDate && dto.ThoiGianBatDau <= dtpBatDau.MaxDate
                ? dto.ThoiGianBatDau
                : DateTime.Now;

            dtpKetThuc.Value = dto.ThoiGianKetThuc >= dtpKetThuc.MinDate && dto.ThoiGianKetThuc <= dtpKetThuc.MaxDate
                ? dto.ThoiGianKetThuc
                : DateTime.Now.AddHours(1);

            txtDiaDiemChiTiet.Text = dto.DiaDiemChiTiet ?? string.Empty;
            txtGhiChu.Text = dto.GhiChu ?? string.Empty;

            lblKiemTraStatus.Text = "Tiết mục hiện tại hợp lệ, không có xung đột.";
            lblKiemTraStatus.ForeColor = ColorSuccess;
            errBaoLoi.Clear();
        }

        private void ClearInputs()
        {
            txtMaLichTrinh.Clear();
            txtTenTietMuc.Clear();
            txtDiaDiemChiTiet.Clear();
            txtGhiChu.Clear();
            if (cboSuKien.Items.Count > 0) cboSuKien.SelectedIndex = 0;
            cboDienGia.SelectedIndex = 0;

            var currentEvent = GetCurrentSelectedEvent();
            if (currentEvent.HasValue)
            {
                dtpBatDau.Value = currentEvent.Value.BatDau;
                dtpKetThuc.Value = currentEvent.Value.BatDau.AddHours(1);
            }
            else
            {
                dtpBatDau.Value = DateTime.Now;
                dtpKetThuc.Value = DateTime.Now.AddHours(1);
            }

            lblKiemTraStatus.Text = "Bấm để kiểm tra giao thoa phòng & trùng lịch diễn giả tức thì";
            lblKiemTraStatus.ForeColor = ColorTextMuted;
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
        /// </summary>
        private void SetFormState(FormMode mode)
        {
            _currentMode = mode;
            errBaoLoi.Clear();

            switch (mode)
            {
                case FormMode.View:
                    txtMaLichTrinh.ReadOnly = true;
                    cboSuKien.Enabled = false;
                    txtTenTietMuc.ReadOnly = true;
                    dtpBatDau.Enabled = false;
                    dtpKetThuc.Enabled = false;
                    cboDienGia.Enabled = false;
                    txtDiaDiemChiTiet.ReadOnly = true;
                    txtGhiChu.ReadOnly = true;
                    btnKiemTraXungDot.Enabled = false;

                    btnThem.Enabled = true;
                    bool hasRows = dgvLichTrinh.CurrentRow != null;
                    btnSua.Enabled = hasRows;
                    btnXoa.Enabled = hasRows;
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false;

                    dgvLichTrinh.Enabled = true;
                    pnlFilter.Enabled = true;

                    if (dgvLichTrinh.CurrentRow != null && dgvLichTrinh.CurrentRow.DataBoundItem is ScheduleItemDto selected)
                    {
                        DisplaySelectedRow(selected);
                    }
                    break;

                case FormMode.Add:
                    txtMaLichTrinh.ReadOnly = false;
                    cboSuKien.Enabled = true;
                    txtTenTietMuc.ReadOnly = false;
                    dtpBatDau.Enabled = true;
                    dtpKetThuc.Enabled = true;
                    cboDienGia.Enabled = true;
                    txtDiaDiemChiTiet.ReadOnly = false;
                    txtGhiChu.ReadOnly = false;
                    btnKiemTraXungDot.Enabled = true;

                    ClearInputs();
                    txtMaLichTrinh.Text = SuggestNextCode();

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvLichTrinh.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtTenTietMuc.Focus();
                    break;

                case FormMode.Edit:
                    txtMaLichTrinh.ReadOnly = true; // Khóa cứng khóa chính
                    cboSuKien.Enabled = true;
                    txtTenTietMuc.ReadOnly = false;
                    dtpBatDau.Enabled = true;
                    dtpKetThuc.Enabled = true;
                    cboDienGia.Enabled = true;
                    txtDiaDiemChiTiet.ReadOnly = false;
                    txtGhiChu.ReadOnly = false;
                    btnKiemTraXungDot.Enabled = true;

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvLichTrinh.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtTenTietMuc.Focus();
                    break;
            }
        }

        private string SuggestNextCode()
        {
            int maxId = 0;
            foreach (var item in _currentList)
            {
                if (item.MaLichTrinh.StartsWith("LT", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(item.MaLichTrinh.Substring(2), out int num))
                {
                    if (num > maxId) maxId = num;
                }
            }

            var all = _scheduleService.GetAll();
            if (all.IsSuccess && all.Data != null)
            {
                foreach (var item in all.Data)
                {
                    if (item.MaLichTrinh.StartsWith("LT", StringComparison.OrdinalIgnoreCase) &&
                        int.TryParse(item.MaLichTrinh.Substring(2), out int num))
                    {
                        if (num > maxId) maxId = num;
                    }
                }
            }

            return $"LT{(maxId + 1):D3}";
        }

        private void dgvLichTrinh_SelectionChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View &&
                dgvLichTrinh.CurrentRow != null &&
                dgvLichTrinh.CurrentRow.DataBoundItem is ScheduleItemDto selected)
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
            if (dgvLichTrinh.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một tiết mục lịch trình cần chỉnh sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SetFormState(FormMode.Edit);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvLichTrinh.CurrentRow == null || !(dgvLichTrinh.CurrentRow.DataBoundItem is ScheduleItemDto selected))
            {
                MessageBox.Show("Vui lòng chọn tiết mục lịch trình cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa tiết mục '{selected.TenTietMuc}' (Mã: {selected.MaLichTrinh}) khỏi lịch trình sự kiện?",
                "Xác nhận xóa lịch trình",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                var result = _scheduleService.Delete(selected.MaLichTrinh);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    SetFormState(FormMode.View);
                }
                else
                {
                    MessageBox.Show(result.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out var dto)) return;

            ServiceResult result;
            if (_currentMode == FormMode.Add)
            {
                result = _scheduleService.Add(dto);
            }
            else
            {
                result = _scheduleService.Update(dto);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);

                // Chọn dòng vừa thêm/sửa
                for (int i = 0; i < dgvLichTrinh.Rows.Count; i++)
                {
                    if (dgvLichTrinh.Rows[i].DataBoundItem is ScheduleItemDto row &&
                        row.MaLichTrinh.Equals(dto.MaLichTrinh, StringComparison.OrdinalIgnoreCase))
                    {
                        dgvLichTrinh.Rows[i].Selected = true;
                        DisplaySelectedRow(row);
                        break;
                    }
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(result.Message);
                if (result.Errors != null && result.Errors.Count > 0)
                {
                    sb.AppendLine("\nChi tiết cảnh báo:");
                    foreach (var err in result.Errors)
                    {
                        sb.AppendLine($"• {err}");
                    }
                }
                MessageBox.Show(sb.ToString(), "Xung đột lịch trình (Quy tắc vi phạm)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_currentMode == FormMode.View)
            {
                if (keyData == Keys.F2)
                {
                    if (btnThem.Enabled) { btnThem.PerformClick(); return true; }
                }
                else if (keyData == Keys.F3)
                {
                    if (btnSua.Enabled) { btnSua.PerformClick(); return true; }
                }
                else if (keyData == Keys.Delete && !(ActiveControl is TextBoxBase))
                {
                    if (btnXoa.Enabled) { btnXoa.PerformClick(); return true; }
                }
                else if (keyData == Keys.F5)
                {
                    if (btnLamMoi.Enabled) { btnLamMoi.PerformClick(); return true; }
                }
            }
            else
            {
                if (keyData == (Keys.Control | Keys.S))
                {
                    if (btnLuu.Enabled) { btnLuu.PerformClick(); return true; }
                }
                else if (keyData == Keys.Escape)
                {
                    if (btnHuy.Enabled) { btnHuy.PerformClick(); return true; }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FrmSchedule_KeyDown(object sender, KeyEventArgs e)
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

        private void btnHuy_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.View);
        }

        private void btnKiemTraXungDot_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out var dto)) return;

            string? excludeId = _currentMode == FormMode.Edit ? dto.MaLichTrinh : null;
            var checkRes = _scheduleService.CheckConflict(dto, excludeId);

            if (checkRes.IsSuccess)
            {
                lblKiemTraStatus.Text = "✅ Hợp lệ: Không có xung đột phòng và diễn giả.";
                lblKiemTraStatus.ForeColor = ColorSuccess;
                MessageBox.Show(
                    "Tuyệt vời! Khung giờ, phòng chức năng và diễn giả hoàn toàn khả dụng (Thỏa mãn 100% quy tắc BR-05 & BR-06).",
                    "Kiểm tra thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                lblKiemTraStatus.Text = "⚠️ Phát hiện xung đột! Vui lòng xem chi tiết.";
                lblKiemTraStatus.ForeColor = ColorDanger;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(checkRes.Message);
                if (checkRes.Errors != null && checkRes.Errors.Count > 0)
                {
                    sb.AppendLine("\nChi tiết đối kháng:");
                    foreach (var err in checkRes.Errors)
                    {
                        sb.AppendLine($"• {err}");
                    }
                }

                MessageBox.Show(sb.ToString(), "Cảnh báo xung đột lịch trình (BR-05/BR-06)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidateInputs(out ScheduleItemDto dto)
        {
            dto = new ScheduleItemDto();
            errBaoLoi.Clear();
            bool isValid = true;

            string maLichTrinh = txtMaLichTrinh.Text.Trim();
            if (string.IsNullOrWhiteSpace(maLichTrinh))
            {
                errBaoLoi.SetError(txtMaLichTrinh, "Mã lịch trình không được để trống.");
                isValid = false;
            }

            var eventItem = cboSuKien.SelectedItem as ComboBoxItem;
            if (eventItem == null || string.IsNullOrWhiteSpace(eventItem.Value))
            {
                errBaoLoi.SetError(cboSuKien, "Vui lòng chọn sự kiện áp dụng.");
                isValid = false;
            }

            string tenTietMuc = txtTenTietMuc.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenTietMuc))
            {
                errBaoLoi.SetError(txtTenTietMuc, "Tên chương trình / tiết mục không được để trống.");
                isValid = false;
            }

            DateTime batDau = dtpBatDau.Value;
            DateTime ketThuc = dtpKetThuc.Value;
            if (ketThuc <= batDau)
            {
                errBaoLoi.SetError(dtpKetThuc, "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");
                isValid = false;
            }

            if (!isValid) return false;

            string? maDienGia = null;
            string? tenDienGia = null;
            if (cboDienGia.SelectedItem is ComboBoxItem speakerItem && !string.IsNullOrWhiteSpace(speakerItem.Value))
            {
                maDienGia = speakerItem.Value;
                tenDienGia = speakerItem.Text;
            }

            dto = new ScheduleItemDto
            {
                MaLichTrinh = maLichTrinh,
                MaSuKien = eventItem!.Value,
                TenSuKien = eventItem!.Text,
                TenTietMuc = tenTietMuc,
                ThoiGianBatDau = batDau,
                ThoiGianKetThuc = ketThuc,
                MaDienGia = maDienGia,
                TenDienGia = tenDienGia,
                DiaDiemChiTiet = string.IsNullOrWhiteSpace(txtDiaDiemChiTiet.Text) ? null : txtDiaDiemChiTiet.Text.Trim(),
                GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim()
            };

            return true;
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (_currentList == null || _currentList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu lịch trình để xuất báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Tệp CSV (*.csv)|*.csv",
                FileName = $"LichTrinh_SuKien_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Mã lịch trình,Mã sự kiện,Sự kiện,Tên tiết mục,Ngày diễn ra,Khung giờ,Phòng/Hội trường,Diễn giả,Ghi chú");

                    foreach (var item in _currentList)
                    {
                        sb.AppendLine($"\"{EscapeCsv(item.MaLichTrinh)}\",\"{EscapeCsv(item.MaSuKien)}\",\"{EscapeCsv(item.TenSuKien)}\",\"{EscapeCsv(item.TenTietMuc)}\",\"{item.NgayDienRa}\",\"{item.KhungGio}\",\"{EscapeCsv(item.DiaDiemChiTiet)}\",\"{EscapeCsv(item.TenDienGia)}\",\"{EscapeCsv(item.GhiChu)}\"");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show($"Xuất dữ liệu lịch trình thành công!\nĐường dẫn: {sfd.FileName}", "Xuất Excel thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            cboLocDienGia.SelectedIndex = 0;
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

        private void cboLocDienGia_SelectedIndexChanged(object sender, EventArgs e)
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
                dtpKetThuc.Value = currentEvent.Value.BatDau.AddHours(1);
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
