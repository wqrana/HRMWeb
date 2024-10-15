$(document).ready(function () {
    $('#tblClient').dataTable({
        "searching": true
    });
    $("#divValidatingState").hide();

    $('#CountryId').change(function (e) {
        debugger;
        var countyId = $(this).val();
        LoadStateDropdown(countyId, '#StateId', null);
        LoadCityDropdown(null, '#CityId', null);
    })
    $('#PayrollCountryId').change(function (e) {
        debugger;
        var countyId = $(this).val();
        LoadStateDropdown(countyId, '#PayrollStateId', null);
        LoadCityDropdown(null, '#PayrollCityId', null);
    })

    $('#StateId').change(function (e) {
        debugger;
        var stateId = $(this).val();
        LoadCityDropdown(stateId, '#CityId', null);
    })
    $('#PayrollStateId').change(function (e) {
        debugger;
        var stateId = $(this).val();
        LoadCityDropdown(stateId, '#PayrollCityId', null);
    })

    $('#btnSaveClient').click(function () {
        saveClient();
    });

    $("#IsTimeAideWindowAE").change(function () {
        var sts = $(this).is(":checked");
        if (sts) {
            $("#divTimeAideWindow").removeClass("hideSection").addClass("showSection");
        }
        else {
            $("#divTimeAideWindow").removeClass("showSection").addClass("hideSection");
        }
    });
});

