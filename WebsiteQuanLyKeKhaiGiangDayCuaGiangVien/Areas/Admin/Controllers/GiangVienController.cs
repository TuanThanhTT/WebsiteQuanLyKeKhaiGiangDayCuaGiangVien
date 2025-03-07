using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.GiangVien;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KhoaService;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Areas.Admin.Controllers
{
    public class GiangVienController : Controller
    {
        // GET: Admin/GiangVien
       
        public ActionResult CaiDatGiangVien()
        {
            ViewBag.UserName = Session["FullName"] as string; // Lấy giá trị từ Session

            return View();
        }

        [HttpPost]
        public JsonResult LoadDanhSachKhoa()
        {
            try
            {
                KhoaService khoaService = new KhoaService();
                var danhSachKhoa = khoaService.GetDanhSachKhoa(); // Không dùng `await`

                if (danhSachKhoa != null && danhSachKhoa.Any())
                {
                    return Json(new
                    {
                        success = 1,
                        data = danhSachKhoa,
                        message = "Thành công!"
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
        public JsonResult LoadDanhSachGiangVien(int page = 1, int pageSize = 5)
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();
                var result = giangVienService.LoadDanhSachGiangVien(page, pageSize); // Không dùng `await`

                if (result.data != null) // Item1 = danh sách giảng viên
                {
                    return Json(new
                    {
                        success = 1,
                        data = result.Item1,  // danh sách giảng viên
                        totalRecords = result.Item2, // tổng số lượng
                        totalPages = (int)Math.Ceiling((double)result.Item2 / pageSize),
                        currentPage = page,
                        message = "Lấy dữ liệu thành công!"
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
        public JsonResult ThemGiangVienMoi(string maGV, string tenGV, string maKhoa, DateTime ngaySinh, bool gioiTinh,
                                   string chucVu, string trinhDo, string soDienThoai, string email,
                                   string diaChi, double heSoLuong, string chuyenNganh, string loaiHinhDaoTao)
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();

                if (string.IsNullOrEmpty(maGV) || string.IsNullOrEmpty(maKhoa) || heSoLuong == 0 ||
                    string.IsNullOrEmpty(tenGV) || string.IsNullOrEmpty(chucVu) || string.IsNullOrEmpty(trinhDo) ||
                    string.IsNullOrEmpty(soDienThoai) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(diaChi) ||
                    string.IsNullOrEmpty(chuyenNganh) || string.IsNullOrEmpty(loaiHinhDaoTao))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Dữ liệu không được để trống!"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Kiểm tra mã giảng viên tồn tại
                if (giangVienService.KiemTraMaGiangVienTonTai(maGV))
                {
                    return Json(new
                    {
                        success = 0,
                        errName = "maGiangVienError",
                        message = "Mã giảng viên đã tồn tại!"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Kiểm tra số điện thoại hợp lệ
                if (!giangVienService.KiemTraSoDienThoaiHopLe(soDienThoai))
                {
                    return Json(new
                    {
                        success = 0,
                        errName = "soDienThoaiError",
                        message = "Số điện thoại không hợp lệ, đủ 10 số và bắt đầu bằng '0'!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Kiểm tra số điện thoại đã tồn tại
                    string errPhone = giangVienService.KiemTraSoDienThoaiTonTai(soDienThoai);
                    if (!string.IsNullOrEmpty(errPhone))
                    {
                        return Json(new
                        {
                            success = 0,
                            errName = "soDienThoaiError",
                            message = "Số điện thoại đã tồn tại!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                // Kiểm tra email hợp lệ
                if (!giangVienService.KiemTraEmailHopLe(email))
                {
                    return Json(new
                    {
                        success = 0,
                        errName = "emailError",
                        message = "Email không hợp lệ!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Kiểm tra email đã tồn tại
                    string errEmail = giangVienService.KiemTraEmailTonTai(email);
                    if (!string.IsNullOrEmpty(errEmail))
                    {
                        return Json(new
                        {
                            success = 0,
                            errName = "emailError",
                            message = "Email đã tồn tại!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                var giangVien = new GiangVien
                {
                    MaGV = maGV,
                    TenGV = tenGV,
                    ChucVu = chucVu,
                    ChuyenNghanh = chuyenNganh,
                    DiaChi = diaChi,
                    Email = email,
                    SoDienThoai = soDienThoai,
                    GioiTinh = gioiTinh,
                    HeSoLuong = heSoLuong,
                    NgaySinh = ngaySinh,
                    MaKhoa = maKhoa,
                    LoaiHinhDaoTao = loaiHinhDaoTao,
                    TrinhDo = trinhDo,
                };

                Console.WriteLine("ngay sinh phia server: " + giangVien.NgaySinh);

                if (giangVien != null)
                {
                    if (giangVienService.ThemGiangVienMoi(giangVien))
                    {
                        return Json(new
                        {
                            success = 1,
                            message = "Thêm giảng viên thành công!"
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
        public JsonResult LayThongTinGiangVienTheoMa(string maGV)
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();

                if (string.IsNullOrEmpty(maGV))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Mã giảng viên không hợp lệ!"
                    }, JsonRequestBehavior.AllowGet);
                }

                var giangVien = giangVienService.GetGiangVienTheoMa(maGV);

                if (giangVien != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = giangVien,
                        message = "Lấy thông tin giảng viên thành công!"
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
                message = "Có lỗi xảy ra, vui lòng thử lại sau"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadFileGiangVien(HttpPostedFileBase file)
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
                string filePath = fileUpload.UploadFileGiangVien(file);
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
                    var danhSachGiangVien = readFileExcel.DocFileThemGIangVien(filePath);

                    if (danhSachGiangVien.Any())
                    {
                        var listGiangVien = readFileExcel.KiemTraGiangVienHopLe(danhSachGiangVien);

                        bool valid = listGiangVien.All(item => string.IsNullOrEmpty(item.ghiChu));

                        if (valid)
                        {
                            Session["tenFile"] = filePath;
                            Session["listGiangVienThem"] = JsonConvert.SerializeObject(listGiangVien);
                        }
                        else
                        {
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }

                        Console.WriteLine("Số lượng đọc được: " + danhSachGiangVien.Count);
                        return Json(new
                        {
                            success = 1,
                            data = listGiangVien,
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
                message = "Có lỗi tải file! Vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LuuThongTinGiangVienTaiLen()
        {
            try
            {
                var giangViens = new List<ThongTinUpLoadGiangVien>();

                var listGiangVienThemJson = Session["listGiangVienThem"] as string;
                if (!string.IsNullOrEmpty(listGiangVienThemJson))
                {
                    giangViens = JsonConvert.DeserializeObject<List<ThongTinUpLoadGiangVien>>(listGiangVienThemJson);
                }

                var filePath = Session["tenFile"] as string;

                if (!string.IsNullOrEmpty(filePath) && giangViens.Any())
                {
                    GiangVienService giangVienService = new GiangVienService();
                    if (giangVienService.ThemDanhSachGiangVienMoi(giangViens))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                            Session.Remove("tenFile");
                            Session.Remove("listGiangVienThem");
                        }
                        return Json(new
                        {
                            success = 1,
                            message = "Thêm danh sách giảng viên thành công"
                        }, JsonRequestBehavior.AllowGet);
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
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CapNhatThongTinGiangVien(string maGV, string tenGV, string maKhoa, DateTime ngaySinh, bool gioiTinh, string chucVu, string trinhDo, string soDienThoai, string email, string diaChi, double heSoLuong, string chuyenNganh, string loaiHinhDaoTao)
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();

                if (string.IsNullOrEmpty(maGV) || string.IsNullOrEmpty(maKhoa) || heSoLuong == 0 ||
                    string.IsNullOrEmpty(tenGV) || string.IsNullOrEmpty(chucVu) || string.IsNullOrEmpty(trinhDo) ||
                    string.IsNullOrEmpty(soDienThoai) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(diaChi) ||
                    string.IsNullOrEmpty(chuyenNganh) || string.IsNullOrEmpty(loaiHinhDaoTao))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Dữ liệu không được để trống!"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Kiểm tra mã giảng viên có tồn tại không
                if (giangVienService.KiemTraMaGiangVienTonTai(maGV))
                {
                    // Kiểm tra số điện thoại hợp lệ
                    if (!giangVienService.KiemTraSoDienThoaiHopLe(soDienThoai))
                    {
                        return Json(new
                        {
                            success = 0,
                            errName = "edit-soDienThoaiError",
                            message = "Số điện thoại không hợp lệ, đủ 10 số và bắt đầu bằng '0'!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        // Kiểm tra số điện thoại đã tồn tại chưa
                        string errPhone = giangVienService.KiemTraSoDienThoaiCapNhatTonTai(soDienThoai, maGV);
                        if (!string.IsNullOrEmpty(errPhone))
                        {
                            return Json(new
                            {
                                success = 0,
                                errName = "edit-soDienThoaiError",
                                message = "Số điện thoại đã tồn tại!"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    // Kiểm tra email hợp lệ
                    if (!giangVienService.KiemTraEmailHopLe(email))
                    {
                        return Json(new
                        {
                            success = 0,
                            errName = "edit-emailError",
                            message = "Email không hợp lệ!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        // Kiểm tra email đã tồn tại chưa
                        string errEmail = giangVienService.KiemTraCapNhatEmailTonTai(email, maGV);
                        if (!string.IsNullOrEmpty(errEmail))
                        {
                            return Json(new
                            {
                                success = 0,
                                errName = "edit-emailError",
                                message = "Email đã tồn tại!"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    var giangVien = new GiangVien
                    {
                        MaGV = maGV,
                        TenGV = tenGV,
                        ChucVu = chucVu,
                        ChuyenNghanh = chuyenNganh,
                        DiaChi = diaChi,
                        Email = email,
                        SoDienThoai = soDienThoai,
                        GioiTinh = gioiTinh,
                        HeSoLuong = heSoLuong,
                        NgaySinh = ngaySinh,
                        MaKhoa = maKhoa,
                        LoaiHinhDaoTao = loaiHinhDaoTao,
                        TrinhDo = trinhDo,
                    };

                    if (giangVien != null)
                    {
                        if (giangVienService.CapNhatThongTinGiangVien(giangVien))
                        {
                            return Json(new
                            {
                                success = 1,
                                message = "Cập nhật thông tin giảng viên thành công!"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không tìm thấy giảng viên!"
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
                message = "Có lỗi xảy ra, vui lòng thử lại sau"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult loadThongTinGiangVienTheoMa(string maGV)
        {
            try
            {
                if (string.IsNullOrEmpty(maGV))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Mã giảng viên không hợp lệ!"
                    }, JsonRequestBehavior.AllowGet);
                }

                GiangVienService giangVienService = new GiangVienService();
                var giangVien = giangVienService.GetGiangVienTheoMa(maGV);

                if (giangVien == null)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không tìm thấy giảng viên!"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    success = 1,
                    data = giangVien,
                    message = "Thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DowloadMauFile()
        {
            try
            {
                string filePath = Server.MapPath("/Content/MauFile/MauDanhSachGiangVienEmpty.xlsx");

                if (!System.IO.File.Exists(filePath))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không tìm thấy tệp mẫu!"
                    }, JsonRequestBehavior.AllowGet);
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                string fileName = "MauDanhSachGiangVien.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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
        public  ActionResult XoaDanhSachGiangVien()
        {
            try
            {
                string idGiangViensJson = Request.Form["idGiangViens"];
                List<string> idGiangViens = JsonConvert.DeserializeObject<List<string>>(idGiangViensJson);

                Console.WriteLine(idGiangViens[0]);

                GiangVienService giangVienService = new GiangVienService();
                if (idGiangViens != null && idGiangViens.Any())
                {
                    bool isDeleted =  giangVienService.XoaDanhSachGiangVien(idGiangViens);
                    if (isDeleted)
                    {
                        return Json(new
                        {
                            success = 1,
                            message = "Đã khóa " + idGiangViens.Count + " giảng viên thành công!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            success = 0,
                            message = "Xóa giảng viên không thành công!"
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

        public ActionResult DanhSachGiangVienDaKhoa()
        {
            // Lấy tên người dùng từ Session
            ViewBag.UserName = Session["FullName"] as string ?? "Khách";

            return View();
        }

        [HttpPost]
        public ActionResult loadDanhSachGiangVienDaKhoa(int page = 1, int pageSize = 5)
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();

                // Gọi phương thức lấy danh sách giảng viên bị khóa (phiên bản đồng bộ)
                var result = giangVienService.LoadDanhSachGiangVienDaKhoa(page, pageSize);

                if (result.data != null)
                {
                    var danhSachGiangVien = result.Item1;
                    var tongSoLuong = result.Item2;

                    if (danhSachGiangVien != null && danhSachGiangVien.Any())
                    {
                        return Json(new
                        {
                            success = 1,
                            data = danhSachGiangVien,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "Lấy dữ liệu thành công!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult moKhoaDanhSachGiangVien()
        {
            try
            {
                string idGiangViensJson = Request.Form["idGiangViens"];
                List<string> idGiangViens = JsonConvert.DeserializeObject<List<string>>(idGiangViensJson);

                Console.WriteLine(idGiangViens[0]);

                GiangVienService giangVienService = new GiangVienService();
                if (idGiangViens != null && idGiangViens.Any())
                {
                    bool isUnlocked = giangVienService.MoKhoaDanhSachGiangVien(idGiangViens);
                    if (isUnlocked)
                    {
                        return Json(new
                        {
                            success = 1,
                            message = "Đã mở khóa " + idGiangViens.Count + " giảng viên thành công!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            success = 0,
                            message = "Mở khóa giảng viên không thành công!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuatDanhSachGiangVienDaKhoa()
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();
                var danhSachGiangVien = giangVienService.DanhSachGiangVienDaKhoa();

                if (danhSachGiangVien == null || !danhSachGiangVien.Any())
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không có dữ liệu để xuất file!"
                    }, JsonRequestBehavior.AllowGet);
                }

                FileUpload fileUpload = new FileUpload();
                string filePath = fileUpload.XuatFileDanhSachGiangVienDaKhoa(danhSachGiangVien);

                if (string.IsNullOrEmpty(filePath))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Lỗi tải file!"
                    }, JsonRequestBehavior.AllowGet);
                }

                string fileUrl = Path.Combine("/FileExport", filePath).Replace("\\", "/");

                if (string.IsNullOrEmpty(fileUrl))
                {
                    return Json(new { success = false, message = "Không thể tạo file Excel!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, fileUrl = fileUrl }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult laodTableGiangVienTheoChuoiTim(string chuoiTim, int page = 1, int pageSize = 5)
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();
                var result = giangVienService.GetDanhSachGiangVienTheoChuoiTim(chuoiTim, page, pageSize);

                if (result.data != null)
                {
                    var danhSach = result.Item1; // Dữ liệu giảng viên
                    var tongSoLuong = result.Item2; // Tổng số lượng

                    return Json(new
                    {
                        success = 1,
                        data = danhSach,
                        totalRecords = tongSoLuong,
                        totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                        currentPage = page,
                        message = "Lấy dữ liệu thành công!"
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

        [HttpPost]
        public ActionResult laodTableGiangVienTheoKhoa(string maKhoa, int page = 1, int pageSize = 5)
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();
                var result = giangVienService.GetDanhSachGiangVienTheoKhoa(maKhoa, page, pageSize);

                if (result.data != null)
                {
                    var danhSach = result.Item1; // Dữ liệu giảng viên
                    var tongSoLuong = result.Item2; // Tổng số lượng

                    return Json(new
                    {
                        success = 1,
                        data = danhSach,
                        totalRecords = tongSoLuong,
                        totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                        currentPage = page,
                        message = "Lấy dữ liệu thành công!"
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

        [HttpPost]
        public ActionResult laodTableGiangVienTheoKhoaVaChuoiTim(string chuoiTim, string maKhoa, int page = 1, int pageSize = 5)
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();
                var result = giangVienService.GetDanhSachGiangVienTheoChuoiTimVaKhoa(chuoiTim, maKhoa, page, pageSize);

                if (result.data != null)
                {
                    var danhSach = result.Item1; // Dữ liệu giảng viên
                    var tongSoLuong = result.Item2; // Tổng số lượng

                    return Json(new
                    {
                        success = 1,
                        data = danhSach,
                        totalRecords = tongSoLuong,
                        totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                        currentPage = page,
                        message = "Lấy dữ liệu thành công!"
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