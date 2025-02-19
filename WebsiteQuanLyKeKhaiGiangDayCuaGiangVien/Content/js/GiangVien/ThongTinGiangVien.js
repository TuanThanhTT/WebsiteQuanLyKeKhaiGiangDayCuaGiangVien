
document.addEventListener("DOMContentLoaded", function () {
    var chonFile = document.getElementById("imgFile"); 
    var btnLuuAvartar = document.getElementById("btnLuuAvartar");


    btnLuuAvartar.disabled = true;

    chonFile.addEventListener("change", function () {

        if (chonFile.files && chonFile.files.length > 0) {
            btnLuuAvartar.disabled = false;
        } else {
            btnTaiFile.disabled = true;
        }
    });


    btnLuuAvartar.addEventListener("click", function () {
        CapNhatAnh(chonFile.files[0]);
    });
});


function CapNhatAnh(file) {
   
    var formData = new FormData();
    formData.append("file", file);
    $.ajax({
        url: '/Home/UpLoadAvartar',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                var data = response.data;
                //cap nhat hien thi lai anh dai dien
                var imgAvartar = document.getElementById("imgAvartar");

                imgAvartar.src = "/ImgAvartar/" + data;
                var inputImg = document.getElementById("imgFile");
                inputImg.value = "";
                var btnLuuAvartar = document.getElementById("btnLuuAvartar");
                btnLuuAvartar.disabled = true;

                

                alert(response.message);
            } else {
                alert(response.message);
            }

           
        },
        error: function (xhr, status, error) {
            alert("Có lỗi xảy ra: " + error);
        }
    });
}

