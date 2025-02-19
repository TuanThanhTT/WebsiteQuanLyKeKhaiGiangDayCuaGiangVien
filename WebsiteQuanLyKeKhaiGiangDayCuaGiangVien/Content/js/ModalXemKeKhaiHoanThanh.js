// Lấy phần tử modal và nút mở modal
var modal = document.getElementById("modal-xem-lichsu-kekhai");
var btn = document.getElementById("open-modal");

// Mở modal khi nhấn nút
function openModal() {
    modal.style.display = "block";
}

// Đóng modal khi nhấn vào biểu tượng "x"
function closeModal() {
    modal.style.display = "none";
}

// Đóng modal khi nhấn vào ngoài vùng modal
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
