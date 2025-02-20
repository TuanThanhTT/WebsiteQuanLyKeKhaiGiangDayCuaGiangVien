using System.Collections.Generic;
namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.ThongKe
{
    public interface IThongKeService
    {
        Models.ThongKe ThongKeTheoDotKeKhai(int maDotKeKhai);
        bool ThongKeTienDoTheoDotThongKe(int maThongKe);
        (List<Models.ThongKeChiTiet> data, int soLuong) LoadThongTinThongKeTienDo(int maDotKeKhai, int page, int pageSize);

        WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ThongKe ThongKeMoiTheoDotKeKhai(int maDotKeKhai);
        bool ThongKeTienDoMoiTheoDotThongKe(int maThongKe);
        (Models.ModelCustom.ThongTinChiTietTienDo thongTinTienDo, List<Models.ModelCustom.DanhSachMonHocThogKeTienDo> data) XemChiTietTienDo(string maGV, int maDotKeKhai);
        List<Models.ModelCustom.ThongTinChiTietTienDo> ThongKeTienDoTheoKhoa(string maKhoa, int maDotKeKhai);
        List<Models.ModelCustom.ThongTinChiTietTienDo> ThongKeTienDoTatCaKhoa(int maDotKeKhai);

        (List<Models.ModelCustom.GiangVienHoanThanhKeKhai> data, int tongSoLuong) ThongKeGiangVienHoanThanhKeKhaiTheoDot(int maDOtKeKhai, int page, int pageSize);
        (List<Models.ModelCustom.GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemThongKeGiangVienHoanThanhKeKhaiTheoDot(int maDOtKeKhai, string chuoiTim, int page, int pageSize);
        (List<Models.ModelCustom.GiangVienHoanThanhKeKhai> data, int tongSoLuong) ThongKeGiangVienChuaHoanThanhKeKhaiTheoDot(int maDOtKeKhai, int page, int pageSize);
        (List<Models.ModelCustom.GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienHoanThanhKeKhaiTheoKhoa(string maKhoa, int maDotKeKhai, int page, int pageSize);
        (List<Models.ModelCustom.GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienHoanThanhKeKhaiTheoKhoaVaChuoiTim(string maKhoa, string chuoiTim, int maDotKeKhai, int page, int pageSize);

        List<Models.ModelCustom.GiangVienHoanThanhKeKhai> DanhSachGiangVienHoanThanh(int maDOtKeKhai);
        List<Models.ModelCustom.GiangVienHoanThanhKeKhai> DanhSachGiangVienChuaHoanThanh(int maDOtKeKhai);
        (List<Models.ModelCustom.GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienChuaHoanThanhKeKhai(string chuoiTim, int maDotKeKhai, int page, int pageSize);
        (List<Models.ModelCustom.GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienChuaHoanThanhKeKhaiTheoKhoa(string maKhoa, int maDotKeKhai, int page, int pageSize);
        (List<Models.ModelCustom.GiangVienHoanThanhKeKhai> data, int tongSoLuong) TimKiemGiangVienChuaHoanThanhKeKhaiTheoKhoaVaChuoiTim(string maKhoa, string chuoiTim, int maDotKeKhai, int page, int pageSize);
    }
}
