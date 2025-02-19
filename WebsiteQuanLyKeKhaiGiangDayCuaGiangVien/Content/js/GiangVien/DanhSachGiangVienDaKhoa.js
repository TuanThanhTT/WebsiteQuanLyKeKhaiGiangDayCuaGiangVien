
//load daanh sach giang vien


function updatePhanTrangTableGiangVien(totalPages, currentPage) {
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    // Nút Previous
    var prev = document.createElement('li');
    prev.className = "page-item" + (currentPage === 1 ? " disabled" : "");
    prev.innerHTML = '<a class="page-link" href="#" onclick="loadDanhSachGiangVien(' + (currentPage - 1) + ')">Trước</a>';
    pagination.appendChild(prev);

    // Các trang
    for (let i = 1; i <= totalPages; i++) {
        var page = document.createElement('li');
        page.className = "page-item" + (i === currentPage ? " active" : ""); // Đặt lớp active cho trang hiện tại
        page.innerHTML = '<a class="page-link" href="#" onclick="loadDanhSachGiangVien(' + i + ')">' + i + '</a>';
        pagination.appendChild(page);
    }

    // Nút Next
    var next = document.createElement('li');
    next.className = "page-item" + (currentPage === totalPages ? " disabled" : "");
    next.innerHTML = '<a class="page-link" href="#" onclick="loadDanhSachGiangVien(' + (currentPage + 1) + ')">Sau</a>';
    pagination.appendChild(next);
}


document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachGiangVien();
});


