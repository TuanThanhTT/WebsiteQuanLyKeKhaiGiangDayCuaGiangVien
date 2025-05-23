using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KeKhaiService;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Controllers
{
    public class KeKhaiController : Controller
    {
        // GET: KeKhai
        public ActionResult ThongBaoDotKeKhai()
        {
            ViewBag.UserName = Session["FullName"]; // Dùng Session đúng với ASP.NET Framework

            DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
            var dotKeKhai = dotKeKhaiService.LayThongTinDotKeKhaiHienTai();

            if (dotKeKhai != null)
            {
                if (dotKeKhaiService.DotKeKhaiSapMo())
                {
                    ViewBag.ThongBaoMo = "Đợt kê khai sắp mở";
                }
                return View(dotKeKhai);
            }

            return View(new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom.ThongBaoDotKeKhai());

        }
        public ActionResult XemPhanCongHocPhan()
        {
            ViewBag.UserName = Session["FullName"]; 
         
            return View();
        }

        [HttpPost]
        public JsonResult XemDotKeKhaiTheoHocKyNamHoc(int maNamHoc, int maHocKy)
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                var dsDotKeKhai = dotKeKhaiService.XemDanhSachDotKeKhaiTheoHocKyNamHoc(maNamHoc, maHocKy); // Không dùng await

                if (dsDotKeKhai != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsDotKeKhai,
                        message = "Thành công"
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
                message = "Có lỗi xảy ra"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadDotKeKhaiTheoHocKyNamHoc(int maNamHoc, int maHocKy)
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                var dsDotKeKhai = dotKeKhaiService.LoadDotKeKhaiTheoHocKyNamHoc(maNamHoc, maHocKy); // Không dùng await

                if (dsDotKeKhai != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsDotKeKhai,
                        message = "Thành công"
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
                message = "Có lỗi xảy ra"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult loadDanhSachPhanCongHocPhanTheoDot(int maDotKeKhai)
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                string maGV = Session["UserName"] as string; // Lấy Session trong ASP.NET MVC 5

                var ds = dotKeKhaiService.LoadDanhSachHocPhanDuocPhanCongTheoDot(maGV, maDotKeKhai); // Gọi hàm đồng bộ (bỏ await)

                if (ds != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = ds,
                        message = "Thành công"
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
                message = "Có lỗi xảy ra"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult loadDanhSachPhanCongHocPhanTheoDotGanNhat()
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                string maGV = Session["UserName"] as string; // Lấy Session trong ASP.NET MVC 5

                var result = dotKeKhaiService.LoadDanhSachHocPhanDuocPhanCongTheoDotGanNhat(maGV); // Gọi hàm đồng bộ

                if (result != null)
                {
                    var ds = result.Item1;  // Danh sách học phần
                    var thongTin = result.Item2; // Thông tin đợt kê khai gần nhất

                    if (ds != null && thongTin != null)
                    {
                        return Json(new
                        {
                            success = 1,
                            data = ds,
                            dotGanNhat = 1,
                            thongTinDotKeKhai = thongTin,
                            message = "Thành công"
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
                message = "Có lỗi xảy ra"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DowloadFilePhanCong(int maDotKeKhai)
        {
            
            try
            {
                string maGV = Session["UserName"] as string; // Lấy Session trong ASP.NET MVC 5
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                List<XemThongTinHocPhanDuocPhanCong> dsPhanCongTheoDot = new List<XemThongTinHocPhanDuocPhanCong>();
                int maDKK = maDotKeKhai;

                if (maDotKeKhai != 0)
                {
                    dsPhanCongTheoDot = dotKeKhaiService.LoadDanhSachHocPhanDuocPhanCongTheoDot(maGV, maDotKeKhai);
                }
                else
                {
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        var hocKy = context.HocKies.OrderByDescending(s => s.MaHocKy).FirstOrDefault();
                        var namHoc = context.NamHocs.OrderByDescending(s => s.Id).FirstOrDefault();

                        if (namHoc != null && hocKy != null)
                        {
                            var dotKeKhai = context.DotKeKhais
                                .Where(op => op.MaHocKy == hocKy.MaHocKy && op.MaNamHoc == namHoc.Id)
                                .FirstOrDefault();

                            if (dotKeKhai != null)
                            {
                                maDKK = dotKeKhai.MaDotKeKhai;
                                dsPhanCongTheoDot = dotKeKhaiService.LoadDanhSachHocPhanDuocPhanCongTheoDot(maGV, dotKeKhai.MaDotKeKhai);
                            }
                        }
                    }
                }

                if (dsPhanCongTheoDot != null && dsPhanCongTheoDot.Any())
                {
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        var dotKeKhai = context.DotKeKhais.Find(maDKK);

                        FileUpload fileUpload = new FileUpload();
                        string fileName = fileUpload.XuatFilePhanCongTheoGiangVien(dsPhanCongTheoDot);
                       // string fileUrl = Path.Combine("~/Content/FileExport", fileName).Replace("\\", "/");
                        string fileUrl = Url.Content("~/Content/FileExport/" + fileName);

                        if (string.IsNullOrEmpty(fileUrl))
                        {
                            return Json(new { success = false, message = "Không thể tạo file Excel!" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = true, fileUrl = fileUrl }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Tải file thất bại!"
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
                message = "Tải file thất bại!"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CacMonDaKeKhai()
        {
            ViewBag.UserName = Session["FullName"] as string;
            return View();
        }

        [HttpPost]
        public ActionResult loadHocPhanKeKhaiTheoDot(int maDotKeKhai, int page = 1, int pageSize = 5)
        {
            try
            {
                string maGV = Session["UserName"] as string;
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();

                List<XemThongTinKeKhaiDaDuyet> dsKeKhai = new List<XemThongTinKeKhaiDaDuyet>();
                int soLuongKeKhai = 0;
                ThongTinDotKeKhaiGanNhat thongTinDotKeKhai = null;

                if (maDotKeKhai == 0)
                {
                    // Lấy thông tin kê khai từ đợt gần nhất (bỏ async)
                    var result = keKhaiHocPhan.getHocPhanDaDuyetTheoDotGanNhat(maGV, page, pageSize);
                    dsKeKhai = result.Item1;
                    soLuongKeKhai = result.Item2;
                    thongTinDotKeKhai = result.Item3;
                }
                else
                {
                    // Lấy thông tin kê khai theo đợt cụ thể (bỏ async)
                    var result = keKhaiHocPhan.getHocPhanDaDuyetTheoDot(maGV, maDotKeKhai, page, pageSize);
                    dsKeKhai = result.Item1;
                    soLuongKeKhai = result.Item2;
                }

                // Kiểm tra danh sách có dữ liệu không
                if (dsKeKhai != null && dsKeKhai.Any())
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsKeKhai,
                        soLuong = soLuongKeKhai,
                        dotGanNhat = (maDotKeKhai == 0) ? 1 : 0,
                        ThongTinDotKeKhai = thongTinDotKeKhai,
                        totalRecords = soLuongKeKhai,
                        totalPages = (int)Math.Ceiling((double)soLuongKeKhai / pageSize),
                        currentPage = page,
                        message = "Lấy danh sách kê khai thành công!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Có lỗi xảy ra, vui lòng thử lại sau!404"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new
                {
                    success = 0,
                    message = "Có lỗi xảy ra, vui lòng thử lại sau!500"
                });
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau!400"
            });
        }

        [HttpPost]
        public JsonResult XemKeKhaiDaDuyetTheoMaKeKhai(int maKeKhai)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                string maGV = Session["UserName"] as string;
                string tenGV = "";

                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Find(maGV);
                    if (giangVien != null)
                    {
                        tenGV = giangVien.TenGV;
                    }
                }

                var thongTinKeKhai = keKhaiHocPhan.getThongTinKeKhaiDaDuyetTheoMa(maKeKhai, maGV);
                if (thongTinKeKhai != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = thongTinKeKhai,
                        maGV = maGV,
                        tenGV = tenGV,
                        message = "Lấy thông tin kê khai thành công"
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
                message = "Có lỗi xảy ra vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult XuatDanhSachKeKhaiTheoDot(int maDotKeKhai)
        {
            try
            {
                string maGV = Session["UserName"] as string;
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                List<XemThongTinKeKhaiDaDuyet> danhSachKeKhai = new List<XemThongTinKeKhaiDaDuyet>();
                string tenDotKeKhai = "";

                if (maDotKeKhai == 0)
                {
                    var result = keKhaiHocPhan.getAllHocPhanDaDuyetTheoDotGanNhat(maGV);
                    danhSachKeKhai = result.Item1;
                    tenDotKeKhai = result.Item2;
                }
                else
                {
                    var result = keKhaiHocPhan.getAllHocPhanDaDuyetTheoDot(maGV, maDotKeKhai);
                    danhSachKeKhai = result.Item1;
                    tenDotKeKhai = result.Item2;
                }

                FileUpload fileUpload = new FileUpload();
                string fileName = fileUpload.XuatFileKeKhaiCuaGiangVienTheoDot(maGV, danhSachKeKhai, tenDotKeKhai);
                string fileUrl = Url.Content("~/FileExport/" + fileName);

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
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult XoaFileTam(string fileName)
        {
            try
            {
                // Lấy đường dẫn thư mục chứa file
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/FileExport", fileName);

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

        public ActionResult KeKhaiHocPhan()
        {
            ViewBag.UserName = Session["FullName"] as string;
          
            return View();
        }

        public ActionResult KeKhaiMonHoc()
        {
            return RedirectToAction("ThongBaoDotKeKhai", "KeKhai");
        }

        [HttpPost]
        public ActionResult LoadTableHocPhanKeKhai(int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                string maGV = Session["UserName"] as string;

                // Gọi phương thức bất đồng bộ và chờ kết quả (do .NET Framework không hỗ trợ async tốt)
                var result = Task.Run(() => keKhaiHocPhan.getHocPhanDuocPhanCongChuaHoanThanhCuaGiangVien(maGV, page, pageSize)).Result;
                var dsPhanCong = result.Item1;
                var totalRecords = result.Item2;

                if (dsPhanCong != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsPhanCong,
                        soLuong = totalRecords,
                        totalRecords = totalRecords,
                        totalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                        currentPage = page,
                        message = "Lấy danh sách phân công thành công!"
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
                message = "Có lỗi xảy ra!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XemThongTinPhanCongHocPhan(int maPhanCong)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();

                // Gọi phương thức async bằng Task.Run().Result để đảm bảo đồng bộ
                var thongTinHocPhan = Task.Run(() => keKhaiHocPhan.XemThongTinPhanCongHocPhanTheoMa(maPhanCong)).Result;

                return Json(new
                {
                    success = 1,
                    data = thongTinHocPhan,
                    message = "Lấy thông tin học phần phân công thành công!"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new
                {
                    success = 0,
                    message = "Có lỗi xảy ra"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult HoanThanhKeKhai(int maPhanCong)
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();

                // Gọi phương thức async bằng Task.Run().Result để đảm bảo đồng bộ
                bool dotKeKhaiDangMo = Task.Run(() => dotKeKhaiService.KiemTraDotKeKhaiDangMo()).Result;
                if (!dotKeKhaiDangMo)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Cập nhật không thành công! Không có đợt kê khai nào đang mở."
                    }, JsonRequestBehavior.AllowGet);
                }

                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                string maGV = Session["UserName"] as string;

                bool hoanThanh = Task.Run(() => keKhaiHocPhan.HoanThanhKeKhai(maPhanCong, maGV)).Result;
                if (hoanThanh)
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Kê khai thành công! Vui lòng chờ quản trị viên duyệt."
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
                message = "Cập nhật không thành công"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CapNhatThongTinPhanCong(int maPhanCong, string tenLop, int siSo, string hinhThucDay)
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();

                // Gọi phương thức async theo cách đồng bộ
                bool dotKeKhaiDangMo = Task.Run(() => dotKeKhaiService.KiemTraDotKeKhaiDangMo()).Result;
                if (!dotKeKhaiDangMo)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Cập nhật thông tin không thành công. Không có đợt kê khai nào đang mở để cập nhật!"
                    }, JsonRequestBehavior.AllowGet);
                }

                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();

                // Gọi phương thức async theo cách đồng bộ
                bool capNhatThanhCong = Task.Run(() => keKhaiHocPhan.CapNhatThongTinHocPhanPhanCong(maPhanCong, tenLop, siSo, hinhThucDay)).Result;
                if (capNhatThanhCong)
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Cập nhật thông tin thành công!"
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
                message = "Cập nhật thông tin không thành công. Vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult loadPhanCongHocPhanChoKeKhai(int page = 1, int pageSize = 5)
        {
            try
            {
                string maGV = HttpContext.Session["UserName"] as string;
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();

                // Gọi phương thức async theo cách đồng bộ
                var result = Task.Run(() => keKhaiHocPhan.getHocPhanDaKeKhaiChoDuyet(maGV, page, pageSize)).Result;
                var dsSachPhanCongChoDuyet = result.Item1;
                var totalRecords = result.Item2;

                if (dsSachPhanCongChoDuyet != null)
                {
                    return Json(new
                    {
                        success = 1,
                        soLuong = totalRecords,
                        data = dsSachPhanCongChoDuyet,
                        totalRecords = totalRecords,
                        totalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                        currentPage = page,
                        message = "Load dữ liệu thành công!"
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
                message = "Có lỗi xảy ra! Vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }




    }
}