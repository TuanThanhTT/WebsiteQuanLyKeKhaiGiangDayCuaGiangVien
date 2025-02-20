using System;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom
{
    public class XemThongTinDotKeKhai
    {
        public int MaDotKeKhai { get; set; }
        public string TenDotKeKhai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string MaGv { get; set; }
        public bool KeKhaiMoi { get; set; }
        public string GhiChu { get; set; }
        public string HocKy { get; set; }
        public string NamHoc { get; set; }
    }
}
