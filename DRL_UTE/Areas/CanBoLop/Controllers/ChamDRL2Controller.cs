using DRL_UTE.Common;
using DRL_UTE.Controllers;
using ModelEF.Dao;
using ModelEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DRL_UTE.Areas.CanBoLop.Controllers
{
    public class ChamDRL2Controller : BaseController
    {
        // GET: CanBoLop/ChamDRL2
        DBContext dbPro = new DBContext();
        DRLDao aDB = new DRLDao();

        [HandleError]
        [HttpGet]
        public ActionResult Index()
        {
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            if (session2 == null) return Redirect("/CanBoLop/DRL2/Index");
            return View();
        }

        [HttpGet]
        public ActionResult Xacnhan(string error, string name)
        {
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else if (session2 == null)
            {
                return Redirect("/CanBoLop/DRL2/Index");
            }
            else
            {
                var maSV = session.UserName;
                var idDC = session2.idDC;
                ViewBag.ProError = error;
                var model = aDB.ListByidMD2(idDC, maSV);
                return View(model);
            }

        }

        [HttpGet]
        public JsonResult LoadData()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            var maSV = session.UserName;
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            var idDC = session2.idDC;
            var model = aDB.ListByidMD2(idDC, maSV);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(string model)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Bang_DRL bang_DRL = serializer.Deserialize<Bang_DRL>(model);

            //save db
            var entity = dbPro.Bang_DRL.Find(bang_DRL.id);
            entity.diemTDG = bang_DRL.diemTDG;
            dbPro.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            var maSV = session.UserName;
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            var idDC = session2.idDC;
            var result = new DRLDao().ChangeStatus(maSV.Trim(), idDC);
            return Json(new { status = result });
        }
    }
}