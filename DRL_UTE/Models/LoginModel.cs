using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DRL_UTE.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Mời nhập tài khoản và mật khẩu")]
        public string UserName { get; set; }
        public string Password { get; set; }

        public string maLop { get; set; }
        public int Status { get; set; }

        public string vaitro { get; set; }
    }
    public enum vaitro
    {
        Sinh_Viên, Cán_Bộ_Lớp, Giáo_Viên_Chủ_Nhiệm, Phòng_Đoàn, Phòng_CTSV, Phòng_Đào_Tạo, Phòng_KH_Tài_Chính
    }
}