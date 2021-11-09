using QuanLyBanQuanAo.Controllers;
using QuanLyBanQuanAo.Models;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Areas.Admin.Controllers
{
    public class NhatKyTaiKhoansController : AuthController
    {
        private QuanLyBanQuanAoEntities _db;

        public NhatKyTaiKhoansController()
        {
            _db = new QuanLyBanQuanAoEntities();
        }

        // GET: Admin/NhatKyTaiKhoans
        public ActionResult Index()
        {
            var nhanKis = _db.NhatKies.ToList();

            return View(nhanKis);
        }
    }
}