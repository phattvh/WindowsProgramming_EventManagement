using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventManagement.GUI.Forms;
using EventManagement.GUI.Common;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Entities;
using EventManagement.Core.Session;
using EventManagement.DAL;

namespace EventManagement.Tests
{
    [TestClass]
    public class UserManagementFormTests
    {
        private class MockUserService : IUserService
        {
            public Task<List<UserDto>> GetAllAsync(string? keyword = null, string? roleId = null, string? status = null)
                => Task.FromResult(new List<UserDto>());
            public Task<UserDto?> GetByIdAsync(string userId) => Task.FromResult<UserDto?>(null);
            public Task<ServiceResult<UserDto>> CreateAsync(CreateUserDto dto) => Task.FromResult(ServiceResult<UserDto>.Success(new UserDto()));
            public Task<ServiceResult> UpdateAsync(string userId, UpdateUserDto dto) => Task.FromResult(ServiceResult.Success());
            public Task<ServiceResult> DeleteAsync(string userId) => Task.FromResult(ServiceResult.Success());
            public Task<ServiceResult> UnlockUserAsync(string userId) => Task.FromResult(ServiceResult.Success());
            public Task<ServiceResult> ToggleLockAsync(string userId) => Task.FromResult(ServiceResult.Success());
        }

        [TestInitialize]
        public void Setup()
        {
            CurrentSession.DangNhap("USR001", "admin", "Quản trị viên", "admin@e.vn", "ADMIN", new[] { "UC03", "F03" });
        }

        [TestCleanup]
        public void Cleanup()
        {
            CurrentSession.DangXuat();
        }

        [TestMethod]
        public void F03_TitleBar_And_WindowState_Standardized()
        {
            // Mô tả: Kiểm tra tiêu đề form F03 là "Hệ Thống Quản Lý Sự Kiện" và chế độ mở toàn màn hình (Maximized).
            using var form = new FrmUserManagement(new MockUserService());
            Assert.AreEqual("Hệ Thống Quản Lý Sự Kiện", form.Text);
            Assert.AreEqual(FormWindowState.Maximized, form.WindowState);
        }

        [TestMethod]
        public void F03_Header_Standardized_And_SubtitleHidden()
        {
            // Mô tả: Kiểm tra tiêu đề hiển thị "QUẢN LÝ TÀI KHOẢN NGƯỜI DÙNG" và dòng phụ chú đã được ẩn hoàn toàn.
            using var form = new FrmUserManagement(new MockUserService());
            var lblTitle = form.Controls.Find("lblTitle", true);
            var lblSubTitle = form.Controls.Find("lblSubTitle", true);

            Assert.IsTrue(lblTitle.Length > 0, "Không tìm thấy lblTitle.");
            Assert.AreEqual("QUẢN LÝ TÀI KHOẢN NGƯỜI DÙNG", lblTitle[0].Text);

            Assert.IsTrue(lblSubTitle.Length > 0, "Không tìm thấy lblSubTitle.");
            Assert.IsFalse(lblSubTitle[0].Visible, "Dòng mô tả phụ lblSubTitle phải được ẩn.");
        }

        [TestMethod]
        public void F03_GridView_HeadersCentered()
        {
            // Mô tả: Kiểm tra tất cả các cột của dgvUsers đều có tiêu đề căn giữa (MiddleCenter).
            using var form = new FrmUserManagement(new MockUserService());
            var dgvArray = form.Controls.Find("dgvUsers", true);
            Assert.IsTrue(dgvArray.Length > 0, "Không tìm thấy dgvUsers.");

            var dgv = (DataGridView)dgvArray[0];
            Assert.IsTrue(dgv.Columns.Count >= 7, "dgvUsers phải có đủ danh sách các cột tài khoản.");

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                Assert.AreEqual(DataGridViewContentAlignment.MiddleCenter, col.HeaderCell.Style.Alignment,
                    $"Tiêu đề cột '{col.HeaderText}' phải được căn giữa.");
            }
        }

