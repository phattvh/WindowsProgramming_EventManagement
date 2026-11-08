using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class ScheduleFormTests
    {
        [TestMethod]
        public void F08_TitleBarAndSubtitle_StandardizedCorrectly()
        {
            // Mô tả: Kiểm tra thanh tiêu đề form F08 được chuẩn hóa thành 'Hệ Thống Quản Lý Sự Kiện' và ẩn dòng phụ đề kỹ thuật
            using var form = new FrmSchedule();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
            form.LblSubtitle.Visible.Should().BeFalse();
        }

        [TestMethod]
        public void F08_ConflictCheckButton_HiddenAndReplacedByAutoStatus()
        {
            // Mô tả: Kiểm tra nút kiểm tra xung đột thủ công được ẩn và thay bằng nhãn trạng thái tự động màu xanh khi hợp lệ
            using var form = new FrmSchedule();
            form.Show();

            form.BtnKiemTraXungDot.Visible.Should().BeFalse();
            form.LblKiemTraStatus.Visible.Should().BeTrue();
            form.LblKiemTraStatus.Text.Should().Contain("hợp lệ");
            form.LblKiemTraStatus.ForeColor.Should().Be(FrmBase.ColorSuccess);
            form.Hide();
        }

        [TestMethod]
        public void F08_ActionButtons_SaveAndCancelHiddenInViewMode()
        {
            // Mô tả: Kiểm tra các nút Lưu và Hủy bị ẩn ở chế độ xem và chỉ hiển thị khi Thêm hoặc Sửa
            using var form = new FrmSchedule();
            form.Show();

            form.BtnThem.Visible.Should().BeTrue();
            form.BtnLuu.Visible.Should().BeFalse();
            form.BtnHuy.Visible.Should().BeFalse();
            form.Hide();
        }

        [TestMethod]
        public void F08_GridViewHeaders_CenteredCorrectly()
        {
            // Mô tả: Kiểm tra toàn bộ tiêu đề cột DataGridView lịch trình được căn giữa
            using var form = new FrmSchedule();

            form.DgvLichTrinh.Columns.Count.Should().BeGreaterThan(0);
            foreach (DataGridViewColumn col in form.DgvLichTrinh.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }
        }
    }
}
