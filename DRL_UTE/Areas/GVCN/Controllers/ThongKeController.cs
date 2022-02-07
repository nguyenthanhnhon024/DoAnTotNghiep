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
    public class ThongKeController : BaseController
    {
        // GET: GVCN/ThongKe
        DBContext dbPro = new DBContext();
        DRLDao aDB = new DRLDao();

        [HandleError]
        [HttpGet]
        public ActionResult Index(string error, string name)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
                ViewBag.ProError = error;
            var model = aDB.ListAll();
            return View(model);

        }

        public ActionResult Details(int idDC, int page = 1, int pageSize = 10)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null) { return Redirect("/Login/Index"); }
            else
            {
                var maLop = session.maLop;
                var maSV = session.UserName;
                var userSS = new CTDotCham();
                userSS.idDC = idDC;
                Session.Add(DotChamHT.DC_SESSION, userSS);
                var model = aDB.ListALLBymaLop2(idDC, maLop);
                return View(model.ToPagedList(page, pageSize));
            }
        }
    }
}