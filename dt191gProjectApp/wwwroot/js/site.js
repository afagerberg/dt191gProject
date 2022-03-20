// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

let navlist = document.getElementById("mobnavlist");

function toggleMobile() {
    if (navlist.style.height == "400px") {
        navlist.style.height = "0";

    } else {
        navlist.style.height = "400px";
    }
}




