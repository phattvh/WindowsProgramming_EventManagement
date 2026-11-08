using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Màn hình F09 – Quản lý diễn giả / khách mời
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// Tuân thủ: Mẫu bố cục 2, 3 trạng thái màn hình (Xem - Thêm - Sửa), BR-06 & BR-20.
    /// </summary>
    public partial class FrmSpeaker : FrmBase
    {
        private readonly ISpeakerService _speakerService;
        private FormMode _currentMode = FormMode.View;
        private List<SpeakerDto> _currentList = new();

        public Button BtnThem => btnThem;
        public Button BtnSua => btnSua;
        public Button BtnXoa => btnXoa;
        public Button BtnLuu => btnLuu;
        public Button BtnHuy => btnHuy;
        public Button BtnLamMoi => btnLamMoi;
        public DataGridView DgvDienGia => dgvDienGia;
        public GroupBox GrbThongTin => grbThongTin;
        public Panel PnlFilter => pnlFilter;

        public FrmSpeaker() : this(new SpeakerService())
        {
        }

        public FrmSpeaker(ISpeakerService speakerService)
        {
            InitializeComponent();
            _speakerService = speakerService;

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
            StyleButton(btnXuatExcel, ButtonRole.Secondary);
            StyleButton(btnLuu, ButtonRole.Primary);
            StyleButton(btnHuy, ButtonRole.Secondary);
            StyleButton(btnLamMoi, ButtonRole.Secondary);
            StyleButton(btnChonAnh, ButtonRole.Secondary);

            StyleDataGridView(dgvDienGia);
            dgvDienGia.AutoGenerateColumns = false;
            dgvDienGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDienGia.ScrollBars = ScrollBars.Both;

            foreach (DataGridViewColumn col in dgvDienGia.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            cboLocTrangThai.SelectedIndex = 0; // "Tất cả"

            AdjustInputLayout();
            AdjustButtonLayout();
            AdjustFilterLayout();
        }

        /// <summary>
        /// Tự động cân đối hồ sơ chi tiết 2 cột dàn đều toàn màn hình và avatar cố định góc phải
        /// </summary>
        private void AdjustInputLayout()
        {
            if (grbThongTin == null) return;
            int clientW = grbThongTin.ClientSize.Width;

            // Ảnh đại diện cố định bên phải
            int avatarWidth = 140;
            int avatarHeight = 165;
            picAvatar.Size = new Size(avatarWidth, avatarHeight);
            picAvatar.Location = new Point(clientW - avatarWidth - 20, 25);

            int contentWidth = picAvatar.Left - 40;
            if (contentWidth < 400) contentWidth = 400;

            // Chia đều 2 cột thông tin chi tiết
            int colWidth = (contentWidth - 20) / 2;
            int col1X = 20;
            int col2X = 20 + colWidth + 20;
            int labelW = 120;
            int inputW = colWidth - labelW - 10;

            // Dòng 1: Mã diễn giả | Họ tên
            lblMaDienGia.Location = new Point(col1X, 31);
            txtMaDienGia.Location = new Point(col1X + labelW, 28);
            txtMaDienGia.Size = new Size(inputW, 23);

            lblHoTen.Location = new Point(col2X, 31);
            txtHoTen.Location = new Point(col2X + labelW, 28);
            txtHoTen.Size = new Size(inputW, 23);

            // Dòng 2: Chức vụ | Đơn vị công tác
            lblChucVu.Location = new Point(col1X, 65);
            txtChucVu.Location = new Point(col1X + labelW, 62);
            txtChucVu.Size = new Size(inputW, 23);

            lblDonViCongTac.Location = new Point(col2X, 65);
            txtDonViCongTac.Location = new Point(col2X + labelW, 62);
            txtDonViCongTac.Size = new Size(inputW, 23);

            // Dòng 3: Số điện thoại | Email
            lblSoDienThoai.Location = new Point(col1X, 99);
            txtSoDienThoai.Location = new Point(col1X + labelW, 96);
            txtSoDienThoai.Size = new Size(inputW, 23);

            lblEmail.Location = new Point(col2X, 99);
            txtEmail.Location = new Point(col2X + labelW, 96);
            txtEmail.Size = new Size(inputW, 23);

            // Dòng 4: Trạng thái | Hình ảnh & Nút chọn ảnh
            lblTrangThai.Location = new Point(col1X, 133);
            cboTrangThai.Location = new Point(col1X + labelW, 130);
            cboTrangThai.Size = new Size(inputW, 23);

            lblHinhAnh.Location = new Point(col2X, 133);
            int btnWidth = 70;
            txtHinhAnh.Location = new Point(col2X + labelW, 130);
            txtHinhAnh.Size = new Size(inputW - btnWidth - 5, 23);
            btnChonAnh.Location = new Point(txtHinhAnh.Right + 5, 129);
            btnChonAnh.Size = new Size(btnWidth, 25);

            // Dòng 5: Tiểu sử trải rộng toàn bộ chiều rộng nội dung
            lblTieuSu.Location = new Point(col1X, 167);
            txtTieuSu.Location = new Point(col1X + labelW, 164);
            txtTieuSu.Size = new Size(contentWidth - labelW, 36);
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
            int statusRight = btnLamMoi.Left - 15;
            cboLocTrangThai.Location = new Point(statusRight - cboLocTrangThai.Width, 11);
            lblLocTrangThai.Location = new Point(cboLocTrangThai.Left - lblLocTrangThai.Width - 8, 14);

            int searchWidth = lblLocTrangThai.Left - 15 - txtTimKiem.Left;
            if (searchWidth > 150)
            {
                txtTimKiem.Width = searchWidth;
            }
        }

        private void FrmSpeaker_Load(object sender, EventArgs e)
        {
            LoadData();
            SetFormState(FormMode.View);
        }

        private void LoadData()
        {
            string? keyword = txtTimKiem.Text.Trim();
            string? status = cboLocTrangThai.SelectedItem?.ToString();

            var result = _speakerService.GetAll(keyword, status);
            if (result.IsSuccess && result.Data != null)
            {
                _currentList = result.Data;
                dgvDienGia.DataSource = null;
                dgvDienGia.DataSource = _currentList;

                if (_currentList.Count > 0)
                {
                    dgvDienGia.Rows[0].Selected = true;
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

        private void DisplaySelectedRow(SpeakerDto dto)
        {
            if (_currentMode != FormMode.View) return;

            txtMaDienGia.Text = dto.MaDienGia;
            txtHoTen.Text = dto.HoTen;
            txtChucVu.Text = dto.ChucVu ?? string.Empty;
            txtDonViCongTac.Text = dto.DonViCongTac ?? string.Empty;
            txtSoDienThoai.Text = dto.SoDienThoai ?? string.Empty;
            txtEmail.Text = dto.Email ?? string.Empty;
            cboTrangThai.SelectedItem = dto.TrangThai;
            txtHinhAnh.Text = dto.HinhAnh ?? string.Empty;
            txtTieuSu.Text = dto.TieuSu ?? string.Empty;

            LoadAvatar(dto.HinhAnh);
            errBaoLoi.Clear();
        }

        private void LoadAvatar(string? fileName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    string appDir = AppDomain.CurrentDomain.BaseDirectory;
                    string fullPath = Path.Combine(appDir, "Images", "Speakers", fileName);

                    if (File.Exists(fullPath))
                    {
                        picAvatar.Image = Image.FromFile(fullPath);
                        return;
                    }

                    // Thử tìm theo đường dẫn tương đối từ workspace
                    string wsDir = Path.GetFullPath(Path.Combine(appDir, "..", "..", "..", "Images", "Speakers", fileName));
                    if (File.Exists(wsDir))
                    {
                        picAvatar.Image = Image.FromFile(wsDir);
                        return;
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi đọc ảnh để không ngắt trải nghiệm người dùng
            }

            picAvatar.Image = null;
        }

        private void ClearInputs()
        {
            txtMaDienGia.Clear();
            txtHoTen.Clear();
            txtChucVu.Clear();
            txtDonViCongTac.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            cboTrangThai.SelectedItem = "Hoạt động";
            txtHinhAnh.Clear();
            txtTieuSu.Clear();
            picAvatar.Image = null;
            errBaoLoi.Clear();
        }

        /// <summary>
        /// Bộ máy quản lý 3 trạng thái màn hình (Xem - Thêm - Sửa) theo chuẩn mục 5 docs/08
        /// Nút Lưu và Hủy chỉ hiển thị khi Thêm hoặc Sửa
        /// </summary>
        private void SetFormState(FormMode mode)
        {
            _currentMode = mode;
            errBaoLoi.Clear();

            switch (mode)
            {
                case FormMode.View:
                    txtMaDienGia.ReadOnly = true;
                    txtHoTen.ReadOnly = true;
                    txtChucVu.ReadOnly = true;
                    txtDonViCongTac.ReadOnly = true;
                    txtSoDienThoai.ReadOnly = true;
                    txtEmail.ReadOnly = true;
                    cboTrangThai.Enabled = false;
                    txtHinhAnh.ReadOnly = true;
                    btnChonAnh.Enabled = false;
                    txtTieuSu.ReadOnly = true;

                    btnThem.Enabled = true;
                    bool hasRows = dgvDienGia.CurrentRow != null;
                    btnSua.Enabled = hasRows;
                    btnXoa.Enabled = hasRows;
                    btnXuatExcel.Enabled = _currentList.Count > 0;
                    btnLuu.Visible = false;
                    btnHuy.Visible = false;

                    dgvDienGia.Enabled = true;
                    pnlFilter.Enabled = true;

                    if (dgvDienGia.CurrentRow != null && dgvDienGia.CurrentRow.DataBoundItem is SpeakerDto selected)
                    {
                        DisplaySelectedRow(selected);
                    }
                    break;

                case FormMode.Add:
                    txtMaDienGia.ReadOnly = false;
                    txtHoTen.ReadOnly = false;
                    txtChucVu.ReadOnly = false;
                    txtDonViCongTac.ReadOnly = false;
                    txtSoDienThoai.ReadOnly = false;
                    txtEmail.ReadOnly = false;
                    cboTrangThai.Enabled = true;
                    txtHinhAnh.ReadOnly = false;
                    btnChonAnh.Enabled = true;
                    txtTieuSu.ReadOnly = false;

                    ClearInputs();
                    txtMaDienGia.Text = SuggestNextCode();
                    cboTrangThai.SelectedItem = "Hoạt động";

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnXuatExcel.Enabled = false;
                    btnLuu.Visible = true;
                    btnHuy.Visible = true;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvDienGia.Enabled = false;
                    pnlFilter.Enabled = false;
                    txtHoTen.Focus();
                    break;

                case FormMode.Edit:
                    txtMaDienGia.ReadOnly = true; // Khóa chính tuyệt đối không cho sửa
                    txtHoTen.ReadOnly = false;
                    txtChucVu.ReadOnly = false;
                    txtDonViCongTac.ReadOnly = false;
                    txtSoDienThoai.ReadOnly = false;
                    txtEmail.ReadOnly = false;
                    cboTrangThai.Enabled = true;
                    txtHinhAnh.ReadOnly = false;
                    btnChonAnh.Enabled = true;
                    txtTieuSu.ReadOnly = false;

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnXuatExcel.Enabled = false;
                    btnLuu.Visible = true;
                    btnHuy.Visible = true;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;

                    dgvDienGia.Enabled = false;
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
                if (item.MaDienGia.StartsWith("DG", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(item.MaDienGia[2..], out int num))
                {
                    if (num > maxId) maxId = num;
                }
            }
            return $"DG{(maxId + 1):D3}";
        }

        private void dgvDienGia_SelectionChanged(object sender, EventArgs e)
        {
            if (_currentMode == FormMode.View && dgvDienGia.CurrentRow != null)
            {
                if (dgvDienGia.CurrentRow.DataBoundItem is SpeakerDto item)
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
            if (dgvDienGia.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn diễn giả cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetFormState(FormMode.Edit);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDienGia.CurrentRow == null || dgvDienGia.CurrentRow.DataBoundItem is not SpeakerDto selected)
            {
                MessageBox.Show("Vui lòng chọn diễn giả cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa hồ sơ diễn giả '{selected.HoTen}' (Mã: {selected.MaDienGia})?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            var result = _speakerService.Delete(selected.MaDienGia);
            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);
            }
            else
            {
                // Xử lý thông báo diễn giả đã tham gia tiết mục lịch trình
                if (result.ErrorCode == "BR-20")
                {
                    var opt = MessageBox.Show(
                        $"Diễn giả này đã tham gia vào các tiết mục trong chương trình, không thể xóa trực tiếp.\n\nBạn có muốn chuyển trạng thái diễn giả sang 'Ngừng hợp tác'?",
                        "Cảnh báo",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (opt == DialogResult.Yes)
                    {
                        var softResult = _speakerService.SoftDelete(selected.MaDienGia);
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

            var dto = new SpeakerDto
            {
                MaDienGia = txtMaDienGia.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                ChucVu = string.IsNullOrWhiteSpace(txtChucVu.Text) ? null : txtChucVu.Text.Trim(),
                DonViCongTac = string.IsNullOrWhiteSpace(txtDonViCongTac.Text) ? null : txtDonViCongTac.Text.Trim(),
                SoDienThoai = string.IsNullOrWhiteSpace(txtSoDienThoai.Text) ? null : txtSoDienThoai.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động",
                HinhAnh = string.IsNullOrWhiteSpace(txtHinhAnh.Text) ? null : txtHinhAnh.Text.Trim(),
                TieuSu = string.IsNullOrWhiteSpace(txtTieuSu.Text) ? null : txtTieuSu.Text.Trim()
            };

            ServiceResult result;
            if (_currentMode == FormMode.Add)
            {
                result = _speakerService.Add(dto);
            }
            else
            {
                result = _speakerService.Update(dto);
            }

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetFormState(FormMode.View);

                // Chọn dòng vừa cập nhật
                foreach (DataGridViewRow row in dgvDienGia.Rows)
                {
                    if (row.DataBoundItem is SpeakerDto item && item.MaDienGia.Equals(dto.MaDienGia, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        dgvDienGia.CurrentCell = row.Cells[0];
                        DisplaySelectedRow(item);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmSpeaker_KeyDown(object sender, KeyEventArgs e)
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

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Chọn ảnh chân dung diễn giả",
                Filter = "Tệp hình ảnh (*.png;*.jpg;*.jpeg;*.webp)|*.png;*.jpg;*.jpeg;*.webp|Tất cả tệp (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtHinhAnh.Text = Path.GetFileName(ofd.FileName);
                try
                {
                    picAvatar.Image = Image.FromFile(ofd.FileName);
                }
                catch
                {
                    picAvatar.Image = null;
                }
            }
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
                Title = "Xuất danh sách diễn giả",
                Filter = "Tệp CSV (Comma-separated values)|*.csv",
                FileName = $"DanhSachDienGia_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);
                    sw.WriteLine("MaDienGia,HoTen,ChucVu,DonViCongTac,SoDienThoai,Email,SoTietMucThamGia,TrangThai");
                    foreach (var s in _currentList)
                    {
                        sw.WriteLine($"\"{s.MaDienGia}\",\"{s.HoTen}\",\"{s.ChucVu}\",\"{s.DonViCongTac}\",\"{s.SoDienThoai}\",\"{s.Email}\",{s.SoTietMucThamGia},\"{s.TrangThai}\"");
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

            if (string.IsNullOrWhiteSpace(txtMaDienGia.Text))
            {
                errBaoLoi.SetError(txtMaDienGia, "Mã diễn giả không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errBaoLoi.SetError(txtHoTen, "Họ và tên diễn giả không được để trống!");
                isValid = false;
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