function loadDanhSachGiangVien(page = 1, pageSize = 5) {
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/GiangVien/loadDanhSachGiangVienDaKhoa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {

            if (response.success) {
                var data = response.data;
                var mainTable = document.getElementById("maintableGiangVien");
                mainTable.innerHTML = '';
                if (data.length > 0) {
                    var btnXuatDanhSachGiangVienDaKhoa = document.getElementById("btnXuatDanhSachGiangVienDaKhoa");
                    btnXuatDanhSachGiangVienDaKhoa.disabled = false;
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');

                        row.innerHTML = '<td><input type="checkbox" value="' + data[i].maGV + '" class="rowCheckbox"></td>' +
                            '<td>' + ((page - 1) * pageSize + 1 + i) + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].ngaySinh + '</td>' +
                            '<td>' +
                            '<button class="btn btn-primary" onclick="XemChiTietGiangVien(\'' + data[i].maGV + '\')" data-bs-toggle="modal" data-bs-target="#modalXemChiTiet">Xem</button>' +
                            '</td>';

                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableGiangVien(response.totalPages, response.currentPage);

                } else {
                    btnXuatDanhSachGiangVienDaKhoa.disabled = true;
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


///xem chi tiet giang vien

function XemChiTietGiangVien(maGV) {
    var formData = new FormData();

    formData.append("maGV", String(maGV));

    $.ajax({
        url: '/Admin/GiangVien/LayThongTinGiangVienTheoMa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {

            if (response.success) {
                var data = response.data;
                var xemMaGV = document.getElementById("xemMaGiangVien");
                xemMaGV.innerHTML = '';
                xemMaGV.innerHTML = data.maGV;
                var xemTenGiangVien = document.getElementById("xemTenGiangVien");
                xemTenGiangVien.innerHTML = '';
                xemTenGiangVien.innerHTML = data.tenGV;
                var xemNgaySinh = document.getElementById("xemNgaySinh");
                xemNgaySinh.innerHTML = '';
                xemNgaySinh.innerHTML = data.ngaySinh;
                var xemTenKhoa = document.getElementById("xemTenKhoa");
                xemTenKhoa.innerHTML = '';
                xemTenKhoa.innerHTML = data.tenKhoa;
                var xemChuyenNghanh = document.getElementById("xemChuyenNghanh");
                xemChuyenNghanh.innerHTML = '';
                xemChuyenNghanh.innerHTML = data.chuyenNganh;
                var xemTrinhDo = document.getElementById("xemTrinhDo");
                xemTrinhDo.innerHTML = '';
                xemTrinhDo.innerHTML = data.trinhDo;
                var xemHeSoLuong = document.getElementById("xemHeSoLuong");
                xemHeSoLuong.innerHTML = '';
                xemHeSoLuong.innerHTML = data.heSoLuong;
                var xemGioiTinh = document.getElementById("xemGioiTinh");
                xemGioiTinh.innerHTML = '';
                xemGioiTinh.innerHTML = data.gioiTinh;
                var xemChucVu = document.getElementById("xemChucVu");
                xemChucVu.innerHTML = '';
                xemChucVu.innerHTML = data.chucVu;
                var xemEmail = document.getElementById("xemEmail");
                xemEmail.innerHTML = '';
                xemEmail.innerHTML = data.email;
                var xemSoDienThoai = document.getElementById("xemSoDienThoai");
                xemSoDienThoai.innerHTML = '';
                xemSoDienThoai.innerHTML = data.soDienThoai;
                var xemDiaChi = document.getElementById("xemDiaChi");
                xemDiaChi.innerHTML = '';
                xemDiaChi.innerHTML = data.diaChi;
                var xemLoaiHinhDaoTao = document.getElementById("xemLoaiHinhDaoTao");
                xemLoaiHinhDaoTao.innerHTML = '';
                xemLoaiHinhDaoTao.innerHTML = data.loaiHinhDaoTao;


            } else {
                alert("Có lỗi xảy ra: " + response.message);
            }

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

// mo khoa giang vien
document.addEventListener("DOMContentLoaded", function () {
    var btnmoKhoaGiangVien = document.getElementById("btnmoKhoaGiangVien");

    // Gắn sự kiện click cho nút mo khoa
    btnmoKhoaGiangVien.addEventListener("click", function () {
        const checkboxes = document.querySelectorAll('.rowCheckbox');
        const isAnyChecked = Array.from(checkboxes).some(checkbox => checkbox.checked);

        if (isAnyChecked) {

            XacNhanMoKhoaGiangVien();
        } else {
            // Hiển thị thông báo nếu không có checkbox nào được chọn
            alert("Vui lòng chọn ít nhất một giảng viên để mở khóa.");
        }
    });
});


function XacNhanMoKhoaGiangVien() {



    const checkboxes = document.querySelectorAll('.rowCheckbox');

    const selectedIds = [];
    checkboxes.forEach(checkbox => {
        if (checkbox.checked) {
            selectedIds.push(checkbox.value);
        }
    });

    if (selectedIds.length === 0) {
        alert("Vui lòng chọn ít nhất một giảng viên để mở khóa.");
        return;
    }


    var modalConfirmKhoa = document.getElementById("modalConfirmKhoa");
    modalConfirmKhoa.style.display = "block";

    const confirmBtn = document.getElementById("confirmBtn");
    confirmBtn.addEventListener("click", function () {
        moKhoaDanhSach(selectedIds);
        modalConfirmKhoa.style.display = "none";

    });
}



function moKhoaDanhSach(idGiangViens) {

    var formData = new FormData();
    formData.append("idGiangViens", JSON.stringify(idGiangViens));
    $.ajax({
        url: '/Admin/GiangVien/moKhoaDanhSachGiangVien',
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

//xuaty danh sach
document.addEventListener("DOMContentLoaded", function () {
    var btnXuatDanhSachGiangVienDaKhoa = document.getElementById("btnXuatDanhSachGiangVienDaKhoa");
    btnXuatDanhSachGiangVienDaKhoa.addEventListener("click", function () {
        xuatDanhSachGiangVienDaKhoa();
    });
});

function xuatDanhSachGiangVienDaKhoa() {
    
    $.ajax({
        url: '/Admin/GiangVien/XuatDanhSachGiangVienDaKhoa',
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