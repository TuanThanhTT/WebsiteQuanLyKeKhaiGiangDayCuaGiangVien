using System;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom
{
    public class ThongKeChiTiet
    {
        public int Id { get; set; }
        public int MaThongKe { get; set; }
        public string MaGV { get; set; }
        public string TenGV { get; set; }
        public string TenKhoa { get; set; }
        public Nullable<int> SoLuongPhanCong { get; set; }
        public Nullable<int> DaThuHien { get; set; }
    }
}