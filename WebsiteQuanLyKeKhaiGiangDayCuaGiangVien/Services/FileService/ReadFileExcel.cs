
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService
{
    public class ReadFileExcel : IReadFileExcel
    {
        public List<ThongTinPhanCong> DocFilePhanCong(string filePath)
        {
            var danhSachPhanCong = new List<ThongTinPhanCong>();
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        Console.WriteLine("Số lượng dòng trong file là: " + rowCount);
                        int index = 1;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var thongTinPhanCong = new ThongTinPhanCong();
                            thongTinPhanCong.id = index;

                            var maGV = worksheet.Cells[row, 1].Value;
                            thongTinPhanCong.maGV = (maGV != null) ? maGV.ToString() : "";

                            var tenGV = worksheet.Cells[row, 2].Value;
                            thongTinPhanCong.tenGV = (tenGV != null) ? tenGV.ToString() : "";

                            var maHP = worksheet.Cells[row, 3].Value;
                            thongTinPhanCong.maHP = (maHP != null) ? maHP.ToString() : "";

                            var tenHP = worksheet.Cells[row, 4].Value;
                            thongTinPhanCong.tenHP = (tenHP != null) ? tenHP.ToString() : "";

                            var tenLop = worksheet.Cells[row, 5].Value;
                            thongTinPhanCong.tenLop = (tenLop != null) ? tenLop.ToString() : "";

                            int siso;
                            if (worksheet.Cells[row, 6].Value != null && int.TryParse(worksheet.Cells[row, 6].Value.ToString(), out siso))
                            {
                                thongTinPhanCong.siSo = siso;
                            }
                            else
                            {
                                thongTinPhanCong.siSo = -1000;
                            }

                            var thoiGianDay = worksheet.Cells[row, 7].Value;
                            thongTinPhanCong.thoiGianDay = (thoiGianDay != null) ? thoiGianDay.ToString() : "";

                            var hocKy = worksheet.Cells[row, 8].Value;
                            thongTinPhanCong.hocKy = (hocKy != null) ? hocKy.ToString() : "";

                            var namHoc = worksheet.Cells[row, 9].Value;
                            thongTinPhanCong.namHoc = (namHoc != null) ? namHoc.ToString() : "";

                            var hinhThucDay = worksheet.Cells[row, 10].Value;
                            thongTinPhanCong.hinhThucDay = (hinhThucDay != null) ? hinhThucDay.ToString() : "";

                            thongTinPhanCong.isPhanCong = false;
                            danhSachPhanCong.Add(thongTinPhanCong);
                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đọc file phân công: " + ex.Message);
            }
            return danhSachPhanCong;
        }


        public List<ThongTinUpLoadGiangVien> DocFileThemGIangVien(string filePath)
        {
            var danhSachGiangVien = new List<ThongTinUpLoadGiangVien>();
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        Console.WriteLine("Số lượng hàng: " + rowCount);
                        int index = 1;

                        for (int row = 3; row <= rowCount; row++)
                        {
                            ThongTinUpLoadGiangVien thongTinGiangVien = new ThongTinUpLoadGiangVien();
                            thongTinGiangVien.id = index;

                            var maGV = worksheet.Cells[row, 1].Value;
                            thongTinGiangVien.maGV = (maGV != null) ? maGV.ToString() : "";

                            var tenGV = worksheet.Cells[row, 2].Value;
                            thongTinGiangVien.tenGV = (tenGV != null) ? tenGV.ToString() : "";

                            var gioiTinh = worksheet.Cells[row, 3].Value;
                            thongTinGiangVien.gioiTinh = (gioiTinh != null) ? gioiTinh.ToString() : "";

                            var maKhoa = worksheet.Cells[row, 4].Value;
                            thongTinGiangVien.maKhoa = (maKhoa != null) ? maKhoa.ToString() : "";

                            var chucVu = worksheet.Cells[row, 5].Value;
                            thongTinGiangVien.chucVu = (chucVu != null) ? chucVu.ToString() : "";

                            var email = worksheet.Cells[row, 6].Value;
                            thongTinGiangVien.email = (email != null) ? email.ToString() : "";

                            var dienThoai = worksheet.Cells[row, 7].Value;
                            thongTinGiangVien.soDienThoai = (dienThoai != null) ? dienThoai.ToString() : "";

                            var diaChi = worksheet.Cells[row, 8].Value;
                            thongTinGiangVien.diaChi = (diaChi != null) ? diaChi.ToString() : "";

                            var ngaySinh = worksheet.Cells[row, 9].Text.Trim();
                            thongTinGiangVien.ngaySinh = ngaySinh;

                            var chuyenNganh = worksheet.Cells[row, 10].Value;
                            thongTinGiangVien.chuyenNghanh = (chuyenNganh != null) ? chuyenNganh.ToString() : "";

                            var trinhDo = worksheet.Cells[row, 11].Value;
                            thongTinGiangVien.trinhDo = (trinhDo != null) ? trinhDo.ToString() : "";

                            var loaiHinhDaoTao = worksheet.Cells[row, 12].Value;
                            thongTinGiangVien.loaiHinhDaoTao = (loaiHinhDaoTao != null) ? loaiHinhDaoTao.ToString() : "";

                            var heSoLuong = worksheet.Cells[row, 13].Value;
                            double heSo;
                            if (heSoLuong != null && double.TryParse(heSoLuong.ToString(), out heSo))
                            {
                                thongTinGiangVien.heSoLuong = heSo;
                            }
                            else
                            {
                                thongTinGiangVien.heSoLuong = -1;
                            }

                            danhSachGiangVien.Add(thongTinGiangVien);
                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đọc file giảng viên: " + ex.Message);
            }
            return danhSachGiangVien;
        }

        public List<ThongTinKhoa> DocFileThemKhoa(string filePath)
        {
            var danhKhoa = new List<ThongTinKhoa>();
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        Console.WriteLine("Số lượng dòng là: " + rowCount);
                        int index = 1;

                        for (int row = 3; row <= rowCount; row++)
                        {
                            ThongTinKhoa thongTinKhoa = new ThongTinKhoa();
                            thongTinKhoa.id = index;

                            var tenKhoa = worksheet.Cells[row, 1].Value;
                            thongTinKhoa.tenKhoa = (tenKhoa != null) ? tenKhoa.ToString() : "";

                            danhKhoa.Add(thongTinKhoa);
                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đọc file khoa: " + ex.Message);
            }
            return danhKhoa;
        }

        public List<ThongTinUpLoadGiangVien> KiemTraGiangVienHopLe(List<ThongTinUpLoadGiangVien> danhSachGiangVien)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    for (int i = 0; i < danhSachGiangVien.Count; i++)
                    {
                        Console.WriteLine("Giảng viên thứ: " + i + " - " + danhSachGiangVien[i].maGV);
                        string errMess = "";

                        // Kiểm tra mã giảng viên hợp lệ
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].maGV))
                        {
                            errMess += "Mã giảng viên rỗng";
                        }
                        else
                        {
                            if (context.GiangViens.Find(danhSachGiangVien[i].maGV) != null)
                            {
                                if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                                errMess += "Mã giảng viên đã tồn tại";
                            }
                        }

                        // Kiểm tra tên giảng viên
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].tenGV))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Tên giảng viên rỗng";
                        }

                        // Kiểm tra giới tính
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].gioiTinh) ||
                            !(danhSachGiangVien[i].gioiTinh.Equals("NAM", StringComparison.OrdinalIgnoreCase) ||
                              danhSachGiangVien[i].gioiTinh.Equals("NỮ", StringComparison.OrdinalIgnoreCase)))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Giới tính không hợp lệ";
                        }

                        // Kiểm tra mã khoa
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].maKhoa) ||
                            context.Khoas.Find(danhSachGiangVien[i].maKhoa) == null)
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Mã khoa không tồn tại hoặc rỗng";
                        }

                        // Kiểm tra chức vụ
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].chucVu))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Chức vụ rỗng";
                        }

                        // Kiểm tra email
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].email) || !kiemTraEmailHopLe(danhSachGiangVien[i].email))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Email không hợp lệ hoặc rỗng";
                        }
                        else if (context.GiangViens.Any(op => op.Email == danhSachGiangVien[i].email))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Email đã được sử dụng";
                        }

                        // Kiểm tra số điện thoại
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].soDienThoai) || !KiemTraSoDienThoaiHopLe(danhSachGiangVien[i].soDienThoai))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Số điện thoại không hợp lệ hoặc rỗng";
                        }
                        else if (context.GiangViens.Any(op => op.SoDienThoai == danhSachGiangVien[i].soDienThoai))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Số điện thoại đã được sử dụng";
                        }

                        // Kiểm tra địa chỉ
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].diaChi))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Địa chỉ rỗng";
                        }

                        // Kiểm tra ngày sinh
                        if (!DateTime.TryParseExact(danhSachGiangVien[i].ngaySinh, new[] { "d/MM/yyyy", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy" },
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ngaySinh) || (DateTime.Now.Year - ngaySinh.Year) < 24)
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Ngày sinh không hợp lệ hoặc dưới 24 tuổi";
                        }

                        // Kiểm tra chuyên ngành
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].chuyenNghanh))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Chuyên ngành rỗng";
                        }

                        // Kiểm tra trình độ
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].trinhDo))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Trình độ rỗng";
                        }

                        // Kiểm tra loại hình đào tạo
                        if (string.IsNullOrEmpty(danhSachGiangVien[i].loaiHinhDaoTao))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Loại hình đào tạo rỗng";
                        }

                        // Kiểm tra hệ số lương
                        if (danhSachGiangVien[i].heSoLuong <= 0)
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Hệ số lương không hợp lệ";
                        }

                        // Kiểm tra dữ liệu bị trùng trong danh sách
                        for (int j = 0; j < danhSachGiangVien.Count; j++)
                        {
                            if (i != j && EqualsGiangVien(danhSachGiangVien[i], danhSachGiangVien[j]))
                            {
                                if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                                errMess += "Thông tin giảng viên bị trùng trong file";
                                break;
                            }
                        }

                        danhSachGiangVien[i].ghiChu = errMess;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi kiểm tra giảng viên: " + ex.Message);
            }

            return danhSachGiangVien;
        }

        public bool EqualsGiangVien(ThongTinUpLoadGiangVien gv, ThongTinUpLoadGiangVien giangVien)
        {
            if (gv.maGV == giangVien.maGV)
            {
                Console.WriteLine("trung ma: " + gv.maGV + " - " + giangVien.maGV);
                return true;
            }
            if (giangVien.email == gv.email)
            {
                Console.WriteLine("Trung email");
                return true;
            }
            if (gv.soDienThoai == giangVien.soDienThoai)
            {
                Console.WriteLine("trung so dien thoai: "+gv.soDienThoai+" - "+giangVien.soDienThoai);
                return true;
            }
            return false;
        }

        public bool kiemTraEmailHopLe(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
        public bool KiemTraSoDienThoaiHopLe(string soDienThoai)
        {
            if (string.IsNullOrEmpty(soDienThoai)) return false;
            if (soDienThoai.Length != 10) return false;
            if (!soDienThoai.StartsWith("0")) return false;
            if (!soDienThoai.All(char.IsDigit)) return false; // Kiểm tra toàn bộ ký tự là số

            return true;
        }

        public List<ThongTinPhanCong> KiemTraHopLeThongTinPhanCong(List<ThongTinPhanCong> danhSachPhanCong)
        {
            try
            {
                for (int i = 0; i < danhSachPhanCong.Count; i++)
                {
                    if (danhSachPhanCong[i].isPhanCong)
                    {
                        continue;
                    }
                    string errMess = "";

                    // Kiểm tra mã giảng viên và mã học phần
                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].maGV))
                        {
                            errMess += "Mã giảng viên trống";
                        }
                        else
                        {
                            var giangVien = context.GiangViens.Find(danhSachPhanCong[i].maGV);
                            if (giangVien == null)
                            {
                                errMess += "Mã giảng viên không tồn tại";
                            }
                        }

                        if (string.IsNullOrEmpty(danhSachPhanCong[i].maHP))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Mã học phần không được bỏ trống";
                        }
                        else
                        {
                            var hocPhan = context.HocPhans.Find(danhSachPhanCong[i].maHP);
                            if (hocPhan == null)
                            {
                                if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                                errMess += "Mã học phần không hợp lệ";
                            }
                        }
                    }

                    // Kiểm tra các trường khác không được null
                    if (string.IsNullOrEmpty(danhSachPhanCong[i].tenGV))
                    {
                        if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                        errMess += "Tên giảng viên không được trống";
                    }
                    if (string.IsNullOrEmpty(danhSachPhanCong[i].tenHP))
                    {
                        if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                        errMess += "Tên học phần không được trống";
                    }
                    if (string.IsNullOrEmpty(danhSachPhanCong[i].tenLop))
                    {
                        if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                        errMess += "Tên lớp không được trống";
                    }
                    if (string.IsNullOrEmpty(danhSachPhanCong[i].thoiGianDay))
                    {
                        if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                        errMess += "Thời gian dạy không được trống";
                    }
                    if (string.IsNullOrEmpty(danhSachPhanCong[i].hocKy))
                    {
                        if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                        errMess += "Học kỳ không được trống";
                    }
                    if (string.IsNullOrEmpty(danhSachPhanCong[i].namHoc))
                    {
                        if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                        errMess += "Năm học không được trống";
                    }
                    if (string.IsNullOrEmpty(danhSachPhanCong[i].hinhThucDay))
                    {
                        if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                        errMess += "Hình thức dạy không được trống";
                    }

                    // Kiểm tra sĩ số
                    if (danhSachPhanCong[i].siSo <= 0)
                    {
                        if (danhSachPhanCong[i].siSo == -1000)
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Sĩ số phải là số nguyên lớn hơn 0";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Sĩ số phải là số nguyên lớn hơn 0";
                        }
                    }

                    // Kiểm tra trùng dữ liệu phân công
                    for (int j = 0; j < danhSachPhanCong.Count; j++)
                    {
                        if (i != j && danhSachPhanCong[i].isTrungPhanCong(danhSachPhanCong[j]))
                        {
                            if (!string.IsNullOrEmpty(errMess)) errMess += ", ";
                            errMess += "Trùng phân công học phần";
                            break;
                        }
                    }

                    danhSachPhanCong[i].ghiChu += errMess;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return danhSachPhanCong;
        }

        public List<ThongTinKhoa> KiemTraKhoaHopLe(List<ThongTinKhoa> danhSachKhoa)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    for (int i = 0; i < danhSachKhoa.Count; i++)
                    {
                        if (string.IsNullOrEmpty(danhSachKhoa[i].tenKhoa))
                        {
                            danhSachKhoa[i].ghiChu = "Tên khoa không được bỏ trống.";
                            continue;
                        }

                        if (context.Khoas.FirstOrDefault(op => op.TenKhoa == danhSachKhoa[i].tenKhoa) != null)
                        {
                            danhSachKhoa[i].ghiChu = "Tên khoa đã tồn tại";
                        }

                        // Kiểm tra dữ liệu bị trùng trong danh sách
                        for (int j = 0; j < danhSachKhoa.Count; j++)
                        {
                            if (i == j) continue; // Không so sánh với chính nó

                            if (danhSachKhoa[i].tenKhoa == danhSachKhoa[j].tenKhoa)
                            {
                                if (string.IsNullOrEmpty(danhSachKhoa[i].ghiChu))
                                {
                                    danhSachKhoa[i].ghiChu = "Dữ liệu nhập vào trùng lặp";
                                }
                                else
                                {
                                    danhSachKhoa[i].ghiChu += ", dữ liệu nhập vào trùng lặp";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return danhSachKhoa;
        }

        public List<ThongTinUploadHocPhan> DocFileThemHocPhan(string filePath)
        {
            var danhSachHocPhan = new List<ThongTinUploadHocPhan>();

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        int index = 1;

                        for (int row = 3; row <= rowCount; row++)
                        {
                            ThongTinUploadHocPhan thongTinHocPhan = new ThongTinUploadHocPhan
                            {
                                id = index,
                                maHP = worksheet.Cells[row, 1].Value?.ToString() ?? "",
                                tenHP = worksheet.Cells[row, 2].Value?.ToString() ?? ""
                            };

                            // Xử lý số tín chỉ
                            if (!int.TryParse(worksheet.Cells[row, 3].Text, out int tinChi))
                            {
                                tinChi = 0;
                            }
                            thongTinHocPhan.soTinChi = tinChi;

                            // Xử lý lý thuyết
                            if (!int.TryParse(worksheet.Cells[row, 4].Text, out int chiLyThuyet))
                            {
                                chiLyThuyet = 0;
                            }
                            thongTinHocPhan.lyThuyet = chiLyThuyet;

                            // Xử lý thực hành
                            if (!int.TryParse(worksheet.Cells[row, 5].Text, out int chiThucHanh))
                            {
                                chiThucHanh = 0;
                            }
                            thongTinHocPhan.thucHanh = chiThucHanh;

                            thongTinHocPhan.ghiChu = worksheet.Cells[row, 6].Value?.ToString() ?? "";

                            index++;
                            danhSachHocPhan.Add(thongTinHocPhan);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return danhSachHocPhan;
        }

        public List<ThongTinUploadHocPhan> KiemTraHocPhanHopLe(List<ThongTinUploadHocPhan> danhSachHocPhan)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    for (int i = 0; i < danhSachHocPhan.Count; i++)
                    {
                        string errMess = "";

                        // Kiểm tra mã học phần
                        if (string.IsNullOrEmpty(danhSachHocPhan[i].maHP))
                        {
                            errMess += "Mã học phần không được bỏ trống.";
                        }
                        else
                        {
                            var hocPhan = context.HocPhans.Find(danhSachHocPhan[i].maHP);
                            if (hocPhan != null)
                            {
                                if (!string.IsNullOrEmpty(errMess))
                                {
                                    errMess += ", ";
                                }
                                errMess += "Mã học phần này đã tồn tại";
                            }
                        }

                        // Kiểm tra tên học phần
                        if (string.IsNullOrEmpty(danhSachHocPhan[i].tenHP))
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Tên học phần rỗng";
                        }

                        // Kiểm tra số tín chỉ
                        if (danhSachHocPhan[i].soTinChi <= 0)
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Số tín chỉ không hợp lệ";
                        }

                        // Kiểm tra số tiết lý thuyết
                        if (danhSachHocPhan[i].lyThuyet < 0)
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Số tiết lý thuyết không hợp lệ";
                        }

                        // Kiểm tra số tiết thực hành
                        if (danhSachHocPhan[i].thucHanh < 0)
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Số tiết thực hành không hợp lệ";
                        }

                        // Kiểm tra trùng mã học phần trong danh sách tải lên
                        for (int j = 0; j < danhSachHocPhan.Count; j++)
                        {
                            if (i != j && danhSachHocPhan[i].maHP == danhSachHocPhan[j].maHP)
                            {
                                if (!string.IsNullOrEmpty(errMess))
                                {
                                    errMess += ", ";
                                }
                                errMess += "Mã học phần tải lên bị trùng!";
                            }
                        }

                        danhSachHocPhan[i].ghiChuLoi = errMess;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return danhSachHocPhan;
        }

        public List<ThongTinPhanCong> KiemTraHopLeThongTinPhanCongBoSung(List<ThongTinPhanCong> danhSachPhanCong, int idDotPhanCong)
        {
            try
            {
                for (int i = 0; i < danhSachPhanCong.Count; i++)
                {
                    if (danhSachPhanCong[i].isPhanCong)
                    {
                        continue;
                    }

                    string errMess = "";

                    using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                    {
                        // Kiểm tra mã giảng viên
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].maGV))
                        {
                            errMess += "Mã giảng viên trống";
                        }
                        else
                        {
                            var giangVien = context.GiangViens.Find(danhSachPhanCong[i].maGV);
                            if (giangVien == null)
                            {
                                if (!string.IsNullOrEmpty(errMess))
                                {
                                    errMess += ", ";
                                }
                                errMess += "Mã giảng viên không tồn tại";
                            }
                        }

                        // Kiểm tra mã học phần
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].maHP))
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Mã học phần không được bỏ trống";
                        }
                        else
                        {
                            var hocPhan = context.HocPhans.Find(danhSachPhanCong[i].maHP);
                            if (hocPhan == null)
                            {
                                if (!string.IsNullOrEmpty(errMess))
                                {
                                    errMess += ", ";
                                }
                                errMess += "Mã học phần không hợp lệ";
                            }
                        }

                        // Kiểm tra các trường không được trống
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].tenGV))
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Tên giảng viên không được trống";
                        }
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].tenHP))
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Tên học phần không được trống";
                        }
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].tenLop))
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Tên lớp không được trống";
                        }
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].thoiGianDay))
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Thời gian dạy không được trống";
                        }
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].hocKy))
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Học kỳ không được trống";
                        }
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].namHoc))
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Năm học không được trống";
                        }
                        if (string.IsNullOrEmpty(danhSachPhanCong[i].hinhThucDay))
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Hình thức dạy không được trống";
                        }

                        // Kiểm tra sĩ số hợp lệ
                        if (danhSachPhanCong[i].siSo <= 0)
                        {
                            if (!string.IsNullOrEmpty(errMess))
                            {
                                errMess += ", ";
                            }
                            errMess += "Sĩ số phải là số nguyên lớn hơn 0";
                        }

                        // Kiểm tra trùng phân công trong danh sách tải lên
                        for (int j = 0; j < danhSachPhanCong.Count; j++)
                        {
                            if (danhSachPhanCong[i].isTrungPhanCong(danhSachPhanCong[j]) && danhSachPhanCong[i].id != danhSachPhanCong[j].id)
                            {
                                if (!string.IsNullOrEmpty(errMess))
                                {
                                    errMess += ", ";
                                }
                                errMess += "Trùng phân công học phần";
                                break;
                            }
                        }

                        // Kiểm tra trùng phân công với dữ liệu trong cơ sở dữ liệu
                        var danhSachDaCo = context.PhanCongHocPhans.Where(op => op.MaDotPhanCong == idDotPhanCong).ToList();
                        if (danhSachDaCo.Any())
                        {
                            foreach (var item in danhSachDaCo)
                            {
                                if (danhSachPhanCong[i].maGV == item.MaGV && danhSachPhanCong[i].maHP == item.MaHP)
                                {
                                    if (!string.IsNullOrEmpty(errMess))
                                    {
                                        errMess += ", ";
                                    }
                                    errMess += "Trùng phân công học phần đã có trước đó";
                                    break;
                                }
                            }
                        }

                        // Ghi lại lỗi nếu có
                        danhSachPhanCong[i].ghiChu += errMess;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return danhSachPhanCong;
        }

    }
}
