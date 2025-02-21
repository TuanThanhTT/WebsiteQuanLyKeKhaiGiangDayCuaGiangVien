using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.HocPhanService;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Areas.Admin.Controllers
{
    public class HocPhanController : Controller
    {
        // GET: Admin/HocPhan
        public ActionResult CaiDatHocPhan()
        {
            ViewBag.UserName = Session["FullName"].ToString();

            return View();
        }

        [HttpPost]
        public ActionResult LoadDanhSachHocPhan(int page = 1, int pageSize = 5)
        {
            try
            {
                HocPhanService hocPhanService = new HocPhanService();
                var (danhSachHocPhan, tongSoLuong) = hocPhanService.LoadDanhSachHocPhan(page, pageSize); // Chuyển async thành đồng bộ
                if (danhSachHocPhan != null && danhSachHocPhan.Any())
                {
                    return Json(new
                    {
                        success = 1,
                        data = danhSachHocPhan,
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
        public ActionResult ThemHocPhan(string maHocPhan, string tenHocPhan, int soTinChi, int lyThuyet, int thucHanh, string ghiChu)
        {
            try
            {
                if (string.IsNullOrEmpty(maHocPhan) || string.IsNullOrEmpty(tenHocPhan) || soTinChi == 0 || lyThuyet == 0 || thucHanh == 0)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Dữ liệu nhập vào rỗng!"
                    });
                }

                HocPhanService hocPhanService = new HocPhanService();

                // Kiểm tra mã học phần đã tồn tại
                if (hocPhanService.KiemTraMaHocPhanChuaTonTai(maHocPhan) == false) // Chuyển async thành đồng bộ
                {
                    return Json(new
                    {
                        success = 0,
                        errName = "maHocPhanError",
                        message = "Mã học phần đã tồn tại!"
                    });
                }

                // Thêm học phần mới
                if (hocPhanService.ThemHocPhanMoi(maHocPhan, tenHocPhan, soTinChi, lyThuyet, thucHanh, ghiChu) == true) // Chuyển async thành đồng bộ
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Thêm học phần mới thành công!"
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
        public ActionResult LayThongTinHocPhanTheoMa(string maHocPhan)
        {
            try
            {
                Console.WriteLine("ma hoc phan can tim: " + maHocPhan);

                if (string.IsNullOrEmpty(maHocPhan))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Mã học phần rỗng!"
                    });
                }

                HocPhanService hocPhanService = new HocPhanService();
                var hocPhan = hocPhanService.GetHocPhanTheoMa(maHocPhan); // Chuyển async thành đồng bộ

                if (hocPhan != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = hocPhan,
                        message = "Thành công"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không tìm thấy thông tin học phần!"
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
        public ActionResult UploadFileHocPhan(HttpPostedFileBase file)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Vui lòng chọn tệp để tải lên!"
                    });
                }

                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (fileExtension != ".xlsx" && fileExtension != ".xls")
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Chỉ nhận định dạng file Excel (.xlsx, .xls)"
                    });
                }

                FileUpload fileUpload = new FileUpload();
                string filePath = fileUpload.UploadFileHocPhan(file); // Đổi sang đồng bộ
                if (string.IsNullOrEmpty(filePath))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Lỗi tải file!"
                    });
                }
                else
                {
                    ReadFileExcel readFileExcel = new ReadFileExcel();
                    var danhSachHocPhan = readFileExcel.DocFileThemHocPhan(filePath); // Đổi sang đồng bộ

                    if (danhSachHocPhan.Any())
                    {
                        var listHocPhan = readFileExcel.KiemTraHocPhanHopLe(danhSachHocPhan); // Đổi sang đồng bộ

                        bool valid = true;
                        foreach (var item in listHocPhan)
                        {
                            if (!string.IsNullOrEmpty(item.ghiChuLoi))
                            {
                                valid = false;
                            }
                        }

                        if (valid)
                        {
                            Session["tenFile"] = filePath;
                            Session["listHocPhanThem"] = JsonConvert.SerializeObject(listHocPhan);
                        }
                        else
                        {
                            System.IO.File.Delete(filePath);
                        }

                        return Json(new
                        {
                            success = 1,
                            data = listHocPhan,
                            valid = valid,
                            message = "Tải file thành công!"
                        });
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
                        });
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
                message = "Có lỗi tải file! Vui lòng thử lại sau."
            });
        }

        [HttpPost]
        public ActionResult LuuThongTinHocPhanTaiLen()
        {
            try
            {
                var hocPhans = new List<ThongTinUploadHocPhan>();

                var listHocPhanThemJson = Session["listHocPhanThem"] as string;
                if (!string.IsNullOrEmpty(listHocPhanThemJson))
                {
                    hocPhans = JsonConvert.DeserializeObject<List<ThongTinUploadHocPhan>>(listHocPhanThemJson);
                }
                var filePath = Session["tenFile"] as string;

                if (!string.IsNullOrEmpty(filePath) && hocPhans.Any())
                {
                    HocPhanService hocPhanService = new HocPhanService();
                    if (hocPhanService.ThemDanhSachHocPhanMoi(hocPhans)) // Đổi thành phương thức đồng bộ
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                            Session.Remove("tenFile");
                            Session.Remove("listHocPhanThem");
                        }

                        return Json(new
                        {
                            success = 1,
                            message = "Thêm danh sách học phần thành công"
                        });
                    }
                    else
                    {
                        Console.WriteLine("Lỗi lưu danh sách");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau"
            });
        }

        [HttpPost]
        public ActionResult CapNhatThongTinHocPhan(string maHocPhan, string tenHocPhan, int soTinChi, int lyThuyet, int thucHanh, string ghiChu)
        {
            try
            {
                if (string.IsNullOrEmpty(tenHocPhan) || string.IsNullOrEmpty(maHocPhan) || soTinChi == 0 || lyThuyet == 0 || thucHanh == 0)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Dữ liệu nhập vào rỗng!"
                    });
                }

                HocPhanService hocPhanService = new HocPhanService();
                if (hocPhanService.CapNhatThongTinHocPhan(maHocPhan, tenHocPhan, soTinChi, lyThuyet, thucHanh, ghiChu)) // Chuyển phương thức đồng bộ
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Cập nhật thông tin thành công!"
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
        public ActionResult LoadThongTinCapNhatHocPhan(string maHocPhan)
        {
            try
            {
                if (string.IsNullOrEmpty(maHocPhan))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Mã học phần rỗng!"
                    });
                }

                HocPhanService hocPhanService = new HocPhanService();
                var hocPhan = hocPhanService.GetHocPhanTheoMa(maHocPhan);  // Chuyển phương thức thành đồng bộ

                if (hocPhan != null)  // Thay `maHocPhan` bằng `hocPhan` trong kiểm tra điều kiện
                {
                    return Json(new
                    {
                        success = 1,
                        data = hocPhan,
                        message = "Thành công!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không tìm thấy học phần!"
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
        public ActionResult DowloadMauFile()
        {
            var filePath = Server.MapPath("~/Content/MauFile/MauDanhSachHocPhanEmpty.xlsx");
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = "MauDanhSachHocPhan.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        public ActionResult XoaDanhSachHocPhan()
        {
            try
            {
                var idHocPhans = JsonConvert.DeserializeObject<List<string>>(Request.Form["idHocPhans"]);

                HocPhanService hocPhanService = new HocPhanService();
                if (idHocPhans != null && idHocPhans.Any())
                {
                    if (hocPhanService.XoaDanhSachHocPhan(idHocPhans))  // Dùng .Result để lấy kết quả từ Task
                    {
                        return Json(new
                        {
                            success = 1,
                            message = "Đã xóa " + idHocPhans.Count + " học phần thành công!"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = 0,
                            message = "Xóa học phần không thành công!"
                        });
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
            });
        }

    }
}