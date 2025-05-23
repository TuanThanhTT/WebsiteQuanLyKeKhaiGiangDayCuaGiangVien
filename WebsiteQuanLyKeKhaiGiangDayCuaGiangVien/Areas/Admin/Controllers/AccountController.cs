using System;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.AccountService;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account
        public ActionResult QuanLyTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadDanhSachTaiKhoan(int page, int pageSize)
        {
            try
            {
                AccountService accountService = new AccountService();
                var result = accountService.LoadDanhSachTaiKhoanQuanLy(page, pageSize); // Gọi phương thức đồng bộ

                if (result.data != null)
                {
                    var danhSachTaiKhoan = result.Item1;
                    var tongSoLuong = result.Item2;
                

                    return Json(new
                    {
                        success = 1,
                        data = danhSachTaiKhoan,
                        totalRecords = tongSoLuong,
                        totalPages = (int)Math.Ceiling((double)tongSoLuong / pageSize),
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
        public ActionResult LoadQuyenTruyCapTheoTaiKhoan(string maGV)
        {
            try
            {
                AccountService accountService = new AccountService();
                var quyenTruyCap = accountService.GetQuyenTruyCapCuaTaiKhoan(maGV); // Gọi phương thức đồng bộ

                if (quyenTruyCap > 0)
                {
                    return Json(new
                    {
                        success = 1,
                        data = quyenTruyCap,
                        message = "Lấy dữ liệu thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Không tìm thấy tài khoản"
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
        public ActionResult CapNhatQuyenTruyCap(string maGV, int maQuyenTruyCap)
        {
            try
            {
                AccountService accountService = new AccountService();

                if (string.IsNullOrEmpty(maGV) || maQuyenTruyCap <= 0)
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Mã quyền truy cập không hợp lệ!"
                    }, JsonRequestBehavior.AllowGet);
                }

                string currentUser = HttpContext.Session["UserName"] as string;
                if (!accountService.KiemTraQuyenCoTheSua(currentUser)) // Gọi phương thức đồng bộ
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Bạn không có quyền truy cập chức năng này!" + currentUser
                    }, JsonRequestBehavior.AllowGet);
                }

                if (accountService.ThayDoiQuyenTruyCap(maGV, maQuyenTruyCap)) // Gọi phương thức đồng bộ
                {
                    return Json(new
                    {
                        success = 1,
                        message = "Cập nhật quyền truy cập thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Cập nhật quyền truy cập không thành công!"
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
        public ActionResult LoaddanhSachTimKiemTaiKhoan(string chuoiTim, int page = 1, int pageSize = 5)
        {
            try
            {
                AccountService accountService = new AccountService();

                if (string.IsNullOrEmpty(chuoiTim))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Chuỗi tìm kiếm rỗng!"
                    }, JsonRequestBehavior.AllowGet);
                }

                var ketQua = accountService.TimKiemTaiKhoan(chuoiTim, page, pageSize); // Gọi phương thức đồng bộ
                var danhSach = ketQua.Item1;
                var tongSoLuong = ketQua.Item2;

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
        public ActionResult LoadDanhSachtaiKhoanTheoKhoa(string maKhoa, int page = 1, int pageSize = 5)
        {
            try
            {
                AccountService accountService = new AccountService();

                if (string.IsNullOrEmpty(maKhoa))
                {
                    return Json(new
                    {
                        success = 0,
                        message = "Chuỗi tìm kiếm rỗng!"
                    }, JsonRequestBehavior.AllowGet);
                }

                var ketQua = accountService.TimKiemTaiKhoanTheoKhoa(maKhoa, page, pageSize); // Gọi phương thức đồng bộ
                var danhSach = ketQua.Item1;
                var tongSoLuong = ketQua.Item2;

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
        public ActionResult LoadDanhSachtaiKhoanTheoKhoaVChuoiTim(string maKhoa, string chuoiTim, int page = 1, int pageSize = 5)
        {
            try
            {
                AccountService accountService = new AccountService();

                if (string.IsNullOrEmpty(maKhoa) || string.IsNullOrEmpty(chuoiTim))
                {
                    Console.WriteLine("Mã khoa và chuỗi tìm kiếm bị null: " + maKhoa + " - " + chuoiTim);
                    return Json(new
                    {
                        success = 0,
                        message = "Chuỗi tìm kiếm rỗng!"
                    }, JsonRequestBehavior.AllowGet);
                }

                var ketQua = accountService.TimKiemTaiKhoanTheoKhoaVaChuoiTim(maKhoa, chuoiTim, page, pageSize); // Gọi phương thức đồng bộ
                var danhSach = ketQua.Item1;
                var tongSoLuong = ketQua.Item2;

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