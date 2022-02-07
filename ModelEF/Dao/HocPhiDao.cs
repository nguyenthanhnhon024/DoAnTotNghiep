using ModelEF.Model;
using ModelEF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEF.Dao
{
    public class HocPhiDao
    {
        DBContext context = new DBContext();
        public List<SinhVien_HocPhiViewModel> ListAllHocKy(int hocKy)
        {
            var model = from a in context.tblSinhViens
                        join b in context.HocPhis on a.maSV equals b.maSV
                        where b.hocKy == hocKy
                        select new SinhVien_HocPhiViewModel()
                        {
                            maSV = a.maSV,
                            tenSV = a.tenSV,
                            maLop = a.maLop,
                            hocKy = b.hocKy
                        };
            return model.OrderBy(x => x.maSV).ToList();
        }


        public HocPhi Find(int hocKy, string maSV)
        {
            return context.HocPhis.FirstOrDefault(x => x.hocKy == hocKy && x.maSV == maSV);
        }
        public bool Delete(int hocKy, string maSV)
        {
            try
            {
                var sp = Find(hocKy, maSV);
                context.HocPhis.Remove(sp);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Edit(HocPhi entity)
        {
            var sp = Find(entity.hocKy, entity.maSV.Trim());
            if (sp == null)
            {
                context.HocPhis.Add(entity);
            }
            context.SaveChanges();
            return entity.hocKy.ToString();
        }
    }
}
