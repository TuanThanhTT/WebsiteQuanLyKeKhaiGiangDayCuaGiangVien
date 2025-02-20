using System.Collections.Generic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.AccountService
{
    public interface IAccountService
    {
        (List<ThongTinTaiKhoanGiangVien> data, int soLuong) LoadDanhSachTaiKhoanQuanLy(int page, int pageSize);
        int GetQuyenTruyCapCuaTaiKhoan(string maGV);
        bool ThayDoiQuyenTruyCap(string maGV, int maQuyenTruyCap);
        bool KiemTraQuyenCoTheSua(string maGV);
        (List<ThongTinTaiKhoanGiangVien> data, int soLuong) TimKiemTaiKhoan(string chuoiTim, int page, int pageSize);
        (List<ThongTinTaiKhoanGiangVien> data, int soLuong) TimKiemTaiKhoanTheoKhoa(string maKhoa, int page, int pageSize);
        (List<ThongTinTaiKhoanGiangVien> data, int soLuong) TimKiemTaiKhoanTheoKhoaVaChuoiTim(string maKhoa, string chuoiTim, int page, int pageSize);
    }
}
