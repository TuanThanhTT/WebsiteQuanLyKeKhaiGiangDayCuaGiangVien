using System.Collections.Generic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService
{
    public interface IReadFileExcel
    {
        List<ThongTinPhanCong> DocFilePhanCong(string filePath);
        List<ThongTinPhanCong> KiemTraHopLeThongTinPhanCong(List<ThongTinPhanCong> danhSachPhanCong);
        List<ThongTinPhanCong> KiemTraHopLeThongTinPhanCongBoSung(List<ThongTinPhanCong> danhSachPhanCong, int idDotPhanCong);

        List<ThongTinKhoa> DocFileThemKhoa(string filePath);
        List<ThongTinKhoa> KiemTraKhoaHopLe(List<ThongTinKhoa> danhSachKhoa);

        List<ThongTinUpLoadGiangVien> DocFileThemGIangVien(string filePath);
        List<ThongTinUpLoadGiangVien> KiemTraGiangVienHopLe(List<ThongTinUpLoadGiangVien> danhSachGiangVien);

        List<ThongTinUploadHocPhan> DocFileThemHocPhan(string filePath);
        List<ThongTinUploadHocPhan> KiemTraHocPhanHopLe(List<ThongTinUploadHocPhan> danhSachHocPhan);

    }
}
