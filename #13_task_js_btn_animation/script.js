const btn = document.getElementById('animateBtn');

btn.addEventListener('click', () => {
    box.classList.add('animate');
    setTimeout(() => {
        box.classList.remove('animate');
    }, 1000);
});
