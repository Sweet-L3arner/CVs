using CryptoLib;
using QuanLyBanQuanAo.Controllers;
using QuanLyBanQuanAo.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Areas.Admin.Controllers
{
    public class NhanViensAdminController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // Lấy tự động mã nhân viên.
        string LayMaNhanVien()
        {
            var maMax = _db.NhanViens.ToList().Select(n => n.MaNhanVien).Max();
            int maNhanVien = int.Parse(maMax.Substring(2)) + 1;
            string NH = String.Concat("00", maNhanVien.ToString());
            return "NV" + NH.Substring(maNhanVien.ToString().Length - 1);
        }

        // GET: Admin/NhanViens
        public ActionResult Index(string maNV = "", string hoTen = "", string gioiTinh = "",
                                  string sdt = "", string email = "", string trangThai = "",
                                  string chucVu = "")
        {
            ViewBag.MaNV = maNV;
            ViewBag.HoTen = hoTen;

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

            ViewBag.SDT = sdt;
            ViewBag.Email = email;
            ViewBag.TrangThai = trangThai;

            bool quanLy = true;
            bool nhanVien = false;
            ViewBag.ChucVu = null;

            if (chucVu == "1")
            {
                nhanVien = true;
                ViewBag.ChucVu = "1";
            }
            else if (chucVu == "0")
            {
                quanLy = false;
                ViewBag.ChucVu = "0";
            }

            var nhanViens = _db.NhanViens.Where(nhanvien => nhanvien.MaNhanVien.Contains(maNV) &&
                                               (nhanvien.Ho + " " + nhanvien.Ten).Contains(hoTen) &&
                                               (nhanvien.GioiTinh == nam || nhanvien.GioiTinh == nu) &&
                                               nhanvien.SDT.Contains(sdt) &&
                                               nhanvien.Email.Contains(email) &&
                                               nhanvien.TrangThai.Contains(trangThai) &&
                                               (nhanvien.QuanLy == quanLy || nhanvien.QuanLy == nhanVien));

            if (nhanViens.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";

            return View(nhanViens.ToList());
        }

        // GET: Admin/NhanVienA/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = _db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Create
        public ActionResult Create()
        {
            ViewBag.MaNhanVien = LayMaNhanVien();
            return View();
        }

        // POST: Admin/NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNhanVien,Ho,Ten,GioiTinh,SDT,Email,MatKhau,TrangThai,QuanLy,AnhNV")] NhanVien nhanVien)
        {
            //System.Web.HttpPostedFileBase Avatar;
            var imgNV = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgNV.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/staffs/" + postedFileName); //
            imgNV.SaveAs(path); //


            if (ModelState.IsValid)
            {
                nhanVien.MaNhanVien = LayMaNhanVien();
                nhanVien.MatKhau = Encryptor.MD5Hash(nhanVien.MatKhau);
                nhanVien.AnhNV = postedFileName;

                _db.NhanViens.Add(nhanVien);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNhanVien = LayMaNhanVien();
            return View(nhanVien);
        }

        // GET: Admin/NhanVienA/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = _db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/NhanVienA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNhanVien,Ho,Ten,GioiTinh,SDT,Email,MatKhau,TrangThai,QuanLy,AnhNV")] NhanVien nhanVien)
        {
            var imgNV = Request.Files["Avatar"];
            try
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(imgNV.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/staffs/" + postedFileName);
                imgNV.SaveAs(path);
            }
            catch
            { }
            if (ModelState.IsValid)
            {
                nhanVien.MatKhau = Encryptor.MD5Hash(nhanVien.MatKhau);
                _db.Entry(nhanVien).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhanVien);
        }

        // GET: Admin/NhanVienA/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = _db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/NhanVienA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhanVien nhanVien = _db.NhanViens.Find(id);
            _db.NhanViens.Remove(nhanVien);
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
