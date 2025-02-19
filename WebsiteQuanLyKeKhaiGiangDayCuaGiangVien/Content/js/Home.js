let currentIndex = 0;
const slides = document.querySelectorAll('.slide');
const totalSlides = slides.length;

// Hàm chuyển đổi slide
function changeSlide(direction) {
    slides[currentIndex].classList.remove('active');
    currentIndex = (currentIndex + direction + totalSlides) % totalSlides;
    slides[currentIndex].classList.add('active');
}

// Tự động chuyển đổi slide sau mỗi 5 giây
setInterval(() => {
    changeSlide(1);
}, 8000);