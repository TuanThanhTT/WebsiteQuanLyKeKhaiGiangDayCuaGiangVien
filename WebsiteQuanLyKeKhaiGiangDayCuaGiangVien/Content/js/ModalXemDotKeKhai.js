// Hàm đóng modal
function closeModalXemDotKeKhai() {
    const modal = document.getElementById("modalXemDotKeKhai");
    if (modal) {
        modal.style.display = "none"; // Ẩn modal
    }
}

// Gắn sự kiện đóng modal khi nhấn nút "Đóng"
document.getElementById("closeModalXemDotKeKhai").addEventListener("click", closeModalXemDotKeKhai);
document.getElementById("closeModalXemDotKeKhaiHeader").addEventListener("click", closeModalXemDotKeKhai);

// Tùy chọn: Đóng modal khi nhấn ra ngoài
window.addEventListener("click", function (event) {
    const modal = document.getElementById("modalXemDotKeKhai");
    if (event.target === modal) {
        closeModalXemDotKeKhai();
    }
});