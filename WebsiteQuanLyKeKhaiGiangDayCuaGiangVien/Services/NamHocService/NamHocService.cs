
using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.NamHocService
{
    public class NamHocService : INamHocService
    {
        public bool CapNhatNamHoc(int maNamHoc, string tenNamHoc)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenNamHoc))
                {
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        var namHoc = context.NamHocs.Find(maNamHoc);
                        if (namHoc != null)
                        {
                            namHoc.TenNamHoc = tenNamHoc;
                            context.SaveChanges();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool ChinhSuaThongTinNamHoc(int maNamHoc, string tenNamHocMoi)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var namHoc = context.NamHocs.Find(maNamHoc);
                    if (namHoc != null)
                    {
                        namHoc.TenNamHoc = tenNamHocMoi;
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public List<HocKy> GetDanhSachHocKyTheoNamHoc(int maNamHoc)
        {
            var dsHocKy = new List<HocKy>();
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    dsHocKy = context.HocKies.Where(op => op.MaNamHoc == maNamHoc).ToList();
                    return dsHocKy;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<HocKy>();
        }

        public List<NamHoc> GetDanhSachNamHocTheoTrang(int page, int pageSize)
        {
            var dsNamHoc = new List<NamHoc>();
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var ds = context.NamHocs
                        .OrderByDescending(op => op.Id)
                        .Where(op => op.isDelete != true)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    if (ds != null && ds.Any())
                    {
                        dsNamHoc.AddRange(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dsNamHoc;
        }

        public int GetSoLuongNamHoc()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    return context.NamHocs.Count(op => op.isDelete != true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public List<NamHoc> GetTop5NamHocGanNhat()
        {
            var dsNamHoc = new List<NamHoc>();
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    int totalCount = context.NamHocs.Count();

                    if (totalCount > 5)
                    {
                        dsNamHoc = context.NamHocs
                            .OrderByDescending(s => s.Id)
                            .Where(op => op.isDelete != true)
                            .Take(5)
                            .ToList();
                    }
                    else
                    {
                        dsNamHoc = context.NamHocs
                            .OrderByDescending(s => s.Id)
                            .Where(op => op.isDelete != true)
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dsNamHoc;
        }

        public bool KhoaNamHoc(int maNamHoc)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var namHoc = context.NamHocs.Find(maNamHoc);
                    if (namHoc != null)
                    {
                        namHoc.isDelete = true;
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool ThemHocKy(string tenHocKy, int maNamHoc)
        {
            try
            {
                if (!string.IsNullOrEmpty(tenHocKy))
                {
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        var namHoc = context.NamHocs.Find(maNamHoc);
                        if (namHoc != null)
                        {
                            int id = context.HocKies.Any() ? context.HocKies.Max(hk => hk.MaHocKy) + 1 : 1;

                            var hocKy = new HocKy
                            {
                                MaHocKy = id,
                                TenHocKy = tenHocKy,
                                MaNamHoc = maNamHoc,
                            };
                            context.HocKies.Add(hocKy);
                            context.SaveChanges();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        public bool ThemHocKyMoiVaoNamHoc(int maNamHoc, string tenNamHoc)
        {
            try
            {
                Console.WriteLine("Ma nam hoc: " + maNamHoc);
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var namHoc = context.NamHocs.Find(maNamHoc);
                    if (namHoc != null)
                    {
                        int idHocKy = context.HocKies.Any() ? context.HocKies.Max(hk => hk.MaHocKy) + 1 : 1;
                        while (context.HocKies.Find(idHocKy) != null)
                        {
                            idHocKy++;
                        }

                        var hocKy = new HocKy
                        {
                            MaHocKy = idHocKy,
                            TenHocKy = tenNamHoc,
                            MaNamHoc = namHoc.Id,
                        };
                        Console.WriteLine("Ma hoc ky: " + hocKy.MaHocKy);
                        context.HocKies.Add(hocKy);
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Nam hoc null");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public bool ThemNamHoc(string tenNamHoc)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    int id = context.NamHocs.Any() ? context.NamHocs.Max(nh => nh.Id) + 1 : 1;

                    if (!string.IsNullOrEmpty(tenNamHoc))
                    {
                        var namHoc = new NamHoc
                        {
                            Id = id,
                            TenNamHoc = tenNamHoc,
                        };
                        context.NamHocs.Add(namHoc);
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool XoaNamHoc(int maNamHoc)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var namHoc = context.NamHocs.Find(maNamHoc);
                    if (namHoc != null)
                    {
                        namHoc.isDelete = true;
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

    }
}
