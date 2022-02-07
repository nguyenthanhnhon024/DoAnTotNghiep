namespace ModelEF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoatDong")]
    public partial class HoatDong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoatDong()
        {
            tblSinhViens = new HashSet<tblSinhVien>();
        }

        [Key]
        public int idHD { get; set; }

        [Required]
        [StringLength(100)]
        public string tenHD { get; set; }

        public int hocKy { get; set; }

        [StringLength(2000)]
        public string noiDungHD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSinhVien> tblSinhViens { get; set; }
    }
}
