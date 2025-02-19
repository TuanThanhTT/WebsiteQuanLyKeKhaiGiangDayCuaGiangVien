
var ngayKetThuc;
var run = false;

// Hàm xử lý ngày tháng từ server với Moment.js
function layRaNgayKeThucKeKhai() {
    $.ajax({
        url: '/Admin/KeKhai/NgayKeThucKeKhai',
        type: 'POST',
        success: function (response) {

            if (response.sapMo) {

                // Xử lý chuỗi ngày tháng và giờ từ server theo định dạng "DD/MM/YYYY hh:mm:ss A"
                var ngayBatDauString = response.ngayBatDau; // Ví dụ: "4/1/2025 11:00:00 PM"

                // Chuyển định dạng ngày tháng về đúng cho Moment.js
                // Sử dụng moment() với định dạng "D/M/YYYY h:mm:ss A"
                ngayKetThuc = new Date(ngayBatDauString);

                run = true; // Cập nhật cờ run sau khi lấy được ngày

                // Hiển thị countdown panel
                var titleCounDown = document.getElementById("titleCounDown");
                titleCounDown.innerHTML = '';
                titleCounDown.innerHTML = 'Đợt kê khai bát đầu sau: ';
                var countDownPanel = document.getElementById("countDownPanel");
                countDownPanel.style.display = 'block';

            } else {
                if (response.success && response.ngayKetThuc) {

                   
                    // Xử lý chuỗi ngày tháng và giờ từ server theo định dạng "DD/MM/YYYY hh:mm:ss A"
                    var ngayKetThucString = response.ngayKetThuc; // Ví dụ: "4/1/2025 11:00:00 PM"

                    // Chuyển định dạng ngày tháng về đúng cho Moment.js
                    // Sử dụng moment() với định dạng "D/M/YYYY h:mm:ss A"
                    ngayKetThuc = new Date(ngayKetThucString);

                    run = true; // Cập nhật cờ run sau khi lấy được ngày

                    // Hiển thị countdown panel
                    var titleCounDown = document.getElementById("titleCounDown");
                    titleCounDown.innerHTML = '';
                    titleCounDown.innerHTML = 'Thời gian đợt kê khai còn lại: ';
                    var countDownPanel = document.getElementById("countDownPanel");
                    countDownPanel.style.display = 'block';
                }
            }

           
        },
        error: function (xhr, status, error) {

            alert("Có lỗi xảy ra khi lấy ngày kết thúc: " + error);
        }
    });
}

// Hàm cập nhật countdown
function capNhatCountdown() {
    const hienTai = new Date(); // Lấy thời gian hiện tại
    const khoangCach = ngayKetThuc - hienTai; // Tính khoảng cách thời gian còn lại


    if (khoangCach <= 0) {
        // Nếu countdown kết thúc
        document.getElementById("ngayKeKhai").innerText = "00";
        document.getElementById("gioiKeKhai").innerText = "00";
        document.getElementById("phutKeKhai").innerText = "00";
        document.getElementById("giayKeKhai").innerText = "00";
        clearInterval(countdownInterval); // Dừng countdown

        var countDownPanel = document.getElementById("countDownPanel");
        countDownPanel.style.display = 'none';
        location.reload();
        return;
    }

    // Tính số ngày, giờ, phút, giây còn lại
    const ngay = Math.floor(khoangCach / (1000 * 60 * 60 * 24));
    const gio = Math.floor((khoangCach % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const phut = Math.floor((khoangCach % (1000 * 60 * 60)) / (1000 * 60));
    const giay = Math.floor((khoangCach % (1000 * 60)) / 1000);

    // Hiển thị countdown
    document.getElementById("ngayKeKhai").innerText = ngay.toString().padStart(2, '0');
    document.getElementById("gioiKeKhai").innerText = gio.toString().padStart(2, '0');
    document.getElementById("phutKeKhai").innerText = phut.toString().padStart(2, '0');
    document.getElementById("giayKeKhai").innerText = giay.toString().padStart(2, '0');
}

// Gọi hàm lấy ngày kết thúc khi trang web load
layRaNgayKeThucKeKhai();


// Khởi tạo countdown sau khi nhận được ngày kết thúc
const countdownInterval = setInterval(function () {
    if (run) {
        var countDownPanel = document.getElementById("countDownPanel");
        countDownPanel.style.display = 'block';

        capNhatCountdown();

    }
}, 1000);
