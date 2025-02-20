
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KeKhaiService
{
    public class DotKeKhaiService : IDotKeKhai
    {
        public bool CapNhatThoiGianDotKeKhai(int maDotKeKhai, DateTime ngayKetThucMoi, string ghiChu)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        // Tiến hành cập nhật thời gian kết thúc
                        if (KiemTraThoiGianKetThuc(dotKeKhai, ngayKetThucMoi))
                        {
                            dotKeKhai.NgayKetThuc = ngayKetThucMoi;
                            if (!string.IsNullOrEmpty(ghiChu))
                            {
                                dotKeKhai.GhiChu = ghiChu;
                            }

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

        public DotKeKhai GetDotKeKhaiDangMo()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhaiHienTai = context.DotKeKhais
                        .OrderByDescending(s => s.MaDotKeKhai)
                        .FirstOrDefault();

                    return dotKeKhaiHienTai;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public bool KhoaDotKeKhaiHienTai(int maDotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null)
                    {
                        // Tiến hành khóa
                        dotKeKhai.NgayKetThuc = DateTime.Now;
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


        public static string ConvertDateFormat(string originalDateString, string originalFormat, string targetFormat)
        { 
            if (DateTime.TryParseExact(originalDateString, originalFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate)) { 
                    return parsedDate.ToString(targetFormat, CultureInfo.InvariantCulture);
            }
            else
            { 
                
                return $"Không thể chuyển đổi ngày từ chuỗi: {originalDateString}"; 
            }
        }

        public bool KiemTraDotKeKhaiDangMo()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.OrderByDescending(s => s.MaDotKeKhai).FirstOrDefault();
                    if (dotKeKhai != null && dotKeKhai.NgayKetThuc.HasValue)
                    {
                        DateTime dbDate = dotKeKhai.NgayKetThuc.Value;
                        DateTime now = DateTime.Now;

                        // So sánh
                        return dbDate > now;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        public bool KiemTraDuLieuDotKeKhai(string tenDotKeKhai, DateTime ngayBatDau, DateTime ngayKetThuc, string maGV, int hocKy, int namHoc)
        {
            using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
            {
                var namhoc = context.NamHocs.Find(namHoc);
                var hocky = context.HocKies.Find(hocKy);
                if (namhoc == null || hocky == null) return false;
            }

            if (string.IsNullOrEmpty(tenDotKeKhai) || string.IsNullOrEmpty(maGV))
                return false;

            DateTime dateNow = DateTime.Now;

            if (ngayBatDau < dateNow)
            {
                return false;
            }

            if (ngayKetThuc < dateNow)
            {
                return false;
            }

            if (ngayBatDau == ngayKetThuc)
            {
                return false;
            }

            return true;
        }

        public bool KiemTraThoiGianKetThuc(DotKeKhai dotKeKhai, DateTime ngayKetThuc)
        {
            if(ngayKetThuc < DateTime.Now) return false;
            if (ngayKetThuc < dotKeKhai.NgayKetThuc) return false;
            if(ngayKetThuc < dotKeKhai.NgayBatDau) return false;    
            if(ngayKetThuc == dotKeKhai.NgayKetThuc) return false;  

            return true;
        }


        public List<ThongTinPhanCong> LoadThongTinPhanCongTrong()
        {
            List<ThongTinPhanCong> dsPhanCongHocPhan = new List<ThongTinPhanCong>();
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var listPhanCongHocPhan = context.PhanCongHocPhans.Where(op => op.MaDotKeKhai == null).ToList();
                    int index = 1;
                    foreach (var item in listPhanCongHocPhan)
                    {
                        var thongTinPhanCong = new ThongTinPhanCong
                        {
                            id = index,
                            hinhThucDay = item.HinhThucDay,
                            ghiChu = "Tạo ngày " + (item.NgayTao?.ToString("dd/MM/yyyy") ?? ""),
                            maGV = item.MaGV,
                            maHP = item.MaHP,
                            tenLop = item.LopPhanCong,
                            hocKy = item.HocKy,
                            thoiGianDay = item.ThoiGianDay,
                            tenHP = context.HocPhans.Find(item.MaHP)?.TenHP,
                            tenGV = context.GiangViens.Find(item.MaGV)?.TenGV,
                            siSo = (int)item.SiSo,
                            namHoc = item.NamHoc,
                            isPhanCong = true
                        };
                        dsPhanCongHocPhan.Add(thongTinPhanCong);
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dsPhanCongHocPhan;
        }

        public bool TaoDotKeKhaiMoi(string tenDotKeKhai, DateTime ngayBatDau, DateTime ngayKetThuc, string maGV, int hocKy, int namHoc, bool loaiDotKeKhai, string ghiChu)
        {
            try
            {
                if (KiemTraDuLieuDotKeKhai(tenDotKeKhai, ngayBatDau, ngayKetThuc, maGV, hocKy, namHoc))
                {
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        var dotKeKhai = new DotKeKhai();

                        // Tạo ID mới
                        int id = context.DotKeKhais.Count() + 1;
                        while (context.DotKeKhais.Find(id) != null)
                        {
                            id++;
                        }

                        dotKeKhai.MaDotKeKhai = id;
                        dotKeKhai.TenDotKeKhai = tenDotKeKhai;
                        dotKeKhai.NgayBatDau = ngayBatDau;
                        dotKeKhai.NgayKetThuc = ngayKetThuc;
                        dotKeKhai.MaGV = maGV;
                        dotKeKhai.MaHocKy = hocKy;
                        dotKeKhai.MaNamHoc = namHoc;
                        dotKeKhai.GhiChu = ghiChu;
                        dotKeKhai.KeKhaiMoi = loaiDotKeKhai;

                        context.DotKeKhais.Add(dotKeKhai);
                        context.SaveChanges();

                        // Cập nhật các học phần phân công vào đợt kê khai mới
                        var dsPhanCong = context.PhanCongHocPhans.Where(op => op.MaDotKeKhai == null).ToList();
                        foreach (var phanCong in dsPhanCong)
                        {
                            phanCong.MaDotKeKhai = dotKeKhai.MaDotKeKhai;
                            phanCong.TrangThai = "Chưa hoàn thành";
                        }

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

        public bool TaoPhanCongHocPhan(List<ThongTinPhanCong> dsPhanCong, string tenPhanCong, int maNamHoc)
        {
            try
            {
                if (string.IsNullOrEmpty(tenPhanCong)) return false;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var namHoc = context.NamHocs.Find(maNamHoc);
                    if (namHoc == null) return false;

                    // Tạo ra DotPhanCong
                    int idDotPhanCongHocPhan = context.DotPhanCongs.Count();
                    while (context.DotPhanCongs.Find(idDotPhanCongHocPhan) != null)
                    {
                        idDotPhanCongHocPhan++;
                    }

                    var dotPhanCongHocPhan = new DotPhanCong
                    {
                        Id = idDotPhanCongHocPhan,
                        TenDotPhanCong = tenPhanCong,
                        MaNamHoc = namHoc.Id
                    };
                    if (dotPhanCongHocPhan == null) return false;

                    context.DotPhanCongs.Add(dotPhanCongHocPhan);

                    // Lấy mã phân công lớn nhất
                    var lastPhanCongHocPhan = context.PhanCongHocPhans.OrderByDescending(s => s.MaPhanCongHocPhan).FirstOrDefault();
                    var lastFileUpload = context.FilePhanCongs.OrderByDescending(s => s.MaFilePhanCong).FirstOrDefault();

                    int lastId = (lastPhanCongHocPhan != null) ? lastPhanCongHocPhan.MaPhanCongHocPhan + 1 : 1;
                    int lastIdFilePhanCong = (lastFileUpload != null) ? lastFileUpload.MaFilePhanCong : -1;

                    foreach (var item in dsPhanCong)
                    {
                        var phanCongCongViec = new PhanCongHocPhan
                        {
                            MaPhanCongHocPhan = lastId,
                            MaFilePhanCong = lastIdFilePhanCong,
                            MaHP = item.maHP,
                            MaGV = item.maGV,
                            NgayTao = DateTime.Now,
                            LopPhanCong = item.tenLop,
                            SiSo = item.siSo,
                            HinhThucDay = item.hinhThucDay,
                            HocKy = item.hocKy,
                            NamHoc = item.namHoc,
                            ThoiGianDay = item.thoiGianDay,
                            MaDotPhanCong = dotPhanCongHocPhan.Id,
                            TrangThai = "Chưa Hoàn Thành"
                        };

                        context.PhanCongHocPhans.Add(phanCongCongViec);
                        lastId++;
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return false;
        }

        public bool TaoDotKeKhaiDangKyBoSung(string maGV, string tenDot, DateTime ngayBatDau, DateTime ngayKetThuc, int hocKy, int namHoc, string ghiChu, bool loaiDotKeKhai)
        {
            try
            {
                if (KiemTraDuLieuDotKeKhai(tenDot, ngayBatDau, ngayKetThuc, maGV, hocKy, namHoc))
                {
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        var dotKeKhai = new DotKeKhai();

                        // Tạo mới ID
                        int id = context.DotKeKhais.Count() + 1;
                        while (context.DotKeKhais.Find(id) != null)
                        {
                            id++;
                        }

                        dotKeKhai.MaDotKeKhai = id;
                        dotKeKhai.TenDotKeKhai = tenDot;
                        dotKeKhai.NgayBatDau = ngayBatDau;
                        dotKeKhai.NgayKetThuc = ngayKetThuc;
                        dotKeKhai.MaGV = maGV;
                        dotKeKhai.MaHocKy = hocKy;
                        dotKeKhai.MaNamHoc = namHoc;
                        dotKeKhai.GhiChu = ghiChu;
                        dotKeKhai.KeKhaiMoi = loaiDotKeKhai;

                        context.DotKeKhais.Add(dotKeKhai);

                        // Gán phân công cho các học phần có MaDotKeKhai == null
                        var danhSachPhanCong = context.PhanCongHocPhans.Where(op => op.MaDotKeKhai == null).ToList();
                        foreach (var item in danhSachPhanCong)
                        {
                            item.MaDotKeKhai = dotKeKhai.MaDotKeKhai;
                            item.TrangThai = "Chưa Hoàn Thành";
                        }

                        context.SaveChanges();
                        return true;
                    }
                }

                Console.WriteLine("Kiểm tra dữ liệu bị fail");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public List<ThongTinPhanCong> LoadThongTinPhanCongDotDangMo()
        {
            List<ThongTinPhanCong> dsPhanCong = new List<ThongTinPhanCong>();

            try
            {
                if (KiemTraDotKeKhaiDangMo()) // Chuyển sang gọi đồng bộ
                {
                    var dotKeKhaiHienTai = GetDotKeKhaiDangMo(); // Chuyển sang đồng bộ
                    Console.WriteLine("Đợt kê khai hiện tại đang mở có mã là: " + dotKeKhaiHienTai.MaDotKeKhai);

                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        if (dotKeKhaiHienTai != null)
                        {
                            var dsHocPhanPhanCong = context.PhanCongHocPhans
                                .Where(op => op.MaDotKeKhai == dotKeKhaiHienTai.MaDotKeKhai)
                                .ToList(); // Gọi đồng bộ

                            foreach (var item in dsHocPhanPhanCong)
                            {
                                var thongTinPhanCong = new ThongTinPhanCong
                                {
                                    id = item.MaPhanCongHocPhan,
                                    hinhThucDay = item.HinhThucDay,
                                    hocKy = item.HocKy,
                                    namHoc = item.NamHoc,
                                    thoiGianDay = item.ThoiGianDay,
                                    maGV = item.MaGV,
                                    maHP = item.MaHP,
                                    tenLop = item.LopPhanCong,
                                    siSo = (int)item.SiSo,
                                    isPhanCong = true,
                                    ghiChu = "Tạo ngày " + item.NgayTao?.ToString("dd/MM/yyyy")
                                };

                                var giangVien = context.GiangViens.Find(item.MaGV); // Gọi đồng bộ
                                var hocPhan = context.HocPhans.Find(item.MaHP); // Gọi đồng bộ

                                thongTinPhanCong.tenGV = giangVien?.TenGV;
                                thongTinPhanCong.tenHP = hocPhan?.TenHP;

                                dsPhanCong.Add(thongTinPhanCong);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dsPhanCong;
        }

        public bool TaoBoSungPhanCongHocPhan(List<ThongTinPhanCong> dsPhanCong, int idDotPhanCong)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotPhanCongTruoc = context.DotPhanCongs.Find(idDotPhanCong);
                    if (dotPhanCongTruoc == null)
                    {
                        return false;
                    }

                    var lastPhanCongHocPhan = context.PhanCongHocPhans
                        .OrderByDescending(s => s.MaPhanCongHocPhan)
                        .FirstOrDefault();

                    var lastFileUpload = context.FilePhanCongs
                        .OrderByDescending(s => s.MaFilePhanCong)
                        .FirstOrDefault();

                    int lastId = 1;
                    int lastIdFilePhanCong = -1;

                    var idDotKeKhai = (from phanCongHocPhan in context.PhanCongHocPhans
                                       join dotPhanCong in context.DotPhanCongs
                                       on phanCongHocPhan.MaDotPhanCong equals dotPhanCong.Id
                                       where phanCongHocPhan.MaDotPhanCong == idDotPhanCong
                                       select phanCongHocPhan.MaDotKeKhai).FirstOrDefault();

                    if (lastPhanCongHocPhan != null)
                    {
                        lastId = lastPhanCongHocPhan.MaPhanCongHocPhan + 1;
                    }

                    if (lastFileUpload != null)
                    {
                        lastIdFilePhanCong = lastFileUpload.MaFilePhanCong;
                    }

                    foreach (var item in dsPhanCong)
                    {
                        var phanCongCongViec = new PhanCongHocPhan
                        {
                            MaPhanCongHocPhan = lastId,
                            MaFilePhanCong = lastIdFilePhanCong,
                            MaHP = item.maHP,
                            MaGV = item.maGV,
                            NgayTao = DateTime.Now,
                            LopPhanCong = item.tenLop,
                            SiSo = item.siSo,
                            HinhThucDay = item.hinhThucDay,
                            HocKy = item.hocKy,
                            NamHoc = item.namHoc,
                            ThoiGianDay = item.thoiGianDay,
                            MaDotPhanCong = idDotPhanCong,
                            TrangThai = "Chưa hoàn thành"
                        };

                        if (idDotKeKhai != null)
                        {
                            phanCongCongViec.MaDotKeKhai = idDotKeKhai;
                        }

                        context.PhanCongHocPhans.Add(phanCongCongViec);
                        lastId++;
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return false;
        }

        public ThongBaoDotKeKhai LayThongTinDotKeKhaiHienTai()
        {
            try
            {
                if (KiemTraDotKeKhaiDangMo()) // Chuyển sang phương thức đồng bộ
                {
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        var dotKeKhai = GetDotKeKhaiDangMo(); // Chuyển sang phương thức đồng bộ
                        if (dotKeKhai != null)
                        {
                            var giangVien = context.GiangViens.Find(dotKeKhai.MaGV);
                            var namHoc = context.NamHocs.Find(dotKeKhai.MaNamHoc);
                            var hocky = context.HocKies.Find(dotKeKhai.MaHocKy);

                            if (giangVien != null && namHoc != null && hocky != null)
                            {
                                var thongTinDotKeKhai = new ThongBaoDotKeKhai
                                {
                                    Id = dotKeKhai.MaDotKeKhai,
                                    tenDotKeKhai = dotKeKhai.TenDotKeKhai,
                                    ngayBatDau = dotKeKhai.NgayBatDau?.ToString("dd/MM/yyyy hh:mm tt"),
                                    ngayKetThuc = dotKeKhai.NgayKetThuc?.ToString("dd/MM/yyyy hh:mm tt"),
                                    nguoiTao = giangVien.TenGV,
                                    ghiChu = dotKeKhai.GhiChu,
                                    namHoc = namHoc.TenNamHoc,
                                    hocKy = hocky.TenHocKy,
                                    trangThai = (bool)dotKeKhai.KeKhaiMoi ? "Đợt kê khai bổ sung" : "Đợt kê khai học phần"
                                };
                                return thongTinDotKeKhai;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public List<DotKeKhai> LoadDotKeKhaiTheoHocKyNamHoc(int maNamHoc, int maHocKy)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dsDotKeKhai = context.DotKeKhais
                        .Where(op => op.MaHocKy == maHocKy && op.MaNamHoc == maNamHoc)
                        .OrderByDescending(op => op.MaDotKeKhai)
                        .ToList(); // Sử dụng phương thức đồng bộ

                    return dsDotKeKhai;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải danh sách đợt kê khai: {ex.Message}");
                return new List<DotKeKhai>();
            }
        }

        public List<XemThongTinHocPhanDuocPhanCong> LoadDanhSachHocPhanDuocPhanCongTheoDot(string maGV, int maDotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dsPhanCongCuaGiangVien = (from pc in context.PhanCongHocPhans
                                                  join hp in context.HocPhans
                                                  on pc.MaHP equals hp.MaHP
                                                  join dkk in context.DotKeKhais
                                                  on pc.MaDotKeKhai equals dkk.MaDotKeKhai
                                                  join gv in context.GiangViens
                                                  on pc.MaGV equals gv.MaGV
                                                  where gv.MaGV == maGV && pc.MaDotKeKhai == maDotKeKhai
                                                  select new XemThongTinHocPhanDuocPhanCong
                                                  {
                                                      Id = pc.MaPhanCongHocPhan,
                                                      hinhThucDay = pc.HinhThucDay,
                                                      hocKy = pc.HocKy,
                                                      namHoc = pc.NamHoc,
                                                      ngayDay = pc.ThoiGianDay,
                                                      siSo = (int)pc.SiSo,
                                                      maHocPhanPhanCong = hp.MaHP,
                                                      tenHocPhanPhanCong = hp.TenHP,
                                                      tenLop = pc.LopPhanCong,

                                                  }).OrderBy(op => op.Id).ToList(); // Dùng phương thức đồng bộ

                    return dsPhanCongCuaGiangVien;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<XemThongTinHocPhanDuocPhanCong>();
            }
        }

        public List<DotKeKhai> XemDanhSachDotKeKhaiTheoHocKyNamHoc(int maNamHoc, int maHocKy)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dsDotKeKhai = context.DotKeKhais
                        .Where(op => op.MaHocKy == maHocKy
                                     && op.MaNamHoc == maNamHoc)
                        .OrderByDescending(op => op.MaDotKeKhai)
                        .ToList(); // Dùng phương thức đồng bộ

                    return dsDotKeKhai;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải danh sách đợt kê khai: {ex.Message}");
                return new List<DotKeKhai>();
            }
        }

        public bool DotKeKhaiSapMo()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais
                        .OrderByDescending(s => s.MaDotKeKhai)
                        .FirstOrDefault(); // Dùng phương thức đồng bộ

                    if (dotKeKhai != null && dotKeKhai.NgayBatDau > DateTime.Now)
                    {
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


        public ThongTinDotKeKhaiGanNhat DotKeKhaiGanNhat()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais
                        .OrderByDescending(op => op.MaDotKeKhai)
                        .FirstOrDefault(); // Truy vấn đồng bộ

                    if (dotKeKhai != null)
                    {
                        var namHoc = context.NamHocs.Find(dotKeKhai.MaNamHoc);
                        var hocKy = context.HocKies.Find(dotKeKhai.MaHocKy);

                        if (namHoc != null && hocKy != null)
                        {
                            return new ThongTinDotKeKhaiGanNhat
                            {
                                maDotKeKhai = dotKeKhai.MaDotKeKhai,
                                maHocKy = hocKy.MaHocKy,
                                maNamHoc = namHoc.Id
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new ThongTinDotKeKhaiGanNhat();
        }

        public Tuple<List<XemThongTinHocPhanDuocPhanCong>, ThongTinDotKeKhaiGanNhat> LoadDanhSachHocPhanDuocPhanCongTheoDotGanNhat(string maGV)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhaiGanNhat = context.DotKeKhais
                        .OrderByDescending(op => op.MaDotKeKhai)
                        .FirstOrDefault();

                    if (dotKeKhaiGanNhat != null)
                    {
                        var thongTin = (from dotKeKhai in context.DotKeKhais
                                        join namHoc in context.NamHocs
                                        on dotKeKhai.MaNamHoc equals namHoc.Id
                                        join hocKy in context.HocKies
                                        on dotKeKhai.MaHocKy equals hocKy.MaHocKy
                                        where dotKeKhai.MaDotKeKhai == dotKeKhaiGanNhat.MaDotKeKhai
                                        select new ThongTinDotKeKhaiGanNhat
                                        {
                                            maDotKeKhai = dotKeKhai.MaDotKeKhai,
                                            maHocKy = hocKy.MaHocKy,
                                            maNamHoc = namHoc.Id
                                        }).FirstOrDefault();

                        var dsPhanCongCuaGiangVien = (from pc in context.PhanCongHocPhans
                                                      join hp in context.HocPhans
                                                      on pc.MaHP equals hp.MaHP
                                                      join dkk in context.DotKeKhais
                                                      on pc.MaDotKeKhai equals dkk.MaDotKeKhai
                                                      join gv in context.GiangViens
                                                      on pc.MaGV equals gv.MaGV
                                                      where gv.MaGV == maGV && pc.MaDotKeKhai == dotKeKhaiGanNhat.MaDotKeKhai
                                                      select new XemThongTinHocPhanDuocPhanCong
                                                      {
                                                          Id = pc.MaPhanCongHocPhan,
                                                          hinhThucDay = pc.HinhThucDay,
                                                          hocKy = pc.HocKy,
                                                          namHoc = pc.NamHoc,
                                                          ngayDay = pc.ThoiGianDay,
                                                          siSo = (int)pc.SiSo,
                                                          maHocPhanPhanCong = hp.MaHP,
                                                          tenHocPhanPhanCong = hp.TenHP,
                                                          tenLop = pc.LopPhanCong,

                                                      }).OrderBy(op => op.Id).ToList();

                        return new Tuple<List<XemThongTinHocPhanDuocPhanCong>, ThongTinDotKeKhaiGanNhat>(dsPhanCongCuaGiangVien, thongTin);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new Tuple<List<XemThongTinHocPhanDuocPhanCong>, ThongTinDotKeKhaiGanNhat>(
                new List<XemThongTinHocPhanDuocPhanCong>(),
                new ThongTinDotKeKhaiGanNhat()
            );
        }

        public string GetTenDotKeKhaiTheoMa(int maDotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dotKeKhai = context.DotKeKhais.Find(maDotKeKhai);
                    if (dotKeKhai != null) return dotKeKhai.TenDotKeKhai;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }


    }
}
