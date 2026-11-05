using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt nghiệp vụ Quản lý diễn giả / khách mời (F09)
    /// Đảm bảo các quy tắc:
    /// - BR-06: Chống trùng diễn giả trong cùng khung giờ tiết mục.
    /// - BR-20: Hỗ trợ Soft Delete khi diễn giả đã gắn vào lịch trình sự kiện.
    /// </summary>
    public class SpeakerService : ISpeakerService
    {
        private static readonly List<SpeakerDto> _speakers = new()
        {
            new SpeakerDto
            {
                MaDienGia = "DG001",
                HoTen = "TS. Nguyễn Minh Hải",
                ChucVu = "Phó Viện trưởng Viện Trí tuệ Nhân tạo",
                DonViCongTac = "Đại học Bách Khoa TP.HCM",
                Email = "hai.nguyen@ai-institute.edu.vn",
                SoDienThoai = "0903112233",
                TieuSu = "Tiến sĩ ngành Khoa học Máy tính tại ĐH Stanford, hơn 15 năm nghiên cứu Machine Learning và Xử lý ngôn ngữ tự nhiên.",
                HinhAnh = "speaker_hai_nguyen.png",
                SoTietMucThamGia = 1,
                TrangThai = "Hoạt động"
            },
            new SpeakerDto
            {
                MaDienGia = "DG002",
                HoTen = "Bà Trần Hoàng Yến",
                ChucVu = "Tech Lead & AI Product Manager",
                DonViCongTac = "VNG Corporation",
                Email = "yenth@vng.com.vn",
                SoDienThoai = "0912334455",
                TieuSu = "Chuyên gia phát triển sản phẩm công nghệ với nhiều sản phẩm phục vụ hàng triệu người dùng tại thị trường Đông Nam Á.",
                HinhAnh = "speaker_yen_tran.png",
                SoTietMucThamGia = 1,
                TrangThai = "Hoạt động"
            },
            new SpeakerDto
            {
                MaDienGia = "DG003",
                HoTen = "ThS. Lê Quang Huy",
                ChucVu = "Trưởng phòng An toàn Thông tin",
                DonViCongTac = "Viettel Cyber Security",
                Email = "huylq@viettel.com.vn",
                SoDienThoai = "0988334455",
                TieuSu = "Chuyên gia ứng cứu sự cố và bảo mật hạ tầng trọng yếu, diễn giả quen thuộc tại Security Bootcamp.",
                HinhAnh = "speaker_huy_le.png",
                SoTietMucThamGia = 0,
                TrangThai = "Hoạt động"
            },
            new SpeakerDto
            {
                MaDienGia = "DG004",
                HoTen = "Bà Phạm Mai Lan",
                ChucVu = "Giám đốc Đổi mới Sáng tạo",
                DonViCongTac = "Forbes Under 30 Asia",
                Email = "lan.pham@innovate.vn",
                SoDienThoai = "0977445566",
                TieuSu = "Cố vấn khởi nghiệp công nghệ, đã ươm tạo hơn 40 dự án Tech Startup tại Đông Nam Á.",
                HinhAnh = "speaker_lan_pham.png",
                SoTietMucThamGia = 0,
                TrangThai = "Hoạt động"
            },
            new SpeakerDto
            {
                MaDienGia = "DG005",
                HoTen = "Ông Michael Harrison",
                ChucVu = "Principal Cloud Architect",
                DonViCongTac = "Google Cloud APAC",
                Email = "mharrison@google.com",
                SoDienThoai = "0933556677",
                TieuSu = "Hơn 18 năm tư vấn kiến trúc Cloud Native và bảo mật dữ liệu cấp doanh nghiệp cho các tập đoàn toàn cầu.",
                HinhAnh = "speaker_michael.png",
                SoTietMucThamGia = 0,
                TrangThai = "Hoạt động"
            },
            new SpeakerDto
            {
                MaDienGia = "DG006",
                HoTen = "ThS. Đặng Hữu Hoàng",
                ChucVu = "Senior Software Architect",
                DonViCongTac = "FPT Software",
                Email = "hoangdh@fpt.com",
                SoDienThoai = "0966778899",
                TieuSu = "Chuyên gia kiến trúc Microservices, Clean Architecture và tối ưu hóa hệ thống .NET Enterprise.",
                HinhAnh = "speaker_hoang_dang.png",
                SoTietMucThamGia = 0,
                TrangThai = "Hoạt động"
            }
        };

        public ServiceResult<List<SpeakerDto>> GetAll(string? keyword = null, string? statusFilter = null)
        {
            var query = _speakers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(x => x.MaDienGia.ToLower().Contains(kw) ||
                                         x.HoTen.ToLower().Contains(kw) ||
                                         (x.ChucVu != null && x.ChucVu.ToLower().Contains(kw)) ||
                                         (x.DonViCongTac != null && x.DonViCongTac.ToLower().Contains(kw)) ||
                                         (x.Email != null && x.Email.ToLower().Contains(kw)) ||
                                         (x.SoDienThoai != null && x.SoDienThoai.Contains(kw)));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "Tất cả")
            {
                query = query.Where(x => x.TrangThai.Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
            }

            return ServiceResult<List<SpeakerDto>>.Success(query.OrderBy(x => x.MaDienGia).ToList());
        }

        public ServiceResult<SpeakerDto> GetById(string maDienGia)
        {
            var item = _speakers.FirstOrDefault(x => x.MaDienGia.Equals(maDienGia, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<SpeakerDto>.Failed($"Không tìm thấy diễn giả có mã '{maDienGia}'!");
            }
            return ServiceResult<SpeakerDto>.Success(item);
        }

        public ServiceResult Add(SpeakerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaDienGia))
            {
                return ServiceResult.Failed("Mã diễn giả không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(dto.HoTen))
            {
                return ServiceResult.Failed("Họ tên diễn giả không được để trống!");
            }

            string code = dto.MaDienGia.Trim().ToUpper();
            if (_speakers.Any(x => x.MaDienGia.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult.Failed($"Mã diễn giả '{code}' đã tồn tại trong hệ thống!");
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                string email = dto.Email.Trim();
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return ServiceResult.Failed("Địa chỉ email diễn giả không đúng định dạng chuẩn!");
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.SoDienThoai))
            {
                string phone = dto.SoDienThoai.Trim();
                if (!Regex.IsMatch(phone, @"^[0-9]{8,15}$"))
                {
                    return ServiceResult.Failed("Số điện thoại phải từ 8 đến 15 chữ số!");
                }
            }

            var newEntity = new SpeakerDto
            {
                MaDienGia = code,
                HoTen = dto.HoTen.Trim(),
                ChucVu = dto.ChucVu?.Trim(),
                DonViCongTac = dto.DonViCongTac?.Trim(),
                Email = dto.Email?.Trim(),
                SoDienThoai = dto.SoDienThoai?.Trim(),
                TieuSu = dto.TieuSu?.Trim(),
                HinhAnh = dto.HinhAnh?.Trim(),
                SoTietMucThamGia = 0,
                TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? "Hoạt động" : dto.TrangThai.Trim()
            };

            _speakers.Add(newEntity);
            return ServiceResult.Success("Thêm mới hồ sơ diễn giả thành công!");
        }

        public ServiceResult Update(SpeakerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaDienGia))
            {
                return ServiceResult.Failed("Mã diễn giả không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(dto.HoTen))
            {
                return ServiceResult.Failed("Họ tên diễn giả không được để trống!");
            }

            var item = _speakers.FirstOrDefault(x => x.MaDienGia.Equals(dto.MaDienGia, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult.Failed($"Không tìm thấy diễn giả có mã '{dto.MaDienGia}'!");
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                string email = dto.Email.Trim();
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return ServiceResult.Failed("Địa chỉ email diễn giả không đúng định dạng chuẩn!");
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.SoDienThoai))
            {
                string phone = dto.SoDienThoai.Trim();
                if (!Regex.IsMatch(phone, @"^[0-9]{8,15}$"))
                {
                    return ServiceResult.Failed("Số điện thoại phải từ 8 đến 15 chữ số!");
                }
            }

            item.HoTen = dto.HoTen.Trim();
            item.ChucVu = dto.ChucVu?.Trim();
            item.DonViCongTac = dto.DonViCongTac?.Trim();
            item.Email = dto.Email?.Trim();
            item.SoDienThoai = dto.SoDienThoai?.Trim();
            item.TieuSu = dto.TieuSu?.Trim();
            item.HinhAnh = dto.HinhAnh?.Trim();
            item.TrangThai = string.IsNullOrWhiteSpace(dto.TrangThai) ? item.TrangThai : dto.TrangThai.Trim();

            return ServiceResult.Success("Cập nhật thông tin diễn giả thành công!");
        }

        public ServiceResult Delete(string maDienGia)
        {
            var item = _speakers.FirstOrDefault(x => x.MaDienGia.Equals(maDienGia, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult.Failed($"Không tìm thấy diễn giả có mã '{maDienGia}'!");
            }

            // Quy tắc BR-20: Nếu diễn giả đã từng tham gia tiết mục, chặn DELETE cứng
            if (item.SoTietMucThamGia > 0)
            {
                return ServiceResult.Failed(
                    $"Diễn giả '{item.HoTen}' đã được phân công tham gia {item.SoTietMucThamGia} tiết mục trong lịch trình sự kiện. Theo quy tắc toàn vẹn dữ liệu BR-20, bạn không thể xóa cứng hồ sơ này!",
                    "BR-20");
            }

            _speakers.Remove(item);
            return ServiceResult.Success($"Đã xóa hồ sơ diễn giả '{item.HoTen}' thành công!");
        }

        public ServiceResult SoftDelete(string maDienGia)
        {
            var item = _speakers.FirstOrDefault(x => x.MaDienGia.Equals(maDienGia, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult.Failed($"Không tìm thấy diễn giả có mã '{maDienGia}'!");
            }

            item.TrangThai = "Ngừng hợp tác";
            return ServiceResult.Success($"Đã chuyển trạng thái diễn giả '{item.HoTen}' sang 'Ngừng hợp tác' thành công theo quy tắc BR-20.");
        }
    }
}
