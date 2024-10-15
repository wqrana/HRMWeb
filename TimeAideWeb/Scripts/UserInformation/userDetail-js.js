var deleteRecordData = null
var deleteRecordType = null
var deactivateWizardS1Title = null;
var deactivateWizardS2Title = null;
var activateWizardS1Title = null;
var activateWizardS2Title = null;
var activateWizardS3Title = null;
//Test Devops changes
//Salman
//Qasim
//WaqarQ
$(document).ready(function () {
    //$("#gen_SSNEnd").mask("9999");
    deactivateWizardS1Title = "Welcome to Deactive Employee Wizard";
    deactivateWizardS2Title = "Employee's Termination Information";

    activateWizardS1Title = "Welcome to activate Employee Wizard";
    activateWizardS2Title = "Enter Employee's Information";
    activateWizardS3Title = "Enter Employee's Access/Login detail";
    transferWizardS1Title = "Welcome to transfer Employee Wizard";
    transferWizardS2Title = "Enter transfer's Information";

    $('#gen_SSN').mask('999-99-9999', { autoclear: false });
    $('#VeteranStatusId').select2();
    $('#divLoadingUpload_userCV').hide();
    $("#showHeader").on("click", function () {
        var checkClass = $(this).hasClass("fa-chevron-down");
        if (checkClass) {
            $(this).removeClass("fa-chevron-down");
            $(this).addClass("fa-chevron-up");
        } else {
            $(this).removeClass("fa-chevron-up");
            $(this).addClass("fa-chevron-down");
        }
    });
    //Multi-select width adjustment
    $(".select2-container").css("width", "100%");

    $("#activeWizardS1").on("click", function () {
        debugger;
        console.log(this);
    });
    // $("#processing-spinner").hide(); 
    $("#btnSaveGeneralInfo").on("click", function () {
        debugger;
        saveGeneralInfo();
    });


    $("#btnDeactivateS1Next").click(function () {
        showDeactivateUserWizardStep("Step2");
    });
    $("#btnDeactivateS2Back").click(function () {
        showDeactivateUserWizardStep("Step1");
    });

    //Save Employee
    $('#btnSaveDeactivateEmployment').click(function () {
        debugger;
        alertID = "#deactivateEmploymentAlert";
        var datafieldsObject = getDeactivateEmployeeFieldsData();
        var isValidated = validateDeactivateEmployeeFields(datafieldsObject);
        if (isValidated) {
            // ajax call for saving data
            saveDeactivateEmployeeData(datafieldsObject);
        }

    });

    $("#btnContactInfoSave").on("click", function () {
        debugger;
        saveContactInfo();

    });
    $("#btnSaveEmergencyContact").on("click", function () {
        debugger;
        saveEmpergencyInfo();
    });
    $("#btnSaveAddress").on("click", function () {
        debugger;
        saveAddressesInfo();
    });


    //$("#deleteContact").on("click", function () {
    //    debugger;
    //    var deltr = $(this).closest("tr");
    //    deltr.remove();
    //});
    //$("#editContact").on("click", function () {
    //    debugger;
    //    var edittr = $(this).closest("tr");
    //    contactMedium = $(edittr).find('td:eq(1)').text().trim();
    //    contactType = $(edittr).find('td:eq(2)').text().trim();
    //    contactText = $(edittr).find('td:eq(3)').text().trim();
    //    $("#contactMedium").val(contactMedium);
    //    $("#contactType").val(contactType);
    //    $("#contactText").val(contactText);
    //    $("#contact_info_modal").modal("show");
    //});
    //Profile picture uploading & Deletion
    $("#userPictureUpload").change(function () {
        debugger;
        var file = $(this)[0].files[0];
        console.log(file.name);
        // alert("Picture is selected")
        showSelectedImage(this);
    });
    //on shown
    $("#upload_picture_modal").on('shown.bs.modal', function () {
        document.getElementById("userPictureUpload").value = null;
        var src = $('#profileMainPicture').attr("src");
        $('#userProfileImageEdit').attr("src", src);

    });
    //on hide
    $("#upload_picture_modal").on('hidden.bs.modal', function () {
        //need implemention if required 
    });
    //On upload Click

    $("#btnUploadPic").click(function () {
        uploadProfilePicture("U")
        //btnDeletePicture
    });

    $("#btnDeletePicture").click(function () {
        uploadProfilePicture("D")

    });
    //$("#btnUploadUserCV").click(function () {
    //    uploadUserCV();

    //});
    //End Uploading & deletion
});
function activateUser() {
    var empStatusId = $("#EmployeeStatusId").val();
    if (empStatusId == 1) {
        showAlertAutoHide("#userDetailAlert", "Error", "Can't initiate activation in current employee status");
        return;
    }

    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/Employment/Activate',
            data: { "id": id },

            success: function (data) {
                debugger;
                console.log(data);
                $("#activatePopupDiv").html(data);
                $("#ActivateEmployment_modal").modal("show");
                showActivateUserWizardStep("Step1");
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
//Employee Transfer
function transferUser() {
    var empStatusId = $("#EmployeeStatusId").val();
    if (empStatusId != 1) {
        showAlertAutoHide("#userDetailAlert", "Error", "Can't initiate employee transfer in current employee status");
        return;
    }

    var id = $('#userID').val();
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/Employment/EmployeeTransfer',
            data: { "id": id },

            success: function (data) {
                debugger;
                console.log(data);
                $("#activatePopupDiv").html(data);
                $("#EmployeeTransfer_modal").modal("show");
                showTransferUserWizardStep("Step1");
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function showDeactivateUserWizardStep(step) {
    switch (step) {
        case "Step1":
            $("#deactiveWizardS1").show();
            $("#deactivateWizardStepTitle span:nth-child(2)").text(deactivateWizardS1Title);
            $("#deactiveWizardS2").hide();
            break;
        case "Step2":
            $("#deactiveWizardS2").show();
            $("#deactivateWizardStepTitle span:nth-child(2)").text(deactivateWizardS2Title);
            $("#deactiveWizardS1").hide();
    }
}

function deactivateUser() {
    debugger;
    var id = $('#userID').val();
    var empStatusId = $("#EmployeeStatusId").val();

    if (empStatusId == 2) {
        validateHiringForClosing(id).then(function (data) {
            debugger;
            if (data.status == "Success") {
                showAlertAutoHide("#userDetailAlert", "Info", "Deactivation is initiated against inactive record");
                showDeactivationWizard();
            }
            else {
                showAlertAutoHide("#userDetailAlert", data.status, data.message);
            }
        }).catch(function (err) {
            // Run this when promise was rejected via reject()
            console.log(err)
        });
    }
    else if (empStatusId == 3) {
        showAlertAutoHide("#userDetailAlert", "Info", "Deactivation is re-initiated against Closed record");
        showDeactivationWizard();
    }
    else {
        showDeactivationWizard();
    }

}
function showDeactivationWizard() {

    //var empId = $('#employeeIdDiv').text().trim();   
    //var ShortFullName = $("#ShortFullName").val();
    //$("#DeactivateEmployment_modal").modal("show");
    //showDeactivateUserWizardStep("Step1");
    //$("#DeactivateEmpID").text(empId);
    //$("#DeactivateEmpName").text(ShortFullName);
    //$('#TerminationDate').val(null);
    //$('#ApprovedDate').val(null);
    //$('#TerminationTypeId').val(null);
    //$('#TerminationReasonId').val(null);
    //$('#TerminationEligibilityId').val(null);
    //$('#ApprovedById').val(null);
    //$('#TerminationNotes').val(null);
    //$('#LockAccountDA').val(null);
    var id = $('#userID').val();
    $.ajax({
        type: "get",
        url: '/Employment/Deactivate',
        data: { "id": id },

        success: function (data) {
            // debugger;
            //console.log(data);
            $("#activatePopupDiv").html(data);
            $("#DeactivateEmployment_modal").modal("show");
            showDeactivateUserWizardStep("Step1");

        },
        error: function () {
            // displayWarningMessage(data.ErrorMessage);
        }
    });
}
function validateHiringForClosing(userId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Employment/AjaxCheckForValidEmployment",
            type: "POST",
            data: {
                "userID": userId
            },
            dataType: "json",
            success: function (data) {
                debugger;
                resolve(data);
            }
            ,
            error: function (data) {
                reject(data)

            }
        });
    });
}
function getDeactivateEmployeeFieldsData() {
    //Get add employee form fields data and save in object
    debugger;
    var dataObj = new Object();
    dataObj.UserInformationId = $('#userID').val();
    dataObj.TerminationDate = $('#TerminationDate').val();
    dataObj.ApprovedDate = $('#ApprovedDate').val();
    dataObj.TerminationTypeId = $('#TerminationTypeId').val();
    dataObj.TerminationReasonId = $('#TerminationReasonId').val();
    dataObj.TerminationEligibilityId = $('#TerminationEligibilityId').val();
    dataObj.ApprovedById = $('#ApprovedById').val();
    dataObj.TerminationNotes = $('#TerminationNotes').val();
    dataObj.LockAccount = $('#LockAccountDA').is(":checked");
    dataObj.EmployeeStatusId = $('#EmployeeStatusIdDA').val();
    dataObj.IsExitInterview = $('#IsExitInterviewAE').is(":checked");
    dataObj.IsExitInterviewMarkDel = $('#IsExitInterviewDocMarkDeleted').val() == "0" ? false : true;
    var totalFiles = document.getElementById("uploadExitInterview").files.length;
    if (totalFiles > 0) {
        var file = document.getElementById("uploadExitInterview").files[0];
        dataObj.UploadedExitInterviewDocFile = file;
    }
    return dataObj;
}

