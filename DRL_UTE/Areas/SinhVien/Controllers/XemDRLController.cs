using DRL_UTE.Areas.SinhVien.Model;
using DRL_UTE.Common;
using DRL_UTE.Controllers;
using ModelEF.Dao;
using ModelEF.Model;
using ModelEF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.SinhVien.Controllers
{
    public class XemDRLController : BaseController
    {
        // GET: SinhVien/XemDRL
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
            var model = aDB.ListByDC(0,maSV);
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
            ViewBag.hocKy = new SelectList(dao.ListDC(),"idDC","hocKy", idDC);
        }
    }
}