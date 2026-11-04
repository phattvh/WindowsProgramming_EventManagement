using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Session;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình Đổi mật khẩu tài khoản (UC21) - Phụ trách: Thành viên 1
    /// Hỗ trợ xác thực BCrypt, ErrorProvider, hiển thị mật khẩu và cập nhật trạng thái
    /// </summary>
    public partial class FrmChangePassword : FrmBase
    {
        private readonly IAuthService? _authService;
        private readonly string _username;

        public FrmChangePassword() : this(null, string.Empty)
        {
        }

        public FrmChangePassword(IAuthService? authService, string? username = null)
        {
            InitializeComponent();
            _authService = authService;
            _username = !string.IsNullOrWhiteSpace(username) 
                ? username 
                : (CurrentSession.IsLoggedIn ? CurrentSession.TenDangNhap : string.Empty);
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            char passChar = chkShowPassword.Checked ? '\0' : '•';
            txtCurrentPassword.PasswordChar = passChar;
            txtNewPassword.PasswordChar = passChar;
            txtConfirmPassword.PasswordChar = passChar;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Xóa lỗi cũ
            errorProvider.Clear();
            lblStatus.Text = string.Empty;

            // Kiểm tra tính hợp lệ của dữ liệu đầu vào
            var validation = ValidateInputs(currentPassword, newPassword, confirmPassword);
            if (!validation.IsValid)
            {
                ApplyValidationError(validation.ErrorField, validation.ErrorMessage);
                return;
            }

            if (_authService == null)
            {
                lblStatus.Text = "Dịch vụ xác thực chưa sẵn sàng!";
                return;
            }

            string targetUser = !string.IsNullOrWhiteSpace(_username) 
                ? _username 
                : (CurrentSession.IsLoggedIn ? CurrentSession.TenDangNhap : string.Empty);

            if (string.IsNullOrWhiteSpace(targetUser))
            {
                lblStatus.Text = "Không xác định được phiên người dùng hiện tại!";
                return;
            }

            try
            {
                btnSave.Enabled = false;
                btnSave.Text = "Đang xử lý...";

                var result = await _authService.ChangePasswordAsync(targetUser, currentPassword, newPassword);

                if (result.IsSuccess)
                {
                    MessageBox.Show(
                        "Đổi mật khẩu thành công! Mật khẩu mới đã được cập nhật.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    lblStatus.Text = result.Message;
                    errorProvider.SetError(txtCurrentPassword, result.Message);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Đã xảy ra lỗi: " + ex.Message;
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text = "Cập nhật";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ApplyValidationError(string? field, string? message)
        {
            lblStatus.Text = message ?? string.Empty;
            switch (field)
            {
                case nameof(txtCurrentPassword):
                    errorProvider.SetError(txtCurrentPassword, message);
                    txtCurrentPassword.Focus();
                    break;
                case nameof(txtNewPassword):
                    errorProvider.SetError(txtNewPassword, message);
                    txtNewPassword.Focus();
                    break;
                case nameof(txtConfirmPassword):
                    errorProvider.SetError(txtConfirmPassword, message);
                    txtConfirmPassword.Focus();
                    break;
            }
        }

        /// <summary>
        /// Logic thẩm định dữ liệu đầu vào cho việc đổi mật khẩu (BR-19, BR-20)
        /// Hàm public static hỗ trợ viết Unit Test tự động mà không cần GUI Windows
        /// </summary>
        public static (bool IsValid, string? ErrorField, string? ErrorMessage) ValidateInputs(
            string currentPassword, 
            string newPassword, 
            string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                return (false, "txtCurrentPassword", "Vui lòng nhập mật khẩu hiện tại!");
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return (false, "txtNewPassword", "Vui lòng nhập mật khẩu mới!");
            }

            if (newPassword.Length < 6)
            {
                return (false, "txtNewPassword", "Mật khẩu mới phải có ít nhất 6 ký tự!");
            }

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                return (false, "txtConfirmPassword", "Vui lòng xác nhận mật khẩu mới!");
            }

            if (newPassword != confirmPassword)
            {
                return (false, "txtConfirmPassword", "Xác nhận mật khẩu mới không khớp!");
            }

            if (currentPassword == newPassword)
            {
                return (false, "txtNewPassword", "Mật khẩu mới không được trùng với mật khẩu hiện tại!");
            }

            return (true, null, null);
        }
    }
}