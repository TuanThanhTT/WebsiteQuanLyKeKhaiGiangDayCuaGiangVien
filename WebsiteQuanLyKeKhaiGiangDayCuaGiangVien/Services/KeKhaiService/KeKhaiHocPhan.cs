
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KeKhaiService
{
    public class KeKhaiHocPhan : IKeKhaiHocPhan
    {
        public bool CapNhatThongTinHocPhanPhanCong(int maHocPhanPhanCong, string tenLop, int soLuong, string hinhThucDay)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var hocPhanPhanCong = context.PhanCongHocPhans.Find(maHocPhanPhanCong);
                    if (hocPhanPhanCong != null)
                    {
                        hocPhanPhanCong.LopPhanCong = tenLop;
                        hocPhanPhanCong.SiSo = soLuong;
                        hocPhanPhanCong.HinhThucDay = hinhThucDay;
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

        public bool DuyetDanhSachKeKhaiHoanThanh(List<int> dsMaKeKhai, string idNguoiDuyet)
        {
            if (string.IsNullOrEmpty(idNguoiDuyet) || dsMaKeKhai == null || dsMaKeKhai.Count == 0)
            {
                return false;
            }

            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var keKhaiList = context.KeKhais
                        .Where(k => dsMaKeKhai.Contains(k.MaKeKhai))
                        .ToList();

                    if (keKhaiList.Count != dsMaKeKhai.Count)
                        return false;

                    var maPhanCongIds = keKhaiList.Select(k => k.MaPhanCongHocPhan).Distinct().ToList();
                    var hocPhanPhanCongList = context.PhanCongHocPhans
                        .Where(p => maPhanCongIds.Contains(p.MaPhanCongHocPhan))
                        .ToList();

                    foreach (var keKhai in keKhaiList)
                    {
                        var hocPhanPhanCong = hocPhanPhanCongList.FirstOrDefault(p => p.MaPhanCongHocPhan == keKhai.MaPhanCongHocPhan);
                        if (hocPhanPhanCong == null)
                            return false;

                        keKhai.NgayDuyetKeKhai = DateTime.Now;
                        keKhai.idNguoiDuyet = idNguoiDuyet;
                        hocPhanPhanCong.TrangThai = "Hoàn thành";
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DuyetKeKhaiHoanThanh(int maKeKhai, string idNguoiDuyet)
        {
            try
            {
                if (string.IsNullOrEmpty(idNguoiDuyet)) return false;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var keKhai = context.KeKhais.Find(maKeKhai);
                    if (keKhai != null)
                    {
                        keKhai.idNguoiDuyet = idNguoiDuyet;
                        keKhai.NgayDuyetKeKhai = DateTime.Now;

                        var hocPhanPhanCong = context.PhanCongHocPhans.Find(keKhai.MaPhanCongHocPhan);
                        if (hocPhanPhanCong != null)
                        {
                            hocPhanPhanCong.TrangThai = "Hoàn thành";
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

        public (List<XemThongTinKeKhaiDaDuyet>, string) getAllHocPhanDaDuyetTheoDot(string maGV, int maDotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from pc in context.PhanCongHocPhans
                                join hp in context.HocPhans on pc.MaHP equals hp.MaHP
                                join gv in context.GiangViens on pc.MaGV equals gv.MaGV
                                join keKhai in context.KeKhais on pc.MaPhanCongHocPhan equals keKhai.MaPhanCongHocPhan
                                join dotKeKhai in context.DotKeKhais on keKhai.MaDotKeKhai equals dotKeKhai.MaDotKeKhai
                                join nguoiDuyet in context.GiangViens on keKhai.idNguoiDuyet equals nguoiDuyet.MaGV
                                where gv.MaGV == maGV && dotKeKhai.MaDotKeKhai == maDotKeKhai
                                      && pc.TrangThai == "Hoàn Thành" && keKhai.idNguoiDuyet != null
                                select new XemThongTinKeKhaiDaDuyet
                                {
                                    Id = keKhai.MaKeKhai,
                                    hinhThucDay = pc.HinhThucDay,
                                    hocKy = pc.HocKy,
                                    maHP = hp.MaHP,
                                    namHoc = pc.NamHoc,
                                    maPhanCongHocPhan = pc.MaPhanCongHocPhan,
                                    ngayDay = pc.ThoiGianDay,
                                    ngayDuyet = (DateTime)keKhai.NgayDuyetKeKhai,
                                    ngayKeKhai = keKhai.NgayKeKhai.ToString("dd/MM/yyyy"),
                                    nguoiDuyet = nguoiDuyet.TenGV,
                                    soLuong = (int)pc.SiSo,
                                    tenHocPhan = hp.TenHP,
                                    tenLop = pc.LopPhanCong,
                                };

                    var dsKeKhaiDaDuyet = query.Distinct().ToList();
                    var dotkekhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotkekhai != null && dsKeKhaiDaDuyet != null)
                    {
                        return (dsKeKhaiDaDuyet, dotkekhai.TenDotKeKhai);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<XemThongTinKeKhaiDaDuyet>(), "");
        }

        public (List<XemThongTinKeKhaiDaDuyet>, string) getAllHocPhanDaDuyetTheoDotGanNhat(string maGV)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    // Lấy ra mã đợt kê khai gần nhất
                    int maDotKeKhai = context.DotKeKhais.Count();

                    var query = from pc in context.PhanCongHocPhans
                                join hp in context.HocPhans on pc.MaHP equals hp.MaHP
                                join gv in context.GiangViens on pc.MaGV equals gv.MaGV
                                join keKhai in context.KeKhais on pc.MaPhanCongHocPhan equals keKhai.MaPhanCongHocPhan
                                join dotKeKhai in context.DotKeKhais on keKhai.MaDotKeKhai equals dotKeKhai.MaDotKeKhai
                                join nguoiDuyet in context.GiangViens on keKhai.idNguoiDuyet equals nguoiDuyet.MaGV
                                where gv.MaGV == maGV && dotKeKhai.MaDotKeKhai == maDotKeKhai
                                      && pc.TrangThai == "Hoàn Thành" && keKhai.idNguoiDuyet != null
                                select new XemThongTinKeKhaiDaDuyet
                                {
                                    Id = keKhai.MaKeKhai,
                                    hinhThucDay = pc.HinhThucDay,
                                    hocKy = pc.HocKy,
                                    maHP = hp.MaHP,
                                    namHoc = pc.NamHoc,
                                    maPhanCongHocPhan = pc.MaPhanCongHocPhan,
                                    ngayDay = pc.ThoiGianDay,
                                    ngayDuyet = (DateTime)keKhai.NgayDuyetKeKhai,
                                    ngayKeKhai = keKhai.NgayKeKhai.ToString("dd/MM/yyyy"),
                                    nguoiDuyet = nguoiDuyet.TenGV,
                                    soLuong = (int)pc.SiSo,
                                    tenHocPhan = hp.TenHP,
                                    tenLop = pc.LopPhanCong,
                                };

                    var dsKeKhaiDaDuyet = query.Distinct().ToList();
                    var dotkekhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotkekhai != null && dsKeKhaiDaDuyet.Count > 0)
                    {
                        return (dsKeKhaiDaDuyet, dotkekhai.TenDotKeKhai);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<XemThongTinKeKhaiDaDuyet>(), "");
        }

        public List<ChiTietKeKhaiChoDuyet> getDanhSachKeKhaiChoDuyetCuaGiangVien(string maGV)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from phanCong in context.PhanCongHocPhans
                                join gv in context.GiangViens on phanCong.MaGV equals gv.MaGV
                                join kekhai in context.KeKhais on phanCong.MaPhanCongHocPhan equals kekhai.MaPhanCongHocPhan
                                join hp in context.HocPhans on phanCong.MaHP equals hp.MaHP
                                where gv.MaGV == maGV
                                && phanCong.TrangThai == "Chờ duyệt"
                                && kekhai.idNguoiDuyet == null
                                select new ChiTietKeKhaiChoDuyet
                                {
                                    maKeKhai = kekhai.MaKeKhai,
                                    hinhThucDay = phanCong.HinhThucDay,
                                    hocKy = phanCong.HocKy,
                                    maGV = gv.MaGV,
                                    maHocPhanPhanCong = phanCong.MaPhanCongHocPhan,
                                    maHP = hp.MaHP,
                                    namHoc = phanCong.NamHoc,
                                    siSo = (int)phanCong.SiSo,
                                    tenGV = gv.TenGV,
                                    tenHP = hp.TenHP,
                                    tenLop = phanCong.LopPhanCong,
                                    thoiGianDay = phanCong.ThoiGianDay,
                                    trangThai = phanCong.TrangThai
                                };

                    return query.Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<ChiTietKeKhaiChoDuyet>();
        }

        public List<WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom.Khoa> getDanhSachKhoa()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    return context.Khoas.Select(op => new Models.ModelCustom.Khoa
                    {
                        MaKhoa = op.MaKhoa,
                        TenKhoa = op.TenKhoa
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom.Khoa>();
        }


        public (List<XemThongTinKeKhaiDaDuyet> , int) getHocPhanDaDuyetTheoDot(string maGV, int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from pc in context.PhanCongHocPhans
                                join hp in context.HocPhans on pc.MaHP equals hp.MaHP
                                join gv in context.GiangViens on pc.MaGV equals gv.MaGV
                                join keKhai in context.KeKhais on pc.MaPhanCongHocPhan equals keKhai.MaPhanCongHocPhan
                                join dotKeKhai in context.DotKeKhais on keKhai.MaDotKeKhai equals dotKeKhai.MaDotKeKhai
                                join nguoiDuyet in context.GiangViens on keKhai.idNguoiDuyet equals nguoiDuyet.MaGV
                                where gv.MaGV == maGV && dotKeKhai.MaDotKeKhai == maDotKeKhai
                                      && pc.TrangThai == "Hoàn Thành" && keKhai.idNguoiDuyet != null
                                select new XemThongTinKeKhaiDaDuyet
                                {
                                    Id = keKhai.MaKeKhai,
                                    hinhThucDay = pc.HinhThucDay,
                                    hocKy = pc.HocKy,
                                    maHP = hp.MaHP,
                                    namHoc = pc.NamHoc,
                                    maPhanCongHocPhan = pc.MaPhanCongHocPhan,
                                    ngayDay = pc.ThoiGianDay,
                                    ngayDuyet = (DateTime)keKhai.NgayDuyetKeKhai,
                                    ngayKeKhai = keKhai.NgayKeKhai.ToString("dd/MM/yyyy"),
                                    nguoiDuyet = nguoiDuyet.TenGV,
                                    soLuong = (int)pc.SiSo,
                                    tenHocPhan = hp.TenHP,
                                    tenLop = pc.LopPhanCong,
                                };

                    var dsKeKhaiDaDuyet = query.Skip((page - 1) * pageSize).Take(pageSize).Distinct().ToList();
                    var soLuongKeKhai = query.Count();
                    return (dsKeKhaiDaDuyet, soLuongKeKhai);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<XemThongTinKeKhaiDaDuyet>(), 0);
        }

        public (List<XemThongTinKeKhaiDaDuyet> , int , ThongTinDotKeKhaiGanNhat ) getHocPhanDaDuyetTheoDotGanNhat(string maGV, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var maDotKeKhai = context.DotKeKhais.Count();
                    var thongTin = (from dotKeKhai in context.DotKeKhais
                                    join namHoc in context.NamHocs on dotKeKhai.MaNamHoc equals namHoc.Id
                                    join hocKy in context.HocKies on dotKeKhai.MaHocKy equals hocKy.MaHocKy
                                    where dotKeKhai.MaDotKeKhai == maDotKeKhai
                                    select new ThongTinDotKeKhaiGanNhat
                                    {
                                        maDotKeKhai = dotKeKhai.MaDotKeKhai,
                                        maNamHoc = namHoc.Id,
                                        maHocKy = hocKy.MaHocKy
                                    }).FirstOrDefault();

                    var query = from pc in context.PhanCongHocPhans
                                join hp in context.HocPhans on pc.MaHP equals hp.MaHP
                                join gv in context.GiangViens on pc.MaGV equals gv.MaGV
                                join keKhai in context.KeKhais on pc.MaPhanCongHocPhan equals keKhai.MaPhanCongHocPhan
                                join dotKeKhai in context.DotKeKhais on keKhai.MaDotKeKhai equals dotKeKhai.MaDotKeKhai
                                join nguoiDuyet in context.GiangViens on keKhai.idNguoiDuyet equals nguoiDuyet.MaGV
                                where gv.MaGV == maGV && dotKeKhai.MaDotKeKhai == maDotKeKhai
                                      && pc.TrangThai == "Hoàn Thành" && keKhai.idNguoiDuyet != null
                                select new XemThongTinKeKhaiDaDuyet
                                {
                                    Id = keKhai.MaKeKhai,
                                    hinhThucDay = pc.HinhThucDay,
                                    hocKy = pc.HocKy,
                                    maHP = hp.MaHP,
                                    namHoc = pc.NamHoc,
                                    maPhanCongHocPhan = pc.MaPhanCongHocPhan,
                                    ngayDay = pc.ThoiGianDay,
                                    ngayDuyet = (DateTime)keKhai.NgayDuyetKeKhai,
                                    ngayKeKhai = keKhai.NgayKeKhai.ToString("dd/MM/yyyy"),
                                    nguoiDuyet = nguoiDuyet.TenGV,
                                    soLuong = (int)pc.SiSo,
                                    tenHocPhan = hp.TenHP,
                                    tenLop = pc.LopPhanCong,
                                };

                    var dsKeKhaiDaDuyet = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    var soLuongKeKhai = query.Count();
                    return (dsKeKhaiDaDuyet, soLuongKeKhai, thongTin);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<XemThongTinKeKhaiDaDuyet>(), 0, new ThongTinDotKeKhaiGanNhat());
        }

        public (List<XemThongTinHocPhanDuocPhanCong> , int ) getHocPhanDaKeKhaiChoDuyet(string maGV, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from pc in context.PhanCongHocPhans
                                join hp in context.HocPhans on pc.MaHP equals hp.MaHP
                                where pc.MaGV == maGV && pc.TrangThai == "Chờ duyệt"
                                select new XemThongTinHocPhanDuocPhanCong
                                {
                                    Id = pc.MaPhanCongHocPhan,
                                    hinhThucDay = pc.HinhThucDay,
                                    hocKy = pc.HocKy,
                                    namHoc = pc.NamHoc,
                                    maHocPhanPhanCong = hp.MaHP,
                                    ngayDay = pc.ThoiGianDay,
                                    siSo = (int)pc.SiSo,
                                    tenHocPhanPhanCong = hp.TenHP,
                                    tenLop = pc.LopPhanCong,
                                };

                    var tongSoLuong = query.Count();
                    var danhSachDaKeKhai = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return (danhSachDaKeKhai, tongSoLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<XemThongTinHocPhanDuocPhanCong>(), 0);
        }

        public (List<XemThongTinHocPhanDuocPhanCong> , int ) getHocPhanDuocPhanCongChuaHoanThanhCuaGiangVien(string maGV, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from pc in context.PhanCongHocPhans
                                join hp in context.HocPhans on pc.MaHP equals hp.MaHP
                                join gv in context.GiangViens on pc.MaGV equals gv.MaGV
                                where gv.MaGV == maGV && pc.TrangThai == "Chưa Hoàn Thành"
                                select new XemThongTinHocPhanDuocPhanCong
                                {
                                    Id = pc.MaPhanCongHocPhan,
                                    hinhThucDay = pc.HinhThucDay,
                                    hocKy = pc.HocKy,
                                    namHoc = pc.NamHoc,
                                    maHocPhanPhanCong = hp.MaHP,
                                    ngayDay = pc.ThoiGianDay,
                                    siSo = (int)pc.SiSo,
                                    tenHocPhanPhanCong = hp.TenHP,
                                    tenLop = pc.LopPhanCong,
                                };

                    int tongSoLuong = query.Count();
                    var dsHocPhanChuaHoanThanh = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return (dsHocPhanChuaHoanThanh, tongSoLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<XemThongTinHocPhanDuocPhanCong>(), 0);
        }

        public (List<XemKeKhaiChoDuyetTheoGiangVien> , int) getKeKhaiGiangVien(int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from kekhai in context.KeKhais
                                join gv in context.GiangViens on kekhai.MaGV equals gv.MaGV
                                join khoa in context.Khoas on gv.MaKhoa equals khoa.MaKhoa
                                where kekhai.idNguoiDuyet == null
                                group new { kekhai, khoa, gv } by gv.MaGV into grouped
                                select new XemKeKhaiChoDuyetTheoGiangVien
                                {
                                    maGV = grouped.Key,
                                    tenGV = grouped.Select(g => g.gv.TenGV).Distinct().FirstOrDefault(),
                                    maKhoa = grouped.Select(g => g.khoa.MaKhoa).Distinct().FirstOrDefault(),
                                    tenKhoa = grouped.Select(g => g.khoa.TenKhoa).Distinct().FirstOrDefault(),
                                    soLuong = grouped.Count()
                                };

                    int tongSoLuong = query.Count();
                    var dsGiangVienChoDuyet = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return (dsGiangVienChoDuyet, tongSoLuong);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return (new List<XemKeKhaiChoDuyetTheoGiangVien>(), 0);
        }

        public (List<XemKeKhaiChoDuyetTheoGiangVien> , int ) getKeKhaiGiangVienTheoDot(int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from kekhai in context.KeKhais
                                join gv in context.GiangViens on kekhai.MaGV equals gv.MaGV
                                join khoa in context.Khoas on gv.MaKhoa equals khoa.MaKhoa
                                where kekhai.idNguoiDuyet == null && kekhai.MaDotKeKhai == maDotKeKhai
                                group new { kekhai, khoa, gv } by gv.MaGV into grouped
                                select new XemKeKhaiChoDuyetTheoGiangVien
                                {
                                    maGV = grouped.Key,
                                    tenGV = grouped.Select(g => g.gv.TenGV).Distinct().FirstOrDefault(),
                                    maKhoa = grouped.Select(g => g.khoa.MaKhoa).Distinct().FirstOrDefault(),
                                    tenKhoa = grouped.Select(g => g.khoa.TenKhoa).Distinct().FirstOrDefault(),
                                    soLuong = grouped.Count()
                                };

                    int tongSoLuong = query.Count();
                    var dsGiangVienChoDuyet = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return (dsGiangVienChoDuyet, tongSoLuong);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return (new List<XemKeKhaiChoDuyetTheoGiangVien>(), 0);
        }

        public (List<XemKeKhaiChoDuyetTheoGiangVien> , int ) getKeKhaiGiangVienTheoDotVaKhoa(string maKhoa, int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from kekhai in context.KeKhais
                                join gv in context.GiangViens on kekhai.MaGV equals gv.MaGV
                                join khoa in context.Khoas on gv.MaKhoa equals khoa.MaKhoa
                                where kekhai.idNguoiDuyet == null && kekhai.MaDotKeKhai == maDotKeKhai && khoa.MaKhoa == maKhoa
                                group new { kekhai, khoa, gv } by gv.MaGV into grouped
                                select new XemKeKhaiChoDuyetTheoGiangVien
                                {
                                    maGV = grouped.Key,
                                    tenGV = grouped.Select(g => g.gv.TenGV).Distinct().FirstOrDefault(),
                                    maKhoa = grouped.Select(g => g.khoa.MaKhoa).Distinct().FirstOrDefault(),
                                    tenKhoa = grouped.Select(g => g.khoa.TenKhoa).Distinct().FirstOrDefault(),
                                    soLuong = grouped.Count()
                                };

                    int tongSoLuong = query.Count();
                    var dsGiangVienChoDuyet = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return (dsGiangVienChoDuyet, tongSoLuong);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return (new List<XemKeKhaiChoDuyetTheoGiangVien>(), 0);
        }

        public (List<XemKeKhaiChoDuyetTheoGiangVien> , int ) getKeKhaiGiangVienTheoKhoa(string maKhoa, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from kekhai in context.KeKhais
                                join gv in context.GiangViens on kekhai.MaGV equals gv.MaGV
                                join khoa in context.Khoas on gv.MaKhoa equals khoa.MaKhoa
                                where kekhai.idNguoiDuyet == null && khoa.MaKhoa == maKhoa
                                group new { kekhai, khoa, gv } by gv.MaGV into grouped
                                select new XemKeKhaiChoDuyetTheoGiangVien
                                {
                                    maGV = grouped.Key,
                                    tenGV = grouped.Select(g => g.gv.TenGV).Distinct().FirstOrDefault(),
                                    maKhoa = grouped.Select(g => g.khoa.MaKhoa).Distinct().FirstOrDefault(),
                                    tenKhoa = grouped.Select(g => g.khoa.TenKhoa).Distinct().FirstOrDefault(),
                                    soLuong = grouped.Count()
                                };

                    int tongSoLuong = query.Count();
                    var dsGiangVienChoDuyet = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return (dsGiangVienChoDuyet, tongSoLuong);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return (new List<XemKeKhaiChoDuyetTheoGiangVien>(), 0);
        }

        public XemThongTinKeKhaiDaDuyet getThongTinKeKhaiDaDuyetTheoMa(int maKeKhai, string maGV)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from pc in context.PhanCongHocPhans
                                join hp in context.HocPhans on pc.MaHP equals hp.MaHP
                                join gv in context.GiangViens on pc.MaGV equals gv.MaGV
                                join keKhai in context.KeKhais on pc.MaPhanCongHocPhan equals keKhai.MaPhanCongHocPhan
                                join dotKeKhai in context.DotKeKhais on keKhai.MaDotKeKhai equals dotKeKhai.MaDotKeKhai
                                join nguoiDuyet in context.GiangViens on keKhai.idNguoiDuyet equals nguoiDuyet.MaGV
                                where gv.MaGV == maGV
                                && pc.TrangThai == "Hoàn Thành"
                                && keKhai.idNguoiDuyet != null
                                && keKhai.MaKeKhai == maKeKhai
                                select new XemThongTinKeKhaiDaDuyet
                                {
                                    Id = keKhai.MaKeKhai,
                                    hinhThucDay = pc.HinhThucDay,
                                    hocKy = pc.HocKy,
                                    maHP = hp.MaHP,
                                    namHoc = pc.NamHoc,
                                    maPhanCongHocPhan = pc.MaPhanCongHocPhan,
                                    ngayDay = pc.ThoiGianDay,
                                    ngayDuyet = (DateTime)keKhai.NgayDuyetKeKhai,
                                    ngayKeKhai = keKhai.NgayKeKhai.ToString("dd/MM/yyyy"),
                                    nguoiDuyet = nguoiDuyet.TenGV,
                                    soLuong = (int)pc.SiSo,
                                    tenHocPhan = hp.TenHP,
                                    tenLop = pc.LopPhanCong,
                                };

                    return query.FirstOrDefault() ?? new XemThongTinKeKhaiDaDuyet();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new XemThongTinKeKhaiDaDuyet();
        }
        public bool HoanThanhKeKhai(int maPhanCongHocPhan, string maGV)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var hocPhanKeKhai = context.PhanCongHocPhans.Find(maPhanCongHocPhan);
                    if (hocPhanKeKhai != null)
                    {
                        // Tạo kê khai
                        int idKeKhai = context.KeKhais.Count() + 1;
                        while (context.KeKhais.Find(idKeKhai) != null)
                        {
                            idKeKhai++;
                        }

                        // Kiểm tra xem có đợt kê khai đang mở không
                        DotKeKhaiService dotKeKhaiService = new DotKeKhaiService();
                        if (dotKeKhaiService.KiemTraDotKeKhaiDangMo())
                        {
                            var dotKeKhai = dotKeKhaiService.GetDotKeKhaiDangMo();
                            Console.WriteLine("Mã đợt kê khai hiện tại là: " + dotKeKhai.MaDotKeKhai);
                            var keKhai = new KeKhai
                            {
                                MaKeKhai = idKeKhai,
                                MaGV = maGV,
                                MaDotKeKhai = dotKeKhai.MaDotKeKhai,
                                NgayKeKhai = DateTime.Now,
                                MaPhanCongHocPhan = hocPhanKeKhai.MaPhanCongHocPhan,
                            };
                            context.KeKhais.Add(keKhai);

                            // Cập nhật trạng thái
                            hocPhanKeKhai.TrangThai = "Chờ duyệt";
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

        public XemThongTinHocPhanDuocPhanCong XemThongTinPhanCongHocPhanTheoMa(int maPhanCongHocPhan)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var hocPhanChuaHoanThanh = (from pc in context.PhanCongHocPhans
                                                join hp in context.HocPhans on pc.MaHP equals hp.MaHP
                                                join gv in context.GiangViens on pc.MaGV equals gv.MaGV
                                                where pc.TrangThai != "Đã duyệt" && pc.MaPhanCongHocPhan == maPhanCongHocPhan
                                                select new XemThongTinHocPhanDuocPhanCong
                                                {
                                                    Id = pc.MaPhanCongHocPhan,
                                                    hinhThucDay = pc.HinhThucDay,
                                                    hocKy = pc.HocKy,
                                                    namHoc = pc.NamHoc,
                                                    maHocPhanPhanCong = hp.MaHP,
                                                    ngayDay = pc.ThoiGianDay,
                                                    siSo = (int)pc.SiSo,
                                                    tenHocPhanPhanCong = hp.TenHP,
                                                    tenLop = pc.LopPhanCong,
                                                }).FirstOrDefault();

                    if (hocPhanChuaHoanThanh != null)
                    {
                        return hocPhanChuaHoanThanh;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new XemThongTinHocPhanDuocPhanCong();
        }

        public (List<ThongTinPhanCongGiangVien> , int , int , string , string , string) getHocPhanPhanCongTheoDot(int maDotKeKhai, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    string tenDotKeKhai = "";
                    string tenNamHoc = "";
                    string tenHocKy = "";

                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        var namHoc = context.NamHocs.Find(dotKeKhai.MaNamHoc);
                        var hocKy = context.HocKies.Find(dotKeKhai.MaHocKy);
                        tenNamHoc = namHoc?.TenNamHoc ?? "";
                        tenHocKy = hocKy?.TenHocKy ?? "";
                        tenDotKeKhai = dotKeKhai.TenDotKeKhai;
                    }

                    var query = from phanCong in context.PhanCongHocPhans
                                join hocphan in context.HocPhans on phanCong.MaHP equals hocphan.MaHP
                                join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                where phanCong.MaDotKeKhai == maDotKeKhai
                                group new { giangVien, khoa } by new
                                {
                                    giangVien.MaGV,
                                    giangVien.TenGV,
                                    khoa.TenKhoa
                                } into grouped
                                select new ThongTinPhanCongGiangVien
                                {
                                    id = grouped.Key.MaGV,
                                    maGV = grouped.Key.MaGV,
                                    tenGV = grouped.Key.TenGV,
                                    khoa = grouped.Key.TenKhoa,
                                    soLuong = grouped.Count()
                                };

                    int soLuong = query.Count();
                    var dsSachPhanCong = query.Distinct().Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return (dsSachPhanCong, soLuong, dotKeKhai.MaDotKeKhai, tenDotKeKhai, tenNamHoc, tenHocKy);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<ThongTinPhanCongGiangVien>(), 0, 0, "", "", "");
        }

        public (List<ThongTinPhanCongGiangVien> , int , int , string, string, string, ThongTinDotKeKhaiGanNhat) getHocPhanPhanCongTheoDotGanNhat(int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    string tenDotKeKhai = "";
                    string tenNamHoc = "";
                    string tenHocKy = "";

                    var dotKeKhai = context.DotKeKhais.OrderByDescending(op => op.MaDotKeKhai).FirstOrDefault();
                    if (dotKeKhai != null)
                    {
                        var thongTin = (from dotkekhai in context.DotKeKhais
                                        join namhoc in context.NamHocs
                                        on dotkekhai.MaNamHoc equals namhoc.Id
                                        join hocky in context.HocKies
                                        on dotkekhai.MaHocKy equals hocky.MaHocKy
                                        where dotkekhai.MaDotKeKhai == dotKeKhai.MaDotKeKhai
                                        select new ThongTinDotKeKhaiGanNhat
                                        {
                                            maDotKeKhai = dotkekhai.MaDotKeKhai,
                                            maNamHoc = namhoc.Id,
                                            maHocKy = hocky.MaHocKy
                                        }).FirstOrDefault();

                        var namHoc = context.NamHocs.Find(dotKeKhai.MaNamHoc);
                        var hocKy = context.HocKies.Find(dotKeKhai.MaHocKy);
                        if (namHoc != null)
                        {
                            tenNamHoc = namHoc.TenNamHoc;
                        }
                        if (hocKy != null)
                        {
                            tenHocKy = hocKy.TenHocKy;
                        }
                        tenDotKeKhai = dotKeKhai.TenDotKeKhai;

                        var query = from phanCong in context.PhanCongHocPhans
                                    join hocphan in context.HocPhans
                                    on phanCong.MaHP equals hocphan.MaHP
                                    join giangVien in context.GiangViens
                                    on phanCong.MaGV equals giangVien.MaGV
                                    join khoa in context.Khoas
                                    on giangVien.MaKhoa equals khoa.MaKhoa
                                    where phanCong.MaDotKeKhai == dotKeKhai.MaDotKeKhai
                                    group new { giangVien, khoa } by new
                                    {
                                        giangVien.MaGV,
                                        giangVien.TenGV,
                                        khoa.TenKhoa
                                    } into grouped
                                    select new ThongTinPhanCongGiangVien
                                    {
                                        id = grouped.Key.MaGV,
                                        maGV = grouped.Key.MaGV,
                                        tenGV = grouped.Key.TenGV,
                                        khoa = grouped.Key.TenKhoa,
                                        soLuong = grouped.Count()
                                    };
                        var soLuong = query.Count();
                        var dsSachPhanCong = query.Distinct().Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        if (dsSachPhanCong != null)
                        {
                            return (dsSachPhanCong, soLuong, dotKeKhai.MaDotKeKhai, tenDotKeKhai, tenNamHoc, tenHocKy, thongTin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<ThongTinPhanCongGiangVien>(), 0, 0, "", "", "", new ThongTinDotKeKhaiGanNhat());
        }

        public (List<XemThongTinHocPhanDuocPhanCong> , string, string, string) getDanhSachHocPhanDuocPhanCongCuaGiangVienTheoDot(string maGV, int maDotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    string tenGV = "";
                    string tenKhoa = "";
                    var gv = context.GiangViens.Find(maGV);
                    if (gv != null)
                    {
                        tenGV = gv.TenGV;
                        var khoa = context.Khoas.Find(gv.MaKhoa);
                        if (khoa != null)
                        {
                            tenKhoa = khoa.TenKhoa;
                        }
                    }

                    var query = from phanCong in context.PhanCongHocPhans
                                join hocPhan in context.HocPhans
                                on phanCong.MaHP equals hocPhan.MaHP
                                join giangVien in context.GiangViens
                                on phanCong.MaGV equals giangVien.MaGV
                                where giangVien.MaGV == maGV && phanCong.MaDotKeKhai == maDotKeKhai
                                select new XemThongTinHocPhanDuocPhanCong
                                {
                                    Id = phanCong.MaPhanCongHocPhan,
                                    hinhThucDay = phanCong.HinhThucDay,
                                    hocKy = phanCong.HocKy,
                                    namHoc = phanCong.NamHoc,
                                    maHocPhanPhanCong = hocPhan.MaHP,
                                    ngayDay = phanCong.ThoiGianDay,
                                    siSo = (int)phanCong.SiSo,
                                    tenHocPhanPhanCong = hocPhan.TenHP,
                                    tenLop = phanCong.LopPhanCong
                                };
                    var dsSachPhanCong = query.Distinct().ToList();
                    if (dsSachPhanCong != null)
                    {
                        return (dsSachPhanCong, maGV, tenGV, tenKhoa);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<XemThongTinHocPhanDuocPhanCong>(), "", "", "");
        }
       
            public (List<ThongTinPhanCongGiangVien> , int , int , string , string , string )
                getHocPhanPhanCongTheoDotVaKhoaGanNhat(string maKhoa, int page, int pageSize)
            {
                try
                {
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        string tenDotKeKhai = "";
                        string tenNamHoc = "";
                        string tenHocKy = "";

                        var dotKeKhai = context.DotKeKhais.OrderByDescending(op => op.MaDotKeKhai).FirstOrDefault();
                        if (dotKeKhai != null)
                        {
                            var namHoc = context.NamHocs.Find(dotKeKhai.MaNamHoc);
                            var hocKy = context.HocKies.Find(dotKeKhai.MaHocKy);
                            if (namHoc != null) tenNamHoc = namHoc.TenNamHoc;
                            if (hocKy != null) tenHocKy = hocKy.TenHocKy;
                            tenDotKeKhai = dotKeKhai.TenDotKeKhai;

                            var query = from phanCong in context.PhanCongHocPhans
                                        join hocphan in context.HocPhans on phanCong.MaHP equals hocphan.MaHP
                                        join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                        join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                        where phanCong.MaDotKeKhai == dotKeKhai.MaDotKeKhai && khoa.MaKhoa == maKhoa
                                        group new { giangVien, khoa } by new { giangVien.MaGV, giangVien.TenGV, khoa.TenKhoa } into grouped
                                        select new ThongTinPhanCongGiangVien
                                        {
                                            id = grouped.Key.MaGV,
                                            maGV = grouped.Key.MaGV,
                                            tenGV = grouped.Key.TenGV,
                                            khoa = grouped.Key.TenKhoa,
                                            soLuong = grouped.Count()
                                        };

                            var soLuong = query.Count();
                            var dsSachPhanCong = query.Distinct().Skip((page - 1) * pageSize).Take(pageSize).ToList();
                            return (dsSachPhanCong, soLuong, dotKeKhai.MaDotKeKhai, tenDotKeKhai, tenNamHoc, tenHocKy);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return (new List<ThongTinPhanCongGiangVien>(), 0, 0, "", "", "");
            }

            public (List<ThongTinPhanCongGiangVien> , int , int , string , string , string )
                getHocPhanPhanCongTheoDotVaKhoa(string maKhoa, int maDotKeKhai, int page, int pageSize)
            {
                try
                {
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        string tenDotKeKhai = "";
                        string tenNamHoc = "";
                        string tenHocKy = "";

                        var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                        if (dotKeKhai != null)
                        {
                            var namHoc = context.NamHocs.Find(dotKeKhai.MaNamHoc);
                            var hocKy = context.HocKies.Find(dotKeKhai.MaHocKy);
                            if (namHoc != null) tenNamHoc = namHoc.TenNamHoc;
                            if (hocKy != null) tenHocKy = hocKy.TenHocKy;
                            tenDotKeKhai = dotKeKhai.TenDotKeKhai;
                        }

                        var query = from phanCong in context.PhanCongHocPhans
                                    join hocphan in context.HocPhans on phanCong.MaHP equals hocphan.MaHP
                                    join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                    join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                    where phanCong.MaDotKeKhai == maDotKeKhai && khoa.MaKhoa == maKhoa
                                    group new { giangVien, khoa } by new { giangVien.MaGV, giangVien.TenGV, khoa.TenKhoa } into grouped
                                    select new ThongTinPhanCongGiangVien
                                    {
                                        id = grouped.Key.MaGV,
                                        maGV = grouped.Key.MaGV,
                                        tenGV = grouped.Key.TenGV,
                                        khoa = grouped.Key.TenKhoa,
                                        soLuong = grouped.Count()
                                    };

                        var soLuong = query.Count();
                        var dsSachPhanCong = query.Distinct().Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        return (dsSachPhanCong, soLuong, maDotKeKhai, tenDotKeKhai, tenNamHoc, tenHocKy);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return (new List<ThongTinPhanCongGiangVien>(), 0, 0, "", "", "");
            }

        public (List<HocPhanDaPhanCong> , int ) GetDanhSachHocPhanDaPhanCongTheoDot(int maDotPhanCong, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotphancong = context.DotPhanCongs.Find(maDotPhanCong);
                    if (dotphancong != null)
                    {
                        var query = (from phanCong in context.PhanCongHocPhans
                                     join dotPhanCong in context.DotPhanCongs
                                     on phanCong.MaDotPhanCong equals dotPhanCong.Id
                                     join giangVien in context.GiangViens
                                     on phanCong.MaGV equals giangVien.MaGV
                                     join hocPhan in context.HocPhans
                                     on phanCong.MaHP equals hocPhan.MaHP
                                     where dotPhanCong.Id == dotphancong.Id
                                     select new HocPhanDaPhanCong
                                     {
                                         maGV = giangVien.MaGV,
                                         maHP = hocPhan.MaHP,
                                         tenGV = giangVien.TenGV,
                                         tenHP = hocPhan.TenHP,
                                         tenLop = phanCong.LopPhanCong,
                                         thoiGianDay = phanCong.ThoiGianDay,
                                         maPhanCongHocPhan = phanCong.MaPhanCongHocPhan
                                     }).Distinct();

                        int soLuong = query.Count();
                        var danhSach = query.OrderBy(op => op.maGV)
                                            .Skip((page - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToList();

                        return (danhSach, soLuong);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<HocPhanDaPhanCong>(), 0);
        }

        public XemHocPhanPhanCongCuaGiangVien ThongTinHocPhanPhanCongCuaGiangVienTheoMa(int maPhanCongHocPhan)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = (from phanCong in context.PhanCongHocPhans
                                 join giangVien in context.GiangViens
                                 on phanCong.MaGV equals giangVien.MaGV
                                 join hocPhan in context.HocPhans
                                 on phanCong.MaHP equals hocPhan.MaHP
                                 where phanCong.MaPhanCongHocPhan == maPhanCongHocPhan
                                 select new XemHocPhanPhanCongCuaGiangVien
                                 {
                                     maGV = giangVien.MaGV,
                                     tenGV = giangVien.TenGV,
                                     maHP = hocPhan.MaHP,
                                     tenHP = hocPhan.TenHP,
                                     thoiGianDay = phanCong.ThoiGianDay,
                                     namHoc = phanCong.NamHoc,
                                     hocKy = phanCong.HocKy,
                                     hinhThucDay = phanCong.HinhThucDay,
                                     siSo = (int)phanCong.SiSo,
                                     tenLop = phanCong.LopPhanCong,
                                     maKhoa = giangVien.MaKhoa,
                                     maPhanCongHocPhan = phanCong.MaPhanCongHocPhan
                                 }).FirstOrDefault();

                    if (query != null) return query;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new XemHocPhanPhanCongCuaGiangVien();
        }

        public (List<HocPhanDaPhanCong> , int ) GetDanhSachHocPhanDaPhanCongTheoKhoa(string maKhoa, int maDotPhanCong, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotphancong = context.DotPhanCongs.Find(maDotPhanCong);
                    if (dotphancong != null)
                    {
                        var query = (from phanCong in context.PhanCongHocPhans
                                     join dotPhanCong in context.DotPhanCongs
                                     on phanCong.MaDotPhanCong equals dotPhanCong.Id
                                     join giangVien in context.GiangViens
                                     on phanCong.MaGV equals giangVien.MaGV
                                     join hocPhan in context.HocPhans
                                     on phanCong.MaHP equals hocPhan.MaHP
                                     where dotPhanCong.Id == dotphancong.Id && giangVien.MaKhoa == maKhoa
                                     select new HocPhanDaPhanCong
                                     {
                                         maGV = giangVien.MaGV,
                                         maHP = hocPhan.MaHP,
                                         tenGV = giangVien.TenGV,
                                         tenHP = hocPhan.TenHP,
                                         tenLop = phanCong.LopPhanCong,
                                         thoiGianDay = phanCong.ThoiGianDay,
                                         maPhanCongHocPhan = phanCong.MaPhanCongHocPhan
                                     }).Distinct();

                        int soLuong = query.Count();
                        var danhSach = query.OrderBy(op => op.maGV)
                                            .Skip((page - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToList();

                        return (danhSach, soLuong);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<HocPhanDaPhanCong>(), 0);
        }

        public bool CapNhatPhanCongHocPhanTheoMa(int maPhanCongHocPhan, string maGV, string tenLop, string namHoc, string hocKy, string thoiGianDay, string maHP, string hinhThucDay, int siSo)
        {
            try
            {
                if (string.IsNullOrEmpty(maGV) || string.IsNullOrEmpty(tenLop) || string.IsNullOrEmpty(namHoc) ||
                    string.IsNullOrEmpty(hocKy) || string.IsNullOrEmpty(thoiGianDay) || string.IsNullOrEmpty(hinhThucDay))
                {
                    return false;
                }
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var phanCongHocPhan = context.PhanCongHocPhans.Find(maPhanCongHocPhan);
                    if (phanCongHocPhan != null)
                    {
                        phanCongHocPhan.MaGV = maGV;
                        phanCongHocPhan.LopPhanCong = tenLop;
                        phanCongHocPhan.NamHoc = namHoc;
                        phanCongHocPhan.HocKy = hocKy;
                        phanCongHocPhan.ThoiGianDay = thoiGianDay;
                        phanCongHocPhan.MaHP = maHP;
                        phanCongHocPhan.HinhThucDay = hinhThucDay;
                        phanCongHocPhan.SiSo = siSo;

                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public bool XoaDanhSachPhanCongHocPhan(List<int> danhSachMaPhanCong)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var danhSachXoa = context.PhanCongHocPhans
                                .Where(pc => danhSachMaPhanCong.Contains(pc.MaPhanCongHocPhan))
                                .ToList();

                            if (danhSachXoa.Any())
                            {
                                context.PhanCongHocPhans.RemoveRange(danhSachXoa);
                                context.SaveChanges();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Lỗi khi xóa: {ex.Message}");
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

        public (List<HocPhanDaPhanCong> , int ) GetDanhSachHocPhanPhanCongTheoChuoiTim(string chuoiTim, int maDotPhanCong, int page, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(chuoiTim)) return (new List<HocPhanDaPhanCong>(), 0);

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotPhanCongs.Find(maDotPhanCong);
                    if (dotKeKhai == null) return (new List<HocPhanDaPhanCong>(), 0);

                    var query = from phanCong in context.PhanCongHocPhans
                                join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                join hocPhan in context.HocPhans on phanCong.MaHP equals hocPhan.MaHP
                                join dotphancong in context.DotPhanCongs on phanCong.MaDotPhanCong equals dotphancong.Id
                                where phanCong.MaDotPhanCong == maDotPhanCong
                                select new HocPhanDaPhanCong
                                {
                                    maGV = giangVien.MaGV,
                                    tenGV = giangVien.TenGV,
                                    maHP = hocPhan.MaHP,
                                    tenHP = hocPhan.TenHP,
                                    tenLop = phanCong.LopPhanCong,
                                    thoiGianDay = phanCong.ThoiGianDay,
                                    maPhanCongHocPhan = dotphancong.Id
                                };

                    var danhSach = query.ToList();
                    var chuoiTimKhongDau = RemoveDiacritics(chuoiTim).ToLower();

                    var ListFilter = danhSach
                        .Where(op => RemoveDiacritics(op.maGV).ToLower().Contains(chuoiTimKhongDau) ||
                                     RemoveDiacritics(op.tenGV).ToLower().Contains(chuoiTimKhongDau) ||
                                     RemoveDiacritics(op.maHP).ToLower().Contains(chuoiTimKhongDau) ||
                                     RemoveDiacritics(op.tenHP).ToLower().Contains(chuoiTimKhongDau))
                        .Distinct()
                        .OrderBy(op => op.maGV)
                        .ToList();

                    int tongSoLuong = ListFilter.Count();
                    var danhSachTimKiem = ListFilter.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    return (danhSachTimKiem, tongSoLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<HocPhanDaPhanCong>(), 0);
        }

        public (List<HocPhanDaPhanCong>, int) GetDanhSachHocPhanPhanCongTheoChuoiTimVaKhoa(string chuoiTim, string maKhoa, int maDotPhanCong, int page, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(chuoiTim))
                    return (new List<HocPhanDaPhanCong>(), 0);

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotPhanCongs.Find(maDotPhanCong);
                    if (dotKeKhai == null)
                        return (new List<HocPhanDaPhanCong>(), 0);

                    var query = from phanCong in context.PhanCongHocPhans
                                join giangVien in context.GiangViens on phanCong.MaGV equals giangVien.MaGV
                                join hocPhan in context.HocPhans on phanCong.MaHP equals hocPhan.MaHP
                                join dotphancong in context.DotPhanCongs on phanCong.MaDotPhanCong equals dotphancong.Id
                                where phanCong.MaDotPhanCong == maDotPhanCong && giangVien.MaKhoa == maKhoa
                                select new HocPhanDaPhanCong
                                {
                                    maGV = giangVien.MaGV,
                                    tenGV = giangVien.TenGV,
                                    maHP = hocPhan.MaHP,
                                    tenHP = hocPhan.TenHP,
                                    tenLop = phanCong.LopPhanCong,
                                    thoiGianDay = phanCong.ThoiGianDay,
                                    maPhanCongHocPhan = dotphancong.Id
                                };

                    var danhSach = query.ToList();
                    var chuoiTimKhongDau = RemoveDiacritics(chuoiTim).ToLower();

                    var ListFilter = danhSach
                        .Where(op =>
                            RemoveDiacritics(op.maGV).ToLower().Contains(chuoiTimKhongDau) ||
                            RemoveDiacritics(op.tenGV).ToLower().Contains(chuoiTimKhongDau) ||
                            RemoveDiacritics(op.maHP).ToLower().Contains(chuoiTimKhongDau) ||
                            RemoveDiacritics(op.tenHP).ToLower().Contains(chuoiTimKhongDau))
                        .Distinct()
                        .OrderBy(op => op.maGV)
                        .ToList();

                    var tongSoLuong = ListFilter.Count();
                    var danhSachTimKiem = ListFilter.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    return (danhSachTimKiem, tongSoLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<HocPhanDaPhanCong>(), 0);
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

        public bool DuyetTatCaKeKhai(string maGV)
        {
            if (string.IsNullOrEmpty(maGV))
            {
                return false;
            }

            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var keKhaiList = context.KeKhais
                                            .Where(op => op.idNguoiDuyet == null && op.NgayDuyetKeKhai == null)
                                            .ToList();

                    var maPhanCongIds = keKhaiList.Select(k => k.MaPhanCongHocPhan).Distinct().ToList();

                    var hocPhanPhanCongList = context.PhanCongHocPhans
                        .Where(p => maPhanCongIds.Contains(p.MaPhanCongHocPhan))
                        .ToList();

                    foreach (var keKhai in keKhaiList)
                    {
                        var hocPhanPhanCong = hocPhanPhanCongList.FirstOrDefault(p => p.MaPhanCongHocPhan == keKhai.MaPhanCongHocPhan);
                        if (hocPhanPhanCong == null)
                            return false;

                        keKhai.NgayDuyetKeKhai = DateTime.Now;
                        keKhai.idNguoiDuyet = maGV;
                        hocPhanPhanCong.TrangThai = "Hoàn thành";
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

      
    }
}
