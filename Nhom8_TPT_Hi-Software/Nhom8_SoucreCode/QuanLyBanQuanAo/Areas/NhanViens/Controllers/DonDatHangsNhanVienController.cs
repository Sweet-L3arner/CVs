using QuanLyBanQuanAo.Controllers;
using QuanLyBanQuanAo.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Areas.NhanViens.Controllers
{
    public class DonDatHangsNhanVienController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // GET: Admin/DonDatHangsNhanVien
        public ActionResult Index(string maDDH = "", string maKH = "", string tinhTrang = "")
        {
            ViewBag.MaDDH = maDDH;
            ViewBag.MaKhachHang = maKH;

            int check1 = 1;
            int check2 = 2;
            int check3 = 3;
            ViewBag.TinhTrang = null;

            if (tinhTrang == "1")
            {
                check2 = 1;
                check3 = 1;
                ViewBag.TinhTrang = "1";
            }
            else if (tinhTrang == "2")
            {
                check1 = 2;
                check3 = 2;
                ViewBag.TinhTrang = "2";
            }
            else if (tinhTrang == "3")
            {
                check1 = 3;
                check2 = 3;
                ViewBag.TinhTrang = "3";
            }

            var donDatHangs = _db.DonDatHangs.Where(ddh => ddh.MaDonDatHang.Contains(maDDH) &&
                                                          ddh.MaKhachHang.Contains(maKH) &&
                                                          (ddh.TinhTrang == check1 || ddh.TinhTrang == check2 || ddh.TinhTrang == check3));

            if (donDatHangs.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";

            return View(donDatHangs.ToList());
        }


        // GET: Admin/DonDatHangsNhanVien/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = _db.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            return View(donDatHang);
        }

        // GET: Admin/DonDatHangsNhanVien/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = _db.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKhachHang = new SelectList(_db.KhachHangs, "MaKhachHang", "Ho", donDatHang.MaKhachHang);
            ViewBag.MaNhanVien = new SelectList(_db.NhanViens, "MaNhanVien", "Ho", donDatHang.MaNhanVien);
            ViewBag.MaDonViVanChuyen = new SelectList(_db.DonViVanChuyens, "MaDonVi", "TenDonVi", donDatHang.MaDonViVanChuyen);
            return View(donDatHang);
        }

        // POST: Admin/DonDatHangsNhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonDatHang,MaKhachHang,MaNhanVien,MaDonViVanChuyen,NgayDatHang,NgayGiaoHang,DiaChiGiao,TinhTrang")] DonDatHang donDatHang)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(donDatHang).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKhachHang = new SelectList(_db.KhachHangs, "MaKhachHang", "Ho", donDatHang.MaKhachHang);
            ViewBag.MaNhanVien = new SelectList(_db.NhanViens, "MaNhanVien", "Ho", donDatHang.MaNhanVien);
            ViewBag.MaDonViVanChuyen = new SelectList(_db.DonViVanChuyens, "MaDonVi", "TenDonVi", donDatHang.MaDonViVanChuyen);
            return View(donDatHang);
        }
    }
}