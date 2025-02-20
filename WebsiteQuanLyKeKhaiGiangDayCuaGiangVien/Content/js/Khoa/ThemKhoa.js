const modalXemChiTiet = new bootstrap.Modal(document.getElementById("modal-messUpdateDepartment"));
document.addEventListener("DOMContentLoaded", function () {
    // Lấy các phần tử
    const modalInput = document.getElementById("tenKhoa");
    const saveButton = document.getElementById("btnLuuKhoa");
    const errorMessage = document.querySelector(".error-message");

    // Hàm kiểm tra dữ liệu nhập
    function validateInput() {
        const inputValue = modalInput.value.trim();

        // Kiểm tra dữ liệu
        if (!inputValue) {
            errorMessage.style.display = "block"; // Hiển thị lỗi
            modalInput.classList.add("is-invalid"); // Thêm border đỏ
            return false;
        } else {
            errorMessage.style.display = "none"; // Ẩn lỗi
            modalInput.classList.remove("is-invalid"); // Xóa border đỏ
            return true;
        }
    }

    // Bắt sự kiện nút "Lưu"
    saveButton.addEventListener("click", function () {
        if (validateInput()) {
            const tenKhoa = modalInput.value.trim();
            themKhoaMoi(tenKhoa);

            modalInput.value = "";
        }
    });

});



function themKhoaMoi(tenKhoa) {
    var formData = new FormData();
    formData.append("tenKhoa", tenKhoa);
    $.ajax({
        url: '/Admin/Khoa/themKhoaMoi',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
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



function updatePhanTrangTableKeKhai(totalPages, currentPage) {
   
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachKhoa(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachKhoa(' + page + ')">' + label + '</a>';
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

function loadDanhSachKhoa(page = 1, pageSize = 5) {
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/Khoa/loadDanhSachKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            if (response.success) {
                var mainTable = document.getElementById("tableKhoa");
                mainTable.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td><input type="checkbox" value="' + data[i].maKhoa + '" class="rowCheckbox"></td>' +
                            '<td>' + ((page - 1) * pageSize+1+i) + '</td>' +
                            '<td>' + data[i].maKhoa + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td> <button class="btn btn-warning" onclick="xemThongTinKhoa(' + data[i].maKhoa + ')" data-bs-toggle="modal" data-bs-target="#modal-messUpdateDepartment">Cập nhật</button></td>';
                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableKeKhai(response.totalPages, response.currentPage);
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="5" style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                    mainTable.appendChild(row);
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachKhoa();
})


function xemThongTinKhoa(maKhoa) {
    var formData = new FormData();
    formData.append("maKhoa", maKhoa);
   
    $.ajax({
        url: '/Admin/Khoa/getThongTinKhoaTheoMa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var data = response.data;
                var tenKhoa = document.getElementById("updateDepartmentName");
                tenKhoa.value = data.tenKhoa;
                var editMaKhoa = document.getElementById("update-maKhoa");
                editMaKhoa.innerHTML = '';
                editMaKhoa.innerHTML = data.maKhoa;

              
                modalXemChiTiet.show();




            } else {
                alert("Có lỗi xảy ra!");
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}



document.addEventListener("DOMContentLoaded", function () {
    // Lấy các phần tử
    const modalInput = document.getElementById("updateDepartmentName");
    const saveButton = document.getElementById("updateDepartment");
    const errorMessage = document.getElementById("update-err");

    // Hàm kiểm tra dữ liệu nhập
    function validateInput() {
        const inputValue = modalInput.value.trim();

        // Kiểm tra dữ liệu
        if (!inputValue) {
            errorMessage.style.display = "block"; // Hiển thị lỗi
            modalInput.classList.add("is-invalid"); // Thêm border đỏ
            return false;
        } else {
            errorMessage.style.display = "none"; // Ẩn lỗi
            modalInput.classList.remove("is-invalid"); // Xóa border đỏ
            return true;
        }
    }

    // Bắt sự kiện nút "Lưu"
    saveButton.addEventListener("click", function () {
        if (validateInput()) {
            const tenKhoa = modalInput.value.trim();
            const maKhoa = document.getElementById("update-maKhoa");
            capNhatKhoa(maKhoa.textContent, tenKhoa);


            modalInput.value = "";
        }
    });

});


function capNhatKhoa(maKhoa, tenKhoa) {
    var formData = new FormData();
    formData.append("maKhoa", maKhoa);
    formData.append("tenKhoa", tenKhoa)
    $.ajax({
        url: '/Admin/Khoa/capNhatKhoa',
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
        uploadFileThemKhoa(chonFile.files[0]);
    });
});






function uploadFileThemKhoa(file) {
    var formData = new FormData();
    formData.append("file", file);
    $.ajax({
        url: '/Admin/Khoa/uploadFileKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {


                var errMess = document.getElementById("errMess");
                errMess.innerHTML = '';
                errMess.style.display = 'none';

                var data = response.data;
                var mainTable = document.getElementById("filePreviewTable");
                mainTable.innerHTML = '';
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var colortext = 'green';
                        var row = document.createElement('tr');
                        var ghiChu = "Hợp lệ";

                        if (data[i].ghiChu) {
                            ghiChu = data[i].ghiChu;
                            colortext = "red";
                        }
                        row.innerHTML = '<td>' + (i + 1) + '</td>' +
                            '<td>' + (data[i].tenKhoa) + '</td>' +
                            '<td style="color: ' + colortext + ';">' + ghiChu + '</td>';
                        mainTable.appendChild(row);
                    }
                }
                var saveFileData = document.getElementById("saveFileData"); b
                if (response.valid) {

                    saveFileData.style.display = 'block';
                    saveFileData.addEventListener('click', function () {
                        LuuDanhSachKhoaThem();
                    });

                } else {
                    saveFileData.style.display = 'none';
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


function LuuDanhSachKhoaThem() {
    $.ajax({
        url: '/Admin/Khoa/LuuDanhSachKhoaTailen',
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


function xoaDanhSach(idKhoas) {
  
    var formData = new FormData();
    formData.append("idKhoas", JSON.stringify(idKhoas));
    $.ajax({
        url: '/Admin/Khoa/XoaKhoaTheoDanhSach',
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


document.addEventListener("DOMContentLoaded", function () {
    var btnMauKhoa = document.getElementById("btnMauKhoa");
    btnMauKhoa.addEventListener("click", TaiMauFileKhoa);

    var xoaKhoa = document.getElementById("xoaKhoa");

    // Gắn sự kiện click cho nút xóa
    xoaKhoa.addEventListener("click", function () {
        const checkboxes = document.querySelectorAll('.rowCheckbox');
        const isAnyChecked = Array.from(checkboxes).some(checkbox => checkbox.checked);

        if (isAnyChecked) {
          
            XacNhanXoaKhoa();
        } else {
            // Hiển thị thông báo nếu không có checkbox nào được chọn
            alert("Vui lòng chọn ít nhất một mục để xóa.");
        }
    });
});




function XacNhanXoaKhoa() {



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

function TaiMauFileKhoa() {
    $.ajax({
        url: '/Admin/Khoa/DowloadMauFile',
        type: 'POST',
        success: function (data) {
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = 'MauDanhSachKhoa.xlsx';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },
        xhrFields: {
            responseType: 'blob'
        }
    });

}