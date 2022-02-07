namespace ModelEF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HocPhi")]
    public partial class HocPhi
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(13)]
        public string maSV { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int hocKy { get; set; }


        public virtual tblSinhVien tblSinhVien { get; set; }
    }
}
