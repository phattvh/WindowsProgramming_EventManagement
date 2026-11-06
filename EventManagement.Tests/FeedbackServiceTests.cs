using System;
using System.Linq;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    [TestClass]
    public class FeedbackServiceTests
    {
        private IFeedbackService _feedbackService = null!;

        [TestInitialize]
        public void Setup()
        {
            _feedbackService = new FeedbackService();
        }

        [TestMethod]
        public void GetAll_ReturnsSeededFeedbacks()
        {
            var result = _feedbackService.GetAll();

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 5);
        }

        [TestMethod]
        public void GetAll_FilterByEvent_ReturnsCorrect()
        {
            var result = _feedbackService.GetAll(maSuKien: "EVT202604");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.All(x => x.MaSuKien == "EVT202604"));
        }

        [TestMethod]
        public void GetAll_FilterByRating_ReturnsCorrect()
        {
            var result = _feedbackService.GetAll(ratingFilter: 5);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.All(x => x.DiemDanhGia == 5));
        }

        [TestMethod]
        public void GetById_Existing_ReturnsSuccess()
        {
            var result = _feedbackService.GetById("PH001");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("PH001", result.Data.MaPhanHoi);
            Assert.AreEqual(5, result.Data.DiemDanhGia);
        }

        [TestMethod]
        public void Add_ValidFeedback_ReturnsSuccess()
        {
            string newCode = "PH_TEST_" + Guid.NewGuid().ToString("N").Substring(0, 6);
            var dto = new FeedbackDto
            {
                MaPhanHoi = newCode,
                MaSuKien = "EVT202603",
                MaNguoiThamGia = "NTG006",
                DiemDanhGia = 4,
                NoiDungPhanHoi = "Hội thảo rất bổ ích, công tác chuẩn bị chu đáo.",
                NgayDanhGia = DateTime.Now
            };

            var result = _feedbackService.Add(dto);

            Assert.IsTrue(result.IsSuccess);
            var check = _feedbackService.GetById(newCode);
            Assert.IsTrue(check.IsSuccess);
            Assert.AreEqual(4, check.Data!.DiemDanhGia);
        }

        [TestMethod]
        public void Add_DuplicateAttendeeForSameEvent_ReturnsFailed_BR16()
        {
            // PH001 đã có MaSuKien = EVT202604, MaNguoiThamGia = NTG001
            string newCode = "PH_DUP_" + Guid.NewGuid().ToString("N").Substring(0, 6);
            var dto = new FeedbackDto
            {
                MaPhanHoi = newCode,
                MaSuKien = "EVT202604",
                MaNguoiThamGia = "NTG001",
                DiemDanhGia = 5,
                NoiDungPhanHoi = "Thử gửi lại phản hồi lần 2"
            };

            var result = _feedbackService.Add(dto);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "đã gửi phản hồi");
            StringAssert.Contains(result.Message, "BR-16");
        }

        [TestMethod]
        public void Add_RatingOutOfRange_ReturnsFailed()
        {
            string newCode = "PH_RANGE_" + Guid.NewGuid().ToString("N").Substring(0, 6);
            var dto = new FeedbackDto
            {
                MaPhanHoi = newCode,
                MaSuKien = "EVT202603",
                MaNguoiThamGia = "NTG007",
                DiemDanhGia = 6 // Lỗi: ngoài khoảng 1-5
            };

            var result = _feedbackService.Add(dto);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "1 đến 5 sao");
        }

        [TestMethod]
        public void GetSummary_CalculatesCorrectAverageAndCsat()
        {
            var result = _feedbackService.GetSummary();

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);

            var s = result.Data;
            Assert.IsTrue(s.TongSoPhanHoi >= 5);
            Assert.IsTrue(s.DiemTrungBinh >= 1.0 && s.DiemTrungBinh <= 5.0);
            Assert.IsTrue(s.TyLeHaiLong >= 0 && s.TyLeHaiLong <= 100);
            Assert.AreEqual(s.TongSoPhanHoi, s.SoLuong5Sao + s.SoLuong4Sao + s.SoLuong3Sao + s.SoLuong2Sao + s.SoLuong1Sao);
        }

        [TestMethod]
        public void SuggestNextCode_GeneratesPHFormat()
        {
            string nextCode = _feedbackService.SuggestNextCode();

            Assert.IsTrue(nextCode.StartsWith("PH"));
            Assert.IsTrue(nextCode.Length >= 5);
        }
    }
}
