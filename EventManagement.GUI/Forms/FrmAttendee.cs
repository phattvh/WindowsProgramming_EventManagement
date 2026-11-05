using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Entities;
using EventManagement.GUI.Common;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Form F10: Quản lý hồ sơ người tham gia sự kiện (Áp dụng Layout Mẫu 2)
    /// Hỗ trợ 3 trạng thái: Xem (View), Thêm (Add), Sửa (Edit) và bắt lỗi qua ErrorProvider
    /// </summary>
    public class FrmAttendee : FrmBase
    {
        private readonly AttendeeService _attendeeService;
        private List<NguoiThamGia> _attendees;
        private FormMode _currentMode = FormMode.View;

        // UI Controls
        private Panel _pnlHeader = null!;
        private Label _lblTitle = null!;
        private Panel _pnlSearch = null!;
        private TextBox _txtSearch = null!;
        private ComboBox _cboGenderFilter = null!;
        private Button _btnSearch = null!;
        private Button _btnResetSearch = null!;
        private DataGridView _dgvAttendees = null!;
        private GroupBox _grpDetails = null!;
        private TextBox _txtMa = null!;
        private TextBox _txtHoTen = null!;
        private ComboBox _cboGioiTinh = null!;
        private TextBox _txtEmail = null!;
        private TextBox _txtSoDienThoai = null!;
        private TextBox _txtDonVi = null!;
        private Panel _pnlButtons = null!;
        private Button _btnAdd = null!;
        private Button _btnEdit = null!;
        private Button _btnDelete = null!;
        private Button _btnSave = null!;
        private Button _btnCancel = null!;
        private ErrorProvider _errorProvider = null!;

        public FrmAttendee()
        {
            _attendeeService = new AttendeeService();
            _attendees = GetInitialSeedData();

            InitializeComponent();
            ApplyMode(FormMode.View);
            LoadDataToGrid(_attendees);
        }

        private void InitializeComponent()
        {
            this.Text = "F10 - Quản lý Người tham gia Sự kiện";
            this.Size = new Size(1000, 680);
            this.MinimumSize = new Size(900, 600);

            _errorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            // 1. Header Panel
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = ColorPrimary,
                Padding = new Padding(20, 0, 0, 0)
            };
            _lblTitle = new Label
            {
                Text = "QUẢN LÝ HỒ SƠ NGƯỜI THAM GIA (F10)",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _pnlHeader.Controls.Add(_lblTitle);

            // 2. Search Panel
            _pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = ColorSurface,
                Padding = new Padding(15, 10, 15, 10)
            };

            var lblSearch = new Label { Text = "Tìm kiếm:", AutoSize = true, Location = new Point(15, 15) };
            _txtSearch = new TextBox { Location = new Point(80, 12), Width = 250, PlaceholderText = "Nhập mã, họ tên, email, sđt..." };
            _txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) DoSearch(); };

            var lblFilter = new Label { Text = "Giới tính:", AutoSize = true, Location = new Point(350, 15) };
            _cboGenderFilter = new ComboBox { Location = new Point(415, 12), Width = 110, DropDownStyle = ComboBoxStyle.DropDownList };
            _cboGenderFilter.Items.AddRange(new object[] { "Tất cả", "Nam", "Nữ", "Khác" });
            _cboGenderFilter.SelectedIndex = 0;
            _cboGenderFilter.SelectedIndexChanged += (s, e) => DoSearch();

            _btnSearch = CreateStyledButton("Tìm kiếm", ColorPrimaryLight, Color.White, 85, 28);
            _btnSearch.Location = new Point(540, 11);
            _btnSearch.Click += (s, e) => DoSearch();

            _btnResetSearch = CreateStyledButton("Làm mới", Color.FromArgb(241, 245, 249), ColorText, 85, 28);
            _btnResetSearch.Location = new Point(635, 11);
            _btnResetSearch.Click += (s, e) =>
            {
                _txtSearch.Clear();
                _cboGenderFilter.SelectedIndex = 0;
                LoadDataToGrid(_attendees);
            };

            _pnlSearch.Controls.AddRange(new Control[] { lblSearch, _txtSearch, lblFilter, _cboGenderFilter, _btnSearch, _btnResetSearch });

            // 3. DataGridView Panel
            _dgvAttendees = new DataGridView
            {
                Dock = DockStyle.Fill
            };
            StyleDataGridView(_dgvAttendees);
            SetupColumns();
            _dgvAttendees.SelectionChanged += DgvAttendees_SelectionChanged;

            // 4. Details GroupBox
            _grpDetails = new GroupBox
            {
                Text = "Thông tin chi tiết",
                Dock = DockStyle.Bottom,
                Height = 150,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(15)
            };

            var lblMa = new Label { Text = "Mã tham gia:", Location = new Point(20, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtMa = new TextBox { Location = new Point(110, 25), Width = 140, ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };

            var lblHoTen = new Label { Text = "Họ và tên (*):", Location = new Point(270, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtHoTen = new TextBox { Location = new Point(360, 25), Width = 230 };

            var lblGioiTinh = new Label { Text = "Giới tính:", Location = new Point(610, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _cboGioiTinh = new ComboBox { Location = new Point(680, 25), Width = 110, DropDownStyle = ComboBoxStyle.DropDownList };
            _cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            _cboGioiTinh.SelectedIndex = 0;

            var lblEmail = new Label { Text = "Email (*):", Location = new Point(20, 68), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtEmail = new TextBox { Location = new Point(110, 65), Width = 200 };

            var lblSdt = new Label { Text = "Điện thoại (*):", Location = new Point(330, 68), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtSoDienThoai = new TextBox { Location = new Point(420, 65), Width = 170 };

            var lblDonVi = new Label { Text = "Đơn vị công tác:", Location = new Point(610, 68), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            _txtDonVi = new TextBox { Location = new Point(720, 65), Width = 220 };

            _grpDetails.Controls.AddRange(new Control[] {
                lblMa, _txtMa, lblHoTen, _txtHoTen, lblGioiTinh, _cboGioiTinh,
                lblEmail, _txtEmail, lblSdt, _txtSoDienThoai, lblDonVi, _txtDonVi
            });

            // 5. Buttons Panel
            _pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                BackColor = ColorSurface,
                Padding = new Padding(20, 10, 20, 10)
            };

            _btnAdd = CreateStyledButton("Thêm mới", ColorSuccess, Color.White);
            _btnAdd.Location = new Point(20, 10);
            _btnAdd.Click += BtnAdd_Click;

            _btnEdit = CreateStyledButton("Chỉnh sửa", ColorPrimaryLight, Color.White);
            _btnEdit.Location = new Point(120, 10);
            _btnEdit.Click += BtnEdit_Click;

            _btnDelete = CreateStyledButton("Xóa hồ sơ", ColorDanger, Color.White);
            _btnDelete.Location = new Point(220, 10);
            _btnDelete.Click += BtnDelete_Click;

            _btnSave = CreateStyledButton("Lưu dữ liệu", ColorPrimary, Color.White);
            _btnSave.Location = new Point(750, 10);
            _btnSave.Click += BtnSave_Click;

            _btnCancel = CreateStyledButton("Hủy bỏ", Color.FromArgb(203, 213, 225), ColorText);
            _btnCancel.Location = new Point(850, 10);
            _btnCancel.Click += BtnCancel_Click;

            _pnlButtons.Controls.AddRange(new Control[] { _btnAdd, _btnEdit, _btnDelete, _btnSave, _btnCancel });

            // Layout Assemble
            var centerPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15, 5, 15, 5) };
            centerPanel.Controls.Add(_dgvAttendees);

            this.Controls.Add(centerPanel);
            this.Controls.Add(_grpDetails);
            this.Controls.Add(_pnlButtons);
            this.Controls.Add(_pnlSearch);
            this.Controls.Add(_pnlHeader);
        }

        private void SetupColumns()
        {
            _dgvAttendees.Columns.Clear();
            _dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguoiThamGia", HeaderText = "Mã", Width = 90 });
            _dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoTen", HeaderText = "Họ và tên", Width = 200 });
            _dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GioiTinh", HeaderText = "Giới tính", Width = 90 });
            _dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 220 });
            _dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "Số điện thoại", Width = 130 });
            _dgvAttendees.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DonViCongTac", HeaderText = "Đơn vị công tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        }

        private void LoadDataToGrid(IEnumerable<NguoiThamGia> data)
        {
            _dgvAttendees.DataSource = null;
            _dgvAttendees.DataSource = data.ToList();
            if (_dgvAttendees.Rows.Count > 0)
            {
                _dgvAttendees.Rows[0].Selected = true;
                BindRowToDetails((NguoiThamGia)_dgvAttendees.Rows[0].DataBoundItem);
            }
            else
            {
                ClearDetails();
            }
        }

        private void DgvAttendees_SelectionChanged(object? sender, EventArgs e)
        {
            if (_currentMode == FormMode.View && _dgvAttendees.CurrentRow?.DataBoundItem is NguoiThamGia item)
            {
                BindRowToDetails(item);
            }
        }

        private void BindRowToDetails(NguoiThamGia item)
        {
            _txtMa.Text = item.MaNguoiThamGia;
            _txtHoTen.Text = item.HoTen;
            _cboGioiTinh.SelectedItem = item.GioiTinh ?? "Nam";
            _txtEmail.Text = item.Email;
            _txtSoDienThoai.Text = item.SoDienThoai;
            _txtDonVi.Text = item.DonViCongTac ?? string.Empty;
        }

        private void ClearDetails()
        {
            _txtMa.Clear();
            _txtHoTen.Clear();
            _cboGioiTinh.SelectedIndex = 0;
            _txtEmail.Clear();
            _txtSoDienThoai.Clear();
            _txtDonVi.Clear();
            _errorProvider.Clear();
        }

        private void ApplyMode(FormMode mode)
        {
            _currentMode = mode;
            bool isEditing = (mode == FormMode.Add || mode == FormMode.Edit);

            _txtHoTen.ReadOnly = !isEditing;
            _cboGioiTinh.Enabled = isEditing;
            _txtEmail.ReadOnly = !isEditing;
            _txtSoDienThoai.ReadOnly = !isEditing;
            _txtDonVi.ReadOnly = !isEditing;
            _dgvAttendees.Enabled = !isEditing;

            _btnAdd.Enabled = !isEditing;
            _btnEdit.Enabled = !isEditing && _dgvAttendees.CurrentRow != null;
            _btnDelete.Enabled = !isEditing && _dgvAttendees.CurrentRow != null;

            _btnSave.Enabled = isEditing;
            _btnCancel.Enabled = isEditing;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            ClearDetails();
            _txtMa.Text = _attendeeService.GenerateNextAttendeeId(_attendees);
            ApplyMode(FormMode.Add);
            _txtHoTen.Focus();
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (_dgvAttendees.CurrentRow == null) return;
            ApplyMode(FormMode.Edit);
            _txtHoTen.Focus();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_dgvAttendees.CurrentRow?.DataBoundItem is NguoiThamGia item)
            {
                var confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa hồ sơ '{item.HoTen}' ({item.MaNguoiThamGia})?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    _attendees.Remove(item);
                    LoadDataToGrid(_attendees);
                    MessageBox.Show("Đã xóa hồ sơ người tham gia thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            _errorProvider.Clear();

            var attendee = new NguoiThamGia
            {
                MaNguoiThamGia = _txtMa.Text.Trim(),
                HoTen = _txtHoTen.Text.Trim(),
                GioiTinh = _cboGioiTinh.SelectedItem?.ToString() ?? "Nam",
                Email = _txtEmail.Text.Trim(),
                SoDienThoai = _txtSoDienThoai.Text.Trim(),
                DonViCongTac = _txtDonVi.Text.Trim()
            };

            var validation = _attendeeService.ValidateAttendee(attendee, isNew: _currentMode == FormMode.Add);
            if (!validation.IsSuccess)
            {
                if (string.IsNullOrWhiteSpace(attendee.HoTen)) _errorProvider.SetError(_txtHoTen, "Họ tên không được rỗng");
                if (string.IsNullOrWhiteSpace(attendee.Email) || !attendee.Email.Contains("@")) _errorProvider.SetError(_txtEmail, "Email không hợp lệ");
                if (string.IsNullOrWhiteSpace(attendee.SoDienThoai)) _errorProvider.SetError(_txtSoDienThoai, "SĐT không hợp lệ");

                MessageBox.Show(validation.Message, "Lỗi kiểm tra", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_currentMode == FormMode.Add)
            {
                _attendees.Add(attendee);
                MessageBox.Show("Thêm mới người tham gia thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (_currentMode == FormMode.Edit)
            {
                var existing = _attendees.FirstOrDefault(a => a.MaNguoiThamGia == attendee.MaNguoiThamGia);
                if (existing != null)
                {
                    existing.HoTen = attendee.HoTen;
                    existing.GioiTinh = attendee.GioiTinh;
                    existing.Email = attendee.Email;
                    existing.SoDienThoai = attendee.SoDienThoai;
                    existing.DonViCongTac = attendee.DonViCongTac;
                }
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ApplyMode(FormMode.View);
            LoadDataToGrid(_attendees);
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            _errorProvider.Clear();
            ApplyMode(FormMode.View);
            if (_dgvAttendees.CurrentRow?.DataBoundItem is NguoiThamGia item)
                BindRowToDetails(item);
            else
                ClearDetails();
        }

        private void DoSearch()
        {
            var results = _attendeeService.SearchAttendees(
                _attendees, 
                _txtSearch.Text, 
                _cboGenderFilter.SelectedItem?.ToString());
            LoadDataToGrid(results);
        }

        private List<NguoiThamGia> GetInitialSeedData()
        {
            return new List<NguoiThamGia>
            {
                new NguoiThamGia { MaNguoiThamGia = "NTG001", HoTen = "Nguyễn Văn An", GioiTinh = "Nam", Email = "an.nguyen@gmail.com", SoDienThoai = "0912345678", DonViCongTac = "Đại học Bách Khoa TP.HCM" },
                new NguoiThamGia { MaNguoiThamGia = "NTG002", HoTen = "Trần Thị Bích", GioiTinh = "Nữ", Email = "bich.tran@gmail.com", SoDienThoai = "0987654321", DonViCongTac = "FPT Software" },
                new NguoiThamGia { MaNguoiThamGia = "NTG003", HoTen = "Lê Hoàng Nam", GioiTinh = "Nam", Email = "nam.le@techcorp.vn", SoDienThoai = "0903112233", DonViCongTac = "TechCorp VN" },
                new NguoiThamGia { MaNguoiThamGia = "NTG004", HoTen = "Phạm Minh Tuấn", GioiTinh = "Nam", Email = "tuan.pham@student.edu.vn", SoDienThoai = "0934556677", DonViCongTac = "Đại học CNTT - ĐHQG" },
                new NguoiThamGia { MaNguoiThamGia = "NTG005", HoTen = "Hoàng Thu Thảo", GioiTinh = "Nữ", Email = "thao.hoang@vng.com.vn", SoDienThoai = "0977889900", DonViCongTac = "VNG Corporation" }
            };
        }
    }
}
