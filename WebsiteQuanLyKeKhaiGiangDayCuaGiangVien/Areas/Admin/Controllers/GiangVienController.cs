using System;
using System.Linq;
using System.Web.Mvc;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Areas.Admin.Controllers
{
    public class GiangVienController : Controller
    {
        // GET: Admin/GiangVien
        public ActionResult CaiDatGiangVien()
        {
            return View();
        }

        public ActionResult InDexGiangVien()
        {
            try
            {
                int soLuong = 0;
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities())
                {
                    soLuong = context.GiangViens.ToList().Count();

                }
                return Content("Đay la trang giang vien! Số lượng giảng viên đang có trong cơ sở dữ liệu là: " + soLuong);

            }
            catch (Exception ex)
            {
                return Content(ex+"");
            }

           
        }
    }
}