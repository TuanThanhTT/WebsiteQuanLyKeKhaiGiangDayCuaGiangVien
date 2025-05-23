using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Services.LoginService
{
    public class LoginService: ILoginService
    {

        private readonly Cache _cache;
        private readonly WebsiteQuanLyKeKhaiGiangDayEntities1 _context;

        public LoginService(WebsiteQuanLyKeKhaiGiangDayEntities1 context)
        {
            _cache = HttpRuntime.Cache; // Use HttpRuntime.Cache for .NET Framework
            _context = context;
        }

        public string DatLaiMatKhau(string maGV)
        {
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            // Lưu token → maGV (để lấy mã giảng viên từ token)
            _cache.Insert(token, maGV, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);

            // Lưu maGV → token (để lấy token từ mã giảng viên)
            string key = $"maGV:{maGV}";
            _cache.Insert(key, token, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);

            return token;
        }

        public bool DoiMatKhauGiangVien(string newPass, string maGV)
        {
            try
            {
                var account = _context.TaiKhoans.FirstOrDefault(op => op.MaGV == maGV);
                if (account != null)
                {
                    Services.MD5Service.MD5Service mD5Service = new Services.MD5Service.MD5Service();   
                    string pass = mD5Service.GetMd5Hash(newPass);
                    account.Passwords = pass;
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public string GetMaGiangVienTuToken(string token)
        {
            string maGV = _cache.Get(token) as string;
            if (!string.IsNullOrEmpty(maGV))
            {
                return maGV;
            }
            return "";
        }

        public bool KiemTraEmail(string email, string maGV)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            try
            {
                var giangVien = _context.GiangViens.Find(maGV);
                if (giangVien != null)
                {
                    return giangVien.Email == email;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool KiemTraTaiKhoanTonTai(string maGV)
        {
            if (string.IsNullOrEmpty(maGV))
            {
                return false;
            }
            try
            {
                var giangVien = _context.GiangViens.Find(maGV);
                return giangVien != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public bool XacThucTokenDatLaiMatKhau(string token)
        {
            if (token == null)
            {
                return false;
            }
            var maGV = _cache.Get(token) as string;
            return maGV != null;
        }
    }
}