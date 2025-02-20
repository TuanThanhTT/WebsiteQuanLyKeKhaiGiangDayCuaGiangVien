using System.Collections.Generic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.HocPhanService
{
    public interface IHocPhanService
    {
        (List<ThongTinHocPhan> data, int tongSoLuong) LoadDanhSachHocPhan(int page, int pageSize);
        bool KiemTraMaHocPhanChuaTonTai(string maHocPhan);
        bool ThemHocPhanMoi(string maHocPhan, string tenHocPhan, int soTinChi, int lyThuyet, int thucHanh, string ghiChu);
        HocPhan GetHocPhanTheoMa(string maHocPhan);
        bool ThemDanhSachHocPhanMoi(List<ThongTinUploadHocPhan> hocPhans);
        bool CapNhatThongTinHocPhan(string maHocPhan, string tenHocPhan, int soTinChi, int lyThuyet, int thucHanh, string ghiChu);
        bool XoaDanhSachHocPhan(List<string> danhSachHocPhans);
        List<XemHocPhan> GetDanhSachHocPhan();
        int ThemPhanCongHocPhanMoi(int maDotPhanCong, string maGV, string maHP, string namHoc, string hocKy, string ngayDay, string hinhThuc, string tenLop, int siSo);
    }
}
