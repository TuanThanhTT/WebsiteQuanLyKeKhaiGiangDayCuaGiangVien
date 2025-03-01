using System.Security.Cryptography;
using System.Text;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Services.MD5Service
{
    public class MD5Service : IMD5Service
    {
        public string GetMd5Hash(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Chuyển byte thành chuỗi hex
                }

                return sb.ToString();
            }
        }
    }
}