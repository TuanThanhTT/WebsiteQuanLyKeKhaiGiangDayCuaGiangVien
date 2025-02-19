

function loadDataBangDotKeKhai() {
    $.ajax({
        url: '/Admin/KeKhai/layThongTinDotKeKhaiDangMo',
        type: 'POST',
        success: function (response) {
            if (response.success) {
                var mainTable = document.getElementById("table-dotKeKhai");
                mainTable.innerHTML = '';
                var data = response.data;
                var row = document.createElement('tr');
                var ghiChu = data.ghiChu;

                row.innerHTML = '<td>1</td>' +
                    '<td>' + data.tenDotKeKhai + '</td>' +
                    '<td>' + data.ngayBatDau + '</td>' +
                    '<td>' + data.ngayKetThuc + '</td>' +
                    '<td>' + data.hocKy + '</td>' +
                    '<td>' + data.namHoc + '</td>' +
                    '<td>' + data.nguoiTao + '</td>' +

                    '<td>' +
                    '<button id="openModalKhoaDotKeKhai" onclick="KhoaDotKeKhai()" class="btn btn-danger" style="margin-right: 5px;">Khóa</button>' +
                    '<button id="openModalChinhSua" onclick="HienThiModalChinhSua()" class="btn btn-warning">Cập nhật</button>' +
                    '</td>';
                mainTable.appendChild(row);
                var btnBoSungPhanCong = document.getElementById("btnBoSungPhanCong");
                btnBoSungPhanCong.style.display = 'block';
                var btnXemDotKeKhaiHienTai = document.getElementById("btnXemDotKeKhaiHienTai");
                btnXemDotKeKhaiHienTai.style.display = 'block';
                var countDownPanel = document.getElementById("countDownPanel");
                countDownPanel.style.display = 'block';

            } else {
                var mainTable = document.getElementById("table-dotKeKhai");
                mainTable.innerHTML = '';
                var row = document.createElement('tr');
                row.innerHTML = '<td class="text-none" colspan="8">Không có dữ liệu hiện thị</td>'
                mainTable.appendChild(row);

                var btnBoSungPhanCong = document.getElementById("btnBoSungPhanCong");
                btnBoSungPhanCong.style.display = 'none';
                var btnXemDotKeKhaiHienTai = document.getElementById("btnXemDotKeKhaiHienTai");
                btnXemDotKeKhaiHienTai.style.display = 'none';
                var countDownPanel = document.getElementById("countDownPanel");
                countDownPanel.style.display = 'none';
                run = false;
            }
          
           
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}



document.addEventListener("DOMContentLoaded", function () {
    // Hàm AJAX để lấy dữ liệu từ server
    

    // Gọi hàm loadData ngay khi DOM được tải xong
    loadDataBangDotKeKhai();
});


document.addEventListener("DOMContentLoaded", function () {
    // Hàm so sánh hai chuỗi ngày (dd/MM/yyyy hh:mm)
    function compareDateStrings(dateTime1, dateTime2) {
        // Tách các phần từ của chuỗi ngày để lấy ngày, tháng, năm, giờ, phút
        var parts1 = dateTime1.split(/[\/\s:]/); // Tách theo ký tự "/" và " " và ":"
        var parts2 = dateTime2.split(/[\/\s:]/);

        // Tạo đối tượng Date từ các phần tách được
        var dt1 = new Date(parts1[2], parts1[1] - 1, parts1[0], parts1[3], parts1[4]); // parts1[2] là năm, parts1[1] là tháng (tháng trong JS bắt đầu từ 0)
        var dt2 = new Date(parts2[2], parts2[1] - 1, parts2[0], parts2[3], parts2[4]);

        // So sánh thời gian
        return dt1.getTime() - dt2.getTime(); // Trả về giá trị dương nếu dt1 > dt2, âm nếu ngược lại
    }

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

        // Lấy giá trị của các trường
        var tenDot = document.getElementById("tbsTenDot").value.trim();
        var startDate = document.getElementById("startDate").value.trim();
        var endDate = document.getElementById("endDate").value.trim();
        var hocKy = document.getElementById("tbshocKy").value.trim();
        var namHoc = document.getElementById("tbsnamHoc").value.trim();
        var ghiChu = document.getElementById("tbsghiChu").value.trim();
        var valid = true;

        // Kiểm tra Tên đợt kê khai (tenDot)
        if (!tenDot) {
            document.getElementById("tbsTenDot").classList.add("error");
            document.getElementById("tbsTenDotError").innerText = "Tên đợt kê khai không được để trống.";
            valid = false;
        }

        // Kiểm tra ngày bắt đầu (startDate)
        if (!startDate) {
            document.getElementById("startDate").classList.add("error");
            document.getElementById("startDateError").innerText = "Ngày bắt đầu không được để trống.";
            valid = false;
        } else if (compareDateStrings(startDate, getCurrentDate()) < 0) {
            document.getElementById("startDate").classList.add("error");
            document.getElementById("startDateError").innerText = "Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại.";
            valid = false;
        }

        // Kiểm tra ngày kết thúc (endDate)
        if (!endDate) {
            document.getElementById("endDate").classList.add("error");
            document.getElementById("endDateError").innerText = "Ngày kết thúc không được để trống.";
            valid = false;
        } else if (compareDateStrings(endDate, getCurrentDate()) <= 0) {
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
            document.getElementById("tbshocKy").classList.add("error");
            document.getElementById("tbshocKyError").innerText = "Vui lòng chọn học kỳ.";
            valid = false;
        }

        // Kiểm tra năm học (namHoc)
        if (!namHoc) {
            document.getElementById("tbsnamHoc").classList.add("error");
            document.getElementById("tbsnamHocError").innerText = "Vui lòng chọn năm học.";
            valid = false;
        }

        // Nếu tất cả các điều kiện đều thỏa mãn, cho phép tạo
        if (valid) {
            //alert("Tạo đợt kê khai thành công!");
            console.log("Ngay ket thuc: " + endDate);
            TaoDotKeKhaiBoSung(tenDot, startDate, endDate, hocKy, namHoc, ghiChu);



            // Reset các giá trị
            document.getElementById("tbsTenDot").value = '';
            document.getElementById("startDate").value = '';
            document.getElementById("endDate").value = '';
            document.getElementById("tbshocKy").value = '';
            document.getElementById("tbsnamHoc").value = '';
            document.getElementById("tbsghiChu").value = '';
        }
    }

    // Lấy ngày hiện tại dưới dạng dd/MM/yyyy hh:mm
    function getCurrentDate() {
        var now = new Date();
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var year = now.getFullYear();
        var hours = ("0" + now.getHours()).slice(-2);
        var minutes = ("0" + now.getMinutes()).slice(-2);
        return day + "/" + month + "/" + year + " " + hours + ":" + minutes;
    }

    // Đăng ký sự kiện khi nhấn "Lưu"
    document.getElementById("tbssaveModal").addEventListener("click", validateForm);
});

function convertToISO(dateString) {
    var parts = dateString.split(/[\/\s:]/);
    return `${parts[2]}-${parts[1]}-${parts[0]}T${parts[3]}:${parts[4]}:00`;
}


function TaoDotKeKhaiBoSung(tenDotKeKhai, ngayBatDau, ngayKT, hocKy, namHoc, ghiChu) {
    var formData = new FormData();
    formData.append("tenDotKeKhai", tenDotKeKhai);
 
    formData.append("ngayKT", convertToISO(ngayKT));
    formData.append("ngayBatDau", convertToISO(ngayBatDau));
   
    formData.append("hocKy", hocKy);
    formData.append("namHoc", namHoc);
    formData.append("ghiChu", ghiChu);
    
    console.log("nagy bat dau: " + ngayKT);
    console.log("ngay ket thuc: " + formData.get("ngayKT"));
    $.ajax({
        url: '/Admin/KeKhai/TaoDotKeKhaiDangKyBoSung',
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

                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Thành Công";
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
                var countDownPanel = document.getElementById("countDownPanel");
                countDownPanel.style.display = 'block';
                layRaNgayKeThucKeKhai();
                loadDataBangDotKeKhai();

            } else {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
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

// Xác nhận (Khóa hành động, thực hiện logic ở đây)





function KhoaDotKeKhaiHienTai() {
    $.ajax({
        url: '/Admin/KeKhai/KhoaDotKeKhaiHienTai',
        type: 'POST',
        success: function (response) {
            if (response.success) {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Thành Công";
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Green";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
                loadDataBangDotKeKhai();
               
                
               
            } else {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
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


function KhoaDotKeKhai() {

    

    var modalConfirmKhoa = document.getElementById("modalConfirmKhoa");
    modalConfirmKhoa.style.display = "block";

    const confirmBtn = document.getElementById("confirmBtn");
    confirmBtn.addEventListener("click", function () {
        KhoaDotKeKhaiHienTai();
        modalConfirmKhoa.style.display = "none";
        
      
    });
   

}




function XemDotKeKhaiHienTaiDangMo() {
  
    $.ajax({
        url: '/Admin/KeKhai/XemDotKeKhaiHienTaiDangMo',
        type: 'POST',
        success: function (response) {
            if (response.success) {

                console.log(JSON.stringify(response));

                var data = response.data;
                var modalXemDotKeKhai = document.getElementById("modalXemDotKeKhai");
                modalXemDotKeKhai.style.display = "block";
                var tenDot = document.getElementById("read-tenDot");
                var ngayBatDau = document.getElementById("read-startDate");
                var ngayKetThuc = document.getElementById("read-endDate");
                var hocKy = document.getElementById("read-hocKy");
                var namHoc = document.getElementById("read-namHoc");
                var ghiChu = document.getElementById("read-ghiChu");
                var nguoiTao = document.getElementById("read-nguoiTao");

                var ghichu = data.ghiChu;
                if (ghiChu == null) {
                    ghichu = " ";
                }

                ghiChu.value = ghichu;
                tenDot.value = data.tenDotKeKhai;
                ngayBatDau.value = data.ngayBatDau;
                ngayKetThuc.value = data.ngayKetThuc;
                hocKy.value = data.hocKy;
                namHoc.value = data.namHoc;
                nguoiTao.value = data.nguoiTao;

            } else {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
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
    // Hàm so sánh hai chuỗi ngày (dd/MM/yyyy hh:mm)
    function compareDateStrings(dateTime1, dateTime2) {
        // Tách các phần từ của chuỗi ngày để lấy ngày, tháng, năm, giờ, phút
        var parts1 = dateTime1.split(/[\/\s:]/); // Tách theo ký tự "/" và " " và ":"
        var parts2 = dateTime2.split(/[\/\s:]/);

        // Tạo đối tượng Date từ các phần tách được
        var dt1 = new Date(parts1[2], parts1[1] - 1, parts1[0], parts1[3], parts1[4]); // parts1[2] là năm, parts1[1] là tháng (tháng trong JS bắt đầu từ 0)
        var dt2 = new Date(parts2[2], parts2[1] - 1, parts2[0], parts2[3], parts2[4]);

        // So sánh thời gian
        return dt1.getTime() - dt2.getTime(); // Trả về giá trị dương nếu dt1 > dt2, âm nếu ngược lại
    }

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

        // Lấy giá trị của các trường
        var startDate = document.getElementById("edit-startDate").value.trim();
        var endDate = document.getElementById("edit-endDate").value.trim();
        var ghiChu = document.getElementById("edit-ghiChu").value.trim();

        var valid = true;

     
        // Kiểm tra ngày kết thúc (endDate)
        if (!endDate) {
            document.getElementById("edit-endDate").classList.add("error");
            document.getElementById("edit-startDateError").innerText = "Ngày kết thúc không được để trống.";
            valid = false;
        } else if (compareDateStrings(endDate, getCurrentDate()) <= 0) {
            document.getElementById("edit-endDate").classList.add("error");
            document.getElementById("edit-startDateError").innerText = "Ngày kết thúc phải lớn hơn ngày hiện tại.";
            valid = false;
        } else if (startDate && compareDateStrings(endDate, startDate) <= 0) {
            document.getElementById("edit-endDate").classList.add("error");
            document.getElementById("edit-startDateError").innerText = "Ngày kết thúc phải lớn hơn ngày bắt đầu.";
            valid = false;
        }

        // Nếu tất cả các điều kiện đều thỏa mãn, cho phép tạo
        if (valid) {
          
            ChinhSuaDotKeKhaiDangMoHienTai(endDate, ghiChu);
            
        }
    }

    // Lấy ngày hiện tại dưới dạng dd/MM/yyyy hh:mm
    function getCurrentDate() {
        var now = new Date();
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var year = now.getFullYear();
        var hours = ("0" + now.getHours()).slice(-2);
        var minutes = ("0" + now.getMinutes()).slice(-2);
        return day + "/" + month + "/" + year + " " + hours + ":" + minutes;
    }

    // Đăng ký sự kiện khi nhấn "Lưu"
    document.getElementById("edit-saveModal").addEventListener("click", validateForm);
});






function chinhSuaDotKeKhai() {
    HienThiModalChinhSua();
}

function HienThiModalChinhSua() {
  
    $.ajax({
        url: '/Admin/KeKhai/HienThiThongTinDotKeKhaiDangMo',
        type: 'POST',
        success: function (response) {
            if (response.success) {
                var data = response.data;
                console.log("da chay");
                var modalChinhSuaDotKeKhai = document.getElementById("modalChinhSuaDotKeKhai");
                modalChinhSuaDotKeKhai.style.display = 'block';
                var tenDot = document.getElementById("edit-tenDot");
                var ngayBatDau = document.getElementById("edit-startDate");
                var ngayKetThuc = document.getElementById("edit-endDate");
                var hocKy = document.getElementById("edit-hocKy");
                var namHoc = document.getElementById("edit-namHoc");
                var ghiChu = document.getElementById("edit-ghiChu");
                console.log(JSON.stringify(data))
                tenDot.value = data.tenDotKeKhai;
                //ngayBatDau.value = data.ngayBatDau;
              //  ngayKetThuc.value = data.ngayKetThuc;
                hocKy.value = data.hocKy;
                namHoc.value = data.namHoc;

                // Dữ liệu từ hệ thống (ISO 8601)
                const systemDate = data.ngayKetThuc;

                // Chuyển đổi định dạng ngày giờ sang "d/m/Y H:i" bằng moment.js
                const formattedDate = moment(systemDate).format("DD/MM/YYYY HH:mm");

                // Gán giá trị đã định dạng vào thẻ input
               
                ngayKetThuc.value = formattedDate;

                // Khởi tạo flatpickr cho thẻ input
                flatpickr("#edit-endDate", {
                    enableTime: true,
                    dateFormat: "d/m/Y H:i", // Định dạng ngày giờ theo mong muốn
                    time_24hr: true,         // Chế độ 24 giờ
                    defaultDate: formattedDate // Đặt giá trị mặc định
                });


                //gan ngay bat dau
                const ngaybd = data.ngayBatDau;
                const fmtNgaybd = moment(ngaybd).format("DD/MM/YYYY HH:mm");
                ngayBatDau.value = fmtNgaybd;
               



                if (data.ghiChu == null) {
                    ghiChu.value = "";
                } else {
                    ghiChu.value = data.ghiChu;
                }
            } else {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
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


function ChinhSuaDotKeKhaiDangMoHienTai(ngayKetThuc, ghiChu) {
    var formData = new FormData();
    formData.append("ngayKetThuc", convertToISO(ngayKetThuc));
    formData.append("ghiChu", ghiChu);
    $.ajax({
        url: '/Admin/KeKhai/ChinhSuaDotKeKhaiDangMo',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {

                var modalChinhSuaDotKeKhai = document.getElementById("modalChinhSuaDotKeKhai");
                modalChinhSuaDotKeKhai.style.display = 'none';
                //hien thong bao
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Thành Công";
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Green";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
                layRaNgayKeThucKeKhai()
            } else {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
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


function LoadNamHoc() {

    $.ajax({
        url: '/Admin/KeKhai/LoadNamHoc',
        type: 'POST',
        success: function (response) {
            var mainNamHoc = document.getElementById("tbsnamHoc");
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

                        option.value = data[i].id;
                        option.textContent = data[i].tenNamHoc;

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
            var mainHocKy = document.getElementById("tbshocKy");
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
                        option.value = data[i].maHocKy;
                        option.textContent = data[i].tenHocKy;
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