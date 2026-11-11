using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement.BLL.Services;
using EventManagement.Core.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Bộ kiểm thử tự động cho Nghiệp vụ Phân công nhân sự theo ca trực (Task Ngày 6 - Thành viên 2)
    /// Đảm bảo các quy tắc:
    /// - BR-13: Chống trùng ca trực nhân sự theo khung giờ giao thoa nhau (Shift Overlap Detection).
    /// - Chặn phân công nhân sự đang ở trạng thái ngừng hoạt động / nghỉ việc.
    /// - CRUD Phân công ca trực Form F15.
    /// Phụ trách: Thành viên 2 (Giao diện UI/UX & Sự kiện)
    /// </summary>
    [TestClass]
    public class StaffAssignmentServiceTests
    {
        private IStaffAssignmentService _assignmentService = null!;
        private StaffScheduleService _shiftValidator = null!;
        private IStaffService _staffService = null!;

        [TestInitialize]
        public void Setup()
        {
            _shiftValidator = new StaffScheduleService();
            _staffService = new StaffService();
            _assignmentService = new StaffAssignmentService(_shiftValidator, _staffService);
        }

        // ==============================================================================
        // 1. KIỂM THỬ TRUY VẤN VÀ BỘ LỌC (GetAll & GetById)
        // ==============================================================================

        [TestMethod]
        public void GetAll_InitialSeedData_ReturnsAtLeast7Items()
        {
            var result = _assignmentService.GetAll();

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 7, $"Số lượng thực tế: {result.Data.Count}");
        }

        [TestMethod]
        public void GetAll_FilterByEvent_ReturnsCorrectItems()
        {
            var resEvent1 = _assignmentService.GetAll(maSuKien: "EVT202601");
            Assert.IsTrue(resEvent1.IsSuccess);
            Assert.IsNotNull(resEvent1.Data);
            Assert.IsTrue(resEvent1.Data.All(x => x.MaSuKien == "EVT202601"));
            Assert.AreEqual(5, resEvent1.Data.Count);

            var resEvent2 = _assignmentService.GetAll(maSuKien: "EVT202602");
            Assert.IsTrue(resEvent2.IsSuccess);
            Assert.IsNotNull(resEvent2.Data);
            Assert.IsTrue(resEvent2.Data.All(x => x.MaSuKien == "EVT202602"));
            Assert.AreEqual(2, resEvent2.Data.Count);
        }

        [TestMethod]
        public void GetAll_FilterByDepartment_ReturnsCorrectItems()
        {
            var resHauCan = _assignmentService.GetAll(phongBan: "Hậu cần");
            Assert.IsTrue(resHauCan.IsSuccess);
            Assert.IsNotNull(resHauCan.Data);
            Assert.IsTrue(resHauCan.Data.All(x => x.PhongBan == "Hậu cần"));
            Assert.IsTrue(resHauCan.Data.Count >= 2);
        }

        [TestMethod]
        public void GetAll_SearchKeyword_ReturnsMatchedItems()
        {
            var resKeyword = _assignmentService.GetAll(keyword: "Soát vé");
            Assert.IsTrue(resKeyword.IsSuccess);
            Assert.IsNotNull(resKeyword.Data);
            Assert.IsTrue(resKeyword.Data.Count >= 1);
            Assert.IsTrue(resKeyword.Data.Any(x => x.NhiemVu.Contains("Soát vé")));
        }

        [TestMethod]
        public void GetById_ExistingItem_ReturnsSuccessWithData()
        {
            var result = _assignmentService.GetById("PC001");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("PC001", result.Data.MaPhanCong);
            Assert.AreEqual("EVT202601", result.Data.MaSuKien);
            Assert.AreEqual("NS001", result.Data.MaNhanSu);
        }

        [TestMethod]
        public void GetById_NonExistingItem_ReturnsFailed()
        {
            var result = _assignmentService.GetById("PC999");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("Không tìm thấy"));
        }

        // ==============================================================================
        // 2. KIỂM THỬ THÊM MỚI, CẬP NHẬT & XÓA (CRUD)
        // ==============================================================================

        [TestMethod]
        public void Add_ValidAssignment_ReturnsSuccess()
        {
            var newAssignment = new StaffAssignmentDto
            {
                MaPhanCong = "PC099",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS002", // Lê Thanh Hằng - Trưởng ban Lễ tân
                NhiemVu = "Điều phối xe đưa đón khách VIP",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 6, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 8, 0, 0),
                GhiChu = "Đón tại cổng số 1"
            };

            var result = _assignmentService.Add(newAssignment);

            Assert.IsTrue(result.IsSuccess, result.Message);

            var queryRes = _assignmentService.GetById("PC099");
            Assert.IsTrue(queryRes.IsSuccess);
            Assert.AreEqual("Lê Thanh Hằng", queryRes.Data?.TenNhanSu);
            Assert.AreEqual("Lễ tân", queryRes.Data?.PhongBan);

            // Dọn dẹp
            _assignmentService.Delete("PC099");
        }

        [TestMethod]
        public void Add_EmptyFields_ReturnsFailed()
        {
            var emptyDto = new StaffAssignmentDto();
            var result = _assignmentService.Add(emptyDto);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("không được để trống"));
        }

        [TestMethod]
        public void Add_DuplicateCode_ReturnsFailed()
        {
            var duplicateDto = new StaffAssignmentDto
            {
                MaPhanCong = "PC001", // Đã tồn tại
                MaSuKien = "EVT202601",
                MaNhanSu = "NS003",
                NhiemVu = "Hỗ trợ test",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 8, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 10, 0, 0)
            };

            var result = _assignmentService.Add(duplicateDto);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("đã tồn tại"));
        }

        [TestMethod]
        public void Add_EndTimeBeforeStartTime_ReturnsFailed()
        {
            var invalidTimeDto = new StaffAssignmentDto
            {
                MaPhanCong = "PC098",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS003",
                NhiemVu = "Hỗ trợ test giờ sai",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 10, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 9, 0, 0) // Kết thúc trước bắt đầu
            };

            var result = _assignmentService.Add(invalidTimeDto);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("lớn hơn thời gian bắt đầu"));
        }

        // ==============================================================================
        // 3. THUẬT TOÁN BR-13: CHỐNG TRÙNG CA TRỰC NHÂN SỰ
        // ==============================================================================

        [TestMethod]
        public void BR13_SameStaff_OverlappingShift_ReturnsFailed()
        {
            // PC001: NS001 trực từ 07:00 đến 18:00 ngày 15/12/2026
            // Phân công ca mới cho NS001 từ 10:00 đến 14:00 -> Vi phạm BR-13
            var overlapDto = new StaffAssignmentDto
            {
                MaPhanCong = "PC097",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS001",
                NhiemVu = "Kiểm tra âm thanh buổi trưa",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 10, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 14, 0, 0)
            };

            var result = _assignmentService.Add(overlapDto);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("BR-13") || result.Message.Contains("Xung đột ca trực"));
        }

        [TestMethod]
        public void BR13_SameStaff_NonOverlappingShift_ReturnsSuccess()
        {
            // PC002: NS004 trực từ 07:30 đến 12:00 ngày 15/12/2026
            // Phân công ca mới cho NS004 vào buổi chiều: 13:00 đến 17:00 -> Không trùng -> Thành công
            var nonOverlapDto = new StaffAssignmentDto
            {
                MaPhanCong = "PC096",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS004",
                NhiemVu = "Trực quầy thông tin buổi chiều",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 13, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 17, 0, 0)
            };

            var result = _assignmentService.Add(nonOverlapDto);

            Assert.IsTrue(result.IsSuccess, result.Message);

            // Dọn dẹp
            _assignmentService.Delete("PC096");
        }

        [TestMethod]
        public void BR13_DifferentStaff_SameTime_ReturnsSuccess()
        {
            // Cùng khung giờ 07:00 - 18:00 nhưng gán cho NS003 (chưa có ca trùng) -> Hợp lệ
            var differentStaffDto = new StaffAssignmentDto
            {
                MaPhanCong = "PC095",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS003",
                NhiemVu = "Kỹ thuật viên màn hình LED",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 7, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 18, 0, 0)
            };

            var result = _assignmentService.Add(differentStaffDto);

            Assert.IsTrue(result.IsSuccess, result.Message);

            // Dọn dẹp
            _assignmentService.Delete("PC095");
        }

        [TestMethod]
        public void BR13_InactiveStaff_ReturnsFailed()
        {
            // Tạo nhân sự ở trạng thái Nghỉ việc
            var inactiveStaff = new StaffDto
            {
                MaNhanSu = "NS_INACTIVE",
                HoTen = "Nhân viên đã nghỉ việc",
                SoDienThoai = "0900000000",
                ChucDanh = "Cộng tác viên cũ",
                PhongBan = "Hậu cần",
                TrangThai = "Nghỉ việc"
            };
            _staffService.Add(inactiveStaff);

            var assignmentDto = new StaffAssignmentDto
            {
                MaPhanCong = "PC094",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS_INACTIVE",
                NhiemVu = "Trực cổng sau",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 8, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 12, 0, 0)
            };

            var result = _assignmentService.Add(assignmentDto);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Message.Contains("BR-13") || result.Message.Contains("ngừng hoạt động") || result.Message.Contains("Nghỉ việc"));
        }

        [TestMethod]
        public void CheckConflictMethod_IdentifiesConflictWithoutSaving()
        {
            // NS001 đã có ca trực PC001 từ 07:00 đến 18:00
            var testDto = new StaffAssignmentDto
            {
                MaPhanCong = "PC_TEST",
                MaSuKien = "EVT202601",
                MaNhanSu = "NS001",
                NhiemVu = "Thử nghiệm kiểm tra trùng ca",
                ThoiGianBatDau = new DateTime(2026, 12, 15, 8, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 15, 11, 0, 0)
            };

            var checkResult = _assignmentService.CheckConflict(testDto);

            Assert.IsFalse(checkResult.IsSuccess);
            Assert.IsTrue(checkResult.Message.Contains("BR-13") || checkResult.Message.Contains("Xung đột ca trực"));
        }

        [TestMethod]
        public void Update_ExcludeCurrentShiftId_DoesNotConflictWithItself()
        {
            // Lấy ca trực PC001
            var getRes = _assignmentService.GetById("PC001");
            Assert.IsTrue(getRes.IsSuccess);
            var item = getRes.Data!;

            // Cập nhật đổi nhiệm vụ, giữ nguyên khung giờ của chính nó
            item.NhiemVu = "Điều phối chung toàn bộ sự kiện (Cập nhật)";
            var updateRes = _assignmentService.Update(item);

            Assert.IsTrue(updateRes.IsSuccess, updateRes.Message);
        }

        [TestMethod]
        public void SuggestNextCode_GeneratesIncrementalCode()
        {
            string code = _assignmentService.SuggestNextCode();

            Assert.IsTrue(code.StartsWith("PC"));
            Assert.IsTrue(int.TryParse(code.Substring(2), out int num));
            Assert.IsTrue(num >= 8, $"Mã sinh ra phải từ PC008 trở lên, thực tế: {code}");
        }

        [TestMethod]
        public void StaffService_AddAndUpdate_WithPhoneLessThan10Digits_ReturnsFailed()
        {
            var addRes = _staffService.Add(new StaffDto
            {
                MaNhanSu = "NS_SHORT_PHONE",
                HoTen = "Test Nhan Su",
                SoDienThoai = "090123456" // 9 chữ số
            });
            Assert.IsFalse(addRes.IsSuccess);

            var existing = _staffService.GetById("NS001").Data!;
            var originalPhone = existing.SoDienThoai;
            try
            {
                var updateRes = _staffService.Update(new StaffDto
                {
                    MaNhanSu = existing.MaNhanSu,
                    HoTen = existing.HoTen,
                    SoDienThoai = "09012345", // 8 chữ số
                    Email = existing.Email,
                    ChucDanh = existing.ChucDanh,
                    PhongBan = existing.PhongBan,
                    TrangThai = existing.TrangThai
                });
                Assert.IsFalse(updateRes.IsSuccess);
            }
            finally
            {
                existing.SoDienThoai = originalPhone;
            }
        }
    }
}
