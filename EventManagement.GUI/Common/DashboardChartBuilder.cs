using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.DTOs;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace EventManagement.GUI.Common
{
    /// <summary>
    /// Lớp hỗ trợ xây dựng dữ liệu biểu đồ LiveCharts2 cho Dashboard F02 (Task T1-16).
    /// </summary>
    public static class DashboardChartBuilder
    {
        public static readonly SKColor BlueRevenue = new(41, 128, 185);
        public static readonly SKColor OrangeExpense = new(230, 126, 34);
        public static readonly SKColor GreenCheckIn = new(46, 204, 113);
        public static readonly SKColor GrayNotCheckIn = new(189, 195, 199);
        public static readonly SKColor TextColor = new(44, 62, 80);

        /// <summary>
        /// Xây dựng chuỗi dữ liệu cột so sánh Doanh thu vs Chi phí theo sự kiện.
        /// </summary>
        public static (ISeries[] Series, string[] XLabels) BuildRevenueExpenseSeries(IEnumerable<EventRevenueChartDto>? data, int maxLabelLength = 12)
        {
            var list = data?.ToList() ?? new List<EventRevenueChartDto>();

            if (list.Count == 0)
            {
                // Mặc định biểu đồ mẫu khi chưa có dữ liệu
                list.Add(new EventRevenueChartDto { TenSuKien = "Chưa có SK", DoanhThu = 0m, ChiPhi = 0m });
            }

            var xLabels = list.Select(e =>
            {
                var name = string.IsNullOrWhiteSpace(e.TenSuKien) ? "Sự kiện" : e.TenSuKien;
                return name.Length > maxLabelLength ? name.Substring(0, maxLabelLength) + "..." : name;
            }).ToArray();

            var revValues = list.Select(e => (double)e.DoanhThu).ToArray();
            var expValues = list.Select(e => (double)e.ChiPhi).ToArray();

            var series = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Name = "Doanh thu",
                    Values = revValues,
                    Fill = new SolidColorPaint(BlueRevenue),
                    Stroke = null,
                    MaxBarWidth = 40
                },
                new ColumnSeries<double>
                {
                    Name = "Chi phí",
                    Values = expValues,
                    Fill = new SolidColorPaint(OrangeExpense),
                    Stroke = null,
                    MaxBarWidth = 40
                }
            };

            return (series, xLabels);
        }

        /// <summary>
        /// Xây dựng chuỗi dữ liệu tròn (Pie Chart) biểu diễn tỷ lệ Check-in thực tế.
        /// </summary>
        public static ISeries[] BuildCheckInPieSeries(CheckInRatioDto? data)
        {
            int checkedIn = data?.DaCheckIn ?? 0;
            int notCheckedIn = data?.ChuaCheckIn ?? 0;

            if (checkedIn == 0 && notCheckedIn == 0)
            {
                // Khi chưa phát hành vé
                notCheckedIn = 1;
            }

            return new ISeries[]
            {
                new PieSeries<int>
                {
                    Name = "Đã check-in",
                    Values = new int[] { checkedIn },
                    Fill = new SolidColorPaint(GreenCheckIn)
                },
                new PieSeries<int>
                {
                    Name = "Chưa check-in",
                    Values = new int[] { notCheckedIn },
                    Fill = new SolidColorPaint(GrayNotCheckIn)
                }
            };
        }

        /// <summary>
        /// Xây dựng trục X cho biểu đồ cột với nhãn chỉ định.
        /// </summary>
        public static Axis[] BuildXAxes(string[] labels, double? textSize = null)
        {
            var axis = new Axis
            {
                Labels = labels,
                LabelsPaint = new SolidColorPaint(TextColor),
                LabelsRotation = 0
            };
            if (textSize.HasValue)
            {
                axis.TextSize = textSize.Value;
            }
            return new Axis[] { axis };
        }

        /// <summary>
        /// Xây dựng trục Y định dạng tiền tệ Việt Nam Đồng (VNĐ).
        /// </summary>
        public static Axis[] BuildCurrencyYAxes()
        {
            return new Axis[]
            {
                new Axis
                {
                    Labeler = value => $"{value:N0} đ",
                    LabelsPaint = new SolidColorPaint(TextColor)
                }
            };
        }
    }
}
