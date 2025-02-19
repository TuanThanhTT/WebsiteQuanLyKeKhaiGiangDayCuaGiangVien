document.querySelector('.btn-close-mess').addEventListener('click', () => {
    console.log("Đã nhấn nút X để tắt modal");
    const errorModal = bootstrap.Modal.getInstance(document.getElementById("custom-error-modal"));
    errorModal.hide(); // Ẩn modal
});

document.querySelector('.btn-secondary-mess').addEventListener('click', () => {
    console.log("Đã nhấn nút đóng");
    const errorModal = bootstrap.Modal.getInstance(document.getElementById("custom-error-modal"));
    errorModal.hide(); // Ẩn modal
});