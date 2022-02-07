using ModelEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEF.Dao
{
    public class DotChamDao
    {
        DBContext context = new DBContext();
        public List<DotCham> ListAll()
        {
            //var da_dk = (from sv_hoatdong in context.sv_hoatdong where sv_hoatdong.tinhTrangDuyet == true select sv_hoatdong).Count();
            IQueryable<DotCham> model = context.DotChams;
            DateTime now = DateTime.Now;

            model = model.Where(x => x.ngayCham <= now && x.ngayKetThuc >= now);
            return model.ToList();
        }
    }
}
