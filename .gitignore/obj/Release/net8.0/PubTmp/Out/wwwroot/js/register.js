const menuBtn = document.querySelector(".menu-toggle");
const navMenu = document.querySelector(".nav-menu");

menuBtn.addEventListener("click", () => {
    navMenu.classList.toggle("active");
});



const toggles = document.querySelectorAll(".toggle-password");

toggles.forEach(toggle => {
    toggle.addEventListener("click", function () {

        const input = this.previousElementSibling;

        if (input.type === "password") {
            input.type = "text";
            this.innerHTML = '<i class="fa-regular fa-eye-slash"></i>';
        } else {
            input.type = "password";
            this.innerHTML = '<i class="fa-regular fa-eye"></i>';
        }

    });
}); ``