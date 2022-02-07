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
    public class XemDRLLopController : BaseController
    {
        // GET: GVCN/XemDRLLop
        DBContext dbPro = new DBContext();
        DRLDao aDB = new DRLDao();

        [HandleError]
        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = 6)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var model = aDB.ListAllDC();
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
            var sp = new DRLDao();
            var model = sp.ListAllDCPost(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpGet]
        public ActionResult Details(int idDC, int page = 1, int pageSize = 6)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];


            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var userSS = new CTDotCham();
            userSS.idDC = idDC;
            Session.Add(DotChamHT.DC_SESSION, userSS);

            var maLop = session.maLop;
            var model = aDB.ListALLBymaLop(idDC, maLop);
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public ActionResult Details(string searchString, int page = 1, int pageSize = 6)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            if (session2 == null) { return Redirect("/Login/Index"); }

            var maLop = session.maLop;
            var idDC = session2.idDC;
            var sp = new DRLDao();
            var model = sp.ListALLBymaLopPost(searchString, page, pageSize, idDC, maLop);
            ViewBag.SearchString = searchString;
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpGet]
        public ActionResult Chitiet(string maSV)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null) { return Redirect("/Login/Index"); }
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            if (session2 == null) { return Redirect("/Login/Index"); }
            var idDC = session2.idDC;
            var model = aDB.ListChiTietSV2(idDC, maSV);
            return View(model);
        }
    }
}