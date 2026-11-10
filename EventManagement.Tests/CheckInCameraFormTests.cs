using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class CheckInCameraFormTests
    {
        [TestMethod]
        public void F12_TitleBar_StandardizedCorrectly()
        {
            // Mô tả: Kiểm tra thanh tiêu đề Form Soát vé (F12) được chuẩn hóa thành 'Hệ Thống Quản Lý Sự Kiện' và ẩn chú thích kỹ thuật
            using var form = new FrmCheckInCamera();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
            form.LblSubTitle.Visible.Should().BeFalse();
        }

        [TestMethod]
        public void F12_GridColumns_Centered()
        {
            // Mô tả: Kiểm tra tất cả tiêu đề cột của DataGridView điểm danh được căn giữa
            using var form = new FrmCheckInCamera();

            form.DgvRecentCheckIns.Columns.Count.Should().BeGreaterThan(0);
            foreach (DataGridViewColumn col in form.DgvRecentCheckIns.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }
        }

        [TestMethod]
        public void F12_TicketTier_ColumnsWeightedAndCentered()
        {
            // Mô tả: Kiểm tra cấu hình độ rộng và căn giữa tiêu đề của bảng hạng vé và kho vé
            using var form = new FrmTicketTier();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
            form.DgvTiers.Columns.Count.Should().BeGreaterThan(0);
            foreach (DataGridViewColumn col in form.DgvTiers.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }

            var colTen = form.DgvTiers.Columns["TenHangVe"] ?? form.DgvTiers.Columns[2];
            colTen.Width.Should().Be(200);
        }

        [TestMethod]
        public void F12_TicketTier_SaveAndCancelHiddenInViewMode()
        {
            // Mô tả: Kiểm tra các nút Lưu và Hủy của Form quản lý hạng vé chỉ hiển thị khi Thêm hoặc Sửa
            using var form = new FrmTicketTier();
            form.Show();

            form.BtnAdd.Visible.Should().BeTrue();
            form.BtnSave.Enabled.Should().BeFalse();
            form.BtnCancel.Enabled.Should().BeFalse();
            form.Hide();
        }
    }
}
