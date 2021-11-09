using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CryptoLib;
using QuanLyBanQuanAo.Controllers;
using QuanLyBanQuanAo.Models;

namespace QuanLyBanQuanAo.Areas.Admin.Controllers
{
    public class KhachHangsAdminController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // lấy mã khách hàng tự động
        string LayMaKhachHang()
        {
            var maMax = _db.KhachHangs.ToList().Select(n => n.MaKhachHang).Max();

            if (maMax == null)
                return "KH001";

            int maKhachHang = int.Parse(maMax.Substring(2)) + 1;
            string KH = String.Concat("000", maKhachHang.ToString());
            return "KH" + KH.Substring(maKhachHang.ToString().Length - 1);
        }

        // GET: Admin/KhachHangA
        // tìm kiếm
        [HttpGet]
        public ActionResult Index(string maKH = "", string hoTen = "", string gioiTinh = "", string soDienThoai = "", string email = "", string tinhThanh = "", string diaChi = "")
        {
            ViewBag.maKhachHang = maKH;
            ViewBag.hoTen = hoTen;

            bool nam = true;
            bool nu = false;
            ViewBag.gioiTinh = null;

            if (gioiTinh == "1")
            {
                nu = true;
                ViewBag.gioiTinh = "1";
            }
            else if (gioiTinh == "0")
            {
                nam = false;
                ViewBag.gioiTinh = "0";
            }

            ViewBag.SoDienThoai = soDienThoai;
            ViewBag.Email = email;
            ViewBag.TinhThanh = tinhThanh;
            ViewBag.DiaChi = diaChi;

            var khachHangs = _db.KhachHangs.Where(kh => kh.MaKhachHang.Contains(maKH) &&
                                                (kh.Ho + " " + kh.Ten).Contains(hoTen) &&
                                                (kh.GioiTinh == nam || kh.GioiTinh == nu) &&
                                                kh.SDT.Contains(soDienThoai) &&
                                                kh.Email.Contains(email) &&
                                                kh.TinhOrTP.Contains(tinhThanh) &&
                                                kh.DiaChi.Contains(diaChi));

            if (khachHangs.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";

            return View(khachHangs.ToList());
        }

        // GET: Admin/KhachHangA/Details/5
        public ActionResult Details(string id)
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

        // GET: Admin/KhachHangA/Create
        public ActionResult Create()
        {
            ViewBag.MaKhachHang = LayMaKhachHang();
            return View();
        }

        // POST: Admin/KhachHangA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKhachHang,Ho,Ten,GioiTinh,SDT,Email,MatKhau,TinhOrTP,DiaChi")] KhachHang khachHang)
        {

            if (ModelState.IsValid)
            {
                khachHang.MaKhachHang = LayMaKhachHang();
                khachHang.MatKhau = Encryptor.MD5Hash(khachHang.MatKhau);

                _db.KhachHangs.Add(khachHang);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKhachHang = LayMaKhachHang();
            return View(khachHang);
        }

        // GET: Admin/KhachHangA/Edit/5
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

        // POST: Admin/KhachHangA/Edit/5
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
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangA/Delete/5
        public ActionResult Delete(string id)
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

        // POST: Admin/KhachHangA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            KhachHang khachHang = _db.KhachHangs.Find(id);
            _db.KhachHangs.Remove(khachHang);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
