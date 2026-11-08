using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Session;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Màn hình Đăng nhập hệ thống (F01) - Phụ trách: Thành viên 1
    /// Hỗ trợ kiểm thử, chống Brute-force với đếm ngược thời gian khóa (AD-7 / BR-19)
    /// </summary>
    public partial class FrmLogin : FrmBase
    {
        private readonly IAuthService? _authService;
        private int _remainingLockoutSeconds = 0;

        public FrmLogin() : this(new AuthService())
        {
        }

        public FrmLogin(IAuthService? authService)
        {
            InitializeComponent();
            _authService = authService;
            this.Load += FrmLogin_Load;
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '•';
        }

        private void FrmLogin_Load(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "admin";
                txtPassword.Text = "Admin@123";
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Xóa lỗi cũ
            errorProvider.Clear();
            lblLockoutNotice.Visible = false;

            // 1. Kiểm tra thẩm định dữ liệu đầu vào
            var validation = ValidateLoginInputs(username, password);
            if (!validation.IsValid)
            {
                if (validation.ErrorField == nameof(txtUsername))
                {
                    errorProvider.SetError(txtUsername, validation.ErrorMessage);
                    txtUsername.Focus();
                }
                else if (validation.ErrorField == nameof(txtPassword))
                {
                    errorProvider.SetError(txtPassword, validation.ErrorMessage);
                    txtPassword.Focus();
                }
                return;
            }

            if (_authService == null)
            {
                lblLockoutNotice.Text = "Dịch vụ xác thực chưa sẵn sàng!";
                lblLockoutNotice.Visible = true;
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang đăng nhập...";

                var result = await _authService.LoginAsync(username, password);

                if (result.IsSuccess)
                {
                    // Đăng nhập thành công -> Đóng Form đăng nhập với DialogResult.OK
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // Đăng nhập thất bại: hiển thị thông báo lỗi
                    lblLockoutNotice.Text = result.Message;
                    lblLockoutNotice.Visible = true;

                    // Nếu thông báo chứa thông tin khóa tạm thời (ví dụ "... sau X giây" hoặc "... trong X phút")
                    int lockDuration = ExtractLockoutSeconds(result.Message);
                    if (lockDuration > 0)
                    {
                        StartLockoutCountdown(lockDuration);
                    }
                }
            }
            catch (Exception ex)
            {
                lblLockoutNotice.Text = "Đã xảy ra lỗi: " + ex.Message;
                lblLockoutNotice.Visible = true;
            }
            finally
            {
                if (_remainingLockoutSeconds <= 0)
                {
                    btnLogin.Enabled = true;
                    btnLogin.Text = "Đăng Nhập";
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void timerLockout_Tick(object sender, EventArgs e)
        {
            _remainingLockoutSeconds--;

            if (_remainingLockoutSeconds > 0)
            {
                lblLockoutNotice.Text = $"Tài khoản đang bị khóa tạm thời. Thử lại sau: {_remainingLockoutSeconds} giây";
            }
            else
            {
                timerLockout.Stop();
                lblLockoutNotice.Visible = false;
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng Nhập";
            }
        }

        public void StartLockoutCountdown(int seconds)
        {
            _remainingLockoutSeconds = seconds;
            btnLogin.Enabled = false;
            btnLogin.Text = $"Bị khóa ({_remainingLockoutSeconds}s)";
            lblLockoutNotice.Text = $"Tài khoản đang bị khóa tạm thời. Thử lại sau: {_remainingLockoutSeconds} giây";
            lblLockoutNotice.Visible = true;
            timerLockout.Start();
        }

        /// <summary>
        /// Phân tích thông báo lỗi để trích xuất số giây hoặc số phút bị khóa
        /// </summary>
        public static int ExtractLockoutSeconds(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return 0;

            // Kiểm tra "sau X giây"
            var secondMatch = Regex.Match(message, @"sau\s+(\d+)\s+giây", RegexOptions.IgnoreCase);
            if (secondMatch.Success && int.TryParse(secondMatch.Groups[1].Value, out int seconds))
            {
                return seconds;
            }

            // Kiểm tra "trong X phút"
            var minuteMatch = Regex.Match(message, @"trong\s+(\d+)\s+phút", RegexOptions.IgnoreCase);
            if (minuteMatch.Success && int.TryParse(minuteMatch.Groups[1].Value, out int minutes))
            {
                return minutes * 60;
            }

            return 0;
        }

        /// <summary>
        /// Thẩm định dữ liệu đầu vào cho màn hình Đăng nhập (Task T1-09)
        /// Hàm static hỗ trợ Unit Testing trực tiếp không cần môi trường GUI
        /// </summary>
        public static (bool IsValid, string? ErrorField, string? ErrorMessage) ValidateLoginInputs(
            string username, 
            string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return (false, "txtUsername", "Vui lòng nhập tên đăng nhập!");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return (false, "txtPassword", "Vui lòng nhập mật khẩu!");
            }

            return (true, null, null);
        }
    }
}