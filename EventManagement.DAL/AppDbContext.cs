using EventManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.DAL
{
    /// <summary>
    /// DbContext chính của hệ thống Quản lý Sự kiện (Entity Framework Core)
    /// Đầy đủ 20 bảng quan hệ theo 01_Database_Schema.sql
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base(DatabaseConfigHelper.CreateDefaultOptions())
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<NhomQuyen> NhomQuyens { get; set; } = null!;
        public DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public DbSet<ChucNang> ChucNangs { get; set; } = null!;
        public DbSet<PhanQuyen> PhanQuyens { get; set; } = null!;
        public DbSet<SuKien> SuKiens { get; set; } = null!;
        public DbSet<HangVe> HangVes { get; set; } = null!;
        public DbSet<ChiTietVe> ChiTietVes { get; set; } = null!;
        public DbSet<CheckIn> CheckIns { get; set; } = null!;
        public DbSet<DangKy> DangKys { get; set; } = null!;
        public DbSet<NguoiThamGia> NguoiThamGias { get; set; } = null!;
        public DbSet<ChiPhiSuKien> ChiPhiSuKiens { get; set; } = null!;
        public DbSet<ChiTietTaiTro> ChiTietTaiTros { get; set; } = null!;
        public DbSet<NhaTaiTro> NhaTaiTros { get; set; } = null!;
        public DbSet<DienGia> DienGias { get; set; } = null!;
        public DbSet<PhanHoi> PhanHois { get; set; } = null!;
        public DbSet<LoaiSuKien> LoaiSuKiens { get; set; } = null!;
        public DbSet<DiaDiem> DiaDiems { get; set; } = null!;
        public DbSet<NhanSu> NhanSus { get; set; } = null!;
        public DbSet<LichTrinh> LichTrinhs { get; set; } = null!;
        public DbSet<PhanCongNhanSu> PhanCongNhanSus { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bảng 1: NhomQuyen
            modelBuilder.Entity<NhomQuyen>(entity =>
            {
                entity.ToTable("NhomQuyen");
                entity.HasKey(e => e.MaNhomQuyen);
                entity.Property(e => e.MaNhomQuyen).HasMaxLength(20);
                entity.Property(e => e.TenNhomQuyen).HasMaxLength(50).IsRequired();
                entity.Property(e => e.MoTa).HasMaxLength(255);
            });

            // Bảng 2: NguoiDung
            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.ToTable("NguoiDung");
                entity.HasKey(e => e.MaNguoiDung);
                entity.Property(e => e.MaNguoiDung).HasMaxLength(20);
                entity.Property(e => e.TenDangNhap).HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.TenDangNhap).IsUnique();
                entity.Property(e => e.MatKhauHash).HasMaxLength(255).IsRequired();
                entity.Property(e => e.HoTen).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.SoDienThoai).HasMaxLength(15);
                entity.Property(e => e.MaNhomQuyen).HasMaxLength(20).IsRequired();
                entity.Property(e => e.TrangThai).HasMaxLength(30).HasDefaultValue("Hoạt động");
                entity.Property(e => e.SoLanDangNhapSai).HasDefaultValue(0);

                entity.HasOne(e => e.NhomQuyen)
                      .WithMany(n => n.NguoiDungs)
                      .HasForeignKey(e => e.MaNhomQuyen);
            });

            // Bảng 3: ChucNang
            modelBuilder.Entity<ChucNang>(entity =>
            {
                entity.ToTable("ChucNang");
                entity.HasKey(e => e.MaChucNang);
                entity.Property(e => e.MaChucNang).HasMaxLength(20);
                entity.Property(e => e.TenChucNang).HasMaxLength(100).IsRequired();
                entity.Property(e => e.MaForm).HasMaxLength(20).IsRequired();
            });

            // Bảng 4: PhanQuyen (Composite Key)
            modelBuilder.Entity<PhanQuyen>(entity =>
            {
                entity.ToTable("PhanQuyen");
                entity.HasKey(e => new { e.MaNhomQuyen, e.MaChucNang });
                entity.Property(e => e.MaNhomQuyen).HasMaxLength(20);
                entity.Property(e => e.MaChucNang).HasMaxLength(20);
                entity.Property(e => e.CoQuyen).HasDefaultValue(true);

                entity.HasOne(e => e.NhomQuyen)
                      .WithMany(n => n.PhanQuyens)
                      .HasForeignKey(e => e.MaNhomQuyen);

                entity.HasOne(e => e.ChucNang)
                      .WithMany(c => c.PhanQuyens)
                      .HasForeignKey(e => e.MaChucNang);
            });

            // Bảng 5: LoaiSuKien
            modelBuilder.Entity<LoaiSuKien>(entity =>
            {
                entity.ToTable("LoaiSuKien");
                entity.HasKey(e => e.MaLoaiSK);
                entity.Property(e => e.MaLoaiSK).HasMaxLength(20);
                entity.Property(e => e.TenLoaiSK).HasMaxLength(100).IsRequired();
            });

            // Bảng 6: DiaDiem
            modelBuilder.Entity<DiaDiem>(entity =>
            {
                entity.ToTable("DiaDiem");
                entity.HasKey(e => e.MaDiaDiem);
                entity.Property(e => e.MaDiaDiem).HasMaxLength(20);
                entity.Property(e => e.TenDiaDiem).HasMaxLength(200).IsRequired();
            });

            // Bảng 7: DienGia
            modelBuilder.Entity<DienGia>(entity =>
            {
                entity.ToTable("DienGia");
                entity.HasKey(e => e.MaDienGia);
            });

            // Bảng 8: NhanSu
            modelBuilder.Entity<NhanSu>(entity =>
            {
                entity.ToTable("NhanSu");
                entity.HasKey(e => e.MaNhanSu);
                entity.Property(e => e.MaNhanSu).HasMaxLength(20);
                entity.Property(e => e.HoTen).HasMaxLength(100).IsRequired();
            });

            // Bảng 9: NhaTaiTro
            modelBuilder.Entity<NhaTaiTro>(entity =>
            {
                entity.ToTable("NhaTaiTro");
                entity.HasKey(e => e.MaNhaTaiTro);
            });

            // Bảng 10: SuKien
            modelBuilder.Entity<SuKien>(entity =>
            {
                entity.ToTable("SuKien");
                entity.HasKey(e => e.MaSuKien);
                entity.Property(e => e.MaSuKien).HasMaxLength(20);
                entity.Property(e => e.TenSuKien).HasMaxLength(200).IsRequired();
                entity.Property(e => e.MaLoaiSK).HasMaxLength(20).IsRequired();
                entity.Property(e => e.MaDiaDiem).HasMaxLength(20).IsRequired();
                entity.Property(e => e.TrangThai).HasMaxLength(30).HasDefaultValue("Dự kiến");
                entity.Property(e => e.NguoiPhuTrach).HasColumnName("MaNguoiTao").HasMaxLength(20);
                entity.Ignore(e => e.SucChua);
                entity.Ignore(e => e.NganSachDuKien);
            });

            // Bảng 11: LichTrinh
            modelBuilder.Entity<LichTrinh>(entity =>
            {
                entity.ToTable("LichTrinh");
                entity.HasKey(e => e.MaLichTrinh);
                entity.Property(e => e.MaLichTrinh).HasMaxLength(20);
                entity.Property(e => e.TenTietMuc).HasMaxLength(200).IsRequired();
            });

            // Bảng 12: ChiTietTaiTro
            modelBuilder.Entity<ChiTietTaiTro>(entity =>
            {
                entity.ToTable("ChiTietTaiTro");
                entity.HasKey(e => e.MaTaiTro);
                entity.Property(e => e.MaTaiTro).HasMaxLength(20);
                entity.Property(e => e.MaSuKien).HasMaxLength(20).IsRequired();
                entity.Property(e => e.MaNhaTaiTro).HasMaxLength(20).IsRequired();
                entity.Property(e => e.HangTaiTro).HasColumnName("GoiTaiTro").HasMaxLength(50);
                entity.Property(e => e.GiaTri).HasColumnName("GiaTriTaiTro").HasPrecision(18, 2);
                entity.Ignore(e => e.QuyenLoiTruyenThong);
            });

            // Bảng 13: HangVe
            modelBuilder.Entity<HangVe>(entity =>
            {
                entity.ToTable("HangVe");
                entity.HasKey(e => e.MaHangVe);
                entity.Property(e => e.MaHangVe).HasMaxLength(20);
                entity.Property(e => e.MaSuKien).HasMaxLength(20).IsRequired();
                entity.Property(e => e.TenHangVe).HasMaxLength(50).IsRequired();
                entity.Property(e => e.GiaVe).HasPrecision(18, 2);
                entity.Ignore(e => e.DonGia);
            });

            // Bảng 14: NguoiThamGia
            modelBuilder.Entity<NguoiThamGia>(entity =>
            {
                entity.ToTable("NguoiThamGia");
                entity.HasKey(e => e.MaNguoiThamGia);
                entity.Ignore(e => e.DonViCongTac);
            });

            // Bảng 15: DangKy
            modelBuilder.Entity<DangKy>(entity =>
            {
                entity.ToTable("DangKy");
                entity.HasKey(e => e.MaDangKy);
            });

            // Bảng 16: ChiTietVe
            modelBuilder.Entity<ChiTietVe>(entity =>
            {
                entity.ToTable("ChiTietVe");
                entity.HasKey(e => e.MaVe);
                entity.Property(e => e.MaVe).HasMaxLength(30);
                entity.Property(e => e.MaDangKy).HasMaxLength(20).IsRequired();
                entity.Property(e => e.MaQR).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.MaQR).IsUnique();
            });

            // Bảng 17: CheckIn
            modelBuilder.Entity<CheckIn>(entity =>
            {
                entity.ToTable("CheckIn");
                entity.HasKey(e => e.MaCheckIn);
                entity.Property(e => e.MaCheckIn).HasMaxLength(20);
                entity.Property(e => e.MaVe).HasMaxLength(30).IsRequired();
                entity.HasIndex(e => e.MaVe).IsUnique();
                entity.Property(e => e.NguoiThucHien).HasMaxLength(20).IsRequired();
                entity.Ignore(e => e.NguoiCheckIn);
                entity.Ignore(e => e.PhuongThuc);

                entity.HasOne(e => e.ChiTietVe)
                      .WithOne(v => v.CheckIn)
                      .HasForeignKey<CheckIn>(e => e.MaVe);
            });

            // Bảng 18: PhanCongNhanSu
            modelBuilder.Entity<PhanCongNhanSu>(entity =>
            {
                entity.ToTable("PhanCongNhanSu");
                entity.HasKey(e => e.MaPhanCong);
                entity.Property(e => e.MaPhanCong).HasMaxLength(20);
            });

            // Bảng 19: ChiPhiSuKien
            modelBuilder.Entity<ChiPhiSuKien>(entity =>
            {
                entity.ToTable("ChiPhiSuKien");
                entity.HasKey(e => e.MaChiPhi);
            });

            // Bảng 20: PhanHoi
            modelBuilder.Entity<PhanHoi>(entity =>
            {
                entity.ToTable("PhanHoi");
                entity.HasKey(e => e.MaPhanHoi);
            });
        }
    }
}
