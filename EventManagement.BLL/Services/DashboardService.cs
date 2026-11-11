using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;
using EventManagement.DAL;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt nghiệp vụ tổng hợp chỉ số KPI, sự kiện sắp diễn ra và dữ liệu biểu đồ cho Dashboard F02.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService() : this(new AppDbContext())
        {
        }

        public DashboardService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ServiceResult<DashboardStatsDto>> GetDashboardStatsAsync()
        {
            try
            {
                var stats = new DashboardStatsDto();

                // 1. Thống kê Sự kiện (hỗ trợ cả bộ trạng thái chuẩn CSDL lẫn bộ trạng thái mở rộng)
                var events = await _context.SuKiens.AsNoTracking().ToListAsync();
                stats.TongSoSuKien = events.Count;
                stats.SuKienChuanBi = events.Count(e => e.TrangThai == "Chuẩn bị" || e.TrangThai == "Dự kiến");
                stats.SuKienSapDienRa = events.Count(e => e.TrangThai == "Sắp diễn ra" || e.TrangThai == "Mở đăng ký");
                stats.SuKienDangDienRa = events.Count(e => e.TrangThai == "Đang diễn ra");
                stats.SuKienDaHoanThanh = events.Count(e => e.TrangThai == "Đã hoàn thành" || e.TrangThai == "Đã kết thúc");
                stats.SuKienDaHuy = events.Count(e => e.TrangThai == "Đã hủy" || e.TrangThai == "Hủy");

                // 2. Thống kê Bán vé & Người tham gia
                var ticketTiers = await _context.HangVes.AsNoTracking().ToListAsync();
                stats.TongVeDaBan = ticketTiers.Sum(t => t.SoLuongDaBan);

                stats.TongNguoiThamGia = await _context.NguoiThamGias.AsNoTracking().CountAsync();

                // 3. Thống kê Doanh thu & Chi phí
                var paidOrders = await _context.DangKys.AsNoTracking()
                    .Where(d => d.TrangThaiThanhToan == "Đã thanh toán")
                    .ToListAsync();

                decimal revenueFromOrders = paidOrders.Sum(d => d.TongTien);
                decimal revenueFromTiers = ticketTiers.Sum(t => t.SoLuongDaBan * t.GiaVe);
                stats.TongDoanhThuVe = Math.Max(revenueFromOrders, revenueFromTiers);

                var expenses = await _context.ChiPhiSuKiens.AsNoTracking().ToListAsync();
                stats.TongChiPhiThucTe = expenses.Sum(c => c.SoTien);

                // 4. Thống kê Check-in
                stats.TongNguoiCheckIn = await _context.CheckIns.AsNoTracking().CountAsync();
                if (stats.TongVeDaBan > 0)
                {
                    double rawRatio = ((double)stats.TongNguoiCheckIn / stats.TongVeDaBan) * 100.0;
                    stats.TyLeCheckIn = Math.Round(Math.Min(100.0, rawRatio), 1);
                }
                else
                {
                    stats.TyLeCheckIn = 0.0;
                }

                // 5. Nhà tài trợ
                stats.TongNhaTaiTro = await _context.NhaTaiTros.AsNoTracking().CountAsync();

                return ServiceResult<DashboardStatsDto>.Success(stats, "Lấy số liệu KPI Dashboard thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult<DashboardStatsDto>.Failed($"Lỗi tính toán chỉ số Dashboard: {ex.Message}");
            }
        }

        public ServiceResult<DashboardStatsDto> GetDashboardStats()
        {
            return Task.Run(GetDashboardStatsAsync).GetAwaiter().GetResult();
        }

        public async Task<ServiceResult<List<UpcomingEventDto>>> GetUpcomingEventsAsync(int daysAhead = 7)
        {
            try
            {
                var today = DateTime.Today;
                var maxDate = today.AddDays(daysAhead).AddDays(1).AddTicks(-1);

                // Lấy các sự kiện bắt đầu từ hôm nay đến mốc daysAhead ngày tới, không tính sự kiện Đã hủy
                var events = await _context.SuKiens.AsNoTracking()
                    .Where(e => e.NgayBatDau >= today && e.NgayBatDau <= maxDate && e.TrangThai != "Đã hủy" && e.TrangThai != "Hủy")
                    .OrderBy(e => e.NgayBatDau)
                    .ToListAsync();

                var result = await BuildDashboardEventDtosAsync(events);
                return ServiceResult<List<UpcomingEventDto>>.Success(result, $"Lấy thành công {result.Count} sự kiện sắp diễn ra.");
            }
            catch (Exception ex)
            {
                return ServiceResult<List<UpcomingEventDto>>.Failed($"Lỗi lấy danh sách sự kiện sắp diễn ra: {ex.Message}");
            }
        }

        public ServiceResult<List<UpcomingEventDto>> GetUpcomingEvents(int daysAhead = 7)
        {
            return Task.Run(() => GetUpcomingEventsAsync(daysAhead)).GetAwaiter().GetResult();
        }

        public async Task<ServiceResult<List<UpcomingEventDto>>> GetAllEventsAsync()
        {
            try
            {
                var now = DateTime.Now;
                var events = await _context.SuKiens.AsNoTracking().ToListAsync();

                // Sắp xếp theo thời gian từ gần nhất tới xa nhất so với hiện tại
                var orderedEvents = events
                    .OrderBy(e => Math.Abs((e.NgayBatDau - now).Ticks))
                    .ThenBy(e => e.NgayBatDau)
                    .ToList();

                var result = await BuildDashboardEventDtosAsync(orderedEvents);
                return ServiceResult<List<UpcomingEventDto>>.Success(result, $"Lấy thành công {result.Count} sự kiện trong hệ thống.");
            }
            catch (Exception ex)
            {
                return ServiceResult<List<UpcomingEventDto>>.Failed($"Lỗi lấy danh sách sự kiện: {ex.Message}");
            }
        }

        public ServiceResult<List<UpcomingEventDto>> GetAllEvents()
        {
            return Task.Run(GetAllEventsAsync).GetAwaiter().GetResult();
        }

        private async Task<List<UpcomingEventDto>> BuildDashboardEventDtosAsync(List<SuKien> events)
        {
            var ticketTiers = await _context.HangVes.AsNoTracking().ToListAsync();
            var dangKys = await _context.DangKys.AsNoTracking().ToListAsync();
            var ticketDetails = await _context.ChiTietVes.AsNoTracking().ToListAsync();
            var checkIns = await _context.CheckIns.AsNoTracking().ToListAsync();
            var venues = await _context.DiaDiems.AsNoTracking()
                .ToDictionaryAsync(v => v.MaDiaDiem, v => v.TenDiaDiem);

            var checkedInTicketIds = checkIns.Select(c => c.MaVe).ToHashSet();
            var result = new List<UpcomingEventDto>();

            foreach (var ev in events)
            {
                var tiersOfEvent = ticketTiers.Where(t => t.MaSuKien == ev.MaSuKien).ToList();
                int sold = tiersOfEvent.Sum(t => t.SoLuongDaBan);

                var eventDangKyIds = dangKys.Where(d => d.MaSuKien == ev.MaSuKien).Select(d => d.MaDangKy).ToHashSet();
                var ticketsOfEvent = ticketDetails.Where(d => eventDangKyIds.Contains(d.MaDangKy)).ToList();
                int checkinCount = ticketsOfEvent.Count(d => checkedInTicketIds.Contains(d.MaVe));

                double ratio = sold > 0 ? Math.Round(Math.Min(100.0, ((double)checkinCount / sold) * 100.0), 1) : 0.0;
                string venueName = venues.TryGetValue(ev.MaDiaDiem, out var tenDiaDiem) ? tenDiaDiem : ev.MaDiaDiem;

                result.Add(new UpcomingEventDto
                {
                    MaSuKien = ev.MaSuKien,
                    TenSuKien = ev.TenSuKien,
                    NgayBatDau = ev.NgayBatDau,
                    NgayKetThuc = ev.NgayKetThuc,
                    DiaDiem = venueName,
                    SoVeDaBan = sold,
                    SoNguoiCheckIn = checkinCount,
                    TyLeCheckIn = ratio,
                    TrangThai = ev.TrangThai
                });
            }

            return result;
        }

        public async Task<ServiceResult<List<EventRevenueChartDto>>> GetEventRevenueComparisonAsync(int limit = 5)
        {
            try
            {
                var events = await _context.SuKiens.AsNoTracking()
                    .Where(e => e.TrangThai != "Đã hủy" && e.TrangThai != "Hủy")
                    .Take(Math.Max(1, limit))
                    .ToListAsync();

                var ticketTiers = await _context.HangVes.AsNoTracking().ToListAsync();
                var expenses = await _context.ChiPhiSuKiens.AsNoTracking().ToListAsync();

                var list = new List<EventRevenueChartDto>();
                foreach (var ev in events)
                {
                    decimal rev = ticketTiers.Where(t => t.MaSuKien == ev.MaSuKien).Sum(t => t.SoLuongDaBan * t.GiaVe);
                    decimal exp = expenses.Where(c => c.MaSuKien == ev.MaSuKien).Sum(c => c.SoTien);

                    list.Add(new EventRevenueChartDto
                    {
                        MaSuKien = ev.MaSuKien,
                        TenSuKien = ev.TenSuKien,
                        DoanhThu = rev,
                        ChiPhi = exp
                    });
                }

                return ServiceResult<List<EventRevenueChartDto>>.Success(list, "Lấy dữ liệu so sánh doanh thu thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult<List<EventRevenueChartDto>>.Failed($"Lỗi lấy dữ liệu doanh thu: {ex.Message}");
            }
        }

        public ServiceResult<List<EventRevenueChartDto>> GetEventRevenueComparison(int limit = 5)
        {
            return Task.Run(() => GetEventRevenueComparisonAsync(limit)).GetAwaiter().GetResult();
        }

        public async Task<ServiceResult<List<MonthlyEventCountDto>>> GetMonthlyEventDistributionAsync(int year)
        {
            try
            {
                var eventsInYear = await _context.SuKiens.AsNoTracking()
                    .Where(e => e.NgayBatDau.Year == year)
                    .ToListAsync();

                var list = new List<MonthlyEventCountDto>();
                for (int m = 1; m <= 12; m++)
                {
                    list.Add(new MonthlyEventCountDto
                    {
                        Thang = m,
                        SoLuongSuKien = eventsInYear.Count(e => e.NgayBatDau.Month == m)
                    });
                }

                return ServiceResult<List<MonthlyEventCountDto>>.Success(list, $"Lấy dữ liệu sự kiện năm {year} thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult<List<MonthlyEventCountDto>>.Failed($"Lỗi lấy dữ liệu sự kiện theo tháng: {ex.Message}");
            }
        }

        public ServiceResult<List<MonthlyEventCountDto>> GetMonthlyEventDistribution(int year)
        {
            return Task.Run(() => GetMonthlyEventDistributionAsync(year)).GetAwaiter().GetResult();
        }

        public async Task<ServiceResult<CheckInRatioDto>> GetCheckInRatioAsync(string? maSuKien = null)
        {
            try
            {
                int sold = 0;
                int checkedIn = 0;

                if (string.IsNullOrWhiteSpace(maSuKien))
                {
                    var tiers = await _context.HangVes.AsNoTracking().ToListAsync();
                    sold = tiers.Sum(t => t.SoLuongDaBan);
                    checkedIn = await _context.CheckIns.AsNoTracking().CountAsync();
                }
                else
                {
                    var tiers = await _context.HangVes.AsNoTracking()
                        .Where(t => t.MaSuKien == maSuKien)
                        .ToListAsync();
                    sold = tiers.Sum(t => t.SoLuongDaBan);

                    var eventDangKyIds = await _context.DangKys.AsNoTracking()
                        .Where(d => d.MaSuKien == maSuKien)
                        .Select(d => d.MaDangKy)
                        .ToListAsync();

                    var ticketIds = await _context.ChiTietVes.AsNoTracking()
                        .Where(d => eventDangKyIds.Contains(d.MaDangKy))
                        .Select(d => d.MaVe)
                        .ToListAsync();

                    var ticketIdSet = ticketIds.ToHashSet();
                    checkedIn = await _context.CheckIns.AsNoTracking()
                        .Where(c => ticketIdSet.Contains(c.MaVe))
                        .CountAsync();
                }

                int notCheckedIn = Math.Max(0, sold - checkedIn);
                double ratio = sold > 0 ? Math.Round(Math.Min(100.0, ((double)checkedIn / sold) * 100.0), 1) : 0.0;

                var dto = new CheckInRatioDto
                {
                    DaCheckIn = checkedIn,
                    ChuaCheckIn = notCheckedIn,
                    TyLePhanTram = ratio
                };

                return ServiceResult<CheckInRatioDto>.Success(dto, "Lấy tỷ lệ check-in thành công.");
            }
            catch (Exception ex)
            {
                return ServiceResult<CheckInRatioDto>.Failed($"Lỗi lấy tỷ lệ check-in: {ex.Message}");
            }
        }

        public ServiceResult<CheckInRatioDto> GetCheckInRatio(string? maSuKien = null)
        {
            return Task.Run(() => GetCheckInRatioAsync(maSuKien)).GetAwaiter().GetResult();
        }
    }
}
