EmployeSearch = function () {
    var self = this;

    self.searchCriteria = {
        DepartmentId: -1,
        GroupsId: -1
    };

    self.Employe = null;

    self.groupList = [];
    self.departmentList = [];

    self.employeList = [];

    self.init = function () {
        // TODO: Change to deferred, add create disabled when two service is not finished 
        Application.Services.CommonService.FindGroupList(null, function (result) {
            if (result!=null && result.length >0) {
                self.groupList = result;
            }
        });

        Application.Services.CommonService.FindDepartmentList(null, function (result) {
            if (result != null && result.length > 0) {
                self.departmentList = result;
            }

            self.buildSelectOption($('#EmployeSearch_Select_DepartmentId'), self.departmentList, 'Id', 'Name');
        });

    }

    /* Search employee zoom */
    self.OnChangeEmployeSearchCriteria = function (event) {
        var Id = event.currentTarget.id;
        var value = event.currentTarget.value;
        switch (Id) {
            case 'EmployeSearch_Select_DepartmentId':
                self.searchCriteria.DepartmentId = value;

                if (self.groupList != null) {
                    var targetedGroupList = self.groupList.filter(p => p.DepartmentId == value);
                    self.buildSelectOption($('#EmployeSearch_Select_GroupsId'), targetedGroupList, 'Id', 'Name');
                }

                break;
            case 'EmployeSearch_Select_GroupsId':
                self.searchCriteria.GroupsId = value;
                break;

            default:
                break;
        }
        // Check all criteria is completed 
        self.checkEmployeSearchCriteriaValidity();
    };

    self.checkEmployeSearchCriteriaValidity = function () {
        if (self.searchCriteria.DepartmentId != null && self.searchCriteria.DepartmentId != -1 &&
            self.searchCriteria.GroupsId != null && self.searchCriteria.GroupsId != -1) {
            $('button#EmployeSearch_Button_Search').removeAttr('disabled');
            return true;
        }
        else {
            $('button#EmployeSearch_Button_Search').prop('disabled', true);
            return false;
        }
    };


    self.buildEmployeTable = function () {
        if (self.employeList != null && self.employeList.length > 0) {

        }
        else {
            $('#EmployeSearch_Table tbody').html('<tr><td>No data to display</td></tr>')
        }
    };

    self.searchEmploye = function () {
        if (self.checkEmployeSearchCriteriaValidity() == true) {
            Application.Services.CommonService.GetEmployeList(self.searchCriteria, function (result) {
                if (result != null && result.length > 0) {
                    self.employeList = result;
                }
                else {
                    self.employeList = [];
                }

                self.buildEmployeTable();
            });
        }
    };

    /* Create / update employee zoom  */
 
    self.initEmployeModal = function (event) {
        var action = $(event.currentTarget).data('action');
        if (action == 'Create') {
           self.Employe = self.getEmptyEmploye();
        }
        else if (action == 'Modify') {

        }
        else {
            self.Employe = null;
        }

        if (self.Employe!= null) {
            self.buildEmployeModal();
        }
    };

    self.getEmptyEmploye = function () {
        return {
            Id: 0,
            GroupsId: -1,
            Name: '',
            EntreEntrepriseDate: null,
            ExternalId: '',
            TechnicalLevel: '',
            SelfPaySocialSercurity: '',
            SelfPayHousingReserves: '',
            HasDorm: false,
            TransportFee: null,
            PositionPay: null,
            IsChefOfGroup: null,
            SeniorityPay:null,
            FixSalary: null,
            DeductionPercentage: null,
            IsTemporaryEmploye: false,
            DepartDate: null,
            EmployeDeductionConfiguration: []
        };
    };

    self.buildEmployeModal = function () {

        self.modalEmployeeTicks = "Modal_" + getTicks();

        Application.Main.initModal(
            self.modalEmployeeTicks,
            'Tp1_EmployeModal',
            { title: isDefined(self.Employe) && isDefined(self.Employe.Id) && self.Employe.Id >0? 'Modify': 'Create'},
            function () { $('#' + self.modalEmployeeTicks).remove(); },
            function () {

                $('.selectpicker').selectpicker('refresh');

                if (isDefined(self.departmentList) && self.departmentList.length>0) {
                    self.buildSelectOption($('#EmployeModal_Select_DepartmentId'), self.departmentList, 'Id', 'Name');
                }


                $('button#EmployeModal_Button_Close').on("click", function (e) { $("#" + self.modalEmployeeTicks).modal('hide'); });
                $("#" + self.modalEmployeeTicks).modal();
            });
    };

    self.OnChangeEmployeModalCriteria = function (event) {
        var Id = event.currentTarget.id;
        var value = event.currentTarget.value;
        switch (Id) {
            case 'EmployeModal_Input_Name':
                if (isDefined(value) && value!='') {
                    self.Employe.Name = value;
                    $(event.currentTarget).removeClass('is-invalid');
                }
                else {
                    self.Employe.Name = '';
                }
                break;
            case 'EmployeModal_Select_DepartmentId':
               
                if (isDefined(value) && value > 0) {
                    self.Employe.DepartmentId = value;
                    $('#EmployeModal_Select_DepartmentId').removeClass('is-invalid');
                    $('#EmployeModal_Select_DepartmentId').closest('div').removeClass('is-invalid');
                    $('.selectpicker').selectpicker('refresh');
                }
                else {
                    self.Employe.DepartmentId = -1; 
                    self.Employe.GroupsId = -1;
                }

                if (self.groupList != null) {
                    var targetedGroupList = self.groupList.filter(p => p.DepartmentId == value);
                    self.buildSelectOption($('#EmployeModal_Select_GroupsId'), targetedGroupList, 'Id', 'Name');
                }
             
                break;
            case 'EmployeModal_Select_GroupsId':
               
                if (isDefined(value) && value > 0) {
                    self.Employe.GroupsId = value;

                    $('#EmployeModal_Select_GroupsId').removeClass('is-invalid');
                    $('#EmployeModal_Select_GroupsId').closest('div').removeClass('is-invalid');
                    $('.selectpicker').selectpicker('refresh');
                }
                else {
                    self.Employe.GroupsId = -1;
                }
                break;

            case 'EmployeModal_Input_EntreEntrepriseDate':
                if (isDefined(value) && value != '') {
                    self.Employe.EntreEntrepriseDate = value;
                    $(event.currentTarget).removeClass('is-invalid');
                }
                else {
                    self.Employe.EntreEntrepriseDate = '';
                }
                break;

            case 'EmployeModal_Input_ExternalId':
                if (isDefined(value) && value != '') {
                    self.Employe.ExternalId = value;
                    $(event.currentTarget).removeClass('is-invalid');
                }
                else {
                    self.Employe.ExternalId = '';
                }
                break;

            case 'EmployeModal_Input_TechnicalLevel':
                self.Employe.TechnicalLevel = value;
                break;

            case 'EmployeModal_Input_SelfPaySocialSercurity':
                self.Employe.SelfPaySocialSercurity = value;
                break;

            case 'EmployeModal_Input_SelfPayHousingReserves':
                self.Employe.SelfPayHousingReserves = value;
                break;

            case 'EmployeModal_CheckBox_HasDorm':
                var checked = $("#" + event.currentTarget.id).is(":checked");
                checked = isDefined(checked) ? checked : false;
                self.Employe.HasDorm = checked;
                break;

            case 'EmployeModal_Input_TransportFee':
                self.Employe.TransportFee = value;
                break;

            case 'EmployeModal_Input_PositionPay':
                self.Employe.PositionPay = value;
                break;

            case 'EmployeModal_CheckBox_IsChefOfGroup':
                var checked = $("#" + event.currentTarget.id).is(":checked");
                checked = isDefined(checked) ? checked : false;
                self.Employe.IsChefOfGroup = checked;
                break;

            case 'EmployeModal_Input_SeniorityPay':
                self.Employe.SeniorityPay = value;
                break;

            case 'EmployeModal_Input_FixSalary':
                self.Employe.DeductionPercentage = value;
                break;

            case 'EmployeModal_Input_DeductionPercentage':
                self.Employe.DeductionPercentage = value;
                break;

            case 'EmployeModal_Input_IsTemporaryEmploye':
                var checked = $("#" + event.currentTarget.id).is(":checked");
                checked = isDefined(checked) ? checked : false;
                self.Employe.IsTemporaryEmploye = checked;
                break;

            case 'EmployeModal_Input_DepartDate':
                self.Employe.DepartDate = value;
                break;

            case 'EmployeModal_Select_DeductionConfiguration':
                self.Employe.EmployeDeductionConfiguration = value;
                break;

            default:
                break;
        }
    };

    self.saveEmployee = function () {
        if (self.checkEmployeModalValidity() == true) {
            $('#' + self.modalEmployeeTicks).mask();
            Application.Services.CommonService.InsertOrUpdateEmploye(self.Employe, function (result) {
                if (result > 0) {

                    $('#' + self.modalEmployeeTicks).unmask();
                    $("#" + self.modalEmployeeTicks).modal('hide');
                    // Refresh page 
                    self.searchEmploye();
                }
            });
        }
    };

    self.checkEmployeModalValidity = function () {
        if (isDefined(self.Employe) && isDefined(self.Employe.DepartmentId) && self.Employe.DepartmentId > 0
            && isDefined(self.Employe.GroupsId) && self.Employe.GroupsId > 0 && isDefined(self.Employe.Name) && self.Employe.Name != ''
            && isDefined(self.Employe.EntreEntrepriseDate) && self.Employe.EntreEntrepriseDate != '' && isDefined(self.Employe.ExternalId) && self.Employe.ExternalId != ''
            ) {

            return true;
        }
        else {
            if (!isDefined(self.Employe.GroupsId) || self.Employe.GroupsId <= 0 ) {
                $('#EmployeModal_Select_GroupsId').addClass('is-invalid');
            }
            if (!isDefined(self.Employe.DepartmentId) || self.Employe.DepartmentId <= 0) {
                $('#EmployeModal_Select_DepartmentId').addClass('is-invalid');
            }
            if (!isDefined(self.Employe.Name) || self.Employe.Name == '') {
                $('#EmployeModal_Input_Name').addClass('is-invalid');
            }
            if (!isDefined(self.Employe.EntreEntrepriseDate) || self.Employe.EntreEntrepriseDate == '') {
                $('#EmployeModal_Input_EntreEntrepriseDate').addClass('is-invalid');
            }
            if (!isDefined(self.Employe.ExternalId) || self.Employe.ExternalId == '') {
                $('#EmployeModal_Input_ExternalId').addClass('is-invalid');
            }
            $('.selectpicker').selectpicker('refresh');
            return false;
        }
    };

    /* Common zoom */
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

}

Application.ViewsScript["EmployeSearch"] = new EmployeSearch();