function validateDeactivateEmployeeFields(dataObj) {
    //validate object data
    var isRequiredValidated = 0;
    var isValidated = true
        ;
    if (dataObj != null) {
        isRequiredValidated += dataObj.TerminationDate.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.TerminationTypeId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.TerminationReasonId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.TerminationEligibilityId.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.TerminationNotes.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.EmployeeStatusId.trim().length > 0 ? 1 : 0;
        if (isRequiredValidated != 6) {
            isValidated = false;
            var message = " Missing Required field(s)";
        }

    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);

    return isValidated;
}
function saveDeactivateEmployeeData(datafieldsObject) {
    debugger;
    var formData = new FormData();
    formData.append("DeactivationData", JSON.stringify(datafieldsObject));
    if (datafieldsObject.UploadedExitInterviewDocFile != undefined) {
        formData.append("ExitInterviewDocFile", datafieldsObject.UploadedExitInterviewDocFile);
    }
    $.ajax({
        type: "POST",
        url: "/Employment/Deactivate",
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            debugger;
            if (data.status == "Success") {
                var userStsCSS = data.userStatus.id == 2 ? "bg-inverse-warning" : "bg-inverse-danger";
                $("#userStatus").removeClass("bg-inverse-warning")
                    .removeClass("bg-inverse-success")
                    .addClass(userStsCSS);
                $("#userStatus").text(data.userStatus.name);
                $("#EmployeeStatusId").val(data.userStatus.id);
                $("#DeactivateEmployment_modal").modal("hide");
                showAlertAutoHide("#userDetailAlert", data.status, data.message);
                location.reload(true);
            }
            else {
                showAlertAutoHide("#deactivateEmploymentAlert", data.status, data.message);
            }
        }
        ,
        error: function (request, status, error) {
            displayErrorMessage('Error in deleting parent alert data');
            return false;
        }
    });
    //$.ajax({
    //    type: "POST",
    //    url: "/Employment/Deactivate",
    //    data: JSON.stringify(datafieldsObject),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        debugger;
    //        if (data.status == "Success") {
    //            $("#userStatus").removeClass("bg-inverse-warning")
    //                .removeClass("bg-inverse-success")
    //                .addClass("bg-inverse-danger");
    //            $("#userStatus").text(data.userStatus.name);
    //            $("#EmployeeStatusId").val(data.userStatus.id);
    //            $("#DeactivateEmployment_modal").modal("hide");
    //            showAlertAutoHide("#userDetailAlert", data.status, data.message);
    //            location.reload(true);
    //        }
    //        else {
    //            showAlertAutoHide("#deactivateEmploymentAlert", data.status, data.message);
    //        }
    //    }
    //    ,
    //    error: function (request, status, error) {
    //        displayErrorMessage('Error in deleting parent alert data');
    //        return false;
    //    }
    //});
}



