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
    public class DonViVanChuyensAdminController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // Lấy mã đơn vị tự động.
        string LayMaDonVi()
        {
            var maMax = _db.DonViVanChuyens.ToList().Select(n => n.MaDonVi).Max();

            if (maMax == null)
                return "DVVC001";

            int maDonVi = int.Parse(maMax.Substring(4)) + 1;
            string donVi = String.Concat("00", maDonVi.ToString());
            return "DVVC" + donVi.Substring(maDonVi.ToString().Length - 1);
        }

        // GET: Admin/DonViVanChuyensAdmin
        public ActionResult Index()
        {
            return View(_db.DonViVanChuyens.ToList());
        }

        // GET: Admin/DonViVanChuyensAdmin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonViVanChuyen donViVanChuyen = _db.DonViVanChuyens.Find(id);
            if (donViVanChuyen == null)
            {
                return HttpNotFound();
            }
            return View(donViVanChuyen);
        }

        // GET: Admin/DonViVanChuyensAdmin/Create
        public ActionResult Create()
        {
            ViewBag.MaDonVi = LayMaDonVi();
            return View();
        }

        // POST: Admin/DonViVanChuyensAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDonVi,TenDonVi,SDT,Email,AnhDVVC")] DonViVanChuyen donViVanChuyen)
        {
            //System.Web.HttpPostedFileBase Avatar;
            var imgDVVC = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgDVVC.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/shippers/" + postedFileName); //
            imgDVVC.SaveAs(path); //

            if (ModelState.IsValid)
            {
                donViVanChuyen.MaDonVi = LayMaDonVi();
                donViVanChuyen.AnhDVVC = postedFileName;
                _db.DonViVanChuyens.Add(donViVanChuyen);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDonVi = LayMaDonVi();
            return View(donViVanChuyen);
        }

        // GET: Admin/DonViVanChuyensAdmin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonViVanChuyen donViVanChuyen = _db.DonViVanChuyens.Find(id);
            if (donViVanChuyen == null)
            {
                return HttpNotFound();
            }
            return View(donViVanChuyen);
        }

        // POST: Admin/DonViVanChuyenA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonVi,TenDonVi,SDT,Email,AnhDVVC")] DonViVanChuyen donViVanChuyen)
        {
            var imgDVVC = Request.Files["Avatar"];
            try
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(imgDVVC.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/shippers/" + postedFileName);
                imgDVVC.SaveAs(path);
            }
            catch
            { }
            if (ModelState.IsValid)
            {
                _db.Entry(donViVanChuyen).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donViVanChuyen);
        }

        // GET: Admin/DonViVanChuyensAdmin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonViVanChuyen donViVanChuyen = _db.DonViVanChuyens.Find(id);
            if (donViVanChuyen == null)
            {
                return HttpNotFound();
            }
            return View(donViVanChuyen);
        }

        // POST: Admin/DonViVanChuyensAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DonViVanChuyen donViVanChuyen = _db.DonViVanChuyens.Find(id);
            _db.DonViVanChuyens.Remove(donViVanChuyen);
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
