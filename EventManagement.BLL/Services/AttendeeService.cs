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
    /// Service quản lý hồ sơ người tham gia sự kiện (Form F10)
    /// </summary>
    public class AttendeeService
    {
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex PhoneRegex = new Regex(@"^0\d{9,10}$", RegexOptions.Compiled);

        /// <summary>
        /// Thẩm định thông tin người tham gia trước khi thêm mới hoặc cập nhật
        /// </summary>
        public ServiceResult<bool> ValidateAttendee(NguoiThamGia attendee, bool isNew = true)
        {
            if (attendee == null)
                return ServiceResult<bool>.Failed("Dữ liệu người tham gia không được rỗng.");

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(attendee.HoTen))
                errors.Add("Họ và tên không được để trống.");
            else if (attendee.HoTen.Trim().Length < 2)
                errors.Add("Họ và tên phải có ít nhất 2 ký tự.");

            if (string.IsNullOrWhiteSpace(attendee.SoDienThoai))
                errors.Add("Số điện thoại không được để trống.");
            else if (!PhoneRegex.IsMatch(attendee.SoDienThoai.Trim()))
                errors.Add("Số điện thoại không hợp lệ (Phải bắt đầu bằng số 0 và có từ 10-11 chữ số).");

            if (string.IsNullOrWhiteSpace(attendee.Email))
                errors.Add("Địa chỉ Email không được để trống.");
            else if (!EmailRegex.IsMatch(attendee.Email.Trim()))
                errors.Add("Địa chỉ Email không đúng định dạng (Ví dụ: name@example.com).");

            if (!string.IsNullOrWhiteSpace(attendee.GioiTinh))
            {
                var validGenders = new[] { "Nam", "Nữ", "Khác" };
                if (!validGenders.Contains(attendee.GioiTinh))
                    errors.Add("Giới tính chỉ được là: Nam, Nữ, hoặc Khác.");
            }

            if (errors.Any())
                return ServiceResult<bool>.Failed("Thông tin người tham gia chưa hợp lệ.", errors);

            return ServiceResult<bool>.Success(true, "Thông tin người tham gia hợp lệ.");
        }

        /// <summary>
        /// Tìm kiếm và lọc người tham gia theo từ khóa (Mã, Tên, Email, SĐT, Đơn vị) sử dụng LINQ
        /// </summary>
        public IEnumerable<NguoiThamGia> SearchAttendees(IEnumerable<NguoiThamGia> source, string? keyword, string? genderFilter = null)
        {
            if (source == null) return Enumerable.Empty<NguoiThamGia>();

            var query = source.AsQueryable();

            if (!string.IsNullOrWhiteSpace(genderFilter) && genderFilter != "Tất cả")
            {
                query = query.Where(a => a.GioiTinh == genderFilter);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();
                query = query.Where(a =>
                    a.MaNguoiThamGia.ToLower().Contains(kw) ||
                    a.HoTen.ToLower().Contains(kw) ||
                    a.Email.ToLower().Contains(kw) ||
                    a.SoDienThoai.Contains(kw) ||
                    (a.DonViCongTac != null && a.DonViCongTac.ToLower().Contains(kw))
                );
            }

            return query.ToList();
        }

        /// <summary>
        /// Tự sinh mã người tham gia tiếp theo (Định dạng: NTG001, NTG002...)
        /// </summary>
        public string GenerateNextAttendeeId(IEnumerable<NguoiThamGia>? existingList)
        {
            if (existingList == null || !existingList.Any())
                return "NTG001";

            int maxNumber = 0;
            foreach (var item in existingList)
            {
                if (!string.IsNullOrEmpty(item.MaNguoiThamGia) && item.MaNguoiThamGia.StartsWith("NTG", StringComparison.OrdinalIgnoreCase))
                {
                    var numPart = item.MaNguoiThamGia.Substring(3);
                    if (int.TryParse(numPart, out int num) && num > maxNumber)
                    {
                        maxNumber = num;
                    }
                }
            }

            return $"NTG{(maxNumber + 1):D3}";
        }

        /// <summary>
        /// Tra cứu lịch sử đăng ký vé và tham gia sự kiện của một cá nhân
        /// </summary>
        public List<AttendeeEventHistoryDto> GetAttendeeEventHistory(string maNguoiThamGia)
        {
            if (string.IsNullOrWhiteSpace(maNguoiThamGia))
                return new List<AttendeeEventHistoryDto>();

            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                string code = maNguoiThamGia.Trim();
                var list = (from dk in db.DangKys
                            where dk.MaNguoiThamGia == code
                            join sk in db.SuKiens on dk.MaSuKien equals sk.MaSuKien
                            join ct in db.ChiTietVes on dk.MaDangKy equals ct.MaDangKy into ctGroup
                            from subCt in ctGroup.DefaultIfEmpty()
                            join hv in db.HangVes on dk.MaHangVe equals hv.MaHangVe into hvGroup
                            from subHv in hvGroup.DefaultIfEmpty()
                            select new AttendeeEventHistoryDto
                            {
                                MaSuKien = sk.MaSuKien,
                                TenSuKien = sk.TenSuKien,
                                TenHangVe = subHv != null ? subHv.TenHangVe : "Vé tham dự",
                                NgayDangKy = dk.NgayDangKy,
                                SoLuong = 1,
                                TongTien = dk.TongTien,
                                TrangThaiThanhToan = dk.TrangThaiThanhToan,
                                TrangThaiCheckIn = subCt != null && subCt.CheckIn != null ? "Đã check-in" : (subCt != null ? subCt.TrangThaiVe : "Chưa check-in"),
                                ThoiGianCheckIn = subCt != null && subCt.CheckIn != null ? (DateTime?)subCt.CheckIn.ThoiGianCheckIn : null
                            }).ToList();

                return list;
            }
            catch
            {
                return new List<AttendeeEventHistoryDto>();
            }
        }
    }
}
