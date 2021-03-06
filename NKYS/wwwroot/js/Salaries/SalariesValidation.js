﻿SalariesValidation = function () {
    var self = this;
    self.$table = $('#SalariesValidation_Table')

    self.employeeList = [];
    self.salarieList = [];

    self.searchCriteria = {
        DepartmentId: -1,
        GroupsId: -1,
        CycleId: -1,
        Validity: null
    };

  

    self.groupList = [];

    self.init = function () {
        self.SalaryValidityList = [
            {
                Label: i18next.t('Yes'),
                Value: 'True',
            },
            {
                Label: i18next.t('No'),
                Value: 'False'
            }
        ];

        $('#SalariesValidation_Table').bootstrapTable({
            detailView: true,
            detailFormatter: "Application.ViewsScript.SalariesValidation.detailFormatter", // display detail info function
            columns: [
                {
                    field: 'Validity',
                    checkbox: true,
                    formatter: function (value, row, index) {
                        if (row.Validated === true) {//如果已经操作禁止选择
                            return { disabled: true, }
                        } else {
                            return { disabled: false, }
                        }
                    }
                },
                {
                    field: 'FinalValidity',
                    title: '经理审核',// todo translate
                    formatter: function (value, row, index) {
                        var html = '';

                        if (isDefined(row.Validity) && row.Validity == true && isDefined(row.FinalValidity) && row.FinalValidity == false) {
                            html = '<input type="checkbox"' + ((isDefined(value) && value == true) ? 'checked' : '') + ' data-salaryid="' + row.Id + '" data-type="FinalValidityCheckbox">'
                        }
                        else {
                            html = '<input type="checkbox"' + ((isDefined(value) && value == true) ? 'checked' : '') +' data-salaryid="' + row.Id + '" data-type="FinalValidityCheckbox" disabled>'
                        }
                      
                        return html;
                    }
                },
                {
                field: 'GroupLabel',
                title: i18next.t('Groups')
            }, {
                field: 'Name',
                title: i18next.t('Name')
            }, {
                field: 'ExternalId',
                title: i18next.t('ExternalId')
                }
                , {
                    field: 'TotalWorkingHours',
                    title: i18next.t('TotalWorkingHours')
                }, {
                    field: 'WorkingDays',
                    title: i18next.t('WorkingDays')
                }, {
                    field: 'StandardWorkingHours',
                    title: i18next.t('StandardWorkingHours')
                },
                {
                    field: 'SelfPaySocialSercurityFee',
                    title: i18next.t('SelfPaySocialSercurityFee')
                },
                {
                    field: 'HousingReservesFee',
                    title: i18next.t('HousingReservesFee')
                },
                {
                    field: 'FullPresencePay',
                    title: i18next.t('FullPresencePay')
                },
                {
                    field: 'TransportFee',
                    title: i18next.t('TransportFee')
                },
                {
                    field: 'SeniorityPay',
                    title: i18next.t('SeniorityPay')
                },
                {
                    field: 'PositionPay',
                    title: i18next.t('PositionPay')
                },
                {
                    field: 'OtherRewardFee',
                    title: i18next.t('OtherRewardFee')
                },
                {
                    field: 'SocialSercurityFee',
                    title: i18next.t('SocialSercurityFee')
                },
            
                {
                    field: 'DormFee',
                    title: i18next.t('DormFee')
                },
                {
                    field: 'DormOtherFee',
                    title: i18next.t('DormOtherFee')
                },
                {
                    field: 'OtherPenaltyFee',
                    title: i18next.t('OtherPenaltyFee')
                },
                
                {
                    field: 'NetSalary',
                    title: i18next.t('NetSalary')
                },
                {
                    field: 'SalaryTax',
                    title: i18next.t('SalaryTax')
                },
                {
                    field: 'FinalSalary',
                    title: i18next.t('FinalSalary')
                }]
        });

        // Bind Check checkbox action
        $('#SalariesValidation_Table').on('check.bs.table uncheck.bs.table ' +
            'check-all.bs.table uncheck-all.bs.table',
            function () {
                // save your data, here just save the current page
                var  selections = self.getIdSelections()
                // push or splice the selections if you want to save all data selections
                if (selections.length>0) {
                    $('button#SalariesValidation_Button_Validation').removeAttr('disabled');
                }
                else {
                    $('button#SalariesValidation_Button_Validation').prop('disabled', 'disabled');
                }
            })
        // Check box check all 
        $('#SalariesValidation_Table').on('all.bs.table', function (e, name, args) {
        });

     
        
        $('#SalariesValidation_Table').on('post-body.bs.table', function () {
            $('[data-type="FinalValidityCheckbox"]').on('change', function () {
                var selections = self.getIdSelections(true);
                if (selections.length > 0) {
                    $('button#SalariesValidation_Button_FinalValidation').removeAttr('disabled');
                }
                else {
                    $('button#SalariesValidation_Button_FinalValidation').prop('disabled', 'disabled');
                }
            });
        });
        $('button#SalariesValidation_Button_Validation').on('click', function () {
            var SalaryIds = self.getIdSelections();
            if (isDefined(SalaryIds) && SalaryIds.length > 0) {
                $('table#SalariesValidation_Table').mask();
                // todo: confirmation 
                Application.Services.CommonService.SalariesValidation({ SalaryIds: SalaryIds }, function (result) {
                    if (result != null) {
                        self.RefreshData();
                    }
                    else {
                    }
                    $('table#SalariesValidation_Table').unmask();
                });
            }
        });


        $('button#SalariesValidation_Button_FinalValidation').on('click', function () {
            var SalaryIds = self.getIdSelections(true);
            if (isDefined(SalaryIds) && SalaryIds.length > 0) {
                $('table#SalariesValidation_Table').mask();
                // todo: confirmation 
                Application.Services.CommonService.SalariesValidation({ SalaryIds: SalaryIds, IsFinalValidity : true }, function (result) {
                    if (result != null) {
                        self.RefreshData();
                    }
                    else {
                    }
                    $('table#SalariesValidation_Table').unmask();
                });
            }
        });


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

    self.getIdSelections = function (IsFinalValidity) {
        var selectionIds = [];
        if (isDefined(IsFinalValidity) && IsFinalValidity ==true) {
            var selectionNodes = $('[data-type="FinalValidityCheckbox"]:checked');

            selectionNodes.each((index, value) => {
                selectionIds.push({ Id: $(value).data('salaryid') });
            })
            // todo 
        }
        else {
            selectionIds = $('#SalariesValidation_Table').bootstrapTable('getSelections');
            selectionIds = selectionIds.filter(p => !isDefined(p.ValidatedBy) && !isDefined(p.ValidatedOn));
        }
 
        return $.map(selectionIds, function (row) {
            return row.Id;
        });
    }

    self.detailFormatter = function(index, row) {
        var html = []
        $.each(row, function (key, value) {
            html.push('<p><b>' + key + ':</b> ' + value + '</p>')
        });
        return html.join('');
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
                    self.searchCriteria.GroupsId = null;
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
                else if (value == 'False'){
                    self.searchCriteria.Validity = false;
                }
                else {
                    self.searchCriteria.Validity = null;
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
                if (isDefined(self.salarieList) && self.salarieList.length > 0) {
           
                   // $('#SalariesValidation_Table_Body').loadTemplate($('#Tp1_SalariesValidation_Table_Body'), self.salarieList);
                    $('#SalariesValidation_Table').bootstrapTable('load', self.salarieList)
                }
                else {
                   // self.NoDataToDisplay();
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

                if (isDefined(salary.FinalValidity) && salary.FinalValidity == true) {
                    salary.SalaryLineClass = 'table-success';
                    salary.FinalValidated = true;
                }
                else {
                    salary.SalaryLineClass = 'table-warning';
                    salary.IsNotFinalValidated = true;
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
            Application.Services.CommonService.SalariesValidation({ SalaryId: Id, IsFinalValidity: false}, function (result) {
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