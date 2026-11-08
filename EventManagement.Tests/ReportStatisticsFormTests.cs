using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventManagement.GUI.Forms;
using EventManagement.BLL.Services;

namespace EventManagement.Tests
{
    [TestClass]
    public class ReportStatisticsFormTests
    {
        [TestMethod]
        public void F19_TitleBar_And_WindowState_Standardized()
        {
            // Mô tả: Kiểm tra tiêu đề form F19 là "Hệ Thống Quản Lý Sự Kiện" và chế độ mở toàn màn hình (Maximized).
            using var form = new FrmReportStatistics();
            Assert.AreEqual("Hệ Thống Quản Lý Sự Kiện", form.Text);
            Assert.AreEqual(FormWindowState.Maximized, form.WindowState);
        }

        [TestMethod]
        public void F19_BannerHeader_Standardized_And_SubtitleHidden()
        {
            // Mô tả: Kiểm tra tiêu đề banner đã đổi thành "BÁO CÁO THỐNG KÊ" và dòng phụ chú được ẩn hoàn toàn.
            using var form = new FrmReportStatistics();
            var lblTitle = form.Controls.Find("lblBannerTitle", true);
            var lblSubtitle = form.Controls.Find("lblBannerSubtitle", true);

            Assert.IsTrue(lblTitle.Length > 0, "Không tìm thấy lblBannerTitle.");
            Assert.AreEqual("BÁO CÁO THỐNG KÊ", lblTitle[0].Text);

            Assert.IsTrue(lblSubtitle.Length > 0, "Không tìm thấy lblBannerSubtitle.");
            Assert.IsFalse(lblSubtitle[0].Visible, "Dòng mô tả phụ lblBannerSubtitle phải được ẩn.");
        }

        [TestMethod]
        public void F19_SettlementGrid_Columns_RenamedAndCentered()
        {
            // Mô tả: Kiểm tra cấu hình cột của bảng quyết toán: cột Diễn giải đổi thành 'Nội dung', tất cả tiêu đề cột được căn giữa.
            using var form = new FrmReportStatistics();
            var dgvArray = form.Controls.Find("dgvReportData", true);
            Assert.IsTrue(dgvArray.Length > 0, "Không tìm thấy dgvReportData.");

            var dgv = (DataGridView)dgvArray[0];
            Assert.IsTrue(dgv.Columns.Count >= 8, "Bảng quyết toán phải có ít nhất 8 cột.");

            bool hasNoiDungColumn = false;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                Assert.AreEqual(DataGridViewContentAlignment.MiddleCenter, col.HeaderCell.Style.Alignment,
                    $"Tiêu đề cột '{col.HeaderText}' phải được căn giữa.");
                if (col.HeaderText == "Nội dung")
                {
                    hasNoiDungColumn = true;
                    Assert.AreEqual(DataGridViewAutoSizeColumnMode.Fill, col.AutoSizeMode, "Cột 'Nội dung' phải tự dãn đầy (Fill).");
                }
            }
            Assert.IsTrue(hasNoiDungColumn, "Cột 'Nội dung' (thay cho 'Diễn giải') phải tồn tại.");
        }

        [TestMethod]
        public void F19_BottomKpiBar_ContainsAllMetrics()
        {
            // Mô tả: Kiểm tra thanh chỉ số KPI đáy màn hình có đầy đủ 4 thẻ: Tổng nguồn thu, Tổng thực chi, Chênh lệch, Kết luận.
            using var form = new FrmReportStatistics();
            var lblTongThu = form.Controls.Find("lblTongThu", true);
            var lblTongChi = form.Controls.Find("lblTongChi", true);
            var lblChenhLech = form.Controls.Find("lblChenhLech", true);
            var lblTrangThai = form.Controls.Find("lblTrangThai", true);

            Assert.IsTrue(lblTongThu.Length > 0, "Phải có nhãn lblTongThu.");
            Assert.IsTrue(lblTongChi.Length > 0, "Phải có nhãn lblTongChi.");
            Assert.IsTrue(lblChenhLech.Length > 0, "Phải có nhãn lblChenhLech.");
            Assert.IsTrue(lblTrangThai.Length > 0, "Phải có nhãn lblTrangThai.");
        }
    }
}
