
document.addEventListener("DOMContentLoaded", function () {
    var modal = document.getElementById("modal-chinhsua-kekhai");

    window.addEventListener('click', function (event) {
        if (event.target === modal) {
            closeModal();
        }
    });
});

// Hàm mở Modal
function openModal() {
    document.getElementById('modal-chinhsua-kekhai').style.display = 'block';
}

// Hàm đóng Modal
function closeModal() {
    document.getElementById('modal-chinhsua-kekhai').style.display = 'none';
}