// Chọn hoặc bỏ chọn tất cả
document.getElementById("selectAll").addEventListener("change", function () {
    const checkboxes = document.querySelectorAll(".rowCheckbox");
    checkboxes.forEach(cb => cb.checked = this.checked);
});