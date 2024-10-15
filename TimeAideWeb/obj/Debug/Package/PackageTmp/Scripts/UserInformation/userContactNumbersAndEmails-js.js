$(document).ready(function () {
    //$('#HomeNumber').mask('(999) 999-9999', { autoclear: false }).on('blur', function () {
    //    ValidateNumbers('HomeNumber', 'Home');
    //});
    //$('#CelNumber').mask('(999) 999-9999', { autoclear: false }).on('blur', function () {
    //    ValidateNumbers('CelNumber', 'Mobile');
    //});
    //$('#FaxNumber').mask('(999) 999-9999', { autoclear: false }).on('blur', function () {
    //    ValidateNumbers('FaxNumber', 'Fax');
    //});
    //$('#OtherNumber').mask('(999) 999-9999', { autoclear: false }).on('blur', function () {
    //    ValidateNumbers('OtherNumber', 'Other')
    //});
    //$('#WorkNumber').mask('(999) 999-9999', { autoclear: false }).on('blur', function () {
    //    ValidateNumbers('WorkNumber', 'Work')
    //});
});

function btnSaveNumbersAndEmails() {
    //if (ValidateNumbersAndEmailsFormats() == false) {
    //    return false;
    //}
    var model = {

        UserInformationId: $('#UserInformationId').val(),
        Id: $('#UserContactInformationId').val(),
        HomeNumber: $('#HomeNumber').mask(),
        CelNumber: $('#CelNumber').mask(),
        FaxNumber: $('#FaxNumber').mask(),
        OtherNumber: $('#OtherNumber').mask(),
        WorkEmail: $('#WorkEmail').val(),
        PersonalEmail: $('#PersonalEmail').val(),
        WorkNumber: $('#WorkNumber').val(),
        WorkExtension: $('#WorkExtension').val(),
        OtherEmail: $('#OtherEmail').val(),

    }
    $.ajax({
        type: 'post',
        url: '/UserContactInformation/CreateNumbersAndEmails',
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        dataType: "html",
        success: function (html) {
            debugger
            //$('#divUserContactInformation').html(html);
            refreshContactInformation();
            $('#myModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            debugger
            var responseText = jQuery.parseJSON(xhr.responseText)
            var errorObject = jQuery.parseJSON(responseText.errors)
            $.each(errorObject, function (idx, errorMessage) {
                if (errorMessage.Key == 'MandatoryContact') {
                    toastr.error(errorMessage.Message, "Alert", { closeButton: true, positionClass: "toast-top-center", timeOut: 0, extendedTImeout: 0 });

                }
                else {
                    $('span[data-valmsg-for="' + errorMessage.Key + '"]').text(errorMessage.Message);
                }
            });
        }
    });
}

function btnSavePhoneNumbers() {
    debugger;
    //if (ValidateNumbersAndEmailsFormats() == false) {
    //    return false;
    //}
    var model = {

        UserInformationId: $('#UserInformationId').val(),
        Id: $('#UserContactInformationId').val(),
        HomeNumber: $('#HomeNumber').mask(),
        CelNumber: $('#CelNumber').mask(),
        FaxNumber: $('#FaxNumber').mask(),
        OtherNumber: $('#OtherNumber').mask(),      
        WorkNumber: $('#WorkNumber').mask(),
        WorkExtension: $('#WorkExtension').val(),
     

    }
    $(".loading").show();
    $.ajax({
        type: 'post',
        url: '/UserContactInformation/CreatePhoneNumbers',
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        dataType: "html",
        success: function (html) {
            debugger
            //$('#divUserContactInformation').html(html);
            refreshContactInformation();
            $(".loading").hide();
            $('#myModal').modal('hide');
            toastr.success('Change request submitted successfully.', "Success!");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            debugger
            $(".loading").hide();
            var responseText = jQuery.parseJSON(xhr.responseText)
            var errorObject = jQuery.parseJSON(responseText.errors)
            $.each(errorObject, function (idx, errorMessage) {
                if (errorMessage.Key == 'MandatoryContact') {
                    toastr.error(errorMessage.Message, "Alert", { closeButton: true, positionClass: "toast-top-center", timeOut: 0, extendedTImeout: 0 });

                }
                else {
                    $('span[data-valmsg-for="' + errorMessage.Key + '"]').text(errorMessage.Message);
                }
            });
        }
    });
}
function btnSaveEmails() {
    //if (ValidateNumbersAndEmailsFormats() == false) {
    //    return false;
    //}
    var model = {

        UserInformationId: $('#UserInformationId').val(),
        Id: $('#UserContactInformationId').val(),
       
        WorkEmail: $('#WorkEmail').val(),
        PersonalEmail: $('#PersonalEmail').val(),
       
        OtherEmail: $('#OtherEmail').val(),

    }
    $(".loading").show();
    $.ajax({
        type: 'post',
        url: '/UserContactInformation/CreateEmails',
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        dataType: "html",
        success: function (html) {
            debugger
            //$('#divUserContactInformation').html(html);
            refreshContactInformation();
            $(".loading").hide();
            $('#myModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            debugger
            $(".loading").hide();
            var responseText = jQuery.parseJSON(xhr.responseText)
            var errorObject = jQuery.parseJSON(responseText.errors)
            $.each(errorObject, function (idx, errorMessage) {
                if (errorMessage.Key == 'MandatoryContact') {
                    $('#WorkEmail').val(model.WorkEmail);
                    $('#PersonalEmail').val(model.PersonalEmail);
                    $('#OtherEmail').val(model.OtherEmail);
                    toastr.error(errorMessage.Message, "Alert", { closeButton: true, positionClass: "toast-top-center", timeOut: 0, extendedTImeout: 0 });

                }
                else {
                    $('span[data-valmsg-for="' + errorMessage.Key + '"]').text(errorMessage.Message);
                }
            });
        }
    });
}


function ValidateNumbersAndEmailsFormats() {
    debugger
    var isSuccess = true;
    if (ValidateNumbers('HomeNumber', 'Home') == false) {
        isSuccess = false;
    }
    if (ValidateNumbers('CelNumber', 'Mobile') == false) {
        isSuccess = false;
    }
    if (ValidateNumbers('FaxNumber', 'Fax') == false) {
        isSuccess = false;
    }
    if (ValidateNumbers('OtherNumber', 'Other') == false) {
        isSuccess = false;
    }
    if (ValidateNumbers('WorkNumber', 'Work') == false) {
        isSuccess = false;
    }
    return isSuccess;
}

function ValidateNumbers(controlName, display) {
    if ($('#' + controlName + '').mask().length > 0 && $('#' + controlName + '').mask().length < 10) {
        $('span[data-valmsg-for="' + controlName + '"]').text('Invalid format.');
        return false;
    }
    else if ($('#' + controlName + '').mask().length == 0 || $('#' + controlName + '').mask().length == 10) {
        $('span[data-valmsg-for="' + controlName + '"]').text('');
        return true;
    }
}