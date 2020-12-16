EmployeSearch = function () {
    var self = this;

    self.searchCriteria = {
        DepartmentId: -1,
        GroupsId: -1,
        Name: null,
        ExternalId: null
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

        self.searchEmploye();
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

            case 'EmployeSearch_Input_Name':
                if (isDefined(value) && value!= '') {
                    self.searchCriteria.Name = value;
                }
                else {
                    self.searchCriteria.Name = null;
                }
                break;

            case 'EmployeSearch_Input_ExternalId':
                if (isDefined(value) && value != '') {
                    self.searchCriteria.ExternalId = value;
                }
                else {
                    self.searchCriteria.ExternalId = null;
                }
                break;

            default:
                break;
        }
        // Check all criteria is completed 
        self.checkEmployeSearchCriteriaValidity();
    };

    self.checkEmployeSearchCriteriaValidity = function () {
        if ((self.searchCriteria.DepartmentId != null && self.searchCriteria.DepartmentId != -1 &&
            self.searchCriteria.GroupsId != null && self.searchCriteria.GroupsId != -1)
            || (isDefined(self.searchCriteria.Name) && self.searchCriteria.Name != '')
            || (isDefined(self.searchCriteria.ExternalId) && self.searchCriteria.ExternalId != '')) {
            $('button#EmployeSearch_Button_Search').removeAttr('disabled');
            return true;
        }
        else {
            $('button#EmployeSearch_Button_Search').prop('disabled', true);
            $('button#EmployeSearch_Button_Create').prop('disabled', true);
            return false;
        }
    };
     
    self.buildEmployeTable = function () {
        if (self.employeList != null && self.employeList.length > 0) {

            self.employeList.map(employe => {

                if (isDefined(employe.Groups)) {
                    if (isDefined(employe.Groups.Name)) {
                        employe.GroupsLabel = employe.Groups.Name;
                    }
                    if (isDefined(employe.Groups.IsFixSalary)) {
                        employe.IsFixSalary = employe.Groups.IsFixSalary;
                    }
                }
                if (isDefined(employe.IsTemporaryEmploye) && employe.IsTemporaryEmploye == true) {
                    employe.TemporaryEmployeChecked = true;
                }
                else {
                    employe.TemporaryEmployeNoChecked = true;
                }

                // todo reste of column 
                
            });

            $('table#EmployeSearch_Table tbody').loadTemplate($('#Tp1_EmployeTable'), self.employeList);
        }
        else {
            var numberOfColum = $('#EmployeSearch_Table thead tr th').length;
            $('#EmployeSearch_Table tbody').html('<tr><td colspan="' + numberOfColum+ '">No data to display</td></tr>')
        }
    };

    self.searchEmploye = function () {
        if (self.checkEmployeSearchCriteriaValidity() == true) {
            Application.Services.CommonService.GetEmployeList(self.searchCriteria, function (result) {
                $('button#EmployeSearch_Button_Create').removeAttr('disabled');
                if (result != null && result.length > 0) {
                    self.employeList = result;
                }
                else {
                    self.employeList = [];
                }

                self.buildEmployeTable();
            });
        }
        else {
            self.employeList = [];
            self.buildEmployeTable();
        }
    };

    /* Create / update employee zoom  */
    self.initEmployeModal = function (event) {
        var action = $(event.currentTarget).data('action');
        if (action == 'Create') {
           self.Employe = self.getEmptyEmploye();
        }
        else if (action == 'Edit') {
            var employeId = $(event.currentTarget).closest('tr').attr('data-employe-id');
            if (isDefined(self.employeList) && self.employeList.length>0) {
                var targetEmploye = self.employeList.find(p => p.Id == employeId);

                if (isDefined(targetEmploye)) {
                    self.Employe = targetEmploye;
                }
            }
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
                // Step 1: Refresh select
                $('.selectpicker').selectpicker('refresh');
                // Step 2: Build Department select options
                if (isDefined(self.departmentList) && self.departmentList.length>0) {
                    self.buildSelectOption($('#EmployeModal_Select_DepartmentId'), self.departmentList, 'Id', 'Name');
                }
                // Step 3: Bind data 
                if (isDefined(self.Employe) && isDefined(self.Employe.Id) && self.Employe.Id >0) {
                    if (isDefined(self.Employe.Groups) && isDefined(self.Employe.Groups.DepartmentId) && self.Employe.Groups.DepartmentId >0) {
                        $('#EmployeModal_Select_DepartmentId').val(self.Employe.Groups.DepartmentId);
                        $('.selectpicker').selectpicker('refresh');
                        $('#EmployeModal_Select_DepartmentId').trigger('change');

                        if (isDefined(self.Employe.GroupsId)) {
                            $('#EmployeModal_Select_GroupsId').val(self.Employe.GroupsId);
                            $('.selectpicker').selectpicker('refresh');

                            $('#EmployeModal_Select_GroupsId').trigger('change');
                        }
                    }

                    if (isDefined(self.Employe.Name) && self.Employe.Name!='') {
                        $('#EmployeModal_Input_Name').val(self.Employe.Name);
                    }

                    if (isDefined(self.Employe.EntreEntrepriseDate) && self.Employe.EntreEntrepriseDate != '') {
                        $('#EmployeModal_Input_EntreEntrepriseDate').val(self.Employe.EntreEntrepriseDate.slice(0,10));
                    }

                    if (isDefined(self.Employe.ExternalId) && self.Employe.ExternalId != '') {
                        $('#EmployeModal_Input_ExternalId').val(self.Employe.ExternalId);
                    }

                    if (isDefined(self.Employe.TechnicalLevel) && self.Employe.TechnicalLevel != '') {
                        $('#EmployeModal_Input_TechnicalLevel').val(self.Employe.TechnicalLevel);
                    }

                    if (isDefined(self.Employe.FixSalary) && self.Employe.FixSalary != '') {
                        $('#EmployeModal_Input_FixSalary').val(self.Employe.FixSalary);
                    }
                }
                else {
                    // If department and group is selected in search, bind automaticly in creation employee
                    if (isDefined(self.searchCriteria.DepartmentId) && self.searchCriteria.DepartmentId>0) {
                        $('#EmployeModal_Select_DepartmentId').val(self.searchCriteria.DepartmentId);
                        $('.selectpicker').selectpicker('refresh');
                        $('#EmployeModal_Select_DepartmentId').trigger('change');

                        if (isDefined(self.searchCriteria.GroupsId) && self.searchCriteria.GroupsId>0) {
                            $('#EmployeModal_Select_GroupsId').val(self.searchCriteria.GroupsId);
                            $('.selectpicker').selectpicker('refresh');

                            $('#EmployeModal_Select_GroupsId').trigger('change');
                        }
                    }
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


                    // Get group information 
                    var targetGroup = self.groupList.find(p => p.Id == value);
                    if (isDefined(targetGroup)) {
                        if (isDefined(targetGroup.IsFixSalary) && targetGroup.IsFixSalary == true) {
                            $('[data-specific="FixSalary"]').removeClass('d-none');
                            $('[data-specific="DeductionPercentage"]').removeClass('d-none');
                            $('[data-specific="DeductionConfiguration"]').removeClass('d-none');
                            
                            $('[data-specific="TechnicalLevel"]').addClass('d-none');

                            self.Employe.TechnicalLevel = null;
                        }
                        else if (isDefined(targetGroup.SharePropotion) && isDefined(targetGroup.ProductionValueTypeId) ) {

                            $('[data-specific="FixSalary"]').addClass('d-none');
                            $('[data-specific="DeductionPercentage"]').addClass('d-none');
                            $('[data-specific="DeductionConfiguration"]').addClass('d-none');

                            $('[data-specific="TechnicalLevel"]').removeClass('d-none');

                            self.Employe.FixSalary = null;
                            self.Employe.DeductionPercentage = null;
                        }
                        else {
                            $('[data-specific]').removeClass('d-none');
                        }
                    }
                    else {
                        $('[data-specific]').removeClass('d-none');
                    }
                }
                else {
                    $('[data-specific]').removeClass('d-none');
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
                self.Employe.FixSalary = value;
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

            Application.Services.CommonService.CheckExternalIdNotExists({
                EmployeId: self.Employe.id,
                ExternalId: self.Employe.ExternalId
            }, function (employeWithSameExternalId) {
                    if (!isDefined(employeWithSameExternalId)) {
                        Application.Services.CommonService.InsertOrUpdateEmploye(self.Employe, function (result) {
                            if (result > 0) {

                                $('#' + self.modalEmployeeTicks).unmask();
                                $("#" + self.modalEmployeeTicks).modal('hide');
                                // Refresh page 
                                self.searchEmploye();
                            }
                        });
                    }
                    else {
                        $('#' + self.modalEmployeeTicks).unmask();
                        // TODO translate error message 
                        $('#EmployeModal_ErrorMessage').html('Cannot save the employee with the same externalId, externalId should be unique');
                    }
            });
        }
    };

    self.checkEmployeModalValidity = function () {
        var salaryModelValidity = false;
        var targetGroup = self.groupList.find(p => p.Id == self.Employe.GroupsId);
        if (isDefined(targetGroup)) {
            if (isDefined(targetGroup.IsFixSalary) && targetGroup.IsFixSalary == true && isDefined(self.Employe.FixSalary) && self.Employe.FixSalary>0) {
                salaryModelValidity = true;
            }
            else if (isDefined(targetGroup.SharePropotion) && isDefined(targetGroup.ProductionValueTypeId) && isDefined(self.Employe.TechnicalLevel) && self.Employe.TechnicalLevel>0) {
                salaryModelValidity = true;
            }
        }

        if (isDefined(self.Employe) && isDefined(self.Employe.DepartmentId) && self.Employe.DepartmentId > 0
            && isDefined(self.Employe.GroupsId) && self.Employe.GroupsId > 0 && isDefined(self.Employe.Name) && self.Employe.Name != ''
            && isDefined(self.Employe.EntreEntrepriseDate) && self.Employe.EntreEntrepriseDate != '' && isDefined(self.Employe.ExternalId) && self.Employe.ExternalId != ''
            && salaryModelValidity==true) {

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