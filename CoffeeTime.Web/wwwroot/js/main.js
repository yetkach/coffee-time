window.onload = function () {
    const range = document.querySelector('.range-style');
    const number = document.querySelector('#number');

    range.addEventListener('input', function () {
        if (range.value == 0) {
            number.textContent = "No sugar";
        } else {
            number.textContent = range.value + " tsp";
        }
    });

    const priceElem = document.querySelector('#price');

    const volumeOptions = document.querySelector('#volume-options');
    volumeOptions.addEventListener('click', function (event) {
        const input = event.target.closest('input#Volume');

        if (input !== null) {
            const price = coffees.find((coffee) => coffee.volume === input.value).price;
            priceElem.textContent = '$' + price;
        }
    });

    function updateDefaultPrice() {
        priceElem.textContent = '$' + coffees[0].price;
    }

    updateDefaultPrice();
}

