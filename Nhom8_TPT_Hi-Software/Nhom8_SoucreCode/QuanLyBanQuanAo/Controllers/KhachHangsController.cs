using CryptoLib;
using QuanLyBanQuanAo.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace QuanLyBanQuanAo.Controllers
{
    public class KhachHangsController : Controller
    {

        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // Thông tin khách hàng.
        public ActionResult ThongTinKhachHang(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = _db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // Chỉnh sửa thông tin khách hàng.
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = _db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/KhachHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKhachHang,Ho,Ten,GioiTinh,SDT,Email,MatKhau,TinhOrTP,DiaChi")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(khachHang).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("ThongTinKhachHang", new { @id = Session["MaKhachHang"] });
            }
            return View(khachHang);
        }

        // Lấy mã khách hàng tự động.
        string LayMaKhachHang()
        {
            var maMax = _db.KhachHangs.ToList().Select(n => n.MaKhachHang).Max();

            if (maMax == null)
                return "KH0001";

            int maKhachHang = int.Parse(maMax.Substring(2)) + 1;
            string NH = String.Concat("000", maKhachHang.ToString());
            return "KH" + NH.Substring(maKhachHang.ToString().Length - 1);
        }

        // Lấy thơi gian.
        DateTime LayThoiGian()
        {
            return DateTime.Now;
        }

        // GET: KhachHang
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginModel model, NhatKy nhatKy)
        {
            string matKhau = Encryptor.MD5Hash(model.MatKhauLogin);

            // Kiểm tra thông tin tài khoản.
            var result1 = _db.KhachHangs.SingleOrDefault(r => r.Email == model.EmailLogin && r.MatKhau == matKhau);

            // Giữ lại thông tin khách hàng.
            KhachHang khachHang = _db.KhachHangs.SingleOrDefault(kh => kh.Email == model.EmailLogin && kh.MatKhau == matKhau);

            if (result1 != null && ModelState.IsValid)
            {
                //FormsAuthentication.SetAuthCookie(model.EmailLogin, model.RememberMe);
                Session["Customer"] = model.EmailLogin;

                // Tạo một sesstion để lưu lại thông tin khách hàng.
                Session["TaiKhoan"] = khachHang;

                // Tạo một session để lấy mã khách hàng.
                Session["MaKhachHang"] = khachHang.MaKhachHang;

                nhatKy.Username = Session["Customer"].ToString();
                nhatKy.TinhTrang = "Đăng nhập";
                nhatKy.GhiNho = LayThoiGian();
                _db.NhatKies.Add(nhatKy);
                _db.SaveChanges();

                return RedirectToAction("Index", "TrangChu");
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout(NhatKy nhatKy)
        {
            // ghi nhật ký đăng xuất
            nhatKy.Username = Session["Customer"].ToString();
            nhatKy.TinhTrang = "Đăng xuất";
            nhatKy.GhiNho = LayThoiGian();

            _db.NhatKies.Add(nhatKy);
            _db.SaveChanges();

            Session["TaiKhoan"] = null;
            Session["Customer"] = null;
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index", "TrangChu");
        }

        [HttpGet]
        public ActionResult Regis()
        {
            ViewBag.MaKhachHang = LayMaKhachHang();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regis(KhachHang khachHang, NhatKy nhatky)
        {
            var result = _db.KhachHangs.SingleOrDefault(kh => kh.Email == khachHang.Email);

            ViewBag.MaKhachHang = LayMaKhachHang();
            if (result != null && ModelState.IsValid)
            {
                ModelState.AddModelError("", "Email đã tồn tại");
            }
            else if (ModelState.IsValid)
            {
                khachHang.MaKhachHang = LayMaKhachHang();
                khachHang.MatKhau = Encryptor.MD5Hash(khachHang.MatKhau);
                _db.KhachHangs.Add(khachHang);
                _db.SaveChanges();

                // Ghi nhật ký đăng ký.
                nhatky.Username = khachHang.Email;
                nhatky.TinhTrang = "Đăng ký";
                nhatky.GhiNho = LayThoiGian();

                _db.NhatKies.Add(nhatky);
                _db.SaveChanges();


                //FormsAuthentication.SetAuthCookie(khachHang.Email, false);
                Session["Customer"] = khachHang.Email;

                // Tạo một sesstion để lưu lại thông tin khách hàng.
                Session["TaiKhoan"] = khachHang;

                // Tạo một session để lấy mã khách hàng.
                Session["MaKhachHang"] = khachHang.MaKhachHang;

                return RedirectToAction("Index", "TrangChu");
            }

            return View(khachHang);
        }
    }
}