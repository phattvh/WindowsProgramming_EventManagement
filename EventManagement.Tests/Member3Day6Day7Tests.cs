using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;

namespace EventManagement.Tests
{
    /// <summary>
    /// Bộ kiểm thử toàn diện cho các tính năng của Thành viên 3:
    /// - Form F17: Quản lý chi phí & ngân sách (Day 5 carryover - BR-14, BR-15)
    /// - Form F11: Đăng ký & Bán vé sự kiện (Day 6 - BR-09, BR-10, BR-17, BR-18)
    /// - Form F13: Soát vé & Điểm danh QR qua Webcam (Day 7 - BR-11, BR-12)
    /// </summary>
    [TestClass]
    public class Member3Day6Day7Tests
    {
        private IBudgetService _budgetService = null!;
        private ITicketOrderService _ticketOrderService = null!;
        private ICheckInService _checkInService = null!;

        [TestInitialize]
        public void Setup()
        {
            _budgetService = new BudgetService();
            _ticketOrderService = new TicketOrderService();
            _checkInService = new CheckInService();
        }

        #region --- 1. KIỂM THỬ F17 & NGÂN SÁCH (BR-14, BR-15) ---

        [TestMethod]
        public void BR14_ExpenseAmount_MustBeGreaterThanZero()
        {
            // Số tiền <= 0 phải bị từ chối theo BR-14
            var invalidExpense = new ExpenseDto
            {
                MaSuKien = "EVT202601",
                KhoanMucChi = "Khoản chi không hợp lệ",
                SoTien = 0m,
                NguoiLapPhieu = "Nguyễn Văn A"
            };

            var result = _budgetService.AddExpense(invalidExpense);
            Assert.IsFalse(result.IsSuccess, "Chi phí bằng 0 phải bị từ chối theo BR-14.");

            invalidExpense.SoTien = -500000m;
            var negativeResult = _budgetService.AddExpense(invalidExpense);
            Assert.IsFalse(negativeResult.IsSuccess, "Chi phí âm phải bị từ chối theo BR-14.");
        }

        [TestMethod]
        public void BR15_BudgetAlert_Under80Percent_IsNormal()
        {
            // Kiểm tra trạng thái bình thường khi chi tiêu dưới 80%
            var alertDto = new BudgetAlertDto
            {
                MaSuKien = "EVT202601",
                NganSachDuKien = 100_000_000m,
                TongThucChiHienTai = 70_000_000m,
                KhoanChiSapThem = 0m
            };

            Assert.AreEqual(70.0, alertDto.TyLeSuDung, 0.01);
            Assert.IsFalse(alertDto.IsWarning80, "Dưới 80% không được kích hoạt cảnh báo vàng.");
            Assert.IsFalse(alertDto.IsOverBudget, "Dưới 80% không được kích hoạt bội chi đỏ.");
        }

        [TestMethod]
        public void BR15_BudgetAlert_Between80And100Percent_IsWarning()
        {
            // Kiểm tra cảnh báo vàng cam khi chạm ngưỡng 80% - 100%
            var alertDto = new BudgetAlertDto
            {
                MaSuKien = "EVT202601",
                NganSachDuKien = 100_000_000m,
                TongThucChiHienTai = 85_000_000m,
                KhoanChiSapThem = 0m
            };

            Assert.AreEqual(85.0, alertDto.TyLeSuDung, 0.01);
            Assert.IsTrue(alertDto.IsWarning80, "Từ 80% - 100% phải kích hoạt cảnh báo vàng cam (BR-15).");
            Assert.IsFalse(alertDto.IsOverBudget, "Dưới 100% chưa bị bội chi.");
        }

        [TestMethod]
        public void BR15_BudgetAlert_Over100Percent_IsDanger()
        {
            // Kiểm tra báo động đỏ khi chi tiêu vượt 100% ngân sách
            var alertDto = new BudgetAlertDto
            {
                MaSuKien = "EVT202601",
                NganSachDuKien = 100_000_000m,
                TongThucChiHienTai = 110_000_000m,
                KhoanChiSapThem = 0m
            };

            Assert.AreEqual(110.0, alertDto.TyLeSuDung, 0.01);
            Assert.IsFalse(alertDto.IsWarning80, "Trên 100% chuyển sang trạng thái bội chi đỏ.");
            Assert.IsTrue(alertDto.IsOverBudget, "Trên 100% phải kích hoạt cảnh báo bội chi đỏ (BR-15).");
        }

