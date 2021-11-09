using QuanLyBanQuanAo.Models;
using System.Net;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Controllers
{
    public class NhanHieusController : Controller
    {
        private QuanLyBanQuanAoEntities _db = new QuanLyBanQuanAoEntities();

        // GET: NhanHieus
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/NhanHieus/Details/5
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