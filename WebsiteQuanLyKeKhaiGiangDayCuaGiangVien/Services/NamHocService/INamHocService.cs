using System.Collections.Generic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.NamHocService
{
    public interface INamHocService
    {
        int GetSoLuongNamHoc();
        List<NamHoc> GetDanhSachNamHocTheoTrang(int page, int pageSize);
        bool ThemNamHoc(string tenNamHoc);
        bool ThemHocKy(string tenHocKy, int maNamHoc);
        bool KhoaNamHoc(int maNamHoc);
        bool CapNhatNamHoc(int maNamHoc, string tenNamHoc);
        List<NamHoc> GetTop5NamHocGanNhat();
        List<HocKy> GetDanhSachHocKyTheoNamHoc(int maNamHoc);
        bool ThemHocKyMoiVaoNamHoc(int maNamHoc, string tenHocKy);
        bool ChinhSuaThongTinNamHoc(int maNamHoc, string tenNamHocMoi);
        bool XoaNamHoc(int maNamHoc);
    }
}
