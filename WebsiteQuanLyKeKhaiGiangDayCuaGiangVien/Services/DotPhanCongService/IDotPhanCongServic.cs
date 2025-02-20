using System.Collections.Generic;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.DotPhanCongServic
{
    public interface IDotPhanCongServic
    {
        List<ChiTietDotPhanCong> GetDanhSachDotPhanCongTheoNamHoc(int maNamHoc);
        (int maNamHoc, int maDotPhanCong) GetDotPhanCongHienTai();
    }
}
