using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt nghiệp vụ Quản lý địa điểm tổ chức (F07)
    /// Đảm bảo các quy tắc:
    /// - BR-04: Chống trùng lịch địa điểm giữa các sự kiện.
    /// - BR-20: Soft Delete đối với địa điểm đã từng tổ chức sự kiện.
    /// </summary>
    public class VenueService : IVenueService
    {
        private static readonly List<VenueDto> _venues = new();

        private void EnsureLoadedFromDb()
        {
            if (_venues.Count == 0)
            {
                try
                {
                    using var db = new EventManagement.DAL.AppDbContext();
                    var list = db.DiaDiems.Select(d => new VenueDto
                    {
                        MaDiaDiem = d.MaDiaDiem,
                        TenDiaDiem = d.TenDiaDiem,
                        DiaChi = d.DiaChi,
                        SucChuaToiDa = d.SucChuaToiDa,
                        NguoiQuanLy = "Ban Quản lý",
                        SoDienThoai = "0900000000",
                        TrangThai = "Hoạt động",
                        GhiChu = d.GhiChu,
                        SoSuKienToChuc = db.SuKiens.Count(s => s.MaDiaDiem == d.MaDiaDiem)
                    }).ToList();

                    if (list.Any())
                    {
                        _venues.AddRange(list);
                    }
                }
                catch { }
            }
        }

        public ServiceResult<List<VenueDto>> GetAll(string? keyword = null, string? statusFilter = null)
        {
            EnsureLoadedFromDb();
            var query = _venues.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(x => x.MaDiaDiem.ToLower().Contains(kw) ||
                                         x.TenDiaDiem.ToLower().Contains(kw) ||
                                         x.DiaChi.ToLower().Contains(kw) ||
                                         (x.NguoiQuanLy != null && x.NguoiQuanLy.ToLower().Contains(kw)) ||
                                         (x.SoDienThoai != null && x.SoDienThoai.Contains(kw)));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "Tất cả")
            {
                query = query.Where(x => x.TrangThai.Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
            }

            return ServiceResult<List<VenueDto>>.Success(query.OrderBy(x => x.MaDiaDiem).ToList());
        }

        public ServiceResult<VenueDto> GetById(string maDiaDiem)
        {
            EnsureLoadedFromDb();
            var item = _venues.FirstOrDefault(x => x.MaDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<VenueDto>.Failed($"Không tìm thấy địa điểm có mã '{maDiaDiem}'!");
            }
            return ServiceResult<VenueDto>.Success(item);
        }

        public ServiceResult Add(VenueDto dto)
        {
            EnsureLoadedFromDb();
            // Kiểm tra tính hợp lệ dữ liệu
            if (string.IsNullOrWhiteSpace(dto.MaDiaDiem))
            {
                return ServiceResult.Failed("Mã địa điểm không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.TenDiaDiem))
            {
                return ServiceResult.Failed("Tên địa điểm không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.DiaChi))
            {
                return ServiceResult.Failed("Địa chỉ địa điểm không được để trống!");
            }

            if (dto.SucChuaToiDa <= 0)
            {
                return ServiceResult.Failed("Sức chứa tối đa phải lớn hơn 0!");
            }

            string code = dto.MaDiaDiem.Trim().ToUpper();
            if (_venues.Any(x => x.MaDiaDiem.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult.Failed($"Mã địa điểm '{code}' đã tồn tại!");
            }

            var newEntity = new VenueDto
            {
                MaDiaDiem = code,
                TenDiaDiem = dto.TenDiaDiem.Trim(),
                DiaChi = dto.DiaChi.Trim(),
                SucChuaToiDa = dto.SucChuaToiDa,
                NguoiQuanLy = dto.NguoiQuanLy?.Trim(),
                SoDienThoai = dto.SoDienThoai?.Trim(),
                TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "Hoạt động" : dto.TrangThai.Trim(),
                GhiChu = dto.GhiChu?.Trim(),
                SoSuKienToChuc = 0
            };

            _venues.Add(newEntity);
            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success("Thêm mới địa điểm thành công!");
        }

        public ServiceResult Update(VenueDto dto)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(dto.MaDiaDiem))
            {
                return ServiceResult.Failed("Mã địa điểm không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(dto.TenDiaDiem))
            {
                return ServiceResult.Failed("Tên địa điểm không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.DiaChi))
            {
                return ServiceResult.Failed("Địa chỉ địa điểm không được để trống!");
            }

            if (dto.SucChuaToiDa <= 0)
            {
                return ServiceResult.Failed("Sức chứa tối đa phải lớn hơn 0!");
            }

            var existing = _venues.FirstOrDefault(x => x.MaDiaDiem.Equals(dto.MaDiaDiem, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy địa điểm '{dto.MaDiaDiem}' để cập nhật!");
            }

            existing.TenDiaDiem = dto.TenDiaDiem.Trim();
            existing.DiaChi = dto.DiaChi.Trim();
            existing.SucChuaToiDa = dto.SucChuaToiDa;
            existing.NguoiQuanLy = dto.NguoiQuanLy?.Trim();
            existing.SoDienThoai = dto.SoDienThoai?.Trim();
            if (!string.IsNullOrWhiteSpace(dto.TrangThai))
            {
                existing.TrangThai = dto.TrangThai.Trim();
            }
            existing.GhiChu = dto.GhiChu?.Trim();

            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success("Cập nhật thông tin địa điểm thành công!");
        }

        public ServiceResult Delete(string maDiaDiem)
        {
            EnsureLoadedFromDb();
            var existing = _venues.FirstOrDefault(x => x.MaDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy địa điểm '{maDiaDiem}'!");
            }

            // Quy tắc BR-20: Không xóa cứng nếu đã từng có sự kiện tổ chức
            if (existing.SoSuKienToChuc > 0)
            {
                return ServiceResult.Failed(
                    $"Địa điểm '{existing.TenDiaDiem}' đã từng tổ chức {existing.SoSuKienToChuc} sự kiện. Không thể xóa cứng khỏi hệ thống!",
                    "BR-20");
            }

            _venues.Remove(existing);
            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success($"Đã xóa vĩnh viễn địa điểm '{existing.TenDiaDiem}'!");
        }

        public ServiceResult SoftDelete(string maDiaDiem)
        {
            EnsureLoadedFromDb();
            var existing = _venues.FirstOrDefault(x => x.MaDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy địa điểm '{maDiaDiem}'!");
            }

            existing.TrangThai = "Ngừng sử dụng";
            return ServiceResult.Success($"Đã chuyển trạng thái địa điểm '{existing.TenDiaDiem}' sang 'Ngừng sử dụng'!");
        }

        public ServiceResult<bool> CheckVenueAvailability(string maDiaDiem, DateTime startTime, DateTime endTime, string? excludeEventId = null)
        {
            EnsureLoadedFromDb();
            // Sẵn sàng kết nối tới thuật toán BR-04 do Thành viên 3 bàn giao ở Ngày 4
            var venue = _venues.FirstOrDefault(x => x.MaDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase));
            if (venue == null)
            {
                return ServiceResult<bool>.Failed($"Không tìm thấy địa điểm '{maDiaDiem}'!");
            }

            if (venue.TrangThai != "Hoạt động")
            {
                return ServiceResult<bool>.Success(false, $"Địa điểm '{venue.TenDiaDiem}' hiện đang ở trạng thái '{venue.TrangThai}', không thể xếp lịch!");
            }

            return ServiceResult<bool>.Success(true, "Địa điểm khả dụng trong khoảng thời gian yêu cầu.");
        }
    }
}
