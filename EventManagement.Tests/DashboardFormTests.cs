using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Session;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class DashboardFormTests
    {
        private class FakeDashboardService : IDashboardService
        {
            public DashboardStatsDto StatsToReturn { get; set; } = new DashboardStatsDto
            {
                TongSoSuKien = 15,
                SuKienChuanBi = 5,
                SuKienSapDienRa = 6,
                SuKienDangDienRa = 2,
                SuKienDaHoanThanh = 2,
                TongVeDaBan = 450,
                TongNguoiThamGia = 400,
                TongDoanhThuVe = 90000000m,
                TongChiPhiThucTe = 35000000m,
                TongNguoiCheckIn = 360,
                TyLeCheckIn = 80.0,
                TongNhaTaiTro = 8
            };

            public List<UpcomingEventDto> UpcomingToReturn { get; set; } = new List<UpcomingEventDto>
            {
                new UpcomingEventDto
                {
                    MaSuKien = "SK_AI",
                    TenSuKien = "Hội thảo Trí tuệ Nhân tạo 2026",
                    NgayBatDau = DateTime.Today.AddDays(2),
                    NgayKetThuc = DateTime.Today.AddDays(3),
                    DiaDiem = "Trung tâm Hội nghị Quốc gia",
                    SoVeDaBan = 200,
                    SoNguoiCheckIn = 150,
                    TyLeCheckIn = 75.0,
                    TrangThai = "Sắp diễn ra"
                }
            };

            public Task<ServiceResult<DashboardStatsDto>> GetDashboardStatsAsync()
            {
                return Task.FromResult(ServiceResult<DashboardStatsDto>.Success(StatsToReturn));
            }

            public ServiceResult<DashboardStatsDto> GetDashboardStats()
            {
                return ServiceResult<DashboardStatsDto>.Success(StatsToReturn);
            }

            public Task<ServiceResult<List<UpcomingEventDto>>> GetUpcomingEventsAsync(int daysAhead = 7)
            {
                return Task.FromResult(ServiceResult<List<UpcomingEventDto>>.Success(UpcomingToReturn));
            }

            public ServiceResult<List<UpcomingEventDto>> GetUpcomingEvents(int daysAhead = 7)
            {
                return ServiceResult<List<UpcomingEventDto>>.Success(UpcomingToReturn);
            }

            public Task<ServiceResult<List<UpcomingEventDto>>> GetAllEventsAsync()
            {
                return Task.FromResult(ServiceResult<List<UpcomingEventDto>>.Success(UpcomingToReturn));
            }

            public ServiceResult<List<UpcomingEventDto>> GetAllEvents()
            {
                return ServiceResult<List<UpcomingEventDto>>.Success(UpcomingToReturn);
            }

            public Task<ServiceResult<List<EventRevenueChartDto>>> GetEventRevenueComparisonAsync(int limit = 5)
            {
                return Task.FromResult(ServiceResult<List<EventRevenueChartDto>>.Success(new List<EventRevenueChartDto>()));
            }

            public ServiceResult<List<EventRevenueChartDto>> GetEventRevenueComparison(int limit = 5)
            {
                return ServiceResult<List<EventRevenueChartDto>>.Success(new List<EventRevenueChartDto>());
            }

            public Task<ServiceResult<List<MonthlyEventCountDto>>> GetMonthlyEventDistributionAsync(int year)
            {
                return Task.FromResult(ServiceResult<List<MonthlyEventCountDto>>.Success(new List<MonthlyEventCountDto>()));
            }

            public ServiceResult<List<MonthlyEventCountDto>> GetMonthlyEventDistribution(int year)
            {
                return ServiceResult<List<MonthlyEventCountDto>>.Success(new List<MonthlyEventCountDto>());
            }

            public Task<ServiceResult<CheckInRatioDto>> GetCheckInRatioAsync(string? maSuKien = null)
            {
                return Task.FromResult(ServiceResult<CheckInRatioDto>.Success(new CheckInRatioDto { DaCheckIn = 360, ChuaCheckIn = 90, TyLePhanTram = 80.0 }));
            }

            public ServiceResult<CheckInRatioDto> GetCheckInRatio(string? maSuKien = null)
            {
                return ServiceResult<CheckInRatioDto>.Success(new CheckInRatioDto { DaCheckIn = 360, ChuaCheckIn = 90, TyLePhanTram = 80.0 });
            }
        }

        [TestMethod]
        [Description("Kiểm tra Form F02 khởi tạo đúng phông chữ Segoe UI 9pt từ FrmBase và kế thừa các đặc tính chuẩn.")]
        public void FrmDashboard_InitializesSuccessfully_WithDefaultComponents()
        {
            // Arrange
            var fakeService = new FakeDashboardService();

            // Act
            using var form = new FrmDashboard(fakeService);

            // Assert
            form.Should().NotBeNull();
            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
            form.Font.Name.Should().Be("Segoe UI");
            form.StartPosition.Should().Be(FormStartPosition.CenterScreen);
        }
        // Mô tả: Test này thẩm định Form F02 kế thừa đúng chuẩn FrmBase với phông chữ Segoe UI 9pt và vị trí căn giữa màn hình theo quy định thiết kế UI.

        [TestMethod]
        [Description("Kiểm tra khi Admin đăng nhập, tiêu đề chào mừng hiển thị đúng vai trò và các nút Quản trị người dùng (F03), Phân quyền (F20) được bật hiển thị.")]
        public void FrmDashboard_ApplyUserSession_ShowsAdminControlsWhenAdmin()
        {
            // Arrange
            CurrentSession.DangNhap(
                maNguoiDung: "ADMIN01",
                tenDangNhap: "admin",
                hoTen: "Nguyễn Văn Admin",
                email: "admin@event.com",
                maNhomQuyen: "ADMIN",
                danhSachQuyen: new[] { "ALL" });

            var fakeService = new FakeDashboardService();
            using var form = new FrmDashboard(fakeService);

            // Act
            form.ApplyUserSessionAndPermissions();

            // Assert
            form.CanAccessAdminFeatures.Should().BeTrue();

            CurrentSession.DangXuat();
        }
        // Mô tả: Đảm bảo kiểm soát phân quyền giao diện (RBAC) trên Dashboard: người dùng Admin nhìn thấy đầy đủ lối tắt cấu hình hệ thống F03 và F20.

        [TestMethod]
        [Description("Kiểm tra khi nhân viên Staff đăng nhập, các nút Quản trị người dùng (F03) và Phân quyền (F20) được tự động ẩn đi.")]
        public void FrmDashboard_ApplyUserSession_HidesAdminControlsWhenStaff()
        {
            // Arrange
            CurrentSession.DangNhap(
                maNguoiDung: "STAFF01",
                tenDangNhap: "staff01",
                hoTen: "Trần Thị Nhân Viên",
                email: "staff@event.com",
                maNhomQuyen: "STAFF",
                danhSachQuyen: new[] { "F10", "F13" });

            var fakeService = new FakeDashboardService();
            using var form = new FrmDashboard(fakeService);

            // Act
            form.ApplyUserSessionAndPermissions();

            // Assert
            form.CanAccessAdminFeatures.Should().BeFalse();

            CurrentSession.DangXuat();
        }
        // Mô tả: Bảo mật tầng giao diện (UI Security): nhân viên Staff không được thấy hoặc bấm vào các nút quản lý tài khoản và ma trận phân quyền.

        [TestMethod]
        [Description("Kiểm tra hàm nạp dữ liệu Dashboard cập nhật chính xác 4 thẻ thống kê và danh sách sự kiện sắp diễn ra.")]
        public async Task FrmDashboard_RefreshDashboardAsync_PopulatesKpiCardsAndUpcomingEvents()
        {
            // Arrange
            var fakeService = new FakeDashboardService();
            using var form = new FrmDashboard(fakeService);

            // Act
            await form.RefreshDashboardAsync();

            // Assert
            form.CurrentStats.Should().NotBeNull();
            form.CurrentStats!.TongSoSuKien.Should().Be(15);
            form.CurrentStats.TongVeDaBan.Should().Be(450);
            form.CurrentStats.TongDoanhThuVe.Should().Be(90000000m);
            form.CurrentStats.TyLeCheckIn.Should().Be(80.0);

            form.CurrentUpcomingEvents.Should().NotBeNull();
            form.CurrentUpcomingEvents!.Count.Should().Be(1);
            form.CurrentUpcomingEvents[0].MaSuKien.Should().Be("SK_AI");

            var lblCard1Value = form.Controls.Find("lblCard1Value", true)[0] as Label;
            var lblCard2Value = form.Controls.Find("lblCard2Value", true)[0] as Label;
            var lblCard3Value = form.Controls.Find("lblCard3Value", true)[0] as Label;
            var lblCard4Value = form.Controls.Find("lblCard4Value", true)[0] as Label;

            lblCard1Value!.Text.Should().Be("15");
            lblCard2Value!.Text.Should().Be("450");
            lblCard3Value!.Text.Should().Contain("90,000,000");
            lblCard4Value!.Text.Should().Contain("80.0");
        }
        // Mô tả: Test này thẩm định luồng đổ dữ liệu thực tế lên 4 Cards KPI số liệu to rõ và nạp danh sách sự kiện 7 ngày tới theo đúng chuẩn Mẫu 1.
        [TestMethod]
        public void FrmDashboard_InheritsFrmBase_AndDefaultsToMaximized()
        {
            // Mô tả: Kiểm tra FrmDashboard kế thừa FrmBase, hỗ trợ phóng to toàn màn hình với MaximizeBox bật và WindowState mặc định là Maximized trên màn hình 1920x1080.
            using var form = new FrmDashboard();
            form.Should().BeAssignableTo<FrmBase>();
            form.MaximizeBox.Should().BeTrue("Dashboard phải bật MaximizeBox để phóng to toàn màn hình.");
            form.WindowState.Should().Be(System.Windows.Forms.FormWindowState.Maximized, "Dashboard phải mở chế độ Maximized để hiển thị toàn bộ biểu đồ và bảng trên màn hình.");
        }

        [TestMethod]
        public void FrmDashboard_TitleAndHeaderAndNavigation_AreFormattedProperly()
        {
            // Mô tả: Kiểm tra tiêu đề form chuẩn "Hệ Thống Quản Lý Sự Kiện", tiêu đề góc trên "TRANG CHỦ" in hoa in đậm, nhãn điều hướng "Điều hướng:" và các nút dàn trải đều trên thanh công cụ.
            using var form = new FrmDashboard();
            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện", "Thanh tiêu đề Dashboard phải là 'Hệ Thống Quản Lý Sự Kiện'");
            var lblHeaderTitle = form.Controls.Find("lblHeaderTitle", true)[0] as Label;
            lblHeaderTitle.Should().NotBeNull();
            lblHeaderTitle!.Text.Should().Be("TRANG CHỦ", "Tiêu đề Dashboard phải là TRANG CHỦ in hoa");
            var lblQuickAccess = form.Controls.Find("lblQuickAccess", true)[0] as Label;
            lblQuickAccess.Should().NotBeNull();
            lblQuickAccess!.Text.Should().Be("Điều hướng:", "Nhãn lối tắt tác nghiệp phải được đổi thành 'Điều hướng:'");
            var flpQuickButtons = form.Controls.Find("flpQuickButtons", true)[0] as FlowLayoutPanel;
            flpQuickButtons.Should().NotBeNull();
            flpQuickButtons!.Dock.Should().Be(DockStyle.Fill, "Thanh nút tác nghiệp phải Dock Fill để dàn đều");
        }

        [TestMethod]
        public async Task FrmDashboard_SortUpcomingGridByColumn_SortsAscendingAndDescending()
        {
            var fakeService = new FakeDashboardService
            {
                UpcomingToReturn = new List<UpcomingEventDto>
                {
                    new UpcomingEventDto { MaSuKien = "EVT002", TenSuKien = "Beta", NgayBatDau = DateTime.Today.AddDays(5), SoVeDaBan = 10, TrangThai = "Dự kiến" },
                    new UpcomingEventDto { MaSuKien = "EVT001", TenSuKien = "Alpha", NgayBatDau = DateTime.Today.AddDays(1), SoVeDaBan = 50, TrangThai = "Mở đăng ký" }
                }
            };

            using var form = new FrmDashboard(fakeService);
            await form.RefreshDashboardAsync();

            form.SortUpcomingGridByColumn(nameof(UpcomingEventDto.TenSuKien), ascending: true);
            form.CurrentUpcomingEvents![0].TenSuKien.Should().Be("Alpha");

            form.SortUpcomingGridByColumn(nameof(UpcomingEventDto.SoVeDaBan), ascending: false);
            form.CurrentUpcomingEvents![0].MaSuKien.Should().Be("EVT001");
        }
    }
}