using System;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom
{
    public class HocPhan
    {
        public string MaHP { get; set; }
        public string TenHP { get; set; }
        public Nullable<int> SoTinChi { get; set; }
        public Nullable<int> LyThuyet { get; set; }
        public Nullable<int> ThucHanh { get; set; }
        public string MoTa { get; set; }
        public bool IsDelete { get; set; }
    }
}