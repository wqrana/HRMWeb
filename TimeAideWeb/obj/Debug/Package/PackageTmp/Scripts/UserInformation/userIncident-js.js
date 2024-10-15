$(document).ready(function () {
    $("#divLoadingIncidentTab").hide();

});
function EmployeeIncidentTabClicked() {
    debugger;
    retreiveEmployeeIncidentTab();
}

function retreiveEmployeeIncidentTab() {
    $("#divLoadingIncidentTab").show();
    getEmployeeIncidentDetail(null);
    getEmployeeIncidentList();
   
}

function getEmployeeIncidentList() {
    debugger;
    var userId = $('#userID').val();
    $.ajax({
        type: "get",
        url: '/EmployeeIncident/IndexByUser',
        data: { "id": userId },
        success: function (data) {
            debugger;
            //console.log(data);
            $("#divEmployeeIncidentIndex").html(data);

        },
        error: function () {
            alert("Error Loading ajax getEmployeeIncidentList");
        }
    });
}
function getEmployeeIncidentDetail(incidentId) {
    debugger;
    var userId = $('#userID').val();
    var action = incidentId == null ? "MostRecentRecord" : "Details";
    incidentId = incidentId == null ? userId : incidentId;


    $.ajax({
        type: "get",
        url: '/EmployeeIncident/' + action,
        data: { "id": incidentId },

        success: function (data) {
            debugger;
            //console.log(data);
            $("#divEmployeeIncidentDetail").html(data);
            $("#divLoadingIncidentTab").hide();
        },
        error: function () {
            alert("Error Loading ajax");
        }
    });
}

function createEditEmployeeIncident(employeeIncidentId) {
    debugger;
    employeeIncidentId = employeeIncidentId == 0 ? $("#EmployeeIncidentDetailId").val() : employeeIncidentId;
    var id = employeeIncidentId == null ? $('#userID').val() : employeeIncidentId;
    var ActionMethod = employeeIncidentId == null ? "CreateByUser" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeIncident/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeIncidentCreateEditDel").html(data);
                $("#EmployeeIncidentCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function saveEmployeeIncidentData() {
    debugger;
    var alertID = "#employeeIncidentCreateEditAlert";
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";
    //Fields value setting in object
    var dataObj = new Object();
    dataObj.id = $('#EmployeeIncidentId').val();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.IncidentTypeId = $('#IncidentTypeId').val();
    dataObj.IsOSHARecordable = $('#IsOSHARecordableAE').is(":checked");
    dataObj.OSHACaseClassificationId = $('#OSHACaseClassificationId').val();
    dataObj.CompletedDate = $('#CompletedDate').val();
    dataObj.CompletedById = $('#CompletedById').val();
    dataObj.LocationId = $('#LocationId').val();
    dataObj.IncidentAreaId = $('#IncidentAreaId').val();
    dataObj.IncidentDate = $('#IncidentDate').val();
    dataObj.IncidentTime = $('#IncidentTime').val();
    dataObj.EmployeeBeganWorkTime = $('#EmployeeBeganWorkTime').val();
    dataObj.EmployeeDoingBeforeIncident = $('#EmployeeDoingBeforeIncident').val();
    dataObj.HowIncidentOccured = $('#HowIncidentOccured').val();
   
    //Required fields validations
    if (dataObj != null) {
        isRequiredValidated += dataObj.IncidentTypeId.trim().length > 0 ? 1 : 0;
        if (dataObj.IsOSHARecordable == false && isRequiredValidated != 1) {
            message = " Missing Required field(s)";
            isValidated = false;
        }
        else if (dataObj.IsOSHARecordable == true) {
           
            isRequiredValidated += dataObj.OSHACaseClassificationId.trim().length > 0 ? 1 : 0;
            isRequiredValidated += dataObj.CompletedDate.trim().length > 0 ? 1 : 0;
            isRequiredValidated += dataObj.CompletedById.trim().length > 0 ? 1 : 0;
            isRequiredValidated += dataObj.LocationId.trim().length > 0 ? 1 : 0;
            isRequiredValidated += dataObj.IncidentAreaId.trim().length > 0 ? 1 : 0;
            isRequiredValidated += dataObj.IncidentDate.trim().length > 0 ? 1 : 0;
            isRequiredValidated += dataObj.EmployeeBeganWorkTime.trim().length > 0 ? 1 : 0;

            isRequiredValidated += dataObj.EmployeeDoingBeforeIncident.trim().length > 0 ? 1 : 0;
            isRequiredValidated += dataObj.HowIncidentOccured.trim().length > 0 ? 1 : 0;
            if (isRequiredValidated != 10) {
                isValidated = false;
                message = ' OSHARecordable Incident, Missing Required field(s) '
                    + '{IncidentType/CaseClassification/CompletedDate/CompletedBy/Establishment/Area/'
                    + 'IncidentDate / BegainWorkTime / BeforeIncident / HowOccured}';
            }
         }
    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeIncident/CreateEdit",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#EmployeeIncidentCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    //location.reload(true);
                    var selectedId = $("#EmployeeIncidentDetailId").val();
                    var retrieveId = (selectedId == dataObj.id && selectedId != 0) ? selectedId : null;
                    getEmployeeIncidentDetail(retrieveId);
                    getEmployeeIncidentList();
                }
                else {
                    showAlertAutoHide(alertID, data.status, data.message);
                }
            }
            ,
            error: function (request, status, error) {
                alert('Error in in saving parent data');
                return false;
            }
        });
    }

}




