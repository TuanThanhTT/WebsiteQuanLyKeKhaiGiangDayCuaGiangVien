namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom
{
    public class ThongTinPhanCong
    {
        public int id { get; set; } 
        public string maGV { get; set; }
        public string tenGV { get; set; }   
        public string maHP { get; set; }    
        public string tenHP { get; set; }   
        public string tenLop { get; set; }
        public int siSo { get; set; }
        public string hocKy { get; set; }   
        public string namHoc { get; set; }  
        public string thoiGianDay { get; set; } 
        public string hinhThucDay { get; set; } 
        public string ghiChu { get; set; }  
        public bool isPhanCong { get; set; }    
      
        public bool isTrungPhanCong(ThongTinPhanCong B)
        {
            if(this.maGV == B.maGV && this.maHP == B.maHP && this.hocKy == B.hocKy && this.namHoc == B.namHoc && this.tenLop == B.tenLop
                && this.siSo == B.siSo && this.thoiGianDay == B.thoiGianDay && this.hinhThucDay == B.hinhThucDay)
            {
                return true;
            }
            return false;
        }
    }
}
