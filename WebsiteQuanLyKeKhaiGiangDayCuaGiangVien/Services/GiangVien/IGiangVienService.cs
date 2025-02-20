using System.Collections.Generic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.GiangVien
{
    public interface IGiangVienService
    {
        bool ThemGiangVienMoi(Models.GiangVien giangVien);
        string KiemTraEmailTonTai(string email);
        string KiemTraCapNhatEmailTonTai(string email, string maGV);
        string KiemTraSoDienThoaiTonTai(string sdt);
        string KiemTraSoDienThoaiCapNhatTonTai(string sdt, string maGV);

        bool KiemTraEmailHopLe(string email);
        bool KiemTraSoDienThoaiHopLe(string soDienThoai);
        bool KiemTraMaGiangVienTonTai(string maGV);

        (List<XemChiTietThongTinGiangVien> data, int soLuong) LoadDanhSachGiangVien(int page, int pageSize);
        (List<XemChiTietThongTinGiangVien> data, int soLuong) LoadDanhSachGiangVienDaKhoa(int page, int pageSize);

        XemChiTietThongTinGiangVien GetGiangVienTheoMa(string maGV);

        bool ThemDanhSachGiangVienMoi(List<Models.ModelCustom.ThongTinUpLoadGiangVien> danhSachGiangVien);
        bool CapNhatThongTinGiangVien(Models.GiangVien giangVien);

        bool XoaDanhSachGiangVien(List<string> ListIdGiangVien);
        bool MoKhoaDanhSachGiangVien(List<string> ListIdGiangVien);
        bool UpdateAvartar(string avartar, string maGV);

        List<XemChiTietThongTinGiangVien> DanhSachGiangVienDaKhoa();

        List<GiangVienTheoKhoa> GetDanhSachGiangVienTheoKhoa(string maKhoa);
        (List<XemChiTietThongTinGiangVien> data, int tongSoLuong) GetDanhSachGiangVienTheoChuoiTim(string chuoiTim, int page, int pageSize);
        (List<XemChiTietThongTinGiangVien> data, int tongSoLuong) GetDanhSachGiangVienTheoKhoa(string maKhoa, int page, int pageSize);
        (List<XemChiTietThongTinGiangVien> data, int tongSoLuong) GetDanhSachGiangVienTheoChuoiTimVaKhoa(string chuoiTim, string maKhoa, int page, int pageSize);

    }
}
