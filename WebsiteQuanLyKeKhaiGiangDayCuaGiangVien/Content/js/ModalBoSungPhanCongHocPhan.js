// Hàm đóng modal khi nhấn nút "Hủy" hoặc dấu "X"
document.getElementById('tbsPhanCongHP-huy').addEventListener('click', function () {
    var modal = document.getElementById('tbsPhanCongHP-modal');
    modal.classList.remove('show'); // Loại bỏ class 'show' để ẩn modal
});

// Đóng modal khi nhấn dấu "X"
document.getElementById('icon-DongModalPhanCongBoSung').addEventListener('click', function () {
    var modal = document.getElementById('tbsPhanCongHP-modal');
    modal.classList.remove('show'); // Loại bỏ class 'show' để ẩn modal
});