function uploadProfilePicture(action) {

    debugger;
    var isProceed = true;
    var formData = new FormData();
    var id = $('#userID').val();
    var pictureSrc = $('#profileMainPicture').attr("src");

    formData.append("UserID", id);
    formData.append("Action", action);


    if (action == 'U') {
        var totalFiles = document.getElementById("userPictureUpload").files.length;
        if (totalFiles > 0) {
            var file = document.getElementById("userPictureUpload").files[0];
            formData.append("FileUpload", file);

        }
        else {
            isProceed = false;
            showAlertAutoHide("#uploadPicAlert", "Error", "Please choose the profile picture");
        }
    }
    else if (action == "D") {
        if (pictureSrc.indexOf("no-profile-image.jpg") > 0) {
            isProceed = false;
            showAlertAutoHide("#uploadPicAlert", "Error", "No picture is available to delete");
        }
    }
    if (isProceed) {
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/UserInformation/AjaxUploadUserPicture',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                debugger;
                if (data.status == "Success") {

                    $('#profileMainPicture').attr('src', data.picturePath + "?timestamp=" + new Date().getTime());
                    $('#upload_picture_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                }
                else {
                    showAlertDismissable("#uploadPicAlert", data.status, data.message);
                }


            },
            error: function (error) {

                showAlertDismissable("#uploadPicAlert", "Error", error);

            }
        });
    }
}

