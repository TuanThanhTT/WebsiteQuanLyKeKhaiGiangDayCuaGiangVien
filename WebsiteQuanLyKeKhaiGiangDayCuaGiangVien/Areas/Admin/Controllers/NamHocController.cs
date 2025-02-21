using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.NamHocService;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Areas.Admin.Controllers
{
    public class NamHocController : Controller
    {
        // GET: Admin/NamHoc
        public ActionResult CaiDatNamHoc()
        {

            if (Session["FullName"] != null)
            {
                ViewBag.UserName =  Session["FullName"].ToString();
            }
            else
            {
                ViewBag.UserName = "Xin chào người dùng!";
            }

            return View();
        }

        [HttpPost]
        public ActionResult LoadTableNamHoc(int page = 1, int pageSize = 10)
        {
            try
            {
                NamHocService namHocService = new NamHocService();

                int totalRecords =  namHocService.GetSoLuongNamHoc();
                var dsNamHoc =  namHocService.GetDanhSachNamHocTheoTrang(page, pageSize);

                if (dsNamHoc != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsNamHoc,
                        totalRecords = totalRecords, // Tổng số bản ghi để tính số trang
                        page = page,
                        pageSize = pageSize,
                        message = ""
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ThemNamHoc(string tenNamHoc)
        {
            try
            {
                Console.WriteLine("Tên năm học: " + tenNamHoc);

                if (string.IsNullOrEmpty(tenNamHoc))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Tên năm học không được rỗng hoặc bỏ trống!"
                    }, JsonRequestBehavior.AllowGet);
                }

                NamHocService namHocService = new NamHocService();
                if (namHocService.ThemNamHoc(tenNamHoc))
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Thêm năm học mới thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Thêm năm học không thành công!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getDanhSachHocKyTheoNamHoc(int maNamHoc)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var namHoc =  context.NamHocs.Find(maNamHoc);
                    if (namHoc != null)
                    {
                        NamHocService namHocService = new NamHocService();
                        var dsHocKy =  namHocService.GetDanhSachHocKyTheoNamHoc(maNamHoc);
                        if (dsHocKy != null)
                        {
                            return Json(new
                            {
                                success = 1,
                                data = dsHocKy,
                                tenNamHoc = namHoc.TenNamHoc,
                                message = "Load danh sách học kỳ thành công!"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Không có học kỳ nào tìm thấy!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ThemHocKyMoiVaoNamHoc(string tenHocKy, int maNamHoc)
        {
            try
            {
                Console.WriteLine("Mã năm học: " + maNamHoc);
                Console.WriteLine("Tên học kỳ: " + tenHocKy);

                NamHocService namHocService = new NamHocService();
                if ( namHocService.ThemHocKyMoiVaoNamHoc(maNamHoc, tenHocKy))
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Thêm học kỳ thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Console.WriteLine("Không thành công");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Lỗi thêm học kỳ!"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ChinhSuaThongTinNamHoc(int maNamHoc, string tenNamHocMoi)
        {
            try
            {
                NamHocService namHocService = new NamHocService();
                if ( namHocService.ChinhSuaThongTinNamHoc(maNamHoc, tenNamHocMoi))
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Chỉnh sửa thông tin năm học thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Cập nhật thông tin năm học không thành công!"
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult XoaNamHoc(int maNamHoc)
        {
            try
            {
                NamHocService namHocService = new NamHocService();
                if ( namHocService.XoaNamHoc(maNamHoc))
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Xóa năm học thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Lỗi xóa năm học!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XoaDanhSachNamHoc()
        {
            try
            {
                // Đọc dữ liệu JSON từ request body
                string jsonString;
                using (var reader = new StreamReader(Request.InputStream))
                {
                    jsonString =  reader.ReadToEnd();
                }

                // Deserialize dữ liệu JSON
                XoaNamHocRequest request = JsonConvert.DeserializeObject<XoaNamHocRequest>(jsonString);

                if (request == null || request.Ids == null || !request.Ids.Any())
                {
                    return Json(new { success = false, message = "Danh sách ID không hợp lệ" }, JsonRequestBehavior.AllowGet);
                }

                NamHocService namHocService = new NamHocService();

                foreach (var id in request.Ids)
                {
                     namHocService.XoaNamHoc(id);
                }

                return Json(new { success = true, message = "Xóa năm học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Lỗi xóa năm học!"
            }, JsonRequestBehavior.AllowGet);
        }


    }
}