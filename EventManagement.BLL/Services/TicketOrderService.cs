using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;

namespace EventManagement.BLL.Services
{
    /// <summary>
    /// Service xử lý nghiệp vụ Đăng ký & Bán vé sự kiện (F11 / UC11)
    /// Đảm trách quy tắc:
    /// - BR-09: Kiểm soát tồn kho vé khả dụng (PhatHanh - DaBan >= SoLuongMua)
    /// - BR-10: Kiểm soát giới hạn mua vé (1 đến 10 vé/đơn), phân rã Master-Detail
    /// - BR-17: Tự động sinh mã vé và mã QR dạng GUID duy nhất
    /// - BR-18: Giao dịch an toàn (ACID Transaction)
    /// Phụ trách: Thành viên 3 (Thuật toán lõi & BLL) - Task T3-17 & T3-18 (Ngày 6)
    /// </summary>
    public class TicketOrderService : ITicketOrderService
    {
        // 1. Danh sách sự kiện mở bán vé
        private static readonly List<(string MaSuKien, string TenSuKien)> _events = new()
        {
            ("EVT202601", "Hội nghị Công nghệ Vietnam Tech Summit 2026"),
            ("EVT202602", "Workshop Thực chiến GenAI & Prompt Engineering 2026"),
            ("EVT202603", "Hội nghị Khoa học Quốc tế về Công nghệ Phần mềm & IoT")
        };

        // 2. Danh sách các hạng vé mở bán (BR-08, BR-09)
        private static readonly List<TicketTierSummaryDto> _ticketTiers = new()
        {
            new TicketTierSummaryDto
            {
                MaHangVe = "HV001",
                MaSuKien = "EVT202601",
                TenHangVe = "Vé Tiêu chuẩn (Standard)",
                GiaVe = 200000m,
                SoLuongPhatHanh = 800,
                SoLuongDaBan = 150
            },
            new TicketTierSummaryDto
            {
                MaHangVe = "HV002",
                MaSuKien = "EVT202601",
                TenHangVe = "Vé VIP (Full-access)",
                GiaVe = 800000m,
                SoLuongPhatHanh = 200,
                SoLuongDaBan = 50
            },
            new TicketTierSummaryDto
            {
                MaHangVe = "HV003",
                MaSuKien = "EVT202602",
                TenHangVe = "Vé Workshop Tiêu chuẩn",
                GiaVe = 500000m,
                SoLuongPhatHanh = 100,
                SoLuongDaBan = 98 // Chỉ còn đúng 2 vé khả dụng để test BR-09
            },
            new TicketTierSummaryDto
            {
                MaHangVe = "HV004",
                MaSuKien = "EVT202603",
                TenHangVe = "Vé Đại biểu Khoa học",
                GiaVe = 150000m,
                SoLuongPhatHanh = 300,
                SoLuongDaBan = 20
            }
        };

        // 3. Danh sách vé con đã phát hành (ChiTietVe) kèm mã QR token
        // Khởi tạo sẵn 3 mã vé mẫu từ scripts/seed_data_member3.sql
        private static readonly List<TicketDetailDto> _issuedTickets = new()
        {
            new TicketDetailDto
            {
                MaVe = "VE00001",
                MaDangKy = "REG001",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                TenHangVe = "Vé VIP (Full-access)",
                GiaVe = 800000m,
                TenNguoiThamGia = "Vũ Thị Mai",
                Email = "mai.vu@techcorp.vn",
                MaQR = "EVT2-QR-MAI-002",
                TrangThaiVe = "Hợp lệ",
                NgayDangKy = new DateTime(2026, 11, 2)
            },
            new TicketDetailDto
            {
                MaVe = "VE00002",
                MaDangKy = "REG002",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                TenHangVe = "Vé Tiêu chuẩn (Standard)",
                GiaVe = 200000m,
                TenNguoiThamGia = "Phạm Nhật Long",
                Email = "long.pham@viettech.vn",
                MaQR = "EVT2-QR-LONG-003",
                TrangThaiVe = "Hợp lệ",
                NgayDangKy = new DateTime(2026, 11, 3)
            },
            new TicketDetailDto
            {
                MaVe = "VE00003",
                MaDangKy = "REG003",
                MaSuKien = "EVT202601",
                TenSuKien = "Hội nghị Công nghệ Vietnam Tech Summit 2026",
                TenHangVe = "Vé Tiêu chuẩn (Standard)",
                GiaVe = 200000m,
                TenNguoiThamGia = "Nguyễn Ngọc Bích",
                Email = "bich.nguyen@fintech.vn",
                MaQR = "EVT2-QR-BICH-004",
                TrangThaiVe = "Hợp lệ",
                NgayDangKy = new DateTime(2026, 11, 4)
            }
        };