function getEmployeeIncidentDeleteData(employeeIncidentId) {
    debugger;
    if (employeeIncidentId != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeIncident/Delete',
            data: { "id": employeeIncidentId },

            success: function (data) {
                //debugger;
                $("#divEmployeeIncidentCreateEditDel").html(data);
                $("#employeeIncidentDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function deleteEmployeeIncident(employeeIncidentId) {
    debugger;
    alertID = "#employeeIncidentDeleteAlert"
    if (employeeIncidentId != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeIncident/ConfirmDelete",
            data: { "id": employeeIncidentId },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#employeeIncidentDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    var currSelEmployeeIncidentId = $("#EmployeeIncidentDetailId").val();
                    var retreiveId = currSelEmployeeIncidentId;
                    if (currSelEmployeeIncidentId == employeeIncidentId) {
                        retreiveId = null;
                    }
                    getEmployeeIncidentDetail(retreiveId);
                    getEmployeeIncidentList();
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

//End Section
//Incident Injury Section
function EditEmployeeIncidentInjury() {
    debugger;
    employeeIncidentId = $("#EmployeeIncidentDetailId").val();

    if (employeeIncidentId != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeIncident/EditInjury',
            data: { "id": employeeIncidentId },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeIncidentCreateEditDel").html(data);
                $("#EmployeeIncidentInjuryEdit_modal").modal("show");

                 },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", " Please select the Incident Case");
    }
}
function saveEmployeeIncidentInjuryData() {
    debugger;
    var alertID = "#employeeIncidentInjuryEditAlert";
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";
    //Fields value setting in object
    var IsOSHARecordable = $('#IsOSHARecordable').is(":checked");
    var dataObj = new Object();
    dataObj.id = $('#EmployeeIncidentId').val();
    

    dataObj.OSHAInjuryClassificationId = $('#OSHAInjuryClassificationId').val();
    dataObj.IncidentBodyPartId = $('#IncidentBodyPartId').val();
    dataObj.IncidentInjuryDescriptionId = $('#IncidentInjuryDescriptionId').val();
    dataObj.IncidentInjurySourceId = $('#IncidentInjurySourceId').val();
    dataObj.DateOfDeath = $('#DateOfDeath').val();
    dataObj.RestrictedFromWorkDays = $('#RestrictedFromWorkDays').val();
    dataObj.AwayFromWorkDays = $('#AwayFromWorkDays').val();
    //Required fields validations
    if (dataObj != null) {
        isRequiredValidated += dataObj.IncidentBodyPartId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.IncidentInjuryDescriptionId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.IncidentInjurySourceId.trim().length > 0 ? 1 : 0;


        if (IsOSHARecordable==true && isRequiredValidated != 3) {
            isValidated = false;           
            message = ' OSHARecordable Incident, Missing Required field(s) '
                + '{BodyPart/InjuryDescription/InjurySource}';
        }
    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeIncident/EditInjury",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#EmployeeIncidentInjuryEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    //location.reload(true);
                    var selectedId = $("#EmployeeIncidentDetailId").val();

                    getEmployeeIncidentDetail(selectedId);
                    getEmployeeIncidentList();
                }
                else {
                    showAlertAutoHide(alertID, data.status, data.message);
                }
            }
            ,
            error: function (request, status, error) {
                alert('Error in in saving parent data');
                return false;
            }
        });
    }

}
//End Injury Section
//Incident Treatment Section
function EditEmployeeIncidentTreatment() {
    debugger;
    employeeIncidentId = $("#EmployeeIncidentDetailId").val();

    if (employeeIncidentId != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeIncident/EditTreatment',
            data: { "id": employeeIncidentId },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeIncidentCreateEditDel").html(data);
                $("#EmployeeIncidentTreatmentEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", " Please select the Incident Case");
    }
}
function saveEmployeeIncidentTreatmentData() {
    debugger;
    var alertID = "#employeeIncidentTreatmentEditAlert";
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";
    //Fields value setting in object
    var IsOSHARecordable = $('#IsOSHARecordable').is(":checked");
    var dataObj = new Object();
    dataObj.id = $('#EmployeeIncidentId').val();
    
    dataObj.PhysicianName = $('#PhysicianName').val();
    dataObj.IncidentTreatmentFacilityId = $('#IncidentTreatmentFacilityId').val();
    dataObj.IsTreatedInEmergencyRoom = $('#IsTreatedInEmergencyRoomAE').is(":checked");
    dataObj.IsHospitalizedOvernight = $('#IsHospitalizedOvernightAE').is(":checked");
    dataObj.HospitalizedDays = $('#HospitalizedDays').val();
  
    //Required fields validations
    if (dataObj != null) {
        isRequiredValidated += dataObj.PhysicianName.trim().length > 0 ? 1 : 0;
     
        if (IsOSHARecordable == true && isRequiredValidated != 1) {
            isValidated = false;
            message = ' OSHARecordable Incident, Missing Required field(s) '
                + '{PhysicianName}';
        }
    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeIncident/EditTreatment",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#EmployeeIncidentTreatmentEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    //location.reload(true);
                    var selectedId = $("#EmployeeIncidentDetailId").val();

                    getEmployeeIncidentDetail(selectedId);
                    getEmployeeIncidentList();
                }
                else {
                    showAlertAutoHide(alertID, data.status, data.message);
                }
            }
            ,
            error: function (request, status, error) {
                alert('Error in in saving parent data');
                return false;
            }
        });
    }

}
function viewOSHA301Report() {
    debugger;
    var incidentId = $("#EmployeeIncidentDetailId").val();
    if (incidentId == "" || incidentId=="0") {
        showAlertAutoHide("", "Error", "No Incident is available for OSHA 301 report");
        return;
    }
    var dataObj = new Object();
    dataObj.ReportId = 26;
    dataObj.CriteriaType = 1;
    dataObj.IncidentId = incidentId;
    
    $.ajax({
        type: "POST",
        url: "/Reports/ReportViewAsDownload",
        data: JSON.stringify(dataObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            debugger;
            console.log(data.downloadFile);
            if (data.status == "Success") {
                showAlertAutoHide("", "Success", "Report is generated successfully!");               
                window.location.href = "/Reports/DownloadReportAsPDF?downloadFileName=" + data.downloadFile;
            }
            else {
                showAlertAutoHide("", 'Error', data.message);
            }
            
        },
        error: function (request, status, error) {
            alert('Error in in runing  report data');
            return false;
        }
    });
}
//End Treatment Section