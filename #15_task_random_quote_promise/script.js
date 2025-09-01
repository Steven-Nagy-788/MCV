const quoteButton = document.getElementById('new-quote');
const quoteDisplay = document.getElementById('quote-display');
const quotes = [
    "Life is what happens while you're busy making other plans. - John Lennon",
    "The only way to do great work is to love what you do. - Steve Jobs",
    "Success is not final, failure is not fatal: it is the courage to continue that counts. - Winston Churchill",
    "Be yourself; everyone else is already taken. - Oscar Wilde",
    "Two things are infinite: the universe and human stupidity; and I'm not sure about the universe. - Albert Einstein",
    "You only live once, but if you do it right, once is enough. - Mae West",
    "Be the change that you wish to see in the world. - Mahatma Gandhi",
    "In three words I can sum up everything I've learned about life: it goes on. - Robert Frost",
    "If you tell the truth, you don't have to remember anything. - Mark Twain",
    "The only impossible journey is the one you never begin. - Tony Robbins"
];


quoteButton.addEventListener('click', () => {
    let MyPromise = new Promise((resolve) => {
        const randomIndex = Math.floor(Math.random() * quotes.length);
        resolve(quotes[randomIndex]);
    });
    MyPromise.then(quote => {
        quoteDisplay.textContent = quote;
    });
});


//generateQuote();