using ModelEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEF.Dao
{
    public class PhongBanDao
    {
        private DBContext db;
        public PhongBanDao()
        {
            db = new DBContext();
        }

        public string Edit(TaiKhoanPB entity)
        {
            var sp = Find(entity.maTK);
            if (sp == null)
            {
                db.TaiKhoanPBs.Add(entity);
            }
            else
            {
                sp.SDT = entity.SDT;
                sp.ngaySinh = entity.ngaySinh;
                sp.diaChi = entity.diaChi;
                sp.Email = entity.Email;
                sp.avatar = entity.avatar;
            }
            db.SaveChanges();
            return entity.maTK.ToString();
        }
        public TaiKhoanPB Find(String maTK)
        {
            return db.TaiKhoanPBs.Find(maTK);
        }

        public int doimatkhau(string maTK, string mk, string mk1, string mk2)
        {
            var sp = Find(maTK);
            if (sp != null)
            {
                if (sp.matKhau.Trim() != mk)
                {
                    return 0;
                }
                else if (mk1 == mk2)
                {
                    sp.matKhau = mk1;
                    db.SaveChanges();
                    return 1;
                }
                else { return 2; }
            }
            return 3;
        }
    }
}
