using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventManagement.BLL.Services;
using EventManagement.Core.Entities;

namespace EventManagement.Tests
{
    [TestClass]
    public class AlgorithmTests
    {
        private EventValidationService _eventValidator = null!;
        private TicketTierService _ticketService = null!;
        private AttendeeService _attendeeService = null!;

        [TestInitialize]
        public void Setup()
        {
            _eventValidator = new EventValidationService();
            _ticketService = new TicketTierService();
            _attendeeService = new AttendeeService();
        }

        // ==============================================================================
        // THUẬT TOÁN BR-03: THỜI GIAN SỰ KIỆN (End >= Start)
        // ==============================================================================

        [TestMethod]
        public void BR03_ValidateEventDates_ValidDates_ReturnsSuccess()
        {
            var start = new DateTime(2026, 12, 1, 8, 0, 0);
            var end = new DateTime(2026, 12, 1, 17, 0, 0);

            var result = _eventValidator.ValidateEventDates(start, end, isNew: false);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void BR03_ValidateEventDates_EndDateBeforeStartDate_ReturnsFailed()
        {
            var start = new DateTime(2026, 12, 5, 10, 0, 0);
            var end = new DateTime(2026, 12, 5, 8, 0, 0); // Lỗi: kết thúc trước bắt đầu

            var result = _eventValidator.ValidateEventDates(start, end, isNew: false);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "không được trước");
        }

        [TestMethod]
        public void BR03_ValidateEventDates_NewEventInPast_ReturnsFailed()
        {
            var now = new DateTime(2026, 11, 4);
            var start = new DateTime(2026, 10, 1, 8, 0, 0);
            var end = new DateTime(2026, 10, 2, 17, 0, 0);

            var result = _eventValidator.ValidateEventDates(start, end, isNew: true, now: now);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "quá khứ");
        }

        // ==============================================================================
        // THUẬT TOÁN BR-05: KHUNG GIỜ TIẾT MỤC LỊCH TRÌNH CON
        // ==============================================================================

        [TestMethod]
        public void BR05_ValidateScheduleItem_WithinEventRange_ReturnsSuccess()
        {
            var eventStart = new DateTime(2026, 12, 15, 8, 0, 0);
            var eventEnd = new DateTime(2026, 12, 15, 18, 0, 0);
            var itemStart = new DateTime(2026, 12, 15, 9, 0, 0);
            var itemEnd = new DateTime(2026, 12, 15, 11, 30, 0);

            var result = _eventValidator.ValidateScheduleItem(eventStart, eventEnd, itemStart, itemEnd);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void BR05_ValidateScheduleItem_StartsBeforeEvent_ReturnsFailed()
        {
            var eventStart = new DateTime(2026, 12, 15, 8, 0, 0);
            var eventEnd = new DateTime(2026, 12, 15, 18, 0, 0);
            var itemStart = new DateTime(2026, 12, 15, 7, 30, 0); // Lỗi: trước 8h
            var itemEnd = new DateTime(2026, 12, 15, 9, 0, 0);

            var result = _eventValidator.ValidateScheduleItem(eventStart, eventEnd, itemStart, itemEnd);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "trước giờ khai mạc");
        }

