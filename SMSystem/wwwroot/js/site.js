// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


'use strict';
    $(document).on('click', '.loader', function () {
        $("#overlay").fadeIn(1)
        setTimeout(function () {
            $("#overlay").fadeOut();
        }, 500);
    });

//To get all list//
function GetData(obj, url, element) {
    $("#overlay").fadeIn(1)

    $.ajax({
        type: "GET",
        url: url,
        data: obj,
        success: function (response) {
            $(element).html(response);
            $("#overlay").fadeOut();

        }
    });
};
