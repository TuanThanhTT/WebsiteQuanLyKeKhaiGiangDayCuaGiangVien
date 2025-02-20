
using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.HocPhanService
{
    public class HocPhanService : IHocPhanService
    {
        public bool CapNhatThongTinHocPhan(string maHocPhan, string tenHocPhan, int soTinChi, int lyThuyet, int thucHanh, string ghiChu)
        {
            try
            {
                if (string.IsNullOrEmpty(maHocPhan)) return false;
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var hocPhan = context.HocPhans.FirstOrDefault(hp => hp.MaHP == maHocPhan);
                    if (hocPhan != null)
                    {
                        hocPhan.TenHP = tenHocPhan;
                        hocPhan.SoTinChi = soTinChi;
                        hocPhan.LyThuyet = lyThuyet;
                        hocPhan.ThucHanh = thucHanh;
                        hocPhan.MoTa = ghiChu;
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

        public List<XemHocPhan> GetDanhSachHocPhan()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var danhSachHocPhan = context.HocPhans.Select(op => new XemHocPhan
                    {
                        maHP = op.MaHP,
                        tenHP = op.TenHP,
                    }).ToList();

                    if (danhSachHocPhan != null) return danhSachHocPhan;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new List<XemHocPhan>();
        }

        public HocPhan GetHocPhanTheoMa(string maHocPhan)
        {
            try
            {
                if (string.IsNullOrEmpty(maHocPhan)) return new HocPhan();
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var hocPhan = context.HocPhans.Find(maHocPhan);
                    if (hocPhan != null) return hocPhan;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new HocPhan();
        }

        public bool KiemTraMaHocPhanChuaTonTai(string maHocPhan)
        {
            try
            {
                if (string.IsNullOrEmpty(maHocPhan)) return false;
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var hocPhan = context.HocPhans.Find(maHocPhan);
                    if (hocPhan == null) return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public (List<ThongTinHocPhan> data, int tongSoLuong) LoadDanhSachHocPhan(int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var danhSachHocPhan = context.HocPhans
                                                .Where(op => op.IsDelete == false)
                                                .Skip((page - 1) * pageSize)
                                                .Take(pageSize)
                                                .Select(k => new ThongTinHocPhan
                                                {
                                                    maHocPhan = k.MaHP,
                                                    tenHocPhan = k.TenHP,
                                                    soTinChi = (int)k.SoTinChi
                                                })
                                                .ToList();

                    var total = context.HocPhans.Count(k => k.IsDelete == false);

                    if (danhSachHocPhan != null && danhSachHocPhan.Any())
                    {
                        return (danhSachHocPhan, total);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<ThongTinHocPhan>(), 0);
        }

        public bool ThemDanhSachHocPhanMoi(List<ThongTinUploadHocPhan> hocPhans)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    foreach (var item in hocPhans)
                    {
                        var hocPhan = new HocPhan
                        {
                            MaHP = item.maHP,
                            TenHP = item.tenHP,
                            SoTinChi = item.soTinChi,
                            LyThuyet = item.lyThuyet,
                            ThucHanh = item.thucHanh,
                            MoTa = item.ghiChu
                        };
                        context.HocPhans.Add(hocPhan);
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool ThemHocPhanMoi(string maHocPhan, string tenHocPhan, int soTinChi, int lyThuyet, int thucHanh, string ghiChu)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var hocPhan = new HocPhan
                    {
                        MaHP = maHocPhan,
                        TenHP = tenHocPhan,
                        LyThuyet = lyThuyet,
                        SoTinChi = soTinChi,
                        ThucHanh = thucHanh,
                        MoTa = ghiChu
                    };

                    context.HocPhans.Add(hocPhan);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public int ThemPhanCongHocPhanMoi(int maDotPhanCong, string maGV, string maHP, string namHoc, string hocKy, string ngayDay, string hinhThuc, string tenLop, int siSo)
        {
            try
            {
                if (string.IsNullOrEmpty(maGV) || string.IsNullOrEmpty(maHP) ||
                    string.IsNullOrEmpty(namHoc) || string.IsNullOrEmpty(hocKy) ||
                    string.IsNullOrEmpty(hinhThuc) || string.IsNullOrEmpty(tenLop))
                {
                    return -1;
                }

                if (siSo <= 0) return -1;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Find(maGV);
                    var hocPhan = context.HocPhans.Find(maHP);
                    var dotPhanCong = context.DotPhanCongs.Find(maDotPhanCong);

                    if (giangVien != null && hocPhan != null && dotPhanCong != null)
                    {
                        // Kiểm tra trùng dữ liệu với đợt phân công đó
                        var danhSachPhanCongTheoDot = context.PhanCongHocPhans
                            .Where(op => op.MaDotPhanCong == maDotPhanCong)
                            .ToList();

                        foreach (var item in danhSachPhanCongTheoDot)
                        {
                            if (item.MaGV == maGV && item.MaHP == maHP && item.HocKy == hocKy
                                && item.NamHoc == namHoc && item.ThoiGianDay == ngayDay)
                            {
                                return 0;
                            }
                        }

                        // Tiến hành thêm phân công mới
                        var idPhanCongMoi = context.PhanCongHocPhans.Count() + 1;
                        while (context.PhanCongHocPhans.Find(idPhanCongMoi) != null)
                        {
                            idPhanCongMoi++;
                        }

                        var phanCongMoi = new PhanCongHocPhan
                        {
                            MaPhanCongHocPhan = idPhanCongMoi,
                            MaGV = maGV,
                            MaHP = maHP,
                            HinhThucDay = hinhThuc,
                            HocKy = hocKy,
                            NamHoc = namHoc,
                            ThoiGianDay = ngayDay,
                            LopPhanCong = tenLop,
                            NgayTao = DateTime.Now,
                            SiSo = siSo,
                            TrangThai = "Chưa Hoàn Thành",
                            MaDotPhanCong = dotPhanCong.Id,
                        };

                        context.PhanCongHocPhans.Add(phanCongMoi);
                        context.SaveChanges();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }

        public bool XoaDanhSachHocPhan(List<string> danhSachHocPhans)
        {
            try
            {
                if (danhSachHocPhans == null || !danhSachHocPhans.Any())
                {
                    return false;
                }

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    // Lấy danh sách các học phần tương ứng
                    var hocPhans = context.HocPhans
                                          .Where(k => danhSachHocPhans.Contains(k.MaHP) && !k.IsDelete)
                                          .ToList();

                    if (hocPhans.Count != danhSachHocPhans.Count)
                    {
                        Console.WriteLine("Không tìm thấy học phần");
                        return false;
                    }

                    // Đánh dấu IsDelete = true
                    foreach (var hocphan in hocPhans)
                    {
                        hocphan.IsDelete = true;
                    }

                    // Lưu thay đổi
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return false;
        }
    }
}
