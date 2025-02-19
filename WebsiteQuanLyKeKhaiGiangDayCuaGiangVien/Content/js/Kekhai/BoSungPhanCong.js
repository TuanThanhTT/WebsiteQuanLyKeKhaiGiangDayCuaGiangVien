//load nam học

function LoadNamHoc() {

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

                        option.value = data[i].id;
                        option.textContent = data[i].tenNamHoc;

                        mainNamHoc.appendChild(option);

                    }
                }
                mainNamHoc.addEventListener('change', function () {
                    var selectedId = mainNamHoc.value;
                    if (selectedId != -1) {
                        loadDotPhanCongTheoNamHoc(selectedId);
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

            UploadFileBoSungPhanCongHocPhan(file);

            // UpFilePhanCong(file, tenPhanCong, maNamHoc);
            console.log("ten phan cong va ma nam hoc: " + tenPhanCong + " - " + maNamHoc);
            // Reset input sau khi thành công
            tenDotPhanCong.value = "";
            fileUpload.value = "";
        }
    });
});

// upfile bo sung phan cong

function UploadFileBoSungPhanCongHocPhan(file) {
    var formData = new FormData();
    formData.append("file", file)
    $.ajax({
        url: '/Admin/KeKhai/UploadFileBoSungPhanCongHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {


            if (response.success) {
                var data = response.data;
                var mainTable = document.getElementById("maintableBoSungPhanCong");
                console.log(JSON.stringify(response));

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
                            '<button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#">Xem</button>' +
                            '<button class="btn btn-warning">Cập nhật</button>' +
                            '</td>';

                        //row.innerHTML = '<td>' + (i + 1) + '</td>' +
                        //    '<td>' + data[i].maGV + '</td>' +
                        //    '<td>' + data[i].tenGV + '</td>' +
                        //    '<td>' + data[i].maHP + '</td>' +
                        //    '<td>' + data[i].tenHP + '</td>' +
                        //    '<td>' + data[i].tenLop + '</td>' +
                        //    '<td>' + data[i].siSo + '</td>' +
                        //    '<td>' + data[i].hocKy + '</td>' +
                        //    '<td>' + data[i].namHoc + '</td>' +
                        //    '<td>' + data[i].hinhThucDay + '</td>' +
                        //    '<td>' + data[i].thoiGianDay + '</td>' +
                        //    '<td style="color: red">' + data[i].ghiChu + '</td>';

                        /**
                         * 
                         *   <td><input type="checkbox" class="rowCheckbox"></td>
                            <td>1</td>
                            <td>1</td>
                            <td>2024-2025</td>
                            <td>2024-2025</td>
                            <td>2024-2025</td>
                            <td>2024-2025</td>
                            <td>
                                <button class="btn btn-primary" data-bs-toggle="modal"
                                        data-bs-target="#">
                                    Xem
                                </button>
                                <button class="btn btn-warning">Cập nhật</button>
                            </td>
                         * */
                        mainTable.appendChild(row);
                        
                    }

                   
                    if (!response.valid) {
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
                    }
                } else {
                    var row = document.createElement("tr");
                    row.innerHTML = '<td colspan="9" class="text-center">Không có dữ liệu hiển thị</td>';
                    mainTable.appendChild(row);
                }
            } else {

                var modal = document.getElementById('tbsPhanCongHP-modal');
                modal.classList.remove('show');


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
