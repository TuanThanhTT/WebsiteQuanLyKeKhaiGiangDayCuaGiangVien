
document.addEventListener("DOMContentLoaded", function () {
    // Get modal and buttons
    const modal = document.getElementById("modalChinhSuaDotKeKhai");
    //const openModal = document.getElementById("openModalChinhSua"); // Bạn cần có một button hoặc event để mở modal
    const closeModal = document.getElementById("closeModalChinhSua");
    const closeModalHeader = document.getElementById("closeModalChinhSuaHeader");

    // Open modal
    //openModal.addEventListener("click", function () {
    //    modal.style.display = "block";
    //    modal.setAttribute("aria-hidden", "false");
    //    modal.setAttribute("aria-modal", "true");
    //});

    // Close modal using button in header
    closeModalHeader.addEventListener("click", function () {
        modal.style.display = "none";
        modal.setAttribute("aria-hidden", "true");
        modal.setAttribute("aria-modal", "false");
    });

    // Close modal using button in footer
    closeModal.addEventListener("click", function () {
        modal.style.display = "none";
        modal.setAttribute("aria-hidden", "true");
        modal.setAttribute("aria-modal", "false");
    });

    // Close modal if clicked outside of the modal
    window.onclick = function (event) {
        if (event.target === modal) {
            modal.style.display = "none";
            modal.setAttribute("aria-hidden", "true");
            modal.setAttribute("aria-modal", "false");
        }
    };

    // Khởi tạo flatpickr cho các trường datetime-local
    flatpickr("#startDate", {
        enableTime: true,
        dateFormat: "d/m/Y H:i",  // Định dạng ngày giờ
        time_24hr: true  // Chế độ 24 giờ
    });

    flatpickr("#endDate", {
        enableTime: true,
        dateFormat: "d/m/Y H:i",  // Định dạng ngày giờ
        time_24hr: true  // Chế độ 24 giờ
    });
  

    flatpickr("#edit-endDate", {
        enableTime: true,
        dateFormat: "d/m/Y H:i",  // Định dạng ngày giờ
        time_24hr: true  // Chế độ 24 giờ
    });

});
