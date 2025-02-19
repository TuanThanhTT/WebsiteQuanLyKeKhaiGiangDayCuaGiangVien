document.addEventListener("DOMContentLoaded", function () {
    var dongModal = document.getElementById("dongModal-icon");

    if (dongModal) {
        dongModal.addEventListener('click', closeModal);
    } else {
        console.log("Không tìm thấy phần tử modal-xem-kekhai-close");
    }

    var btnDOng = document.getElementById("dongModal-button");
    btnDOng.addEventListener('click', closeModal);

    var modal = document.getElementById("modal-xem-kekhai");


    window.addEventListener('click', function (event) {
        if (event.target === modal) {
            closeModal();
        }
    });

    function openModal() {
        console.log("Có mở modal xem");
        document.getElementById('modal-xem-kekhai').style.display = 'block';
    }

    function closeModal() {
        console.log("Có đóng modal xem");
        document.getElementById('modal-xem-kekhai').style.display = 'none';
    }
});
