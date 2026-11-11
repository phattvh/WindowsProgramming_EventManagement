using System.Linq;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class SponsorServiceTests
    {
        private ISponsorService _sponsorService = null!;

        [TestInitialize]
        public void Setup()
        {
            _sponsorService = new SponsorService();
        }

        [TestMethod]
        public void GetAll_ReturnsSeededSponsors()
        {
            var result = _sponsorService.GetAll();

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 5);
        }

        [TestMethod]
        public void GetAll_FilterByPackage_ReturnsCorrect()
        {
            var result = _sponsorService.GetAll(packageFilter: "Kim Cương");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.All(x => x.GoiTaiTro == "Kim Cương"));
        }

        [TestMethod]
        public void GetAll_FilterByStatus_ReturnsCorrect()
        {
            var result = _sponsorService.GetAll(statusFilter: "Đang hợp tác");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.All(x => x.TrangThai == "Đang hợp tác"));
        }

        [TestMethod]
        public void GetAll_SearchKeyword_ReturnsMatched()
        {
            var result = _sponsorService.GetAll(keyword: "Viettel");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 1);
            Assert.IsTrue(result.Data.Any(x => x.TenToChuc.Contains("Viettel")));
        }

        [TestMethod]
        public void GetById_Existing_ReturnsSuccess()
        {
            var result = _sponsorService.GetById("NTT001");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("NTT001", result.Data.MaNhaTaiTro);
        }

        [TestMethod]
        public void GetById_NonExisting_ReturnsFailed()
        {
            var result = _sponsorService.GetById("NTT9999");

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void Add_ValidSponsor_ReturnsSuccess()
        {
            string newCode = "NTT_TEST_" + System.Guid.NewGuid().ToString("N").Substring(0, 6);
            var dto = new SponsorDto
            {
                MaNhaTaiTro = newCode,
                TenToChuc = "Tập đoàn Công nghệ FPT Software",
                NguoiDaiDien = "Trương Gia Bình",
                Email = "fpt@fpt.com.vn",
                SoDienThoai = "02473007300",
                GoiTaiTro = "Vàng",
                TongGiaTriTaiTro = 50000000m,
                HinhThucTaiTro = "Tiền mặt",
                TrangThai = "Đang hợp tác"
            };

            var result = _sponsorService.Add(dto);

            Assert.IsTrue(result.IsSuccess);
            var check = _sponsorService.GetById(newCode);
            Assert.IsTrue(check.IsSuccess);
        }

        [TestMethod]
        public void Add_DuplicatePrimaryKey_ReturnsFailed()
        {
            var dto = new SponsorDto
            {
                MaNhaTaiTro = "NTT001", // Đã có
                TenToChuc = "Công ty TNHH Trùng Mã"
            };

            var result = _sponsorService.Add(dto);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "đã tồn tại");
        }

        [TestMethod]
        public void Add_InvalidEmail_ReturnsFailed()
        {
            string newCode = "NTT_ERR_" + System.Guid.NewGuid().ToString("N").Substring(0, 6);
            var dto = new SponsorDto
            {
                MaNhaTaiTro = newCode,
                TenToChuc = "Công ty Email Lỗi",
                Email = "email_khong_hop_le"
            };

            var result = _sponsorService.Add(dto);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "Email không hợp lệ");
        }

        [TestMethod]
        public void Delete_WithSponsorshipHistory_PerformsSoftDelete_BR20()
        {
            // NTT001 có SoSuKienTaiTro = 2
            var item = _sponsorService.GetById("NTT001").Data;
            Assert.IsNotNull(item);
            Assert.IsTrue(item.SoSuKienTaiTro > 0);

            var deleteResult = _sponsorService.Delete("NTT001");

            Assert.IsTrue(deleteResult.IsSuccess);
            StringAssert.Contains(deleteResult.Message, "Ngừng hợp tác");

            // Kiểm tra trạng thái đã chuyển thành Ngừng hợp tác
            var check = _sponsorService.GetById("NTT001").Data;
            Assert.IsNotNull(check);
            Assert.AreEqual("Ngừng hợp tác", check.TrangThai);
        }

        [TestMethod]
        public void SuggestNextCode_GeneratesNTTFormat()
        {
            string nextCode = _sponsorService.SuggestNextCode();

            Assert.IsTrue(nextCode.StartsWith("NTT"));
            Assert.IsTrue(nextCode.Length >= 6);
        }
    }
}
