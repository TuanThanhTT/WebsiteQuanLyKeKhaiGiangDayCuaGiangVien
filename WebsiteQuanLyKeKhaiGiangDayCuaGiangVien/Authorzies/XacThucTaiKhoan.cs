using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Authorzies
{
    public class XacThucTaiKhoan : AuthorizeAttribute
    {
        private readonly string[] _permissions;
        private readonly string[] _roles;

        public XacThucTaiKhoan(string permission, string role)
        {
            _permissions = permission.Split(',').Select(r => r.Trim()).ToArray();
            _roles = role.Split(',').Select(r => r.Trim()).ToArray();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserName"] == null)
            {
                return false; // Không có session => chưa đăng nhập
            }

            var userPermission = httpContext.Session["permission"] as string;
            if (string.IsNullOrEmpty(userPermission))
            {
                return false; // Không có quyền => từ chối truy cập
            }

            var userPermissions = userPermission.Split(',').Select(r => r.Trim()).ToArray();
            return _permissions.Any(p => userPermissions.Contains(p));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserName"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Login", action = "Login" })
                );
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult(); // Trả về 401 Unauthorized
            }
        }
    }

}


