//load khoa
document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachKhoa();
});

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

            // Gán sự kiện thay đổi cho dropdown khoa
            mainKhoa.addEventListener('change', handleKhoaChange);

            // Gán sự kiện lọc cho nút "Lọc"
            var btnlocPhanCong = document.getElementById("btnLocPhanTheoKhoa");
            btnlocPhanCong.addEventListener("click", handleLocClick);
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function handleKhoaChange() {
    var mainKhoa = document.getElementById("khoa");
    var selectedId = mainKhoa.value;

    // Kiểm tra xem có khoa nào được chọn không
    var btnlocKeKhai = document.getElementById("btnLocPhanTheoKhoa");
    if (selectedId !== "") {
        btnlocKeKhai.disabled = false; // Kích hoạt nút lọc nếu có chọn khoa
    } else {
        btnlocKeKhai.disabled = true; // Vô hiệu hóa nút lọc nếu không chọn khoa
    }
}

function handleLocClick() {
    var mainKhoa = document.getElementById("khoa");
    var chuoiTIm = document.getElementById("txtTimKiem");

    var selectedId = mainKhoa.value;
    if (selectedId != "") {
        if (chuoiTIm.value) {
            // Lọc theo khoa và chuoi tim kiem
            loadDanhSachTaiKhoanTheoKhoaVaChuoiTim();
        } else {
            console.log("Ma khoa da chon: " + selectedId);
            // Lọc theo khoa
            loadDanhSachTaiKhoanTheoKhoa();
            
        }
    }
}

//load danh sach tai khoan
document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachTaiKhoan();
});

