using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Bộ kiểm thử tự động cho Nghiệp vụ Quản lý sự kiện tổng thể Form F04 (Task Ngày 7 - Thành viên 2)
    /// Đảm bảo các quy tắc nghiệp vụ:
    /// - BR-02: Ràng buộc xóa sự kiện (chỉ xóa khi chưa phát sinh dữ liệu liên kết, nếu có thì chuyển 'Đã hủy').
    /// - BR-03: Kiểm tra logic thời gian sự kiện (NgayKetThuc >= NgayBatDau).
    /// - BR-04: Chống trùng lịch địa điểm giữa các sự kiện (Interval Overlap Detection).
    /// - BR-17: Tự động đề xuất mã sự kiện kế tiếp (SuggestNextCode).
    /// - Tìm kiếm đa điều kiện, lọc theo loại sự kiện và trạng thái sự kiện.
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// </summary>
    [TestClass]
    public class EventServiceTests
    {
        private IEventService _eventService = null!;

        [TestInitialize]
        public void Setup()
        {
            _eventService = new EventService();
        }

        // ==============================================================================
        // 1. KIỂM THỬ TRUY VẤN VÀ BỘ LỌC ĐA TIÊU CHÍ (GetAll & Filter)
        // ==============================================================================

        [TestMethod]
        public void GetAll_InitialSeedData_ReturnsAtLeast7Events()
        {
            var result = _eventService.GetAll();

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 7, $"Số lượng sự kiện thực tế: {result.Data.Count}");
        }

        [TestMethod]
        public void GetAll_FilterByKeyword_ReturnsMatchingEvents()
        {
            var result = _eventService.GetAll(keyword: "Tech Summit");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual("EVT202601", result.Data[0].MaSuKien);
        }

        [TestMethod]
        public void GetAll_FilterByPersonInCharge_ReturnsMatchingEvents()
        {
            var result = _eventService.GetAll(keyword: "Trần Minh Tâm");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 2);
            Assert.IsTrue(result.Data.All(x => x.NguoiPhuTrach.Contains("Trần Minh Tâm")));
        }

        [TestMethod]
        public void GetAll_FilterByEventType_ReturnsMatchingEvents()
        {
            var result = _eventService.GetAll(maLoaiSK: "Hội thảo Công nghệ");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 2);
            Assert.IsTrue(result.Data.All(x => x.TenLoaiSK == "Hội thảo Công nghệ" || x.MaLoaiSK == "LSK01"));
        }

        [TestMethod]
        public void GetAll_FilterByEventTypeCode_ReturnsMatchingEvents()
        {
            var result = _eventService.GetAll(maLoaiSK: "LSK05");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual("EVT202602", result.Data[0].MaSuKien);
        }

        [TestMethod]
        public void GetAll_FilterByStatus_ReturnsMatchingEvents()
        {
            var result = _eventService.GetAll(trangThai: "Đang diễn ra");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 1);
            Assert.IsTrue(result.Data.All(x => x.TrangThai == "Đang diễn ra"));
        }

        [TestMethod]
        public void GetAll_FilterByMultipleCriteria_ReturnsMatchingEvents()
        {
            // Kết hợp từ khóa + Loại sự kiện + Trạng thái
            var result = _eventService.GetAll(
                keyword: "Quân Khu 7",
                maLoaiSK: "Giải đấu Thể thao Điện tử",
                trangThai: "Chuẩn bị");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual("EVT202605", result.Data[0].MaSuKien);
        }

        [TestMethod]
        public void GetAll_FilterByDateRange_ReturnsEventsWithinRange()
        {
            DateTime tuNgay = new DateTime(2026, 11, 1);
            DateTime denNgay = new DateTime(2026, 11, 30);

            var result = _eventService.GetAll(
                keyword: null,
                maLoaiSK: null,
                trangThai: null,
                maDiaDiem: null,
                tuNgay: tuNgay,
                denNgay: denNgay);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 2);
        }

        // ==============================================================================
        // 2. KIỂM THỬ TRUY VẤN THEO MÃ (GetById)
        // ==============================================================================

        [TestMethod]
        public void GetById_ExistingEvent_ReturnsSuccess()
        {
            var result = _eventService.GetById("EVT202601");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("EVT202601", result.Data.MaSuKien);
            Assert.AreEqual("Hội nghị Công nghệ Vietnam Tech Summit 2026", result.Data.TenSuKien);
        }

        [TestMethod]
        public void GetById_NonExistingEvent_ReturnsFailed()
        {
            var result = _eventService.GetById("EVT_UNKNOWN_999");

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "Không tìm thấy");
        }

        // ==============================================================================
        // 3. KIỂM THỬ THUẬT TOÁN BR-03 (Logic thời gian sự kiện: NgayKetThuc >= NgayBatDau)
        // ==============================================================================

        [TestMethod]
        public void BR03_AddEvent_EndDateBeforeStartDate_ReturnsFailedWithBR03()
        {
            var invalidEvent = new EventListDto
            {
                MaSuKien = "EVT_BR03_TEST",
                TenSuKien = "Sự kiện thời gian lỗi",
                MaLoaiSK = "LSK01",
                MaDiaDiem = "DD004",
                NgayBatDau = new DateTime(2026, 12, 10, 17, 0, 0),
                NgayKetThuc = new DateTime(2026, 12, 10, 8, 0, 0), // Lỗi logic thời gian
                SucChua = 300,
                NganSachDuKien = 50000000m
            };

            var result = _eventService.Add(invalidEvent);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("BR-03", result.ErrorCode);
            StringAssert.Contains(result.Message, "không được trước ngày bắt đầu");
        }

        [TestMethod]
        public void BR03_UpdateEvent_EndDateBeforeStartDate_ReturnsFailedWithBR03()
        {
            var existing = _eventService.GetById("EVT202602").Data!;
            existing.NgayKetThuc = existing.NgayBatDau.AddHours(-2); // Lỗi thời gian

            var result = _eventService.Update(existing);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("BR-03", result.ErrorCode);
        }

        // ==============================================================================
        // 4. KIỂM THỬ THUẬT TOÁN BR-04 (Chống trùng lịch địa điểm giữa các sự kiện)
        // ==============================================================================

        [TestMethod]
        public void BR04_AddEvent_SameVenueTimeOverlap_ReturnsFailedWithBR04()
        {
            // EVT202601 tại DD001 từ 2026-12-15 08:00 đến 17:30
            var conflictEvent = new EventListDto
            {
                MaSuKien = "EVT_BR04_TEST",
                TenSuKien = "Sự kiện trùng lịch địa điểm DD001",
                MaLoaiSK = "LSK01",
                MaDiaDiem = "DD001",
                NgayBatDau = new DateTime(2026, 12, 15, 10, 0, 0), // Giao thoa khung giờ
                NgayKetThuc = new DateTime(2026, 12, 15, 14, 0, 0),
                SucChua = 200,
                NganSachDuKien = 30000000m
            };

            var result = _eventService.Add(conflictEvent);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("BR-04", result.ErrorCode);
            StringAssert.Contains(result.Message, "trùng lặp");
        }

        [TestMethod]
        public void BR04_AddEvent_SameVenueDifferentDate_ReturnsSuccess()
        {
            // Cùng địa điểm DD001 nhưng vào ngày khác (2026-12-18)
            var validEvent = new EventListDto
            {
                MaSuKien = "EVT_BR04_VALID",
                TenSuKien = "Sự kiện cùng địa điểm khác ngày",
                MaLoaiSK = "LSK01",
                MaDiaDiem = "DD001",
                NgayBatDau = new DateTime(2026, 12, 18, 8, 0, 0),
                NgayKetThuc = new DateTime(2026, 12, 18, 17, 0, 0),
                SucChua = 300,
                NganSachDuKien = 40000000m
            };

            var result = _eventService.Add(validEvent);

            Assert.IsTrue(result.IsSuccess);
            _eventService.Delete("EVT_BR04_VALID");
        }

        // ==============================================================================
        // 5. KIỂM THỬ THUẬT TOÁN BR-02 (Ràng buộc xóa sự kiện)
        // ==============================================================================

        [TestMethod]
        public void BR02_DeleteEvent_WithSoldTickets_ReturnsFailedWithBR02()
        {
            // EVT202601 đã có 450 vé bán -> Chặn xóa
            var result = _eventService.Delete("EVT202601");

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("BR-02", result.ErrorCode);
            StringAssert.Contains(result.Message, "đã phát sinh dữ liệu");
        }

        [TestMethod]
        public void BR02_CancelEvent_TransitionsStatusToCancelled()
        {
            var result = _eventService.CancelEvent("EVT202603");

            Assert.IsTrue(result.IsSuccess);
            var updated = _eventService.GetById("EVT202603").Data;
            Assert.IsNotNull(updated);
            Assert.AreEqual("Đã hủy", updated.TrangThai);
        }

        [TestMethod]
        public void BR02_DeleteEvent_NoTicketsSold_ReturnsSuccess()
        {
            // EVT202606 có 0 vé bán -> Cho phép xóa
            var result = _eventService.Delete("EVT202606");

            Assert.IsTrue(result.IsSuccess);
            var getResult = _eventService.GetById("EVT202606");
            Assert.IsFalse(getResult.IsSuccess);
        }

        // ==============================================================================
        // 6. KIỂM THỬ THÊM MỚI, CẬP NHẬT VÀ SINH MÃ TỰ ĐỘNG
        // ==============================================================================

        [TestMethod]
        public void AddEvent_DuplicateCode_ReturnsFailed()
        {
            var dupEvent = new EventListDto
            {
                MaSuKien = "EVT202601", // Trùng mã đã có
                TenSuKien = "Sự kiện trùng mã",
                MaLoaiSK = "LSK01",
                MaDiaDiem = "DD004",
                NgayBatDau = new DateTime(2026, 12, 28, 8, 0, 0),
                NgayKetThuc = new DateTime(2026, 12, 28, 17, 0, 0),
                SucChua = 200,
                NganSachDuKien = 20000000m
            };

            var result = _eventService.Add(dupEvent);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "đã tồn tại");
        }

        [TestMethod]
        public void AddEvent_CapacityExceedsVenueMax_ReturnsFailed()
        {
            // DD001 có sức chứa tối đa 500
            var overCapacityEvent = new EventListDto
            {
                MaSuKien = "EVT_OVER_CAP",
                TenSuKien = "Sự kiện vượt sức chứa",
                MaLoaiSK = "LSK01",
                MaDiaDiem = "DD001",
                NgayBatDau = new DateTime(2026, 12, 29, 8, 0, 0),
                NgayKetThuc = new DateTime(2026, 12, 29, 17, 0, 0),
                SucChua = 5000, // Vượt quá 500 của DD001
                NganSachDuKien = 20000000m
            };

            var result = _eventService.Add(overCapacityEvent);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "vượt quá sức chứa tối đa");
        }

        [TestMethod]
        public void SuggestNextCode_GeneratesEVTCode()
        {
            string nextCode = _eventService.SuggestNextCode();

            Assert.IsNotNull(nextCode);
            Assert.IsTrue(nextCode.StartsWith("EVT"));
            Assert.IsTrue(nextCode.Length >= 8);
        }

        [TestMethod]
        public void UpdateEvent_ValidData_ReturnsSuccess()
        {
            var existing = _eventService.GetById("EVT202602").Data!;
            existing.NguoiPhuTrach = "Lê Thanh Hằng (Cập nhật)";
            existing.NganSachDuKien = 110000000m;

            var result = _eventService.Update(existing);

            Assert.IsTrue(result.IsSuccess, result.Message);
            var updated = _eventService.GetById("EVT202602").Data!;
            Assert.AreEqual("Lê Thanh Hằng (Cập nhật)", updated.NguoiPhuTrach);
            Assert.AreEqual(110000000m, updated.NganSachDuKien);
        }
    }
}
