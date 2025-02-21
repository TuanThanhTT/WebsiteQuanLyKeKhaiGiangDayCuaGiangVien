using System;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom
{
    public class DotKeKhai
    {
        public int MaDotKeKhai { get; set; }
        public string TenDotKeKhai { get; set; }
        public Nullable<System.DateTime> NgayBatDau { get; set; }
        public Nullable<System.DateTime> NgayKetThuc { get; set; }
        public string MaGV { get; set; }
        public Nullable<bool> KeKhaiMoi { get; set; }
        public string GhiChu { get; set; }
        public int MaHocKy { get; set; }
        public int MaNamHoc { get; set; }

    }
}