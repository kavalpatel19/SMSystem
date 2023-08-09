//Paggination//
$(document).on('click', '.paggingBtn', function () {
    let obj = {
        SId: $("#teach_Id").val(),
        Name: $("#teach_Name").val(),
        Phone: $("#teach_Phone").val(),
        PageIndex: $(this).val()
    };
    let url = "/Teacher/GetAll";
    let element = "#teacherData";
    GetData(obj, url, element);
});

//Searching//
function searchingTeach() {
    let obj = {
        SId: $("#teach_Id").val(),
        Name: $("#teach_Name").val(),
        Phone: $("#teach_Phone").val(),
        PageIndex: 1
    };

    $.ajax({
        type: "GET",
        url: "/Teacher/GetAll",
        data: obj,
        success: function (response) {
            $("#teacherData").html(response);
        }
    });
};

//Delete Data//
$(document).on('click', '#dlt_teacher', function (e) {
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
                    url: "/Teacher/Delete/",
                    data: { id: dlt_id },
                    success: function (response) {
                        swal("Deleted!", "Your file has been deleted.", "success");
                        $("#teacherData").html(response);
                    }
                });
            }
        });
});

// Export toast notification //
$("#Export-Teach").on('click', function () {
    var url = "/Teacher/ExportExcel";
    ExportData(url);
});