$(document).ready(function () {
    $("#divLoadingDentalInsuranceTab").hide();

});
function EmployeeDentalInsuranceTabClicked() {
    retreiveEmployeeDentalInsuranceTab();

}

function createEditUserDentalInsurance(dentalInsuranceId) {
    debugger;
    dentalInsuranceId = $("#EmployeeDentalInsuranceDetailId").val();
    var id = dentalInsuranceId == 0 ? $('#userID').val() : dentalInsuranceId;
    var ActionMethod = dentalInsuranceId == 0 ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeDentalInsurance/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divEmployeeDentalInsuranceCreateEdit").html(data);
                $("#employeeDentalInsuranceCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retreiveEmployeeDentalInsuranceTab() {
    $("#divLoadingDentalInsuranceTab").show();
    debugger;
    getUserDentalInsuranceDetail(null);
    getUserDependentForDentalInsuranceList();
    getUserDentalCobraPaymentList();
}
function getUserDentalCobraPaymentList() {
    //debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/DentalInsuranceCobraHistory/IndexByUserInsurance',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divDentalCobraPaymentDetail").html(data);
                $("#divLoadingDentalInsuranceTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserDependentForDentalInsuranceList() {
    debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeDependent/IndexByUserInsurance',
            data: {
                "id": id,
                "insuranceType": 'D'
            },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divDentalDependentDetail").html(data);
                // $("#divLoadingHealthInsuranceTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserDentalInsuranceDetail(dentalInsuranceId) {
    //debugger;
    var id = dentalInsuranceId == null ? $('#userID').val() : dentalInsuranceId;
    var ActionMethod = dentalInsuranceId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeDentalInsurance/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeDentalInsuranceDetail").html(data);

            },
            error: function (request, status, error) {
                debugger;
                // displayWarningMessage(data.ErrorMessage);
                alert(error);
            }
        });
    }
}
function saveUserDentalInsuranceData() {
    debugger;
    alertID = "#employeeDentalInsuranceCreateEditAlert";
    var datafieldsObject = getCreateEditUserDentalInsuranceFieldsData();
    var isValidated = validateCreateEditUserDentalInsuranceFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeDentalInsurance/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeDentalInsuranceCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    getUserDentalInsuranceDetail(null);
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
function getCreateEditUserDentalInsuranceFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.id = $('#EmployeeDentalInsuranceId').val();
    dataObj.InsuranceStatusId = $('#DentalInsuranceStatusId').val();
    dataObj.InsuranceStartDate = $('#DentalInsuranceStartDate').val();
    dataObj.InsuranceExpiryDate = $('#DentalInsuranceExpiryDate').val();
    dataObj.GroupId = $('#DentalInsuranceGroupId').val();
    dataObj.InsuranceTypeId = $('#DentalInsuranceTypeId').val();
    dataObj.InsuranceCoverageId = $('#DentalInsuranceCoverageId').val();
    dataObj.EmployeeContribution = $('#DentalInsuranceEmployeeContribution').val();
    dataObj.CompanyContribution = $('#DentalInsuranceCompanyContribution').val();
    dataObj.OtherContribution = $('#DentalInsuranceOtherContribution').val();
    dataObj.TotalContribution = $('#DentalInsuranceTotalContribution').val();
    dataObj.PCORIFee = $('#DentalInsurancePCORIFee').val();
    dataObj.CobraStatusId = $('#DentalInsuranceCobraStatusId').val();
    dataObj.LeyCobraStartDate = $('#DentalInsuranceLeyCobraStartDate').val();
    dataObj.LeyCobraExpiryDate = $('#DentalInsuranceLeyCobraExpiryDate').val();
    dataObj.InsurancePremium = $('#DentalInsuranceInsurancePremium').val();

    return dataObj;
}
function validateCreateEditUserDentalInsuranceFields(dataObj) {
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

function getUserDentalInsuranceDependentDetail(dependentId) {
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

function createEditUserDInsCobraHistory(dentalInsuranceCobraHistoryId) {
    debugger;

    var id = dentalInsuranceCobraHistoryId == 0 ? $('#userID').val() : dentalInsuranceCobraHistoryId;
    var ActionMethod = dentalInsuranceCobraHistoryId == 0 ? "Create" : "Edit";
    var isRequiredCount = 0;
    //Check for Ley Cobra information
    if (ActionMethod == "Create") {
        //DentalInsuranceCobraStatusDetailName
        //isRequiredCount += $('#DentalInsuranceCobraStatusDetailId').val().trim() == "1" ? 1 : 0;
        isRequiredCount += $('#DentalInsuranceCobraStatusDetailName').val().trim() == "Active" ? 1 : 0;
        isRequiredCount += $('#DentalInsuranceLeyCobraDetailStartDate').val().trim().length > 0 ? 1 : 0;
        if (isRequiredCount != 2) {
            showAlertAutoHide("#userDetailAlert", "Error", "Missing Required Ley Cobra Information for Payment Tracker");
            return;
        }
    }
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/DentalInsuranceCobraHistory/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divEmployeeDentalInsuranceCreateEdit").html(data);
                $("#employeeDInsCobraHistoryCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}
function saveUserDInsCobraHistoryData() {
    debugger;
    alertID = "#employeeDInsCobraHistoryCreateEditAlert";
    var dataObj = new Object();
    var isValidated = false;
    var isRequiredValidated = 0;
    var message = "";
    dataObj.id = $('#DentalInsuranceCobraHistoryId').val();
    dataObj.EmployeeDentalInsuranceId = $('#EmployeeDentalInsuranceDetailId').val();
    dataObj.DueDate = $('#DentalInsuranceCobraDueDate').val();
    dataObj.PaymentDate = $('#DentalInsuranceCobraPaymentDate').val();
    dataObj.PaymentAmount = $('#DentalInsuranceCobraPaymentAmount').val();
    dataObj.CobraPaymentStatusId = $('#DentalInsuranceCobraPaymentStatusId').val();
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
            url: "/DentalInsuranceCobraHistory/CreateEdit",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeDInsCobraHistoryCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    getUserDentalCobraPaymentList();
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
function getUserDInsCobraHistoryDeleteData(id) {
    debugger;
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/DentalInsuranceCobraHistory/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeDentalInsuranceCreateEdit").html(data);
                $("#employeeDInsCobraHistoryDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function deleteUserDInsCobraHistory(id) {
    debugger;
    alertID = "#employeeDInsCobraHistoryDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/DentalInsuranceCobraHistory/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeDInsCobraHistoryDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    getUserDentalCobraPaymentList();
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



