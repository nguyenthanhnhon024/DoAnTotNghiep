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
    public class CNDotChamController : BaseController
    {
        // GET: PhongCTSV/CNDotCham
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
        public ActionResult Create()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(DotCham model)
        {
            DateTime now = DateTime.Now;
            
            try
            {   if (model.hocKy == null || model.tieuDe == null || model.ngayCham == null || model.ngayKetThuc == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                } else if(model.ngayCham > model.ngayKetThuc || model.ngayKetThuc < now)
                {
                    SetAlert("Vui lòng kiểm tra lại ngày tháng nhập vào", "error");
                    return View();
                }
                else
                {
                    if (aDB.FindHK(model.hocKy) == 0)
                    {
                        aDB.EditDC(model);
                        SetAlert("Thêm mới thành công", "success");
                        return RedirectToAction("Index", "CNDotCham");
                    }
                    else
                    {
                        SetAlert("Học kỳ này đã tồn tại", "error");
                        return View();
                    }
                    
                }     
            }
            catch (Exception)
            {
                SetAlert("Thêm mới thất bại", "error");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int idDC)
        {
            var dao = new DRLDao();
            var result = dao.FindDC(idDC);
            if (result != null)
                return View(result);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(DotCham model)
        {
            DateTime now = DateTime.Now;

            try
            {
                if (model.hocKy == null || model.tieuDe == null || model.ngayCham == null || model.ngayKetThuc == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                }
                else if (model.ngayCham > model.ngayKetThuc || model.ngayKetThuc < now)
                {
                    SetAlert("Vui lòng kiểm tra lại ngày tháng nhập vào", "error");
                    return View();
                }
                else
                {

                        aDB.EditDC(model);
                        SetAlert("Chỉnh sửa thành công", "success");
                        return RedirectToAction("Index", "CNDotCham");


                }
            }
            catch (Exception)
            {
                SetAlert("Cập nhật thất bại", "error");
            }
            return View();
        }

    }
}