using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt nghiệp vụ Quản lý lịch trình / chương trình tiết mục (F08 / UC08)
    /// Đảm bảo các quy tắc nghiệp vụ:
    /// - BR-05: Khung giờ tiết mục con phải nằm hoàn toàn trong thời gian diễn ra sự kiện.
    /// - BR-06: Chống trùng diễn giả & chống trùng phòng chức năng trong cùng thời điểm.
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện - Task T2-11)
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        private readonly ScheduleValidationService _validator;

        // Bảng dữ liệu sự kiện để kiểm tra BR-05 và đổ dữ liệu ComboBox
        private static readonly List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> _events = new()
        {
            ("EVT202601", "Hội nghị Công nghệ Vietnam Tech Summit 2026", new DateTime(2026, 12, 15, 8, 0, 0), new DateTime(2026, 12, 15, 17, 30, 0)),
            ("EVT202602", "Workshop Thực chiến GenAI & Prompt Engineering 2026", new DateTime(2026, 11, 3, 8, 30, 0), new DateTime(2026, 11, 4, 17, 0, 0)),
            ("EVT202603", "Hội nghị Khoa học Quốc tế về Công nghệ Phần mềm & IoT", new DateTime(2026, 12, 25, 8, 0, 0), new DateTime(2026, 12, 26, 18, 0, 0))
        };

        // Bảng dữ liệu diễn giả để ánh xạ tên và đổ dữ liệu ComboBox
        private static readonly Dictionary<string, string> _speakers = new(StringComparer.OrdinalIgnoreCase)
        {
            ["DG001"] = "TS. Nguyễn Minh Hải",
            ["DG002"] = "Bà Trần Hoàng Yến",
            ["DG003"] = "ThS. Lê Quang Huy",
            ["DG004"] = "Bà Phạm Mai Lan",
            ["DG005"] = "Ông Michael Harrison",
            ["DG006"] = "ThS. Đặng Hữu Hoàng"
        };

        // Danh sách lưu trữ lịch trình khởi tạo từ CSDL mẫu (02_SeedData & seed_data_member2)
        private static readonly List<ScheduleItemDto> _schedules = new()
        {
            new ScheduleItemDto
            {
                MaLichTrinh = "LT001",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                TenTietMuc = "Khai mạc & Keynote: Xu hướng GenAI & Agentic Coding 2026",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 8, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 10, 0, 0),
                MaDienGia = "DG001",
                TenDienGia = "TS. Nguyễn Minh Hải",
                DiaDiemChiTiet = "Hội trường chính Grand Ballroom",
                GhiChu = "Trình chiếu keynote khai mạc"
            },
            new ScheduleItemDto
            {
                MaLichTrinh = "LT002",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                TenTietMuc = "Tọa đàm: Bảo mật thông tin & Kiến trúc Cloud-Native",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 10, 15, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 11, 45, 0),
                MaDienGia = "DG002",
                TenDienGia = "Bà Trần Hoàng Yến",
                DiaDiemChiTiet = "Phòng Hội thảo 2",
                GhiChu = "Phiên hỏi đáp Q&A cùng khán giả"
            },
            new ScheduleItemDto
            {
                MaLichTrinh = "LT003",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                TenTietMuc = "Tiệc Teabreak & Kết nối Doanh nghiệp B2B",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 10, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 10, 30, 0),
                MaDienGia = null,
                TenDienGia = null,
                DiaDiemChiTiet = "Sảnh Expo Lounge",
                GhiChu = "Thưởng thức trà, cafe và giao lưu kết nối mạng lưới"
            },
            new ScheduleItemDto
            {
                MaLichTrinh = "LT004",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                TenTietMuc = "Kiến trúc Hệ thống Phân tán Siêu quy mô (Hyperscale Systems)",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 13, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 15, 0, 0),
                MaDienGia = "DG005",
                TenDienGia = "Ông Michael Harrison",
                DiaDiemChiTiet = "Hội trường chính Grand Ballroom",
                GhiChu = "Phiên chuyên sâu về hạ tầng Cloud quy mô lớn"
            },
            new ScheduleItemDto
            {
                MaLichTrinh = "LT005",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                TenTietMuc = "Panel Discussion: Tương lai Lập trình viên trong kỷ nguyên AI",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 15, 15, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 16, 30, 0),
                MaDienGia = "DG006",
                TenDienGia = "ThS. Đặng Hữu Hoàng",
                DiaDiemChiTiet = "Hội trường chính Grand Ballroom",
                GhiChu = "Thảo luận bàn tròn cùng các CTO đầu ngành"
            },
            new ScheduleItemDto
            {
                MaLichTrinh = "LT006",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                TenTietMuc = "Khai mạc & Hướng dẫn Cài đặt Môi trường Lab",
                ThoiGianBatDau = new DateTime(2026, 11, 3, 8, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 11, 3, 9, 30, 0),
                MaDienGia = "DG006",
                TenDienGia = "ThS. Đặng Hữu Hoàng",
                DiaDiemChiTiet = "Phòng Lab Kỹ thuật A",
                GhiChu = "Cấp tài khoản Cloud và khởi tạo workspace"
            },
            new ScheduleItemDto
            {
                MaLichTrinh = "LT007",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                TenTietMuc = "Thực chiến Prompt Engineering & Kỹ thuật RAG Nâng cao",
                ThoiGianBatDau = new DateTime(2026, 11, 3, 9, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 11, 3, 12, 0, 0),
                MaDienGia = "DG003",
                TenDienGia = "ThS. Lê Quang Huy",
                DiaDiemChiTiet = "Phòng Lab Kỹ thuật A",
                GhiChu = "Bài tập lab tương tác trực tiếp 1-1"
            },
            new ScheduleItemDto
            {
                MaLichTrinh = "LT008",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                TenTietMuc = "Tích hợp AI Agents vào Phần mềm C# Windows Forms",
                ThoiGianBatDau = new DateTime(2026, 11, 3, 13, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 11, 3, 17, 0, 0),
                MaDienGia = "DG006",
                TenDienGia = "ThS. Đặng Hữu Hoàng",
                DiaDiemChiTiet = "Phòng Lab Kỹ thuật A",
                GhiChu = "Hướng dẫn gọi API và streaming response lên UI"
            }
        };

        public ScheduleService() : this(new ScheduleValidationService())
        {
        }

        public ScheduleService(ScheduleValidationService validator)
        {
            _validator = validator ?? new ScheduleValidationService();
        }

        public ServiceResult<List<ScheduleItemDto>> GetAll(string? maSuKien = null, string? keyword = null, string? maDienGia = null)
        {
            try
            {
                var query = _schedules.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(maSuKien) && !maSuKien.Equals("Tất cả sự kiện", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(s => s.MaSuKien.Equals(maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(maDienGia) && !maDienGia.Equals("Tất cả diễn giả", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(s => !string.IsNullOrEmpty(s.MaDienGia) && s.MaDienGia.Equals(maDienGia.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string kw = keyword.Trim().ToLower();
                    query = query.Where(s =>
                        s.MaLichTrinh.ToLower().Contains(kw) ||
                        s.TenTietMuc.ToLower().Contains(kw) ||
                        (!string.IsNullOrEmpty(s.DiaDiemChiTiet) && s.DiaDiemChiTiet.ToLower().Contains(kw)) ||
                        (!string.IsNullOrEmpty(s.TenDienGia) && s.TenDienGia.ToLower().Contains(kw)) ||
                        (!string.IsNullOrEmpty(s.GhiChu) && s.GhiChu.ToLower().Contains(kw))
                    );
                }

                var list = query
                    .OrderBy(s => s.MaSuKien)
                    .ThenBy(s => s.ThoiGianBatDau)
                    .Select(CloneDto)
                    .ToList();

                return ServiceResult<List<ScheduleItemDto>>.Success(list);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<ScheduleItemDto>>.Failed($"Lỗi lấy danh sách lịch trình: {ex.Message}");
            }
        }

        public ServiceResult<ScheduleItemDto> GetById(string maLichTrinh)
        {
            if (string.IsNullOrWhiteSpace(maLichTrinh))
            {
                return ServiceResult<ScheduleItemDto>.Failed("Mã lịch trình không được để trống.");
            }

            var item = _schedules.FirstOrDefault(s => s.MaLichTrinh.Equals(maLichTrinh.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<ScheduleItemDto>.Failed($"Không tìm thấy lịch trình có mã '{maLichTrinh}'.");
            }

            return ServiceResult<ScheduleItemDto>.Success(CloneDto(item));
        }

        public ServiceResult Add(ScheduleItemDto dto)
        {
            if (dto == null)
            {
                return ServiceResult.Failed("Dữ liệu lịch trình không được để trống.");
            }

            // 1. Kiểm tra tính hợp lệ cơ bản
            if (string.IsNullOrWhiteSpace(dto.MaLichTrinh))
            {
                return ServiceResult.Failed("Mã lịch trình không được để trống.");
            }

            dto.MaLichTrinh = dto.MaLichTrinh.Trim().ToUpper();

            if (_schedules.Any(s => s.MaLichTrinh.Equals(dto.MaLichTrinh, StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult.Failed($"Mã lịch trình '{dto.MaLichTrinh}' đã tồn tại trong hệ thống. Vui lòng chọn mã khác.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaSuKien))
            {
                return ServiceResult.Failed("Vui lòng chọn sự kiện áp dụng cho lịch trình.");
            }

            if (string.IsNullOrWhiteSpace(dto.TenTietMuc))
            {
                return ServiceResult.Failed("Tên chương trình / tiết mục không được để trống.");
            }

            if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
            {
                return ServiceResult.Failed("Thời gian kết thúc tiết mục phải lớn hơn thời gian bắt đầu.");
            }

            // 2. Tìm thông tin sự kiện và kiểm tra BR-05 & BR-06
            var evt = _events.FirstOrDefault(e => e.MaSuKien.Equals(dto.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            DateTime? eventStart = evt != default ? evt.BatDau : null;
            DateTime? eventEnd = evt != default ? evt.KetThuc : null;

            var valResult = _validator.ValidateScheduleItem(dto, _schedules, eventStart, eventEnd);
            if (!valResult.IsSuccess)
            {
                return ServiceResult.Failed(valResult.Message, valResult.Errors);
            }

            // 3. Chuẩn hóa dữ liệu tên sự kiện & tên diễn giả
            dto.TenSuKien = evt != default ? evt.TenSuKien : dto.MaSuKien;
            if (!string.IsNullOrWhiteSpace(dto.MaDienGia) && _speakers.TryGetValue(dto.MaDienGia.Trim(), out string? spkName))
            {
                dto.TenDienGia = spkName;
            }

            _schedules.Add(CloneDto(dto));
            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success("Thêm mới tiết mục lịch trình thành công!");
        }

        public ServiceResult Update(ScheduleItemDto dto)
        {
            if (dto == null)
            {
                return ServiceResult.Failed("Dữ liệu lịch trình không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaLichTrinh))
            {
                return ServiceResult.Failed("Mã lịch trình không được để trống.");
            }

            var existing = _schedules.FirstOrDefault(s => s.MaLichTrinh.Equals(dto.MaLichTrinh.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy lịch trình có mã '{dto.MaLichTrinh}' để cập nhật.");
            }

            if (string.IsNullOrWhiteSpace(dto.MaSuKien))
            {
                return ServiceResult.Failed("Vui lòng chọn sự kiện áp dụng cho lịch trình.");
            }

            if (string.IsNullOrWhiteSpace(dto.TenTietMuc))
            {
                return ServiceResult.Failed("Tên chương trình / tiết mục không được để trống.");
            }

            if (dto.ThoiGianKetThuc <= dto.ThoiGianBatDau)
            {
                return ServiceResult.Failed("Thời gian kết thúc tiết mục phải lớn hơn thời gian bắt đầu.");
            }

            // Kiểm tra BR-05 & BR-06 (loại trừ chính bản ghi đang cập nhật)
            var evt = _events.FirstOrDefault(e => e.MaSuKien.Equals(dto.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            DateTime? eventStart = evt != default ? evt.BatDau : null;
            DateTime? eventEnd = evt != default ? evt.KetThuc : null;

            var otherSchedules = _schedules.Where(s => !s.MaLichTrinh.Equals(dto.MaLichTrinh.Trim(), StringComparison.OrdinalIgnoreCase));
            var valResult = _validator.ValidateScheduleItem(dto, otherSchedules, eventStart, eventEnd);
            if (!valResult.IsSuccess)
            {
                return ServiceResult.Failed(valResult.Message, valResult.Errors);
            }

            // Cập nhật thông tin
            existing.MaSuKien = dto.MaSuKien.Trim();
            existing.TenSuKien = evt != default ? evt.TenSuKien : dto.TenSuKien;
            existing.TenTietMuc = dto.TenTietMuc.Trim();
            existing.ThoiGianBatDau = dto.ThoiGianBatDau;
            existing.ThoiGianKetThuc = dto.ThoiGianKetThuc;
            existing.MaDienGia = string.IsNullOrWhiteSpace(dto.MaDienGia) ? null : dto.MaDienGia.Trim();
            if (!string.IsNullOrWhiteSpace(existing.MaDienGia) && _speakers.TryGetValue(existing.MaDienGia, out string? spkName))
            {
                existing.TenDienGia = spkName;
            }
            else
            {
                existing.TenDienGia = dto.TenDienGia;
            }
            existing.DiaDiemChiTiet = string.IsNullOrWhiteSpace(dto.DiaDiemChiTiet) ? null : dto.DiaDiemChiTiet.Trim();
            existing.GhiChu = string.IsNullOrWhiteSpace(dto.GhiChu) ? null : dto.GhiChu.Trim();

            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success("Cập nhật thông tin lịch trình thành công!");
        }

        public ServiceResult Delete(string maLichTrinh)
        {
            if (string.IsNullOrWhiteSpace(maLichTrinh))
            {
                return ServiceResult.Failed("Mã lịch trình không được để trống.");
            }

            var existing = _schedules.FirstOrDefault(s => s.MaLichTrinh.Equals(maLichTrinh.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                return ServiceResult.Failed($"Không tìm thấy lịch trình có mã '{maLichTrinh}' để xóa.");
            }

            _schedules.Remove(existing);
            DataSyncNotifier.NotifyDataChanged();
            return ServiceResult.Success($"Đã xóa thành công tiết mục lịch trình [{maLichTrinh}].");
        }

        public ServiceResult<bool> CheckConflict(ScheduleItemDto dto, string? excludeMaLichTrinh = null)
        {
            if (dto == null)
            {
                return ServiceResult<bool>.Failed("Dữ liệu cần kiểm tra không được để trống.");
            }

            var evt = _events.FirstOrDefault(e => e.MaSuKien.Equals(dto.MaSuKien?.Trim(), StringComparison.OrdinalIgnoreCase));
            DateTime? eventStart = evt != default ? evt.BatDau : null;
            DateTime? eventEnd = evt != default ? evt.KetThuc : null;

            var existingList = _schedules.Where(s =>
                string.IsNullOrWhiteSpace(excludeMaLichTrinh) ||
                !s.MaLichTrinh.Equals(excludeMaLichTrinh.Trim(), StringComparison.OrdinalIgnoreCase)
            );

            return _validator.ValidateScheduleItem(dto, existingList, eventStart, eventEnd);
        }

        public List<(string MaSuKien, string TenSuKien, DateTime BatDau, DateTime KetThuc)> GetAvailableEvents()
        {
            return _events.ToList();
        }

        public List<(string MaDienGia, string HoTen)> GetAvailableSpeakers()
        {
            return _speakers.Select(kv => (kv.Key, kv.Value)).OrderBy(x => x.Key).ToList();
        }

        private static ScheduleItemDto CloneDto(ScheduleItemDto src)
        {
            return new ScheduleItemDto
            {
                MaLichTrinh = src.MaLichTrinh,
                MaSuKien = src.MaSuKien,
                TenSuKien = src.TenSuKien,
                TenTietMuc = src.TenTietMuc,
                ThoiGianBatDau = src.ThoiGianBatDau,
                ThoiGianKetThuc = src.ThoiGianKetThuc,
                MaDienGia = src.MaDienGia,
                TenDienGia = src.TenDienGia,
                DiaDiemChiTiet = src.DiaDiemChiTiet,
                GhiChu = src.GhiChu
            };
        }
    }
}
