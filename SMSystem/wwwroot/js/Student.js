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
                        swal("Deleted!", "Your file has been deleted.", "success");
                        $("#studentData").html(response);
                    }
                });
            }
        });
});
