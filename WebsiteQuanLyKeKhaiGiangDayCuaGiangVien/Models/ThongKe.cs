//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ThongKe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThongKe()
        {
            this.ThongKeChiTiets = new HashSet<ThongKeChiTiet>();
        }
    
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThongKeChiTiet> ThongKeChiTiets { get; set; }
    }
}
