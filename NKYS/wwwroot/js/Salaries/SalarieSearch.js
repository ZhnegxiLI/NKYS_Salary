SalarieSearch = function () {
    var self = this;

    self.searchCriteria = {
        DepartmentId: -1,
        GroupId: -1,
        CycleId: -1
    };

    self.groupList = [];
    self.init = function () {
        console.log(Application.Main.CurrentView);
        Application.Services.CommonService.FindGroupList(null, function (result) {
            if (result!=null && result.length >0) {
                self.groupList = result;
                console.log(self.groupList);
            }
        });

    
    }

    self.OnChangeCriteria = function (event) {
        console.log(event.currentTarget)
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
        }
    };

    self.checkCriteriaValidity = function () {
        if (self.searchCriteria.DepartmentId != null && self.searchCriteria.DepartmentId != -1 &&
            self.searchCriteria.GroupId != null && self.searchCriteria.GroupId != -1 &&
            self.searchCriteria.CycleId != null && self.searchCriteria.CycleId != -1) {
            $('button#SalariesSearchFilter_Button_Search').removeAttr('disabled');
        }
        else {
            $('button#SalariesSearchFilter_Button_Search').prop('disabled', true);
        }
    };

    self.RefreshData = function () {
        $('#SalariesSearch_Table_Body').load('Salaries/OnGetSalaryTablePartial', self.searchCriteria);
    };
}

Application.ViewsScript["SalarieSearch"] = new SalarieSearch();