function loadDanhSachTaiKhoan(page = 1, pageSize = 5) {
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/Account/LoadDanhSachTaiKhoan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response))
            if (response.success) {
                var tableGiangVienHoanThanh = document.getElementById("tableTaiKhoanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].loaiTaiKhoan + '</td>' +
                            '<td>' +
                            '<button onclick="loadQuyenTruyCap(\'' + data[i].maGV + '\')" data-bs-toggle="modal" data-bs-target="#modal-messAddDepartment" class="btn btn-warning"> <i class="fa-solid fa-gear"></i></button>'+
                            '</td>';
                        tableGiangVienHoanThanh.appendChild(row);
                    }
                    updatePhanTrangTableTaiKhoan(response.totalPages, response.currentPage);
                  
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    tableGiangVienHoanThanh.appendChild(row);
                }
            } else {
                var tableGiangVienHoanThanh = document.getElementById("tableTaiKhoanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                tableGiangVienHoanThanh.appendChild(row);
              
            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}



function updatePhanTrangTableTaiKhoan(totalPages, currentPage) {
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachTaiKhoan(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachTaiKhoan(' + page + ')">' + label + '</a>';
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
//load quyen truy cap len modal chinh sua

function loadQuyenTruyCap(maGv) {
    var formData = new FormData();
    console.log("ma giang vien xem quyen la: " + maGv);
    formData.append("maGV", maGv);
    $.ajax({
        url: '/Admin/Account/LoadQuyenTruyCapTheoTaiKhoan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            var loaiTaiKhoan = document.getElementById("loaiTaiKhoan");
            if (response.success) {
                console.log(JSON.stringify(response));
                var data = response.data;
                loaiTaiKhoan.value = data;
                var maTaiKhoan = document.getElementById("maTaiKhoan");
                maTaiKhoan.value = maGv;
                var btnLuuThayDoi = document.getElementById("btnLuuThayDoi");

                btnLuuThayDoi.addEventListener("click", function () {
                    luuQuyenTruyCap(maTaiKhoan.value);

                });
            } else {
                alert(response.message);
            }
        },
      
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

// luu quyen truy cap
function luuQuyenTruyCap(maGv) {
    var loaiTaiKhoan = document.getElementById("loaiTaiKhoan");
    var formData = new FormData();
    formData.append("maGV", maGv);
    formData.append("maQuyenTruyCap", loaiTaiKhoan.value);
    $.ajax({
        url: '/Admin/Account/CapNhatQuyenTruyCap',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            alert(response.message);
            loadDanhSachTaiKhoan();

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

//tim kiem tai khoan
document.addEventListener("DOMContentLoaded", function () {
    var btnTimKiem = document.getElementById("btnTimKiem");
    var chuoiiTim = document.getElementById("txtTimKiem");
    chuoiiTim.addEventListener("input", function () {
        if (chuoiiTim.value) {
            btnTimKiem.disabled = false;
        } else {
            btnTimKiem.disabled = true;
        }
    });
    btnTimKiem.addEventListener("click", function () {
        loadDanhSachTaiKhoanTheoChuoiTim();
        var khoa = document.getElementById("khoa");
        khoa.value = "";
        var btnLoc = document.getElementById("btnLocPhanTheoKhoa");
        btnLoc.disabled = true;
    });
    var btnreload = document.getElementById("btnreload");
    btnreload.addEventListener("click", function () {
        location.reload();
    });
});



function loadDanhSachTaiKhoanTheoChuoiTim(page = 1, pageSize = 5) {
    var formData = new FormData();
    var chuoiTim = document.getElementById("txtTimKiem").value;

    formData.append("chuoiTim", chuoiTim);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/Account/LoaddanhSachTimKiemTaiKhoan',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response))
            if (response.success) {
                var tableGiangVienHoanThanh = document.getElementById("tableTaiKhoanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].loaiTaiKhoan + '</td>' +
                            '<td>' +
                            '<button onclick="loadQuyenTruyCap(\'' + data[i].maGV + '\')" data-bs-toggle="modal" data-bs-target="#modal-messAddDepartment" class="btn btn-warning"> <i class="fa-solid fa-gear"></i></button>' +
                            '</td>';
                        tableGiangVienHoanThanh.appendChild(row);
                    }
                    updatePhanTrangTableTaiKhoanTheoChuoiTim(response.totalPages, response.currentPage);

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    tableGiangVienHoanThanh.appendChild(row);
                }
            } else {
                var tableGiangVienHoanThanh = document.getElementById("tableTaiKhoanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                tableGiangVienHoanThanh.appendChild(row);

            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function updatePhanTrangTableTaiKhoanTheoChuoiTim(totalPages, currentPage) {
  
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachTaiKhoanTheoChuoiTim(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachTaiKhoanTheoChuoiTim(' + page + ')">' + label + '</a>';
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
//loc theo khoa

function loadDanhSachTaiKhoanTheoKhoa(page = 1, pageSize = 5) {
    var formData = new FormData();
    var mainKhoa = document.getElementById("khoa");

    formData.append("maKhoa", mainKhoa.value);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/Account/LoadDanhSachtaiKhoanTheoKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response))
            if (response.success) {
                var tableGiangVienHoanThanh = document.getElementById("tableTaiKhoanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].loaiTaiKhoan + '</td>' +
                            '<td>' +
                            '<button onclick="loadQuyenTruyCap(\'' + data[i].maGV + '\')" data-bs-toggle="modal" data-bs-target="#modal-messAddDepartment" class="btn btn-warning"> <i class="fa-solid fa-gear"></i></button>' +
                            '</td>';
                        tableGiangVienHoanThanh.appendChild(row);
                    }
                    updatePhanTrangTableTaiKhoanTheoKhoa(response.totalPages, response.currentPage);

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    tableGiangVienHoanThanh.appendChild(row);
                }
            } else {
                var tableGiangVienHoanThanh = document.getElementById("tableTaiKhoanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                tableGiangVienHoanThanh.appendChild(row);

            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function updatePhanTrangTableTaiKhoanTheoKhoa(totalPages, currentPage) {
  
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachTaiKhoanTheoKhoa(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachTaiKhoanTheoKhoa(' + page + ')">' + label + '</a>';
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
//loc theo khoa va chuoi tim kiem


function loadDanhSachTaiKhoanTheoKhoaVaChuoiTim(page = 1, pageSize = 5) {
    var formData = new FormData();
    var mainKhoa = document.getElementById("khoa");

    var chuoiTim = document.getElementById("txtTimKiem").value;
    formData.append("maKhoa", mainKhoa.value);
    formData.append("chuoiTim", chuoiTim);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/Account/LoadDanhSachtaiKhoanTheoKhoaVChuoiTim',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response))
            if (response.success) {
                var tableGiangVienHoanThanh = document.getElementById("tableTaiKhoanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].loaiTaiKhoan + '</td>' +
                            '<td>' +
                            '<button onclick="loadQuyenTruyCap(\'' + data[i].maGV + '\')" data-bs-toggle="modal" data-bs-target="#modal-messAddDepartment" class="btn btn-warning"> <i class="fa-solid fa-gear"></i></button>' +
                            '</td>';
                        tableGiangVienHoanThanh.appendChild(row);
                    }
                    updatePhanTrangTableTaiKhoanTheoKhoaVaChuoiTim(response.totalPages, response.currentPage);

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    tableGiangVienHoanThanh.appendChild(row);
                }
            } else {
                var tableGiangVienHoanThanh = document.getElementById("tableTaiKhoanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                tableGiangVienHoanThanh.appendChild(row);

            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
function updatePhanTrangTableTaiKhoanTheoKhoaVaChuoiTim(totalPages, currentPage) {
  
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachTaiKhoanTheoKhoaVaChuoiTim(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachTaiKhoanTheoKhoaVaChuoiTim(' + page + ')">' + label + '</a>';
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