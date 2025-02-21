using System.Collections.Generic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KeKhaiService
{
    public interface IKeKhaiHocPhan
    {
        (List<XemThongTinHocPhanDuocPhanCong>, int) getHocPhanDuocPhanCongChuaHoanThanhCuaGiangVien(string maGV, int page, int pageSize);
        XemThongTinHocPhanDuocPhanCong XemThongTinPhanCongHocPhanTheoMa(int maPhanCongHocPhan);
        bool HoanThanhKeKhai(int maPhanCongHocPhan, string maGV);
        List<WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom.Khoa> getDanhSachKhoa();
        bool CapNhatThongTinHocPhanPhanCong(int maHocPhanPhanCong, string tenLop, int soLuong, string hinhThucDay);
        (List<XemThongTinHocPhanDuocPhanCong>, int) getHocPhanDaKeKhaiChoDuyet(string maGV, int page, int pageSize);
        (List<XemThongTinKeKhaiDaDuyet>, int) getHocPhanDaDuyetTheoDot(string maGV, int maDotKeKhai, int page, int pageSize);
        (List<XemThongTinKeKhaiDaDuyet>, int, ThongTinDotKeKhaiGanNhat) getHocPhanDaDuyetTheoDotGanNhat(string maGV, int page, int pageSize);
        (List<XemThongTinKeKhaiDaDuyet>, string) getAllHocPhanDaDuyetTheoDot(string maGV, int maDotKeKhai);
        (List<XemThongTinKeKhaiDaDuyet>, string) getAllHocPhanDaDuyetTheoDotGanNhat(string maGV);
        (List<XemKeKhaiChoDuyetTheoGiangVien>, int) getKeKhaiGiangVien(int page, int pageSize);
        (List<XemKeKhaiChoDuyetTheoGiangVien>, int) getKeKhaiGiangVienTheoDot(int maDotKeKhai, int page, int pageSize);
        (List<XemKeKhaiChoDuyetTheoGiangVien>, int) getKeKhaiGiangVienTheoDotVaKhoa(string maKhoa, int maDotKeKhai, int page, int pageSize);
        (List<XemKeKhaiChoDuyetTheoGiangVien>, int) getKeKhaiGiangVienTheoKhoa(string maKhoa, int page, int pageSize);
        List<ChiTietKeKhaiChoDuyet> getDanhSachKeKhaiChoDuyetCuaGiangVien(string maGV);
        XemThongTinKeKhaiDaDuyet getThongTinKeKhaiDaDuyetTheoMa(int maKeKhai, string maGV);
        bool DuyetKeKhaiHoanThanh(int maKeKhai, string idNguoiDuyet);
        bool DuyetDanhSachKeKhaiHoanThanh(List<int> dsMaKeKhai, string idNguoiDuyet);
        (List<ThongTinPhanCongGiangVien>, int, int, string, string, string) getHocPhanPhanCongTheoDot(int maDotKeKhai, int page, int pageSize);
        (List<ThongTinPhanCongGiangVien>, int, int, string, string, string, ThongTinDotKeKhaiGanNhat) getHocPhanPhanCongTheoDotGanNhat(int page, int pageSize);
        (List<XemThongTinHocPhanDuocPhanCong>, string, string, string) getDanhSachHocPhanDuocPhanCongCuaGiangVienTheoDot(string maGV, int maDotKeKhai);
        (List<ThongTinPhanCongGiangVien>, int, int, string, string, string) getHocPhanPhanCongTheoDotVaKhoaGanNhat(string maKhoa, int page, int pageSize);
        (List<ThongTinPhanCongGiangVien>, int, int, string, string, string) getHocPhanPhanCongTheoDotVaKhoa(string maKhoa, int maDotKeKhai, int page, int pageSize);
        (List<HocPhanDaPhanCong>, int) GetDanhSachHocPhanDaPhanCongTheoDot(int maDotPhanCong, int page, int pageSize);
        (List<HocPhanDaPhanCong>, int) GetDanhSachHocPhanDaPhanCongTheoKhoa(string maKhoa, int maDotPhanCong, int page, int pageSize);
        XemHocPhanPhanCongCuaGiangVien ThongTinHocPhanPhanCongCuaGiangVienTheoMa(int maPhanCongHocPhan);
        bool CapNhatPhanCongHocPhanTheoMa(int maPhanCongHocPhan, string maGV, string tenLop, string namHoc, string hocKy, string thoiGianDay, string maHP, string hinhThucDay, int siSo);
        bool XoaDanhSachPhanCongHocPhan(List<int> danhSachMaPhanCong);
        (List<HocPhanDaPhanCong>, int) GetDanhSachHocPhanPhanCongTheoChuoiTim(string chuoiTim, int maDotPhanCong, int page, int pageSize);
        (List<HocPhanDaPhanCong>, int) GetDanhSachHocPhanPhanCongTheoChuoiTimVaKhoa(string chuoiTim, string maKhoa, int maDotPhanCong, int page, int pageSize);
        bool DuyetTatCaKeKhai(string maGV);

    }
}
