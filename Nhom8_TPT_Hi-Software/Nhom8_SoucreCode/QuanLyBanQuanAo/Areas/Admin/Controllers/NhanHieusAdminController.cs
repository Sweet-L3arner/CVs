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
    public class NhanHieusAdminController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // lấy mã nhãn hiệu
        string LayMaNH()
        {
            var maMax = _db.NhanHieux.ToList().Select(n => n.MaNhanHieu).Max();

            if (maMax == null)
                return "NH001";

            int maNH = int.Parse(maMax.Substring(2)) + 1;
            string NH = String.Concat("00", maNH.ToString());
            return "NH" + NH.Substring(maNH.ToString().Length - 1);
        }

        // GET: Admin/NhanHieus
        public ActionResult Index()
        {
            return View(_db.NhanHieux.ToList());
        }

        // GET: Admin/NhanHieuA/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanHieu nhanHieu = _db.NhanHieux.Find(id);
            if (nhanHieu == null)
            {
                return HttpNotFound();
            }
            return View(nhanHieu);
        }

        // GET: Admin/NhanHieuA/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaNH = LayMaNH();
            return View();
        }

        // POST: Admin/NhanHieuA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNhanHieu,TenNhanHieu,GhiChu,AnhNH")] NhanHieu nhanHieu)
        {
            //System.Web.HttpPostedFileBase Avatar;
            var imgNH = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgNH.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/brands/" + postedFileName); //
            imgNH.SaveAs(path); //

            if (ModelState.IsValid)
            {
                nhanHieu.MaNhanHieu = LayMaNH();
                nhanHieu.AnhNH = postedFileName;
                _db.NhanHieux.Add(nhanHieu);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNH = LayMaNH();
            return View(nhanHieu);
        }

        // GET: Admin/NhanHieuA/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanHieu nhanHieu = _db.NhanHieux.Find(id);
            if (nhanHieu == null)
            {
                return HttpNotFound();
            }
            return View(nhanHieu);
        }

        // POST: Admin/NhanHieuA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNhanHieu,TenNhanHieu,GhiChu,AnhNH")] NhanHieu nhanHieu)
        {
            var imgNH = Request.Files["Avatar"];
            try
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(imgNH.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/brands/" + postedFileName);
                imgNH.SaveAs(path);
            }
            catch
            { }
            if (ModelState.IsValid)
            {
                _db.Entry(nhanHieu).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhanHieu);
        }

        // GET: Admin/NhanHieuA/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanHieu nhanHieu = _db.NhanHieux.Find(id);
            if (nhanHieu == null)
            {
                return HttpNotFound();
            }
            return View(nhanHieu);
        }

        // POST: Admin/NhanHieuA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhanHieu nhanHieu = _db.NhanHieux.Find(id);
            _db.NhanHieux.Remove(nhanHieu);
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
