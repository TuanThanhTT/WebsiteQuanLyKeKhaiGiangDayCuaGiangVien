using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "LoginDefault",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional },
              namespaces: new[] { "WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Controllers" }

          );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Controllers" }
            );
        }
    }
}
