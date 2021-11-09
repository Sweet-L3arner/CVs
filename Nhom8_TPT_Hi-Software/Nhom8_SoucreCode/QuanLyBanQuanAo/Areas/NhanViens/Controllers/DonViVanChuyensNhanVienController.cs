using QuanLyBanQuanAo.Controllers;
using QuanLyBanQuanAo.Models;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Areas.NhanViens.Controllers
{
    public class DonViVanChuyensNhanVienController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // GET: NhanViens/DonViVanChuyenNV
        public ActionResult Index()
        {
            return View(_db.DonViVanChuyens.ToList());
        }
    }
}