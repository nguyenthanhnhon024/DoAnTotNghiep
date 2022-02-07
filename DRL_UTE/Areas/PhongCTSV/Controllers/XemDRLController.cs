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

namespace DRL_UTE.Areas.PhongCTSV.Controllers
{
    public class XemDRLController : BaseController
    {
        // GET: PhongCTSV/XemDRL
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

            SetViewBag();

            var model = aDB.ListALLBymaLop(0, "0");
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchString,string key, int key2, int page = 1, int pageSize = 6)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }


            var maLop = key;
            var idDC = key2;
            SetViewBag();

            var sp = new DRLDao();
            var model = sp.ListALLBymaLopPost(searchString, page, pageSize, idDC, maLop);
            ViewBag.SearchString = searchString;
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpGet]
        public ActionResult Chitiet(int idDC ,string maSV)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null) { return Redirect("/Login/Index"); }

            var model = aDB.ListChiTietSV2(idDC, maSV);
            return View(model);
        }
        public void SetViewBag()
        {
            var dao = new DRLDao();
            ViewBag.maLop = new SelectList(dao.ListALLLop(), "maLop", "maLop");

            ViewBag.hocKy = new SelectList(dao.ListDC(), "idDC", "hocKy");
        }
    }
}