namespace ModelEF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bang_DRL
    {
        public int id { get; set; }

        [StringLength(13)]
        public string maSV { get; set; }

        public int idDC { get; set; }

        public int idMD { get; set; }

        public int? diemTDG { get; set; }

        public int? diemLDG { get; set; }

        public string minhChung { get; set; }

        public virtual DotCham DotCham { get; set; }

        public virtual DanhMucDiem DanhMucDiem { get; set; }

        public virtual tblSinhVien tblSinhVien { get; set; }
    }
}
