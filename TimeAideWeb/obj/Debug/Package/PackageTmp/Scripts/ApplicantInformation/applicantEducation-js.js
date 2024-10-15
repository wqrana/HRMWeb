$(document).ready(function () {
    $("#divLoadingEduTab").hide();

});
function ApplicantEducationTabClicked() {
    $("#applicantError_modal").parent().html("");
    retreiveApplicantEducationTab();

}

function createEditApplicantEducation(educationId) {
    debugger;
    educationId = educationId == 0 ? $("#ApplicantEducationDetailId").val() : educationId;
    var id = educationId == null ? $('#ApplicantInformationId').val() : educationId;
    var ActionMethod = educationId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/ApplicantEducation/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                // console.log(data);
                $("#divApplicantEducationCreateEdit").html(data);
                $("#applicantEducationCreateEdit_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retreiveApplicantEducationTab() {
    $("#divLoadingEduTab").show();
    getApplicantEducationDetail(null);
    getApplicantEducationList();
}
function getApplicantEducationList() {
    //debugger;
    var id = $('#ApplicantInformationId').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantEducation/Index',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divApplicantEducationIndex").html(data);
                $("#divLoadingEduTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getApplicantEducationDetail(educationId) {
    //debugger;
    var id = educationId == null ? $('#ApplicantInformationId').val() : educationId;
    var ActionMethod = educationId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantEducation/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                //console.log(data);
                $("#divApplicantEducationDetail").html(data);

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function saveApplicantEducationData() {
    //debugger;
    alertID = "#employeeEducationCreateEditAlert";
    var datafieldsObject = getCreateEditApplicantEducationFieldsData();
    var isValidated = validateCreateEditApplicantEducationFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/ApplicantEducation/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantEducationCreateEdit_modal").modal("hide");
                    showAlertAutoHide("", data.status, data.message);
                    retreiveApplicantEducationTab();
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
function getCreateEditApplicantEducationFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.ApplicantInformationId = $('#ApplicantInformationId').val();
    dataObj.id = $('#ApplicantEducationId').val();
    dataObj.DegreeId = $('#DegreeId').val();
    dataObj.DateCompleted = $('#DateCompleted').val();
    dataObj.InstitutionName = $('#InstitutionName').val();
    dataObj.Title = $('#Title').val();
    dataObj.Note = $('#Note').val();
    return dataObj;
}
function validateCreateEditApplicantEducationFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.DegreeId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.DateCompleted.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.InstitutionName.trim().length > 0 ? 1 : 0;

        if (isRequiredValidated != 3) {
            isValidated = false;
            message = " Missing Required field(s)";
        }


    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getApplicantEducationDeleteData(id) {
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantEducation/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divApplicantEducationCreateEdit").html(data);
                $("#applicantEducationDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getApplicantEducationDocView(educationId) {
     debugger;
    var id = educationId == null ? $("#ApplicantEducationDetailId").val() : educationId;
    if (id != 0) {

        $.ajax({
            type: "get",
            url: '/ApplicantEducation/UploadDocument',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEApplicantEducationDocUpload").html(data);
                $("#upload_EducationDoc_modal").modal("show");

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
function uploadApplicantEducationDoc(id, droppedFiles) {

    //debugger;
    alertID = "#userEducationDocAlert"
    var isProceed = true;
    var formData = new FormData();
    //var id = $('#eduDocRecordID').val();

    formData.append("eduDocRecordID", id);

    var totalFiles = document.getElementById("uploadEduDocFile").files.length;
    if (totalFiles > 0) {
        var file = document.getElementById("uploadEduDocFile").files[0];
        formData.append("EduDocFile", file);

    }
    else if (droppedFiles) {
        Array.prototype.forEach.call(droppedFiles, function (eachFile) {
            formData.append("EduDocFile", eachFile);
        });
    }
    else {
        isProceed = false;
        showAlertAutoHide(alertID, "Error", "Please choose the User Education Document");
    }

    if (isProceed) {
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/ApplicantEducation/UploadDocument',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                // debugger;
                if (data.status == "Success") {
                    $('#upload_EducationDoc_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveApplicantEducationTab();
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
function downloadApplicantEducationDoc(educationId) {
    //debugger;
    var id = educationId == null ? $("#ApplicantEducationDetailId").val() : educationId;
    if (id != 0) {
        $.ajax({

            url: "/ApplicantEducation/AjaxCheckDocument",
            type: "get",
            data: {
                "id": id
            },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    //$("#processing-spinner").hide();
                    window.location.href = "/ApplicantEducation/DownloadDocument/" + id;
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
function deleteApplicantEducation(id) {
    alertID = "#employeeEducationDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/ApplicantEducation/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantEducationDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveApplicantEducationTab();
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
function autoCompleteAjaxSetting(request, response, scr, field) {
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