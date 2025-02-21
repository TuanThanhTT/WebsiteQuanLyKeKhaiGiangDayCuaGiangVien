using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KeKhaiService;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        public ActionResult ThongKe()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var fullName = Convert.ToString(Session["FullName"]); // Lấy giá trị từ Session

            if (string.IsNullOrEmpty(fullName))
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
            }

            ViewBag.UserName = fullName;
           
            return View();
        }

        [HttpPost]
        public ActionResult ThongKeTheoDot(int maDotKeKhai)
        {
            try
            {
                var thongKe = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();
                var result = thongKe.ThongKeTheoDotKeKhai(maDotKeKhai); // Không dùng await trong .NET Framework nếu không có async

                if (result != null)
                {
                    if (thongKe.ThongKeTienDoTheoDotThongKe(result.ID))
                    {
                        Console.WriteLine("Trả về thành công");
                        return Json(new
                        {
                            success = 1,
                            data = result,
                            message = "Thành công"
                        }, JsonRequestBehavior.AllowGet); // Cần `JsonRequestBehavior.AllowGet` trong ASP.NET MVC Framework
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
        public ActionResult LoadTienDoGiangVien(int maDotKeKhai, int page = 1, int pageSize = 5)
        {
            try
            {
                WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKe = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                // Gọi phương thức đồng bộ (nếu có)
                var result = thongKe.LoadThongTinThongKeTienDo(maDotKeKhai, page, pageSize);

                if (result.Item1 != null)
                {
                    var danhSachTienDo = result.Item1;
                    var tongSoLuong = result.Item2;

                    if (danhSachTienDo != null && danhSachTienDo.Any())
                    {
                        return Json(new
                        {
                            success = 1,
                            data = danhSachTienDo,
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
        public ActionResult LamMoiThongKe(int maDotKeKhai)
        {
            try
            {
                WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeservice = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                // Gọi phương thức đồng bộ (nếu có)
                var result = thongKeservice.ThongKeMoiTheoDotKeKhai(maDotKeKhai);

                if (result != null)
                {
                    if (thongKeservice.ThongKeTienDoMoiTheoDotThongKe(result.ID))
                    {
                        Console.WriteLine("Trả về thành công");
                        return Json(new
                        {
                            success = 1,
                            data = result,
                            message = "Thành công"
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
        public ActionResult ThongKeTheoDotMoiNhat()
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();

                // Gọi phương thức đồng bộ (nếu có)
                var thongTin = dotKeKhaiService.DotKeKhaiGanNhat();
                if (thongTin == null)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Có lỗi xảy ra, vui lòng thử lại sau!"
                    }, JsonRequestBehavior.AllowGet);
                }

                WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeservice = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();
                var result = thongKeservice.ThongKeTheoDotKeKhai(thongTin.maDotKeKhai);

                if (result != null)
                {
                    if (thongKeservice.ThongKeTienDoTheoDotThongKe(result.ID) == true)
                    {
                        return Json(new
                        {
                            success = 1,
                            data = result,
                            thongTin = thongTin,
                            message = "Thành công"
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
        public ActionResult XemChiTietTienDoGiangVien(string maGV, int maDotKeKhai)
        {
            try
            {
                WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                // Gọi phương thức đồng bộ (nếu có)
                var result = thongKeService.XemChiTietTienDo(maGV, maDotKeKhai);
                var thongTin = result.Item1;
                var danhSachMon = result.Item2;

                if (thongTin != null && danhSachMon != null && danhSachMon.Any())
                {
                    return Json(new
                    {
                        success = 1,
                        data = danhSachMon,
                        thongTin = thongTin,
                        message = "Lấy dữ liệu thành công!"
                    }, JsonRequestBehavior.AllowGet);
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
        public ActionResult XuatDanhSachThongKeTheoKhoa(string maKhoa, int maDotKeKhai)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhoa))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Khoa chưa được chọn!!"
                    }, JsonRequestBehavior.AllowGet);
                }

                WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                // Gọi phương thức đồng bộ (nếu có)
                var danhSach = thongKeService.ThongKeTienDoTheoKhoa(maKhoa, maDotKeKhai);

                if (danhSach != null && danhSach.Any())
                {
                    FileUpload fileUpload = new FileUpload();
                    string filePath = fileUpload.XuatFileThongKeTheoKhoa(danhSach); // Gọi đồng bộ

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
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không có thông tin phân công để thống kê!"
                    }, JsonRequestBehavior.AllowGet);
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
        public ActionResult XuatDanhSachThongKeTatCaKhoa(int maDotKeKhai)
        {
            try
            {
                WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                // Gọi phương thức đồng bộ (nếu có)
                var danhSach = thongKeService.ThongKeTienDoTatCaKhoa(maDotKeKhai);

                if (danhSach != null && danhSach.Any())
                {
                    FileUpload fileUpload = new FileUpload();
                    string filePath = fileUpload.XuatFileThongKe(danhSach); // Gọi đồng bộ

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
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không có thông tin phân công để thống kê!"
                    }, JsonRequestBehavior.AllowGet);
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
        public ActionResult XoaFileTam(string fileName)
        {
            try
            {
                string filePath = Path.Combine(Server.MapPath("~/FileExport/"), fileName);
                Console.WriteLine("File đã truyền vào: " + fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Json(new { success = true, message = "File đã được xóa." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "File không tồn tại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xóa file! " + ex.Message);
                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa file." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GiangVienHoanThanhKeKhai(int maDotKeKhai)
        {
            Session["FullName"] = Session["FullName"]; // Lấy lại FullName từ Session

            if (Session["maDotKeKhai"] != null)
            {
                Session["maDotKeKhai"] = "";
            }
            Session["maDotKeKhai"] = maDotKeKhai.ToString();

            DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
            var tenDotKeKhai = dotKeKhaiService.GetTenDotKeKhaiTheoMa(maDotKeKhai); // Gọi phương thức đồng bộ
            ViewBag.TenDotKeKhai = tenDotKeKhai;

            return View();
        }

        [HttpPost]
        public JsonResult LoadTableGiangVienHoanThanhKeKhai(int page = 1, int pageSize = 10)
        {
            try
            {
                int maDotKeKhai;
                if (int.TryParse(Session["maDotKeKhai"] as string, out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"] as string);
                }

                if (maDotKeKhai > 0)
                {
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức đồng bộ thay vì `await`
                    var result = thongKeService.ThongKeGiangVienHoanThanhKeKhaiTheoDot(maDotKeKhai, page, pageSize);
                    var danhSach = result.Item1;
                    var tongSoLuong = result.Item2;

                    if (danhSach != null)
                    {
                        return Json(new
                        {
                            success = 1,
                            data = danhSach,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "Lấy dữ liệu thành công!"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Đợt phân công chưa được chọn!"
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
        public JsonResult TimKiemGiangVienHoanThanh(string chuoiTim, int page = 1, int pageSize = 10)
        {
            try
            {
                int maDotKeKhai;
                if (int.TryParse(Session["maDotKeKhai"] as string, out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"] as string);
                }

                if (maDotKeKhai > 0)
                {
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức đồng bộ thay vì `await`
                    var result = thongKeService.TimKiemThongKeGiangVienHoanThanhKeKhaiTheoDot(maDotKeKhai, chuoiTim, page, pageSize);
                    var danhSach = result.Item1;
                    var tongSoLuong = result.Item2;

                    if (danhSach != null)
                    {
                        return Json(new
                        {
                            success = 1,
                            data = danhSach,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "Lấy dữ liệu thành công!"
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

        [HttpPost]
        public JsonResult XuatDanhSachGiangVienHoanThanhTheoDot()
        {
            try
            {
                int maDotKeKhai;
                if (int.TryParse(Session["maDotKeKhai"] as string, out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"] as string);
                }
                if (maDotKeKhai > 0)
                {
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức đồng bộ (nếu là async, cần thêm .Result)
                    var danhSach = thongKeService.DanhSachGiangVienHoanThanh(maDotKeKhai);
                    if (danhSach == null || !danhSach.Any())
                    {
                        return Json(new
                        {
                            success = 0,
                            message = "Không có dữ liệu thống kê để xuất file!"
                        });
                    }

                    FileUpload fileUpload = new FileUpload();
                    string filePath = fileUpload.XuatFileThongKeGiangVienHoanThanhKeKhaiTheoDot(danhSach);

                    if (string.IsNullOrEmpty(filePath))
                    {
                        return Json(new
                        {
                            success = 0,
                            message = "Lỗi tải file!"
                        });
                    }
                    string fileUrl = Path.Combine("/FileExport", filePath).Replace("\\", "/");
                    if (string.IsNullOrEmpty(fileUrl))
                    {
                        return Json(new { success = false, message = "Không thể tạo file Excel!" });
                    }
                    else
                    {
                        return Json(new { success = true, fileUrl = fileUrl });
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

        [HttpPost]
        public JsonResult TimKiemGiangVienHoanThanhTheoKhoa(string maKhoa, int page = 1, int pageSize = 10)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhoa))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Vui lòng chọn khoa tìm kiếm!"
                    });
                }

                Console.WriteLine("ma khoa tim la: " + maKhoa);
                int maDotKeKhai;
                if (int.TryParse(Session["maDotKeKhai"] as string, out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"] as string);
                }
                if (maDotKeKhai > 0)
                {
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức đồng bộ (nếu là async, cần thêm .Result)
                    var result = thongKeService.TimKiemGiangVienHoanThanhKeKhaiTheoKhoa(maKhoa, maDotKeKhai, page, pageSize);
                    var danhSach = result.Item1;
                    var tongSoLuong = result.Item2;

                    if (danhSach != null)
                    {
                        return Json(new
                        {
                            success = 1,
                            data = danhSach,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "Lấy dữ liệu thành công!"
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

        [HttpPost]
        public JsonResult TimKiemGiangVienHoanThanhTheoKhoaVaChuoiTim(string maKhoa, string chuoiTim, int page = 1, int pageSize = 10)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhoa) || string.IsNullOrEmpty(chuoiTim))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Vui lòng nhập khoa và chuỗi tìm kiếm!"
                    });
                }

                Console.WriteLine("Mã khoa tìm là: " + maKhoa);

                int maDotKeKhai;
                if (int.TryParse(Session["maDotKeKhai"] as string, out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"] as string);
                }

                if (maDotKeKhai > 0)
                {
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức đồng bộ (nếu là async, cần thêm .Result)
                    var result = thongKeService.TimKiemGiangVienHoanThanhKeKhaiTheoKhoaVaChuoiTim(maKhoa, chuoiTim, maDotKeKhai, page, pageSize);
                    var danhSach = result.Item1;
                    var tongSoLuong = result.Item2;

                    if (danhSach != null)
                    {
                        return Json(new
                        {
                            success = 1,
                            data = danhSach,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "Lấy dữ liệu thành công!"
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


        public ActionResult GiangVienChuaKeKhai(int maDotKeKhai)
        {
            ViewBag.UserName = Session["FullName"] as string;

            if (Session["maDotKeKhai"] != null)
            {
                Session["maDotKeKhai"] = "";
            }
            Session["maDotKeKhai"] = maDotKeKhai.ToString();

            DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();

            // Gọi phương thức bất đồng bộ bằng .Result nếu không có phiên bản đồng bộ
            string tenDotKeKhai = dotKeKhaiService.GetTenDotKeKhaiTheoMa(maDotKeKhai);
            ViewBag.TenDotKeKhai = tenDotKeKhai;

            return View();
        }

        [HttpPost]
        public JsonResult LoadTableGiangVienChuaHoanThanhKeKhai(int page = 1, int pageSize = 10)
        {
            try
            {
                int maDotKeKhai;
                if (Session["maDotKeKhai"] != null && int.TryParse(Session["maDotKeKhai"].ToString(), out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"].ToString());
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Đợt phân công chưa được chọn!"
                    });
                }

                if (maDotKeKhai > 0)
                {
                    Console.WriteLine("Mã đợt kê khai: " + maDotKeKhai);
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức bất đồng bộ bằng .Result nếu không có phiên bản đồng bộ
                    var result = thongKeService.ThongKeGiangVienChuaHoanThanhKeKhaiTheoDot(maDotKeKhai, page, pageSize);

                    if (result.Item1 != null)
                    {
                        var (danhSach, tongSoLuong) = result;

                        return Json(new
                        {
                            success = 1,
                            data = danhSach,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "Lấy dữ liệu thành công!"
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

        [HttpPost]
        public JsonResult XuatDanhSachGiangVienChuaHoanThanhTheoDot()
        {
            try
            {
                int maDotKeKhai;
                if (Session["maDotKeKhai"] != null && int.TryParse(Session["maDotKeKhai"].ToString(), out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"].ToString());
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Đợt kê khai chưa được chọn!"
                    });
                }

                if (maDotKeKhai > 0)
                {
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức bất đồng bộ bằng .Result nếu không có phiên bản đồng bộ
                    var danhSach = thongKeService.DanhSachGiangVienChuaHoanThanh(maDotKeKhai);

                    if (danhSach == null || !danhSach.Any())
                    {
                        return Json(new
                        {
                            success = 0,
                            message = "Không có dữ liệu thống kê để xuất file!"
                        });
                    }

                    FileUpload fileUpload = new FileUpload();
                    string filePath = fileUpload.XuatFileThongKeGiangVienChuaHoanThanhKeKhaiTheoDot(danhSach);

                    if (string.IsNullOrEmpty(filePath))
                    {
                        return Json(new
                        {
                            success = 0,
                            message = "Lỗi tải file!"
                        });
                    }

                    string fileUrl = Path.Combine("/FileExport", filePath).Replace("\\", "/");

                    if (string.IsNullOrEmpty(fileUrl))
                    {
                        return Json(new { success = false, message = "Không thể tạo file Excel!" });
                    }
                    else
                    {
                        return Json(new { success = true, fileUrl = fileUrl });
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

        [HttpPost]
        public JsonResult TimKiemGiangVienChuaHoanThanhKeKhaiTheoDot(string chuoiTim, int page = 1, int pageSize = 10)
        {
            try
            {
                int maDotKeKhai;
                if (Session["maDotKeKhai"] != null && int.TryParse(Session["maDotKeKhai"].ToString(), out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"].ToString());
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Đợt kê khai chưa được chọn!"
                    });
                }

                if (maDotKeKhai > 0)
                {
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức bất đồng bộ bằng .Result nếu không có phiên bản đồng bộ
                    var result = thongKeService.TimKiemGiangVienChuaHoanThanhKeKhai(chuoiTim, maDotKeKhai, page, pageSize);

                    if (result.data != null)
                    {
                        var (danhSach, tongSoLuong) = result;
                        return Json(new
                        {
                            success = 1,
                            data = danhSach,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "Lấy dữ liệu thành công!"
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

        [HttpPost]
        public JsonResult TimKiemGiangVienChuaHoanThanhTheoKhoa(string maKhoa, int page = 1, int pageSize = 10)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhoa))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Vui lòng chọn khoa tìm kiếm!"
                    });
                }

                Console.WriteLine("Mã khoa tìm là: " + maKhoa);

                int maDotKeKhai;
                if (Session["maDotKeKhai"] != null && int.TryParse(Session["maDotKeKhai"].ToString(), out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"].ToString());
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Đợt kê khai chưa được chọn!"
                    });
                }

                if (maDotKeKhai > 0)
                {
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức bất đồng bộ bằng .Result nếu không có phiên bản đồng bộ
                    var result = thongKeService.TimKiemGiangVienChuaHoanThanhKeKhaiTheoKhoa(maKhoa, maDotKeKhai, page, pageSize);

                    if (result.data != null)
                    {
                        var (danhSach, tongSoLuong) = result;
                        return Json(new
                        {
                            success = 1,
                            data = danhSach,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "Lấy dữ liệu thành công!"
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

        [HttpPost]
        public JsonResult TimKiemGiangVienChuaHoanThanhTheoKhoaVaChuoiTim(string maKhoa, string chuoiTim, int page = 1, int pageSize = 10)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhoa) || string.IsNullOrEmpty(chuoiTim))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Vui lòng nhập đầy đủ thông tin tìm kiếm!"
                    });
                }

                Console.WriteLine("Mã khoa tìm là: " + maKhoa);

                int maDotKeKhai;
                if (Session["maDotKeKhai"] != null && int.TryParse(Session["maDotKeKhai"].ToString(), out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"].ToString());
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Đợt kê khai chưa được chọn!"
                    });
                }

                if (maDotKeKhai > 0)
                {
                    WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe thongKeService = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe.ThongKe();

                    // Gọi phương thức bất đồng bộ bằng .Result nếu không có phiên bản đồng bộ
                    var result = thongKeService.TimKiemGiangVienChuaHoanThanhKeKhaiTheoKhoaVaChuoiTim(maKhoa, chuoiTim, maDotKeKhai, page, pageSize);

                    if (result.data != null)
                    {
                        var (danhSach, tongSoLuong) = result;
                        return Json(new
                        {
                            success = 1,
                            data = danhSach,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "Lấy dữ liệu thành công!"
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

        [HttpPost]
        public JsonResult XemChiTietGiangVienChuaHoanThanhKeKhai()
        {
            try
            {
                int maDotKeKhai;
                if (Session["maDotKeKhai"] != null && int.TryParse(Session["maDotKeKhai"].ToString(), out maDotKeKhai))
                {
                    maDotKeKhai = int.Parse(Session["maDotKeKhai"].ToString());

                    if (maDotKeKhai > 0)
                    {
                        return Json(new
                        {
                            success = 1,
                            data = maDotKeKhai,
                            message = "Thành công"
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