function uploadUserCV(droppedFiles) {

    debugger;
    var isProceed = true;
    var formData = new FormData();
    var id = $('#userID').val();

    formData.append("UserID", id);

    var forms = document.getElementsByClassName('needs-validation');

    Array.prototype.filter.call(forms, function (form) {

        form.classList.remove('is-uploading');

    });

    var totalFiles = document.getElementById("uploadUserCVFile").files.length;
    if (totalFiles > 0) {
        var file = document.getElementById("uploadUserCVFile").files[0];
        formData.append("FileUpload", file);

    }
    else if (droppedFiles) {
        Array.prototype.forEach.call(droppedFiles, function (eachFile) {
            formData.append("FileUpload", eachFile);
        });
    }
    else {
        isProceed = false;
      
        showAlertAutoHide("#uploadCVAlert", "Error", "Please choose the User CV");
    }

    if (isProceed) {
        $('#divLoadingUpload_userCV').show();
        //Ajax call for uploading & deletion
        $.ajax({
            type: "POST",
            url: '/UserInformation/AjaxUploadUserCV',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                debugger;
                $('#divLoadingUpload_userCV').hide();
                if (data.status == "Success") {
                    $('#CVNameDiv').text(data.cvName);
                    $('#upload_userCV_modal').modal('hide');
                    showAlertAutoHide("#userDetailAlert", data.status, data.message);
                }
                else {
                    showAlertAutoHide("#uploadCVAlert", data.status, data.message);
                }
            },
            error: function (error) {
                $('#divLoadingUpload_userCV').hide();
                showAlertAutoHide("#uploadCVAlert", "Error", error);

            }
        });
    }
    
}
function downloadUserCV() {
    debugger;
    var id = $('#userID').val();
    $.ajax({
        url: "/UserInformation/AjaxCheckUserCV",
        type: "POST",
        data: {
            "userID": id
        },
        dataType: "json",
        success: function (data) {
            debugger;
            if (data.status == "Success") {
                //$("#processing-spinner").hide();
                window.location.href = "/UserInformation/DownloadUserCV/" + id;
            }
            else {
                showAlertAutoHide('#userDetailAlert', data.status, data.message);
            }
        }
        ,
        error: function (data) {
            showAlertDismissable("#userDetailAlert", "Error", data);

        }
    });

}

