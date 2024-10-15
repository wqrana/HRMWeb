$(document).ready(function () {
    $("#divLoadingAppraisalTab").hide();

});
function EmployeeAppraisalTabClicked() {
    debugger;
    retreiveEmployeeAppraislTab();
}

function retreiveEmployeeAppraislTab() {
    $("#divLoadingAppraisalTab").show();
    getEmployeeAppraisalDetail(null);
    getEmployeeAppraisalList();
    
}

function getEmployeeAppraisalList() {
    debugger;
    var userId = $('#userID').val();
    $.ajax({
        type: "get",
        url: '/EmployeeAppraisal/IndexByUser',
        data: { "id": userId },
        success: function (data) {
            debugger;
            //console.log(data);
            $("#divEmployeeAppraisalIndex").html(data);

        },
        error: function () {
            alert("Error Loading ajax getEmployeeAppraisalList");
        }
    });
}
function getEmployeeAppraisalDetail(appraisalId) {
    debugger;
    var userId = $('#userID').val();
    var action = appraisalId == null ? "MostRecentRecord" : "Details";
    appraisalId = appraisalId == null ? userId : appraisalId;
    

    $.ajax({
        type: "get",
        url: '/EmployeeAppraisal/' + action,
        data: { "id": appraisalId },

        success: function (data) {
            debugger;
            //console.log(data);
            $("#divEmployeeAppraisalDetail").html(data);
            $("#divLoadingAppraisalTab").hide();
        },
        error: function () {
            alert("Error Loading ajax");
        }
    });
}

