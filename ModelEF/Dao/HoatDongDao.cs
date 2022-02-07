using ModelEF.Model;
using ModelEF.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEF.Dao
{
    public class HoatDongDao
    {
        DBContext context = new DBContext();
        public List<HoatDong> ListAll()
        {
            IQueryable<HoatDong> model = context.HoatDongs;

            return model.OrderByDescending(x => x.idHD).ToList();
        }

        public IEnumerable<HoatDong> ListAllPost(string keysearch, int page, int pageSize)
        {
            //var da_dk = (from sv_hoatdong in context.sv_hoatdong where sv_hoatdong.tinhTrangDuyet == true select sv_hoatdong).Count();
            IQueryable<HoatDong> model = context.HoatDongs;
            if (!string.IsNullOrEmpty(keysearch.ToString()))
                model = model.Where(x => x.tenHD.ToString().Contains(keysearch));
            return model.OrderByDescending(x => x.idHD).ToPagedList(page, pageSize);
        }

        public string Edit(HoatDong entity)
        {

            

            var sp = Find(entity.idHD);
            if (sp == null)
            {
                context.HoatDongs.Add(entity);
            }
            else
            {
                sp.tenHD = entity.tenHD;
                sp.hocKy = entity.hocKy;
                sp.noiDungHD = entity.noiDungHD;
            }
            context.SaveChanges();
            return entity.idHD.ToString();
        }

        public HoatDong Find(int idHD)
        {
            return context.HoatDongs.Find(idHD);
        }

        public int FindidHD(int idHD)
        {
            var tt = context.HoatDongs.FirstOrDefault(x => x.idHD == idHD);
            if (tt != null)
                return 1;
            else
                return 0;
        }

        public List<SinhVien_HoatDongViewModel> ListAllDiem(int idHD)
        {
            var model = from a in context.tblSinhViens
                        join b in context.SV_HD on a.maSV equals b.maSV
                        join c in context.HoatDongs on b.idHD equals c.idHD
                        where c.idHD == idHD
                        select new SinhVien_HoatDongViewModel()
                        {
                            idHD = c.idHD,
                            maSV = a.maSV,
                            tenSV = a.tenSV,
                            maLop = a.maLop,
                            tenHD = c.tenHD,
                            hocKy = c.hocKy
                        };
            return model.OrderBy(x => x.maSV).ToList();
        }



        public SV_HD FindSV_HD(int idHD, string maSV)
        {
            return context.SV_HD.FirstOrDefault(x => x.idHD == idHD && x.maSV == maSV);
        }
        public bool Delete(int idHD, string maSV)
        {
            try
            {
                var sp = FindSV_HD(idHD,maSV);
                context.SV_HD.Remove(sp);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Edit_SVHD(SV_HD entity)
        {
            var sp = FindSV_HD(entity.idHD,entity.maSV.Trim());
            if (sp == null)
            {
                context.SV_HD.Add(entity);
            }
            context.SaveChanges();
            return entity.idHD.ToString();
        }

    }
}
