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

                    if (isDefined(salary.WorkingHours) && salary.WorkingHours >0) {
                        employee.WorkingHoursValue = salary.WorkingHours;
                    }

                    if (isDefined(salary.WorkingScore) && salary.WorkingScore > 0) {
                        employee.WorkingScoreValue = salary.WorkingScore;
                    }

                    if (isDefined(salary.FullPresencePay) && salary.FullPresencePay > 0) {
                        employee.FullPresencePayValue = salary.FullPresencePay;
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
                    employee.Salary = self.GetEmptySalary(employee.Id,employee.CycleId);

                }


                if (isDefined(employee) && isDefined(employee.Groups)) {
                    if (isDefined(employee.Groups.Name)) {
                        employee.GroupLabel = employee.Groups.Name;
                    }
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
            WorkingHours: null,
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
                $('input[data-type="WorkingHours"],input[data-type="WorkingScore"],input[data-type="FullPresencePay"]').numeric({ negative: false, decimal: '.' });
                $('input[data-type="OtherPenaltyFee"],input[data-type="OtherRewardFee"]').numeric({ negative: true, decimal: '.' });
                //$('#EmployeModal_Input_TechnicalLevel').numeric({ negative: false, decimal: '.' });
                //// Step 1: Refresh select
                //$('.selectpicker').selectpicker('refresh');
                //// Step 2: Build Department select options
                //if (isDefined(self.departmentList) && self.departmentList.length > 0) {
                //    self.buildSelectOption($('#EmployeModal_Select_DepartmentId'), self.departmentList, 'Id', 'Name');
                //}
                //// Step 3: Bind data 
                //if (isDefined(self.Employe) && isDefined(self.Employe.Id) && self.Employe.Id > 0) {
                //    if (isDefined(self.Employe.Groups) && isDefined(self.Employe.Groups.DepartmentId) && self.Employe.Groups.DepartmentId > 0) {
                //        $('#EmployeModal_Select_DepartmentId').val(self.Employe.Groups.DepartmentId);
                //        $('.selectpicker').selectpicker('refresh');
                //        $('#EmployeModal_Select_DepartmentId').trigger('change');

                //        if (isDefined(self.Employe.GroupsId)) {
                //            $('#EmployeModal_Select_GroupsId').val(self.Employe.GroupsId);
                //            $('.selectpicker').selectpicker('refresh');

                //            $('#EmployeModal_Select_GroupsId').trigger('change');
                //        }
                //    }

                //    if (isDefined(self.Employe.Name) && self.Employe.Name != '') {
                //        $('#EmployeModal_Input_Name').val(self.Employe.Name);
                //    }

                //    if (isDefined(self.Employe.EntreEntrepriseDate) && self.Employe.EntreEntrepriseDate != '') {
                //        $('#EmployeModal_Input_EntreEntrepriseDate').val(self.Employe.EntreEntrepriseDate.slice(0, 10));
                //    }

                //    if (isDefined(self.Employe.ExternalId) && self.Employe.ExternalId != '') {
                //        $('#EmployeModal_Input_ExternalId').val(self.Employe.ExternalId);
                //    }

                //    if (isDefined(self.Employe.TechnicalLevel) && self.Employe.TechnicalLevel != '') {
                //        $('#EmployeModal_Input_TechnicalLevel').val(self.Employe.TechnicalLevel);
                //    }

                //    if (isDefined(self.Employe.FixSalary) && self.Employe.FixSalary != '') {
                //        $('#EmployeModal_Input_FixSalary').val(self.Employe.FixSalary);
                //    }
                //}
                //else {
                //    // If department and group is selected in search, bind automaticly in creation employee
                //    if (isDefined(self.searchCriteria.DepartmentId) && self.searchCriteria.DepartmentId > 0) {
                //        $('#EmployeModal_Select_DepartmentId').val(self.searchCriteria.DepartmentId);
                //        $('.selectpicker').selectpicker('refresh');
                //        $('#EmployeModal_Select_DepartmentId').trigger('change');

                //        if (isDefined(self.searchCriteria.GroupsId) && self.searchCriteria.GroupsId > 0) {
                //            $('#EmployeModal_Select_GroupsId').val(self.searchCriteria.GroupsId);
                //            $('.selectpicker').selectpicker('refresh');

                //            $('#EmployeModal_Select_GroupsId').trigger('change');
                //        }
                //    }
                //}


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
                        case 'WorkingHours':
                            employee.Salary.WorkingHours = value;
                            break;
                        case 'WorkingScore':
                            employee.Salary.WorkingScore = value;
                            break;
                        case 'FullPresencePay':
                            employee.Salary.FullPresencePay = value;
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
}

Application.ViewsScript["SalarieSearch"] = new SalarieSearch();