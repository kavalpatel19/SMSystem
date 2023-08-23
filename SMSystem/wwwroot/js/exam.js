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
                            $(".sa-button-container").find("button.cancel").trigger("click")
                            $("#exception").append(response.message);
                            $("#exception").addClass("show");
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