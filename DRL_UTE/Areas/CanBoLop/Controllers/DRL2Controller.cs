using DRL_UTE.Common;
using DRL_UTE.Controllers;
using ModelEF.Dao;
using ModelEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.CanBoLop.Controllers
{
    public class DRL2Controller : BaseController
    {
        // GET: CanBoLop/DRL
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

        public ActionResult Details(int idDC)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            
            if (session == null) { return Redirect("/Login/Index"); }
            else
            {
                var maSV = session.UserName;
                var userSS = new CTDotCham();
                userSS.idDC = idDC;
                Session.Add(DotChamHT.DC_SESSION, userSS);
                var rs = aDB.check(maSV, userSS.idDC);
                if (rs == 1)
                    return RedirectToAction("Index", "ChamDRL2");
                else
                {
                    SetAlert("Bạn đã chấm rồi! Không thể thực hiện lại", "error");
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new DRLDao();
            var result = dao.Find(id);
            if (result != null)
                return View(result);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Bang_DRL sp, HttpPostedFileBase image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new DRLDao();

                    if (image == null)
                    {
                        SetAlert("Vui lòng chọn ảnh", "error");
                    }
                    string fileName = System.IO.Path.GetFileName(image.FileName);
                    string urlImage = Server.MapPath("~/assets/imgMC/" + fileName);
                    image.SaveAs(urlImage);
                    sp.minhChung = fileName;

                    string result = "";

                    result = dao.Edit(sp);
                    if (!string.IsNullOrEmpty(result))
                    {
                        SetAlert("Cập nhật thành công", "success");
                        return RedirectToAction("Index", "ChamDRL2");
                    }
                    else
                    {
                        SetAlert("Cập nhật thất bại", "error");
                        //ModelState.AddModelError("", "Tạo mới thất bại");
                    }
                }

            }
            catch (Exception)
            {
            }
            return View();
        }

        [HttpPost]
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                SetAlert("Vui lòng chọn ảnh", "error");
            }
            string fileName = System.IO.Path.GetFileName(file.FileName);
            string urlImage = Server.MapPath("~/assets/imgMC/" + fileName);
            file.SaveAs(urlImage);
            return "/assets/imgMC/" + file.FileName;
        }
    }
}