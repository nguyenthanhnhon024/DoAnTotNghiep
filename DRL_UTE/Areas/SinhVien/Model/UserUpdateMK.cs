using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DRL_UTE.Areas.SinhVien.Model
{
    public class UserUpdateMK
    {
        [Required(ErrorMessage = "Mời nhập mật khẩu hiện tại")]
        public string matKhau { get; set; }
        public string mk1 { get; set; }

        public string mk2 { get; set; }
    }
}