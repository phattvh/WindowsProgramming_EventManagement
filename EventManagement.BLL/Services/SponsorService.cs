using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Cài đặt dịch vụ nghiệp vụ Quản lý nhà tài trợ (Form F16)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// Đảm bảo các quy tắc:
    /// - BR-20: Soft delete đối với Nhà tài trợ đã có dữ liệu tài trợ lịch sử.
    /// - Validation định dạng Email, Số điện thoại và tính duy nhất của Mã nhà tài trợ.
    /// </summary>
    public class SponsorService : ISponsorService
    {
        private static readonly List<SponsorDto> _sponsors = new()
        {
            new SponsorDto
            {
                MaNhaTaiTro = "NTT001",
                TenToChuc = "Tập đoàn Viễn thông & Công nghệ VietTech",
                NguoiDaiDien = "Ông Phạm Nhật Long",
                Email = "contact@viettech.vn",
                SoDienThoai = "0283899999",
                DiaChi = "Tòa nhà VietTech Tower, Quận 1, TP.HCM",
                GhiChu = "Tài trợ độc quyền tiệc Gala Dinner",
                GoiTaiTro = "Kim Cương",
                TongGiaTriTaiTro = 150000000m,
                HinhThucTaiTro = "Tiền mặt",
                TrangThai = "Đang hợp tác",
                SoSuKienTaiTro = 2
            },
            new SponsorDto
            {
                MaNhaTaiTro = "NTT002",
                TenToChuc = "Ngân hàng Số FinPay",
                NguoiDaiDien = "Bà Võ Kim Oanh",
                Email = "partnership@finpay.vn",
                SoDienThoai = "0283788888",
                DiaChi = "Khu Đô thị Sala, TP. Thủ Đức, TP.HCM",
                GhiChu = "Tài trợ toàn bộ hệ thống quà tặng công nghệ",
                GoiTaiTro = "Vàng",
                TongGiaTriTaiTro = 60000000m,
                HinhThucTaiTro = "Tiền mặt",
                TrangThai = "Đang hợp tác",
                SoSuKienTaiTro = 1
            },
            new SponsorDto
            {
                MaNhaTaiTro = "NTT003",
                TenToChuc = "Tập đoàn Công nghiệp - Viễn thông Quân đội (Viettel)",
                NguoiDaiDien = "Ông Tào Đức Thắng",
                Email = "support@viettel.com.vn",
                SoDienThoai = "18008098",
                DiaChi = "Số 1 Trần Hữu Dực, Nam Từ Liêm, Hà Nội",
                GhiChu = "Tài trợ đường truyền Internet cáp quang tốc độ cao 1Gbps",
                GoiTaiTro = "Bạc",
                TongGiaTriTaiTro = 30000000m,
                HinhThucTaiTro = "Dịch vụ",
                TrangThai = "Đang hợp tác",
                SoSuKienTaiTro = 1
            },
            new SponsorDto
            {
                MaNhaTaiTro = "NTT004",
                TenToChuc = "Công ty Cổ phần Dịch vụ Di động Trực tuyến (Ví MoMo)",
                NguoiDaiDien = "Ông Nguyễn Bá Diệp",
                Email = "hotro@momo.vn",
                SoDienThoai = "1900545441",
                DiaChi = "Tòa nhà Phú Mỹ Hưng, Quận 7, TP.HCM",
                GhiChu = "Tài trợ cổng thanh toán MoMo miễn phí giao dịch vé",
                GoiTaiTro = "Đồng",
                TongGiaTriTaiTro = 15000000m,
                HinhThucTaiTro = "Hiện vật",
                TrangThai = "Đang hợp tác",
                SoSuKienTaiTro = 1
            },
            new SponsorDto
            {
                MaNhaTaiTro = "NTT005",
                TenToChuc = "Tập đoàn Vingroup (Quỹ Đổi mới sáng tạo VINIF)",
                NguoiDaiDien = "Bà Phan Thu Hiền",
                Email = "vinif@vinbigdata.org",
                SoDienThoai = "02439749999",
                DiaChi = "Số 7 Đường Bằng Lăng 1, KĐT Vinhomes Riverside, Long Biên, Hà Nội",
                GhiChu = "Cấp 300 tài khoản GPU Cloud cho học viên xuất sắc",
                GoiTaiTro = "Kim Cương",
                TongGiaTriTaiTro = 80000000m,
                HinhThucTaiTro = "Hiện vật",
                TrangThai = "Đang hợp tác",
                SoSuKienTaiTro = 1
            }
        };

        public ServiceResult<List<SponsorDto>> GetAll(string? keyword = null, string? packageFilter = null, string? statusFilter = null)
        {
            var query = _sponsors.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(x =>
                    x.MaNhaTaiTro.ToLower().Contains(kw) ||
                    x.TenToChuc.ToLower().Contains(kw) ||
                    (x.NguoiDaiDien != null && x.NguoiDaiDien.ToLower().Contains(kw)) ||
                    (x.Email != null && x.Email.ToLower().Contains(kw)) ||
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(kw)) ||
                    (x.DiaChi != null && x.DiaChi.ToLower().Contains(kw)));
            }

            if (!string.IsNullOrWhiteSpace(packageFilter) && packageFilter != "Tất cả")
            {
                query = query.Where(x => string.Equals(x.GoiTaiTro, packageFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "Tất cả")
            {
                query = query.Where(x => string.Equals(x.TrangThai, statusFilter, StringComparison.OrdinalIgnoreCase));
            }

            return ServiceResult<List<SponsorDto>>.Success(query.OrderBy(x => x.MaNhaTaiTro).ToList());
        }

        public ServiceResult<SponsorDto> GetById(string maNhaTaiTro)
        {
            if (string.IsNullOrWhiteSpace(maNhaTaiTro))
            {
                return ServiceResult<SponsorDto>.Failed("Mã nhà tài trợ không được để trống.");
            }

            var item = _sponsors.FirstOrDefault(x => string.Equals(x.MaNhaTaiTro, maNhaTaiTro.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                return ServiceResult<SponsorDto>.Failed($"Không tìm thấy nhà tài trợ có mã '{maNhaTaiTro}'.");
            }

            return ServiceResult<SponsorDto>.Success(item);
        }

        public ServiceResult Add(SponsorDto dto)
        {
            if (dto == null)
                return ServiceResult.Failed("Dữ liệu nhà tài trợ không hợp lệ.");

            var valResult = ValidateSponsor(dto, isUpdate: false);
            if (!valResult.IsSuccess)
                return valResult;

            _sponsors.Add(dto);
            return ServiceResult.Success("Thêm nhà tài trợ mới thành công.");
        }

        public ServiceResult Update(SponsorDto dto)
        {
            if (dto == null)
                return ServiceResult.Failed("Dữ liệu nhà tài trợ không hợp lệ.");

            var existing = _sponsors.FirstOrDefault(x => string.Equals(x.MaNhaTaiTro, dto.MaNhaTaiTro.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return ServiceResult.Failed($"Không tìm thấy nhà tài trợ có mã '{dto.MaNhaTaiTro}'.");

            var valResult = ValidateSponsor(dto, isUpdate: true);
            if (!valResult.IsSuccess)
                return valResult;

            existing.TenToChuc = dto.TenToChuc.Trim();
            existing.NguoiDaiDien = dto.NguoiDaiDien?.Trim();
            existing.Email = dto.Email?.Trim();
            existing.SoDienThoai = dto.SoDienThoai?.Trim();
            existing.DiaChi = dto.DiaChi?.Trim();
            existing.GhiChu = dto.GhiChu?.Trim();
            existing.GoiTaiTro = dto.GoiTaiTro;
            existing.TongGiaTriTaiTro = dto.TongGiaTriTaiTro;
            existing.HinhThucTaiTro = dto.HinhThucTaiTro;
            existing.TrangThai = dto.TrangThai;
            existing.MaSuKien = dto.MaSuKien;

            return ServiceResult.Success("Cập nhật thông tin nhà tài trợ thành công.");
        }

        public ServiceResult Delete(string maNhaTaiTro)
        {
            if (string.IsNullOrWhiteSpace(maNhaTaiTro))
                return ServiceResult.Failed("Mã nhà tài trợ không được để trống.");

            var existing = _sponsors.FirstOrDefault(x => string.Equals(x.MaNhaTaiTro, maNhaTaiTro.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing == null)
                return ServiceResult.Failed($"Không tìm thấy nhà tài trợ có mã '{maNhaTaiTro}'.");

            // Ràng buộc BR-20: Soft delete nếu đã có sự kiện tài trợ
            if (existing.SoSuKienTaiTro > 0)
            {
                existing.TrangThai = "Ngừng hợp tác";
                return ServiceResult.Success($"Nhà tài trợ '{existing.TenToChuc}' đã có lịch sử tham gia {existing.SoSuKienTaiTro} sự kiện nên đã được chuyển sang trạng thái 'Ngừng hợp tác' (BR-20 Soft Delete).");
            }

            _sponsors.Remove(existing);
            return ServiceResult.Success("Xóa nhà tài trợ thành công.");
        }

        public string SuggestNextCode()
        {
            int maxId = 0;
            foreach (var item in _sponsors)
            {
                if (item.MaNhaTaiTro.StartsWith("NTT", StringComparison.OrdinalIgnoreCase))
                {
                    string numPart = item.MaNhaTaiTro.Substring(3);
                    if (int.TryParse(numPart, out int idVal) && idVal > maxId)
                    {
                        maxId = idVal;
                    }
                }
            }
            return $"NTT{(maxId + 1):D3}";
        }

        public List<string> GetPackageList()
        {
            return new List<string> { "Kim Cương", "Vàng", "Bạc", "Đồng", "Đồng hành" };
        }

        public List<string> GetStatusList()
        {
            return new List<string> { "Đang hợp tác", "Tiềm năng", "Ngừng hợp tác" };
        }

        private ServiceResult ValidateSponsor(SponsorDto dto, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(dto.MaNhaTaiTro))
                return ServiceResult.Failed("Mã nhà tài trợ không được để trống.");

            if (!isUpdate && _sponsors.Any(x => string.Equals(x.MaNhaTaiTro, dto.MaNhaTaiTro.Trim(), StringComparison.OrdinalIgnoreCase)))
                return ServiceResult.Failed($"Mã nhà tài trợ '{dto.MaNhaTaiTro}' đã tồn tại trên hệ thống.");

            if (string.IsNullOrWhiteSpace(dto.TenToChuc))
                return ServiceResult.Failed("Tên tổ chức/công ty tài trợ không được để trống.");

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(dto.Email.Trim(), emailPattern))
                    return ServiceResult.Failed("Định dạng Email không hợp lệ.");
            }

            if (!string.IsNullOrWhiteSpace(dto.SoDienThoai))
            {
                string phonePattern = @"^[0-9]{8,15}$";
                if (!Regex.IsMatch(dto.SoDienThoai.Trim(), phonePattern))
                    return ServiceResult.Failed("Số điện thoại phải từ 8 đến 15 chữ số.");
            }

            if (dto.TongGiaTriTaiTro < 0)
                return ServiceResult.Failed("Giá trị tài trợ phải >= 0!");

            return ServiceResult.Success();
        }
    }
}
