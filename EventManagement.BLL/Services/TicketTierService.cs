using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.Entities;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Service quản lý các hạng vé sự kiện và kho vé (Form F12)
    /// Đảm trách các quy tắc: BR-08 (Bảo vệ số vé phát hành), BR-09 (Tồn kho khả dụng)
    /// </summary>
    public class TicketTierService
    {
        /// <summary>
        /// Thuật toán BR-08: Thẩm định khi cập nhật hạng vé
        /// Quy tắc: Không cho phép sửa giảm số vé phát hành nhỏ hơn số lượng vé đã thực tế bán ra.
        /// </summary>
        /// <param name="tier">Hạng vé cần cập nhật</param>
        /// <param name="newPhatHanh">Số lượng phát hành mới</param>
        /// <returns>ServiceResult kết quả thẩm định</returns>
        public ServiceResult<bool> ValidateTierCapacityUpdate(HangVe tier, int newPhatHanh)
        {
            if (tier == null)
                return ServiceResult<bool>.Failed("Hạng vé không tồn tại.");

            if (newPhatHanh <= 0)
                return ServiceResult<bool>.Failed("Số lượng phát hành phải lớn hơn 0.");

            if (newPhatHanh < tier.SoLuongDaBan)
            {
                return ServiceResult<bool>.Failed(
                    $"Không thể giảm số lượng phát hành xuống {newPhatHanh} vé vì đã bán thực tế {tier.SoLuongDaBan} vé (BR-08).",
                    new List<string> { $"Yêu cầu: SoLuongPhatHanh ({newPhatHanh}) >= SoLuongDaBan ({tier.SoLuongDaBan})" }
                );
            }

            return ServiceResult<bool>.Success(true, "Số lượng phát hành hợp lệ.");
        }

        /// <summary>
        /// Thuật toán BR-09: Kiểm tra số lượng vé đặt mua so với tồn kho khả dụng
        /// Quy tắc: SoLuongMua <= (SoLuongPhatHanh - SoLuongDaBan) và SoLuongMua trong khoảng [1, 10] (BR-10)
        /// </summary>
        /// <param name="tier">Hạng vé muốn mua</param>
        /// <param name="soLuongMua">Số lượng vé muốn mua</param>
        /// <param name="currentTime">Thời điểm mua vé (kiểm tra khung giờ mở/đóng bán)</param>
        /// <returns>ServiceResult cho biết có đủ vé để bán không</returns>
        public ServiceResult<int> CheckTicketAvailability(HangVe tier, int soLuongMua, DateTime? currentTime = null)
        {
            if (tier == null)
                return ServiceResult<int>.Failed("Hạng vé không tồn tại.");

            var now = currentTime ?? DateTime.Now;

            // Kiểm tra khung giờ mở bán vé (nếu có cấu hình)
            if (tier.ThoiGianMoBan.HasValue && now < tier.ThoiGianMoBan.Value)
            {
                return ServiceResult<int>.Failed(
                    $"Hạng vé '{tier.TenHangVe}' chưa đến giờ mở bán (Mở bán lúc {tier.ThoiGianMoBan.Value:dd/MM/yyyy HH:mm})."
                );
            }

            if (tier.ThoiGianDongBan.HasValue && now > tier.ThoiGianDongBan.Value)
            {
                return ServiceResult<int>.Failed(
                    $"Hạng vé '{tier.TenHangVe}' đã kết thúc thời gian mở bán (Đóng bán lúc {tier.ThoiGianDongBan.Value:dd/MM/yyyy HH:mm})."
                );
            }

            // Kiểm tra giới hạn số lượng đặt mua (1 đến 10 vé - BR-10)
            if (soLuongMua < 1 || soLuongMua > 10)
            {
                return ServiceResult<int>.Failed("Số lượng vé đặt mua phải từ 1 đến 10 vé trong một đơn hàng (BR-10).");
            }

            // Tính tồn kho khả dụng
            int conLai = tier.SoLuongPhatHanh - tier.SoLuongDaBan;
            if (conLai <= 0)
            {
                return ServiceResult<int>.Failed($"Hạng vé '{tier.TenHangVe}' đã hết vé.");
            }

            if (soLuongMua > conLai)
            {
                return ServiceResult<int>.Failed(
                    $"Số lượng vé yêu cầu ({soLuongMua}) vượt quá số lượng vé còn lại trong kho ({conLai} vé khả dụng)."
                );
            }

            return ServiceResult<int>.Success(conLai - soLuongMua, $"Còn khả dụng {conLai} vé, đủ cung ứng {soLuongMua} vé.");
        }

        /// <summary>
        /// Thẩm định toàn diện thông tin hạng vé trước khi lưu (F12)
        /// </summary>
        public ServiceResult<bool> ValidateTicketTierInfo(HangVe tier)
        {
            if (tier == null)
                return ServiceResult<bool>.Failed("Thông tin hạng vé không được rỗng.");

            if (string.IsNullOrWhiteSpace(tier.TenHangVe))
                return ServiceResult<bool>.Failed("Tên hạng vé không được để trống.");

            if (tier.GiaVe < 0 || tier.SoLuongPhatHanh <= 0)
                return ServiceResult<bool>.Failed("Giá vé phải >= 0 và Số lượng phát hành phải lớn hơn 0!");

            if (tier.ThoiGianMoBan.HasValue && tier.ThoiGianDongBan.HasValue && tier.ThoiGianDongBan.Value <= tier.ThoiGianMoBan.Value)
            {
                return ServiceResult<bool>.Failed("Thời gian đóng bán vé phải lớn hơn thời gian mở bán.");
            }

            return ServiceResult<bool>.Success(true, "Thông tin hạng vé hợp lệ.");
        }
    }
}
