using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.GiangVien;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KeKhaiService;

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
            ViewBag.UserName = Session["FullName"] as string;
            return View();
        }
        public ActionResult ThongTin()
        {
            ViewBag.UserName = Session["FullName"] as string;
            var maGV = Session["UserName"] as string;
            var giangVienInfo = new XemChiTietThongTinGiangVien();

            if (!string.IsNullOrEmpty(maGV))
            {
                GiangVienService giangVienService = new GiangVienService();
                var thongTinGiangVien = giangVienService.GetGiangVienTheoMa(maGV);

                if (thongTinGiangVien != null)
                {
                    giangVienInfo = thongTinGiangVien;
                    if (!string.IsNullOrEmpty(giangVienInfo.Avartar))
                    {
                        ViewBag.Avartar = giangVienInfo.Avartar;
                        Session["Avartar"] = giangVienInfo.Avartar; // Thay thế HttpContext.Session.SetString()
                    }
                }

                // Thông tin tài khoản
                ViewBag.TenDangNhap = maGV;
                var loaiTaiKhoan = Session["permission"] as string;
                ViewBag.LoaiTaiKhoan = loaiTaiKhoan;
            }

            // Kiểm tra đợt kê khai
            DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
            string trangThaiKeKhai = "";

            if (dotKeKhaiService.KiemTraDotKeKhaiDangMo())
            {
                trangThaiKeKhai = "Đợt kê khai đang mở!";
            }
            else if (dotKeKhaiService.DotKeKhaiSapMo())
            {
                trangThaiKeKhai = "Đợt kê khai sắp mở!";
            }
            else
            {
                trangThaiKeKhai = "Không có đợt kê khai nào!";
            }

            if (!string.IsNullOrEmpty(trangThaiKeKhai))
            {
                ViewBag.ThongBaoKeKhai = trangThaiKeKhai;
            }

            return View(giangVienInfo);
        }

        [HttpPost]
        public ActionResult UpLoadAvartar(HttpPostedFileBase file)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Vui lòng chọn tệp để tải lên!"
                    }, JsonRequestBehavior.AllowGet);
                }

                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".gif")
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Chỉ nhận định dạng ảnh (.png, .jpg, .gif)"
                    }, JsonRequestBehavior.AllowGet);
                }

                var maGV = Session["UserName"] as string;
                if (string.IsNullOrEmpty(maGV))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Bạn chưa đăng nhập!"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Lưu file vào thư mục Uploads
                string folderPath = Server.MapPath("~/Content/ImgAvartar ");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, file.FileName);
                file.SaveAs(filePath);

                // Xóa ảnh cũ nếu có
                var imgUrl = Session["Avartar"] as string;
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    string oldFilePath = Path.Combine(folderPath, imgUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Cập nhật ảnh đại diện mới vào DB
                GiangVienService giangVienService = new GiangVienService();
                bool updateSuccess = giangVienService.UpdateAvartar(file.FileName, maGV);
                if (updateSuccess)
                {
                    Session["Avartar"] = file.FileName; // Cập nhật session với ảnh mới
                    return Json(new
                    {
                        success = 1,
                        data = file.FileName,
                        message = "Cập nhật ảnh đại diện thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Cập nhật ảnh đại diện không thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }



    }
}