using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
using EventManagement.GUI.Forms;

namespace EventManagement.Tests
{
    /// <summary>
    /// Bộ kiểm thử toàn diện cho các tính năng của Thành viên 3 trong Ngày 8:
    /// - Task T3-21: Mẫu in vé điện tử kèm mã QR (VeDienTu.rdlc / RptTicketInvoice.rdlc)
    /// - Task T3-22: Mẫu in báo cáo quyết toán tài chính (BaoCaoQuyetToan.rdlc / RptFinancialSettlement.rdlc)
    /// - Task T3-23: Màn hình F19 Báo cáo & Thống kê tổng hợp sự kiện (FrmReportStatistics)
    /// - Kiến trúc AD-5: Tách biệt DTOs, sinh ảnh QR độc lập nền tảng, xuất CSV dự phòng
    /// </summary>
    [TestClass]
    public class Member3Day8ReportTests
    {
        private IReportService _reportService = null!;

        [TestInitialize]
        public void Setup()
        {
            _reportService = new ReportService();
        }

        #region --- 1. KIỂM THỬ TASK T3-21: VÉ ĐIỆN TỬ & SINH ẢNH MÃ QR ---

        [TestMethod]
        public void T321_GenerateQrCodeImageBytes_ValidContent_ReturnsValidBmpBytes()
        {
            // Kiểm tra sinh mảng byte ảnh QR trực tiếp từ chuỗi
            string qrContent = "TICKET:EVT202601:TK0001:VERIFY";
            byte[] bytes = _reportService.GenerateQrCodeImageBytes(qrContent, 200, 200);

            Assert.IsNotNull(bytes, "Mảng byte ảnh QR không được null.");
            Assert.IsTrue(bytes.Length > 54, "File BMP phải lớn hơn kích thước Header chuẩn 54 byte.");

            // Kiểm tra chữ ký file BMP (0x42 'B', 0x4D 'M')
            Assert.AreEqual((byte)'B', bytes[0], "Header byte 0 của ảnh BMP phải là 'B'.");
            Assert.AreEqual((byte)'M', bytes[1], "Header byte 1 của ảnh BMP phải là 'M'.");
        }

