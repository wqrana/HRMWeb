$(document).ready(function () {
    $("#divLoadingEmploymentTab").hide();

});
function ApplicantEmploymentTabClicked() {
    $("#applicantError_modal").parent().html("");
    retreiveApplicantEmploymentTab();

}

function createEditApplicantEmployment(employmentId) {
    debugger;
    employmentId = employmentId == 0 ? $("#ApplicantEmploymentDetailId").val() : employmentId;
    var id = employmentId == null ? $('#ApplicantInformationId').val() : employmentId;
    var ActionMethod = employmentId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/ApplicantEmployment/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                // console.log(data);
                $("#divApplicantEmploymentCreateEdit").html(data);
                $("#applicantEmploymentCreateEdit_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retreiveApplicantEmploymentTab() {
    $("#divLoadingEmploymentTab").show();
    getApplicantEmploymentDetail(null);
    getApplicantEmploymentList();
}
function getApplicantEmploymentList() {
    //debugger;
    var id = $('#ApplicantInformationId').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantEmployment/Index',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divApplicantEmploymentIndex").html(data);
                $("#divLoadingEmploymentTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getApplicantEmploymentDetail(employmentId) {
    //debugger;
    var id = employmentId == null ? $('#ApplicantInformationId').val() : employmentId;
    var ActionMethod = employmentId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantEmployment/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                //console.log(data);
                $("#divApplicantEmploymentDetail").html(data);

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function saveApplicantEmploymentData() {
    //debugger;
    alertID = "#";
    var datafieldsObject = getCreateEditApplicantEmploymentFieldsData();
    var isValidated = validateCreateEditApplicantEmploymentFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/ApplicantEmployment/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantEmploymentCreateEdit_modal").modal("hide");
                    showAlertAutoHide("", data.status, data.message);
                    retreiveApplicantEmploymentTab();
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
function getCreateEditApplicantEmploymentFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.ApplicantInformationId = $('#ApplicantInformationId').val();
    dataObj.id = $('#ApplicantEmploymentId').val();
    dataObj.ApplicantCompanyId = $('#ApplicantCompanyId').val();
    dataObj.CompanyTelephone = $('#CompanyTelephone').val();
    dataObj.CompanyAddress = $('#CompanyAddress').val();
    dataObj.ApplicantPositionId = $('#ApplicantPositionId').val();
    dataObj.EmploymentStartDate = $('#EmploymentStartDate').val();
    dataObj.IsCurrentEmployment = $('#IsCurrentEmployment').is(":checked");
    dataObj.EmploymentEndDate = $('#EmploymentEndDate').val();
    dataObj.Rate = $('#Rate').val();
    dataObj.RateFrequencyId = $('#RateFrequencyId').val();
    dataObj.SuperviorName = $('#SuperviorName').val();
    dataObj.ApplicantExitTypeId = $('#ApplicantExitTypeId').val();
    dataObj.ExitReason = $('#ExitReason').val();

    return dataObj;
}
function validateCreateEditApplicantEmploymentFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.ApplicantCompanyId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.ApplicantPositionId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.ApplicantExitTypeId.trim().length > 0 ? 1 : 0;

        if (isRequiredValidated != 3) {
            isValidated = false;
            message = " Missing Required field(s)";
        }


    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getApplicantEmploymentDeleteData(id) {
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantEmployment/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divApplicantEmploymentCreateEdit").html(data);
                $("#applicantEmploymentDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}


function deleteApplicantEmployment(id) {
    alertID = "#"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/ApplicantEmployment/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantEmploymentDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveApplicantEmploymentTab();
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
