//JS for User main page 
var alertID = null;
$(document).ready(function () {
    $('#emp_SSN').mask('999-99-9999');
    //Default User List on list view
    $("#divLoadingUsers").hide();
    $("#divUserGridHeaderBar").hide();
   
    //Default retreive List view data
    getUserViewData(0);
    $('#EmployeeId,#EmployeeName').on("keyup", function (e) {
        debugger;
        if (e.which == 13) {
            getFilterUserList();
        }
    });

    //$('#EmployeeStatusId,#PositionId').on("change", function () {
    //    debugger;
    //    getFilterUserList();
    //});
    $('#DepartmentId').on("change", function () {
        debugger;
        var deptId = $(this).val();
        LoadSubDepartmentDropdown(deptId, "#SubDepartmentId");
    });
    $('#userGridPageLen').on("change", function () {
        debugger;
        var newPageSize = $(this).val();
        $('#PageSize').val(newPageSize);
        getUserGridPageData(1);
    });
    $('#userGridSearchTxt').on("keyup", function () {
        debugger;
        var searchtext = $(this).val();
        $("#GridSearchTxt").val(searchtext);
        getUserGridPageData(1);
    });

    //Save Employee
    $('#btnSaveEmployee').click(function () {
        debugger;
        alertID = "#employeeCreateEditAlert";
        var datafieldsObject = getInputFieldsData();
        var isValidated = validateInputFields(datafieldsObject);
        if (isValidated) {
            // ajax call for saving data
            saveEmployeeData(datafieldsObject);
        }

    });

    $("#employeeCreateEdit_modal").on('shown.bs.modal', function () {
        $('#emp_empid').val("");
        $('#emp_SSN').val("");
        $('#emp_firstName').val("");
        $('#emp_middleName').val("");
        $('#emp_firstLastName').val("");
        $('#emp_secondLastName').val("");
       // $('#CompanyId').val("");
    });

});
function getFilterUserList() {
    var viewTypeId = $('#ViewTypeId').val();
    if (viewTypeId == 0) { //list view
        getUserList(viewTypeId);
    }
    else { //Grid view
        getUserGridPageData(1);
    }
}
function getUserGridPageData(page) {
    debugger;
    var currentPage = $('#PageNo').val();
    if (page == -1) {
        currentPage--;
    }
    else if (page == 0) {
        currentPage++;
    }
    else {
        currentPage = page;
    }
    $('#PageNo').val(currentPage);
    getUserList(1);
}
function getUserViewData(ViewTypeId) {
    if (ViewTypeId == 1) {// Gridview params setting
        $("#PageNo").val(1);
        $("#PageSize").val(12);
        $('#userGridPageLen').val(12);
        $("#GridSearchTxt").val("");        
        $('#userGridSearchTxt').val("");
    }
    getUserList(ViewTypeId);
}
function getUserList(ViewTypeId) {
    $("#divLoadingUsers").show();
    debugger;
    var filterObject = new Object();
    filterObject.EmployeeId = $('#EmployeeId').val();
    filterObject.EmployeeName = $('#EmployeeName').val();
    filterObject.DepartmentId = $('#DepartmentId').val()
    filterObject.SubDepartmentId = $('#SubDepartmentId').val()
    filterObject.PositionId = $('#PositionId').val();
    filterObject.EmploymentTypeId = $('#EmploymentTypeId').val();
    filterObject.EmployeeTypeId = $('#EmployeeTypeId').val();
    filterObject.EmployeeStatusId = $('#EmployeeStatusId').val();
    filterObject.SupervisorId = $('#SupervisorId').val();
    filterObject.ViewTypeId = ViewTypeId;
    filterObject.PageSize = $('#PageSize').val();
    filterObject.PageNo = $('#PageNo').val();
    filterObject.SearchText = $('#GridSearchTxt').val();
    

        $.ajax({
            type: "POST",
            url: '/UserInformation/IndexByViewType',
            data: JSON.stringify(filterObject),

            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                //console.log(data);
                var activeBtnId = "#btnListView";
                var inactiveBtnId = "#btnGridView";

                if (ViewTypeId == 1) {
                    inactiveBtnId = "#btnListView";
                    activeBtnId = "#btnGridView";
                    $("#divUserGridHeaderBar").show();
                }
                else {
                    $("#divUserGridHeaderBar").hide();
                }

                $("#divLayoutView").html(data);
                $('#ViewTypeId').val(ViewTypeId);


                $(inactiveBtnId).removeClass("active");
                $(activeBtnId).removeClass("active").addClass("active");
                $("#divLoadingUsers").hide();
            },
            error: function () {
                // displayWarningMessage(data.ErrorMessage);
            }
        });
          
   
}