        [TestMethod]
        public void BudgetService_AddAndDeleteExpense_CalculatesBalanceCorrectly()
        {
            string maSuKien = "EVT202601";
            var initialStatus = _budgetService.GetBudgetStatus(maSuKien);
            Assert.IsTrue(initialStatus.IsSuccess);
            decimal initialSpent = initialStatus.Data!.TongThucChiHienTai;

            // Thêm chi phí mới
            var newExpense = new ExpenseDto
            {
                MaChiPhi = _budgetService.GetNextExpenseCode(),
                MaSuKien = maSuKien,
                KhoanMucChi = "In backdrop sân khấu lớn",
                SoTien = 5_000_000m,
                NguoiLapPhieu = "Nguyễn Văn Thắng",
                NgayChi = DateTime.Now
            };

            var addRes = _budgetService.AddExpense(newExpense);
            Assert.IsTrue(addRes.IsSuccess);

            var afterAddStatus = _budgetService.GetBudgetStatus(maSuKien);
            Assert.AreEqual(initialSpent + 5_000_000m, afterAddStatus.Data!.TongThucChiHienTai);

            // Xóa chi phí vừa thêm
            var deleteRes = _budgetService.DeleteExpense(newExpense.MaChiPhi);
            Assert.IsTrue(deleteRes.IsSuccess);

            var afterDeleteStatus = _budgetService.GetBudgetStatus(maSuKien);
            Assert.AreEqual(initialSpent, afterDeleteStatus.Data!.TongThucChiHienTai);
        }

        #endregion

        #region --- 2. KIỂM THỬ F11 & BÁN VÉ (BR-09, BR-10, BR-17, BR-18) ---

        [TestMethod]
        public void BR10_TicketOrder_ZeroOrNegativeQuantity_ReturnsFailed()
        {
            var request = new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202601",
                MaHangVe = "HV001",
                SoLuongMua = 0,
                HoTenNguoiMua = "Trần Văn An",
                SoDienThoai = "0901234567"
            };

            var result = _ticketOrderService.RegisterTicketOrder(request);
            Assert.IsFalse(result.IsSuccess, "Mua 0 vé phải bị từ chối theo BR-10.");
        }

        [TestMethod]
        public void BR10_TicketOrder_QuantityGreaterThan10_ReturnsFailed()
        {
            var request = new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202601",
                MaHangVe = "HV001",
                SoLuongMua = 11, // Vượt quá 10 vé
                HoTenNguoiMua = "Lê Thị Bích",
                SoDienThoai = "0912345678"
            };

