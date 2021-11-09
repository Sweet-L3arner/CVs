using CryptoLib;
using QuanLyBanQuanAo.Areas.Admin.Model;
using QuanLyBanQuanAo.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace QuanLyBanQuanAo.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        private static DateTime LayThoiGian()
        {
            return DateTime.Now;
        }

        // GET: Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Admin = 1;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model, NhatKy nhatKy)
        {
            //var result = new AccountModel().Login(model.UsernameLoginA, Encryptor.MD5Hash(model.PasswordLoginA), model.QuanLy);

            // Kiểm tra có tồn tại tài khoản không?
            string matKhau = Encryptor.MD5Hash(model.Password);
            var result = _db.NhanViens.SingleOrDefault(tk => tk.Email == model.Email && tk.MatKhau == matKhau);

            Session["Staff"] = model.Email;
            Session["QuanLy"] = model.QuanLy;

            // Check xem đây có phải là tài khoản của quản lý hay không.
            if (ModelState.IsValid && result != null && result.QuanLy == true)
            {
                FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

                // Lưu nhật ký đăng nhập của quản lý.
                nhatKy.Username = model.Email;
                nhatKy.TinhTrang = "Đăng nhập - Quản lý";
                nhatKy.GhiNho = LayThoiGian();
                _db.NhatKies.Add(nhatKy);
                _db.SaveChanges();

                return RedirectToAction("Index", "HomeAdmin");
            }
            // Check xem đây có phải là tài khoản của nhân viên hay không.
            else if (ModelState.IsValid && result != null && result.QuanLy == false)
            {
                FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

                // Lưu nhật ký đăng xuất của nhân viên.
                nhatKy.Username = model.Email;
                nhatKy.TinhTrang = "Đăng nhập - Nhân viên";
                nhatKy.GhiNho = LayThoiGian();
                _db.NhatKies.Add(nhatKy);
                _db.SaveChanges();

                return RedirectToAction("Index", "HomeNhanVien", new { area = "NhanViens" });
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
            }

            return View(model);
        }

        public ActionResult Logout(NhatKy nhatKy)
        {
            FormsAuthentication.SignOut();

            nhatKy.Username = Session["Staff"].ToString();
            nhatKy.TinhTrang = "Đăng xuất";
            nhatKy.GhiNho = LayThoiGian();
            _db.NhatKies.Add(nhatKy);
            _db.SaveChanges();

            Session["Staff"] = null;
            Session["QuanLy"] = null;

            return RedirectToAction("Index", "Login");
        }
    }
}