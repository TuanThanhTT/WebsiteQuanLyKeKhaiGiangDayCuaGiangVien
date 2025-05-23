using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var razorEngine = ViewEngines.Engines.OfType<RazorViewEngine>().First();
            razorEngine.ViewLocationFormats = new[]
            {
        "~/Views/{1}/{0}.cshtml",
        "~/Views/Shared/{0}.cshtml"
    };
            razorEngine.MasterLocationFormats = new[]
            {
        "~/Views/{1}/{0}.cshtml",
        "~/Views/Shared/{0}.cshtml"
    };
            razorEngine.PartialViewLocationFormats = new[]
            {
        "~/Views/{1}/{0}.cshtml",
        "~/Views/Shared/{0}.cshtml"
    };

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(razorEngine);
        }
    }
}
