using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models;
using WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.ModelCustom;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Service.FileService
{
    public class FileUpload : IFileUpload
    {
        public string UploadFilePhanCong(HttpPostedFileBase file)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                    return "";

                // Thay đổi thư mục lưu file từ wwwroot/FileUpload -> Content/FileUpload
                var upload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/FileUpload");

                if (!Directory.Exists(upload))
                {
                    Directory.CreateDirectory(upload);
                }

                var filePath = Path.Combine(upload, file.FileName);

                // Nếu file đã tồn tại, đổi tên file để tránh trùng lặp
                if (File.Exists(filePath))
                {
                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
                    filePath = Path.Combine(upload, uniqueFileName);
                }

                file.SaveAs(filePath);
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }


        public int SaveFilePhanCong(string fileName)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Models.WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    int id = context.FilePhanCongs.Count() + 1;

                    while (context.FilePhanCongs.Find(id) != null)
                    {
                        id++;
                    }

                    var filePhanCong = new FilePhanCong
                    {
                        MaFilePhanCong = id,
                        UrlFile = fileName
                    };

                    context.FilePhanCongs.Add(filePhanCong);
                    context.SaveChanges();
                    return filePhanCong.MaFilePhanCong;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }


        public string XuatFilePhanCongTheoGiangVien(List<XemThongTinHocPhanDuocPhanCong> dsHocPhanPhanCong)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("STT", typeof(int));
                    dataTable.Columns.Add("Mã Học Phần", typeof(string));
                    dataTable.Columns.Add("Tên Học Phần", typeof(string));
                    dataTable.Columns.Add("Thời gian dạy", typeof(string));
                    dataTable.Columns.Add("Tên Lớp", typeof(string));
                    dataTable.Columns.Add("Sĩ Số", typeof(int));
                    dataTable.Columns.Add("Học Kỳ", typeof(string));
                    dataTable.Columns.Add("Năm Học", typeof(string));
                    dataTable.Columns.Add("Hình Thức Dạy", typeof(string));

                    if (dsHocPhanPhanCong != null && dsHocPhanPhanCong.Any())
                    {
                        int index = 1;
                        foreach (var item in dsHocPhanPhanCong)
                        {
                            dataTable.Rows.Add(index, item.maHocPhanPhanCong, item.tenHocPhanPhanCong, item.ngayDay, item.tenLop,
                                item.siSo, item.hocKy, item.namHoc, item.hinhThucDay);
                            index++;
                        }

                        using (var package = new ExcelPackage())
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            var worksheet = package.Workbook.Worksheets.Add("Danh Sách Học Phần Được Phân Công");
                            worksheet.Cells["A1"].Value = "Danh Sách Phân Công";
                            worksheet.Cells["A1:I1"].Merge = true;

                            worksheet.Cells["A1"].Style.Font.Bold = true;
                            worksheet.Cells["A1"].Style.Font.Size = 14;
                            worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            worksheet.Cells["A2"].Value = "Số lượng phân công: " + dataTable.Rows.Count;
                            worksheet.Cells["A2:I2"].Merge = true;

                            worksheet.Cells["A3"].LoadFromDataTable(dataTable, true);

                            using (var range = worksheet.Cells["A3:I3"])
                            {
                                range.Style.Font.Bold = true;
                                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            }

                            using (var range = worksheet.Cells["A3:I" + worksheet.Dimension.End.Row])
                            {
                                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            }

                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                            var fileName = $"PhanCong_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                            //var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "FileExport");
                            //var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "/Content/FileExport");
                            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileExport");

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                            var filePath = Path.Combine(folderPath, fileName);
                            File.WriteAllBytes(filePath, package.GetAsByteArray());

                            return fileName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
            return "";
        }

        public string XuatFileKeKhaiCuaGiangVienTheoDot(string maGV, List<XemThongTinKeKhaiDaDuyet> listKeKhai, string dotKeKhai)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var giangVien = context.GiangViens.Find(maGV);
                    if (giangVien != null)
                    {
                        var dataTable = new DataTable();
                        dataTable.Columns.Add("STT", typeof(int));
                        dataTable.Columns.Add("Mã Học Phần", typeof(string));
                        dataTable.Columns.Add("Tên Học Phần", typeof(string));
                        dataTable.Columns.Add("Tên Lớp", typeof(string));
                        dataTable.Columns.Add("Sĩ Số", typeof(int));
                        dataTable.Columns.Add("Thời Gian Dạy", typeof(string));
                        dataTable.Columns.Add("Học Kỳ", typeof(string));
                        dataTable.Columns.Add("Năm Học", typeof(string));
                        dataTable.Columns.Add("Hình Thức Dạy", typeof(string));
                        dataTable.Columns.Add("Ngày Kê Khai", typeof(string));
                        dataTable.Columns.Add("Ngày Duyệt", typeof(string));
                        dataTable.Columns.Add("Người Duyệt", typeof(string));

                        if (listKeKhai != null && listKeKhai.Any())
                        {
                            int index = 1;
                            foreach (var item in listKeKhai)
                            {
                                dataTable.Rows.Add(index, item.maHP, item.tenHocPhan, item.tenLop, item.soLuong, item.ngayDay,
                                    item.hocKy, item.namHoc, item.hinhThucDay, item.ngayKeKhai,
                                    item.ngayDuyet.ToString("dd/MM/yyyy"), item.nguoiDuyet);
                                index++;
                            }

                            using (var package = new ExcelPackage())
                            {
                                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                                var worksheet = package.Workbook.Worksheets.Add("Danh Sách Học Phần Đã Kê Khai Đợt: " + dotKeKhai);
                                worksheet.Cells["A1"].Value = "Danh Sách Học Phần Đã Kê Khai";
                                worksheet.Cells["A1:L1"].Merge = true;

                                worksheet.Cells["A1"].Style.Font.Bold = true;
                                worksheet.Cells["A1"].Style.Font.Size = 14;
                                worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                                worksheet.Cells["A2"].Value = "Đợt kê khai: " + dotKeKhai;
                                worksheet.Cells["A2:E2"].Merge = true;

                                worksheet.Cells["F2"].Value = "Số lượng kê khai: " + dataTable.Rows.Count;
                                worksheet.Cells["F2:L2"].Merge = true;

                                worksheet.Cells["A3"].Value = "Mã giảng viên: " + giangVien.MaGV;
                                worksheet.Cells["A3:E3"].Merge = true;

                                worksheet.Cells["F3"].Value = "Tên giảng viên: " + giangVien.TenGV;
                                worksheet.Cells["F3:L3"].Merge = true;

                                worksheet.Cells["A5"].LoadFromDataTable(dataTable, true);

                                using (var range = worksheet.Cells["A5:L5"])
                                {
                                    range.Style.Font.Bold = true;
                                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                }

                                using (var range = worksheet.Cells["A5:L" + worksheet.Dimension.End.Row])
                                {
                                    range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                }

                                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                                var fileName = $"KeKhai_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                                var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileExport");
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }
                                var filePath = Path.Combine(folderPath, fileName);

                                // Lưu file đồng bộ (do .NET Framework không hỗ trợ SaveAsAsync)
                                File.WriteAllBytes(filePath, package.GetAsByteArray());

                                return fileName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }


        public string UploadFileKhoa(HttpPostedFileBase file)
        {
            try
            {
                var uploadFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileUpload");

                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var filePath = Path.Combine(uploadFolder, file.FileName);

                // Nếu file đã tồn tại, thêm timestamp để tránh trùng lặp
                if (File.Exists(filePath))
                {
                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
                    filePath = Path.Combine(uploadFolder, uniqueFileName);
                }

                // Lưu file đồng bộ (do .NET Framework không hỗ trợ async với FileStream)
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    file.InputStream.CopyTo(stream);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }


        public string UploadFileGiangVien(HttpPostedFileBase file)
        {
            try
            {
                var uploadFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileUpload");

                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var filePath = Path.Combine(uploadFolder, file.FileName);

                // Nếu file đã tồn tại, thêm timestamp để tránh trùng lặp
                if (File.Exists(filePath))
                {
                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
                    filePath = Path.Combine(uploadFolder, uniqueFileName);
                }

                // Lưu file đồng bộ (do .NET Framework không hỗ trợ async với FileStream)
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    file.InputStream.CopyTo(stream);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }


        public string UploadFileHocPhan(HttpPostedFileBase file)
        {
            try
            {
                var uploadFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileUpload");

                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var filePath = Path.Combine(uploadFolder, file.FileName);

                // Nếu file đã tồn tại, thêm timestamp để tránh trùng lặp
                if (File.Exists(filePath))
                {
                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
                    filePath = Path.Combine(uploadFolder, uniqueFileName);
                }

                // Lưu file đồng bộ (do .NET Framework không hỗ trợ async với FileStream)
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    file.InputStream.CopyTo(stream);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }


        public string XuatFileThongKeTheoKhoa(List<ThongTinChiTietTienDo> danhSachThongKe)
        {
            try
            {
                using (var context = new WebsiteQuanLyKeKhaiGiangDayEntities1())
                {
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("STT", typeof(int));
                    dataTable.Columns.Add("Mã Giảng Viên", typeof(string));
                    dataTable.Columns.Add("Tên Giảng Viên", typeof(string));
                    dataTable.Columns.Add("Tên Khoa", typeof(string));
                    dataTable.Columns.Add("Tiến Độ", typeof(string));

                    if (danhSachThongKe != null && danhSachThongKe.Any())
                    {
                        int index = 1;
                        foreach (var item in danhSachThongKe)
                        {
                            string tienDo = item.soLuongHoanThanh + "/" + item.soLuongPhanCong;
                            dataTable.Rows.Add(index, item.maGV, item.tenGV, item.tenKhoa, tienDo);
                            index++;
                        }

                        string tenKhoa = danhSachThongKe[0].tenKhoa;
                        using (var package = new ExcelPackage())
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            var worksheet = package.Workbook.Worksheets.Add("Tiến Độ Kê Khai Của Giảng Viên - Khoa " + tenKhoa);
                            worksheet.Cells["A1"].Value = "Tiến Độ Kê Khai Của Giảng Viên";
                            worksheet.Cells["A1:E1"].Merge = true;

                            worksheet.Cells["A1"].Style.Font.Bold = true;
                            worksheet.Cells["A1"].Style.Font.Size = 14;
                            worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            worksheet.Cells["A2"].Value = "Số lượng phân công: " + dataTable.Rows.Count;
                            worksheet.Cells["A2:E2"].Merge = true;

                            worksheet.Cells["A3"].Value = "Khoa: " + tenKhoa;
                            worksheet.Cells["A3:E3"].Merge = true;

                            worksheet.Cells["A4"].LoadFromDataTable(dataTable, true);

                            using (var range = worksheet.Cells["A4:E4"])
                            {
                                range.Style.Font.Bold = true;
                                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            }

                            using (var range = worksheet.Cells["A4:E" + worksheet.Dimension.End.Row])
                            {
                                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            }

                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                            var fileName = $"ThongKeTienDo_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileExport");

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            var filePath = Path.Combine(folderPath, fileName);

                            // Thay thế File.WriteAllBytesAsync bằng File.WriteAllBytes (đồng bộ)
                            File.WriteAllBytes(filePath, package.GetAsByteArray());

                            return fileName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public string XuatFileThongKe(List<ThongTinChiTietTienDo> danhSachThongKe)
        {
            try
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add("STT", typeof(int));
                dataTable.Columns.Add("Mã Giảng Viên", typeof(string));
                dataTable.Columns.Add("Tên Giảng Viên", typeof(string));
                dataTable.Columns.Add("Tên Khoa", typeof(string));
                dataTable.Columns.Add("Tiến Độ", typeof(string));

                if (danhSachThongKe != null && danhSachThongKe.Any())
                {
                    int index = 1;
                    foreach (var item in danhSachThongKe)
                    {
                        string tienDo = item.soLuongHoanThanh + "/" + item.soLuongPhanCong;
                        dataTable.Rows.Add(index, item.maGV, item.tenGV, item.tenKhoa, tienDo);
                        index++;
                    }

                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Tiến Độ Kê Khai Của Giảng Viên");
                        worksheet.Cells["A1"].Value = "Tiến Độ Kê Khai Của Giảng Viên";
                        worksheet.Cells["A1:E1"].Merge = true;
                        worksheet.Cells["A1"].Style.Font.Bold = true;
                        worksheet.Cells["A1"].Style.Font.Size = 14;
                        worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells["A2"].Value = "Số lượng phân công: " + dataTable.Rows.Count;
                        worksheet.Cells["A2:E2"].Merge = true;

                        worksheet.Cells["A4"].LoadFromDataTable(dataTable, true);

                        using (var range = worksheet.Cells["A4:E4"])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        }

                        using (var range = worksheet.Cells["A4:E" + worksheet.Dimension.End.Row])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        var fileName = $"ThongKeTienDo_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                        var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileExport");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        var filePath = Path.Combine(folderPath, fileName);

                        File.WriteAllBytes(filePath, package.GetAsByteArray());

                        return fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public string XuatFileThongKeGiangVienChuaHoanThanhKeKhaiTheoDot(List<GiangVienHoanThanhKeKhai> danhSachThongKe)
        {
            try
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add("STT", typeof(int));
                dataTable.Columns.Add("Mã Giảng Viên", typeof(string));
                dataTable.Columns.Add("Tên Giảng Viên", typeof(string));
                dataTable.Columns.Add("Tên Khoa", typeof(string));
                dataTable.Columns.Add("Tiến Độ", typeof(string));

                if (danhSachThongKe != null && danhSachThongKe.Any())
                {
                    int index = 1;
                    foreach (var item in danhSachThongKe)
                    {
                        string tienDo = item.tienDo;
                        dataTable.Rows.Add(index, item.maGV, item.tenGV, item.tenKhoa, tienDo);
                        index++;
                    }

                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Giảng Viên Chưa Hoàn Thành Kê Khai");
                        worksheet.Cells["A1"].Value = "Danh Sách Giảng Viên Chưa Hoàn Thành Kê Khai";
                        worksheet.Cells["A1:E1"].Merge = true;
                        worksheet.Cells["A1"].Style.Font.Bold = true;
                        worksheet.Cells["A1"].Style.Font.Size = 14;
                        worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells["A2"].Value = "Số lượng giảng viên: " + dataTable.Rows.Count;
                        worksheet.Cells["A2:E2"].Merge = true;

                        worksheet.Cells["A4"].LoadFromDataTable(dataTable, true);

                        using (var range = worksheet.Cells["A4:E4"])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        }

                        using (var range = worksheet.Cells["A4:E" + worksheet.Dimension.End.Row])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        var fileName = $"ThongKeGiangVienChuaHoanThanhKeKhai_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                        var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileExport");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        var filePath = Path.Combine(folderPath, fileName);

                        File.WriteAllBytes(filePath, package.GetAsByteArray());

                        return fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public string XuatFileThongKeGiangVienHoanThanhKeKhaiTheoDot(List<GiangVienHoanThanhKeKhai> danhSachThongKe)
        {
            try
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add("STT", typeof(int));
                dataTable.Columns.Add("Mã Giảng Viên", typeof(string));
                dataTable.Columns.Add("Tên Giảng Viên", typeof(string));
                dataTable.Columns.Add("Tên Khoa", typeof(string));
                dataTable.Columns.Add("Tiến Độ", typeof(string));

                if (danhSachThongKe != null && danhSachThongKe.Any())
                {
                    int index = 1;
                    foreach (var item in danhSachThongKe)
                    {
                        string tienDo = item.tienDo;
                        dataTable.Rows.Add(index, item.maGV, item.tenGV, item.tenKhoa, tienDo);
                        index++;
                    }

                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Giảng Viên Hoàn Thành Kê Khai");
                        worksheet.Cells["A1"].Value = "Danh Sách Giảng Viên Hoàn Thành Kê Khai";
                        worksheet.Cells["A1:E1"].Merge = true;
                        worksheet.Cells["A1"].Style.Font.Bold = true;
                        worksheet.Cells["A1"].Style.Font.Size = 14;
                        worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells["A2"].Value = "Số lượng giảng viên: " + dataTable.Rows.Count;
                        worksheet.Cells["A2:E2"].Merge = true;

                        worksheet.Cells["A4"].LoadFromDataTable(dataTable, true);

                        using (var range = worksheet.Cells["A4:E4"])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        }

                        using (var range = worksheet.Cells["A4:E" + worksheet.Dimension.End.Row])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        var fileName = $"ThongKeGiangVienHoanThanhKeKhai_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                        var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileExport");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var filePath = Path.Combine(folderPath, fileName);
                        File.WriteAllBytes(filePath, package.GetAsByteArray());

                        return fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public string UploadAvatar(HttpPostedFileBase file)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                    return "";

                var upload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/ImgAvartar");

                if (!Directory.Exists(upload))
                {
                    Directory.CreateDirectory(upload);
                }

                var filePath = Path.Combine(upload, file.FileName);

                if (File.Exists(filePath))
                {
                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
                    filePath = Path.Combine(upload, uniqueFileName);
                }

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    file.InputStream.CopyTo(stream);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }

        public string XuatFileDanhSachGiangVienDaKhoa(List<XemChiTietThongTinGiangVien> danhSachGiangVien)
        {
            try
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add("STT", typeof(int));
                dataTable.Columns.Add("Mã Giảng Viên", typeof(string));
                dataTable.Columns.Add("Tên Giảng Viên", typeof(string));
                dataTable.Columns.Add("Ngày Sinh", typeof(string));
                dataTable.Columns.Add("Khoa", typeof(string));
                dataTable.Columns.Add("Chức Vụ", typeof(string));
                dataTable.Columns.Add("Chuyên Nghành", typeof(string));
                dataTable.Columns.Add("Địa Chỉ", typeof(string));
                dataTable.Columns.Add("Hệ Số Lương", typeof(string));
                dataTable.Columns.Add("Email", typeof(string));
                dataTable.Columns.Add("Giới Tính", typeof(string));
                dataTable.Columns.Add("Trình Độ", typeof(string));
                dataTable.Columns.Add("Loại Hình Đào Tạo", typeof(string));

                if (danhSachGiangVien != null && danhSachGiangVien.Any())
                {
                    int index = 1;
                    foreach (var item in danhSachGiangVien)
                    {
                        dataTable.Rows.Add(index, item.maGV, item.tenGV, item.ngaySinh, item.tenKhoa, item.chucVu, item.chuyenNghanh,
                            item.diaChi, item.heSoLuong, item.email, item.gioiTinh, item.trinhDo, item.loaiHinhDaoTao);
                        index++;
                    }

                    using (var package = new ExcelPackage())
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var worksheet = package.Workbook.Worksheets.Add("Danh Sách Giảng Viên Đã Khóa");
                        worksheet.Cells["A1"].Value = "Danh Sách Giảng Viên Đã Khóa";
                        worksheet.Cells["A1:M1"].Merge = true;

                        worksheet.Cells["A1"].Style.Font.Bold = true;
                        worksheet.Cells["A1"].Style.Font.Size = 14;
                        worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells["A2"].Value = "Số lượng giảng viên: " + dataTable.Rows.Count;
                        worksheet.Cells["A2:M2"].Merge = true;

                        worksheet.Cells["A3"].LoadFromDataTable(dataTable, true);

                        using (var range = worksheet.Cells["A3:M3"])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        }

                        using (var range = worksheet.Cells["A3:M" + worksheet.Dimension.End.Row])
                        {
                            // Thiết lập viền cho toàn bộ phạm vi
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        var fileName = $"DanhSachGiangVienDaKhoa_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                        var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "FileExport");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var filePath = Path.Combine(folderPath, fileName);

                        // Lưu file đồng bộ (không cần async)
                        File.WriteAllBytes(filePath, package.GetAsByteArray());

                        return fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }
    }
}
