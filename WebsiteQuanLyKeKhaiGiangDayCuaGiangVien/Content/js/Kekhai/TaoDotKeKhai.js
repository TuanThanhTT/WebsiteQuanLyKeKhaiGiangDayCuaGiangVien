document.addEventListener("DOMContentLoaded", function () {
    // Lấy các phần tử
    const modalInput = document.getElementById("tenPhanCong");
    const saveButton = document.getElementById("btnPhanCong");
    const errorMessage = document.querySelector(".error-message");
    const namHoc = document.getElementById("namHocPhanCong");
    const errNamHoc = document.getElementById("namHocError");
    const fileInput = document.getElementById("fileInput");

    // Hàm kiểm tra dữ liệu nhập
    function validateInput() {
        let isValid = true;

        const inputValue = modalInput.value.trim();
        const selectValue = namHoc.value;

        if (!selectValue) {
            errNamHoc.style.display = "block";
            namHoc.classList.add("is-invalid");
            isValid = false;
        } else {
            errNamHoc.style.display = "none";
            namHoc.classList.remove("is-invalid");
        }

        if (!inputValue) {
            errorMessage.style.display = "block";
            modalInput.classList.add("is-invalid");
            isValid = false;
        } else {
            errorMessage.style.display = "none";
            modalInput.classList.remove("is-invalid");
        }

        return isValid;
    }

    // Bắt sự kiện nút "Lưu"
    saveButton.addEventListener("click", function () {
        if (validateInput()) {
            const file = fileInput.files[0];
            const tenPhanCong = modalInput.value.trim();
            const maNamHoc = namHoc.value;

            UpFilePhanCong(file, tenPhanCong, maNamHoc);
            console.log("TenPhancong la: " + tenPhanCong);

            modalInput.value = "";
            fileInput.value = ""; // Reset file input sau khi upload
        }
    });
});



