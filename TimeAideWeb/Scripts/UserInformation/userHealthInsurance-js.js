$(document).ready(function () {
    $("#divLoadingHealthInsuranceTab").hide();

});
function EmployeeHealthInsuranceTabClicked() {
    retreiveEmployeeHealthInsuranceTab();

}

function createEditUserHealthInsurance(healthInsuranceId) {
    debugger;
    healthInsuranceId = $("#EmployeeHealthInsuranceDetailId").val();
    var id = healthInsuranceId == 0 ? $('#userID').val() : healthInsuranceId;
    var ActionMethod = healthInsuranceId == 0 ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeHealthInsurance/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divEmployeeHealthInsuranceCreateEdit").html(data);
                $("#employeeHealthInsuranceCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retreiveEmployeeHealthInsuranceTab() {
    $("#divLoadingHealthInsuranceTab").show();
    debugger;
    getUserHealthInsuranceDetail(null);
    getUserDependentForInsuranceList();
    getUserCobraPaymentList();
}
function getUserCobraPaymentList() {
    //debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/HealthInsuranceCobraHistory/IndexByUserInsurance',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divCobraPaymentDetail").html(data);
                $("#divLoadingHealthInsuranceTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserDependentForInsuranceList() {
    debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeDependent/IndexByUserInsurance',
            data: {
                "id": id,
                "insuranceType": 'H'
                },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divDependentDetail").html(data);
               // $("#divLoadingHealthInsuranceTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserHealthInsuranceDetail(healthInsuranceId) {
    //debugger;
    var id = healthInsuranceId == null ? $('#userID').val() : healthInsuranceId;
    var ActionMethod = healthInsuranceId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeHealthInsurance/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeHealthInsuranceDetail").html(data);

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
                alert("Error");
            }
        });
    }
}
function saveUserHealthInsuranceData() {
    debugger;
    alertID = "#employeeHealthInsuranceCreateEditAlert";
    var datafieldsObject = getCreateEditUserHealthInsuranceFieldsData();
    var isValidated = validateCreateEditUserHealthInsuranceFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeHealthInsurance/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeHealthInsuranceCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    getUserHealthInsuranceDetail(null);
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
function getCreateEditUserHealthInsuranceFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.id = $('#EmployeeHealthInsuranceId').val();
    dataObj.InsuranceStatusId = $('#InsuranceStatusId').val();
    dataObj.InsuranceStartDate = $('#InsuranceStartDate').val();
    dataObj.InsuranceExpiryDate = $('#InsuranceExpiryDate').val();
    dataObj.GroupId = $('#GroupId').val();
    dataObj.InsuranceTypeId = $('#InsuranceTypeId').val();
    dataObj.InsuranceCoverageId = $('#InsuranceCoverageId').val();
    dataObj.EmployeeContribution = $('#EmployeeContribution').val();
    dataObj.CompanyContribution = $('#CompanyContribution').val();
    dataObj.OtherContribution = $('#OtherContribution').val();
    dataObj.TotalContribution = $('#TotalContribution').val();
    dataObj.PCORIFee = $('#PCORIFee').val();
    dataObj.CobraStatusId = $('#CobraStatusId').val();
    dataObj.LeyCobraStartDate = $('#LeyCobraStartDate').val();
    dataObj.LeyCobraExpiryDate = $('#LeyCobraExpiryDate').val();
    dataObj.InsurancePremium = $('#InsurancePremium').val();
   
    return dataObj;
}
function validateCreateEditUserHealthInsuranceFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.InsuranceStatusId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.InsuranceStartDate.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.GroupId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.InsuranceTypeId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.InsuranceCoverageId.trim().length > 0 ? 1 : 0;
        if (isRequiredValidated != 5) {
            isValidated = false;
            message = " Missing Required field(s)";
        }

    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getUserInsuranceDependentDetail(dependentId) {
    debugger;
    $.ajax({
        type: "get",
        url: '/EmployeeDependent/Details',
        data: { "id": dependentId },

        success: function (data) {
            debugger;
            //console.log(data);
            $("#employeeDependentDetail_modalBodyContent").html(data);
            $("#employeeDependentDetail_modal").modal("show");

        },
        error: function () {
            // displayWarningMessage(data.ErrorMessage);
            alert("Error");
        }
    });
}
//Cobra Payment History handling

function createEditUserHInsCobraHistory(healthInsuranceCobraHistoryId) {
    debugger;
    
    var id = healthInsuranceCobraHistoryId == 0 ? $('#userID').val() : healthInsuranceCobraHistoryId;
    var ActionMethod = healthInsuranceCobraHistoryId == 0 ? "Create" : "Edit";
    var isRequiredCount = 0;
    //Check for Ley Cobra information
    if (ActionMethod == "Create") {
        

        //isRequiredCount += $('#CobraStatusDetailId').val().trim() == "1" ? 1 : 0;
        isRequiredCount += $('#CobraStatusDetailName').val().trim() == "Active" ? 1 : 0;
        isRequiredCount += $('#LeyCobraDetailStartDate').val().trim().length > 0 ? 1 : 0;
        if (isRequiredCount != 2) {
            showAlertAutoHide("#userDetailAlert", "Error", "Missing Required Ley Cobra Information for Payment Tracker");
            return;    
        }
    }
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/HealthInsuranceCobraHistory/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divEmployeeHealthInsuranceCreateEdit").html(data);
                $("#employeeHInsCobraHistoryCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}
function saveUserHInsCobraHistoryData() {
    debugger;
    alertID = "#employeeHInsCobraHistoryCreateEditAlert";
    var dataObj = new Object();
    var isValidated = false;
    var isRequiredValidated = 0;
    var message = "";
    dataObj.id = $('#HealthInsuranceCobraHistoryId').val();
    dataObj.EmployeeHealthInsuranceId = $('#EmployeeHealthInsuranceDetailId').val();
    dataObj.DueDate = $('#DueDate').val();
    dataObj.PaymentDate = $('#PaymentDate').val();
    dataObj.PaymentAmount = $('#PaymentAmount').val();
    dataObj.CobraPaymentStatusId = $('#CobraPaymentStatusId').val();
    //Validation 
    if (dataObj != null) {
        isValidated = true;
        isRequiredValidated += dataObj.DueDate.trim().length > 0 ? 1 : 0;
         if (isRequiredValidated != 1) {
            isValidated = false;
            message = " Missing Required field(s)";
        }

    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/HealthInsuranceCobraHistory/CreateEdit",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeHInsCobraHistoryCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    getUserCobraPaymentList();
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
function getUserHInsCobraHistoryDeleteData(id) {
    debugger;
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/HealthInsuranceCobraHistory/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeHealthInsuranceCreateEdit").html(data);
                $("#employeeHInsCobraHistoryDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function deleteUserHInsCobraHistory(id) {
    alertID = "#employeeHInsCobraHistoryDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/HealthInsuranceCobraHistory/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeHInsCobraHistoryDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    getUserCobraPaymentList();
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



