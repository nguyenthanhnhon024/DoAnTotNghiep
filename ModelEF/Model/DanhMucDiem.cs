namespace ModelEF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhMucDiem")]
    public partial class DanhMucDiem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhMucDiem()
        {
            Bang_DRL = new HashSet<Bang_DRL>();
        }

        [Key]
        public int idMD { get; set; }

        [Required]
        [StringLength(3000)]
        public string noiDung { get; set; }

        public int loaiMD { get; set; }

        public int diemMax { get; set; }

        public int canMinhChung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bang_DRL> Bang_DRL { get; set; }
    }
}
