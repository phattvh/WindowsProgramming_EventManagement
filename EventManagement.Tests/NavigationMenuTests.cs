using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using EventManagement.GUI.Forms;
using EventManagement.Core.Session;

namespace EventManagement.Tests
{
    [TestClass]
    public class NavigationMenuTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            CurrentSession.DangXuat();
        }

        [TestMethod]
        public void NavigationMenu_Configuration_ExcludesSmallDialogs()
        {
            // Các dialog nhỏ / đăng nhập không kích hoạt menu điều hướng
            using var frmLogin = new FrmLogin();
            frmLogin.EnableGlobalNavigationMenu.Should().BeFalse();

            using var frmPassword = new FrmChangePassword();
            frmPassword.EnableGlobalNavigationMenu.Should().BeFalse();

            using var frmConfig = new FrmServerConfig();
            frmConfig.EnableGlobalNavigationMenu.Should().BeFalse();
        }

        [TestMethod]
        public void NavigationMenu_BuildsSevenRootMenus_WithShortcuts()
        {
            using var form = new FrmBase();
            form.InitializeGlobalNavigationMenu();

            var menu = form.NavigationMenuStrip;
            menu.Should().NotBeNull();
            menu!.Items.Count.Should().Be(7);

            string[] expectedMenus = { "&Hệ thống", "&Sự kiện", "&Vé & Soát vé", "&Nhân sự & Đối tác", "&Tài chính & BC", "&Quản trị", "&Trợ giúp" };
            for (int i = 0; i < expectedMenus.Length; i++)
            {
                menu.Items[i].Text.Should().Be(expectedMenus[i]);
            }

            // Kiểm tra các phím tắt chính
            var systemMenu = (ToolStripMenuItem)menu.Items[0];
            var dashboardItem = systemMenu.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(x => x.Text.IndexOf("Dashboard", StringComparison.OrdinalIgnoreCase) >= 0);
            dashboardItem.Should().NotBeNull();
            dashboardItem!.ShortcutKeys.Should().Be(Keys.Control | Keys.Shift | Keys.H);

            var eventMenu = (ToolStripMenuItem)menu.Items[1];
            var eventListItem = eventMenu.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(x => x.Text.IndexOf("Danh sách sự kiện", StringComparison.OrdinalIgnoreCase) >= 0);
            eventListItem.Should().NotBeNull();
            eventListItem!.ShortcutKeys.Should().Be(Keys.Control | Keys.Shift | Keys.E);

            var ticketMenu = (ToolStripMenuItem)menu.Items[2];
            var ticketOrderItem = ticketMenu.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(x => x.Text.IndexOf("Bán vé", StringComparison.OrdinalIgnoreCase) >= 0);
            ticketOrderItem.Should().NotBeNull();
            ticketOrderItem!.ShortcutKeys.Should().Be(Keys.Control | Keys.Shift | Keys.T);

            var checkInItem = ticketMenu.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(x => x.Text.IndexOf("Webcam QR", StringComparison.OrdinalIgnoreCase) >= 0);
            checkInItem.Should().NotBeNull();
            checkInItem!.ShortcutKeys.Should().Be(Keys.Control | Keys.Shift | Keys.K);

            var financeMenu = (ToolStripMenuItem)menu.Items[4];
            var budgetItem = financeMenu.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(x => x.Text.IndexOf("ngân sách", StringComparison.OrdinalIgnoreCase) >= 0);
            budgetItem.Should().NotBeNull();
            budgetItem!.ShortcutKeys.Should().Be(Keys.Control | Keys.Shift | Keys.B);

            var reportItem = financeMenu.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(x => x.Text.IndexOf("Báo cáo", StringComparison.OrdinalIgnoreCase) >= 0);
            reportItem.Should().NotBeNull();
            reportItem!.ShortcutKeys.Should().Be(Keys.Control | Keys.Shift | Keys.R);

            var adminMenu = (ToolStripMenuItem)menu.Items[5];
            var usersItem = adminMenu.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(x => x.Text.IndexOf("người dùng", StringComparison.OrdinalIgnoreCase) >= 0);
            usersItem.Should().NotBeNull();
            usersItem!.ShortcutKeys.Should().Be(Keys.Control | Keys.Shift | Keys.U);

            var rolesItem = adminMenu.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(x => x.Text.IndexOf("phân quyền", StringComparison.OrdinalIgnoreCase) >= 0);
            rolesItem.Should().NotBeNull();
            rolesItem!.ShortcutKeys.Should().Be(Keys.Control | Keys.Shift | Keys.Q);

            var helpMenu = (ToolStripMenuItem)menu.Items[6];
            var shortcutsHelpItem = helpMenu.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(x => x.Text.IndexOf("phím tắt", StringComparison.OrdinalIgnoreCase) >= 0);
            shortcutsHelpItem.Should().NotBeNull();
            shortcutsHelpItem!.ShortcutKeys.Should().Be(Keys.F1);
        }

        [TestMethod]
        public void NavigationMenu_DockingZOrder_PreventsOverlappingWithHeaderPanel()
        {
            // Kiểm tra layout: MenuStrip phải nằm ở Top = 0 và đẩy pnlHeader xuống dưới menu.Height,
            // bảo đảm 100% không bị tình trạng đè chữ hay đè khung.
            using var form = new Form();
            form.Size = new Size(1024, 680);

            var pnlHeader = new Panel
            {
                Name = "pnlHeader",
                Dock = DockStyle.Top,
                Height = 60
            };
            form.Controls.Add(pnlHeader);

            var menu = new MenuStrip
            {
                Name = "navMenuStrip",
                Dock = DockStyle.Top,
                Height = 28
            };
            form.Controls.Add(menu);
            menu.SendToBack();

            form.PerformLayout();

            menu.Top.Should().Be(0);
            pnlHeader.Top.Should().Be(menu.Height);
            pnlHeader.Top.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void NavigationMenu_PermissionAuthorization_DisablesUnauthorizedFeatures()
        {
            // Đăng nhập tài khoản nhân viên chỉ có quyền F04, UC04
            CurrentSession.DangNhap("NV01", "staff1", "Nhân viên 1", "staff1@event.vn", "STAFF", new[] { "UC04", "F04" });

            using var form = new FrmBase();
            form.InitializeGlobalNavigationMenu();

            var menu = form.NavigationMenuStrip;
            menu.Should().NotBeNull();

            // Menu Sự kiện -> F04 phải Enabled
            var eventMenu = (ToolStripMenuItem)menu!.Items[1];
            var eventListItem = eventMenu.DropDownItems.OfType<ToolStripMenuItem>().First(x => x.Tag?.ToString() == "UC04");
            eventListItem.Enabled.Should().BeTrue();

            // Menu Quản trị -> F03 & F20 không có quyền -> Enabled = false
            var adminMenu = (ToolStripMenuItem)menu.Items[5];
            var usersItem = adminMenu.DropDownItems.OfType<ToolStripMenuItem>().First(x => x.Tag?.ToString() == "UC03");
            usersItem.Enabled.Should().BeFalse();

            var rolesItem = adminMenu.DropDownItems.OfType<ToolStripMenuItem>().First(x => x.Tag?.ToString() == "UC20");
            rolesItem.Enabled.Should().BeFalse();
        }
    }
}
