using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWebMVC.Data;

public partial class EcommerceWebContext : DbContext
{
    public EcommerceWebContext()
    {
    }

    public EcommerceWebContext(DbContextOptions<EcommerceWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CthoaDon> CthoaDons { get; set; }

    public virtual DbSet<HangHoa> HangHoas { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<Loai> Loais { get; set; }

    public virtual DbSet<Ncc> Nccs { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

    public virtual DbSet<VanChuyen> VanChuyens { get; set; }

    public virtual DbSet<YeuThich> YeuThiches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MILO;Database=EcommerceWeb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CthoaDon>(entity =>
        {
            entity.HasKey(e => e.MaCt).HasName("PK__CTHoaDon__27258E74D7F3B673");

            entity.ToTable("CTHoaDon");

            entity.Property(e => e.MaCt).HasColumnName("MaCT");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiamGia)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.CthoaDons)
                .HasForeignKey(d => d.MaHd)
                .HasConstraintName("FK__CTHoaDon__MaHD__5AEE82B9");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.CthoaDons)
                .HasForeignKey(d => d.MaHh)
                .HasConstraintName("FK__CTHoaDon__MaHH__5BE2A6F2");
        });

        modelBuilder.Entity<HangHoa>(entity =>
        {
            entity.HasKey(e => e.MaHh).HasName("PK__HangHoa__2725A6E4B92C8F9E");

            entity.ToTable("HangHoa");

            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiamGia)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Hinh).HasMaxLength(255);
            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.SoLanXem).HasDefaultValue(0);
            entity.Property(e => e.TenHh)
                .HasMaxLength(100)
                .HasColumnName("TenHH");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK__HangHoa__MaLoai__3F466844");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaNcc)
                .HasConstraintName("FK__HangHoa__MaNCC__403A8C7D");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HoaDon__2725A6E020FAEED6");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(50);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__HoaDon__MaKH__5535A963");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaTrangThai)
                .HasConstraintName("FK__HoaDon__MaTrangT__571DF1D5");

            entity.HasOne(d => d.MaVanChuyenNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaVanChuyen)
                .HasConstraintName("FK__HoaDon__MaVanChu__5629CD9C");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1E15E8FB4B");

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.TenDn, "UQ__KhachHan__4CF96558DB2460FC").IsUnique();

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.DienThoai).HasMaxLength(20);
            entity.Property(e => e.Hinh).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaPq).HasColumnName("MaPQ");
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.RandomKey).HasMaxLength(50);
            entity.Property(e => e.SoDiem).HasDefaultValue(0);
            entity.Property(e => e.TenDn)
                .HasMaxLength(50)
                .HasColumnName("TenDN");

            entity.HasOne(d => d.MaPqNavigation).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.MaPq)
                .HasConstraintName("FK__KhachHang__MaPQ__44FF419A");
        });

        modelBuilder.Entity<Loai>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__Loai__730A5759D0C38783");

            entity.ToTable("Loai");

            entity.Property(e => e.Hinh).HasMaxLength(255);
            entity.Property(e => e.TenLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<Ncc>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__NCC__3A185DEB43EFA7EF");

            entity.ToTable("NCC");

            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.DienThoai).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.TenCongTy).HasMaxLength(100);
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.MaPq).HasName("PK__PhanQuye__2725E7F377E58D7C");

            entity.ToTable("PhanQuyen");

            entity.Property(e => e.MaPq).HasColumnName("MaPQ");
            entity.Property(e => e.ChucVu).HasMaxLength(50);
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai).HasName("PK__TrangTha__AADE41384DA1BC23");

            entity.ToTable("TrangThai");

            entity.Property(e => e.TenTrangThai).HasMaxLength(100);
        });

        modelBuilder.Entity<VanChuyen>(entity =>
        {
            entity.HasKey(e => e.MaVanChuyen).HasName("PK__VanChuye__4B22972DA455174C");

            entity.ToTable("VanChuyen");

            entity.Property(e => e.PhiVanChuyen).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PhuongThuc).HasMaxLength(100);
        });

        modelBuilder.Entity<YeuThich>(entity =>
        {
            entity.HasKey(e => e.MaYt).HasName("PK__YeuThich__272330D458E6DA49");

            entity.ToTable("YeuThich");

            entity.Property(e => e.MaYt).HasColumnName("MaYT");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaHh)
                .HasConstraintName("FK__YeuThich__MaHH__5070F446");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__YeuThich__MaKH__5165187F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
