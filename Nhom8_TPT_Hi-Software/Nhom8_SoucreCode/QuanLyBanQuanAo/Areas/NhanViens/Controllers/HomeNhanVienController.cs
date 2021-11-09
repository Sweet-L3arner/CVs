using QuanLyBanQuanAo.Controllers;
using System.Web.Mvc;

namespace QuanLyBanQuanAo.Areas.NhanViens.Controllers
{
    public class HomeNhanVienController : AuthController
    {
        // GET: NhanVien/NhanVienHome
        public ActionResult Index()
        {
            return View();
        }
    }
}