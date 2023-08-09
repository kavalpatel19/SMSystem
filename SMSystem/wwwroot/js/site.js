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

function ExportData(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function () {
            Command: toastr["success"]("Data Exported Successfully.", "Success")

            toastr.options = {
                "closeButton": true,
                "positionClass": "toast-top-right",
                "onclick": null,
                "timeOut": "1500",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        },
        Error: function () {
            Command: toastr["error"]("Something's Wrong", "Error")

            toastr.options = {
                "closeButton": true,
                "positionClass": "toast-top-right",
                "onclick": null,
                "timeOut": "1500",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
    });
};
