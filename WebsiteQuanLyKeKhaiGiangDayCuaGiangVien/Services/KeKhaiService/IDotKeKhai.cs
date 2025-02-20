using System;
using System.Collections.Generic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.KeKhaiService
{
    public interface IDotKeKhai
    {

        bool TaoDotKeKhaiMoi(string tenDotKeKhai, DateTime ngayBatDau, DateTime ngayKetThuc, string maGV, int hocKy, int namHoc, bool loaiDotKeKhai, string ghiChu);
        bool KhoaDotKeKhaiHienTai(int maDotKeKhai);
        bool CapNhatThoiGianDotKeKhai(int maDotKeKhai, DateTime ngayKetThucMoi, string ghiChu);
        DotKeKhai GetDotKeKhaiDangMo();
        bool KiemTraDotKeKhaiDangMo();
        bool DotKeKhaiSapMo();
        bool TaoPhanCongHocPhan(List<ThongTinPhanCong> dsPhanCong, string tenPhanCong, int maNamHoc);
        bool TaoBoSungPhanCongHocPhan(List<ThongTinPhanCong> dsPhanCong, int idDotPhanCong);
        List<ThongTinPhanCong> LoadThongTinPhanCongTrong();
        List<ThongTinPhanCong> LoadThongTinPhanCongDotDangMo();
        bool TaoDotKeKhaiDangKyBoSung(string maGV, string tenDot, DateTime ngayBatDau, DateTime ngayKetThuc, int hocKy, int namHoc, string ghiChu, bool loaiDotKeKhai);
        ThongBaoDotKeKhai LayThongTinDotKeKhaiHienTai();
        List<DotKeKhai> LoadDotKeKhaiTheoHocKyNamHoc(int maNamHoc, int maHocKy);
        List<DotKeKhai> XemDanhSachDotKeKhaiTheoHocKyNamHoc(int maNamHoc, int maHocKy);
        List<XemThongTinHocPhanDuocPhanCong> LoadDanhSachHocPhanDuocPhanCongTheoDot(string maGV, int maDotKeKhai);
        Tuple<List<XemThongTinHocPhanDuocPhanCong>, ThongTinDotKeKhaiGanNhat> LoadDanhSachHocPhanDuocPhanCongTheoDotGanNhat(string maGV);
        ThongTinDotKeKhaiGanNhat DotKeKhaiGanNhat();
        string GetTenDotKeKhaiTheoMa(int maDotKeKhai);

    }
}
