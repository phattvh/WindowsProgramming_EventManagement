using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Service thẩm định xung đột lịch trình sự kiện
    /// Đảm trách quy tắc nghiệp vụ:
    /// - BR-05: Khung giờ tiết mục con phải nằm trọn trong khung giờ sự kiện chính.
    /// - BR-06: Chống trùng diễn giả & trùng phòng/khu vực chức năng.
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-10)
    /// </summary>
    public class ScheduleValidationService
    {
        #region 1. THẨM ĐỊNH THỜI GIAN & KHUNG GIỜ TIẾT MỤC (BR-05)

        /// <summary>
        /// Thuật toán BR-05: Kiểm tra khung giờ của tiết mục con so với sự kiện chính
        /// Quy tắc: Event.NgayBatDau <= Schedule.ThoiGianBatDau < Schedule.ThoiGianKetThuc <= Event.NgayKetThuc
        /// </summary>
        public ServiceResult<bool> CheckEventTimeRange(DateTime eventStart, DateTime eventEnd, DateTime itemStart, DateTime itemEnd)
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
                    "Xung đột khung giờ sự kiện (BR-05): Tiết mục không được bắt đầu trước giờ khai mạc của sự kiện chính.",
                    new List<string> { $"Tiết mục bắt đầu lúc {itemStart:dd/MM/yyyy HH:mm}, trong khi sự kiện bắt đầu lúc {eventStart:dd/MM/yyyy HH:mm}" }
                );
            }

            if (itemEnd > eventEnd)
            {
                return ServiceResult<bool>.Failed(
                    "Xung đột khung giờ sự kiện (BR-05): Tiết mục không được kéo dài vượt quá giờ bế mạc của sự kiện chính.",
                    new List<string> { $"Tiết mục kết thúc lúc {itemEnd:dd/MM/yyyy HH:mm}, trong khi sự kiện kết thúc lúc {eventEnd:dd/MM/yyyy HH:mm}" }
                );
            }

            return ServiceResult<bool>.Success(true, "Khung giờ tiết mục hợp lệ và nằm trọn trong thời gian sự kiện.");
        }

        #endregion

        #region 2. THUẬT TOÁN BR-06 (PHẦN 1): CHỐNG TRÙNG DIỄN GIẢ

        /// <summary>
        /// Thuật toán BR-06 (Phần 1): Chống trùng lịch Diễn giả (Entity LichTrinh)
        /// Không cho phép 1 diễn giả thuyết trình ở 2 tiết mục/phiên có khung giờ giao thoa nhau.
        /// Công thức giao thoa: (Start_New < End_Old) && (End_New > Start_Old)
        /// </summary>
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
        /// Thuật toán BR-06 (Phần 1): Chống trùng lịch Diễn giả (DTO ScheduleItemDto)
        /// </summary>
        public ServiceResult<bool> CheckSpeakerConflict(
            string? maDienGia,
            DateTime start,
            DateTime end,
            string? excludeMaLichTrinh,
            IEnumerable<ScheduleItemDto> existingSchedules)
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
                string tenDienGia = !string.IsNullOrWhiteSpace(conflict.TenDienGia) ? conflict.TenDienGia : conflict.MaDienGia ?? maDienGia;
                return ServiceResult<bool>.Failed(
                    $"Xung đột lịch diễn giả (BR-06): Diễn giả '{tenDienGia}' đã được phân công tiết mục '{conflict.TenTietMuc}' từ {conflict.ThoiGianBatDau:dd/MM/yyyy HH:mm} đến {conflict.ThoiGianKetThuc:dd/MM/yyyy HH:mm}.",
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

        #endregion

        #region 3. THUẬT TOÁN BR-06 (PHẦN 2): CHỐNG TRÙNG PHÒNG CHỨC NĂNG

        /// <summary>
        /// Thuật toán BR-06 (Phần 2): Chống trùng Phòng / Hội trường chức năng (Entity LichTrinh)
        /// Không xếp 2 tiết mục cùng một phòng / hội trường tại cùng thời điểm giao thoa.
        /// </summary>
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
        /// Thuật toán BR-06 (Phần 2): Chống trùng Phòng / Hội trường chức năng (DTO ScheduleItemDto)
        /// </summary>
        public ServiceResult<bool> CheckRoomConflict(
            string? diaDiemChiTiet,
            DateTime start,
            DateTime end,
            string? excludeMaLichTrinh,
            IEnumerable<ScheduleItemDto> existingSchedules)
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

        #endregion

        #region 4. THẨM ĐỊNH TỔNG HỢP TIẾT MỤC LỊCH TRÌNH

        /// <summary>
        /// Hàm thẩm định tổng hợp một bản ghi Entity LichTrinh
        /// </summary>
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

        /// <summary>
        /// Hàm thẩm định tổng hợp một bản ghi ScheduleItemDto
        /// Kiểm tra đồng thời BR-05 (khung giờ sự kiện) và BR-06 (xung đột diễn giả & phòng)
        /// </summary>
        public ServiceResult<bool> ValidateScheduleItem(
            ScheduleItemDto schedule,
            IEnumerable<ScheduleItemDto> existingSchedules,
            DateTime? eventStart = null,
            DateTime? eventEnd = null)
        {
            if (schedule == null)
            {
                return ServiceResult<bool>.Failed("Thông tin tiết mục lịch trình không được để trống.");
            }

            if (schedule.ThoiGianKetThuc <= schedule.ThoiGianBatDau)
            {
                return ServiceResult<bool>.Failed(
                    "Thời gian kết thúc tiết mục phải lớn hơn thời gian bắt đầu.",
                    new List<string> { $"ThoiGianBatDau ({schedule.ThoiGianBatDau:HH:mm}) >= ThoiGianKetThuc ({schedule.ThoiGianKetThuc:HH:mm})" }
                );
            }

            // 1. Kiểm tra BR-05 nếu có thông tin thời gian sự kiện
            if (eventStart.HasValue && eventEnd.HasValue)
            {
                var eventTimeCheck = CheckEventTimeRange(
                    eventStart.Value,
                    eventEnd.Value,
                    schedule.ThoiGianBatDau,
                    schedule.ThoiGianKetThuc
                );

                if (!eventTimeCheck.IsSuccess)
                {
                    return eventTimeCheck;
                }
            }

            // 2. Kiểm tra Diễn giả (BR-06)
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

            // 3. Kiểm tra Phòng chức năng (BR-06)
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

            return ServiceResult<bool>.Success(true, "Lịch trình hợp lệ, thỏa mãn toàn bộ quy tắc BR-05 và BR-06.");
        }

        #endregion
    }
}
