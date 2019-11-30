// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function attachMinusAndPlusEvents() {
    document.querySelectorAll('#minus').forEach(item => {
        item.addEventListener('click', event => {
            let currentValue = item.parentNode.getElementsByClassName('menu-order-number')[0].innerText;
            let result = Number(currentValue) - 1;

            if (result >= 0) {
                item.parentNode.getElementsByClassName('menu-order-number')[0].innerText = result;
            }
        })
    })
    document.querySelectorAll('#plus').forEach(item => {
        item.addEventListener('click', event => {
            let currentValue = item.parentNode.getElementsByClassName('menu-order-number')[0].innerText;
            let result = Number(currentValue) + 1;

            item.parentNode.getElementsByClassName('menu-order-number')[0].innerText = result;
        })
    })

} 

attachMinusAndPlusEvents();