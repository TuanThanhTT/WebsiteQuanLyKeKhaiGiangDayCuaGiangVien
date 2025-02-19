const btn_login = document.querySelector("#login");
const btn_reset = document.querySelector("#reset");
const lg_form = document.querySelector(".login-form");
const rs_form = document.querySelector(".reset-form");

btn_login.addEventListener('click', () => {
    btn_login.style.backgroundColor = '#21264D';
    btn_login.style.color = '#ffff';

    btn_reset.style.backgroundColor = 'rgba(255,255,255,0.2)';
    btn_reset.style.color = '#000';
    lg_form.style.left = "50%";
    rs_form.style.left = "-50%";

    lg_form.style.opacity = 1;
    rs_form.style.opacity = 0;
}
)
btn_reset.addEventListener('click', () => {
    btn_reset.style.backgroundColor = '#21264D';
    btn_reset.style.color = '#ffff';

    btn_login.style.backgroundColor = 'rgba(255,255,255,0.2)';
    btn_login.style.color = '#000';
    rs_form.style.left = "50%";
    lg_form.style.left = "-50%";

    rs_form.style.opacity = 1;
    lg_form.style.opacity = 0;

}
)