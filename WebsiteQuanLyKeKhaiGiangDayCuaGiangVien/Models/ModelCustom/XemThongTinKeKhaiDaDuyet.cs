using System;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom
{
    public class XemThongTinKeKhaiDaDuyet
    {
        public int Id { get; set; }
        public int maPhanCongHocPhan { get; set; }
        public string maHP { get; set; }
        public string tenHocPhan { set; get; }
        public int soLuong { set; get; }
        public string tenLop { set; get; }
        public string hinhThucDay { set; get; }
        public string ngayKeKhai { set; get; }
        public DateTime ngayDuyet { set; get; }
        public string nguoiDuyet { get; set; }
        public string hocKy { set; get; }
        public string namHoc { set; get; }
        public string ngayDay { set; get; }    



    }
}
