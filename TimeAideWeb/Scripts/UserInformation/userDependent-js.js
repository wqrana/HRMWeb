$(document).ready(function () {
    $("#divLoadingDependentTab").hide();

});
function EmployeeDependentTabClicked() {
    retreiveEmployeeDependentTab();

}

function createEditUserDependent(dependentId) {
    debugger;
    dependentId = dependentId == 0 ? $("#EmployeeDependentDetailId").val() : dependentId;
    var id = dependentId == null ? $('#userID').val() : dependentId;
    var ActionMethod = dependentId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeDependent/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divEmployeeDependentCreateEdit").html(data);
                $("#employeeDependentCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retreiveEmployeeDependentTab() {
    $("#divLoadingDependentTab").show();
    getUserDependentDetail(null);
    getUserDependentList();
}
function getUserDependentList() {
    debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeDependent/IndexByUser',
            data: { "id": id },

            success: function (data) {
                debugger;
                // console.log(data);
                $("#divEmployeeDependentIndex").html(data);
                $("#divLoadingDependentTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserDependentDetail(dependentId) {
    //debugger;
    var id = dependentId == null ? $('#userID').val() : dependentId;
    var ActionMethod = dependentId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeDependent/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeDependentDetail").html(data);

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
                alert("Error");
            }
        });
    }
}
function saveUserDependentData() {
    debugger;
    $("#btnSaveEmployeeDependent").attr("disabled", "disabled");
    alertID = "#employeeDependentCreateEditAlert";
    var datafieldsObject = getCreateEditUserDependentFieldsData();
    debugger
    var isValidated = validateCreateEditUserDependentFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeDependent/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                $("#btnSaveEmployeeDependent").removeAttr("disabled");
                if (data.status == "Success") {
                    $("#employeeDependentCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeDependentTab();
                    // location.reload(true);
                }
                else {
                    showAlertAutoHide(alertID, data.status, data.message);
                }
            }
            ,
            error: function (request, status, error) {

                $("#btnSaveEmployeeDependent").removeAttr("disabled");
                displayErrorMessage('Error in deleting parent alert data');
                return false;
            }
        });
    }
    else {
        $("#btnSaveEmployeeDependent").removeAttr("disabled");
    }
}
function getCreateEditUserDependentFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.id = $('#EmployeeDependentId').val();
    dataObj.FirstName = $('#FirstName').val();
    dataObj.LastName = $('#LastName').val();
    dataObj.SSN = $('#SSN').val();
    if (dataObj.SSN.length == 11) {
        debugger
        dataObj.SSN = dataObj.SSN.replace("-", "").replace("-", "");
    }
    dataObj.BirthDate = $('#BirthDate').val();
    dataObj.GenderId = $('#GenderId').val();
    dataObj.RelationshipId    = $('#RelationshipId').val();
    dataObj.DependentStatusId = $('#DependentStatusId').val();
    dataObj.IsFullTimeStudent = $('#IsFullTimeStudentAE').is(":checked");
    dataObj.SchoolAttending   = $('#SchoolAttending').val();
    dataObj.IsHealthInsurance = $('#IsHealthInsuranceAE').is(":checked");
    dataObj.IsDentalInsurance = $('#IsDentalInsuranceAE').is(":checked");
    dataObj.IsTaxPurposes     = $('#IsTaxPurposesAE').is(":checked");
    return dataObj;
}
function validateCreateEditUserDependentFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.FirstName.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.GenderId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.RelationshipId.trim().length > 0 ? 1 : 0;

        if (isRequiredValidated != 3) {
            isValidated = false;
            message = " Missing Required field(s)";
        }


    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getUserDependentDeleteData(id) {
    debugger;
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeDependent/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeDependentCreateEdit").html(data);
                $("#employeeDependentDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserDependentDocView(dependentId) {
    // debugger;
    var id = dependentId == null ? $("#EmployeeDependentDetailId").val() : dependentId;
    if (id != 0) {

        $.ajax({
            type: "get",
            url: '/EmployeeDependent/UploadDocument',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeDependentDocUpload").html(data);
                $("#upload_DependentDoc_modal").modal("show");

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
function uploadUserDependentDoc(id,expiryDate) {

    debugger;
    alertID = "#userDependentDocAlert"
    var isProceed = true;
    var formData = new FormData();
    //var id = $('#eduDocRecordID').val();

    formData.append("dependentDocRecordID", id);
    formData.append("expiryDate", expiryDate);

    var totalFiles = document.getElementById("uploadDependentDocFile").files.length;
    if (totalFiles > 0) {
        var file = document.getElementById("uploadDependentDocFile").files[0];
        formData.append("dependentDocFile", file);

    }
    else {
        isProceed = false;
        showAlertAutoHide(alertID, "Error", "Please choose the User Dependent Document");
    }

    if (isProceed) {
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/EmployeeDependent/UploadDocument',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                // debugger;
                if (data.status == "Success") {
                    $('#upload_DependentDoc_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeDependentTab();
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
function downloadUserDependentDoc(dependentId) {
    //debugger;
    var id = dependentId == null ? $("#EmployeeDependentDetailId").val() : dependentId;
    if (id != 0) {
        $.ajax({

            url: "/EmployeeDependent/AjaxCheckDocument",
            type: "get",
            data: {
                "id": id
            },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    //$("#processing-spinner").hide();
                    window.location.href = "/EmployeeDependent/DownloadDocument/" + id;
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
function deleteUserDependent(id) {
    alertID = "#employeeDependentDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeDependent/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeDependentDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeDependentTab();
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