function showSelectedImage(input) {
    debugger;
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#userProfileImageEdit').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function deleteContact(e) {
    debugger;
    var deltr = $(e).closest("tr");
    //deltr.remove();
    deleteRecordData = deltr;
    deleteRecordType = "Contact Info";
    $("#deleteRecordTitle").text(deleteRecordType);
    $("#deleteConfirmationPopup").show();
}

function deleteRecord() {
    debugger;
    if (deleteRecordData != null) {
        deleteRecordData.remove();
        deleteRecordData = null;
        deleteRecordType = null;
        $("#deleteConfirmationPopup").hide();
    }

}
function cancelDelete() {
    $("#deleteConfirmationPopup").hide();
    deleteRecordData = null;
    deleteRecordType = null;
}

function editContact(id) {
    debugger;
    // var edittr = $(e).closest("tr");
    var tblObj = $("#tblContactInfoDataTable").DataTable();
    var currentDataRows = tblObj.rows().data();
    var dataRow = $.grep(currentDataRows, function (row) {
        return row.ContactId == id;
    });
    if (dataRow.length == 1) {

        //contactMedium = $(edittr).find('td:eq(1)').text().trim();
        //contactType = $(edittr).find('td:eq(2)').text().trim();
        //contactText = $(edittr).find('td:eq(3)').text().trim();

        $("#ContactMediumId").val(dataRow.ContactMediumId);
        $("#ContactTypeId").val(dataRow.ContactTypeId);
        $("#contactText").val(dataRow.Contact);
        $("#contact_info_modal").modal("show");
    }
}


function contactInfoTabClicked() {

    // alert("contact clicked");
    $("#tblContactInfoDataTable").dataTable({
        "paging": false,
        "searching": false,
        "sorting": false,
        "bInfo": false,
        "bDestroy": true,
        "bAutoWidth": false,

        "ajax": {
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/ContactInformation/GetUserContactList",
            data: function (d) {
                d.userID = $('#userID').val();
                return JSON.stringify(d);
            }
        },
        "aoColumns": [

            {
                "sTitle": "Action", "mData": null, "sClass": "center", "mRender":
                    function (data, type, row) {
                        debugger;

                        var tdContent =

                            '<div class="dropdown dropdown-action">' +
                            '<a href="#" class="action-icon dropdown-toggle" data-toggle="dropdown" aria-expanded="false"><i class="material-icons">more_vert</i></a>' +
                            '<div class="dropdown-menu dropdown-menu-right" x-placement="bottom-end" style="position: absolute; will-change: transform; top: 0px; left: 0px; transform: translate3d(38px, 32px, 0px);">' +
                            '<a id="editContact" href="javascript:editContact(' + row.ContactId + ');" class="dropdown-item"><i class="fa fa-pencil m-r-5"></i> Edit</a>' +
                            '<a id="deleteContact" href="javascript:deleteContact(' + row.ContactId + ');" class="dropdown-item"><i class="fa fa-trash-o m-r-5"></i> Delete</a>' +
                            '</div>' +
                            '</div>';


                        return tdContent;
                    }
            },
            { "sTitle": "Contact Meduim", "mData": "ContactMediumName", "sClass": "center" },
            { "sTitle": "Contact Type", "mData": "ContactTypeName", "sClass": "center" },
            { "sTitle": "Contact", "mData": "Contact", "sClass": "center" },


        ],
        "language": {
            "loadingRecords": "Loading...",
            "zeroRecords": "No record(s) available",
            "infoEmpty": "No record(s) available",

        },
        "order": [[0, "asc"]]
    });
}
function addressesInfoTabClicked() {

}
function hiringInfoTabClicked() {

}

