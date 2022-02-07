using java.io;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DRL_UTE.Common
{
    [Serializable]
    public class UserLogin
    {
        [Required(ErrorMessage = "Mời nhập UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mời nhập Password")]
        public string Password { get; set; }

        public string maLop { get; set; }
        public int Status { get; set; }
    }
}