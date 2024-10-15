$(document).ready(function () {
    $("#divLoadingCustomFieldTab").hide();

});
function ApplicantCustomFieldTabClicked() {
    debugger;
    $("#applicantError_modal").parent().html("");
    retreiveApplicantCustomFieldTab();

}
function ajaxCheckExpirableCustomField(id) {
    debugger;
    alertID = "#employeeEducationDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "get",
            url: "/ApplicantCustomField/AjaxCheckExpirableCustomField",
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
                        $("#ExpirationDate").val("");
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
function createEditApplicantCustomField(customFieldId) {
    debugger;
    
    var id = customFieldId == null ? $('#ApplicantInformationId').val() : customFieldId;
    var ActionMethod = customFieldId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/ApplicantCustomField/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                // console.log(data);
                $("#divApplicantCustomFieldCreateEdit").html(data);
                $("#applicantCustomFieldCreateEdit_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retreiveApplicantCustomFieldTab() {
    $("#divLoadingCustomFieldTab").show();
     getApplicantCustomFieldList();
}
function getApplicantCustomFieldList() {
    //debugger;
    var id = $('#ApplicantInformationId').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantCustomField/Index',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divApplicantCustomFieldIndex").html(data);
                $("#divLoadingCustomFieldTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}


function saveApplicantCustomFieldData() {
    //debugger;
    alertID = "#employeeEducationCreateEditAlert";
    var datafieldsObject = getCreateEditApplicantCustomFieldFieldsData();
    var isValidated = validateCreateEditApplicantCustomFieldFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/ApplicantCustomField/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantCustomFieldCreateEdit_modal").modal("hide");
                    showAlertAutoHide("", data.status, data.message);
                    retreiveApplicantCustomFieldTab();
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
function getCreateEditApplicantCustomFieldFieldsData() {
    //Get add employee form fields data and save in object
    debugger;
    var dataObj = new Object();
    dataObj.ApplicantInformationId = $('#ApplicantInformationId').val();
    dataObj.id = $('#ApplicantCustomFieldId').val();
    dataObj.CustomFieldId = $('#CustomFieldId').val();
    dataObj.CustomFieldValue = $('#CustomFieldValue').val();
    dataObj.CustomFieldNote = $('#CustomFieldNote').val();
    dataObj.IsExpirable = $('#IsExpirable').val();
    dataObj.ExpirationDate = dataObj.IsExpirable ? $('#ExpirationDate').val() : null;
    
    return dataObj;
}
function validateCreateEditApplicantCustomFieldFields(dataObj) {
    debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.CustomFieldId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.CustomFieldValue.trim().length > 0 ? 1 : 0;
        if (dataObj.IsExpirable.toLowerCase() == "true") {
            isRequiredValidated += dataObj.ExpirationDate.trim().length > 0 ? 1 : 0;
        }
       
        if ((dataObj.IsExpirable.toLowerCase() =="false" && isRequiredValidated != 2) ||
            (dataObj.IsExpirable.toLowerCase() == "true"  && isRequiredValidated != 3)) {
            isValidated = false;
            message = " Missing Required field(s)";
        }


    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getApplicantCustomFieldDeleteData(id) {
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantCustomField/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divApplicantCustomFieldCreateEdit").html(data);
                $("#applicantCustomFieldDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}


function deleteApplicantCustomField(id) {
    alertID = "#employeeEducationDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/ApplicantCustomField/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantCustomFieldDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveApplicantCustomFieldTab();
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
