using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;

namespace EventManagement.Tests
{
    /// <summary>
    /// Bộ kiểm thử tự động cho Nghiệp vụ Quản lý Lịch trình (Task T2-10 & T2-11)
    /// Đảm bảo các quy tắc:
    /// - BR-05: Khung giờ tiết mục con phải nằm hoàn toàn trong thời gian sự kiện.
    /// - BR-06: Chống trùng diễn giả & chống trùng phòng chức năng trong cùng thời điểm.
    /// - CRUD Lịch trình F08.
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// </summary>
    [TestClass]
    public class ScheduleServiceTests
    {
        private IScheduleService _scheduleService = null!;
        private ScheduleValidationService _validator = null!;

        [TestInitialize]
        public void Setup()
        {
            _validator = new ScheduleValidationService();
            _scheduleService = new ScheduleService(_validator);
        }

        // ==============================================================================
        // 1. KIỂM THỬ TRUY VẤN VÀ BỘ LỌC (GetAll & GetById)
        // ==============================================================================

        [TestMethod]
        public void GetAll_InitialSeedData_ReturnsAtLeast8Items()
        {
            var result = _scheduleService.GetAll();

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 8, $"Số lượng thực tế: {result.Data.Count}");
        }

        [TestMethod]
        public void GetAll_FilterByEvent_ReturnsCorrectItems()
        {
            var resEvent1 = _scheduleService.GetAll(maSuKien: "EVT202601");
            Assert.IsTrue(resEvent1.IsSuccess);
            Assert.IsNotNull(resEvent1.Data);
            Assert.IsTrue(resEvent1.Data.All(x => x.MaSuKien == "EVT202601"));
            Assert.AreEqual(5, resEvent1.Data.Count);

            var resEvent2 = _scheduleService.GetAll(maSuKien: "EVT202602");
            Assert.IsTrue(resEvent2.IsSuccess);
            Assert.IsNotNull(resEvent2.Data);
            Assert.IsTrue(resEvent2.Data.All(x => x.MaSuKien == "EVT202602"));
            Assert.AreEqual(3, resEvent2.Data.Count);
        }

        [TestMethod]
        public void GetAll_FilterBySpeaker_ReturnsCorrectItems()
        {
            var resSpeaker = _scheduleService.GetAll(maDienGia: "DG006");

            Assert.IsTrue(resSpeaker.IsSuccess);
            Assert.IsNotNull(resSpeaker.Data);
            Assert.IsTrue(resSpeaker.Data.All(x => x.MaDienGia == "DG006"));
            Assert.IsTrue(resSpeaker.Data.Count >= 3);
        }

        [TestMethod]
        public void GetAll_SearchKeyword_ReturnsMatchedItems()
        {
            var resKeyword = _scheduleService.GetAll(keyword: "Keynote");

            Assert.IsTrue(resKeyword.IsSuccess);
            Assert.IsNotNull(resKeyword.Data);
            Assert.IsTrue(resKeyword.Data.Count >= 1);
            Assert.IsTrue(resKeyword.Data.Any(x => x.TenTietMuc.Contains("Keynote")));
        }

        [TestMethod]
        public void GetById_ExistingItem_ReturnsSuccessWithData()
        {
            var result = _scheduleService.GetById("LT001");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("LT001", result.Data.MaLichTrinh);
            Assert.AreEqual("EVT202601", result.Data.MaSuKien);
            Assert.AreEqual("DG001", result.Data.MaDienGia);
        }

