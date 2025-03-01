
document.getElementById('dongModalThem').addEventListener('click', function () {
    document.getElementById("themNamHocModal").style.display = 'none'; // Đóng modal
});

function LoadBangThongTinNamHoc() {
    $.ajax({
        url: '/Admin/NamHoc/LoadTableNamHoc',
        type: 'POST',
        success: function (response) {
            var mainTableNamHoc = document.getElementById("mainTableNamHoc");
            mainTableNamHoc.innerHTML = '';

            if (response.success) {
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement("tr");
                        row.innerHTML = '<td><input value="'+data[i].Id+'" type="checkbox" class="rowCheckbox"></td>' +
                            '<td>' + (i + 1) + '</td>' +
                            '<td>' + data[i].Id + '</td>' + 
                            '<td>' + data[i].TenNamHoc+'</td>' +
                            '<td>' +
                            '<button class="btn btn-success" onclick="showModalThemHocKy('+data[i].Id+')"> <i class="fa-solid fa-calendar-plus"></i> Học kỳ </button>' +
                            '<button type="button" class="btn btn-warning" data-toggle="modal" data-target="#editNamHocModal" onclick="MoModalCapNhatNamHoc(' + data[i].Id + ',\'' + data[i].TenNamHoc + '\')"> <i class="fa-solid fa-pen-to-square"></i> Cập nhật</button>'+

                            '<button type="button" onclick="XacNhanXoa(' + data[i].Id +')" class="btn btn-danger"><i class="fa-solid fa-trash-can"></i> Xóa </button>' +
                            '</td>';
                        mainTableNamHoc.appendChild(row);
                    }
                } else {
                    var row = document.createElement("tr");
                    row.innerHTML = '<td colspan="4" class="none-data">Không có dữ liệu hiện thị!</td>';
                    mainTableNamHoc.appendChild(row);
                }
            }
        }, // <-- Đóng đúng hàm success
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function LoadBangThongTinNamHoc(page = 1, pageSize = 10) {
    $.ajax({
        url: '/Admin/NamHoc/LoadTableNamHoc',
        type: 'POST',
        data: { page: page, pageSize: pageSize }, // Gửi page và pageSize tới controller
        success: function (response) {
            var mainTableNamHoc = document.getElementById("mainTableNamHoc");
            mainTableNamHoc.innerHTML = '';

            if (response.success) {
                var data = response.data;
                if (data.length > 0) {
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement("tr");
                        row.innerHTML = '<td><input value="' + data[i].Id + '" type="checkbox" class="rowCheckbox"></td>' +
                            '<td>' + (i + 1) + '</td>' +
                            '<td>' + data[i].Id + '</td>' +
                            '<td>' + data[i].TenNamHoc + '</td>' +
                            '<td>' +
                            '<button class="btn btn-success" onclick="showModalThemHocKy(' + data[i].Id + ')"> <i class="fa-solid fa-calendar-plus"></i> Học kỳ </button>' +
                            '<button type="button" class="btn btn-warning" data-toggle="modal" data-target="#editNamHocModal" onclick="MoModalCapNhatNamHoc(' + data[i].Id + ',\'' + data[i].TenNamHoc + '\')"> <i class="fa-solid fa-pen-to-square"></i> Cập nhật</button>' +

                            '<button type="button" onclick="XacNhanXoa(' + data[i].Id + ')" class="btn btn-danger"><i class="fa-solid fa-trash-can"></i> Xóa </button>' +
                            '</td>';
                        mainTableNamHoc.appendChild(row);
                    }
                } else {
                    var row = document.createElement("tr");
                    row.innerHTML = '<td colspan="4" class="none-data">Không có dữ liệu hiển thị!</td>';
                    mainTableNamHoc.appendChild(row);
                }

                // Hiển thị phân trang
                var pagination = document.getElementById("pagination");
                pagination.innerHTML = ''; // Xóa các nút phân trang cũ
                var totalPages = Math.ceil(response.totalRecords / pageSize);

                // Nút Previous
                var prevButton = document.createElement("li");
                prevButton.classList.add("page-item");
                prevButton.innerHTML = `<a class="page-link" href="#" onclick="LoadBangThongTinNamHoc(${page - 1}, ${pageSize})">Trước</a>`;
                if (page <= 1) prevButton.classList.add("disabled");
                pagination.appendChild(prevButton);

                // Các số trang
                for (let i = 1; i <= totalPages; i++) {
                    var pageButton = document.createElement("li");
                    pageButton.classList.add("page-item");
                    pageButton.innerHTML = `<a class="page-link" href="#" onclick="LoadBangThongTinNamHoc(${i}, ${pageSize})">${i}</a>`;
                    if (i === page) pageButton.classList.add("active");
                    pagination.appendChild(pageButton);
                }

                // Nút Next
                var nextButton = document.createElement("li");
                nextButton.classList.add("page-item");
                nextButton.innerHTML = `<a class="page-link" href="#" onclick="LoadBangThongTinNamHoc(${page + 1}, ${pageSize})">Sau</a>`;
                if (page >= totalPages) nextButton.classList.add("disabled");
                pagination.appendChild(nextButton);
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    LoadBangThongTinNamHoc(1, 10);
});
function goToPage(page) {
    LoadBangThongTinNamHoc(page, 10);
}

function updatePagination(currentPage, totalRecords, pageSize) {
    var totalPages = Math.ceil(totalRecords / pageSize);
    var paginationContainer = document.getElementById("pagination");

    // Xóa các nút phân trang cũ
    paginationContainer.innerHTML = '';

    // Thêm nút "Previous"
    if (currentPage > 1) {
        paginationContainer.innerHTML += '<li class="page-item"><a class="page-link" href="#" onclick="goToPage(' + (currentPage - 1) + ')">Previous</a></li>';
    } else {
        paginationContainer.innerHTML += '<li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>';
    }

    // Thêm các số trang
    for (let i = 1; i <= totalPages; i++) {
        if (i === currentPage) {
            paginationContainer.innerHTML += '<li class="page-item active"><a class="page-link" href="#">' + i + '</a></li>';
        } else {
            paginationContainer.innerHTML += '<li class="page-item"><a class="page-link" href="#" onclick="goToPage(' + i + ')">' + i + '</a></li>';
        }
    }

    // Thêm nút "Next"
    if (currentPage < totalPages) {
        paginationContainer.innerHTML += '<li class="page-item"><a class="page-link" href="#" onclick="goToPage(' + (currentPage + 1) + ')">Next</a></li>';
    } else {
        paginationContainer.innerHTML += '<li class="page-item disabled"><a class="page-link" href="#">Next</a></li>';
    }
    

}




document.addEventListener("DOMContentLoaded", function () {
  
    LoadBangThongTinNamHoc();
});


document.addEventListener("DOMContentLoaded", function () {
    // Lấy các phần tử
    const modalInput = document.getElementById("tenNamHoc");
    const saveButton = document.querySelector(".themNamHocBtnSave");
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
            const tenNamHoc = modalInput.value.trim();
           

            ThemNamHoc(tenNamHoc);
            modalInput.value = "";           
        }
    });

});

