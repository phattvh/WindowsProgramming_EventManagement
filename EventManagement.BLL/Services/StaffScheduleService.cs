using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Service quản lý và thẩm định phân công ca trực nhân sự
    /// Đảm trách quy tắc nghiệp vụ: BR-13 (Chống trùng ca trực nhân sự theo khung giờ)
    /// Phụ trách: Thành viên 3 (Thuật toán lõi & BLL) - Bàn giao cho Thành viên 2 nhúng vào Form F15
    /// </summary>
    public class StaffScheduleService
    {
        /// <summary>
        /// Thuật toán BR-13: Chống trùng ca trực nhân sự (Shift Overlap Detection)
        /// Một nhân sự không thể được phân công 2 ca trực có khung thời gian giao thoa nhau.
        /// Công thức giao thoa: (Start_New < End_Old) && (End_New > Start_Old)
        /// </summary>
        /// <param name="maNhanSu">Mã nhân sự được phân công</param>
        /// <param name="start">Thời điểm bắt đầu ca trực dự kiến</param>
        /// <param name="end">Thời điểm kết thúc ca trực dự kiến</param>
        /// <param name="excludeMaPhanCong">Mã phân công bỏ qua (khi cập nhật lại ca trực hiện tại)</param>
        /// <param name="existingAssignments">Danh sách các ca trực hiện có trong hệ thống</param>
        /// <returns>ServiceResult cho biết nhân sự có khả dụng hay bị trùng ca</returns>
        public ServiceResult<bool> CheckShiftConflict(
            string maNhanSu,
            DateTime start,
            DateTime end,
            string? excludeMaPhanCong,
            IEnumerable<PhanCongNhanSu> existingAssignments)
        {
            if (string.IsNullOrWhiteSpace(maNhanSu))
            {
                return ServiceResult<bool>.Failed("Mã nhân sự không được để trống khi phân công ca trực.");
            }

            if (end <= start)
            {
                return ServiceResult<bool>.Failed(
                    "Thời gian kết thúc ca trực phải lớn hơn thời gian bắt đầu.",
                    new List<string> { $"ThoiGianBatDau ({start:HH:mm}) >= ThoiGianKetThuc ({end:HH:mm})" }
                );
            }

            var conflict = existingAssignments.FirstOrDefault(a =>
                a.MaNhanSu == maNhanSu &&
                a.MaPhanCong != excludeMaPhanCong &&
                start < a.ThoiGianKetThuc &&
                end > a.ThoiGianBatDau
            );

            if (conflict != null)
            {
                return ServiceResult<bool>.Failed(
                    $"Xung đột ca trực nhân sự (BR-13): Nhân sự [{maNhanSu}] đã có ca trực nhiệm vụ '{conflict.NhiemVu}' từ {conflict.ThoiGianBatDau:dd/MM/yyyy HH:mm} đến {conflict.ThoiGianKetThuc:dd/MM/yyyy HH:mm}.",
                    new List<string>
                    {
                        $"Mã phân công xung đột: {conflict.MaPhanCong}",
                        $"Nhiệm vụ trùng lặp: {conflict.NhiemVu}",
                        $"Khung giờ đã trực: {conflict.ThoiGianBatDau:dd/MM/yyyy HH:mm} - {conflict.ThoiGianKetThuc:HH:mm}",
                        $"Khung giờ yêu cầu mới: {start:dd/MM/yyyy HH:mm} - {end:HH:mm}"
                    }
                );
            }

            return ServiceResult<bool>.Success(true, "Nhân sự hoàn toàn khả dụng cho ca trực này.");
        }

        /// <summary>
        /// Thẩm định toàn diện một phân công nhân sự trước khi lưu vào CSDL
        /// </summary>
        /// <param name="assignment">Đối tượng phân công cần kiểm tra</param>
        /// <param name="existingAssignments">Danh sách phân công hiện hữu</param>
        /// <returns>ServiceResult thành công hoặc thất bại</returns>
        public ServiceResult<bool> ValidateAssignment(
            PhanCongNhanSu assignment,
            IEnumerable<PhanCongNhanSu> existingAssignments)
        {
            if (assignment == null)
            {
                return ServiceResult<bool>.Failed("Thông tin phân công nhân sự không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(assignment.MaSuKien))
            {
                return ServiceResult<bool>.Failed("Mã sự kiện không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(assignment.NhiemVu))
            {
                return ServiceResult<bool>.Failed("Nhiệm vụ phân công không được để trống.");
            }

            return CheckShiftConflict(
                assignment.MaNhanSu,
                assignment.ThoiGianBatDau,
                assignment.ThoiGianKetThuc,
                assignment.MaPhanCong,
                existingAssignments
            );
        }
    }
}
