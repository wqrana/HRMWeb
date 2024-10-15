$(document).ready(function () {
    $("#divLoadingActionTab").hide();
   

});
function setUserActionTabDataTable() {
    $('#tblUserAction').dataTable({
        "searching": false,
        "paging": false
    });
}
function EmployeeActionTabClicked() {
    retreiveEmployeeActionTab();

}

function createEditUserAction(actionId) {
    debugger;
    actionId = actionId == 0 ? $("#EmployeeActionDetailId").val() : actionId;
    var id = actionId == null ? $('#userID').val() : actionId;
    var ActionMethod = actionId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeAction/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                // debugger;
                //console.log(data);
                $("#divEmployeeActionCreateEdit").html(data);
                $("#employeeActionCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function retreiveEmployeeActionTab() {
    $("#divLoadingActionTab").show();
    getUserActionDetail(null);
    getUserActionList();
}
function getUserActionList() {
    // debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeAction/IndexByUser',
            data: { "id": id },

            success: function (data) {
                // debugger;
                // console.log(data);
                $("#divEmployeeActionIndex").html(data);
                $("#divLoadingActionTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserActionDetail(actionId) {
    //debugger;
    var id = actionId == null ? $('#userID').val() : actionId;
    var ActionMethod = actionId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeAction/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                //console.log(data);
                $("#divEmployeeActionDetail").html(data);

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function saveUserActionData() {
    debugger;
    alertID = "#employeeActionCreateEditAlert";
    var datafieldsObject = getCreateEditUserActionFieldsData();
    var isValidated = validateCreateEditUserActionFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $(".loading").show();
        $.ajax({
            type: "POST",
            url: "/EmployeeAction/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                $(".loading").hide();
                if (data.status == "Success") {
                    $("#employeeActionCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeActionTab();
                    // location.reload(true);
                }
                else {
                    showAlertAutoHide(alertID, data.status, data.message);
                }
            }
            ,
            error: function (request, status, error) {
                $(".loading").hide();
                displayErrorMessage('Error in Create/Edit action record');
                return false;
            }
        });
    }
}
function getCreateEditUserActionFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.id = $('#EmployeeActionId').val();
    dataObj.ActionTypeId = $('#ActionTypeId').val();
    dataObj.ActionDate = $('#ActionDate').val();
    dataObj.ActionExpiryDate = $('#ActionExpiryDate').val();
    dataObj.ActionName = $('#ActionName').val();
    dataObj.ActionDescription = $('#ActionDescription').val();
    dataObj.ActionNotes = $('#ActionNotes').val();
    dataObj.ActionEndDate = $('#ActionEndDate').val();
    dataObj.ActionApprovedDate = $('#ActionApprovedDate').val();
    dataObj.ApprovedById = $('#ApprovedById').val();
    dataObj.ActionClosingInfo = $('#ActionClosingInfo').val();
  
    return dataObj;
}
function validateCreateEditUserActionFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.ActionTypeId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.ActionDate.trim().length > 0 ? 1 : 0;
       

        if (isRequiredValidated != 2) {
            isValidated = false;
            message = " Missing Required field(s)";
        }


    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getUserActionDeleteData(id) {
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeAction/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeActionCreateEdit").html(data);
                $("#employeeActionDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserActionDocView(actionId) {
    //debugger;
    var id = actionId == null ? $("#EmployeeActionDetailId").val() : actionId;
    if (id != 0) {

        $.ajax({
            type: "get",
            url: '/EmployeeAction/UploadDocument',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeActionDocUpload").html(data);
                $("#upload_ActionDoc_modal").modal("show");

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
function uploadUserActionDoc(id, droppedFiles) {

    debugger;
    alertID = "#userActionDocAlert"
    var isProceed = true;
    var formData = new FormData();
    //var id = $('#eduDocRecordID').val();

    formData.append("actionDocRecordID", id);

    var totalFiles = document.getElementById("uploadActionDocFile").files.length;
    if (totalFiles > 0) {
        var file = document.getElementById("uploadActionDocFile").files[0];
        formData.append("actionDocFile", file);

    }
    else if (droppedFiles) {
        Array.prototype.forEach.call(droppedFiles, function (eachFile) {
            formData.append("actionDocFile", eachFile);
        });
    }
    else {
        isProceed = false;
        showAlertAutoHide(alertID, "Error", "Please choose the User Action Document");
    }

    if (isProceed) {
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/EmployeeAction/UploadDocument',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $('#upload_ActionDoc_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeActionTab();
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
function downloadUserActionDoc(actionId) {
    debugger;
    var id = actionId == null ? $("#EmployeeActionDetailId").val() : actionId;
    if (id != 0) {
        $.ajax({

            url: "/EmployeeAction/AjaxCheckDocument",
            type: "get",
            data: {
                "id": id
            },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    //$("#processing-spinner").hide();
                    window.location.href = "/EmployeeAction/DownloadDocument/" + id;
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
function deleteUserAction(id) {
    alertID = "#employeeActionDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeAction/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeActionDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeActionTab();
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
function autoCompleteAjaxSettingAction(request, response, scr, field) {
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