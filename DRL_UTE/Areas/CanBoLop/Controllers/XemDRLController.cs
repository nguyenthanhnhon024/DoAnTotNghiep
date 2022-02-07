using DRL_UTE.Common;
using ModelEF.Dao;
using ModelEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.CanBoLop.Controllers
{
    public class XemDRLController : Controller
    {
        // GET: CanBoLop/XemDRL
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
            var maSV = session.UserName;
            SetViewBag();
            var model = aDB.ListByDC(0, maSV);
            return View(model);

        }

        [HttpPost]
        public ActionResult Index(int key)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            var maSV = session.UserName;
            var idDC = key;
            SetViewBag();
            var model = aDB.ListByDC(idDC, maSV);
            return View(model);


        }

        public void SetViewBag(long? idDC = null)
        {
            var dao = new DRLDao();
            ViewBag.hocKy = new SelectList(dao.ListDC(), "idDC", "hocKy", idDC);
        }
    }
}