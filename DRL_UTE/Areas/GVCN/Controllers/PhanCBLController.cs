using DRL_UTE.Common;
using DRL_UTE.Controllers;
using ModelEF.Dao;
using ModelEF.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.GVCN.Controllers
{
    public class PhanCBLController : BaseController
    {
        // GET: GVCN/PhanCBL
        DBContext dbPro = new DBContext();
        DRLDao aDB = new DRLDao();

        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = 6)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];


            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            var maLop = session.maLop;
            var model = aDB.ListAllSV(maLop);
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 6)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            var maLop = session.maLop;
            var sp = new DRLDao();
            var model = sp.ListAllSVPost(searchString, page, pageSize, maLop);
            ViewBag.SearchString = searchString;
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpGet]
        public ActionResult Chitiet(string maSV)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var dao = new SinhVienDao();
                var result = dao.Find(maSV);
                if (result != null)
                    return View(result);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Chitiet(tblSinhVien sv)
        {
            if (sv.chucVu != "Other")
            {
                try
                {
                    var dao = new SinhVienDao();

                    string result = "";

                    result = dao.EditCVu(sv);

                    SetAlert("Cập nhật thành công ", "success");
                    return RedirectToAction("Index", "PhanCBL");

                }
                catch (Exception)
                {
                    SetAlert("Cập nhật thất bại", "error");
                }
            }else
            {
                SetAlert("Vui lòng nhập chúc vụ !!!", "error");
                return Redirect("/GVCN/PhanCBL/Chitiet?maSV=" + sv.maSV);
            }
            
            return View();
        }
    }
}