function resetUserFilterList() {
    debugger;
    $('#EmployeeId').val("");
    $('#EmployeeName').val("");
    $(".searchableDDL").each(function () {
        $(this).val("");
        $(this).select2('destroy');
        $(this).select2();
        $(".select2-container").css("width", "100%");
    });
   
    getFilterUserList();
}
function getInputFieldsData() {
    debugger;
    //Get add employee form fields data and save in object
    var dataObj = new Object();
    dataObj.empid = $('#emp_empid').val();
    //dataObj.ssn = $('#emp_SSN').val();
    dataObj.ssn = $('#emp_SSN').mask();
    dataObj.firstName = $('#emp_firstName').val();
    dataObj.middleName = $('#emp_middleName').val();
    dataObj.firstLastName = $('#emp_firstLastName').val();
    dataObj.secondLastName = $('#emp_secondLastName').val();
   // dataObj.companyId = $('#CompanyId').val();
    
    return dataObj;
}

function validateInputFields(dataObj) {
    //validate object data
    var isRequiredValidated = 0;
    var ValidationMessage = 'Minimum 3 Alphabets Allowed For';
    var isValidatedLength = true
    var isValidated = true
        ;
    if (dataObj != null)
    {
        isRequiredValidated += dataObj.empid.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.firstName.trim().length > 0 ? 1 : 0;
        isRequiredValidated += dataObj.firstLastName.trim().length > 0 ? 1 : 0;
      //  isRequiredValidated += dataObj.secondLastName.trim().length > 0 ? 1 : 0;
       // isRequiredValidated += dataObj.companyId.trim().length > 0 ? 1 : 0;
        if (isRequiredValidated != 3) {
            isValidated = false;
            var message = " Missing Required field(s)";
        }

        //if (isValidated && (dataObj.ssn.trim().length > 0 && dataObj.ssn.trim().length < 11)) {
        //    isValidated = false;
        //    var message = " Invalid SSN";
        //}

        if (isValidated && dataObj.firstName.trim().length < 3) {
            isValidatedLength = false;
            var Label = $('#First_Name').text();
            ValidationMessage += " " + Label.trim().slice(0, -1) +",";
        }
        if (isValidated && dataObj.firstLastName.trim().length < 3) {
            isValidatedLength = false;
            var Label = $('#1st_LastName').text();
            ValidationMessage += " " + Label.trim().slice(0, -1) + ",";
        }
        if (isValidated && dataObj.secondLastName.trim().length < 3 && dataObj.secondLastName.trim().length > 0) {
            isValidatedLength = false;
            var Label = $('#2nd_LastName').text();
            ValidationMessage += " " + Label.trim().slice(0, -1) +",";
        }
    }
    if (!isValidated) showAlertAutoHide(alertID, 'Error', message);
    if (!isValidatedLength) showAlertAutoHide(alertID, 'Error', ValidationMessage.trim().slice(0, -1));

    if (!isValidatedLength) {
        isValidated = false
    }

    return isValidated;
}

function saveEmployeeData(dataObj) {
    //var message = " User is added succesfully!"
    //showAlertAutoHide(alertID, 'Success', message);

    // save employee data
    if (dataObj.empid > 0) {
        $(".loading").show();
        $.ajax({
            url: "/UserInformation/AjaxCreateEditUser",
            type: "POST",
            data: {
                "EmployeeId": dataObj.empid,
                "FirstName": dataObj.firstName,
                "MiddleInitial": dataObj.middleName,
                "FirstLastName": dataObj.firstLastName,
                "SecondLastName": dataObj.secondLastName,
               // "CompanyID": dataObj.companyId,
                "SSN": dataObj.ssn

            },
            dataType: "json",
            success: function (data) {
                debugger;
                $(".loading").hide();
                if (data.status == "Success") {

                    showAlertAutoHide("#employeeIndexPageAlert", data.status, data.message);
                    window.location.href = "/UserInformation/Details/" + data.id;
                    $("#employeeCreateEdit_modal").modal("hide");
                }
                else if (data.status == "Error") {

                    showAlertAutoHide("#employeeCreateEditAlert", "Error", data.message);
                }

            }
            ,
            error: function (data) {
                $(".loading").hide();
                showAlertAutoHide("#employeeCreateEditAlert", "Error", data.message);

            }
        });
    }
    else {
        showAlertAutoHide("#employeeCreateEditAlert", "Error", "Employee ID should be a positive number (> 0).");
    }
}
function LoadSubDepartmentDropdown(departmentId, targetElement) {

    $.ajax({
        url: '/SubDepartment/SubDepartmentByDepartment',
        data: { 'departmentId': departmentId }, //dataString,
        dataType: 'json',
        type: 'GET',
        success: function (res) {
            debugger;
            var data = res;
            $(targetElement + ' option').remove();
            var option = '<option value=""> All </option>';
            $(targetElement).append(option);
            $(data).each(function () {
                var option = '<option value=' + this.id + '>' + this.name + '</option>';
                $(targetElement).append(option);
            });
            // if (selectedValue != null)
            //$(targetElement).val(selectedValue == null ? "" : selectedValue);
        },
        error: function (xhr, status, error) {
            //displayErrorMessage('Error during retrieving Data:' + error);
        }
    });

}