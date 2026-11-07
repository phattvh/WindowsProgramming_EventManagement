using System;
using System.Collections.Generic;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Giao diện dịch vụ Báo cáo & Thống kê sự kiện (F19 - Task T3-21, T3-22, T3-23)
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Tạo danh sách vé điện tử kèm mảng byte ảnh QR để in mẫu vé (T3-21 / RptTicketInvoice.rdlc)
        /// </summary>
        List<TicketReceiptDto> GenerateTicketReceipts(string? maSuKien, string? maVe = null);

        /// <summary>
        /// Tạo báo cáo quyết toán tài chính sự kiện (T3-22 / RptFinancialSettlement.rdlc)
        /// </summary>
        SettlementDto GenerateFinancialSettlement(string? maSuKien, DateTime? tuNgay = null, DateTime? denNgay = null);

        /// <summary>
        /// Tạo chỉ số tổng hợp KPI sự kiện phục vụ thống kê tổng thể
        /// </summary>
        EventSummaryKpiDto GenerateEventSummary(string? maSuKien);

        /// <summary>
        /// Tạo danh sách chỉ số KPI cho tất cả sự kiện hiện có
        /// </summary>
        List<EventSummaryKpiDto> GenerateAllEventSummaries();

        /// <summary>
        /// Xuất báo cáo quyết toán tài chính ra file CSV (Dự phòng theo AD-5)
        /// </summary>
        string ExportSettlementToCsv(SettlementDto settlement, string? targetPath = null);

        /// <summary>
        /// Xuất danh sách vé điện tử ra file CSV
        /// </summary>
        string ExportTicketsToCsv(List<TicketReceiptDto> tickets, string? targetPath = null);

        /// <summary>
        /// Sinh mảng byte hình ảnh mã QR từ chuỗi token bằng ZXing.Net
        /// </summary>
        byte[] GenerateQrCodeImageBytes(string content, int width = 200, int height = 200);
    }
}