            var result = _ticketOrderService.RegisterTicketOrder(request);
            Assert.IsFalse(result.IsSuccess, "Mua > 10 vé phải bị từ chối theo BR-10.");
        }

        [TestMethod]
        public void BR10_TicketOrder_ValidQuantity1To10_DecomposesDetailTickets()
        {
            int qty = 3;
            var request = new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202601",
                MaHangVe = "HV001",
                SoLuongMua = qty,
                HoTenNguoiMua = "Đặng Hoàng Nam",
                SoDienThoai = "0987654321",
                Email = "nam.dang@test.com",
                DanhSachNguoiDiCung = new List<string> { "Đặng Hoàng Nam", "Nguyễn Thu Hà", "Trần Minh Quân" }
            };

            var result = _ticketOrderService.RegisterTicketOrder(request);
            Assert.IsTrue(result.IsSuccess, "Mua 3 vé hợp lệ phải thành công.");
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(qty, result.Data.DanhSachVe.Count, "Số vé con được phân rã phải đúng bằng số lượng mua.");
            Assert.AreEqual(600000m, result.Data.TongTien, "Tổng tiền 3 vé x 200,000 đ = 600,000 đ.");
        }

        [TestMethod]
        public void BR09_TicketOrder_InsufficientInventory_ReturnsFailed()
        {
            // HV003 của EVT202602 có phát hành 100, đã bán 98 -> còn đúng 2 vé khả dụng
            var request = new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202602",
                MaHangVe = "HV003",
                SoLuongMua = 5, // Yêu cầu 5 vé trong khi chỉ còn 2 vé
                HoTenNguoiMua = "Phạm Văn Cường",
                SoDienThoai = "0933221144"
            };

            var result = _ticketOrderService.RegisterTicketOrder(request);
            Assert.IsFalse(result.IsSuccess, "Không được phép bán vượt số lượng vé khả dụng (BR-09).");
        }

        [TestMethod]
        public void BR17_TicketOrder_GeneratesUniqueGuidQrTokens()
        {
            int qty = 4;
            var request = new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202601",
                MaHangVe = "HV002",
                SoLuongMua = qty,
                HoTenNguoiMua = "Võ Minh Trí",
                SoDienThoai = "0944556677",
                Email = "tri.vo@test.com",
                DanhSachNguoiDiCung = new List<string> { "Người 1", "Người 2", "Người 3", "Người 4" }
            };

            var result = _ticketOrderService.RegisterTicketOrder(request);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);

            // Kiểm tra tính duy nhất của mã vé và mã QR token
            var qrTokens = result.Data.DanhSachVe.Select(v => v.MaQR).ToList();
            var ticketCodes = result.Data.DanhSachVe.Select(v => v.MaVe).ToList();

            Assert.AreEqual(qty, qrTokens.Distinct().Count(), "Mỗi vé con phải có một mã QR token GUID độc lập duy nhất (BR-17).");
            Assert.AreEqual(qty, ticketCodes.Distinct().Count(), "Mỗi vé con phải có mã vé độc lập duy nhất.");
        }

        [TestMethod]
        public void BR18_TicketOrder_SimulatesAtomicTransaction()
        {
            var tiers = _ticketOrderService.GetTicketTiersByEvent("EVT202601");
            var targetTier = tiers.First(t => t.MaHangVe == "HV001");
            int initialSold = targetTier.SoLuongDaBan;

            var request = new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202601",
                MaHangVe = "HV001",
                SoLuongMua = 2,
                HoTenNguoiMua = "Bùi Anh Tuấn",
                SoDienThoai = "0977889900",
                Email = "tuan.bui@test.com"
            };

            var result = _ticketOrderService.RegisterTicketOrder(request);
            Assert.IsTrue(result.IsSuccess);

            // Kiểm tra tồn kho sau khi đặt vé
            var afterTiers = _ticketOrderService.GetTicketTiersByEvent("EVT202601");
            var afterTier = afterTiers.First(t => t.MaHangVe == "HV001");

            Assert.AreEqual(initialSold + 2, afterTier.SoLuongDaBan, "Số lượng đã bán phải tăng chính xác sau giao dịch.");
        }

        #endregion

        #region --- 3. KIỂM THỬ F13 & SOÁT VÉ QR (BR-11, BR-12) ---

        [TestMethod]
        public void BR11_CheckIn_NonExistentCode_ReturnsFailed()
        {
            var req = new CheckInRequestDto
            {
                MaSuKien = "EVT202601",
                MaQRLuocDoc = "MA_KHONG_TON_TAI_99999",
                PhuongThuc = "Nhập mã thủ công"
            };

            var result = _checkInService.ProcessCheckIn(req);
            Assert.IsFalse(result.IsSuccess, "Mã vé không tồn tại phải bị từ chối check-in (BR-11).");
        }

        [TestMethod]
        public void BR11_CheckIn_WrongEvent_ReturnsFailed()
        {
            // VE00001 thuộc EVT202601 (Vietnam Tech Summit)
            // Thử check-in tại sự kiện EVT202602 (Workshop GenAI)
            var req = new CheckInRequestDto
            {
                MaSuKien = "EVT202602",
                MaQRLuocDoc = "VE00001",
                PhuongThuc = "Webcam QR"
            };

            var result = _checkInService.ProcessCheckIn(req);
            Assert.IsFalse(result.IsSuccess, "Vé không thuộc sự kiện đang chọn phải bị từ chối (BR-11).");
        }

        [TestMethod]
        public void BR11_CheckIn_ValidTicket_Succeeds()
        {
            // Reset trạng thái vé VE00001 về hợp lệ
            _checkInService.ResetCheckInForDemo("VE00001");

            var req = new CheckInRequestDto
            {
                MaSuKien = "EVT202601",
                MaQRLuocDoc = "VE00001",
                PhuongThuc = "Webcam QR"
            };

            var result = _checkInService.ProcessCheckIn(req);
            Assert.IsTrue(result.IsSuccess, "Vé hợp lệ quét lần đầu phải check-in thành công (BR-11).");
            Assert.IsNotNull(result.Data);
            Assert.IsFalse(result.Data.IsWarningDuplicate);
            Assert.IsNotNull(result.Data.ThoiGianCheckIn);
        }

        [TestMethod]
        public void BR12_CheckIn_DuplicateScan_TriggersWarningAndRejects()
        {
            // Quét lần 1: thành công
            _checkInService.ResetCheckInForDemo("VE00002");

            var req1 = new CheckInRequestDto
            {
                MaSuKien = "EVT202601",
                MaQRLuocDoc = "VE00002",
                PhuongThuc = "Webcam QR"
            };

            var result1 = _checkInService.ProcessCheckIn(req1);
            Assert.IsTrue(result1.IsSuccess, "Lần quét 1 phải thành công.");

            // Quét lần 2: phải bị cảnh báo trùng lặp (BR-12)
            var result2 = _checkInService.ProcessCheckIn(req1);

            Assert.IsNotNull(result2.Data);
            Assert.IsFalse(result2.Data.IsSuccess, "Lần quét 2 không được phép check-in thành công.");
            Assert.IsTrue(result2.Data.IsWarningDuplicate, "Phải kích hoạt cờ cảnh báo trùng lặp (BR-12).");
            Assert.IsNotNull(result2.Data.ThoiGianCheckInTruocDo, "Phải ghi nhận thời gian quét trước đó.");
        }

        [TestMethod]
        public void CheckIn_GetStatistics_ReflectsAccurateCounts()
        {
            string maSuKien = "EVT202601";
            var stats = _checkInService.GetCheckInStatistics(maSuKien);

            Assert.IsTrue(stats.TongVePhatHanh >= 3);
            Assert.IsTrue(stats.DaCheckIn >= 0);
            Assert.AreEqual(stats.TongVePhatHanh - stats.DaCheckIn, stats.ChuaCheckIn);
            Assert.IsTrue(stats.TyLeCheckIn >= 0 && stats.TyLeCheckIn <= 100);
        }

        [TestMethod]
        public void TicketOrder_SyncsWithCheckInService_CanCheckInNewlyPurchasedTicket()
        {
            // 1. Đặt mua 1 vé mới trên TicketOrderService
            var orderReq = new RegisterTicketRequestDto
            {
                MaSuKien = "EVT202601",
                MaHangVe = "HV001",
                SoLuongMua = 1,
                HoTenNguoiMua = "Khách Hàng Mới Liên Thông",
                SoDienThoai = "0988776655",
                Email = "lienthong@test.com"
            };

            var orderResult = _ticketOrderService.RegisterTicketOrder(orderReq);
            Assert.IsTrue(orderResult.IsSuccess);
            Assert.IsNotNull(orderResult.Data);

            var newTicket = orderResult.Data.DanhSachVe.First();
            string qrToken = newTicket.MaQR;

            // 2. Điểm danh vé vừa mua bằng mã QR token trên CheckInService
            var checkInReq = new CheckInRequestDto
            {
                MaSuKien = "EVT202601",
                MaQRLuocDoc = qrToken,
                PhuongThuc = "Quét QR Webcam"
            };

            var checkInResult = _checkInService.ProcessCheckIn(checkInReq);
            Assert.IsTrue(checkInResult.IsSuccess, "Vé mới mua từ TicketOrderService phải được check-in thành công trên CheckInService.");
            Assert.AreEqual(newTicket.MaVe, checkInResult.Data!.MaVe);
        }

        #endregion
    }
}