function laodNamHocDotPhanCongHocPhan() {

    $.ajax({
        url: '/Admin/KeKhai/LoadNamHoc',
        type: 'POST',
        success: function (response) {
            var mainNamHoc = document.getElementById("namHocPhanCong");
            mainNamHoc.innerHTML = '';
            if (response.success) {
                var data = response.data;

                var chonNamHoc = document.createElement("option");
                chonNamHoc.selected = true;
                chonNamHoc.value = "";
                chonNamHoc.textContent = "Chọn năm học";

                mainNamHoc.appendChild(chonNamHoc);


                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement("option");

                        option.value = data[i].Id;
                        option.textContent = data[i].TenNamHoc;

                        mainNamHoc.appendChild(option);

                    }
                }
                mainNamHoc.addEventListener('change', function () {
                    var selectedId = mainNamHoc.value;
                    if (selectedId != -1) {
                        loadHocKyTheoNamHoc(selectedId);
                    }
                });
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//tao phan cong hoc phan




//
function convertToDateTimeISO(dateString) {
    var parts = dateString.split(/[\/\s:]/);
    return `${parts[2]}-${parts[1]}-${parts[0]}T${parts[3]}:${parts[4]}:00`;
}

var isload = false;
document.addEventListener("DOMContentLoaded", function () {
    // Hàm AJAX để lấy dữ liệu từ server
    function loadBangPhanCong() {
        $.ajax({
            url: '/Admin/KeKhai/LoadBangPhanCong',
            type: 'POST',
            success: function (response) {
              
                if (response.success) {
                    var mainTable = document.getElementById("mainTable");
                    mainTable.innerHTML = '';
                    var data = response.data;
                   
                    if (data.length > 0) {
                       
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            var row = document.createElement('tr');
                            var ghiChuColor = item.isPhanCong ? '#32CD32' : 'red';

                            row.innerHTML = '<td>' + (i + 1) + '</td>' +
                                '<td>' + item.maGV + '</td>' +
                                '<td>' + item.tenGV + '</td>' +
                                '<td>' + item.maHP + '</td>' +
                                '<td>' + item.tenHP + '</td>' +
                                '<td>' + item.hinhThucDay + '</td>' +
                                '<td>' + item.hocKy + '</td>' +
                                '<td>' + item.namHoc + '</td>' +
                                '<td>' + item.siSo + '</td>' +
                                '<td>' + item.tenLop + '</td>' +
                                '<td>' + item.thoiGianDay + '</td>' +
                                '<td style="color: ' + ghiChuColor + ';">' + item.ghiChu + '</td>';
                            mainTable.appendChild(row);
                        }
                        var elements = document.getElementsByClassName("Modal-main-err-header");
                        for (var i = 0; i < elements.length; i++) {
                            elements[i].style.backgroundColor = "blue";
                        }
                        const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                        errorModal.show();
                        var titileModal = document.getElementById("Modal-main-err-Label");
                        titileModal.innerHTML = "Thông Báo"
                        var textmain = document.getElementById("text-main");
                        textmain.innerHTML = response.message;



                         
                        var countDownPanel = document.getElementById("countDownPanel");
                        countDownPanel.style.display = 'block';


                    } else {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td class="text-none" colspan="12">Không có dữ liệu hiện thị</td>';
                        mainTable.appendChild(row);

                        var countDownPanel = document.getElementById("countDownPanel");
                        countDownPanel.style.display = 'none';
                        run = false;
                    }
                }

            },
            error: function (xhr, status, error) {
                alert("Có lỗi xảy ra: " + error);
            }
        });
    }

    // Gọi hàm loadData ngay khi DOM được tải xong
    loadBangPhanCong();
});

function TaiMauFilePhanCong() {
    $.ajax({
        url: '/Admin/KeKhai/DowloadMauFile',
        type: 'POST',
        success: function (data) {
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = 'MauPhanCongHocPhan.xlsx';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },
        xhrFields: {
            responseType: 'blob'
        }
    });

}




function UpFilePhanCong(file, tenPhanCong, maNamHoc) {
    var formData = new FormData();
    formData.append("file", file);
    formData.append("tenPhanCong", tenPhanCong);
    formData.append("maNamHoc", maNamHoc);
    $.ajax({
        url: '/Admin/KeKhai/UpLoadFilePhanCong',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log("có chạy hàm upload file");
            console.log(JSON.stringify(response));
            console.log("success: " + response.success)
            if (response.success) {
                
                var mainTable = document.getElementById("mainTable");
                mainTable.innerHTML = '';
                var data = response.data;
                
                if (data.length > 0) {
                    console.log("có chạy");
                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        var row = document.createElement('tr');
                        var siSo = item.siSo >= 0 ? item.siSo : 'Không hợp lệ';
                        var ghiChuColor = item.isPhanCong ? '#32CD32' : 'red';
                        row.innerHTML = '<td>'+(i+1)+'</td>'+
                            '<td>' + item.maGV + '</td>' +
                            '<td>' + item.tenGV + '</td>' +
                            '<td>' + item.maHP + '</td>' +
                            '<td>' + item.tenHP + '</td>' +
                            '<td>' + item.hinhThucDay + '</td>' +
                            '<td>' + item.hocKy + '</td>' +
                            '<td>' + item.namHoc + '</td>' +
                            '<td>' + siSo + '</td>' +
                            '<td>' + item.tenLop + '</td>' +
                            '<td>' + item.thoiGianDay + '</td>' +
                            '<td style="color: ' + ghiChuColor + ';">' + item.ghiChu + '</td>';
                        mainTable.appendChild(row);
                    }
                    var elements = document.getElementsByClassName("Modal-main-err-header");
                    for (var i = 0; i < elements.length; i++)
                    {
                        elements[i].style.backgroundColor = "green";
                    }
                    var fileInput = document.getElementById("fileInput");
                    fileInput.value = "";

                    
                   
                    var mess = response.message;
                    if (response.valid == false) {
                        //var openModal = document.getElementById("openModal");
                        //openModal.style.display = "block";
                        var xuatDanhSachLoi = document.getElementById("xuatDanhSachLoi");
                        xuatDanhSachLoi.style.display = "none";

                    } else {
                        //var openModal = document.getElementById("openModal");
                        //openModal.style.display = "none";

                        var xuatDanhSachLoi = document.getElementById("xuatDanhSachLoi");
                        xuatDanhSachLoi.style.display = 'block';
                        mess = "Tải file phân công thành công! Phân công học phần không thành công!"

                    }

                    const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                    errorModal.show();
                    if (response.valid == false) {
                        var titileModal = document.getElementById("Modal-main-err-Label");
                        titileModal.innerHTML = "Thành Công"
                       
                    } else {
                        var elements = document.getElementsByClassName("Modal-main-err-header");
                        for (var i = 0; i < elements.length; i++) {
                            elements[i].style.backgroundColor = "red";
                        }
                        var titileModal = document.getElementById("Modal-main-err-Label");
                        titileModal.innerHTML = "Lỗi"
                    }
                    var textmain = document.getElementById("text-main");
                    textmain.innerHTML = mess;

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td class="text-none" colspan="12">Không có dữ liệu hiện thị</td>';
                    mainTable.appendChild(row);

                }
            } else {
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "red";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
               
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}



document.addEventListener("DOMContentLoaded", function () {
    function validateForm() {
        // Xóa các thông báo lỗi cũ
        var errorElements = document.getElementsByClassName("error-message");
        for (var i = 0; i < errorElements.length; i++) {
            errorElements[i].innerText = "";
        }
        var errorInputs = document.getElementsByClassName("error");
        while (errorInputs.length > 0) {
            errorInputs[0].classList.remove("error");
        }

        // Lấy giá trị của các thuộc tính
        var tenDot = document.getElementById("tenDot").value.trim();
        var startDate = document.getElementById("startDate").value.trim();
        var endDate = document.getElementById("endDate").value.trim();
        var hocKy = document.getElementById("hocKy").value.trim();
        var namHoc = document.getElementById("namHoc").value.trim();
        var ghiChu = document.getElementById("ghiChu").value.trim();
        var valid = true;

        // Lấy thời gian hiện tại và chuyển đổi thành định dạng dd/MM/yyyy hh:mm
        var now = new Date();
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var year = now.getFullYear();
        var hours = ("0" + now.getHours()).slice(-2);
        var minutes = ("0" + now.getMinutes()).slice(-2);
        var nowStr = day + "/" + month + "/" + year + " " + hours + ":" + minutes;

        // In ra giá trị của thời gian hiện tại để kiểm tra
        console.log("Ngày hiện tại: " + nowStr);
        console.log("ngay bat dau: " + startDate); 
        console.log("ngay ket thuc: " + endDate);

        // So sánh ngày tháng dưới dạng chuỗi dd/MM/yyyy hh:mm
        function compareDateStrings(dateTime1, dateTime2) {
            var parts1 = dateTime1.split(/[\/\s:]/);
            var parts2 = dateTime2.split(/[\/\s:]/);
            var dt1 = new Date(parts1[2], parts1[1] - 1, parts1[0], parts1[3], parts1[4]);
            var dt2 = new Date(parts2[2], parts2[1] - 1, parts2[0], parts2[3], parts2[4]);
            return dt1.getTime() - dt2.getTime();
        }

        // Kiểm tra Tên đợt kê khai (tenDot)
        if (!tenDot) {
            document.getElementById("tenDot").classList.add("error");
            document.getElementById("tenDotError").innerText = "Tên đợt kê khai không được để trống.";
            valid = false;
        }

        // Kiểm tra ngày bắt đầu (startDate)
        if (!startDate) {
            document.getElementById("startDate").classList.add("error");
            document.getElementById("startDateError").innerText = "Ngày bắt đầu không được để trống.";
            valid = false;
        } else if (compareDateStrings(startDate, nowStr) < 0) {
            document.getElementById("startDate").classList.add("error");
            document.getElementById("startDateError").innerText = "Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại.";
            valid = false;
        }

        // Kiểm tra ngày kết thúc (endDate)
        if (!endDate) {
            document.getElementById("endDate").classList.add("error");
            document.getElementById("endDateError").innerText = "Ngày kết thúc không được để trống.";
            valid = false;
        } else if (compareDateStrings(endDate, nowStr) <= 0) {
            document.getElementById("endDate").classList.add("error");
            document.getElementById("endDateError").innerText = "Ngày kết thúc phải lớn hơn ngày hiện tại.";
            valid = false;
        } else if (startDate && compareDateStrings(endDate, startDate) <= 0) {
            document.getElementById("endDate").classList.add("error");
            document.getElementById("endDateError").innerText = "Ngày kết thúc phải lớn hơn ngày bắt đầu.";
            valid = false;
        }

        // Kiểm tra học kỳ (hocKy)
        if (!hocKy) {
            document.getElementById("hocKy").classList.add("error");
            document.getElementById("hocKyError").innerText = "Học kỳ không được để trống.";
            valid = false;
        }

        // Kiểm tra năm học (namHoc)
        if (!namHoc) {
            document.getElementById("namHoc").classList.add("error");
            document.getElementById("namHocError").innerText = "Năm học không được để trống.";
            valid = false;
        }

        // Nếu tất cả các điều kiện đều thỏa mãn, cho phép tạo
        if (valid) {
            //alert("Tạo đợt kê khai thành công!");
            TaoDotKeKhai(tenDot, startDate, endDate, hocKy, namHoc, ghiChu);


            document.getElementById("tenDot").value = '';
            document.getElementById("startDate").value = '';
            document.getElementById("endDate").value = '';
            document.getElementById("hocKy").value = '';
            document.getElementById("namHoc").value = '';
            document.getElementById("ghiChu").value = '';
           
            // Thực hiện các hành động lưu hoặc xử lý dữ liệu tại đây
        }
    }

    // Xóa trạng thái lỗi khi người dùng bắt đầu nhập lại
    function clearError(inputId, errorId) {
        var inputElement = document.getElementById(inputId);
        var errorElement = document.getElementById(errorId);
        inputElement.addEventListener("input", function () {
            inputElement.classList.remove("error");
            errorElement.innerText = "";
        });
    }

    // Áp dụng xóa lỗi cho từng trường
    clearError("tenDot", "tenDotError");
    clearError("startDate", "startDateError");
    clearError("endDate", "endDateError");
    clearError("hocKy", "hocKyError");
    clearError("namHoc", "namHocError");

    document.getElementById("saveModal").addEventListener("click", validateForm);
});

function TaoDotKeKhai(tenDotKeKhai, ngayBatDau, ngayKetThuc, hocKy, namHoc, ghiChu) {
    var formData = new FormData();
    formData.append("tenDotKeKhai", tenDotKeKhai);
    formData.append("ngayBatDau", convertToDateTimeISO(ngayBatDau));
    formData.append("ngayKetThuc", convertToDateTimeISO(ngayKetThuc));
    formData.append("hocKy", hocKy);
    formData.append("namHoc", namHoc);
    formData.append("ghiChu", ghiChu);
    $.ajax({
        url: '/Admin/KeKhai/TaoDotKeKhaiMoi',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {

                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Green";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;

             
                layRaNgayKeThucKeKhai();
                    
               

                
            } else {
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "red";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
            }
           
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });

}





function XuatTableLoiPhanCong(tableID) {
    var table = document.getElementById(tableID);
    if (!table) {
        console.error('Table not found!');
        return;
    }

    var wb = XLSX.utils.book_new();
    var ws = XLSX.utils.table_to_sheet(table);

    // Duyệt qua tất cả các ô trong bảng
    var range = XLSX.utils.decode_range(ws['!ref']);
    for (var R = range.s.r; R <= range.e.r; ++R) {
        for (var C = range.s.c; C <= range.e.c; ++C) {
            var cell_address = { c: C, r: R };
            var cell_ref = XLSX.utils.encode_cell(cell_address);
            if (!ws[cell_ref]) continue;

            var cellValue = ws[cell_ref].v;

            // Lấy màu nền của thẻ <td>
            var cellElement = table.rows[R].cells[C];
            var color = window.getComputedStyle(cellElement, null).getPropertyValue('color');

            // Kiểm tra nếu màu nền là màu xanh lá cây (mã #32CD32)
            if (color === 'rgb(50, 205, 50)') { // Màu xanh lá cây với mã RGB (50, 205, 50)
                ws[cell_ref].v = "Đã tạo trước đó"; // Thay đổi nội dung ô thành "Đã tạo trước đó"
                ws[cell_ref].t = 's'; // Đặt kiểu ô thành chuỗi
            } else {
                ws[cell_ref].t = 's'; // Đặt kiểu ô thành chuỗi cho các giá trị khác
            }

            // Đặt định dạng cho ô
            ws[cell_ref].s = {
                font: { name: "Arial", sz: 12, color: { rgb: "FF000000" } },
                alignment: { horizontal: "center", vertical: "center" },
                border: {
                    top: { style: "thin", color: { rgb: "FF000000" } },
                    bottom: { style: "thin", color: { rgb: "FF000000" } },
                    left: { style: "thin", color: { rgb: "FF000000" } },
                    right: { style: "thin", color: { rgb: "FF000000" } }
                }
            };
        }
    }

    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    XLSX.writeFile(wb, 'DanhSachLoiPhanCong.xlsx');
}


function LoadNamHoc() {
   
    $.ajax({
        url: '/Admin/KeKhai/LoadNamHoc',
        type: 'POST',
        success: function (response) {
            console.log(JSON.stringify(response))
            var mainNamHoc = document.getElementById("namHoc");
            mainNamHoc.innerHTML = '';
            if (response.success) {
                var data = response.data;
                
                var chonNamHoc = document.createElement("option");
                chonNamHoc.selected = true;
                chonNamHoc.value = "";
                chonNamHoc.textContent = "Chọn năm học";
               
                mainNamHoc.appendChild(chonNamHoc);
               
                
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement("option");
                       
                        option.value = data[i].Id;
                        option.textContent = data[i].TenNamHoc;
                        
                        mainNamHoc.appendChild(option);

                    }
                }
                mainNamHoc.addEventListener('change', function () {
                    var selectedId = mainNamHoc.value;
                    if (selectedId != -1) { 
                        loadHocKyTheoNamHoc(selectedId);
                    }
                });
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    LoadNamHoc();
    laodNamHocDotPhanCongHocPhan();
});


function loadHocKyTheoNamHoc(namHoc) {
    var formData = new FormData();
    formData.append("namHoc", namHoc);
    console.log("co chay ham chon option");
    $.ajax({
        url: '/Admin/KeKhai/LoadHocKyTheoNamHoc',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            var mainHocKy = document.getElementById("hocKy");
            mainHocKy.innerHTML = '';
            if (response.success) {
                var data = response.data;
              
                var hocky = document.createElement('option');
                hocky.selected = true;
                hocky.value = "";
                hocky.textContent = "Chọn học kỳ";
                
                mainHocKy.appendChild(hocky);
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement('option');
                        option.value = data[i].MaHocKy;
                        option.textContent = data[i].TenHocKy;
                        mainHocKy.appendChild(option);
                    }
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
