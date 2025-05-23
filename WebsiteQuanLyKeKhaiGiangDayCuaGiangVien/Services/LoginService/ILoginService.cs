using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Services.LoginService
{
    internal interface ILoginService
    {
        bool KiemTraTaiKhoanTonTai(string maGV);
        string DatLaiMatKhau(string maGV);
        bool KiemTraEmail(string email, string maGV);
        bool XacThucTokenDatLaiMatKhau(string token);
        string GetMaGiangVienTuToken(string token);
        bool DoiMatKhauGiangVien(string newPass, string maGV);
    }
}
