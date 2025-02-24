//load table
document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachGiangVien();

    loadDanhSachKhoa();
    var btnXuatDanhsach = document.getElementById("btnXuatDanhsach");
    btnXuatDanhsach.addEventListener("click", function () {
        xuatDanhSachThongKe();
    });
});


function loadDanhSachGiangVien(page = 1, pageSize = 5) {
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/ThongKe/LoadTableGiangVienChuaHoanThanhKeKhai',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response))
            if (response.success) {
                var tableGiangVienHoanThanh = document.getElementById("tableGiangVienChuaHoanThanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].tienDo + '</td>' +
                            '<td>' +
                            '<button onclick="xemChiTietGiangVien(\'' + data[i].maGV + '\')" class="btn btn-warning">Xem chi tiet</button>'+
                            '</td>';
                        tableGiangVienHoanThanh.appendChild(row);
                    }
                    updatePhanTrangTableGiangVien(response.totalPages, response.currentPage);
                    var khoa = document.getElementById("khoa");
                    khoa.style.display = 'block';
                    var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                    btnLocPhanTheoKhoa.style.display = 'block';
                    var btnXuatDanhsach = document.getElementById("btnXuatDanhsach");
                    btnXuatDanhsach.disabled = false;
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    tableGiangVienHoanThanh.appendChild(row);
                }
            } else {
                var tableGiangVienHoanThanh = document.getElementById("tableGiangVienChuaHoanThanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                tableGiangVienHoanThanh.appendChild(row);
                var khoa = document.getElementById("khoa");
                khoa.style.display = 'none';
                var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                btnLocPhanTheoKhoa.style.display = 'none';
                var btnXuatDanhsach = document.getElementById("btnXuatDanhsach");
                btnXuatDanhsach.disabled = true;
            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}



function updatePhanTrangTableGiangVien(totalPages, currentPage) {
    //var pagination = document.querySelector(".pagination");
    //pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    //// Nút Previous
    //var prev = document.createElement('li');
    //prev.className = "page-item" + (currentPage === 1 ? " disabled" : "");
    //prev.innerHTML = '<a class="page-link" href="#" onclick="loadDanhSachGiangVien(' + (currentPage - 1) + ')">Trước</a>';
    //pagination.appendChild(prev);

    //// Các trang
    //for (let i = 1; i <= totalPages; i++) {
    //    var page = document.createElement('li');
    //    page.className = "page-item" + (i === currentPage ? " active" : ""); // Đặt lớp active cho trang hiện tại
    //    page.innerHTML = '<a class="page-link" href="#" onclick="loadDanhSachGiangVien(' + i + ')">' + i + '</a>';
    //    pagination.appendChild(page);
    //}

    //// Nút Next
    //var next = document.createElement('li');
    //next.className = "page-item" + (currentPage === totalPages ? " disabled" : "");
    //next.innerHTML = '<a class="page-link" href="#" onclick="loadDanhSachGiangVien(' + (currentPage + 1) + ')">Sau</a>';
    //pagination.appendChild(next);
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVien(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVien(' + page + ')">' + label + '</a>';
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

//load danh sach khoa



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
            locTheoKhoaVaChuoiTim();
        } else {
            console.log("Ma khoa da chon: " + selectedId);
            // Lọc theo khoa
            locTheoKhoa();
        }
    }
}

function locTheoKhoa(page = 1, pageSize = 10) {
    var formData = new FormData();
    var khoaTim = document.getElementById("khoa");
    maKhoa = khoaTim.value;
    formData.append("maKhoa", maKhoa);
    formData.append("page", page);
    formData.append("pageSize", pageSize);

    $.ajax({
        url: '/Admin/ThongKe/TimKiemGiangVienChuaHoanThanhTheoKhoa',
        data: formData,
        contentType: false,
        processData: false,
        method: 'POST',
        success: function (response) {
            console.log(JSON.stringify(response))
            if (response.success) {
             
                var tableGiangVienHoanThanh = document.getElementById("tableGiangVienChuaHoanThanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].tienDo + '</td>' +
                            '<td>' +
                            '<button onclick="xemChiTietGiangVien(\'' + data[i].maGV + '\')" class="btn btn-warning">Xem chi tiet</button>' +
                            '</td>';
                        tableGiangVienHoanThanh.appendChild(row);
                    }
                    updatePhanTrangTableGiangVienTheoMaKhoa(response.totalPages, response.currentPage);
                    var khoa = document.getElementById("khoa");
                    khoa.style.display = 'block';
                    var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                    btnLocPhanTheoKhoa.style.display = 'block';
                    var btnXuatDanhsach = document.getElementById("btnXuatDanhsach");
                    btnXuatDanhsach.disabled = false;
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="5" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    tableGiangVienHoanThanh.appendChild(row);
                }
            } else {
                var tableGiangVienHoanThanh = document.getElementById("tableGiangVienChuaHoanThanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="5" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                tableGiangVienHoanThanh.appendChild(row);
                var khoa = document.getElementById("khoa");
                khoa.style.display = 'none';
                var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                btnLocPhanTheoKhoa.style.display = 'none';
                var btnXuatDanhsach = document.getElementById("btnXuatDanhsach");
                btnXuatDanhsach.disabled = true;
            }
        },
        error: function () {
            alert("Đã xảy ra lỗi khi tải file.");
        }
    });
}


