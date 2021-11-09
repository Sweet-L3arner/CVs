using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanQuanAo.Areas.NhanViens.Model
{
    public class LoginModel
    {
        public string UsernameLoginNV { get; set; }
        public string PasswordLoginNV { get; set; }
        public bool QuanLy { get; set; }
        public bool RememberMe { get; set; }
    }
}