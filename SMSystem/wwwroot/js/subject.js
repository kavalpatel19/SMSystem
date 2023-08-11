
//Paggination//
$(document).on('click', '.paggingBtn', function () {
    let obj = {
        SId: $("#sub_Id").val(),
        Name: $("#sub_Name").val(),
        Class: $("#sub_Class").val(),
        PageIndex: $(this).val()
    };
    let url = "/Subject/GetAll";
    let element = "#SubjectData";
    GetData(obj, url, element);
});

//Searching//
function searchingSub() {
    let obj = {
        SId: $("#sub_Id").val(),
        Name: $("#sub_Name").val(),
        Class: $("#sub_Class").val(),
        PageIndex: 1
    };

    $.ajax({
        type: "GET",
        url: "/Subject/GetAll",
        data: obj,
        success: function (response) {
            $("#SubjectData").html(response);
        }
    });
};

//Delete Data//
$(document).on('click', '#dlt_subject', function (e) {
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
                    url: "/Subject/Delete/",
                    data: { id: dlt_id },
                    success: function (response) {
                        swal("Deleted!", "Your file has been deleted.", "success");
                        $("#SubjectData").html(response);
                    }
                });
            }
        });
});

$("#Export-Sub").on('click', function () {
    var url = "/Subject/ExportExcel";
    ExportData(url);
});