function locTheoKhoaVaChuoiTim(page = 1, pageSize = 10) {
    var formData = new FormData();
    var khoaTim = document.getElementById("khoa");
    maKhoa = khoaTim.value;
    var btnTim = document.getElementById("txtTimKiem");
    var chuoiTim = btnTim.value;
    formData.append("chuoiTim", chuoiTim);
    formData.append("maKhoa", maKhoa);
    formData.append("page", page);
    formData.append("pageSize", pageSize);

    $.ajax({
        url: '/Admin/ThongKe/TimKiemGiangVienChuaHoanThanhTheoKhoaVaChuoiTim',
        data: formData,
        contentType: false,
        processData: false,
        method: 'POST',
        success: function (response) {
            console.log("loc theo khoa va chuoi tim ");
            console.log(JSON.stringify(response))
            if (response.success) {

                var tableGiangVienHoanThanh = document.getElementById("tableGiangVienChuaHoanThanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].tienDo + '</td>' +
                            '<td>' +
                            '<button onclick="xemChiTietGiangVien(\'' + data[i].maGV + '\')" class="btn btn-warning">Xem chi tiet</button>' +
                            '</td>';
                        tableGiangVienHoanThanh.appendChild(row);
                    }
                    updatePhanTrangTableGiangVienTheoMaKhoaVaChuoiTim(response.totalPages, response.currentPage);
                    var khoa = document.getElementById("khoa");
                    khoa.style.display = 'block';
                    var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                    btnLocPhanTheoKhoa.style.display = 'block';
                    var btnXuatDanhsach = document.getElementById("btnXuatDanhsach");
                    btnXuatDanhsach.disabled = false;
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="5" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    tableGiangVienHoanThanh.appendChild(row);
                }
            } else {
                var tableGiangVienHoanThanh = document.getElementById("tableGiangVienChuaHoanThanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="5" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                tableGiangVienHoanThanh.appendChild(row);
                var khoa = document.getElementById("khoa");
                khoa.style.display = 'none';
                var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                btnLocPhanTheoKhoa.style.display = 'none';
                var btnXuatDanhsach = document.getElementById("btnXuatDanhsach");
                btnXuatDanhsach.disabled = true;
            }
        },
        error: function () {
            alert("Đã xảy ra lỗi khi tải file.");
        }
    });
}



