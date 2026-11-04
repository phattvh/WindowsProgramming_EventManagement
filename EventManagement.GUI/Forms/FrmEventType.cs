using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình F06 – Quản lý loại sự kiện
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// Tuân thủ: Mẫu bố cục 2, 3 trạng thái màn hình, BR-07 & BR-20.
    /// </summary>
    public partial class FrmEventType : FrmBase
    {
        private readonly IEventTypeService _eventTypeService;
        private FormMode _currentMode = FormMode.View;
        private List<EventTypeDto> _currentList = new();

        public FrmEventType() : this(new EventTypeService())
        {
        }

        public FrmEventType(IEventTypeService eventTypeService)
        {
            InitializeComponent();
            _eventTypeService = eventTypeService;
            ApplyCustomStyles();
        }

        private void ApplyCustomStyles()
        {
            // Thiết lập phong cách cho các nút bấm theo quy chuẩn
            StyleButton(btnThem, ButtonRole.Success);
            StyleButton(btnSua, ButtonRole.Primary);
            StyleButton(btnXoa, ButtonRole.Danger);
            StyleButton(btnLuu, ButtonRole.Primary);
            StyleButton(btnHuy, ButtonRole.Secondary);
            StyleButton(btnLamMoi, ButtonRole.Secondary);

            dgvLoaiSuKien.AutoGenerateColumns = false;
            cboLocTrangThai.SelectedIndex = 0; // "Tất cả"
        }

        private void FrmEventType_Load(object sender, EventArgs e)
        {
            LoadData();
            SetFormState(FormMode.View);
        }

        private void LoadData()
        {
            string? keyword = txtTimKiem.Text.Trim();
            string? status = cboLocTrangThai.SelectedItem?.ToString();

            var result = _eventTypeService.GetAll(keyword, status);
            if (result.IsSuccess && result.Data != null)
            {
                _currentList = result.Data;
                dgvLoaiSuKien.DataSource = null;
                dgvLoaiSuKien.DataSource = _currentList;

                if (_currentList.Count > 0)
                {
                    dgvLoaiSuKien.Rows[0].Selected = true;
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

        private void DisplaySelectedRow(EventTypeDto dto)
        {
            if (_currentMode != FormMode.View) return;

            txtMaLoaiSK.Text = dto.MaLoaiSK;
            txtTenLoaiSK.Text = dto.TenLoaiSK;
            txtMoTa.Text = dto.MoTa ?? string.Empty;
            cboTrangThai.SelectedItem = dto.TrangThai;

            // Xóa lỗi đang hiển thị nếu có
            errBaoLoi.Clear();
        }

        private void ClearInputs()
        {
            txtMaLoaiSK.Clear();
            txtTenLoaiSK.Clear();
            txtMoTa.Clear();
            cboTrangThai.SelectedItem = "Hoạt động";
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
                    txtMaLoaiSK.ReadOnly = true;
                    txtTenLoaiSK.ReadOnly = true;
                    txtMoTa.ReadOnly = true;
                    cboTrangThai.Enabled = false;

                    btnThem.Enabled = true;
                    bool hasRows = dgvLoaiSuKien.CurrentRow != null;
                    btnSua.Enabled = hasRows;
                    btnXoa.Enabled = hasRows;
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false;

                    dgvLoaiSuKien.Enabled = true;
                    pnlFilter.Enabled = true;

                    if (dgvLoaiSuKien.CurrentRow != null && dgvLoaiSuKien.CurrentRow.DataBoundItem is EventTypeDto selected)
                    {
                        DisplaySelectedRow(selected);
                    }
                    break;

                case FormMode.Add:
                    txtMaLoaiSK.ReadOnly = false;
                    txtTenLoaiSK.ReadOnly = false;
                    txtMoTa.ReadOnly = false;
                    cboTrangThai.Enabled = true;

                    ClearInputs();
                    txtMaLoaiSK.Text = SuggestNextCode();
                    cboTrangThai.SelectedItem = "Hoạt động";

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvLoaiSuKien.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtTenLoaiSK.Focus();
                    break;

                case FormMode.Edit:
                    txtMaLoaiSK.ReadOnly = true; // Khóa chính tuyệt đối không cho sửa
                    txtTenLoaiSK.ReadOnly = false;
                    txtMoTa.ReadOnly = false;
                    cboTrangThai.Enabled = true;

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvLoaiSuKien.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtTenLoaiSK.Focus();
                    break;
            }
        }

        private string SuggestNextCode()
        {
            int maxId = 0;
            foreach (var item in _currentList)
            {
                if (item.MaLoaiSK.StartsWith("LSK", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(item.MaLoaiSK[3..], out int num))
                {
                    if (num > maxId) maxId = num;
                }
            }
            return $"LSK{(maxId + 1):D2}";
        }

        private void dgvLoaiSuKien_SelectionChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View && dgvLoaiSuKien.CurrentRow != null)
            {
                if (dgvLoaiSuKien.CurrentRow.DataBoundItem is EventTypeDto item)
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
            if (dgvLoaiSuKien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn loại sự kiện cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetFormState(FormMode.Edit);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvLoaiSuKien.CurrentRow == null || dgvLoaiSuKien.CurrentRow.DataBoundItem is not EventTypeDto selected)
            {
                MessageBox.Show("Vui lòng chọn loại sự kiện cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa loại sự kiện '{selected.TenLoaiSK}' (Mã: {selected.MaLoaiSK})?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            var result = _eventTypeService.Delete(selected.MaLoaiSK);
            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);
            }
            else
            {
                // Xử lý vi phạm quy tắc BR-07: Hỏi có muốn chuyển sang Soft Delete (BR-20)
                if (result.ErrorCode == "BR-07")
                {
                    var opt = MessageBox.Show(
                        $"{result.Message}\n\nBạn có muốn chuyển trạng thái loại sự kiện sang 'Ngừng áp dụng' theo quy tắc BR-20 thay vì xóa?",
                        "Cảnh báo nghiệp vụ BR-07",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (opt == DialogResult.Yes)
                    {
                        var softResult = _eventTypeService.SoftDelete(selected.MaLoaiSK);
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

            var dto = new EventTypeDto
            {
                MaLoaiSK = txtMaLoaiSK.Text.Trim(),
                TenLoaiSK = txtTenLoaiSK.Text.Trim(),
                MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động"
            };

            ServiceResult result;
            if (_currentMode == FormMode.Add)
            {
                result = _eventTypeService.Add(dto);
            }
            else
            {
                result = _eventTypeService.Update(dto);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);

                // Chọn dòng vừa thao tác
                foreach (DataGridViewRow row in dgvLoaiSuKien.Rows)
                {
                    if (row.DataBoundItem is EventTypeDto item && item.MaLoaiSK.Equals(dto.MaLoaiSK, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        dgvLoaiSuKien.CurrentCell = row.Cells[0];
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

        private void btnHuy_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.View);
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            errBaoLoi.Clear();

            if (string.IsNullOrWhiteSpace(txtMaLoaiSK.Text))
            {
                errBaoLoi.SetError(txtMaLoaiSK, "Mã loại sự kiện không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtTenLoaiSK.Text))
            {
                errBaoLoi.SetError(txtTenLoaiSK, "Tên loại sự kiện không được để trống!");
                isValid = false;
            }

            return isValid;
        }
    }
}
