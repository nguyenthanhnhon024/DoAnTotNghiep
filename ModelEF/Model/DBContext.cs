using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ModelEF.Model
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<Bang_DRL> Bang_DRL { get; set; }
        public virtual DbSet<DanhMucDiem> DanhMucDiems { get; set; }
        public virtual DbSet<diemTBHocKy> diemTBHocKies { get; set; }
        public virtual DbSet<DotCham> DotChams { get; set; }
        public virtual DbSet<HoatDong> HoatDongs { get; set; }
        public virtual DbSet<HocPhi> HocPhis { get; set; }
        public virtual DbSet<Lop> Lops { get; set; }
        public virtual DbSet<PhongBan> PhongBans { get; set; }
        public virtual DbSet<SV_DC> SV_DC { get; set; }

        public virtual DbSet<SV_HD> SV_HD { get; set; }
        public virtual DbSet<TaiKhoanPB> TaiKhoanPBs { get; set; }
        public virtual DbSet<tblSinhVien> tblSinhViens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bang_DRL>()
                .Property(e => e.maSV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Bang_DRL>()
                .Property(e => e.minhChung)
                .IsUnicode(false);

            modelBuilder.Entity<diemTBHocKy>()
                .Property(e => e.maSV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DotCham>()
                .HasMany(e => e.SV_DC)
                .WithRequired(e => e.DotCham)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HoatDong>()
                .HasMany(e => e.tblSinhViens)
                .WithMany(e => e.HoatDongs);

            modelBuilder.Entity<HocPhi>()
                .Property(e => e.maSV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Lop>()
                .Property(e => e.maLop)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Lop>()
                .HasMany(e => e.tblSinhViens)
                .WithRequired(e => e.Lop)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lop>()
                .HasMany(e => e.TaiKhoanPBs)
                .WithOptional(e => e.Lop)
                .HasForeignKey(e => e.chuNhiemLop);

            modelBuilder.Entity<PhongBan>()
                .HasMany(e => e.TaiKhoanPBs)
                .WithRequired(e => e.PhongBan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SV_DC>()
                .Property(e => e.maSV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SV_HD>()
                .Property(e => e.maSV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoanPB>()
                .Property(e => e.maTK)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoanPB>()
                .Property(e => e.chuNhiemLop)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoanPB>()
                .Property(e => e.matKhau)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSinhVien>()
                .Property(e => e.maSV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSinhVien>()
                .Property(e => e.maLop)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSinhVien>()
                .Property(e => e.SDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSinhVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<tblSinhVien>()
                .Property(e => e.matKhau)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSinhVien>()
                .HasMany(e => e.diemTBHocKies)
                .WithRequired(e => e.tblSinhVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblSinhVien>()
                .HasMany(e => e.HocPhis)
                .WithRequired(e => e.tblSinhVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblSinhVien>()
                .HasMany(e => e.SV_DC)
                .WithRequired(e => e.tblSinhVien)
                .WillCascadeOnDelete(false);
        }
    }
}
