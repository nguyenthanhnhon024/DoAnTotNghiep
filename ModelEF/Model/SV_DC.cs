namespace ModelEF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SV_DC
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(13)]
        public string maSV { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idDC { get; set; }

        public int ttCham { get; set; }

        public int tongDiem { get; set; }

        public virtual DotCham DotCham { get; set; }

        public virtual tblSinhVien tblSinhVien { get; set; }
    }
}
