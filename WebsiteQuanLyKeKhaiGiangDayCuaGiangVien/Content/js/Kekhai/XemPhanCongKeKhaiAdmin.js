function LoadNamHoc() {

    $.ajax({
        url: '/Admin/KeKhai/LoadNamHoc',
        type: 'POST',
        success: function (response) {
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

function loadDotKeKhaiTheoHocKyNamHoc(maNamHoc, maHocKy) {
    var formData = new FormData();
    formData.append("maNamHoc", maNamHoc);
    formData.append("maHocKy", maHocKy);
    $.ajax({
        url: '/KeKhai/XemDotKeKhaiTheoHocKyNamHoc',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log("thanh cong");
            var mainDotKeKhai = document.getElementById("dotKeKhai");
            mainDotKeKhai.innerHTML = '';
            if (response.success) {
                var data = response.data;

                var dotkekhai = document.createElement('option');
                dotkekhai.selected = true;
                dotkekhai.value = "";
                dotkekhai.textContent = "Chọn đợt kê khai";

                mainDotKeKhai.appendChild(dotkekhai);
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var option = document.createElement('option');
                        option.value = data[i].MaDotKeKhai;
                        option.textContent = data[i].TenDotKeKhai;
                        mainDotKeKhai.appendChild(option);
                    }
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function checkAndLoadDotKeKhai() {
    var mainNamHoc = document.getElementById("namHoc");
    var mainHocKy = document.getElementById("hocKy");

    const selectedNamHoc = mainNamHoc.value;
    const selectedHocKy = mainHocKy.value;

    // Kiểm tra nếu cả năm học và học kỳ đều được chọn
    if (selectedNamHoc && selectedHocKy) {
        loadDotKeKhaiTheoHocKyNamHoc(selectedNamHoc, selectedHocKy);
    }
}


document.addEventListener("DOMContentLoaded", function () {
    const namHoc = document.getElementById("namHoc");
    const hocKy = document.getElementById("hocKy");

    namHoc.addEventListener("change", function () {
        checkAndLoadDotKeKhai();
    });

    hocKy.addEventListener("change", function () {
        checkAndLoadDotKeKhai();
    });
});




function updatePhanTrangTableKeKhai(totalPages, currentPage) {

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadTablePhanCongTheoDotKeKhai(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadTablePhanCongTheoDotKeKhai(' + page + ')">' + label + '</a>';
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
    //load dot gần nhất
    //loadTablePhanCongTheoDotKeKhai();
});



function loadTablePhanCongTheoDotKeKhai(page = 1, pageSize = 5) {
    var maDotKeKhai = document.getElementById("dotKeKhai").value;
    var formData = new FormData();
    formData.append("maDotKeKhai", maDotKeKhai);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/KeKhai/loadHocPhanPhanCongTheoDot',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var data = response.data;
                var mainTable = document.getElementById("tablePhanCongKeKhai");
                mainTable.innerHTML = '';
                if (data.length > 0) {

                    if (response.dotGanNhat) {

                        var namHoc = document.getElementById("namHoc");
                        var hocKyChon = document.getElementById("hocKy");
                        var dotKeKhai = document.getElementById("dotKeKhai");

                        // Đầu tiên, chọn năm học
                        namHoc.value = response.thongTinDotKeKhai.maNamHoc;
                        namHoc.dispatchEvent(new Event("change"));

                        // Chờ tải danh sách học kỳ xong, sau đó mới chọn học kỳ
                        setTimeout(() => {
                            hocKyChon.value = response.thongTinDotKeKhai.maHocKy;
                            hocKyChon.dispatchEvent(new Event("change"));

                            // Chờ tải danh sách đợt kê khai xong, sau đó mới chọn đợt kê khai
                            setTimeout(() => {
                                dotKeKhai.value = response.thongTinDotKeKhai.maDotKeKhai;
                                dotKeKhai.dispatchEvent(new Event("change"));

                                // Gọi thống kê
                                //thongKeTheoDot(dotKeKhai.value);
                                //thongKeTienDoGiangVien();
                            }, 500); // Chờ 500ms để đảm bảo dữ liệu đã cập nhật
                        }, 500);
                    }

                    var tenDotKeKhaiPhanCong = document.getElementById("tenDotKeKhaiPhanCong");
                    tenDotKeKhaiPhanCong.innerHTML = '';
                    tenDotKeKhaiPhanCong.innerHTML = response.tenDotKeKhai;
                    var namHocPhanCong = document.getElementById("namHocPhanCong");
                    namHocPhanCong.innerHTML = '';
                    namHocPhanCong.innerHTML = response.tenNamHoc;
                    var hocKyPhanCong = document.getElementById("hocKyPhanCong");
                    hocKyPhanCong.innerHTML = '';
                    hocKyPhanCong.innerHTML = response.tenHocKy;

                    var maDotKeKhai = document.getElementById("maDotKeKhai");
                    maDotKeKhai.innerHTML = '';
                    maDotKeKhai.innerHTML = response.maDotKeKhai;


                    var soLuong = document.getElementById("soLuong");
                    soLuong.innerHTML = '';
                    soLuong.innerHTML = response.totalRecords;

                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');

                        row.innerHTML = "<td>" + (i + 1) + "</td>" +
                            "<td>" + data[i].maGV + "</td>" +
                            "<td>" + data[i].tenGV + "</td>" +
                            "<td>" + data[i].khoa + "</td>" +
                            "<td>" + data[i].soLuong + "</td>" +
                            "<td> <button onclick=\"xemChiTietPhanCongCuaGiangVien('" + data[i].maGV + "', " + response.maDotKeKhai + ")\" class='btn btn-warning' type='button'>Xem</button></td>";


                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableKeKhai(response.totalPages, response.currentPage);
                   

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan = "5" style="text-align: center;">Không có thông tin hiện thị!</td>';
                    mainTable.appendChild(row);
                }

            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function xemChiTietPhanCongCuaGiangVien(maGV, maDotKeKhai) {
    var formData = new FormData();
    formData.append("maGV", maGV);
    formData.append("maDotKeKhai", maDotKeKhai);
   
    $.ajax({
        url: '/Admin/KeKhai/xemChiTietPhanCongCuaGiangVien',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            const modal = new bootstrap.Modal(document.getElementById("modal-XPC-giangVienThongTin"));
            modal.show();
            if (response.success) {
                var data = response.data;
                var mainTable = document.getElementById("modal-XPC-bangHocPhan");
                mainTable.innerHTML = '';
                var maGV = document.getElementById("modal-XPC-maSoGV");
                maGV.innerHTML = '';
                maGV.innerHTML = response.idGV;
                var tenGV = document.getElementById("modal-XPC-tenGV");
                tenGV.innerHTML = '';
                tenGV.innerHTML = response.tenGiangVien;
                var tenKhoa = document.getElementById("modal-XPC-khoa");
                tenKhoa.innerHTML = '';
                tenKhoa.innerHTML = response.tenKhoa;
                if (data.length > 0) {
                    var soLuongPhanCong = document.getElementById("modal-XPC-soLuongMon");
                    soLuongPhanCong.innerHTML = '';
                    soLuongPhanCong.innerHTML = data.length;

                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + (i + 1) + '</td>' +
                            '<td>' + data[i].maHocPhanPhanCong + '</td>' +
                            '<td>' + data[i].tenHocPhanPhanCong + '</td>' +
                            '<td>' + data[i].ngayDay + '</td>' +
                            '<td>' + data[i].hinhThucDay + '</td>' +
                            '<td>' + data[i].tenLop + '</td>' +
                            '<td>' + data[i].siSo + '</td>' +
                            '<td>' + data[i].hocKy + '</td>' +
                            '<td>' + data[i].namHoc + '</td>';
                        mainTable.appendChild(row);
                    }
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="9" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    mainTable.appendChild(row);
                }
            } else {
                alert(response.message);

            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}



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
                        option.value = data[i].MaKhoa;
                        option.textContent = data[i].TenKhoa;
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

// Hàm xử lý sự kiện khi thay đổi khoa
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

// Hàm xử lý sự kiện khi click nút "Lọc"
function handleLocClick() {
    var mainKhoa = document.getElementById("khoa");
    var dotKeKhai = document.getElementById("dotKeKhai");

    var selectedId = mainKhoa.value;
    if (selectedId !== "") {
        if (dotKeKhai.value) {
            // Lọc theo khoa và đợt
            // loadTableGiangVienKeKhaiTheoDotVaKhoa(selectedId, dotKeKhai.value);
            loadTablePhanCongTheoDotVaKhoa(selectedId, dotKeKhai.value);
        } else {
            console.log("Ma khoa da chon: " + selectedId);
            // Lọc theo khoa
            //loadDataTableKeKhaiTheoKhoa(selectedId);
            loadTablePhanCongTheoDotVaKhoa();
        }
    }
}



document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachKhoa();
    var dotKeKhai = document.getElementById("dotKeKhai");
    var btnXemPhanCong = document.getElementById("btnXemPhanCong");

    // Bật/tắt nút khi giá trị của `dotKeKhai` thay đổi
    dotKeKhai.addEventListener('input', function () {
        if (dotKeKhai.value) {
            btnXemPhanCong.disabled = false;
        } else {
            btnXemPhanCong.disabled = true;
        }
    });

    // Thêm sự kiện click cho nút (chỉ hoạt động khi không bị vô hiệu hóa)
    btnXemPhanCong.addEventListener('click', function () {
        if (dotKeKhai.value) {
            loadTablePhanCongTheoDotKeKhai();
        }
    });

    // Kiểm tra giá trị ban đầu để thiết lập trạng thái của nút
    btnXemPhanCong.disabled = !dotKeKhai.value;
});


function LamMoiTrangWeb() {
    location.reload();
}



function loadTablePhanCongTheoDotVaKhoa(page = 1, pageSize = 5) {
    var maKhoa = document.getElementById("khoa").value;
    var maDotKeKhai = document.getElementById("dotKeKhai").value;
    var formData = new FormData();
    formData.append("maKhoa", maKhoa);
    formData.append("maDotKeKhai", maDotKeKhai);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/KeKhai/loadPhanCongHocPhanTheoKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            if (response.success) {
                var data = response.data;
                var mainTable = document.getElementById("tablePhanCongKeKhai");
                mainTable.innerHTML = '';
                if (data.length > 0) {
                    var tenDotKeKhaiPhanCong = document.getElementById("tenDotKeKhaiPhanCong");
                    tenDotKeKhaiPhanCong.innerHTML = '';
                    tenDotKeKhaiPhanCong.innerHTML = response.tenDotKeKhai;
                    var namHocPhanCong = document.getElementById("namHocPhanCong");
                    namHocPhanCong.innerHTML = '';
                    namHocPhanCong.innerHTML = response.tenNamHoc;
                    var hocKyPhanCong = document.getElementById("hocKyPhanCong");
                    hocKyPhanCong.innerHTML = '';
                    hocKyPhanCong.innerHTML = response.tenHocKy;

                    var maDotKeKhai = document.getElementById("maDotKeKhai");
                    maDotKeKhai.innerHTML = '';
                    maDotKeKhai.innerHTML = response.maDotKeKhai;


                    var soLuong = document.getElementById("soLuong");
                    soLuong.innerHTML = '';
                    soLuong.innerHTML = response.totalRecords;

                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');

                        row.innerHTML = "<td>" + (i + 1) + "</td>" +
                            "<td>" + data[i].maGV + "</td>" +
                            "<td>" + data[i].tenGV + "</td>" +
                            "<td>" + data[i].khoa + "</td>" +
                            "<td>" + data[i].soLuong + "</td>" +
                            "<td> <button onclick=\"xemChiTietPhanCongCuaGiangVien('" + data[i].maGV + "', " + response.maDotKeKhai + ")\" class='btn btn-warning' type='button'>Xem</button></td>";

                          //  "<td> <button id='btn-XPC-openModal' onclick=\"xemChiTietPhanCongCuaGiangVien('" + data[i].maGV + "', " + response.maDotKeKhai + ")\" class='btn btn-warning' type='button'>Xem</button></td>";


                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableKeKhaiTheoDotVaKhoa(response.totalPages, response.currentPage);
                    //document.getElementById("btn-XPC-openModal").addEventListener("click", function () {
                    //    // Hiển thị modal
                    //    const modal = new bootstrap.Modal(document.getElementById("modal-XPC-giangVienThongTin"));
                    //    modal.show();
                    //});

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan = "5" style="text-align: center;">Không có thông tin hiện thị!</td>';
                    mainTable.appendChild(row);
                }

            } else {
                alert(response.message);
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function updatePhanTrangTableKeKhaiTheoDotVaKhoa(totalPages, currentPage) {

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadTablePhanCongTheoDotVaKhoa(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadTablePhanCongTheoDotVaKhoa(' + page + ')">' + label + '</a>';
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
