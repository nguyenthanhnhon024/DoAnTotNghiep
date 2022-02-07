using ModelEF.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEF.Dao
{
    public class TaiKhoanPBDao
    {
        DBContext context = new DBContext();
        public List<TaiKhoanPB> ListAll()
        {
            IQueryable<TaiKhoanPB> model = context.TaiKhoanPBs;

            return model.OrderByDescending(x => x.maTK).ToList();
        }

        public IEnumerable<TaiKhoanPB> ListAllPost(string keysearch, int page, int pageSize)
        {
            //var da_dk = (from sv_hoatdong in context.sv_hoatdong where sv_hoatdong.tinhTrangDuyet == true select sv_hoatdong).Count();
            IQueryable<TaiKhoanPB> model = context.TaiKhoanPBs;
            if (!string.IsNullOrEmpty(keysearch.ToString()))
                model = model.Where(x => x.hoTen.ToString().Contains(keysearch));
            return model.OrderByDescending(x => x.maTK).ToPagedList(page, pageSize);
        }

        public string Edit(TaiKhoanPB entity)
        {
            var sp = Find(entity.maTK);
            if (sp == null)
            {
                context.TaiKhoanPBs.Add(entity);
            }
            else
            {
                sp.hoTen = entity.hoTen;
                sp.chuNhiemLop = entity.chuNhiemLop;
                sp.maPB = entity.maPB;
                sp.matKhau = entity.matKhau;
                sp.SDT = entity.SDT;
                sp.Email = entity.Email;
                sp.gioiTinh = entity.gioiTinh;
                sp.ngaySinh = entity.ngaySinh;
                sp.diaChi = entity.diaChi;
                sp.avatar = entity.avatar;
            }
            context.SaveChanges();
            return entity.maTK.ToString();
        }

        public TaiKhoanPB Find(string maTK)
        {
            return context.TaiKhoanPBs.Find(maTK);
        }

        public int FindmaTK(string maTK)
        {
            var tt = context.TaiKhoanPBs.FirstOrDefault(x => x.maTK == maTK);
            if (tt != null)
                return 1;
            else
                return 0;
        }

        public int ChangeStatus(string id)
        {
            var a = context.TaiKhoanPBs.SingleOrDefault(x => x.maTK.Contains(id));
            if (a.ttTaiKhoan == 1)
            {
                a.ttTaiKhoan = 0;
            }
            else 
            {
                a.ttTaiKhoan = 1;
            }
            context.SaveChanges();
            return a.ttTaiKhoan;
        }
    }
}
