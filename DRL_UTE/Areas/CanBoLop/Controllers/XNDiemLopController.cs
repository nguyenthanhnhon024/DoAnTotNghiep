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
    public class XNDiemLopController : BaseController
    {
        // GET: CanBoLop/XNDiemLop
        DBContext dbPro = new DBContext();
        DRLDao aDB = new DRLDao();

        [HandleError]
        [HttpGet]
        public ActionResult Index()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var model = aDB.ListAll();
            return View(model);
        }

        public ActionResult Details(int idDC)
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

                var rs = aDB.check(maSV, userSS.idDC);
                if (rs == 1) 
                {
                    SetAlert("Vui lòng chấm điểm cá nhân trước khi duyệt điểm của lớp", "error");
                    return RedirectToAction("Index", "ChamDRL2");
                }   
                else
                {
                    
                    var model = aDB.ListBymaLop(idDC, maLop);
                    return View(model);
                }
            }
        }

        [HttpGet]
        public ActionResult Chitiet(string maSV)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null) { return Redirect("/Login/Index"); }
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            var idDC = session2.idDC;
            Session.Add(SinhVienXN.XN_SESSION, maSV);
            var model = aDB.ListChiTietSV(idDC, maSV);
            return View(model);
        }

        [HttpPost]
        public JsonResult Update(string model)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Bang_DRL bang_DRL = serializer.Deserialize<Bang_DRL>(model);

            //save db
            var entity = dbPro.Bang_DRL.Find(bang_DRL.id);
            entity.diemLDG = bang_DRL.diemLDG;
            dbPro.SaveChanges();
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus()
        {
            var maSV = Session[SinhVienXN.XN_SESSION].ToString();
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            var idDC = session2.idDC;
            var result = new DRLDao().ChangeStatus(maSV.Trim(), idDC);
            return Json(new { status = result });
        }

        [HandleError]
        [HttpGet]

        public ActionResult Xacnhan(string maSV)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            var session2 = (CTDotCham)Session[DotChamHT.DC_SESSION];
            var idDC = session2.idDC;
            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            //var maSV = session.UserName;
            var model = aDB.ListByDC(idDC, maSV);
            return View(model);


        }
    }
}