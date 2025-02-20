using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string userName, string passWord)
        {
            string errMessage = "";
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var user = context.TaiKhoans.Where(op => op.UserName == userName).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.Passwords == passWord)
                        {
                            Session["UserName"] = user.UserName;
                            Session["PassWord"] = user.Passwords;

                            var giangVien = context.GiangViens.Find(userName);
                            string fullName = giangVien == null ? "" : giangVien.TenGV;
                            Session["FullName"] = fullName;

                            // Kiểm tra quyền tài khoản
                            var quyenTaiKhoan = (from taiKhoan in context.TaiKhoans
                                                 join taiKhoanRole in context.AccountRoles on taiKhoan.MaTaiKhoan equals taiKhoanRole.AccountId
                                                 join role in context.Roles on taiKhoanRole.RoleId equals role.RoleId
                                                 where taiKhoan.MaGV == userName
                                                 select role.RoleName).ToList();

                            string permissionString = string.Join(",", quyenTaiKhoan);
                            string roleString = "";

                            List<string> dsRole = new List<string>();
                            foreach (var item in quyenTaiKhoan)
                            {
                                if (item != "User")
                                {
                                    dsRole = (from taiKhoan in context.TaiKhoans
                                              join taiKhoanRole in context.AccountRoles on taiKhoan.MaTaiKhoan equals taiKhoanRole.AccountId
                                              join role in context.Roles on taiKhoanRole.RoleId equals role.RoleId
                                              join rolePer in context.RolePermissions on role.RoleId equals rolePer.RoleId
                                              join per in context.Quyens on rolePer.PermissionId equals per.MaQuyen
                                              where taiKhoan.MaGV == userName
                                              select per.TenQuyen).ToList();
                                }
                            }

                            roleString = string.Join(",", dsRole);
                            Session["permission"] = permissionString;
                            Session["role"] = roleString;

                            // Xác thực người dùng bằng Forms Authentication
                            FormsAuthentication.SetAuthCookie(userName, false);

                            Console.WriteLine("Đăng nhập thành công");
                            Console.WriteLine("Quyền của bạn là: " + permissionString);
                            Console.WriteLine("Role của bạn là: " + roleString);

                            string url = "/Home/TrangChu";
                            if (permissionString.Contains("Admin") || permissionString.Contains("Administrator"))
                            {
                                url = "/Admin/ThongKe/ThongKe";
                            }

                            return Json(new
                            {
                                success = 1,
                                message = "",
                                urlPath = url
                            });
                        }
                        else
                        {
                            errMessage = "Mật khẩu không chính xác!";
                        }
                    }
                    else
                    {
                        errMessage = "Không tìm thấy tài khoản!";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex);
            }

            return Json(new
            {
                success = 0,
                message = errMessage
            });
        }

        [HttpPost]
        public ActionResult Logout()
        {
            // Xóa tất cả session đã lưu
            Session.Clear();

            // Đăng xuất người dùng khỏi hệ thống xác thực Forms Authentication
            FormsAuthentication.SignOut();

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("Login", "Login"); // Thay đổi tên controller/action nếu cần
        }


    }
}