        [TestMethod]
        public void T321_GenerateTicketReceipts_ReturnsReceiptsWithEmbeddedQrCode()
        {
            // Kiểm tra nạp danh sách vé điện tử kèm mảng byte QR
            var receipts = _reportService.GenerateTicketReceipts(null);

            Assert.IsNotNull(receipts);
            Assert.IsTrue(receipts.Count > 0, "Phải có danh sách vé điện tử mẫu được tạo ra.");

            var first = receipts.First();
            Assert.IsFalse(string.IsNullOrWhiteSpace(first.MaVe), "Mã vé không được để trống.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(first.MaQR), "Mã token QR không được để trống.");
            Assert.IsNotNull(first.QRCodeImageBytes, "Mảng byte ảnh QR nhúng vào RDLC không được null.");
            Assert.IsTrue(first.QRCodeImageBytes.Length > 54, "Ảnh QR phải là định dạng BMP hợp lệ.");
        }

        [TestMethod]
        public void T321_GenerateTicketReceipts_FilterByMaVe_ReturnsMatchingReceipt()
        {
            var receipts = _reportService.GenerateTicketReceipts(null);
            var sample = receipts.First();

            var filtered = _reportService.GenerateTicketReceipts(null, sample.MaVe);
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual(sample.MaVe, filtered[0].MaVe);
        }

        [TestMethod]
        public void T321_ExportTicketsToCsv_GeneratesValidCsvStringWithUtf8Bom()
        {
            var receipts = _reportService.GenerateTicketReceipts(null);
            string filePath = _reportService.ExportTicketsToCsv(receipts);

            Assert.IsNotNull(filePath);
            Assert.IsTrue(File.Exists(filePath), "File CSV phải tồn tại trên đĩa.");
            string csv = File.ReadAllText(filePath);
            Assert.IsTrue(csv.StartsWith("\uFEFF"), "File CSV phải chứa ký tự UTF-8 BOM để Excel hiển thị đúng dấu tiếng Việt.");
            Assert.IsTrue(csv.Contains("DANH SÁCH VÉ ĐIỆN TỬ SỰ KIỆN"), "File CSV phải có tiêu đề báo cáo.");
            Assert.IsTrue(csv.Contains("Mã vé;Mã đăng ký;Người tham dự;Hạng vé;Đơn giá (VNĐ)"), "File CSV phải có header cột.");
        }
        #endregion

        #region --- 2. KIỂM THỬ TASK T3-22: QUYẾT TOÁN TÀI CHÍNH & CÂN ĐỐI THU CHI ---

        [TestMethod]
        public void T322_GenerateFinancialSettlement_CalculatesTotalRevenueAndSurplusCorrectly()
        {
            var settlement = _reportService.GenerateFinancialSettlement("EVT202601");

            Assert.IsNotNull(settlement);
            Assert.AreEqual("EVT202601", settlement.MaSuKien);
            Assert.IsTrue(settlement.TongThu == settlement.TongDoanhThuBanVe + settlement.TongTienTaiTro,
                "Tổng nguồn thu (TongThu) phải bằng Doanh thu vé + Tiền tài trợ.");
            Assert.IsTrue(settlement.ChenhLechThuChi == settlement.TongThu - settlement.TongThucChi,
                "Chênh lệch thu chi phải bằng Tổng thu - Tổng thực chi.");

            if (settlement.ChenhLechThuChi >= 0)
            {
                Assert.AreEqual("Thặng dư tài chính", settlement.DanhGiaQuyetToan);
            }
            else
            {
                Assert.AreEqual("Thâm hụt tài chính", settlement.DanhGiaQuyetToan);
            }
        }

        [TestMethod]
        public void T322_GenerateFinancialSettlement_DateFilter_FiltersExpensesProperly()
        {
            DateTime tuNgay = new DateTime(2026, 11, 1);
            DateTime denNgay = new DateTime(2026, 11, 30);

            var settlement = _reportService.GenerateFinancialSettlement("EVT202601", tuNgay, denNgay);

            Assert.IsNotNull(settlement);
            foreach (var expense in settlement.DanhSachChiPhi)
            {
                Assert.IsTrue(expense.NgayChi.Date >= tuNgay.Date && expense.NgayChi.Date <= denNgay.Date,
                    $"Khoản chi ngày {expense.NgayChi:dd/MM/yyyy} phải nằm trong khoảng lọc {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}.");
            }
        }

        [TestMethod]
        public void T322_ExportSettlementToCsv_GeneratesValidCsvStringWithUtf8Bom()
        {
            var settlement = _reportService.GenerateFinancialSettlement("EVT202601");
            string filePath = _reportService.ExportSettlementToCsv(settlement);

            Assert.IsNotNull(filePath);
            Assert.IsTrue(File.Exists(filePath), "File CSV phải tồn tại trên đĩa.");
            string csv = File.ReadAllText(filePath);
            Assert.IsTrue(csv.StartsWith("\uFEFF"), "File CSV phải chứa UTF-8 BOM.");
            Assert.IsTrue(csv.Contains("BÁO CÁO QUYẾT TOÁN TÀI CHÍNH SỰ KIỆN"), "File CSV phải chứa tiêu đề quyết toán.");
            Assert.IsTrue(csv.Contains("I. TỔNG HỢP NGUỒN THU"), "File CSV phải chứa Mục I tổng hợp nguồn thu.");
            Assert.IsTrue(csv.Contains("II. CHI TIẾT CÁC KHOẢN CHI THỰC TẾ"), "File CSV phải chứa Mục II chi tiết chi phí.");
            Assert.IsTrue(csv.Contains("III. CÂN ĐỐI TÀI CHÍNH & KẾT LUẬN"), "File CSV phải chứa Mục III cân đối kết luận.");
        }
        #endregion

        #region --- 3. KIỂM THỬ TASK T3-23: THỐNG KÊ KPI & FORM F19 ---

        [TestMethod]
        public void T323_GenerateEventSummary_CalculatesKpiCorrectly()
        {
            var kpi = _reportService.GenerateEventSummary("EVT202601");

            Assert.IsNotNull(kpi);
            Assert.AreEqual("EVT202601", kpi.MaSuKien);
            Assert.IsFalse(string.IsNullOrWhiteSpace(kpi.TenSuKien));
            Assert.IsTrue(kpi.TongVePhatHanh >= 0);
            Assert.IsTrue(kpi.TongVeDaBan >= 0);
            Assert.IsTrue(kpi.TongLuotCheckIn >= 0);

            // Kiểm tra các thuộc tính tính toán tỷ lệ
            Assert.IsTrue(kpi.TyLeCheckIn >= 0.0 && kpi.TyLeCheckIn <= 100.0);
            Assert.IsTrue(kpi.TyLeBanVe >= 0.0 && kpi.TyLeBanVe <= 100.0);
            Assert.IsTrue(kpi.TyLeThamDu >= 0.0 && kpi.TyLeThamDu <= 100.0);
        }

        [TestMethod]
        public void T323_GenerateAllEventSummaries_ReturnsAllEvents()
        {
            var list = _reportService.GenerateAllEventSummaries();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0, "Phải có danh sách KPI các sự kiện.");
            foreach (var item in list)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(item.MaSuKien));
                Assert.IsFalse(string.IsNullOrWhiteSpace(item.TenSuKien));
            }
        }

        [TestMethod]
        public void T323_FrmReportStatistics_CanBeInstantiatedHeadless()
        {
            // Kiểm tra khởi tạo màn hình F19 không phụ thuộc UI, không gây văng Exception
            using var form = new FrmReportStatistics(_reportService);
            Assert.IsNotNull(form);
            Assert.AreEqual("F19 - Báo Cáo & Thống Kê Tổng Hợp Sự Kiện", form.Text);
            Assert.IsNotNull(form.Font);
            Assert.AreEqual("Segoe UI", form.Font.FontFamily.Name);
        }

        [TestMethod]
        public void T321_T322_RdlcFiles_ExistInSolution()
        {
            // Kiểm tra sự tồn tại của các file mẫu in RDLC phục vụ Visual Studio ReportViewer
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            // Tìm thư mục Reports trong output hoặc source
            string rdlcPath1 = Path.Combine(baseDir, "Reports", "VeDienTu.rdlc");
            string rdlcPath2 = Path.Combine(baseDir, "Reports", "BaoCaoQuyetToan.rdlc");

            // Nếu chạy test từ runner thư mục con
            if (!File.Exists(rdlcPath1))
            {
                rdlcPath1 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\EventManagement.GUI\Reports\VeDienTu.rdlc"));
            }
            if (!File.Exists(rdlcPath2))
            {
                rdlcPath2 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\EventManagement.GUI\Reports\BaoCaoQuyetToan.rdlc"));
            }

            Assert.IsTrue(File.Exists(rdlcPath1), $"File VeDienTu.rdlc phải tồn tại tại: {rdlcPath1}");
            Assert.IsTrue(File.Exists(rdlcPath2), $"File BaoCaoQuyetToan.rdlc phải tồn tại tại: {rdlcPath2}");
        }
        #endregion
    }
}
