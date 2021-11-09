using QuanLyBanQuanAo.Controllers;
using QuanLyBanQuanAo.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Areas.NhanViens.Controllers
{
    public class NhanHieusNhanVienController : AuthController
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // GET: NhanViens/NhanHieusNhanVien
        public ActionResult Index()
        {
            var nhanHieus = _db.NhanHieux.ToList();

            return View(nhanHieus);
        }

        // GET: Admin/NhanHieusNhanVien/Details/5
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
    }
}