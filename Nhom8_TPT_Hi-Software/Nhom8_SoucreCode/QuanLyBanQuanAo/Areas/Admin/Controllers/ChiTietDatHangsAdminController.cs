using QuanLyBanQuanAo.Controllers;
using QuanLyBanQuanAo.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace QuanLyBanQuanAo.Areas.Admin.Controllers
{
    public class ChiTietDatHangsAdminController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // GET: Admin/ChiTietDatHangsAdmin
        public ActionResult Index()
        {
            var chiTietDatHangs = _db.ChiTietDatHangs.Include(c => c.DonDatHang).Include(c => c.SanPham);
            return View(chiTietDatHangs.ToList());
        }

        // GET: Admin/ChiTietDatHangAdmin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDatHang chiTietDatHang = _db.ChiTietDatHangs.Find(id);
            if (chiTietDatHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDatHang);
        }

        // GET: Admin/ChiTietDatHangsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.MaDonDatHang = new SelectList(_db.DonDatHangs, "MaDonDatHang", "MaKhachHang");
            ViewBag.MaSanPham = new SelectList(_db.SanPhams, "MaSanPham", "TenSanPham");
            return View();
        }

        // POST: Admin/ChiTietDatHangsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDonDatHang,MaSanPham,SoLuong,Gia")] ChiTietDatHang chiTietDatHang)
        {
            if (ModelState.IsValid)
            {
                _db.ChiTietDatHangs.Add(chiTietDatHang);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDonDatHang = new SelectList(_db.DonDatHangs, "MaDonDatHang", "MaKhachHang", chiTietDatHang.MaDonDatHang);
            ViewBag.MaSanPham = new SelectList(_db.SanPhams, "MaSanPham", "TenSanPham", chiTietDatHang.MaSanPham);
            return View(chiTietDatHang);
        }

        // GET: Admin/ChiTietDatHangAdmin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDatHang chiTietDatHang = _db.ChiTietDatHangs.Find(id);
            if (chiTietDatHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDonDatHang = new SelectList(_db.DonDatHangs, "MaDonDatHang", "MaKhachHang", chiTietDatHang.MaDonDatHang);
            ViewBag.MaSanPham = new SelectList(_db.SanPhams, "MaSanPham", "TenSanPham", chiTietDatHang.MaSanPham);
            return View(chiTietDatHang);
        }

        // POST: Admin/ChiTietDatHangAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonDatHang,MaSanPham,SoLuong,Gia")] ChiTietDatHang chiTietDatHang)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(chiTietDatHang).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDonDatHang = new SelectList(_db.DonDatHangs, "MaDonDatHang", "MaKhachHang", chiTietDatHang.MaDonDatHang);
            ViewBag.MaSanPham = new SelectList(_db.SanPhams, "MaSanPham", "TenSanPham", chiTietDatHang.MaSanPham);
            return View(chiTietDatHang);
        }

        // GET: Admin/ChiTietDatHangAdmin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDatHang chiTietDatHang = _db.ChiTietDatHangs.Find(id);
            if (chiTietDatHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDatHang);
        }

        // POST: Admin/ChiTietDatHangAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ChiTietDatHang chiTietDatHang = _db.ChiTietDatHangs.Find(id);
            _db.ChiTietDatHangs.Remove(chiTietDatHang);
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
