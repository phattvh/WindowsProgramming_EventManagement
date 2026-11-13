using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt nghiệp vụ Quản lý sự kiện tổng thể (F04 / UC04)
    /// Đảm bảo các quy tắc nghiệp vụ:
    /// - BR-02: Ràng buộc xóa sự kiện (chỉ xóa khi chưa có dữ liệu liên kết, nếu có phải chuyển sang 'Đã hủy').
    /// - BR-03: Logic thời gian sự kiện (NgayKetThuc >= NgayBatDau).
    /// - BR-04: Chống trùng lịch địa điểm (Interval Overlap: Start_A < End_B && End_A > Start_B).
    /// - BR-17: Đề xuất mã sự kiện tự động theo chuẩn quy ước.
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-12 / Ngày 7)
    /// </summary>
    public class EventService : IEventService
    {
        private readonly EventValidationService _validationService;
        private readonly IEventTypeService _eventTypeService;
        private readonly IVenueService _venueService;

        // Danh sách lưu trữ bộ nhớ đệm cho tầng BLL
        private static readonly List<EventListDto> _events = new();

        private void EnsureLoadedFromDb()
        {
            if (_events.Count == 0)
            {
                try
                {
                    using var db = new EventManagement.DAL.AppDbContext();
                    var list = (from s in db.SuKiens
                                join l in db.LoaiSuKiens on s.MaLoaiSK equals l.MaLoaiSK into lj
                                from l in lj.DefaultIfEmpty()
                                join d in db.DiaDiems on s.MaDiaDiem equals d.MaDiaDiem into dj
                                from d in dj.DefaultIfEmpty()
                                select new EventListDto
                                {
                                    MaSuKien = s.MaSuKien,
                                    TenSuKien = s.TenSuKien,
                                    MaLoaiSK = s.MaLoaiSK,
                                    TenLoaiSK = l != null ? l.TenLoaiSK : s.MaLoaiSK,
                                    MaDiaDiem = s.MaDiaDiem,
                                    TenDiaDiem = d != null ? d.TenDiaDiem : s.MaDiaDiem,
                                    NgayBatDau = s.NgayBatDau,
                                    NgayKetThuc = s.NgayKetThuc,
                                    SucChua = s.SucChua,
                                    NganSachDuKien = s.NganSachDuKien,
                                    NguoiPhuTrach = s.NguoiPhuTrach,
                                    TrangThai = s.TrangThai,
                                    SoVeDaBan = db.ChiTietVes.Count(c => db.DangKys.Any(dk => dk.MaDangKy == c.MaDangKy && dk.MaSuKien == s.MaSuKien)),
                                    SoKhachCheckIn = db.CheckIns.Count(ci => db.ChiTietVes.Any(c => c.MaVe == ci.MaVe && db.DangKys.Any(dk => dk.MaDangKy == c.MaDangKy && dk.MaSuKien == s.MaSuKien))),
                                    MoTa = s.MoTa
                                }).ToList();

                    if (list.Any())
                    {
                        _events.AddRange(list);
                    }
                }
                catch { }
            }
        }

        public EventService() : this(new EventValidationService(), new EventTypeService(), new VenueService())
        {
        }

        public EventService(
            EventValidationService validationService,
            IEventTypeService eventTypeService,
            IVenueService venueService)
        {
            _validationService = validationService ?? new EventValidationService();
            _eventTypeService = eventTypeService ?? new EventTypeService();
            _venueService = venueService ?? new VenueService();
        }

        public ServiceResult<List<EventListDto>> GetAll(EventFilterDto filter)
        {
            if (filter == null)
            {
                return GetAll(null, null, null, null, null, null);
            }

            return GetAll(
                filter.TuKhoa,
                filter.MaLoaiSK,
                filter.TrangThai,
                filter.MaDiaDiem,
                filter.TuNgay,
                filter.DenNgay);
        }

        public ServiceResult<List<EventListDto>> GetAll(
            string? keyword = null,
            string? maLoaiSK = null,
            string? trangThai = null,
            string? maDiaDiem = null,
            DateTime? tuNgay = null,
            DateTime? denNgay = null)
        {
            EnsureLoadedFromDb();
            var query = _events.AsEnumerable();

            // 1. Tìm kiếm đa điều kiện theo từ khóa (Mã, Tên, Người phụ trách, Tên loại, Tên địa điểm, Mô tả)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(x =>
                    x.MaSuKien.ToLower().Contains(kw) ||
                    x.TenSuKien.ToLower().Contains(kw) ||
                    (!string.IsNullOrEmpty(x.NguoiPhuTrach) && x.NguoiPhuTrach.ToLower().Contains(kw)) ||
                    (!string.IsNullOrEmpty(x.TenLoaiSK) && x.TenLoaiSK.ToLower().Contains(kw)) ||
                    (!string.IsNullOrEmpty(x.TenDiaDiem) && x.TenDiaDiem.ToLower().Contains(kw)) ||
                    (!string.IsNullOrEmpty(x.MoTa) && x.MoTa.ToLower().Contains(kw)));
            }

            // 2. Lọc theo Loại sự kiện
            if (!string.IsNullOrWhiteSpace(maLoaiSK) && !maLoaiSK.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.MaLoaiSK.Equals(maLoaiSK, StringComparison.OrdinalIgnoreCase) ||
                                         x.TenLoaiSK.Equals(maLoaiSK, StringComparison.OrdinalIgnoreCase));
            }

            // 3. Lọc theo Trạng thái sự kiện
            if (!string.IsNullOrWhiteSpace(trangThai) && !trangThai.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase));
            }

            // 4. Lọc theo Địa điểm
            if (!string.IsNullOrWhiteSpace(maDiaDiem) && !maDiaDiem.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.MaDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase) ||
                                         x.TenDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase));
            }

            // 5. Lọc theo Khoảng thời gian
            if (tuNgay.HasValue)
            {
                query = query.Where(x => x.NgayKetThuc >= tuNgay.Value.Date);
            }

            if (denNgay.HasValue)
            {
                DateTime endOfDay = denNgay.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.NgayBatDau <= endOfDay);
            }

            // Sắp xếp: sự kiện mới hơn hoặc theo ngày bắt đầu tăng dần
            var result = query.OrderBy(x => x.NgayBatDau).ToList();
            return ServiceResult<List<EventListDto>>.Success(result);
        }

        public ServiceResult<EventListDto> GetById(string maSuKien)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maSuKien))
            {
                return ServiceResult<EventListDto>.Failed("Mã sự kiện không được để trống!");
            }

            var item = _events.FirstOrDefault(x => x.MaSuKien.Equals(maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<EventListDto>.Failed($"Không tìm thấy sự kiện có mã '{maSuKien}'!");
            }

            // Trả về bản sao (clone) để bảo vệ trạng thái nội bộ không bị thay đổi ngoài ý muốn
            var clone = new EventListDto
            {
                MaSuKien = item.MaSuKien,
                TenSuKien = item.TenSuKien,
                MaLoaiSK = item.MaLoaiSK,
                TenLoaiSK = item.TenLoaiSK,
                MaDiaDiem = item.MaDiaDiem,
                TenDiaDiem = item.TenDiaDiem,
                NgayBatDau = item.NgayBatDau,
                NgayKetThuc = item.NgayKetThuc,
                SucChua = item.SucChua,
                NganSachDuKien = item.NganSachDuKien,
                NguoiPhuTrach = item.NguoiPhuTrach,
                TrangThai = item.TrangThai,
                SoVeDaBan = item.SoVeDaBan,
                SoKhachCheckIn = item.SoKhachCheckIn,
                MoTa = item.MoTa
            };

            return ServiceResult<EventListDto>.Success(clone);
        }

        public ServiceResult Add(EventListDto dto)
        {
            EnsureLoadedFromDb();
            if (dto == null)
            {
                return ServiceResult.Failed("Dữ liệu sự kiện không hợp lệ!");
            }

            // 1. Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(dto.MaSuKien))
            {
                return ServiceResult.Failed("Mã sự kiện không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.TenSuKien))
            {
                return ServiceResult.Failed("Tên sự kiện không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.MaLoaiSK))
            {
                return ServiceResult.Failed("Vui lòng chọn loại sự kiện!");
            }

            if (string.IsNullOrWhiteSpace(dto.MaDiaDiem))
            {
                return ServiceResult.Failed("Vui lòng chọn địa điểm tổ chức!");
            }

            string code = dto.MaSuKien.Trim().ToUpper();
            if (_events.Any(x => x.MaSuKien.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult.Failed($"Mã sự kiện '{code}' đã tồn tại trong hệ thống!");
            }

            // 2. Thuật toán BR-03: Kiểm tra logic thời gian sự kiện
            var dateValidation = _validationService.ValidateEventDates(dto.NgayBatDau, dto.NgayKetThuc, isNew: false);
            if (!dateValidation.IsSuccess)
            {
                return ServiceResult.Failed(dateValidation.Message, "BR-03");
            }

            // 3. Thuật toán BR-04: Chống trùng lịch địa điểm giữa các sự kiện
            var venueCheck = CheckVenueConflict(dto.MaDiaDiem, dto.NgayBatDau, dto.NgayKetThuc, excludeMaSuKien: null);
            if (!venueCheck.IsSuccess)
            {
                return venueCheck;
            }

            // 4. Kiểm tra sức chứa và ngân sách
            if (dto.SucChua <= 0)
            {
                return ServiceResult.Failed("Sức chứa sự kiện phải lớn hơn 0!");
            }

            if (dto.NganSachDuKien < 0)
            {
                return ServiceResult.Failed("Ngân sách dự kiến không được âm!");
            }

            // 5. Kiểm tra sức chứa tối đa của địa điểm
            var venue = _venueService.GetById(dto.MaDiaDiem);
            if (venue.IsSuccess && venue.Data != null && venue.Data.SucChuaToiDa > 0 && dto.SucChua > venue.Data.SucChuaToiDa)
            {
                return ServiceResult.Failed(
                    $"Sức chứa dự kiến ({dto.SucChua}) vượt quá sức chứa tối đa của địa điểm '{venue.Data.TenDiaDiem}' ({venue.Data.SucChuaToiDa} người)!");
            }

            // Ánh xạ tên loại và tên địa điểm
            string tenLoai = dto.TenLoaiSK;
            if (string.IsNullOrWhiteSpace(tenLoai))
            {
                var eventType = _eventTypeService.GetById(dto.MaLoaiSK);
                tenLoai = eventType.IsSuccess && eventType.Data != null ? eventType.Data.TenLoaiSK : dto.MaLoaiSK;
            }

            string tenDiaDiem = dto.TenDiaDiem;
            if (string.IsNullOrWhiteSpace(tenDiaDiem) && venue.IsSuccess && venue.Data != null)
            {
                tenDiaDiem = venue.Data.TenDiaDiem;
            }

            var newEntity = new EventListDto
            {
                MaSuKien = code,
                TenSuKien = dto.TenSuKien.Trim(),
                MaLoaiSK = dto.MaLoaiSK.Trim(),
                TenLoaiSK = tenLoai,
                MaDiaDiem = dto.MaDiaDiem.Trim(),
                TenDiaDiem = tenDiaDiem,
                NgayBatDau = dto.NgayBatDau,
                NgayKetThuc = dto.NgayKetThuc,
                SucChua = dto.SucChua,
                NganSachDuKien = dto.NganSachDuKien,
                NguoiPhuTrach = string.IsNullOrWhiteSpace(dto.NguoiPhuTrach) ? "Chưa phân công" : dto.NguoiPhuTrach.Trim(),
                TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "Chuẩn bị" : dto.TrangThai.Trim(),
                SoVeDaBan = 0,
                SoKhachCheckIn = 0,
                MoTa = dto.MoTa?.Trim()
            };

            _events.Add(newEntity);
            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var entity = new EventManagement.Core.Entities.SuKien
                {
                    MaSuKien = newEntity.MaSuKien,
                    TenSuKien = newEntity.TenSuKien,
                    MaLoaiSK = newEntity.MaLoaiSK,
                    MoTa = newEntity.MoTa,
                    NgayBatDau = newEntity.NgayBatDau,
                    NgayKetThuc = newEntity.NgayKetThuc,
                    MaDiaDiem = newEntity.MaDiaDiem,
                    SucChua = newEntity.SucChua,
                    NganSachDuKien = newEntity.NganSachDuKien,
                    NguoiPhuTrach = newEntity.NguoiPhuTrach,
                    TrangThai = newEntity.TrangThai
                };
                db.SuKiens.Add(entity);
                db.SaveChanges();
            }
            catch { }

            EventManagement.Core.Common.DataSyncNotifier.NotifyEventChanged(newEntity.MaSuKien, "Added");
            return ServiceResult.Success("Thêm mới sự kiện thành công!");
        }

        public ServiceResult Update(EventListDto dto)
        {
            EnsureLoadedFromDb();
            if (dto == null || string.IsNullOrWhiteSpace(dto.MaSuKien))
            {
                return ServiceResult.Failed("Mã sự kiện không hợp lệ!");
            }

            var existing = _events.FirstOrDefault(x => x.MaSuKien.Equals(dto.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy sự kiện '{dto.MaSuKien}' để cập nhật!");
            }

            if (string.IsNullOrWhiteSpace(dto.TenSuKien))
            {
                return ServiceResult.Failed("Tên sự kiện không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.MaLoaiSK))
            {
                return ServiceResult.Failed("Vui lòng chọn loại sự kiện!");
            }

            if (string.IsNullOrWhiteSpace(dto.MaDiaDiem))
            {
                return ServiceResult.Failed("Vui lòng chọn địa điểm tổ chức!");
            }

            // 1. Thuật toán BR-03: Logic thời gian sự kiện
            var dateValidation = _validationService.ValidateEventDates(dto.NgayBatDau, dto.NgayKetThuc, isNew: false);
            if (!dateValidation.IsSuccess)
            {
                return ServiceResult.Failed(dateValidation.Message, "BR-03");
            }

            // 2. Thuật toán BR-04: Chống trùng lịch địa điểm giữa các sự kiện
            var venueCheck = CheckVenueConflict(dto.MaDiaDiem, dto.NgayBatDau, dto.NgayKetThuc, excludeMaSuKien: existing.MaSuKien);
            if (!venueCheck.IsSuccess)
            {
                return venueCheck;
            }

            if (dto.SucChua <= 0)
            {
                return ServiceResult.Failed("Sức chứa sự kiện phải lớn hơn 0!");
            }

            if (dto.NganSachDuKien < 0)
            {
                return ServiceResult.Failed("Ngân sách dự kiến không được âm!");
            }

            var venue = _venueService.GetById(dto.MaDiaDiem);
            if (venue.IsSuccess && venue.Data != null && venue.Data.SucChuaToiDa > 0 && dto.SucChua > venue.Data.SucChuaToiDa)
            {
                return ServiceResult.Failed(
                    $"Sức chứa dự kiến ({dto.SucChua}) vượt quá sức chứa tối đa của địa điểm '{venue.Data.TenDiaDiem}' ({venue.Data.SucChuaToiDa} người)!");
            }

            string tenLoai = dto.TenLoaiSK;
            if (string.IsNullOrWhiteSpace(tenLoai))
            {
                var eventType = _eventTypeService.GetById(dto.MaLoaiSK);
                tenLoai = eventType.IsSuccess && eventType.Data != null ? eventType.Data.TenLoaiSK : dto.MaLoaiSK;
            }

            string tenDiaDiem = dto.TenDiaDiem;
            if (string.IsNullOrWhiteSpace(tenDiaDiem) && venue.IsSuccess && venue.Data != null)
            {
                tenDiaDiem = venue.Data.TenDiaDiem;
            }

            existing.TenSuKien = dto.TenSuKien.Trim();
            existing.MaLoaiSK = dto.MaLoaiSK.Trim();
            existing.TenLoaiSK = tenLoai;
            existing.MaDiaDiem = dto.MaDiaDiem.Trim();
            existing.TenDiaDiem = tenDiaDiem;
            existing.NgayBatDau = dto.NgayBatDau;
            existing.NgayKetThuc = dto.NgayKetThuc;
            existing.SucChua = dto.SucChua;
            existing.NganSachDuKien = dto.NganSachDuKien;
            if (!string.IsNullOrWhiteSpace(dto.NguoiPhuTrach))
            {
                existing.NguoiPhuTrach = dto.NguoiPhuTrach.Trim();
            }
            if (!string.IsNullOrWhiteSpace(dto.TrangThai))
            {
                existing.TrangThai = dto.TrangThai.Trim();
            }
            existing.MoTa = dto.MoTa?.Trim();

            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var sk = db.SuKiens.FirstOrDefault(x => x.MaSuKien == existing.MaSuKien);
                if (sk != null)
                {
                    sk.TenSuKien = existing.TenSuKien;
                    sk.MaLoaiSK = existing.MaLoaiSK;
                    sk.MoTa = existing.MoTa;
                    sk.NgayBatDau = existing.NgayBatDau;
                    sk.NgayKetThuc = existing.NgayKetThuc;
                    sk.MaDiaDiem = existing.MaDiaDiem;
                    sk.SucChua = existing.SucChua;
                    sk.NganSachDuKien = existing.NganSachDuKien;
                    sk.NguoiPhuTrach = existing.NguoiPhuTrach;
                    sk.TrangThai = existing.TrangThai;
                    db.SaveChanges();
                }
            }
            catch { }

            EventManagement.Core.Common.DataSyncNotifier.NotifyEventChanged(existing.MaSuKien, "Updated");
            return ServiceResult.Success("Cập nhật thông tin sự kiện thành công!");
        }

        public ServiceResult Delete(string maSuKien)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maSuKien))
            {
                return ServiceResult.Failed("Mã sự kiện không được để trống!");
            }

            var existing = _events.FirstOrDefault(x => x.MaSuKien.Equals(maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy sự kiện '{maSuKien}'!");
            }

            // Quy tắc nghiệp vụ BR-02:
            // Chỉ cho phép xóa khi chưa phát sinh dữ liệu liên kết (vé bán, người đăng ký, check-in, chi phí, phản hồi).
            // Nếu đã có vé bán > 0 hoặc đã check-in, chặn xóa và yêu cầu chuyển sang 'Đã hủy'.
            if (existing.SoVeDaBan > 0 || existing.SoKhachCheckIn > 0)
            {
                return ServiceResult.Failed(
                    $"Sự kiện '{existing.TenSuKien}' đã phát sinh dữ liệu liên kết ({existing.SoVeDaBan} vé đã bán, {existing.SoKhachCheckIn} lượt check-in). Không thể xóa vĩnh viễn khỏi hệ thống! Vui lòng chuyển trạng thái sự kiện sang 'Đã hủy'.",
                    "BR-02");
            }

            _events.Remove(existing);
            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var sk = db.SuKiens.FirstOrDefault(x => x.MaSuKien == existing.MaSuKien);
                if (sk != null)
                {
                    db.SuKiens.Remove(sk);
                    db.SaveChanges();
                }
            }
            catch { }

            EventManagement.Core.Common.DataSyncNotifier.NotifyEventChanged(existing.MaSuKien, "Deleted");
            return ServiceResult.Success($"Đã xóa vĩnh viễn sự kiện '{existing.TenSuKien}' ({existing.MaSuKien}) khỏi hệ thống!");
        }

        public ServiceResult CancelEvent(string maSuKien)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maSuKien))
            {
                return ServiceResult.Failed("Mã sự kiện không được để trống!");
            }

            var existing = _events.FirstOrDefault(x => x.MaSuKien.Equals(maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy sự kiện '{maSuKien}'!");
            }

            if (string.Equals(existing.TrangThai, "Đang diễn ra", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(existing.TrangThai, "Đã kết thúc", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(existing.TrangThai, "Đã hoàn thành", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult.Failed("Sự kiện Đang diễn ra hoặc Đã kết thúc không được phép hủy (không thể thực hiện thao tác hủy)!");
            }
            existing.TrangThai = "Đã hủy";
            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var sk = db.SuKiens.FirstOrDefault(x => x.MaSuKien == existing.MaSuKien);
                if (sk != null)
                {
                    sk.TrangThai = "Đã hủy";
                    db.SaveChanges();
                }
            }
            catch { }

            EventManagement.Core.Common.DataSyncNotifier.NotifyEventChanged(existing.MaSuKien, "Cancelled");
            return ServiceResult.Success($"Đã chuyển trạng thái sự kiện '{existing.TenSuKien}' sang 'Đã hủy'!");
        }

        public string SuggestNextCode()
        {
            EnsureLoadedFromDb();
            int currentYear = DateTime.Now.Year;
            string prefix = $"EVT{currentYear}";

            int maxIndex = 0;
            foreach (var ev in _events)
            {
                if (ev.MaSuKien.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    string suffix = ev.MaSuKien.Substring(prefix.Length);
                    if (int.TryParse(suffix, out int idx) && idx > maxIndex)
                    {
                        maxIndex = idx;
                    }
                }
                else if (Regex.IsMatch(ev.MaSuKien, @"^EVT\d+$"))
                {
                    string numPart = ev.MaSuKien.Substring(3);
                    if (int.TryParse(numPart, out int num) && num > maxIndex)
                    {
                        maxIndex = num;
                    }
                }
            }

            return $"{prefix}{(maxIndex + 1):D2}";
        }

        public List<EventTypeDto> GetAvailableEventTypes()
        {
            var res = _eventTypeService.GetAll();
            return res.IsSuccess && res.Data != null ? res.Data : new List<EventTypeDto>();
        }

        public List<VenueDto> GetAvailableVenues()
        {
            var res = _venueService.GetAll();
            return res.IsSuccess && res.Data != null ? res.Data : new List<VenueDto>();
        }

        private ServiceResult CheckVenueConflict(string maDiaDiem, DateTime start, DateTime end, string? excludeMaSuKien)
        {
            EnsureLoadedFromDb();
            // Lấy danh sách các sự kiện tại địa điểm này (loại trừ các sự kiện đã hủy và sự kiện đang cập nhật)
            var conflictingEvent = _events
                .Where(e => e.MaDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase))
                .Where(e => !e.TrangThai.Equals("Đã hủy", StringComparison.OrdinalIgnoreCase) &&
                            !e.TrangThai.Equals("Hủy", StringComparison.OrdinalIgnoreCase))
                .Where(e => excludeMaSuKien == null || !e.MaSuKien.Equals(excludeMaSuKien, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault(e => start < e.NgayKetThuc && end > e.NgayBatDau);

            if (conflictingEvent != null)
            {
                return ServiceResult.Failed(
                    $"Địa điểm '{conflictingEvent.TenDiaDiem}' đã được xếp lịch cho sự kiện '{conflictingEvent.TenSuKien}' ({conflictingEvent.MaSuKien}) từ {conflictingEvent.NgayBatDau:dd/MM/yyyy HH:mm} đến {conflictingEvent.NgayKetThuc:dd/MM/yyyy HH:mm}. Khoảng thời gian yêu cầu ({start:dd/MM/yyyy HH:mm} - {end:dd/MM/yyyy HH:mm}) bị trùng lặp!",
                    "BR-04");
            }

            return ServiceResult.Success();
        }
    }
}
