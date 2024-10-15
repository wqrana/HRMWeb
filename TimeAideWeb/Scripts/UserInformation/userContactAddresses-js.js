$(document).ready(function () {
    //$('#HomeZipCode').mask('99999-9999', { autoclear: false }).on('blur', function () {
    //    ValidateZipCode('HomeZipCode', 'HomeZipCode');
    //});
    //$('#MailingZipCode').mask('99999-9999', { autoclear: false }).on('blur', function () {
    //    ValidateZipCode('MailingZipCode', 'MailingZipCode');
    //});
    //$('#ddlCountryId_Home').change(function (e) {
    //    debugger;
    //    var countyId = $(this).val();
    //    LoadStateDropdown(countyId, '#ddlStateId_Home', null);
    //    LoadCityDropdown(null, '#ddlCityId_Home', null);
    //})
    //$('#ddlStateId_Home').change(function (e) {
    //    debugger;
    //    var stateId = $(this).val();
    //    LoadCityDropdown(stateId, '#ddlCityId_Home', null);
    //})
    //$('#ddlCountryId_Mailing').change(function (e) {
    //    debugger;
    //    var countyId = $(this).val();
    //    LoadStateDropdown(countyId, '#ddlStateId_Mailing', null);
    //    LoadCityDropdown(null, '#ddlCityId_Mailing', null);
    //})
    //$('#ddlStateId_Mailing').change(function (e) {
    //    debugger;
    //    var stateId = $(this).val();
    //    LoadCityDropdown(stateId, '#ddlCityId_Mailing', null);
    //})
});
function IsSameHomeAddressChange() {
    debugger;
    if ($('#IsSameHomeAddress').is(":checked")) {
        var countyId = $('#ddlCountryId_Home').val();
        LoadStateDropdown1(countyId, '#ddlStateId_Mailing', null);
    }
    else {
        $('#MailingAddress1').val();
        $('#MailingAddress2').val();
        $('#ddlCityId_Mailing').val("");
        $('#ddlStateId_Mailing').val("");
        $('#ddlCountryId_Mailing').val("");
        $('#MailingZipCode').val("");
    }
}
function LoadStateDropdown(contryId, targetElement, selectedValue) {
    LoadDroDown('State', 'AjaxGetCountryState', 'countryId', contryId, targetElement, selectedValue);
}
function LoadCityDropdown(stateId, targetElement, selectedValue) {
    LoadDroDown('City', 'AjaxGetStateCity', 'stateId', stateId, targetElement, selectedValue);
}
function LoadStateDropdown1(contryId, targetElement, selectedValue) {
    LoadDroDown('State', 'AjaxGetCountryState', 'countryId', contryId, targetElement, selectedValue);
}
function LoadCityDropdown1(stateId, targetElement, selectedValue) {
    LoadDroDown('City', 'AjaxGetStateCity', 'stateId', stateId, targetElement, selectedValue);
}
function LoadDroDown(controller, action, parentField, parentId, targetElement, selectedValue) {
    var model = {
    }
    debugger
    model[parentField] = parentId;
    $.ajax({
        url: '/' + controller + '/' + action + '?' + parentField + '=' + parentId,
        //data: JSON.stringify(model), //dataString,
        dataType: 'json',
        type: 'GET',
        success: function (res) {
            debugger;
            var data = res;
            $(targetElement + ' option').remove();
            var option = '<option value=""> Please Select </option>';
            $(targetElement).append(option);
            $(data).each(function () {
                var option = '<option value=' + this.id + '>' + this.name + '</option>';
                $(targetElement).append(option);
            });
            // if (selectedValue != null)
            $(targetElement).val(selectedValue == null ? "" : selectedValue);
        },
        error: function (xhr, status, error) {
            //displayErrorMessage('Error during retrieving Data:' + error);
        }
    });
}
function btnSaveAddresses() {
    //if (ValidateNumbersAndEmailsFormats() == false) {
    //    return false;
    //}
    var model = {
        UserInformationId: $('#UserInformationId').val(),
        Id: $('#UserContactInformationId').val(),

        HomeAddress1: $('#HomeAddress1').val(),
        HomeAddress2: $('#HomeAddress2').val(),
        HomeCityId: $('#ddlCityId_Home').val(),
        HomeStateId: $('#ddlStateId_Home').val(),
        HomeCountryId: $('#ddlCountryId_Home').val(),
        HomeZipCode: $('#HomeZipCode').mask(),
        IsSameHomeAddress: $('#IsSameHomeAddress').is(":checked"),

        MailingAddress1: $('#MailingAddress1').val(),
        MailingAddress2: $('#MailingAddress2').val(),
        MailingCityId: $('#ddlCityId_Mailing').val(),
        MailingStateId: $('#ddlStateId_Mailing').val(),
        MailingCountryId: $('#ddlCountryId_Mailing').val(),
        MailingZipCode: $('#MailingZipCode').mask(),
    }
    $(".loading").show();
    $.ajax({
        type: 'post',
        url: '/UserContactInformation/CreateAddresses',
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        dataType: "html",
        success: function (html) {
            debugger
            $(".loading").hide();
            refreshContactInformation();
            $('#myModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            debugger
            $(".loading").hide();
            var responseText = jQuery.parseJSON(xhr.responseText)
            var errorObject = jQuery.parseJSON(responseText.errors)
            $.each(errorObject, function (idx, errorMessage) {
                $('span[data-valmsg-for="' + errorMessage.Key + '"]').text(errorMessage.Message);
            });
        }
    });
}


function btnSaveHomeAddress() {
    //if (ValidateNumbersAndEmailsFormats() == false) {
    //    return false;
    //}
    var model = {
        UserInformationId: $('#UserInformationId').val(),
        Id: $('#UserContactInformationId').val(),

        HomeAddress1: $('#HomeAddress1').val(),
        HomeAddress2: $('#HomeAddress2').val(),
        HomeCityId: $('#ddlCityId_Home').val(),
        HomeStateId: $('#ddlStateId_Home').val(),
        HomeCountryId: $('#ddlCountryId_Home').val(),
        HomeZipCode: $('#HomeZipCode').mask(),

    }
    $(".loading").show();
    $.ajax({
        type: 'post',
        url: '/UserContactInformation/CreateHomeAddress',
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        dataType: "html",
        success: function (html) {
            debugger
            $(".loading").hide();
            refreshContactInformation();
            $('#myModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            debugger
            $(".loading").hide();
            var responseText = jQuery.parseJSON(xhr.responseText)
            var errorObject = jQuery.parseJSON(responseText.errors)
            $.each(errorObject, function (idx, errorMessage) {
                $('span[data-valmsg-for="' + errorMessage.Key + '"]').text(errorMessage.Message);
            });
        }
    });
}

function btnSaveMailingAddress() {
    //if (ValidateNumbersAndEmailsFormats() == false) {
    //    return false;
    //}
    var model = {
        UserInformationId: $('#UserInformationId').val(),
        Id: $('#UserContactInformationId').val(),
        IsSameHomeAddress: $('#IsSameHomeAddress').is(":checked"),

        MailingAddress1: $('#MailingAddress1').val(),
        MailingAddress2: $('#MailingAddress2').val(),
        MailingCityId: $('#ddlCityId_Mailing').val(),
        MailingStateId: $('#ddlStateId_Mailing').val(),
        MailingCountryId: $('#ddlCountryId_Mailing').val(),
        MailingZipCode: $('#MailingZipCode').mask(),
    }
    $.ajax({
        type: 'post',
        url: '/UserContactInformation/CreateMailingAddress',
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        dataType: "html",
        success: function (html) {
            debugger
            refreshContactInformation();
            $('#myModal').modal('hide');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            debugger
            var responseText = jQuery.parseJSON(xhr.responseText)
            var errorObject = jQuery.parseJSON(responseText.errors)
            $.each(errorObject, function (idx, errorMessage) {
                $('span[data-valmsg-for="' + errorMessage.Key + '"]').text(errorMessage.Message);
            });
        }
    });
}
function ValidateZipCodeFormats() {
    debugger
    var isSuccess = true;
    if (ValidateZipCode('HomeZipCode', 'HomeZipCode') == false) {
        isSuccess = false;
    }
    if (ValidateZipCode('MailingZipCode', 'MailingZipCode') == false) {
        isSuccess = false;
    }
    return isSuccess;
}

function ValidateZipCode(controlName, display) {
    debugger
    if ($('#' + controlName + '').mask().length > 0 && $('#' + controlName + '').mask().length < 9) {
        if ($('#' + controlName + '').mask().length != 5)
            $('span[data-valmsg-for="' + controlName + '"]').text('Invalid format.');
        return false;
    }
    else if ($('#' + controlName + '').mask().length == 0 || $('#' + controlName + '').mask().length == 9) {
        $('span[data-valmsg-for="' + controlName + '"]').text('');
        return true;
    }
}
function ValidateZipCodeChangeRequest(controlName, display) {
    debugger
    if ($('#' + controlName + '').mask().length > 0 && $('#' + controlName + '').mask().length < 9) {
        if ($('#' + controlName + '').mask().length != 5)
            $('span[data-valmsg-for="' + controlName.replace("_", ".")+ '"]').text('Invalid format.');
        return false;
    }
    else if ($('#' + controlName + '').mask().length == 0 || $('#' + controlName + '').mask().length == 9) {
        $('span[data-valmsg-for="' + controlName.replace("_", ".") + '"]').text('');
        return true;
    }
}