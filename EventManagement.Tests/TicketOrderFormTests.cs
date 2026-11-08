using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class TicketOrderFormTests
    {
        [TestMethod]
        public void F11_TitleBar_StandardizedCorrectly()
        {
            // Mô tả: Kiểm tra thanh tiêu đề Form F11 được chuẩn hóa thành 'Hệ Thống Quản Lý Sự Kiện' và ẩn dòng chú thích cũ
            using var form = new FrmTicketOrder();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
            form.LblSubTitle.Visible.Should().BeFalse();
        }

        [TestMethod]
        public void F11_GridColumns_Centered()
        {
            // Mô tả: Kiểm tra tất cả tiêu đề cột của danh sách người đi cùng và vé phát hành được căn giữa
            using var form = new FrmTicketOrder();

            form.DgvNguoiDiCung.Columns.Count.Should().BeGreaterThan(0);
            foreach (DataGridViewColumn col in form.DgvNguoiDiCung.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }

            form.DgvIssuedTickets.Columns.Count.Should().BeGreaterThan(0);
            foreach (DataGridViewColumn col in form.DgvIssuedTickets.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }
        }

        [TestMethod]
        public void F11_TierDropdown_AutoPopulatedOnLoad()
        {
            // Mô tả: Kiểm tra hạng vé được tự động nạp khi mở màn hình, khắc phục lỗi chọn hạng vé trống
            using var form = new FrmTicketOrder();
            form.Show();

            if (form.CboSuKien.Items.Count > 0)
            {
                form.CboHangVe.Items.Count.Should().BeGreaterThan(0);
            }
            form.Hide();
        }

        [TestMethod]
        public void F11_OrderInfoGroup_RenamedAndBalanced()
        {
            // Mô tả: Kiểm tra nhóm thông tin đặt vé và danh sách vé điện tử được đổi tên ngắn gọn, chuẩn hóa
            using var form = new FrmTicketOrder();

            form.GrpOrderInfo.Text.Should().Be("THÔNG TIN ĐẶT VÉ");
            form.GrpIssuedTickets.Text.Should().Be("DANH SÁCH VÉ ĐIỆN TỬ");
        }
    }
}
