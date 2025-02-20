//load nam học

function LoadNamHoc() {

    $.ajax({
        url: '/Admin/KeKhai/LoadNamHoc',
        type: 'POST',
        success: function (response) {
            var mainNamHoc = document.getElementById("namHocPhanCong");
            var namPhanCong = document.getElementById("namPhanCong");
            namPhanCong.innerHTML = '';
            mainNamHoc.innerHTML = '';
            if (response.success) {
                var data = response.data;

                var chonNamHoc = document.createElement("option");
                chonNamHoc.selected = true;
                chonNamHoc.value = "";
                chonNamHoc.textContent = "Chọn năm học";

                mainNamHoc.appendChild(chonNamHoc);

                var chonNamHocTheoDotPhanCong = document.createElement("option");
                chonNamHocTheoDotPhanCong.selected = true;
                chonNamHocTheoDotPhanCong.value = "";
                chonNamHocTheoDotPhanCong.textContent = "Chọn năm học";

                namPhanCong.appendChild(chonNamHocTheoDotPhanCong)

                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement("option");

                        option.value = data[i].id;
                        option.textContent = data[i].tenNamHoc;

                        mainNamHoc.appendChild(option);

                        var optionNamHocPhanCong = document.createElement("option");
                        optionNamHocPhanCong.value = data[i].id;
                        optionNamHocPhanCong.textContent = data[i].tenNamHoc;

                        namPhanCong.appendChild(optionNamHocPhanCong);

                    }
                }
                mainNamHoc.addEventListener('change', function () {
                    var selectedId = mainNamHoc.value;
                    if (selectedId != -1) {
                        loadDotPhanCongTheoNamHoc(selectedId);
                    }
                });

                namPhanCong.addEventListener('change', function () {
                    var selectedId = namPhanCong.value;
                    if (selectedId != -1) {
                        loadDotPhanCongTheoNamHocTimKiem(selectedId);
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
    loadBangPhanCongHocPhanGanNhat();
});
//load dot phan cong theo nam hoc

function loadDotPhanCongTheoNamHoc(namHoc) {
    var formData = new FormData();
    formData.append("maNamHoc", namHoc);
    console.log("co chay ham chon option");
    $.ajax({
        url: '/Admin/KeKhai/LoadDotPhanCongTheoNamHoc',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            var mainDotPhanCong = document.getElementById("dotPhanCong");
            mainDotPhanCong.innerHTML = '';
            if (response.success) {
                var data = response.data;

                var dotPhanCong = document.createElement('option');
                dotPhanCong.selected = true;
                dotPhanCong.value = "";
                dotPhanCong.textContent = "Chọn học kỳ";

                mainDotPhanCong.appendChild(dotPhanCong);
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement('option');
                        option.value = data[i].id;
                        option.textContent = data[i].tenDot;
                        mainDotPhanCong.appendChild(option);
                    }
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function loadDotPhanCongTheoNamHocTimKiem(namHoc) {
    var formData = new FormData();
    formData.append("maNamHoc", namHoc);
    console.log("co chay ham chon option");
    $.ajax({
        url: '/Admin/KeKhai/LoadDotPhanCongTheoNamHoc',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            var mainDotPhanCong = document.getElementById("dotPhanCongHocPhan");
            mainDotPhanCong.innerHTML = '';
            if (response.success) {
                var data = response.data;

                var dotPhanCong = document.createElement('option');
                dotPhanCong.selected = true;
                dotPhanCong.value = "";
                dotPhanCong.textContent = "Chọn học kỳ";

                mainDotPhanCong.appendChild(dotPhanCong);
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement('option');
                        option.value = data[i].id;
                        option.textContent = data[i].tenDot;
                        mainDotPhanCong.appendChild(option);
                    }
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
// validate form tai file bo sung phan cong
document.addEventListener("DOMContentLoaded", function () {
    // Lấy các phần tử
    const tenDotPhanCong = document.getElementById("dotPhanCong");
    const saveButton = document.getElementById("btnBoSung");
    const tenDotPhanCongErrorMessage = document.getElementById("dotPhanCongError");
    const namHoc = document.getElementById("namHocPhanCong");
    const errNamHoc = document.getElementById("namHocError");
    const fileUpload = document.getElementById("tbsPhanCongHP-inputFile");
    const fileErr = document.getElementById("fileError");

    // Hàm kiểm tra dữ liệu nhập
    function validateInput() {
        let isValid = true;

        if (!tenDotPhanCong.value.trim()) {
            tenDotPhanCong.classList.add("is-invalid");
            tenDotPhanCongErrorMessage.style.display = "block";
            isValid = false;
        } else {
            tenDotPhanCong.classList.remove("is-invalid");
            tenDotPhanCongErrorMessage.style.display = "none";
        }

        if (!namHoc.value.trim()) {
            namHoc.classList.add("is-invalid");
            errNamHoc.style.display = "block";
            isValid = false;
        } else {
            namHoc.classList.remove("is-invalid");
            errNamHoc.style.display = "none";
        }

        if (!fileUpload.files.length) {
            fileUpload.classList.add("is-invalid");
            fileErr.style.display = "block";
            isValid = false;
        } else {
            fileUpload.classList.remove("is-invalid");
            fileErr.style.display = "none";
        }

        return isValid;
    }

    // Bắt sự kiện nút "Lưu"
    saveButton.addEventListener("click", function () {
        if (validateInput()) {
            var file = fileUpload.files[0];
            const tenPhanCong = tenDotPhanCong.value.trim();
            const maNamHoc = namHoc.value;
          
           

            UploadFileBoSungPhanCongHocPhan(file, tenDotPhanCong.value);
            
            // UpFilePhanCong(file, tenPhanCong, maNamHoc);
            console.log("ten phan cong va ma nam hoc: " + tenPhanCong + " - " + maNamHoc);
            // Reset input sau khi thành công
            tenDotPhanCong.value = "";
            fileUpload.value = "";
            namHoc.value = "";
            var modalElement = document.getElementById("modal-boSungPhanCong");
            var modal = bootstrap.Modal.getInstance(modalElement);
            if (modal) {
                modal.hide();
            }

           
            setTimeout(() => {
                document.querySelectorAll(".modal-backdrop").forEach(el => el.remove());
            }, 500);

            
        }
    });
});

// upfile bo sung phan cong

function UploadFileBoSungPhanCongHocPhan(file, idDotPhanCong) {
    var formData = new FormData();
    formData.append("file", file);
    formData.append("idDotPhanCong", idDotPhanCong);
    $.ajax({
        url: '/Admin/KeKhai/UploadFileBoSungPhanCongHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {


            if (response.success) {
                var data = response.data;
              
                console.log(JSON.stringify(response));

               
                if (response.valid) {
                    var tablePhanCong = document.getElementById("tbsPhanCong-table");
                    tablePhanCong.innerHTML = '';
                    var tableBoSung = document.getElementById("mainTablePhanCongBoSungBody");
                    tableBoSung.classList.remove("hide");
                 
                    if (data.length > 0) {
                        for (let i = 0; i < data.length; i++) {

                            var row = document.createElement('tr');
                            row.innerHTML = '<td>' + (i + 1) + '</td>' +
                                '<td>' + data[i].maGV + '</td>' +
                                '<td>' + data[i].tenGV + '</td>' +
                                '<td>' + data[i].maHP + '</td>' +
                                '<td>' + data[i].tenHP + '</td>' +
                                '<td>' + data[i].tenLop + '</td>' +
                                '<td>' + data[i].siSo + '</td>' +
                                '<td>' + data[i].hocKy + '</td>' +
                                '<td>' + data[i].namHoc + '</td>' +
                                '<td>' + data[i].hinhThucDay + '</td>' +
                                '<td>' + data[i].thoiGianDay + '</td>' +
                                '<td style="color: red">' + data[i].ghiChu + '</td>';
                            tablePhanCong.appendChild(row);
                        }
                      
                    } else {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td colspan="12" style="text-align: center;">Không có dữ liệu hiện thị!</td>'
                        tablePhanCong.appendChild(row);
                    }
                        //co loi khi phan cong bang file bo sung (du lieuu bi trung hoac khong hop le)
                     var title = document.getElementById("Modal-main-err-Label");
                        title.innerHTML = '';
                        title.innerHTML = "Lỗi";
                        var elements = document.getElementsByClassName("Modal-main-err-header");
                        for (var i = 0; i < elements.length; i++) {
                            elements[i].style.backgroundColor = "Red";
                        }
                        const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                        errorModal.show();
                        var textmain = document.getElementById("text-main");
                        textmain.innerHTML = "Phân công học phần kê khai bị trùng với đợt phân công trước đó!";
                } else {
                    var mainTable = document.getElementById("maintableBoSungPhanCong");
                    mainTable.innerHTML = '';
                    var tablePhanCong = document.getElementById("mainTablePhanCongBoSungBody");
                    tablePhanCong.classList.add("hide");
                    if (data.length > 0) {
                        mainTable.innerHTML = '';
                        for (let i = 0; i < data.length; i++) {
                            var row = document.createElement("tr");
                            row.innerHTML = '<td><input type="checkbox" class="rowCheckbox"></td>' +
                                '<td>' + (i + 1) + '</td>' +
                                '<td>' + data[i].maGV + '</td>' +
                                '<td>' + data[i].tenGV + '</td>' +
                                '<td>' + data[i].maHP + '</td>' +
                                '<td>' + data[i].tenHP + '</td>' +
                                '<td>' + data[i].tenLop + '</td>' +
                                '<td>' + data[i].thoiGianDay + '</td>' +
                                '<td>' +
                                '<button class="btn btn-primary"  onclick="xemThongTinPhanCongTheoMa(' + data[i].maPhanCongHocPhan +')" data-bs-toggle="modal" data-bs-target="#modalXemPhanCong">Xem</button>' +
                                '<button onclick="loadThongTinCapNhat(' + data[i].maPhanCongHocPhan +')" data-bs-toggle="modal" data-bs-target="#modalChinhSuaPhanCong" class="btn btn-warning">Cập nhật</button>' +
                                '</td>';
                            mainTable.appendChild(row);
                        }
                            var title = document.getElementById("Modal-main-err-Label");
                            title.innerHTML = '';
                            title.innerHTML = "Thông Báo";
                            var elements = document.getElementsByClassName("Modal-main-err-header");
                            for (var i = 0; i < elements.length; i++) {
                                elements[i].style.backgroundColor = "Green";
                            }
                            const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                            errorModal.show();
                            var textmain = document.getElementById("text-main");
                            textmain.innerHTML = response.message;
                    } else {
                        var row = document.createElement("tr");
                        row.innerHTML = '<td colspan="9" class="text-center">Không có dữ liệu hiển thị</td>';
                        mainTable.appendChild(row);
                    }

                }
             
            } else {

                //var modal = document.getElementById('tbsPhanCongHP-modal');
                //modal.classList.remove('show');

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
//load phan cog ra bang 
function loadBangPhanCongHocPhanGanNhat(page=1, pageSize=5) {
    var formData = new FormData();
 
    formData.append("page", page);
    formData.append("pageSize", pageSize);

    console.log("co chay ham chon option");
    $.ajax({
        url: '/Admin/KeKhai/loadTableHocPhanDaPhanCongGanNhat',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var data = response.data;
                var mainTableHocPhanDaPhanCong = document.getElementById("maintableBoSungPhanCong");
                mainTableHocPhanDaPhanCong.innerHTML = '';
                if (data.length > 0) {
                    var soLuongPhanCong = document.getElementById("soLuongPhanCong");
                    soLuongPhanCong.innerHTML = '';
                    soLuongPhanCong.innerHTML = data.length;
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td><input type="checkbox" value="' + data[i].maPhanCongHocPhan + '" class="rowCheckbox"></td>' +
                            '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + (data[i].maGV) + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].maHP + '</td>' +
                            '<td>' + data[i].tenHP + '</td>' +
                            '<td>' + data[i].tenLop + '</td>' +
                            '<td>' + data[i].thoiGianDay + '</td>' +
                            '<td>'+
                            '<button class="btn btn-primary"  onclick="xemThongTinPhanCongTheoMa(' + data[i].maPhanCongHocPhan +')" data-bs-toggle="modal" data-bs-target="#modalXemPhanCong"> Xem</button>'+
                            '<button onclick="loadThongTinCapNhat(' + data[i].maPhanCongHocPhan +')" data-bs-toggle="modal" data-bs-target="#modalChinhSuaPhanCong" class="btn btn-warning">Cập nhật</button>'+
                            '</td>';
                        mainTableHocPhanDaPhanCong.appendChild(row);
                    }
                    updatePhanTrangTablePhanCongHocPhanTheoDotGanNhat(response.totalPages, response.currentPage);
                    var namPhanCong = document.getElementById("namPhanCong");
                    var dotPhanCongHocPhan = document.getElementById("dotPhanCongHocPhan");
                 
                    setTimeout(() => {
                        namPhanCong.value = response.maNamHoc;
                        namPhanCong.dispatchEvent(new Event("change"));

                        // Chờ tải danh sách đợt kê khai xong, sau đó mới chọn đợt kê khai
                        setTimeout(() => {
                            dotPhanCongHocPhan.value = response.maDotPhanCong;
                            dotPhanCongHocPhan.dispatchEvent(new Event("change"));
                        }, 500);
                    }, 500);
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="8" style="text-align: center;">Không có sữ liệu hiện thị!</td>';
                    mainTableHocPhanDaPhanCong.appendChild(row);
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function updatePhanTrangTablePhanCongHocPhanTheoDotGanNhat(totalPages, currentPage) {
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanGanNhat(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanGanNhat(' + page + ')">' + label + '</a>';
        pagination.appendChild(li);
    }

    // Nút "Trước" (Về trang đầu)
    createPageItem(1, "Trước", false, currentPage === 1);

    // Hiển thị tối đa 3 trang gần currentPage
    let startPage = Math.max(1, currentPage - 1);
    let endPage = Math.min(totalPages, currentPage + 1);

    for (let i = startPage; i <= endPage; i++) {
        createPageItem(i, i, currentPage === i);
    }

    // Nút "Sau" (Về trang cuối)
    createPageItem(totalPages, "Sau", false, currentPage === totalPages);
}

/// xen phan cong hoc phan theo maPhanCong


document.addEventListener("DOMContentLoaded", function () {
    var btnLamMoi = document.getElementById("btnLamMoi");
    btnLamMoi.addEventListener("click", function () {
        reloadPage();
    });
    loadDanhSachKhoa();
    var btnXemPhanCongHocPhan = document.getElementById("btnXemPhanCongHocPhan");
    var dotPhanCong = document.getElementById("dotPhanCongHocPhan");

    function toggleButtonState() {
        var hasValue = dotPhanCong.value; // Kiểm tra nếu có giá trị
        btnXemPhanCongHocPhan.disabled = !hasValue;
    }

    function handleButtonClick() {
        if (dotPhanCong.value) {
            loadBangPhanCongHocPhanTheoMaPhanCong(dotPhanCong.value);
        }
    }

    // Gán sự kiện change để cập nhật button và sự kiện click
    dotPhanCong.addEventListener("change", function () {
        toggleButtonState();
        btnXemPhanCongHocPhan.removeEventListener("click", handleButtonClick); // Xóa sự kiện cũ
        btnXemPhanCongHocPhan.addEventListener("click", handleButtonClick); // Gán sự kiện mới
    });

    // Kiểm tra trạng thái ban đầu khi trang tải
    toggleButtonState();
});


function loadBangPhanCongHocPhanTheoMaPhanCong(page = 1, pageSize = 5) {
    var dotPhanCong = document.getElementById("dotPhanCongHocPhan");
    var formData = new FormData();

    formData.append("page", page);
    formData.append("pageSize", pageSize);
    formData.append("maDotPhanCong", dotPhanCong.value);
   
    $.ajax({
        url: '/Admin/KeKhai/loadTableHocPhanDaPhanCongTheoDot',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log("da chay su kien tim kiem theo dot phan cong" + JSON.stringify(response));
            if (response.success) {
                var data = response.data;
                var mainTableHocPhanDaPhanCong = document.getElementById("maintableBoSungPhanCong");
                mainTableHocPhanDaPhanCong.innerHTML = '';
                if (data.length > 0) {
                    var soLuongPhanCong = document.getElementById("soLuongPhanCong");
                    soLuongPhanCong.innerHTML = '';
                    soLuongPhanCong.innerHTML = data.length;
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td><input type="checkbox" value="' + data[i].maPhanCongHocPhan + '" class="rowCheckbox"></td>' +
                            '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + (data[i].maGV) + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].maHP + '</td>' +
                            '<td>' + data[i].tenHP + '</td>' +
                            '<td>' + data[i].tenLop + '</td>' +
                            '<td>' + data[i].thoiGianDay + '</td>' +
                            '<td>' +
                            '<button class="btn btn-primary"  onclick="xemThongTinPhanCongTheoMa(' + data[i].maPhanCongHocPhan +')" data-bs-toggle="modal" data-bs-target="#modalXemPhanCong"> Xem</button>' +
                            '<button onclick="loadThongTinCapNhat(' + data[i].maPhanCongHocPhan +')" data-bs-toggle="modal" data-bs-target="#modalChinhSuaPhanCong" class="btn btn-warning">Cập nhật</button>' +
                            '</td>';
                        mainTableHocPhanDaPhanCong.appendChild(row);
                    }
                    updatePhanTrangTableHocPhanKeKhaiTheoDot(response.totalPages, response.currentPage);
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="8" style="text-align: center;">Không có sữ liệu hiện thị!</td>';
                    mainTableHocPhanDaPhanCong.appendChild(row);
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function updatePhanTrangTableHocPhanKeKhaiTheoDot(totalPages, currentPage) {
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanTheoMaPhanCong(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanTheoMaPhanCong(' + page + ')">' + label + '</a>';
        pagination.appendChild(li);
    }

    // Nút "Trước" (Về trang đầu)
    createPageItem(1, "Trước", false, currentPage === 1);

    // Hiển thị tối đa 3 trang gần currentPage
    let startPage = Math.max(1, currentPage - 1);
    let endPage = Math.min(totalPages, currentPage + 1);

    for (let i = startPage; i <= endPage; i++) {
        createPageItem(i, i, currentPage === i);
    }

    // Nút "Sau" (Về trang cuối)
    createPageItem(totalPages, "Sau", false, currentPage === totalPages);
}

//load khoa 

function loadDanhSachKhoa() {
    $.ajax({
        url: '/Admin/KeKhai/loadThongTinKhoa',
        type: 'POST',
        success: function (response) {
            var mainKhoa = document.getElementById("khoa");
            mainKhoa.innerHTML = ''; // Làm trống dropdown trước khi thêm mới

            if (response.success) {
                var data = response.data;

                var chonKhoa = document.createElement("option");
                chonKhoa.selected = true;
                chonKhoa.value = "";
                chonKhoa.textContent = "Chọn khoa";

                mainKhoa.appendChild(chonKhoa);

                if (data.length > 0) {
                    // Thêm các lựa chọn khoa vào dropdown
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement("option");
                        option.value = data[i].maKhoa;
                        option.textContent = data[i].tenKhoa;
                        mainKhoa.appendChild(option);
                    }
                }
            }

            // Lưu lại sự kiện xử lý để đảm bảo removeEventListener hoạt động chính xác
            function handleLocPhanTheoKhoa() {
                loadBangPhanCongHocPhanTheoKhoa();
            }

            function handleLocPhanTheoKhoaVaChuoiTim() {
                loadBangPhanCongHocPhanTheoChuoiTimVaKhoa();
            }

            // Sự kiện khi chọn Khoa
            mainKhoa.addEventListener("change", function () {
                var selectedId = mainKhoa.value.trim();
                var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                var chuoiTim = document.getElementById("txtChuoiTim").value.trim();

                // Xóa tất cả sự kiện click trước đó để tránh đăng ký nhiều lần
                btnLocPhanTheoKhoa.removeEventListener("click", handleLocPhanTheoKhoa);
                btnLocPhanTheoKhoa.removeEventListener("click", handleLocPhanTheoKhoaVaChuoiTim);

                if (selectedId !== "" && chuoiTim === "") {
                    btnLocPhanTheoKhoa.disabled = false;
                    btnLocPhanTheoKhoa.addEventListener("click", handleLocPhanTheoKhoa);
                } else if (selectedId !== "" && chuoiTim !== "") {
                    btnLocPhanTheoKhoa.disabled = false;
                    btnLocPhanTheoKhoa.addEventListener("click", handleLocPhanTheoKhoaVaChuoiTim);
                } else {
                    btnLocPhanTheoKhoa.disabled = true;
                }
            });


         
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//lam moi trang
function reloadPage() {
    location.reload();
}

//load thong tin vao form them moi phan cong
document.addEventListener("DOMContentLoaded", function () {
    loadNamHocFormPhanCong();
    loadDanhSachKhoaFormPhanCong();
    loadDanhSachHocPhanFormPhanCong();
});

function loadNamHocFormPhanCong() {

    $.ajax({
        url: '/Admin/KeKhai/LoadNamHoc',
        type: 'POST',
        success: function (response) {
            var mainNamHoc = document.getElementById("namHocThenPhanCong");
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
                        loadDotPhanCongTheoNamHocFormPhanCong(selectedId);
                    }
                });
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function loadDotPhanCongTheoNamHocFormPhanCong(namHoc) {
    var formData = new FormData();
    formData.append("maNamHoc", namHoc);
    console.log("co chay ham chon option");
    $.ajax({
        url: '/Admin/KeKhai/LoadDotPhanCongTheoNamHoc',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            var mainDotPhanCong = document.getElementById("dotPhanCongThemMoi");
            mainDotPhanCong.innerHTML = '';
            if (response.success) {
                var data = response.data;

                var dotPhanCong = document.createElement('option');
                dotPhanCong.selected = true;
                dotPhanCong.value = "";
                dotPhanCong.textContent = "Chọn học kỳ";

                mainDotPhanCong.appendChild(dotPhanCong);
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement('option');
                        option.value = data[i].id;
                        option.textContent = data[i].tenDot;
                        mainDotPhanCong.appendChild(option);
                    }
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function loadDanhSachKhoaFormPhanCong() {
    $.ajax({
        url: '/Admin/KeKhai/loadThongTinKhoa',
        type: 'POST',
        success: function (response) {
            var mainKhoa = document.getElementById("khoaThemPhanCong");
            mainKhoa.innerHTML = ''; // Làm trống dropdown trước khi thêm mới

            if (response.success) {
                var data = response.data;

                var chonKhoa = document.createElement("option");
                chonKhoa.selected = true;
                chonKhoa.value = "";
                chonKhoa.textContent = "Chọn khoa";

                mainKhoa.appendChild(chonKhoa);

                if (data.length > 0) {
                    // Thêm các lựa chọn khoa vào dropdown
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement("option");
                        option.value = data[i].maKhoa;
                        option.textContent = data[i].tenKhoa;
                        mainKhoa.appendChild(option);
                    }
                }
            }

            // Gán sự kiện thay đổi cho dropdown khoa
            mainKhoa.addEventListener('change', function () {
                var selectedId = mainKhoa.value;
                if (selectedId != -1) {
                    loadGiangVienTheoKhoa(selectedId);
                }
            });

            // Gán sự kiện lọc cho nút "Lọc"
            //var btnlocPhanCong = document.getElementById("btnLocPhanTheoKhoa");
            //btnlocPhanCong.addEventListener("click", handleLocClick);
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function loadGiangVienTheoKhoa(maKhoa) {
    var formData = new FormData();
    formData.append("maKhoa", maKhoa);
  
    $.ajax({
        url: '/Admin/KeKhai/GetDanhSachGiangVienTheoKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            var mainGiangVien = document.getElementById("giangVienThemPhanCong");
            mainGiangVien.innerHTML = '';
            if (response.success) {
                var data = response.data;

                var giangVien = document.createElement('option');
                giangVien.selected = true;
                giangVien.value = "";
                giangVien.textContent = "Chọn giảng viên";

                mainGiangVien.appendChild(giangVien);
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement('option');
                        option.value = data[i].maGV;
                        option.textContent = data[i].tenGV;
                        mainGiangVien.appendChild(option);
                    }
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function loadDanhSachHocPhanFormPhanCong() {
    $.ajax({
        url: '/Admin/KeKhai/GetSanhSachHocPhan',
        type: 'POST',
        success: function (response) {
            var mainHP = document.getElementById("hocPhanThemPhanCong");
            mainHP.innerHTML = ''; // Làm trống dropdown trước khi thêm mới

            if (response.success) {
                var data = response.data;

                var chonHP = document.createElement("option");
                chonHP.selected = true;
                chonHP.value = "";
                chonHP.textContent = "Chọn học phâ";

                mainHP.appendChild(chonHP);

                if (data.length > 0) {
                    // Thêm các lựa chọn khoa vào dropdown
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement("option");
                        option.value = data[i].maHP;
                        option.textContent = data[i].tenHP;
                        mainHP.appendChild(option);
                    }
                }
            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

//validate form them phan cong moi
document.getElementById("btnThemMoiPhanCong").addEventListener("click", function () {
    let isValid = true;

    // Gán giá trị vào các biến riêng
    let namHocThenPhanCong, khoaThemPhanCong, giangVienThemPhanCong, namHocGiangDayThemPhanCong, hocKyThemPhanCong;
    let hocPhanThemPhanCong, dotPhanCongThemMoi, thoiGianDayThemPhanCong, tenLopThemPhanCong, siSoThemPhanCong, hinhThucDayThemPhanCong;

    // Kiểm tra và gán giá trị vào từng biến
    let namHocThenPhanCongElement = document.getElementById("namHocThenPhanCong");
    if (namHocThenPhanCongElement && namHocThenPhanCongElement.value.trim() !== "") {
        namHocThenPhanCong = namHocThenPhanCongElement.value.trim();
    } else {
        document.getElementById("namHocThenPhanCongError").innerText = "Vui lòng chọn năm học phân công.";
        isValid = false;
    }

    let khoaThemPhanCongElement = document.getElementById("khoaThemPhanCong");
    if (khoaThemPhanCongElement && khoaThemPhanCongElement.value.trim() !== "") {
        khoaThemPhanCong = khoaThemPhanCongElement.value.trim();
    } else {
        document.getElementById("khoaThemPhanCongError").innerText = "Vui lòng chọn khoa.";
        isValid = false;
    }

    let giangVienThemPhanCongElement = document.getElementById("giangVienThemPhanCong");
    if (giangVienThemPhanCongElement && giangVienThemPhanCongElement.value.trim() !== "") {
        giangVienThemPhanCong = giangVienThemPhanCongElement.value.trim();
    } else {
        document.getElementById("giangVienThemPhanCongError").innerText = "Vui lòng chọn giảng viên dạy.";
        isValid = false;
    }

    let namHocGiangDayThemPhanCongElement = document.getElementById("namHocGiangDayThemPhanCong");
    if (namHocGiangDayThemPhanCongElement && namHocGiangDayThemPhanCongElement.value.trim() !== "") {
        namHocGiangDayThemPhanCong = namHocGiangDayThemPhanCongElement.value.trim();
    } else {
        document.getElementById("namHocGiangDayThemPhanCongError").innerText = "Vui lòng nhập năm học giảng dạy.";
        isValid = false;
    }

    let hocKyThemPhanCongElement = document.getElementById("hocKyThemPhanCong");
    if (hocKyThemPhanCongElement && hocKyThemPhanCongElement.value.trim() !== "") {
        hocKyThemPhanCong = hocKyThemPhanCongElement.value.trim();
    } else {
        document.getElementById("hocKyThemPhanCongError").innerText = "Vui lòng nhập học kỳ.";
        isValid = false;
    }

    let hocPhanThemPhanCongElement = document.getElementById("hocPhanThemPhanCong");
    if (hocPhanThemPhanCongElement && hocPhanThemPhanCongElement.value.trim() !== "") {
        hocPhanThemPhanCong = hocPhanThemPhanCongElement.value.trim();
    } else {
        document.getElementById("hocPhanThemPhanCongError").innerText = "Vui lòng chọn học phần.";
        isValid = false;
    }

    let dotPhanCongThemMoiElement = document.getElementById("dotPhanCongThemMoi");
    if (dotPhanCongThemMoiElement && dotPhanCongThemMoiElement.value.trim() !== "") {
        dotPhanCongThemMoi = dotPhanCongThemMoiElement.value.trim();
    } else {
        document.getElementById("dotPhanCongThemMoiError").innerText = "Vui lòng chọn đợt phân công.";
        isValid = false;
    }

    let thoiGianDayThemPhanCongElement = document.getElementById("thoiGianDayThemPhanCong");
    if (thoiGianDayThemPhanCongElement && thoiGianDayThemPhanCongElement.value.trim() !== "") {
        thoiGianDayThemPhanCong = thoiGianDayThemPhanCongElement.value.trim();
    } else {
        document.getElementById("thoiGianDayThemPhanCongError").innerText = "Vui lòng nhập thời gian dạy.";
        isValid = false;
    }

    let tenLopThemPhanCongElement = document.getElementById("tenLopThemPhanCong");
    if (tenLopThemPhanCongElement && tenLopThemPhanCongElement.value.trim() !== "") {
        tenLopThemPhanCong = tenLopThemPhanCongElement.value.trim();
    } else {
        document.getElementById("tenLopThemPhanCongError").innerText = "Vui lòng nhập tên lớp.";
        isValid = false;
    }

    let siSoThemPhanCongElement = document.getElementById("siSoThemPhanCong");
    if (siSoThemPhanCongElement && siSoThemPhanCongElement.value.trim() !== "" && !isNaN(siSoThemPhanCongElement.value) && parseInt(siSoThemPhanCongElement.value) > 0) {
        siSoThemPhanCong = parseInt(siSoThemPhanCongElement.value);
    } else {
        document.getElementById("siSoThemPhanCongError").innerText = "Sĩ số phải là số nguyên dương và lớn hơn 0.";
        isValid = false;
    }

    let hinhThucDayThemPhanCongElement = document.getElementById("hinhThucDayThemPhanCong");
    if (hinhThucDayThemPhanCongElement && hinhThucDayThemPhanCongElement.value.trim() !== "") {
        hinhThucDayThemPhanCong = hinhThucDayThemPhanCongElement.value.trim();
    } else {
        document.getElementById("hinhThucDayThemPhanCongError").innerText = "Vui lòng nhập hình thức dạy.";
        isValid = false;
    }

    // Nếu hợp lệ, thực hiện xử lý
    if (isValid) {



        ThemMoiPhanCong(dotPhanCongThemMoi, giangVienThemPhanCong, hocPhanThemPhanCong, namHocGiangDayThemPhanCong, hocKyThemPhanCong, thoiGianDayThemPhanCong, hinhThucDayThemPhanCong, tenLopThemPhanCong, siSoThemPhanCong);

        var modalElement = document.getElementById("modalAddGiangVien");
        var modal = bootstrap.Modal.getInstance(modalElement);
        if (modal) {
            modal.hide();
        }

        setTimeout(() => {
            document.querySelectorAll(".modal-backdrop").forEach(el => el.remove());
        }, 500);

        namHocThenPhanCongElement.value = "";
        khoaThemPhanCongElement.value = "";
        giangVienThemPhanCongElement.value = "";
        namHocGiangDayThemPhanCongElement.value = "";
        hocKyThemPhanCongElement.value = "";
        hocPhanThemPhanCongElement.value = "";
        dotPhanCongThemMoiElement.value = "";
        thoiGianDayThemPhanCongElement.value = "";
        tenLopThemPhanCongElement.value = "";
        siSoThemPhanCongElement.value = "";
        hinhThucDayThemPhanCongElement.value = "";
    }
});


// them moi phan cônng

function ThemMoiPhanCong(maDotPhanCong, maGV, maHP, namHoc, hocKy, ngayDay, hinhThuc, tenLop, siSo ) {
    var formData = new FormData();
    formData.append("maDotPhanCong", maDotPhanCong);
    formData.append("maGV", maGV);
    formData.append("maHP", maHP);
    formData.append("namHoc", namHoc);
    formData.append("hocKy", hocKy);
    formData.append("ngayDay", ngayDay);
    formData.append("hinhThuc", hinhThuc);
    formData.append("tenLop", tenLop);
    formData.append("siSo", siSo);
  
    $.ajax({
        url: '/Admin/KeKhai/ThemHocPhanPhanCongMoi',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
          
            document.querySelectorAll(".modal-backdrop").forEach(el => el.remove());
            if (response.success) {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Thông Báo";
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Green";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
                var closeButtons = document.querySelectorAll('#Modal-main-err .btn-close, #modalClosebtn');

                // Gán sự kiện cho cả hai nút đóng
                closeButtons.forEach(function (button) {
                    button.addEventListener("click", function () {
                        reloadPage();  // Gọi hàm reloadPage() khi nhấn nút đóng
                    });
                });
                errorModal.addEventListener("click", function (event) {
                    // Kiểm tra xem người dùng có nhấn vào lớp phủ ngoài modal không
                    if (event.target === modal) {
                        reloadPage();  // Gọi hàm reloadPage() nếu người dùng nhấn ngoài modal
                    }
                });
            } else {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Red";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
                var closeButtons = document.querySelectorAll('#Modal-main-err .btn-close, #modalClosebtn');

                // Gán sự kiện cho cả hai nút đóng
                closeButtons.forEach(function (button) {
                    button.addEventListener("click", function () {
                        reloadPage();  // Gọi hàm reloadPage() khi nhấn nút đóng
                    });
                });
                errorModal.addEventListener("click", function (event) {
                    // Kiểm tra xem người dùng có nhấn vào lớp phủ ngoài modal không
                    if (event.target === modal) {
                        reloadPage();  // Gọi hàm reloadPage() nếu người dùng nhấn ngoài modal
                    }
                });
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

///loc theo khoa cong tac
function loadBangPhanCongHocPhanTheoKhoa(page = 1, pageSize = 5) {
    var khoa = document.getElementById("khoa").value;
    var maPhanCong = document.getElementById("dotPhanCongHocPhan").value;
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    formData.append("maDotPhanCong", maPhanCong);
    formData.append("maKhoa", khoa);

    $.ajax({
        url: '/Admin/KeKhai/loadTableHocPhanDaPhanCongTheoKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
          
            if (response.success) {
                var data = response.data;
                var mainTableHocPhanDaPhanCong = document.getElementById("maintableBoSungPhanCong");
                mainTableHocPhanDaPhanCong.innerHTML = '';
                if (data.length > 0) {
                    var soLuongPhanCong = document.getElementById("soLuongPhanCong");
                    soLuongPhanCong.innerHTML = '';
                    soLuongPhanCong.innerHTML = data.length;
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td><input type="checkbox" value="' + data[i].maPhanCongHocPhan + '" class="rowCheckbox"></td>' +
                            '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + (data[i].maGV) + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].maHP + '</td>' +
                            '<td>' + data[i].tenHP + '</td>' +
                            '<td>' + data[i].tenLop + '</td>' +
                            '<td>' + data[i].thoiGianDay + '</td>' +
                            '<td>' +
                            '<button class="btn btn-primary" onclick="xemThongTinPhanCongTheoMa(' + data[i].maPhanCongHocPhan+')" data-bs-toggle="modal" data-bs-target="#modalXemPhanCong"> Xem</button>' +
                            '<button onclick="loadThongTinCapNhat(' + data[i].maPhanCongHocPhan+')" data-bs-toggle="modal" data-bs-target="#modalChinhSuaPhanCong" class="btn btn-warning">Cập nhật</button>' +
                            '</td>';
                        mainTableHocPhanDaPhanCong.appendChild(row);
                    }
                    updatePhanTrangTableHocPhanKeKhaiTheoKhoa(response.totalPages, response.currentPage);
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="8" style="text-align: center;">Không có sữ liệu hiện thị!</td>';
                    mainTableHocPhanDaPhanCong.appendChild(row);
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function updatePhanTrangTableHocPhanKeKhaiTheoKhoa(totalPages, currentPage) {
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanTheoKhoa(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanTheoKhoa(' + page + ')">' + label + '</a>';
        pagination.appendChild(li);
    }

    // Nút "Trước" (Về trang đầu)
    createPageItem(1, "Trước", false, currentPage === 1);

    // Hiển thị tối đa 3 trang gần currentPage
    let startPage = Math.max(1, currentPage - 1);
    let endPage = Math.min(totalPages, currentPage + 1);

    for (let i = startPage; i <= endPage; i++) {
        createPageItem(i, i, currentPage === i);
    }

    // Nút "Sau" (Về trang cuối)
    createPageItem(totalPages, "Sau", false, currentPage === totalPages);
}

// load thong tin form xem
function xemThongTinPhanCongTheoMa(maDotPhanCong) {
 
    var formData = new FormData();
    formData.append("maDotPhanCong", maDotPhanCong);
    $.ajax({
        url: '/Admin/KeKhai/XemThongTinPhanCongTheoMa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            if (response.success) {
                var data = response.data;
               
                var xemMaGV = document.getElementById("xemMaGV");
                xemMaGV.innerHTML = '';
                xemMaGV.innerHTML = data.maGV;
                var xemTenGV = document.getElementById("xemTenGV");
                xemTenGV.innerHTML = '';
                xemTenGV.innerHTML = data.tenGV;
                var xemMaHP = document.getElementById("xemMaHP");
                xemMaHP.innerHTML = '';
                xemMaHP.innerHTML = data.maHP;
                var xemTenHP = document.getElementById("xemTenHP");
                xemTenHP.innerHTML = '';
                xemTenHP.innerHTML = data.tenHP;
                var xemTenLop = document.getElementById("xemTenLop");
                xemTenLop.innerHTML = '';
                xemTenLop.innerHTML = data.tenLop;
                var xemNamHoc = document.getElementById("xemNamHoc");
                xemNamHoc.innerHTML = '';
                xemNamHoc.innerHTML = data.namHoc;
                var xemHocKy = document.getElementById("xemHocKy");
                xemHocKy.innerHTML = '';
                xemHocKy.innerHTML = data.hocKy;
                var xemHinhThuc = document.getElementById("xemHinhThuc");
                xemHinhThuc.innerHTML = '';
                xemHinhThuc.innerHTML = data.hinhThucDay;
                var xemThoiGian = document.getElementById("xemThoiGian");
                xemThoiGian.innerHTML = '';
                xemThoiGian.innerHTML = data.thoiGianDay;
                var xemSiSo = document.getElementById("xemSiSo");

                xemSiSo.innerHTML = '';
                xemSiSo.innerHTML = data.siSo;

            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

//load form edit khoa, giang vien
document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachKhoaFormCapNhat();

});


function loadDanhSachKhoaFormCapNhat() {
    $.ajax({
        url: '/Admin/KeKhai/loadThongTinKhoa',
        type: 'POST',
        success: function (response) {
            var mainKhoa = document.getElementById("editKhoaThemPhanCong");
            mainKhoa.innerHTML = ''; // Làm trống dropdown trước khi thêm mới

            if (response.success) {
                var data = response.data;

                var chonKhoa = document.createElement("option");
                chonKhoa.selected = true;
                chonKhoa.value = "";
                chonKhoa.textContent = "Chọn khoa";

                mainKhoa.appendChild(chonKhoa);

                if (data.length > 0) {
                    // Thêm các lựa chọn khoa vào dropdown
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement("option");
                        option.value = data[i].maKhoa;
                        option.textContent = data[i].tenKhoa;
                        mainKhoa.appendChild(option);
                    }
                }
            }

            // Gán sự kiện thay đổi cho dropdown khoa
            mainKhoa.addEventListener('change', function () {
                var selectedId = mainKhoa.value;
                if (selectedId != -1) {
                    loadGiangVienCapNhatTheoKhoa(selectedId);
                }
            });

            // Gán sự kiện lọc cho nút "Lọc"
            //var btnlocPhanCong = document.getElementById("btnLocPhanTheoKhoa");
            //btnlocPhanCong.addEventListener("click", handleLocClick);
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function loadGiangVienCapNhatTheoKhoa(maKhoa) {
    var formData = new FormData();
    formData.append("maKhoa", maKhoa);

    $.ajax({
        url: '/Admin/KeKhai/GetDanhSachGiangVienTheoKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            var mainGiangVien = document.getElementById("editGiangVienThemPhanCong");
            mainGiangVien.innerHTML = '';
            if (response.success) {
                var data = response.data;

                var giangVien = document.createElement('option');
                giangVien.selected = true;
                giangVien.value = "";
                giangVien.textContent = "Chọn giảng viên";

                mainGiangVien.appendChild(giangVien);
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement('option');
                        option.value = data[i].maGV;
                        option.textContent = data[i].tenGV;
                        mainGiangVien.appendChild(option);
                    }
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//load form cap nhat
function loadThongTinCapNhat(maDotPhanCong) {
    var formData = new FormData();
    formData.append("maDotPhanCong", maDotPhanCong);
    $.ajax({
        url: '/Admin/KeKhai/XemThongTinPhanCongTheoMa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            if (response.success) {
                var data = response.data;
                var editKhoaThemPhanCong = document.getElementById("editKhoaThemPhanCong");
                var giangVien = document.getElementById("editGiangVienThemPhanCong");

                setTimeout(() => {
                    editKhoaThemPhanCong.value = data.maKhoa;
                    editKhoaThemPhanCong.dispatchEvent(new Event("change"));

                    // Chờ tải danh sách đợt kê khai xong, sau đó mới chọn đợt kê khai
                    setTimeout(() => {
                        giangVien.value = data.maGV;
                    }, 500);
                }, 500);
                var editNamHocGiangDayThemPhanCong = document.getElementById("editNamHocGiangDayThemPhanCong");
                editNamHocGiangDayThemPhanCong.value = data.namHoc;
                var editHocKyThemPhanCong = document.getElementById("editHocKyThemPhanCong");
                editHocKyThemPhanCong.value = data.hocKy;
                var editHocPhanThemPhanCong = document.getElementById("editHocPhanThemPhanCong");
                editHocPhanThemPhanCong.value = data.tenHP;
                var editThoiGianDayThemPhanCong = document.getElementById("editThoiGianDayThemPhanCong");
                editThoiGianDayThemPhanCong.value = data.thoiGianDay;
                var editTenLopThemPhanCong = document.getElementById("editTenLopThemPhanCong");
                editTenLopThemPhanCong.value = data.tenLop;
                var editSiSoThemPhanCong = document.getElementById("editSiSoThemPhanCong");
                editSiSoThemPhanCong.value = data.siSo;
                var editHinhThucDayThemPhanCong = document.getElementById("editHinhThucDayThemPhanCong");
                editHinhThucDayThemPhanCong.value = data.hinhThucDay;
                var editMaPhanCongHocPhan = document.getElementById("editMaPhanCongHocPhan");
                editMaPhanCongHocPhan.value = data.maPhanCongHocPhan;
                var editmaHocPhan = document.getElementById("editmaHocPhan");
                editmaHocPhan.value = data.maHP;
            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

//validate form cap nhat
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("btnLuuCapNhatPhanCong").addEventListener("click", function () {
        let isValid = true;

        // Hàm kiểm tra và hiển thị lỗi
        function validateField(id, errorId, errorMessage) {
            let field = document.getElementById(id);
            let errorField = document.getElementById(errorId);
            if (!field.value.trim()) {
                errorField.textContent = errorMessage;
                isValid = false;
            } else {
                errorField.textContent = "";
            }
        }

        // Kiểm tra các trường select
        validateField("editKhoaThemPhanCong", "editKhoaThemPhanCongError", "Vui lòng chọn khoa.");
        validateField("editGiangVienThemPhanCong", "editGiangVienThemPhanCongError", "Vui lòng chọn giảng viên.");

        // Kiểm tra các trường input text
        validateField("editNamHocGiangDayThemPhanCong", "editNamHocGiangDayThemPhanCongError", "Vui lòng nhập năm học.");
        validateField("editHocKyThemPhanCong", "editHocKyThemPhanCongError", "Vui lòng nhập học kỳ.");
        validateField("editThoiGianDayThemPhanCong", "editThoiGianDayThemPhanCongError", "Vui lòng nhập thời gian dạy.");
        validateField("editTenLopThemPhanCong", "editTenLopThemPhanCongError", "Vui lòng nhập tên lớp.");
        validateField("editHinhThucDayThemPhanCong", "editHinhThucDayThemPhanCongError", "Vui lòng nhập hình thức dạy.");

        // Kiểm tra trường Sĩ số (phải là số nguyên dương)
        let siSoField = document.getElementById("editSiSoThemPhanCong");
        let siSoError = document.getElementById("editSiSoThemPhanCongError");
        if (!siSoField.value.trim() || parseInt(siSoField.value) < 1) {
            siSoError.textContent = "Sĩ số phải là số nguyên dương.";
            isValid = false;
        } else {
            siSoError.textContent = "";
        }

        // Nếu hợp lệ, thực hiện hành động (ví dụ: gửi dữ liệu)
        if (isValid) {
            var maPhanCongHocPhan = document.getElementById("editMaPhanCongHocPhan").value.trim();
            var maGV = document.getElementById("editGiangVienThemPhanCong").value;
            var tenLop = document.getElementById("editTenLopThemPhanCong").value;
            var maHP = document.getElementById("editmaHocPhan").value;
            var namHoc = document.getElementById("editNamHocGiangDayThemPhanCong").value;
            var hocKy = document.getElementById("editHocKyThemPhanCong").value;
            var thoiGianDay = document.getElementById("editThoiGianDayThemPhanCong").value;
            var hinhThucDay = document.getElementById("editHinhThucDayThemPhanCong").value;
            var siSo = document.getElementById("editSiSoThemPhanCong").value;

            capNhatThongTinPhanCongHocPhan(maPhanCongHocPhan, maGV, tenLop, maHP, namHoc, hocKy, thoiGianDay, hinhThucDay, siSo);

            var modalElement = document.getElementById("modalChinhSuaPhanCong");
            var modal = bootstrap.Modal.getInstance(modalElement);
            if (modal) {
                modal.hide();
            }

            setTimeout(() => {
                document.querySelectorAll(".modal-backdrop").forEach(el => el.remove());
            }, 500);

        }
    });
});

//cap  nhat thong tin phan cong hoc phan
//string maGV, string tenLop, string namHoc, string hocKy, string thoiGianDay, string maHP, string hinhThucDay, int siSo
function capNhatThongTinPhanCongHocPhan(maPhanCongHocPhan, maGV, tenLop, maHP, namHoc, hocKy, thoiGianDay, hinhThucDay, siSo) {
    var formData = new FormData();
    formData.append("maPhanCongHocPhan", maPhanCongHocPhan);
    formData.append("maGV", maGV);
    formData.append("tenLop", tenLop);
    formData.append("maHP", maHP);
    formData.append("namHoc", namHoc);
    formData.append("hocKy", hocKy);
    formData.append("thoiGianDay", thoiGianDay);
    formData.append("hinhThucDay", hinhThucDay);
    formData.append("siSo", siSo);

    $.ajax({
        url: '/Admin/KeKhai/CapNhatThongTinPhanCongTheoMa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {

            if (response.success) {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Thông Báo";
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Green";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
                var closeButtons = document.querySelectorAll('#Modal-main-err .btn-close, #modalClosebtn');

                // Gán sự kiện cho cả hai nút đóng
                closeButtons.forEach(function (button) {
                    button.addEventListener("click", function () {
                        reloadPage();  // Gọi hàm reloadPage() khi nhấn nút đóng
                    });
                });
                errorModal.addEventListener("click", function (event) {
                    // Kiểm tra xem người dùng có nhấn vào lớp phủ ngoài modal không
                    if (event.target === modal) {
                        reloadPage();  // Gọi hàm reloadPage() nếu người dùng nhấn ngoài modal
                    }
                });
            } else {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Red";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
                var closeButtons = document.querySelectorAll('#Modal-main-err .btn-close, #modalClosebtn');

                // Gán sự kiện cho cả hai nút đóng
                closeButtons.forEach(function (button) {
                    button.addEventListener("click", function () {
                        reloadPage();  // Gọi hàm reloadPage() khi nhấn nút đóng
                    });
                });
                errorModal.addEventListener("click", function (event) {
                    // Kiểm tra xem người dùng có nhấn vào lớp phủ ngoài modal không
                    if (event.target === modal) {
                        reloadPage();  // Gọi hàm reloadPage() nếu người dùng nhấn ngoài modal
                    }
                });
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//xoa phan cong hoc phan

document.addEventListener("DOMContentLoaded", function () {
    var btnXoaPhanCong = document.getElementById("btnXoaPhanCong");
    btnXoaPhanCong.addEventListener("click", function () {
        XacNhanXoaPhanCong();
    });
});



function XacNhanXoaPhanCong() {

    const checkboxes = document.querySelectorAll('.rowCheckbox');

    const selectedIds = [];
    checkboxes.forEach(checkbox => {
        if (checkbox.checked) {
            selectedIds.push(checkbox.value);
        }
    });

    if (selectedIds.length === 0) {
        alert("Vui lòng chọn ít nhất một mục để xóa.");
        return;
    }


    var modalConfirmKhoa = document.getElementById("modalConfirmKhoa");
    modalConfirmKhoa.style.display = "block";
   
    const confirmBtn = document.getElementById("confirmBtn");
    confirmBtn.addEventListener("click", function () {
        xoaDanhSach(selectedIds);
        modalConfirmKhoa.style.display = "none";

    });
}


function xoaDanhSach(idPhanCongs) {

    var formData = new FormData();
    formData.append("idPhanCongs", JSON.stringify(idPhanCongs));
    $.ajax({
        url: '/Admin/KeKhai/XoaDanhSachPhanCong',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Thông Báo";
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Green";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
                var closeButtons = document.querySelectorAll('#Modal-main-err .btn-close, #modalClosebtn');

                // Gán sự kiện cho cả hai nút đóng
                closeButtons.forEach(function (button) {
                    button.addEventListener("click", function () {
                        reloadPage();  // Gọi hàm reloadPage() khi nhấn nút đóng
                    });
                });
                errorModal.addEventListener("click", function (event) {
                    // Kiểm tra xem người dùng có nhấn vào lớp phủ ngoài modal không
                    if (event.target === modal) {
                        reloadPage();  // Gọi hàm reloadPage() nếu người dùng nhấn ngoài modal
                    }
                });
            } else {
                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Red";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
                var closeButtons = document.querySelectorAll('#Modal-main-err .btn-close, #modalClosebtn');

                // Gán sự kiện cho cả hai nút đóng
                closeButtons.forEach(function (button) {
                    button.addEventListener("click", function () {
                        reloadPage();  // Gọi hàm reloadPage() khi nhấn nút đóng
                    });
                });
                errorModal.addEventListener("click", function (event) {
                    // Kiểm tra xem người dùng có nhấn vào lớp phủ ngoài modal không
                    if (event.target === modal) {
                        reloadPage();  // Gọi hàm reloadPage() nếu người dùng nhấn ngoài modal
                    }
                });
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//tim kiem
document.addEventListener("DOMContentLoaded", function () {
    var txtChuoiTim = document.getElementById("txtChuoiTim");
    var btnTimKiemPhanCong = document.getElementById("btnTimKiemPhanCong");
    function timKiemPhanCong() {
        loadBangPhanCongHocPhanTheoChuoiTim();
    }

    txtChuoiTim.addEventListener("input", function () {
        if (txtChuoiTim.value.trim() !== "") {
           
            btnTimKiemPhanCong.disabled = false;
            btnTimKiemPhanCong.addEventListener("click", timKiemPhanCong);
        } else {
            btnTimKiemPhanCong.disabled = true;
            btnTimKiemPhanCong.removeEventListener("click", timKiemPhanCong);
        }
    });
});

//load tabel theo chuoi tim 

function loadBangPhanCongHocPhanTheoChuoiTim(page = 1, pageSize = 5) {
    var chuoiTim = document.getElementById("txtChuoiTim").value;
    var maPhanCong = document.getElementById("dotPhanCongHocPhan").value;
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    formData.append("maDotPhanCong", maPhanCong);
    formData.append("chuoiTim", chuoiTim);

    $.ajax({
        url: '/Admin/KeKhai/loadTablePhanCongHocPhanTheoChuoiTim',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            var khoa = document.getElementById("khoa").value = "";
            if (response.success) {
                var data = response.data;
                var mainTableHocPhanDaPhanCong = document.getElementById("maintableBoSungPhanCong");
                mainTableHocPhanDaPhanCong.innerHTML = '';
                if (data.length > 0) {
                    var soLuongPhanCong = document.getElementById("soLuongPhanCong");
                    soLuongPhanCong.innerHTML = '';
                    soLuongPhanCong.innerHTML = data.length;
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td><input type="checkbox" value="' + data[i].maPhanCongHocPhan + '" class="rowCheckbox"></td>' +
                            '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + (data[i].maGV) + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].maHP + '</td>' +
                            '<td>' + data[i].tenHP + '</td>' +
                            '<td>' + data[i].tenLop + '</td>' +
                            '<td>' + data[i].thoiGianDay + '</td>' +
                            '<td>' +
                            '<button class="btn btn-primary" onclick="xemThongTinPhanCongTheoMa(' + data[i].maPhanCongHocPhan + ')" data-bs-toggle="modal" data-bs-target="#modalXemPhanCong"> Xem</button>' +
                            '<button onclick="loadThongTinCapNhat(' + data[i].maPhanCongHocPhan + ')" data-bs-toggle="modal" data-bs-target="#modalChinhSuaPhanCong" class="btn btn-warning">Cập nhật</button>' +
                            '</td>';
                        mainTableHocPhanDaPhanCong.appendChild(row);
                    }
                    updatePhanTrangTableHocPhanKeKhaiTheoChuoiTim(response.totalPages, response.currentPage);
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="8" style="text-align: center;">Không có sữ liệu hiện thị!</td>';
                    mainTableHocPhanDaPhanCong.appendChild(row);
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function updatePhanTrangTableHocPhanKeKhaiTheoChuoiTim(totalPages, currentPage) {
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanTheoChuoiTim(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanTheoChuoiTim(' + page + ')">' + label + '</a>';
        pagination.appendChild(li);
    }

    // Nút "Trước" (Về trang đầu)
    createPageItem(1, "Trước", false, currentPage === 1);

    // Hiển thị tối đa 3 trang gần currentPage
    let startPage = Math.max(1, currentPage - 1);
    let endPage = Math.min(totalPages, currentPage + 1);

    for (let i = startPage; i <= endPage; i++) {
        createPageItem(i, i, currentPage === i);
    }

    // Nút "Sau" (Về trang cuối)
    createPageItem(totalPages, "Sau", false, currentPage === totalPages);
}

//tim kiem theo chuoi tim va khoa

function loadBangPhanCongHocPhanTheoChuoiTimVaKhoa(page = 1, pageSize = 5) {
    var chuoiTim = document.getElementById("txtChuoiTim").value;
    var maPhanCong = document.getElementById("dotPhanCongHocPhan").value;
    var khoa = document.getElementById("khoa");
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    formData.append("maDotPhanCong", maPhanCong);
    formData.append("chuoiTim", chuoiTim);
    formData.append("maKhoa", khoa.value);

    $.ajax({
        url: '/Admin/KeKhai/loadTablePhanCongHocPhanTheoChuoiTimVaKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {

            if (response.success) {
                var data = response.data;
                var mainTableHocPhanDaPhanCong = document.getElementById("maintableBoSungPhanCong");
                mainTableHocPhanDaPhanCong.innerHTML = '';
                if (data.length > 0) {
                    var soLuongPhanCong = document.getElementById("soLuongPhanCong");
                    soLuongPhanCong.innerHTML = '';
                    soLuongPhanCong.innerHTML = data.length;
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td><input type="checkbox" value="' + data[i].maPhanCongHocPhan + '" class="rowCheckbox"></td>' +
                            '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + (data[i].maGV) + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].maHP + '</td>' +
                            '<td>' + data[i].tenHP + '</td>' +
                            '<td>' + data[i].tenLop + '</td>' +
                            '<td>' + data[i].thoiGianDay + '</td>' +
                            '<td>' +
                            '<button class="btn btn-primary" onclick="xemThongTinPhanCongTheoMa(' + data[i].maPhanCongHocPhan + ')" data-bs-toggle="modal" data-bs-target="#modalXemPhanCong"> Xem</button>' +
                            '<button onclick="loadThongTinCapNhat(' + data[i].maPhanCongHocPhan + ')" data-bs-toggle="modal" data-bs-target="#modalChinhSuaPhanCong" class="btn btn-warning">Cập nhật</button>' +
                            '</td>';
                        mainTableHocPhanDaPhanCong.appendChild(row);
                    }
                    updatePhanTrangTableHocPhanKeKhaiTheoChuoiTimVaKhoa(response.totalPages, response.currentPage);
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="8" style="text-align: center;">Không có sữ liệu hiện thị!</td>';
                    mainTableHocPhanDaPhanCong.appendChild(row);
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function updatePhanTrangTableHocPhanKeKhaiTheoChuoiTimVaKhoa(totalPages, currentPage) {
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanTheoChuoiTimVaKhoa(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadBangPhanCongHocPhanTheoChuoiTimVaKhoa(' + page + ')">' + label + '</a>';
        pagination.appendChild(li);
    }

    // Nút "Trước" (Về trang đầu)
    createPageItem(1, "Trước", false, currentPage === 1);

    // Hiển thị tối đa 3 trang gần currentPage
    let startPage = Math.max(1, currentPage - 1);
    let endPage = Math.min(totalPages, currentPage + 1);

    for (let i = startPage; i <= endPage; i++) {
        createPageItem(i, i, currentPage === i);
    }

    // Nút "Sau" (Về trang cuối)
    createPageItem(totalPages, "Sau", false, currentPage === totalPages);
}
