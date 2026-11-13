using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class EventDetailFormTests
    {
        [TestMethod]
        public void F05_TitleAndHeader_StandardizedCorrectly()
        {
            // Mô tả: Kiểm tra thanh tiêu đề form đạt chuẩn 'Hệ Thống Quản Lý Sự Kiện', tiêu đề 'CHI TIẾT SỰ KIỆN' và ẩn phụ đề
            using var form = new FrmEventDetail();

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");

            var lblTitle = form.GetType().GetField("lblTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Label;
            lblTitle.Should().NotBeNull();
            lblTitle!.Text.Should().Be("CHI TIẾT SỰ KIỆN");

            var lblSubtitle = form.GetType().GetField("lblSubtitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Label;
            lblSubtitle.Should().NotBeNull();
            lblSubtitle!.Visible.Should().BeFalse();
        }

        [TestMethod]
        public void F05_Tab1ActionButtons_SaveAndCancelHiddenInViewMode()
        {
            // Mô tả: Kiểm tra các nút Lưu và Hủy trên Tab 1 bị ẩn đi khi ở chế độ xem, chỉ hiện khi nhấn Chỉnh sửa
            using var form = new FrmEventDetail();
            form.Show();

            var btnSua = form.GetType().GetField("btnSuaThongTin", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Button;
            var btnLuu = form.GetType().GetField("btnLuuThongTin", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Button;
            var btnHuy = form.GetType().GetField("btnHuyThongTin", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Button;

            btnSua.Should().NotBeNull();
            btnLuu.Should().NotBeNull();
            btnHuy.Should().NotBeNull();

            btnSua!.Visible.Should().BeTrue();
            btnLuu!.Visible.Should().BeFalse();
            btnHuy!.Visible.Should().BeFalse();

            form.Hide();
        }

        [TestMethod]
        public void F05_ToolbarButtons_StandardizedNamesAndRoles()
        {
            // Mô tả: Kiểm tra các nút điều hướng nghiệp vụ trên các tab bỏ ký tự mũi tên và mã form (F08, F15, F16, F17)
            using var form = new FrmEventDetail();

            var btnMoF08 = form.GetType().GetField("btnMoF08", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Button;
            var btnMoF15 = form.GetType().GetField("btnMoF15", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Button;
            var btnMoF16 = form.GetType().GetField("btnMoF16", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Button;
            var btnMoF17 = form.GetType().GetField("btnMoF17", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Button;
            var btnMoF18 = form.GetType().GetField("btnMoF18", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Button;

            btnMoF08!.Text.Should().Be("Quản lý lịch trình");
            btnMoF15!.Text.Should().Be("Quản lý phân công");
            btnMoF16!.Text.Should().Be("Quản lý tài trợ");
            btnMoF17!.Text.Should().Be("Quản lý chi phí");
            btnMoF18!.Visible.Should().BeFalse(); // Form 18 đã được lược bỏ khỏi giao diện F05
        }

        [TestMethod]
        public void F05_Tab8HeaderStandardized_And_FeedbackTabRemoved()
        {
            // Mô tả: Kiểm tra tiêu đề Tab 8 ('TIẾN ĐỘ NGÂN SÁCH'), F05 còn 8 Tab (đã bỏ Tab 'Khảo sát & Phản hồi') và nhãn chọn sự kiện không bị che
            using var form = new FrmEventDetail();
            form.Show();

            var lblTab8Header = form.GetType().GetField("lblTab8Header", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Label;
            var tabMain = form.GetType().GetField("tabMain", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as TabControl;
            var lblChonSuKien = form.GetType().GetField("lblChonSuKien", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as Label;
            var cboChonSuKien = form.GetType().GetField("cboChonSuKien", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(form) as ComboBox;

            lblTab8Header!.Text.Should().Be("TIẾN ĐỘ NGÂN SÁCH");
            tabMain.Should().NotBeNull();
            tabMain!.TabPages.Count.Should().Be(8);
            tabMain.TabPages.ContainsKey("tabDanhGia").Should().BeFalse();
            lblChonSuKien!.Right.Should().BeLessThanOrEqualTo(cboChonSuKien!.Left - 6);

            form.Hide();
        }

        [TestMethod]
        public void F05_TicketOrderService_LoadsAttendeeAndTiersForEvent()
        {
            // Mô tả: Kiểm tra TicketOrderService nạp thành công người tham gia và cơ cấu hạng vé theo mã sự kiện EVT202601
            var service = new TicketOrderService();

            var tiers = service.GetTicketTiersByEvent("EVT202601");
            tiers.Should().NotBeNullOrEmpty();
            tiers.Count.Should().BeGreaterThanOrEqualTo(2);

            var ticketsRes = service.GetTicketsByEvent("EVT202601");
            ticketsRes.IsSuccess.Should().BeTrue();
            ticketsRes.Data.Should().NotBeNullOrEmpty();
            ticketsRes.Data!.Count.Should().BeGreaterThanOrEqualTo(3);
        }
    }
}
