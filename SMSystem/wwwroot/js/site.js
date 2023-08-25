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



if (responseCode != 200 && responseCode != '') {
    $("#ExcMsg").append(message);
    $("#exception").addClass("show");
    setTimeout("$('#exception').removeClass('show');", 4000);
    setTimeout("$('#ExcMsg').removeText();", 1500);

    $.fn.removeText = function () {
        this.each(function () {

            // Get elements contents
            var $cont = $(this).contents();

            // Loop through the contents
            $cont.each(function () {
                var $this = $(this);

                // If it's a text node
                if (this.nodeType == 3) {
                    $this.remove(); // Remove it
                } else if (this.nodeType == 1) { // If its an element node
                    $this.removeText(); //Recurse
                }
            });
        });
    }
}
if (responseCode == 200) {
    Command: toastr["success"](message, "Success")

    toastr.options = {
        "closeButton": true,
        "positionClass": "toast-bottom-right",
        "onclick": null,
        "timeOut": "3000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

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
    $("#overlay").fadeIn(1)


    if (responseCode != 200 && responseCode != '') {
        $("#ExcMsg").append(message);
        $("#exception").addClass("show");
        setTimeout("$('#exception').removeClass('show');", 4000);
        setTimeout("$('#ExcMsg').removeText();", 4000);

        $.fn.removeText = function () {
            this.each(function () {

                // Get elements contents
                var $cont = $(this).contents();

                // Loop through the contents
                $cont.each(function () {
                    var $this = $(this);

                    // If it's a text node
                    if (this.nodeType == 3) {
                        $this.remove(); // Remove it 
                    } else if (this.nodeType == 1) { // If its an element node
                        $this.removeText(); //Recurse
                    }
                });
            });
        }
    }
    else {
        $.ajax({
            type: "GET",
            url: url,
            success: function (fn) {
                $("#overlay").fadeOut();

                Command: toastr["success"]("Data Exported Successfully.", "Success")

                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": false,
                    "positionClass": "toast-bottom-right",
                    "preventDuplicates": false,
                    "onclick": null,
                    "timeOut": "1500",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            }
        });
    }
};