function empergencyInfoTabClicked() {

}

function PayInformationHistoryTabClicked() {

}
//function for saving tabs infomation by using ajax call
//save GeneralInfo
function editGeneralInfo() {
    debugger;
    var employeeId = $('#employeeIdDiv').text().trim();
    var firstName = $('#firstNameDiv').text().trim();
    var middleInitial = $('#middleInitialDiv').text().trim();
    var firstLastName = $('#firstLastNameDiv').text().trim();
    var secondLastName = $('#secondLastNameDiv').text().trim();
    var shortFullName = $('#ShortFullName').val();
    var birthDate = $('#dobDiv').text().trim();
    var birthPlace = $("#BirthPlaceDiv").text().trim();
    //var sSNEnd = $('#SSNEndDiv').text().trim();
    //var sSN = $('#hdnSSNDe').val().trim();
    var sSNMasked = $('#hdnSSNMasked').val().trim();
    var genderId = $('#genderIdDiv').val().trim();
    var maritalStatusId = $('#maritalStatusIdDiv').val().trim();
    var ethnicityId = $('#ethnicityIdDiv').val().trim();
    var disabilityId = $('#disabilityIdDiv').val().trim();
    var VeteranStatusIds = $('#VeteranStatusIdDiv').val().trim().split(',');
    // var isSSNEndDisabled = (sSNEnd == "" || sSNEnd == null) ? true : false;    
    $("#Gen_employeeId").val(employeeId);
    $('#Gen_firstName').val(firstName);
    $('#Gen_middleName').val(middleInitial);
    $('#gen_firstLastName').val(firstLastName);
    $('#gen_secondLastName').val(secondLastName);
    $('#gen_shortFullName').val(shortFullName);

    $('#gen_birthDate').val(birthDate);
    $('#gen_birthPlace').val(birthPlace);
    // $('#gen_SSNEnd').val(sSNEnd);
    // $("#gen_SSNEnd").prop('disabled', isSSNEndDisabled);       
    $('#gen_SSN').val(sSNMasked);

    $('#GenderId').val(genderId);
    $('#MaritalStatusId').val(maritalStatusId);
    $('#DisabilityId').val(disabilityId);
    $('#EthnicityId').val(ethnicityId);
    $("#VeteranStatusId").val(VeteranStatusIds);
    $('#VeteranStatusId').select2();
    //Multi-select width adjustment
    $(".select2-container").css("width", "100%");

    $("#general_info_modal").modal("show");
}

