namespace ModelEF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("diemTBHocKy")]
    public partial class diemTBHocKy
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int hocKy { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(13)]
        public string maSV { get; set; }

        public double diemTB { get; set; }

        public virtual tblSinhVien tblSinhVien { get; set; }
    }
}
