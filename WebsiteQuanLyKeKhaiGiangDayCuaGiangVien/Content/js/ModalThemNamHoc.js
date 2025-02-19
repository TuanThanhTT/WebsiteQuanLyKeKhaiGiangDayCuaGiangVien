// Xử lý nút "Lưu"
document.querySelector('.themNamHocBtnSave').addEventListener('click', function () {
    const tenNamHoc = document.getElementById('tenNamHoc').value;
    if (tenNamHoc) {
        alert('Lưu năm học: ' + tenNamHoc);
        // Thực hiện lưu dữ liệu năm học (gửi qua API hoặc lưu vào cơ sở dữ liệu)
        $('#themNamHocModal').modal('hide');  // Đóng modal sau khi lưu
    } else {
        alert('Vui lòng nhập tên năm học.');
    }
});

// Xử lý nút "Xóa"
document.querySelector('.themNamHocBtnClear').addEventListener('click', function () {
    document.getElementById('tenNamHoc').value = '';  // Xóa trường nhập tên năm học
});