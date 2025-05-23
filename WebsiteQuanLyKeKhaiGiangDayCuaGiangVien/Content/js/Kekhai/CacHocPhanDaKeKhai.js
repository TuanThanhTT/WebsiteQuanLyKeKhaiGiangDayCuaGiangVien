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
                console.log("log dot ke khai theo nam hoc: " + JSON.stringify(response));
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
    var DotKeKhai = document.getElementById("dotKeKhai");
    const selectedDotKeKhai = DotKeKhai.value;
   // loadDanhSachKeKhaiTheoDot();
});

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
            : '<a class="page-link" href="#" onclick="loadDanhSachKeKhaiTheoDot(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachKeKhaiTheoDot(' + page + ')">' + label + '</a>';
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

function loadDanhSachKeKhaiTheoDot(page = 1, pageSize = 5) {
    var maDotKeKhai = document.getElementById("dotKeKhai").value;
    console.log("dợt kê khai dang chon là: " + maDotKeKhai);
    $.ajax({
        url: '/KeKhai/loadHocPhanKeKhaiTheoDot',
        type: 'POST',
        data: { maDotKeKhai: maDotKeKhai, page: page, pageSize: pageSize },
        success: function (response) {
            console.log(JSON.stringify(response));
            var mainTable = document.getElementById("tableKeKhai");
            tableKeKhai.innerHTML = '';

            var xuatFile = document.getElementById("xuatFile");
           

            if (response.success) {
                var data = response.data;

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
                            thongKeTheoDot(dotKeKhai.value);
                            thongKeTienDoGiangVien();
                        }, 500); // Chờ 500ms để đảm bảo dữ liệu đã cập nhật
                    }, 500);
                }

                if (data.length > 0) {
                   


                    var soLuong = document.getElementById("soLuongkeKhaiDaHoanThanh");
                    soLuong.innerHTML = '';
                    soLuong.innerHTML = response.soLuong;

                  
                    xuatFile.style.display = 'block';


                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + ((page - 1) * pageSize + i + 1) + '</td>' +
                            '<td>' + data[i].maHP+'</td>' +
                            '<td>' + data[i].tenHocPhan + '</td>' +
                            '<td>' + data[i].tenLop+'</td>' +
                            '<td><button id="open-modal" onclick="xemThongTinKeKhai('+data[i].Id+')" class="btn btn-primary">Xem</button></td>';
                        mainTable.appendChild(row);
                    }
                } else {
                    xuatFile.style.display = 'none';
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="5" style="text-align: center;">Không có dữ liệu hiện thị!</td>';
                    mainTable.appendChild(row);
                }
                updatePhanTrangTableKeKhai(response.totalPages, response.page);


            } else {
                xuatFile.style.display = 'none';
            }
          
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra:" + error);
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    var btnXemKeKhai = document.getElementById("btnXemKeKhai");
    var dotKeKhaiChon = document.getElementById("dotKeKhai");
   
    if (dotKeKhai && dotKeKhai.value) {
     
        btnXemKeKhai.addEventListener("click", function () {
            loadDanhSachKeKhaiTheoDot();
        });
    }


    if (dotKeKhaiChon) {
        dotKeKhaiChon.addEventListener("change", function () {

            btnXemKeKhai.addEventListener("click", function () {
                loadDanhSachKeKhaiTheoDot();
            });
        });
    }

});

function xemThongTinKeKhai(maKeKhai) {
    $.ajax({
        url: '/KeKhai/XemKeKhaiDaDuyetTheoMaKeKhai',
        type: 'POST',
        data: { maKeKhai: maKeKhai },
        success: function (response) {
            console.log("ma ke kahi la: " + maKeKhai);
            console.log("thong tin la: " + JSON.stringify(response));
            if (response.success) {
                var data = response.data;

                var magv = document.getElementById("ma-giang-vien");
                var tengv = document.getElementById("ten-giang-vien");
                var hocKy = document.getElementById("hoc-ky");
                var namHoc = document.getElementById("nam-hoc");
             
                var thoiGianDay = document.getElementById("ngay-day");
                var maHocPhan = document.getElementById("ma-hoc-phan");
                var tenHocPhan = document.getElementById("ten-hoc-phan");
                var hinhThucDay = document.getElementById("hinh-thuc-day");
                var tenLop = document.getElementById("ten-lop");
                var siSo = document.getElementById("si-so");
                var ngayKeKhai = document.getElementById("ngay-ke-khai");
                var nguoiDuyet = document.getElementById("ngay-duyet");

                magv.innerHTML = '';
                magv.innerHTML = response.maGV;
                tengv.innerHTML = '';
                tengv.innerHTML = response.tenGV
                hocKy.innerHTML = '';
                hocKy.innerHTML = data.hocKy;
                namHoc.innerHTML = '';
                namHoc.innerHTML = data.namHoc;
                nguoiDuyet.innerHTML = '';
                nguoiDuyet.innerHTML = data.nguoiDuyet;
                thoiGianDay.innerHTML = '';
                thoiGianDay.innerHTML = data.ngayDay;
                maHocPhan.innerHTML = '';
                maHocPhan.innerHTML = data.maHP;
                tenHocPhan.innerHTML = '';
                tenHocPhan.innerHTML = data.tenHocPhan;
                hinhThucDay.innerHTML = '';
                hinhThucDay.innerHTML = data.hinhThucDay;
                tenLop.innerHTML = '';
                tenLop.innerHTML = data.tenLop;
                siSo.innerHTML = '';
                siSo.innerHTML = data.soLuong;
                ngayKeKhai.innerHTML = '';
                ngayKeKhai.innerHTML = data.ngayKeKhai;
              
                openModal();
            }

           



        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    var btnXuatFileKeKhai = document.getElementById("btnXuatFileKeKhai");
    if (btnXuatFileKeKhai) {
        btnXuatFileKeKhai.addEventListener("click", XuatFileKeKhaiTheoDot);
    }
});

function XuatFileKeKhaiTheoDot() {
    var dotKeKhai = document.getElementById("dotKeKhai");
    var maDotKeKhai = 0;
    if (dotKeKhai.value) {
        maDotKeKhai = dotKeKhai.value;
    }

    var formData = new FormData();
    formData.append("maDotKeKhai", maDotKeKhai);
    $.ajax({
        url: '/KeKhai/XuatDanhSachKeKhaiTheoDot',
        method: 'POST',
        data: formData,
        contentType: false,
        processData: false,
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
                        url: '/KeKhai/XoaFileTam',
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
                }, 500000); // Chờ 3 giây trước khi gửi yêu cầu xóa

            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("Đã xảy ra lỗi khi tải file.");
        }
    });
}
