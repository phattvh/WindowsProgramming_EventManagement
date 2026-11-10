using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.GUI.Common;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình F07 – Quản lý địa điểm tổ chức
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// Tuân thủ: Mẫu bố cục 2, 3 trạng thái màn hình, BR-04 & BR-20.
    /// </summary>
    public partial class FrmVenue : FrmBase
    {
        private readonly IVenueService _venueService;
        private FormMode _currentMode = FormMode.View;
        private List<VenueDto> _currentList = new();

        public FrmVenue() : this(new VenueService())
        {
        }

        public FrmVenue(IVenueService venueService)
        {
            InitializeComponent();
            _venueService = venueService;
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            // Thiết lập màu sắc nút bấm theo chuẩn
            StyleButton(btnThem, ButtonRole.Success);
            StyleButton(btnSua, ButtonRole.Primary);
            StyleButton(btnXoa, ButtonRole.Danger);
            StyleButton(btnLuu, ButtonRole.Primary);
            StyleButton(btnHuy, ButtonRole.Secondary);
            StyleButton(btnLamMoi, ButtonRole.Secondary);

            StyleDataGridView(dgvDiaDiem);
            dgvDiaDiem.AutoGenerateColumns = false;
            foreach (DataGridViewColumn col in dgvDiaDiem.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            colMaDiaDiem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSucChuaToiDa.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSoSuKienToChuc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Bố trí thanh lọc và bảng không bị đè bởi Header
            pnlFilter.Top = pnlHeader.Bottom + 10;
            dgvDiaDiem.Top = pnlFilter.Bottom + 10;
            if (pnlButtons != null)
            {
                dgvDiaDiem.Height = Math.Max(120, pnlButtons.Top - 10 - dgvDiaDiem.Top);
            }

            // Tối ưu độ rộng cột để không bị cắt cụt tên địa điểm và địa chỉ
            colMaDiaDiem.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colMaDiaDiem.Width = 90;
            colTenDiaDiem.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colTenDiaDiem.Width = 250;
            colSucChuaToiDa.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSucChuaToiDa.Width = 100;
            colNguoiQuanLy.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colNguoiQuanLy.Width = 140;
            colSoDienThoai.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSoDienThoai.Width = 110;
            colTrangThai.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colTrangThai.Width = 110;
            colSoSuKienToChuc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSoSuKienToChuc.Width = 90;
            colDiaChi.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colSoDienThoai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            cboLocTrangThai.SelectedIndex = 0; // "Tất cả"
        }

        private void FrmVenue_Load(object sender, EventArgs e)
        {
            LoadData();
            SetFormState(FormMode.View);
        }

        private void LoadData()
        {
            string? keyword = txtTimKiem.Text.Trim();
            string? status = cboLocTrangThai.SelectedItem?.ToString();

            var result = _venueService.GetAll(keyword, status);
            if (result.IsSuccess && result.Data != null)
            {
                _currentList = result.Data;
                dgvDiaDiem.DataSource = null;
                dgvDiaDiem.DataSource = _currentList;

                if (_currentList.Count > 0)
                {
                    dgvDiaDiem.Rows[0].Selected = true;
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

        private void DisplaySelectedRow(VenueDto dto)
        {
            if (_currentMode != FormMode.View) return;

            txtMaDiaDiem.Text = dto.MaDiaDiem;
            txtTenDiaDiem.Text = dto.TenDiaDiem;
            txtDiaChi.Text = dto.DiaChi;
            nudSucChuaToiDa.Value = dto.SucChuaToiDa > 0 ? dto.SucChuaToiDa : 100;
            txtNguoiQuanLy.Text = dto.NguoiQuanLy ?? string.Empty;
            txtSoDienThoai.Text = dto.SoDienThoai ?? string.Empty;
            cboTrangThai.SelectedItem = dto.TrangThai;
            txtGhiChu.Text = dto.GhiChu ?? string.Empty;

            errBaoLoi.Clear();
        }

        private void ClearInputs()
        {
            txtMaDiaDiem.Clear();
            txtTenDiaDiem.Clear();
            txtDiaChi.Clear();
            nudSucChuaToiDa.Value = 100;
            txtNguoiQuanLy.Clear();
            txtSoDienThoai.Clear();
            cboTrangThai.SelectedItem = "Hoạt động";
            txtGhiChu.Clear();
            errBaoLoi.Clear();
        }

        /// <summary>
        /// Bộ máy quản lý 3 trạng thái màn hình (Xem - Thêm - Sửa) theo chuẩn mục 5 docs/08
        /// </summary>
        private void SetFormState(FormMode mode)
        {
            _currentMode = mode;
            errBaoLoi.Clear();

            switch (mode)
            {
                case FormMode.View:
                    txtMaDiaDiem.ReadOnly = true;
                    txtTenDiaDiem.ReadOnly = true;
                    txtDiaChi.ReadOnly = true;
                    nudSucChuaToiDa.Enabled = false;
                    txtNguoiQuanLy.ReadOnly = true;
                    txtSoDienThoai.ReadOnly = true;
                    cboTrangThai.Enabled = false;
                    txtGhiChu.ReadOnly = true;

                    btnThem.Enabled = true;
                    bool hasRows = dgvDiaDiem.CurrentRow != null;
                    btnSua.Enabled = hasRows;
                    btnXoa.Enabled = hasRows;
                    btnLuu.Visible = false;
                    btnHuy.Visible = false;
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false;

                    dgvDiaDiem.Enabled = true;
                    pnlFilter.Enabled = true;

                    if (dgvDiaDiem.CurrentRow != null && dgvDiaDiem.CurrentRow.DataBoundItem is VenueDto selected)
                    {
                        DisplaySelectedRow(selected);
                    }
                    break;

                case FormMode.Add:
                    txtMaDiaDiem.ReadOnly = false;
                    txtTenDiaDiem.ReadOnly = false;
                    txtDiaChi.ReadOnly = false;
                    nudSucChuaToiDa.Enabled = true;
                    txtNguoiQuanLy.ReadOnly = false;
                    txtSoDienThoai.ReadOnly = false;
                    cboTrangThai.Enabled = true;
                    txtGhiChu.ReadOnly = false;

                    ClearInputs();
                    txtMaDiaDiem.Text = SuggestNextCode();
                    cboTrangThai.SelectedItem = "Hoạt động";

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Visible = true;
                    btnHuy.Visible = true;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvDiaDiem.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtTenDiaDiem.Focus();
                    break;

                case FormMode.Edit:
                    txtMaDiaDiem.ReadOnly = true; // Khóa chính tuyệt đối không cho sửa
                    txtTenDiaDiem.ReadOnly = false;
                    txtDiaChi.ReadOnly = false;
                    nudSucChuaToiDa.Enabled = true;
                    txtNguoiQuanLy.ReadOnly = false;
                    txtSoDienThoai.ReadOnly = false;
                    cboTrangThai.Enabled = true;
                    txtGhiChu.ReadOnly = false;

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Visible = true;
                    btnHuy.Visible = true;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvDiaDiem.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtTenDiaDiem.Focus();
                    break;
            }
        }

        private string SuggestNextCode()
        {
            int maxId = 0;
            foreach (var item in _currentList)
            {
                if (item.MaDiaDiem.StartsWith("DD", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(item.MaDiaDiem[2..], out int num))
                {
                    if (num > maxId) maxId = num;
                }
            }
            return $"DD{(maxId + 1):D3}";
        }

        private void dgvDiaDiem_SelectionChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View && dgvDiaDiem.CurrentRow != null)
            {
                if (dgvDiaDiem.CurrentRow.DataBoundItem is VenueDto item)
                {
                    DisplaySelectedRow(item);
                }
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View)
            {
                LoadData();
            }
        }

        private void cboLocTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View)
            {
                LoadData();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboLocTrangThai.SelectedIndex = 0;
            LoadData();
            SetFormState(FormMode.View);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.Add);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDiaDiem.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn địa điểm cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetFormState(FormMode.Edit);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDiaDiem.CurrentRow == null || dgvDiaDiem.CurrentRow.DataBoundItem is not VenueDto selected)
            {
                MessageBox.Show("Vui lòng chọn địa điểm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa địa điểm '{selected.TenDiaDiem}' (Mã: {selected.MaDiaDiem})?",
                "Xác nhận xóa địa điểm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            var result = _venueService.Delete(selected.MaDiaDiem);
            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);
            }
            else
            {
                // Xử lý vi phạm quy tắc BR-20: Đã từng tổ chức sự kiện
                if (result.ErrorCode == "BR-20")
                {
                    var opt = MessageBox.Show(
                        $"{result.Message}\n\nBạn có muốn chuyển trạng thái địa điểm này sang 'Ngừng sử dụng' theo quy tắc BR-20 thay vì xóa?",
                        "Cảnh báo nghiệp vụ BR-20",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (opt == DialogResult.Yes)
                    {
                        var softResult = _venueService.SoftDelete(selected.MaDiaDiem);
                        MessageBox.Show(softResult.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        SetFormState(FormMode.View);
                    }
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi xóa dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var dto = new VenueDto
            {
                MaDiaDiem = txtMaDiaDiem.Text.Trim(),
                TenDiaDiem = txtTenDiaDiem.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                SucChuaToiDa = (int)nudSucChuaToiDa.Value,
                NguoiQuanLy = string.IsNullOrWhiteSpace(txtNguoiQuanLy.Text) ? null : txtNguoiQuanLy.Text.Trim(),
                SoDienThoai = string.IsNullOrWhiteSpace(txtSoDienThoai.Text) ? null : txtSoDienThoai.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động",
                GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim()
            };

            ServiceResult result;
            if (_currentMode == FormMode.Add)
            {
                result = _venueService.Add(dto);
            }
            else
            {
                result = _venueService.Update(dto);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);

                // Chọn dòng vừa cập nhật
                foreach (DataGridViewRow row in dgvDiaDiem.Rows)
                {
                    if (row.DataBoundItem is VenueDto item && item.MaDiaDiem.Equals(dto.MaDiaDiem, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        dgvDiaDiem.CurrentCell = row.Cells[0];
                        DisplaySelectedRow(item);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi thực thi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmVenue_KeyDown(object sender, KeyEventArgs e)
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
                else if (e.KeyCode == Keys.Delete)
                {
                    btnXoa.PerformClick();
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

        private bool ValidateInputs()
        {
            bool isValid = true;
            errBaoLoi.Clear();

            if (string.IsNullOrWhiteSpace(txtMaDiaDiem.Text))
            {
                errBaoLoi.SetError(txtMaDiaDiem, "Mã địa điểm không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtTenDiaDiem.Text))
            {
                errBaoLoi.SetError(txtTenDiaDiem, "Tên địa điểm không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                errBaoLoi.SetError(txtDiaChi, "Địa chỉ không được để trống!");
                isValid = false;
            }

            if (nudSucChuaToiDa.Value <= 0)
            {
                errBaoLoi.SetError(nudSucChuaToiDa, "Sức chứa tối đa phải lớn hơn 0!");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                string phone = txtSoDienThoai.Text.Trim();
                if (!Regex.IsMatch(phone, @"^[0-9]{8,15}$"))
                {
                    errBaoLoi.SetError(txtSoDienThoai, "Số điện thoại phải từ 8 đến 15 chữ số!");
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