        private static int _orderCounter = 100;
        private static int _ticketCounter = 1000;
        private static readonly object _syncLock = new();

        #region Public Methods

        public List<(string MaSuKien, string TenSuKien)> GetAvailableEvents()
        {
            return _events.ToList();
        }

        public List<TicketTierSummaryDto> GetTicketTiersByEvent(string maSuKien)
        {
            if (string.IsNullOrWhiteSpace(maSuKien))
                return _ticketTiers.ToList();

            var list = _ticketTiers
                .Where(t => string.Equals(t.MaSuKien, maSuKien.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (list.Count == 0)
            {
                var fallbackTier = new TicketTierSummaryDto
                {
                    MaHangVe = "HV-" + maSuKien.Trim(),
                    MaSuKien = maSuKien.Trim(),
                    TenHangVe = "Vé Tiêu chuẩn (Standard)",
                    GiaVe = 250000m,
                    SoLuongPhatHanh = 500,
                    SoLuongDaBan = 0
                };
                lock (_syncLock)
                {
                    _ticketTiers.Add(fallbackTier);
                }
                list.Add(fallbackTier);
            }

            return list;
        }

        public ServiceResult<List<TicketDetailDto>> GetTicketsByEvent(string? maSuKien)
        {
            if (string.IsNullOrWhiteSpace(maSuKien))
                return ServiceResult<List<TicketDetailDto>>.Success(_issuedTickets.ToList());

            var trimmed = maSuKien.Trim();
            var list = _issuedTickets
                .Where(t => string.Equals(t.MaSuKien, trimmed, StringComparison.OrdinalIgnoreCase)
                    || t.TenSuKien.Contains(trimmed, StringComparison.OrdinalIgnoreCase)
                    || t.MaDangKy.Contains(trimmed, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(t => t.NgayDangKy)
                .ToList();

            return ServiceResult<List<TicketDetailDto>>.Success(list);
        }

        public ServiceResult<TicketDetailDto> GetTicketByCode(string codeOrQr)
        {
            if (string.IsNullOrWhiteSpace(codeOrQr))
                return ServiceResult<TicketDetailDto>.Failed("Mã vé hoặc mã QR không được để trống.");

            var trimmed = codeOrQr.Trim();
            var ticket = _issuedTickets.FirstOrDefault(t =>
                string.Equals(t.MaVe, trimmed, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(t.MaQR, trimmed, StringComparison.OrdinalIgnoreCase));

            if (ticket == null)
                return ServiceResult<TicketDetailDto>.Failed($"Không tìm thấy vé tương ứng với mã '{trimmed}'.");

            return ServiceResult<TicketDetailDto>.Success(ticket);
        }

        /// <summary>
        /// Nghiệp vụ Đăng ký & Bán vé (BR-09, BR-10, BR-17, BR-18)
        /// </summary>
        public ServiceResult<TicketOrderResultDto> RegisterTicketOrder(RegisterTicketRequestDto request)
        {
            if (request == null)
                return ServiceResult<TicketOrderResultDto>.Failed("Yêu cầu đặt vé không được để trống.");

            // 1. Thẩm định số lượng vé (BR-10: 1 đến 10 vé)
            if (request.SoLuongMua < 1 || request.SoLuongMua > 10)
            {
                return ServiceResult<TicketOrderResultDto>.Failed(
                    "Quy tắc BR-10: Mỗi đơn hàng chỉ cho phép mua từ 1 đến 10 vé.",
                    new List<string> { $"Số lượng vé yêu cầu: {request.SoLuongMua} (hợp lệ: 1 - 10)" }
                );
            }

            // 2. Thẩm định thông tin người mua
            if (string.IsNullOrWhiteSpace(request.HoTenNguoiMua))
            {
                return ServiceResult<TicketOrderResultDto>.Failed("Họ tên người mua vé không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(request.SoDienThoai))
            {
                return ServiceResult<TicketOrderResultDto>.Failed("Số điện thoại liên hệ không được để trống.");
            }

            string contactEmail = string.IsNullOrWhiteSpace(request.Email) 
                ? $"{request.SoDienThoai.Trim()}@event.vn" 
                : request.Email.Trim();

            // 3. Khóa giao dịch kiểm soát tồn kho khả dụng (BR-09 & BR-18 Transaction)
            lock (_syncLock)
            {
                var tier = _ticketTiers.FirstOrDefault(t => string.Equals(t.MaHangVe, request.MaHangVe.Trim(), StringComparison.OrdinalIgnoreCase));
                if (tier == null)
                {
                    return ServiceResult<TicketOrderResultDto>.Failed($"Không tìm thấy hạng vé có mã '{request.MaHangVe}'.");
                }

                int available = tier.SoLuongConLai;
                if (request.SoLuongMua > available)
                {
                    return ServiceResult<TicketOrderResultDto>.Failed(
                        $"Quy tắc BR-09: Kho vé khả dụng không đủ đáp ứng số lượng yêu cầu.",
                        new List<string>
                        {
                            $"Hạng vé: {tier.TenHangVe}",
                            $"Số vé còn lại trong kho: {available} vé",
                            $"Số vé yêu cầu mua: {request.SoLuongMua} vé"
                        }
                    );
                }

                // 4. Sinh mã đơn hàng và danh sách vé con (BR-17)
                string orderCode = $"REG2026_{++_orderCounter:D4}";
                decimal unitPrice = tier.GiaVe;
                decimal totalAmount = request.SoLuongMua * unitPrice;

                var evt = _events.FirstOrDefault(e => string.Equals(e.MaSuKien, request.MaSuKien.Trim(), StringComparison.OrdinalIgnoreCase));
                string eventName = evt != default ? evt.TenSuKien : request.MaSuKien;

                var createdTickets = new List<TicketDetailDto>();

                for (int i = 0; i < request.SoLuongMua; i++)
                {
                    string ticketCode = $"VE{++_ticketCounter:D5}";
                    // Mã QR dạng GUID duy nhất (BR-17)
                    string qrCode = $"EVT-QR-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";

                    string attendeeName = request.HoTenNguoiMua;
                    if (i > 0 && request.DanhSachNguoiDiCung != null && request.DanhSachNguoiDiCung.Count >= i && !string.IsNullOrWhiteSpace(request.DanhSachNguoiDiCung[i - 1]))
                    {
                        attendeeName = request.DanhSachNguoiDiCung[i - 1].Trim();
                    }
                    else if (i > 0)
                    {
                        attendeeName = $"{request.HoTenNguoiMua} (Người đi cùng {i})";
                    }

                    var ticketDto = new TicketDetailDto
                    {
                        MaVe = ticketCode,
                        MaDangKy = orderCode,
                        MaSuKien = request.MaSuKien.Trim(),
                        TenSuKien = eventName,
                        TenHangVe = tier.TenHangVe,
                        GiaVe = unitPrice,
                        TenNguoiThamGia = attendeeName,
                        Email = request.Email.Trim(),
                        MaQR = qrCode,
                        TrangThaiVe = "Hợp lệ",
                        NgayDangKy = DateTime.Now
                    };

                    createdTickets.Add(ticketDto);
                }

                // 5. Cập nhật số lượng đã bán (BR-09) và lưu vào danh sách hệ thống
                tier.SoLuongDaBan += request.SoLuongMua;
                _issuedTickets.AddRange(createdTickets);

                var orderResult = new TicketOrderResultDto
                {
                    MaDangKy = orderCode,
                    MaSuKien = request.MaSuKien,
                    TenSuKien = eventName,
                    MaHangVe = tier.MaHangVe,
                    TenHangVe = tier.TenHangVe,
                    DonGia = unitPrice,
                    SoLuong = request.SoLuongMua,
                    TongTien = totalAmount,
                    NgayDangKy = DateTime.Now,
                    TrangThaiThanhToan = "Đã thanh toán",
                    HoTenNguoiMua = request.HoTenNguoiMua.Trim(),
                    SoDienThoai = request.SoDienThoai.Trim(),
                    Email = contactEmail,
                    DanhSachVe = createdTickets
                };

                EventManagement.Core.Common.DataSyncNotifier.NotifyTicketSold(request.MaSuKien, request.SoLuongMua, totalAmount);
                return ServiceResult<TicketOrderResultDto>.Success(
                    orderResult,
                    $"Đăng ký đơn hàng thành công! Đã phát hành {request.SoLuongMua} vé điện tử với mã QR độc lập."
                );
            }
        }

        /// <summary>
        /// Lấy tham chiếu danh sách vé phục vụ liên thông kiểm tra điểm danh (CheckInService)
        /// </summary>
        public static List<TicketDetailDto> GetIssuedTicketsList()
        {
            lock (_syncLock)
            {
                return _issuedTickets;
            }
        }

        #endregion
    }
}
