using ModelEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModelEF.Dao
{
    public class UserDao
    {
        private DBContext db;
        public UserDao()
        {
            db = new DBContext();
        }
        public int login(string row, string user, string pass)
        {
            if(row == "Sinh_Viên")
            {
                var result = db.tblSinhViens.SingleOrDefault(x => x.maSV == user && x.matKhau == pass);
                var tt = db.tblSinhViens.FirstOrDefault(x => x.maSV == user && x.ttHoc == 1);
                var cb = db.tblSinhViens.FirstOrDefault(x => x.maSV == user && x.chucVu == "LT");
                if (cb != null)
                {
                    return 99;
                }
                else if (result == null)
                {
                    return 0;
                }
                else if (tt == null)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            } else if(row == "Cán_Bộ_Lớp") {
                var result = db.tblSinhViens.SingleOrDefault(x => x.maSV == user && x.matKhau == pass);
                var tt = db.tblSinhViens.FirstOrDefault(x => x.maSV == user && x.ttHoc == 1);
                var cb = db.tblSinhViens.FirstOrDefault(x => x.maSV == user && x.chucVu == "LT");
                if (result == null)
                {
                    return 3;
                }
                else if (tt == null)
                {
                    return 4;
                }
                else if (cb == null)
                {
                    return 5;
                }
                else
                {
                    return 6;
                }
            }
            else if (row == "Giáo_Viên_Chủ_Nhiệm"){
                var result = db.TaiKhoanPBs.SingleOrDefault(x => x.maTK == user && x.matKhau == pass);
                var cnl = db.TaiKhoanPBs.FirstOrDefault(x => x.maTK == user && x.chuNhiemLop == "KhongCN");
                var cnl2 = db.TaiKhoanPBs.FirstOrDefault(x => x.maTK == user && x.chuNhiemLop == null);
                if (result == null)
                {
                    return 7;
                }
                else if (cnl != null | cnl2 != null)
                {
                    return 8;
                }
                else
                {
                    return 9;
                }
            }
            else if (row == "Phòng_Đoàn")
            {
                var result = db.TaiKhoanPBs.SingleOrDefault(x => x.maTK == user && x.matKhau == pass);
                var pb = db.TaiKhoanPBs.FirstOrDefault(x => x.maTK == user && x.maPB == 2);
                if (result == null)
                {
                    return 10;
                }
                else if (pb == null)
                {
                    return 11;
                }
                else
                {
                    return 12;
                }
            }
            else if (row == "Phòng_CTSV")
            {
                var result = db.TaiKhoanPBs.SingleOrDefault(x => x.maTK == user && x.matKhau == pass);
                var pb = db.TaiKhoanPBs.FirstOrDefault(x => x.maTK == user && x.maPB == 3);
                if (result == null)
                {
                    return 13;
                }
                else if (pb == null)
                {
                    return 14;
                }
                else
                {
                    return 15;
                }
            }
            else if (row == "Phòng_Đào_Tạo")
            {
                var result = db.TaiKhoanPBs.SingleOrDefault(x => x.maTK == user && x.matKhau == pass);
                var pb = db.TaiKhoanPBs.FirstOrDefault(x => x.maTK == user && x.maPB == 5);
                if (result == null)
                {
                    return 16;
                }
                else if (pb == null)
                {
                    return 17;
                }
                else
                {
                    return 18;
                }
            }
            else if (row == "Phòng_KH_Tài_Chính")
            {
                var result = db.TaiKhoanPBs.SingleOrDefault(x => x.maTK == user && x.matKhau == pass);
                var pb = db.TaiKhoanPBs.FirstOrDefault(x => x.maTK == user && x.maPB == 4);
                if (result == null)
                {
                    return 19;
                }
                else if (pb == null)
                {
                    return 20;
                }
                else
                {
                    return 21;
                }
            }
            return 100;
            
            //DateTime now = DateTime.Now;
            
        }
        public tblSinhVien GetBymaSV(string ma)
        {
            return db.tblSinhViens.SingleOrDefault(x => x.maSV == ma);
        }
        public TaiKhoanPB GetBymaTK(string ma)
        {
            return db.TaiKhoanPBs.SingleOrDefault(x => x.maTK == ma);
        }
    }
}
