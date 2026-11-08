using System;
using System.Drawing;
using System.Windows.Forms;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class BudgetExpenseFormTests
    {
        [TestMethod]
        public void F17_TitleBar_StandardizedCorrectly()
        {
            // Mô tả: Kiểm tra thanh tiêu đề Form F17 được chuẩn hóa thành 'Hệ Thống Quản Lý Sự Kiện' và ẩn dòng chú thích cũ
            using var form = new FrmBudgetExpense();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
            form.LblSubTitle.Visible.Should().BeFalse();
        }

        [TestMethod]
        public void F17_BudgetCards_DistributedAcrossFullWidth()
        {
            // Mô tả: Kiểm tra 4 thẻ thống kê ngân sách được dàn trải đều theo chiều ngang không bị dồn cục
            using var form = new FrmBudgetExpense();
            form.Width = 1920;
            form.Height = 1080;
            form.Show();

            form.PnlCardBudget.Left.Should().Be(12);
            form.PnlCardSpent.Left.Should().BeGreaterThan(form.PnlCardBudget.Right);
            form.PnlCardRemaining.Left.Should().BeGreaterThan(form.PnlCardSpent.Right);
            form.PnlCardPercent.Left.Should().BeGreaterThan(form.PnlCardRemaining.Right);
            form.PnlCardPercent.Right.Should().BeCloseTo(form.PnlBudgetCards.ClientSize.Width - 12, 20);
            form.Hide();
        }

        [TestMethod]
        public void F17_GridColumns_CenteredAndWeighted()
        {
            // Mô tả: Kiểm tra tất cả tiêu đề cột của DataGridView chi phí được căn giữa
            using var form = new FrmBudgetExpense();

            form.DgvChiPhi.Columns.Count.Should().BeGreaterThan(0);
            foreach (DataGridViewColumn col in form.DgvChiPhi.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter);
            }
        }

        [TestMethod]
        public void F17_ActionButtons_SaveAndCancelHiddenInViewMode()
        {
            // Mô tả: Kiểm tra các nút Lưu và Hủy bị ẩn ở chế độ xem và chỉ hiển thị khi Thêm hoặc Sửa
            using var form = new FrmBudgetExpense();
            form.Show();

            form.BtnThem.Visible.Should().BeTrue();
            form.BtnLuu.Visible.Should().BeFalse();
            form.BtnHuy.Visible.Should().BeFalse();
            form.Hide();
        }
    }
}
