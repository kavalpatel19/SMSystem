//Paggination//
$(document).on('click', '.paggingBtn', function () {
    let obj = {
        PageIndex: $(this).val()
    };
    let url = "/Exam/GetAll";
    let element = "#examData";
    GetData(obj, url, element);
});

//Delete Data//
$(document).on('click', '#dlt_exam', function (e) {
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
                    url: "/Exam/Delete/",
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

//Export Data//
$("#Export-Exam").on('click', function () {
    var url = "/Exam/ExportExcel";
    ExportData(url);
});