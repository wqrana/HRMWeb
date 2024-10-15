$(document).ready(function () {
    $("#divLoadingDocumentTab").hide();

});
function ApplicantDocumentTabClicked() {
    $("#applicantError_modal").parent().html("");
    retrieveApplicantDocumentTab();
    
}
function ajaxCheckExpirableDocument(id) {
    debugger;
    alertID = "#"
    if (id != '') {
        $.ajax({
            type: "get",
            url: "/ApplicantDocument/AjaxCheckExpirableDocument",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    // $("#divExpirationDate").modal("hide");
                    $("#IsExpirable").val(data.isExpirable);
                    if (data.isExpirable) {
                        $(".hideExpiryDate").addClass("showExpiryDate").removeClass("hideExpiryDate");
                    }
                    else {
                        $(".showExpiryDate").addClass("hideExpiryDate").removeClass("showExpiryDate");
                    }

                }
                // location.reload(true);

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
function createEditApplicantDocument(documentId) {
    debugger;
   
    var id = documentId == null ? $('#ApplicantInformationId').val() : documentId;
    var ActionMethod = documentId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/ApplicantDocument/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                // console.log(data);
                $("#divApplicantDocumentCreateEdit").html(data);
                $("#applicantDocumentCreateEdit_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retrieveApplicantDocumentTab() {
    $("#divLoadingDocumentTab").show();
     getApplicantDocumentList();
}
function getApplicantDocumentList() {
    //debugger;
    var id = $('#ApplicantInformationId').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantDocument/Index',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divApplicantDocumentIndex").html(data);
                $("#divLoadingDocumentTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}


function saveApplicantDocumentData() {
    //debugger;
    alertID = "#employeeEducationCreateEditAlert";
    var datafieldsObject = getCreateEditApplicantDocumentFieldsData();
    var isValidated = validateCreateEditApplicantDocumentFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/ApplicantDocument/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantDocumentCreateEdit_modal").modal("hide");
                    showAlertAutoHide("", data.status, data.message);
                    retrieveApplicantDocumentTab();
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
function getCreateEditApplicantDocumentFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.ApplicantInformationId = $('#ApplicantInformationId').val();
    dataObj.id = $('#ApplicantDocumentId').val();
    dataObj.DocumentId = $('#DocumentId').val();
    dataObj.DocumentNote = $('#DocumentNote').val();  
    dataObj.IsExpirable = $('#IsExpirable').val();
    dataObj.ExpirationDate = dataObj.IsExpirable ? $('#ExpirationDate').val() : null;
    return dataObj;
}
function validateCreateEditApplicantDocumentFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.DocumentId.trim().length > 0 ? 1 : 0;     

        if (dataObj.IsExpirable.toLowerCase() == "true") {
            isRequiredValidated += dataObj.ExpirationDate.trim().length > 0 ? 1 : 0;
        }

        if ((dataObj.IsExpirable.toLowerCase() == "false" && isRequiredValidated != 1 ||
            (dataObj.IsExpirable.toLowerCase() == "true" && isRequiredValidated != 2))) {
            isValidated = false;
            message = " Missing Required field(s)";
        }
      
    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getApplicantDocumentDeleteData(id) {
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantDocument/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divApplicantDocumentCreateEdit").html(data);
                $("#applicantDocumentDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function getApplicantDocumentView(documentId) {
    debugger;
    var id = documentId;
    if (id != 0) {

        $.ajax({
            type: "get",
            url: '/ApplicantDocument/UploadDocument',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divApplicantDocumentCreateEdit").html(data);
                $("#upload_Document_modal").modal("show");

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

function uploadApplicantDocument(id, droppedFiles) {

    //debugger;
    alertID = "#"
    var isProceed = true;
    var formData = new FormData();
    //var id = $('#eduDocRecordID').val();

    formData.append("docRecordID", id);

    var totalFiles = document.getElementById("uploadDocumentFile").files.length;
    if (totalFiles > 0) {
        var file = document.getElementById("uploadDocumentFile").files[0];
        formData.append("DocumentFile", file);

    }
    else if (droppedFiles) {
        Array.prototype.forEach.call(droppedFiles, function (eachFile) {
            formData.append("DocumentFile", eachFile);
        });
    }
    else {
        isProceed = false;
        showAlertAutoHide(alertID, "Error", "Please choose the Applicant Document");
    }

    if (isProceed) {
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/ApplicantDocument/UploadDocument',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                // debugger;
                if (data.status == "Success") {
                    $('#upload_Document_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retrieveApplicantDocumentTab();
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
function downloadApplicantDocument(documentId) {
    //debugger;
    var id = documentId;
    if (id != 0) {
        $.ajax({

            url: "/ApplicantDocument/AjaxCheckDocument",
            type: "get",
            data: {
                "id": id
            },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    //$("#processing-spinner").hide();
                    window.location.href = "/ApplicantDocument/DownloadDocument/" + id;
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
function deleteApplicantDocument(id) {
    alertID = "#employeeEducationDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/ApplicantDocument/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantDocumentDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retrieveApplicantDocumentTab();
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
