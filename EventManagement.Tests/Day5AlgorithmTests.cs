using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventManagement.BLL.Services;
using EventManagement.Core.Entities;

namespace EventManagement.Tests
{
    [TestClass]
    public class Day5AlgorithmTests
    {
        private ScheduleValidationService _scheduleValidator = null!;
        private StaffScheduleService _staffScheduleService = null!;
        private BudgetService _budgetService = null!;

        [TestInitialize]
        public void Setup()
        {
            _scheduleValidator = new ScheduleValidationService();
            _staffScheduleService = new StaffScheduleService();
            _budgetService = new BudgetService();
        }

        // ==============================================================================
        // THUẬT TOÁN BR-06 (PHẦN 1): CHỐNG TRÙNG DIỄN GIẢ (Speaker Conflict Detection)
        // ==============================================================================

        [TestMethod]
        public void BR06_SpeakerConflict_SameSpeaker_OverlappingTimes_ReturnsFailed()
        {
            var existing = new List<LichTrinh>
            {
                new LichTrinh
                {
                    MaLichTrinh = "LT01",
                    MaDienGia = "DG01",
                    TenTietMuc = "Keynote Khai mạc",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 9, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 10, 30, 0)
                }
            };

            // Tiết mục mới cho DG01 từ 9:30 đến 11:00 (giao thoa 9:30 - 10:30)
            var startNew = new DateTime(2026, 12, 1, 9, 30, 0);
            var endNew = new DateTime(2026, 12, 1, 11, 0, 0);

