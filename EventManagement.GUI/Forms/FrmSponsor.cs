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
    /// Màn hình F16 – Quản lý nhà tài trợ & đối tác sự kiện
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// Tuân thủ: Mẫu bố cục 2 (Danh sách kết hợp Nhập liệu), 3 trạng thái Form (Xem - Thêm - Sửa), BR-20.
    /// </summary>
    public partial class FrmSponsor : FrmBase
    {
        private readonly ISponsorService _sponsorService;
        private FormMode _currentMode = FormMode.View;
        private List<SponsorDto> _currentList = new();
        private bool _isLoading = true;

        public FrmSponsor() : this(new SponsorService())
        {
        }

        public FrmSponsor(ISponsorService sponsorService)
        {
            InitializeComponent();
            _sponsorService = sponsorService;
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

            dgvNhaTaiTro.AutoGenerateColumns = false;
        }

        private void FrmSponsor_Load(object sender, EventArgs e)
        {
            _isLoading = true;
            cboLocGoiTaiTro.SelectedIndex = 0; // "Tất cả"
            cboLocTrangThai.SelectedIndex = 0; // "Tất cả"
            _isLoading = false;

            LoadData();
            SetFormState(FormMode.View);
        }

        private void LoadData()
        {
            if (_isLoading) return;

            string? keyword = txtTimKiem.Text.Trim();
            string? goiFilter = cboLocGoiTaiTro.SelectedItem?.ToString();
            string? statusFilter = cboLocTrangThai.SelectedItem?.ToString();

            var result = _sponsorService.GetAll(keyword, goiFilter, statusFilter);
            if (result.IsSuccess && result.Data != null)
            {
                _currentList = result.Data;
                dgvNhaTaiTro.DataSource = null;
                dgvNhaTaiTro.DataSource = _currentList;

                if (_currentList.Count > 0)
                {
                    dgvNhaTaiTro.Rows[0].Selected = true;
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

        private void DisplaySelectedRow(SponsorDto dto)
        {
            if (_currentMode != FormMode.View) return;

            txtMaNhaTaiTro.Text = dto.MaNhaTaiTro;
            txtTenToChuc.Text = dto.TenToChuc;
            txtNguoiDaiDien.Text = dto.NguoiDaiDien ?? string.Empty;
            txtSoDienThoai.Text = dto.SoDienThoai ?? string.Empty;
            txtEmail.Text = dto.Email ?? string.Empty;

            cboGoiTaiTro.SelectedItem = dto.GoiTaiTro;
            if (cboGoiTaiTro.SelectedIndex == -1 && cboGoiTaiTro.Items.Count > 0)
                cboGoiTaiTro.SelectedIndex = 0;

            nudGiaTriTaiTro.Value = Math.Clamp(dto.TongGiaTriTaiTro, nudGiaTriTaiTro.Minimum, nudGiaTriTaiTro.Maximum);

            cboHinhThuc.SelectedItem = dto.HinhThucTaiTro;
            if (cboHinhThuc.SelectedIndex == -1 && cboHinhThuc.Items.Count > 0)
                cboHinhThuc.SelectedIndex = 0;

            cboTrangThai.SelectedItem = dto.TrangThai;
            if (cboTrangThai.SelectedIndex == -1 && cboTrangThai.Items.Count > 0)
                cboTrangThai.SelectedIndex = 0;

            txtDiaChi.Text = dto.DiaChi ?? string.Empty;
            txtGhiChu.Text = dto.GhiChu ?? string.Empty;

            errBaoLoi.Clear();
        }

        private void SetFormState(FormMode mode)
        {
            _currentMode = mode;

            bool isEditing = mode == FormMode.Add || mode == FormMode.Edit;

            // Vùng nhập liệu
            txtMaNhaTaiTro.ReadOnly = mode != FormMode.Add;
            txtTenToChuc.ReadOnly = !isEditing;
            txtNguoiDaiDien.ReadOnly = !isEditing;
            txtSoDienThoai.ReadOnly = !isEditing;
            txtEmail.ReadOnly = !isEditing;
            cboGoiTaiTro.Enabled = isEditing;
            nudGiaTriTaiTro.Enabled = isEditing;
            cboHinhThuc.Enabled = isEditing;
            cboTrangThai.Enabled = isEditing;
            txtDiaChi.ReadOnly = !isEditing;
            txtGhiChu.ReadOnly = !isEditing;

            // Nút bấm
            btnThem.Enabled = !isEditing;
            btnSua.Enabled = !isEditing && _currentList.Count > 0;
            btnXoa.Enabled = !isEditing && _currentList.Count > 0;
            btnLuu.Enabled = isEditing;
            btnHuy.Enabled = isEditing;

            // Danh sách và bộ lọc
            dgvNhaTaiTro.Enabled = !isEditing;
            pnlFilter.Enabled = !isEditing;

            errBaoLoi.Clear();
        }

        private void ClearInputs()
        {
            txtMaNhaTaiTro.Text = string.Empty;
            txtTenToChuc.Text = string.Empty;
            txtNguoiDaiDien.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;
            txtEmail.Text = string.Empty;
            if (cboGoiTaiTro.Items.Count > 0) cboGoiTaiTro.SelectedIndex = 0;
            nudGiaTriTaiTro.Value = 0;
            if (cboHinhThuc.Items.Count > 0) cboHinhThuc.SelectedIndex = 0;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
            txtDiaChi.Text = string.Empty;
            txtGhiChu.Text = string.Empty;
            errBaoLoi.Clear();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.Add);
            ClearInputs();
            txtMaNhaTaiTro.Text = _sponsorService.SuggestNextCode();
            txtTenToChuc.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhaTaiTro.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhà tài trợ cần chỉnh sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SetFormState(FormMode.Edit);
            txtTenToChuc.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhaTaiTro.CurrentRow?.DataBoundItem is not SponsorDto selected)
            {
                MessageBox.Show("Vui lòng chọn nhà tài trợ cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa/ngừng hợp tác với nhà tài trợ '{selected.TenToChuc}' (Mã: {selected.MaNhaTaiTro})?\n\nLưu ý: Nếu đối tác đã tham gia tài trợ sự kiện, hệ thống sẽ thực hiện Soft Delete (BR-20).",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var result = _sponsorService.Delete(selected.MaNhaTaiTro);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var dto = new SponsorDto
            {
                MaNhaTaiTro = txtMaNhaTaiTro.Text.Trim(),
                TenToChuc = txtTenToChuc.Text.Trim(),
                NguoiDaiDien = string.IsNullOrWhiteSpace(txtNguoiDaiDien.Text) ? null : txtNguoiDaiDien.Text.Trim(),
                SoDienThoai = string.IsNullOrWhiteSpace(txtSoDienThoai.Text) ? null : txtSoDienThoai.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                GoiTaiTro = cboGoiTaiTro.SelectedItem?.ToString() ?? "Đồng",
                TongGiaTriTaiTro = nudGiaTriTaiTro.Value,
                HinhThucTaiTro = cboHinhThuc.SelectedItem?.ToString() ?? "Tiền mặt",
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Đang hợp tác",
                DiaChi = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim(),
                GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim()
            };

            ServiceResult result;
            if (_currentMode == FormMode.Add)
            {
                result = _sponsorService.Add(dto);
            }
            else
            {
                result = _sponsorService.Update(dto);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetFormState(FormMode.View);
                LoadData();
                SelectRowByCode(dto.MaNhaTaiTro);
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
            if (dgvNhaTaiTro.CurrentRow?.DataBoundItem is SponsorDto selected)
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
            cboLocGoiTaiTro.SelectedIndex = 0;
            cboLocTrangThai.SelectedIndex = 0;
            _isLoading = false;

            LoadData();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (_currentList == null || _currentList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất file.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Tệp CSV (Excel UTF-8) (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = $"DanhSachNhaTaiTro_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Mã NTT,Tên tổ chức / Doanh nghiệp,Người đại diện,Điện thoại,Email,Gói tài trợ,Tổng giá trị (VNĐ),Hình thức,Trạng thái,Địa chỉ,Ghi chú");

                    foreach (var s in _currentList)
                    {
                        sb.AppendLine($"\"{s.MaNhaTaiTro}\",\"{EscapeCsv(s.TenToChuc)}\",\"{EscapeCsv(s.NguoiDaiDien)}\",\"{s.SoDienThoai}\",\"{s.Email}\",\"{s.GoiTaiTro}\",{s.TongGiaTriTaiTro},\"{s.HinhThucTaiTro}\",\"{s.TrangThai}\",\"{EscapeCsv(s.DiaChi)}\",\"{EscapeCsv(s.GhiChu)}\"");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất danh sách nhà tài trợ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (string.IsNullOrWhiteSpace(txtMaNhaTaiTro.Text))
            {
                errBaoLoi.SetError(txtMaNhaTaiTro, "Mã nhà tài trợ không được để trống.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtTenToChuc.Text))
            {
                errBaoLoi.SetError(txtTenToChuc, "Tên tổ chức/doanh nghiệp tài trợ không được để trống.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), emailPattern))
                {
                    errBaoLoi.SetError(txtEmail, "Địa chỉ Email không đúng định dạng.");
                    isValid = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                string phonePattern = @"^[0-9]{8,15}$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text.Trim(), phonePattern))
                {
                    errBaoLoi.SetError(txtSoDienThoai, "Số điện thoại phải từ 8 đến 15 chữ số.");
                    isValid = false;
                }
            }

            return isValid;
        }

        private void SelectRowByCode(string code)
        {
            foreach (DataGridViewRow row in dgvNhaTaiTro.Rows)
            {
                if (row.DataBoundItem is SponsorDto dto && string.Equals(dto.MaNhaTaiTro, code, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    DisplaySelectedRow(dto);
                    break;
                }
            }
        }

        private void dgvNhaTaiTro_SelectionChanged(object sender, EventArgs e)
        {
            if (_isLoading || _currentMode != FormMode.View) return;

            if (dgvNhaTaiTro.CurrentRow?.DataBoundItem is SponsorDto selected)
            {
                DisplaySelectedRow(selected);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboLocGoiTaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboLocTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void FrmSponsor_KeyDown(object sender, KeyEventArgs e)
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
    }
}
