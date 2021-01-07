SalariesValidation = function () {
    var self = this;

    self.employeeList = [];
    self.salarieList = [];

    self.searchCriteria = {
        DepartmentId: -1,
        GroupsId: -1,
        CycleId: -1,
        Validity: null
    };

    self.SalaryValidityList = [
        {
            Label: 'True', // TODO: translate
            Value: 'True',
        },
        {
            Label: 'False', // TODO: translate 
            Value:'False'
        }
    ]

    self.groupList = [];
    self.init = function () {
        $('.container').css('min-width', '100%');

        $('.selectpicker').selectpicker('refresh');
        $('#SalariesValidation_Table_Head').loadTemplate('#Tp1_SalariesValidation_Table_Head');

        // Bind validity selection 
        self.buildSelectOption($('#SalariesValidation_Select_SalaryValidity'), self.SalaryValidityList, 'Value', 'Label');


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
            case 'SalariesValidation_Select_DepartmentId':
                self.searchCriteria.DepartmentId = value;

                if (self.groupList!=null) {
                    var targetedGroupList = self.groupList.filter(p => p.DepartmentId == value);
                    self.buildSelectOption($('#SalariesValidation_Select_GroupId'), targetedGroupList, 'Id', 'Name');
                }
               
                break;
            case 'SalariesValidation_Select_GroupId':
                self.searchCriteria.GroupsId = value;
                break;
            case 'SalariesValidation_Select_PeriodId':
                self.searchCriteria.CycleId = value;
                break;
            case 'SalariesValidation_Select_SalaryValidity':
                if (value == 'True') {
                    self.searchCriteria.Validity = true;
                }
                else {
                    self.searchCriteria.Validity = false;
                }
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
        if ((self.searchCriteria.Validity != null ||
            (self.searchCriteria.DepartmentId != null && self.searchCriteria.DepartmentId != -1 && self.searchCriteria.GroupsId != null && self.searchCriteria.GroupsId != -1))
            && self.searchCriteria.CycleId != null && self.searchCriteria.CycleId != -1) {
            $('button#SalariesValidation_Button_Search').removeAttr('disabled');
            return true;
        }
        else {
            $('button#SalariesValidation_Button_Search').prop('disabled', true);
            return false;
        }
    };

    self.RefreshData = function () {
        if (self.checkCriteriaValidity() == true) {
            var dfSaleriesList = $.Deferred().done(function (result) {
                if (result != null && result.length > 0) {

                    self.salarieList = result;
                }
                else {
                    self.salarieList = [];
                }
            });

            $.when(dfSaleriesList).done(function () {
                self.formatData();
                if (isDefined(self.salarieList) && self.salarieList.length >0 ) {
                    $('#SalariesValidation_Table_Body').loadTemplate($('#Tp1_SalariesValidation_Table_Body'), self.salarieList);

                }
                else {
                    self.NoDataToDisplay();
                }
               
            });

            self.SalariesSearch(dfSaleriesList);
        }
    };

    self.formatData = function () {
        if (self.salarieList != null && self.salarieList.length >0) {
            self.salarieList.map(function (salary) {
                if (isDefined(salary) && isDefined(salary.Employe)) {
                    salary.Name = salary.Employe.Name;
                    salary.ExternalId = salary.Employe.ExternalId;
                    salary.GroupLabel = salary.Employe.Groups.Name;
                }
                if (isDefined(salary) && isDefined(salary.Cycle)) {
                    salary.StandardWorkingHours = salary.Cycle.StandardWorkingHours;
                }
                salary.TotalWorkingHours = 0;
                if (isDefined(salary.WorkingHoursDay)) {
                    salary.TotalWorkingHours = salary.TotalWorkingHours + salary.WorkingHoursDay;
                }
                if (isDefined(salary.WorkingHoursHoliday)) {
                    salary.TotalWorkingHours = salary.TotalWorkingHours + salary.WorkingHoursHoliday;
                }
                if (isDefined(salary.WorkingHoursNight)) {
                    salary.TotalWorkingHours = salary.TotalWorkingHours + salary.WorkingHoursNight;
                }

                if (isDefined(salary.Validity) && salary.Validity == true) {
                    salary.SalaryLineClass = 'table-success';
                    salary.Validated = true;
                }
                else {
                    salary.SalaryLineClass = 'table-warning';
                    salary.IsNotValidated = true;
                }
            });
        }
    };

    self.NoDataToDisplay = function () {
        var numberOfColum = $('#SalariesValidation_Table thead tr th').length;
        $('#SalariesValidation_Table_Body').html('<tr><td colspan="' + numberOfColum + '">No data to display</td></tr>');
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



    self.validateSalaries = function (event) {
        var Id = event.currentTarget.id; 
        if (isDefined(Id) && Id > 0) {
            $('table#SalariesValidation_Table').mask();
            // todo: confirmation 
            Application.Services.CommonService.SalariesValidation({ SalaryId: Id}, function (result) {
                if (result != null) {
                    self.RefreshData();
                }
                else {
                }
                $('table#SalariesValidation_Table').unmask();
            });
        }
    };


}

Application.ViewsScript["SalariesValidation"] = new SalariesValidation();