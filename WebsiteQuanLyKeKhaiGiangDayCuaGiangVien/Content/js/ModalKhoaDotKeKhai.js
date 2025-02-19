document.addEventListener("DOMContentLoaded", function () {
    const modalConfirm = document.getElementById("modalConfirmKhoa");
   // const openModalBtn = document.getElementById("openModalKhoaDotKeKhai"); // Nút mở modal (thêm vào HTML)
    const closeModalBtn = document.getElementById("closeModalConfirm");
    const cancelBtn = document.getElementById("cancelBtn");
    

  
    // Đóng modal
    closeModalBtn.addEventListener("click", function () {
        modalConfirm.style.display = "none";
        modalConfirm.setAttribute("aria-hidden", "true");
        modalConfirm.setAttribute("aria-modal", "false");
    });

    // Hủy (Đóng modal mà không làm gì)
    cancelBtn.addEventListener("click", function () {
        modalConfirm.style.display = "none";
        modalConfirm.setAttribute("aria-hidden", "true");
        modalConfirm.setAttribute("aria-modal", "false");
    });

    

    // Đóng modal khi người dùng nhấn ra ngoài modal
    window.onclick = function (event) {
        if (event.target === modalConfirm) {
            modalConfirm.style.display = "none";
            modalConfirm.setAttribute("aria-hidden", "true");
            modalConfirm.setAttribute("aria-modal", "false");
        }
    };
});