function saveGeneralInfo() {
    debugger;
    // $("#processing-spinner").show();
    debugger;
    var showSSNLastFourDig = "";
    var userId = $('#userID').val();
    var requiredfields = 0;
    //var sSNEndCurrent = $('#SSNEndDiv').text().trim();
    //var SSNEnd = $('#gen_SSNEnd').val();

    //if (sSNEndCurrent != "" && SSNEnd == "") {
    //    showAlertAutoHide('#GenInfoEditAlert', "Error", "Please Enter the valid SSN End");
    //    return;
    //}
    //Get & Set updated values from modal popup
    var employeeId = $("#Gen_employeeId").val();
    requiredfields += employeeId.trim().length > 0 ? 1 : 0;
    var firstName = $('#Gen_firstName').val();
    requiredfields += firstName.trim().length > 0 ? 1 : 0;
    var middleInitial = $('#Gen_middleName').val();
    var firstLastName = $('#gen_firstLastName').val();
    requiredfields += firstLastName.trim().length > 0 ? 1 : 0;
    var secondLastName = $('#gen_secondLastName').val();
    //requiredfields += secondLastName.trim().length > 0 ? 1 : 0;
    var shortFullName = $('#gen_shortFullName').val();
    requiredfields += shortFullName.trim().length > 0 ? 1 : 0;
    var birthDate = $('#gen_birthDate').val();
    var birthPlace = $('#gen_birthPlace').val();
    //var ssn = $('#gen_SSN').mask();
    //var ssnmasked = $('#gen_SSN').mask();
    var ssn = $('#gen_SSN').mask();
    var ssnmasked = $('#gen_SSN').val();
    if (ssn.length == 0) {
        debugger
        ssn = ssnmasked.replace("-", "").replace("-", "");
    }

    var genderId = $('#GenderId').val();
    var genderText = $('#GenderId option:selected').text();
    var maritalStatusId = $('#MaritalStatusId').val();
    var maritalStatusText = $('#MaritalStatusId option:selected').text();
    var ethnicityId = $('#EthnicityId').val();
    var ethnicityText = $('#EthnicityId option:selected').text();
    var disabilityId = $('#DisabilityId').val();
    var disabilityText = $('#DisabilityId option:selected').text();

    if (requiredfields != 4) {
        showAlertAutoHide('#GenInfoEditAlert', "Error", " Missing Required* field(s)");
        return;
    }

    var selectedVeteranIdList = [];
    var selectedVeteranTextList = [];
    var selectedVeteranIdStr = "";
    var selectedVeteranTextStr = "";

    $('#VeteranStatusId option:selected').each(function () {
        selectedVeteranIdList.push($(this).val());
        selectedVeteranTextList.push($(this).text());
    });

    if (selectedVeteranIdList.length > 0) {
        selectedVeteranIdStr = selectedVeteranIdList.join(",");
        selectedVeteranTextStr = selectedVeteranTextList.join(",");
    }

    $.ajax({
        url: "/UserInformation/AjaxUpdateGenInfo",
        type: "POST",
        data: {
            "Id": userId,
            "EmployeeId": employeeId,
            "FirstName": firstName,
            "MiddleInitial": middleInitial,
            "FirstLastName": firstLastName,
            "SecondLastName": secondLastName,
            "ShortFullName": shortFullName,
            "GenderId": genderId,
            "BirthDate": birthDate,
            "BirthPlace": birthPlace,
            "MaritalStatusId": maritalStatusId,
            //  "SSNEnd": SSNEnd,
            "EthnicityId": ethnicityId,
            "DisabilityId": disabilityId,
            "SelectedVeteranStatus": selectedVeteranIdStr,
            "SSN": ssn
        },
        dataType: "json",
        success: function (data) {
            debugger;
            showAlertAutoHide('#userDetailAlert', data.status, data.message);
            if (data.status == "Success") {
                //$("#processing-spinner").hide();
                $('#ShortFullName').val(shortFullName);
                $('#ShortFullNameSpan').text(shortFullName);
                $('#mainEmployeeIdSpan').text(employeeId);
                $('#employeeIdDiv').text(employeeId);
                $('#firstNameDiv').text(firstName);
                $('#middleInitialDiv').text(middleInitial);
                $('#firstLastNameDiv').text(firstLastName);
                $('#secondLastNameDiv').text(secondLastName);
                $('#dobDiv').text(birthDate);
                $('#BirthPlaceDiv').text(birthPlace);
                //$('#SSNEndDiv').text(SSNEnd);
                if (ssnmasked.length == 11) {
                    showSSNLastFourDig = "***-**-" + ssnmasked.substr(7, 4);
                }
                $('#SSNDiv').text(showSSNLastFourDig);
                $("#hdnSSNMasked").val(ssnmasked);
                $('#genderIdDiv').val(genderId);
                $('#genderTextDiv').text(genderText);
                $('#maritalStatusIdDiv').val(maritalStatusId);
                $('#martialStatusTextDiv').text(maritalStatusText);
                $('#ethnicityIdDiv').val(ethnicityId);
                $('#ethnicityTextDiv').text(ethnicityText);

                $('#disabilityIdDiv').val(disabilityId);
                $('#disabilityTextDiv').text(disabilityText);

                $('#VeteranStatusIdDiv').val(selectedVeteranIdStr);
                $('#VeteranStatusTextDiv').text(selectedVeteranTextStr);

                $("#general_info_modal").modal("hide");
            }
            else {
                showAlertAutoHide('#GenInfoEditAlert', data.status, data.message);
            }
        }
        ,
        error: function (data) {
            showAlertDismissable("#GenInfoEditAlert", "Error", data);

        }
    });

}
// save 
function saveContactInfo() {
    debugger;
    var contactMedium = $("#contactMedium").val();
    var contactType = $("#contactType").val();
    var contactText = $("#contactText").val();
    $("#contactMedium").val("");
    $("#contactType").val("");
    $("#contactText").val("");
    var row = $('#tblContactInfo').closest('table').find('tbody tr:eq(0)');
    var clonerow = row.clone();
    $(clonerow).find('td:eq(1)').text(contactMedium);
    $(clonerow).find('td:eq(2)').text(contactType);
    $(clonerow).find('td:eq(3)').text(contactText);
    $('#tblContactInfo tr:last').after(clonerow);
    $("#contact_info_modal").modal("hide");

}

