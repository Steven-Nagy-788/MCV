const counter = document.getElementById("counter");
const timeInput = document.getElementById("timeInput");
const startButton = document.getElementById("startButton");
let isCounting = false;
let timeLeft = null;

function start() {
    if (isCounting||!timeInput.value||timeInput.value<=0) return;
    isCounting = true;
    timeLeft = timeInput.value;
    counter.textContent = timeLeft;

    const interval = setInterval(() => {
        update();
        if (timeLeft <= 0) {
            clearInterval(interval);
            isCounting = false;
        }
    }, 1000);
}

function update(){
    timeLeft--;
    counter.textContent = timeLeft;
}