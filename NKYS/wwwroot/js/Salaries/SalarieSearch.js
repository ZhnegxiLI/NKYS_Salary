SalarieSearch = function () {
    var self = this;

    self.employeeList = [];
    self.salarieList = [];

    self.searchCriteria = {
        DepartmentId: -1,
        GroupId: -1,
        CycleId: -1
    };

    self.groupList = [];
    self.init = function () {

        $('.selectpicker').selectpicker('refresh');
        $('#SalariesSearch_Table_Head').loadTemplate('#Tp1_SalariesSearch_Table_Head');

        Application.Services.CommonService.FindGroupList(null, function (result) {
            if (result!=null && result.length >0) {
                self.groupList = result;
            }
        });
    }

    self.OnChangeCriteria = function (event) {
        var Id = event.currentTarget.id; 
        var value = event.currentTarget.value;
        switch (Id) {
            case 'SalariesSearchFilter_Select_DepartmentId':
                self.searchCriteria.DepartmentId = value;

                if (self.groupList!=null) {
                    var targetedGroupList = self.groupList.filter(p => p.DepartmentId == value);
                    self.buildSelectOption($('#SalariesSearchFilter_Select_GroupId'), targetedGroupList, 'Id', 'Name');
                }
               
                break;
            case 'SalariesSearchFilter_Select_GroupId':
                self.searchCriteria.GroupId = value;
                break;
            case 'SalariesSearchFilter_Select_PeriodId':
                self.searchCriteria.CycleId = value;
                break;
            default:
                break;
        }
        // Check all criteria is completed 
        self.checkCriteriaValidity();
    }

    self.buildSelectOption = function (pSelector, pList, pValue, pLabel, pSelectedOptionValue) {
        var options = ' <option selected value="-1"></option> ';
        if (pList!= null && pList.length>0) {
            pList.map(function (val) {
                if (pSelectedOptionValue != null && val[pValue] == pSelectedOptionValue) {
                    options += ' <option value=' + val[pValue] + ' selected>' + val[pLabel] + '</option> ';
                }
                else {
                    options += ' <option value=' + val[pValue] + '>' + val[pLabel] + '</option> ';
                }
            });
        }
        if (pSelector!=null) {
            pSelector.html(options);
            pSelector.selectpicker('refresh');
        }

    };

    self.checkCriteriaValidity = function () {
        if (self.searchCriteria.DepartmentId != null && self.searchCriteria.DepartmentId != -1 &&
            self.searchCriteria.GroupId != null && self.searchCriteria.GroupId != -1 &&
            self.searchCriteria.CycleId != null && self.searchCriteria.CycleId != -1) {
            $('button#SalariesSearchFilter_Button_Search').removeAttr('disabled');
            return true;
        }
        else {
            $('button#SalariesSearchFilter_Button_Search').prop('disabled', true);
            return false;
        }
    };

    self.RefreshData = function () {
        if (self.checkCriteriaValidity() == true) {
            // var employeeList = Application.Services.CommonService.FindGroupList()
            var dfEmployeeList = $.Deferred().done(function (result) {
                if (result!= null && result.length >0) {

                    self.employeeList = result;
                }
                else {
                    self.employeeList = [];
                }
            });

            var dfSaleriesList = $.Deferred().done(function (result) {
                if (result != null && result.length > 0) {

                    self.salarieList = result;
                }
                else {
                    self.salarieList = [];
                }
            });

            $.when(dfEmployeeList, dfSaleriesList).done(function () {
                var formatedData = self.formatData();
                if (isDefined(formatedData) && formatedData.length >0 ) {
                    $('#SalariesSearch_Table_Body').loadTemplate($('#Tp1_SalariesSearch_Table_Body'), formatedData);
                }
                else {
                    self.NoDataToDisplay();
                }
               
            });

            self.GetEmployeList(dfEmployeeList);
            self.SalariesSearch(dfSaleriesList);
        }
    };

    self.formatData = function () {
        var formatedDataList = [];
        if (self.employeeList != null && self.employeeList.length>0) {
            self.employeeList.map(employee => {
                var salary = self.salarieList.find(s => {
                    return s.EmployeId == employee.Id; 
                });

                if (salary!=null) {

                }
            });
        }
        return formatedDataList;
    };

    self.NoDataToDisplay = function () {
        $('#SalariesSearch_Table_Body').html('<tr><td>' + 'No data to display' + '</td></tr>');
    }

    self.GetEmployeList = function (pGetEmployeList) {
        Application.Services.CommonService.GetEmployeList(self.searchCriteria, function (result) {
            if (result != null) {
                pGetEmployeList.resolve(result);
            }
            else {
                pGetEmployeList.reject();
            }
        });
    }

    self.SalariesSearch = function (pSalaryList) {
        Application.Services.CommonService.SalariesSearch(self.searchCriteria, function (result) {
            if (result != null) {
                pSalaryList.resolve(result);
            }
            else {
                pSalaryList.reject();
            }
        });
    }
}

Application.ViewsScript["SalarieSearch"] = new SalarieSearch();