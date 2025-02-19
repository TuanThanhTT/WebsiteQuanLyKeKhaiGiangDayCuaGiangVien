using System.Web.Mvc;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TrangChu()
        {
            return View();
        }
        public ActionResult ThongTin()
        {
            return View();
        }

    }
}