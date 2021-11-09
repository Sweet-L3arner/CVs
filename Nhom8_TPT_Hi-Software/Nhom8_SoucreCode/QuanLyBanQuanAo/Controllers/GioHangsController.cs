using QuanLyBanQuanAo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Controllers
{
    public class GioHangsController : Controller
    {
        #region Giỏ hàng
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // Lấy giỏ hàng
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;

            if (listGioHang == null)
            {
                // Nếu giỏ hàng chưa tồn tại
                // thì mình tiến hành khởi tạo list giỏ hàng (session[GioHang]).
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }

            return listGioHang;
        }

        // Thêm giỏ hàng
        // Định nghĩa tham số cho action để nhận tham số tương ứng.
        public ActionResult ThemGioHang(string maSanPham, string strURL)
        {
            // Tìm xem sản phẩm này đã có trong cửa hàng hay chưa.
            SanPham sp = _db.SanPhams.SingleOrDefault(a => a.MaSanPham == maSanPham);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy ra session giỏ hàng.
            List<GioHang> listGioHang = LayGioHang();

            // Kiểm tra sách này đã tồn tại trong session[gioHang] hay chưa.
            GioHang sanPham = listGioHang.Find(b => b.MaSanPhamGioHang == maSanPham);
            if (sanPham == null)
            {
                sanPham = new GioHang(maSanPham);

                // Thêm sản phẩm mới vào giỏ hàng.
                listGioHang.Add(sanPham);
                return Redirect(strURL);
            }
            else
            {
                sanPham.SoLuongGioHang++;
                return Redirect(strURL);
            }
        }

        // Cập nhật giỏ hàng.
        public ActionResult CapNhatGioHang(string maSanPham, FormCollection f)
        {
            // Kiểm tra mã sản phẩm có trong cơ dở dữ liệu ko?
            SanPham sp = _db.SanPhams.SingleOrDefault(a => a.MaSanPham == maSanPham);

            // Nếu get sai mã sản phẩm thì trả về trang lỗi 404.
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy giỏ hàng ra từ session.
            List<GioHang> listGioHang = LayGioHang();

            // Kiểm tra san phảm có tồn tại trong session["GioHang"].
            GioHang sanPham = listGioHang.SingleOrDefault(b => b.MaSanPhamGioHang == maSanPham);

            // Nếu tồn tại thì cho sửa số lượng.
            if (sanPham != null)
            {
                sanPham.SoLuongGioHang = int.Parse(f["txtSoLuong"].ToString());
            }

            return RedirectToAction("GioHang");
        }

        // Xóa giỏ hàng.
        public ActionResult XoaGioHang(string maSanPham)
        {
            // Kiểm tra mã sản phẩm có trong cơ dở dữ liệu ko?
            SanPham sp = _db.SanPhams.SingleOrDefault(a => a.MaSanPham == maSanPham);

            // Nếu get sai mã sản phẩm thì trả về trang lỗi 404.
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy giỏ hàng ra từ session
            List<GioHang> listGioHang = LayGioHang();

            // Kiểm tra san phảm có tồn tại trong session["GioHang"]
            GioHang sanPham = listGioHang.SingleOrDefault(b => b.MaSanPhamGioHang == maSanPham);

            // Nếu tồn tại thì cho sửa số lượng
            if (sanPham != null)
            {
                listGioHang.RemoveAll(a => a.MaSanPhamGioHang == maSanPham);
            }
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("GioHang", "GioHangs");
            }
            return RedirectToAction("GioHang");
        }

        // Xây dựng trang giỏ hàng.
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null || TongSoLuong() == 0)
            {
                return View();
            }

            List<GioHang> listGioHang = LayGioHang();
            ViewBag.MaDonViVanChuyen = new SelectList(_db.DonViVanChuyens, "MaDonVi", "TenDonVi");
            ViewBag.TongTien = TongSoTien();
            return View(listGioHang);
        }

        // Xây dựng tính tổng số lượng và tổng tiền.
        // Tính tổng số lượng.
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;

            if (listGioHang != null)
            {
                iTongSoLuong = listGioHang.Sum(n => n.SoLuongGioHang);
            }

            return iTongSoLuong;
        }

        // Tính tổng số tiền.
        private int TongSoTien()
        {
            int tongSoTien = 0;
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;

            if (listGioHang != null)
            {
                tongSoTien = listGioHang.Sum(n => n.ThanhTien());
            }

            return tongSoTien;
        }

        // Tạo partial giỏ hàng. 
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }

            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }

        // Xây dựng một view cho người dùng chỉnh sửa giỏ hàng.
        public ActionResult SuaGioHang()
        {
            if (TongSoLuong() == 0)
            {
                return RedirectToAction("GioHang", "GioHangs");
            }

            List<GioHang> listGioHang = LayGioHang();

            ViewBag.TongTien = TongSoTien();
            return View(listGioHang);
        }
        #endregion



        #region Đặt hàng

        string LayMaDonHang()
        {
            var maMax = _db.DonDatHangs.ToList().Select(n => n.MaDonDatHang).Max();

            if (maMax == null)
                return "DH001";

            int maDH = int.Parse(maMax.Substring(2)) + 1;
            string DH = String.Concat("00", maDH.ToString());
            return "DH" + DH.Substring(maDH.ToString().Length - 1);
        }

        // Xây dựng chức năng đặt hàng.
        [HttpPost]
        public ActionResult DatHang(string maDonViVanChuyen, string diaChiGiao = "")
        {
            // Kiểm tra đăng nhập.
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return View("ThongBao");
            }

            // Kiểm tra giỏ hàng.
            if (Session["GioHang"] == null)
            {
                return View("GioHang");
            }

            // Thêm đơn hàng.
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<GioHang> gh = LayGioHang();

            ddh.MaDonDatHang = LayMaDonHang();
            ddh.MaKhachHang = kh.MaKhachHang;
            ddh.MaNhanVien = "NV000";
            ddh.NgayDatHang = DateTime.Now;
            ddh.DiaChiGiao = kh.DiaChi;
            ddh.MaDonViVanChuyen = maDonViVanChuyen;

            if (diaChiGiao != "")
            {
                ddh.DiaChiGiao = diaChiGiao;
            }

            ddh.TinhTrang = 1;

            // Thêm chi tiết đơn hàng.
            _db.DonDatHangs.Add(ddh);
            _db.SaveChanges();

            foreach (var item in gh)
            {
                ChiTietDatHang ctDH = new ChiTietDatHang();
                ctDH.MaDonDatHang = ddh.MaDonDatHang;
                ctDH.MaSanPham = item.MaSanPhamGioHang;
                ctDH.SoLuong = item.SoLuongGioHang;
                ctDH.ThanhTien = item.ThanhTien();
                _db.ChiTietDatHangs.Add(ctDH);
            }

            gh.Clear();
            _db.SaveChanges();
            return RedirectToAction("Index", "TrangChu");
        }

        #endregion
    }
}