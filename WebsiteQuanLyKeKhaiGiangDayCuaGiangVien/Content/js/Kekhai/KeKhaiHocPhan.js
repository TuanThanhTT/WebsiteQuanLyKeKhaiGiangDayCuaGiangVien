function updatePhanTrangTablePhanCong(totalPages, currentPage) {
 
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadTableHocPhanDuocPhanCong(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadTableHocPhanDuocPhanCong(' + page + ')">' + label + '</a>';
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



function loadTableHocPhanDuocPhanCong(page = 1, pageSize = 5) {
    $.ajax({
        url: '/KeKhai/LoadTableHocPhanKeKhai',
        type: 'POST',
        data: { page: page, pageSize: pageSize },
        success: function (response) {
            console.log("Các học phần đã đc phân công la: " + JSON.stringify(response));
            var data = response.data;
            var soLuong = document.getElementById("soLuong");
            soLuong.innerHTML = '';
            soLuong.innerHTML = response.soLuong;
            var mainTable = document.getElementById("tableKeKhaiHocPhan");
            mainTable.innerHTML = '';
            if (response.success) {
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>'+ ((page - 1) * pageSize + i + 1) +'</td>'+
                            '<td>' + (data[i].maHocPhanPhanCong)+'</td>' +
                            '<td>' + (data[i].tenHocPhanPhanCong) + '</td>' +
                            '<td>' + (data[i].tenLop) + '</td>' +
                            '<td>' + (data[i].siSo) + '</td>' +
                        '<td>'+
                            '<div class="action-buttons">' +
                            '<button onclick="HienThiThongTinPhanCongHocPhan(\'' + data[i].Id + '\')" class="btn-view">Xem</button>' +
                            '<button onclick="hoanThanhKeKhai(\'' + data[i].Id + '\')" class="btn-done">Hoàn Thành</button>'+
                            '<button onclick="hienThongTinSuaPhanCongKeKhai(\'' + data[i].Id + '\')" class="btn-edit">Sửa</button>'+
                            '</div>'+
                            '</td >';
                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTablePhanCong(response.totalPages, response.currentPage);
                }
            } else {
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="6"style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                mainTable.appendChild(row);
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    loadTableHocPhanDuocPhanCong();
    loadTablePhanCongChoDuyet();
});


function updatePhanTrangTableChoDuyet(totalPages, currentPage) {
 
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadTablePhanCongChoDuyet(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadTablePhanCongChoDuyet(' + page + ')">' + label + '</a>';
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


function loadTablePhanCongChoDuyet(page = 1, pageSize = 5) {
    $.ajax({
        url: '/KeKhai/loadPhanCongHocPhanChoKeKhai',
        type: 'POST',
        data: { page: page, pageSize: pageSize },
        success: function (response) {

            var data = response.data;
            var mainTable = document.getElementById("tablePhanCongChoDuyet");
            mainTable.innerHTML = '';
            var soLuongKeKhaiChoDuyet = document.getElementById("soLuongKeKhaiChoDuyet");
            soLuongKeKhaiChoDuyet.innerHTML = response.soLuong;
            if (response.success) {



                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + (data[i].maHocPhanPhanCong) + '</td>' +
                            '<td>' + (data[i].tenHocPhanPhanCong) + '</td>' +
                            '<td>' + (data[i].tenLop) + '</td>' +
                            '<td>' + (data[i].siSo) + '</td>' +
                            '<td>' +
                            '<div class="action-buttons">' +
                            '<button onclick="HienThiThongTinPhanCongHocPhan(\'' + data[i].Id + '\')" class="btn-view">Xem</button>' +
                            '<button onclick="hienThongTinSuaPhanCongKeKhai(\'' + data[i].Id + '\')" class="btn-edit">Sửa</button>' +
                            '</div>' +
                            '</td >';
                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableChoDuyet(response.totalPages, response.currentPage);
                }
            } else {
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="6"style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                mainTable.appendChild(row);
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}




function HienThiThongTinPhanCongHocPhan(maPhanCong) {
    var formData = new FormData();
    formData.append("maPhanCong", maPhanCong);

    console.log("ma phan cong la: " + maPhanCong);
   
    $.ajax({
        url: '/KeKhai/XemThongTinPhanCongHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            var modalInfo = document.getElementById("modal-xem-kekhai");
            

            if (response.success) {
                var data = response.data;
              
                var tenHocPhan = document.getElementById("tenHocPhan");
                var maHocPhan = document.getElementById("maHocPhan");
                var thoiGianDay = document.getElementById("thoiGianDay");
                var hocKy = document.getElementById("hocKy");
                var tenLop = document.getElementById("tenLop");
                var siSo = document.getElementById("siSo");
                var namHoc = document.getElementById("namHoc");
                var hinhThucDay = document.getElementById("hinhThucDay");


                tenHocPhan.innerHTML = '';
                tenHocPhan.innerHTML = data.tenHocPhanPhanCong;
                maHocPhan.innerHTML = '';
                maHocPhan.innerHTML = data.maHocPhanPhanCong;

                thoiGianDay.innerHTML = '';
                thoiGianDay.innerHTML = data.ngayDay;
                hocKy.innerHTML = '';
                hocKy.innerHTML = data.hocKy;
                tenLop.innerHTML = '';
                tenLop.innerHTML = data.tenLop;
                siSo.innerHTML = '';
                siSo.innerHTML = data.siSo;
                namHoc.innerHTML = '';
                namHoc.innerHTML = data.namHoc;

                hinhThucDay.innerHTML = '';
                hinhThucDay.innerHTML = data.hinhThucDay;

                modalInfo.style.display = "block";

            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}



function hienThongTinSuaPhanCongKeKhai(maPhanCong) { 
    var formData = new FormData();
    formData.append("maPhanCong", maPhanCong);


    $.ajax({
        url: '/KeKhai/XemThongTinPhanCongHocPhan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var data = response.data;

                var tenHocPhan = document.getElementById("edit-tenMonHoc");
                var maHocPhan = document.getElementById("edit-maMonHoc");
                var thoiGianDay = document.getElementById("edit-ngayDay");
                var hocKy = document.getElementById("edit-hocKy");
                var tenLop = document.getElementById("edit-tenLop");
                var siSo = document.getElementById("edit-siSo");
                var namHoc = document.getElementById("edit-namHoc");
                var hinhThucDay = document.getElementById("edit-hinhThucDay");

                var maPhanCongHocPhan = document.getElementById("edit-maPhanCongHocPhan");

                maPhanCongHocPhan.value = data.id;

                tenHocPhan.value = data.tenHocPhanPhanCong;
                maHocPhan.value = data.maHocPhanPhanCong;
                thoiGianDay.value = data.ngayDay;
                hocKy.value = data.hocKy;
                tenLop.value = data.tenLop;
                siSo.value = data.siSo;
                namHoc.value = data.namHoc;
                hinhThucDay.value = data.hinhThucDay;

                openModal();
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function hoanThanhKeKhai(maPhanCong) {
    var formData = new FormData();
    formData.append("maPhanCong", maPhanCong);


    $.ajax({
        url: '/KeKhai/HoanThanhKeKhai',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            var modal = document.getElementById("Modal-main-err");
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
                loadTableHocPhanDuocPhanCong();
                loadTablePhanCongChoDuyet();
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
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}



document.addEventListener("DOMContentLoaded", function () {
   
    function validateForm() {
      
        const inputs = document.querySelectorAll(".modal-chinhsua-kekhai-input:not([readonly])");
        let isValid = true;

 
        inputs.forEach((input) => {
            const errorMessage = input.parentElement.querySelector(".error-message");

            if (!input.value.trim()) {
           
                input.classList.add("error");

                
                if (!errorMessage) {
                    const error = document.createElement("div");
                    error.classList.add("error-message");
                    error.textContent = "Dữ liệu này không được để trống.";
                    input.parentElement.appendChild(error);
                }
                isValid = false;
            } else {
             
                input.classList.remove("error");
                if (errorMessage) {
                    errorMessage.remove();
                }
            }
        });

        return isValid;
    }

  
    const saveButton = document.querySelector(".modal-chinhsua-kekhai-footer button:nth-child(2)");
    saveButton.addEventListener("click", function (event) {
      
        if (!validateForm()) {
            event.preventDefault();
        } else {
            var maPhanCongHocPhan = document.getElementById("edit-maPhanCongHocPhan");
            var tenLop = document.getElementById("edit-tenLop");
            var siSo = document.getElementById("edit-siSo");
            var hinhThucDay = document.getElementById("edit-hinhThucDay");


            luuThayDoiPhanCongHocPhan(maPhanCongHocPhan.value, tenLop.value, siSo.value, hinhThucDay.value);

        }
    });
});


function luuThayDoiPhanCongHocPhan(maPhanCong, tenLop, siSo, hinhThucDay) {
    var formData = new FormData();
    formData.append("maPhanCong", maPhanCong);
    formData.append("tenLop", tenLop);
    formData.append("siSo", siSo);
    formData.append("hinhThucDay", hinhThucDay);
    $.ajax({
        url: '/KeKhai/CapNhatThongTinPhanCong',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            var modal = document.getElementById("Modal-main-err");
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
                loadTableHocPhanDuocPhanCong();
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
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


