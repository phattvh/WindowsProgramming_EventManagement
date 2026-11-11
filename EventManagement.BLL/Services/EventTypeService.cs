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
        // Danh sách lưu trữ mô phỏng cho tầng BLL khi chưa gắn DbContext thực tế
        private static readonly List<EventTypeDto> _eventTypes = new()
        {
            new EventTypeDto { MaLoaiSK = "LSK01", TenLoaiSK = "Hội thảo Công nghệ", MoTa = "Diễn đàn trao đổi công nghệ và giải pháp phần mềm hiện đại", SoSuKien = 2, TrangThai = "Hoạt động" },
            new EventTypeDto { MaLoaiSK = "LSK02", TenLoaiSK = "Hội nghị Khách hàng", MoTa = "Gặp gỡ, tri ân đối tác và khách hàng thường niên", SoSuKien = 1, TrangThai = "Hoạt động" },
            new EventTypeDto { MaLoaiSK = "LSK03", TenLoaiSK = "Giải đấu Thể thao Điện tử", MoTa = "Giải đấu giao hữu eSports dành cho sinh viên", SoSuKien = 1, TrangThai = "Hoạt động" },
            new EventTypeDto { MaLoaiSK = "LSK04", TenLoaiSK = "Hội nghị Khoa học (Symposium)", MoTa = "Diễn đàn học thuật báo cáo nghiên cứu và công bố công trình khoa học", SoSuKien = 0, TrangThai = "Hoạt động" },
            new EventTypeDto { MaLoaiSK = "LSK05", TenLoaiSK = "Workshop Kỹ năng & Đào tạo", MoTa = "Các buổi thực hành chuyên môn ngắn hạn, tương tác trực tiếp", SoSuKien = 0, TrangThai = "Hoạt động" },
            new EventTypeDto { MaLoaiSK = "LSK06", TenLoaiSK = "Ngày hội Việc làm (Career Fair)", MoTa = "Kết nối doanh nghiệp tuyển dụng với sinh viên và ứng viên CNTT", SoSuKien = 0, TrangThai = "Hoạt động" }
        };

        public ServiceResult<List<EventTypeDto>> GetAll(string? keyword = null, string? statusFilter = null)
        {
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
            var item = _eventTypes.FirstOrDefault(x => x.MaLoaiSK.Equals(maLoaiSK, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<EventTypeDto>.Failed($"Không tìm thấy loại sự kiện có mã '{maLoaiSK}'!");
            }
            return ServiceResult<EventTypeDto>.Success(item);
        }

        public ServiceResult Add(EventTypeDto dto)
        {
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
            return ServiceResult.Success("Thêm mới loại sự kiện thành công!");
        }

        public ServiceResult Update(EventTypeDto dto)
        {
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

            return ServiceResult.Success("Cập nhật thông tin loại sự kiện thành công!");
        }

        public ServiceResult Delete(string maLoaiSK)
        {
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
            return ServiceResult.Success($"Đã xóa vĩnh viễn loại sự kiện '{existing.TenLoaiSK}'!");
        }

        public ServiceResult SoftDelete(string maLoaiSK)
        {
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
