SalarieSearch = function () {
    var self = this;

    self.employeeList = [];
    self.salarieList = [];

    self.searchCriteria = {
        DepartmentId: -1,
        GroupsId: -1,
        CycleId: -1
    };

    self.groupList = [];
    self.init = function () {
        $('.container').css('min-width', '100%');

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
                self.searchCriteria.GroupsId = value;
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
            self.searchCriteria.GroupsId != null && self.searchCriteria.GroupsId != -1 &&
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

                    $('#SalariesSearchFilter_Button_Edit').removeAttr('disabled');
                }
                else {
                    self.NoDataToDisplay();
                    $('#SalariesSearchFilter_Button_Edit').prop('disabled', true);
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
                    //todo add salary information

                    if (isDefined(salary.Id) && salary.Id > 0) {
                        employee.SalaryId = salary.Id;
                    }

                    if (isDefined(salary.CycleId) && salary.CycleId > 0) {
                        employee.CycleId = salary.CycleId;
                    }

                    if (isDefined(salary.WorkingHoursDay) && salary.WorkingHoursDay >0) {
                        employee.WorkingHoursDayValue = salary.WorkingHoursDay;
                    }

                    if (isDefined(salary.WorkingHoursNight) && salary.WorkingHoursNight > 0) {
                        employee.WorkingHoursNightValue = salary.WorkingHoursNight;
                    }

                    if (isDefined(salary.WorkingHoursHoliday) && salary.WorkingHoursHoliday > 0) {
                        employee.WorkingHoursHolidayValue = salary.WorkingHoursHoliday;
                    }


                    if (isDefined(salary.WorkingScore) && salary.WorkingScore > 0) {
                        employee.WorkingScoreValue = salary.WorkingScore;
                    }

                    if (isDefined(salary.FullPresencePay) && salary.FullPresencePay > 0) {
                        employee.FullPresencePayValue = salary.FullPresencePay;
                    }

                    if (isDefined(salary.DormOtherFee)) {
                        employee.DormOtherFeeValue = salary.DormOtherFee;
                    }
                    
                    if (isDefined(salary.OtherRewardFee)) {
                        employee.OtherRewardFeeValue = salary.OtherRewardFee;
                    }
                    if (isDefined(salary.OtherPenaltyFee)) {
                        employee.OtherPenaltyFeeValue = salary.OtherPenaltyFee;
                    }
                    
                    if (isDefined(salary.Comment) && salary.Comment !='') {
                        employee.CommentValue = salary.Comment;
                    }

                    // Bind salary object 
                    employee.Salary = salary;
                }
                else {
                    employee.SalaryId = null;
                    employee.CycleId = self.searchCriteria.CycleId;

                    // Create un empty salary object for employee
                    employee.Salary = self.GetEmptySalary(employee.Id, employee.CycleId);

                    employee.SalaryLineClass = 'table-warning';

                }


                if (isDefined(employee) && isDefined(employee.Groups)) {
                    if (isDefined(employee.Groups.Name)) {
                        employee.GroupLabel = employee.Groups.Name;
                    }

                    if (isDefined(employee.Groups.IsFixSalary) && employee.Groups.IsFixSalary == true ) {
                        employee.IsFixSalary = true;
                    }
                    else {
                        employee.IsProductionValueBasedSalary = true;
                    }
                }

                if (isDefined(employee) && isDefined(employee.HasDorm) && employee.HasDorm == true) {
                    employee.HasDormLabel = true;
                }
                else {
                    employee.HasNoDormLabel = true;
                }
             

                formatedDataList.push(employee);
            });
        }
        return formatedDataList;
    };

    self.GetEmptySalary = function (EmployeeId, CycleId) {
        return {
            Id:null,
            CycleId: CycleId,
            EmployeId: EmployeeId,
            WorkingHoursDay: null,
            WorkingHoursNight: null,
            WorkingHoursHoliday: null,
            WorkingScore: null,
            AbsentHours: null,
            SocialSercurityFee: null,
            HousingReserves: null,
            FullPresencePay: null,
            OvertimePay: null,
            AbsentDeduct: null,
            DormFee: null,
            TransportFee: null,
            OtherRewardFee: null,
            OtherPenaltyFee: null,
            Comment: null,
            DeferredHolidayHours: null,
            DormOtherFee: null,
            SelfPaySocialSercurityFee: null,
            SeniorityPay: null,
            IsChanged: false // js param, check it and filter the IsChanged Object in final step
        };
    }

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


    self.EditSalariesModal = function () {

        self.modalEditSalariesTicks = "Modal_" + getTicks();

        Application.Main.initModal(
            self.modalEditSalariesTicks,
            'Tp1_EditSalariesModal',
            { title: 'Modify' },
            function () { $('#' + self.modalEditSalariesTicks).remove(); },
            function () {
                $('#EditSalariesModal_Table_Head').loadTemplate($('#Tp1_SalariesEdit_Table_Head'));

                var formatedData = self.formatData();
                if (isDefined(formatedData) && formatedData.length > 0) {
                    $('#EditSalariesModal_Table_Body').loadTemplate($('#Tp1_SalariesEdit_Table_Body'), formatedData);
                }
                //// Step 0: Only number input  
                $('input[data-type="WorkingHoursDayValue"],input[data-type="WorkingHoursNight"],input[data-type="WorkingHoursHolidayValue"],input[data-type="WorkingScore"],input[data-type="FullPresencePay"]').numeric({ negative: false, decimal: '.' });
                $('input[data-type="OtherPenaltyFee"],input[data-type="OtherRewardFee"],input[data-type="DormOtherFee"]').numeric({ negative: true, decimal: '.' });


                $('button#EditSalariesModal_Button_Close').on("click", function (e) { $("#" + self.modalEditSalariesTicks).modal('hide'); });
                $("#" + self.modalEditSalariesTicks).modal();
            });
    };

    self.OnChangeEditSalarieModalField = function (event) {
        var salaryId = $(event.currentTarget).closest('tr').attr('data-salary-id');
        var employeeId = $(event.currentTarget).closest('tr').attr('data-employee-id');
        var cycleId = $(event.currentTarget).closest('tr').attr('data-cycle-id');

        var type = $(event.currentTarget).attr('data-type');
        var value = $(event.currentTarget).val();
        if (isDefined(cycleId) && cycleId > 0 && isDefined(employeeId) && employeeId > 0) {
            self.employeeList.map(function (employee) {
                if (isDefined(employee.Salary) && employee.Id == employeeId && employee.Salary.CycleId == cycleId) {
                    employee.Salary.IsChanged = true;
                    // todo complete other attribute  
                    switch (type) {

                        case 'WorkingHoursDay':
                            employee.Salary.WorkingHoursDay = value;
                            break;
                        case 'WorkingHoursNight':
                            employee.Salary.WorkingHoursNight = value;
                            break;
                        case 'WorkingHoursHoliday':
                            employee.Salary.WorkingHoursHoliday = value;
                            break;

                        case 'WorkingScore':
                            employee.Salary.WorkingScore = value;
                            break;
                        case 'FullPresencePay':
                            employee.Salary.FullPresencePay = value;
                            break;

                        case 'DormOtherFee':
                            employee.Salary.DormOtherFee = value;
                            break;

                        case 'OtherRewardFee':
                            employee.Salary.OtherRewardFee = value;
                            break;
                        case 'OtherPenaltyFee':
                            employee.Salary.OtherPenaltyFee = value;
                            break;

                        case 'Comment':
                            employee.Salary.Comment = value;
                            break;
                        default:
                    }
                    // Show the changes
                    $('tr[data-employee-id=' + employeeId + '][data-cycle-id=' + cycleId + '] td input[data-type=' + type + ']').addClass('IsModified');
                    $('tr[data-employee-id=' + employeeId + '][data-cycle-id=' + cycleId + '] td textarea[data-type=' + type + ']').addClass('IsModified');
                }
          
            });
        }
 
    }


    self.saveSalaries = function (event) {
        var saveValidity = false;
        if (isDefined(self.employeeList) && self.employeeList.length > 0) {
            var toBeSavedSalariesList = [];
            self.employeeList.map(employee => {
                if (isDefined(employee.Salary) && isDefined(employee.Salary.IsChanged) && employee.Salary.IsChanged == true) {
                    toBeSavedSalariesList.push(employee.Salary);
                }
            });
      
            if (isDefined(toBeSavedSalariesList) && toBeSavedSalariesList.length >0) {
                saveValidity = true;
                // Todo service save salary
                $('#' + self.modalEditSalariesTicks).mask();
                Application.Services.CommonService.SaveSalaries({ salariesList: toBeSavedSalariesList }, function (numberOfSalariesSaved) {
                    if (isDefined(numberOfSalariesSaved) && numberOfSalariesSaved > 0) {
                        self.RefreshData();
                        $("#" + self.modalEditSalariesTicks).modal('hide');
                    }

                    $('#' + self.modalEditSalariesTicks).unmask();
                });
            }
        }
        if (saveValidity == false) {
            // TODO show error message
        }
    }

    // Validation salaries  
    self.ValidationSalariesModal = function () {

        
        //self.modalValidationSalariesTicks = "Modal_" + getTicks();

        //Application.Main.initModal(
        //    self.modalValidationSalariesTicks,
        //    'Tp1_ValidationSalariesModal',
        //    { title: 'Validation' },
        //    function () { $('#' + self.modalValidationSalariesTicks).remove(); },
        //    function () {






        //        $('button#ValidationSalariesModal_Button_Close').on("click", function (e) { $("#" + self.modalValidationSalariesTicks).modal('hide'); });
        //        $("#" + self.modalValidationSalariesTicks).modal();
        //    });
    };


    self.validateSalaries = function () {

    };


}

Application.ViewsScript["SalarieSearch"] = new SalarieSearch();