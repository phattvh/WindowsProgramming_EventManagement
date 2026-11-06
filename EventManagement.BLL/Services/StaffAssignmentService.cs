using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt nghiệp vụ Phân công nhân sự theo ca trực (F15 / UC15)
    /// Tích hợp thuật toán lõi BR-13 của Thành viên 3 (StaffScheduleService)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-13/Day 6)
    /// </summary>
    public class StaffAssignmentService : IStaffAssignmentService
    {
        private readonly StaffScheduleService _shiftValidator;
        private readonly IStaffService _staffService;

        // Bảng dữ liệu sự kiện để kiểm tra và nạp ComboBox
        private static readonly List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> _events = new()
        {
            ("EVT202601", "Hội nghị Công nghệ Vietnam Tech Summit 2026", new DateTime(2026, 12, 15, 8, 0, 0), new DateTime(2026, 12, 15, 17, 30, 0)),
            ("EVT202602", "Workshop Thực chiến GenAI & Prompt Engineering 2026", new DateTime(2026, 11, 3, 8, 30, 0), new DateTime(2026, 11, 4, 17, 0, 0)),
            ("EVT202603", "Hội nghị Khoa học Quốc tế về Công nghệ Phần mềm & IoT", new DateTime(2026, 12, 25, 8, 0, 0), new DateTime(2026, 12, 26, 18, 0, 0))
        };

        // Danh sách phân công ca trực khởi tạo từ seed_data_member2.sql (7 ca trực thỏa mãn BR-13)
        private static readonly List<StaffAssignmentDto> _assignments = new()
        {
            new StaffAssignmentDto
            {
                MaPhanCong = "PC001",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                MaNhanSu = "NS001",
                TenNhanSu = "Trần Minh Tâm",
                SoDienThoai = "0909123001",
                ChucVuNhanSu = "Trưởng ban Hậu cần",
                PhongBan = "Hậu cần",
                NhiemVu = "Điều phối chung toàn bộ sự kiện",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 7, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 18, 0, 0),
                GhiChu = "Bao quát khu vực lễ tân và sân khấu chính"
            },
            new StaffAssignmentDto
            {
                MaPhanCong = "PC002",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                MaNhanSu = "NS004",
                TenNhanSu = "Vũ Thị Mai",
                SoDienThoai = "0909123004",
                ChucVuNhanSu = "Nhân viên Đón tiếp",
                PhongBan = "Lễ tân",
                NhiemVu = "Trực bàn đón tiếp khách VIP và đại biểu",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 7, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 12, 0, 0),
                GhiChu = "Đón tiếp các diễn giả và lãnh đạo doanh nghiệp"
            },
            new StaffAssignmentDto
            {
                MaPhanCong = "PC003",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                MaNhanSu = "NS005",
                TenNhanSu = "Phan Đình Tùng",
                SoDienThoai = "0909123005",
                ChucVuNhanSu = "Nhân viên Soát vé QR",
                PhongBan = "Hậu cần",
                NhiemVu = "Soát vé và quét mã QR Webcam cửa 1",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 7, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 12, 0, 0),
                GhiChu = "Sử dụng laptop quét mã QR trên vé in hoặc điện thoại"
            },
            new StaffAssignmentDto
            {
                MaPhanCong = "PC004",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                MaNhanSu = "NS007",
                TenNhanSu = "Nguyễn Tuấn Anh",
                SoDienThoai = "0909123007",
                ChucVuNhanSu = "Kỹ thuật viên Ánh sáng",
                PhongBan = "Kỹ thuật",
                NhiemVu = "Vận hành âm thanh ánh sáng hội trường",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 7, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 17, 30, 0),
                GhiChu = "Trực bàn điều khiển kỹ thuật P.A."
            },
            new StaffAssignmentDto
            {
                MaPhanCong = "PC005",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                MaNhanSu = "NS010",
                TenNhanSu = "Đỗ Văn Khang",
                SoDienThoai = "0909123010",
                ChucVuNhanSu = "Đội trưởng An ninh",
                PhongBan = "An ninh",
                NhiemVu = "Bảo đảm an ninh trật tự khu vực sảnh",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 6, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 18, 30, 0),
                GhiChu = "Phối hợp với bảo vệ tòa nhà phân luồng xe"
            },
            new StaffAssignmentDto
            {
                MaPhanCong = "PC006",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                MaNhanSu = "NS006",
                TenNhanSu = "Bùi Kim Ngân",
                SoDienThoai = "0909123006",
                ChucVuNhanSu = "Hướng dẫn khách mời",
                PhongBan = "Lễ tân",
                NhiemVu = "Điểm danh học viên và phát tài liệu lab",
                ThoiGianBatDau = new DateTime(2026, 11, 3, 8, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 11, 3, 12, 0, 0),
                GhiChu = "Phát thẻ đeo và tài liệu thực hành"
            },
            new StaffAssignmentDto
            {
                MaPhanCong = "PC007",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                MaNhanSu = "NS007",
                TenNhanSu = "Nguyễn Tuấn Anh",
                SoDienThoai = "0909123007",
                ChucVuNhanSu = "Kỹ thuật viên Ánh sáng",
                PhongBan = "Kỹ thuật",
                NhiemVu = "Hỗ trợ kỹ thuật mạng phòng máy tính",
                ThoiGianBatDau = new DateTime(2026, 11, 3, 8, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 11, 3, 17, 0, 0),
                GhiChu = "Xử lý sự cố kết nối Cloud cho học viên"
            }
        };

        public StaffAssignmentService() : this(new StaffScheduleService(), new StaffService())
        {
        }

        public StaffAssignmentService(StaffScheduleService shiftValidator, IStaffService staffService)
        {
            _shiftValidator = shiftValidator ?? new StaffScheduleService();
            _staffService = staffService ?? new StaffService();
        }

        public ServiceResult<List<StaffAssignmentDto>> GetAll(
            string? maSuKien = null,
            string? keyword = null,
            string? maNhanSu = null,
            string? phongBan = null)
        {
            IEnumerable<StaffAssignmentDto> query = _assignments;

            // 1. Lọc theo Sự kiện
            if (!string.IsNullOrWhiteSpace(maSuKien) && !maSuKien.StartsWith("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(a => a.MaSuKien.Equals(maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // 2. Lọc theo Nhân sự
            if (!string.IsNullOrWhiteSpace(maNhanSu) && !maNhanSu.StartsWith("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(a => a.MaNhanSu.Equals(maNhanSu.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // 3. Lọc theo Phòng ban
            if (!string.IsNullOrWhiteSpace(phongBan) && !phongBan.StartsWith("Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(a => a.PhongBan.Equals(phongBan.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            // 4. Tìm kiếm từ khóa đa trường
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(a =>
                    a.MaPhanCong.ToLower().Contains(kw) ||
                    a.TenNhanSu.ToLower().Contains(kw) ||
                    a.NhiemVu.ToLower().Contains(kw) ||
                    a.SoDienThoai.Contains(kw) ||
                    (a.GhiChu != null && a.GhiChu.ToLower().Contains(kw))
                );
            }

            var result = query
                .OrderBy(a => a.MaSuKien)
                .ThenBy(a => a.ThoiGianBatDau)
                .Select(CloneDto)
                .ToList();

            return ServiceResult<List<StaffAssignmentDto>>.Success(result);
        }

        public ServiceResult<StaffAssignmentDto> GetById(string maPhanCong)
        {
            if (string.IsNullOrWhiteSpace(maPhanCong))
            {
                return ServiceResult<StaffAssignmentDto>.Failed("Mã phân công không được để trống.");
            }

            var item = _assignments.FirstOrDefault(a => a.MaPhanCong.Equals(maPhanCong.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<StaffAssignmentDto>.Failed($"Không tìm thấy phân công có mã '{maPhanCong}'.");
            }

            return ServiceResult<StaffAssignmentDto>.Success(CloneDto(item));
        }

        public ServiceResult Add(StaffAssignmentDto dto)
        {
            if (dto == null)
            {
                return ServiceResult.Failed("Dữ liệu phân công không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaPhanCong))
            {
                return ServiceResult.Failed("Mã phân công không được để trống.");
            }

            if (_assignments.Any(a => a.MaPhanCong.Equals(dto.MaPhanCong.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult.Failed($"Mã phân công '{dto.MaPhanCong.Trim()}' đã tồn tại trong hệ thống.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaSuKien))
            {
                return ServiceResult.Failed("Vui lòng chọn sự kiện áp dụng cho ca trực.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaNhanSu))
            {
                return ServiceResult.Failed("Vui lòng chọn nhân sự được phân công.");
            }

            if (string.IsNullOrWhiteSpace(dto.NhiemVu))
            {
                return ServiceResult.Failed("Nhiệm vụ phân công không được để trống.");
            }

            if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
            {
                return ServiceResult.Failed("Thời gian kết thúc ca trực phải lớn hơn thời gian bắt đầu.");
            }

            // Kiểm tra quy tắc BR-13 (Chống trùng ca trực & Nhân sự hoạt động)
            var conflictCheck = CheckConflict(dto, null);
            if (!conflictCheck.IsSuccess)
            {
                return ServiceResult.Failed(conflictCheck.Message, conflictCheck.Errors);
            }

            // Điền thông tin sự kiện & nhân sự
            var evt = _events.FirstOrDefault(e => e.MaSuKien.Equals(dto.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            var staffRes = _staffService.GetById(dto.MaNhanSu.Trim());
            var staff = staffRes.Data;

            var newItem = new StaffAssignmentDto
            {
                MaPhanCong = dto.MaPhanCong.Trim().ToUpper(),
                MaSuKien = dto.MaSuKien.Trim(),
                TenSuKien = evt != default ? evt.TenSuKien : dto.TenSuKien,
                MaNhanSu = dto.MaNhanSu.Trim().ToUpper(),
                TenNhanSu = staff != null ? staff.HoTen : dto.TenNhanSu,
                SoDienThoai = staff != null ? staff.SoDienThoai : dto.SoDienThoai,
                ChucVuNhanSu = staff != null ? staff.ChucDanh : dto.ChucVuNhanSu,
                PhongBan = staff != null ? staff.PhongBan : dto.PhongBan,
                NhiemVu = dto.NhiemVu.Trim(),
                ThoiGianBatDau = dto.ThoiGianBatDau,
                ThoiGianKetThuc = dto.ThoiGianKetThuc,
                GhiChu = string.IsNullOrWhiteSpace(dto.GhiChu) ? null : dto.GhiChu.Trim()
            };

            _assignments.Add(newItem);
            return ServiceResult.Success("Thêm mới phân công ca trực nhân sự thành công!");
        }

        public ServiceResult Update(StaffAssignmentDto dto)
        {
            if (dto == null)
            {
                return ServiceResult.Failed("Dữ liệu phân công không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaPhanCong))
            {
                return ServiceResult.Failed("Mã phân công không được để trống.");
            }

            var existing = _assignments.FirstOrDefault(a => a.MaPhanCong.Equals(dto.MaPhanCong.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy phân công có mã '{dto.MaPhanCong}' để cập nhật.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaSuKien))
            {
                return ServiceResult.Failed("Vui lòng chọn sự kiện áp dụng cho ca trực.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaNhanSu))
            {
                return ServiceResult.Failed("Vui lòng chọn nhân sự được phân công.");
            }

            if (string.IsNullOrWhiteSpace(dto.NhiemVu))
            {
                return ServiceResult.Failed("Nhiệm vụ phân công không được để trống.");
            }

            if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
            {
                return ServiceResult.Failed("Thời gian kết thúc ca trực phải lớn hơn thời gian bắt đầu.");
            }

            // Kiểm tra quy tắc BR-13 (loại trừ chính mã phân công đang sửa)
            var conflictCheck = CheckConflict(dto, dto.MaPhanCong.Trim());
            if (!conflictCheck.IsSuccess)
            {
                return ServiceResult.Failed(conflictCheck.Message, conflictCheck.Errors);
            }

            var evt = _events.FirstOrDefault(e => e.MaSuKien.Equals(dto.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            var staffRes = _staffService.GetById(dto.MaNhanSu.Trim());
            var staff = staffRes.Data;

            existing.MaSuKien = dto.MaSuKien.Trim();
            existing.TenSuKien = evt != default ? evt.TenSuKien : dto.TenSuKien;
            existing.MaNhanSu = dto.MaNhanSu.Trim().ToUpper();
            existing.TenNhanSu = staff != null ? staff.HoTen : dto.TenNhanSu;
            existing.SoDienThoai = staff != null ? staff.SoDienThoai : dto.SoDienThoai;
            existing.ChucVuNhanSu = staff != null ? staff.ChucDanh : dto.ChucVuNhanSu;
            existing.PhongBan = staff != null ? staff.PhongBan : dto.PhongBan;
            existing.NhiemVu = dto.NhiemVu.Trim();
            existing.ThoiGianBatDau = dto.ThoiGianBatDau;
            existing.ThoiGianKetThuc = dto.ThoiGianKetThuc;
            existing.GhiChu = string.IsNullOrWhiteSpace(dto.GhiChu) ? null : dto.GhiChu.Trim();

            return ServiceResult.Success("Cập nhật thông tin phân công ca trực thành công!");
        }

        public ServiceResult Delete(string maPhanCong)
        {
            if (string.IsNullOrWhiteSpace(maPhanCong))
            {
                return ServiceResult.Failed("Mã phân công không được để trống.");
            }

            var existing = _assignments.FirstOrDefault(a => a.MaPhanCong.Equals(maPhanCong.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy phân công có mã '{maPhanCong}' để xóa.");
            }

            _assignments.Remove(existing);
            return ServiceResult.Success($"Đã xóa thành công ca trực [{maPhanCong}] của nhân sự '{existing.TenNhanSu}'.");
        }

        public ServiceResult<bool> CheckConflict(StaffAssignmentDto dto, string? excludeMaPhanCong = null)
        {
            if (dto == null)
            {
                return ServiceResult<bool>.Failed("Dữ liệu cần kiểm tra không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaNhanSu))
            {
                return ServiceResult<bool>.Failed("Mã nhân sự không được để trống khi kiểm tra xung đột.");
            }

            // 1. Kiểm tra trạng thái hoạt động của nhân sự (Quy tắc BR-13)
            var staffRes = _staffService.GetById(dto.MaNhanSu.Trim());
            if (!staffRes.IsSuccess || staffRes.Data == null)
            {
                return ServiceResult<bool>.Failed($"Không tìm thấy nhân sự có mã '{dto.MaNhanSu}' trong hệ thống.");
            }

            if (!staffRes.Data.TrangThai.Equals("Hoạt động", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<bool>.Failed(
                    $"Xung đột phân công (BR-13): Nhân sự [{dto.MaNhanSu} - {staffRes.Data.HoTen}] hiện đang ở trạng thái '{staffRes.Data.TrangThai}', không thể phân công ca trực.",
                    new List<string> { $"Nhân sự '{staffRes.Data.HoTen}' đã ngừng làm việc hoặc nghỉ việc (Trạng thái = {staffRes.Data.TrangThai})." }
                );
            }

            if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
            {
                return ServiceResult<bool>.Failed(
                    "Thời gian kết thúc ca trực phải lớn hơn thời gian bắt đầu.",
                    new List<string> { $"ThoiGianBatDau ({dto.ThoiGianBatDau:HH:mm}) >= ThoiGianKetThuc ({dto.ThoiGianKetThuc:HH:mm})" }
                );
            }

            // 2. Thuật toán BR-13 kiểm tra trùng ca giao thoa thời gian (Gọi qua StaffScheduleService của TV3)
            var otherAssignments = _assignments.Where(a =>
                string.IsNullOrWhiteSpace(excludeMaPhanCong) ||
                !a.MaPhanCong.Equals(excludeMaPhanCong.Trim(), StringComparison.OrdinalIgnoreCase)
            );

            return _shiftValidator.CheckShiftConflict(
                dto.MaNhanSu.Trim(),
                dto.ThoiGianBatDau,
                dto.ThoiGianKetThuc,
                excludeMaPhanCong,
                otherAssignments
            );
        }

        public List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> GetAvailableEvents()
        {
            return _events.ToList();
        }

        public List<StaffDto> GetAvailableStaff()
        {
            var res = _staffService.GetAll();
            return res.IsSuccess && res.Data != null ? res.Data : new List<StaffDto>();
        }

        public string SuggestNextCode()
        {
            int maxId = 0;
            foreach (var item in _assignments)
            {
                if (item.MaPhanCong.StartsWith("PC", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(item.MaPhanCong.Substring(2), out int num))
                {
                    if (num > maxId) maxId = num;
                }
            }

            return $"PC{(maxId + 1):D3}";
        }

        private static StaffAssignmentDto CloneDto(StaffAssignmentDto src)
        {
            return new StaffAssignmentDto
            {
                MaPhanCong = src.MaPhanCong,
                MaSuKien = src.MaSuKien,
                TenSuKien = src.TenSuKien,
                MaNhanSu = src.MaNhanSu,
                TenNhanSu = src.TenNhanSu,
                SoDienThoai = src.SoDienThoai,
                ChucVuNhanSu = src.ChucVuNhanSu,
                PhongBan = src.PhongBan,
                NhiemVu = src.NhiemVu,
                ThoiGianBatDau = src.ThoiGianBatDau,
                ThoiGianKetThuc = src.ThoiGianKetThuc,
                GhiChu = src.GhiChu
            };
        }
    }
}
