using System;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KhoaService;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Areas.Admin.Controllers
{
    public class KhoaController : Controller
    {
        // GET: Admin/Khoa
        public ActionResult CaiDatKhoa()
        {
            ViewBag.UserName = HttpContext.Session["FullName"]; // Truy cập session để lấy giá trị "FullName"
            return View();
        }

        [HttpPost]
        public ActionResult themKhoaMoi(string tenKhoa)
        {
            try
            {
                KhoaService khoaService = new KhoaService();
                if (khoaService.ThemKhoaMoi(tenKhoa))  // Sử dụng .Result để đồng bộ hóa async (nếu bạn sử dụng async)
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Thêm khoa mới thành công!"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Thêm khoa mới không thành công!"
            });
        }

        [HttpPost]
        public  ActionResult loadDanhSachKhoa(int page = 1, int pageSize = 5)
        {
            try
            {
                KhoaService khoaService = new KhoaService();
                int tongSoLuong = 0;
                var dsKhoa =  khoaService.GetDanhSachKhoa(page, pageSize, out tongSoLuong); // Sử dụng .Result để đồng bộ hóa async
               
               

                if (dsKhoa != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsKhoa,
                        totalRecords = tongSoLuong,
                        totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                        currentPage = page,
                        message = "Lấy dữ liệu thành công!"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            });
        }


    }
}