using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyBanQuanAo.Areas.Admin.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public string Password { get; set; }
        public bool QuanLy { get; set; }
        public bool RememberMe { get; set; }
    }
}