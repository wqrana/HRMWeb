$(document).ready(function () {
    $("#divLoadingEduTab").hide();

});
function EmployeeEducationTabClicked() {
    retreiveEmployeeEducationTab();

}

function createEditUserEducation(educationId) {
    debugger;
    educationId = educationId == 0 ? $("#EmployeeEducationDetailId").val() : educationId;   
    var id = educationId == null ? $('#userID').val() : educationId;
    var ActionMethod = educationId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeEducation/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divEmployeeEducationCreateEdit").html(data);
                $("#employeeEducationCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retreiveEmployeeEducationTab() {
    $("#divLoadingEduTab").show();
    getUserEducationDetail(null);
    getUserEducationList();
}
function getUserEducationList() {
    //debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeEducation/IndexByUser',
            data: { "id": id },

            success: function (data) {
                //debugger;
               // console.log(data);
                $("#divEmployeeEducationIndex").html(data);
                $("#divLoadingEduTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserEducationDetail(educationId) {
    //debugger;
    var id = educationId == null ? $('#userID').val() : educationId;
    var ActionMethod = educationId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeEducation/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                //console.log(data);
                $("#divEmployeeEducationDetail").html(data);
                              
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function saveUserEducationData() {
    //debugger;
    alertID = "#employeeEducationCreateEditAlert";
    var datafieldsObject = getCreateEditUserEducationFieldsData();
    var isValidated = validateCreateEditUserEducationFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeEducation/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeEducationCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeEducationTab();
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
function getCreateEditUserEducationFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.id = $('#EmployeeEducationId').val();
    dataObj.DegreeId = $('#DegreeId').val();
    dataObj.DateCompleted = $('#DateCompleted').val();
    dataObj.InstitutionName = $('#InstitutionName').val();
    dataObj.Title = $('#Title').val();
    dataObj.Note = $('#Note').val();
    return dataObj;
}
function validateCreateEditUserEducationFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = " Missing Required field(s)";

    if (dataObj != null) {
        if (dataObj.DegreeId.trim().length == 0) {
            message += "<br\> Degree is required";
        }
        else {
            isRequiredValidated += 1;
        }
        //isRequiredValidated += dataObj.DegreeId.trim().length > 0 ? 1 : 0;
        if (dataObj.DateCompleted.trim().length == 0) {
            message += "<br\> Date Completed is required";
        }
        else {
            isRequiredValidated += 1;
        }
        //isRequiredValidated += dataObj.DateCompleted.trim().length > 0 ? 1 : 0;
        if (dataObj.InstitutionName.trim().length == 0) {
            message += "<br\> Institution Name is required";
        }
        else {
            isRequiredValidated += 1;
        }
        //isRequiredValidated += dataObj.InstitutionName.trim().length > 0 ? 1 : 0;
       
        if (isRequiredValidated != 3) {
            isValidated = false;
            
        }
        

     }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getUserEducationDeleteData(id) {
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeEducation/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeEducationCreateEdit").html(data);
                $("#employeeEducationDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserEducationDocView(educationId) {
   // debugger;
    var id = educationId == null ? $("#EmployeeEducationDetailId").val() : educationId;
    if (id != 0) {

        $.ajax({
            type: "get",
            url: '/EmployeeEducation/UploadDocument',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeEducationDocUpload").html(data);
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
function uploadUserEducationDoc(id) {

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
    else {
        isProceed = false;
        showAlertAutoHide(alertID, "Error", "Please choose the User Education Document");
    }

    if (isProceed) {
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/EmployeeEducation/UploadDocument',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
               // debugger;
                if (data.status == "Success") {
                    $('#upload_EducationDoc_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeEducationTab();
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
function downloadUserEducationDoc(educationId) {
    //debugger;
    var id = educationId == null ? $("#EmployeeEducationDetailId").val() : educationId;
    if (id != 0) {
        $.ajax({

            url: "/EmployeeEducation/AjaxCheckDocument",
            type: "get",
            data: {
                "id": id
            },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    //$("#processing-spinner").hide();
                    window.location.href = "/EmployeeEducation/DownloadDocument/" + id;
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
function deleteUserEducation(id) {
    alertID = "#employeeEducationDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeEducation/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeEducationDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeEducationTab();
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