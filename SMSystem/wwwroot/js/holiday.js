//Paggination//
$(document).on('click', '.paggingBtn', function () {
    let obj = {
        PageIndex: $(this).val()
    };
    let url = "/Holiday/GetAll";
    let element = "#holidayData";
    GetData(obj, url, element);
});

//Export Data//
$("#Export-Holi").on('click', function () {
    var url = "/Holiday/ExportExcel";
    ExportData(url);
});