//Paggination//
$(document).on('click', '.paggingBtn', function () {
    let obj = {
        SId: $("#dep_Id").val(),
        Name: $("#dep_Name").val(),
        Year: $("#dep_Year").val(),
        PageIndex: $(this).val()
    };
    let url = "/Department/GetAll";
    let element = "#departmentData";
    GetData(obj, url, element);
});

//Searching//
function searchingDep() {
    let obj = {
        SId: $("#dep_Id").val(),
        Name: $("#dep_Name").val(),
        Year: $("#dep_Year").val(),
        PageIndex: 1
    };

    $.ajax({
        type: "GET",
        url: "/Department/GetAll",
        data: obj,
        success: function (response) {
            $("#departmentData").html(response);
        }
    }); };

//Delete Data//
$(document).on('click', '#dlt_departmnet', function (e) {
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
                    url: "/Department/Delete/",
                    data: { id: dlt_id },
                    success: function (response) {
                        swal("Deleted!", "Your file has been deleted.", "success");
                        $("#departmentData").html(response);
                    }
                });
            }
        });
});

$("#Export-Dep").on('click', function () {
    var url = "/Department/ExportExcel";
    ExportData(url);
});