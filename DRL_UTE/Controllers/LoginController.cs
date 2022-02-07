using DRL_UTE.Common;
using DRL_UTE.Models;
using ModelEF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.login(user.vaitro, user.UserName, user.Password);
                if (result == 99)
                {
                    ModelState.AddModelError("", "Bạn là cán bộ lớp mà !! Chọn lại mục đăng nhập đi");
                } 
                else if (result == 2)
                {
                    var dv = dao.GetBymaSV(user.UserName);
                    var userSS = new UserLogin();
                    userSS.UserName = dv.maSV;
                    userSS.maLop = dv.maLop;    
                    userSS.Status = dv.ttHoc;
                    Session.Add(Constants.USER_SESSION, userSS);
                    return Redirect("/SinhVien/Home/Index");
                }
                else if (result == 1)
                {
                    ModelState.AddModelError("", "Tai khoản của bạn đang bị khóa");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
                }
                else if (result == 6)
                {
                    var dv = dao.GetBymaSV(user.UserName);
                    var userSS = new UserLogin();
                    userSS.UserName = dv.maSV;
                    userSS.maLop = dv.maLop;
                    userSS.Status = dv.ttHoc;
                    Session.Add(Constants.USER_SESSION, userSS);
                    return Redirect("/CanBoLop/Home/Index");
                }
                else if (result == 5)
                {
                    ModelState.AddModelError("", "Bạn Không phải cán bộ lớp");
                }
                else if (result == 4)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn đang bị khóa");
                }
                else if (result == 3)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
                }
                else if (result == 9)
                {
                    var dv = dao.GetBymaTK(user.UserName);
                    var userSS = new UserLogin();
                    userSS.UserName = dv.maTK;
                    userSS.maLop = dv.chuNhiemLop;
                    userSS.Status = dv.ttTaiKhoan;
                    Session.Add(Constants.USER_SESSION, userSS);
                    return Redirect("/GVCN/Home/Index");
                }
                else if (result == 8)
                {
                    ModelState.AddModelError("", "Bạn Không phải GVCN");
                }
                else if (result == 7)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
                }
                else if (result == 12)
                {
                    var dv = dao.GetBymaTK(user.UserName);
                    var userSS = new UserLogin();
                    userSS.UserName = dv.maTK;
                    Session.Add(Constants.USER_SESSION, userSS);
                    return Redirect("/PhongDoan/Home/Index");
                }
                else if (result == 11)
                {
                    ModelState.AddModelError("", "Bạn Không thuộc Phòng Đoàn");
                }
                else if (result == 10)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
                }
                else if (result == 15)
                {
                    var dv = dao.GetBymaTK(user.UserName);
                    var userSS = new UserLogin();
                    userSS.UserName = dv.maTK;
                    Session.Add(Constants.USER_SESSION, userSS);
                    return Redirect("/PhongCTSV/Home/Index");
                }
                else if (result == 14)
                {
                    ModelState.AddModelError("", "Bạn Không thuộc Phòng CTSV");
                }
                else if (result == 13)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
                }
                else if (result == 18)
                {
                    var dv = dao.GetBymaTK(user.UserName);
                    var userSS = new UserLogin();
                    userSS.UserName = dv.maTK;
                    Session.Add(Constants.USER_SESSION, userSS);
                    return Redirect("/PhongDaoTao/Home/Index");
                }
                else if (result == 17)
                {
                    ModelState.AddModelError("", "Bạn Không thuộc Phòng Đào Tạo");
                }
                else if (result == 16)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
                }
                else if (result == 21)
                {
                    var dv = dao.GetBymaTK(user.UserName);
                    var userSS = new UserLogin();
                    userSS.UserName = dv.maTK;
                    Session.Add(Constants.USER_SESSION, userSS);
                    return Redirect("/PhongKHTC/Home/Index");
                }
                else if (result == 20)
                {
                    ModelState.AddModelError("", "Bạn Không thuộc Phòng Kế Hoạch Tài Chính");
                }
                else if (result == 19)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
                }
                else 
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                }
            }
            return View("Index");
        }
    }
}