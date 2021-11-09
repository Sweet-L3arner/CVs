using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyBanQuanAo.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string EmailLogin { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string MatKhauLogin { get; set; }

        public bool RememberMe { get; set; }
    }
}