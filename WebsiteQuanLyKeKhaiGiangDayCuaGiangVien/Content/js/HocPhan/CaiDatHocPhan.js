


function updatePhanTrangTableHocPhan(totalPages, currentPage) {
   

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachHocPhan(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachHocPhan(' + page + ')">' + label + '</a>';
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

document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachHocPhan();
});

function loadDanhSachHocPhan(page = 1, pageSize = 5) {
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/HocPhan/LoadDanhSachHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            if (response.success) {
                var data = response.data;
                var mainTable = document.getElementById("maintableHocPhan");
                mainTable.innerHTML = '';
                if (data.length > 0) {

                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');

                        row.innerHTML = '<td><input type="checkbox" value="' + data[i].maHocPhan + '" class="rowCheckbox"></td>' +
                            '<td>' + ((page - 1) * pageSize + 1 + i) + '</td>' +
                            '<td>' + data[i].maHocPhan + '</td>' +
                            '<td>' + data[i].tenHocPhan + '</td>' +
                            '<td>' +
                            '<button class="btn btn-primary" onclick="XemChiTietHocPhan(\'' + data[i].maHocPhan + '\')" data-bs-toggle="modal" data-bs-target="#modalXemChiTiet">Xem</button>' +
                            '<button class="btn btn-warning" onclick="loadThongTinCapNhatHocPhan(\'' + data[i].maHocPhan + '\')">Cập nhật</button>' +
                            '</td>';

                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableHocPhan(response.totalPages, response.currentPage);

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan= "7" style="text-align: center;">Không có dữ liệu hiện thị</td>'
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//them hoc phan

document.addEventListener("DOMContentLoaded", function () {
    var saveButton = document.getElementById("btnSave");  // Nút Lưu

    saveButton.addEventListener("click", function () {
        var isValid = true;

        // Lấy tất cả các trường cần kiểm tra
        var fields = [
            { id: "maHocPhan", message: "Mã học phần không được để trống." },
            { id: "tenHocPhan", message: "Tên học phần không được để trống." },
            { id: "soTinChi", message: "Số tín chỉ không được để trống." },
            { id: "lyThuyet", message: "Số chỉ lý thuyết không được để trống." },
            { id: "thucHanh", message: "Số chỉ thực hành không được để trống." },
         
        ];

        // Reset lại các lỗi trước
        resetErrors();

        // Kiểm tra tất cả các trường, nếu có trường nào bỏ trống, thêm lỗi
        fields.forEach(function (field) {
            var input = document.getElementById(field.id);
            var errorElement = document.getElementById(field.id + "Error");

            // Kiểm tra trường input (text, email, number, etc.)
            if (input && input.tagName.toLowerCase() !== "select" && !input.value.trim()) {
                input.classList.add("error");
                if (errorElement) {
                    errorElement.textContent = field.message;
                }
                isValid = false;
            }

          
        });

        // Nếu tất cả các trường hợp lệ, thực hiện xử lý khác
        if (isValid) {
            
            var maHocPhan = document.getElementById("maHocPhan").value.trim();
            var tenHocPhan = document.getElementById("tenHocPhan").value.trim();
            var soTinChi = document.getElementById("soTinChi").value.trim();
            var lyThuyet = document.getElementById("lyThuyet").value.trim();
            var thucHanh = document.getElementById("thucHanh").value.trim();
            var ghiChu = document.getElementById("ghiChu").value.trim();
         
            themHocPhanMoi(maHocPhan, tenHocPhan, soTinChi, lyThuyet, thucHanh, ghiChu);

        }
    });

    function resetErrors() {
        var errorMessages = document.querySelectorAll(".error-message");
        var inputs = document.querySelectorAll(".form-control");

        errorMessages.forEach(function (message) {
            message.textContent = "";
        });

        inputs.forEach(function (input) {
            input.classList.remove("error");
        });
    }
});


function themHocPhanMoi(maHocPhan, tenHocPhan, soTinChi, lyThuyet, thucHanh, ghiChu) {
    var formData = new FormData();
    formData.append("maHocPhan", maHocPhan);
    formData.append("tenHocPhan", tenHocPhan);
    formData.append("soTinChi", soTinChi);
    formData.append("lyThuyet", lyThuyet);
    formData.append("thucHanh", thucHanh);
    formData.append("ghiChu", ghiChu);
  
    $.ajax({
        url: '/Admin/HocPhan/ThemHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {

            var title = document.getElementById("errorModalLabel");
            var elements = document.getElementsByClassName("modal-mess-header");
            const errorModal = new bootstrap.Modal(document.getElementById("custom-error-modal"));
            var textmain = document.getElementById("text-main");

            if (response.success) {
                title.innerHTML = '';
                title.innerHTML = "Thành Công";
                title.style.color = "white";
                document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "green";
                }
                textmain.innerHTML = response.message;
                closeModalById("modalAddHocPhan");
                errorModal.show();
                var closeButton = document.getElementById('dongModalBaoLoi');
                if (closeButton) {
                    closeButton.addEventListener('click', function () {

                        location.reload();
                    });
                } else { console.error("Close button not found"); }
            } else {
                if (response.errName) {

                    var errText = document.getElementById(response.errName);
                    errText.innerHTML = '';
                    errText.innerHTML = response.message;
                    errText.style.display = 'block';


                } else {
                    
                        title.innerHTML = "Lỗi";
                        title.style.color = "white";
                        document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                        for (var i = 0; i < elements.length; i++) {
                            elements[i].style.backgroundColor = "red";
                        }
                        textmain.innerHTML = response.message;
                        closeModalById("modalAddHocPhan");
                        errorModal.show();
                        var closeButton = document.getElementById('dongModalBaoLoi');
                        if (closeButton) {
                            closeButton.addEventListener('click', function () {

                                location.reload();
                            });
                        } else { console.error("Close button not found"); }
                   
                   
                }

            }

            // Hiển thị modal

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function closeModalById(modalId) {
    var modalElement = document.getElementById(modalId);

    if (modalElement && modalElement.classList.contains("show")) {
        var modalInstance = bootstrap.Modal.getInstance(modalElement);

        if (modalInstance) {
            modalInstance.hide(); // Đóng modal
        }
    }
}
//xem chi tiet hoc phan

function XemChiTietHocPhan(maHocPhan) {
    var formData = new FormData();

    formData.append("maHocPhan", String(maHocPhan));

    $.ajax({
        url: '/Admin/HocPhan/LayThongTinHocPhanTheoMa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(response);
            if (response.success) {
                var data = response.data;
                var xemMaHocPhan = document.getElementById("xemMaHocPhan");
                xemMaHocPhan.innerHTML = '';
                xemMaHocPhan.innerHTML = data.maHp;
                var xemTenHocPhan = document.getElementById("xemTenHocPhan");
                xemTenHocPhan.innerHTML = '';
                xemTenHocPhan.innerHTML = data.tenHp;
                var xemSoTinChi = document.getElementById("xemSoTinChi");
                xemSoTinChi.innerHTML = '';
                xemSoTinChi.innerHTML = data.soTinChi;
                var xemLyThuyet = document.getElementById("xemLyThuyet");
                xemLyThuyet.innerHTML = '';
                xemLyThuyet.innerHTML = data.lyThuyet;
                var xemThucHanh = document.getElementById("xemThucHanh");
                xemThucHanh.innerHTML = '';
                xemThucHanh.innerHTML = data.thucHanh;
                var xemGhiChu = document.getElementById("xemGhiChu");
                xemGhiChu.innerHTML = '';
                xemGhiChu.innerHTML = data.moTa;
               
            } else {
                alert("Có lỗi xảy ra: " + response.message);
            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//tai file hoc phan
document.addEventListener("DOMContentLoaded", function () {
    var chonFile = document.getElementById("fileInput");
    var btnTaiFile = document.getElementById("btnTaiFile");


    btnTaiFile.disabled = true;

    chonFile.addEventListener("change", function () {

        if (chonFile.files && chonFile.files.length > 0) {
            btnTaiFile.disabled = false;
        } else {
            btnTaiFile.disabled = true;
        }
    });

    btnTaiFile.addEventListener("click", function () {
        uploadFileThemHocPhan(chonFile.files[0]);
    });
});


function uploadFileThemHocPhan(file) {
    var formData = new FormData();
    formData.append("file", file);
    $.ajax({
        url: '/Admin/HocPhan/UploadFileHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            if (response.success) {


                var errMess = document.getElementById("errMess");
                errMess.innerHTML = '';
                errMess.style.display = 'none';

                var data = response.data;
                var mainTable = document.getElementById("filePreviewTable");
                mainTable.innerHTML = '';
                if (data.length > 0) {
                    var soLuongGiangVien = document.getElementById("soLuongHocPhan");
                    soLuongGiangVien.innerHTML = '';
                    soLuongGiangVien.innerHTML = data.length;
                    var soLuongTailen = document.getElementById("soLuongTailen");
                    soLuongTailen.style.display = 'block';
                    for (let i = 0; i < data.length; i++) {
                        var colortext = 'green';
                        var row = document.createElement('tr');
                        var ghiChu = "Hợp lệ";

                        if (data[i].ghiChuLoi) {
                            ghiChu = data[i].ghiChuLoi;
                            colortext = "red";
                        }


                        row.innerHTML = '<td>' + data[i].id + '</td>' +
                            '<td>' + data[i].maHP + '</td>' +
                            '<td>' + data[i].tenHP + '</td>' +
                            '<td>' + data[i].soTinChi + '</td>' +
                            '<td>' + data[i].lyThuyet + '</td>' +
                            '<td>' + data[i].thucHanh + '</td>' +
                            '<td>' + data[i].ghiChu + '</td>' +                     
                            '<td style="color: ' + colortext + ';">' + ghiChu + '</td>';

                        mainTable.appendChild(row);
                    }
                }
                var saveFileData = document.getElementById("saveFileData");
                if (response.valid) {

                    saveFileData.style.visibility = 'visible';
                    saveFileData.addEventListener('click', function () {
                        LuuDanhSachHocPhanThem();
                    });

                } else {
                    var soLuongTailen = document.getElementById("soLuongTailen");
                    soLuongTailen.style.display = 'none';
                    saveFileData.style.visibility = 'hidden';
                    var errMess = document.getElementById("errMess");
                    errMess.innerHTML = '';
                    errMess.innerHTML = '*Dữ liệu file không hợp lệ!'
                    errMess.style.display = 'block';

                }

            } else {

                var errMess = document.getElementById("errMess");
                errMess.innerHTML = '';
                errMess.innerHTML = response.message;
                errMess.style.display = 'block';


            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function LuuDanhSachHocPhanThem() {
    $.ajax({
        url: '/Admin/HocPhan/LuuThongTinHocPhanTaiLen',
        type: 'POST',
        success: function (response) {
            var title = document.getElementById("errorModalLabel");
            var elements = document.getElementsByClassName("modal-mess-header");
            const errorModal = new bootstrap.Modal(document.getElementById("custom-error-modal"));
            var textmain = document.getElementById("text-main");

            if (response.success) {
                title.innerHTML = '';
                title.innerHTML = "Thành Công";
                title.style.color = "white";
                document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "green";
                }
                textmain.innerHTML = response.message;


            } else {
                title.innerHTML = "Lỗi";
                title.style.color = "white";
                document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "red";
                }
                textmain.innerHTML = response.message;
            }

            // Hiển thị modal
            errorModal.show();
            var closeButton = document.getElementById('dongModalBaoLoi');
            if (closeButton) {
                closeButton.addEventListener('click', function () {

                    location.reload();
                });
            } else { console.error("Close button not found"); }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//cap nhat thong tin hoc phan
document.addEventListener("DOMContentLoaded", function () {
    var btnMauFile = document.getElementById("btnMauFile");
    btnMauFile.addEventListener("click", TaiMauFileHocPhan);

    var btnsave = document.getElementById("btneditSave");  // Nút Lưu

    btnsave.addEventListener("click", function () {
        var isValid = true;
        console.log("co chay");
        // Lấy tất cả các trường cần kiểm tra
        var fields = [
            { id: "edittenHocPhan", message: "Tên học phần không được để trống." },
            { id: "editsoTinChi", message: "Số tín chỉ không được để trống." },
            { id: "editlyThuyet", message: "Số tiết lý thuyết không được để trống." },
            { id: "editthucHanh", message: "Số tiết thực hành không được để trống." },
        
        ];

        resetErrors();


        fields.forEach(function (field) {
            var input = document.getElementById(field.id);
            var errorElement = document.getElementById(field.id + "Error");

            if (input && input.tagName.toLowerCase() !== "select" && !input.value.trim()) {
                input.classList.add("error");
                if (errorElement) {
                    errorElement.textContent = field.message;
                }
                isValid = false;
            }


            if (input && input.tagName.toLowerCase() === "select") {

                if (!input.value || input.value === "") {
                    input.classList.add("error");
                    if (errorElement) {
                        errorElement.textContent = field.message;
                    }
                    isValid = false;
                }
            }
        });
        // Nếu tất cả các trường hợp lệ, thực hiện xử lý khác
        if (isValid) {

            var editmaHocPhan = document.getElementById("editmaHocPhan").value.trim();
            var edittenHocPhan = document.getElementById("edittenHocPhan").value.trim();
            var editsoTinChi = document.getElementById("editsoTinChi").value.trim();
            var editlyThuyet = document.getElementById("editlyThuyet").value.trim();
            var editthucHanh = document.getElementById("editthucHanh").value.trim();
            var editghiChu = document.getElementById("editghiChu").value.trim();
            


            capNhatThongTinHocPhan(editmaHocPhan, edittenHocPhan, editsoTinChi, editlyThuyet, editthucHanh, editghiChu);

        }
    });

    function resetErrors() {
        var errorMessages = document.querySelectorAll(".error-message");
        var inputs = document.querySelectorAll(".form-control");

        errorMessages.forEach(function (message) {
            message.textContent = "";
        });

        inputs.forEach(function (input) {
            input.classList.remove("error");
        });
    }
});


function capNhatThongTinHocPhan(maHocPhan, tenHocPhan, soTinChi, lyThuyet, thucHanh, ghiChu) {
    var formData = new FormData();
    formData.append("maHocPhan", maHocPhan);
    formData.append("tenHocPhan", tenHocPhan);
    formData.append("soTinChi", soTinChi);
    formData.append("lyThuyet", lyThuyet);
    formData.append("thucHanh", thucHanh);
    formData.append("ghiChu", ghiChu);
    $.ajax({
        url: '/Admin/HocPhan/CapNhatThongTinHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {

            var title = document.getElementById("errorModalLabel");
            var elements = document.getElementsByClassName("modal-mess-header");
            const errorModal = new bootstrap.Modal(document.getElementById("custom-error-modal"));
            var textmain = document.getElementById("text-main");
            closeModalById("modalUpdateHocPhan");
            if (response.success) {
                title.innerHTML = '';
                title.innerHTML = "Thành Công";
                title.style.color = "white";
                document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "green";
                }
                textmain.innerHTML = response.message;
                closeModalById("modalCapNhatThongTinGiangVien");
                errorModal.show();
                var closeButton = document.getElementById('dongModalBaoLoi');
                if (closeButton) {
                    closeButton.addEventListener('click', function () {

                        location.reload();
                    });
                } else { console.error("Close button not found"); }
            } else {
                if (response.errName) {

                    var errText = document.getElementById(response.errName);
                    errText.innerHTML = '';
                    errText.innerHTML = response.message;
                    errText.style.display = 'block';
                } else {
                    title.innerHTML = "Lỗi";
                    title.style.color = "white";
                    document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                    for (var i = 0; i < elements.length; i++) {
                        elements[i].style.backgroundColor = "red";
                    }
                    textmain.innerHTML = response.message;
                    errorModal.show();
                    var closeButton = document.getElementById('dongModalBaoLoi');
                    if (closeButton) {
                        closeButton.addEventListener('click', function () {

                            location.reload();
                        });
                    } else { console.error("Close button not found"); }
                }

            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function loadThongTinCapNhatHocPhan(maHocPhan) {


    var formData = new FormData();
    formData.append("maHocPhan", maHocPhan);

    $.ajax({
        url: '/Admin/HocPhan/loadThongTinCapNhatHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {

                var data = response.data;
                if (data) {
                    var maHocPhan = document.getElementById("editmaHocPhan");
                    var tenHocPhan = document.getElementById("edittenHocPhan");
                    var soTinChi = document.getElementById("editsoTinChi");
                    var lyThuyet = document.getElementById("editlyThuyet");
                    var thucHanh = document.getElementById("editthucHanh");
                    var ghiChu = document.getElementById("editghiChu");

                    maHocPhan.value = data.maHp;
                    tenHocPhan.value = data.tenHp;
                    soTinChi.value = data.soTinChi;
                    lyThuyet.value = data.lyThuyet;
                    thucHanh.value = data.thucHanh;
                    ghiChu.value = data.moTa;
                  
                }

            } else {
                alert(response.message);
            }
            var modalCapNhatThongTin = new bootstrap.Modal(document.getElementById("modalUpdateHocPhan"));

            modalCapNhatThongTin.show();



        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//tai mau file
function TaiMauFileHocPhan() {
    $.ajax({
        url: '/Admin/HocPhan/DowloadMauFile',
        type: 'POST',
        success: function (data) {
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = 'MauDanhSachHocPhan.xlsx';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },
        xhrFields: {
            responseType: 'blob'
        }
    });

}
// xoa hoc phan
document.addEventListener("DOMContentLoaded", function () {
    var btnXoaGiangVien = document.getElementById("btnXoaHocPhan");

    // Gắn sự kiện click cho nút xóa
    btnXoaGiangVien.addEventListener("click", function () {
        const checkboxes = document.querySelectorAll('.rowCheckbox');
        const isAnyChecked = Array.from(checkboxes).some(checkbox => checkbox.checked);

        if (isAnyChecked) {

            XacNhanXoaHocPhan();
        } else {
            // Hiển thị thông báo nếu không có checkbox nào được chọn
            alert("Vui lòng chọn ít nhất một mục để xóa.");
        }
    });
});

function XacNhanXoaHocPhan() {
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

function xoaDanhSach(idHocPhans) {
    var formData = new FormData();
    formData.append("idHocPhans", JSON.stringify(idHocPhans));
    $.ajax({
        url: '/Admin/HocPhan/XoaDanhSachHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            var title = document.getElementById("errorModalLabel");
            var elements = document.getElementsByClassName("modal-mess-header");
            const errorModal = new bootstrap.Modal(document.getElementById("custom-error-modal"));
            var textmain = document.getElementById("text-main");

            if (response.success) {
                title.innerHTML = '';
                title.innerHTML = "Thành Công";
                title.style.color = "white";
                document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "green";
                }
                textmain.innerHTML = response.message;


            } else {
                title.innerHTML = "Lỗi";
                title.style.color = "white";
                document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "red";
                }
                textmain.innerHTML = response.message;
            }

            // Hiển thị modal
            errorModal.show();
            var closeButton = document.getElementById('dongModalBaoLoi');
            if (closeButton) {
                closeButton.addEventListener('click', function () {

                    location.reload();
                });
            } else { console.error("Close button not found"); }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}