        [TestMethod]
        public void BR05_ValidateScheduleItem_EndsAfterEvent_ReturnsFailed()
        {
            var eventStart = new DateTime(2026, 12, 15, 8, 0, 0);
            var eventEnd = new DateTime(2026, 12, 15, 18, 0, 0);
            var itemStart = new DateTime(2026, 12, 15, 16, 0, 0);
            var itemEnd = new DateTime(2026, 12, 15, 19, 0, 0); // Lỗi: sau 18h

            var result = _eventValidator.ValidateScheduleItem(eventStart, eventEnd, itemStart, itemEnd);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "vượt quá");
        }

        // ==============================================================================
        // THUẬT TOÁN BR-04: CHỐNG TRÙNG LỊCH ĐỊA ĐIỂM (INTERVAL OVERLAP)
        // ==============================================================================

        [TestMethod]
        public void BR04_CheckVenueAvailability_OverlappingTimeSameVenue_ReturnsFailed()
        {
            var existing = new List<SuKien>
            {
                new SuKien
                {
                    MaSuKien = "EVT001",
                    TenSuKien = "Vietnam Tech Summit 2026",
                    MaDiaDiem = "DD001",
                    NgayBatDau = new DateTime(2026, 12, 15, 8, 0, 0),
                    NgayKetThuc = new DateTime(2026, 12, 15, 17, 0, 0),
                    TrangThai = "Mở đăng ký"
                }
            };

            // Thử đặt sự kiện 2 cùng DD001 từ 13h đến 20h (Giao thoa từ 13h đến 17h)
            var newStart = new DateTime(2026, 12, 15, 13, 0, 0);
            var newEnd = new DateTime(2026, 12, 15, 20, 0, 0);

            var result = _eventValidator.CheckVenueAvailability("DD001", newStart, newEnd, null, existing);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "đã được đăng ký");
        }

        [TestMethod]
        public void BR04_CheckVenueAvailability_SameVenueDifferentDay_ReturnsSuccess()
        {
            var existing = new List<SuKien>
            {
                new SuKien
                {
                    MaSuKien = "EVT001",
                    TenSuKien = "Vietnam Tech Summit 2026",
                    MaDiaDiem = "DD001",
                    NgayBatDau = new DateTime(2026, 12, 15, 8, 0, 0),
                    NgayKetThuc = new DateTime(2026, 12, 15, 17, 0, 0),
                    TrangThai = "Mở đăng ký"
                }
            };

            // Đặt ngày hôm sau 16/12 -> Hoàn toàn hợp lệ
            var newStart = new DateTime(2026, 12, 16, 8, 0, 0);
            var newEnd = new DateTime(2026, 12, 16, 17, 0, 0);

            var result = _eventValidator.CheckVenueAvailability("DD001", newStart, newEnd, null, existing);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void BR04_CheckVenueAvailability_ConflictingEventWasCancelled_ReturnsSuccess()
        {
            var existing = new List<SuKien>
            {
                new SuKien
                {
                    MaSuKien = "EVT002",
                    TenSuKien = "Triển lãm đã hủy",
                    MaDiaDiem = "DD001",
                    NgayBatDau = new DateTime(2026, 12, 15, 8, 0, 0),
                    NgayKetThuc = new DateTime(2026, 12, 15, 17, 0, 0),
                    TrangThai = "Hủy" // Đã hủy nên không tính là chiếm địa điểm
                }
            };

            var newStart = new DateTime(2026, 12, 15, 9, 0, 0);
            var newEnd = new DateTime(2026, 12, 15, 16, 0, 0);

            var result = _eventValidator.CheckVenueAvailability("DD001", newStart, newEnd, null, existing);

            Assert.IsTrue(result.IsSuccess);
        }

        // ==============================================================================
        // THUẬT TOÁN BR-08 & BR-09: QUẢN LÝ KHO VÉ & SỐ LƯỢNG MUA
        // ==============================================================================

        [TestMethod]
        public void BR08_ValidateTierCapacityUpdate_ReduceBelowSold_ReturnsFailed()
        {
            var tier = new HangVe
            {
                MaHangVe = "HV001",
                TenHangVe = "Vé VIP",
                SoLuongPhatHanh = 100,
                SoLuongDaBan = 45 // Đã bán 45 vé
            };

            // Thử sửa giảm phát hành xuống 40 vé (< 45)
            var result = _ticketService.ValidateTierCapacityUpdate(tier, 40);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "Không thể giảm");
        }

        [TestMethod]
        public void BR08_ValidateTierCapacityUpdate_IncreaseCapacity_ReturnsSuccess()
        {
            var tier = new HangVe
            {
                MaHangVe = "HV001",
                TenHangVe = "Vé VIP",
                SoLuongPhatHanh = 100,
                SoLuongDaBan = 45
            };

            var result = _ticketService.ValidateTierCapacityUpdate(tier, 150);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void BR09_CheckTicketAvailability_QuantityWithinStock_ReturnsSuccess()
        {
            var tier = new HangVe
            {
                MaHangVe = "HV001",
                TenHangVe = "Vé Standard",
                SoLuongPhatHanh = 100,
                SoLuongDaBan = 80 // Còn lại 20 vé
            };

            // Mua 5 vé (hợp lệ trong khoảng 1-10 vé và <= 20)
            var result = _ticketService.CheckTicketAvailability(tier, 5);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(15, result.Data); // Còn lại 15 vé sau khi mua
        }

        [TestMethod]
        public void BR09_CheckTicketAvailability_ExceedsAvailableStock_ReturnsFailed()
        {
            var tier = new HangVe
            {
                MaHangVe = "HV001",
                TenHangVe = "Vé Standard",
                SoLuongPhatHanh = 50,
                SoLuongDaBan = 48 // Chỉ còn lại 2 vé
            };

            // Mua 5 vé trong khi chỉ còn 2 vé
            var result = _ticketService.CheckTicketAvailability(tier, 5);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "vượt quá số lượng");
        }

        [TestMethod]
        public void BR10_CheckTicketAvailability_BuyMoreThan10Tickets_ReturnsFailed()
        {
            var tier = new HangVe
            {
                MaHangVe = "HV001",
                TenHangVe = "Vé Standard",
                SoLuongPhatHanh = 500,
                SoLuongDaBan = 50
            };

            // Mua 12 vé (vi phạm BR-10: tối đa 10 vé/lần)
            var result = _ticketService.CheckTicketAvailability(tier, 12);

            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(result.Message, "từ 1 đến 10 vé");
        }
    }
}
