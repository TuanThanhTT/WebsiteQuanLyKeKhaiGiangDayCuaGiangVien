 

function convertDateFormat(dateString) {
    var parts = dateString.split("/");
    if (parts.length === 3) {
        return parts[1] + "/" + parts[0] + "/" + parts[2]; // MM/dd/yyyy
    }
    return null; // Trả về null nếu định dạng không đúng
}


document.addEventListener("DOMContentLoaded", function () {
    console.log("co chay load modal")
    loadModal();
});

function loadModal() {
    $.ajax({
        url: '/Admin/GiangVien/loadDanhSachKhoa',
        type: 'POST',
        success: function (response) {
            console.log("Dữ liệu nhận về:", response);

            if (response.success) {
                var data = response.data || []; // Đảm bảo không bị lỗi undefined
                var khoa = document.getElementById("khoa");
                var editKhoa = document.getElementById("edit-khoa");
                var Addkhoa = document.getElementById("Addkhoa");

                if (!khoa || !editKhoa || !Addkhoa) {
                    console.error("Không tìm thấy phần tử <select>");
                    return;
                }

                // Xóa danh sách cũ
                khoa.innerHTML = '<option value="">Chọn khoa</option>';
                editKhoa.innerHTML = '<option value="">Chọn khoa</option>';
                Addkhoa.innerHTML = '<option value="">Chọn khoa</option>';
                if (data.length > 0) {
                    data.forEach(k => {
                        let option = new Option(k.TenKhoa, k.MaKhoa);
                        let optionEdit = new Option(k.TenKhoa, k.MaKhoa);
                        let optionAdd = new Option(k.TenKhoa, k.MaKhoa);
                        khoa.appendChild(option);
                        editKhoa.appendChild(optionEdit);
                        Addkhoa.appendChild(optionAdd);
                    });
                } else {
                    console.warn("Danh sách khoa rỗng.");
                }
            } else {
                console.error("API trả về success = false");
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi AJAX:", error);
        }
    });

}


document.addEventListener("DOMContentLoaded", function () {
    var saveButton = document.getElementById("btnSave");  // Nút Lưu

    saveButton.addEventListener("click", function () {
        var isValid = true;

        // Lấy tất cả các trường cần kiểm tra
        var fields = [
            { id: "maGiangVien", message: "Mã giảng viên không được để trống." },
            { id: "tenGiangVien", message: "Tên giảng viên không được để trống." },
            { id: "Addkhoa", message: "Khoa không được để trống." },
            { id: "ngaySinh", message: "Ngày sinh không được để trống." },
            { id: "chucVu", message: "Chức vụ không được để trống." },
            { id: "trinhDo", message: "Trình độ không được để trống." },
            { id: "soDienThoai", message: "Số điện thoại không được để trống." },
            { id: "email", message: "Email không được để trống." },
            { id: "diaChi", message: "Địa chỉ không được để trống." },
            { id: "gioiTinh", message: "Giới tính không được để trống." },
            { id: "heSoLuong", message: "Hệ số lương không được để trống." },
            { id: "chuyenNganh", message: "Chuyên ngành không được để trống." },
            { id: "loaiHinhDaoTao", message: "Loại hình đào tạo không được để trống." }
        ];

        // Reset lại các lỗi trước
        resetErrors();

        // Kiểm tra tất cả các trường, nếu có trường nào bỏ trống, thêm lỗi
        fields.forEach(function (field) {
            var input = document.getElementById(field.id);
            var errorElement = document.getElementById(field.id + "Error");

            // Kiểm tra trường input (text, email, number, etc.)
            if (input && input.tagName.toLowerCase() !== "select" && !input.value.trim()) {
                input.classList.add("error");
                if (errorElement) {
                    errorElement.textContent = field.message;
                }
                isValid = false;
            }

            // Kiểm tra trường select
            if (input && input.tagName.toLowerCase() === "select") {
                // Nếu giá trị của select là rỗng hoặc chưa được chọn (chọn mặc định hoặc không có giá trị)
                if (!input.value || input.value === "") {
                    input.classList.add("error");
                    if (errorElement) {
                        errorElement.textContent = field.message;
                    }
                    isValid = false;
                }
            }
        });

        // Kiểm tra ngày sinh (phải đủ 24 tuổi)
        var ngaySinh = document.getElementById("ngaySinh").value;
        var ngaySinhError = document.getElementById("ngaySinhError");

        if (ngaySinh) {
            var birthDate = new Date(ngaySinh);
            var currentDate = new Date();
            var age = currentDate.getFullYear() - birthDate.getFullYear();
            var month = currentDate.getMonth() - birthDate.getMonth();
            if (month < 0 || (month === 0 && currentDate.getDate() < birthDate.getDate())) {
                age--;
            }

            if (age < 24) {
                document.getElementById("ngaySinh").classList.add("error");
                if (ngaySinhError) {
                    ngaySinhError.textContent = "Giảng viên phải đủ 24 tuổi.";
                }
                isValid = false;
            }
        } else {
            document.getElementById("ngaySinh").classList.add("error");
            if (ngaySinhError) {
                ngaySinhError.textContent = "Ngày sinh không được để trống.";
            }
            isValid = false;
        }

        // Nếu tất cả các trường hợp lệ, thực hiện xử lý khác
        if (isValid) {

            var maGV = document.getElementById("maGiangVien").value.trim();
            var tenGV = document.getElementById("tenGiangVien").value.trim();
            var maKhoa = document.getElementById("Addkhoa").value.trim();
            var ngaysinh = document.getElementById("ngaySinh").value.trim();
            var chucVu = document.getElementById("chucVu").value.trim();
            var trinhDo = document.getElementById("trinhDo").value.trim();
            var soDienThoai = document.getElementById("soDienThoai").value.trim();
            var email = document.getElementById("email").value.trim();
            var diaChi = document.getElementById("diaChi").value.trim();
            var gioiTinh = document.getElementById("gioiTinh").value.trim();
            var heSoLuong = document.getElementById("heSoLuong").value.trim();
            var chuyenNganh = document.getElementById("chuyenNganh").value.trim();
            var loaiHinhDaoTao = document.getElementById("loaiHinhDaoTao").value.trim();

            var ngaySinhFormatted = convertDateFormat(ngaysinh);
          
            themGiangVienMoi(maGV, tenGV, maKhoa, ngaySinhFormatted, soDienThoai, trinhDo, email, diaChi, gioiTinh, heSoLuong, chuyenNganh,
                loaiHinhDaoTao, chucVu);
           
        }
    });

    function resetErrors() {
        var errorMessages = document.querySelectorAll(".error-message");
        var inputs = document.querySelectorAll(".form-control");

        errorMessages.forEach(function (message) {
            message.textContent = "";
        });

        inputs.forEach(function (input) {
            input.classList.remove("error");
        });
    }
});



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


document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachGiangVien();
});


