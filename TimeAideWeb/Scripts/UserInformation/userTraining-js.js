$(document).ready(function () {
    $("#divLoadingTrainingTab").hide();

});
function EmployeeTrainingTabClicked() {
    retreiveEmployeeTrainingTab();

}
function openTrainingSelectionPopUp() {
    var id = $('#userID').val();
    $.ajax({
        type: "get",
        url: '/EmployeeTraining/TrainingSelectionPopUp' ,
        data: { "id": id },

        success: function (data) {
            // debugger;
           var options = { "backdrop": "static", keyboard: true };
            $('#divDdlPopupContent').html(data);
            $('#divDdlPopup').modal(options);
            $('#divDdlPopup').modal('show');

        },
        error: function () {
            // displayWarningMessage(data.ErrorMessage);
        }
    });
}
function setSelectedTraining(trainingId) {
    $("#TrainingId").val(trainingId);
    $("#divDdlPopup").modal("hide");
}
function ajaxLoadTrainingSelectionDropdown(ddlElmentId, isAllMasterData) {
    debugger;
    var id = $('#userID').val();
    $.ajax({
        url: '/EmployeeTraining/AjaxGetTrainingList',
        data: {
            'id': id,
            'isAllMasterData': isAllMasterData
        }, //dataString,
        dataType: 'json',
        type: 'GET',
        success: function (res) {
            debugger;
            var data = res;

            $(ddlElmentId + ' option').remove();
            var option = '<option value=""> Please Select </option>';
            $(ddlElmentId).append(option);
            $(data).each(function () {
                var option = '<option value=' + this.id + '>' + this.text + '</option>';
                $(ddlElmentId).append(option);
            });
           
           
        },
        error: function (xhr, status, error) {
            alert("Error ajaxLoadTrainingSelectionDropdown");
        }
    });
}

function createEditUserTraining(employeeTrainingId,selectedTraingId) {
    //debugger;
    ActionMethod = "Edit";
    employeeTrainingId = employeeTrainingId == 0 ? $("#EmployeeTrainingDetailId").val() : employeeTrainingId;   
    var id = employeeTrainingId == null ? $('#userID').val() : employeeTrainingId;
    //var ActionMethod = employeeTrainingId == null && selectedTraingId != null ? "Create" : "Edit";
    if (employeeTrainingId == null && selectedTraingId != null) {
        ActionMethod = "SubmitTraining";
    }
    else if (employeeTrainingId == null && selectedTraingId == null) {
        ActionMethod = "Create";
    }
    if (id != 0)
    {
        $.ajax({
            type: "get",
            url: '/EmployeeTraining/' + ActionMethod,
            data: { "id": id, "trainingId": selectedTraingId },

            success: function (data) {
               // debugger;
                //console.log(data);
                $("#divEmployeeTrainingCreateEdit").html(data);
                $("#employeeTrainingCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function retreiveEmployeeTrainingTab() {
    $("#divLoadingTrainingTab").show();
    //getUserTrainingDetail(null);
    getUserTrainingList();
}
function getUserTrainingList() {
   // debugger;
    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeTraining/IndexByUser',
            data: { "id": id },

            success: function (data) {
               // debugger;
                // console.log(data);
                $("#divEmployeeTrainingIndex").html(data);
                $("#divLoadingTrainingTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserTrainingDetail(trainingId) {
    //debugger;
    var id = trainingId == null ? $('#userID').val() : trainingId;
    var ActionMethod = trainingId == null ? "MostRecentRecord" : "Details";
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeTraining/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                //debugger;
                //console.log(data);
                $("#divEmployeeTrainingDetail").html(data);

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function saveUserTrainingData() {
    //debugger;
    alertID = "#employeeTrainingCreateEditAlert";
    var datafieldsObject = getCreateEditUserTrainingFieldsData();
    var isValidated = validateCreateEditUserTrainingFields(datafieldsObject);
    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeTraining/CreateEdit",
            data: JSON.stringify(datafieldsObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeTrainingCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeTrainingTab();
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
function getCreateEditUserTrainingFieldsData() {
    //Get add employee form fields data and save in object
    //debugger;
    var dataObj = new Object();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.id = $('#EmployeeTrainingId').val();
    dataObj.TrainingId = $('#TrainingId').val();
    dataObj.TrainingDate = $('#TrainingDate').val();
    dataObj.TrainingTypeId = $('#TrainingTypeId').val();
    dataObj.ExpiryDate = $('#ExpiryDate').val();
    dataObj.Note = $('#Note').val();
    return dataObj;
}
function validateCreateEditUserTrainingFields(dataObj) {
    //debugger;
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";

    if (dataObj != null) {
        isRequiredValidated += dataObj.TrainingId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.TrainingDate.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.TrainingTypeId.trim().length > 0 ? 1 : 0;

        if (isRequiredValidated != 3) {
            isValidated = false;
            message = " Missing Required field(s)";
        }


    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}

function getUserTrainingDeleteData(id) {
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeTraining/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeTrainingCreateEdit").html(data);
                $("#employeeTrainingDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}

function getUserTrainingDocView(trainingId) {
    //debugger;
    var id = trainingId == null ? $("#EmployeeTrainingDetailId").val() : trainingId;
    if (id != 0) {

        $.ajax({
            type: "get",
            url: '/EmployeeTraining/UploadDocument',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divEmployeeTrainingDocUpload").html(data);
                $("#upload_TrainingDoc_modal").modal("show");

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
function uploadUserTrainingDoc(id) {

    //debugger;
    alertID = "#userTrainingDocAlert"
    var isProceed = true;
    var formData = new FormData();
    //var id = $('#eduDocRecordID').val();

    formData.append("trainingDocRecordID", id);

    var totalFiles = document.getElementById("uploadTrainingDocFile").files.length;
    if (totalFiles > 0) {
        var file = document.getElementById("uploadTrainingDocFile").files[0];
        formData.append("TrainingDocFile", file);

    }
    else {
        isProceed = false;
        showAlertAutoHide(alertID, "Error", "Please choose the User Training Document");
    }

    if (isProceed) {
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/EmployeeTraining/UploadDocument',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $('#upload_TrainingDoc_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeTrainingTab();
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
function downloadUserTrainingDoc(trainingId) {
    //debugger;
    var id = trainingId == null ? $("#EmployeeTrainingDetailId").val() : trainingId;
    if (id != 0) {
        $.ajax({

            url: "/EmployeeTraining/AjaxCheckDocument",
            type: "get",
            data: {
                "id": id
            },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    //$("#processing-spinner").hide();
                    window.location.href = "/EmployeeTraining/DownloadDocument/" + id;
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
function deleteUserTraining(id) {
    alertID = "#employeeTrainingDeleteAlert"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeTraining/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#employeeTrainingDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retreiveEmployeeTrainingTab();
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
function autoCompleteAjaxSettingTraining(request, response, scr, field) {
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