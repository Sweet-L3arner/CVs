using QuanLyBanQuanAo.Models;
using System.Net;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Controllers
{
    public class DonDatHangsController : Controller
    {
        private QuanLyBanQuanAoEntities db = new QuanLyBanQuanAoEntities();

        // GET: DonDatHangs
        public ActionResult Index()
        {
            return View();
        }

        // thông tin chi tiết đơn đặt hàng
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = db.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            return View(donDatHang);
        }
    }
}