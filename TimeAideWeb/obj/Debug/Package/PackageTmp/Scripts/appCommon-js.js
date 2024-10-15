//Common JS file for whole application
var toastMsgOptions = {
    "closeButton": true,
    "debug": false,
    "positionClass": "toast-top-center",
    "onclick": null,
    "showDuration": "1000",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut",
    "preventDuplicates": true
}

var masterDataScrDdl = null;
var masterDataController = null;
$(document).ready(function () {
    // Bootstrap date-picker for datetime field
    $('input[id*=Date]').each(function () {
        $(this).datetimepicker({ format: 'MM/DD/YYYY' });

    });
    $(".searchableDDL").each(function () {
        $(this).select2();
        $(".select2-container").css("width", "100%");
    });
    var indexColumns = $("#tblIndexList > thead > tr:first > th").length;
    //alert(indexColumns);
    var exportColumns = "[";
    for (let i = 0; i < indexColumns-1; i++) {
        exportColumns += i + ","
    }
    if (indexColumns > 0) {
        exportColumns = exportColumns.slice(0, -1)
    }
    exportColumns += "]";
    //alert(exportColumns);

   // debugger
    $('#tblIndexList').DataTable({
        searching: true,
        "order": [],
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copyHtml5',
                text: '<i class="fa fa-files-o"></i>',
                className: 'btn-primary',
                titleAttr: 'Copy',
                exportOptions: {
                    columns: jQuery.parseJSON(exportColumns)
                }
            },
            {
                extend: 'excelHtml5',
                text: '<i class="fa fa-file-excel-o"></i>',
                className: 'btn-primary',
                titleAttr: 'Excel',
                exportOptions: {
                    columns: jQuery.parseJSON(exportColumns)
                }
            },
            {
                extend: 'csvHtml5',
                text: '<i class="fa fa-file-text-o"></i>',
                className: 'btn-primary',
                titleAttr: 'CSV',
                exportOptions: {
                    columns: jQuery.parseJSON(exportColumns)
                }
            },
            {
                extend: 'pdfHtml5',
                text: '<i class="fa fa-file-pdf-o"></i>',
                className: 'btn-primary',
                titleAttr: 'PDF',
                exportOptions: {
                    columns: jQuery.parseJSON(exportColumns)
                }
            },
            {
                extend: 'pageLength',
                className: 'btn-primary',
            },
        ],
    });
    $(".dt-button-collection>div").removeClass("dropdown-menu");
    $('#alertDismiss').on('click', function () {
       // debugger;
        $(this).parent().hide();
    })
    //Overcome the issue of multiple modal, if one is closed then overcome the scroll issue,
    // uncomment below
    $(document)
        .on('hidden.bs.modal', '.modal', function () {
          //  debugger;
            /**
             * Check if there are still modals open
             * Body still needs class modal-open
             */
            if ($('body').find('.modal.show').length) {
                $('body').addClass('modal-open');
            }
        });
});
//Open popup For Master Data
function openMasterDataPopUp(srcDdl) {
    //debugger;
    if (srcDdl != null) {
        var id = $(srcDdl)[0].id;
        masterDataScrDdl = srcDdl;
        masterDataController = id.substring(0, id.length - 2);

        $.ajax({
            type: "get",
            url: '/' + masterDataController + '/CreateAdhocMasterData',
            success: function (data) {
                //debugger;
                //console.log(data);
                // $("#divDdlPopup").html(data);
                var options = { "backdrop": "static", keyboard: true };
                //$('#createMasterDataPopUp_modal').modal(options);
                //$("#createMasterDataPopUp_modal").modal("show");
                $('#divDdlPopupContent').html(data);
                $('#divDdlPopup').modal(options);
                $('#divDdlPopup').modal('show');


            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });

    }
}
//Save Master Data Popup Record
function saveMasterDataPopUp(masterDataObject) {
    //debugger;
    $.ajax({
        type: "POST",
        url: '/' + masterDataController + '/CreateAdhocMasterData',
        //data: JSON.stringify(dataObject),
        data: masterDataObject,
        // contentType: "application/json; charset=utf-8",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            //debugger;
            if (data.status == "Success") {

                $("#divDdlPopup").modal("hide");
                $(masterDataScrDdl).append(new Option(data.text, data.id));
                $(masterDataScrDdl).val(data.id);
                $(masterDataScrDdl).trigger("change");
                masterDataScrDdl = null;
                masterDataController = null;

            }
            else {
                showAlertAutoHide("#createMasterDataPopUpAlert", data.status, data.message);
            }
        }
        ,
        error: function (request, status, error) {
            displayErrorMessage('Error in deleting parent alert data');
            return false;
        }
    });
}
function openAdhocReport(reportId, RefId,RefSelectionType) {
   // debugger;
    var dataObj = new Object();
    dataObj.ReportId = reportId;
    dataObj.CriteriaType = 1;
    dataObj.EmployeeSelectionIds = RefId
    dataObj.CustomFieldTypeSelectionIds = RefSelectionType
    $.ajax({
        type: "POST",
        url: "/Reports/AdhocReportPopup",
        data: JSON.stringify(dataObj),
        contentType: "application/json; charset=utf-8",
        // dataType: "json",
        success: function (data) {
            //debugger;
            //console.log(data);
            $("#divAdhocReportPopup").html(data);
            // setTimeout(function () { $('#divLoadingReport').hide(); }, 3000);
            $("#AdhocReportPopup_modal").modal('show');
        },
        error: function (request, status, error) {
            alert('Error in in runing  report data');
            return false;
        }
    });
}


function showAlertAutoHide(id, type, message) {
    //$(id).children("span").text(message);
    if (type == "Error") {
        //   $(id).removeClass('alert-success').addClass('alert-danger').show();
        displayToastErrorMessage(message);
    }
    else if (type == "Info") {
        //   $(id).removeClass('alert-success').addClass('alert-danger').show();
        displayToastInfoMessage(message);
    }
    else {
        //  $(id).removeClass('alert-danger').addClass('alert-success').show();
        displayToastSuccessMessage(message);
    }
    //setTimeout(function () { $(id).hide('slow', 'swing'); }, 4000);

}
function showAlertDismissable(id, type, message) {
    $(id).children("span").text(message);
    if (type == "Error") {
        $(id).removeClass('alert-success').addClass('alert-danger').show();
    }
    else {
        $(id).removeClass('alert-danger').addClass('alert-success').show();
    }


}
function displayToastErrorMessage(message) {

    toastr.options = toastMsgOptions;
    toastr.error(message, "Error!");

}

function displayToastWarningMessage(message) {

    toastr.options = toastMsgOptions;
    toastr.warning(message, "Warning!");

}
function displayToastSuccessMessage(message) {

    toastr.options = toastMsgOptions;
    toastr.success(message, "Success!");
}
function displayToastInfoMessage(message) {

    toastr.options = toastMsgOptions;
    toastr.info(message, "Info!");
}

function disabledSubmitAction(isDisabled) {
    if (isDisabled) {
        //In Processing 
        $(".submit-btn").attr('disabled', 'disabled');
        $(".submit-in-processing").show();
    }
    else {
        //Out of Processing
        $(".submit-btn").removeAttr('disabled');
        $(".submit-in-processing").hide();

    }
}


function DisabledRequestActionButton(isDisabled,classname) {

   // debugger
    if (isDisabled) {
        //In Processing 
        $("." + classname).attr('disabled', 'disabled');
        $(".loading").show();
    }
    else
    {
        //Out of Processing
        $("." + classname).removeAttr('disabled');
        $(".loading").hide();

    }
}