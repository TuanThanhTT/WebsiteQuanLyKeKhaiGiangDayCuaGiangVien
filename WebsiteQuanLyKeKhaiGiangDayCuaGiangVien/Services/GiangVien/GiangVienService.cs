
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Services.MD5Service;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.GiangVien
{
    public class GiangVienService : IGiangVienService
    {
        public bool CapNhatThongTinGiangVien(Models.GiangVien giangVien)
        {
            try
            {
                if (giangVien == null) return false;
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var gv = context.GiangViens.Find(giangVien.MaGV); // Dùng phương thức đồng bộ
                    if (gv != null)
                    {
                        gv.MaKhoa = giangVien.MaKhoa;
                        gv.TenGV = giangVien.TenGV;
                        gv.MaGV = giangVien.MaGV;
                        gv.ChucVu = giangVien.ChucVu;
                        gv.Email = giangVien.Email;
                        gv.SoDienThoai = giangVien.SoDienThoai;
                        gv.ChuyenNghanh = giangVien.ChuyenNghanh;
                        gv.TrinhDo = giangVien.TrinhDo;
                        gv.NgaySinh = giangVien.NgaySinh;
                        gv.DiaChi = giangVien.DiaChi;
                        gv.LoaiHinhDaoTao = giangVien.LoaiHinhDaoTao;
                        gv.GioiTinh = giangVien.GioiTinh;
                        gv.HeSoLuong = giangVien.HeSoLuong;

                        context.SaveChanges(); // Dùng phương thức đồng bộ
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

        public List<XemChiTietThongTinGiangVien> DanhSachGiangVienDaKhoa()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from gv in context.GiangViens
                                join khoa in context.Khoas on gv.MaKhoa equals khoa.MaKhoa
                                where gv.IsDelete == true
                                select new Models.ModelCustom.XemChiTietThongTinGiangVien
                                {
                                    maGV = gv.MaGV,
                                    tenGV = gv.TenGV,
                                    ngaySinh = gv.NgaySinh.ToString("dd/MM/yyyy"),
                                    chucVu = gv.ChucVu,
                                    diaChi = gv.DiaChi,
                                    email = gv.Email,
                                    gioiTinh = (gv.GioiTinh == true) ? "Nam" : "Nữ",
                                    chuyenNghanh = gv.ChuyenNghanh,
                                    tenKhoa = khoa.TenKhoa,
                                    heSoLuong = (double)gv.HeSoLuong,
                                    soDienThoai = gv.SoDienThoai,
                                    trinhDo = gv.TrinhDo,
                                    loaiHinhDaoTao = gv.LoaiHinhDaoTao,
                                    maKhoa = khoa.MaKhoa
                                };

                    return query.ToList(); // Dùng ToList() thay vì ToListAsync()
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new List<XemChiTietThongTinGiangVien>();
        }

        public (List<XemChiTietThongTinGiangVien> data, int tongSoLuong) GetDanhSachGiangVienTheoChuoiTim(string chuoiTim, int page, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(chuoiTim))
                {
                    return (new List<XemChiTietThongTinGiangVien>(), 0);
                }

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = (from giangVien in context.GiangViens
                                 join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                 where giangVien.IsDelete == false
                                 select new
                                 {
                                     maGV = giangVien.MaGV,
                                     gioiTinh = (giangVien.GioiTinh == true) ? "Nam" : "Nữ",
                                     chucVu = giangVien.ChucVu,
                                     diaChi = giangVien.DiaChi,
                                     chuyenNghanh = giangVien.ChuyenNghanh,
                                     email = giangVien.Email,
                                     heSoLuong = (double)giangVien.HeSoLuong,
                                     loaiHinhDaoTao = giangVien.LoaiHinhDaoTao,
                                     maKhoa = giangVien.MaKhoa,
                                     namSinh = giangVien.NgaySinh,
                                     ngaySinh = giangVien.NgaySinh, // Giữ nguyên DateTime
                                     soDienThoai = giangVien.SoDienThoai,
                                     tenGV = giangVien.TenGV,
                                     tenKhoa = khoa.TenKhoa,
                                     trinhDo = giangVien.TrinhDo
                                 })
               .AsEnumerable() // Chỉ sau đây mới xử lý định dạng ngày tháng
               .Select(x => new XemChiTietThongTinGiangVien
               {
                   maGV = x.maGV,
                   gioiTinh = x.gioiTinh,
                   chucVu = x.chucVu,
                   diaChi = x.diaChi,
                   chuyenNghanh = x.chuyenNghanh,
                   email = x.email,
                   heSoLuong = x.heSoLuong,
                   loaiHinhDaoTao = x.loaiHinhDaoTao,
                   maKhoa = x.maKhoa,
                   namSinh = x.namSinh,
                   ngaySinh = x.ngaySinh.ToString("dd/MM/yyyy"), // Định dạng sau khi đã lấy về RAM
                   soDienThoai = x.soDienThoai,
                   tenGV = x.tenGV,
                   tenKhoa = x.tenKhoa,
                   trinhDo = x.trinhDo
               });


                    var danhSach = query.ToList(); // Đổi từ ToListAsync() sang ToList()

                    // Lọc và tìm kiếm
                    var chuoiTimKhongDau = RemoveDiacritics(chuoiTim).ToLower().Trim();
                    var listFilter = danhSach
                        .Where(op =>(RemoveDiacritics(op.maGV.ToLower()).Contains(chuoiTimKhongDau)||
                                    RemoveDiacritics(op.tenGV.ToLower()).Contains(chuoiTimKhongDau))
                                    )
                        .OrderBy(op => op.maGV)
                        .ToList();

                    var soLuong = listFilter.Count;
                    var result = listFilter.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    return (result, soLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<XemChiTietThongTinGiangVien>(), 0);
        }

        public (List<XemChiTietThongTinGiangVien> data, int tongSoLuong) GetDanhSachGiangVienTheoChuoiTimVaKhoa(string chuoiTim, string maKhoa, int page, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(chuoiTim))
                {
                    return (new List<XemChiTietThongTinGiangVien>(), 0);
                }

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var khoaTim = context.Khoas.Find(maKhoa);
                    if (khoaTim == null)
                    {
                        return (new List<XemChiTietThongTinGiangVien>(), 0);
                    }

                    var query = (from giangVien in context.GiangViens
                                 join khoa in context.Khoas
                                 on giangVien.MaKhoa equals khoa.MaKhoa
                                 where giangVien.MaKhoa == maKhoa
                                 select new
                                 {
                                     maGV = giangVien.MaGV,
                                     gioiTinh = (giangVien.GioiTinh == true) ? "Nam" : "Nữ",
                                     chucVu = giangVien.ChucVu,
                                     diaChi = giangVien.DiaChi,
                                     chuyenNghanh = giangVien.ChuyenNghanh,
                                     email = giangVien.Email,
                                     heSoLuong = (double)giangVien.HeSoLuong,
                                     loaiHinhDaoTao = giangVien.LoaiHinhDaoTao,
                                     maKhoa = giangVien.MaKhoa,
                                     namSinh = giangVien.NgaySinh,
                                     ngaySinh = giangVien.NgaySinh, // Giữ nguyên DateTime
                                     soDienThoai = giangVien.SoDienThoai,
                                     tenGV = giangVien.TenGV,
                                     tenKhoa = khoa.TenKhoa,
                                     trinhDo = giangVien.TrinhDo
                                 })
                .AsEnumerable() // Sau đây mới xử lý ngày tháng
                .Select(x => new XemChiTietThongTinGiangVien
                {
                    maGV = x.maGV,
                    gioiTinh = x.gioiTinh,
                    chucVu = x.chucVu,
                    diaChi = x.diaChi,
                    chuyenNghanh = x.chuyenNghanh,
                    email = x.email,
                    heSoLuong = x.heSoLuong,
                    loaiHinhDaoTao = x.loaiHinhDaoTao,
                    maKhoa = x.maKhoa,
                    namSinh = x.namSinh,
                    ngaySinh = x.ngaySinh.ToString("dd/MM/yyyy"), // Chuyển đổi ở đây
                    soDienThoai = x.soDienThoai,
                    tenGV = x.tenGV,
                    tenKhoa = x.tenKhoa,
                    trinhDo = x.trinhDo
                });


                    var danhSach = query.ToList();
                    var chuoiTimKhongDau = RemoveDiacritics(chuoiTim).ToLower().Trim();
                    var listFilter = danhSach.Where(op => RemoveDiacritics(op.maGV).Contains(chuoiTim) ||
                                                          RemoveDiacritics(op.tenGV).Contains(chuoiTim))
                                             .OrderBy(op => op.maGV)
                                             .ToList();

                    var soLuong = listFilter.Count;
                    var result = listFilter.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return (result, soLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return (new List<XemChiTietThongTinGiangVien>(), 0);
        }

        public List<GiangVienTheoKhoa> GetDanhSachGiangVienTheoKhoa(string maKhoa)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var khoa = context.Khoas.Find(maKhoa);
                    if (khoa != null)
                    {
                        var query = from giangVien in context.GiangViens
                                    join k in context.Khoas
                                    on giangVien.MaKhoa equals k.MaKhoa
                                    where k.MaKhoa == khoa.MaKhoa
                                    select new GiangVienTheoKhoa
                                    {
                                        maGV = giangVien.MaGV,
                                        tenGV = giangVien.TenGV
                                    };

                        var danhSachGiangVien = query.ToList();
                        return danhSachGiangVien;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new List<GiangVienTheoKhoa>();
        }

        public (List<XemChiTietThongTinGiangVien> data, int tongSoLuong) GetDanhSachGiangVienTheoKhoa(string maKhoa, int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var khoaTim = context.Khoas.Find(maKhoa);
                    if (khoaTim == null)
                    {
                        return (new List<XemChiTietThongTinGiangVien>(), 0);
                    }

                    var query = (from giangVien in context.GiangViens
                                 join khoa in context.Khoas
                                 on giangVien.MaKhoa equals khoa.MaKhoa
                                 where giangVien.MaKhoa == maKhoa
                                 select new
                                 {
                                     maGV = giangVien.MaGV,
                                     gioiTinh = (giangVien.GioiTinh == true) ? "Nam" : "Nữ",
                                     chucVu = giangVien.ChucVu,
                                     diaChi = giangVien.DiaChi,
                                     chuyenNghanh = giangVien.ChuyenNghanh,
                                     email = giangVien.Email,
                                     heSoLuong = (double)giangVien.HeSoLuong,
                                     loaiHinhDaoTao = giangVien.LoaiHinhDaoTao,
                                     maKhoa = giangVien.MaKhoa,
                                     namSinh = giangVien.NgaySinh,
                                     ngaySinh = giangVien.NgaySinh, // Giữ nguyên kiểu DateTime
                                     soDienThoai = giangVien.SoDienThoai,
                                     tenGV = giangVien.TenGV,
                                     tenKhoa = khoa.TenKhoa,
                                     trinhDo = giangVien.TrinhDo
                                 }).AsEnumerable() // Đưa về bộ nhớ
             .Select(x => new XemChiTietThongTinGiangVien
             {
                 maGV = x.maGV,
                 gioiTinh = x.gioiTinh,
                 chucVu = x.chucVu,
                 diaChi = x.diaChi,
                 chuyenNghanh = x.chuyenNghanh,
                 email = x.email,
                 heSoLuong = x.heSoLuong,
                 loaiHinhDaoTao = x.loaiHinhDaoTao,
                 maKhoa = x.maKhoa,
                 namSinh = x.namSinh,
                 ngaySinh = x.ngaySinh.ToString("dd/MM/yyyy"), // Đổi định dạng sau khi lấy về RAM
                 soDienThoai = x.soDienThoai,
                 tenGV = x.tenGV,
                 tenKhoa = x.tenKhoa,
                 trinhDo = x.trinhDo
             });


                    var danhSach = query.ToList();
                    var soLuong = danhSach.Count;
                    var result = danhSach.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    return (result, soLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                
            }
            return (new List<XemChiTietThongTinGiangVien>(), 0);
        }

        public Models.ModelCustom.XemChiTietThongTinGiangVien GetGiangVienTheoMa(string maGV)
        {
            try
            {
                if (string.IsNullOrEmpty(maGV)) return null;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    //var giangVien = (from gv in context.GiangViens
                    //                 join khoa in context.Khoas on gv.MaKhoa equals khoa.MaKhoa
                    //                 where gv.MaGV == maGV
                    //                 select new Models.ModelCustom.XemChiTietThongTinGiangVien
                    //                 {
                    //                     maGV = gv.MaGV,
                    //                     tenGV = gv.TenGV,
                    //                     tenKhoa = khoa.TenKhoa,
                    //                     maKhoa = khoa.MaKhoa,
                    //                     ngaySinh = gv.NgaySinh.ToString("dd/MM/yyyy"),
                    //                     chucVu = gv.ChucVu,
                    //                     chuyenNghanh = gv.ChuyenNghanh,
                    //                     diaChi = gv.DiaChi,
                    //                     email = gv.Email,
                    //                     soDienThoai = gv.SoDienThoai,
                    //                     gioiTinh = (gv.GioiTinh == true) ? "Nam" : "Nữ",
                    //                     heSoLuong = (double)gv.HeSoLuong,
                    //                     loaiHinhDaoTao = gv.LoaiHinhDaoTao,
                    //                     trinhDo = gv.TrinhDo,
                    //                     namSinh = gv.NgaySinh,
                    //                     Avartar = gv.Avartar
                    //                 }).FirstOrDefault();
                    var giangVien = context.GiangViens
                       .Join(context.Khoas,
                             gv => gv.MaKhoa,
                             khoa => khoa.MaKhoa,
                             (gv, khoa) => new { gv, khoa })
                       .Where(x => x.gv.MaGV == maGV)
                       .AsEnumerable() // Chuyển sang xử lý trên bộ nhớ
                       .Select(x => new Models.ModelCustom.XemChiTietThongTinGiangVien
                       {
                           maGV = x.gv.MaGV,
                           tenGV = x.gv.TenGV,
                           tenKhoa = x.khoa.TenKhoa,
                           maKhoa = x.khoa.MaKhoa,
                           ngaySinh = x.gv.NgaySinh.ToString("dd/MM/yyyy"), // Định dạng ở bộ nhớ
                           chucVu = x.gv.ChucVu,
                           chuyenNghanh = x.gv.ChuyenNghanh,
                           diaChi = x.gv.DiaChi,
                           email = x.gv.Email,
                           soDienThoai = x.gv.SoDienThoai,
                           gioiTinh = (x.gv.GioiTinh == true) ? "Nam" : "Nữ",
                           heSoLuong = (double)x.gv.HeSoLuong,
                           loaiHinhDaoTao = x.gv.LoaiHinhDaoTao,
                           trinhDo = x.gv.TrinhDo,
                           namSinh = x.gv.NgaySinh,
                           Avartar = x.gv.Avartar
                       }).FirstOrDefault();


                    return giangVien;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return null;
        }

        public string KiemTraCapNhatEmailTonTai(string email, string maGV)
        {
            string errMess = "";
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Where(op => op.Email == email).FirstOrDefault();
                    if (giangVien != null && giangVien.MaGV != maGV)
                    {
                        errMess = "Email đã tồn tại!";
                        return errMess;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return errMess;
        }

        public bool KiemTraEmailHopLe(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public string KiemTraEmailTonTai(string email)
        {
            string errMess = "";
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Where(op => op.Email == email).FirstOrDefault();
                    if (giangVien != null)
                    {
                        errMess = "Email đã tồn tại!";
                        return errMess;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return errMess;
        }

        public bool KiemTraMaGiangVienTonTai(string maGV)
        {
            try
            {
                if (string.IsNullOrEmpty(maGV)) return false;
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Find(maGV);
                    if (giangVien != null)
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

        public string KiemTraSoDienThoaiCapNhatTonTai(string sdt, string maGV)
        {
            string errMess = "";
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Where(op => op.SoDienThoai == sdt).FirstOrDefault();
                    if (giangVien != null && giangVien.MaGV != maGV)
                    {
                        errMess = "Số điện thoại này đã tồn tại!";
                        return errMess;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return errMess;
        }

        public bool KiemTraSoDienThoaiHopLe(string soDienThoai)
        {
            if (string.IsNullOrEmpty(soDienThoai)) return false;
            if(soDienThoai.Length <10 || soDienThoai.Length > 10) return false;
            if (!soDienThoai.StartsWith("0")) return false;
            return true;
        }

        public string KiemTraSoDienThoaiTonTai(string sdt)
        {
            string errMess = "";
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Where(op => op.SoDienThoai == sdt).FirstOrDefault();
                    if (giangVien != null)
                    {
                        errMess = "Số điện thoại này đã tồn tại!";
                        return errMess;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return errMess;
        }

        public (List<Models.ModelCustom.XemChiTietThongTinGiangVien> data, int soLuong) LoadDanhSachGiangVien(int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    //var query = from gv in context.GiangViens
                    //            join khoa in context.Khoas
                    //            on gv.MaKhoa equals khoa.MaKhoa
                    //            where gv.IsDelete == false
                    //            select new Models.ModelCustom.XemChiTietThongTinGiangVien
                    //            {
                    //                maGV = gv.MaGV,
                    //                tenGV = gv.TenGV,
                    //                ngaySinh = gv.NgaySinh.ToString("dd/MM/yyyy"),
                    //                chucVu = gv.ChucVu,
                    //                diaChi = gv.DiaChi,
                    //                email = gv.Email,
                    //                gioiTinh = (gv.GioiTinh == true) ? "Nam" : "Nữ",
                    //                chuyenNghanh = gv.ChuyenNghanh,
                    //                tenKhoa = khoa.TenKhoa,
                    //                heSoLuong = (double)gv.HeSoLuong,
                    //                soDienThoai = gv.SoDienThoai,
                    //                trinhDo = gv.TrinhDo,
                    //                loaiHinhDaoTao = gv.LoaiHinhDaoTao,
                    //                maKhoa = khoa.MaKhoa
                    //            };

                    var query = context.GiangViens
            .Join(context.Khoas,
                  gv => gv.MaKhoa,
                  khoa => khoa.MaKhoa,
                  (gv, khoa) => new { gv, khoa })
            .Where(x => x.gv.IsDelete == false)
            .AsEnumerable() // Chuyển sang xử lý trong bộ nhớ
            .Select(x => new Models.ModelCustom.XemChiTietThongTinGiangVien
            {
                maGV = x.gv.MaGV,
                tenGV = x.gv.TenGV,
                ngaySinh = x.gv.NgaySinh.ToString("dd/MM/yyyy"), // Được gọi trong bộ nhớ
                chucVu = x.gv.ChucVu,
                diaChi = x.gv.DiaChi,
                email = x.gv.Email,
                gioiTinh = (x.gv.GioiTinh == true) ? "Nam" : "Nữ",
                chuyenNghanh = x.gv.ChuyenNghanh,
                tenKhoa = x.khoa.TenKhoa,
                heSoLuong = (double)x.gv.HeSoLuong,
                soDienThoai = x.gv.SoDienThoai,
                trinhDo = x.gv.TrinhDo,
                loaiHinhDaoTao = x.gv.LoaiHinhDaoTao,
                maKhoa = x.khoa.MaKhoa
            }).ToList();


                    var soLuong = query.Count(); // Đếm tổng số giảng viên
                    var danhSachGiangVien = query.Skip((page - 1) * pageSize).Take(pageSize).ToList(); // Lấy danh sách theo phân trang

                    if (danhSachGiangVien.Any())
                    {
                        return (danhSachGiangVien, soLuong);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (new List<Models.ModelCustom.XemChiTietThongTinGiangVien>(), 0);
            }
            return (new List<Models.ModelCustom.XemChiTietThongTinGiangVien>(), 0);
        }

        public (List<Models.ModelCustom.XemChiTietThongTinGiangVien> data, int soLuong) LoadDanhSachGiangVienDaKhoa(int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from gv in context.GiangViens
                                join khoa in context.Khoas
                                on gv.MaKhoa equals khoa.MaKhoa
                                where gv.IsDelete == true
                                select new Models.ModelCustom.XemChiTietThongTinGiangVien
                                {
                                    maGV = gv.MaGV,
                                    tenGV = gv.TenGV,
                                    ngaySinh = gv.NgaySinh.ToString("dd/MM/yyyy"),
                                    chucVu = gv.ChucVu,
                                    diaChi = gv.DiaChi,
                                    email = gv.Email,
                                    gioiTinh = (gv.GioiTinh == true) ? "Nam" : "Nữ",
                                    chuyenNghanh = gv.ChuyenNghanh,
                                    tenKhoa = khoa.TenKhoa,
                                    heSoLuong = (double)gv.HeSoLuong,
                                    soDienThoai = gv.SoDienThoai,
                                    trinhDo = gv.TrinhDo,
                                    loaiHinhDaoTao = gv.LoaiHinhDaoTao,
                                    maKhoa = khoa.MaKhoa
                                };

                    var soLuong = query.Count(); // Đếm tổng số giảng viên đã khóa
                    var danhSachGiangVien = query.Skip((page - 1) * pageSize).Take(pageSize).ToList(); // Lấy danh sách theo phân trang

                    if (danhSachGiangVien.Any())
                    {
                        return (danhSachGiangVien, soLuong);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (new List<Models.ModelCustom.XemChiTietThongTinGiangVien>(), 0);
        }

        public bool MoKhoaDanhSachGiangVien(List<string> ListIdGiangVien)
        {
            try
            {
                if (ListIdGiangVien == null || !ListIdGiangVien.Any())
                {
                    Console.WriteLine("null danh sach mo khoa");
                    return false;
                }

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    // Lấy danh sách các giảng viên tương ứng
                    var giangViens = context.GiangViens
                                            .Where(k => ListIdGiangVien.Contains(k.MaGV))
                                            .ToList();
                    
                    if (giangViens.Count != ListIdGiangVien.Count)
                    {
                        Console.WriteLine("lay ra giang vien mo khoa null");
                        return false;
                    }

                    // Đánh dấu IsDelete = false
                    foreach (var giangvien in giangViens)
                    {
                        giangvien.IsDelete = false;
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

        public bool ThemDanhSachGiangVienMoi(List<ThongTinUpLoadGiangVien> danhSachGiangVien)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    foreach (var item in danhSachGiangVien)
                    {
                        var gioiTinh = (item.gioiTinh.ToLower() == "nam") ? true : false;
                        var strngaySinh = item.ngaySinh;
                        DateTime ngaySinh;
                        string[] formats = { "d/MM/yyyy", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy" };

                        if (DateTime.TryParseExact(strngaySinh, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out ngaySinh))
                        {
                            var giangVien = new Models.GiangVien
                            {
                                MaGV = item.maGV,
                                TenGV = item.tenGV,
                                ChucVu = item.chucVu,
                                Email = item.email,
                                GioiTinh = gioiTinh,
                                HeSoLuong = item.heSoLuong,
                                NgaySinh = ngaySinh,
                                DiaChi = item.diaChi,
                                ChuyenNghanh = item.chuyenNghanh,
                                MaKhoa = item.maKhoa,
                                SoDienThoai = item.soDienThoai,
                                TrinhDo = item.trinhDo,
                                LoaiHinhDaoTao = item.loaiHinhDaoTao,
                            };

                            // Tạo tài khoản
                            var idAccount = context.TaiKhoans.Count();
                            while (context.TaiKhoans.Find(idAccount) != null)
                            {
                                idAccount++;
                            }
                            MD5Service mD5Service = new MD5Service();

                            var taiKhoan = new TaiKhoan
                            {
                                MaTaiKhoan = idAccount,
                                UserName = giangVien.MaGV,
                                Passwords = mD5Service.GetMd5Hash(giangVien.MaGV),
                                MaGV = giangVien.MaGV,
                                NgayTao = DateTime.Now,
                            };
                            context.TaiKhoans.Add(taiKhoan);

                            // Tạo AccountRole
                            var idAccounRole = context.AccountRoles.Count();
                            while (context.AccountRoles.Find(idAccounRole) != null)
                            {
                                idAccounRole++;
                            }
                            var accrole = new AccountRole
                            {
                                AccountRoleId = idAccounRole,
                                AccountId = taiKhoan.MaTaiKhoan,
                                RoleId = 3,
                            };
                            context.AccountRoles.Add(accrole);

                            // Thêm giảng viên
                            context.GiangViens.Add(giangVien);
                        }
                        else
                        {
                            Console.WriteLine($"Ngày sinh không hợp lệ cho giảng viên {item.tenGV}: {strngaySinh}");
                        }
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public bool ThemGiangVienMoi(Models.GiangVien giangVien)
        {
            try
            {
                if (giangVien == null) return false;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    context.GiangViens.Add(giangVien);

                    // Thêm tài khoản
                    var idAccount = context.TaiKhoans.Count();
                    while (context.TaiKhoans.Find(idAccount) != null)
                    {
                        idAccount++;
                    }
                    MD5Service mD5Service = new MD5Service();

                    var taiKhoan = new TaiKhoan
                    {
                        MaTaiKhoan = idAccount,
                        UserName = giangVien.MaGV,
                        MaGV = giangVien.MaGV,
                        Passwords = mD5Service.GetMd5Hash(giangVien.MaGV),
                        NgayTao = DateTime.Now
                    };
                    context.TaiKhoans.Add(taiKhoan);

                    // Thêm account role
                    int idAccountRole = context.AccountRoles.Count();
                    while (context.AccountRoles.Find(idAccountRole) != null)
                    {
                        idAccountRole++;
                    }

                    var accountRole = new AccountRole
                    {
                        AccountRoleId = idAccountRole,
                        AccountId = taiKhoan.MaTaiKhoan,
                        RoleId = 3
                    };
                    context.AccountRoles.Add(accountRole);

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return false;
        }

        public bool UpdateAvartar(string avartar, string maGV)
        {
            try
            {
                if (string.IsNullOrEmpty(avartar) || string.IsNullOrEmpty(maGV))
                    return false;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Find(maGV);
                    if (giangVien != null)
                    {
                        giangVien.Avartar = avartar;
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

        public bool XoaDanhSachGiangVien(List<string> ListIdGiangVien)
        {
            try
            {
                if (ListIdGiangVien == null || !ListIdGiangVien.Any())
                {
                    return false;
                }

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    // Lấy danh sách các giảng viên tương ứng
                    var giangViens = context.GiangViens
                                            .Where(k => ListIdGiangVien.Contains(k.MaGV) && !k.IsDelete)
                                            .ToList();

                    Console.WriteLine("Số lượng: " + giangViens.Count + " - " + ListIdGiangVien.Count);
                    if (giangViens.Count != ListIdGiangVien.Count)
                    {
                        Console.WriteLine("Không tìm thấy giảng viên");
                        return false;
                    }

                    // Đánh dấu IsDelete = true
                    foreach (var giangvien in giangViens)
                    {
                        giangvien.IsDelete = true;
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
    }
}
