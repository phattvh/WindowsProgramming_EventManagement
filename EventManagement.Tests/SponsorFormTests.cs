using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class SponsorFormTests
    {
        [TestMethod]
        public void F16_TitleBar_StandardizedCorrectly()
        {
            // Mô tả: Kiểm tra thanh tiêu đề Form F16 được chuẩn hóa thành 'Hệ Thống Quản Lý Sự Kiện'
            using var form = new FrmSponsor();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
        }

        [TestMethod]
        public void F16_RefreshButton_AlignedRight()
        {
            // Mô tả: Kiểm tra nút làm mới được đưa sang góc phải của thanh công cụ tìm kiếm
            using var form = new FrmSponsor();
            form.Show();

            form.BtnLamMoi.Right.Should().BeGreaterThan(form.PnlFilter.ClientSize.Width / 2);
            form.Hide();
        }

        [TestMethod]
        public void F16_GridColumns_CenteredAndWeighted()
        {
            // Mô tả: Kiểm tra tất cả tiêu đề cột của DataGridView nhà tài trợ được căn giữa
            using var form = new FrmSponsor();

            form.DgvNhaTaiTro.Columns.Count.Should().BeGreaterThan(0);
            foreach (DataGridViewColumn col in form.DgvNhaTaiTro.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }
        }

        [TestMethod]
        public void F16_ActionButtons_SaveAndCancelHiddenInViewMode()
        {
            // Mô tả: Kiểm tra các nút Lưu và Hủy bị ẩn ở chế độ xem và chỉ hiển thị khi Thêm hoặc Sửa
            using var form = new FrmSponsor();
            form.Show();

            form.BtnThem.Visible.Should().BeTrue();
            form.BtnLuu.Visible.Should().BeFalse();
            form.BtnHuy.Visible.Should().BeFalse();
            form.Hide();
        }
    }
}
