using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService;
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

        [HttpPost]
        public ActionResult CapNhatKhoa(string maKhoa, string tenKhoa)
        {
            try
            {
                KhoaService khoaService = new KhoaService();
                if ( khoaService.CapNhatKhoa(tenKhoa, maKhoa))
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Cập nhật thông tin khoa thành công!"
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
                message = "Cập nhật thông tin khoa không thành công!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetThongTinKhoaTheoMa(string maKhoa)
        {
            try
            {
                KhoaService khoaService = new KhoaService();
                var khoa =  khoaService.GetKhoaTheoMa(maKhoa);
                if (khoa != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = khoa,
                        message = "Lấy thông tin thành công!"
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
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFileKhoa(HttpPostedFileBase file)
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
                if (fileExtension != ".xlsx" && fileExtension != ".xls")
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Chỉ nhận định dạng file Excel (.xlsx, .xls)"
                    }, JsonRequestBehavior.AllowGet);
                }

                FileUpload fileUpload = new FileUpload();
                string filePath =  fileUpload.UploadFileKhoa(file);
                if (string.IsNullOrEmpty(filePath))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Lỗi tải file!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ReadFileExcel readFileExcel = new ReadFileExcel();
                    var danhSachKhoa =  readFileExcel.DocFileThemKhoa(filePath);
                    Console.WriteLine("Danh sách khoa tải lên: " + danhSachKhoa.Count);

                    if (danhSachKhoa.Any())
                    {
                        var listKhoa =  readFileExcel.KiemTraKhoaHopLe(danhSachKhoa);
                        bool valid = listKhoa.All(item => string.IsNullOrEmpty(item.ghiChu));

                        if (valid)
                        {
                            Session["tenFile"] = filePath;
                            Session["listKhoaThem"] = JsonConvert.SerializeObject(listKhoa);
                        }
                        else
                        {
                            System.IO.File.Delete(filePath);
                        }

                        return Json(new
                        {
                            success = 1,
                            data = listKhoa,
                            valid = valid,
                            message = "Tải file thành công!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        return Json(new
                        {
                            success = 0,
                            message = "File tải lên rỗng!"
                        }, JsonRequestBehavior.AllowGet);
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
                message = "Lỗi tải file!"
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult LuuDanhSachKhoaTailen()
        {
            try
            {
                var khoas = new List<ThongTinKhoa>();
                Console.WriteLine("Chạy phương thức LuuDanhSachKhoaTailen");

                var listKhoaThemJson = Session["listKhoaThem"] as string;
                if (!string.IsNullOrEmpty(listKhoaThemJson))
                {
                    khoas = JsonConvert.DeserializeObject<List<ThongTinKhoa>>(listKhoaThemJson);
                }

                var filePath = Session["tenFile"] as string;

                if (!string.IsNullOrEmpty(filePath) && khoas.Any())
                {
                    KhoaService khoaService = new KhoaService();
                    if ( khoaService.ThemDanhSachKhoaMoi(khoas))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                            Session.Remove("tenFile");
                            Session.Remove("listKhoaThem");
                        }
                        return Json(new
                        {
                            success = 1,
                            message = "Thêm danh sách khoa thành công!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Console.WriteLine("Lỗi khi lưu danh sách khoa!");
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
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XoaKhoaTheoDanhSach()
        {
            try
            {
                var idKhoas = JsonConvert.DeserializeObject<List<string>>(Request.Form["idKhoas"]);

                KhoaService khoaService = new KhoaService();
                if (idKhoas != null && idKhoas.Any())
                {
                    if ( khoaService.XoaDanhSachKhoa(idKhoas))
                    {
                        return Json(new
                        {
                            success = 1,
                            message = "Đã xóa " + idKhoas.Count + " khoa thành công!"
                        }, JsonRequestBehavior.AllowGet);
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
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DownloadMauFile()
        {
            string filePath = Path.Combine(Server.MapPath("~/MauFile"), "MauDanhSachKhoaEmpty.xlsx");

            if (!System.IO.File.Exists(filePath))
            {
                return Json(new
                {
                    success = 0,
                    message = "Không tìm thấy tệp mẫu!"
                }, JsonRequestBehavior.AllowGet);
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "MauDanhSachKhoa.xlsx";

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}