using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
using EventManagement.Core.Session;
using EventManagement.GUI.Common;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình F03 – Quản lý tài khoản người dùng & Mở khóa Brute-force (Task T1-12 & T1-13)
    /// Phụ trách: Thành viên 1 (Kiến trúc, CSDL & Bảo mật)
    /// Tuân thủ: Mẫu bố cục 2, chuẩn 3 trạng thái FormMode (Xem/Thêm/Sửa), ErrorProvider
    /// </summary>
    public partial class FrmUserManagement : FrmBase
    {
        private readonly IUserService _userService;
        private FormMode _currentMode = FormMode.View;
        private List<UserDto> _currentList = new();
        private UserDto? _selectedUser;

        public Button BtnThem => btnThem;
        public Button BtnSua => btnSua;
        public Button BtnXoa => btnXoa;
        public Button BtnLuu => btnLuu;
        public Button BtnHuy => btnHuy;
        public Button BtnMoKhoa => btnMoKhoa;

        public FrmUserManagement() : this(new UserService())
        {
        }

        public FrmUserManagement(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.WindowState = FormWindowState.Maximized;
            dgvUsers.AutoGenerateColumns = false;

            foreach (DataGridViewColumn col in dgvUsers.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            colMaNguoiDung.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSoDienThoai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colNhomQuyen.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTrangThai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSoLanSai.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colKhoaDenKhi.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grpDetail.Resize += (s, e) => LayoutDetailGroup();
            LayoutDetailGroup();
        }

        private async void FrmUserManagement_Load(object sender, EventArgs e)
        {
            if (CurrentSession.IsLoggedIn && !PermissionHelper.CheckAccessAndNotify(this, "UC03", "Quản lý người dùng"))
            {
                this.Close();
                return;
            }

            btnThem.Tag = "UC03";
            btnSua.Tag = "UC03";
            btnXoa.Tag = "UC03";
            btnMoKhoa.Tag = "UC03";
            PermissionHelper.ApplyAuthorization(pnlButtons, hideInsteadOfDisable: false);

            cboLocNhomQuyen.SelectedIndex = 0; // "Tất cả"
            cboLocTrangThai.SelectedIndex = 0; // "Tất cả"
            cboNhomQuyen.SelectedIndex = 2;    // "STAFF"
            cboTrangThai.SelectedIndex = 0;    // "Hoạt động"

            LayoutDetailGroup();
            await LoadDataAsync();
            SetFormState(FormMode.View);
        }

        public void LayoutDetailGroup()
        {
            int totalW = grpDetail.ClientSize.Width - 40;
            if (totalW < 300) return;
            int colW = totalW / 3;

            // Cột 1
            int c1X = 20;
            lblMaNguoiDung.Location = new Point(c1X, 30);
            txtMaNguoiDung.Location = new Point(c1X + 115, 26);
            txtMaNguoiDung.Width = Math.Max(100, colW - 130);

            lblHoTen.Location = new Point(c1X, 70);
            txtHoTen.Location = new Point(c1X + 115, 66);
            txtHoTen.Width = Math.Max(100, colW - 130);

            lblNhomQuyen.Location = new Point(c1X, 110);
            cboNhomQuyen.Location = new Point(c1X + 115, 106);
            cboNhomQuyen.Width = Math.Max(100, colW - 130);

            // Cột 2
            int c2X = c1X + colW + 15;
            lblTenDangNhap.Location = new Point(c2X, 30);
            txtTenDangNhap.Location = new Point(c2X + 115, 26);
            txtTenDangNhap.Width = Math.Max(100, colW - 130);

            lblEmail.Location = new Point(c2X, 70);
            txtEmail.Location = new Point(c2X + 115, 66);
            txtEmail.Width = Math.Max(100, colW - 130);

            lblTrangThai.Location = new Point(c2X, 110);
            cboTrangThai.Location = new Point(c2X + 115, 106);
            cboTrangThai.Width = Math.Max(100, colW - 130);

            // Cột 3
            int c3X = c2X + colW + 15;
            lblMatKhau.Location = new Point(c3X, 30);
            txtMatKhau.Location = new Point(c3X + 90, 26);
            txtMatKhau.Width = Math.Max(100, colW - 105);

            lblSoDienThoai.Location = new Point(c3X, 70);
            txtSoDienThoai.Location = new Point(c3X + 90, 66);
            txtSoDienThoai.Width = Math.Max(100, colW - 105);

            lblThongTinKhoa.Location = new Point(20, 150);
        }

        public async Task LoadDataAsync()
        {
            string? keyword = txtTimKiem.Text.Trim();
            string? roleFilter = cboLocNhomQuyen.SelectedItem?.ToString();
            string? statusFilter = cboLocTrangThai.SelectedItem?.ToString();

            try
            {
                _currentList = await _userService.GetAllAsync(keyword, roleFilter, statusFilter);
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = _currentList;

                if (_currentList.Any())
                {
                    dgvUsers.Rows[0].Selected = true;
                    _selectedUser = _currentList[0];
                    PopulateDetail(_selectedUser);
                }
                else
                {
                    _selectedUser = null;
                    ClearDetail();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách tài khoản người dùng. Vui lòng thử lại sau.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void SetFormState(FormMode mode)
        {
            _currentMode = mode;
            errorProvider.Clear();

            switch (mode)
            {
                case FormMode.View:
                    // Các ô nhập liệu bị khóa ở chế độ Xem
                    txtMaNguoiDung.Enabled = false;
                    txtTenDangNhap.Enabled = false;
                    txtMatKhau.Enabled = false;
                    txtHoTen.Enabled = false;
                    txtEmail.Enabled = false;
                    txtSoDienThoai.Enabled = false;
                    cboNhomQuyen.Enabled = false;
                    cboTrangThai.Enabled = false;

                    btnThem.Visible = true;
                    btnSua.Visible = true;
                    btnXoa.Visible = true;
                    btnMoKhoa.Visible = true;
                    btnLuu.Visible = false;
                    btnHuy.Visible = false;

                    btnThem.Enabled = true;
                    btnSua.Enabled = _selectedUser != null;
                    btnXoa.Enabled = _selectedUser != null;
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false;
                    dgvUsers.Enabled = true;

                    UpdateUnlockButtonState();
                    break;

                case FormMode.Add:
                    ClearDetail();
                    txtMaNguoiDung.Text = "(Tự động sinh)";
                    txtMaNguoiDung.Enabled = false;
                    txtTenDangNhap.Enabled = true;
                    txtMatKhau.Enabled = true;
                    txtMatKhau.PlaceholderText = "Tối thiểu 6 ký tự";
                    txtHoTen.Enabled = true;
                    txtEmail.Enabled = true;
                    txtSoDienThoai.Enabled = true;
                    cboNhomQuyen.Enabled = true;
                    cboNhomQuyen.SelectedItem = "STAFF";
                    cboTrangThai.Enabled = true;
                    cboTrangThai.SelectedItem = "Hoạt động";

                    btnThem.Visible = false;
                    btnSua.Visible = false;
                    btnXoa.Visible = false;
                    btnMoKhoa.Visible = false;
                    btnLuu.Visible = true;
                    btnHuy.Visible = true;

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    btnMoKhoa.Enabled = false;
                    dgvUsers.Enabled = false;

                    btnLuu.Location = new Point(0, 7);
                    btnHuy.Location = new Point(120, 7);

                    txtTenDangNhap.Focus();
                    break;

                case FormMode.Edit:
                    txtMaNguoiDung.Enabled = false;
                    txtTenDangNhap.Enabled = false; // Không cho sửa Username
                    txtMatKhau.Enabled = true;
                    txtMatKhau.PlaceholderText = "(Để trống nếu không đổi)";
                    txtMatKhau.Text = string.Empty;
                    txtHoTen.Enabled = true;
                    txtEmail.Enabled = true;
                    txtSoDienThoai.Enabled = true;
                    cboNhomQuyen.Enabled = true;
                    cboTrangThai.Enabled = true;

                    btnThem.Visible = false;
                    btnSua.Visible = false;
                    btnXoa.Visible = false;
                    btnMoKhoa.Visible = false;
                    btnLuu.Visible = true;
                    btnHuy.Visible = true;

                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    btnMoKhoa.Enabled = false;
                    dgvUsers.Enabled = false;

                    btnLuu.Location = new Point(0, 7);
                    btnHuy.Location = new Point(120, 7);

                    txtHoTen.Focus();
                    break;
            }
        }

        private void PopulateDetail(UserDto? user)
        {
            if (user == null)
            {
                ClearDetail();
                return;
            }

            txtMaNguoiDung.Text = user.MaNguoiDung;
            txtTenDangNhap.Text = user.TenDangNhap;
            txtMatKhau.Text = string.Empty;
            txtMatKhau.PlaceholderText = "(Để trống nếu giữ nguyên)";
            txtHoTen.Text = user.HoTen;
            txtEmail.Text = user.Email;
            txtSoDienThoai.Text = user.SoDienThoai ?? string.Empty;

            cboNhomQuyen.SelectedItem = user.MaNhomQuyen;
            cboTrangThai.SelectedItem = user.TrangThai;

            // Hiển thị cảnh báo nếu tài khoản đang bị khóa
            if (user.KhoaDenKhi.HasValue && user.KhoaDenKhi.Value > DateTime.Now)
            {
                int remainingSeconds = (int)Math.Ceiling((user.KhoaDenKhi.Value - DateTime.Now).TotalSeconds);
                lblThongTinKhoa.Text = $"⚠️ Tài khoản đang bị KHÓA TẠM THỜI (còn {remainingSeconds} giây) do nhập sai {user.SoLanDangNhapSai} lần liên tiếp!";
                lblThongTinKhoa.ForeColor = Color.Red;
            }
            else if (user.TrangThai == "Khóa vĩnh viễn")
            {
                lblThongTinKhoa.Text = "⚠️ Tài khoản đang bị KHÓA VĨNH VIỄN bởi Quản trị viên!";
                lblThongTinKhoa.ForeColor = Color.DarkRed;
            }
            else if (user.SoLanDangNhapSai > 0)
            {
                lblThongTinKhoa.Text = $"ℹ️ Tài khoản đã nhập sai mật khẩu {user.SoLanDangNhapSai}/5 lần.";
                lblThongTinKhoa.ForeColor = Color.FromArgb(217, 119, 6);
            }
            else
            {
                lblThongTinKhoa.Text = "✓ Tài khoản hoạt động bình thường.";
                lblThongTinKhoa.ForeColor = Color.Green;
            }

            UpdateUnlockButtonState();
        }

        private void ClearDetail()
        {
            txtMaNguoiDung.Text = string.Empty;
            txtTenDangNhap.Text = string.Empty;
            txtMatKhau.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;
            lblThongTinKhoa.Text = string.Empty;
        }

        private void UpdateUnlockButtonState()
        {
            if (_selectedUser != null && (_selectedUser.IsLocked || _selectedUser.SoLanDangNhapSai > 0))
            {
                btnMoKhoa.Enabled = true;
                btnMoKhoa.Text = "🔓 Mở khóa tài khoản";
            }
            else
            {
                btnMoKhoa.Enabled = false;
                btnMoKhoa.Text = "Tài khoản bình thường";
            }
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (_currentMode != FormMode.View) return;

            if (dgvUsers.SelectedRows.Count > 0)
            {
                _selectedUser = dgvUsers.SelectedRows[0].DataBoundItem as UserDto;
                PopulateDetail(_selectedUser);
                btnSua.Enabled = _selectedUser != null;
                btnXoa.Enabled = _selectedUser != null;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.Add);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null) return;
            SetFormState(FormMode.Edit);
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa tài khoản '{_selectedUser.TenDangNhap}' ({_selectedUser.HoTen})?",
                "Xác nhận xóa tài khoản",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var result = await _userService.DeleteAsync(_selectedUser.MaNguoiDung);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi xóa tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            string username = txtTenDangNhap.Text.Trim();
            string fullName = txtHoTen.Text.Trim();
            string email = txtEmail.Text.Trim();
            string? phone = string.IsNullOrWhiteSpace(txtSoDienThoai.Text) ? null : txtSoDienThoai.Text.Trim();
            string password = txtMatKhau.Text;
            bool isNew = _currentMode == FormMode.Add;

            errorProvider.Clear();

            // Thẩm định tính hợp lệ của dữ liệu
            var validation = ValidateUserInput(username, fullName, email, phone, password, isNew);
            if (!validation.IsValid)
            {
                ApplyValidationError(validation.ErrorField, validation.ErrorMessage);
                return;
            }

            string role = cboNhomQuyen.SelectedItem?.ToString() ?? "STAFF";
            string status = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động";

            if (_currentMode == FormMode.Add)
            {
                var createDto = new CreateUserDto
                {
                    TenDangNhap = username,
                    MatKhau = password,
                    HoTen = fullName,
                    Email = email,
                    SoDienThoai = phone,
                    MaNhomQuyen = role,
                    TrangThai = status
                };

                var result = await _userService.CreateAsync(createDto);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    SetFormState(FormMode.View);
                }
                else
                {
                    MessageBox.Show(result.Message, "Không thể thêm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    errorProvider.SetError(txtTenDangNhap, result.Message);
                }
            }
            else if (_currentMode == FormMode.Edit && _selectedUser != null)
            {
                var updateDto = new UpdateUserDto
                {
                    HoTen = fullName,
                    Email = email,
                    SoDienThoai = phone,
                    MaNhomQuyen = role,
                    TrangThai = status,
                    MatKhauMoi = string.IsNullOrWhiteSpace(password) ? null : password
                };

                var result = await _userService.UpdateAsync(_selectedUser.MaNguoiDung, updateDto);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    SetFormState(FormMode.View);
                }
                else
                {
                    MessageBox.Show(result.Message, "Không thể cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            SetFormState(FormMode.View);
            PopulateDetail(_selectedUser);
        }

        /// <summary>
        /// Nút Mở khóa tài khoản tức thì cho Admin (Nhiệm vụ T1-13 / AD-7 / BR-19)
        /// </summary>
        private async void btnMoKhoa_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null) return;

            var confirm = MessageBox.Show(
                $"Bạn có muốn mở khóa và khôi phục trạng thái hoạt động ngay cho tài khoản '{_selectedUser.TenDangNhap}'?",
                "Mở khóa tài khoản",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var result = await _userService.UnlockUserAsync(_selectedUser.MaNguoiDung);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async void Filter_Changed(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = string.Empty;
            cboLocNhomQuyen.SelectedIndex = 0;
            cboLocTrangThai.SelectedIndex = 0;
            await LoadDataAsync();
        }

        private async void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await LoadDataAsync();
            }
        }

        private void ApplyValidationError(string? field, string? message)
        {
            switch (field)
            {
                case nameof(txtTenDangNhap):
                    errorProvider.SetError(txtTenDangNhap, message);
                    txtTenDangNhap.Focus();
                    break;
                case nameof(txtHoTen):
                    errorProvider.SetError(txtHoTen, message);
                    txtHoTen.Focus();
                    break;
                case nameof(txtEmail):
                    errorProvider.SetError(txtEmail, message);
                    txtEmail.Focus();
                    break;
                case nameof(txtSoDienThoai):
                    errorProvider.SetError(txtSoDienThoai, message);
                    txtSoDienThoai.Focus();
                    break;
                case nameof(txtMatKhau):
                    errorProvider.SetError(txtMatKhau, message);
                    txtMatKhau.Focus();
                    break;
            }
        }

        /// <summary>
        /// Logic thẩm định tính hợp lệ dữ liệu người dùng Form F03
        /// Hàm public static phục vụ viết Unit Test tự động không cần môi trường GUI
        /// </summary>
        public static (bool IsValid, string? ErrorField, string? ErrorMessage) ValidateUserInput(
            string username, 
            string fullName, 
            string email, 
            string? phone, 
            string? password, 
            bool isNewUser)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return (false, nameof(txtTenDangNhap), "Tên đăng nhập không được để trống!");
            }

            if (username.Length < 3)
            {
                return (false, nameof(txtTenDangNhap), "Tên đăng nhập phải có ít nhất 3 ký tự!");
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                return (false, nameof(txtHoTen), "Họ và tên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, nameof(txtEmail), "Email không được để trống!");
            }

            // Kiểm tra định dạng Email chuẩn
            string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase))
            {
                return (false, nameof(txtEmail), "Định dạng email không hợp lệ (ví dụ: user@example.com)!");
            }

            // Kiểm tra Số điện thoại nếu có nhập
            if (!string.IsNullOrWhiteSpace(phone))
            {
                string cleanPhone = phone.Trim();
                if (!Regex.IsMatch(cleanPhone, @"^(0|\+84)[0-9]{9,10}$"))
                {
                    return (false, nameof(txtSoDienThoai), "Số điện thoại không hợp lệ (cần 10 số, bắt đầu bằng 0 hoặc +84)!");
                }
            }

            // Kiểm tra Mật khẩu khi tạo mới
            if (isNewUser)
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    return (false, nameof(txtMatKhau), "Mật khẩu không được để trống khi tạo tài khoản mới!");
                }
                if (password.Length < 6)
                {
                    return (false, nameof(txtMatKhau), "Mật khẩu phải có độ dài tối thiểu 6 ký tự!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(password) && password.Length < 6)
            {
                return (false, nameof(txtMatKhau), "Mật khẩu mới phải có độ dài tối thiểu 6 ký tự!");
            }

            return (true, null, null);
        }
    }
}