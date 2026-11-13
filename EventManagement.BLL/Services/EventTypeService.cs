using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt nghiệp vụ Quản lý loại sự kiện (F06)
    /// Đảm bảo các quy tắc:
    /// - BR-07: Không cho phép xóa Loại sự kiện nếu đang có sự kiện áp dụng.
    /// - BR-20: Hỗ trợ chuyển trạng thái sang 'Ngừng áp dụng' (Soft Delete).
    /// </summary>
    public class EventTypeService : IEventTypeService
    {
        private static readonly List<EventTypeDto> _eventTypes = new();

        private void EnsureLoadedFromDb()
        {
            if (_eventTypes.Count == 0)
            {
                try
                {
                    using var db = new EventManagement.DAL.AppDbContext();
                    var list = db.LoaiSuKiens.Select(l => new EventTypeDto
                    {
                        MaLoaiSK = l.MaLoaiSK,
                        TenLoaiSK = l.TenLoaiSK,
                        MoTa = l.MoTa,
                        SoSuKien = db.SuKiens.Count(s => s.MaLoaiSK == l.MaLoaiSK),
                        TrangThai = "Hoạt động"
                    }).ToList();

                    if (list.Any())
                    {
                        _eventTypes.AddRange(list);
                    }
                }
                catch { }
            }
        }

        public ServiceResult<List<EventTypeDto>> GetAll(string? keyword = null, string? statusFilter = null)
        {
            EnsureLoadedFromDb();
            var query = _eventTypes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(x => x.MaLoaiSK.ToLower().Contains(kw) ||
                                         x.TenLoaiSK.ToLower().Contains(kw) ||
                                         (x.MoTa != null && x.MoTa.ToLower().Contains(kw)));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "Tất cả")
            {
                query = query.Where(x => x.TrangThai.Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
            }

            return ServiceResult<List<EventTypeDto>>.Success(query.OrderBy(x => x.MaLoaiSK).ToList());
        }

        public ServiceResult<EventTypeDto> GetById(string maLoaiSK)
        {
            EnsureLoadedFromDb();
            var item = _eventTypes.FirstOrDefault(x => x.MaLoaiSK.Equals(maLoaiSK, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<EventTypeDto>.Failed($"Không tìm thấy loại sự kiện có mã '{maLoaiSK}'!");
            }
            return ServiceResult<EventTypeDto>.Success(item);
        }

        public ServiceResult Add(EventTypeDto dto)
        {
            EnsureLoadedFromDb();
            // Kiểm tra tính hợp lệ dữ liệu
            if (string.IsNullOrWhiteSpace(dto.MaLoaiSK))
            {
                return ServiceResult.Failed("Mã loại sự kiện không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.TenLoaiSK))
            {
                return ServiceResult.Failed("Tên loại sự kiện không được để trống!");
            }

            string code = dto.MaLoaiSK.Trim().ToUpper();
            if (_eventTypes.Any(x => x.MaLoaiSK.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult.Failed($"Mã loại sự kiện '{code}' đã tồn tại trong hệ thống!");
            }

            var newEntity = new EventTypeDto
            {
                MaLoaiSK = code,
                TenLoaiSK = dto.TenLoaiSK.Trim(),
                MoTa = dto.MoTa?.Trim(),
                SoSuKien = 0,
                TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "Hoạt động" : dto.TrangThai.Trim()
            };

            _eventTypes.Add(newEntity);
            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success("Thêm mới loại sự kiện thành công!");
        }

        public ServiceResult Update(EventTypeDto dto)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(dto.MaLoaiSK))
            {
                return ServiceResult.Failed("Mã loại sự kiện không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(dto.TenLoaiSK))
            {
                return ServiceResult.Failed("Tên loại sự kiện không được để trống!");
            }

            var existing = _eventTypes.FirstOrDefault(x => x.MaLoaiSK.Equals(dto.MaLoaiSK, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy loại sự kiện '{dto.MaLoaiSK}' để cập nhật!");
            }

            existing.TenLoaiSK = dto.TenLoaiSK.Trim();
            existing.MoTa = dto.MoTa?.Trim();
            if (!string.IsNullOrWhiteSpace(dto.TrangThai))
            {
                existing.TrangThai = dto.TrangThai.Trim();
            }

            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success("Cập nhật thông tin loại sự kiện thành công!");
        }

        public ServiceResult Delete(string maLoaiSK)
        {
            EnsureLoadedFromDb();
            var existing = _eventTypes.FirstOrDefault(x => x.MaLoaiSK.Equals(maLoaiSK, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy loại sự kiện '{maLoaiSK}'!");
            }

            // Quy tắc BR-07: Không cho phép xóa nếu đã có sự kiện áp dụng
            if (existing.SoSuKien > 0)
            {
                return ServiceResult.Failed(
                    $"Loại sự kiện '{existing.TenLoaiSK}' đang được áp dụng cho {existing.SoSuKien} sự kiện. Không thể xóa dữ liệu!",
                    "BR-07");
            }

            _eventTypes.Remove(existing);
            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success($"Đã xóa vĩnh viễn loại sự kiện '{existing.TenLoaiSK}'!");
        }

        public ServiceResult SoftDelete(string maLoaiSK)
        {
            EnsureLoadedFromDb();
            // Quy tắc BR-20: Soft delete chuyển trạng thái
            var existing = _eventTypes.FirstOrDefault(x => x.MaLoaiSK.Equals(maLoaiSK, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy loại sự kiện '{maLoaiSK}'!");
            }

            existing.TrangThai = "Ngừng áp dụng";
            return ServiceResult.Success($"Đã chuyển trạng thái loại sự kiện '{existing.TenLoaiSK}' sang 'Ngừng áp dụng'!");
        }
    }
}
