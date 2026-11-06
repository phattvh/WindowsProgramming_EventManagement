using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt dịch vụ nghiệp vụ Quản lý phản hồi và khảo sát (Form F18)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// Đảm bảo các quy tắc:
    /// - BR-16: Giới hạn 1 phản hồi / người tham gia / sự kiện (UNIQUE MaSuKien + MaNguoiThamGia).
    /// - Điểm đánh giá phải từ 1 đến 5 sao (CK_PhanHoi_DiemDanhGia).
    /// - Tự động tổng hợp chỉ số CSAT và tỷ lệ phân bổ các mức sao.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        private static readonly List<FeedbackDto> _feedbacks = new()
        {
            new FeedbackDto
            {
                MaPhanHoi = "PH001",
                MaSuKien = "EVT202604",
                TenSuKien = "Đêm Nhạc Gây Quỹ Sinh Viên - Vòng Tay Bè Bạn 2026",
                MaNguoiThamGia = "NTG001",
                TenNguoiThamGia = "Nguyễn Văn An",
                Email = "an.nguyen@gmail.com",
                SoDienThoai = "0912345678",
                DiemDanhGia = 5,
                NoiDungPhanHoi = "Chương trình ca nhạc rất sôi động và ý nghĩa, các tiết mục chuẩn bị chuyên nghiệp. Rất mong duy trì hàng năm!",
                NgayDanhGia = new DateTime(2026, 10, 16, 9, 0, 0)
            },
            new FeedbackDto
            {
                MaPhanHoi = "PH002",
                MaSuKien = "EVT202604",
                TenSuKien = "Đêm Nhạc Gây Quỹ Sinh Viên - Vòng Tay Bè Bạn 2026",
                MaNguoiThamGia = "NTG002",
                TenNguoiThamGia = "Trần Thị Bích",
                Email = "bich.tran@gmail.com",
                SoDienThoai = "0987654321",
                DiemDanhGia = 4,
                NoiDungPhanHoi = "Khâu soát vé check-in QR rất nhanh, không phải chờ đợi lâu. Âm thanh ánh sáng tuyệt vời!",
                NgayDanhGia = new DateTime(2026, 10, 16, 9, 30, 0)
            },
            new FeedbackDto
            {
                MaPhanHoi = "PH003",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                MaNguoiThamGia = "NTG003",
                TenNguoiThamGia = "Lê Hoàng Nam",
                Email = "nam.le@gmail.com",
                SoDienThoai = "0903123456",
                DiemDanhGia = 5,
                NoiDungPhanHoi = "Nội dung bài lab thực hành Prompt Engineering rất thực chiến, diễn giả hướng dẫn cực kỳ nhiệt tình.",
                NgayDanhGia = new DateTime(2026, 11, 4, 17, 30, 0)
            },
            new FeedbackDto
            {
                MaPhanHoi = "PH004",
                MaSuKien = "EVT202602",
                TenSuKien = "Workshop Thực chiến GenAI & Prompt Engineering 2026",
                MaNguoiThamGia = "NTG004",
                TenNguoiThamGia = "Phạm Thị Thu",
                Email = "thu.pham@fpt.edu.vn",
                SoDienThoai = "0918998877",
                DiemDanhGia = 5,
                NoiDungPhanHoi = "Tài liệu lab đầy đủ, hệ thống mạng và GPU Cloud được cấp hoạt động rất mượt mà.",
                NgayDanhGia = new DateTime(2026, 11, 4, 18, 0, 0)
            },
            new FeedbackDto
            {
                MaPhanHoi = "PH005",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                MaNguoiThamGia = "NTG005",
                TenNguoiThamGia = "Vũ Đình Trọng",
                Email = "trong.vu@viettech.vn",
                SoDienThoai = "0977223344",
                DiemDanhGia = 4,
                NoiDungPhanHoi = "Hội thảo quy mô lớn, các phiên thuyết trình keynote chất lượng. Sảnh đón tiếp hơi đông lúc 8h.",
                NgayDanhGia = new DateTime(2026, 12, 15, 17, 45, 0)
            }
        };

        private static readonly List<KeyValuePair<string, string>> _events = new()
        {
            new("EVT202604", "Đêm Nhạc Gây Quỹ Sinh Viên - Vòng Tay Bè Bạn 2026"),
            new("EVT202602", "Workshop Thực chiến GenAI & Prompt Engineering 2026"),
            new("EVT202601", "Hội nghị Công nghệ Vietnam Tech Summit 2026"),
            new("EVT202603", "Hội nghị Khoa học Quốc tế về Công nghệ Phần mềm & IoT")
        };

        private static readonly List<KeyValuePair<string, string>> _attendees = new()
        {
            new("NTG001", "Nguyễn Văn An (an.nguyen@gmail.com)"),
            new("NTG002", "Trần Thị Bích (bich.tran@gmail.com)"),
            new("NTG003", "Lê Hoàng Nam (nam.le@gmail.com)"),
            new("NTG004", "Phạm Thị Thu (thu.pham@fpt.edu.vn)"),
            new("NTG005", "Vũ Đình Trọng (trong.vu@viettech.vn)"),
            new("NTG006", "Đặng Minh Tâm (tam.dang@techhub.vn)"),
            new("NTG007", "Hoàng Kim Oanh (oanh.hoang@gmail.com)")
        };

        public ServiceResult<List<FeedbackDto>> GetAll(string? keyword = null, string? maSuKien = null, int? ratingFilter = null)
        {
            var query = _feedbacks.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(x =>
                    x.MaPhanHoi.ToLower().Contains(kw) ||
                    x.TenSuKien.ToLower().Contains(kw) ||
                    x.TenNguoiThamGia.ToLower().Contains(kw) ||
                    (x.NoiDungPhanHoi != null && x.NoiDungPhanHoi.ToLower().Contains(kw)) ||
                    (x.Email != null && x.Email.ToLower().Contains(kw)));
            }

            if (!string.IsNullOrWhiteSpace(maSuKien) && maSuKien != "Tất cả")
            {
                query = query.Where(x => string.Equals(x.MaSuKien, maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            if (ratingFilter.HasValue && ratingFilter.Value > 0)
            {
                query = query.Where(x => x.DiemDanhGia == ratingFilter.Value);
            }

            return ServiceResult<List<FeedbackDto>>.Success(query.OrderByDescending(x => x.NgayDanhGia).ToList());
        }

        public ServiceResult<FeedbackDto> GetById(string maPhanHoi)
        {
            if (string.IsNullOrWhiteSpace(maPhanHoi))
                return ServiceResult<FeedbackDto>.Failed("Mã phản hồi không được để trống.");

            var item = _feedbacks.FirstOrDefault(x => string.Equals(x.MaPhanHoi, maPhanHoi.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item == null)
                return ServiceResult<FeedbackDto>.Failed($"Không tìm thấy phản hồi có mã '{maPhanHoi}'.");

            return ServiceResult<FeedbackDto>.Success(item);
        }

        public ServiceResult Add(FeedbackDto dto)
        {
            if (dto == null)
                return ServiceResult.Failed("Dữ liệu phản hồi không hợp lệ.");

            var valResult = ValidateFeedback(dto, isUpdate: false);
            if (!valResult.IsSuccess)
                return valResult;

            // Bổ sung tên sự kiện & tên người tham gia nếu chưa có
            if (string.IsNullOrWhiteSpace(dto.TenSuKien))
            {
                var evt = _events.FirstOrDefault(x => string.Equals(x.Key, dto.MaSuKien, StringComparison.OrdinalIgnoreCase));
                dto.TenSuKien = evt.Value ?? dto.MaSuKien;
            }

            if (string.IsNullOrWhiteSpace(dto.TenNguoiThamGia))
            {
                var att = _attendees.FirstOrDefault(x => string.Equals(x.Key, dto.MaNguoiThamGia, StringComparison.OrdinalIgnoreCase));
                dto.TenNguoiThamGia = att.Value?.Split('(')[0].Trim() ?? dto.MaNguoiThamGia;
            }

            _feedbacks.Add(dto);
            return ServiceResult.Success("Gửi ý kiến đánh giá phản hồi thành công.");
        }

        public ServiceResult Update(FeedbackDto dto)
        {
            if (dto == null)
                return ServiceResult.Failed("Dữ liệu phản hồi không hợp lệ.");

            var existing = _feedbacks.FirstOrDefault(x => string.Equals(x.MaPhanHoi, dto.MaPhanHoi.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return ServiceResult.Failed($"Không tìm thấy phản hồi có mã '{dto.MaPhanHoi}'.");

            var valResult = ValidateFeedback(dto, isUpdate: true);
            if (!valResult.IsSuccess)
                return valResult;

            existing.DiemDanhGia = dto.DiemDanhGia;
            existing.NoiDungPhanHoi = dto.NoiDungPhanHoi?.Trim();
            existing.NgayDanhGia = dto.NgayDanhGia;

            return ServiceResult.Success("Cập nhật đánh giá phản hồi thành công.");
        }

        public ServiceResult Delete(string maPhanHoi)
        {
            if (string.IsNullOrWhiteSpace(maPhanHoi))
                return ServiceResult.Failed("Mã phản hồi không được để trống.");

            var existing = _feedbacks.FirstOrDefault(x => string.Equals(x.MaPhanHoi, maPhanHoi.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return ServiceResult.Failed($"Không tìm thấy phản hồi có mã '{maPhanHoi}'.");

            _feedbacks.Remove(existing);
            return ServiceResult.Success("Xóa phản hồi thành công.");
        }

        public ServiceResult<FeedbackSummaryDto> GetSummary(string? maSuKien = null)
        {
            var query = _feedbacks.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(maSuKien) && maSuKien != "Tất cả")
            {
                query = query.Where(x => string.Equals(x.MaSuKien, maSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            var list = query.ToList();
            var summary = new FeedbackSummaryDto
            {
                TongSoPhanHoi = list.Count,
                DiemTrungBinh = list.Count > 0 ? Math.Round(list.Average(x => x.DiemDanhGia), 1) : 0,
                SoLuong5Sao = list.Count(x => x.DiemDanhGia == 5),
                SoLuong4Sao = list.Count(x => x.DiemDanhGia == 4),
                SoLuong3Sao = list.Count(x => x.DiemDanhGia == 3),
                SoLuong2Sao = list.Count(x => x.DiemDanhGia == 2),
                SoLuong1Sao = list.Count(x => x.DiemDanhGia == 1)
            };

            return ServiceResult<FeedbackSummaryDto>.Success(summary);
        }

        public string SuggestNextCode()
        {
            int maxId = 0;
            foreach (var item in _feedbacks)
            {
                if (item.MaPhanHoi.StartsWith("PH", StringComparison.OrdinalIgnoreCase))
                {
                    string numPart = item.MaPhanHoi.Substring(2);
                    if (int.TryParse(numPart, out int idVal) && idVal > maxId)
                    {
                        maxId = idVal;
                    }
                }
            }
            return $"PH{(maxId + 1):D3}";
        }

        public List<KeyValuePair<string, string>> GetCompletedEvents()
        {
            return _events;
        }

        public List<KeyValuePair<string, string>> GetEligibleAttendees(string? maSuKien = null)
        {
            return _attendees;
        }

        private ServiceResult ValidateFeedback(FeedbackDto dto, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(dto.MaPhanHoi))
                return ServiceResult.Failed("Mã phản hồi không được để trống.");

            if (!isUpdate && _feedbacks.Any(x => string.Equals(x.MaPhanHoi, dto.MaPhanHoi.Trim(), StringComparison.OrdinalIgnoreCase)))
                return ServiceResult.Failed($"Mã phản hồi '{dto.MaPhanHoi}' đã tồn tại.");

            if (string.IsNullOrWhiteSpace(dto.MaSuKien))
                return ServiceResult.Failed("Vui lòng chọn sự kiện được đánh giá.");

            if (string.IsNullOrWhiteSpace(dto.MaNguoiThamGia))
                return ServiceResult.Failed("Vui lòng chọn người tham gia gửi phản hồi.");

            // Ràng buộc BR-16: 1 phản hồi / người / sự kiện
            if (!isUpdate)
            {
                bool alreadySubmitted = _feedbacks.Any(x =>
                    string.Equals(x.MaSuKien, dto.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(x.MaNguoiThamGia, dto.MaNguoiThamGia.Trim(), StringComparison.OrdinalIgnoreCase));

                if (alreadySubmitted)
                {
                    return ServiceResult.Failed("Người tham gia này đã gửi phản hồi cho sự kiện rồi. Mỗi người chỉ được gửi 1 lần theo quy định (BR-16).");
                }
            }

            if (dto.DiemDanhGia < 1 || dto.DiemDanhGia > 5)
                return ServiceResult.Failed("Điểm đánh giá phải từ 1 đến 5 sao theo thang đo CSAT (1-5★).");

            return ServiceResult.Success();
        }
    }
}
