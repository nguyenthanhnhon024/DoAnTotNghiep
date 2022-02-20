using DRL_UTE.Common;
using DRL_UTE.Controllers;
using ModelEF.Dao;
using ModelEF.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.PhongCTSV.Controllers
{
    public class CNTaiKhoanController : BaseController
    {
        // GET: PhongCTSV/CNTaiKhoan
        DBContext dbPro = new DBContext();
        TaiKhoanPBDao aDB = new TaiKhoanPBDao();

        [HandleError]
        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = 6)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            var model = aDB.ListAll();
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
            var sp = new TaiKhoanPBDao();
            var model = sp.ListAllPost(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var result = new TaiKhoanPBDao().ChangeStatus(id.Trim());
            return Json(new { status = result });
        }

        [HandleError]
        [HttpGet]
        public ActionResult Details(string maTK)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var dao = new PhongBanDao();
                var result = dao.Find(maTK);
                if (result != null)
                    return View(result);
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(string maTK)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            Session.Add(SinhVienXN.XN_SESSION, maTK);
            //SetViewBag();
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                TaiKhoanPB taikhoan = dbPro.TaiKhoanPBs.Find(maTK);
                var dao = new PhongBanDao();
                var result = dao.Find(maTK);
                ViewBag.chuNhiemLop = new SelectList(dbPro.Lops, "maLop", "maLop", taikhoan.chuNhiemLop);
                if (result != null)
                    return View(result);
                return View(result);
            }
        }


        [HttpPost]
        public ActionResult Edit(TaiKhoanPB editPro)
            {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            //SetViewBag();
            //TaiKhoanPB taikhoan = dbPro.TaiKhoanPBs.Find(maTK);

            var ma = Session[SinhVienXN.XN_SESSION];

            ViewBag.chuNhiemLop = new SelectList(dbPro.Lops, "maLop", "maLop");
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var sv = aDB.Find(editPro.maTK);
              
                if (editPro.SDT != null)
                {
                    editPro.SDT = editPro.SDT.Trim();
                    if (IsNumber(editPro.SDT) != true)
                    {
                        SetAlert("Vui lòng nhập số có 10 hoặc 11 kí tự", "error");
                        //return RedirectToAction("Index", "CNTaiKhoan");
                        return Redirect("/PhongCTSV/CNTaiKhoan/Edit?maTK=" + ma);
                    }
                    if (editPro.SDT.Length == 10 || editPro.SDT.Length == 11)
                    {
                        try
                        {                           
                            editPro.avatar = sv.avatar;
                            aDB.Edit(editPro);
                            SetAlert("Cập nhật thành công ", "success");
                            return RedirectToAction("Index", "CNTaiKhoan");
                            //return Redirect("/PhongCTSV/CNTaiKhoan/Edit?maTK=" + ma);
                        }
                        catch (Exception)
                        {
                            SetAlert("Cập nhật thất bại", "error");
                            return Redirect("/PhongCTSV/CNTaiKhoan/Edit?maTK=" + ma);
                        }
                    }
                    else
                    {
                        SetAlert("Vui lòng nhập số có 10 hoặc 11 kí tự", "error");
                        //return RedirectToAction("Index", "CNTaiKhoan");
                        return Redirect("/PhongCTSV/CNTaiKhoan/Edit?maTK=" + ma);
                    }
                }
                else
                {
                    try
                    {
                        editPro.avatar = sv.avatar;
                        aDB.Edit(editPro);
                        SetAlert("Cập nhật thành công ", "success");
                        return RedirectToAction("Index", "CNTaiKhoan");
                        //return Redirect("/PhongCTSV/CNTaiKhoan/Edit?maTK=" + ma);
                    }
                    catch (Exception)
                    {
                        SetAlert("Cập nhật thất bại", "error");
                        return Redirect("/PhongCTSV/CNTaiKhoan/Edit?maTK=" + ma);
                    }
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];

            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            ViewBag.chuNhiemLop = new SelectList(dbPro.Lops, "maLop", "maLop");
            return View();
        }

        [HttpPost]
        public ActionResult Create(TaiKhoanPB model)
        {
            DateTime now = DateTime.Now;
            var session = (UserLogin)Session[Constants.USER_SESSION];
            ViewBag.chuNhiemLop = new SelectList(dbPro.Lops, "maLop", "maLop");


            if (session == null)
            {
                return Redirect("/Login/Index");
            }

            try
            {
                if (model.maTK == null || model.hoTen == null || model.maPB == null || model.gioiTinh == null)
                {
                    SetAlert("Vui lòng nhập đầy đủ các thông tin", "error");
                    return View();
                }
                else
                {   if (model.maPB == 1 && model.chuNhiemLop != null)
                    {
                        if (aDB.FindmaTK(model.maTK) == 0)
                        {
                            model.ttTaiKhoan = 1;
                            model.matKhau = "123456";
                            aDB.Edit(model);
                            SetAlert("Thêm mới thành công", "success");
                            return RedirectToAction("Index", "CNTaiKhoan");
                        }
                        else
                        {
                            SetAlert("Lớp này đã tồn tại", "error");
                            return View();
                        }
                    } else if(model.maPB != 1)
                    {
                        if (aDB.FindmaTK(model.maTK) == 0)
                        {
                            model.ttTaiKhoan = 1;
                            model.matKhau = "123456";
                            model.chuNhiemLop = null;
                            aDB.Edit(model);
                            SetAlert("Thêm mới thành công", "success");
                            return RedirectToAction("Index", "CNTaiKhoan");
                        }
                        else
                        {
                            SetAlert("Lớp này đã tồn tại", "error");
                            return View();
                        }
                    }
                }
            }
            catch (Exception)
            {
                SetAlert("Thêm mới thất bại", "error");
            }
            return View();
        }

        public void SetViewBag(string maLop = null)
        {
            ViewBag.chuNhiemLop = new SelectList(dbPro.Lops, "maLop", "maLop",maLop);
        }
        private static bool IsNumber(string val)
        {
            if (val != "")
                return Regex.IsMatch(val, @"^[0-9]\d*\.?[0]*$");
            else return true;
        }
    }
}