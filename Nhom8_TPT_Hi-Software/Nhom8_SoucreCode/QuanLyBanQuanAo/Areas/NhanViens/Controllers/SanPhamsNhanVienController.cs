using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using QuanLyBanQuanAo.Models;
using QuanLyBanQuanAo.Controllers;

namespace QuanLyBanQuanAo.Areas.NhanViens.Controllers
{
    public class SanPhamsNhanVienController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // GET: NhanViens/SanPhamsNhanVien
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
    }
}