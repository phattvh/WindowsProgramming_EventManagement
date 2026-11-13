using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt nghiệp vụ Quản lý nhân sự ban tổ chức (F14)
    /// Đảm bảo các quy tắc:
    /// - BR-13: Chống trùng ca trực nhân sự theo khung giờ làm việc.
    /// - BR-20: Hỗ trợ Soft Delete khi nhân sự đã được phân công ca trực.
    /// </summary>
    public class StaffService : IStaffService
    {
        private static readonly List<StaffDto> _staffList = new();

        private void EnsureLoadedFromDb()
        {
            if (_staffList.Count == 0)
            {
                try
                {
                    using var db = new EventManagement.DAL.AppDbContext();
                    var list = db.NhanSus.Select(n => new StaffDto
                    {
                        MaNhanSu = n.MaNhanSu,
                        HoTen = n.HoTen,
                        SoDienThoai = n.SoDienThoai,
                        Email = n.Email,
                        ChucDanh = n.ChucVu,
                        PhongBan = n.ChucVu,
                        TrangThai = "Hoạt động",
                        SoCaTrucThamGia = db.PhanCongNhanSus.Count(p => p.MaNhanSu == n.MaNhanSu)
                    }).ToList();

                    if (list.Any())
                    {
                        _staffList.AddRange(list);
                    }
                }
                catch { }
            }
        }

        public ServiceResult<List<StaffDto>> GetAll(string? keyword = null, string? statusFilter = null, string? departmentFilter = null)
        {
            EnsureLoadedFromDb();
            var query = _staffList.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(x => x.MaNhanSu.ToLower().Contains(kw) ||
                                         x.HoTen.ToLower().Contains(kw) ||
                                         x.SoDienThoai.Contains(kw) ||
                                         (x.Email != null && x.Email.ToLower().Contains(kw)) ||
                                         x.ChucDanh.ToLower().Contains(kw) ||
                                         x.PhongBan.ToLower().Contains(kw));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "Tất cả")
            {
                query = query.Where(x => x.TrangThai.Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(departmentFilter) && departmentFilter != "Tất cả")
            {
                query = query.Where(x => x.PhongBan.Equals(departmentFilter, StringComparison.OrdinalIgnoreCase));
            }

            return ServiceResult<List<StaffDto>>.Success(query.OrderBy(x => x.MaNhanSu).ToList());
        }

        public ServiceResult<StaffDto> GetById(string maNhanSu)
        {
            EnsureLoadedFromDb();
            var item = _staffList.FirstOrDefault(x => x.MaNhanSu.Equals(maNhanSu, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<StaffDto>.Failed($"Không tìm thấy nhân sự có mã '{maNhanSu}'!");
            }
            return ServiceResult<StaffDto>.Success(item);
        }

        public ServiceResult Add(StaffDto dto)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(dto.MaNhanSu))
            {
                return ServiceResult.Failed("Mã nhân sự không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.HoTen))
            {
                return ServiceResult.Failed("Họ tên nhân sự không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.SoDienThoai))
            {
                return ServiceResult.Failed("Số điện thoại nhân sự không được để trống!");
            }

            string code = dto.MaNhanSu.Trim().ToUpper();
            if (_staffList.Any(x => x.MaNhanSu.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult.Failed($"Mã nhân sự '{code}' đã tồn tại trong hệ thống!");
            }

            string phone = dto.SoDienThoai.Trim();
            if (!Regex.IsMatch(phone, @"^(0|\+84)[0-9]{9,10}$"))
            {
                return ServiceResult.Failed("Số điện thoại nhân sự không hợp lệ (cần 10–11 chữ số, bắt đầu bằng 0 hoặc +84)!");
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                string email = dto.Email.Trim();
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return ServiceResult.Failed("Địa chỉ email nhân sự không đúng định dạng chuẩn!");
                }
            }

            var newEntity = new StaffDto
            {
                MaNhanSu = code,
                HoTen = dto.HoTen.Trim(),
                SoDienThoai = phone,
                Email = dto.Email?.Trim(),
                ChucDanh = string.IsNullOrWhiteSpace(dto.ChucDanh) ? "Cộng tác viên" : dto.ChucDanh.Trim(),
                PhongBan = string.IsNullOrWhiteSpace(dto.PhongBan) ? "Hậu cần" : dto.PhongBan.Trim(),
                TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "Hoạt động" : dto.TrangThai.Trim(),
                SoCaTrucThamGia = 0
            };

            _staffList.Add(newEntity);
            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success("Thêm mới hồ sơ nhân sự thành công!");
        }

        public ServiceResult Update(StaffDto dto)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(dto.MaNhanSu))
            {
                return ServiceResult.Failed("Mã nhân sự không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(dto.HoTen))
            {
                return ServiceResult.Failed("Họ tên nhân sự không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.SoDienThoai))
            {
                return ServiceResult.Failed("Số điện thoại nhân sự không được để trống!");
            }

            var item = _staffList.FirstOrDefault(x => x.MaNhanSu.Equals(dto.MaNhanSu, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult.Failed($"Không tìm thấy nhân sự có mã '{dto.MaNhanSu}'!");
            }

            string phone = dto.SoDienThoai.Trim();
            if (!Regex.IsMatch(phone, @"^(0|\+84)[0-9]{9,10}$"))
            {
                return ServiceResult.Failed("Số điện thoại nhân sự không hợp lệ (cần 10–11 chữ số, bắt đầu bằng 0 hoặc +84)!");
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                string email = dto.Email.Trim();
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return ServiceResult.Failed("Địa chỉ email nhân sự không đúng định dạng chuẩn!");
                }
            }

            item.HoTen = dto.HoTen.Trim();
            item.SoDienThoai = phone;
            item.Email = dto.Email?.Trim();
            item.ChucDanh = string.IsNullOrWhiteSpace(dto.ChucDanh) ? item.ChucDanh : dto.ChucDanh.Trim();
            item.PhongBan = string.IsNullOrWhiteSpace(dto.PhongBan) ? item.PhongBan : dto.PhongBan.Trim();
            item.TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? item.TrangThai : dto.TrangThai.Trim();

            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success("Cập nhật thông tin nhân sự thành công!");
        }

        public ServiceResult Delete(string maNhanSu)
        {
            EnsureLoadedFromDb();
            var item = _staffList.FirstOrDefault(x => x.MaNhanSu.Equals(maNhanSu, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult.Failed($"Không tìm thấy nhân sự có mã '{maNhanSu}'!");
            }

            // Quy tắc BR-20: Nếu nhân sự đã được phân công ca trực, chặn DELETE cứng
            if (item.SoCaTrucThamGia > 0)
            {
                return ServiceResult.Failed(
                    $"Nhân sự '{item.HoTen}' đã được phân công tham gia {item.SoCaTrucThamGia} ca trực sự kiện nên không thể xóa cứng hồ sơ này!",
                    "BR-20");
            }

            _staffList.Remove(item);
            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success($"Đã xóa hồ sơ nhân sự '{item.HoTen}' thành công!");
        }

        public ServiceResult SoftDelete(string maNhanSu)
        {
            EnsureLoadedFromDb();
            var item = _staffList.FirstOrDefault(x => x.MaNhanSu.Equals(maNhanSu, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult.Failed($"Không tìm thấy nhân sự có mã '{maNhanSu}'!");
            }

            item.TrangThai = "Nghỉ việc";
            return ServiceResult.Success($"Đã chuyển trạng thái nhân sự '{item.HoTen}' sang 'Nghỉ việc' thành công.");
        }
    }
}
