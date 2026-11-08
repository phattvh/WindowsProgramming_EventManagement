using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using EventManagement.GUI.Forms;
using EventManagement.GUI.Common;
using EventManagement.BLL.Services;
using EventManagement.Core.Common;
using EventManagement.Core.DTOs;
using EventManagement.Core.Session;

namespace EventManagement.Tests
{
    [TestClass]
    public class RolePermissionFormTests
    {
        private class MockRolePermissionService : IRolePermissionService
        {
            public Task<List<RoleDto>> GetAllRolesAsync()
            {
                return Task.FromResult(new List<RoleDto>
                {
                    new RoleDto { MaNhomQuyen = "ADMIN", TenNhomQuyen = "Quản trị viên" },
                    new RoleDto { MaNhomQuyen = "STAFF", TenNhomQuyen = "Nhân viên hỗ trợ" }
                });
            }

            public Task<List<ChucNangDto>> GetAllFunctionsAsync()
            {
                return Task.FromResult(new List<ChucNangDto>
                {
                    new ChucNangDto { MaChucNang = "UC01", TenChucNang = "Đăng nhập hệ thống", MaForm = "F01" },
                    new ChucNangDto { MaChucNang = "UC20", TenChucNang = "Quản lý nhóm quyền", MaForm = "F20" }
                });
            }

            public Task<List<PhanQuyenMatrixDto>> GetPermissionMatrixAsync(string roleId)
            {
                return Task.FromResult(new List<PhanQuyenMatrixDto>
                {
                    new PhanQuyenMatrixDto { MaChucNang = "UC01", TenChucNang = "Đăng nhập hệ thống", MaForm = "F01", CoQuyen = true },
                    new PhanQuyenMatrixDto { MaChucNang = "UC20", TenChucNang = "Quản lý nhóm quyền", MaForm = "F20", CoQuyen = roleId == "ADMIN" }
                });
            }

            public Task<ServiceResult> SavePermissionMatrixAsync(string roleId, List<PhanQuyenMatrixDto> matrix)
            {
                return Task.FromResult(ServiceResult.Success("Đã lưu ma trận phân quyền thành công!"));
            }

            public Task<ServiceResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
            {
                return Task.FromResult(ServiceResult<RoleDto>.Success(new RoleDto { MaNhomQuyen = dto.MaNhomQuyen, TenNhomQuyen = dto.TenNhomQuyen }));
            }

            public Task<ServiceResult> DeleteRoleAsync(string roleId)
            {
                return Task.FromResult(ServiceResult.Success("Đã xóa nhóm quyền thành công!"));
            }
        }

        [TestInitialize]
        public void Setup()
        {
            CurrentSession.DangNhap("USR001", "admin", "Quản trị viên", "admin@event.vn", "ADMIN", new[] { "UC20", "F20" });
        }

        [TestCleanup]
        public void Cleanup()
        {
            CurrentSession.DangXuat();
        }

        [TestMethod]
        public void F20_TitleBar_And_WindowState_Standardized()
        {
            // Mô tả: Kiểm tra Form F20 có tiêu đề chuẩn "Hệ Thống Quản Lý Sự Kiện" và khởi tạo ở trạng thái Maximized.
            using var form = new FrmRolePermission(new MockRolePermissionService());

            form.Text.Should().Be("Hệ Thống Quản Lý Sự Kiện");
            form.WindowState.Should().Be(FormWindowState.Maximized);
        }

        [TestMethod]
        public void F20_Header_Standardized_And_SubtitleHidden()
        {
            // Mô tả: Kiểm tra tiêu đề banner là "QUẢN LÝ NHÓM QUYỀN", dòng phụ đề đã được ẩn đi.
            using var form = new FrmRolePermission(new MockRolePermissionService());

            form.LblTitle.Text.Should().Be("QUẢN LÝ NHÓM QUYỀN");
            form.LblSubTitle.Visible.Should().BeFalse();
        }

        [TestMethod]
        public void F20_GridView_HeadersCentered()
        {
            // Mô tả: Kiểm tra tất cả tiêu đề cột của bảng phân quyền dgvPhanQuyen được căn giữa.
            using var form = new FrmRolePermission(new MockRolePermissionService());

            foreach (DataGridViewColumn col in form.DgvPhanQuyen.Columns)
            {
                col.HeaderCell.Style.Alignment.Should().Be(DataGridViewContentAlignment.MiddleCenter,
                    $"Cột '{col.HeaderText}' phải được căn giữa tiêu đề");
            }
        }

        [TestMethod]
        public void F20_ActionButtons_ConfiguredProperly()
        {
            // Mô tả: Kiểm tra các nút tác vụ (Lưu thay đổi, Thêm vai trò, Xóa vai trò, Chọn tất cả, Bỏ chọn tất cả) hiển thị đầy đủ và có định dạng chuẩn.
            using var form = new FrmRolePermission(new MockRolePermissionService());

            form.BtnLuu.Text.Should().Be("Lưu thay đổi");
            form.BtnThemNhom.Text.Should().Be("+ Thêm vai trò");
            form.BtnXoaNhom.Text.Should().Be("Xóa vai trò");
            form.BtnChonTatCa.Text.Should().Be("Chọn tất cả");
            form.BtnBoChonTatCa.Text.Should().Be("Bỏ chọn tất cả");

            form.BtnLuu.BackColor.Should().Be(Color.FromArgb(22, 163, 74));
            form.BtnThemNhom.BackColor.Should().Be(Color.FromArgb(37, 99, 235));
            form.BtnXoaNhom.BackColor.Should().Be(Color.FromArgb(239, 68, 68));
        }

        [TestMethod]
        public async Task F20_LoadRolesAndMatrix_PopulatesDataCorrectly()
        {
            // Mô tả: Kiểm tra tải danh mục vai trò và ma trận quyền cho ADMIN mặc định toàn quyền.
            using var form = new FrmRolePermission(new MockRolePermissionService());

            await form.LoadRolesAsync();

            form.CboNhomQuyen.Items.Count.Should().Be(2);
            form.DgvPhanQuyen.Rows.Count.Should().Be(2);
        }
    }
}
