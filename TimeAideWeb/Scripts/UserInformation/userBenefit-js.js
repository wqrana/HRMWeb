$(document).ready(function () {
    $("#divLoadingBenefitTab").hide();

});
function EmployeeBenefitTabClicked() {
    retreiveEmployeeBenefitTab();

}

function createEditUserBenefit(userBenefitId) {
    debugger;
    userBenefitId = userBenefitId == 0 ? $("#EmployeeBenefitDetailId").val() : userBenefitId;
    var id = userBenefitId == null ? $('#userID').val() : userBenefitId;
    var ActionMethod = userBenefitId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeBenefitHistory/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divEmployeeBenefitCreateEdit").html(data);
                $("#employeeBenefitCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retreiveEmployeeBenefitTab() {
    $("#divLoadingBenefitTab").show();
    getUserBenefitDetail(null);
    getUserBenefitList();
  //  getUserEnlistedBenefitList();
}
function getUserEnlistedBenefitList() {
    debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeBenefitEnlisted/IndexByUser',
            data: { "id": id },

            success: function (data) {
                debugger;
                // console.log(data);
                $("#divEnlistedBenefitDetail").html(data);
                $("#divLoadingBenefitTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function getUserBenefitList() {
    debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeBenefitHistory/IndexByUser',
            data: { "id": id },

            success: function (data) {
                debugger;
                // console.log(data);
                $("#divBenefitHistoryDetail").html(data);
                $("#divLoadingBenefitTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserBenefitDetail(userBenefitId) {
    debugger;
    var id = userBenefitId == null ? $('#userID').val() : userBenefitId;
    var ActionMethod = userBenefitId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeBenefitHistory/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeBenefitDetail").html(data);

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
                alert("Error");
            }
        });
    }
}
function saveUserBenefitData() {
    debugger;
    alertID = "#employeeBenefitCreateEditAlert";
    var datafieldsObject = getCreateEditUserBenefitFieldsData();
    var isValidated = validateCreateEditUserBenefitFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeBenefitHistory/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeBenefitCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeBenefitTab();
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
function getCreateEditUserBenefitFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.id = $('#EmployeeBenefitHistoryId').val();
    dataObj.BenefitId = $('#BenefitId').val();
    dataObj.StartDate = $('#BenefitStartDate').val();
    dataObj.ExpiryDate = $('#BenefitExpiryDate').val();
    dataObj.Amount = $('#BenefitAmount').val();
    dataObj.PayFrequencyId = $('#BenefitPayFrequencyId').val();
    dataObj.Notes = $('#BenefitNotes').val();
    dataObj.EmployeeContribution = $('#BenefitEmployeeContribution').val();
    dataObj.CompanyContribution = $('#BenefitCompanyContribution').val();
    
    dataObj.OtherContribution = $('#BenefitOtherContribution').val();
    dataObj.TotalContribution = $('#BenefitTotalContribution').val();
    
    return dataObj;
}
function validateCreateEditUserBenefitFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.BenefitId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.StartDate.trim().length > 0 ? 1 : 0;
        //isRequiredValidated += dataObj.ExpiryDate.trim().length > 0 ? 1 : 0;
        
        if (isRequiredValidated != 2) {
            isValidated = false;
            message = " Missing Required field(s)";
        }


    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getUserBenefitDeleteData(id) {
    debugger;
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeBenefitHistory/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeBenefitCreateEdit").html(data);
                $("#employeeBenefitDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function deleteUserBenefit(id) {
    alertID = "#employeeBenefitDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeBenefitHistory/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeBenefitDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeBenefitTab();
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

function createEditUserBenefitEnlisted() {
    debugger;
    var id = $('#userID').val();  
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeBenefitEnlisted/Edit',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divEmployeeBenefitCreateEdit").html(data);
                $("#employeeBenefitEnlistedCreateEdit_modal").modal("show");
                
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}
function saveUserBenefitEnlistedData(enlistedStr) {
    debugger;
    var id = $('#userID').val();
    //enlisteddataObject = new Object();
    //enlisteddataObject.id = id;
    //enlisteddataObject.enlistedBenefitids = enlistedStr;
    // ajax call for saving data
    $.ajax({
        type: "POST",
        url: "/EmployeeBenefitEnlisted/CreateEdit",
        data: { "id": id, "enlistedBenefitids": enlistedStr},       
        dataType: "json",
        success: function (data) {
            debugger;
            if (data.status == "Success") {
                $("#employeeBenefitEnlistedCreateEdit_modal").modal("hide");
                showAlertAutoHide("#userDetailAlert", data.status, data.message);
                getUserEnlistedBenefitList();
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
