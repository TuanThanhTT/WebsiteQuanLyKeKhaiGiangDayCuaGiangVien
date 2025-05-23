
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.AccountService
{
    public class AccountService : IAccountService
    {
        public int GetQuyenTruyCapCuaTaiKhoan(string maGV)
        {
            try
            {
                if (string.IsNullOrEmpty(maGV))
                {
                    return -1;
                }
                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Find(maGV);
                    if (giangVien != null)
                    {
                       
                        var quyenTruyCap = (from gv in context.GiangViens 
                                            join taiKhoan in context.TaiKhoans
                                            on gv.MaGV equals taiKhoan.MaGV
                                            join accRole in context.AccountRoles
                                            on taiKhoan.MaTaiKhoan equals accRole.AccountId
                                            join role in context.Roles
                                            on accRole.RoleId equals role.RoleId
                                            where gv.MaGV == maGV
                                            select role.RoleId).FirstOrDefault();
                        return quyenTruyCap;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return -1;
        }


        public bool KiemTraQuyenCoTheSua(string maGV)
        {
            try
            {
                if (string.IsNullOrEmpty(maGV)) return false;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Find(maGV);
                    if (giangVien == null) return false;

                    var taiKhoan = context.TaiKhoans
                                          .Where(op => op.MaGV == giangVien.MaGV)
                                          .FirstOrDefault();
                    if (taiKhoan == null) return false;

                    var accRole = context.AccountRoles
                                         .Where(op => op.AccountId == taiKhoan.MaTaiKhoan)
                                         .FirstOrDefault();
                    if (accRole == null) return false;

                    return accRole.RoleId == 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }


        public (List<ThongTinTaiKhoanGiangVien> data, int soLuong) LoadDanhSachTaiKhoanQuanLy(int page, int pageSize)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from giangVien in context.GiangViens
                                join khoa in context.Khoas on giangVien.MaKhoa equals khoa.MaKhoa
                                join taiKhoan in context.TaiKhoans on giangVien.MaGV equals taiKhoan.MaGV
                                join accper in context.AccountRoles on taiKhoan.MaTaiKhoan equals accper.AccountId
                                join role in context.Roles on accper.RoleId equals role.RoleId
                                select new ThongTinTaiKhoanGiangVien
                                {
                                    maGV = giangVien.MaGV,
                                    tenGV = giangVien.TenGV,
                                    tenKhoa = khoa.TenKhoa,
                                    loaiTaiKhoan = role.RoleName
                                };

                    var soLuong = query.Count();
                    var danhSach = query
    .OrderBy(gv => gv.maGV) // Sắp xếp theo trường nào đó, ví dụ maGV
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToList();
                   

                    return (danhSach, soLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
            return (new List<ThongTinTaiKhoanGiangVien>(), 100000);
        }

        public bool ThayDoiQuyenTruyCap(string maGV, int maQuyenTruyCap)
        {
            try
            {
                if (string.IsNullOrEmpty(maGV)) return false;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Find(maGV);
                    if (giangVien == null) return false;

                    var taiKhoan = context.TaiKhoans.FirstOrDefault(op => op.MaGV == giangVien.MaGV);
                    if (taiKhoan == null) return false;

                    var accRole = context.AccountRoles.FirstOrDefault(op => op.AccountId == taiKhoan.MaTaiKhoan);
                    if (accRole == null) return false;

                    accRole.RoleId = maQuyenTruyCap;
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
        public (List<ThongTinTaiKhoanGiangVien> data, int soLuong) TimKiemTaiKhoan(string chuoiTim, int page, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(chuoiTim)) return (new List<ThongTinTaiKhoanGiangVien>(), 0);

                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var query = from giangVien in context.GiangViens
                                join khoa in context.Khoas
                                on giangVien.MaKhoa equals khoa.MaKhoa
                                join taiKhoan in context.TaiKhoans
                                on giangVien.MaGV equals taiKhoan.MaGV
                                join accper in context.AccountRoles
                                on taiKhoan.MaTaiKhoan equals accper.AccountId
                                join role in context.Roles
                                on accper.RoleId equals role.RoleId
                                select new ThongTinTaiKhoanGiangVien
                                {
                                    maGV = giangVien.MaGV,
                                    tenGV = giangVien.TenGV,
                                    tenKhoa = khoa.TenKhoa,
                                    loaiTaiKhoan = role.RoleName
                                };

                    // Chuyển dữ liệu về List để xử lý chuỗi
                    var danhSach = query.ToList();

                    // Tìm kiếm theo tên giảng viên hoặc mã giảng viên (bỏ dấu)
                    var queryFilter = danhSach.Where(op =>
                        RemoveDiacritics(op.tenGV).ToUpper().Contains(RemoveDiacritics(chuoiTim).ToUpper()) ||
                        RemoveDiacritics(op.maGV).ToUpper().Contains(RemoveDiacritics(chuoiTim).ToUpper())
                    ).ToList();

                    var soLuong = queryFilter.Count();
                    var danhSachTim = queryFilter.OrderBy(op=>op.maGV).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    return (danhSachTim, soLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<ThongTinTaiKhoanGiangVien>(), 0);
        }

        public (List<ThongTinTaiKhoanGiangVien> data, int soLuong) TimKiemTaiKhoanTheoKhoa(string maKhoa, int page, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhoa))
                {
                    return (new List<ThongTinTaiKhoanGiangVien>(), 0);
                }

                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var khoa = context.Khoas.Find(maKhoa);
                    if (khoa != null)
                    {
                        var query = from giangVien in context.GiangViens
                                    join taiKhoan in context.TaiKhoans
                                    on giangVien.MaGV equals taiKhoan.MaGV
                                    join accper in context.AccountRoles
                                    on taiKhoan.MaTaiKhoan equals accper.AccountId
                                    join role in context.Roles
                                    on accper.RoleId equals role.RoleId
                                    where giangVien.MaKhoa == khoa.MaKhoa
                                    select new ThongTinTaiKhoanGiangVien
                                    {
                                        maGV = giangVien.MaGV,
                                        tenGV = giangVien.TenGV,
                                        tenKhoa = khoa.TenKhoa,
                                        loaiTaiKhoan = role.RoleName
                                    };

                        int soLuong = query.Count(); // Đếm số lượng kết quả
                        var danhsach = query.OrderBy(op=>op.maGV).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                        return (danhsach, soLuong);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<ThongTinTaiKhoanGiangVien>(), 0);
        }

        public (List<ThongTinTaiKhoanGiangVien> data, int soLuong) TimKiemTaiKhoanTheoKhoaVaChuoiTim(string maKhoa, string chuoiTim, int page, int pageSize)
        {
            try
            {
                Console.WriteLine("Mã khoa và chuỗi tìm là: " + maKhoa + " - " + chuoiTim);

                if (string.IsNullOrEmpty(chuoiTim) || string.IsNullOrEmpty(maKhoa))
                    return (new List<ThongTinTaiKhoanGiangVien>(), 0);

                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var khoaTim = context.Khoas.Find(maKhoa);
                    if (khoaTim == null) return (new List<ThongTinTaiKhoanGiangVien>(), 0);

                    var query = from giangVien in context.GiangViens
                                join khoa in context.Khoas
                                on giangVien.MaKhoa equals khoa.MaKhoa
                                join taiKhoan in context.TaiKhoans
                                on giangVien.MaGV equals taiKhoan.MaGV
                                join accper in context.AccountRoles
                                on taiKhoan.MaTaiKhoan equals accper.AccountId
                                join role in context.Roles
                                on accper.RoleId equals role.RoleId
                                where giangVien.MaKhoa == maKhoa
                                select new ThongTinTaiKhoanGiangVien
                                {
                                    maGV = giangVien.MaGV,
                                    tenGV = giangVien.TenGV,
                                    tenKhoa = khoa.TenKhoa,
                                    loaiTaiKhoan = role.RoleName
                                };

                    var danhSach = query.ToList(); // Lấy danh sách từ database
                    var queryFilter = danhSach.Where(op =>
                        RemoveDiacritics(op.tenGV).ToUpper().Contains(RemoveDiacritics(chuoiTim).ToUpper()) ||
                        RemoveDiacritics(op.maGV).ToUpper().Contains(RemoveDiacritics(chuoiTim).ToUpper()))
                        .ToList();

                    int soLuong = queryFilter.Count();
                    var danhSachTim = queryFilter.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    return (danhSachTim, soLuong);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return (new List<ThongTinTaiKhoanGiangVien>(), 0);
        }

    }
}
