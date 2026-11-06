using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.DAL;

namespace EventManagement.GUI.Forms
{
    /// <summary>
    /// Hộp thoại cấu hình và thẩm định kết nối CSDL SQL Server động (AD-6).
    /// </summary>
    public partial class FrmServerConfig : FrmBase
    {
        public bool IsConnectionSuccessful { get; private set; } = false;

        public FrmServerConfig()
        {
            InitializeComponent();
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            try
            {
                var connStr = DatabaseConfigHelper.GetConnectionString();
                var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connStr);
                txtServer.Text = string.IsNullOrWhiteSpace(builder.DataSource) ? "." : builder.DataSource;
                txtDatabase.Text = string.IsNullOrWhiteSpace(builder.InitialCatalog) ? "EventManagementDb" : builder.InitialCatalog;

                if (builder.IntegratedSecurity)
                {
                    rbWindowsAuth.Checked = true;
                    pnlSqlAuth.Enabled = false;
                }
                else
                {
                    rbSqlAuth.Checked = true;
                    pnlSqlAuth.Enabled = true;
                    txtUsername.Text = builder.UserID;
                    txtPassword.Text = builder.Password;
                }
            }
            catch
            {
                rbWindowsAuth.Checked = true;
                pnlSqlAuth.Enabled = false;
            }
        }

        private void rbAuth_CheckedChanged(object sender, EventArgs e)
        {
            pnlSqlAuth.Enabled = rbSqlAuth.Checked;
        }

        public string GetConstructedConnectionString()
        {
            return DatabaseConfigHelper.BuildConnectionString(
                server: txtServer.Text,
                database: txtDatabase.Text,
                useWindowsAuth: rbWindowsAuth.Checked,
                username: txtUsername.Text,
                password: txtPassword.Text);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Đang kiểm tra kết nối...";
            lblStatus.ForeColor = Color.DarkGoldenrod;
            Application.DoEvents();

            try
            {
                var connStr = GetConstructedConnectionString();
                var (success, message) = DatabaseConfigHelper.TestConnection(connStr, timeoutSeconds: 3);

                if (success)
                {
                    lblStatus.Text = "✓ " + message;
                    lblStatus.ForeColor = Color.ForestGreen;
                    IsConnectionSuccessful = true;
                }
                else
                {
                    lblStatus.Text = "✗ " + message;
                    lblStatus.ForeColor = Color.Firebrick;
                    IsConnectionSuccessful = false;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "✗ " + ex.Message;
                lblStatus.ForeColor = Color.Firebrick;
                IsConnectionSuccessful = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var connStr = GetConstructedConnectionString();
                var (success, message) = DatabaseConfigHelper.TestConnection(connStr, timeoutSeconds: 3);

                if (!success)
                {
                    var dialogResult = MessageBox.Show(
                        $"Kiểm tra kết nối thất bại:\n{message}\n\nBạn có chắc chắn muốn lưu chuỗi kết nối này không?",
                        "Cảnh Báo Kết Nối",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (dialogResult != DialogResult.Yes)
                    {
                        return;
                    }
                }

                DatabaseConfigHelper.SaveConnectionString(connStr);
                IsConnectionSuccessful = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu cấu hình: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
