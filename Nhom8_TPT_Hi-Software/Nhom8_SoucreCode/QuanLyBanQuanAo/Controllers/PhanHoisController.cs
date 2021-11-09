using System.Web.Mvc;
using QuanLyBanQuanAo.Models;

namespace QuanLyBanQuanAo.Controllers
{
    public class PhanHoisController : Controller
    {
        QuanLyBanQuanAoEntities db = new QuanLyBanQuanAoEntities();
        
        // GET: PhanHois
        public ActionResult Index(string maSanPham, string feedback)
        {
            // Phải đăng nhập mới phản hồi được
            // check xem khách hàng đã đăng nhập hay chưa
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return View("ThongBao");
            }

            PhanHoi phanHoi = new PhanHoi();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];

            phanHoi.MaSanPham = maSanPham;
            phanHoi.MaKhachHang = kh.MaKhachHang;
            phanHoi.PhanHoi1 = feedback;

            db.PhanHois.Add(phanHoi);
            db.SaveChanges();
            return RedirectToAction("Details", "SanPhams", new { @id = maSanPham });

        }
    }
}