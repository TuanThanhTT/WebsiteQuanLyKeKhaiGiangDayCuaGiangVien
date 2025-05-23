using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Services.MD5Service;

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
                MD5Service mD5Service = new MD5Service();


                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var user = context.TaiKhoans.Where(op => op.UserName == userName).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.Passwords == mD5Service.GetMd5Hash(passWord))
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

        [HttpPost]
        public ActionResult QuenMatKhau(string maGV, string email)
        {
            var _context = new WebsiteQuanLyKeKhaiGiangDayEntities1();
            Services.LoginService.LoginService _loginService = new Services.LoginService.LoginService(_context);
            Services.EmailService.EmailService _emailService = new Services.EmailService.EmailService();

            try
            {
                // 1️⃣ Kiểm tra tài khoản có tồn tại không
                if (!_loginService.KiemTraTaiKhoanTonTai(maGV))
                {
                    return Json(new { success = 0, message = "Tài khoản không tồn tại!" });
                }

                // 2️⃣ Kiểm tra email có trùng với tài khoản không
                if (!_loginService.KiemTraEmail(email, maGV))
                {
                    return Json(new { success = 0, message = "Email không chính xác!" });
                }

                // 3️⃣ Tạo token đặt lại mật khẩu
                string resetToken = _loginService.DatLaiMatKhau(maGV);

                // 4️⃣ Gửi email với link đặt lại mật khẩu
                string resetLink = Url.Action("DatLaiMatKhau", "Login", new { token = resetToken }, "https");

                string message = @"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta name='viewport' content='width=device-width, initial-scale=1'>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }
                            .email-container {
                                max-width: 600px;
                                margin: 30px auto;
                                background-color: #ffffff;
                                padding: 20px;
                                border-radius: 10px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                text-align: center;
                            }
                            h1 {
                                color: #007bff;
                                font-size: 24px;
                            }
                            p {
                                font-size: 16px;
                                color: #333;
                                line-height: 1.5;
                            }
                            .button {
                                display: inline-block;
                                padding: 12px 20px;
                                margin-top: 20px;
                                font-size: 16px;
                                color: #fff;
                                background-color: #28a745;
                                text-decoration: none;
                                border-radius: 5px;
                                font-weight: bold;
                            }
                            .button:hover {
                                background-color: #218838;
                            }
                            .footer {
                                margin-top: 20px;
                                font-size: 14px;
                                color: #888;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='email-container'>
                            <h1>WEBSITE QUẢN LÝ KÊ KHAI GIẢNG DẠY</h1>
                            <p>Bạn đã yêu cầu đặt lại mật khẩu. Vui lòng nhấp vào nút bên dưới để đặt lại mật khẩu:</p>
                            <a href='" + resetLink + @"' target='_blank' class='button'>Đặt lại mật khẩu</a>
                            <p class='footer'>Liên kết này có hiệu lực trong <strong>30 phút</strong>.</p>
                        </div>
                    </body>
                    </html>";

                _emailService.SendEmail(email, "Đặt lại mật khẩu tài khoản", message);

                return Json(new { success = 1, message = "Vui lòng kiểm tra email để đặt lại mật khẩu!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = 0, message = "Có lỗi xảy ra, vui lòng thử lại sau!"+ ex.Message });
            }
        }
        public ActionResult Reset() {

            MD5Service mD5Service = new MD5Service();   
            var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1();
            var dsAcc = context.TaiKhoans.ToList();
            foreach(var item in dsAcc)
            {
                item.Passwords = mD5Service.GetMd5Hash(item.MaGV);
            }
            context.SaveChanges();  

            return Content("thanh cong");
        }


        [HttpGet]
        public ActionResult DatLaiMatKhau(string token)
        {


            
            var _context = new WebsiteQuanLyKeKhaiGiangDayEntities1();
            Services.LoginService.LoginService _loginService = new Services.LoginService.LoginService(_context);

            try
            {
                if (string.IsNullOrEmpty(token) || !_loginService.XacThucTokenDatLaiMatKhau(token))
                {
                    Console.WriteLine("Invalid token: " + (token ?? "null"));
                    ViewBag.ErrorMessage = "Token không hợp lệ hoặc đã hết hạn.";
                    return View("Error");
                }

                Console.WriteLine("Rendering view DatLaiMatKhau with token: " + token);
                return View("DatLaiMatKhau", (object)token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DatLaiMatKhau: " + ex.Message);
                ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult DoiMatKhauMoi(string newPassword, string token)
        {
            var _context = new WebsiteQuanLyKeKhaiGiangDayEntities1();
            Services.LoginService.LoginService _loginService = new Services.LoginService.LoginService(_context);
            Services.EmailService.EmailService _emailService = new Services.EmailService.EmailService();

            var maGV = _loginService.GetMaGiangVienTuToken(token);
            bool doiMatKhauThanhCong = _loginService.DoiMatKhauGiangVien(newPassword, maGV);

            ViewBag.ThongBao = doiMatKhauThanhCong ? "Đổi mật khẩu thành công!" : "Đổi mật khẩu không thành công.";
            ViewBag.ThanhCong = doiMatKhauThanhCong;

            return View();
        }

        public ActionResult LamMoioMatKhau()
        {
            var _context = new WebsiteQuanLyKeKhaiGiangDayEntities1();
            Services.LoginService.LoginService _loginService = new Services.LoginService.LoginService(_context);
            Services.EmailService.EmailService _emailService = new Services.EmailService.EmailService();


            var danhSach = _context.TaiKhoans.ToList();
            MD5Service mD5Service = new MD5Service();
            foreach (var item in danhSach)
            {
                item.Passwords = mD5Service.GetMd5Hash(item.MaGV);
            }
            _context.SaveChanges();

            return Content("");
        }


    }
}