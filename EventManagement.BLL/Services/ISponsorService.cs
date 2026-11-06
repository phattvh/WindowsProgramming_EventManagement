using System.Collections.Generic;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện dịch vụ nghiệp vụ Quản lý nhà tài trợ (Form F16)
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// </summary>
    public interface ISponsorService
    {
        ServiceResult<List<SponsorDto>> GetAll(string? keyword = null, string? packageFilter = null, string? statusFilter = null);
        ServiceResult<SponsorDto> GetById(string maNhaTaiTro);
        ServiceResult Add(SponsorDto dto);
        ServiceResult Update(SponsorDto dto);
        ServiceResult Delete(string maNhaTaiTro);
        string SuggestNextCode();
        List<string> GetPackageList();
        List<string> GetStatusList();
    }
}
