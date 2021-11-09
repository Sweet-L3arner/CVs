using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QuanLyBanQuanAo.Models;
using QuanLyBanQuanAo.ViewModels;

namespace QuanLyBanQuanAo.Controllers
{
    public class TrangChuController : Controller
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // GET: TrangChu
        public ActionResult Index()
        {
            var loaiSanPhams = _db.LoaiSanPhams.ToList();
            var nhanHieus = _db.NhanHieux.ToList();
            var sanPhams = _db.SanPhams
                .Include(s => s.LoaiSanPham)
                .Include(s => s.NhanHieu)
                .ToList();

            var viewModels = new TrangChuViewModel
            {
                LoaiSanPhams = loaiSanPhams,
                NhanHieus = nhanHieus,
                SanPhams = sanPhams
            };

            return View(viewModels);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}