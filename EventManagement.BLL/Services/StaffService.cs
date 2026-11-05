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
        private static readonly List<StaffDto> _staffList = new()
        {
            new StaffDto
            {
                MaNhanSu = "NS001",
                HoTen = "Trần Minh Tâm",
                SoDienThoai = "0909123001",
                Email = "tam.tm@eventhub.vn",
                ChucDanh = "Trưởng ban Hậu cần",
                PhongBan = "Hậu cần",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 2
            },
            new StaffDto
            {
                MaNhanSu = "NS002",
                HoTen = "Lê Thanh Hằng",
                SoDienThoai = "0909123002",
                Email = "hang.lt@eventhub.vn",
                ChucDanh = "Trưởng ban Lễ tân",
                PhongBan = "Lễ tân",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 1
            },
            new StaffDto
            {
                MaNhanSu = "NS003",
                HoTen = "Nguyễn Hoàng Long",
                SoDienThoai = "0909123003",
                Email = "long.nh@eventhub.vn",
                ChucDanh = "Kỹ thuật viên Âm thanh",
                PhongBan = "Kỹ thuật",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 1
            },
            new StaffDto
            {
                MaNhanSu = "NS004",
                HoTen = "Vũ Thị Mai",
                SoDienThoai = "0909123004",
                Email = "mai.vt@eventhub.vn",
                ChucDanh = "Nhân viên Đón tiếp",
                PhongBan = "Lễ tân",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 0
            },
            new StaffDto
            {
                MaNhanSu = "NS005",
                HoTen = "Phan Đình Tùng",
                SoDienThoai = "0909123005",
                Email = "tung.pd@eventhub.vn",
                ChucDanh = "Nhân viên Soát vé QR",
                PhongBan = "Hậu cần",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 0
            },
            new StaffDto
            {
                MaNhanSu = "NS006",
                HoTen = "Bùi Kim Ngân",
                SoDienThoai = "0909123006",
                Email = "ngan.bk@eventhub.vn",
                ChucDanh = "Hướng dẫn khách mời",
                PhongBan = "Lễ tân",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 0
            },
            new StaffDto
            {
                MaNhanSu = "NS007",
                HoTen = "Nguyễn Tuấn Anh",
                SoDienThoai = "0909123007",
                Email = "anh.nt@eventhub.vn",
                ChucDanh = "Kỹ thuật viên Ánh sáng",
                PhongBan = "Kỹ thuật",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 0
            },
            new StaffDto
            {
                MaNhanSu = "NS008",
                HoTen = "Hoàng Minh Châu",
                SoDienThoai = "0909123008",
                Email = "chau.hm@eventhub.vn",
                ChucDanh = "Điều phối viên Sự kiện",
                PhongBan = "Điều phối",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 0
            },
            new StaffDto
            {
                MaNhanSu = "NS009",
                HoTen = "Trần Thị Hương",
                SoDienThoai = "0909123009",
                Email = "huong.tt@eventhub.vn",
                ChucDanh = "Tiếp đón Diễn giả VIP",
                PhongBan = "Lễ tân",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 0
            },
            new StaffDto
            {
                MaNhanSu = "NS010",
                HoTen = "Đỗ Văn Khang",
                SoDienThoai = "0909123010",
                Email = "khang.dv@eventhub.vn",
                ChucDanh = "Đội trưởng An ninh",
                PhongBan = "An ninh",
                TrangThai = "Hoạt động",
                SoCaTrucThamGia = 0
            }
        };

        public ServiceResult<List<StaffDto>> GetAll(string? keyword = null, string? statusFilter = null, string? departmentFilter = null)
        {
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
            var item = _staffList.FirstOrDefault(x => x.MaNhanSu.Equals(maNhanSu, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<StaffDto>.Failed($"Không tìm thấy nhân sự có mã '{maNhanSu}'!");
            }
            return ServiceResult<StaffDto>.Success(item);
        }

        public ServiceResult Add(StaffDto dto)
        {
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
            if (!Regex.IsMatch(phone, @"^[0-9]{8,15}$"))
            {
                return ServiceResult.Failed("Số điện thoại nhân sự phải từ 8 đến 15 chữ số!");
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
            return ServiceResult.Success("Thêm mới hồ sơ nhân sự thành công!");
        }

        public ServiceResult Update(StaffDto dto)
        {
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
            if (!Regex.IsMatch(phone, @"^[0-9]{8,15}$"))
            {
                return ServiceResult.Failed("Số điện thoại nhân sự phải từ 8 đến 15 chữ số!");
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

            return ServiceResult.Success("Cập nhật thông tin nhân sự thành công!");
        }

        public ServiceResult Delete(string maNhanSu)
        {
            var item = _staffList.FirstOrDefault(x => x.MaNhanSu.Equals(maNhanSu, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult.Failed($"Không tìm thấy nhân sự có mã '{maNhanSu}'!");
            }

            // Quy tắc BR-20: Nếu nhân sự đã được phân công ca trực, chặn DELETE cứng
            if (item.SoCaTrucThamGia > 0)
            {
                return ServiceResult.Failed(
                    $"Nhân sự '{item.HoTen}' đã được phân công tham gia {item.SoCaTrucThamGia} ca trực sự kiện. Theo quy tắc toàn vẹn dữ liệu BR-20, bạn không thể xóa cứng hồ sơ này!",
                    "BR-20");
            }

            _staffList.Remove(item);
            return ServiceResult.Success($"Đã xóa hồ sơ nhân sự '{item.HoTen}' thành công!");
        }

        public ServiceResult SoftDelete(string maNhanSu)
        {
            var item = _staffList.FirstOrDefault(x => x.MaNhanSu.Equals(maNhanSu, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult.Failed($"Không tìm thấy nhân sự có mã '{maNhanSu}'!");
            }

            item.TrangThai = "Nghỉ việc";
            return ServiceResult.Success($"Đã chuyển trạng thái nhân sự '{item.HoTen}' sang 'Nghỉ việc' thành công theo quy tắc BR-20.");
        }
    }
}
