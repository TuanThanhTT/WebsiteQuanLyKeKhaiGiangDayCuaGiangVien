// Lấy modal từ Bootstrap
const errorModal = new bootstrap.Modal(document.getElementById("Modal-main-err"));

// Lấy các phần tử đóng modal
const closeErrorBtn = document.getElementById("closeErrorModal");
const iconErrMesss = document.getElementById("iconModalErr");

// Đóng modal khi nhấn nút "Đóng"
closeErrorBtn.addEventListener("click", function () {
    errorModal.hide(); // Đóng modal bằng API Bootstrap
});

// Đóng modal khi nhấn vào icon (nút x)
iconErrMesss.addEventListener("click", function () {
    errorModal.hide(); // Đóng modal bằng API Bootstrap
});

// Đóng modal khi nhấn ra ngoài modal
document.addEventListener("click", function (event) {
    const modalElement = document.getElementById("Modal-main-err");
    if (event.target === modalElement) {
        errorModal.hide(); // Đóng modal bằng API Bootstrap
    }
});

// Xử lý sự cố lớp phủ (backdrop) không bị xóa
document.addEventListener("hidden.bs.modal", function () {
    const backdrop = document.querySelector(".modal-backdrop");
    if (backdrop) {
        backdrop.remove(); // Xóa lớp phủ nếu còn tồn tại
    }
    document.body.classList.remove("modal-open"); // Loại bỏ lớp 'modal-open' khỏi body
    document.body.style.paddingRight = ""; // Đặt lại padding nếu bị thay đổi
});
