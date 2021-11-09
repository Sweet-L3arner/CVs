using QuanLyBanQuanAo.Controllers;
using System.Web.Mvc;


namespace QuanLyBanQuanAo.Areas.Admin.Controllers
{
    public class HomeAdminController : AuthController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}