function ThemNamHoc(tenNamHoc) {
    var formData = new FormData();
    formData.append("tenNamHoc", tenNamHoc);

    $.ajax({
        url: '/Admin/NamHoc/ThemNamHoc',
        data: formData,
        contentType: false,
        processData: false,
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
                LoadBangThongTinNamHoc();
               
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
                    console.log("ci load lai trang");
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
    const modalInput = document.getElementById("tenHocKyMoi");
    const addButton = document.querySelector(".themHocKyBtnAdd");
    const errorMessage = document.getElementById("loi-hocthemhocky");

    // Hàm kiểm tra dữ liệu nhập
    function validateInput() {
        const inputValue = modalInput.value.trim();

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

    // Bắt sự kiện nút "Thêm"
    addButton.addEventListener("click", function () {
        if (validateInput()) {
            const tenHocKy = modalInput.value.trim();

            var maNamHocElement = document.getElementById("maNamHoc");
            var maNamHocValue = maNamHocElement.textContent.replace("Mã năm học: ", "").trim();
            console.log("Mã năm học là: " + maNamHocValue);

            ThemHocKyMoiVaoNamHoc(tenHocKy, maNamHocValue);



       
            console.log("Thêm học kỳ: " + tenHocKy);

            // Đóng modal sau khi xử lý thành công
            const themHocKyModal = new bootstrap.Modal(document.getElementById('themHocKyModal'));
            themHocKyModal.hide();

            // Xóa nội dung input sau khi thêm học kỳ
            modalInput.value = "";
        }
    });
});

function ThemHocKyMoiVaoNamHoc(tenHocKy, maNamHoc) {
    var formData = new FormData();
    formData.append("tenHocKy", tenHocKy);
    formData.append("maNamHoc", maNamHoc);
    $.ajax({
        url: '/Admin/NamHoc/ThemHocKyMoiVaoNamHoc',
        data: formData,
        contentType: false,
        processData: false,
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
                LoadBangThongTinNamHoc();

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
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}





function showModalThemHocKy(maNamHoc) {
    var formData = new FormData();
    formData.append("maNamHoc", maNamHoc);
    console.log("ma nam hoc la: " + maNamHoc);
    $.ajax({
        url: '/Admin/NamHoc/getDanhSachHocKyTheoNamHoc',
        data: formData,
        contentType: false,
        processData: false,
        type: 'POST',
        success: function (response) {
            if (response.success) {
                var data = response.data;
                var mainTable = document.getElementById("tableHocKy");
                var tenNamHoc = document.getElementById("tenNamHoc");
                tenNamHoc.innerHTML = '';
                tenNamHoc.innerHTML = response.tenNamHoc;
               
                var idNamHoc = document.getElementById("maNamHoc");
                idNamHoc.innerHTML = '<strong>Mã năm học:</strong> ' + maNamHoc;


                mainTable.innerHTML = '';
                if (data.length > 0) {
                    console.log(JSON.stringify(data));
                    for (let i = 0; i < data.length; i++) {
                        
                        console.log("ten nam hoc la: " + data[i].TenNamHoc);
                        var row = document.createElement('tr');
                        row.innerHTML = '<td>' + (i + 1) + '</td >' +
                            '<td>' + data[i].TenHocKy + '</td>';
                        mainTable.append(row);
                    }
                         
                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan=2 style="text-align: center;">không có dữ liệu hiện thị</td>'
                    mainTable.append(row);
                }
                const modalThemHocKy = new bootstrap.Modal(document.getElementById('themHocKyModal'));

                modalThemHocKy.show();
                document.getElementById('closeButton').addEventListener('click', function () { modalThemHocKy.hide(); });
                document.querySelector('.themHocKyBtnClose').addEventListener('click', function () { modalThemHocKy.hide(); });

                


            } 
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function MoModalCapNhatNamHoc(maNamHocVal, tenNamHocVal) {
    var maNamHoc = document.getElementById("edit-manamhoc");
    maNamHoc.textContent = maNamHocVal;
    var tenNamHoc = document.getElementById("luuNamHocInput");
   
    tenNamHoc.value = tenNamHocVal;
}



document.addEventListener("DOMContentLoaded", function () {
    const modalInput = document.getElementById("luuNamHocInput");
    const addButton = document.getElementById("luuNamHocSaveButton");
    const errorMessage = document.getElementById("loi-Edithocky");

    // Hàm kiểm tra dữ liệu nhập
    function validateInput() {
        const inputValue = modalInput.value.trim();

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

    // Bắt sự kiện nút "Thêm"
    addButton.addEventListener("click", function () {
        if (validateInput()) {
            console.log("Them thanh cong");
            var maNamHoc = document.getElementById("edit-manamhoc");

            ChinhSuaThongTinNamHoc(maNamHoc.textContent.trim(), modalInput.value.trim());
        }
    });
});


function ChinhSuaThongTinNamHoc(maNamHoc,tenNamHocMoi) {
    var formData = new FormData();
    formData.append("maNamHoc", maNamHoc);
    formData.append("tenNamHocMoi", tenNamHocMoi);

    $.ajax({
        url: '/Admin/NamHoc/ChinhSuaThongTinNamHoc',
        data: formData,
        contentType: false,
        processData: false,
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
                LoadBangThongTinNamHoc();

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
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

function XacNhanXoa(maNamHoc) {
    const modal = new bootstrap.Modal(document.getElementById('ModalXacNhanXoa'));
    modal.show();
    document.getElementById("XacNhanXoaHocKy").addEventListener('click', function () { XoaNamHocDaChon(maNamHoc, modal); });
}
function XoaNamHocDaChon(manMaHoc, modal) {
    modal.hide();
    //modal.dispose();
   
    var formData = new FormData();
    formData.append("maNamHoc", manMaHoc);
    $.ajax({
        url: '/Admin/NamHoc/XoaNamHoc',
        data: formData,
        contentType: false,
        processData: false,
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
                LoadBangThongTinNamHoc();

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
            document.getElementById('dongModalBaoLoi').addEventListener('click', function () { location.reload(); });
            
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

var xoaNamHocBtnConfirm = document.getElementById("xoaNamHocBtnConfirm");

xoaNamHocBtnConfirm.addEventListener('click', () => {
    // Thu thập danh sách ID của các hàng được chọn
    const selectedIds = [];
    const checkboxes = mainTableNamHoc.querySelectorAll('.rowCheckbox');

    checkboxes.forEach(checkbox => {
        if (checkbox.checked) {
            selectedIds.push(checkbox.value); // Lấy ID từ value của checkbox
        }
    });

    if (selectedIds.length === 0) {
        return;
    }
    console.log("co chay");
    fetch('/Admin/NamHoc/XoaDanhSachNamHoc', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ ids: selectedIds }), 
    })
        .then(response => response.json())
        .then(result => {
            var title = document.getElementById("errorModalLabel");
            var elements = document.getElementsByClassName("modal-mess-header");
            const errorModal = new bootstrap.Modal(document.getElementById("custom-error-modal"));
            var textmain = document.getElementById("text-main");
            if (result.success) {
                LoadBangThongTinNamHoc();
                title.innerHTML = '';
                title.innerHTML = "Thành Công";
                title.style.color = "white";
                document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "green";
                }
                textmain.innerHTML = result.message;
                LoadBangThongTinNamHoc();

            } else {
                title.innerHTML = "Lỗi";
                title.style.color = "white";
                document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.backgroundColor = "red";
                }
                textmain.innerHTML = result.message;
            }
            errorModal.show();

        })
        .catch(error => {
            console.error('Lỗi:', error);
            alert('Không thể xóa, vui lòng kiểm tra lại kết nối!');
        });
});

