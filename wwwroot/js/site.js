document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.getElementById("darkModeToggle");
    const body = document.body;

    // Retrieve the user's theme preference from localStorage
    const currentTheme = localStorage.getItem("theme") || "light";
    if (currentTheme === "dark") {
        body.classList.add("dark-mode");
        toggleButton.textContent = "☀️";
    } else {
        body.classList.remove("dark-mode");
        toggleButton.textContent = "🌙";
    }

    // Add event listener to the toggle button
    toggleButton.addEventListener("click", function () {
        body.classList.toggle("dark-mode");
        const isDarkMode = body.classList.contains("dark-mode");

        // Update localStorage and button text based on the current mode
        localStorage.setItem("theme", isDarkMode ? "dark" : "light");
        toggleButton.textContent = isDarkMode ? "☀️" : "🌙";
    });
});


document.addEventListener('DOMContentLoaded', function () {
    const links = document.querySelectorAll('a');
    links.forEach(link => {
        link.addEventListener('click', function (event) {
            const href = link.getAttribute('href');
            if (href && href.startsWith('#')) return;
            event.preventDefault();
            document.body.classList.add('fade-out');
            setTimeout(() => {
                window.location.href = href;
            }, 500);
        });
    });
});


document.addEventListener('DOMContentLoaded', function () {
   
    function fadeOutEffect(event) {
        event.preventDefault();
        const targetUrl = event.currentTarget.href; 

        document.body.classList.add('fade-out');
     
        setTimeout(function () {
            window.location.href = targetUrl;
        }, 500); 
    }

    const internalLinks = document.querySelectorAll('a[href^="/"]');
    internalLinks.forEach(function (link) {
        link.addEventListener('click', fadeOutEffect);
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const preloader = document.getElementById('preloader');

    preloader.style.display = 'flex';

    window.addEventListener('load', function () {
        preloader.classList.add('fade-out');

        preloader.addEventListener('transitionend', function () {
            preloader.style.display = 'none';
        });
    });
});


