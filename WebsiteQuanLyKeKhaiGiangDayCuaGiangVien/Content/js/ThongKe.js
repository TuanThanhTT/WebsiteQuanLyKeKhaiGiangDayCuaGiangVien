// Hàm vẽ biểu đồ tròn
function renderPieChart(data, labels) {
    const ctx = document.getElementById('modal-duyetKK-pieChart').getContext('2d');

    // Kiểm tra và hủy biểu đồ cũ (nếu có)
    if (window.myPieChart) {
        window.myPieChart.destroy();
    }

    // Tạo một biểu đồ mới
    window.myPieChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                label: 'Trạng Thái Kê Khai',
                data: data,
                backgroundColor: ['#28a745', '#dc3545', '#ffc107','#17a2b8'], // Màu sắc cho các phần tử trong biểu đồ
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top'
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            return tooltipItem.label + ': ' + tooltipItem.raw + ' phân công';
                        }
                    }
                }
            }
        }
    });
}

// Dữ liệu mẫu
const sampleData = [10, 5, 2, 1]; // Dữ liệu mẫu: Đã Kê Khai, Chưa Kê Khai, Chờ Duyệt
const sampleLabels = ["Đã Kê Khai", "Chưa Kê Khai", "Chờ Duyệt","Phân Công Đợt Trước"];

// Vẽ biểu đồ với dữ liệu mẫu khi trang được tải
renderPieChart(sampleData, sampleLabels);

