
using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.DotPhanCongServic
{
    public class DotPhanCongService : IDotPhanCongServic
    {
        public List<ChiTietDotPhanCong> GetDanhSachDotPhanCongTheoNamHoc(int maNamHoc)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var namHoc = context.NamHocs.Find(maNamHoc);
                    if (namHoc != null)
                    {
                        var danhSachDotPhanCong = context.DotPhanCongs
                                                         .Where(op => op.MaNamHoc == namHoc.Id)
                                                         .Select(op => new ChiTietDotPhanCong
                                                         {
                                                             id = op.Id,
                                                             tenDot = op.TenDotPhanCong
                                                         })
                                                         .ToList();
                        return danhSachDotPhanCong;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new List<ChiTietDotPhanCong>();
        }

        public (int maNamHoc, int maDotPhanCong) GetDotPhanCongHienTai()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotPhanCongHienTai = context.DotPhanCongs
                                                    .OrderByDescending(op => op.Id)
                                                    .FirstOrDefault();

                    if (dotPhanCongHienTai != null)
                    {
                        return (dotPhanCongHienTai.MaNamHoc, dotPhanCongHienTai.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (-1, -1);
        }

    }
}
