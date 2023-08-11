//Paggination//
$(document).on('click', '.paggingBtn', function () {
    let obj = {
        PageIndex: $(this).val()
    };
    let url = "/Fees/GetAll";
    let element = "#feesData";
    GetData(obj, url, element);
});

//Delete Data//
$(document).on('click', '#dlt_fees', function (e) {
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
                    url: "/Fees/Delete/",
                    data: { id: dlt_id },
                    success: function (response) {
                        swal("Deleted!", "Your file has been deleted.", "success");
                        $("#feesData").html(response);
                    }
                });
            }
        });
});

//Export Data//
$("#Export-Fees").on('click', function () {
    var url = "/Fees/ExportExcel";
    ExportData(url);
});