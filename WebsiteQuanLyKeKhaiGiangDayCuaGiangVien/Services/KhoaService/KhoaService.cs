
using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KhoaService
{
    public class KhoaService : IKhoaService
    {
        public bool CapNhatKhoa(string tenKhoa, string maKhoa)
        {
            try
            {
                if (string.IsNullOrEmpty(tenKhoa)) return false;
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var khoa = context.Khoas.Find(maKhoa); // Sử dụng Find() thay vì FindAsync()
                    if (khoa != null)
                    {
                        khoa.TenKhoa = tenKhoa;
                        context.SaveChanges(); // Gọi SaveChanges() đồng bộ
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

        public List<Khoa> GetDanhSachKhoa(int page, int pageSize, out int tongSoLuong)
        {
            tongSoLuong = 0;
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    tongSoLuong = context.Khoas.Count(op => !op.IsDelete); // Đồng bộ, không dùng CountAsync()
                    var danhSachKhoa = context.Khoas
                        .Where(op => !op.IsDelete)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList(); // Đồng bộ, không dùng ToListAsync()
                    return danhSachKhoa;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<Khoa>();
        }

        public List<Khoa> GetDanhSachKhoa()
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var danhSachKhoa = context.Khoas.ToList(); // Đồng bộ, không dùng ToListAsync()
                    if (danhSachKhoa.Any())
                    {
                        return danhSachKhoa;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<Khoa>();
        }


        public Khoa GetKhoaTheoMa(string maKhoa)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhoa)) return new Khoa();

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var khoa = context.Khoas.Find(maKhoa); // Đồng bộ, không dùng FindAsync()
                    if (khoa != null)
                    {
                        return khoa;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new Khoa();
        }

        public bool ThemDanhSachKhoaMoi(List<ThongTinKhoa> danhSachKhoa)
        {
            try
            {
                if (danhSachKhoa == null || danhSachKhoa.Count <= 0) return false;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    foreach (var item in danhSachKhoa)
                    {
                        if (!string.IsNullOrEmpty(item.tenKhoa))
                        {
                            int id = context.Khoas.Count() + 1; // Đồng bộ, không dùng CountAsync()
                            while (context.Khoas.Find(id.ToString()) != null) // Đồng bộ, không dùng FindAsync()
                            {
                                id++;
                            }

                            var khoa = new Khoa
                            {
                                MaKhoa = id.ToString(),
                                TenKhoa = item.tenKhoa,
                                IsDelete = false,
                            };
                            context.Khoas.Add(khoa);
                        }
                    }
                    context.SaveChanges(); // Đồng bộ, không dùng SaveChangesAsync()
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi lưu khoa: " + ex);
            }
            return false;
        }

        public bool ThemKhoaMoi(string tenKhoa)
        {
            try
            {
                if (string.IsNullOrEmpty(tenKhoa))
                {
                    return false;
                }
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    int id = context.Khoas.Count(); // Đồng bộ, không dùng CountAsync()
                    while (context.Khoas.Find(id.ToString()) != null) // Đồng bộ, không dùng FindAsync()
                    {
                        id++;
                    }
                    var khoa = new Khoa
                    {
                        MaKhoa = id.ToString(),
                        TenKhoa = tenKhoa,
                        IsDelete = false,
                    };
                    context.Khoas.Add(khoa);
                    context.SaveChanges(); // Đồng bộ, không dùng SaveChangesAsync()
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        public bool XoaDanhSachKhoa(List<string> idKhoas)
        {
            try
            {
                if (idKhoas == null || !idKhoas.Any())
                {
                    return false;
                }

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    // Lấy danh sách các khoa tương ứng với idKhoas
                    var khoas = context.Khoas
                                       .Where(k => idKhoas.Contains(k.MaKhoa) && !k.IsDelete)
                                       .ToList(); // Đồng bộ, không dùng ToListAsync()

                    if (khoas.Count != idKhoas.Count)
                    {
                        return false;
                    }

                    // Đánh dấu IsDelete = true
                    foreach (var khoa in khoas)
                    {
                        khoa.IsDelete = true;
                    }

                    // Lưu thay đổi
                    context.SaveChanges(); // Đồng bộ, không dùng SaveChangesAsync()
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
