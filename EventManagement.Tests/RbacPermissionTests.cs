using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EventManagement.Core.Security;
using EventManagement.Core.Session;
using EventManagement.GUI.Common;
using EventManagement.GUI.Forms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Bộ kiểm thử hệ thống phân quyền động hai lớp RBAC (Task T1-17 / AD-4 - Ngày 8).
    /// Phụ trách: Thành viên 1 (Kiến trúc, CSDL & Bảo mật).
    /// </summary>
    [TestClass]
    public class RbacPermissionTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            CurrentSession.DangXuat();
        }

        [TestMethod]
        [Description("Kiểm tra CurrentSession hỗ trợ ánh xạ hai chiều giữa mã nghiệp vụ UC và mã màn hình Form.")]
        public void CurrentSession_DualMapping_SupportsBothUcAndFormCodes()
        {
            // Arrange
            CurrentSession.DangNhap("USR_TEST", "test_user", "Người Dùng Thử Nghiệm", "test@event.vn", "CUSTOM", new[] { "UC03", "F11" });

            // Act & Assert
            // Được cấp UC03 -> Phải có quyền cả UC03 và F03
            CurrentSession.CoQuyen("UC03").Should().BeTrue();
            CurrentSession.CoQuyen("F03").Should().BeTrue();

            // Được cấp F11 -> Phải có quyền cả F11 và UC11
            CurrentSession.CoQuyen("F11").Should().BeTrue();
            CurrentSession.CoQuyen("UC11").Should().BeTrue();

            // Không được cấp F20 / UC20
            CurrentSession.CoQuyen("UC20").Should().BeFalse();
            CurrentSession.CoQuyen("F20").Should().BeFalse();
        }
        // Mô tả: Test này đảm bảo tính tương thích cao của cơ chế phân quyền, cho phép tầng GUI và BLL kiểm tra quyền theo cả mã Use Case (UC03) lẫn mã Form (F03) mà không cần chuyển đổi thủ công.

        [TestMethod]
        [Description("Kiểm tra vai trò Quản trị viên (ADMIN) tự động có toàn bộ quyền trong hệ thống.")]
        public void CurrentSession_AdminRole_HasAllPermissions_Automatically()
        {
            // Arrange: Admin không cần nạp danh sách quyền cụ thể
            CurrentSession.DangNhap("USR_ADM", "admin", "Quản Trị Viên", "admin@event.vn", "ADMIN");

            // Act & Assert
            CurrentSession.IsAdmin.Should().BeTrue();
            CurrentSession.CoQuyen("UC01").Should().BeTrue();
            CurrentSession.CoQuyen("UC03").Should().BeTrue();
            CurrentSession.CoQuyen("UC20").Should().BeTrue();
            CurrentSession.CoQuyen("F19").Should().BeTrue();
            CurrentSession.CoQuyen("BAT_KY_QUYEN_MOI_NAO").Should().BeTrue();
        }
        // Mô tả: Test này xác thực quyền tối cao của Quản trị viên theo đặc tả AD-4: tài khoản nhóm ADMIN luôn luôn có thẩm quyền truy cập mọi phân hệ mà không bị chặn bởi bất kỳ bộ lọc phân quyền nào.

        [TestMethod]
        [Description("Kiểm tra phân quyền nhóm Quản lý sự kiện (MANAGER) có quyền nghiệp vụ nhưng không có quyền quản trị hệ thống.")]
        public void CurrentSession_ManagerRole_HasEventsAndReports_NotUsersOrRoles()
        {
            // Arrange: MANAGER theo seed data có hầu hết các quyền trừ UC03 và UC20
            var managerPerms = new[] { "UC01", "UC02", "UC04", "UC05", "UC06", "UC07", "UC08", "UC09", "UC10", "UC11", "UC12", "UC14", "UC15", "UC16", "UC17", "UC18", "UC19", "UC21" };
            CurrentSession.DangNhap("USR_MGR", "manager01", "Trần Quản Lý", "mgr@event.vn", "MANAGER", managerPerms);

            // Act & Assert
            CurrentSession.CoQuyen("UC04").Should().BeTrue(); // Danh sách sự kiện
            CurrentSession.CoQuyen("UC17").Should().BeTrue(); // Ngân sách & Chi phí
            CurrentSession.CoQuyen("UC19").Should().BeTrue(); // Báo cáo & Thống kê

            CurrentSession.CoQuyen("UC03").Should().BeFalse(); // Không được quản lý người dùng
            CurrentSession.CoQuyen("UC20").Should().BeFalse(); // Không được cấu hình phân quyền
        }
        // Mô tả: Test này đảm bảo ranh giới phân quyền giữa Quản lý và Quản trị viên: Quản lý chỉ được điều phối sự kiện và tài chính, tuyệt đối không được cấp quyền quản lý tài khoản người dùng hoặc chỉnh sửa ma trận quyền.

        [TestMethod]
        [Description("Kiểm tra phân quyền nhóm Nhân viên (STAFF) chỉ được tác nghiệp bán vé, soát vé và khảo sát.")]
        public void CurrentSession_StaffRole_HasTicketsAndCheckIn_NotBudgetOrAdmin()
        {
            // Arrange: STAFF theo seed data chỉ có UC01, UC02, UC10, UC11, UC13, UC18, UC21
            var staffPerms = new[] { "UC01", "UC02", "UC10", "UC11", "UC13", "UC18", "UC21" };
            CurrentSession.DangNhap("USR_STF", "staff01", "Lê Nhân Viên", "staff@event.vn", "STAFF", staffPerms);

            // Act & Assert
            CurrentSession.CoQuyen("UC11").Should().BeTrue(); // Bán vé
            CurrentSession.CoQuyen("UC13").Should().BeTrue(); // Soát vé QR
            CurrentSession.CoQuyen("UC21").Should().BeTrue(); // Đổi mật khẩu

            CurrentSession.CoQuyen("UC03").Should().BeFalse(); // Không quản lý user
            CurrentSession.CoQuyen("UC17").Should().BeFalse(); // Không xem chi phí/ngân sách
            CurrentSession.CoQuyen("UC20").Should().BeFalse(); // Không phân quyền
        }
        // Mô tả: Test này xác thực chính sách Least Privilege (Quyền hạn tối thiểu) đối với tài khoản Nhân viên tác nghiệp tại hiện trường, bảo vệ dữ liệu tài chính nhạy cảm và cấu hình hệ thống.

        [TestMethod]
        [Description("Kiểm tra toán tử OR (CoQuyenBatKy) trả về true khi thỏa mãn ít nhất một quyền.")]
        public void CurrentSession_CoQuyenBatKy_ReturnsTrueWhenAtLeastOneMatches()
        {
            // Arrange
            CurrentSession.DangNhap("USR01", "user01", "Tên", "u@e.vn", "STAFF", new[] { "UC11" });

            // Act & Assert
            CurrentSession.CoQuyenBatKy("UC03", "UC11", "UC20").Should().BeTrue();
            CurrentSession.CoQuyenBatKy("UC03", "UC20").Should().BeFalse();
        }
        // Mô tả: Test này kiểm tra toán tử kiểm tra quyền đa năng (OR) dùng cho các nút bấm hoặc thanh điều hướng có thể được truy cập bởi nhiều vai trò khác nhau.

        [TestMethod]
        [Description("Kiểm tra toán tử AND (CoDuQuyen) chỉ trả về true khi có đầy đủ tất cả các quyền.")]
        public void CurrentSession_CoDuQuyen_ReturnsTrueOnlyWhenAllMatch()
        {
            // Arrange
            CurrentSession.DangNhap("USR01", "user01", "Tên", "u@e.vn", "STAFF", new[] { "UC11", "UC13" });

            // Act & Assert
            CurrentSession.CoDuQuyen("UC11", "UC13").Should().BeTrue();
            CurrentSession.CoDuQuyen("UC11", "UC13", "UC17").Should().BeFalse();
        }
        // Mô tả: Test này thẩm định toán tử kiểm tra quyền liên hoàn (AND) bảo vệ các chức năng phức hợp yêu cầu thẩm quyền kết hợp.

        [TestMethod]
        [Description("Kiểm tra SecurityGuard (Lớp 2 BLL) cho phép thực thi khi tài khoản có đủ quyền hạn.")]
        public void SecurityGuard_YeuCauQuyen_WhenAuthorized_PassesCleanly()
        {
            // Arrange
            CurrentSession.DangNhap("USR01", "user01", "Nguyễn Văn A", "a@e.vn", "STAFF", new[] { "UC11" });

            // Act
            Action act = () => SecurityGuard.YeuCauQuyen("UC11");

            // Assert: Không ném exception
            act.Should().NotThrow();
        }
        // Mô tả: Test này đảm bảo cơ chế bảo vệ BLL (Lớp 2) hoạt động mượt mà và không gây gián đoạn khi người dùng hợp lệ thực hiện thao tác được cấp phép.

        [TestMethod]
        [Description("Kiểm tra SecurityGuard (Lớp 2 BLL) ném UnauthorizedAccessException khi tài khoản không có quyền.")]
        public void SecurityGuard_YeuCauQuyen_WhenUnauthorized_ThrowsUnauthorizedAccessException()
        {
            // Arrange: Tài khoản chỉ có UC11 (Bán vé), cố tình gọi nghiệp vụ UC03 (Quản lý User)
            CurrentSession.DangNhap("USR01", "staff01", "Nguyễn Văn A", "a@e.vn", "STAFF", new[] { "UC11" });

            // Act
            Action act = () => SecurityGuard.YeuCauQuyen("UC03");

            // Assert
            act.Should().Throw<UnauthorizedAccessException>()
                .WithMessage("*không có quyền 'UC03'*");
        }
        // Mô tả: Test này xác thực ranh giới bảo mật then chốt của Lớp 2 (BLL): Chặn đứng mọi hành vi truy cập trái phép ngay cả khi kẻ tấn công can thiệp vào tầng giao diện hoặc giả lập request.

        [TestMethod]
        [Description("Kiểm tra SecurityGuard ném UnauthorizedAccessException khi gọi nghiệp vụ trong trạng thái chưa đăng nhập.")]
        public void SecurityGuard_YeuCauQuyen_WhenNotLoggedIn_ThrowsUnauthorizedAccessException()
        {
            // Arrange: Đăng xuất hoàn toàn
            CurrentSession.DangXuat();

            // Act
            Action act = () => SecurityGuard.YeuCauQuyen("UC01");

            // Assert
            act.Should().Throw<UnauthorizedAccessException>()
                .WithMessage("*Phiên làm việc chưa được khởi tạo*");
        }
        // Mô tả: Test này đảm bảo không có bất kỳ lời gọi nghiệp vụ nào được phép vượt qua nếu tiến trình ứng dụng chưa được định danh danh tính qua phiên đăng nhập hợp lệ.

        [TestMethod]
        [Description("Kiểm tra SecurityGuard.YeuCauAdmin ném ngoại lệ khi tài khoản không phải vai trò ADMIN.")]
        public void SecurityGuard_YeuCauAdmin_WhenNotAdmin_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            CurrentSession.DangNhap("USR_MGR", "manager01", "Trần Quản Lý", "mgr@e.vn", "MANAGER", new[] { "UC04", "UC17" });

            // Act
            Action act = () => SecurityGuard.YeuCauAdmin();

            // Assert
            act.Should().Throw<UnauthorizedAccessException>()
                .WithMessage("*không có quyền quản trị tối cao (ADMIN)*");
        }
        // Mô tả: Test này kiểm tra phương thức bảo vệ độc quyền cho các chức năng đặc biệt nhạy cảm của Admin như xóa vai trò, mở khóa brute-force hoặc thay đổi thiết lập hệ thống.

        [TestMethod]
        [Description("Kiểm tra PermissionHelper (Lớp 1 GUI) ẩn điều khiển khi người dùng không có quyền hạn tương ứng.")]
        public void PermissionHelper_EvaluateControlPermission_HidesUnauthorizedControl()
        {
            // Arrange: Tài khoản Staff không có quyền UC03
            CurrentSession.DangNhap("USR_STF", "staff01", "Lê Staff", "staff@e.vn", "STAFF", new[] { "UC11" });

            var btn = new Button
            {
                Text = "Quản lý người dùng",
                Tag = "UC03",
                Visible = true
            };

            // Act: Rà soát phân quyền theo cơ chế ẩn
            bool isAllowed = PermissionHelper.EvaluateControlPermission(btn, hideInsteadOfDisable: true);

            // Assert
            isAllowed.Should().BeFalse();
            btn.Visible.Should().BeFalse();
        }
        // Mô tả: Test này thẩm định tầng giao diện Lớp 1 (GUI): Tự động quét thuộc tính Tag và ẩn triệt để các nút bấm người dùng không có thẩm quyền nhằm tối ưu trải nghiệm và bảo mật trực quan.

        [TestMethod]
        [Description("Kiểm tra PermissionHelper (Lớp 1 GUI) vô hiệu hóa nút bấm khi chọn chế độ hideInsteadOfDisable = false.")]
        public void PermissionHelper_EvaluateControlPermission_DisablesWhenHideIsFalse()
        {
            // Arrange: Tài khoản Staff không có quyền UC20
            CurrentSession.DangNhap("USR_STF", "staff01", "Lê Staff", "staff@e.vn", "STAFF", new[] { "UC11" });

            var btn = new Button
            {
                Text = "Lưu ma trận",
                Tag = "UC20",
                Enabled = true,
                Visible = true
            };

            // Act
            bool isAllowed = PermissionHelper.EvaluateControlPermission(btn, hideInsteadOfDisable: false);

            // Assert
            isAllowed.Should().BeFalse();
            btn.Enabled.Should().BeFalse();
            btn.Visible.Should().BeTrue(); // Không bị ẩn nhưng bị khóa nút
        }
        // Mô tả: Test này kiểm tra chế độ hiển thị vô hiệu hóa (disabled) của PermissionHelper, hữu ích cho các form quản lý nghiệp vụ cần thông báo cho người dùng biết nút bấm tồn tại nhưng không thể thao tác.

        [TestMethod]
        [Description("Kiểm tra PermissionHelper phân tích chính xác các định dạng Tag: tiền tố AUTH, toán tử OR và ROLE:ADMIN.")]
        public void PermissionHelper_IsPermissionSatisfied_SupportsVariousTagFormats()
        {
            // Arrange
            CurrentSession.DangNhap("USR01", "user01", "Nguyễn A", "a@e.vn", "MANAGER", new[] { "UC04", "UC17" });

            // Act & Assert
            PermissionHelper.IsPermissionSatisfied("UC04").Should().BeTrue();
            PermissionHelper.IsPermissionSatisfied("AUTH:UC04").Should().BeTrue();
            PermissionHelper.IsPermissionSatisfied("UC03,UC04").Should().BeTrue(); // OR: có UC04
            PermissionHelper.IsPermissionSatisfied("UC03,UC20").Should().BeFalse(); // OR: không có cả 2
            PermissionHelper.IsPermissionSatisfied("ROLE:ADMIN").Should().BeFalse(); // Đang là MANAGER
            PermissionHelper.IsPermissionSatisfied("ROLE:MANAGER").Should().BeTrue();
        }
        // Mô tả: Test này xác thực tính linh hoạt của parser cú pháp Tag trên giao diện, hỗ trợ đầy đủ các biểu thức logic phân quyền phong phú.

        [TestMethod]
        [Description("Kiểm tra FrmDashboard hiển thị đầy đủ toàn bộ các nút lối tắt tác nghiệp khi đăng nhập tài khoản ADMIN.")]
        public void FrmDashboard_ApplyPermissions_ShowsAllButtonsForAdmin()
        {
            // Arrange
            CurrentSession.DangNhap("USR_ADM", "admin", "Quản Trị Viên", "admin@e.vn", "ADMIN");

            using var form = new FrmDashboard();

            // Act
            form.ApplyUserSessionAndPermissions();

            // Assert
            form.CanAccessAdminFeatures.Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavEvents).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavTickets).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavCheckIn).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavTicketTiers).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavExpenses).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavReports).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavUsers).Should().BeTrue();
            form.IsButtonAuthorized(form.btnRefresh).Should().BeTrue();
        }
        // Mô tả: Test này đảm bảo màn hình trung tâm Dashboard F02 kích hoạt toàn bộ 8 lối tắt tác nghiệp và chức năng quản trị hệ thống cho tài khoản Admin.

        [TestMethod]
        [Description("Kiểm tra FrmDashboard ẩn các nút Quản trị (Người dùng, Phân quyền) và Chi phí khi đăng nhập tài khoản STAFF.")]
        public void FrmDashboard_ApplyPermissions_HidesAdminAndBudgetButtonsForStaff()
        {
            // Arrange: STAFF chỉ có Bán vé (UC11) và Soát vé (UC13)
            var staffPerms = new[] { "UC01", "UC02", "UC10", "UC11", "UC13", "UC18", "UC21" };
            CurrentSession.DangNhap("USR_STF", "staff01", "Lê Nhân Viên", "staff@e.vn", "STAFF", staffPerms);

            using var form = new FrmDashboard();

            // Act
            form.ApplyUserSessionAndPermissions();

            // Assert
            form.CanAccessAdminFeatures.Should().BeFalse();
            form.IsButtonAuthorized(form.btnNavTickets).Should().BeTrue();   // Bán vé F11
            form.IsButtonAuthorized(form.btnNavCheckIn).Should().BeTrue();   // Soát vé F13

            form.IsButtonAuthorized(form.btnNavUsers).Should().BeFalse();    // Ẩn Người dùng F03
            form.IsButtonAuthorized(form.btnNavExpenses).Should().BeFalse(); // Ẩn Ngân sách F17
        }
        // Mô tả: Test này thẩm định việc thực thi phân quyền động thực tế trên Form F02 ở Ngày 8: Nhân viên đăng nhập không bao giờ nhìn thấy các nút quản trị người dùng hoặc số liệu ngân sách nội bộ.

        [TestMethod]
        [Description("Kiểm tra FrmDashboard hiển thị sự kiện, bán vé, ngân sách, báo cáo nhưng ẩn người dùng & phân quyền cho vai trò MANAGER.")]
        public void FrmDashboard_ApplyPermissions_ShowsManagerButtonsExcludingUsersAndRoles()
        {
            // Arrange: MANAGER có sự kiện, chi phí, báo cáo nhưng không có UC03, UC20
            var managerPerms = new[] { "UC01", "UC02", "UC04", "UC08", "UC11", "UC12", "UC13", "UC17", "UC19", "UC21" };
            CurrentSession.DangNhap("USR_MGR", "mgr01", "Nguyễn Quản Lý", "mgr@e.vn", "MANAGER", managerPerms);

            using var form = new FrmDashboard();

            // Act
            form.ApplyUserSessionAndPermissions();

            // Assert
            form.IsButtonAuthorized(form.btnNavEvents).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavTickets).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavTicketTiers).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavExpenses).Should().BeTrue();
            form.IsButtonAuthorized(form.btnNavReports).Should().BeTrue();

            form.IsButtonAuthorized(form.btnNavUsers).Should().BeFalse();
        }
        // Mô tả: Test này đảm bảo màn hình Dashboard đáp ứng hoàn hảo ma trận phân quyền thực tế của doanh nghiệp: Quản lý xem được đầy đủ nghiệp vụ vận hành và tài chính nhưng không can thiệp vào tài khoản hệ thống.
    }
}
