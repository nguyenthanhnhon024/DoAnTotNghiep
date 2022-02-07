using ModelEF.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEF.Dao
{
    public class LopDao
    {
        DBContext context = new DBContext();
        public List<Lop> ListAll()
        {
            IQueryable<Lop> model = context.Lops;

            return model.OrderByDescending(x => x.khoaHoc).ToList();
        }

        public IEnumerable<Lop> ListAllPost(string keysearch, int page, int pageSize)
        {
            //var da_dk = (from sv_hoatdong in context.sv_hoatdong where sv_hoatdong.tinhTrangDuyet == true select sv_hoatdong).Count();
            IQueryable<Lop> model = context.Lops;
            if (!string.IsNullOrEmpty(keysearch.ToString()))
                model = model.Where(x => x.maLop.ToString().Contains(keysearch));
            return model.OrderByDescending(x => x.khoaHoc).ToPagedList(page, pageSize);
        }

        public string Edit(Lop entity)
        {
            var sp = Find(entity.maLop);
            if (sp == null)
            {
                context.Lops.Add(entity);
            }
            else
            {
                sp.maLop = entity.maLop;
                sp.tenLop = entity.tenLop;
                sp.khoaHoc = entity.khoaHoc;
            }
            context.SaveChanges();
            return entity.maLop.ToString();
        }

        public Lop Find(string maLop)
        {
            return context.Lops.Find(maLop);
        }

        public int FindmaLop(string maLop)
        {
            var tt = context.Lops.FirstOrDefault(x => x.maLop == maLop);
            if (tt != null)
                return 1;
            else
                return 0;
        }
    }
}