            var result = _scheduleValidator.CheckSpeakerConflict("DG01", startNew, endNew, null, existing);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("Xung đột lịch diễn giả (BR-06)"));
        }

        [TestMethod]
        public void BR06_SpeakerConflict_SameSpeaker_PartialOverlap_ReturnsFailed()
        {
            var existing = new List<LichTrinh>
            {
                new LichTrinh
                {
                    MaLichTrinh = "LT01",
                    MaDienGia = "DG01",
                    TenTietMuc = "Chuyên đề AI",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 14, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 16, 0, 0)
                }
            };

            // Tiết mục mới từ 13:00 đến 14:30 (giao thoa 14:00 - 14:30)
            var startNew = new DateTime(2026, 12, 1, 13, 0, 0);
            var endNew = new DateTime(2026, 12, 1, 14, 30, 0);

            var result = _scheduleValidator.CheckSpeakerConflict("DG01", startNew, endNew, null, existing);

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void BR06_SpeakerConflict_SameSpeaker_NonOverlapping_ReturnsSuccess()
        {
            var existing = new List<LichTrinh>
            {
                new LichTrinh
                {
                    MaLichTrinh = "LT01",
                    MaDienGia = "DG01",
                    TenTietMuc = "Phiên Sáng",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 8, 30, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 10, 0, 0)
                }
            };

            // Tiết mục mới bắt đầu lúc 14:00 (hoàn toàn cách biệt)
            var startNew = new DateTime(2026, 12, 1, 14, 0, 0);
            var endNew = new DateTime(2026, 12, 1, 15, 30, 0);

            var result = _scheduleValidator.CheckSpeakerConflict("DG01", startNew, endNew, null, existing);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void BR06_SpeakerConflict_SameSpeaker_ContiguousTimes_ReturnsSuccess()
        {
            var existing = new List<LichTrinh>
            {
                new LichTrinh
                {
                    MaLichTrinh = "LT01",
                    MaDienGia = "DG01",
                    TenTietMuc = "Tiết mục 1",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 9, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 10, 0, 0)
                }
            };

            // Tiết mục 2 bắt đầu đúng lúc 10:00 (liền kề, không giao thoa)
            var startNew = new DateTime(2026, 12, 1, 10, 0, 0);
            var endNew = new DateTime(2026, 12, 1, 11, 0, 0);

            var result = _scheduleValidator.CheckSpeakerConflict("DG01", startNew, endNew, null, existing);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void BR06_SpeakerConflict_ExcludeSelf_ReturnsSuccess()
        {
            var existing = new List<LichTrinh>
            {
                new LichTrinh
                {
                    MaLichTrinh = "LT01",
                    MaDienGia = "DG01",
                    TenTietMuc = "Keynote",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 9, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 10, 0, 0)
                }
            };

            // Cập nhật chính LT01 với khung giờ giữ nguyên
            var start = new DateTime(2026, 12, 1, 9, 0, 0);
            var end = new DateTime(2026, 12, 1, 10, 0, 0);

            var result = _scheduleValidator.CheckSpeakerConflict("DG01", start, end, excludeMaLichTrinh: "LT01", existing);

            Assert.IsTrue(result.IsSuccess);
        }

        // ==============================================================================
        // THUẬT TOÁN BR-06 (PHẦN 2): CHỐNG TRÙNG PHÒNG CHỨC NĂNG (Room Conflict)
        // ==============================================================================

        [TestMethod]
        public void BR06_RoomConflict_SameRoom_OverlappingTimes_ReturnsFailed()
        {
            var existing = new List<LichTrinh>
            {
                new LichTrinh
                {
                    MaLichTrinh = "LT01",
                    DiaDiemChiTiet = "Hội trường Grand Ballroom",
                    TenTietMuc = "Lễ Khai Mạc",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 8, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 11, 0, 0)
                }
            };

            // Xếp tiết mục khác vào cùng hội trường từ 10:00 đến 12:00
            var startNew = new DateTime(2026, 12, 1, 10, 0, 0);
            var endNew = new DateTime(2026, 12, 1, 12, 0, 0);

            var result = _scheduleValidator.CheckRoomConflict("Hội trường Grand Ballroom", startNew, endNew, null, existing);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("Xung đột phòng chức năng (BR-06)"));
        }

        [TestMethod]
        public void BR06_RoomConflict_SameRoom_CaseInsensitive_ReturnsFailed()
        {
            var existing = new List<LichTrinh>
            {
                new LichTrinh
                {
                    MaLichTrinh = "LT01",
                    DiaDiemChiTiet = "Phòng Hội Thảo 1",
                    TenTietMuc = "Talkshow",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 14, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 15, 30, 0)
                }
            };

            var startNew = new DateTime(2026, 12, 1, 14, 30, 0);
            var endNew = new DateTime(2026, 12, 1, 16, 0, 0);

            // Tên phòng viết thường 'phòng hội thảo 1' vẫn phải phát hiện trùng
            var result = _scheduleValidator.CheckRoomConflict("phòng hội thảo 1", startNew, endNew, null, existing);

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void BR06_RoomConflict_DifferentRoom_SameTime_ReturnsSuccess()
        {
            var existing = new List<LichTrinh>
            {
                new LichTrinh
                {
                    MaLichTrinh = "LT01",
                    DiaDiemChiTiet = "Phòng A",
                    TenTietMuc = "Hội thảo A",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 9, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 11, 0, 0)
                }
            };

            // Cùng giờ 9:00 - 11:00 nhưng diễn ra ở Phòng B -> Hợp lệ
            var startNew = new DateTime(2026, 12, 1, 9, 0, 0);
            var endNew = new DateTime(2026, 12, 1, 11, 0, 0);

            var result = _scheduleValidator.CheckRoomConflict("Phòng B", startNew, endNew, null, existing);

            Assert.IsTrue(result.IsSuccess);
        }

        // ==============================================================================
        // THUẬT TOÁN BR-13: CHỐNG TRÙNG CA TRỰC NHÂN SỰ (Shift Conflict Detection)
        // ==============================================================================

        [TestMethod]
        public void BR13_ShiftConflict_SameStaff_OverlappingShifts_ReturnsFailed()
        {
            var existing = new List<PhanCongNhanSu>
            {
                new PhanCongNhanSu
                {
                    MaPhanCong = "PC01",
                    MaNhanSu = "NS001",
                    NhiemVu = "Soát vé cổng chính",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 7, 30, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 11, 30, 0)
                }
            };

            // Phân công NS001 đi hỗ trợ bàn VIP từ 10:00 đến 13:00 -> Trùng
            var startNew = new DateTime(2026, 12, 1, 10, 0, 0);
            var endNew = new DateTime(2026, 12, 1, 13, 0, 0);

            var result = _staffScheduleService.CheckShiftConflict("NS001", startNew, endNew, null, existing);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("Xung đột ca trực nhân sự (BR-13)"));
        }

        [TestMethod]
        public void BR13_ShiftConflict_SameStaff_EnclosingShift_ReturnsFailed()
        {
            var existing = new List<PhanCongNhanSu>
            {
                new PhanCongNhanSu
                {
                    MaPhanCong = "PC01",
                    MaNhanSu = "NS002",
                    NhiemVu = "Hậu cần sân khấu",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 9, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 11, 0, 0)
                }
            };

            // Ca trực mới bao trọn ca cũ (8:00 đến 12:00) -> Trùng
            var startNew = new DateTime(2026, 12, 1, 8, 0, 0);
            var endNew = new DateTime(2026, 12, 1, 12, 0, 0);

            var result = _staffScheduleService.CheckShiftConflict("NS002", startNew, endNew, null, existing);

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void BR13_ShiftConflict_SameStaff_ContiguousShifts_ReturnsSuccess()
        {
            var existing = new List<PhanCongNhanSu>
            {
                new PhanCongNhanSu
                {
                    MaPhanCong = "PC01",
                    MaNhanSu = "NS001",
                    NhiemVu = "Ca Sáng",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 7, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 12, 0, 0)
                }
            };

            // Ca Chiều bắt đầu đúng lúc 12:00 (liền kề, không giao thoa) -> Hợp lệ
            var startNew = new DateTime(2026, 12, 1, 12, 0, 0);
            var endNew = new DateTime(2026, 12, 1, 17, 0, 0);

            var result = _staffScheduleService.CheckShiftConflict("NS001", startNew, endNew, null, existing);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void BR13_ShiftConflict_DifferentStaff_SameShift_ReturnsSuccess()
        {
            var existing = new List<PhanCongNhanSu>
            {
                new PhanCongNhanSu
                {
                    MaPhanCong = "PC01",
                    MaNhanSu = "NS001",
                    NhiemVu = "Trực quầy",
                    ThoiGianBatDau = new DateTime(2026, 12, 1, 8, 0, 0),
                    ThoiGianKetThuc = new DateTime(2026, 12, 1, 12, 0, 0)
                }
            };

            // Phân công NS002 cùng thời gian 8:00 - 12:00 -> Hoàn toàn hợp lệ
            var startNew = new DateTime(2026, 12, 1, 8, 0, 0);
            var endNew = new DateTime(2026, 12, 1, 12, 0, 0);

            var result = _staffScheduleService.CheckShiftConflict("NS002", startNew, endNew, null, existing);

            Assert.IsTrue(result.IsSuccess);
        }

        // ==============================================================================
        // THUẬT TOÁN BR-14 & BR-15: NGÂN SÁCH & CẢNH BÁO BỘI CHI (Budget Overspending Alert)
        // ==============================================================================

        [TestMethod]
        public void BR14_ValidateExpense_PositiveAmount_ReturnsSuccess()
        {
            var result = _budgetService.ValidateExpenseAmount(5_000_000m);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void BR14_ValidateExpense_ZeroOrNegativeAmount_ReturnsFailed()
        {
            var zeroResult = _budgetService.ValidateExpenseAmount(0m);
            var negativeResult = _budgetService.ValidateExpenseAmount(-100_000m);

            Assert.IsFalse(zeroResult.IsSuccess);
            Assert.IsFalse(negativeResult.IsSuccess);
        }

        [TestMethod]
        public void BR15_BudgetStatus_Under80Percent_NoAlert()
        {
            decimal nganSach = 100_000_000m;
            decimal daChi = 40_000_000m;
            decimal chiMoi = 10_000_000m; // Tổng chi 50 triệu = 50%

            var status = _budgetService.CalculateBudgetStatus("EV01", nganSach, daChi, chiMoi);

            Assert.AreEqual(50.0, status.TyLeSuDung, 0.01);
            Assert.IsFalse(status.IsWarning80);
            Assert.IsFalse(status.IsOverBudget);
            Assert.IsTrue(status.ThongBaoTrangThai.Contains("an toàn"));
        }

        [TestMethod]
        public void BR15_BudgetStatus_Exactly80Percent_TriggersWarning()
        {
            decimal nganSach = 100_000_000m;
            decimal daChi = 60_000_000m;
            decimal chiMoi = 20_000_000m; // Tổng chi 80 triệu = 80.0%

            var status = _budgetService.CalculateBudgetStatus("EV01", nganSach, daChi, chiMoi);

            Assert.AreEqual(80.0, status.TyLeSuDung, 0.01);
            Assert.IsTrue(status.IsWarning80, "Chạm đúng 80% phải kích hoạt cảnh báo vàng cam (IsWarning80 = true)");
            Assert.IsFalse(status.IsOverBudget);
            Assert.IsTrue(status.ThongBaoTrangThai.Contains("CẢNH BÁO"));
        }

        [TestMethod]
        public void BR15_BudgetStatus_Between80And100Percent_TriggersWarning()
        {
            decimal nganSach = 100_000_000m;
            decimal daChi = 75_000_000m;
            decimal chiMoi = 20_000_000m; // Tổng chi 95 triệu = 95.0%

            var status = _budgetService.CalculateBudgetStatus("EV01", nganSach, daChi, chiMoi);

            Assert.AreEqual(95.0, status.TyLeSuDung, 0.01);
            Assert.IsTrue(status.IsWarning80);
            Assert.IsFalse(status.IsOverBudget);
        }

        [TestMethod]
        public void BR15_BudgetStatus_Over100Percent_TriggersOverBudget()
        {
            decimal nganSach = 100_000_000m;
            decimal daChi = 90_000_000m;
            decimal chiMoi = 25_000_000m; // Tổng chi 115 triệu = 115.0%

            var status = _budgetService.CalculateBudgetStatus("EV01", nganSach, daChi, chiMoi);

            Assert.AreEqual(115.0, status.TyLeSuDung, 0.01);
            Assert.IsFalse(status.IsWarning80, "Khi vượt 100% cờ cảnh báo bội chi đỏ phải bật thay vì vàng cam");
            Assert.IsTrue(status.IsOverBudget, "Vượt quá 100% phải kích hoạt IsOverBudget = true");
            Assert.IsTrue(status.ThongBaoTrangThai.Contains("BỘI CHI"));
        }

        [TestMethod]
        public void BR15_CheckBudgetAlert_ValidSoftWarning_ReturnsSuccessWithAlertData()
        {
            var chiPhis = new List<ChiPhiSuKien>
            {
                new ChiPhiSuKien { MaChiPhi = "CP01", SoTien = 50_000_000m },
                new ChiPhiSuKien { MaChiPhi = "CP02", SoTien = 35_000_000m }
            };

            // Ngân sách 100 triệu, đã chi 85 triệu, chi thêm 10 triệu -> 95 triệu (95%)
            var result = _budgetService.CheckBudgetAlert("EV01", 10_000_000m, "Âm thanh ánh sáng", 100_000_000m, chiPhis);

            // Cảnh báo mềm: Kết quả thành công (cho phép lưu) nhưng dữ liệu mang cảnh báo
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.IsWarning80);
            Assert.IsFalse(result.Data.IsOverBudget);
        }

        [TestMethod]
        public void BR15_CalculateRemainingBudget_ComputesCorrectly()
        {
            decimal nganSach = 200_000_000m;
            decimal tongChi = 135_000_000m;

            var remaining = _budgetService.CalculateRemainingBudget(nganSach, tongChi);

            Assert.AreEqual(65_000_000m, remaining);
        }
    }
}
