using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.GUI.Common;
using EventManagement.GUI.Forms;
using FluentAssertions;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class DashboardChartTests
    {
        [TestMethod]
        [Description("Kiểm tra DashboardChartBuilder tạo đủ 2 cột (Doanh thu và Chi phí) cùng nhãn trục X tương ứng khi có dữ liệu.")]
        public void BuildRevenueExpenseSeries_WithValidData_Returns2ColumnSeriesAndXLabels()
        {
            // Arrange
            var data = new List<EventRevenueChartDto>
            {
                new EventRevenueChartDto { TenSuKien = "Hội thảo AI", DoanhThu = 50000000m, ChiPhi = 20000000m },
                new EventRevenueChartDto { TenSuKien = "Tech Expo 2026", DoanhThu = 80000000m, ChiPhi = 35000000m }
            };

            // Act
            var (series, xLabels) = DashboardChartBuilder.BuildRevenueExpenseSeries(data);

            // Assert
            series.Should().NotBeNull();
            series.Length.Should().Be(2); // Cột 1: Doanh thu, Cột 2: Chi phí
            series[0].Name.Should().Be("Doanh thu");
            series[1].Name.Should().Be("Chi phí");

            xLabels.Should().NotBeNull();
            xLabels.Length.Should().Be(2);
            xLabels[0].Should().Be("Hội thảo AI");
        }
        // Mô tả: Test này đảm bảo biểu đồ cột LiveCharts2 (Task T1-16) phân bổ đúng 2 Series trực quan để người dùng đối chiếu tức thì doanh thu và chi phí sự kiện.

        [TestMethod]
        [Description("Kiểm tra DashboardChartBuilder xử lý an toàn khi danh sách dữ liệu rỗng hoặc null, không ném exception văng form.")]
        public void BuildRevenueExpenseSeries_WithEmptyOrNullData_ReturnsFallbackSampleSeries()
        {
            // Act
            var (seriesNull, xLabelsNull) = DashboardChartBuilder.BuildRevenueExpenseSeries(null);
            var (seriesEmpty, xLabelsEmpty) = DashboardChartBuilder.BuildRevenueExpenseSeries(new List<EventRevenueChartDto>());

            // Assert
            seriesNull.Should().NotBeNull();
            seriesNull.Length.Should().Be(2);
            xLabelsNull.Should().Contain("Chưa có SK");

            seriesEmpty.Should().NotBeNull();
            seriesEmpty.Length.Should().Be(2);
            xLabelsEmpty.Should().Contain("Chưa có SK");
        }
        // Mô tả: Test này kiểm tra cơ chế phòng vệ lỗi (Defensive Programming), bảo đảm biểu đồ vẫn hiển thị mượt mà không crash khi cơ sở dữ liệu chưa có sự kiện nào.

        [TestMethod]
        [Description("Kiểm tra DashboardChartBuilder tự động rút gọn tên sự kiện quá dài để nhãn trục X không bị tràn giao diện.")]
        public void BuildRevenueExpenseSeries_LongEventNames_TruncatesProperly()
        {
            // Arrange
            var data = new List<EventRevenueChartDto>
            {
                new EventRevenueChartDto { TenSuKien = "Hội Nghị Thượng Đỉnh Toàn Cầu Về Khoa Học Dữ Liệu 2026", DoanhThu = 10000000m, ChiPhi = 5000000m }
            };

            // Act
            var (series, xLabels) = DashboardChartBuilder.BuildRevenueExpenseSeries(data);

            // Assert
            xLabels.Length.Should().Be(1);
            xLabels[0].Length.Should().BeLessThanOrEqualTo(15);
            xLabels[0].Should().EndWith("...");
        }
        // Mô tả: Kiểm tra tính thẩm mỹ của biểu đồ theo chuẩn Segoe UI: tên sự kiện dài được cắt gọt tự động kèm dấu ... để tránh vỡ bố cục trên màn hình nhỏ.

        [TestMethod]
        [Description("Kiểm tra biểu đồ tròn Pie Chart tỷ lệ Check-in tạo đủ 2 lát cắt: Đã check-in và Chưa check-in.")]
        public void BuildCheckInPieSeries_WithValidData_ReturnsCheckedInAndRemainingSlices()
        {
            // Arrange
            var ratio = new CheckInRatioDto
            {
                DaCheckIn = 150,
                ChuaCheckIn = 50,
                TyLePhanTram = 75.0
            };

            // Act
            var series = DashboardChartBuilder.BuildCheckInPieSeries(ratio);

            // Assert
            series.Should().NotBeNull();
            series.Length.Should().Be(2);
            series[0].Name.Should().Be("Đã check-in");
            series[1].Name.Should().Be("Chưa check-in");
        }
        // Mô tả: Thẩm định lát cắt biểu đồ tròn LiveCharts2 (Task T1-16) thể hiện trực quan tỷ lệ người tham dự thực tế đã check-in qua camera QR so với tổng vé phát hành.

        [TestMethod]
        [Description("Kiểm tra biểu đồ tròn Pie Chart xử lý an toàn khi số vé bằng 0, không bị lỗi chia 0.")]
        public void BuildCheckInPieSeries_WithZeroTickets_ReturnsDefaultSliceWithoutCrashing()
        {
            // Arrange
            var ratio = new CheckInRatioDto
            {
                DaCheckIn = 0,
                ChuaCheckIn = 0,
                TyLePhanTram = 0.0
            };

            // Act
            var series = DashboardChartBuilder.BuildCheckInPieSeries(ratio);

            // Assert
            series.Should().NotBeNull();
            series.Length.Should().Be(2);
        }
        // Mô tả: Đảm bảo lát cắt biểu đồ tròn không bị trống rỗng khi sự kiện mới tạo chưa phát hành vé.

        [TestMethod]
        [Description("Kiểm tra trục Y biểu đồ cột định dạng tiền tệ có hậu tố 'đ' và phân cách hàng nghìn chuẩn.")]
        public void BuildCurrencyYAxes_FormatsNumbersWithVndSuffix()
        {
            // Act
            var yAxes = DashboardChartBuilder.BuildCurrencyYAxes();

            // Assert
            yAxes.Should().NotBeNull();
            yAxes.Length.Should().Be(1);
            yAxes[0].Labeler.Should().NotBeNull();

            string formatted = yAxes[0].Labeler!(50000000);
            formatted.Should().Contain("50,000,000");
            formatted.Should().EndWith("đ");
        }
        // Mô tả: Thẩm định định dạng tiền tệ Việt Nam (VNĐ) trên trục tung biểu đồ LiveCharts2 để số liệu dễ đọc và chuyên nghiệp.

        [TestMethod]
        [Description("Kiểm tra tích hợp LiveCharts2 trên Form F02: hàm nạp biểu đồ cập nhật chuỗi dữ liệu vào chartRevenue và chartCheckIn.")]
        public async Task FrmDashboard_LoadChartsDataAsync_PopulatesChartRevenueAndPieChart()
        {
            // Arrange
            var fakeService = new FakeDashboardServiceWithChartData();
            using var form = new FrmDashboard(fakeService);

            // Act
            await form.RefreshDashboardAsync();

            // Assert
            form.chartRevenue.Should().NotBeNull();
            form.chartRevenue.Series.Should().NotBeNull();
            form.chartRevenue.Series.Count().Should().Be(2); // Doanh thu & Chi phí

            form.chartCheckIn.Should().NotBeNull();
            form.chartCheckIn.Series.Should().NotBeNull();
            form.chartCheckIn.Series.Count().Should().Be(2); // Đã checkin & Chưa checkin
        }
        // Mô tả: Test này thẩm định trọn vẹn Task T1-16 (Ngày 7): Form F02 tích hợp động cả biểu đồ cột CartesianChart và biểu đồ tròn PieChart của thư viện LiveCharts2.

        private class FakeDashboardServiceWithChartData : IDashboardService
        {
            public Task<ServiceResult<DashboardStatsDto>> GetDashboardStatsAsync()
            {
                return Task.FromResult(ServiceResult<DashboardStatsDto>.Success(new DashboardStatsDto()));
            }

            public ServiceResult<DashboardStatsDto> GetDashboardStats()
            {
                return ServiceResult<DashboardStatsDto>.Success(new DashboardStatsDto());
            }

            public Task<ServiceResult<List<UpcomingEventDto>>> GetUpcomingEventsAsync(int daysAhead = 7)
            {
                return Task.FromResult(ServiceResult<List<UpcomingEventDto>>.Success(new List<UpcomingEventDto>()));
            }

            public ServiceResult<List<UpcomingEventDto>> GetUpcomingEvents(int daysAhead = 7)
            {
                return ServiceResult<List<UpcomingEventDto>>.Success(new List<UpcomingEventDto>());
            }

            public Task<ServiceResult<List<EventRevenueChartDto>>> GetEventRevenueComparisonAsync(int limit = 5)
            {
                var list = new List<EventRevenueChartDto>
                {
                    new EventRevenueChartDto { TenSuKien = "Event 1", DoanhThu = 20000000m, ChiPhi = 10000000m },
                    new EventRevenueChartDto { TenSuKien = "Event 2", DoanhThu = 35000000m, ChiPhi = 15000000m }
                };
                return Task.FromResult(ServiceResult<List<EventRevenueChartDto>>.Success(list));
            }

            public ServiceResult<List<EventRevenueChartDto>> GetEventRevenueComparison(int limit = 5)
            {
                return GetEventRevenueComparisonAsync(limit).GetAwaiter().GetResult();
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
                var ratio = new CheckInRatioDto { DaCheckIn = 80, ChuaCheckIn = 20, TyLePhanTram = 80.0 };
                return Task.FromResult(ServiceResult<CheckInRatioDto>.Success(ratio));
            }

            public ServiceResult<CheckInRatioDto> GetCheckInRatio(string? maSuKien = null)
            {
                return GetCheckInRatioAsync(maSuKien).GetAwaiter().GetResult();
            }
        }
    }
}
