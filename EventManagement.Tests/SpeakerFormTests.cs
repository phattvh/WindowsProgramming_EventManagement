using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class SpeakerFormTests
    {
        [TestMethod]
        public void F09_TitleBar_StandardizedCorrectly()
        {
            // Mô tả: Kiểm tra thanh tiêu đề Form F09 được chuẩn hóa thành 'Hệ Thống Quản Lý Sự Kiện'
            using var form = new FrmSpeaker();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
        }

        [TestMethod]
        public void F09_RefreshButton_AlignedRight()
        {
            // Mô tả: Kiểm tra nút làm mới được bố trí về phía góc phải của bảng lọc tìm kiếm
            using var form = new FrmSpeaker();
            form.Show();

            form.BtnLamMoi.Right.Should().BeGreaterThan(form.PnlFilter.ClientSize.Width / 2);
            form.Hide();
        }

        [TestMethod]
        public void F09_ActionButtons_SaveAndCancelHiddenInViewMode()
        {
            // Mô tả: Kiểm tra nút Lưu và Hủy bị ẩn ở chế độ xem và chỉ hiển thị khi Thêm hoặc Sửa
            using var form = new FrmSpeaker();
            form.Show();

            form.BtnThem.Visible.Should().BeTrue();
            form.BtnLuu.Visible.Should().BeFalse();
            form.BtnHuy.Visible.Should().BeFalse();
            form.Hide();
        }

        [TestMethod]
        public void F09_GridViewHeaders_CenteredCorrectly()
        {
            // Mô tả: Kiểm tra tất cả tiêu đề cột của DataGridView diễn giả được căn giữa
            using var form = new FrmSpeaker();

            form.DgvDienGia.Columns.Count.Should().BeGreaterThan(0);
            foreach (DataGridViewColumn col in form.DgvDienGia.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }
        }
    }
}
