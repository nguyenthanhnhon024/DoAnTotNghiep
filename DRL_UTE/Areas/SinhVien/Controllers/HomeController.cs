using DRL_UTE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRL_UTE.Areas.SinhVien.Controllers
{
    public class HomeController : Controller
    {
        // GET: SinhVien/Home
        public ActionResult Index()
        {
            //Về Trang Login nếu chưa đăng nhập
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null) return Redirect("/Login/Index");
            //------------------------------------------------------
            return View();
        }
    }
}