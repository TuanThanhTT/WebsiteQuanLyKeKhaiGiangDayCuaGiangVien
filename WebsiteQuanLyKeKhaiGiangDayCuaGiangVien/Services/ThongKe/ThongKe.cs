
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe
{
    public class ThongKe : IThongKeService
    {
        public (List<Models.ModelCustom.ThongKeChiTiet>, int) LoadThongTinThongKeTienDo(int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                Console.WriteLine("ma dot ke khai trong ham LoadThongTinThongKeTienDo: " + maDotKeKhai);
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var thongKe = context.ThongKes.FirstOrDefault(op => op.MaDotKeKhai == maDotKeKhai);
                    if (thongKe != null)
                    {
                        var danhSachTienDo = context.ThongKeChiTiets
                            .Where(op => op.MaThongKe == thongKe.ID)
                            .OrderBy(op => op.Id)
                            .Select(op=>new Models.ModelCustom.ThongKeChiTiet
                            {
                                DaThuHien = op.DaThuHien,
                                Id = op.Id,
                                MaGV = op.MaGV,
                                MaThongKe = op.MaThongKe,
                                SoLuongPhanCong = op.SoLuongPhanCong,
                                TenGV = op.TenGV,
                                TenKhoa = op.TenKhoa,
                                
                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        int soLuong = context.ThongKeChiTiets.Count(op => op.MaThongKe == thongKe.ID);
                        return (danhSachTienDo, soLuong);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<Models.ModelCustom.ThongKeChiTiet>(), 0);
        }

        public (List<GiangVienHoanThanhKeKhai>, int) ThongKeGiangVienHoanThanhKeKhaiTheoDot(int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        var query = from phanCong in context.PhanCongHocPhans
                                    join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                    join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                    where phanCong.MaDotKeKhai == maDotKeKhai
                                    group phanCong by new { giangVien.MaGV, giangVien.TenGV, khoa.TenKhoa } into grouped
                                    select new
                                    {
                                        Key = grouped.Key,
                                        SoLuongChoDuyet = grouped.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                        SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                    };

                        var queryFiltered = query
                            .Where(g => g.SoLuongChuaHoanThanh == 0 && g.SoLuongChoDuyet == 0)
                            .Select(g => new GiangVienHoanThanhKeKhai
                            {
                                maGV = g.Key.MaGV,
                                tenGV = g.Key.TenGV,
                                tenKhoa = g.Key.TenKhoa,
                                tienDo = "Hoàn thành"
                            });

                        int soLuong = queryFiltered.Count();
                        var danhSach = queryFiltered
                            .OrderBy(gv => gv.maGV)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        return (danhSach, soLuong);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<GiangVienHoanThanhKeKhai>(), 0);
        }


        public WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ThongKe ThongKeMoiTheoDotKeKhai(int maDotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        var thongKeCu = context.ThongKes
                            .FirstOrDefault(op => op.MaDotKeKhai == maDotKeKhai && op.NgayTao > DateTime.UtcNow.AddHours(-24));

                        if (thongKeCu != null)
                        {
                            return thongKeCu;
                        }

                        var thongKeMoi = new Models.ThongKe
                        {
                            MaDotKeKhai = maDotKeKhai,
                            NgayTao = DateTime.UtcNow
                        };

                        context.ThongKes.Add(thongKeMoi);
                        context.SaveChanges();

                        return thongKeMoi;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public Models.ThongKe ThongKeTheoDotKeKhai(int maDotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        var thongkecu = context.ThongKes.FirstOrDefault(op => op.MaDotKeKhai == maDotKeKhai && op.NgayTao > DateTime.UtcNow.AddHours(-24));
                        if (thongkecu != null)
                        {
                            return thongkecu;
                        }
                        else if (context.ThongKes.Any(op => op.MaDotKeKhai == maDotKeKhai))
                        {
                            var thongKeCu = context.ThongKes.First(op => op.MaDotKeKhai == maDotKeKhai);
                            var query = context.PhanCongHocPhans.Where(pc => pc.MaDotKeKhai == maDotKeKhai);

                            thongKeCu.SoLuongGiangVienPhanCong = query.Select(pc => pc.MaGV).Distinct().Count();
                            thongKeCu.SoLuongGiangVienDaHoanThanh = query.GroupBy(pc => pc.MaGV)
                                .Count(gvGroup => gvGroup.All(pc => pc.TrangThai == "Hoàn Thành"));
                            thongKeCu.SoLuongGiangVienChuaHoanThanh = thongKeCu.SoLuongGiangVienPhanCong - thongKeCu.SoLuongGiangVienDaHoanThanh;
                            thongKeCu.SoLuongHocPhan = query.Count();
                            thongKeCu.SoLuongHocPhanDaDuocKeKhai = query.Count(pc => pc.TrangThai == "Hoàn thành");
                            thongKeCu.SoLuongPhanCongChoDuyet = query.Count(pc => pc.TrangThai == "Chờ duyệt");
                            thongKeCu.SoLuongHocPhanCacDotTruoc = context.PhanCongHocPhans.Count(pc => pc.MaDotKeKhai > maDotKeKhai && pc.TrangThai == "Chưa Hoàn Thành");
                            thongKeCu.NgayTao = DateTime.Now;
                            context.SaveChanges();
                            return thongKeCu;
                        }
                        else
                        {
                            int id = context.ThongKes.Count() + 1;
                            while (context.ThongKes.Find(id) != null)
                            {
                                id++;
                            }
                            var query = context.PhanCongHocPhans.Where(pc => pc.MaDotKeKhai == maDotKeKhai);

                            var thongKeResult = new Models.ThongKe
                            {
                                ID = id,
                                MaDotKeKhai = maDotKeKhai,
                                SoLuongGiangVienPhanCong = query.Select(pc => pc.MaGV).Distinct().Count(),
                                SoLuongGiangVienDaHoanThanh = query.GroupBy(pc => pc.MaGV)
                                    .Count(gvGroup => gvGroup.All(pc => pc.TrangThai == "Hoàn Thành")),
                                SoLuongGiangVienChuaHoanThanh = query.Select(pc => pc.MaGV).Distinct().Count() -
                                                                query.GroupBy(pc => pc.MaGV)
                                                                    .Count(gvGroup => gvGroup.All(pc => pc.TrangThai == "Hoàn Thành")),
                                SoLuongHocPhan = query.Count(),
                                SoLuongHocPhanDaDuocKeKhai = query.Count(pc => pc.TrangThai == "Hoàn thành"),
                                SoLuongPhanCongChoDuyet = query.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                SoLuongHocPhanCacDotTruoc = context.PhanCongHocPhans.Count(pc => pc.MaDotKeKhai > maDotKeKhai && pc.TrangThai == "Chưa Hoàn Thành"),
                                NgayTao = DateTime.Now,
                            };
                            context.ThongKes.Add(thongKeResult);
                            context.SaveChanges();
                            return thongKeResult;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new Models.ThongKe();
        }


        public bool ThongKeTienDoMoiTheoDotThongKe(int maThongKe)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var thongKe = context.ThongKes.Find(maThongKe);
                    if (thongKe != null)
                    {
                        // Kiểm tra xem đã có thống kê trước đó hay chưa
                        var soLuongThongKeCu = context.ThongKeChiTiets.Count(op => op.MaThongKe == thongKe.ID);
                        if (soLuongThongKeCu > 0)
                        {
                            var danhSachTienDo = context.ThongKeChiTiets.Where(op => op.MaThongKe == maThongKe).ToList();
                            if (danhSachTienDo == null || danhSachTienDo.Count == 0)
                            {
                                return false;
                            }

                            // Tính lại thống kê
                            int index = context.ThongKeChiTiets.Count();

                            var data = (from phanCong in context.PhanCongHocPhans
                                        join giangVien in context.GiangViens
                                        on phanCong.MaGV equals giangVien.MaGV
                                        join khoa in context.Khoas
                                        on giangVien.MaKhoa equals khoa.MaKhoa
                                        where phanCong.MaDotKeKhai == thongKe.MaDotKeKhai
                                        group phanCong by new
                                        {
                                            phanCong.MaGV,
                                            giangVien.TenGV,
                                            khoa.TenKhoa
                                        } into grouped
                                        select new
                                        {
                                            MaGv = grouped.Key.MaGV,
                                            TenGv = grouped.Key.TenGV,
                                            TenKhoa = grouped.Key.TenKhoa,
                                            SoLuongPhanCong = grouped.Count(),
                                            DaThuHien = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                        }).ToList();

                            if (data != null && data.Count > 0)
                            {
                                var query = data.Select(x => new Models.ThongKeChiTiet
                                {
                                    Id = index++,
                                    MaThongKe = thongKe.ID,
                                    MaGV = x.MaGv,
                                    TenGV = x.TenGv,
                                    TenKhoa = x.TenKhoa,
                                    SoLuongPhanCong = x.SoLuongPhanCong,
                                    DaThuHien = x.DaThuHien
                                }).ToList();

                                context.ThongKeChiTiets.RemoveRange(danhSachTienDo);
                                context.ThongKeChiTiets.AddRange(query);
                                context.SaveChanges();
                                return true;
                            }
                        }
                        else
                        {
                            // Tiến hành thống kê mới
                            int index = context.ThongKeChiTiets.Count();

                            var data = (from phanCong in context.PhanCongHocPhans
                                        join giangVien in context.GiangViens
                                        on phanCong.MaGV equals giangVien.MaGV
                                        join khoa in context.Khoas
                                        on giangVien.MaKhoa equals khoa.MaKhoa
                                        where phanCong.MaDotKeKhai == thongKe.MaDotKeKhai
                                        group phanCong by new
                                        {
                                            phanCong.MaGV,
                                            giangVien.TenGV,
                                            khoa.TenKhoa
                                        } into grouped
                                        select new
                                        {
                                            MaGv = grouped.Key.MaGV,
                                            TenGv = grouped.Key.TenGV,
                                            TenKhoa = grouped.Key.TenKhoa,
                                            SoLuongPhanCong = grouped.Count(),
                                            DaThuHien = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                        }).ToList();

                            if (data != null && data.Count > 0)
                            {
                                var query = data.Select(x => new Models.ThongKeChiTiet
                                {
                                    Id = index++,
                                    MaThongKe = thongKe.ID,
                                    MaGV = x.MaGv,
                                    TenGV = x.TenGv,
                                    TenKhoa = x.TenKhoa,
                                    SoLuongPhanCong = x.SoLuongPhanCong,
                                    DaThuHien = x.DaThuHien
                                }).ToList();

                                context.ThongKeChiTiets.AddRange(query);
                                context.SaveChanges();
                                return true;
                            }
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

        public bool ThongKeTienDoTheoDotThongKe(int maThongKe)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var thongKe = context.ThongKes.Find(maThongKe);
                    if (thongKe != null)
                    {
                        // Kiểm tra xem đã có thống kê chưa
                        var soLuongThongKeCu = context.ThongKeChiTiets.Where(op => op.MaThongKe == thongKe.ID).Count();
                        if (soLuongThongKeCu > 0)
                        {
                            return true;
                        }
                        else
                        {
                            // Tiến hành thống kê
                            int index = context.ThongKeChiTiets.Count();
                            while (context.ThongKeChiTiets.Find(index) != null)
                            {
                                index++;
                            }

                            var data = (from phanCong in context.PhanCongHocPhans
                                        join giangVien in context.GiangViens
                                        on phanCong.MaGV equals giangVien.MaGV
                                        join khoa in context.Khoas
                                        on giangVien.MaKhoa equals khoa.MaKhoa
                                        where phanCong.MaDotKeKhai == thongKe.MaDotKeKhai
                                        group phanCong by new
                                        {
                                            phanCong.MaGV,
                                            giangVien.TenGV,
                                            khoa.TenKhoa
                                        } into grouped
                                        select new
                                        {
                                            MaGv = grouped.Key.MaGV,
                                            TenGv = grouped.Key.TenGV,
                                            TenKhoa = grouped.Key.TenKhoa,
                                            SoLuongPhanCong = grouped.Count(),
                                            DaThuHien = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                        }).ToList();

                            if (data != null && data.Any())
                            {
                                var query = data.Select(x => new Models.ThongKeChiTiet
                                {
                                    Id = index++,
                                    MaThongKe = thongKe.ID,
                                    MaGV = x.MaGv,
                                    TenGV = x.TenGv,
                                    TenKhoa = x.TenKhoa,
                                    SoLuongPhanCong = x.SoLuongPhanCong,
                                    DaThuHien = x.DaThuHien
                                }).ToList();
                                context.ThongKeChiTiets.AddRange(query);
                                context.SaveChanges();
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }


        public (List<GiangVienHoanThanhKeKhai> , int ) TimKiemThongKeGiangVienHoanThanhKeKhaiTheoDot(int maDOtKeKhai, string chuoiTim, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDOtKeKhai);
                    if (dotKeKhai != null)
                    {
                        chuoiTim = chuoiTim.ToLower().Trim();
                        var query = from phanCong in context.PhanCongHocPhans
                                    join giangVien in context.GiangViens
                                    on phanCong.MaGV equals giangVien.MaGV
                                    join khoa in context.Khoas
                                    on giangVien.MaKhoa equals khoa.MaKhoa
                                    where phanCong.MaDotKeKhai == maDOtKeKhai
                                    group phanCong by new
                                    {
                                        giangVien.MaGV,
                                        giangVien.TenGV,
                                        khoa.TenKhoa
                                    } into grouped
                                    select new
                                    {
                                        Key = grouped.Key,
                                        SoLuongChoDuyet = grouped.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                        SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                        TongSoLuong = grouped.Count(),
                                        SoLuongHoanThanh = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                    };

                        var queryFiltered = query.Where(g => g.SoLuongChuaHoanThanh == 0 && g.SoLuongChoDuyet == 0)
                                                 .Select(g => new GiangVienHoanThanhKeKhai
                                                 {
                                                     maGV = g.Key.MaGV,
                                                     tenGV = g.Key.TenGV,
                                                     tenKhoa = g.Key.TenKhoa,
                                                     tienDo = $"{g.SoLuongHoanThanh}/{g.TongSoLuong}"
                                                 });

                        var chuoiTimKhongDau = RemoveDiacritics(chuoiTim).ToLower();
                        var danhSachAll = queryFiltered.ToList(); // Lấy toàn bộ danh sách từ SQL Server

                        var danhSach = danhSachAll
                            .Where(op => RemoveDiacritics(op.maGV).ToLower().Contains(chuoiTimKhongDau) ||
                                         RemoveDiacritics(op.tenGV).ToLower().Contains(chuoiTimKhongDau))
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        var soLuong = danhSach.Count;
                        if (danhSach != null && danhSach.Any())
                        {
                            return (danhSach, soLuong);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<GiangVienHoanThanhKeKhai>(), 0);
        }

        public string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            string normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        public (ThongTinChiTietTienDo thongTinTienDo, List<DanhSachMonHocThogKeTienDo> data) XemChiTietTienDo(string maGV, int maDotKeKhai)
        {
            try
            {
                if (string.IsNullOrEmpty(maGV)) return (new ThongTinChiTietTienDo(), new List<DanhSachMonHocThogKeTienDo>());

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKekhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKekhai != null)
                    {
                        var result = (from phanCong in context.PhanCongHocPhans
                                      join giangVien in context.GiangViens
                                      on phanCong.MaGV equals giangVien.MaGV
                                      join khoa in context.Khoas
                                      on giangVien.MaKhoa equals khoa.MaKhoa
                                      join dotkk in context.DotKeKhais
                                      on phanCong.MaDotKeKhai equals dotkk.MaDotKeKhai
                                      join namHoc in context.NamHocs
                                      on dotkk.MaNamHoc equals namHoc.Id
                                      join hocKy in context.HocKies
                                      on dotkk.MaHocKy equals hocKy.MaHocKy
                                      where giangVien.MaGV == maGV && dotkk.MaDotKeKhai == maDotKeKhai
                                      group phanCong by new
                                      {
                                          giangVien.MaGV,
                                          giangVien.TenGV,
                                          khoa.TenKhoa,
                                          dotkk.TenDotKeKhai,
                                          hocKy.TenHocKy,
                                          namHoc.TenNamHoc
                                      } into g
                                      select new ThongTinChiTietTienDo
                                      {
                                          maGV = g.Key.MaGV,
                                          tenGV = g.Key.TenGV,
                                          tenKhoa = g.Key.TenKhoa,
                                          tenDotPhanCong = g.Key.TenDotKeKhai,
                                          tenHocKy = g.Key.TenHocKy,
                                          tenNamHoc = g.Key.TenNamHoc,
                                          soLuongPhanCong = g.Count(),
                                          soLuongHoanThanh = g.Count(pc => pc.TrangThai == "Hoàn thành"),
                                          soLuongChuaHoanThanh = g.Count(pc => pc.TrangThai == "Chưa Hoàn Thành")
                                      }).FirstOrDefault();

                        var danhSachMonHoc = (from phanCong in context.PhanCongHocPhans
                                              join hocPhan in context.HocPhans
                                              on phanCong.MaHP equals hocPhan.MaHP
                                              where phanCong.MaGV == maGV && phanCong.MaDotKeKhai == maDotKeKhai
                                              select new DanhSachMonHocThogKeTienDo
                                              {
                                                  id = phanCong.MaPhanCongHocPhan,
                                                  maHP = hocPhan.MaHP,
                                                  tenHP = hocPhan.TenHP,
                                                  ngayDay = phanCong.ThoiGianDay,
                                                  tenLop = phanCong.LopPhanCong,
                                                  siSo = (int)phanCong.SiSo,
                                                  trangThai = phanCong.TrangThai
                                              }).ToList();

                        if (result != null && danhSachMonHoc != null)
                        {
                            return (result, danhSachMonHoc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new ThongTinChiTietTienDo(), new List<DanhSachMonHocThogKeTienDo>());
        }

        public List<ThongTinChiTietTienDo> ThongKeTienDoTheoKhoa(string maKhoa, int maDotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKekhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKekhai != null)
                    {
                        var result = (from phanCong in context.PhanCongHocPhans
                                      join giangVien in context.GiangViens
                                      on phanCong.MaGV equals giangVien.MaGV
                                      join khoa in context.Khoas
                                      on giangVien.MaKhoa equals khoa.MaKhoa
                                      join dotkk in context.DotKeKhais
                                      on phanCong.MaDotKeKhai equals dotkk.MaDotKeKhai
                                      join namHoc in context.NamHocs
                                      on dotkk.MaNamHoc equals namHoc.Id
                                      join hocKy in context.HocKies
                                      on dotkk.MaHocKy equals hocKy.MaHocKy
                                      where dotkk.MaDotKeKhai == maDotKeKhai && khoa.MaKhoa == maKhoa
                                      group phanCong by new
                                      {
                                          giangVien.MaGV,
                                          giangVien.TenGV,
                                          khoa.TenKhoa,
                                          dotkk.TenDotKeKhai,
                                          hocKy.TenHocKy,
                                          namHoc.TenNamHoc
                                      } into g
                                      select new ThongTinChiTietTienDo
                                      {
                                          maGV = g.Key.MaGV,
                                          tenGV = g.Key.TenGV,
                                          tenKhoa = g.Key.TenKhoa,
                                          tenDotPhanCong = g.Key.TenDotKeKhai,
                                          tenHocKy = g.Key.TenHocKy,
                                          tenNamHoc = g.Key.TenNamHoc,
                                          soLuongPhanCong = g.Count(),
                                          soLuongHoanThanh = g.Count(pc => pc.TrangThai == "Hoàn thành"),
                                          soLuongChuaHoanThanh = g.Count(pc => pc.TrangThai == "Chưa Hoàn Thành")
                                      }).Distinct().ToList();

                        if (result != null && result.Any())
                        {
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<ThongTinChiTietTienDo>();
        }

        public List<ThongTinChiTietTienDo> ThongKeTienDoTatCaKhoa(int maDotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKekhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKekhai != null)
                    {
                        var result = (from phanCong in context.PhanCongHocPhans
                                      join giangVien in context.GiangViens
                                      on phanCong.MaGV equals giangVien.MaGV
                                      join khoa in context.Khoas
                                      on giangVien.MaKhoa equals khoa.MaKhoa
                                      join dotkk in context.DotKeKhais
                                      on phanCong.MaDotKeKhai equals dotkk.MaDotKeKhai
                                      join namHoc in context.NamHocs
                                      on dotkk.MaNamHoc equals namHoc.Id
                                      join hocKy in context.HocKies
                                      on dotkk.MaHocKy equals hocKy.MaHocKy
                                      where dotkk.MaDotKeKhai == maDotKeKhai
                                      group phanCong by new
                                      {
                                          giangVien.MaGV,
                                          giangVien.TenGV,
                                          khoa.TenKhoa,
                                          dotkk.TenDotKeKhai,
                                          hocKy.TenHocKy,
                                          namHoc.TenNamHoc
                                      } into g
                                      select new ThongTinChiTietTienDo
                                      {
                                          maGV = g.Key.MaGV,
                                          tenGV = g.Key.TenGV,
                                          tenKhoa = g.Key.TenKhoa,
                                          tenDotPhanCong = g.Key.TenDotKeKhai,
                                          tenHocKy = g.Key.TenHocKy,
                                          tenNamHoc = g.Key.TenNamHoc,
                                          soLuongPhanCong = g.Count(),
                                          soLuongHoanThanh = g.Count(pc => pc.TrangThai == "Hoàn thành"),
                                          soLuongChuaHoanThanh = g.Count(pc => pc.TrangThai == "Chưa Hoàn Thành")
                                      }).Distinct().OrderBy(op => op.tenKhoa).ToList();

                        if (result != null && result.Any())
                        {
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<ThongTinChiTietTienDo>();
        }

        public  (List<GiangVienHoanThanhKeKhai> , int ) ThongKeGiangVienChuaHoanThanhKeKhaiTheoDot(int maDOtKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai =  context.DotKeKhais.Find(maDOtKeKhai);
                    if (dotKeKhai != null)
                    {
                        var query = from phanCong in context.PhanCongHocPhans
                                    join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                    join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                    where phanCong.MaDotKeKhai == maDOtKeKhai
                                    group phanCong by new { giangVien.MaGV, giangVien.TenGV, khoa.TenKhoa } into grouped
                                    select new
                                    {
                                        Key = grouped.Key,
                                        SoLuongChoDuyet = grouped.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                        SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                        TongSoLuong = grouped.Count(),
                                        SoLuongHoanThanh = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                    };

                        var queryFiltered = query.Where(g => g.SoLuongChuaHoanThanh > 0 || g.SoLuongChoDuyet > 0)
                                                 .Select(g => new GiangVienHoanThanhKeKhai
                                                 {
                                                     maGV = g.Key.MaGV,
                                                     tenGV = g.Key.TenGV,
                                                     tenKhoa = g.Key.TenKhoa,
                                                     tienDo = $"{g.SoLuongHoanThanh}/{g.TongSoLuong}"
                                                 });

                        var soLuong =  queryFiltered.Count();
                        var danhSach =  queryFiltered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                        if (danhSach.Any())
                        {
                            return (danhSach, soLuong);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<GiangVienHoanThanhKeKhai>(), 0);
        }

        public  List<GiangVienHoanThanhKeKhai> DanhSachGiangVienChuaHoanThanh(int maDOtKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai =  context.DotKeKhais.Find(maDOtKeKhai);
                    if (dotKeKhai != null)
                    {
                        var query = from phanCong in context.PhanCongHocPhans
                                    join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                    join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                    where phanCong.MaDotKeKhai == maDOtKeKhai
                                    group phanCong by new { giangVien.MaGV, giangVien.TenGV, khoa.TenKhoa } into grouped
                                    select new
                                    {
                                        Key = grouped.Key,
                                        SoLuongChoDuyet = grouped.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                        SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                        TongSoLuong = grouped.Count(),
                                        SoLuongHoanThanh = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                    };

                        var queryFiltered = query.Where(g => g.SoLuongChuaHoanThanh > 0 || g.SoLuongChoDuyet > 0)
                                                 .Select(g => new GiangVienHoanThanhKeKhai
                                                 {
                                                     maGV = g.Key.MaGV,
                                                     tenGV = g.Key.TenGV,
                                                     tenKhoa = g.Key.TenKhoa,
                                                     tienDo = $"{g.SoLuongHoanThanh}/{g.TongSoLuong}"
                                                 });

                        var danhSach =  queryFiltered.OrderBy(op => op.tenKhoa).ToList();
                        if (danhSach.Any())
                        {
                            return danhSach;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<GiangVienHoanThanhKeKhai>();
        }

        public (List<GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienChuaHoanThanhKeKhai(string chuoiTim, int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai =  context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        chuoiTim = chuoiTim.ToLower().Trim();
                        var query = from phanCong in context.PhanCongHocPhans
                                    join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                    join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                    where phanCong.MaDotKeKhai == maDotKeKhai
                                    group phanCong by new { giangVien.MaGV, giangVien.TenGV, khoa.TenKhoa } into grouped
                                    select new
                                    {
                                        Key = grouped.Key,
                                        SoLuongChoDuyet = grouped.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                        SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                        TongSoLuong = grouped.Count(),
                                        SoLuongHoanThanh = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                    };

                        var queryFiltered = query.Where(g => g.SoLuongChuaHoanThanh > 0 || g.SoLuongChoDuyet > 0)
                                                 .Select(g => new GiangVienHoanThanhKeKhai
                                                 {
                                                     maGV = g.Key.MaGV,
                                                     tenGV = g.Key.TenGV,
                                                     tenKhoa = g.Key.TenKhoa,
                                                     tienDo = $"{g.SoLuongHoanThanh}/{g.TongSoLuong}"
                                                 });

                        var chuoiTimKhongDau = RemoveDiacritics(chuoiTim).ToLower();
                        var danhSachAll =  queryFiltered.ToList();

                        var danhSach = danhSachAll
                            .Where(op => RemoveDiacritics(op.maGV).ToLower().Contains(chuoiTimKhongDau) ||
                                         RemoveDiacritics(op.tenGV).ToLower().Contains(chuoiTimKhongDau))
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        var soLuong = danhSach.Count;
                        if (danhSach.Any())
                        {
                            return (danhSach, soLuong);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<GiangVienHoanThanhKeKhai>(), 0);
        }

        public (List<GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienChuaHoanThanhKeKhaiTheoKhoa(string maKhoa, int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        var khoaTim = context.Khoas.Find(maKhoa);
                        if (khoaTim != null)
                        {
                            var query = from phanCong in context.PhanCongHocPhans
                                        join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                        join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                        where phanCong.MaDotKeKhai == maDotKeKhai
                                        group phanCong by new
                                        {
                                            giangVien.MaGV,
                                            giangVien.TenGV,
                                            khoa.MaKhoa,
                                            khoa.TenKhoa
                                        } into grouped
                                        select new
                                        {
                                            Key = grouped.Key,
                                            SoLuongChoDuyet = grouped.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                            SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                            TongSoLuong = grouped.Count(),
                                            SoLuongHoanThanh = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                        };

                            var queryFiltered = query.Where(g => (g.SoLuongChuaHoanThanh > 0 || g.SoLuongChoDuyet > 0) && g.Key.MaKhoa == khoaTim.MaKhoa)
                                                      .Select(g => new GiangVienHoanThanhKeKhai
                                                      {
                                                          maGV = g.Key.MaGV,
                                                          tenGV = g.Key.TenGV,
                                                          tenKhoa = g.Key.TenKhoa,
                                                          tienDo = $"{g.SoLuongHoanThanh}/{g.TongSoLuong}"
                                                      });

                            int soLuong = queryFiltered.Count();

                            var danhSach = queryFiltered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                            if (danhSach != null && danhSach.Any())
                            {
                                return (danhSach, soLuong);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<GiangVienHoanThanhKeKhai>(), 0);
        }

        public (List<GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienChuaHoanThanhKeKhaiTheoKhoaVaChuoiTim(string maKhoa, string chuoiTim, int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        var khoaTim = context.Khoas.Find(maKhoa);
                        if (khoaTim != null)
                        {
                            chuoiTim = chuoiTim.ToLower().Trim();
                            var query = from phanCong in context.PhanCongHocPhans
                                        join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                        join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                        where phanCong.MaDotKeKhai == maDotKeKhai
                                        group phanCong by new
                                        {
                                            giangVien.MaGV,
                                            giangVien.TenGV,
                                            khoa.MaKhoa,
                                            khoa.TenKhoa
                                        } into grouped
                                        select new
                                        {
                                            Key = grouped.Key,
                                            SoLuongChoDuyet = grouped.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                            SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                            TongSoLuong = grouped.Count(),
                                            SoLuongHoanThanh = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                        };

                            var queryFiltered = query.Where(g => (g.SoLuongChuaHoanThanh > 0 || g.SoLuongChoDuyet > 0) && g.Key.MaKhoa == khoaTim.MaKhoa)
                                                      .Select(g => new GiangVienHoanThanhKeKhai
                                                      {
                                                          maGV = g.Key.MaGV,
                                                          tenGV = g.Key.TenGV,
                                                          tenKhoa = g.Key.TenKhoa,
                                                          tienDo = $"{g.SoLuongHoanThanh}/{g.TongSoLuong}"
                                                      });

                            var chuoiTimKhongDau = RemoveDiacritics(chuoiTim).ToLower();
                            var danhSachAll = queryFiltered.ToList();

                            var danhSach = danhSachAll
                                .Where(op => RemoveDiacritics(op.maGV).ToLower().Contains(chuoiTimKhongDau) ||
                                             RemoveDiacritics(op.tenGV).ToLower().Contains(chuoiTimKhongDau))
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

                            int soLuong = danhSach.Count;

                            if (danhSach != null && danhSach.Any())
                            {
                                return (danhSach, soLuong);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<GiangVienHoanThanhKeKhai>(), 0);
        }

        public List<GiangVienHoanThanhKeKhai> DanhSachGiangVienHoanThanh(int maDOtKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDOtKeKhai);
                    if (dotKeKhai != null)
                    {
                        var query = from phanCong in context.PhanCongHocPhans
                                    join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                    join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                    where phanCong.MaDotKeKhai == maDOtKeKhai
                                    group phanCong by new
                                    {
                                        giangVien.MaGV,
                                        giangVien.TenGV,
                                        khoa.TenKhoa
                                    } into grouped
                                    select new
                                    {
                                        Key = grouped.Key,
                                        SoLuongChoDuyet = grouped.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                        SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                        TongSoLuong = grouped.Count(),
                                        SoLuongHoanThanh = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                    };

                        var queryFiltered = query.Where(g => g.SoLuongChuaHoanThanh == 0 && g.SoLuongChoDuyet == 0)
                                                 .Select(g => new GiangVienHoanThanhKeKhai
                                                 {
                                                     maGV = g.Key.MaGV,
                                                     tenGV = g.Key.TenGV,
                                                     tenKhoa = g.Key.TenKhoa,
                                                     tienDo = $"{g.SoLuongHoanThanh}/{g.TongSoLuong}"
                                                 });

                        var danhSach = queryFiltered.OrderBy(op => op.tenKhoa).ToList();

                        if (danhSach.Any())
                        {
                            return danhSach;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<GiangVienHoanThanhKeKhai>();
        }

        public (List<GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienHoanThanhKeKhaiTheoKhoa(string maKhoa, int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        var khoaTim = context.Khoas.Find(maKhoa);
                        if (khoaTim != null)
                        {
                            var query = from phanCong in context.PhanCongHocPhans
                                        join giangVien in context.GiangViens
                                        on phanCong.MaGV equals giangVien.MaGV
                                        join khoa in context.Khoas
                                        on giangVien.MaKhoa equals khoa.MaKhoa
                                        where phanCong.MaDotKeKhai == maDotKeKhai
                                        group phanCong by new
                                        {
                                            giangVien.MaGV,
                                            giangVien.TenGV,
                                            khoa.MaKhoa,
                                            khoa.TenKhoa
                                        } into grouped
                                        select new
                                        {
                                            Key = grouped.Key,
                                            SoLuongChoDuyet = grouped.Count(pc => pc.TrangThai == "Chờ duyệt"),
                                            SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                            TongSoLuong = grouped.Count(),
                                            SoLuongHoanThanh = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                        };

                            var queryFiltered = query.Where(g => g.SoLuongChuaHoanThanh == 0 && g.SoLuongChoDuyet == 0 && g.Key.MaKhoa == khoaTim.MaKhoa)
                                                     .Select(g => new GiangVienHoanThanhKeKhai
                                                     {
                                                         maGV = g.Key.MaGV,
                                                         tenGV = g.Key.TenGV,
                                                         tenKhoa = g.Key.TenKhoa,
                                                         tienDo = $"{g.SoLuongHoanThanh}/{g.TongSoLuong}"
                                                     });

                            int soLuong = queryFiltered.Count();
                            var danhSach = queryFiltered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                            if (danhSach != null && danhSach.Any())
                            {
                                return (danhSach, soLuong);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<GiangVienHoanThanhKeKhai>(), 0);
        }

        public (List<GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienHoanThanhKeKhaiTheoKhoaVaChuoiTim(string maKhoa, string chuoiTim, int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        var khoaTim = context.Khoas.Find(maKhoa);
                        if (khoaTim != null)
                        {
                            chuoiTim = chuoiTim.ToLower().Trim();
                            var query = from phanCong in context.PhanCongHocPhans
                                        join giangVien in context.GiangViens
                                        on phanCong.MaGV equals giangVien.MaGV
                                        join khoa in context.Khoas
                                        on giangVien.MaKhoa equals khoa.MaKhoa
                                        where phanCong.MaDotKeKhai == maDotKeKhai
                                        group phanCong by new
                                        {
                                            giangVien.MaGV,
                                            giangVien.TenGV,
                                            khoa.MaKhoa,
                                            khoa.TenKhoa
                                        } into grouped
                                        select new
                                        {
                                            Key = grouped.Key,
                                            SoLuongChoDuyet = grouped.Count(op => op.TrangThai == "Chờ duyệt"),
                                            SoLuongChuaHoanThanh = grouped.Count(pc => pc.TrangThai == "Chưa Hoàn Thành"),
                                            TongSoLuong = grouped.Count(),
                                            SoLuongHoanThanh = grouped.Count(pc => pc.TrangThai == "Hoàn thành")
                                        };

                            var queryFiltered = query.Where(g => g.SoLuongChuaHoanThanh == 0 && g.SoLuongChoDuyet == 0 && g.Key.MaKhoa == khoaTim.MaKhoa)
                                                     .Select(g => new GiangVienHoanThanhKeKhai
                                                     {
                                                         maGV = g.Key.MaGV,
                                                         tenGV = g.Key.TenGV,
                                                         tenKhoa = g.Key.TenKhoa,
                                                         tienDo = $"{g.SoLuongHoanThanh}/{g.TongSoLuong}"
                                                     });

                            var chuoiTimKhongDau = RemoveDiacritics(chuoiTim).ToLower();
                            var danhSachAll = queryFiltered.ToList(); // Lấy toàn bộ danh sách từ SQL Server

                            var danhSach = danhSachAll
                                .Where(op => RemoveDiacritics(op.maGV).ToLower().Contains(chuoiTimKhongDau) ||
                                             RemoveDiacritics(op.tenGV).ToLower().Contains(chuoiTimKhongDau))
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

                            int soLuong = danhSach.Count;
                            if (danhSach != null && danhSach.Any())
                            {
                                return (danhSach, soLuong);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<GiangVienHoanThanhKeKhai>(), 0);
        }
    }
}
