using System.Collections.Generic;
using System.Web;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService
{
    public interface IFileUpload
    {
        string UploadFilePhanCong(HttpPostedFileBase file);
        int SaveFilePhanCong(string fileName);
        string XuatFilePhanCongTheoGiangVien(List<XemThongTinHocPhanDuocPhanCong> dsHocPhanPhanCong);
        string XuatFileKeKhaiCuaGiangVienTheoDot(string maGV, List<XemThongTinKeKhaiDaDuyet> listKeKhai, string dotKeKhai);

        string UploadFileKhoa(HttpPostedFileBase file);
        string UploadFileGiangVien(HttpPostedFileBase file);
        string UploadFileHocPhan(HttpPostedFileBase file);

        string XuatFileThongKeTheoKhoa(List<ThongTinChiTietTienDo> danhSachThongKe);
        string XuatFileThongKe(List<ThongTinChiTietTienDo> danhSachThongKe);

        string XuatFileThongKeGiangVienChuaHoanThanhKeKhaiTheoDot(List<GiangVienHoanThanhKeKhai> danhSachThongKe);
        string XuatFileThongKeGiangVienHoanThanhKeKhaiTheoDot(List<GiangVienHoanThanhKeKhai> danhSachThongKe);

        string UploadAvatar(HttpPostedFileBase file);
        string XuatFileDanhSachGiangVienDaKhoa(List<XemChiTietThongTinGiangVien> danhSachGiangVien);

    }
}
