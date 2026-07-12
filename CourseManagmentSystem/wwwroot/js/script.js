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
    if (dots[current]) {
        dots[current].classList.add("active");
    }

}

if (next) {
    next.addEventListener("click", () => {

        current++;

        if (current >= slides.length) {
            current = 0;
        }

        updateSlider();

    });
}

if (prev) {
    prev.addEventListener("click", () => {

        current--;

        if (current < 0) {
            current = slides.length - 1;
        }

        updateSlider();

    });
}


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


// "Most Selling Courses" carousel
if (document.querySelector(".sellingSwiper")) {
    const sellingSwiper = new Swiper(".sellingSwiper", {

        slidesPerView: 3,
        spaceBetween: 16,
        loop: true,

        pagination: {
            el: ".sellingSwiper .swiper-pagination",
            clickable: true,
        },

        breakpoints: {
            0: {
                slidesPerView: 1.15,
                spaceBetween: 12,
            },
            576: {
                slidesPerView: 2,
                spaceBetween: 14,
            },
            992: {
                slidesPerView: 3,
                spaceBetween: 16,
            },
        },

    });
}

// "Testimonials" carousel
if (document.querySelector(".testimonialSwiper")) {
    const testimonialSwiper = new Swiper(".testimonialSwiper", {

        slidesPerView: 3,
        spaceBetween: 10,
        loop: true,

        navigation: {
            nextEl: ".testimonialSwiper ~ .swiper-button-next, .feedbacks .swiper-button-next",
            prevEl: ".testimonialSwiper ~ .swiper-button-prev, .feedbacks .swiper-button-prev",
        },

        pagination: {
            el: ".feedbacks .swiper-pagination",
            clickable: true,
        },

        breakpoints: {
            0: {
                slidesPerView: 1,
                spaceBetween: 10,
            },
            768: {
                slidesPerView: 2,
                spaceBetween: 10,
            },
            1200: {
                slidesPerView: 3,
                spaceBetween: 10,
            },
        },

    });
}

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
