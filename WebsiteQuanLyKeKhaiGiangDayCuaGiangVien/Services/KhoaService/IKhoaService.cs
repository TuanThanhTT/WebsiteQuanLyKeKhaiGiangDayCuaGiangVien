using System.Collections.Generic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KhoaService
{
    public interface IKhoaService
    {
        bool ThemKhoaMoi(string tenKhoa);
        bool CapNhatKhoa(string tenKhoa, string maKhoa);
        WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom.Khoa GetKhoaTheoMa(string maKhoa);
        List<WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom.Khoa> GetDanhSachKhoa(int page, int pageSize, out int tongSoLuong);

        bool ThemDanhSachKhoaMoi(List<ThongTinKhoa> danhSachKhoa);
        bool XoaDanhSachKhoa(List<string> idKhoas);
        List<WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom.Khoa> GetDanhSachKhoa();
    }
}
