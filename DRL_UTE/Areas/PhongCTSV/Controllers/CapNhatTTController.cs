using DRL_UTE.Areas.SinhVien.Model;
using DRL_UTE.Common;
using DRL_UTE.Controllers;
using ModelEF.Dao;
using ModelEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.PhongCTSV.Controllers
{
    public class CapNhatTTController : BaseController
    {
        // GET: PhongCTSV/CapNhatTT
        DBContext dbPro = new DBContext();
        PhongBanDao aDB = new PhongBanDao();

        [HandleError]
        [HttpGet]
        public ActionResult Index()
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var maTK = session.UserName;
                var dao = new PhongBanDao();
                var result = dao.Find(maTK);
                if (result != null)
                    return View(result);
                return View();
            }
        }


        [HandleError]
        [HttpPost]
        public ActionResult Index(TaiKhoanPB editPro, HttpPostedFileBase image)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var sv = aDB.Find(editPro.maTK);

                if (editPro.Email != null)
                {
                    if (IsEmail(editPro.Email) != true)
                    {
                        SetAlert("Email không hợp lệ", "error");
                        return RedirectToAction("Index", "CapNhatTT");
                    }
                }
                if (editPro.SDT != null)
                {
                    editPro.SDT = editPro.SDT.Trim();
                    if (IsNumber(editPro.SDT) != true)
                    {
                        SetAlert("Vui lòng nhập số có 10 hoặc 11 kí tự", "error");
                        return RedirectToAction("Index", "CapNhatTT");
                    }
                    if (editPro.SDT.Length == 10 || editPro.SDT.Length == 11)
                    {
                        try
                        {
                            if (image != null)
                            {
                                string fileName = System.IO.Path.GetFileName(image.FileName);
                                string urlImage = Server.MapPath("~/assets/imgMC/" + fileName);
                                image.SaveAs(urlImage);
                                editPro.avatar = fileName;
                                aDB.Edit(editPro);
                                SetAlert("Cập nhật thành công ", "success");
                                return RedirectToAction("Index", "CapNhatTT");
                            }
                            else
                            {
                                editPro.avatar = sv.avatar;
                                aDB.Edit(editPro);
                                SetAlert("Cập nhật thành công ", "success");
                                return RedirectToAction("Index", "CapNhatTT");
                            }
                        }
                        catch (Exception)
                        {
                            SetAlert("Cập nhật thất bại", "error");
                        }
                    }
                    else
                    {
                        SetAlert("Vui lòng nhập số có 10 hoặc 11 kí tự", "error");
                        return RedirectToAction("Index", "CapNhatTT");
                    }
                }
                else
                {
                    try
                    {
                        if (image != null)
                        {
                            string fileName = System.IO.Path.GetFileName(image.FileName);
                            string urlImage = Server.MapPath("~/assets/imgMC/" + fileName);
                            image.SaveAs(urlImage);
                            editPro.avatar = fileName;
                            aDB.Edit(editPro);
                            SetAlert("Cập nhật thành công ", "success");
                            return RedirectToAction("Index", "CapNhatTT");
                        }
                        else
                        {
                            editPro.avatar = sv.avatar;
                            aDB.Edit(editPro);
                            SetAlert("Cập nhật thành công ", "success");
                            return RedirectToAction("Index", "CapNhatTT");
                        }
                    }
                    catch (Exception)
                    {
                        SetAlert("Cập nhật thất bại", "error");
                    }
                }
                return View();
            }
        }


        public ActionResult UpdateMK()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]

        public ActionResult UpdateMK(UserUpdateMK userud)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
            {
                return Redirect("/Login/Index");
            }
            else
            {
                var maTK = session.UserName;
                var dao = new PhongBanDao();
                var result = dao.doimatkhau(maTK, userud.matKhau, userud.mk1, userud.mk2);
                if (result == 0)
                {
                    SetAlert("Mật khẩu hiện tại không chính xác", "error");
                }
                else if (result == 1)
                {
                    SetAlert("Đổi mật khẩu thành công ", "success");
                    return RedirectToAction("UpdateMK", "CapNhatTT");
                }
                else if (result == 2)
                {
                    SetAlert("Mật khẩu mới không khớp", "error");
                }
                else
                {
                    SetAlert("Cập nhật thất bại", "error");
                }
                return View("UpdateMK");
            }
        }
        private static bool IsNumber(string val)
        {
            if (val != "")
                return Regex.IsMatch(val, @"^[0-9]\d*\.?[0]*$");
            else return true;
        }

        public static bool IsEmail(string email)
        {
            if (email != "")
                return Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            else return true;
        }
    }
}