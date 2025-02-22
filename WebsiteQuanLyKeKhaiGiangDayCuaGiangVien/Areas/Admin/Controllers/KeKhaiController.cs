using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.DotPhanCongServic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.GiangVien;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.HocPhanService;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KeKhaiService;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.NamHocService;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Areas.Admin.Controllers
{
    public class KeKhaiController : Controller
    {
        private int maFilePhanCong = -1;
        // GET: Admin/KeKhai
        public ActionResult TaoDotKeKhai()
        {
            ViewBag.UserName = Session["FullName"] as string;
            ViewBag.NgayKetThuc = Session["NgayKetThuc"] as string;
            // Console.WriteLine("ngay ket thuc: " + Session["NgayKetThuc"]);
            return View();
        }

        [HttpPost]
        public ActionResult UpLoadFilePhanCong(HttpPostedFileBase file, string tenPhanCong, int maNamHoc)
        {
            // Console.WriteLine("co chay ham UpLoadFilePhanCong");
            DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
            //if(await dotKeKhaiService.KiemTraDotKeKhaiDangMoAsync())
            //{
            //   // Console.WriteLine("loi vi dang mo ke khai");
            //    return Json(new
            //    {
            //        success = 0,
            //        message = "Đợt kê khai hiện đang mở. Không thể tiến hành tạo phân công cho đợt mới!"
            //    });
            //}

            FileUpload fileUpload = new FileUpload();
            string filePath = fileUpload.UploadFilePhanCong(file);
            if (filePath == "")
            {
                //Console.WriteLine("loi vi chua chon file phan cong");
                return Json(new
                {
                    success = 0,
                    message = "Vui lòng chọn file phân công"
                });
            }
            //hien thị lên bang
            ReadFileExcel readFileExcel = new ReadFileExcel();
            var danhSachThongTinPhanCong = readFileExcel.DocFilePhanCong(filePath);
            var dsPhanCongTruoc =  dotKeKhaiService.LoadThongTinPhanCongTrong();
            if (dsPhanCongTruoc.Count > 0)
            {
                danhSachThongTinPhanCong.AddRange(dsPhanCongTruoc);
                for (int i = 0; i < danhSachThongTinPhanCong.Count; i++)
                {
                    danhSachThongTinPhanCong[i].id = (i + 1);
                }
            }
            var danhSachDaKiemTra =  readFileExcel.KiemTraHopLeThongTinPhanCong(danhSachThongTinPhanCong);
            //Console.WriteLine("Danh sach doc excel ban dau la: "+ danhSachThongTinPhanCong.Count);  
            bool check = false;
            for (int i = 0; i < danhSachDaKiemTra.Count; i++)
            {
                if (!string.IsNullOrEmpty(danhSachDaKiemTra[i].ghiChu))
                {
                    check = true;
                    break;
                }
            }

            if (!check)
            {
                this.maFilePhanCong =  fileUpload.SaveFilePhanCong(Path.GetFileName(filePath));
                if (this.maFilePhanCong != -1)
                {
                    //tao phan cong hoc phan
                    if ( dotKeKhaiService.TaoPhanCongHocPhan(danhSachDaKiemTra, tenPhanCong, maNamHoc) == false)
                    {
                        Console.WriteLine("Loi tao phan cong hoc phan");
                    }
                }
            }
            else
            {
                System.IO.File.Delete(filePath);
            }
            //Console.WriteLine("Co chay den cuoi ham");
            return Json(new
            {
                success = 1,
                data = danhSachDaKiemTra,
                valid = check,
                message = "Tải file phân công thành công. Phân công học phần thành công!"
            });
        }

       
        [HttpPost]
        public JsonResult LoadNamHoc()
        {
            try
            {
                NamHocService namHocService = new NamHocService();
                var ds = namHocService.GetTop5NamHocGanNhat();
                if (ds != null && ds.Any())
                {
                    return Json(new
                    {
                        success = 1,
                        data = ds,
                        message = "Lấy thông tin năm học thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // 🚀 Ghi log lỗi ra file hoặc hiển thị trong Debug
                System.Diagnostics.Debug.WriteLine("Lỗi LoadNamHoc: " + ex.Message);

                return Json(new
                {
                    success = 0,
                    message = "Lỗi khi lấy thông tin năm học: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = 0,
                message = "Không có thông tin năm học"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public  ActionResult LoadHocKyTheoNamHoc(int namHoc)
        {
            try
            {
                NamHocService namHocService = new NamHocService();
                var ds =  namHocService.GetDanhSachHocKyTheoNamHoc(namHoc);
                if (ds != null && ds.Any())
                {
                    return Json(new
                    {
                        success = 1,
                        data = ds,
                        message = "Load thông tin năm học thành công"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Json(new
            {
                success = 0,
                message = "Không có học kỳ nào cho năm học này!"
            });
        }

        [HttpPost]
        public ActionResult LoadBangPhanCong()
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                var danhSachDaTao =  dotKeKhaiService.LoadThongTinPhanCongTrong();

                if (danhSachDaTao.Count > 0)
                {
                    Console.WriteLine("so luong phan cong truoc do la: " + danhSachDaTao.Count);
                    return Json(new
                    {
                        success = 1,
                        data = danhSachDaTao,
                        valid = false,
                        message = $"Bạn đã tạo {danhSachDaTao.Count} phân công trước đó!"
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
                message = ""
            });
        }

        [HttpPost]
        public ActionResult DowloadMauFile()
        {
            var filePath = Path.Combine(Server.MapPath("~/Content/MauFile"), "MauPhanCongHocPhanEmpty.xlsx");
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = "MauPhanCongHocPhan.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        public ActionResult TaoDotKeKhaiMoi(string tenDotKeKhai, DateTime ngayBatDau, DateTime ngayKetThuc, int hocKy, int namHoc, string ghiChu)
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();

                if ( dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Tạo đợt kê khai mới không thành công. Hiện đang có một đợt kê khai hiện tại!"
                    });
                }

                string maGV = Session["UserName"]?.ToString();

                if ( dotKeKhaiService.TaoDotKeKhaiMoi(tenDotKeKhai, ngayBatDau, ngayKetThuc, maGV, hocKy, namHoc, false, ghiChu))
                {
                    Session["NgayKetThuc"] = ngayKetThuc.ToString();

                    return Json(new
                    {
                        success = 1,
                        message = "Tạo đợt kê khai mới thành công",
                        ngayKetThuc = ngayKetThuc.ToString()
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
                message = "Tạo đợt kê khai mới không thành công"
            });
        }

        [HttpPost]
        public ActionResult NgayKeThucKeKhai()
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                if ( dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    var dotKeKhai =  dotKeKhaiService.GetDotKeKhaiDangMo();
                    if (dotKeKhai != null &&  dotKeKhaiService.DotKeKhaiSapMo() == false)
                    {
                        return Json(new
                        {
                            success = 1,
                            ngayKetThuc = dotKeKhai.NgayKetThuc.ToString(),
                            sapMo = 0,
                            message = ""
                        });
                    }
                    else
                    {
                        if ( dotKeKhaiService.DotKeKhaiSapMo())
                        {
                            return Json(new
                            {
                                success = 1,
                                ngayBatDau = dotKeKhai.NgayBatDau.ToString(),
                                sapMo = 1,
                                message = ""
                            });
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
                message = ""
            });
        }

        public ActionResult BoSungPhanCongHocPhan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadDotPhanCongTheoNamHoc(int maNamHoc)
        {
            try
            {
                DotPhanCongService dotPhanCongService = new DotPhanCongService();
                var danhSach =  dotPhanCongService.GetDanhSachDotPhanCongTheoNamHoc(maNamHoc);
                if (danhSach != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = danhSach,
                        message = "Lấy dữ liệu thành công!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không có đợt phân công nào tìm thấy!"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau! " + maNamHoc
            }) ;
        }

        [HttpPost]
        public ActionResult loadTableHocPhanDaPhanCongGanNhat(int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhanService = new KeKhaiHocPhan();
                DotPhanCongService dotPhanCongService = new DotPhanCongService();
                var (namHoc, maPhanCong) =  dotPhanCongService.GetDotPhanCongHienTai();
                var (danhSach, tongSoLuong) =  keKhaiHocPhanService.GetDanhSachHocPhanDaPhanCongTheoDot(maPhanCong, page, pageSize);
                if (danhSach != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = danhSach,
                        maNamHoc = namHoc,
                        maDotPhanCong = maPhanCong,
                        totalRecords = tongSoLuong,
                        totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                        currentPage = page,
                        message = "Lấy dữ liệu thành công!"
                    });
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
            });
        }

        [HttpPost]
        public ActionResult loadTableHocPhanDaPhanCongTheoDot(int maDotPhanCong, int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhanService = new KeKhaiHocPhan();
                Console.WriteLine("Mã đợt phân công đã nhận là: " + maDotPhanCong);
                var (danhSach, tongSoLuong) =  keKhaiHocPhanService.GetDanhSachHocPhanDaPhanCongTheoDot(maDotPhanCong, page, pageSize);
                if (danhSach != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = danhSach,
                        totalRecords = tongSoLuong,
                        totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                        currentPage = page,
                        message = "Lấy dữ liệu thành công!" + maDotPhanCong
                    }) ;
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
        public ActionResult loadTableHocPhanDaPhanCongTheoKhoa(int maDotPhanCong, string maKhoa, int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhanService = new KeKhaiHocPhan();

                var (danhSach, tongSoLuong) =  keKhaiHocPhanService.GetDanhSachHocPhanDaPhanCongTheoKhoa(maKhoa, maDotPhanCong, page, pageSize);
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
        public ActionResult GetDanhSachGiangVienTheoKhoa(string maKhoa)
        {
            try
            {
                GiangVienService giangVienService = new GiangVienService();
                var danhSach =  giangVienService.GetDanhSachGiangVienTheoKhoa(maKhoa);
                if (danhSach != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = danhSach,
                        message = "Lấy dữ liệu thành công!"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra vui lòng thử lại sau!"
            });
        }

        [HttpPost]
        public ActionResult GetSanhSachHocPhan()
        {
            try
            {
                HocPhanService hocPhanService = new HocPhanService();
                var danhSachHocPhan =  hocPhanService.GetDanhSachHocPhan();
                if (danhSachHocPhan != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = danhSachHocPhan,
                        message = "Lấy dữ liệu thành công!"
                    });
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
            });
        }

        [HttpPost]
        public ActionResult  ThemHocPhanPhanCongMoi(int maDotPhanCong, string maGV, string maHP, string namHoc, string hocKy, string ngayDay, string hinhThuc, string tenLop, int siSo)
        {
            try
            {
                HocPhanService hocPhanService = new HocPhanService();
                var result =  hocPhanService.ThemPhanCongHocPhanMoi(maDotPhanCong, maGV, maHP, namHoc, hocKy, ngayDay, hinhThuc, tenLop, siSo);

                if (result == 0)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Học phần phân công bị trùng với các phân công trước đó!"
                    });
                }
                if (result == 1)
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Thêm phân công mới thành công!"
                    });
                }

            }
            catch (Exception ex) { Console.WriteLine(ex); }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra vui lòng thử lại sau!"
            });
        }

        [HttpPost]
        public ActionResult XemThongTinPhanCongTheoMa(int maDotPhanCong)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhanServic = new KeKhaiHocPhan();
                var thongTin =  keKhaiHocPhanServic.ThongTinHocPhanPhanCongCuaGiangVienTheoMa(maDotPhanCong);
                if (thongTin != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = thongTin,
                        message = "Lấy dữ liệu thành công!"
                    });
                }

            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra vui lòng thử lại sau!"
            });
        }

        [HttpPost]
        public ActionResult CapNhatThongTinPhanCongTheoMa(int maPhanCongHocPhan, string maGV, string tenLop, string namHoc, string hocKy, string thoiGianDay, string maHP, string hinhThucDay, int siSo)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhanServic = new KeKhaiHocPhan();
                var result =  keKhaiHocPhanServic.CapNhatPhanCongHocPhanTheoMa(maPhanCongHocPhan, maGV, tenLop, namHoc, hocKy, thoiGianDay, maHP, hinhThucDay, siSo);
                if (result)
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Cập nhật thông tin thành công!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Cập nhật thông tin không thành công!"
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra vui lòng thử lại sau!"
            });
        }

        [HttpPost]
        public ActionResult loadTablePhanCongHocPhanTheoChuoiTim(int maDotPhanCong, string chuoiTim, int page = 1, int pageSize = 5)
        {
            try
            {
                if (string.IsNullOrEmpty(chuoiTim))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Có lỗi xảy ra, vui lòng thử lại sau"
                    });
                }
                KeKhaiHocPhan keKhaiHocPhanServic = new KeKhaiHocPhan();

                var (danhSach, tongSoLuong) =  keKhaiHocPhanServic.GetDanhSachHocPhanPhanCongTheoChuoiTim(chuoiTim, maDotPhanCong, page, pageSize);
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
        public ActionResult loadTablePhanCongHocPhanTheoChuoiTimVaKhoa(int maDotPhanCong, string chuoiTim, string maKhoa, int page = 1, int pageSize = 5)
        {
            try
            {
                Console.WriteLine("madophancong " + maDotPhanCong + " chuoitim: " + chuoiTim + " makhoa: " + maKhoa);

                if (string.IsNullOrEmpty(chuoiTim))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Có lỗi xảy ra, vui lòng thử lại sau"
                    });
                }

                KeKhaiHocPhan keKhaiHocPhanServic = new KeKhaiHocPhan();
                var (danhSach, tongSoLuong) =  keKhaiHocPhanServic.GetDanhSachHocPhanPhanCongTheoChuoiTimVaKhoa(chuoiTim, maKhoa, maDotPhanCong, page, pageSize);

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
        public ActionResult XoaDanhSachPhanCong()
        {
            try
            {
                var idPhanCongs = JsonConvert.DeserializeObject<List<int>>(Request.Form["idPhanCongs"]);

                Console.WriteLine(idPhanCongs[0]);

                KeKhaiHocPhan keKhaiHocPhanService = new KeKhaiHocPhan();

                if (idPhanCongs != null && idPhanCongs.Any())
                {
                    if ( keKhaiHocPhanService.XoaDanhSachPhanCongHocPhan(idPhanCongs))
                    {
                        return Json(new
                        {
                            success = 1,
                            message = "Đã khóa " + idPhanCongs.Count + " học phần phân công thành công!"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = 0,
                            message = "Xóa giảng viên không thành công!"
                        });
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra, vui lòng thử lại sau!"
            });
        }

        public ActionResult TaoDotKeKhaiBoSung()
        {
            // Gán tên người dùng và ngày kết thúc vào các ViewBag khác nhau
            ViewBag.UserName = Session["FullName"];
            ViewBag.NgayKetThuc = Session["NgayKetThuc"];

            // Đặt tiêu đề cho trang
        

            // Trả về view
            return View();
        }

        [HttpPost]
        public ActionResult layThongTinDotKeKhaiDangMo()
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();

                // Kiểm tra xem đợt kê khai có đang mở không
                if ( dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    var dotKeKhaiHienTai =  dotKeKhaiService.GetDotKeKhaiDangMo();

                    if (dotKeKhaiHienTai != null)
                    {
                        // Truy cập context của Entity Framework
                        using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                        {
                            var hocKy = context.HocKies.Find(dotKeKhaiHienTai.MaHocKy)?.TenHocKy;
                            var namHoc = context.NamHocs.Find(dotKeKhaiHienTai.MaNamHoc)?.TenNamHoc;

                            var thongTinDotKeKhai = new ThongTinTaoDotKeKhai();
                            thongTinDotKeKhai.id = dotKeKhaiHienTai.MaDotKeKhai;
                            thongTinDotKeKhai.tenDotKeKhai = dotKeKhaiHienTai.TenDotKeKhai;
                            thongTinDotKeKhai.ngayKetThuc = dotKeKhaiHienTai.NgayKetThuc?.ToString("dd/MM/yyyy HH:mm");
                            thongTinDotKeKhai.ngayBatDau = dotKeKhaiHienTai.NgayBatDau?.ToString("dd/MM/yyyy HH:mm");
                            thongTinDotKeKhai.ghiChu = dotKeKhaiHienTai.GhiChu;
                            thongTinDotKeKhai.namHoc = string.IsNullOrEmpty(namHoc) ? "" : namHoc;
                            thongTinDotKeKhai.hocKy = string.IsNullOrEmpty(hocKy) ? "" : hocKy;

                            // Lấy tên giảng viên tạo đợt kê khai
                            var giangVien =  context.GiangViens.Find(dotKeKhaiHienTai.MaGV);
                            thongTinDotKeKhai.nguoiTao = giangVien.TenGV;

                            return Json(new
                            {
                                success = 1,
                                data = thongTinDotKeKhai,
                                message = ""
                            });
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
                message = "Không có đợt kê khai nào đang mở."
            });
        }

        [HttpPost]
        public ActionResult TaoDotKeKhaiDangKyBoSung(string tenDotKeKhai, DateTime ngayBatDau, DateTime ngayKT, int hocKy, int namHoc, string ghiChu)
        {
            try
            {
                Console.WriteLine("Ngay bat dau: " + ngayBatDau.ToString());
                Console.WriteLine("Ngay ket thuc: " + ngayKT);

                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();

                // Kiểm tra xem có đợt kê khai đang mở không
                if ( dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    Console.WriteLine("co chay o day");
                    return Json(new
                    {
                        success = 0,
                        message = "Tạo đợt đăng ký bổ sung không thành công. Hiện đang có đợt kê khai đang mở!",
                    });
                }
                else
                {
                    string maGV = Session["UserName"]?.ToString(); // Lấy giá trị từ Session
                    Console.WriteLine("co chay vao day");

                    Console.WriteLine("Ngay ket thuc: " + ngayKT.ToString());
                    if ( dotKeKhaiService.TaoDotKeKhaiDangKyBoSung(maGV, tenDotKeKhai, ngayBatDau, ngayKT, hocKy, namHoc, ghiChu, false))
                    {
                        return Json(new
                        {
                            success = 1,
                            message = "Tạo đợt kê khai đăng ký bổ sung thành công!",
                        });
                    }
                    Console.WriteLine("loi o duoi");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Tạo đợt kê khai đăng ký bổ sung không thành công!",
            });
        }

        [HttpPost]
        public ActionResult XemDotKeKhaiHienTaiDangMo()
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                if ( dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    var dotKeKhaiHienTai =  dotKeKhaiService.GetDotKeKhaiDangMo();
                    if (dotKeKhaiHienTai != null)
                    {
                        Console.WriteLine("ghi chú: " + dotKeKhaiHienTai.GhiChu);
                        using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                        {
                            var hocKy = context.HocKies.Find(dotKeKhaiHienTai.MaHocKy).TenHocKy;
                            var namHoc = context.NamHocs.Find(dotKeKhaiHienTai.MaNamHoc).TenNamHoc;
                            var thongTinDotKeKhai = new ThongTinTaoDotKeKhai();
                            thongTinDotKeKhai.id = dotKeKhaiHienTai.MaDotKeKhai;
                            thongTinDotKeKhai.ghiChu = dotKeKhaiHienTai.GhiChu;
                            thongTinDotKeKhai.hocKy = string.IsNullOrEmpty(hocKy) ? "" : hocKy;
                            thongTinDotKeKhai.namHoc = string.IsNullOrEmpty(namHoc) ? "" : namHoc;
                            thongTinDotKeKhai.ngayBatDau = dotKeKhaiHienTai.NgayBatDau?.ToString("dd/MM/yyyy HH:mm:ss tt");
                            thongTinDotKeKhai.ngayKetThuc = dotKeKhaiHienTai.NgayKetThuc?.ToString("dd/MM/yyyy HH:mm:ss tt");
                            thongTinDotKeKhai.tenDotKeKhai = dotKeKhaiHienTai.TenDotKeKhai;
                            var giangVien =  context.GiangViens.Find(dotKeKhaiHienTai.MaGV);
                            thongTinDotKeKhai.nguoiTao = giangVien.TenGV;

                            return Json(new
                            {
                                success = 1,
                                data = thongTinDotKeKhai,
                                message = "Thành công!"
                            });
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
                message = "Không có đợt kê khai nào đang mở. Vui lòng liên hệ quản trị!"
            });
        }

        [HttpPost]
        public ActionResult KhoaDotKeKhaiHienTai()
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                if ( dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    var dotKeKhaiHienTai =  dotKeKhaiService.GetDotKeKhaiDangMo();
                    if (dotKeKhaiHienTai != null)
                    {
                        if ( dotKeKhaiService.KhoaDotKeKhaiHienTai(dotKeKhaiHienTai.MaDotKeKhai))
                        {
                            return Json(new
                            {
                                success = 1,
                                message = "Khóa đợt kê khai hiện tại thành công!"
                            });
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
                message = "Có lỗi xảy ra, vui lòng thử lại!"
            });
        }


        [HttpPost]
        public ActionResult HienThiThongTinDotKeKhaiDangMo()
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                if ( dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    var dotKeKhaiHienTai =  dotKeKhaiService.GetDotKeKhaiDangMo();
                    if (dotKeKhaiHienTai != null)
                    {
                        using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                        {
                            var tenHocKy = context.HocKies.Find(dotKeKhaiHienTai.MaHocKy).TenHocKy;
                            var tenNamHoc = context.NamHocs.Find(dotKeKhaiHienTai.MaNamHoc).TenNamHoc;
                            var xemThongTinDotKeKhai = new XemThongTinDotKeKhai
                            {
                                MaDotKeKhai = dotKeKhaiHienTai.MaDotKeKhai,
                                TenDotKeKhai = dotKeKhaiHienTai.TenDotKeKhai,
                                NgayBatDau = (DateTime)dotKeKhaiHienTai.NgayBatDau,
                                NgayKetThuc = (DateTime)dotKeKhaiHienTai.NgayKetThuc,
                                MaGv = dotKeKhaiHienTai.MaGV,
                                KeKhaiMoi = (bool)dotKeKhaiHienTai.KeKhaiMoi,
                                GhiChu = dotKeKhaiHienTai.GhiChu,
                                HocKy = string.IsNullOrEmpty(tenHocKy) ? "" : tenHocKy,
                                NamHoc = string.IsNullOrEmpty(tenNamHoc) ? "" : tenNamHoc
                            };

                            return Json(new
                            {
                                success = 1,
                                data = xemThongTinDotKeKhai,
                                message = "Thành công!"
                            });
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
                message = "Không tìm thấy đợt kê khai nào đang mở!"
            });
        }

        [HttpPost]
        public ActionResult ChinhSuaDotKeKhaiDangMo(DateTime ngayKetThuc, string ghiChu)
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                if ( dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    var dotKeKhai =  dotKeKhaiService.GetDotKeKhaiDangMo();
                    if (dotKeKhai != null)
                    {
                        if (dotKeKhaiService.CapNhatThoiGianDotKeKhai(dotKeKhai.MaDotKeKhai, ngayKetThuc, ghiChu))
                        {
                            return Json(new
                            {
                                success = 1,
                                message = "Cập nhật đợt kê khai thành công!"
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = 0,
                                message = "Lỗi ngày kết thúc cần phải lớn hơn ngày kết thúc cũ!"
                            });
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
                message = "Cập nhật thông tin không thành công. Vui lòng thử lại!"
            });
        }

        [HttpPost]
        public ActionResult UploadFileBoSungPhanCongHocPhan(HttpPostedFileBase file, int idDotPhanCong)
        {
            try
            {
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                //if(await dotKeKhaiService.KiemTraDotKeKhaiDangMoAsync())
                //{
                //var dotKeKhaiHienTai = await dotKeKhaiService.getDotKeKhaiDangMo();
                //if(dotKeKhaiHienTai !=null)
                //{
                //Console.WriteLine("loai dot ke khai hien tai: " + dotKeKhaiHienTai?.KeKhaiMoi);
                //if(dotKeKhaiHienTai?.KeKhaiMoi == true)
                //{
                //     return Json(new
                //     {
                //         success = 0,
                //         message = "Đợt kê khai hiện tại đang là đợt kê khai đăng ký bổ sung, không thể tải file phân công bổ sung!"
                //     });
                //}
                //  else
                //  {
                //tien hanh phan cong
                FileUpload fileUpload = new FileUpload();
                ReadFileExcel readFileExcel = new ReadFileExcel();

                string filePath =  fileUpload.UploadFilePhanCong(file);

                if (filePath == "")
                {
                    Console.WriteLine("loi vi chua chon file phan cong");
                    return Json(new
                    {
                        success = 0,
                        message = "Vui lòng chọn file phân công"
                    });
                }
                //hien thị lên bang
                var danhSachThongTinPhanCong =  readFileExcel.DocFilePhanCong(filePath);
                //load phan cong da tao truoc do

                var dsPhanCongTruoc =  dotKeKhaiService.LoadThongTinPhanCongDotDangMo();

                if (dsPhanCongTruoc.Count > 0)
                {
                    danhSachThongTinPhanCong.AddRange(dsPhanCongTruoc);
                    for (int i = 0; i < danhSachThongTinPhanCong.Count; i++)
                    {
                        danhSachThongTinPhanCong[i].id = (i + 1);
                    }
                }
                var danhSachDaKiemTra =  readFileExcel.KiemTraHopLeThongTinPhanCongBoSung(danhSachThongTinPhanCong, idDotPhanCong);
                Console.WriteLine("Danh sach doc excel ban dau la: " + danhSachThongTinPhanCong.Count);
                bool check = false;
                for (int i = 0; i < danhSachDaKiemTra.Count; i++)
                {
                    if (!string.IsNullOrEmpty(danhSachDaKiemTra[i].ghiChu))
                    {
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    this.maFilePhanCong =  fileUpload.SaveFilePhanCong(Path.GetFileName(filePath));
                    if (this.maFilePhanCong != -1)
                    {
                        //tao phan cong hoc phan
                        if ( dotKeKhaiService.TaoBoSungPhanCongHocPhan(danhSachDaKiemTra, idDotPhanCong) == false)
                        {
                            Console.WriteLine("Loi tao phan cong hoc phan");
                        }
                    }
                }
                else
                {
                    System.IO.File.Delete(filePath);
                }
                Console.WriteLine("Co chay den cuoi ham");
                return Json(new
                {
                    success = 1,
                    data = danhSachDaKiemTra,
                    valid = check,
                    message = "Tải file phân công thành công. Phân công học phần thành công!"
                });
                // }
                //}

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return Json(new
            {
                success = 0,
                message = "Tải file phân công không thành công!"
            });
        }

        public ActionResult XemPhanCongKeKhai()
        {
            ViewBag.UserName = Session["FullName"];
          
            return View();
        }

        [HttpPost]
        public ActionResult loadHocPhanPhanCongTheoDot(int maDotKeKhai, int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                List<ThongTinPhanCongGiangVien> dsPhanCong = new List<ThongTinPhanCongGiangVien>();
                string tenDotKeKhai = null, tenNamHoc = null, tenHocKy = null;
                int tongSoLuong = 0;
                int idDotKeKhai = 0;
                if (maDotKeKhai == 0)
                {
                    var thongTinDotkeKhai = new ThongTinDotKeKhaiGanNhat();
                    (dsPhanCong, tongSoLuong, idDotKeKhai, tenDotKeKhai, tenNamHoc, tenHocKy, thongTinDotkeKhai) = keKhaiHocPhan.getHocPhanPhanCongTheoDotGanNhat(page, pageSize);
                    return Json(new
                    {
                        success = 1,
                        data = dsPhanCong,
                        dotGanNhat = 1,
                        thongTinDotKeKhai = thongTinDotkeKhai,
                        tenDotKeKhai = tenDotKeKhai,
                        tenNamHoc = tenNamHoc,
                        tenHocKy = tenHocKy,
                        maDotKeKhai = idDotKeKhai,
                        totalRecords = tongSoLuong,
                        totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                        currentPage = page,
                        message = "lấy dữ liệu thành công!"
                    });
                }
                else
                {
                    var result = keKhaiHocPhan.getHocPhanPhanCongTheoDot(maDotKeKhai, page, pageSize);
                    dsPhanCong = result.Item1;
                    tongSoLuong = result.Item2;
                    idDotKeKhai = result.Item3;
                    tenDotKeKhai = result.Item4;
                    tenNamHoc = result.Item5;
                    tenHocKy = result.Item6;

                    if (dsPhanCong != null)
                    {
                        return Json(new
                        {
                            success = 1,
                            data = dsPhanCong,
                            tenDotKeKhai = tenDotKeKhai,
                            tenNamHoc = tenNamHoc,
                            tenHocKy = tenHocKy,
                            maDotKeKhai = idDotKeKhai,
                            totalRecords = tongSoLuong,
                            totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                            currentPage = page,
                            message = "lấy dữ liệu thành công!"
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
                message = "Có lỗi xảy ra vui lòng thử lại sau!"
            });
        }

        [HttpPost]
        public ActionResult loadPhanCongHocPhanTheoKhoa(string maKhoa, int maDotKeKhai, int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                List<ThongTinPhanCongGiangVien> dsPhanCong = new List<ThongTinPhanCongGiangVien>();
                string tenDotKeKhai = null, tenNamHoc = null, tenHocKy = null;
                int tongSoLuong = 0;
                int idDotKeKhai = 0;

                if (maDotKeKhai == 0)
                {
                    var result = keKhaiHocPhan.getHocPhanPhanCongTheoDotVaKhoaGanNhat(maKhoa, page, pageSize);
                    dsPhanCong = result.Item1;
                    tongSoLuong = result.Item2;
                    idDotKeKhai = result.Item3;
                    tenDotKeKhai = result.Item4;
                    tenNamHoc = result.Item5;
                    tenHocKy = result.Item6;
                }
                else
                {
                    var result = keKhaiHocPhan.getHocPhanPhanCongTheoDotVaKhoa(maKhoa, maDotKeKhai, page, pageSize);
                    dsPhanCong = result.Item1;
                    tongSoLuong = result.Item2;
                    idDotKeKhai = result.Item3;
                    tenDotKeKhai = result.Item4;
                    tenNamHoc = result.Item5;
                    tenHocKy = result.Item6;
                }

                if (dsPhanCong != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsPhanCong,
                        tenDotKeKhai = tenDotKeKhai,
                        tenNamHoc = tenNamHoc,
                        tenHocKy = tenHocKy,
                        maDotKeKhai = idDotKeKhai,
                        totalRecords = tongSoLuong,
                        totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
                        currentPage = page,
                        message = "lấy dữ liệu thành công!"
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra vui lòng thử lại sau!"
            });
        }

        [HttpPost]
        public ActionResult xemChiTietPhanCongCuaGiangVien(string maGV, int maDotKeKhai)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                var result = keKhaiHocPhan.getDanhSachHocPhanDuocPhanCongCuaGiangVienTheoDot(maGV, maDotKeKhai);

                var dsPhanCong = result.Item1;
                var idGV = result.Item2;
                var tenGiangVien = result.Item3;
                var tenKhoa = result.Item4;

                if (dsPhanCong != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsPhanCong,
                        idGV = idGV,
                        tenGiangVien = tenGiangVien,
                        tenKhoa = tenKhoa,
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
                message = "Có lỗi xảy ra vui lòng thử lại sau."
            });
        }

        public ActionResult DuyetKeKhai()
        {
            ViewBag.UserName = Session["FullName"];
            ViewBag.NgayKetThuc = Session["NgayKetThuc"];
           
            return View();
        }

        [HttpPost]
        public ActionResult loadThongTinKhoa()
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                var dsKhoa =  keKhaiHocPhan.getDanhSachKhoa();
                if (dsKhoa != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsKhoa,
                        message = "Lấy danh sách khoa thành công!"
                    }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet để hỗ trợ GET
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra. Vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult getDanhSachDuyetKeKhaiChoDuyet(int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                var (dsGiangVien, soLuong) =  keKhaiHocPhan.getKeKhaiGiangVien(page, pageSize);
                if (dsGiangVien != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsGiangVien,
                        totalRecords = soLuong,
                        totalPages = (int)Math.Ceiling((double)soLuong / pageSize),
                        currentPage = page,
                        message = "Load danh sách thành công!"
                    }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet
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
        public ActionResult getDanhSachGiangVienKeKhaiTheoDot(int maDotKeKhai, int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                var (dsKeKhai, soLuong) =  keKhaiHocPhan.getKeKhaiGiangVienTheoDot(maDotKeKhai, page, pageSize);
                if (dsKeKhai != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsKeKhai,
                        totalRecords = soLuong,
                        totalPages = (int)Math.Ceiling((double)soLuong / pageSize),
                        currentPage = page,
                        message = "Load danh sách thành công!"
                    }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra. Vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet
        }

        [HttpPost]
        public  ActionResult getDanhSachGiangVienKeKhaiTheoDotVaKhoa(string maKhoa, int maDotKeKhai, int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                var (dsKeKhai, soLuong) =  keKhaiHocPhan.getKeKhaiGiangVienTheoDotVaKhoa(maKhoa, maDotKeKhai, page, pageSize);
                if (dsKeKhai != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsKeKhai,
                        totalRecords = soLuong,
                        totalPages = (int)Math.Ceiling((double)soLuong / pageSize),
                        currentPage = page,
                        message = "Load danh sách thành công!"
                    }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra. Vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet
        }

        [HttpPost]
        public ActionResult getDanhSachDuyetKeKhaiChoDuyetTheoKhoa(string maKhoa, int page = 1, int pageSize = 5)
        {
            try
            {
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                var (dsGiangVien, soLuong) =  keKhaiHocPhan.getKeKhaiGiangVienTheoKhoa(maKhoa, page, pageSize);

                Console.WriteLine("Load dữ liệu theo khoa: " + dsGiangVien.Count());

                if (dsGiangVien != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsGiangVien,
                        totalRecords = soLuong,
                        totalPages = (int)Math.Ceiling((double)soLuong / pageSize),
                        currentPage = page,
                        message = "Load danh sách thành công!"
                    }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet
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
            }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet
        }

        [HttpPost]
        public  ActionResult XemDanhSachKeKhaiChoDuyetTheoGiangVien(string maGV)
        {
            try
            {
                // Console.WriteLine("Mã giang vien : " + maGV);
                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                var dsKeKhaiChoDuyet =  keKhaiHocPhan.getDanhSachKeKhaiChoDuyetCuaGiangVien(maGV);
                if (dsKeKhaiChoDuyet != null)
                {
                    return Json(new
                    {
                        success = 1,
                        data = dsKeKhaiChoDuyet,
                        message = "Lấy thông tin thành công!"
                    }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra. Vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet); // Thêm JsonRequestBehavior.AllowGet
        }

        [HttpPost]
        public ActionResult DuyetKeKhaiHoanThanh(int maKeKhai)
        {
            try
            {
                string maGV = HttpContext.Session["UserName"] as string;
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();

                // Kiểm tra xem đợt kê khai có đang mở không
                if ( dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Kê khai hiện đang mở, không thể duyệt học phần kê khai. Vui lòng chờ đợt kê khai kết thúc!"
                    }, JsonRequestBehavior.AllowGet);  // Chỉ định JsonRequestBehavior.AllowGet
                }

                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();

                // Duyệt kê khai học phần hoàn thành
                if (keKhaiHocPhan.DuyetKeKhaiHoanThanh(maKeKhai, maGV))
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Duyệt kê khai học phần thành công!"
                    }, JsonRequestBehavior.AllowGet);  // Chỉ định JsonRequestBehavior.AllowGet
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Nếu có lỗi xảy ra
            return Json(new
            {
                success = 0,
                message = "Duyệt kê khai không thành công. Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);  // Chỉ định JsonRequestBehavior.AllowGet
        }

        [HttpPost]
        public ActionResult DuyetToanBoKeKhaiCuaGiangVien(FormCollection form)
        {
            try
            {
                string maGV = Session["UserName"] as string;  // Lấy maGV từ session

                if (string.IsNullOrEmpty(maGV))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không tìm thấy thông tin giảng viên. Vui lòng đăng nhập lại!"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Lấy danh sách ID kê khai từ request
                string listIdKeKhaiString = form["listIdKeKhai"];
                List<int> listIdKeKhai = new List<int>();

                if (!string.IsNullOrEmpty(listIdKeKhaiString))
                {
                    listIdKeKhai = listIdKeKhaiString.Split(',').Select(int.Parse).ToList();
                }

                if (listIdKeKhai.Count == 0)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Danh sách kê khai không hợp lệ!"
                    }, JsonRequestBehavior.AllowGet);
                }

                KeKhaiHocPhan keKhaiHocPhan = new KeKhaiHocPhan();
                DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();

                // Kiểm tra xem đợt kê khai có đang mở không
                if (dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Kê khai hiện đang mở, không thể duyệt học phần kê khai. Vui lòng chờ đợt kê khai kết thúc!"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Duyệt danh sách kê khai hoàn thành
                if (keKhaiHocPhan.DuyetDanhSachKeKhaiHoanThanh(listIdKeKhai, maGV))
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Duyệt kê khai học phần thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Học phần kê khai không tồn tại!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Nếu có lỗi xảy ra
            return Json(new
            {
                success = 0,
                message = "Duyệt kê khai không thành công. Có lỗi xảy ra, vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DuyetTatCaKeKhai()
        {
            try
            {
                string maGV = HttpContext.Session["UserName"] as string;  // Truy cập session để lấy maGV
                KeKhaiHocPhan keKhaiHocPhanServic = new KeKhaiHocPhan();

                // Gọi phương thức DuyetTatCaKeKhai với tham số maGV
                if ( keKhaiHocPhanServic.DuyetTatCaKeKhai(maGV))
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Duyệt tất cả kê khai thành công!"
                    }, JsonRequestBehavior.AllowGet);  // Chỉ định JsonRequestBehavior.AllowGet
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Lỗi không duyệt được kê khai!"
                    }, JsonRequestBehavior.AllowGet);  // Chỉ định JsonRequestBehavior.AllowGet
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            // Trả về thông báo lỗi nếu có vấn đề xảy ra
            return Json(new
            {
                success = 0,
                message = "Có lỗi xảy ra vui lòng thử lại sau!"
            }, JsonRequestBehavior.AllowGet);  // Chỉ định JsonRequestBehavior.AllowGet
        }





    }
}