function saveEmpergencyInfo() {
    debugger;
    var EmerContPerson = $("#EmerContPerson").val();
    var EmerContRelationship = $("#EmerContRelationship").val();
    var EmerContPrimary = $("#EmerContPrimary").val();
    var EmerMainNumber = $("#EmerMainNumber").val();
    var EmerMainAlt = $("#EmerMainAlt").val();
    $("#EmerContPerson").val("");
    $("#EmerContRelationship").val("");
    $("#EmerContPrimary").val("");
    $("#EmerMainNumber").val("");
    $("#EmerMainAlt").val("");

    var row = $('#tblEmergencyInfo').closest('table').find('tbody tr:eq(1)');
    var clonerow = row.clone();
    $(clonerow).find('td:eq(1)').text(EmerContPerson);
    $(clonerow).find('td:eq(2)').text(EmerContRelationship);
    $(clonerow).find('td:eq(3)').text(EmerContPrimary);
    $(clonerow).find('td:eq(4)').text(EmerMainNumber);
    $(clonerow).find('td:eq(5)').text(EmerMainAlt);

    $('#tblEmergencyInfo tr:last').after(clonerow);
    $("#emergency_contact_modal").modal("hide");


}

function saveAddressesInfo() {
    debugger;
    var addressType = $("#addressType").val();
    var address1 = $("#address1").val();
    var address2 = $("#address2").val();
    var addressCity = $("#addressCity").val();
    var addressZipCode = $("#addressZipCode").val();
    $("#addressType").val("");
    $("#address1").val("");
    $("#address2").val("");
    $("#addressCity").val("");
    $("#addressZipCode").val("");

    var row = $('#tblAddresses').closest('table').find('tbody tr:eq(0)');
    var clonerow = row.clone();
    $(clonerow).find('td:eq(1)').text(addressType);
    $(clonerow).find('td:eq(2)').text(address1);
    $(clonerow).find('td:eq(3)').text(address2);
    $(clonerow).find('td:eq(4)').text(addressCity);
    $(clonerow).find('td:eq(5)').text(addressZipCode);

    $('#tblAddresses tr:last').after(clonerow);
    $("#addresses_info_modal").modal("hide");
}
