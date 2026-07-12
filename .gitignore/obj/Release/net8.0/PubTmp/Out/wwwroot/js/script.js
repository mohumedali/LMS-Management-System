
const popup = document.getElementById("videoPopup");
const close = document.getElementById("closeVideo");
const frame = document.getElementById("videoFrame");

const watchButtons = document.querySelectorAll(".watch-btn");

watchButtons.forEach(button => {

    button.addEventListener("click", function (e) {

        e.preventDefault();

        frame.src = this.dataset.video;

        popup.style.display = "flex";

    });

});

close.addEventListener("click", function () {

    popup.style.display = "none";
    frame.src = "";

});

window.addEventListener("click", function (e) {

    if (e.target === popup) {

        popup.style.display = "none";
        frame.src = "";

    }

});
// arrow


const slider = document.querySelector(".slider");
const slides = document.querySelectorAll(".slide");

const next = document.querySelector(".right");
const prev = document.querySelector(".left");

const dots = document.querySelectorAll(".dots span");

let current = 0;

function updateSlider() {

    slider.style.transform = `translateX(-${current * 50}%)`;

    dots.forEach(dot => dot.classList.remove("active"));
    dots[current].classList.add("active");

}

next.addEventListener("click", () => {

    current++;

    if (current >= slides.length) {
        current = 0;
    }

    updateSlider();

});

prev.addEventListener("click", () => {

    current--;

    if (current < 0) {
        current = slides.length - 1;
    }

    updateSlider();

});


dots.forEach((dot, index) => {

    dot.addEventListener("click", () => {

        current = index;
        updateSlider();

    });

});



// courses
console.log("before tabs");

const tabs = document.querySelectorAll(".tab");
const contents = document.querySelectorAll(".tab-content");

tabs.forEach(tab => {
    tab.addEventListener("click", function (e) {

        console.log("clicked");

        e.preventDefault();

        tabs.forEach(item => item.classList.remove("active"));
        contents.forEach(content => content.classList.remove("active"));

        this.classList.add("active");

        const target = this.dataset.tab;

        document.getElementById(target).classList.add("active");
    });
});

const swiper = new Swiper(".mySwiper", {

    slidesPerView: 3,
    spaceBetween: 10,
    loop: true,

    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },

    breakpoints: {

        0: {
            slidesPerView: 1,
        },

        768: {
            slidesPerView: 2,
        },

        1200: {
            slidesPerView: 3,
        }

    }

});

document.querySelector(".mySwiper").style.overflow = "visible";
document.querySelector(".mySwiper .swiper-wrapper").style.overflow = "visible";



const counters = document.querySelectorAll(".counter-number");

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {

        if (entry.isIntersecting) {

            const counter = entry.target;
            const target = parseInt(counter.dataset.target);
            let current = 0;

            const increment = Math.ceil(target / 100);

            function updateCounter() {

                current += increment;

                if (current >= target) {
                    counter.textContent = target.toLocaleString();
                } else {
                    counter.textContent = current.toLocaleString();
                    requestAnimationFrame(updateCounter);
                }

            }

            updateCounter();

            observer.unobserve(counter);
        }

    });
}, {
    threshold: 0.5
});

counters.forEach(counter => observer.observe(counter));