using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagement.Core.Entities;
using EventManagement.DAL;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventManagement.Tests
{
    /// <summary>
    /// Kiểm thử hạ tầng AppDbContext với EF Core InMemory (Thành viên 1)
    /// </summary>
    [TestClass]
    public class AppDbContextTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [TestMethod]
        public async Task AppDbContext_CanSaveAndRetrieveNguoiDungWithNhomQuyen()
        {
            // Arrange
            using var context = CreateContext();
            var role = new NhomQuyen
            {
                MaNhomQuyen = "ADMIN",
                TenNhomQuyen = "Quản trị viên",
                MoTa = "Toàn quyền quản trị"
            };
            var user = new NguoiDung
            {
                MaNguoiDung = "USR001",
                TenDangNhap = "admin",
                MatKhauHash = "$2a$11$samplehashstringfortestingpurposeonly00000000000000",
                HoTen = "Quản trị viên Hệ thống",
                Email = "admin@event.vn",
                MaNhomQuyen = "ADMIN",
                TrangThai = "Hoạt động",
                SoLanDangNhapSai = 0
            };

            // Act
            await context.NhomQuyens.AddAsync(role);
            await context.NguoiDungs.AddAsync(user);
            int saved = await context.SaveChangesAsync();

            // Assert
            saved.Should().Be(2);

            var retrievedUser = await context.NguoiDungs
                .Include(u => u.NhomQuyen)
                .FirstOrDefaultAsync(u => u.TenDangNhap == "admin");

            retrievedUser.Should().NotBeNull();
            retrievedUser!.HoTen.Should().Be("Quản trị viên Hệ thống");
            retrievedUser.NhomQuyen.Should().NotBeNull();
            retrievedUser.NhomQuyen!.TenNhomQuyen.Should().Be("Quản trị viên");
        }

        [TestMethod]
        public async Task AppDbContext_CanSavePermissionMatrix()
        {
            // Arrange
            using var context = CreateContext();
            var role = new NhomQuyen { MaNhomQuyen = "STAFF", TenNhomQuyen = "Nhân viên" };
            var func = new ChucNang { MaChucNang = "UC13", TenChucNang = "Soát vé Check-in", MaForm = "F13" };
            var permission = new PhanQuyen { MaNhomQuyen = "STAFF", MaChucNang = "UC13", CoQuyen = true };

            // Act
            await context.NhomQuyens.AddAsync(role);
            await context.ChucNangs.AddAsync(func);
            await context.PhanQuyens.AddAsync(permission);
            await context.SaveChangesAsync();

            // Assert
            var retrieved = await context.PhanQuyens.FindAsync("STAFF", "UC13");
            retrieved.Should().NotBeNull();
            retrieved!.CoQuyen.Should().BeTrue();
        }
    
        [TestMethod]
        public void AppDbContext_All20DbSets_AreConfiguredAndAccessible()
        {
            using var context = CreateContext();

            context.NhomQuyens.Should().NotBeNull();
            context.NguoiDungs.Should().NotBeNull();
            context.ChucNangs.Should().NotBeNull();
            context.PhanQuyens.Should().NotBeNull();
            context.SuKiens.Should().NotBeNull();
            context.HangVes.Should().NotBeNull();
            context.ChiTietVes.Should().NotBeNull();
            context.CheckIns.Should().NotBeNull();
            context.DangKys.Should().NotBeNull();
            context.NguoiThamGias.Should().NotBeNull();
            context.ChiPhiSuKiens.Should().NotBeNull();
            context.ChiTietTaiTros.Should().NotBeNull();
            context.NhaTaiTros.Should().NotBeNull();
            context.DienGias.Should().NotBeNull();
            context.PhanHois.Should().NotBeNull();
            context.LoaiSuKiens.Should().NotBeNull();
            context.DiaDiems.Should().NotBeNull();
            context.NhanSus.Should().NotBeNull();
            context.LichTrinhs.Should().NotBeNull();
            context.PhanCongNhanSus.Should().NotBeNull();

            // Verify EF Core Model contains 20 entities
            context.Model.GetEntityTypes().Should().HaveCount(20);
        }

        [TestMethod]
        public void AppDbContext_SqlServerModel_HasNoShadowForeignKeyColumns_AndMatchesSqlSchema()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=.;Database=EventManagementDB;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            using var context = new AppDbContext(options);

            // 1. Không được có bất kỳ shadow property nào (ví dụ DiaDiemMaDiaDiem, LoaiSuKienMaLoaiSK, SuKienMaSuKien...)
            var shadowProps = context.Model.GetEntityTypes()
                .SelectMany(et => et.GetProperties().Where(p => p.IsShadowProperty())
                    .Select(p => $"{et.GetTableName()}.{p.GetColumnName()}"))
                .ToList();
            shadowProps.Should().BeEmpty("mọi khóa ngoại phải map trực tiếp vào cột khóa ngoại thật trong CSDL");

            // 2. Kiểm tra danh sách cột các bảng DashboardService truy vấn khớp 01_Database_Schema.sql
            var expectedColumns = new Dictionary<string, string[]>
            {
                ["SuKien"] = new[] { "MaSuKien", "TenSuKien", "MaLoaiSK", "MaDiaDiem", "NgayBatDau", "NgayKetThuc", "TrangThai", "MoTa" },
                ["HangVe"] = new[] { "MaHangVe", "MaSuKien", "TenHangVe", "GiaVe", "SoLuongPhatHanh", "SoLuongDaBan", "ThoiGianMoBan", "ThoiGianDongBan" },
                ["DangKy"] = new[] { "MaDangKy", "MaSuKien", "MaNguoiThamGia", "MaHangVe", "NgayDangKy", "SoLuong", "TongTien", "TrangThaiThanhToan", "PhuongThucThanhToan" },
                ["ChiTietVe"] = new[] { "MaVe", "MaDangKy", "MaQR", "TenNguoiThamGia", "Email", "TrangThaiVe" },
                ["CheckIn"] = new[] { "MaCheckIn", "MaVe", "ThoiGianCheckIn", "NguoiThucHien", "HinhThucCheckIn", "GhiChu" },
                ["ChiPhiSuKien"] = new[] { "MaChiPhi", "MaSuKien", "KhoanMucChi", "SoTien", "NgayChi", "NguoiDuyet", "GhiChu" },
                ["DiaDiem"] = new[] { "MaDiaDiem", "TenDiaDiem", "DiaChi", "SucChuaToiDa", "GhiChu" },
                ["NguoiThamGia"] = new[] { "MaNguoiThamGia", "HoTen", "GioiTinh", "Email", "SoDienThoai", "DonViCongTac" },
                ["NhaTaiTro"] = new[] { "MaNhaTaiTro", "TenToChuc", "NguoiDaiDien", "Email", "SoDienThoai", "DiaChi" }
            };

            foreach (var kv in expectedColumns)
            {
                var entityType = context.Model.GetEntityTypes()
                    .First(et => et.GetTableName() == kv.Key);
                var tableId = StoreObjectIdentifier.Table(kv.Key, entityType.GetSchema());
                var actualCols = entityType.GetProperties()
                    .Select(p => p.GetColumnName(tableId))
                    .ToList();

                actualCols.Should().BeEquivalentTo(kv.Value, $"bảng {kv.Key} phải khớp đúng cột SQL Server");
            }
        }
    }
}