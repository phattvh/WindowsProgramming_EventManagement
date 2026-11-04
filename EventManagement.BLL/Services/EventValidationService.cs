using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Service xử lý các thuật toán thẩm định thời gian và lịch trình sự kiện
    /// Đảm trách các thuật toán: BR-03, BR-04, BR-05 (Thành viên 3 phụ trách)
    /// </summary>
    public class EventValidationService
    {
        /// <summary>
        /// Thuật toán BR-03: Kiểm tra tính hợp lệ của thời gian bắt đầu và kết thúc sự kiện
        /// Quy tắc: NgayKetThuc >= NgayBatDau. Nếu là sự kiện mới (isNew = true), NgayBatDau không được ở quá khứ.
        /// </summary>
        /// <param name="ngayBatDau">Thời điểm bắt đầu sự kiện</param>
        /// <param name="ngayKetThuc">Thời điểm kết thúc sự kiện</param>
        /// <param name="isNew">Là thêm mới sự kiện hay đang chỉnh sửa</param>
        /// <param name="now">Mốc thời gian đối chiếu (mặc định DateTime.Now)</param>
        /// <returns>ServiceResult thành công hoặc thất bại kèm thông điệp lỗi</returns>
        public ServiceResult<bool> ValidateEventDates(DateTime ngayBatDau, DateTime ngayKetThuc, bool isNew = true, DateTime? now = null)
        {
            var currentTime = now ?? DateTime.Now;

            if (ngayKetThuc < ngayBatDau)
            {
                return ServiceResult<bool>.Failed(
                    "Ngày kết thúc sự kiện không được trước ngày bắt đầu.",
                    new List<string> { $"NgayBatDau ({ngayBatDau:dd/MM/yyyy HH:mm}) > NgayKetThuc ({ngayKetThuc:dd/MM/yyyy HH:mm})" }
                );
            }

            if (isNew && ngayBatDau < currentTime.Date)
            {
                return ServiceResult<bool>.Failed(
                    "Ngày bắt đầu sự kiện mới không được nằm trong quá khứ.",
                    new List<string> { $"NgayBatDau ({ngayBatDau:dd/MM/yyyy}) < HienTai ({currentTime:dd/MM/yyyy})" }
                );
            }

            return ServiceResult<bool>.Success(true, "Thời gian sự kiện hoàn toàn hợp lệ.");
        }

        /// <summary>
        /// Thuật toán BR-05: Kiểm tra khung giờ của tiết mục con so với sự kiện chính
        /// Quy tắc: Event.NgayBatDau <= Schedule.ThoiGianBatDau < Schedule.ThoiGianKetThuc <= Event.NgayKetThuc
        /// </summary>
        /// <param name="eventStart">Thời gian bắt đầu sự kiện chính</param>
        /// <param name="eventEnd">Thời gian kết thúc sự kiện chính</param>
        /// <param name="itemStart">Thời gian bắt đầu tiết mục con</param>
        /// <param name="itemEnd">Thời gian kết thúc tiết mục con</param>
        /// <returns>ServiceResult kết quả thẩm định</returns>
        public ServiceResult<bool> ValidateScheduleItem(DateTime eventStart, DateTime eventEnd, DateTime itemStart, DateTime itemEnd)
        {
            if (itemEnd <= itemStart)
            {
                return ServiceResult<bool>.Failed(
                    "Thời gian kết thúc tiết mục phải lớn hơn thời gian bắt đầu.",
                    new List<string> { $"ThoiGianBatDau ({itemStart:HH:mm}) >= ThoiGianKetThuc ({itemEnd:HH:mm})" }
                );
            }

            if (itemStart < eventStart)
            {
                return ServiceResult<bool>.Failed(
                    "Tiết mục không được bắt đầu trước giờ khai mạc của sự kiện chính.",
                    new List<string> { $"Tiết mục bắt đầu lúc {itemStart:dd/MM/yyyy HH:mm}, trong khi sự kiện mở lúc {eventStart:dd/MM/yyyy HH:mm}" }
                );
            }

            if (itemEnd > eventEnd)
            {
                return ServiceResult<bool>.Failed(
                    "Tiết mục không được kéo dài vượt quá thời điểm bế mạc của sự kiện chính.",
                    new List<string> { $"Tiết mục kết thúc lúc {itemEnd:dd/MM/yyyy HH:mm}, trong khi sự kiện kết thúc lúc {eventEnd:dd/MM/yyyy HH:mm}" }
                );
            }

            return ServiceResult<bool>.Success(true, "Khung giờ tiết mục nằm trọn vẹn trong sự kiện chính.");
        }

        /// <summary>
        /// Thuật toán BR-04: Chống trùng lịch địa điểm giữa các sự kiện (Interval Overlap Detection)
        /// Hai sự kiện tại cùng 1 địa điểm xung đột khi và chỉ khi thời gian giao thoa:
        /// Overlap <=> (Start_A < End_B) && (End_A > Start_B)
        /// </summary>
        /// <param name="maDiaDiem">Mã địa điểm tổ chức</param>
        /// <param name="start">Thời điểm bắt đầu dự kiến</param>
        /// <param name="end">Thời điểm kết thúc dự kiến</param>
        /// <param name="excludeMaSuKien">Bỏ qua mã sự kiện này (dùng khi cập nhật chính sự kiện đó)</param>
        /// <param name="existingEvents">Danh sách toàn bộ các sự kiện hiện có trong CSDL</param>
        /// <returns>ServiceResult cho biết địa điểm có khả dụng hay bị trùng lịch</returns>
        public ServiceResult<bool> CheckVenueAvailability(
            string maDiaDiem, 
            DateTime start, 
            DateTime end, 
            string? excludeMaSuKien = null, 
            IEnumerable<SuKien>? existingEvents = null)
        {
            if (string.IsNullOrWhiteSpace(maDiaDiem))
            {
                return ServiceResult<bool>.Failed("Mã địa điểm không được để trống.");
            }

            if (end <= start)
            {
                return ServiceResult<bool>.Failed("Thời điểm kết thúc phải lớn hơn thời điểm bắt đầu.");
            }

            if (existingEvents == null)
            {
                return ServiceResult<bool>.Success(true, "Địa điểm khả dụng (không có sự kiện nào khác).");
            }

            // Sử dụng LINQ để kiểm tra xung đột giao thoa thời gian (bỏ qua các sự kiện đã có trạng thái 'Hủy')
            var conflictingEvent = existingEvents
                .Where(e => e.MaDiaDiem.Equals(maDiaDiem, StringComparison.OrdinalIgnoreCase))
                .Where(e => !e.TrangThai.Equals("Hủy", StringComparison.OrdinalIgnoreCase))
                .Where(e => excludeMaSuKien == null || !e.MaSuKien.Equals(excludeMaSuKien, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault(e => start < e.NgayKetThuc && end > e.NgayBatDau);

            if (conflictingEvent != null)
            {
                return ServiceResult<bool>.Failed(
                    $"Địa điểm này đã được đăng ký bởi sự kiện '{conflictingEvent.TenSuKien}' ({conflictingEvent.MaSuKien}).",
                    new List<string>
                    {
                        $"Sự kiện trùng lặp diễn ra từ {conflictingEvent.NgayBatDau:dd/MM/yyyy HH:mm} đến {conflictingEvent.NgayKetThuc:dd/MM/yyyy HH:mm}.",
                        $"Khoảng thời gian yêu cầu: {start:dd/MM/yyyy HH:mm} đến {end:dd/MM/yyyy HH:mm} bị giao thoa."
                    }
                );
            }

            return ServiceResult<bool>.Success(true, "Địa điểm hoàn toàn khả dụng trong khung thời gian này.");
        }
    }
}
