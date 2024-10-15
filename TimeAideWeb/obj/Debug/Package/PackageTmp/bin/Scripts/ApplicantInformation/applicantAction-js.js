$(document).ready(function () {
    $("#divLoadingActionTab").hide();


});



function setApplicantActionTabDataTable() {
    $('#tblApplicantAction').dataTable({
        "searching": false,
        "paging": false
    });
}
function ApplicantActionTabClicked() {
    retreiveApplicantActionTab();

}

function createEditApplicantAction(actionId) {
    debugger;
    actionId = actionId == 0 ? $("#ApplicantActionDetailId").val() : actionId;
    var id = actionId == null ? $('#ApplicantInformationId').val() : actionId;
    var ActionMethod = actionId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/ApplicantAction/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                 debugger;
                //console.log(data);
                $("#divApplicantActionCreateEdit").html(data);
                $("#applicantActionCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function retreiveApplicantActionTab() {
    $("#divLoadingActionTab").show();
    getApplicantActionDetail(null);
    getApplicantActionList();
}
function getApplicantActionList() {
    // debugger;
    var id = $('#ApplicantInformationId').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantAction/Index',
            data: { "id": id },

            success: function (data) {
                // debugger;
                // console.log(data);
                $("#divApplicantActionIndex").html(data);
                $("#divLoadingActionTab").hide();
                
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getApplicantActionDetail(actionId) {
    //debugger;
    var id = actionId == null ? $('#ApplicantInformationId').val() : actionId;
    var ActionMethod = actionId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantAction/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                //console.log(data);
                $("#divApplicantActionDetail").html(data);

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function saveApplicantActionData() {
    debugger;
    alertID = "#";
    var datafieldsObject = getCreateEditApplicantActionFieldsData();
    var isValidated = validateCreateEditApplicantActionFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/ApplicantAction/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantActionCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveApplicantActionTab();
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
function getCreateEditApplicantActionFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.ApplicantInformationId = $('#ApplicantInformationId').val();
    dataObj.id = $('#ApplicantActionId').val();
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
function validateCreateEditApplicantActionFields(dataObj) {
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

function getApplicantActionDeleteData(id) {
    debugger;
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantAction/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divApplicantActionCreateEdit").html(data);
                $("#applicantActionDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getApplicantActionDocView(actionId) {
    debugger;
    var id = actionId == null ? $("#ApplicantActionDetailId").val() : actionId;
    if (id != 0) {

        $.ajax({
            type: "get",
            url: '/ApplicantAction/UploadDocument',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divApplicantActionCreateEdit").html(data);
                $("#upload_ActionDoc_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
    else {
        showAlertAutoHide("#", "Error", "Record doesn't exists!");
    }
}
function uploadApplicantActionDoc(id, droppedFiles) {

    debugger;
    alertID = "#"
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
        showAlertAutoHide(alertID, "Error", "Please choose the Applicant Action Document");
    }

    if (isProceed) {
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/ApplicantAction/UploadDocument',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $('#upload_ActionDoc_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveApplicantActionTab();
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
function downloadApplicantActionDoc(actionId) {
    debugger;
    var id = actionId == null ? $("#ApplicantActionDetailId").val() : actionId;
    if (id != 0) {
        $.ajax({

            url: "/ApplicantAction/AjaxCheckDocument",
            type: "get",
            data: {
                "id": id
            },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    //$("#processing-spinner").hide();
                    window.location.href = "/ApplicantAction/DownloadDocument/" + id;
                }
                else {
                    showAlertAutoHide('#', data.status, data.message);
                }
            }
            ,
            error: function (data) {
                showAlertAutoHide("#", "Error", data);

            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", "Record doesn't exists!");
    }
}
function deleteApplicantAction(id) {
    alertID = "#"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/ApplicantAction/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantActionDelete_modal").modal("hide");
                    showAlertAutoHide("#", data.status, data.message);
                    retreiveApplicantActionTab();
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