function createEditEmployeeAppraisal(employeeAppraisalId) {
    debugger;
    employeeAppraisalId = employeeAppraisalId == 0 ? $("#EmployeeAppraisalDetailId").val() : employeeAppraisalId;
    var id = employeeAppraisalId == null ? $('#userID').val() : employeeAppraisalId;
    var ActionMethod = employeeAppraisalId == null ? "CreateByUser" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeAppraisal/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                 debugger;
                //console.log(data);
                $("#divEmployeeAppraisalCreateEditDel").html(data);
                $("#EmployeeAppraisalCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function saveEmployeeAppraisalData() {
    debugger;
    var alertID = "#employeeAppraisalCreateEditAlert";
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";
    //Fields value setting in object
    var dataObj = new Object();
    dataObj.id = $('#EmployeeAppraisalId').val();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.AppraisalReviewDate = $('#AppraisalReviewDate').val();
    dataObj.AppraisalDueDate = $('#AppraisalDueDate').val();
    dataObj.NextAppraisalDueDate = $('#NextAppraisalDueDate').val();
    dataObj.AppraisalTemplateId = $('#EmployeeAppraisalTemplateId').val();
    dataObj.EvaluationStartDate = $('#EvaluationStartDate').val();
    dataObj.EvaluationEndDate = $('#EvaluationEndDate').val();
    dataObj.PositionId = $('#EmployeeAppraisalPositionId').val();
    dataObj.CompanyId = $('#EmployeeAppraisalCompanyID').val();
    dataObj.DepartmentId = $('#EmployeeAppraisalDepartmentId').val();
    //Required fields validations
    if (dataObj != null) {
        isRequiredValidated += dataObj.AppraisalReviewDate.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.AppraisalTemplateId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.EvaluationStartDate.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.EvaluationEndDate.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.PositionId.trim().length > 0 ? 1 : 0;
        //isRequiredValidated += dataObj.CompanyId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.DepartmentId.trim().length > 0 ? 1 : 0;


        if (isRequiredValidated != 6) {
            isValidated = false;
            message = " Missing Required field(s)";
        }
    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeAppraisal/CreateEdit",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#EmployeeAppraisalCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    //location.reload(true);
                    var selectedId = $("#EmployeeAppraisalDetailId").val();
                    var retrieveId = (selectedId == dataObj.id && selectedId!=0) ? selectedId : null; 
                    getEmployeeAppraisalDetail(retrieveId);
                    getEmployeeAppraisalList();
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

//Template Selection Popup Section
function openAppraisalTemplateSelectionPopUp(positionId) {

//var id = $('#userID').val();
$.ajax({
    type: "get",
    url: '/EmployeeAppraisal/AppraisalTemplateSelectionPopUp',
    data: { "id": positionId },

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
function setSelectedAppraisalTemplate(templateId) {
    $("#EmployeeAppraisalTemplateId").val(templateId);
    $("#divDdlPopup").modal("hide");
}
function ajaxLoadAppraisalTemplateSelectionDropdown(ddlElmentId, isAllMasterData) {
    debugger;
    var positionId = $('#EmployeeAppraisalPositionId').val();
    $.ajax({
        url: '/EmployeeAppraisal/AjaxGetAppraisalTemplateList',
        data: {
            'id': positionId,
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
            alert("Error ajaxLoadTemplateSelectionDropdown");
        }
    });
}
function getEmployeeAppraisalDeleteData(employeeAppraisalId) {
debugger;
    if (employeeAppraisalId != '') {

    $.ajax({
        type: "get",
        url: '/EmployeeAppraisal/Delete',
        data: { "id": employeeAppraisalId },

        success: function (data) {
            //debugger;
            $("#divEmployeeAppraisalCreateEditDel").html(data);
            $("#employeeAppraisalDelete_modal").modal("show");

        },
        error: function () {
            // displayWarningMessage(data.ErrorMessage);
        }
    });
    }
 }
function deleteEmployeeAppraisal(employeeAppraisalId) {
    debugger;
    alertID = "#employeeAppraisalDeleteAlert"
    if (employeeAppraisalId != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeAppraisal/ConfirmDelete",
            data: { "id": employeeAppraisalId },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#employeeAppraisalDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    var currSelEmployeeAppraisalId = $("#EmployeeAppraisalDetailId").val();
                    var retreiveId = currSelEmployeeAppraisalId;
                    if (currSelEmployeeAppraisalId == employeeAppraisalId) {
                        retreiveId = null;
                    }
                    getEmployeeAppraisalDetail(retreiveId);
                    getEmployeeAppraisalList();
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

//EmployeeAppraisalGoal section
//divEmployeeAppraisalGoals (listing goal)

function getEmployeeAppraisalGoalList(employeeAppraisalId) {
    debugger;
    $.ajax({
        type: "get",
        url: '/EmployeeAppraisalGoal/IndexByAppraisal',
        data: { "id": employeeAppraisalId },
        success: function (data) {
            debugger;
            //console.log(data);
            $("#divEmployeeAppraisalGoals").html(data);

        },
        error: function () {
            alert("Error Loading ajax getEmployeeAppraisalGoalList");
        }
    });
}
function createEditEmployeeAppraisalGoal(employeeAppraisalGoalId) {
    debugger;
    
    var id = employeeAppraisalGoalId == 0 ? $('#EmployeeAppraisalDetailId').val() : employeeAppraisalGoalId;
    var ActionMethod = employeeAppraisalGoalId == 0 ? "CreateByAppraisal" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeAppraisalGoal/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeAppraisalCreateEditDel").html(data);
                $("#EmployeeAppraisalGoalCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", " Please select the Appraisal");
    }
}
 
function saveEmployeeAppraisalGoalData() {
    debugger;
    var alertID = "#employeeAppraisalGoalCreateEditAlert";
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";
    //Fields value setting in object
    var dataObj = new Object();
    dataObj.id = $('#EmployeeAppraisalGoalId').val();
    dataObj.EmployeeAppraisalId = $('#EmployeeAppraisalDetailId').val();
    dataObj.AppraisalGoalId = $('#AppraisalGoalId').val();
    dataObj.AppraisalRatingScaleDetailId = $('#AppraisalRatingScaleDetailId').val();
    dataObj.GoalRatingName = $('#GoalRatingName').val();
    dataObj.GoalRatingValue = $('#GoalRatingValue').val();
    dataObj.GoalScaleMaxValue = $('#GoalScaleMaxValue').val();
    
    dataObj.ReviewerComments = $('#ReviewerComments').val();
    
    //Required fields validations
    if (dataObj != null) {
        isRequiredValidated += dataObj.AppraisalGoalId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.AppraisalRatingScaleDetailId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.GoalRatingValue.trim().length > 0 ? 1 : 0;
       
        if (isRequiredValidated != 3) {
            isValidated = false;
            message = " Missing Required field(s)";
        }
    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeAppraisalGoal/CreateEdit",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#EmployeeAppraisalGoalCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    //location.reload(true);
                    getEmployeeAppraisalGoalList(dataObj.EmployeeAppraisalId);
                    getEmployeeAppraisalResult(dataObj.EmployeeAppraisalId);
                    
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

function openAppraisalGoalSelectionPopUp() {

    var templateId = $("#AppraisalTemplateDetailId").val();
    $.ajax({
        type: "get",
        url: '/EmployeeAppraisalGoal/AppraisalGoalSelectionPopUp',
        data: { "id": templateId },

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

function setSelectedAppraisalGoal(goalId){

    $("#AppraisalGoalId").val(goalId);
    $("#divDdlPopup").modal("hide");
    ajaxLoadAppraisalGoalRatingDropdown("#AppraisalRatingScaleDetailId", goalId);
}
function ajaxLoadAppraisalGoalSelectionDropdown(ddlElmentId, isAllMasterData) {
            debugger;
        var templateId = $("#AppraisalTemplateDetailId").val();
            $.ajax({
                url: '/EmployeeAppraisalGoal/AjaxGetAppraisalGoalList',
                data: {
                    'id': templateId,
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
                    alert("Error ajaxLoadTemplateSelectionDropdown");
                }
            });
        }

function ajaxLoadAppraisalGoalRatingDropdown(ddlElmentId, goalId) {
    debugger;
   
    $.ajax({
        url: '/EmployeeAppraisalGoal/AjaxGetAppraisalGoalRatingList',
        data: {
            'id': goalId,
            
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
            alert("Error ajaxLoadTemplateSelectionDropdown");
        }
    });
}
function ajaxSetAppraisalGoalRatingValue(ElmentId, ratingDetailId) {
    if (ratingDetailId != "") {
        $.ajax({
            url: '/EmployeeAppraisalGoal/AjaxGetAppraisalGoalRatingValue',
            data: {
                'id': ratingDetailId,

            }, //dataString,
            dataType: 'json',
            type: 'GET',
            success: function (res) {
                debugger;
                var data = res;

                $(ElmentId).val(data.ratingValue);
                $('#GoalScaleMaxValue').val(data.maxRatingValue);
                $('#spanGoalScaleMaxValue').text(data.maxRatingValue);
            }

        });
    }
    else {
        $(ElmentId).val('0.00');
        $('#spanGoalScaleMaxValue').text('0.00');
    }

}

function getEmployeeAppraisalGoalDeleteData(employeeAppraisalGoalId) {
    debugger;
    if (employeeAppraisalGoalId != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeAppraisalGoal/Delete',
            data: { "id": employeeAppraisalGoalId },

            success: function (data) {
                //debugger;
                $("#divEmployeeAppraisalCreateEditDel").html(data);
                $("#employeeAppraisalGoalDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function deleteEmployeeAppraisalGoal(employeeAppraisalGoalId) {
    debugger;
    alertID = "#employeeAppraisalGoalDeleteAlert"
    if (employeeAppraisalGoalId != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeAppraisalGoal/ConfirmDelete",
            data: { "id": employeeAppraisalGoalId },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#employeeAppraisalGoalDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    var currSelEmployeeAppraisalId = $("#EmployeeAppraisalDetailId").val();
                    getEmployeeAppraisalGoalList(currSelEmployeeAppraisalId);
                    getEmployeeAppraisalResult(currSelEmployeeAppraisalId);
                   

               
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



//End Goal Section

//EmployeeAppraisalSkill section
//divEmployeeAppraisalSkills (lising skills)
function getEmployeeAppraisalSkillList(employeeAppraisalId) {
    debugger;
    $.ajax({
        type: "get",
        url: '/EmployeeAppraisalSkill/IndexByAppraisal',
        data: { "id": employeeAppraisalId },
        success: function (data) {
            debugger;
            //console.log(data);
            $("#divEmployeeAppraisalSkills").html(data);

        },
        error: function () {
            alert("Error Loading ajax getEmployeeAppraisalSkillList");
        }
    });
}
function createEditEmployeeAppraisalSkill(employeeAppraisalSkillId) {
    debugger;

    var id = employeeAppraisalSkillId == 0 ? $('#EmployeeAppraisalDetailId').val() : employeeAppraisalSkillId;
    var ActionMethod = employeeAppraisalSkillId == 0 ? "CreateByAppraisal" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeAppraisalSkill/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeAppraisalCreateEditDel").html(data);
                $("#EmployeeAppraisalSkillCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", " Please select the Appraisal");
    }
}

function saveEmployeeAppraisalSkillData() {
    debugger;
    var alertID = "#employeeAppraisalSkillCreateEditAlert";
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";
    //Fields value setting in object
    var dataObj = new Object();
    dataObj.id = $('#EmployeeAppraisalSkillId').val();
    dataObj.EmployeeAppraisalId = $('#EmployeeAppraisalDetailId').val();
    dataObj.AppraisalSkillId = $('#AppraisalSkillId').val();
    dataObj.AppraisalRatingScaleDetailId = $('#AppraisalRatingScaleDetailId').val();
    dataObj.SkillRatingName = $('#SkillRatingName').val();
    dataObj.SkillRatingValue = $('#SkillRatingValue').val();
    dataObj.SkillScaleMaxValue = $('#SkillScaleMaxValue').val();

    dataObj.ReviewerComments = $('#ReviewerComments').val();

    //Required fields validations
    if (dataObj != null) {
        isRequiredValidated += dataObj.AppraisalSkillId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.AppraisalRatingScaleDetailId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.SkillRatingValue.trim().length > 0 ? 1 : 0;

        if (isRequiredValidated != 3) {
            isValidated = false;
            message = " Missing Required field(s)";
        }
    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/EmployeeAppraisalSkill/CreateEdit",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#EmployeeAppraisalSkillCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    //location.reload(true);
                    getEmployeeAppraisalSkillList(dataObj.EmployeeAppraisalId);
                    getEmployeeAppraisalResult(dataObj.EmployeeAppraisalId);
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

function openAppraisalSkillSelectionPopUp() {

    var templateId = $("#AppraisalTemplateDetailId").val();
    $.ajax({
        type: "get",
        url: '/EmployeeAppraisalSkill/AppraisalSkillSelectionPopUp',
        data: { "id": templateId },

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

function setSelectedAppraisalSkill(skillId) {

    $("#AppraisalSkillId").val(skillId);
    $("#divDdlPopup").modal("hide");
    ajaxLoadAppraisalSkillRatingDropdown("#AppraisalRatingScaleDetailId", skillId);
}
function ajaxLoadAppraisalSkillSelectionDropdown(ddlElmentId, isAllMasterData) {
    debugger;
    var templateId = $("#AppraisalTemplateDetailId").val();
    $.ajax({
        url: '/EmployeeAppraisalSkill/AjaxGetAppraisalSkillList',
        data: {
            'id': templateId,
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
            alert("Error ajaxLoadTemplateSelectionDropdown");
        }
    });
}

function ajaxLoadAppraisalSkillRatingDropdown(ddlElmentId, skillId) {
    debugger;

    $.ajax({
        url: '/EmployeeAppraisalSkill/AjaxGetAppraisalSkillRatingList',
        data: {
            'id': skillId,

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
            alert("Error ajaxLoadTemplateSelectionDropdown");
        }
    });
}
function ajaxSetAppraisalSkillRatingValue(ElmentId, ratingDetailId) {
    if (ratingDetailId != "") {
        $.ajax({
            url: '/EmployeeAppraisalSkill/AjaxGetAppraisalSkillRatingValue',
            data: {
                'id': ratingDetailId,

            }, //dataString,
            dataType: 'json',
            type: 'GET',
            success: function (res) {
                debugger;
                var data = res;

                $(ElmentId).val(data.ratingValue);
                $('#SkillScaleMaxValue').val(data.maxRatingValue);
                $('#spanSkillScaleMaxValue').text(data.maxRatingValue);
            }

        });
    }
    else {
        $(ElmentId).val('0.00');
        $('#spanSkillScaleMaxValue').text('0.00');
    }

}

function getEmployeeAppraisalSkillDeleteData(employeeAppraisalSkillId) {
    debugger;
    if (employeeAppraisalSkillId != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeAppraisalSkill/Delete',
            data: { "id": employeeAppraisalSkillId },

            success: function (data) {
                //debugger;
                $("#divEmployeeAppraisalCreateEditDel").html(data);
                $("#employeeAppraisalSkillDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function deleteEmployeeAppraisalSkill(employeeAppraisalSkillId) {
    debugger;
    alertID = "#employeeAppraisalSkillDeleteAlert"
    if (employeeAppraisalSkillId != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeAppraisalSkill/ConfirmDelete",
            data: { "id": employeeAppraisalSkillId },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#employeeAppraisalSkillDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    var currSelEmployeeAppraisalId = $("#EmployeeAppraisalDetailId").val();
                    getEmployeeAppraisalSkillList(currSelEmployeeAppraisalId);
                    getEmployeeAppraisalResult(currSelEmployeeAppraisalId);

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

//EmployeeAppraisalDocument section
//divEmployeeAppraisalDocuments (lising documents)
function getEmployeeAppraisalDocumentList(employeeAppraisalId) {
    debugger;
    $.ajax({
        type: "get",
        url: '/EmployeeAppraisalDocument/IndexByAppraisal',
        data: { "id": employeeAppraisalId },
        success: function (data) {
            debugger;
            //console.log(data);
            $("#divEmployeeAppraisalDocuments").html(data);

        },
        error: function () {
            alert("Error Loading ajax getEmployeeAppraisalDocumentList");
        }
    });
}
function createEditEmployeeAppraisalDocument(employeeAppraisalDocumentId) {
    debugger;

    var id = employeeAppraisalDocumentId == 0 ? $('#EmployeeAppraisalDetailId').val() : employeeAppraisalDocumentId;
    var ActionMethod = employeeAppraisalDocumentId == 0 ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeAppraisalDocument/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeAppraisalCreateEditDel").html(data);
                $("#EmployeeAppraisalDocumentCreateEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", " Please select the Appraisal");
    }
}
//End function

function saveEmployeeAppraisalDocumentData() {
    debugger;
    var alertID = "#employeeAppraisalDocumentCreateEditAlert";
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";
    //Fields value setting in object
    var dataObj = new Object();
    dataObj.Id = $('#EmployeeAppraisalDocumentId').val();
    dataObj.EmployeeAppraisalId = $('#EmployeeAppraisalDetailId').val();
    dataObj.AppraisalDocumentName = $('#AppraisalDocumentName').val();

    //Required fields validations
    if (dataObj != null) {
        isRequiredValidated += dataObj.AppraisalDocumentName.trim().length > 0 ? 1 : 0;
        if (isRequiredValidated != 1) {
            isValidated = false;
            message = " Missing Required field(s)";
        }
        else {
            var totalFiles = document.getElementById("AppraisalDocumentFile").files.length;
            if (totalFiles < 1) {
                isValidated = false;
                message = " Missing Attach file";
            }

        }

    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    if (isValidated) {
        var formData = new FormData();
        var file = document.getElementById("AppraisalDocumentFile").files[0];     
        formData.append("Id", dataObj.Id);
        formData.append("EmployeeAppraisalId", dataObj.EmployeeAppraisalId);
        formData.append("AppraisalDocumentName", dataObj.AppraisalDocumentName);
        formData.append("AppraisalDocumentFile", file);

        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: '/EmployeeAppraisalDocument/CreateEdit',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#EmployeeAppraisalDocumentCreateEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    //location.reload(true);
                    getEmployeeAppraisalDocumentList(dataObj.EmployeeAppraisalId);
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
//End function
function downloadEmployeeAppraisalDocument(employeeAppraisalDocumentId) {
    debugger;
   
    if (employeeAppraisalDocumentId != 0) {
        $.ajax({

            url: "/EmployeeAppraisalDocument/AjaxCheckAppraisalDocument",
            type: "get",
            data: {
                "id": employeeAppraisalDocumentId
            },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    //$("#processing-spinner").hide();
                    window.location.href = "/EmployeeAppraisalDocument/DownloadAppraisalDocument/" + employeeAppraisalDocumentId;
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

function getEmployeeAppraisalDocumentDeleteData(employeeAppraisalDocumentId) {
    debugger;
    if (employeeAppraisalDocumentId != '') {

        $.ajax({
            type: "get",
            url: '/EmployeeAppraisalDocument/Delete',
            data: { "id": employeeAppraisalDocumentId },

            success: function (data) {
                //debugger;
                $("#divEmployeeAppraisalCreateEditDel").html(data);
                $("#employeeAppraisalDocumentDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function deleteEmployeeAppraisalDocument(employeeAppraisalDocumentId) {
    debugger;
    alertID = "#employeeAppraisalDocumentDeleteAlert"
    if (employeeAppraisalDocumentId != '') {
        $.ajax({
            type: "Post",
            url: "/EmployeeAppraisalDocument/ConfirmDelete",
            data: { "id": employeeAppraisalDocumentId },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#employeeAppraisalDocumentDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    var currSelEmployeeAppraisalId = $("#EmployeeAppraisalDetailId").val();
                    getEmployeeAppraisalDocumentList(currSelEmployeeAppraisalId);

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
// End Section Appraisal Document


//Appraisal Result Section
function getEmployeeAppraisalResult(employeeAppraisalId) {
    debugger;
   // employeeAppraisalId = $("#EmployeeAppraisalDetailId").val();

    if (employeeAppraisalId != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeAppraisal/DisplayResult',
            data: { "id": employeeAppraisalId },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeAppraisalResult").html(data);
                getEmployeeAppraisalList();
               
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", " Please select the Appraisal");
    }
}

function EditEmployeeAppraisalResult() {
    debugger;
    employeeAppraisalId =  $("#EmployeeAppraisalDetailId").val();
   
    if (employeeAppraisalId != 0) {
        $.ajax({
            type: "get",
            url: '/EmployeeAppraisal/EditResult',
            data: { "id": employeeAppraisalId },

            success: function (data) {
                debugger;
                //console.log(data);
                $("#divEmployeeAppraisalCreateEditDel").html(data);
                $("#EmployeeAppraisalResultEdit_modal").modal("show");


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
    else {
        showAlertAutoHide("#userDetailAlert", "Error", " Please select the Appraisal");
    }
}
function saveEmployeeAppraisalResultData() {
    debugger;
    var alertID = "#employeeAppraisalResultEditAlert";
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";
    //Fields value setting in object
    var dataObj = new Object();
    dataObj.id = $('#EmployeeAppraisalId').val();
    dataObj.AppraisalResultId = $('#AppraisalResultId').val();
    dataObj.AppraisalOverallScore = $('#AppraisalOverallScore').val();
    dataObj.AppraisalTotalMaxValue = $('#AppraisalTotalMaxValue').val();
    dataObj.AppraisalOverallPct = $('#AppraisalOverallPct').val();
    dataObj.AppraisalReviewerComments = $('#AppraisalReviewerComments').val();
    dataObj.AppraisalEmployeeComments = $('#AppraisalEmployeeComments').val();
    
    //Required fields validations
    if (dataObj != null) {
        isRequiredValidated += dataObj.AppraisalResultId.trim().length > 0 ? 1 : 0;
        
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
            url: "/EmployeeAppraisal/EditResult",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    $("#EmployeeAppraisalResultEdit_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    //location.reload(true);
                    var selectedId = $("#EmployeeAppraisalDetailId").val();
                   
                    getEmployeeAppraisalDetail(selectedId);
                    getEmployeeAppraisalList();
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
//End Result Section