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
    public class SanPhamsAdminController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // GET: Admin/SanPhamA
        public ActionResult Index(string maSP = "", string tenSP = "", string maLSP = "", string maNH = "",
                                  string noiSanXuat = "", string giaMin = "", string giaMax = "",
                                  string tinhTrang = "")
        {
            ViewBag.maSP = maSP;
            ViewBag.tenSP = tenSP;

            ViewBag.MaLSP = new SelectList(_db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham");
            ViewBag.MaNH = new SelectList(_db.NhanHieux, "MaNhanHieu", "TenNhanHieu");

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

            var sanPhams = _db.SanPhams.Include(s => s.LoaiSanPham).Include(s => s.NhanHieu)
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

        // GET: Admin/SanPhamA/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = _db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/SanPhamA/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSanPham = new SelectList(_db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham");
            ViewBag.MaNhanHieu = new SelectList(_db.NhanHieux, "MaNhanHieu", "TenNhanHieu");
            return View();
        }

        // POST: Admin/SanPhamA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSanPham,TenSanPham,MaNhanHieu,MaLoaiSanPham,NoiSanXuat,DonGia,TinhTrang,MoTa,AnhSP")] SanPham sanPham)
        {
            //System.Web.HttpPostedFileBase Avatar;
            var imgSP = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgSP.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/products/" + postedFileName); //
            imgSP.SaveAs(path); //

            var kiemTraMaSanPham = _db.SanPhams.SingleOrDefault(sp => sp.MaSanPham == sanPham.MaSanPham);


            if (kiemTraMaSanPham == null && ModelState.IsValid)
            {
                sanPham.AnhSP = postedFileName;
                _db.SanPhams.Add(sanPham);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Mã sản phẩm này đã tồn tại");
            }

            ViewBag.MaLoaiSanPham = new SelectList(_db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewBag.MaNhanHieu = new SelectList(_db.NhanHieux, "MaNhanHieu", "TenNhanHieu", sanPham.MaNhanHieu);
            return View(sanPham);
        }

        // GET: Admin/SanPhamA/Edit/5
        // nhận một tham số id và kiểm tra nó có tồn tại hay không
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = _db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSanPham = new SelectList(_db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewBag.MaNhanHieu = new SelectList(_db.NhanHieux, "MaNhanHieu", "TenNhanHieu", sanPham.MaNhanHieu);
            return View(sanPham);
        }

        // POST: Admin/SanPhamA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSanPham,TenSanPham,MaNhanHieu,MaLoaiSanPham,NoiSanXuat,DonGia,TinhTrang,MoTa,AnhSP")] SanPham sanPham)
        {
            var imgSP = Request.Files["Avatar"];
            try
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(imgSP.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/products/" + postedFileName);
                imgSP.SaveAs(path);
            }
            catch
            { }
            if (ModelState.IsValid)
            {
                _db.Entry(sanPham).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiSanPham = new SelectList(_db.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewBag.MaNhanHieu = new SelectList(_db.NhanHieux, "MaNhanHieu", "TenNhanHieu", sanPham.MaNhanHieu);
            return View(sanPham);
        }

        // GET: Admin/SanPhamA/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = _db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhamA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SanPham sanPham = _db.SanPhams.Find(id);
            _db.SanPhams.Remove(sanPham);
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
