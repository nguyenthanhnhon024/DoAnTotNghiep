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
    public class SinhVienDao
    {
        private DBContext db;
        public SinhVienDao()
        {
            db = new DBContext();
        }

        public string Edit(tblSinhVien entity)
        {
            var sp = Find(entity.maSV);
            if (sp == null)
            {
                db.tblSinhViens.Add(entity);
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
            return entity.maSV.ToString();
        }

        public string EditCVu(tblSinhVien entity)
        {
            var sp = Find(entity.maSV);
            if (sp == null)
            {
                db.tblSinhViens.Add(entity);
            }
            else
            {
                sp.chucVu = entity.chucVu;
            }
            db.SaveChanges();
            return entity.maSV.ToString();
        }
        public tblSinhVien Find(String maSV)
        {
            return db.tblSinhViens.Find(maSV);
        }

        public diemTBHocKy FindDiem(String maSV, int hocKy)
        {
            return db.diemTBHocKies.FirstOrDefault(x => x.maSV == maSV && x.hocKy == hocKy);
        }

        public diemTBHocKy EditDiem(diemTBHocKy entity)
        {
            var sp = FindDiem(entity.maSV, entity.hocKy);
            if (sp == null)
            {
                db.diemTBHocKies.Add(entity);
            }
            else
            {
                sp.diemTB = entity.diemTB;
            }
            db.SaveChanges();
            return entity;
        }

        public int doimatkhau (string maSV,string mk,string mk1, string mk2)
        {
            var sp = Find(maSV);
            if (sp != null)
            {
                if(sp.matKhau.Trim() != mk)
                {
                    return 0;
                } else if (mk1 == mk2 )
                {
                    sp.matKhau = mk1;
                    db.SaveChanges();
                    return 1;
                }
                else { return 2; }
            }
            return 3;
        }


        public List<tblSinhVien> ListAll()
        {
            IQueryable<tblSinhVien> model = db.tblSinhViens;

            return model.OrderByDescending(x => x.maSV).ToList();
        }

        public IEnumerable<tblSinhVien> ListAllPost(string keysearch, int page, int pageSize)
        {
            //var da_dk = (from sv_hoatdong in context.sv_hoatdong where sv_hoatdong.tinhTrangDuyet == true select sv_hoatdong).Count();
            IQueryable<tblSinhVien> model = db.tblSinhViens;
            if (!string.IsNullOrEmpty(keysearch.ToString()))
                model = model.Where(x => x.tenSV.ToString().Contains(keysearch));
            return model.OrderByDescending(x => x.maSV).ToPagedList(page, pageSize);
        }

        public string EditSV (tblSinhVien entity)
        {
            var sp = Find(entity.maSV);
            if (sp == null)
            {
                db.tblSinhViens.Add(entity);
            }
            else
            {
                sp.tenSV = entity.tenSV;
                sp.maLop = entity.maLop;
                sp.matKhau = entity.matKhau;
                sp.SDT = entity.SDT;
                sp.Email = entity.Email;
                sp.gioiTinh = entity.gioiTinh;
                sp.ngaySinh = entity.ngaySinh;
                sp.diaChi = entity.diaChi;
                sp.avatar = entity.avatar;
                sp.chucVu = entity.chucVu;
            }
            db.SaveChanges();
            return entity.maSV.ToString();
        }

        public int FindmaSV(string maSV)
        {
            var tt = db.tblSinhViens.FirstOrDefault(x => x.maSV == maSV);
            if (tt != null)
                return 1;
            else
                return 0;
        }

        public int ChangeStatus(string id)
        {
            var a = db.tblSinhViens.SingleOrDefault(x => x.maSV.Contains(id));
            if (a.ttHoc == 1)
            {
                a.ttHoc = 0;
            }
            else
            {
                a.ttHoc = 1;
            }
            db.SaveChanges();
            return a.ttHoc;
        }

        public List<SinhVien_DiemViewModel> ListAllDiem(int hocKy)
        {
            var model = from a in db.tblSinhViens
                        join b in db.diemTBHocKies on a.maSV equals b.maSV
                        where a.maSV == b.maSV && b.hocKy == hocKy
                        select new SinhVien_DiemViewModel()
                        {
                            
                            maSV = a.maSV,
                            tenSV = a.tenSV,
                            maLop = a.maLop,
                            hocKy = b.hocKy,
                            diemTB = b.diemTB,
                        };
            return model.OrderBy(x => x.maSV).ToList();
        }

        public IEnumerable<SinhVien_DiemViewModel> ListALLDiemPost(string keysearch, int hocKy, int page, int pageSize)
        {
            var model = from a in db.tblSinhViens
                        join b in db.diemTBHocKies on a.maSV equals b.maSV
                        where a.maSV == b.maSV && b.hocKy == hocKy
                        select new SinhVien_DiemViewModel()
                        {

                            maSV = a.maSV,
                            tenSV = a.maSV,
                            maLop = a.maLop,
                            hocKy = b.hocKy,
                            diemTB = b.diemTB,
                        };
            if (!string.IsNullOrEmpty(keysearch.ToString()))
                model = model.Where(x => x.tenSV.ToString().Contains(keysearch));
            return model.OrderBy(x => x.maSV).ToPagedList(page, pageSize);
        }
    }
}
