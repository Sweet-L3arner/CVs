using QuanLyBanQuanAo.Controllers;
using QuanLyBanQuanAo.Models;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Areas.NhanViens.Controllers
{
    public class LoaiSanPhamsNhanVienController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // GET: NhanViens/LoaiSanPhamsNhanVien
        public ActionResult Index()
        {
            var loaiSanPhams = _db.LoaiSanPhams.ToList();

            return View(loaiSanPhams);
        }
    }
}