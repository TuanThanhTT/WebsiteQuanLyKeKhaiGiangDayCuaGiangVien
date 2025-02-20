//load hoc ky nam hoc dot ke khai
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
            alert("Có lỗi xảy ra trong ham load nam hoc: " + error);
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
                        option.value = data[i].maDotKeKhai;
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
});

document.addEventListener("DOMContentLoaded", function () {
    
    var dotKeKhai = document.getElementById("dotKeKhai");
    var btnXemThongKe = document.getElementById("btnXemThongKe");

    // Bật/tắt nút khi giá trị của `dotKeKhai` thay đổi
    dotKeKhai.addEventListener('input', function () {
        if (dotKeKhai.value) {
            btnXemThongKe.disabled = false;
        } else {
            btnXemThongKe.disabled = true;
        }
    });

    // Thêm sự kiện click cho nút (chỉ hoạt động khi không bị vô hiệu hóa)
    btnXemThongKe.addEventListener('click', function () {
        if (dotKeKhai.value) {
            thongKeTheoDot(dotKeKhai.value);
            thongKeTienDoGiangVien();
        }
    });

    // Kiểm tra giá trị ban đầu để thiết lập trạng thái của nút
    btnXemThongKe.disabled = !dotKeKhai.value;
});

function thongKeTheoDot(maDotKeKhai) {

    var formData = new FormData();
    formData.append("maDotKeKhai", maDotKeKhai);
  
    $.ajax({
        url: '/Admin/ThongKe/ThongKeTheoDot',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            if (response.success) {
                console.log("Co hay render");
                var data = response.data;
                //ve bieu do
                var sampleData = [];
                const sampleLabels = ["Đã Kê Khai", "Chưa Kê Khai", "Chờ Duyệt", "Phân Công Đợt Trước"];
                var soLuongChuaKeKhai = data.soLuongHocPhan - data.soLuongHocPhanDaDuocKeKhai - data.soLuongPhanCongChoDuyet;
                sampleData.push(data.soLuongHocPhanDaDuocKeKhai);
                sampleData.push(soLuongChuaKeKhai);
                sampleData.push(data.soLuongPhanCongChoDuyet);
                sampleData.push(data.soLuongHocPhanCacDotTruoc);
                renderPieChart(sampleData, sampleLabels);

                var soLuongHocPhanDaKeKhai = document.getElementById("soLuongHocPhanDaKeKhai");
                soLuongHocPhanDaKeKhai.innerHTML = '';
                soLuongHocPhanDaKeKhai.innerHTML = data.soLuongHocPhanDaDuocKeKhai;
                var soLuongHocPhanCacDotKhac = document.getElementById("soLuongHocPhanCacDotKhac");
                soLuongHocPhanCacDotKhac.innerHTML = '';
                soLuongHocPhanCacDotKhac.innerHTML = data.soLuongHocPhanCacDotTruoc;
                var soLuongHocPhanPhanCong = document.getElementById("soLuongHocPhanPhanCong");
                soLuongHocPhanPhanCong.innerHTML = '';
                soLuongHocPhanPhanCong.innerHTML = data.soLuongHocPhan; 
                var soLuongGiangVienPhanCong = document.getElementById("soLuongGiangVienPhanCong");
                soLuongGiangVienPhanCong.innerHTML = '';
                soLuongGiangVienPhanCong.innerHTML = data.soLuongGiangVienPhanCong;
                var soLuongGiangVienChuaKeKhai = document.getElementById("soLuongGiangVienChuaKeKhai");
                soLuongGiangVienChuaKeKhai.innerHTML = '';
                soLuongGiangVienChuaKeKhai.innerHTML = data.soLuongGiangVienChuaHoanThanh;
                var soLuongGiangVienDaKeKhai = document.getElementById("soLuongGiangVienDaKeKhai");
                soLuongGiangVienDaKeKhai.innerHTML = '';
                soLuongGiangVienDaKeKhai.innerHTML = data.soLuongGiangVienDaHoanThanh;
            }
          

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra trong load theo dot: " + error);
        }
    });
}

