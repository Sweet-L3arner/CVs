using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyBanQuanAo.Controllers;
using QuanLyBanQuanAo.Models;

namespace QuanLyBanQuanAo.Areas.Admin.Controllers
{
    public class LoaiSanPhamsAdminController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // Lấy mã loại sản phẩm tự động
        string LayMaLSP()
        {
            var maMax = _db.LoaiSanPhams.ToList().Select(n => n.MaLoaiSanPham).Max();

            if (maMax == null)
                return "LSP001";

            int maLSP = int.Parse(maMax.Substring(3)) + 1;
            string LSP = String.Concat("00", maLSP.ToString());
            return "LSP" + LSP.Substring(maLSP.ToString().Length - 1);
        }

        // GET: Admin/LoaiSanPhama
        public ActionResult Index()
        {
            return View(_db.LoaiSanPhams.ToList());
        }

        // GET: Admin/LoaiSanPhama/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = _db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhama/Create
        public ActionResult Create()
        {
            ViewBag.MaLSP = LayMaLSP();
            return View();
        }

        // POST: Admin/LoaiSanPhama/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoaiSanPham,TenLoaiSanPham,DVT")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                loaiSanPham.MaLoaiSanPham = LayMaLSP();
                _db.LoaiSanPhams.Add(loaiSanPham);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhama/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = _db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // POST: Admin/LoaiSanPhama/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoaiSanPham,TenLoaiSanPham,DVT")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(loaiSanPham).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhama/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = _db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // POST: Admin/LoaiSanPhama/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LoaiSanPham loaiSanPham = _db.LoaiSanPhams.Find(id);
            _db.LoaiSanPhams.Remove(loaiSanPham);
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
