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
            var btnlocKeKhai = document.getElementById("btnLocKeKhaiTheoKhoa");
            btnlocKeKhai.addEventListener("click", handleLocClick);
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
    var btnlocKeKhai = document.getElementById("btnLocKeKhaiTheoKhoa");
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
            loadTableGiangVienKeKhaiTheoDotVaKhoa(selectedId, dotKeKhai.value);
        } else {
            // Lọc theo khoa
            loadDataTableKeKhaiTheoKhoa(selectedId);
        }
    }
}



document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachKhoa();
    var btnCloseModal = document.getElementById("btnCloseModal");
    btnCloseModal.addEventListener("click", function () {
        reloadPage();
    });
    var closeBtnModal = document.getElementById("closeBtnModal");
    closeBtnModal.addEventListener("click", function () {
        reloadPage();
    });
});

function reloadPage() {

    location.reload();
}


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

function loadDotKeKhaiTheoHocKyNamHoc(maNamHoc, maHocKy) {
    var formData = new FormData();
    formData.append("maNamHoc", maNamHoc);
    formData.append("maHocKy", maHocKy);
    $.ajax({
        url: '/KeKhai/LoadDotKeKhaiTheoHocKyNamHoc',
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
                        option.value = data[i].maDotKeKhai;
                        console.log("ma dot ke khai: " + data[i].maDotKeKhai);
                        option.textContent = data[i].tenDotKeKhai;
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

    loadDataTableDanhSachGiangVienChoDuyet();
});




function loadDataTableDanhSachGiangVienChoDuyet(page = 1,pageSize = 5) {
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/KeKhai/getDanhSachDuyetKeKhaiChoDuyet',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log("Duyet ke khai dot moi nhat");
            if (response.success) {
                var mainTable = document.getElementById("tableGiangVienChoDuyet");
                mainTable.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    var soLuongGiangVienChoDuyet = document.getElementById("soLuongGiangVienChoDuyet");
                    soLuongGiangVienChoDuyet.innerHTML = '';
                    soLuongGiangVienChoDuyet.innerHTML = data.length;
                    console.log(JSON.stringify(response));
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize+i+1) + '</td>' +
                            '<td>' + data[i].maGV+'</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].soLuong + '</td>' +
                            '<td>' +
                            '<button onclick="xemDanhSachKeKaiChoDuyetCuaGiangVien(\'' + data[i].maGV + '\')" class="btn btn-warning" type="button">Xem</button>' +
                            '</td>';
                       
                        mainTable.appendChild(row);
                    }
                  
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                    mainTable.appendChild(row);
                }
             
                updatePhanTrangTableKeKhai(response.totalPages, response.currentPage)


            } 
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function loadDataTableKeKhaiTheoKhoa(maKhoa, page = 1, pageSize = 5) {
    var formData = new FormData();
    formData.append("maKhoa", maKhoa);
    formData.append("page", page);
    formData.append("pageSize", pageSize);

    $.ajax({
        url: '/Admin/KeKhai/getDanhSachDuyetKeKhaiChoDuyetTheoKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var mainTable = document.getElementById("tableGiangVienChoDuyet");
                mainTable.innerHTML = '';
                var data = response.data;
                console.log("so luong la: " + data.length);

                if (data.length > 0) {
                    var soLuongGiangVienChoDuyet = document.getElementById("soLuongGiangVienChoDuyet");
                    soLuongGiangVienChoDuyet.innerHTML = data.length;
                    console.log("loc theo ma khoa: " + JSON.stringify(response));

                    // Thêm dữ liệu vào bảng
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].soLuong + '</td>' +
                            '<td>' +
                            '<button id="btn-duyetKK-openModal-' + data[i].maGV + '" onclick="xemDanhSachKeKaiChoDuyetCuaGiangVien(\'' + data[i].maGV + '\')" class="btn btn-warning" type="button">Xem</button>' +
                            '</td>';
                        mainTable.appendChild(row);
                    }

                   

                    // Cập nhật phân trang
                    updatePhanTrangTableKeKhai(response.totalPages, response.currentPage);

                    // Thêm sự kiện mở modal sau khi bảng được hiển thị
                    // Gắn sự kiện vào các nút "Xem"
                   /* for (let i = 0; i < data.length; i++) {
                        let button = document.getElementById("btn-duyetKK-openModal-" + data[i].maGV);
                        if (button) {
                            button.addEventListener("click", function () {
                                const modal = new bootstrap.Modal(document.getElementById("modal-duyetKK"));
                                modal.show();
                            });
                        }
                    }*/

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                    mainTable.appendChild(row);
                }

            } else {
                var mainTable = document.getElementById("tableGiangVienChoDuyet");
                mainTable.innerHTML = '';

                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                mainTable.appendChild(row);
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function updatePhanTrangTableKeKhai(totalPages, currentPage) {
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    // Nút Previous
    var prev = document.createElement('li');
    prev.className = "page-item" + (currentPage === 1 ? " disabled" : "");
    prev.innerHTML = '<a class="page-link" href="#" onclick="loadTableHocPhanDuocPhanCong(' + (currentPage - 1) + ')">Trước</a>';
    pagination.appendChild(prev);

    // Các trang
    for (let i = 1; i <= totalPages; i++) {
        var page = document.createElement('li');
        page.className = "page-item" + (i === currentPage ? " active" : ""); // Đặt lớp active cho trang hiện tại
        page.innerHTML = '<a class="page-link" href="#" onclick="loadTableHocPhanDuocPhanCong(' + i + ')">' + i + '</a>';
        pagination.appendChild(page);
    }

    // Nút Next
    var next = document.createElement('li');
    next.className = "page-item" + (currentPage === totalPages ? " disabled" : "");
    next.innerHTML = '<a class="page-link" href="#" onclick="loadTableHocPhanDuocPhanCong(' + (currentPage + 1) + ')">Sau</a>';
    pagination.appendChild(next);
}


function xemDanhSachKeKaiChoDuyetCuaGiangVien(maGV) {
    var formData = new FormData();
    formData.append("maGV", maGV);
    $.ajax({
        url: '/Admin/KeKhai/XemDanhSachKeKhaiChoDuyetTheoGiangVien',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            const myModal = new bootstrap.Modal(document.getElementById("modal-duyetKK"));
            myModal.show();
          

            
            
            if (response.success) {
                var mainTable = document.getElementById("modal-duyetKK-bangKeKhai");
                mainTable.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {

                    var soLuong = document.getElementById("modal-duyetKK-soLuongKeKhai");
                    soLuong.innerHTML = "";
                    soLuong.innerHTML = data.length;
                    var textMaGV = document.getElementById("modal-duyetKK-maGiangVien");
                    textMaGV.innerHTML = "";
                    textMaGV.innerHTML = maGV;

                    var textTenGV = document.getElementById("modal-duyetKK-tenGiangVien");
                    textTenGV.innerHTML = "";
                    textTenGV.innerHTML = data[0].tenGV;


                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + (i + 1) + '</td>' +
                            '<td class="idMaKeKhai" style="display: none;">' + data[i].maKeKhai + '</td>' +
                            '<td>' + data[i].maHP + '</td>' +
                            '<td>' + data[i].tenHP + '</td>' +
                            '<td>' + data[i].tenLop + '</td>' +
                            '<td>' + data[i].siSo + '</td>' +
                            '<td>' + data[i].hocKy + '</td>' +
                            '<td>' + data[i].namHoc + '</td>' +
                            '<td>' + data[i].hinhThucDay + '</td>' +
                            '<td>' + data[i].thoiGianDay + '</td>' +
                            '<td>' + data[i].trangThai + '</td>' +
                            '<td><button onclick="DuyetKeKhai(' + data[i].maKeKhai+')" class="btn btn-success btn-sm">Duyệt</button></td>';
                        mainTable.appendChild(row);

                        var duyetTatCa = document.getElementById("modal-duyetKK-duyetTatCa");
                        duyetTatCa.addEventListener("click", DuyetToanBoKeKhai);
                    }
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="11" style="text-align: center;">Không có dữ liệu hiện thị!</td>'
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {

   

    var xemKeKhaiTheoDot = document.getElementById("xemKeKhaiTheoDot");
    var dotKeKhaiChon = document.getElementById("dotKeKhai");
    console.log("dot ke khai da chon: " + dotKeKhaiChon.value);
    if (dotKeKhai && dotKeKhai.value) {
        console.log("co chay loadTableGiangVienKeKhaiTheoDot");

        // Đăng ký sự kiện click, truyền giá trị maDotKeKhai vào khi nhấn
        xemKeKhaiTheoDot.addEventListener("click", function () {
            loadTableGiangVienKeKhaiTheoDot(dotKeKhaiChon.value);
        });
    }


    if (dotKeKhaiChon) {
        dotKeKhaiChon.addEventListener("change", function () {
            console.log("Đợt kê khai đã thay đổi: " + dotKeKhaiChon.value);
            
            xemKeKhaiTheoDot.addEventListener("click", function () {
                loadTableGiangVienKeKhaiTheoDot(dotKeKhaiChon.value);
            });
        });
    }
   
});

function lamMoiTrang() {
    location.reload();
}





function loadTableGiangVienKeKhaiTheoDot(maDotKeKhai, page=1, pageSize = 5) {
    var formData = new FormData();
    formData.append("maDotKeKhai", maDotKeKhai);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/KeKhai/getDanhSachGiangVienKeKhaiTheoDot',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var mainTable = document.getElementById("tableGiangVienChoDuyet");
                mainTable.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    console.log(JSON.stringify(response));
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].soLuong + '</td>' +
                            '<td>' +
                            '<button  onclick="xemDanhSachKeKaiChoDuyetCuaGiangVien(\'' + data[i].maGV + '\')" class="btn btn-warning" type="button">Xem</button>' +
                            '</td>';

                        mainTable.appendChild(row);
                    }
                    //{"success":1,"data":[{"maGV":"0000000002","tenGV":null,"maKhoa":"1","tenKhoa":"Công Nghệ và Kỹ Thuật","soLuong":1}],"totalRecords":1,"totalPages":1,"currentPage":1,"message":"Load danh sách thành công!"}
                   /* document.getElementsByClassName("btn-duyetKK-openModal").addEventListener("click", function () {
                        // Hiển thị modal
                        const modal = new bootstrap.Modal(document.getElementById("modal-duyetKK"));
                        modal.show();
                    });*/
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                    mainTable.appendChild(row);
                }
              
                updatePhanTrangTableKeKhai(response.totalPages, response.currentPage)


            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}




function loadTableGiangVienKeKhaiTheoDotVaKhoa(maKhoa,maDotKeKhai, page = 1, pageSize = 5) {
    var formData = new FormData();
    formData.append("maKhoa", maKhoa);
    formData.append("maDotKeKhai", maDotKeKhai);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/KeKhai/getDanhSachGiangVienKeKhaiTheoDotVaKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log("Duyet theo khoa")
            if (response.success) {
                var mainTable = document.getElementById("tableGiangVienChoDuyet");
                mainTable.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    console.log(JSON.stringify(response));
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].soLuong + '</td>' +
                            '<td>' +
                            '<button onclick="xemDanhSachKeKaiChoDuyetCuaGiangVien(\'' + data[i].maGV + '\')" class="btn btn-warning" type="button">Xem</button>' +
                            '</td>';

                        mainTable.appendChild(row);
                    }
                    //{"success":1,"data":[{"maGV":"0000000002","tenGV":null,"maKhoa":"1","tenKhoa":"Công Nghệ và Kỹ Thuật","soLuong":1}],"totalRecords":1,"totalPages":1,"currentPage":1,"message":"Load danh sách thành công!"}
                    
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                    mainTable.appendChild(row);
                }

                updatePhanTrangTableKeKhai(response.totalPages, response.currentPage)
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}




