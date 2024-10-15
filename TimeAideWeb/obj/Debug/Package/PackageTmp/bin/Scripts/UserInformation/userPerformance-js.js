$(document).ready(function () {
    $("#divLoadingPerformanceTab").hide();

});
function EmployeePerformanceTabClicked() {
    retreiveEmployeePerformanceTab();

}

function createEditUserPerformance(performanceId) {
    //debugger;
    performanceId = performanceId == 0 ? $("#EmployeePerformanceDetailId").val() : performanceId;   
    var id = performanceId == null ? $('#userID').val() : performanceId;
    var ActionMethod = performanceId == null ? "Create" : "Edit";
    if (id != 0) {
    $.ajax({
        type: "get",
        url: '/EmployeePerformance/' + ActionMethod,
        data: { "id": id },

        success: function (data) {
            // debugger;
            //console.log(data);
            $("#divEmployeePerformanceCreateEdit").html(data);
            $("#employeePerformanceCreateEdit_modal").modal("show");


        },
        error: function () {
            // displayWarningMessage(data.ErrorMessage);
        }
    });
    }
}

function retreiveEmployeePerformanceTab() {
    $("#divLoadingPerformanceTab").show();
    getUserPerformanceDetail(null);
    getUserPerformanceList();
}
function getUserPerformanceList() {
    // debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeePerformance/IndexByUser',
            data: { "id": id },

            success: function (data) {
                // debugger;
                // console.log(data);
                $("#divEmployeePerformanceIndex").html(data);
                $("#divLoadingPerformanceTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserPerformanceDetail(performanceId) {
    //debugger;
    var id = performanceId == null ? $('#userID').val() : performanceId;
    var ActionMethod = performanceId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeePerformance/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                //console.log(data);
                $("#divEmployeePerformanceDetail").html(data);

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function saveUserPerformanceData() {
    debugger;
    alertID = "#employeePerformanceCreateEditAlert";
    var datafieldsObject = getCreateEditUserPerformanceFieldsData();
    var isValidated = validateCreateEditUserPerformanceFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeePerformance/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeePerformanceCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeePerformanceTab();
                    // location.reload(true);
                }
                else {
                    showAlertAutoHide(alertID, data.status, data.message);
                }
            }
            ,
            error: function (request, status, error) {
                displayErrorMessage('Error in deleting parent alert data');
                return false;
            }
        });
    }
}
function getCreateEditUserPerformanceFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.id = $('#EmployeePerformanceId').val();
    dataObj.ReviewDate = $('#ReviewDate').val();
    dataObj.SupervisorId = $('#SupervisorId').val();
    dataObj.PerformanceDescriptionId = $('#PerformanceDescriptionId').val();
    dataObj.PerformanceResultId = $('#PerformanceResultId').val();
    dataObj.ActionTakenId = $('#ActionTakenId').val();
    dataObj.ExpiryDate = $('#ExpiryDate').val();
    dataObj.ReviewSummary = $('#ReviewSummary').val();
    dataObj.ReviewNote = $('#ReviewNote').val();
    return dataObj;
}
function validateCreateEditUserPerformanceFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.ReviewDate.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.SupervisorId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.PerformanceDescriptionId.trim().length > 0 ? 1 : 0;

        if (isRequiredValidated != 3) {
            isValidated = false;
            message = " Missing Required field(s)";
        }


    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getUserPerformanceDeleteData(id) {
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeePerformance/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeePerformanceCreateEdit").html(data);
                $("#employeePerformanceDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserPerformanceDocView(performanceId) {
    //debugger;
    var id = performanceId == null ? $("#EmployeePerformanceDetailId").val() : performanceId;
    if (id != 0) {

        $.ajax({
            type: "get",
            url: '/EmployeePerformance/UploadDocument',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeePerformanceDocUpload").html(data);
                $("#upload_PerformanceDoc_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", "Record doesn't exists!");
    }
}
function uploadUserPerformanceDoc(id) {

    debugger;
    alertID = "#userPerformanceDocAlert"
    var isProceed = true;
    var formData = new FormData();
    //var id = $('#eduDocRecordID').val();

    formData.append("performanceDocRecordID", id);

    var totalFiles = document.getElementById("uploadPerformanceDocFile").files.length;
    if (totalFiles > 0) {
        var file = document.getElementById("uploadPerformanceDocFile").files[0];
        formData.append("PerformanceDocFile", file);

    }
    else {
        isProceed = false;
        showAlertAutoHide(alertID, "Error", "Please choose the User Performance Document");
    }

    if (isProceed) {
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/EmployeePerformance/UploadDocument',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $('#upload_PerformanceDoc_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeePerformanceTab();
                }
                else {
                    showAlertAutoHide(alertID, data.status, data.message);
                }
            },
            error: function (error) {

                showAlertAutoHide(alertID, "Error", error);

            }
        });
    }
}
function downloadUserPerformanceDoc(performanceId) {
    debugger;
    var id = performanceId == null ? $("#EmployeePerformanceDetailId").val() : performanceId;
    if (id != 0) {
        $.ajax({

            url: "/EmployeePerformance/AjaxCheckDocument",
            type: "get",
            data: {
                "id": id
            },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    //$("#processing-spinner").hide();
                    window.location.href = "/EmployeePerformance/DownloadDocument/" + id;
                }
                else {
                    showAlertAutoHide('#userDetailAlert', data.status, data.message);
                }
            }
            ,
            error: function (data) {
                showAlertAutoHide("#userDetailAlert", "Error", data);

            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", "Record doesn't exists!");
    }
}
function deleteUserPerformance(id) {
    alertID = "#employeePerformanceDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeePerformance/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeePerformanceDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeePerformanceTab();
                    // location.reload(true);
                }
                else {
                    showAlertAutoHide(alertID, data.status, data.message);
                }
            }
            ,
            error: function (request, status, error) {
                showAlertAutoHide(alertID, "Error", error);
                return false;
            }
        });
    }
}
function autoCompleteAjaxSettingPerformance(request, response, scr, field) {
    $.ajax({
        url: scr,
        dataType: "json",
        data: {
            term: request.term,
            fieldName: field
        },
        success: function (data) {
            response(data);
        }
    });
}