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
        private static readonly List<FeedbackDto> _feedbacks = new();
        private static readonly List<KeyValuePair<string, string>> _events = new();
        private static readonly List<KeyValuePair<string, string>> _attendees = new();

        private void EnsureLoadedFromDb()
        {
            if (_feedbacks.Count == 0 && _events.Count == 0 && _attendees.Count == 0)
            {
                try
                {
                    using var db = new EventManagement.DAL.AppDbContext();

                    var evts = db.SuKiens
                        .OrderBy(s => s.NgayBatDau)
                        .Select(s => new KeyValuePair<string, string>(s.MaSuKien, s.TenSuKien))
                        .ToList();
                    if (evts.Any())
                    {
                        _events.AddRange(evts);
                    }

                    var atts = db.NguoiThamGias
                        .OrderBy(n => n.MaNguoiThamGia)
                        .Select(n => new KeyValuePair<string, string>(n.MaNguoiThamGia, $"{n.HoTen} ({n.Email})"))
                        .ToList();
                    if (atts.Any())
                    {
                        _attendees.AddRange(atts);
                    }

                    var list = (from p in db.PhanHois
                                join s in db.SuKiens on p.MaSuKien equals s.MaSuKien into sj
                                from s in sj.DefaultIfEmpty()
                                join n in db.NguoiThamGias on p.MaNguoiThamGia equals n.MaNguoiThamGia into nj
                                from n in nj.DefaultIfEmpty()
                                select new FeedbackDto
                                {
                                    MaPhanHoi = p.MaPhanHoi,
                                    MaSuKien = p.MaSuKien,
                                    TenSuKien = s != null ? s.TenSuKien : p.MaSuKien,
                                    MaNguoiThamGia = p.MaNguoiThamGia,
                                    TenNguoiThamGia = n != null ? n.HoTen : p.MaNguoiThamGia,
                                    Email = n != null ? (n.Email ?? string.Empty) : string.Empty,
                                    SoDienThoai = n != null ? (n.SoDienThoai ?? string.Empty) : string.Empty,
                                    DiemDanhGia = p.DiemDanhGia,
                                    NoiDungPhanHoi = p.NoiDungPhanHoi,
                                    NgayDanhGia = p.NgayDanhGia
                                }).ToList();

                    if (list.Any())
                    {
                        _feedbacks.AddRange(list);
                    }
                }
                catch { }
            }
        }

        public ServiceResult<List<FeedbackDto>> GetAll(string? keyword = null, string? maSuKien = null, int? ratingFilter = null)
        {
            EnsureLoadedFromDb();
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
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maPhanHoi))
                return ServiceResult<FeedbackDto>.Failed("Mã phản hồi không được để trống.");

            var item = _feedbacks.FirstOrDefault(x => string.Equals(x.MaPhanHoi, maPhanHoi.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item == null)
                return ServiceResult<FeedbackDto>.Failed($"Không tìm thấy phản hồi có mã '{maPhanHoi}'.");

            return ServiceResult<FeedbackDto>.Success(item);
        }

        public ServiceResult Add(FeedbackDto dto)
        {
            EnsureLoadedFromDb();
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
            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var entity = new EventManagement.Core.Entities.PhanHoi
                {
                    MaPhanHoi = dto.MaPhanHoi,
                    MaSuKien = dto.MaSuKien,
                    MaNguoiThamGia = dto.MaNguoiThamGia,
                    DiemDanhGia = dto.DiemDanhGia,
                    NoiDungPhanHoi = dto.NoiDungPhanHoi,
                    NgayDanhGia = dto.NgayDanhGia
                };
                db.PhanHois.Add(entity);
                db.SaveChanges();
            }
            catch { }

            return ServiceResult.Success("Gửi ý kiến đánh giá phản hồi thành công.");
        }

        public ServiceResult Update(FeedbackDto dto)
        {
            EnsureLoadedFromDb();
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

            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var ph = db.PhanHois.FirstOrDefault(x => x.MaPhanHoi == existing.MaPhanHoi);
                if (ph != null)
                {
                    ph.DiemDanhGia = existing.DiemDanhGia;
                    ph.NoiDungPhanHoi = existing.NoiDungPhanHoi;
                    ph.NgayDanhGia = existing.NgayDanhGia;
                    db.SaveChanges();
                }
            }
            catch { }

            return ServiceResult.Success("Cập nhật đánh giá phản hồi thành công.");
        }

        public ServiceResult Delete(string maPhanHoi)
        {
            EnsureLoadedFromDb();
            if (string.IsNullOrWhiteSpace(maPhanHoi))
                return ServiceResult.Failed("Mã phản hồi không được để trống.");

            var existing = _feedbacks.FirstOrDefault(x => string.Equals(x.MaPhanHoi, maPhanHoi.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return ServiceResult.Failed($"Không tìm thấy phản hồi có mã '{maPhanHoi}'.");

            _feedbacks.Remove(existing);
            try
            {
                using var db = new EventManagement.DAL.AppDbContext();
                var ph = db.PhanHois.FirstOrDefault(x => x.MaPhanHoi == existing.MaPhanHoi);
                if (ph != null)
                {
                    db.PhanHois.Remove(ph);
                    db.SaveChanges();
                }
            }
            catch { }

            return ServiceResult.Success("Xóa phản hồi thành công.");
        }

        public ServiceResult<FeedbackSummaryDto> GetSummary(string? maSuKien = null)
        {
            EnsureLoadedFromDb();
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
            EnsureLoadedFromDb();
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
            EnsureLoadedFromDb();
            return _events;
        }

        public List<KeyValuePair<string, string>> GetEligibleAttendees(string? maSuKien = null)
        {
            EnsureLoadedFromDb();
            if (!string.IsNullOrWhiteSpace(maSuKien))
            {
                try
                {
                    using var db = new EventManagement.DAL.AppDbContext();
                    var attendees = (from c in db.ChiTietVes
                                     join dk in db.DangKys on c.MaDangKy equals dk.MaDangKy
                                     where dk.MaSuKien == maSuKien
                                     select new KeyValuePair<string, string>(dk.MaNguoiThamGia, $"{c.TenNguoiThamGia} ({c.Email})"))
                                     .Distinct()
                                     .ToList();
                    if (attendees.Any()) return attendees;
                }
                catch { }
            }
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
