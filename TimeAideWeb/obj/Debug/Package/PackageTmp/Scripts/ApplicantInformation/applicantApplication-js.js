$(document).ready(function () {
    $("#divLoadingApplicationTab").hide();

});
function ApplicantApplicationTabClicked() {
    debugger;
    $("#applicantError_modal").parent().html("");
    retreiveApplicantApplicationTab();

}
function retreiveApplicantApplicationTab() {
    $("#divLoadingApplicationTab").show();
    getApplicantApplicationDetail();
}
function getApplicantApplicationDetail() {
    //debugger;
    var id = $('#ApplicantInformationId').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantApplication/Details',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divApplicantApplicationDetail").html(data);
                $("#divLoadingApplicationTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function editApplicantApplicationShift() {
    debugger;

    var id = $('#ApplicantInformationId').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantApplication/EditApplicantShift',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divApplicantApplicationEdit").html(data);
                $("#applicantApplicationShiftEdit_modal").modal("show");
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}
function editApplicantApplication() {
    debugger;

    var id =$('#ApplicantInformationId').val();
   
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/ApplicantApplication/EditApplication',
            data: { "id": id },

            success: function (data) {
                debugger;
                // console.log(data);
                $("#divApplicantApplicationEdit").html(data);
                $("#applicantApplicationCreateEdit_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}



function saveApplicantApplicationData() {
    debugger;
    
    var dataObj = new Object();
    dataObj.ApplicantInformationId = $('#ApplicantInformationId').val();
    dataObj.Id = $('#ApplicantApplicationId').val();
    dataObj.DateApplied = $('#DateApplied').val();
    dataObj.PositionId = $('#PositionId').val();
    dataObj.ApplicantReferenceTypeId = $('#ApplicantReferenceTypeId').val();
    dataObj.ApplicantReferenceSourceId = $('#ApplicantReferenceSourceId').val();
    dataObj.JobLocationId = $('#JobLocationId').val(); 
    dataObj.Rate = $('#Rate').val();
    dataObj.RateFrequencyId = $('#RateFrequencyId').val();
    dataObj.DateAvailable = $('#DateAvailable').val();
    dataObj.IsOvertime = $('#IsOvertime').is(":checked");
    dataObj.IsWorkedBefore = $('#IsWorkedBefore').is(":checked");
    dataObj.WorkedBeforeDate = $('#WorkedBeforeDate').val();
    dataObj.IsRelativeInCompany = $('#IsRelativeInCompany').is(":checked");
    dataObj.RelativeName = $('#RelativeName').val();

    var selectedIdsList = [];   
    var selectedIdsStr = "";   
    $('#EmploymentTypeIds option:selected').each(function () {
        selectedIdsList.push($(this).val());      
    });
    if (selectedIdsList.length > 0) {
        selectedIdsStr = selectedIdsList.join(",");       
    }
    dataObj.SelectedEmploymentTypeId = selectedIdsStr;
    selectedIdsList = [];
    selectedIdsStr = "";
    $('#JobLocationIds option:selected').each(function () {
        selectedIdsList.push($(this).val());
    });
    if (selectedIdsList.length > 0) {
        selectedIdsStr = selectedIdsList.join(",");
    }
    dataObj.SelectedJobLocationId = selectedIdsStr;
    //Fields validation
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.DateApplied.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.PositionId.trim().length > 0 ? 1 : 0;

        if (isRequiredValidated != 2) {
            isValidated = false;
            message = " Missing Required field(s)";
        }
        if (!isValidated) showAlertAutoHide("", 'Error', message);
    }
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/ApplicantApplication/EditApplication",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantApplicationCreateEdit_modal").modal("hide");
                    showAlertAutoHide("", data.status, data.message);
                    retreiveApplicantApplicationTab();
                    // location.reload(true);
                }
                else {
                    showAlertAutoHide("", data.status, data.message);
                }
            }
            ,
            error: function (request, status, error) {
               // displayErrorMessage('Error in deleting parent alert data');
                showAlertAutoHide("", "Error", error);
                return false;
            }
        });
    }
}

function saveApplicantApplicationShiftData() {
    //debugger;

    var dataObj = new Object();
    dataObj.ApplicantInformationId = $('#ApplicantInformationId').val();
    dataObj.Id = $('#ApplicantApplicationId').val();
    dataObj.IsMondayShift = $('#IsMondayShift').is(":checked");
    if (dataObj.IsMondayShift == true) {
        dataObj.MondayStartShift = $('#MondayStartShift').val();
        dataObj.MondayEndShift = $('#MondayEndShift').val();
    }
    dataObj.IsTuesdayShift = $('#IsTuesdayShift').is(":checked");
    if (dataObj.IsTuesdayShift == true) {
        dataObj.TuesdayStartShift = $('#TuesdayStartShift').val();
        dataObj.TuesdayEndShift = $('#TuesdayEndShift').val();
    }
    dataObj.IsWednesdayShift = $('#IsWednesdayShift').is(":checked");
    if (dataObj.IsWednesdayShift == true) {
        dataObj.WednesdayStartShift = $('#WednesdayStartShift').val();
        dataObj.WednesdayEndShift = $('#WednesdayEndShift').val();
    }
    dataObj.IsThursdayShift = $('#IsThursdayShift').is(":checked");
    if (dataObj.IsThursdayShift == true) {
        dataObj.ThursdayStartShift = $('#ThursdayStartShift').val();
        dataObj.ThursdayEndShift = $('#ThursdayEndShift').val();
    }
    dataObj.IsFridayShift = $('#IsFridayShift').is(":checked");
    if (dataObj.IsFridayShift == true) {
        dataObj.FridayStartShift = $('#FridayStartShift').val();
        dataObj.FridayEndShift = $('#FridayEndShift').val();
    }
    dataObj.IsSaturdayShift = $('#IsSaturdayShift').is(":checked");
    if (dataObj.IsSaturdayShift == true) {
        dataObj.SaturdayStartShift = $('#SaturdayStartShift').val();
        dataObj.SaturdayEndShift = $('#SaturdayEndShift').val();
    }
    dataObj.IsSundayShift = $('#IsSundayShift').is(":checked");
    if (dataObj.IsSundayShift == true) {
        dataObj.SundayStartShift = $('#SundayStartShift').val();
        dataObj.SundayEndShift = $('#SundayEndShift').val();
    }
    //Fields validation
   
    var isValidated = true;
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/ApplicantApplication/EditApplicantShift",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantApplicationShiftEdit_modal").modal("hide");
                    showAlertAutoHide("", data.status, data.message);
                    retreiveApplicantApplicationTab();
                    // location.reload(true);
                }
                else {
                    showAlertAutoHide("", data.status, data.message);
                }
            }
            ,
            error: function (request, status, error) {
                // displayErrorMessage('Error in deleting parent alert data');
                showAlertAutoHide("", "Error", error);
                return false;
            }
        });
    }
}
