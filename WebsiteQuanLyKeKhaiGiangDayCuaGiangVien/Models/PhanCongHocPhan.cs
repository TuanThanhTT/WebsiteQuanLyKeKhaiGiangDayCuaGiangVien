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
    
    public partial class PhanCongHocPhan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhanCongHocPhan()
        {
            this.KeKhais = new HashSet<KeKhai>();
        }
    
        public int MaPhanCongHocPhan { get; set; }
        public string MaGV { get; set; }
        public string MaHP { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string LopPhanCong { get; set; }
        public Nullable<int> SiSo { get; set; }
        public Nullable<int> MaDotKeKhai { get; set; }
        public string TrangThai { get; set; }
        public string HinhThucDay { get; set; }
        public Nullable<int> MaFilePhanCong { get; set; }
        public string HocKy { get; set; }
        public string NamHoc { get; set; }
        public string ThoiGianDay { get; set; }
        public Nullable<int> MaDotPhanCong { get; set; }
    
        public virtual DotKeKhai DotKeKhai { get; set; }
        public virtual DotPhanCong DotPhanCong { get; set; }
        public virtual FilePhanCong FilePhanCong { get; set; }
        public virtual GiangVien GiangVien { get; set; }
        public virtual HocPhan HocPhan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KeKhai> KeKhais { get; set; }
    }
}
