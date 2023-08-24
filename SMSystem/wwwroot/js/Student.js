//Paggination//
$(document).on('click', '.paggingBtn', function () {
    let obj = {
        SId: $("#std_Id").val(),
        Name: $("#std_Name").val(),
        Phone: $("#std_Phone").val(),
        PageIndex: $(this).val()
    };
    let url = "/Student/GetAll";
    let element = "#studentData";
    GetData(obj, url, element);
});

//Searching//
function searchingStud() {
    let obj = {
        SId: $("#std_Id").val(),
        Name: $("#std_Name").val(),
        Phone: $("#std_Phone").val(),
        PageIndex: 1
    };

    $.ajax({
        type: "GET",
        url: "/Student/GetAll",
        data: obj,
        success: function (response) {
            $("#studentData").html(response);
        }
    });
};

//Delete Data//
$(document).on('click', '#dlt_student', function (e) {
    e.preventDefault();
    var dlt_id = $(this).attr('value');

    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "Cancel",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: "POST",
                    url: "/Student/Delete/",
                    data: { id: dlt_id },
                    success: function (response) {
                        if (response.responseCode == 500) {
                            $("#ExcMsg").append(response.message);
                            $(".sa-button-container").find("button.cancel").trigger("click");
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
                            swal("Deleted!", "Your file has been deleted.", "success");
                            $("#examData").html(response);
                        }
                    }
                });
            }
        });
});
$("#Export-Stud").on('click', function () {
    var url = "/Student/ExportExcel";
    ExportData(url);
});
