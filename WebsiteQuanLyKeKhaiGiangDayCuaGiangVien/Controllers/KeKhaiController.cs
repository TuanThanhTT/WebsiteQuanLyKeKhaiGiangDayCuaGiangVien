using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public JsonResult loadDanhSachPhanCongHocPhanTheoDotGanNhat(int maDotKeKhai)
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
                        string fileUrl = Path.Combine("/FileExport", fileName).Replace("\\", "/");

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