function loadDanhSachGiangVien(page = 1, pageSize = 5) {
    var formData = new FormData();
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/GiangVien/loadDanhSachGiangVien',
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
                   
                    for (let i = 0; i < data.length; i++) {
                        var row = document.createElement('tr');
                     
                        row.innerHTML = '<td><input type="checkbox" value="' + data[i].maGV +'" class="rowCheckbox"></td>' +
                            '<td>' + ((page - 1)*pageSize+1+i)+ '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].tenKhoa + '</td>' +
                            '<td>' + data[i].ngaySinh + '</td>' +
                            '<td>' +
                            '<button class="btn btn-primary" onclick="XemChiTietGiangVien(\'' + data[i].maGV + '\')" data-bs-toggle="modal" data-bs-target="#modalXemChiTiet">Xem</button>' +
                            '<button class="btn btn-warning" onclick="loadThongTinCapNhatGiangVien(\'' + data[i].maGV + '\')">Cập nhật</button>' +
                            '</td>';
                        
                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableGiangVien(response.totalPages, response.currentPage);

                } else {
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


function themGiangVienMoi(maGV, tenGV, maKhoa, ngaySinh, soDienThoai, trinhDo, email, diaChi, gioiTinh, heSoLuong, chuyenNganh, loaiHinhDaoTao, chucVu) {
    var formData = new FormData();
    formData.append("maGV", maGV);
    formData.append("tenGV", tenGV);
    formData.append("maKhoa", maKhoa);
    formData.append("ngaySinh", ngaySinh);
    formData.append("chucVu", chucVu);
    formData.append("trinhDo", trinhDo);
    formData.append("soDienThoai", soDienThoai);
    formData.append("email", email);
    formData.append("diaChi", diaChi);
    //formData.append("gioiTinh", gioiTinh);
    gioiTinh = gioiTinh === "true" ? true : false;
    formData.append("gioiTinh", gioiTinh);

    formData.append("heSoLuong", heSoLuong);
    formData.append("chuyenNganh", chuyenNganh);
    formData.append("loaiHinhDaoTao", loaiHinhDaoTao);
 
    $.ajax({
        url: '/Admin/GiangVien/ThemGiangVienMoi',
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

                errorModal.show();
                var closeButton = document.getElementById('dongModalBaoLoi');
                if (closeButton) {
                    closeButton.addEventListener('click', function () {

                        location.reload();
                    });
                } else { console.error("Close button not found"); } 
            } else {
                if (response.errName) {

                    var errText = document.getElementById(response.errName);
                    errText.innerHTML = '';
                    errText.innerHTML = response.message;
                    errText.style.display = 'block';


                } else {
                    title.innerHTML = "Lỗi";
                    title.style.color = "white";
                    document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                    for (var i = 0; i < elements.length; i++) {
                        elements[i].style.backgroundColor = "red";
                    }
                    textmain.innerHTML = response.message;
                    errorModal.show();
                    var closeButton = document.getElementById('dongModalBaoLoi');
                    if (closeButton) {
                        closeButton.addEventListener('click', function () {

                            location.reload();
                        });
                    } else { console.error("Close button not found"); } 
                }
              
            }

            // Hiển thị modal
           
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


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
                xemChuyenNghanh.innerHTML = data.chuyenNghanh;
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

//su kien chon file tai len
document.addEventListener("DOMContentLoaded", function () {
    var chonFile = document.getElementById("fileInput");
    var btnTaiFile = document.getElementById("btnTaiFile");


    btnTaiFile.disabled = true;

    chonFile.addEventListener("change", function () {

        if (chonFile.files && chonFile.files.length > 0) {
            btnTaiFile.disabled = false;
        } else {
            btnTaiFile.disabled = true;
        }
    });


    btnTaiFile.addEventListener("click", function () {
        uploadFileThemGiangVien(chonFile.files[0]);
    });
});


function uploadFileThemGiangVien(file) {
    var formData = new FormData();
    formData.append("file", file);
    $.ajax({
        url: '/Admin/GiangVien/UploadFileGiangVien',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            console.log(JSON.stringify(response));
            if (response.success) {


                var errMess = document.getElementById("errMess");
                errMess.innerHTML = '';
                errMess.style.display = 'none';

                var data = response.data;
                var mainTable = document.getElementById("filePreviewTable");
                mainTable.innerHTML = '';
                if (data.length > 0) {
                    var soLuongGiangVien = document.getElementById("soLuongGiangVien");
                    soLuongGiangVien.innerHTML = '';
                    soLuongGiangVien.innerHTML = data.length;
                    var soLuongTailen = document.getElementById("soLuongTailen");
                    soLuongTailen.style.display = 'block';
                    for (let i = 0; i < data.length; i++) {
                        var colortext = 'green';
                        var row = document.createElement('tr');
                        var ghiChu = "Hợp lệ";

                        if (data[i].ghiChu) {
                            ghiChu = data[i].ghiChu;
                            colortext = "red";
                        }
                    
               
                        row.innerHTML = '<td>' + data[i].id + '</td>' +
                            '<td>' + data[i].maGV + '</td>' +
                            '<td>' + data[i].tenGV + '</td>' +
                            '<td>' + data[i].maKhoa + '</td>' +
                            '<td>' + data[i].ngaySinh + '</td>' +
                            '<td>' + data[i].soDienThoai + '</td>' +
                            '<td>' + data[i].email + '</td>' +
                            '<td>' + data[i].diaChi + '</td>' +
                            '<td>' + data[i].gioiTinh + '</td>' +
                            '<td>' + data[i].chucVu + '</td>' +
                            '<td>' + data[i].heSoLuong + '</td>' +
                            '<td>' + data[i].trinhDo + '</td>' +
                            '<td>' + data[i].chuyenNghanh + '</td>' +
                            '<td>' + data[i].loaiHinhDaoTao + '</td>' +
                            '<td style="color: ' + colortext + ';">' + ghiChu + '</td>';
                   
                        mainTable.appendChild(row);
                    }
                }
                var saveFileData = document.getElementById("saveFileData");
                if (response.valid) {

                    saveFileData.style.visibility = 'visible';
                    saveFileData.addEventListener('click', function () {
                        LuuDanhSachGiangVienThem();
                    });

                } else {
                    var soLuongTailen = document.getElementById("soLuongTailen");
                    soLuongTailen.style.display = 'none';
                    saveFileData.style.visibility = 'hidden';
                    var errMess = document.getElementById("errMess");
                    errMess.innerHTML = '';
                    errMess.innerHTML = '*Dữ liệu file không hợp lệ!'
                    errMess.style.display = 'block';

                }

            } else {

                var errMess = document.getElementById("errMess");
                errMess.innerHTML = '';
                errMess.innerHTML = response.message;
                errMess.style.display = 'block';


            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}



function LuuDanhSachGiangVienThem() {
    $.ajax({
        url: '/Admin/GiangVien/LuuThongTinGiangVienTaiLen',
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
//cap nhat giang vien
document.addEventListener("DOMContentLoaded", function () {
    var btnMauFile =  document.getElementById("btnMauFile");
    btnMauFile.addEventListener("click", TaiMauFileGiangVien);
    var btnsave = document.getElementById("btnedit-Save");  // Nút Lưu

    btnsave.addEventListener("click", function () {
        var isValid = true;
        console.log("co chay");
        // Lấy tất cả các trường cần kiểm tra
        var fields = [
            { id: "edit-tenGiangVien", message: "Tên giảng viên không được để trống." },
            { id: "edit-khoa", message: "Khoa không được để trống." },
            { id: "edit-ngaySinh", message: "Ngày sinh không được để trống." },
            { id: "edit-chucVu", message: "Chức vụ không được để trống." },
            { id: "edit-trinhDo", message: "Trình độ không được để trống." },
            { id: "edit-soDienThoai", message: "Số điện thoại không được để trống." },
            { id: "edit-email", message: "Email không được để trống." },
            { id: "edit-diaChi", message: "Địa chỉ không được để trống." },
            { id: "edit-gioiTinh", message: "Giới tính không được để trống." },
            { id: "edit-heSoLuong", message: "Hệ số lương không được để trống." },
            { id: "edit-chuyenNganh", message: "Chuyên ngành không được để trống." },
            { id: "edit-loaiHinhDaoTao", message: "Loại hình đào tạo không được để trống." }
        ];

        resetErrors();

    
        fields.forEach(function (field) {
            var input = document.getElementById(field.id);
            var errorElement = document.getElementById("edit-"+field.id + "Error");

            if (input && input.tagName.toLowerCase() !== "select" && !input.value.trim()) {
                input.classList.add("error");
                if (errorElement) {
                    errorElement.textContent = field.message;
                }
                isValid = false;
                console.log("loi ơ day: "+field.id);
            }

          
            if (input && input.tagName.toLowerCase() === "select") {

                if (!input.value || input.value === "") {
                    input.classList.add("error");
                    if (errorElement) {
                        errorElement.textContent = field.message;
                    }
                    isValid = false;
                    console.log("loi ơ day: " + field.id);
                }
            }
        });

       
        var ngaySinh = document.getElementById("edit-ngaySinh").value;
        var ngaySinhError = document.getElementById("edit-ngaySinhError");

        if (ngaySinh) {
            var birthDate = new Date(ngaySinh);
            var currentDate = new Date();
            var age = currentDate.getFullYear() - birthDate.getFullYear();
            var month = currentDate.getMonth() - birthDate.getMonth();
            if (month < 0 || (month === 0 && currentDate.getDate() < birthDate.getDate())) {
                age--;
            }

            if (age < 24) {
                document.getElementById("edit-ngaySinh").classList.add("error");
                if (ngaySinhError) {
                    ngaySinhError.textContent = "Giảng viên phải đủ 24 tuổi.";
                }
                isValid = false;
                console.log("loi ơ day");
            }
        } else {
            document.getElementById("edit-ngaySinh").classList.add("error");
            if (ngaySinhError) {
                ngaySinhError.textContent = "Ngày sinh không được để trống.";
            }
            isValid = false;
            console.log("loi ơ day");
        }

        // Nếu tất cả các trường hợp lệ, thực hiện xử lý khác
        if (isValid) {


            var maGV = document.getElementById("edit-maGiangVien").value.trim();
            var tenGV = document.getElementById("edit-tenGiangVien").value.trim();
            var maKhoa = document.getElementById("edit-khoa").value.trim();
            var ngaysinh = document.getElementById("edit-ngaySinh").value.trim();
            var chucVu = document.getElementById("edit-chucVu").value.trim();
            var trinhDo = document.getElementById("edit-trinhDo").value.trim();
            var soDienThoai = document.getElementById("edit-soDienThoai").value.trim();
            var email = document.getElementById("edit-email").value.trim();
            var diaChi = document.getElementById("edit-diaChi").value.trim();
            var gioiTinh = document.getElementById("edit-gioiTinh").value.trim();
            var heSoLuong = document.getElementById("edit-heSoLuong").value.trim();
            var chuyenNganh = document.getElementById("edit-chuyenNganh").value.trim();
            var loaiHinhDaoTao = document.getElementById("edit-loaiHinhDaoTao").value.trim();

            var ngaySinhFormatted = convertDateFormat(ngaysinh);
            console.log("ma khoa cap nhat moi la: " + maKhoa);
            console.log("Có cahy nut save");
            capNhatThongTinGiangVien(maGV, tenGV, maKhoa, ngaySinhFormatted, soDienThoai, trinhDo, email, diaChi, gioiTinh, heSoLuong, chuyenNganh,
                loaiHinhDaoTao, chucVu);

        } else {
            console.log("Không hop le de luu thong tin cap nhat");
        }
    });

    function resetErrors() {
        var errorMessages = document.querySelectorAll(".error-message");
        var inputs = document.querySelectorAll(".form-control");

        errorMessages.forEach(function (message) {
            message.textContent = "";
        });

        inputs.forEach(function (input) {
            input.classList.remove("error");
        });
    }
});

function capNhatThongTinGiangVien(maGV, tenGV, maKhoa, ngaySinh, soDienThoai, trinhDo, email, diaChi, gioiTinh, heSoLuong, chuyenNganh, loaiHinhDaoTao, chucVu) 
{
    console.log("gioi tinh la: " + gioiTinh)
    var formData = new FormData();
    formData.append("maGV", maGV);
    formData.append("tenGV", tenGV);
    formData.append("maKhoa", maKhoa);
    formData.append("ngaySinh", ngaySinh);
    formData.append("chucVu", chucVu);
    formData.append("trinhDo", trinhDo);
    formData.append("soDienThoai", soDienThoai);
    formData.append("email", email);
    formData.append("diaChi", diaChi);
    console.log("Ma khoa can cap nhat moi la: " + maKhoa);
   // formData.append("gioiTinh", gioiTinh);
    formData.append("gioiTinh", gioiTinh === "1" || gioiTinh === 1 ? true : false);

    formData.append("heSoLuong", heSoLuong);
    formData.append("chuyenNganh", chuyenNganh);
    formData.append("loaiHinhDaoTao", loaiHinhDaoTao);

    $.ajax({
        url: '/Admin/GiangVien/CapNhatThongTinGiangVien',
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
                closeModalById("modalCapNhatThongTinGiangVien");
                errorModal.show();
                var closeButton = document.getElementById('dongModalBaoLoi');
                if (closeButton) {
                    closeButton.addEventListener('click', function () {

                        location.reload();
                    });
                } else { console.error("Close button not found"); }
            } else {
                if (response.errName) {

                    var errText = document.getElementById(response.errName);
                    errText.innerHTML = '';
                    errText.innerHTML = response.message;
                    errText.style.display = 'block';
                } else {
                    title.innerHTML = "Lỗi";
                    title.style.color = "white";
                    document.querySelector('.btn-close-mess').style.color = "white"; // Đặt màu nút đóng

                    for (var i = 0; i < elements.length; i++) {
                        elements[i].style.backgroundColor = "red";
                    }
                    textmain.innerHTML = response.message;
                    errorModal.show();
                    var closeButton = document.getElementById('dongModalBaoLoi');
                    if (closeButton) {
                        closeButton.addEventListener('click', function () {

                            location.reload();
                        });
                    } else { console.error("Close button not found"); }
                }

            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function loadThongTinCapNhatGiangVien(maGV) {


    var formData = new FormData();
    formData.append("maGV", maGV);

    $.ajax({
        url: '/Admin/GiangVien/loadThongTinGiangVienTheoMa',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                console.log("thong tin xem cap nhat gv: " + JSON.stringify(response));
                var data = response.data;
                if (data) {
                    var maGV = document.getElementById("edit-maGiangVien");
                    var tenGV = document.getElementById("edit-tenGiangVien");
                    var khoa = document.getElementById("edit-khoa");
                    var ngaySinh = document.getElementById("edit-ngaySinh");
                    var chucVu = document.getElementById("edit-chucVu");
                    var trinhDo = document.getElementById("edit-trinhDo");
                    var soDienThoai = document.getElementById("edit-soDienThoai");
                    var email = document.getElementById("edit-email");
                    var diaChi = document.getElementById("edit-diaChi");
                    var gioiTinh = document.getElementById("edit-gioiTinh");
                    var heSoLuong = document.getElementById("edit-heSoLuong");
                    var chuyenNganh = document.getElementById("edit-chuyenNganh");
                    var loaiHinhDaoTao = document.getElementById("edit-loaiHinhDaoTao");

                    maGV.value = data.maGV;
                    tenGV.value = data.tenGV;
                   
                    trinhDo.value = data.trinhDo;
                    soDienThoai.value = data.soDienThoai;
                    email.value = data.email;
                    diaChi.value = data.diaChi;
                    heSoLuong.value = data.heSoLuong;
                    chuyenNganh.value = data.chuyenNghanh;
                    if (data.gioiTinh === "Nam") {
                        gioiTinh.value = 1;
                    } else {
                        gioiTinh.value = 0;
                    }
                    chucVu.value = data.chucVu;
                    khoa.value = data.maKhoa;
                    console.log("ma khoa xem cap nhat la: "+data.maKhoa);
                    loaiHinhDaoTao.value = data.loaiHinhDaoTao;
                    console.log("loai hinh dao tao la: " + data.loaiHinhDaoTao);

                    var timestampMatch = data.namSinh.match(/\d+/);

                    if (timestampMatch) {
                        var timestamp = parseInt(timestampMatch[0], 10);

                      
                        var dateTime = new Date(timestamp);

                      
                        var formattedDate = dateTime.toISOString().split("T")[0];

                      
                        document.getElementById("edit-ngaySinh").value = formattedDate;

                        flatpickr("#edit-ngaySinh", {
                            dateFormat: "d/m/Y",  
                            defaultDate: formattedDate
                        });
                    } else {
                        console.error("Lỗi: Định dạng namSinh không hợp lệ!", data.namSinh);
                    }

             
                    //var dateTime = new Date(data.namSinh);
                    //var formattedDate = dateTime.toISOString().split("T")[0];


                    //ngaySinh.value = formattedDate;

                 
                    //flatpickr("#edit-ngaySinh", {
                    //    dateFormat: "d/m/Y",  // Hiển thị theo d/m/Y
                    //    defaultDate: formattedDate
                    //});

                    
                }

            } else {
                alert(response.message);
            }
            var modalCapNhatThongTin = new bootstrap.Modal(document.getElementById("modalCapNhatThongTinGiangVien"));

            modalCapNhatThongTin.show();

            

        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });


    
}

function closeModalById(modalId) {
    var modalElement = document.getElementById(modalId);

    if (modalElement && modalElement.classList.contains("show")) {
        var modalInstance = bootstrap.Modal.getInstance(modalElement);

        if (modalInstance) {
            modalInstance.hide(); // Đóng modal
        }
    }
}

function TaiMauFileGiangVien() {
    $.ajax({
        url: '/Admin/GiangVien/DowloadMauFile',
        type: 'POST',
        success: function (data) {
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = 'MauDanhSachGiangVien.xlsx';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },
        xhrFields: {
            responseType: 'blob'
        }
    });

}


document.addEventListener("DOMContentLoaded", function () {
    var btnXoaGiangVien = document.getElementById("btnXoaGiangVien");

    // Gắn sự kiện click cho nút xóa
    btnXoaGiangVien.addEventListener("click", function () {
        const checkboxes = document.querySelectorAll('.rowCheckbox');
        const isAnyChecked = Array.from(checkboxes).some(checkbox => checkbox.checked);

        if (isAnyChecked) {

            XacNhanXoaGiangVien();
        } else {
            // Hiển thị thông báo nếu không có checkbox nào được chọn
            alert("Vui lòng chọn ít nhất một giảng viên để xóa.");
        }
    });
});


function XacNhanXoaGiangVien() {



    const checkboxes = document.querySelectorAll('.rowCheckbox');

    const selectedIds = [];
    checkboxes.forEach(checkbox => {
        if (checkbox.checked) {
            selectedIds.push(checkbox.value);
        }
    });

    if (selectedIds.length === 0) {
        alert("Vui lòng chọn ít nhất một mục để xóa.");
        return;
    }


    var modalConfirmKhoa = document.getElementById("modalConfirmKhoa");
    modalConfirmKhoa.style.display = "block";

    const confirmBtn = document.getElementById("confirmBtn");
    confirmBtn.addEventListener("click", function () {
        xoaDanhSach(selectedIds);
        modalConfirmKhoa.style.display = "none";

    });
}



function xoaDanhSach(idGiangViens) {
   
    var formData = new FormData();
    formData.append("idGiangViens", JSON.stringify(idGiangViens));
    $.ajax({
        url: '/Admin/GiangVien/XoaDanhSachGiangVien',
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

//load khoa

document.addEventListener("DOMContentLoaded", function () {
    loadDanhSachKhoa();
    var btnLamMoi = document.getElementById("btnLamMoi");
    btnLamMoi.addEventListener("click", function () {
        reloadPage();
    });
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
                        option.value = data[i].MaKhoa;
                        option.textContent = data[i].TenKhoa;
                        mainKhoa.appendChild(option);
                    }
                }
            }

            // Lưu lại sự kiện xử lý để đảm bảo removeEventListener hoạt động chính xác
            function handleLocPhanTheoKhoa() {
                loadDanhSachGiangVienTheoKhoa();   
            }

            function handleLocPhanTheoKhoaVaChuoiTim() {
                loadDanhSachGiangVienTheoChuoiTimVaKhoa();
            }

            // Sự kiện khi chọn Khoa
            mainKhoa.addEventListener("change", function () {
                var selectedId = mainKhoa.value.trim();
                var btnLocPhanTheoKhoa = document.getElementById("btnLocPhanTheoKhoa");
                var chuoiTim = document.getElementById("txtChuoiTim").value.trim();

                // Xóa tất cả sự kiện click trước đó để tránh đăng ký nhiều lần
                btnLocPhanTheoKhoa.removeEventListener("click", handleLocPhanTheoKhoa);
                btnLocPhanTheoKhoa.removeEventListener("click", handleLocPhanTheoKhoaVaChuoiTim);

                if (selectedId !== "" && chuoiTim === "") {
                    btnLocPhanTheoKhoa.disabled = false;
                    btnLocPhanTheoKhoa.addEventListener("click", handleLocPhanTheoKhoa);
                } else if (selectedId !== "" && chuoiTim !== "") {
                    btnLocPhanTheoKhoa.disabled = false;
                    btnLocPhanTheoKhoa.addEventListener("click", handleLocPhanTheoKhoaVaChuoiTim);
                } else {
                    btnLocPhanTheoKhoa.disabled = true;
                }
            });
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}
//lam moi trang
function reloadPage() {
    location.reload();
}


//loc theo khoa

function loadDanhSachGiangVienTheoKhoa(page = 1, pageSize = 5) {
    var khoa = document.getElementById("khoa");
    var formData = new FormData();
    formData.append("maKhoa", khoa.value);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/GiangVien/laodTableGiangVienTheoKhoa',
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
                            '<button class="btn btn-warning" onclick="loadThongTinCapNhatGiangVien(\'' + data[i].maGV + '\')">Cập nhật</button>' +
                            '</td>';

                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableGiangVienTheoKhoa(response.totalPages, response.currentPage);

                } else {
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


function updatePhanTrangTableGiangVienTheoKhoa(totalPages, currentPage) {
   
    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTheoKhoa(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTheoKhoa(' + page + ')">' + label + '</a>';
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

//tim kiem theo chuoi tim
function loadDanhSachGiangVienTheoChuoiTim(page = 1, pageSize = 5) {
    var chuoiTim = document.getElementById("txtChuoiTim").value;
    var formData = new FormData();
    formData.append("chuoiTim", chuoiTim);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/GiangVien/laodTableGiangVienTheoChuoiTim',
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
                            '<button class="btn btn-warning" onclick="loadThongTinCapNhatGiangVien(\'' + data[i].maGV + '\')">Cập nhật</button>' +
                            '</td>';

                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableGiangVienTheoChuoiTim(response.totalPages, response.currentPage);

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan= "7" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    mainTable.appendChild(row);
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function updatePhanTrangTableGiangVienTheoChuoiTim(totalPages, currentPage) {

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTheoChuoiTim(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTheoChuoiTim(' + page + ')">' + label + '</a>';
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

//tim kiem theo chuoi tim va khoa
function loadDanhSachGiangVienTheoChuoiTimVaKhoa(page = 1, pageSize = 5) {
    var khoa = document.getElementById("khoa");
    var chuoiTim = document.getElementById("txtChuoiTim").value;

    var formData = new FormData();
    formData.append("chuoiTim", chuoiTim);
    formData.append("maKhoa", khoa.value);
    formData.append("page", page);
    formData.append("pageSize", pageSize);
    $.ajax({
        url: '/Admin/GiangVien/laodTableGiangVienTheoKhoaVaChuoiTim',
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
                            '<button class="btn btn-warning" onclick="loadThongTinCapNhatGiangVien(\'' + data[i].maGV + '\')">Cập nhật</button>' +
                            '</td>';

                        mainTable.appendChild(row);
                    }
                    updatePhanTrangTableGiangVienTheoChuoiTimVaKhoa(response.totalPages, response.currentPage);

                } else {
                    var row = document.createElement('tr');
                    row.innerHTML = '<td colspan= "7" style="text-align: center;">Không có dữ liệu hiện thị</td>';
                    mainTable.appendChild(row);
                }
            }
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}


function updatePhanTrangTableGiangVienTheoChuoiTimVaKhoa(totalPages, currentPage) {

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">...</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTheoChuoiTimVaKhoa(' + page + ')">' + page + '</a>';
        pagination.appendChild(li);
    }

    var pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Xóa nội dung phân trang cũ

    function createPageItem(page, label, isActive = false, isDisabled = false) {
        var li = document.createElement('li');
        li.className = "page-item" + (isActive ? " active" : "") + (isDisabled ? " disabled" : "");
        li.innerHTML = isDisabled
            ? '<span class="page-link">' + label + '</span>'
            : '<a class="page-link" href="#" onclick="loadDanhSachGiangVienTheoChuoiTimVaKhoa(' + page + ')">' + label + '</a>';
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


//tim kiem
document.addEventListener("DOMContentLoaded", function () {
    var txtChuoiTim = document.getElementById("txtChuoiTim");
    var btnTimKiemPhanCong = document.getElementById("btnTimKiemPhanCong");
    function timKiemGiangVien() {
        loadDanhSachGiangVienTheoChuoiTim();
    }

    txtChuoiTim.addEventListener("input", function () {
        if (txtChuoiTim.value.trim() !== "") {

            btnTimKiemPhanCong.disabled = false;
            btnTimKiemPhanCong.addEventListener("click", timKiemGiangVien);
        } else {
            btnTimKiemPhanCong.disabled = true;
            btnTimKiemPhanCong.removeEventListener("click", timKiemGiangVien);
        }
    });
});
