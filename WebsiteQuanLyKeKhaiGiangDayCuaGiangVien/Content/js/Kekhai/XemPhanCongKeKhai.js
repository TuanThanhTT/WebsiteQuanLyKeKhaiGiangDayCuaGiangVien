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

document.getElementById("xemPhanCong").addEventListener("click", function () {
    var DotKeKhai = document.getElementById("dotKeKhai");
    const selectedDotKeKhai = DotKeKhai.value;
    loadHocPhanDuocPhanCongTrongKy(selectedDotKeKhai);
});


function loadHocPhanDuocPhanCongTrongKy(maDotKeKhai) {
    var formData = new FormData();
    formData.append("maDotKeKhai", maDotKeKhai);
  
    $.ajax({
        url: '/KeKhai/loadDanhSachPhanCongHocPhanTheoDot',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var mainTablePhanCong = document.getElementById("tableDotKeKhai");
                mainTablePhanCong.innerHTML = '';
                var data = response.data;

                console.log(JSON.stringify(data));
                if (data.length > 0) {
                    var soLuong = document.getElementById("soLuongPhanCong");
                    soLuong.innerHTML = '';
                    soLuong.innerHTML = data.length;
                    var thongTinPhanCong = document.getElementById("thongTinPhanCong");
                    thongTinPhanCong.style.visibility = 'visible';
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + (i + 1) + '</td>' +
                            '<td>' + data[i].maHocPhanPhanCong + '</td>' +
                            '<td>' + data[i].tenHocPhanPhanCong + '</td>' +
                            '<td>' + data[i].ngayDay + '</td>' +
                            '<td>' + data[i].tenLop + '</td>' +
                            '<td>' + data[i].siSo + '</td>' +
                            '<td>' + data[i].hocKy + '</td>' +
                            '<td>' + data[i].namHoc + '</td>' +
                            '<td>' + data[i].hinhThucDay + '</td>';
                        mainTablePhanCong.appendChild(row);

                    }
                } else {
                    var thongTinPhanCong = document.getElementById("thongTinPhanCong");
                    thongTinPhanCong.style.visibility = 'hidden';
                    var row = document.createElement("tr");
                    var cell = document.createElement("td");
                    cell.colSpan = 9;  
                    cell.style.textAlign = "center"; 
                    cell.textContent = "Không có dữ liệu hiển thị"; 
                    row.appendChild(cell); 
                    mainTablePhanCong.appendChild(row);

                }
            }

           
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    loadPhanCongHienTai();
});

function loadPhanCongHienTai() {
    $.ajax({
        url: '/KeKhai/loadDanhSachPhanCongHocPhanTheoDotGanNhat',
        type: 'POST',
        success: function (response) {
            console.log("Co chay");
            console.log(JSON.stringify(response));
            if (response.success) {
                var mainTablePhanCong = document.getElementById("tableDotKeKhai");
                mainTablePhanCong.innerHTML = '';
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
                            //thongKeTheoDot(dotKeKhai.value);
                            //thongKeTienDoGiangVien();
                        }, 500); // Chờ 500ms để đảm bảo dữ liệu đã cập nhật
                    }, 500);
                }
                if (data.length > 0) {
                    var soLuong = document.getElementById("soLuongPhanCong");
                    soLuong.innerHTML = '';
                    soLuong.innerHTML = data.length;
                    var thongTinPhanCong = document.getElementById("thongTinPhanCong");
                    thongTinPhanCong.style.visibility = 'visible';
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + (i + 1) + '</td>' +
                            '<td>' + data[i].maHocPhanPhanCong + '</td>' +
                            '<td>' + data[i].tenHocPhanPhanCong + '</td>' +
                            '<td>' + data[i].ngayDay + '</td>' +
                            '<td>' + data[i].tenLop + '</td>' +
                            '<td>' + data[i].siSo + '</td>' +
                            '<td>' + data[i].hocKy + '</td>' +
                            '<td>' + data[i].namHoc + '</td>' +
                            '<td>' + data[i].hinhThucDay + '</td>';
                        mainTablePhanCong.appendChild(row);

                    }
                } else {
                    var thongTinPhanCong = document.getElementById("thongTinPhanCong");
                    thongTinPhanCong.style.visibility = 'hidden';
                    var row = document.createElement("tr");
                    var cell = document.createElement("td");
                    cell.colSpan = 9;
                    cell.style.textAlign = "center";
                    cell.textContent = "Không có dữ liệu hiển thị";
                    row.appendChild(cell);
                    mainTablePhanCong.appendChild(row);
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
} 

document.addEventListener("DOMContentLoaded", function () {
    var XuatFilePhanCong = document.getElementById("XuatFilePhanCong");
    if (XuatFilePhanCong) { 
        XuatFilePhanCong.addEventListener("click", XuatFilePhanCongTheoDot);
    }
});




function XuatFilePhanCongTheoDot() {
    var dotKeKhai = document.getElementById("dotKeKhai");
    var maDotKeKhai = 0;
    if (dotKeKhai.value) {
        maDotKeKhai = dotKeKhai.value;
    }
    

    var formData = new FormData();
    formData.append("maDotKeKhai", maDotKeKhai);
            $.ajax({
                url: '/KeKhai/DowloadFilePhanCong', 
                type: 'POST',
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
                        console.log("dowwn load file thanh cong!");
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
                        }, 5000000); // Chờ 3 giây trước khi gửi yêu cầu xóa


                    } else {
                        alert(response.message);
                    }
                },
                error: function () {
                    alert("Đã xảy ra lỗi khi tải file.");
                }
            });
}