function LoadStateDropdown(contryId, targetElement, selectedValue) {

    $.ajax({
        url: '/State/AjaxGetCountryState',
        data: { 'countryId': contryId }, //dataString,
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
function LoadCityDropdown(stateId, targetElement,selectedValue) {
    $.ajax({
        url: '/City/AjaxGetStateCity',
        data: { 'stateId': stateId }, //dataString,
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
function CreateOrEdit(e) {
    debugger;
    // Get the id from the link
    var id = $(e).attr("data-id");
     
    if (id != '') {

        $.ajax({
            type: "get",
            url: '/Client/CreateEdit',
            data: { "id": id },
            dataType: "json",
            success: function (data) {
                debugger;
                console.log(data);
                setClientPopDetail(data);
            },
            error: function () {
               // displayWarningMessage(data.ErrorMessage);
            }
        });
    }    
}
function ClientDetails(id) {
    debugger;
    // Get the id from the link
   // var id = $(e).attr("data-id");

    if (id != '') {

        $.ajax({
            type: "get",
            url: '/Client/Details',
            data: { "id": id },
          
            success: function (data) {
                debugger;
                console.log(data);
                $("#clientPopupView").html(data);
                $("#DetailPayroll_cmp_modal").modal("show");
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function ClientDelete(id) {
    debugger;
    // Get the id from the link
    // var id = $(e).attr("data-id");

    if (id != '') {

        $.ajax({
            type: "get",
            url: '/Client/Delete',
            data: { "id": id },

            success: function (data) {
                debugger;
                console.log(data);
                $("#clientPopupView").html(data);
                $("#DeletePayroll_cmp_modal").modal("show");
                //setClientPopDetail(data);
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
    }
}
function setClientPopDetail(data) {
    debugger;
    $('#Id').val(data.Id);
    $('#ClientName').val(data.ClientName);
    $('#PayrollName').val(data.PayrollName);
    $('#CompanyStartDate').val(ToJavaScriptDate(data.CompanyStartDate));
    $('#Address1').val(data.Address1);
    $('#Address2').val(data.Address2);
    $('#ZipCode').val(data.ZipCode);
    //
    $('#CountryId').val(data.CountryId);
   // if (data.CountryId != null)
        LoadStateDropdown(data.CountryId, '#StateId', data.StateId);
    //else 
    //    $('#StateId').val(data.StateId);
   // if (data.StateId != null)
        LoadCityDropdown(data.StateId, '#CityId', data.CityId);
   // else
  //  $('#CityId').val(data.CityId);
    //
    $('#Phone').val(data.Phone);
    $('#Fax').val(data.Fax);
    $('#Email').val(data.Email);
    $('#WebSite').val(data.WebSite);
    $('#ContactName').val(data.ContactName);

    $('#PayrollAddress1').val(data.PayrollAddress1);
    $('#PayrollAddress2').val(data.PayrollAddress2);
    $('#PayrollZipCode').val(data.PayrollZipCode);

    $('#PayrollCountryId').val(data.PayrollCountryId);
    //$('#PayrollStateId').val(data.PayrollStateId);
    //$('#PayrollCityId').val(data.PayrollCityId);
    //if (data.PayrollCountryId != null)
        LoadStateDropdown(data.PayrollCountryId, '#PayrollStateId', data.PayrollStateId);
    //else
    //    $('#PayrollStateId').val(data.PayrollStateId);
    //if (data.PayrollStateId != null)
        LoadCityDropdown(data.PayrollStateId, '#PayrollCityId', data.PayrollCityId);
    //else
    //    $('#PayrollCityId').val(data.PayrollCityId);

    $('#PayrollContactPhone').val(data.PayrollContactPhone);
    $('#PayrollFax').val(data.PayrollFax);
    $('#PayrollEmail').val(data.PayrollEmail);
    $('#PayrollContactTitle').val(data.PayrollEmail);
    $('#PayrollContactName').val(data.PayrollContactName);
    $('#EIN').val(data.EIN);
    $('#SICCode').val(data.SICCode);
    $('#NAICSCode').val(data.NAICSCode);
    $('#SeguroChoferilAccount').val(data.SeguroChoferilAccount);
    $('#DepartamentoDelTrabajoAccount').val(data.DepartamentoDelTrabajoAccount);
    $('#DepartamentoDelTrabajoRate').val(data.DepartamentoDelTrabajoRate);
    $('#IsTimeAideWindowAE').prop('checked', data.IsTimeAideWindow);
    if (data.IsTimeAideWindow) {
        $("#divTimeAideWindow").removeClass("hideSection").addClass("showSection");
    }
    else {
        $("#divTimeAideWindow").removeClass("showSection").addClass("hideSection");
    }
    $('#DBServerName').val(data.DBServerName);
    $('#DBName').val(data.DBName);
    $('#DBUser').val(data.DBUser);
    $('#DBPassword').val(data.DBPassword);

    $('#CreatedBy').val(data.CreatedBy);
    $('#CreatedDate').val(ToJavaScriptDate(data.CreatedDate));
}

function ToJavaScriptDate(value) {
    debugger;
    if (value != null) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
    }
    return null;
}
function validateTAWindowConnection() {
    $("#divValidatingState").show();
    $("#btnValidateConnection").prop("disabled", true);
    
    $.ajax({
        type: "post",
        url: '/Client/ValidateTAWindowConnection',
        data:
        {
            DBServerName: $('#DBServerName').val(),
            DBName: $('#DBName').val(),
            DBUser: $('#DBUser').val(),
            DBPassword: $('#DBPassword').val(),          
        },
        dataType: "json",
        success: function (data) {
            debugger;
            $("#divValidatingState").hide();
            $("#btnValidateConnection").prop("disabled", false);
            showAlertAutoHide("", data.status, data.message);
        },
        error: function () {
            // displayWarningMessage(data.ErrorMessage);
        }
    });
}
function saveClient() {
    debugger;
    var isRequiredValidated = 0;
   
    var message = "";
    var clientName = $('#ClientName').val();
    var eIN = $('#EIN').val();
    var IsTimeAideWindow = $('#IsTimeAideWindowAE').is(':checked');
    var DBServerName = $('#DBServerName').val();
    var DBName = $('#DBName').val();
    var DBUser = $('#DBUser').val();
    var DBPassword = $('#DBPassword').val(); 
 
        isRequiredValidated += clientName.trim().length > 0 ? 1 : 0;
        isRequiredValidated += eIN.trim().length > 0 ? 1 : 0;
       
        if (isRequiredValidated != 2) {
            
            message = " Missing Required field(s) {Client Name/EIN}";
            showAlertAutoHide("#PayrollCreateEditAlert", 'Error', message);
            return;
    }
    if (IsTimeAideWindow == true) {
        isRequiredValidated = 0;
        isRequiredValidated += DBServerName.trim().length > 0 ? 1 : 0;
        isRequiredValidated += DBName.trim().length > 0 ? 1 : 0;
        isRequiredValidated += DBUser.trim().length > 0 ? 1 : 0;
        isRequiredValidated += DBPassword.trim().length > 0 ? 1 : 0;
        if (isRequiredValidated != 4) {

            message = "TimeAide-Window option is selected, Missing Required field(s) {Server Name/DB Name/User/Password}";
            showAlertAutoHide("", 'Error', message);
            return;
        }
    } 

    $.ajax({
        type: "post",
        url: '/Client/CreateEdit',
        data: {
            Id: $('#Id').val(),
            ClientName          : $('#ClientName').val(),
            PayrollName         : $('#PayrollName').val(),
            CompanyStartDate    : $('#CompanyStartDate').val(),
            Address1            : $('#Address1').val(),
            Address2            : $('#Address2').val(),
            ZipCode             : $('#ZipCode').val(),
            //
            CountryId           : $('#CountryId').val(),
            StateId             : $('#StateId').val(),
            CityId              : $('#CityId').val(),
            //
            Phone               : $('#Phone').val(),
            Fax                 : $('#Fax').val(),
            Email               : $('#Email').val(),
            WebSite             : $('#WebSite').val(),
            ContactName         : $('#ContactName').val(),

            PayrollAddress1     : $('#PayrollAddress1').val(),
            PayrollAddress2     : $('#PayrollAddress2').val(),
            PayrollZipCode      : $('#PayrollZipCode').val(),

            PayrollCountryId    : $('#PayrollCountryId').val(),
            PayrollStateId      : $('#PayrollStateId').val(),
            PayrollCityId       : $('#PayrollCityId').val(),

            PayrollContactPhone : $('#PayrollContactPhone').val(),
            PayrollFax          : $('#PayrollFax').val(),
            PayrollEmail        : $('#PayrollEmail').val(),
            PayrollEmail        : $('#PayrollContactTitle').val(),
            PayrollContactName  : $('#PayrollContactName').val(),
            EIN                 : $('#EIN').val(),
            SICCode             : $('#SICCode').val(),
            NAICSCode           : $('#NAICSCode').val(),
            SeguroChoferilAccount:$('#SeguroChoferilAccount').val(),
            DepartamentoDelTrabajoAccount:$('#DepartamentoDelTrabajoAccount').val(),
            DepartamentoDelTrabajoRate: $('#DepartamentoDelTrabajoRate').val(),
            IsTimeAideWindow: $('#IsTimeAideWindowAE').is(':checked'),
            DBServerName: $('#DBServerName').val(),
            DBName: $('#DBName').val(),
            DBUser: $('#DBUser').val(),
            DBPassword: $('#DBPassword').val(),
            CreatedBy           : $('#CreatedBy').val(),
            CreatedDate         : $('#CreatedDate').val()

        },
        dataType: "json",
        success: function (data) {
            debugger;
            console.log(data);
            if (data.status == "Success") {
                window.location.reload(true);

            } else {

                showAlertDismissable("#PayrollCreateEditAlert", data.status, data.errorMessage);
            }
        },
        error: function () {
            // displayWarningMessage(data.ErrorMessage);
        }
    });

}
