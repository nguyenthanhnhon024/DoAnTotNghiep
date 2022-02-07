namespace ModelEF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSinhVien")]
    public partial class tblSinhVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblSinhVien()
        {
            Bang_DRL = new HashSet<Bang_DRL>();
            diemTBHocKies = new HashSet<diemTBHocKy>();
            HocPhis = new HashSet<HocPhi>();
            SV_DC = new HashSet<SV_DC>();
            HoatDongs = new HashSet<HoatDong>();
        }

        [Key]
        [StringLength(13)]
        public string maSV { get; set; }

        [Required]
        [StringLength(100)]
        public string tenSV { get; set; }

        [Required]
        [StringLength(7)]
        public string maLop { get; set; }

        [StringLength(11)]
        public string SDT { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int gioiTinh { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngaySinh { get; set; }

        [StringLength(100)]
        public string diaChi { get; set; }

        [StringLength(100)]
        public string chucVu { get; set; }

        public int ttHoc { get; set; }

        [StringLength(20)]
        public string matKhau { get; set; }

        [StringLength(100)]
        public string avatar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bang_DRL> Bang_DRL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<diemTBHocKy> diemTBHocKies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HocPhi> HocPhis { get; set; }

        public virtual Lop Lop { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SV_DC> SV_DC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoatDong> HoatDongs { get; set; }
    }
}
