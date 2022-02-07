namespace ModelEF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoanPB")]
    public partial class TaiKhoanPB
    {
        [Key]
        [StringLength(13)]
        public string maTK { get; set; }

        [StringLength(100)]
        public string hoTen { get; set; }

        [StringLength(7)]
        public string chuNhiemLop { get; set; }

        public int maPB { get; set; }

        [StringLength(20)]
        public string matKhau { get; set; }

        public int ttTaiKhoan { get; set; }

        [StringLength(11)]
        public string SDT { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int gioiTinh { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngaySinh { get; set; }

        [StringLength(100)]
        public string diaChi { get; set; }

        public string avatar { get; set; }

        [StringLength(100)]
        public virtual Lop Lop { get; set; }

        public virtual PhongBan PhongBan { get; set; }
    }
}
