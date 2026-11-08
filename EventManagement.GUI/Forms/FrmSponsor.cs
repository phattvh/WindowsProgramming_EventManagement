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

        public Button BtnThem => btnThem;
        public Button BtnSua => btnSua;
        public Button BtnXoa => btnXoa;
        public Button BtnLuu => btnLuu;
        public Button BtnHuy => btnHuy;
        public Button BtnLamMoi => btnLamMoi;
        public DataGridView DgvNhaTaiTro => dgvNhaTaiTro;
        public GroupBox GrbThongTin => grbThongTin;
        public Panel PnlFilter => pnlFilter;

        public FrmSponsor() : this(new SponsorService())
        {
        }

        public FrmSponsor(ISponsorService sponsorService)
        {
            InitializeComponent();
            _sponsorService = sponsorService;

            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            ApplyCustomStyles();

            // Lắng nghe sự kiện co giãn để bố cục responsive cân đối toàn màn hình
            grbThongTin.Resize += (s, e) => AdjustInputLayout();
            pnlButtons.Resize += (s, e) => AdjustButtonLayout();
            pnlFilter.Resize += (s, e) => AdjustFilterLayout();
            this.Resize += (s, e) => { AdjustInputLayout(); AdjustButtonLayout(); AdjustFilterLayout(); };
        }

        private void ApplyCustomStyles()
        {
            this.Text = "Hệ Thống Quản Lý Sự Kiện";

            StyleButton(btnThem, ButtonRole.Success);
            StyleButton(btnSua, ButtonRole.Primary);
            StyleButton(btnXoa, ButtonRole.Danger);
            StyleButton(btnLuu, ButtonRole.Primary);
            StyleButton(btnHuy, ButtonRole.Secondary);
            StyleButton(btnLamMoi, ButtonRole.Secondary);
            StyleButton(btnXuatExcel, ButtonRole.Secondary);

            StyleDataGridView(dgvNhaTaiTro);
            dgvNhaTaiTro.AutoGenerateColumns = false;

            // Điều chỉnh tỷ lệ cột: mã, gói, giá trị, SĐT, hình thức, trạng thái nhỏ lại; tên tổ chức & người đại diện to ra
            colMaNhaTaiTro.FillWeight = 50;
            colTenToChuc.FillWeight = 160;
            colNguoiDaiDien.FillWeight = 130;
            colSoDienThoai.FillWeight = 65;
            colEmail.FillWeight = 100;
            colGoiTaiTro.FillWeight = 60;
            colGiaTriTaiTro.FillWeight = 70;
            colHinhThucTaiTro.FillWeight = 60;
            colTrangThai.FillWeight = 60;

            dgvNhaTaiTro.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhaTaiTro.ScrollBars = ScrollBars.Both;

            foreach (DataGridViewColumn col in dgvNhaTaiTro.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            AdjustInputLayout();
            AdjustButtonLayout();
            AdjustFilterLayout();
        }

        /// <summary>
        /// Tự động cân đối thông tin chi tiết nhà tài trợ thành 3 cột đều nhau trên toàn màn hình (Responsive 1920x1080)
        /// </summary>
        private void AdjustInputLayout()
        {
            if (grbThongTin == null) return;
            int clientW = grbThongTin.ClientSize.Width;
            int margin = 20;
            int gap = 25;
            int colWidth = (clientW - (margin * 2) - (gap * 2)) / 3;
            if (colWidth < 220) colWidth = 220;

            int col1X = margin;
            int col2X = margin + colWidth + gap;
            int col3X = margin + (colWidth * 2) + (gap * 2);

            int labelW = 115;
            int inputW = colWidth - labelW - 10;

            // Cột 1: Thông tin tổ chức
            // Row 1: Mã NTT
            lblMaNhaTaiTro.Location = new Point(col1X, 31);
            txtMaNhaTaiTro.Location = new Point(col1X + labelW, 28);
            txtMaNhaTaiTro.Size = new Size(inputW, 23);

            // Row 2: Tên tổ chức (*)
            lblTenToChuc.Location = new Point(col1X, 65);
            txtTenToChuc.Location = new Point(col1X + labelW, 62);
            txtTenToChuc.Size = new Size(inputW, 23);

            // Row 3: Người đại diện
            lblNguoiDaiDien.Location = new Point(col1X, 99);
            txtNguoiDaiDien.Location = new Point(col1X + labelW, 96);
            txtNguoiDaiDien.Size = new Size(inputW, 23);

            // Row 4: Địa chỉ
            lblDiaChi.Location = new Point(col1X, 133);
            txtDiaChi.Location = new Point(col1X + labelW, 130);
            txtDiaChi.Size = new Size(inputW, 23);

            // Cột 2: Liên hệ & Trạng thái
            // Row 1: Số điện thoại
            lblSoDienThoai.Location = new Point(col2X, 31);
            txtSoDienThoai.Location = new Point(col2X + labelW, 28);
            txtSoDienThoai.Size = new Size(inputW, 23);

            // Row 2: Email
            lblEmail.Location = new Point(col2X, 65);
            txtEmail.Location = new Point(col2X + labelW, 62);
            txtEmail.Size = new Size(inputW, 23);

            // Row 3: Trạng thái
            lblTrangThai.Location = new Point(col2X, 99);
            cboTrangThai.Location = new Point(col2X + labelW, 96);
            cboTrangThai.Size = new Size(inputW, 23);

            // Row 4: Ghi chú
            lblGhiChu.Location = new Point(col2X, 133);
            txtGhiChu.Location = new Point(col2X + labelW, 130);
            txtGhiChu.Size = new Size(inputW, 23);

            // Cột 3: Gói & Giá trị tài trợ
            // Row 1: Gói tài trợ
            lblGoiTaiTro.Location = new Point(col3X, 31);
            cboGoiTaiTro.Location = new Point(col3X + labelW, 28);
            cboGoiTaiTro.Size = new Size(inputW, 23);

            // Row 2: Tổng giá trị
            lblGiaTriTaiTro.Location = new Point(col3X, 65);
            nudGiaTriTaiTro.Location = new Point(col3X + labelW, 62);
            nudGiaTriTaiTro.Size = new Size(inputW, 23);

            // Row 3: Hình thức
            lblHinhThuc.Location = new Point(col3X, 99);
            cboHinhThuc.Location = new Point(col3X + labelW, 96);
            cboHinhThuc.Size = new Size(inputW, 23);
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
            int filterRight = btnLamMoi.Left - 15;
            cboLocTrangThai.Location = new Point(filterRight - cboLocTrangThai.Width, 11);
            lblLocTrangThai.Location = new Point(cboLocTrangThai.Left - lblLocTrangThai.Width - 8, 14);

            int goiRight = lblLocTrangThai.Left - 15;
            cboLocGoiTaiTro.Location = new Point(goiRight - cboLocGoiTaiTro.Width, 11);
            lblLocGoi.Location = new Point(cboLocGoiTaiTro.Left - lblLocGoi.Width - 8, 14);

            int searchWidth = lblLocGoi.Left - 15 - txtTimKiem.Left;
            if (searchWidth > 150)
            {
                txtTimKiem.Width = searchWidth;
            }
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

        /// <summary>
        /// Nút Lưu và Hủy chỉ hiển thị khi Thêm hoặc Sửa
        /// </summary>
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
            btnLuu.Visible = isEditing;
            btnHuy.Visible = isEditing;
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
                $"Bạn có chắc chắn muốn xóa/ngừng hợp tác với nhà tài trợ '{selected.TenToChuc}' (Mã: {selected.MaNhaTaiTro})?",
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
                MessageBox.Show(result.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Mã NTT,Tên tổ chức,Người đại diện,Số ĐT,Email,Gói tài trợ,Giá trị,Hình thức,Trạng thái,Địa chỉ,Ghi chú");

                    foreach (var s in _currentList)
                    {
                        sb.AppendLine($"\"{s.MaNhaTaiTro}\",\"{s.TenToChuc}\",\"{s.NguoiDaiDien}\",\"{s.SoDienThoai}\",\"{s.Email}\",\"{s.GoiTaiTro}\",\"{s.TongGiaTriTaiTro}\",\"{s.HinhThucTaiTro}\",\"{s.TrangThai}\",\"{s.DiaChi}\",\"{s.GhiChu}\"");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất file Excel (CSV) thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SelectRowByCode(string code)
        {
            for (int i = 0; i < dgvNhaTaiTro.Rows.Count; i++)
            {
                if (dgvNhaTaiTro.Rows[i].DataBoundItem is SponsorDto item &&
                    item.MaNhaTaiTro.Equals(code, StringComparison.OrdinalIgnoreCase))
                {
                    dgvNhaTaiTro.Rows[i].Selected = true;
                    DisplaySelectedRow(item);
                    break;
                }
            }
        }

        private bool ValidateInputs()
        {
            bool isValid = true;
            errBaoLoi.Clear();

            if (string.IsNullOrWhiteSpace(txtMaNhaTaiTro.Text))
            {
                errBaoLoi.SetError(txtMaNhaTaiTro, "Mã nhà tài trợ không được để trống.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtTenToChuc.Text))
            {
                errBaoLoi.SetError(txtTenToChuc, "Tên tổ chức không được để trống.");
                isValid = false;
            }

            if (nudGiaTriTaiTro.Value < 0)
            {
                errBaoLoi.SetError(nudGiaTriTaiTro, "Giá trị tài trợ không được âm.");
                isValid = false;
            }

            return isValid;
        }

        private void dgvNhaTaiTro_SelectionChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View && dgvNhaTaiTro.CurrentRow?.DataBoundItem is SponsorDto selected)
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
                if (e.KeyCode == Keys.F2) { btnThem.PerformClick(); e.Handled = true; }
                else if (e.KeyCode == Keys.F3) { btnSua.PerformClick(); e.Handled = true; }
                else if (e.KeyCode == Keys.Delete && !(ActiveControl is TextBoxBase)) { btnXoa.PerformClick(); e.Handled = true; }
                else if (e.KeyCode == Keys.F5) { btnLamMoi.PerformClick(); e.Handled = true; }
            }
            else
            {
                if (e.Control && e.KeyCode == Keys.S) { btnLuu.PerformClick(); e.Handled = true; }
                else if (e.KeyCode == Keys.Escape) { btnHuy.PerformClick(); e.Handled = true; }
            }
        }
    }
}