function DuyetKeKhai(maKeKhai) {
    var formData = new FormData();
    formData.append("maKeKhai", maKeKhai);
    $.ajax({
        url:"/Admin/KeKhai/DuyetKeKhaiHoanThanh",
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

                var maGV = document.getElementById("modal-duyetKK-maGiangVien");

                xemDanhSachKeKaiChoDuyetCuaGiangVien(maGV.textContent);


            } else {
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Red";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();

                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
            }


        },

        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
 
}

function getIdKeKhaiFromTable() {
    var idKeKhaiList = []; 

    // Lấy tất cả các ô td có lớp 'idMaKeKhai'
    var idCells = document.querySelectorAll('.idMaKeKhai');

    idCells.forEach(function (cell) {
        var idKeKhai = cell.textContent || cell.innerText;
        idKeKhaiList.push(idKeKhai); 
    });

    return idKeKhaiList; 
}





function DuyetToanBoKeKhai() {

    var listIdKeKhai = getIdKeKhaiFromTable();
 
    $.ajax({
        url: "/Admin/KeKhai/DuyetToanBoKeKhaiCuaGiangVien",
        type: 'POST',
        data: JSON.stringify(listIdKeKhai),
        contentType: 'application/json',
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

                var maGV = document.getElementById("modal-duyetKK-maGiangVien");

                xemDanhSachKeKaiChoDuyetCuaGiangVien(maGV.textContent);

                var dotKeKhai = document.getElementById("dotKeKhai");
                if (dotKeKhai.value) {
                    loadTableGiangVienKeKhaiTheoDot(dotKeKhai.value);
                } else {
                    loadDataTableDanhSachGiangVienChoDuyet();
                }

                

            } else {
                var elements = document.getElementsByClassName("Modal-main-err-header");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "Red";
                }
                const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));
                errorModal.show();

                var title = document.getElementById("Modal-main-err-Label");
                title.innerHTML = '';
                title.innerHTML = "Lỗi";
                var textmain = document.getElementById("text-main");
                textmain.innerHTML = response.message;
            }


        },

        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });



}