function thongKeTienDoGiangVien(page = 1, pageSize = 5) {
    var dotKeKhai = document.getElementById("dotKeKhai");
    var formData = new FormData();
    var maDotKeKhai = dotKeKhai.value;
   
    formData.append("maDotKeKhai", maDotKeKhai);
    formData.append("page", page);
    formData.append("pageSize", pageSize);

    $.ajax({
        url: '/Admin/ThongKe/LoadTienDoGiangVien',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var mainTable = document.getElementById("tableTienDo");
                mainTable.innerHTML = '';
                var data = response.data;
                var namHoc = document.getElementById("namHoc");
                var hocKy = document.getElementById("hocKy");
                var tabletenDotKeKhai = document.getElementById("tabletenDotKeKhai");
                tabletenDotKeKhai.innerHTML = '';
              
                var tabletenNamHoc = document.getElementById("tabletenNamHoc");
                tabletenNamHoc.innerHTML = '';
              
                var tabletenHocKy = document.getElementById("tabletenHocKy");
                tabletenHocKy.innerHTML = '';
        
                tabletenDotKeKhai.innerHTML = dotKeKhai.options[dotKeKhai.selectedIndex].text;
                tabletenNamHoc.innerHTML = namHoc.options[namHoc.selectedIndex].text;
                tabletenHocKy.innerHTML = hocKy.options[hocKy.selectedIndex].text;

                console.log(JSON.stringify(data));
                if (data.length > 0) {
                    var tablesoLuongGiangVien = document.getElementById("tablesoLuongGiangVien");
                    thongKeTienDoGiangVien.innerHTML = '';
                    tablesoLuongGiangVien.innerHTML = data.length;
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<th scope="row">'+((page-1)*page+i+1)+'</th>'+
                            '<td>'+data[i].maGv+'</td>' +
                            '<td>'+data[i].tenGv+'</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].daThuHien + '/' + data[i].soLuongPhanCong + '</td>' +
                            '<td class=btnXem>'+
                            '<button id="openDetailsModal" onclick="xemChiTietTienDoCuaGiangVien(\'' + data[i].maGv + '\')" class="btn btn-sm btn-primary">xem</button>'+
                            '</td>';
                        mainTable.appendChild(row);
                    }
                  
                    updatePhanTrangTableTienDoGiangVien(response.totalPages, response.currentPage);
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan="6" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    mainTable.appendChild(row);
                }

            } else {
                alert("loi xay ra trong thong ke thay tien do giang vien khi load dau trang khi vao: "+response.message);
            }


        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function updatePhanTrangTableTienDoGiangVien(totalPages, currentPage) {
    //var pagination = document.querySelector(".pagination");
    //pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    //// Nút Previous
    //var prev = document.createElement('li');
    //prev.className = "page-item" + (currentPage === 1 ? " disabled" : "");
    //prev.innerHTML = '<a class="page-link" href="#" onclick="thongKeTienDoGiangVien(' + (currentPage - 1) + ')">Trước</a>';
    //pagination.appendChild(prev);

    //// Các trang
    //for (let i = 1; i <= totalPages; i++) {
    //    var page = document.createElement('li');
    //    page.className = "page-item" + (i === currentPage ? " active" : ""); // Đặt lớp active cho trang hiện tại
    //    page.innerHTML = '<a class="page-link" href="#" onclick="thongKeTienDoGiangVien(' + i + ')">' + i + '</a>';
    //    pagination.appendChild(page);
    //}

    //// Nút Next
    //var next = document.createElement('li');
    //next.className = "page-item" + (currentPage === totalPages ? " disabled" : "");
    //next.innerHTML = '<a class="page-link" href="#" onclick="thongKeTienDoGiangVien(' + (currentPage + 1) + ')">Sau</a>';
    //pagination.appendChild(next);
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="thongKeTienDoGiangVien(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="thongKeTienDoGiangVien(' + page + ')">' + label + '</a>';
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
//lam moi thong ke

document.addEventListener("DOMContentLoaded", function () {
    var btnLamMoiThongKe = document.getElementById("btnLamMoiThongKe");
    btnLamMoiThongKe.addEventListener("click", function () {

    });

    var dotKeKhai = document.getElementById("dotKeKhai");  var btnXemThongKe = document.getElementById("btnXemThongKe");

    // Bật/tắt nút khi giá trị của `dotKeKhai` thay đổi
    dotKeKhai.addEventListener('input', function () {
        if (dotKeKhai.value) {
            btnLamMoiThongKe.disabled = false;
        } else {
            btnLamMoiThongKe.disabled = true;
        }
    });

    // Thêm sự kiện click cho nút (chỉ hoạt động khi không bị vô hiệu hóa)
    btnLamMoiThongKe.addEventListener('click', function () {
        if (dotKeKhai.value) {
            LamMoiThongKe(dotKeKhai.value);
            thongKeTienDoGiangVien();
        }
    });

    // Kiểm tra giá trị ban đầu để thiết lập trạng thái của nút
    btnLamMoiThongKe.disabled = !dotKeKhai.value;
});


function LamMoiThongKe(maDotKeKhai) {

    var formData = new FormData();
    formData.append("maDotKeKhai", maDotKeKhai);

    $.ajax({
        url: '/Admin/ThongKe/LamMoiThongKe',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            if (response.success) {
                console.log("Co hay render");
                var data = response.data;
                //ve bieu do
                var sampleData = [];
                const sampleLabels = ["Đã Kê Khai", "Chưa Kê Khai", "Chờ Duyệt", "Phân Công Đợt Trước"];
                var soLuongChuaKeKhai = data.soLuongHocPhan - data.soLuongHocPhanDaDuocKeKhai - data.soLuongPhanCongChoDuyet;
                sampleData.push(data.soLuongHocPhanDaDuocKeKhai);
                sampleData.push(soLuongChuaKeKhai);
                sampleData.push(data.soLuongPhanCongChoDuyet);
                sampleData.push(data.soLuongHocPhanCacDotTruoc);
                renderPieChart(sampleData, sampleLabels);

                var soLuongHocPhanDaKeKhai = document.getElementById("soLuongHocPhanDaKeKhai");
                soLuongHocPhanDaKeKhai.innerHTML = '';
                soLuongHocPhanDaKeKhai.innerHTML = data.soLuongHocPhanDaDuocKeKhai;
                var soLuongHocPhanCacDotKhac = document.getElementById("soLuongHocPhanCacDotKhac");
                soLuongHocPhanCacDotKhac.innerHTML = '';
                soLuongHocPhanCacDotKhac.innerHTML = data.soLuongHocPhanCacDotTruoc;
                var soLuongHocPhanPhanCong = document.getElementById("soLuongHocPhanPhanCong");
                soLuongHocPhanPhanCong.innerHTML = '';
                soLuongHocPhanPhanCong.innerHTML = data.soLuongHocPhan;
                var soLuongGiangVienPhanCong = document.getElementById("soLuongGiangVienPhanCong");
                soLuongGiangVienPhanCong.innerHTML = '';
                soLuongGiangVienPhanCong.innerHTML = data.soLuongGiangVienPhanCong;
                var soLuongGiangVienChuaKeKhai = document.getElementById("soLuongGiangVienChuaKeKhai");
                soLuongGiangVienChuaKeKhai.innerHTML = '';
                soLuongGiangVienChuaKeKhai.innerHTML = data.soLuongGiangVienChuaHoanThanh;
                var soLuongGiangVienDaKeKhai = document.getElementById("soLuongGiangVienDaKeKhai");
                soLuongGiangVienDaKeKhai.innerHTML = '';
                soLuongGiangVienDaKeKhai.innerHTML = data.soLuongGiangVienDaHoanThanh;
                thongKeTienDoGiangVien();
            }


        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//thong ke theo phan cong gan nhat

document.addEventListener("DOMContentLoaded", function () {
   
    loadDanhSachKhoa();
    thongKeTheoDotGanNhat();
});

function thongKeTheoDotGanNhat() {
    $.ajax({
        url: '/Admin/ThongKe/ThongKeTheoDotMoiNhat',
        type: 'POST',
        success: function (response) {
            console.log("chay ham thong ke thongKeTheoDotGanNhat "+JSON.stringify(response));
            if (response.success) {
                var thongTin = response.thongTin;
                var namHoc = document.getElementById("namHoc");
                var hocKyChon = document.getElementById("hocKy");
                var dotKeKhai = document.getElementById("dotKeKhai");
             
                // Đầu tiên, chọn năm học
                namHoc.value = response.thongTin.maNamHoc;
                namHoc.dispatchEvent(new Event("change"));

                // Chờ tải danh sách học kỳ xong, sau đó mới chọn học kỳ
                setTimeout(() => {
                    hocKyChon.value = response.thongTin.maHocKy;
                    hocKyChon.dispatchEvent(new Event("change"));

                    // Chờ tải danh sách đợt kê khai xong, sau đó mới chọn đợt kê khai
                    setTimeout(() => {
                        dotKeKhai.value = response.thongTin.maDotKeKhai;
                        dotKeKhai.dispatchEvent(new Event("change"));

                        // Gọi thống kê
                          thongKeTheoDot(dotKeKhai.value);
                          thongKeTienDoGiangVien();
                    }, 500); // Chờ 500ms để đảm bảo dữ liệu đã cập nhật
                }, 500);
              

            } else {
                alert("Có lỗi xảy ra trong ham thong ke theo dot gan nhat: "+response.message);
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra trong ham thong ke theo dot gan nhat: " + error);
        }
    });
}
//lay thông tin chi tiet của giuang vien

function xemChiTietTienDoCuaGiangVien(maGV) {
    var dotKeKhai = document.getElementById("dotKeKhai");
    var formData = new FormData();
    formData.append("maGV", maGV);
    formData.append("maDotKeKhai", dotKeKhai.value);
   
    $.ajax({
        url: '/Admin/ThongKe/XemChiTietTienDoGiangVien',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
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
//load khoa
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
            var btnXuatDanhSachTheoKhoa = document.getElementById("btnXuatDanhSachTheoKhoa");
            btnXuatDanhSachTheoKhoa.addEventListener("click", handleLocClick);
            btnXuatDanhSachTheoKhoa.disabled = true;
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
    var btnXuatDanhSachTheoKhoa = document.getElementById("btnXuatDanhSachTheoKhoa");
    if (selectedId !== "") {
        btnXuatDanhSachTheoKhoa.disabled = false; // Kích hoạt nút lọc nếu có chọn khoa
    } else {
        btnXuatDanhSachTheoKhoa.disabled = true; // Vô hiệu hóa nút lọc nếu không chọn khoa
    }
}

function handleLocClick() {
    var mainKhoa = document.getElementById("khoa");
    var dotKeKhai = document.getElementById("dotKeKhai");

    var selectedId = mainKhoa.value;
    if (selectedId !== "") {
        if (dotKeKhai.value) {
            console.log("tien hanh xuat file");
            xuatDanhSachThongKeTheoKhoa(selectedId, dotKeKhai.value);
        } 
    }
}


function xuatDanhSachThongKeTheoKhoa(maKhoa) {
    var dotKeKhai = document.getElementById("dotKeKhai");
    var maDotKeKhai = maDotKeKhai = dotKeKhai.value;
  
    var formData = new FormData();
    formData.append("maKhoa", maKhoa);
    formData.append("maDotKeKhai", maDotKeKhai);
    $.ajax({
        url: '/Admin/ThongKe/XuatDanhSachThongKeTheoKhoa',
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

document.addEventListener("DOMContentLoaded", function () {
    var btnXuatThongKeTatCa = document.getElementById("btnXuatThongKeTatCa");
    var dotKeKhai = document.getElementById("dotKeKhai");

    if (btnXuatThongKeTatCa && dotKeKhai) {
        btnXuatThongKeTatCa.addEventListener("click", function () {
            if (dotKeKhai.value) {
                console.log("Tiến hành xuất file");
                xuatDanhSachThongKeTatCaKhoa(dotKeKhai.value);
            } else {
                alert("Vui lòng chọn đợt kê khai trước khi xuất file!");
            }
        });
    }
});


function xuatDanhSachThongKeTatCaKhoa() {
    var dotKeKhai = document.getElementById("dotKeKhai");
    var maDotKeKhai = maDotKeKhai = dotKeKhai.value;

    var formData = new FormData();
    formData.append("maDotKeKhai", maDotKeKhai);
    $.ajax({
        url: '/Admin/ThongKe/XuatDanhSachThongKeTatCaKhoa',
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

//link xem chi tiet
document.addEventListener("DOMContentLoaded", function () {
    var dotKeKhai = document.getElementById("dotKeKhai");
    var xemChiTietGiangVienChuaHoanThanh = document.getElementById("xemChiTietGiangVienChuaHoanThanh"); // Định nghĩa nút
    var xemChiTietGiangVienHoanThanh = document.getElementById("xemChiTietGiangVienHoanThanh");
    if (dotKeKhai && xemChiTietGiangVienChuaHoanThanh && xemChiTietGiangVienHoanThanh) {
        xemChiTietGiangVienChuaHoanThanh.addEventListener("click", function () {
            if (dotKeKhai.value) {
                console.log("Có click va ma dotKeKhai la: " + dotKeKhai.value);
                xemTrangGiangVienChuaHoanThanh(dotKeKhai.value);


            } else {
                alert("Vui lòng chọn đợt kê khai trước khi xem!");
            }
        });
        xemChiTietGiangVienHoanThanh.addEventListener("click", function () {
            if (dotKeKhai.value) {
                console.log("Có click va ma dotKeKhai la: " + dotKeKhai.value);
                xemTrangGiangVienHoanThanh(dotKeKhai.value);


            } else {
                alert("Vui lòng chọn đợt kê khai trước khi xem!");
            }
        });

    }
});

function xemTrangGiangVienChuaHoanThanh(maDotKeKhai) {

    window.location.href = `/Admin/ThongKe/GiangVienChuaKeKhai?maDotKeKhai=` + maDotKeKhai;
}

function xemTrangGiangVienHoanThanh(maDotKeKhai) {

    window.location.href = `/Admin/ThongKe/GiangVienHoanThanhKeKhai?maDotKeKhai=` + maDotKeKhai;
}

