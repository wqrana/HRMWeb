$(function () {
    $("#closbtn").click(function () {
        $('#myModal').modal('hide');
    });
});


function fnAddEditUserContactInformation(id, method) {
    debugger;
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: '/UserContactInformation/Create' + method,
        contentType: "application/json; charset=utf-8",
        data: { "Id": id },
        datatype: "json",
        success: function (data) {
            debugger;
            $('#myModaldialog').removeClass("modal-dialog modal-md").addClass("modal-dialog modal-lg");
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function refreshContactInformation() {
    $.ajax({
        type: "GET",
        url: '/UserContactInformation/Details',
        contentType: "application/json; charset=utf-8",
        data: { "Id": $('#UserInformationId').val() },
        datatype: "html",
        success: function (html) {
            $('#divUserContactInformation').html(html);
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}