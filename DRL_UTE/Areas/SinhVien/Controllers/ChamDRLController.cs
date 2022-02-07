using DRL_UTE.Common;
using ModelEF.Dao;
using ModelEF.Model;
using ModelEF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DRL_UTE.Areas.SinhVien.Controllers
{
    public class ChamDRLController : Controller
    {
        // GET: SinhVien/ChamDRL
        DBContext dbPro = new DBContext();
        DRLDao aDB = new DRLDao();

        [HandleError]
        [HttpGet]
        public ActionResult Index()
        {
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            if (session2 == null) return Redirect("/SinhVien/DRL/Index");
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
                return Redirect("/SinhVien/DRL/Index");
            } else
            {
                var maSV = session.UserName;
                var idDC = session2.idDC;
                ViewBag.ProError = error;
                var model = aDB.ListByidMD(idDC, maSV);
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
            var model = aDB.ListByidMD(idDC, maSV);
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
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}