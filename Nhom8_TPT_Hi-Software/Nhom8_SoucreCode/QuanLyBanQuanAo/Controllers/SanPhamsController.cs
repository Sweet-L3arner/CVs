using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyBanQuanAo.Models;

namespace QuanLyBanQuanAo.Controllers
{
    public class SanPhamsController : Controller
    {
        private QuanLyBanQuanAoEntities db = new QuanLyBanQuanAoEntities();

        // GET: SanPhams
        public ActionResult Index(string maSP = "", string tenSP = "", string maLSP = "", string maNH = "",
                                  string noiSanXuat = "", string giaMin = "", string giaMax = "",
                                  string tinhTrang = "")
        {
            ViewBag.maSP = maSP;
            ViewBag.tenSP = tenSP;

            ViewBag.MaLSP = new SelectList(db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham");
            ViewBag.MaNH = new SelectList(db.NhanHieux, "MaNhanHieu", "TenNhanHieu");

            ViewBag.NoiSanXuat = noiSanXuat;

            int min = 0, max = Int32.MaxValue;

            if (giaMin == "")
            {
                ViewBag.GiaMin = "";
                min = 0;
            }
            else
            {
                ViewBag.GiaMin = giaMin;
                min = int.Parse(giaMin);
            }
            if (giaMax == "")
            {
                max = Int32.MaxValue;
                ViewBag.GiaMax = "";
            }
            else
            {
                ViewBag.GiaMax = giaMax;
                max = int.Parse(giaMax);
            }

            ViewBag.TinhTrang = tinhTrang;

            var sanPhams = db.SanPhams.Include(s => s.LoaiSanPham).Include(s => s.NhanHieu)
                                      .Where(sp => sp.MaSanPham.Contains(maSP) &&
                                                    sp.TenSanPham.Contains(tenSP) &&
                                                    sp.MaLoaiSanPham.Contains(maLSP) &&
                                                    sp.MaNhanHieu.Contains(maNH) &&
                                                    sp.NoiSanXuat.Contains(noiSanXuat) &&
                                                    sp.DonGia >= min && sp.DonGia <= max &&
                                                    sp.TinhTrang.Contains(tinhTrang));

            if (sanPhams.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            return View(sanPhams.ToList());
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSanPham = new SelectList(db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham");
            ViewBag.MaNhanHieu = new SelectList(db.NhanHieux, "MaNhanHieu", "TenNhanHieu");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSanPham,TenSanPham,MaNhanHieu,MaLoaiSanPham,NoiSanXuat,DonGia,TinhTrang,MoTa,AnhSP")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiSanPham = new SelectList(db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewBag.MaNhanHieu = new SelectList(db.NhanHieux, "MaNhanHieu", "TenNhanHieu", sanPham.MaNhanHieu);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSanPham = new SelectList(db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewBag.MaNhanHieu = new SelectList(db.NhanHieux, "MaNhanHieu", "TenNhanHieu", sanPham.MaNhanHieu);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSanPham,TenSanPham,MaNhanHieu,MaLoaiSanPham,NoiSanXuat,DonGia,TinhTrang,MoTa,AnhSP")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiSanPham = new SelectList(db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewBag.MaNhanHieu = new SelectList(db.NhanHieux, "MaNhanHieu", "TenNhanHieu", sanPham.MaNhanHieu);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
