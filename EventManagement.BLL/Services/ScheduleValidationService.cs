using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Service thẩm định xung đột lịch trình sự kiện
    /// Đảm trách quy tắc nghiệp vụ: BR-06 (Chống trùng diễn giả & trùng phòng/khu vực chức năng)
    /// Phụ trách: Thành viên 3 (Thuật toán lõi & BLL) - Bàn giao cho Thành viên 2 nhúng vào Form F08
    /// </summary>
    public class ScheduleValidationService
    {
        /// <summary>
        /// Thuật toán BR-06 (Phần 1): Chống trùng lịch Diễn giả
        /// Không cho phép 1 diễn giả thuyết trình ở 2 tiết mục/phiên có khung giờ giao thoa nhau.
        /// Công thức giao thoa: (Start_New < End_Old) && (End_New > Start_Old)
        /// </summary>
        /// <param name="maDienGia">Mã diễn giả cần xếp lịch</param>
        /// <param name="start">Thời điểm bắt đầu tiết mục dự kiến</param>
        /// <param name="end">Thời điểm kết thúc tiết mục dự kiến</param>
        /// <param name="excludeMaLichTrinh">Mã lịch trình bỏ qua (khi cập nhật chính tiết mục đó)</param>
        /// <param name="existingSchedules">Danh sách các lịch trình hiện có (trong sự kiện hoặc toàn hệ thống)</param>
        /// <returns>ServiceResult cho biết diễn giả có khả dụng hay bị trùng lịch</returns>
        public ServiceResult<bool> CheckSpeakerConflict(
            string? maDienGia,
            DateTime start,
            DateTime end,
            string? excludeMaLichTrinh,
            IEnumerable<LichTrinh> existingSchedules)
        {
            if (string.IsNullOrWhiteSpace(maDienGia))
            {
                return ServiceResult<bool>.Success(true, "Không có diễn giả phân công, bỏ qua kiểm tra.");
            }

            if (end <= start)
            {
                return ServiceResult<bool>.Failed(
                    "Thời gian kết thúc tiết mục phải lớn hơn thời gian bắt đầu.",
                    new List<string> { $"ThoiGianBatDau ({start:HH:mm}) >= ThoiGianKetThuc ({end:HH:mm})" }
                );
            }

            var conflict = existingSchedules.FirstOrDefault(s =>
                s.MaDienGia == maDienGia &&
                s.MaLichTrinh != excludeMaLichTrinh &&
                start < s.ThoiGianKetThuc &&
                end > s.ThoiGianBatDau
            );

            if (conflict != null)
            {
                return ServiceResult<bool>.Failed(
                    $"Xung đột lịch diễn giả (BR-06): Diễn giả [{maDienGia}] đã có lịch tiết mục '{conflict.TenTietMuc}' từ {conflict.ThoiGianBatDau:dd/MM/yyyy HH:mm} đến {conflict.ThoiGianKetThuc:dd/MM/yyyy HH:mm}.",
                    new List<string>
                    {
                        $"Mã lịch trình xung đột: {conflict.MaLichTrinh}",
                        $"Tiết mục đối kháng: {conflict.TenTietMuc}",
                        $"Khung giờ đối kháng: {conflict.ThoiGianBatDau:HH:mm} - {conflict.ThoiGianKetThuc:HH:mm}",
                        $"Khung giờ yêu cầu: {start:HH:mm} - {end:HH:mm}"
                    }
                );
            }

            return ServiceResult<bool>.Success(true, "Diễn giả hoàn toàn khả dụng trong khung giờ này.");
        }

        /// <summary>
        /// Thuật toán BR-06 (Phần 2): Chống trùng Phòng / Hội trường chức năng
        /// Không xếp 2 tiết mục cùng một phòng / hội trường tại cùng thời điểm giao thoa.
        /// </summary>
        /// <param name="diaDiemChiTiet">Tên phòng hoặc hội trường (VD: 'Hội trường A', 'Phòng Hội thảo 2')</param>
        /// <param name="start">Thời điểm bắt đầu tiết mục dự kiến</param>
        /// <param name="end">Thời điểm kết thúc tiết mục dự kiến</param>
        /// <param name="excludeMaLichTrinh">Mã lịch trình bỏ qua (khi cập nhật)</param>
        /// <param name="existingSchedules">Danh sách các lịch trình hiện có</param>
        /// <returns>ServiceResult cho biết phòng chức năng có khả dụng hay bị trùng</returns>
        public ServiceResult<bool> CheckRoomConflict(
            string? diaDiemChiTiet,
            DateTime start,
            DateTime end,
            string? excludeMaLichTrinh,
            IEnumerable<LichTrinh> existingSchedules)
        {
            if (string.IsNullOrWhiteSpace(diaDiemChiTiet))
            {
                return ServiceResult<bool>.Success(true, "Chưa chỉ định phòng cụ thể, bỏ qua kiểm tra phòng.");
            }

            if (end <= start)
            {
                return ServiceResult<bool>.Failed(
                    "Thời gian kết thúc tiết mục phải lớn hơn thời gian bắt đầu.",
                    new List<string> { $"ThoiGianBatDau ({start:HH:mm}) >= ThoiGianKetThuc ({end:HH:mm})" }
                );
            }

            var trimmedRoom = diaDiemChiTiet.Trim();
            var conflict = existingSchedules.FirstOrDefault(s =>
                !string.IsNullOrWhiteSpace(s.DiaDiemChiTiet) &&
                string.Equals(s.DiaDiemChiTiet.Trim(), trimmedRoom, StringComparison.OrdinalIgnoreCase) &&
                s.MaLichTrinh != excludeMaLichTrinh &&
                start < s.ThoiGianKetThuc &&
                end > s.ThoiGianBatDau
            );

            if (conflict != null)
            {
                return ServiceResult<bool>.Failed(
                    $"Xung đột phòng chức năng (BR-06): Phòng '{trimmedRoom}' đã được xếp cho tiết mục '{conflict.TenTietMuc}' từ {conflict.ThoiGianBatDau:dd/MM/yyyy HH:mm} đến {conflict.ThoiGianKetThuc:dd/MM/yyyy HH:mm}.",
                    new List<string>
                    {
                        $"Mã lịch trình xung đột: {conflict.MaLichTrinh}",
                        $"Tiết mục đối kháng: {conflict.TenTietMuc}",
                        $"Khung giờ đối kháng: {conflict.ThoiGianBatDau:HH:mm} - {conflict.ThoiGianKetThuc:HH:mm}",
                        $"Khung giờ yêu cầu: {start:HH:mm} - {end:HH:mm}"
                    }
                );
            }

            return ServiceResult<bool>.Success(true, "Phòng chức năng hoàn toàn khả dụng trong khung giờ này.");
        }

        /// <summary>
        /// Hàm thẩm định tổng hợp một bản ghi lịch trình trước khi lưu
        /// Kiểm tra đồng thời cả Diễn giả lẫn Phòng chức năng
        /// </summary>
        /// <param name="schedule">Bản ghi lịch trình cần kiểm tra</param>
        /// <param name="existingSchedules">Danh sách lịch trình hiện hữu</param>
        /// <returns>ServiceResult thành công hoặc thất bại kèm chi tiết lỗi</returns>
        public ServiceResult<bool> ValidateScheduleItem(LichTrinh schedule, IEnumerable<LichTrinh> existingSchedules)
        {
            if (schedule == null)
            {
                return ServiceResult<bool>.Failed("Thông tin tiết mục lịch trình không được để trống.");
            }

            // 1. Kiểm tra Diễn giả
            var speakerCheck = CheckSpeakerConflict(
                schedule.MaDienGia,
                schedule.ThoiGianBatDau,
                schedule.ThoiGianKetThuc,
                schedule.MaLichTrinh,
                existingSchedules
            );

            if (!speakerCheck.IsSuccess)
            {
                return speakerCheck;
            }

            // 2. Kiểm tra Phòng chức năng
            var roomCheck = CheckRoomConflict(
                schedule.DiaDiemChiTiet,
                schedule.ThoiGianBatDau,
                schedule.ThoiGianKetThuc,
                schedule.MaLichTrinh,
                existingSchedules
            );

            if (!roomCheck.IsSuccess)
            {
                return roomCheck;
            }

            return ServiceResult<bool>.Success(true, "Lịch trình hợp lệ, không có bất kỳ xung đột nào.");
        }
    }
}