        [TestMethod]
        public void GetById_NonExistingItem_ReturnsFailed()
        {
            var result = _scheduleService.GetById("LT999");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("Không tìm thấy"));
        }

        // ==============================================================================
        // 2. KIỂM THỬ THÊM MỚI, CẬP NHẬT & XÓA (CRUD)
        // ==============================================================================

        [TestMethod]
        public void Add_ValidScheduleItem_ReturnsSuccess()
        {
            var newItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT099",
                MaSuKien = "EVT202601",
                TenTietMuc = "Mini Game công nghệ & Quay số may mắn",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 12, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 12, 30, 0),
                MaDienGia = null,
                DiaDiemChiTiet = "Sảnh Expo Lounge",
                GhiChu = "Tiết mục giải trí trưa"
            };

            var result = _scheduleService.Add(newItem);

            Assert.IsTrue(result.IsSuccess, result.Message);
            var getCheck = _scheduleService.GetById("LT099");
            Assert.IsTrue(getCheck.IsSuccess);
            Assert.AreEqual("Mini Game công nghệ & Quay số may mắn", getCheck.Data?.TenTietMuc);
        }

        [TestMethod]
        public void Add_DuplicatePrimaryKey_ReturnsFailed()
        {
            var duplicateItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT001", // Trùng mã đã có
                MaSuKien = "EVT202601",
                TenTietMuc = "Trùng lặp mã",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 12, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 12, 30, 0)
            };

            var result = _scheduleService.Add(duplicateItem);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("đã tồn tại"));
        }

        [TestMethod]
        public void Add_EndTimeBeforeStartTime_ReturnsFailed()
        {
            var invalidTimeItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT100",
                MaSuKien = "EVT202601",
                TenTietMuc = "Lỗi giờ bắt đầu lớn hơn kết thúc",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 14, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 13, 0, 0) // Lỗi
            };

            var result = _scheduleService.Add(invalidTimeItem);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("lớn hơn thời gian bắt đầu"));
        }

        [TestMethod]
        public void Update_ExistingItem_ReturnsSuccess()
        {
            var updateDto = new ScheduleItemDto
            {
                MaLichTrinh = "LT002",
                MaSuKien = "EVT202601",
                TenTietMuc = "Tọa đàm: Bảo mật thông tin & Kiến trúc Cloud-Native (Cập nhật)",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 10, 15, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 11, 45, 0),
                MaDienGia = "DG002",
                DiaDiemChiTiet = "Phòng Hội thảo 2",
                GhiChu = "Có trình diễn demo trực tiếp"
            };

            var result = _scheduleService.Update(updateDto);

            Assert.IsTrue(result.IsSuccess, result.Message);
            var updated = _scheduleService.GetById("LT002");
            Assert.AreEqual("Tọa đàm: Bảo mật thông tin & Kiến trúc Cloud-Native (Cập nhật)", updated.Data?.TenTietMuc);
            Assert.AreEqual("Có trình diễn demo trực tiếp", updated.Data?.GhiChu);
        }

        [TestMethod]
        public void Delete_ExistingItem_ReturnsSuccess()
        {
            // Thêm một mục tạm để xóa
            var temp = new ScheduleItemDto
            {
                MaLichTrinh = "LT900",
                MaSuKien = "EVT202601",
                TenTietMuc = "Tiết mục xóa thử nghiệm",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 12, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 12, 30, 0)
            };
            _scheduleService.Add(temp);

            var result = _scheduleService.Delete("LT900");

            Assert.IsTrue(result.IsSuccess);
            var check = _scheduleService.GetById("LT900");
            Assert.IsFalse(check.IsSuccess);
        }

        // ==============================================================================
        // 3. THUẬT TOÁN BR-05: KHUNG GIỜ TIẾT MỤC CON NẰM TRỌN TRONG SỰ KIỆN CHÍNH
        // ==============================================================================

        [TestMethod]
        public void BR05_ItemStartsBeforeEvent_ReturnsFailed()
        {
            // Sự kiện EVT202601 bắt đầu lúc 08:00:00 ngày 15/12/2026
            // Tiết mục bắt đầu lúc 07:30:00 -> Vi phạm BR-05
            var earlyItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT101",
                MaSuKien = "EVT202601",
                TenTietMuc = "Đón tiếp sớm ngoài giờ khai mạc",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 7, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 8, 30, 0),
                DiaDiemChiTiet = "Sảnh chính"
            };

            var result = _scheduleService.Add(earlyItem);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("BR-05") || result.Message.Contains("trước giờ khai mạc"));
        }

        [TestMethod]
        public void BR05_ItemEndsAfterEvent_ReturnsFailed()
        {
            // Sự kiện EVT202601 kết thúc lúc 17:30:00 ngày 15/12/2026
            // Tiết mục kết thúc lúc 18:00:00 -> Vi phạm BR-05
            var lateItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT102",
                MaSuKien = "EVT202601",
                TenTietMuc = "Tiệc sau bế mạc vượt khung giờ",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 17, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 18, 0, 0),
                DiaDiemChiTiet = "Hội trường chính"
            };

            var result = _scheduleService.Add(lateItem);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("BR-05") || result.Message.Contains("vượt quá giờ bế mạc"));
        }

        // ==============================================================================
        // 4. THUẬT TOÁN BR-06: CHỐNG TRÙNG DIỄN GIẢ & CHỐNG TRÙNG PHÒNG CHỨC NĂNG
        // ==============================================================================

        [TestMethod]
        public void BR06_SpeakerConflict_OverlappingWithExistingSchedule_ReturnsFailed()
        {
            // LT001 có DG001 từ 08:30 đến 10:00 ngày 15/12/2026
            // Tiết mục mới gán DG001 từ 09:00 đến 10:30 -> Xung đột giao thoa BR-06
            var conflictItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT103",
                MaSuKien = "EVT202601",
                TenTietMuc = "Thảo luận song song cùng DG001",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 9, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 10, 30, 0),
                MaDienGia = "DG001",
                DiaDiemChiTiet = "Phòng Họp VIP 1"
            };

            var result = _scheduleService.Add(conflictItem);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("Xung đột lịch diễn giả (BR-06)"));
        }

        [TestMethod]
        public void BR06_RoomConflict_OverlappingWithExistingSchedule_ReturnsFailed()
        {
            // LT001 tại 'Hội trường chính Grand Ballroom' từ 08:30 đến 10:00 ngày 15/12/2026
            // Tiết mục mới xếp cùng hội trường từ 09:30 đến 11:00 -> Xung đột phòng BR-06
            var conflictRoomItem = new ScheduleItemDto
            {
                MaLichTrinh = "LT104",
                MaSuKien = "EVT202601",
                TenTietMuc = "Biểu diễn văn nghệ khai mạc",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 9, 30, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 11, 0, 0),
                MaDienGia = null,
                DiaDiemChiTiet = "Hội trường chính Grand Ballroom"
            };

            var result = _scheduleService.Add(conflictRoomItem);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("Xung đột phòng chức năng (BR-06)"));
        }

        [TestMethod]
        public void BR06_CheckConflictMethod_IdentifiesConflictWithoutSaving()
        {
            var conflictDto = new ScheduleItemDto
            {
                MaLichTrinh = "LT999",
                MaSuKien = "EVT202601",
                TenTietMuc = "Tiết mục kiểm tra xung đột",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 8, 45, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 9, 45, 0),
                MaDienGia = "DG001"
            };

            var checkResult = _scheduleService.CheckConflict(conflictDto);

            Assert.IsFalse(checkResult.IsSuccess);
            Assert.IsTrue(checkResult.Message.Contains("Xung đột lịch diễn giả (BR-06)"));
        }
    }
}