function updatePhanTrangTableGiangVienTheoMaKhoa(totalPages, currentPage) {
    //var pagination = document.querySelector(".pagination");
    //pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    //// Nút Previous
    //var prev = document.createElement('li');
    //prev.className = "page-item" + (currentPage === 1 ? " disabled" : "");
    //prev.innerHTML = '<a class="page-link" href="#" onclick="locTheoKhoa(' + (currentPage - 1) + ')">Trước</a>';
    //pagination.appendChild(prev);

    //// Các trang
    //for (let i = 1; i <= totalPages; i++) {
    //    var page = document.createElement('li');
    //    page.className = "page-item" + (i === currentPage ? " active" : ""); // Đặt lớp active cho trang hiện tại
    //    page.innerHTML = '<a class="page-link" href="#" onclick="locTheoKhoa(' + i + ')">' + i + '</a>';
    //    pagination.appendChild(page);
    //}

    //// Nút Next
    //var next = document.createElement('li');
    //next.className = "page-item" + (currentPage === totalPages ? " disabled" : "");
    //next.innerHTML = '<a class="page-link" href="#" onclick="locTheoKhoa(' + (currentPage + 1) + ')">Sau</a>';
    //pagination.appendChild(next);
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="locTheoKhoa(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="locTheoKhoa(' + page + ')">' + label + '</a>';
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

function updatePhanTrangTableGiangVienTheoMaKhoaVaChuoiTim(totalPages, currentPage) {
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    // Nút Previous
    var prev = document.createElement('li');
    prev.className = "page-item" + (currentPage === 1 ? " disabled" : "");
    prev.innerHTML = '<a class="page-link" href="#" onclick="locTheoKhoaVaChuoiTim(' + (currentPage - 1) + ')">Trước</a>';
    pagination.appendChild(prev);

    // Các trang
    for (let i = 1; i <= totalPages; i++) {
        var page = document.createElement('li');
        page.className = "page-item" + (i === currentPage ? " active" : ""); // Đặt lớp active cho trang hiện tại
        page.innerHTML = '<a class="page-link" href="#" onclick="locTheoKhoaVaChuoiTim(' + i + ')">' + i + '</a>';
        pagination.appendChild(page);
    }

    // Nút Next
    var next = document.createElement('li');
    next.className = "page-item" + (currentPage === totalPages ? " disabled" : "");
    next.innerHTML = '<a class="page-link" href="#" onclick="locTheoKhoaVaChuoiTim(' + (currentPage + 1) + ')">Sau</a>';
    pagination.appendChild(next);
}



// xuat file thong ke
function xuatDanhSachThongKe() {
  
    $.ajax({
        url: '/Admin/ThongKe/XuatDanhSachGiangVienChuaHoanThanhTheoDot',
        method: 'POST',
        success: function (response) {
            if (response.success) {
                const link = document.createElement('a');
                link.href = response.fileUrl;
                link.download = response.fileUrl.split('/').pop();
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);

                // Gửi yêu cầu xóa file
                // Gửi tên file qua FormData
                var fromDate = new FormData();
                fromDate.append("fileName", response.fileUrl.split('/').pop());
                setTimeout(() => {
                    var fromDate = new FormData();
                    fromDate.append("fileName", response.fileUrl.split('/').pop());
                    $.ajax({
                        url: '/Admin/ThongKe/XoaFileTam',
                        method: 'POST',
                        data: fromDate,
                        contentType: false,
                        processData: false,
                        success: function () {
                            console.log("File đã được xóa.");
                        },
                        error: function () {
                            console.error("Không thể xóa file.");
                        }
                    });
                }, 500000); // Chờ 5 giây trước khi gửi yêu cầu xóa

            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("Đã xảy ra lỗi khi tải file.");
        }
    });
}


//tim giang vien theo ma va ten

document.addEventListener("DOMContentLoaded", function () {
    var btnLamMoi = document.getElementById("btnLamMoi");
    btnLamMoi.addEventListener("click", reloadPage);
    var chuoiTimKiem = document.getElementById("txtTimKiem");
    var btnTimKiem = document.getElementById("btnTimKiem");

    // Hàm kiểm tra giá trị input và kích hoạt nút tìm kiếm
    function checkInput() {
        if (chuoiTimKiem.value.trim() !== "") {
            btnTimKiem.disabled = false;
        } else {
            btnTimKiem.disabled = true;
        }
    }
    checkInput();

    chuoiTimKiem.addEventListener("input", checkInput);

    btnTimKiem.addEventListener("click", function () {
        if (!btnTimKiem.disabled) {
            loadDanhSachGiangVienTimKiem();
        }
    });

});


function loadDanhSachGiangVienTimKiem(page = 1, pageSize = 5) {
    var formData = new FormData();
    var chuoiTim = document.getElementById("txtTimKiem");
    formData.append("chuoiTim", chuoiTim.value);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/ThongKe/TimKiemGiangVienChuaHoanThanhKeKhaiTheoDot',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            var khoaLoc = document.getElementById("khoa");
            khoaLoc.value = "";

          
            if (response.success) {
                
                var tableGiangVienHoanThanh = document.getElementById("tableGiangVienChuaHoanThanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].tienDo + '</td>' +
                            '<td>' +
                            '<button onclick="xemChiTietGiangVien(\'' + data[i].maGV + '\')" class="btn btn-warning">Xem chi tiet</button>' +
                            '</td>';
                        tableGiangVienHoanThanh.appendChild(row);
                    }
                    updatePhanTrangTableGiangVienTimKiem(response.totalPages, response.currentPage);
                    var khoa = document.getElementById("khoa");
                    khoa.style.display = 'block';
                    var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                    btnLocPhanTheoKhoa.style.display = 'block';
                    var btnXuatDanhsach = document.getElementById("btnXuatDanhsach");
                    btnXuatDanhsach.disabled = false;
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="5" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    tableGiangVienHoanThanh.appendChild(row);
                }
            } else {
                var tableGiangVienHoanThanh = document.getElementById("tableGiangVienChuaHoanThanh");
                tableGiangVienHoanThanh.innerHTML = '';
                var row = document.createElement('tr');
                row.innerHTML = '<td colspan="5" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                tableGiangVienHoanThanh.appendChild(row);
                var khoa = document.getElementById("khoa");
                khoa.style.display = 'none';
                var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                btnLocPhanTheoKhoa.style.display = 'none';
                var btnXuatDanhsach = document.getElementById("btnXuatDanhsach");
                btnXuatDanhsach.disabled = true;
            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function updatePhanTrangTableGiangVienTimKiem(totalPages, currentPage) {
    //var pagination = document.querySelector(".pagination");
    //pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    //// Nút Previous
    //var prev = document.createElement('li');
    //prev.className = "page-item" + (currentPage === 1 ? " disabled" : "");
    //prev.innerHTML = '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTimKiem(' + (currentPage - 1) + ')">Trước</a>';
    //pagination.appendChild(prev);

    //// Các trang
    //for (let i = 1; i <= totalPages; i++) {
    //    var page = document.createElement('li');
    //    page.className = "page-item" + (i === currentPage ? " active" : ""); // Đặt lớp active cho trang hiện tại
    //    page.innerHTML = '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTimKiem(' + i + ')">' + i + '</a>';
    //    pagination.appendChild(page);
    //}

    //// Nút Next
    //var next = document.createElement('li');
    //next.className = "page-item" + (currentPage === totalPages ? " disabled" : "");
    //next.innerHTML = '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTimKiem(' + (currentPage + 1) + ')">Sau</a>';
    //pagination.appendChild(next);
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTimKiem(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTimKiem(' + page + ')">' + label + '</a>';
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

function reloadPage() {
    location.reload();
}


///xem chi tiet

function xemChiTietGiangVien(maGV) {
    $.ajax({
        url: '/Admin/ThongKe/xemChiTietGiangVienChuaHoanThanhKeKhai',
        type: 'POST',
        success: function (response) {
            console.log(JSON.stringify(response))
            if (response.success) {
                console.log("ma giang vien khi goi: " + maGV);
                var maDotKeKhai = response.data;
                xemChiTietTienDoCuaGiangVien(maGV,maDotKeKhai);
            } else {
                alert("Có lỗi xảy ra vui lòng thử lại sau!");
            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function xemChiTietTienDoCuaGiangVien(maGV, maDotKeKhai) {
    console.log("Ma gv: " + maGV);
    var formData = new FormData();
    formData.append("maGV", maGV);
    formData.append("maDotKeKhai", maDotKeKhai);

    $.ajax({
        url: '/Admin/ThongKe/XemChiTietTienDoGiangVien',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response))
            if (response.success) {

                var mainTable = document.getElementById("tableXemChiTietTienDo");
                mainTable.innerHTML = '';
                var thongTin = response.thongTin;

                var xemtongSoPhanCong = document.getElementById("xemtongSoPhanCong");
                xemtongSoPhanCong.innerHTML = '';
                xemtongSoPhanCong.innerHTML = thongTin.soLuongPhanCong;
                var xemSoLuongConLai = document.getElementById("xemSoLuongConLai");
                xemSoLuongConLai.innerHTML = '';
                xemSoLuongConLai.innerHTML = thongTin.soLuongChuaHoanThanh;
                var xemSoLuongHoanThanh = document.getElementById("xemSoLuongHoanThanh");
                xemSoLuongHoanThanh.innerHTML = '';
                xemSoLuongHoanThanh.innerHTML = thongTin.soLuongHoanThanh;
                var xemNamHoc = document.getElementById("xemNamHoc");
                xemNamHoc.innerHTML = '';
                xemNamHoc.innerHTML = thongTin.tenNamHoc;
                var xemHocKy = document.getElementById("xemHocKy");
                xemHocKy.innerHTML = '';
                xemHocKy.innerHTML = thongTin.tenHocKy;
                var xemDotKeKhai = document.getElementById("xemDotKeKhai");
                xemDotKeKhai.innerHTML = '';
                xemDotKeKhai.innerHTML = thongTin.tenDotPhanCong;
                var xemKhoa = document.getElementById("xemKhoa");
                xemKhoa.innerHTML = '';
                xemKhoa.innerHTML = thongTin.tenKhoa;
                var xemTenGV = document.getElementById("xemTenGV");
                xemTenGV.innerHTML = '';
                xemTenGV.innerHTML = thongTin.tenGV;
                var xemmaGV = document.getElementById("xemmaGV");
                xemmaGV.innerHTML = '';
                xemmaGV.innerHTML = thongTin.maGV;

                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<th scope="row">' + (i + 1) + '</th>' +
                            '<td scope="row">' + data[i].maHP + '</td>' +
                            '<td scope="row">' + data[i].tenHP + '</td>' +
                            '<td scope="row">' + data[i].ngayDay + '</td>' +
                            '<td scope="row">' + data[i].tenLop + '</td>' +
                            '<td scope="row">' + data[i].siSo + '</td>' +
                            '<td scope="row">' + data[i].trangThai + '</td>';
                        mainTable.appendChild(row);
                    }

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="7" style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                }

                // var openDetailsModal = document.getElementById("openDetailsModal");
                var detailsModal = new bootstrap.Modal(document.getElementById("detailsModal"));

                //  openDetailsModal.addEventListener("click", function () {
                detailsModal.show();
                //});
            } else {
                alert(response.message);
            }


        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}