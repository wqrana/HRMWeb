$(document).ready(function () {
    $("#divLoadingDocumentTab").hide();

});
function ApplicantInterviewTabClicked() {
    $("#applicantError_modal").parent().html("");
    retrieveApplicantInterviewTab();

}
function ajaxGetSelectedAnswerRating(id) {
    debugger;
    alertID = "#"
    if (id != '') {
        $.ajax({
            type: "get",
            url: "/ApplicantInterview/AjaxGetInterviewAnswerRating",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.status == "Success") {
                    // $("#divExpirationDate").modal("hide");
                    $("#InterviewAnswerValue").val(data.answerRating.AnwserValue);
                    $("#InterviewAnswerMaxValue").val(data.answerRating.AnswerMaxValue);
                    $("#spanInterviewAnswerMaxValue").text(data.answerRating.AnswerMaxValue);
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
function createEditApplicantInterviewQA(interviewQAId) {
    debugger;

    var id = interviewQAId == null ? $('#ApplicantInformationId').val() : interviewQAId;
    var ActionMethod = interviewQAId == null ? "Create" : "Edit";
    if (id != 0) {
        $.ajax({
            type: "get",
            url: '/ApplicantInterview/' + ActionMethod,
            data: { "id": id },

            success: function (data) {
                debugger;
                // console.log(data);
                $("#divApplicantInterviewCreateEdit").html(data);
                $("#applicantInterviewCreateEdit_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }

}

function retrieveApplicantInterviewTab() {
    $("#divLoadingInterviewTab").show();
    getApplicantInterviewList();
}
function getApplicantInterviewList() {
    //debugger;
    var id = $('#ApplicantInformationId').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantInterview/Index',
            data: { "id": id },

            success: function (data) {
                //debugger;
                // console.log(data);
                $("#divApplicantInterviewIndex").html(data);
                $("#divLoadingInterviewTab").hide();
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}


function saveApplicantInterviewQAData() {
    //debugger;
    alertID = "#";
    var isRequiredValidated = 0;
    var isValidated = true;
    var message = "";
    var dataObj = new Object();
    dataObj.ApplicantInformationId = $('#ApplicantInformationId').val();
    dataObj.id = $('#ApplicantInterviewId').val();
    dataObj.ApplicantAnswer = $('#ApplicantAnswer').val();
    dataObj.ApplicantInterviewQuestionId = $('#ApplicantInterviewQuestionId').val();
    dataObj.ApplicantInterviewAnswerId = $('#ApplicantInterviewAnswerId').val();
    dataObj.InterviewAnswerValue = $('#InterviewAnswerValue').val();
    dataObj.InterviewAnswerMaxValue = $('#InterviewAnswerMaxValue').val();
    dataObj.Note = $('#Note').val();

    if (dataObj != null) {
        isRequiredValidated += dataObj.ApplicantInterviewQuestionId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.ApplicantInterviewAnswerId.trim().length > 0 ? 1 : 0;

        if ( isRequiredValidated != 2) {
            isValidated = false;
            message = " Missing Required field(s)";
        }

    }
    if (!isValidated) {
        showAlertAutoHide(alertID, 'Error', message);
        return;
    }

    if (isValidated) {
        // ajax call for saving data
        $.ajax({
            type: "POST",
            url: "/ApplicantInterview/CreateEdit",
            data: JSON.stringify(dataObj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantInterviewCreateEdit_modal").modal("hide");
                    showAlertAutoHide("", data.status, data.message);
                    retrieveApplicantInterviewTab();
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



function getApplicantInterviewQAData(id) {
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/ApplicantInterview/Delete',
            data: { "id": id },

            success: function (data) {
                //debugger;
                $("#divApplicantInterviewCreateEdit").html(data);
                $("#applicantInterviewDelete_modal").modal("show");

            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}


function deleteApplicantInterviewQA(id) {
    alertID = "#"
    if (id != '') {
        $.ajax({
            type: "Post",
            url: "/ApplicantInterview/ConfirmDelete",
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                //debugger;
                if (data.status == "Success") {
                    $("#applicantInterviewDelete_modal").modal("hide");
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                    retrieveApplicantInterviewTab();
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