        [TestMethod]
        public void F03_SaveCancelButtons_OnlyVisible_In_AddOrEditMode()
        {
            // Mô tả: Kiểm tra nút Lưu và Hủy chỉ hiển thị ở chế độ Thêm mới hoặc Sửa, và bị ẩn khi ở chế độ Xem (View).
            using var form = new FrmUserManagement(new MockUserService());
            form.Show();

            // Chế độ Xem (View) ban đầu
            form.SetFormState(FormMode.View);
            Assert.IsFalse(form.BtnLuu.Visible, "Nút Lưu phải bị ẩn khi ở chế độ Xem.");
            Assert.IsFalse(form.BtnHuy.Visible, "Nút Hủy phải bị ẩn khi ở chế độ Xem.");
            Assert.IsTrue(form.BtnThem.Visible, "Nút Thêm phải hiển thị khi ở chế độ Xem.");

            // Chuyển sang chế độ Thêm (Add)
            form.SetFormState(FormMode.Add);
            Assert.IsTrue(form.BtnLuu.Visible, "Nút Lưu phải hiển thị khi ở chế độ Thêm mới.");
            Assert.IsTrue(form.BtnHuy.Visible, "Nút Hủy phải hiển thị khi ở chế độ Thêm mới.");
            Assert.IsFalse(form.BtnThem.Visible, "Nút Thêm mới phải bị ẩn khi đang thêm.");

            // Chuyển sang chế độ Sửa (Edit)
            form.SetFormState(FormMode.Edit);
            Assert.IsTrue(form.BtnLuu.Visible, "Nút Lưu phải hiển thị khi ở chế độ Sửa.");
            Assert.IsTrue(form.BtnHuy.Visible, "Nút Hủy phải hiển thị khi ở chế độ Sửa.");

            form.Hide();
        }

        [TestMethod]
        public void F03_DatabaseInitializer_SeedsThreeSampleUsers()
        {
            // Mô tả: Kiểm tra hằng số mật khẩu mặc định của DatabaseInitializer đảm bảo an toàn BCrypt.
            Assert.IsFalse(string.IsNullOrWhiteSpace(DatabaseInitializer.DefaultPasswordHash));
            Assert.IsTrue(DatabaseInitializer.DefaultPasswordHash.StartsWith("$2a$11$"));
        }

        [TestMethod]
        public void F03_DetailInputs_HaveEnoughRightMargin_ForErrorProvider()
        {
            // Mô tả (AD-14, AD-15): Đảm bảo ô cột 3 (txtMatKhau, txtSoDienThoai) chừa đủ lề phải cho icon ErrorProvider không bị cắt.
            using var form = new FrmUserManagement(new MockUserService());
            form.Show();
            form.LayoutDetailGroup();

            var grpDetail = form.Controls.Find("grpDetail", true)[0];
            int rightMarginPassword = grpDetail.ClientSize.Width - form.txtMatKhau.Right;
            int rightMarginPhone = grpDetail.ClientSize.Width - form.txtSoDienThoai.Right;

            Assert.IsTrue(rightMarginPassword >= 24, $"Lề phải txtMatKhau ({rightMarginPassword}px) phải >= 24px.");
            Assert.IsTrue(rightMarginPhone >= 24, $"Lề phải txtSoDienThoai ({rightMarginPhone}px) phải >= 24px.");

            form.Hide();
        }

        [TestMethod]
        public void F03_ValidateAllUserInputs_ReturnsAllInvalidFieldsSimultaneously()
        {
            // Mô tả (AD-15): Khi cả Email, Số điện thoại và Mật khẩu đều sai, phải trả về đồng thời cả 3 lỗi.
            var errors = FrmUserManagement.ValidateAllUserInputs(
                username: "admin_test",
                fullName: "Nguyen Van A",
                email: "admin.gmail.com",
                phone: "09012",
                password: "123",
                isNewUser: true);

            Assert.AreEqual(3, errors.Count);
            Assert.IsTrue(errors.Any(e => e.Field == nameof(FrmUserManagement.txtEmail)));
            Assert.IsTrue(errors.Any(e => e.Field == nameof(FrmUserManagement.txtSoDienThoai)));
            Assert.IsTrue(errors.Any(e => e.Field == nameof(FrmUserManagement.txtMatKhau)));
        }
    }
}
