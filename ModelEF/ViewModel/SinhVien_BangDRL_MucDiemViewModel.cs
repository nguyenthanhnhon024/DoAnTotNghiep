using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEF.ViewModel
{
    public class SinhVien_BangDRL_MucDiemViewModel
    {
        public int id { set; get; }
        public string maSV { set; get; }

        public string chucVu { set; get; }

        public string tenSV { get; set; }
        public int idDC { set; get; }
        public int idMD { set; get; }
        public string noiDung { set; get; }
        public int loaiMD { set; get; }
        public int diemMax { set; get; }

        public int? diemTDG { set; get; }
        public int? diemLDG { set; get; }
        public int canMinhChung { set; get; }
        public string minhChung { set; get; }
    }
}
