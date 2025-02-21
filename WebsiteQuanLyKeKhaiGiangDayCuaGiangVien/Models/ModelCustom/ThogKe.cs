using System;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom
{
    public class ThogKe
    {
        public int ID { get; set; }
        public int MaDotKeKhai { get; set; }
        public Nullable<int> SoLuongGiangVienPhanCong { get; set; }
        public Nullable<int> SoLuongGiangVienDaHoanThanh { get; set; }
        public Nullable<int> SoLuongGiangVienChuaHoanThanh { get; set; }
        public Nullable<int> SoLuongHocPhan { get; set; }
        public Nullable<int> SoLuongHocPhanDaDuocKeKhai { get; set; }
        public Nullable<int> SoLuongHocPhanCacDotTruoc { get; set; }
        public int SoLuongPhanCongChoDuyet { get; set; }
        public System.DateTime NgayTao { get; set; }
    }
}