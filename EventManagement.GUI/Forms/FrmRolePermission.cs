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
    /// Màn hình F20 – Quản lý nhóm quyền & Phân quyền ma trận động (Task T1-14)
    /// Phụ trách: Thành viên 1 (Kiến trúc, CSDL & Bảo mật)
    /// </summary>
    public partial class FrmRolePermission : FrmBase
    {
        private readonly IRolePermissionService _rolePermissionService;
        private List<RoleDto> _roles = new();
        private List<PhanQuyenMatrixDto> _currentMatrix = new();

        public Label LblTitle => lblTitle;
        public Label LblSubTitle => lblSubTitle;
        public Button BtnLuu => btnLuu;
        public Button BtnThemNhom => btnThemNhom;
        public Button BtnXoaNhom => btnXoaNhom;
        public Button BtnChonTatCa => btnChonTatCa;
        public Button BtnBoChonTatCa => btnBoChonTatCa;
        public Button BtnDong => btnDong;
        public ComboBox CboNhomQuyen => cboNhomQuyen;
        public DataGridView DgvPhanQuyen => dgvPhanQuyen;

        public FrmRolePermission() : this(new RolePermissionService())
        {
        }

        public FrmRolePermission(IRolePermissionService rolePermissionService)
        {
            InitializeComponent();
            _rolePermissionService = rolePermissionService;
            this.Text = "Hệ Thống Quản Lý Sự Kiện";
            this.WindowState = FormWindowState.Maximized;
            dgvPhanQuyen.AutoGenerateColumns = false;

            foreach (DataGridViewColumn col in dgvPhanQuyen.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private async void FrmRolePermission_Load(object sender, EventArgs e)
        {
            if (CurrentSession.IsLoggedIn && !PermissionHelper.CheckAccessAndNotify(this, "UC20", "Phân quyền & Nhóm quyền"))
            {
                this.Close();
                return;
            }

            btnLuu.Tag = "UC20";
            btnThemNhom.Tag = "UC20";
            btnXoaNhom.Tag = "UC20";
            PermissionHelper.ApplyAuthorization(this, hideInsteadOfDisable: false);

            await LoadRolesAsync();
        }

        public async Task LoadRolesAsync()
        {
            try
            {
                _roles = await _rolePermissionService.GetAllRolesAsync();
                cboNhomQuyen.DataSource = null;
                cboNhomQuyen.DisplayMember = "TenNhomQuyen";
                cboNhomQuyen.ValueMember = "MaNhomQuyen";
                cboNhomQuyen.DataSource = _roles;

                if (_roles.Any())
                {
                    cboNhomQuyen.SelectedIndex = 0;
                    await LoadMatrixAsync(_roles[0].MaNhomQuyen);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh mục vai trò. Chi tiết: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task LoadMatrixAsync(string roleId)
        {
            try
            {
                _currentMatrix = await _rolePermissionService.GetPermissionMatrixAsync(roleId);
                dgvPhanQuyen.DataSource = null;
                dgvPhanQuyen.DataSource = _currentMatrix;

                bool isAdmin = roleId == "ADMIN";
                colCoQuyen.ReadOnly = isAdmin;
                btnChonTatCa.Enabled = !isAdmin;
                btnBoChonTatCa.Enabled = !isAdmin;

                var selectedRole = _roles.FirstOrDefault(r => r.MaNhomQuyen == roleId);
                btnXoaNhom.Enabled = selectedRole != null && !selectedRole.IsSystemRole;

                if (isAdmin)
                {
                    lblGhiChuAdmin.Text = "Nhóm Quản trị viên (ADMIN) mặc định toàn quyền hệ thống, không thể thu hồi.";
                    lblGhiChuAdmin.ForeColor = Color.FromArgb(37, 99, 235);
                }
                else
                {
                    lblGhiChuAdmin.Text = "Đánh dấu các chức năng cho phép nhóm này truy cập, sau đó nhấn 'Lưu thay đổi'.";
                    lblGhiChuAdmin.ForeColor = Color.FromArgb(180, 83, 9);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải ma trận phân quyền: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cboNhomQuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNhomQuyen.SelectedValue is string roleId && !string.IsNullOrWhiteSpace(roleId))
            {
                await LoadMatrixAsync(roleId);
            }
        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            dgvPhanQuyen.EndEdit();
            foreach (var item in _currentMatrix)
            {
                item.CoQuyen = true;
            }
            dgvPhanQuyen.Refresh();
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            dgvPhanQuyen.EndEdit();
            foreach (var item in _currentMatrix)
            {
                item.CoQuyen = false;
            }
            dgvPhanQuyen.Refresh();
        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            dgvPhanQuyen.EndEdit();
            if (cboNhomQuyen.SelectedValue is not string roleId) return;

            try
            {
                var result = await _rolePermissionService.SavePermissionMatrixAsync(roleId, _currentMatrix);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadMatrixAsync(roleId);
                }
                else
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lưu phân quyền: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThemNhom_Click(object sender, EventArgs e)
        {
            using var dlg = new Form
            {
                Text = "Thêm vai trò / nhóm quyền mới",
                Width = 420,
                Height = 240,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Font = new Font("Segoe UI", 9F)
            };

            var lblCode = new Label { Text = "Mã vai trò (3-20 ký tự hoa):", Left = 20, Top = 20, Width = 180 };
            var txtCode = new TextBox { Left = 20, Top = 45, Width = 360, CharacterCasing = CharacterCasing.Upper };
            var lblName = new Label { Text = "Tên vai trò / mô tả:", Left = 20, Top = 80, Width = 180 };
            var txtName = new TextBox { Left = 20, Top = 105, Width = 360 };
            var btnOk = new Button { Text = "Tạo mới", Left = 180, Top = 155, Width = 95, Height = 30, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(37, 99, 235), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var btnCancel = new Button { Text = "Hủy", Left = 285, Top = 155, Width = 95, Height = 30, DialogResult = DialogResult.Cancel };

            dlg.Controls.AddRange(new Control[] { lblCode, txtCode, lblName, txtName, btnOk, btnCancel });
            dlg.AcceptButton = btnOk;
            dlg.CancelButton = btnCancel;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string code = txtCode.Text.Trim();
                string name = txtName.Text.Trim();

                var validation = ValidateRoleInput(code, name);
                if (!validation.IsValid)
                {
                    MessageBox.Show(validation.ErrorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = await _rolePermissionService.CreateRoleAsync(new CreateRoleDto
                {
                    MaNhomQuyen = code,
                    TenNhomQuyen = name
                });

                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadRolesAsync();
                    cboNhomQuyen.SelectedValue = code;
                }
                else
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async void btnXoaNhom_Click(object sender, EventArgs e)
        {
            if (cboNhomQuyen.SelectedValue is not string roleId) return;

            var role = _roles.FirstOrDefault(r => r.MaNhomQuyen == roleId);
            if (role == null) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa vai trò '{role.TenNhomQuyen}' ({role.MaNhomQuyen}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var result = await _rolePermissionService.DeleteRoleAsync(roleId);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadRolesAsync();
                }
                else
                {
                    MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Logic thẩm định dữ liệu đầu vào khi tạo nhóm quyền mới
        /// Public static phục vụ viết Unit Test
        /// </summary>
        public static (bool IsValid, string? ErrorMessage) ValidateRoleInput(string roleCode, string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleCode))
            {
                return (false, "Mã nhóm quyền không được để trống!");
            }

            string cleanCode = roleCode.Trim().ToUpper();
            if (!Regex.IsMatch(cleanCode, "^[A-Z0-9_]{3,20}$"))
            {
                return (false, "Mã nhóm quyền phải từ 3 đến 20 ký tự (chữ hoa không dấu, số hoặc gạch dưới)!");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                return (false, "Tên nhóm quyền không được để trống!");
            }

            return (true, null);
        }
    }
}
