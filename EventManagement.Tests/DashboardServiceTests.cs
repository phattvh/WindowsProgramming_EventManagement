using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagement.BLL.Services;
using EventManagement.Core.Entities;
using EventManagement.DAL;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class DashboardServiceTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [TestMethod]
        [Description("Kiểm tra tính toán chỉ số KPI khi cơ sở dữ liệu rỗng, đảm bảo không bị lỗi chia cho 0 (DivideByZeroException) ở tỷ lệ check-in.")]
        public async Task GetDashboardStatsAsync_WithEmptyDatabase_ReturnsZeroStatsWithoutError()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var service = new DashboardService(context);

            // Act
            var result = await service.GetDashboardStatsAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.TongSoSuKien.Should().Be(0);
            result.Data.TongVeDaBan.Should().Be(0);
            result.Data.TongDoanhThuVe.Should().Be(0m);
            result.Data.TongChiPhiThucTe.Should().Be(0m);
            result.Data.TyLeCheckIn.Should().Be(0.0);
        }
        // Mô tả: Test này bảo đảm Dashboard hiển thị an toàn khi hệ thống mới cài đặt chưa có dữ liệu, tránh sập ứng dụng do chia cho 0.

        [TestMethod]
        [Description("Kiểm tra tính toán chính xác tổng sự kiện, tổng vé bán, doanh thu, chi phí và tỷ lệ check-in từ dữ liệu thực.")]
        public async Task GetDashboardStatsAsync_WithSampleData_CalculatesCorrectAggregates()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var service = new DashboardService(context);

            context.SuKiens.AddRange(
                new SuKien { MaSuKien = "SK01", TenSuKien = "Hội thảo A", TrangThai = "Chuẩn bị", NgayBatDau = DateTime.Today.AddDays(2), NgayKetThuc = DateTime.Today.AddDays(3), MaDiaDiem = "Hall A" },
                new SuKien { MaSuKien = "SK02", TenSuKien = "Hội thảo B", TrangThai = "Sắp diễn ra", NgayBatDau = DateTime.Today.AddDays(5), NgayKetThuc = DateTime.Today.AddDays(6), MaDiaDiem = "Hall B" },
                new SuKien { MaSuKien = "SK03", TenSuKien = "Hội thảo C", TrangThai = "Đã hoàn thành", NgayBatDau = DateTime.Today.AddDays(-10), NgayKetThuc = DateTime.Today.AddDays(-9), MaDiaDiem = "Hall C" }
            );

            context.HangVes.AddRange(
                new HangVe { MaHangVe = "HV01", MaSuKien = "SK01", TenHangVe = "Standard", GiaVe = 100000m, SoLuongPhatHanh = 100, SoLuongDaBan = 40 },
                new HangVe { MaHangVe = "HV02", MaSuKien = "SK02", TenHangVe = "VIP", GiaVe = 200000m, SoLuongPhatHanh = 50, SoLuongDaBan = 10 }
            );

            context.ChiPhiSuKiens.AddRange(
                new ChiPhiSuKien { MaChiPhi = "CP01", MaSuKien = "SK01", KhoanMucChi = "Âm thanh", SoTien = 1500000m },
                new ChiPhiSuKien { MaChiPhi = "CP02", MaSuKien = "SK02", KhoanMucChi = "Teabreak", SoTien = 500000m }
            );

            context.CheckIns.AddRange(
                new CheckIn { MaCheckIn = "CI01", MaVe = "VE01", ThoiGianCheckIn = DateTime.Now },
                new CheckIn { MaCheckIn = "CI02", MaVe = "VE02", ThoiGianCheckIn = DateTime.Now }
            );

            context.NhaTaiTros.Add(new NhaTaiTro { MaNhaTaiTro = "NTT01", TenToChuc = "Google", NguoiDaiDien = "Phát", SoDienThoai = "0901234567" });

            await context.SaveChangesAsync();

            // Act
            var result = await service.GetDashboardStatsAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.TongSoSuKien.Should().Be(3);
            result.Data.SuKienChuanBi.Should().Be(1);
            result.Data.SuKienSapDienRa.Should().Be(1);
            result.Data.SuKienDaHoanThanh.Should().Be(1);
            result.Data.TongVeDaBan.Should().Be(50); // 40 + 10
            result.Data.TongDoanhThuVe.Should().Be(6000000m); // 40*100k + 10*200k = 6tr
            result.Data.TongChiPhiThucTe.Should().Be(2000000m); // 1.5tr + 500k = 2tr
            result.Data.TongNguoiCheckIn.Should().Be(2);
            result.Data.TyLeCheckIn.Should().Be(4.0); // (2 / 50) * 100 = 4%
            result.Data.TongNhaTaiTro.Should().Be(1);
        }
        // Mô tả: Test này kiểm tra tính tổng hợp toàn diện cả 4 thẻ KPI của Dashboard F02 gồm số sự kiện, doanh thu tính từ hạng vé, chi phí thực tế và tỷ lệ check-in.

        [TestMethod]
        [Description("Kiểm tra bộ lọc sự kiện sắp diễn ra trong 7 ngày tới, loại trừ sự kiện đã hủy và sự kiện ngoài khoảng ngày.")]
        public async Task GetUpcomingEventsAsync_FiltersWithinDaysRangeAndExcludesCancelled()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var service = new DashboardService(context);

            context.SuKiens.AddRange(
                new SuKien { MaSuKien = "SK_IN_1", TenSuKien = "Sự kiện ngày mai", TrangThai = "Sắp diễn ra", NgayBatDau = DateTime.Today.AddDays(1), NgayKetThuc = DateTime.Today.AddDays(2), MaDiaDiem = "Phòng A" },
                new SuKien { MaSuKien = "SK_IN_2", TenSuKien = "Sự kiện ngày thứ 5", TrangThai = "Chuẩn bị", NgayBatDau = DateTime.Today.AddDays(5), NgayKetThuc = DateTime.Today.AddDays(6), MaDiaDiem = "Phòng B" },
                new SuKien { MaSuKien = "SK_CANCEL", TenSuKien = "Sự kiện đã hủy trong tuần", TrangThai = "Đã hủy", NgayBatDau = DateTime.Today.AddDays(3), NgayKetThuc = DateTime.Today.AddDays(4), MaDiaDiem = "Phòng C" },
                new SuKien { MaSuKien = "SK_OUT", TenSuKien = "Sự kiện tháng sau", TrangThai = "Chuẩn bị", NgayBatDau = DateTime.Today.AddDays(30), NgayKetThuc = DateTime.Today.AddDays(31), MaDiaDiem = "Phòng D" },
                new SuKien { MaSuKien = "SK_PAST", TenSuKien = "Sự kiện tuần trước", TrangThai = "Đã hoàn thành", NgayBatDau = DateTime.Today.AddDays(-7), NgayKetThuc = DateTime.Today.AddDays(-6), MaDiaDiem = "Phòng E" }
            );

            await context.SaveChangesAsync();

            // Act
            var result = await service.GetUpcomingEventsAsync(daysAhead: 7);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Count.Should().Be(2);
            result.Data.Should().Contain(e => e.MaSuKien == "SK_IN_1");
            result.Data.Should().Contain(e => e.MaSuKien == "SK_IN_2");
            result.Data.Should().NotContain(e => e.MaSuKien == "SK_CANCEL");
            result.Data.Should().NotContain(e => e.MaSuKien == "SK_OUT");
            result.Data.Should().NotContain(e => e.MaSuKien == "SK_PAST");
        }
        // Mô tả: Test này thẩm định đúng yêu cầu của Mẫu 1: bảng dưới cùng trên F02 chỉ hiển thị sự kiện trong 7 ngày tới và loại trừ các sự kiện đã hủy.

        [TestMethod]
        [Description("Kiểm tra phân bổ số lượng sự kiện theo đủ 12 tháng trong năm.")]
        public async Task GetMonthlyEventDistributionAsync_Returns12MonthsWithAccurateCounts()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var service = new DashboardService(context);
            int currentYear = DateTime.Today.Year;

            context.SuKiens.AddRange(
                new SuKien { MaSuKien = "SK_M1_1", TenSuKien = "Tháng 1 A", NgayBatDau = new DateTime(currentYear, 1, 10), NgayKetThuc = new DateTime(currentYear, 1, 11), MaDiaDiem = "A", TrangThai = "Chuẩn bị" },
                new SuKien { MaSuKien = "SK_M1_2", TenSuKien = "Tháng 1 B", NgayBatDau = new DateTime(currentYear, 1, 20), NgayKetThuc = new DateTime(currentYear, 1, 21), MaDiaDiem = "B", TrangThai = "Chuẩn bị" },
                new SuKien { MaSuKien = "SK_M6", TenSuKien = "Tháng 6", NgayBatDau = new DateTime(currentYear, 6, 15), NgayKetThuc = new DateTime(currentYear, 6, 16), MaDiaDiem = "C", TrangThai = "Chuẩn bị" }
            );

            await context.SaveChangesAsync();

            // Act
            var result = await service.GetMonthlyEventDistributionAsync(currentYear);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Count.Should().Be(12);
            result.Data.Should().Contain(m => m.Thang == 1 && m.SoLuongSuKien == 2);
            result.Data.Should().Contain(m => m.Thang == 6 && m.SoLuongSuKien == 1);
            result.Data.Should().Contain(m => m.Thang == 12 && m.SoLuongSuKien == 0);
        }
        // Mô tả: Test này bảo đảm dữ liệu biểu đồ phân bổ sự kiện theo 12 tháng trả về đúng 12 tháng chuẩn mực cho LiveCharts2.

        [TestMethod]
        [Description("Kiểm tra tỷ lệ Check-in thực tế tính toán đúng số vé đã check-in và số vé chưa check-in.")]
        public async Task GetCheckInRatioAsync_CalculatesCorrectCheckedInAndNotCheckedIn()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var service = new DashboardService(context);

            context.SuKiens.Add(new SuKien { MaSuKien = "SK_CHK", TenSuKien = "Sự kiện Checkin", NgayBatDau = DateTime.Today, NgayKetThuc = DateTime.Today, MaDiaDiem = "A", TrangThai = "Đang diễn ra" });
            context.HangVes.Add(new HangVe { MaHangVe = "HV_CHK", MaSuKien = "SK_CHK", TenHangVe = "Standard", GiaVe = 50000m, SoLuongPhatHanh = 100, SoLuongDaBan = 80 });

            context.DangKys.Add(new DangKy { MaDangKy = "DK01", MaSuKien = "SK_CHK", MaHangVe = "HV_CHK", SoLuong = 2, TongTien = 100000m, TrangThaiThanhToan = "Đã thanh toán" });

            context.ChiTietVes.AddRange(
                new ChiTietVe { MaVe = "V01", MaDangKy = "DK01", MaQR = "QR01", TrangThaiVe = "Đã check-in" },
                new ChiTietVe { MaVe = "V02", MaDangKy = "DK01", MaQR = "QR02", TrangThaiVe = "Đã check-in" }
            );

            context.CheckIns.AddRange(
                new CheckIn { MaCheckIn = "C1", MaVe = "V01", ThoiGianCheckIn = DateTime.Now },
                new CheckIn { MaCheckIn = "C2", MaVe = "V02", ThoiGianCheckIn = DateTime.Now }
            );

            await context.SaveChangesAsync();

            // Act
            var result = await service.GetCheckInRatioAsync("SK_CHK");

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.DaCheckIn.Should().Be(2);
            result.Data.ChuaCheckIn.Should().Be(78); // 80 - 2
            result.Data.TyLePhanTram.Should().Be(2.5); // (2 / 80) * 100 = 2.5%
        }
        // Mô tả: Test này kiểm tra độ chính xác của biểu đồ tròn (Pie chart) tỷ lệ Check-in đã đến vs chưa đến.

        [TestMethod]
        [Description("Kiểm tra tính doanh thu và chi phí cho từng sự kiện phục vụ biểu đồ cột so sánh.")]
        public async Task GetEventRevenueComparisonAsync_CalculatesRevenueAndExpensePerEvent()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var service = new DashboardService(context);

            context.SuKiens.Add(new SuKien { MaSuKien = "SK_REV", TenSuKien = "Sự kiện Gala", TrangThai = "Đang diễn ra", NgayBatDau = DateTime.Today, NgayKetThuc = DateTime.Today, MaDiaDiem = "Resort" });
            context.HangVes.Add(new HangVe { MaHangVe = "HV_REV", MaSuKien = "SK_REV", TenHangVe = "VIP", GiaVe = 1000000m, SoLuongPhatHanh = 10, SoLuongDaBan = 5 });
            context.ChiPhiSuKiens.Add(new ChiPhiSuKien { MaChiPhi = "CP_REV", MaSuKien = "SK_REV", KhoanMucChi = "Thuê sảnh", SoTien = 3000000m });

            await context.SaveChangesAsync();

            // Act
            var result = await service.GetEventRevenueComparisonAsync(limit: 5);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().Contain(e => e.MaSuKien == "SK_REV" && e.DoanhThu == 5000000m && e.ChiPhi == 3000000m);
        }
        // Mô tả: Thẩm định dữ liệu so sánh thu chi phục vụ vẽ biểu đồ cột đa trục trong LiveCharts2.
    }
}
