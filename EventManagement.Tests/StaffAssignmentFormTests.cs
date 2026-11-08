using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class StaffAssignmentFormTests
    {
        [TestMethod]
        public void F15_TitleBar_StandardizedCorrectly()
        {
            // Mô tả: Kiểm tra thanh tiêu đề Form F15 được chuẩn hóa thành 'Hệ Thống Quản Lý Sự Kiện'
            using var form = new FrmStaffAssignment();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
        }

        [TestMethod]
        public void F15_GridColumns_RenamedAndCentered()
        {
            // Mô tả: Kiểm tra cột 'Sự kiện áp dụng' được đổi tên thành 'Sự kiện' và toàn bộ tiêu đề cột được căn giữa
            using var form = new FrmStaffAssignment();

            form.DgvPhanCong.Columns["colTenSuKien"]!.HeaderText.Should().Be("Sự kiện");
            form.DgvPhanCong.Columns["colTenNhanSu"]!.HeaderText.Should().Be("Họ và tên");

            foreach (DataGridViewColumn col in form.DgvPhanCong.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }
        }

        [TestMethod]
        public void F15_ConflictCheckButton_HiddenAndReplacedByAutoStatus()
        {
            // Mô tả: Kiểm tra nút kiểm tra ca trực thủ công bị ẩn và thay bằng nhãn trạng thái tự động màu xanh khi hợp lệ
            using var form = new FrmStaffAssignment();
            form.Show();

            form.BtnKiemTraXungDot.Visible.Should().BeFalse();
            form.LblKiemTraStatus.Visible.Should().BeTrue();
            form.LblKiemTraStatus.Text.Should().Contain("hợp lệ");
            form.LblKiemTraStatus.ForeColor.Should().Be(FrmBase.ColorSuccess);
            form.Hide();
        }

        [TestMethod]
        public void F15_ActionButtons_SaveAndCancelHiddenInViewMode()
        {
            // Mô tả: Kiểm tra các nút Lưu và Hủy bị ẩn ở chế độ xem và chỉ hiển thị khi Thêm hoặc Sửa
            using var form = new FrmStaffAssignment();
            form.Show();

            form.BtnThem.Visible.Should().BeTrue();
            form.BtnLuu.Visible.Should().BeFalse();
            form.BtnHuy.Visible.Should().BeFalse();
            form.Hide();
        }
    }
}
