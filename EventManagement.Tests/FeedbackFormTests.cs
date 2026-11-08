using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class FeedbackFormTests
    {
        [TestMethod]
        public void F18_TitleBar_StandardizedCorrectly()
        {
            // Mô tả: Kiểm tra thanh tiêu đề Form F18 được chuẩn hóa thành 'Hệ Thống Quản Lý Sự Kiện'
            using var form = new FrmFeedback();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
        }

        [TestMethod]
        public void F18_KpiBadRating_CalculatedAndDisplayed()
        {
            // Mô tả: Kiểm tra chỉ số KPI tỷ lệ đánh giá xấu (1-2 sao) được tích hợp hiển thị đầy đủ bên cạnh các KPI khác
            using var form = new FrmFeedback();
            form.Width = 1920;
            form.Height = 1080;
            form.Show();

            form.LblKpiDanhGiaXauValue.Should().NotBeNull();
            form.LblKpiDanhGiaXauValue.Text.Should().Contain("%");
            form.Hide();
        }

        [TestMethod]
        public void F18_GridColumns_Centered()
        {
            // Mô tả: Kiểm tra tất cả tiêu đề cột của DataGridView khảo sát phản hồi được căn giữa
            using var form = new FrmFeedback();

            form.DgvPhanHoi.Columns.Count.Should().BeGreaterThan(0);
            foreach (DataGridViewColumn col in form.DgvPhanHoi.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }
        }

        [TestMethod]
        public void F18_ActionButtons_SaveAndCancelHiddenInViewMode()
        {
            // Mô tả: Kiểm tra các nút Lưu và Hủy bị ẩn ở chế độ xem và chỉ hiển thị khi Thêm hoặc Sửa
            using var form = new FrmFeedback();
            form.Show();

            form.BtnThem.Visible.Should().BeTrue();
            form.BtnLuu.Visible.Should().BeFalse();
            form.BtnHuy.Visible.Should().BeFalse();
            form.Hide();
        }
    }
}
