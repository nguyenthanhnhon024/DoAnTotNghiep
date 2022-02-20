using ModelEF.Model;
using ModelEF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace ModelEF.Dao
{
    public class DRLDao
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

        public IEnumerable<Bang_DRL> ListWhereAll(string maSV, int idDC)
        {
            var model = (from Bang_DRL in context.Bang_DRL where Bang_DRL.idDC == idDC && Bang_DRL.maSV == maSV select Bang_DRL);
            model.OrderBy(x => x.idMD);
            return model.ToList();
        }

        public List<ChiTietDRLViewModel> ListByidMD(int idDC, string maSV)
        {
            var tt = context.tblSinhViens.FirstOrDefault(x => x.maSV == maSV && x.chucVu == "SV");
            if (tt != null)
            {
                var model = from a in context.Bang_DRL
                            join b in context.DanhMucDiems on a.idMD equals b.idMD
                            where a.idMD == b.idMD && a.maSV == maSV && a.idDC == idDC && b.loaiMD == 1
                            select new ChiTietDRLViewModel()
                            {
                                id = a.id,
                                maSV = a.maSV,
                                idDC = a.idDC,
                                idMD = a.idMD,
                                noiDung = b.noiDung,
                                loaiMD = b.loaiMD,
                                diemMax = b.diemMax,
                                diemTDG = a.diemTDG,
                                diemLDG = a.diemLDG,
                                canMinhChung = b.canMinhChung,
                                minhChung = a.minhChung
                            };
                return model.OrderBy(x => x.idMD).ToList();
            }else
            {
                var model = from a in context.Bang_DRL
                            join b in context.DanhMucDiems on a.idMD equals b.idMD
                            where a.idMD == b.idMD && a.maSV == maSV && a.idDC == idDC && b.loaiMD != 0
                            select new ChiTietDRLViewModel()
                            {
                                id = a.id,
                                maSV = a.maSV,
                                idDC = a.idDC,
                                idMD = a.idMD,
                                noiDung = b.noiDung,
                                loaiMD = b.loaiMD,
                                diemMax = b.diemMax,
                                diemTDG = a.diemTDG,
                                diemLDG = a.diemLDG,
                                canMinhChung = b.canMinhChung,
                                minhChung = a.minhChung
                            };
                return model.OrderBy(x => x.idMD).ToList();
            }
            
        }

        public List<ChiTietDRLViewModel> ListByidMD2(int idDC, string maSV)
        {
            var model = from a in context.Bang_DRL
                        join b in context.DanhMucDiems on a.idMD equals b.idMD
                        where a.idMD == b.idMD && a.maSV == maSV && a.idDC == idDC && b.loaiMD != 0
                        select new ChiTietDRLViewModel()
                        {
                            id = a.id,
                            maSV = a.maSV,
                            idDC = a.idDC,
                            idMD = a.idMD,
                            noiDung = b.noiDung,
                            loaiMD = b.loaiMD,
                            diemMax = b.diemMax,
                            diemTDG = a.diemTDG,
                            diemLDG = a.diemLDG,
                            canMinhChung = b.canMinhChung,
                            minhChung = a.minhChung
                        };
            return model.OrderBy(x => x.idMD).ToList();
        }

        public List<SinhVien_BangDRL_MucDiemViewModel> ListChiTietSV(int idDC, string maSV)
        {
            var model = from a in context.Bang_DRL
                        join b in context.DanhMucDiems on a.idMD equals b.idMD
                        join c in context.tblSinhViens on a.maSV equals c.maSV
                        where a.idMD == b.idMD && a.maSV == maSV && a.idDC == idDC && b.loaiMD != 0
                        select new SinhVien_BangDRL_MucDiemViewModel()
                        {
                            id = a.id,
                            maSV = a.maSV,
                            tenSV = c.tenSV,
                            chucVu = c.chucVu,
                            idDC = a.idDC,
                            idMD = a.idMD,
                            noiDung = b.noiDung,
                            loaiMD = b.loaiMD,
                            diemMax = b.diemMax,
                            diemTDG = a.diemTDG,
                            diemLDG = a.diemLDG,
                            canMinhChung = b.canMinhChung,
                            minhChung = a.minhChung
                        };
            return model.OrderBy(x => x.idMD).ToList();
        }

        public List<SinhVien_BangDRL_MucDiem_SVDCViewModel> ListChiTietSV2(int idDC, string maSV)
        {
            var model = from a in context.Bang_DRL
                        join b in context.DanhMucDiems on a.idMD equals b.idMD
                        join c in context.tblSinhViens on a.maSV equals c.maSV
                        join d in context.SV_DC on a.idDC equals d.idDC
                        where a.idMD == b.idMD && a.maSV == maSV && a.idDC == idDC && d.maSV == maSV
                        select new SinhVien_BangDRL_MucDiem_SVDCViewModel()
                        {
                            id = a.id,
                            maSV = a.maSV,
                            tenSV = c.tenSV,
                            chucVu = c.chucVu,
                            idDC = a.idDC,
                            idMD = a.idMD,
                            noiDung = b.noiDung,
                            loaiMD = b.loaiMD,
                            diemMax = b.diemMax,
                            diemTDG = a.diemTDG,
                            diemLDG = a.diemLDG,
                            canMinhChung = b.canMinhChung,
                            minhChung = a.minhChung,
                            ttCham = d.ttCham,
                            tongDiem = d.tongDiem
                        };
            return model.OrderBy(x => x.idMD).ToList();
        }

        public string Edit(Bang_DRL entity)
        {
            var sp = Find(entity.id);
            if (sp == null)
            {
                context.Bang_DRL.Add(entity);
            }
            else
            {
                sp.minhChung = entity.minhChung;
            }
            context.SaveChanges();
            return entity.id.ToString();
        }
        public Bang_DRL Find(int id)
        {
            return context.Bang_DRL.Find(id);
        }

        public int check(string maSV, int idDC)
        {
            var tt = context.SV_DC.FirstOrDefault(x => x.maSV == maSV && x.idDC == idDC && x.ttCham == 0);
            if (tt != null)
                return 1;
            else
                return 0;
        }

        public int ChangeStatus(string maSV, int idDC)
        {
            var a = context.SV_DC.SingleOrDefault(x => x.maSV == maSV && x.idDC == idDC);
            if(a.ttCham == 0)
            {
                a.ttCham = 1;
            }else if (a.ttCham == 1)
            {
                a.ttCham = 2;
            }else
            {
                a.ttCham = 3;
            }
            
            context.SaveChanges();
            return a.ttCham;
        }

        public List<SPChiTietDRLViewModel> ListByDC(int idDC, string maSV)
        {
            var model = from a in context.Bang_DRL
                        join b in context.DanhMucDiems on a.idMD equals b.idMD
                        join c in context.DotChams on a.idDC equals c.idDC
                        join d in context.SV_DC on a.maSV equals d.maSV
                        where a.idMD == b.idMD && a.maSV == maSV && a.idDC == idDC && d.idDC == idDC
                        select new SPChiTietDRLViewModel()
                        {
                            id = a.id,
                            maSV = a.maSV,
                            idDC = a.idDC,
                            idMD = a.idMD,
                            noiDung = b.noiDung,
                            loaiMD = b.loaiMD,
                            diemMax = b.diemMax,
                            diemTDG = a.diemTDG,
                            diemLDG = a.diemLDG,
                            canMinhChung = b.canMinhChung,
                            minhChung = a.minhChung,
                            hocKy = c.hocKy,
                            tongDiem = d.tongDiem
                        };
            return model.OrderBy(x => x.idMD).ToList();
        }

        public List<DotCham> ListDC()
        {

            return context.DotChams.OrderByDescending(x => x.idDC).ToList();
        }

        public List<SinhVien_SVDCViewModel> ListBymaLop(int idDC, string maLop)
        {
            var model = from a in context.tblSinhViens
                        join b in context.SV_DC on a.maSV equals b.maSV
                        where a.maLop == maLop && b.idDC == idDC && b.ttCham == 1
                        select new SinhVien_SVDCViewModel()
                        {
                            maSV = a.maSV,
                            tenSV = a.tenSV,
                            maLop = a.maLop,
                            idDC = b.idDC,
                            ttCham = b.ttCham,
                            tongDiem = b.tongDiem
                        };
            return model.OrderBy(x => x.maSV).ToList();
        }
        public List<SinhVien_SVDCViewModel> ListBymaLop2(int idDC, string maLop)
        {
            var model = from a in context.tblSinhViens
                        join b in context.SV_DC on a.maSV equals b.maSV
                        where a.maLop == maLop && b.idDC == idDC && b.ttCham == 2
                        select new SinhVien_SVDCViewModel()
                        {
                            maSV = a.maSV,
                            tenSV = a.tenSV,
                            maLop = a.maLop,
                            idDC = b.idDC,
                            ttCham = b.ttCham,
                            tongDiem = b.tongDiem
                        };
            return model.OrderBy(x => x.maSV).ToList();
        }


        public List<Lop> ListALLLop()
        {
            IQueryable<Lop> model = context.Lops;

            return model.OrderBy(x => x.maLop).ToList();
        }
        public List<SinhVien_SVDCViewModel> ListALLBymaLop(int idDC, string maLop)
        {
            var model = from a in context.tblSinhViens
                        join b in context.SV_DC on a.maSV equals b.maSV
                        where a.maLop == maLop && b.idDC == idDC
                        select new SinhVien_SVDCViewModel()
                        {
                            maSV = a.maSV,
                            tenSV = a.tenSV,
                            maLop = a.maLop,
                            idDC = b.idDC,
                            ttCham = b.ttCham,
                            tongDiem = b.tongDiem
                        };
            return model.OrderBy(x => x.maSV).ToList();
        }



        public List<SinhVien_SVDCViewModel> ListALLBymaLop2(int idDC, string maLop)
        {
            var model = from a in context.tblSinhViens
                        join b in context.SV_DC on a.maSV equals b.maSV
                        where a.maLop == maLop && b.idDC == idDC
                        select new SinhVien_SVDCViewModel()
                        {
                            maSV = a.maSV,
                            tenSV = a.tenSV,
                            maLop = a.maLop,
                            idDC = b.idDC,
                            ttCham = b.ttCham,
                            tongDiem = b.tongDiem
                        };
            return model.OrderBy(x => x.ttCham).ToList();
        }

        public IEnumerable<SinhVien_SVDCViewModel> ListALLBymaLopPost(string keysearch,int idDC, string maLop)
        {
            var model = from a in context.tblSinhViens
                        join b in context.SV_DC on a.maSV equals b.maSV
                        where a.maLop == maLop && b.idDC == idDC
                        select new SinhVien_SVDCViewModel()
                        {
                            maSV = a.maSV,
                            tenSV = a.tenSV,
                            maLop = a.maLop,
                            idDC = b.idDC,
                            ttCham = b.ttCham,
                            tongDiem = b.tongDiem
                        };
            if (!string.IsNullOrEmpty(keysearch.ToString()))
                model = model.Where(x => x.tenSV.ToString().Contains(keysearch));
            return model.OrderBy(x => x.maSV).ToList();
        }

        public IEnumerable<DotCham> ListAllDCPost(string keysearch, int page,int pageSize)
        {
            //var da_dk = (from sv_hoatdong in context.sv_hoatdong where sv_hoatdong.tinhTrangDuyet == true select sv_hoatdong).Count();
            IQueryable<DotCham> model = context.DotChams;
            if (!string.IsNullOrEmpty(keysearch.ToString()))
                model = model.Where(x => x.hocKy.ToString().Contains(keysearch));
            return model.OrderByDescending(x => x.idDC).ToPagedList(page, pageSize);
        }

        public List<DotCham> ListAllDC()
        {
            IQueryable<DotCham> model = context.DotChams;

            return model.OrderByDescending(x => x.idDC).ToList();
        }

        public List<tblSinhVien> ListAllSV(string maLop)
        {
            IQueryable<tblSinhVien> model = context.tblSinhViens;
            model = model.Where(x => x.maLop == maLop);
            return model.OrderBy(x => x.maSV).ToList();
        }

        public IEnumerable<tblSinhVien> ListAllSVPost(string keysearch, int page, int pageSize, string maLop)
        {
            //var da_dk = (from sv_hoatdong in context.sv_hoatdong where sv_hoatdong.tinhTrangDuyet == true select sv_hoatdong).Count();
            IQueryable<tblSinhVien> model = context.tblSinhViens;

            model = model.Where(x => x.maLop == maLop);

            if (!string.IsNullOrEmpty(keysearch.ToString()))
                model = model.Where(x => x.tenSV.ToString().Contains(keysearch));
            return model.OrderBy(x => x.maSV).ToPagedList(page, pageSize);
        }

        public List<tblSinhVien> ListChiTietbyMaSV(string maSV)
        {
            IQueryable<tblSinhVien> model = context.tblSinhViens;

            model = model.Where(x => x.maSV == maSV);
            return model.OrderBy(x => x.maSV).ToList();
        }


        public DotCham FindDC(int id)
        {
            return context.DotChams.Find(id);
        }
        public string EditDC(DotCham entity)
        {
            var sp = FindDC(entity.idDC);
            if (sp == null)
            {
                context.DotChams.Add(entity);
            }
            else
            {
                sp.hocKy = entity.hocKy;
                sp.ngayCham = entity.ngayCham;
                sp.ngayKetThuc = entity.ngayKetThuc;
                sp.tieuDe = entity.tieuDe;
            }
            context.SaveChanges();
            return entity.idDC.ToString();
        }

        public int FindHK(int hocKy)
        {
            var tt = context.DotChams.FirstOrDefault(x => x.hocKy == hocKy);
            if (tt != null)
                return 1;
            else
                return 0;
        }
    }
}
