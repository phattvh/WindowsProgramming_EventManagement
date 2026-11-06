using System;
using System.Collections.Generic;
using System.IO;
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
    /// Màn hình F14 – Quản lý nhân sự ban tổ chức
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// Tuân thủ: Mẫu bố cục 2, 3 trạng thái màn hình (Xem - Thêm - Sửa), BR-13 & BR-20.
    /// </summary>
    public partial class FrmStaff : FrmBase
    {
        private readonly IStaffService _staffService;
        private FormMode _currentMode = FormMode.View;
        private List<StaffDto> _currentList = new();

        public FrmStaff() : this(new StaffService())
        {
        }

        public FrmStaff(IStaffService staffService)
        {
            InitializeComponent();
            _staffService = staffService;
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

            dgvNhanSu.AutoGenerateColumns = false;
            cboLocPhongBan.SelectedIndex = 0; // "Tất cả"
            cboLocTrangThai.SelectedIndex = 0; // "Tất cả"
        }

        private void FrmStaff_Load(object sender, EventArgs e)
        {
            LoadData();
            SetFormState(FormMode.View);
        }

        private void LoadData()
        {
            string? keyword = txtTimKiem.Text.Trim();
            string? department = cboLocPhongBan.SelectedItem?.ToString();
            string? status = cboLocTrangThai.SelectedItem?.ToString();

            var result = _staffService.GetAll(keyword, status, department);
            if (result.IsSuccess && result.Data != null)
            {
                _currentList = result.Data;
                dgvNhanSu.DataSource = null;
                dgvNhanSu.DataSource = _currentList;

                if (_currentList.Count > 0)
                {
                    dgvNhanSu.Rows[0].Selected = true;
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

        private void DisplaySelectedRow(StaffDto dto)
        {
            if (_currentMode != FormMode.View) return;

            txtMaNhanSu.Text = dto.MaNhanSu;
            txtHoTen.Text = dto.HoTen;
            txtSoDienThoai.Text = dto.SoDienThoai;
            txtEmail.Text = dto.Email ?? string.Empty;
            cboChucDanh.Text = dto.ChucDanh;
            cboPhongBan.Text = dto.PhongBan;
            cboTrangThai.SelectedItem = dto.TrangThai;

            errBaoLoi.Clear();
        }

        private void ClearInputs()
        {
            txtMaNhanSu.Clear();
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            cboChucDanh.SelectedIndex = cboChucDanh.Items.Count > 0 ? 0 : -1;
            cboPhongBan.SelectedIndex = cboPhongBan.Items.Count > 0 ? 0 : -1;
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
                    txtMaNhanSu.ReadOnly = true;
                    txtHoTen.ReadOnly = true;
                    txtSoDienThoai.ReadOnly = true;
                    txtEmail.ReadOnly = true;
                    cboChucDanh.Enabled = false;
                    cboPhongBan.Enabled = false;
                    cboTrangThai.Enabled = false;

                    btnThem.Enabled = true;
                    bool hasRows = dgvNhanSu.CurrentRow != null;
                    btnSua.Enabled = hasRows;
                    btnXoa.Enabled = hasRows;
                    btnXuatExcel.Enabled = _currentList.Count > 0;
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false;

                    dgvNhanSu.Enabled = true;
                    pnlFilter.Enabled = true;

                    if (dgvNhanSu.CurrentRow != null && dgvNhanSu.CurrentRow.DataBoundItem is StaffDto selected)
                    {
                        DisplaySelectedRow(selected);
                    }
                    break;

                case FormMode.Add:
                    txtMaNhanSu.ReadOnly = false;
                    txtHoTen.ReadOnly = false;
                    txtSoDienThoai.ReadOnly = false;
                    txtEmail.ReadOnly = false;
                    cboChucDanh.Enabled = true;
                    cboPhongBan.Enabled = true;
                    cboTrangThai.Enabled = true;

                    ClearInputs();
                    txtMaNhanSu.Text = SuggestNextCode();
                    cboTrangThai.SelectedItem = "Hoạt động";

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnXuatExcel.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvNhanSu.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtHoTen.Focus();
                    break;

                case FormMode.Edit:
                    txtMaNhanSu.ReadOnly = true; // Khóa chính tuyệt đối không cho sửa
                    txtHoTen.ReadOnly = false;
                    txtSoDienThoai.ReadOnly = false;
                    txtEmail.ReadOnly = false;
                    cboChucDanh.Enabled = true;
                    cboPhongBan.Enabled = true;
                    cboTrangThai.Enabled = true;

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnXuatExcel.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvNhanSu.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtHoTen.Focus();
                    break;
            }
        }

        private string SuggestNextCode()
        {
            int maxId = 0;
            foreach (var item in _currentList)
            {
                if (item.MaNhanSu.StartsWith("NS", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(item.MaNhanSu[2..], out int num))
                {
                    if (num > maxId) maxId = num;
                }
            }
            return $"NS{(maxId + 1):D3}";
        }

        private void dgvNhanSu_SelectionChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View && dgvNhanSu.CurrentRow != null)
            {
                if (dgvNhanSu.CurrentRow.DataBoundItem is StaffDto item)
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

        private void cboLocPhongBan_SelectedIndexChanged(object sender, EventArgs e)
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
            cboLocPhongBan.SelectedIndex = 0;
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
            if (dgvNhanSu.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân sự cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetFormState(FormMode.Edit);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanSu.CurrentRow == null || dgvNhanSu.CurrentRow.DataBoundItem is not StaffDto selected)
            {
                MessageBox.Show("Vui lòng chọn nhân sự cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa hồ sơ nhân sự '{selected.HoTen}' (Mã: {selected.MaNhanSu})?",
                "Xác nhận xóa nhân sự",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            var result = _staffService.Delete(selected.MaNhanSu);
            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);
            }
            else
            {
                // Xử lý vi phạm quy tắc BR-20: Nhân sự đã được phân công ca trực
                if (result.ErrorCode == "BR-20")
                {
                    var opt = MessageBox.Show(
                        $"{result.Message}\n\nBạn có muốn chuyển trạng thái nhân sự này sang 'Nghỉ việc' theo quy tắc BR-20 thay vì xóa vĩnh viễn?",
                        "Cảnh báo nghiệp vụ BR-20",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (opt == DialogResult.Yes)
                    {
                        var softResult = _staffService.SoftDelete(selected.MaNhanSu);
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

            var dto = new StaffDto
            {
                MaNhanSu = txtMaNhanSu.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                ChucDanh = string.IsNullOrWhiteSpace(cboChucDanh.Text) ? "Cộng tác viên" : cboChucDanh.Text.Trim(),
                PhongBan = string.IsNullOrWhiteSpace(cboPhongBan.Text) ? "Hậu cần" : cboPhongBan.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động"
            };

            ServiceResult result;
            if (_currentMode == FormMode.Add)
            {
                result = _staffService.Add(dto);
            }
            else
            {
                result = _staffService.Update(dto);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);

                // Chọn dòng vừa cập nhật
                foreach (DataGridViewRow row in dgvNhanSu.Rows)
                {
                    if (row.DataBoundItem is StaffDto item && item.MaNhanSu.Equals(dto.MaNhanSu, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        dgvNhanSu.CurrentCell = row.Cells[0];
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

        private void FrmStaff_KeyDown(object sender, KeyEventArgs e)
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

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (_currentList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất tệp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Title = "Xuất danh sách nhân sự ban tổ chức",
                Filter = "Tệp CSV (Comma-separated values)|*.csv",
                FileName = $"DanhSachNhanSu_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);
                    sw.WriteLine("MaNhanSu,HoTen,SoDienThoai,Email,ChucDanh,PhongBan,SoCaTrucThamGia,TrangThai");
                    foreach (var s in _currentList)
                    {
                        sw.WriteLine($"\"{s.MaNhanSu}\",\"{s.HoTen}\",\"{s.SoDienThoai}\",\"{s.Email}\",\"{s.ChucDanh}\",\"{s.PhongBan}\",{s.SoCaTrucThamGia},\"{s.TrangThai}\"");
                    }
                    MessageBox.Show($"Xuất dữ liệu thành công ra tệp:\n{sfd.FileName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất tệp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            errBaoLoi.Clear();

            if (string.IsNullOrWhiteSpace(txtMaNhanSu.Text))
            {
                errBaoLoi.SetError(txtMaNhanSu, "Mã nhân sự không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errBaoLoi.SetError(txtHoTen, "Họ và tên nhân sự không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                errBaoLoi.SetError(txtSoDienThoai, "Số điện thoại nhân sự không được để trống!");
                isValid = false;
            }
            else
            {
                string phone = txtSoDienThoai.Text.Trim();
                if (!Regex.IsMatch(phone, @"^[0-9]{8,15}$"))
                {
                    errBaoLoi.SetError(txtSoDienThoai, "Số điện thoại phải từ 8 đến 15 chữ số!");
                    isValid = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string email = txtEmail.Text.Trim();
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    errBaoLoi.SetError(txtEmail, "Email không đúng định dạng chuẩn (ví dụ: name@domain.com)!");
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
