using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt nghiệp vụ Phân công nhân sự theo ca trực (F15 / UC15)
    /// Tích hợp thuật toán lõi BR-13 của Thành viên 3 (StaffScheduleService)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-13/Day 6)
    /// </summary>
    public class StaffAssignmentService : IStaffAssignmentService
    {
        private readonly StaffScheduleService _shiftValidator;
        private readonly IStaffService _staffService;

        private static readonly List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> _events = new();
        private static readonly List<StaffAssignmentDto> _assignments = new();

        private void EnsureLoadedFromDb()
        {
            if (_assignments.Count == 0 && _events.Count == 0)
            {
                try
                {
                    using var db = new EventManagement.DAL.AppDbContext();
                    var evts = db.SuKiens.Select(s => new { s.MaSuKien, s.TenSuKien, s.NgayBatDau, s.NgayKetThuc }).ToList();
                    foreach (var e in evts)
                    {
                        if (!_events.Any(x => x.MaSuKien == e.MaSuKien))
                            _events.Add((e.MaSuKien, e.TenSuKien, e.NgayBatDau, e.NgayKetThuc));
                    }

                    var assigns = db.PhanCongNhanSus.Select(p => new StaffAssignmentDto
                    {
                        MaPhanCong = p.MaPhanCong,
                        MaSuKien = p.MaSuKien,
                        TenSuKien = p.SuKien != null ? p.SuKien.TenSuKien : p.MaSuKien,
                        MaNhanSu = p.MaNhanSu,
                        TenNhanSu = p.NhanSu != null ? p.NhanSu.HoTen : p.MaNhanSu,
                        SoDienThoai = p.NhanSu != null ? p.NhanSu.SoDienThoai : "",
                        ChucVuNhanSu = p.NhanSu != null ? p.NhanSu.ChucVu : "",
                        PhongBan = p.NhanSu != null ? p.NhanSu.ChucVu : "",
                        NhiemVu = p.NhiemVu,
                        ThoiGianBatDau = p.ThoiGianBatDau,
                        ThoiGianKetThuc = p.ThoiGianKetThuc,
                        GhiChu = p.GhiChu
                    }).ToList();

                    if (assigns.Any())
                    {
                        _assignments.AddRange(assigns);
                    }
                }
                catch { }
            }
        }

        public StaffAssignmentService() : this(new StaffScheduleService(), new StaffService())
        {
        }

        public StaffAssignmentService(StaffScheduleService shiftValidator, IStaffService staffService)
        {
            _shiftValidator = shiftValidator ?? new StaffScheduleService();
            _staffService = staffService ?? new StaffService();
        }

        public ServiceResult<List<StaffAssignmentDto>> GetAll(
            string? maSuKien = null,
            string? keyword = null,
            string? maNhanSu = null,
            string? phongBan = null)
        {
            EnsureLoadedFromDb();
            IEnumerable<StaffAssignmentDto> query = _assignments;

            // 1. Lọc theo Sự kiện
            if (!string.IsNullOrWhiteSpace(maSuKien) && !maSuKien.StartsWith("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(a => a.MaSuKien.Equals(maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // 2. Lọc theo Nhân sự
            if (!string.IsNullOrWhiteSpace(maNhanSu) && !maNhanSu.StartsWith("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(a => a.MaNhanSu.Equals(maNhanSu.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // 3. Lọc theo Phòng ban
            if (!string.IsNullOrWhiteSpace(phongBan) && !phongBan.StartsWith("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(a => a.PhongBan.Equals(phongBan.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // 4. Tìm kiếm từ khóa đa trường
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(a =>
                    a.MaPhanCong.ToLower().Contains(kw) ||
                    a.TenNhanSu.ToLower().Contains(kw) ||
                    a.NhiemVu.ToLower().Contains(kw) ||
                    a.SoDienThoai.Contains(kw) ||
                    (a.GhiChu != null && a.GhiChu.ToLower().Contains(kw))
                );
            }

            var result = query
                .OrderBy(a => a.MaSuKien)
                .ThenBy(a => a.ThoiGianBatDau)
                .Select(CloneDto)
                .ToList();

            return ServiceResult<List<StaffAssignmentDto>>.Success(result);
        }

        public ServiceResult<StaffAssignmentDto> GetById(string maPhanCong)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maPhanCong))
            {
                return ServiceResult<StaffAssignmentDto>.Failed("Mã phân công không được để trống.");
            }

            var item = _assignments.FirstOrDefault(a => a.MaPhanCong.Equals(maPhanCong.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<StaffAssignmentDto>.Failed($"Không tìm thấy phân công có mã '{maPhanCong}'.");
            }

            return ServiceResult<StaffAssignmentDto>.Success(CloneDto(item));
        }

        public static bool IsEventEnded(string? maSuKien)
        {
            if (string.IsNullOrWhiteSpace(maSuKien)) return false;
            var code = maSuKien.Trim();

            try
            {
                using var context = new EventManagement.DAL.AppDbContext();
                var evt = context.SuKiens.FirstOrDefault(e => e.MaSuKien == code);
                if (evt != null)
                {
                    if (evt.TrangThai == "Đã kết thúc" || evt.TrangThai == "Đã hoàn thành" || evt.NgayKetThuc < DateTime.Now)
                        return true;
                }
            }
            catch { }

            var evInMem = _events.FirstOrDefault(e => e.MaSuKien.Equals(code, StringComparison.OrdinalIgnoreCase));
            if (evInMem != default && evInMem.KetThuc < DateTime.Now)
                return true;

            return false;
        }

        public ServiceResult Add(StaffAssignmentDto dto)
        {
            EnsureLoadedFromDb();
            if (dto == null)
            {
                return ServiceResult.Failed("Dữ liệu phân công không được để trống.");
            }

            if (IsEventEnded(dto.MaSuKien))
            {
                return ServiceResult.Failed("Không thể phân công nhân sự vào sự kiện đã kết thúc!");
            }

            if (string.IsNullOrWhiteSpace(dto.MaPhanCong))
            {
                return ServiceResult.Failed("Mã phân công không được để trống.");
            }

            if (_assignments.Any(a => a.MaPhanCong.Equals(dto.MaPhanCong.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult.Failed($"Mã phân công '{dto.MaPhanCong.Trim()}' đã tồn tại trong hệ thống.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaSuKien))
            {
                return ServiceResult.Failed("Vui lòng chọn sự kiện áp dụng cho ca trực.");
            }

            if (IsEventEnded(dto.MaSuKien))
            {
                return ServiceResult.Failed("Không thể phân công nhân sự vào sự kiện đã kết thúc!");
            }

            if (string.IsNullOrWhiteSpace(dto.MaNhanSu))
            {
                return ServiceResult.Failed("Vui lòng chọn nhân sự được phân công.");
            }

            if (string.IsNullOrWhiteSpace(dto.NhiemVu))
            {
                return ServiceResult.Failed("Nhiệm vụ phân công không được để trống.");
            }

            if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
            {
                return ServiceResult.Failed("Thời gian kết thúc ca trực phải lớn hơn thời gian bắt đầu.");
            }

            // Kiểm tra quy tắc BR-13 (Chống trùng ca trực & Nhân sự hoạt động)
            var conflictCheck = CheckConflict(dto, null);
            if (!conflictCheck.IsSuccess)
            {
                return ServiceResult.Failed(conflictCheck.Message, conflictCheck.Errors);
            }

            // Điền thông tin sự kiện & nhân sự
            var availableEvts = GetAvailableEvents();
            var evt = availableEvts.FirstOrDefault(e => e.MaSuKien.Equals(dto.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            var staffRes = _staffService.GetById(dto.MaNhanSu.Trim());
            var staff = staffRes.Data;

            var newItem = new StaffAssignmentDto
            {
                MaPhanCong = dto.MaPhanCong.Trim().ToUpper(),
                MaSuKien = dto.MaSuKien.Trim(),
                TenSuKien = evt != default ? evt.TenSuKien : dto.TenSuKien,
                MaNhanSu = dto.MaNhanSu.Trim().ToUpper(),
                TenNhanSu = staff != null ? staff.HoTen : dto.TenNhanSu,
                SoDienThoai = staff != null ? staff.SoDienThoai : dto.SoDienThoai,
                ChucVuNhanSu = staff != null ? staff.ChucDanh : dto.ChucVuNhanSu,
                PhongBan = staff != null ? staff.PhongBan : dto.PhongBan,
                NhiemVu = dto.NhiemVu.Trim(),
                ThoiGianBatDau = dto.ThoiGianBatDau,
                ThoiGianKetThuc = dto.ThoiGianKetThuc,
                GhiChu = string.IsNullOrWhiteSpace(dto.GhiChu) ? null : dto.GhiChu.Trim()
            };

            _assignments.Add(newItem);
            return ServiceResult.Success("Thêm mới phân công ca trực nhân sự thành công!");
        }

        public ServiceResult Update(StaffAssignmentDto dto)
        {
            EnsureLoadedFromDb();
            if (dto == null)
            {
                return ServiceResult.Failed("Dữ liệu phân công không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaPhanCong))
            {
                return ServiceResult.Failed("Mã phân công không được để trống.");
            }

            var existing = _assignments.FirstOrDefault(a => a.MaPhanCong.Equals(dto.MaPhanCong.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy phân công có mã '{dto.MaPhanCong}' để cập nhật.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaSuKien))
            {
                return ServiceResult.Failed("Vui lòng chọn sự kiện áp dụng cho ca trực.");
            }

            if (IsEventEnded(dto.MaSuKien))
            {
                return ServiceResult.Failed("Không thể phân công nhân sự vào sự kiện đã kết thúc!");
            }

            if (string.IsNullOrWhiteSpace(dto.MaNhanSu))
            {
                return ServiceResult.Failed("Vui lòng chọn nhân sự được phân công.");
            }

            if (string.IsNullOrWhiteSpace(dto.NhiemVu))
            {
                return ServiceResult.Failed("Nhiệm vụ phân công không được để trống.");
            }

            if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
            {
                return ServiceResult.Failed("Thời gian kết thúc ca trực phải lớn hơn thời gian bắt đầu.");
            }

            // Kiểm tra quy tắc BR-13 (loại trừ chính mã phân công đang sửa)
            var conflictCheck = CheckConflict(dto, dto.MaPhanCong.Trim());
            if (!conflictCheck.IsSuccess)
            {
                return ServiceResult.Failed(conflictCheck.Message, conflictCheck.Errors);
            }

            var availableEvts = GetAvailableEvents();
            var evt = availableEvts.FirstOrDefault(e => e.MaSuKien.Equals(dto.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            var staffRes = _staffService.GetById(dto.MaNhanSu.Trim());
            var staff = staffRes.Data;

            existing.MaSuKien = dto.MaSuKien.Trim();
            existing.TenSuKien = evt != default ? evt.TenSuKien : dto.TenSuKien;
            existing.MaNhanSu = dto.MaNhanSu.Trim().ToUpper();
            existing.TenNhanSu = staff != null ? staff.HoTen : dto.TenNhanSu;
            existing.SoDienThoai = staff != null ? staff.SoDienThoai : dto.SoDienThoai;
            existing.ChucVuNhanSu = staff != null ? staff.ChucDanh : dto.ChucVuNhanSu;
            existing.PhongBan = staff != null ? staff.PhongBan : dto.PhongBan;
            existing.NhiemVu = dto.NhiemVu.Trim();
            existing.ThoiGianBatDau = dto.ThoiGianBatDau;
            existing.ThoiGianKetThuc = dto.ThoiGianKetThuc;
            existing.GhiChu = string.IsNullOrWhiteSpace(dto.GhiChu) ? null : dto.GhiChu.Trim();

            return ServiceResult.Success("Cập nhật thông tin phân công ca trực thành công!");
        }

        public ServiceResult Delete(string maPhanCong)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maPhanCong))
            {
                return ServiceResult.Failed("Mã phân công không được để trống.");
            }

            var existing = _assignments.FirstOrDefault(a => a.MaPhanCong.Equals(maPhanCong.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy phân công có mã '{maPhanCong}' để xóa.");
            }

            _assignments.Remove(existing);
            return ServiceResult.Success($"Đã xóa thành công ca trực [{maPhanCong}] của nhân sự '{existing.TenNhanSu}'.");
        }

        public ServiceResult<bool> CheckConflict(StaffAssignmentDto dto, string? excludeMaPhanCong = null)
        {
            EnsureLoadedFromDb();
            if (dto == null)
            {
                return ServiceResult<bool>.Failed("Dữ liệu cần kiểm tra không được để trống.");
            }

            if (!string.IsNullOrWhiteSpace(dto.MaSuKien) && IsEventEnded(dto.MaSuKien))
            {
                return ServiceResult<bool>.Failed("Không thể phân công nhân sự vào sự kiện đã kết thúc!");
            }

            if (string.IsNullOrWhiteSpace(dto.MaNhanSu))
            {
                return ServiceResult<bool>.Failed("Mã nhân sự không được để trống khi kiểm tra xung đột.");
            }

            // 1. Kiểm tra trạng thái hoạt động của nhân sự (Quy tắc BR-13)
            var staffRes = _staffService.GetById(dto.MaNhanSu.Trim());
            if (!staffRes.IsSuccess || staffRes.Data == null)
            {
                return ServiceResult<bool>.Failed($"Không tìm thấy nhân sự có mã '{dto.MaNhanSu}' trong hệ thống.");
            }

            if (!staffRes.Data.TrangThai.Equals("Hoạt động", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<bool>.Failed(
                    $"Xung đột phân công: Nhân sự [{dto.MaNhanSu} - {staffRes.Data.HoTen}] hiện đang ở trạng thái '{staffRes.Data.TrangThai}', không thể phân công ca trực.",
                    new List<string> { $"Nhân sự '{staffRes.Data.HoTen}' đã ngừng làm việc hoặc nghỉ việc (Trạng thái = {staffRes.Data.TrangThai})." }
                );
            }

            if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
            {
                return ServiceResult<bool>.Failed(
                    "Thời gian kết thúc ca trực phải lớn hơn thời gian bắt đầu.",
                    new List<string> { $"ThoiGianBatDau ({dto.ThoiGianBatDau:HH:mm}) >= ThoiGianKetThuc ({dto.ThoiGianKetThuc:HH:mm})" }
                );
            }

            // 2. Thuật toán BR-13 kiểm tra trùng ca giao thoa thời gian (Gọi qua StaffScheduleService của TV3)
            var otherAssignments = _assignments.Where(a =>
                string.IsNullOrWhiteSpace(excludeMaPhanCong) ||
                !a.MaPhanCong.Equals(excludeMaPhanCong.Trim(), StringComparison.OrdinalIgnoreCase)
            );

            return _shiftValidator.CheckShiftConflict(
                dto.MaNhanSu.Trim(),
                dto.ThoiGianBatDau,
                dto.ThoiGianKetThuc,
                excludeMaPhanCong,
                otherAssignments
            );
        }

        public List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> GetAvailableEvents()
        {
            EnsureLoadedFromDb();
            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var dbEvents = db.SuKiens.AsNoTracking().OrderBy(e => e.MaSuKien).ToList();
                if (dbEvents.Count > 0)
                {
                    return dbEvents.Select(e => (e.MaSuKien, e.TenSuKien, e.NgayBatDau, e.NgayKetThuc)).ToList();
                }
            }
            catch { }

            try
            {
                var eventService = new EventService();
                var all = eventService.GetAll();
                if (all.IsSuccess && all.Data != null && all.Data.Count > 0)
                {
                    return all.Data.Select(e => (e.MaSuKien, e.TenSuKien, e.NgayBatDau, e.NgayKetThuc)).ToList();
                }
            }
            catch { }

            return _events.ToList();
        }

        public List<StaffDto> GetAvailableStaff()
        {
            var res = _staffService.GetAll();
            return res.IsSuccess && res.Data != null ? res.Data : new List<StaffDto>();
        }

        public string SuggestNextCode()
        {
            EnsureLoadedFromDb();
            int maxId = 0;
            foreach (var item in _assignments)
            {
                if (item.MaPhanCong.StartsWith("PC", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(item.MaPhanCong.Substring(2), out int num))
                {
                    if (num > maxId) maxId = num;
                }
            }

            return $"PC{(maxId + 1):D3}";
        }

        private static StaffAssignmentDto CloneDto(StaffAssignmentDto src)
        {
            return new StaffAssignmentDto
            {
                MaPhanCong = src.MaPhanCong,
                MaSuKien = src.MaSuKien,
                TenSuKien = src.TenSuKien,
                MaNhanSu = src.MaNhanSu,
                TenNhanSu = src.TenNhanSu,
                SoDienThoai = src.SoDienThoai,
                ChucVuNhanSu = src.ChucVuNhanSu,
                PhongBan = src.PhongBan,
                NhiemVu = src.NhiemVu,
                ThoiGianBatDau = src.ThoiGianBatDau,
                ThoiGianKetThuc = src.ThoiGianKetThuc,
                GhiChu = src.GhiChu
            };
        }
    }
}
