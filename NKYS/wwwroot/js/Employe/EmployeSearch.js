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
    self.productionValueTypeList = [];

    self.init = function () {
        $('.container').css('min-width', '100%');

        // TODO: Change to deferred, add create disabled when two service is not finished 
        Application.Services.CommonService.GetProductionValueTypeList(null, function (result) {
            if (result != null && result.length > 0) {
                self.productionValueTypeList = result;
            }
        });



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

                if (isDefined(employe.HasTransportFee) && employe.HasTransportFee == true) {
                    employe.HasTransportFeeChecked = true;
                }
                else {
                    employe.HasTransportFeeNoChecked = true;
                }


                if (isDefined(employe.IsChefOfGroup) && employe.IsChefOfGroup == true) {
                    employe.IsChefOfGroupChecked = true;
                }
                else {
                    employe.IsChefOfGroupNoChecked = true;
                }




                // todo reste of column 
                
            });

            $('table#EmployeSearch_Table tbody').loadTemplate($('#Tp1_EmployeTable'), self.employeList);
        }
        else {
            var numberOfColum = $('#EmployeSearch_Table thead tr th').length;
            $('#EmployeSearch_Table tbody').html('<tr><td colspan="' + numberOfColum + '">' + i18next.t('NoData'), + '</td></tr>');
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
                    self.Employe =CloneObject(targetEmploye);
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
            SocialSercurityFee: '',
            SelfPayHousingReserves: '',
            DormFee: null,
            TransportFee: null,
            PositionPay: null,
            IsChefOfGroup: null,
            HasTransportFee: null,
            SeniorityPay:null,
            FixSalary: null,
            IsTemporaryEmploye: false,
            DepartDate: null,
            EmployeDeductionConfigurations: [],
        };
    };

    self.buildEmployeModal = function () {

        self.modalEmployeeTicks = "Modal_" + getTicks();

        Application.Main.initModal(
            self.modalEmployeeTicks,
            'Tp1_EmployeModal',
            { title: isDefined(self.Employe) && isDefined(self.Employe.Id) && self.Employe.Id > 0 ? i18next.t('Edit') : i18next.t('Create') },
            function () { $('#' + self.modalEmployeeTicks).remove(); },
            function () {

                // Step 0: Only number input 
                $('#EmployeModal_Input_FixSalary').numeric({ negative: false, decimal: '.' });
                $('#EmployeModal_Input_TechnicalLevel').numeric({ negative: false, decimal: '.' });
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

                    if (isDefined(self.Employe.FixSalary) && self.Employe.FixSalary != '') {
                        $('#EmployeModal_Input_FixSalary').val(self.Employe.FixSalary);
                    }

                    if (isDefined(self.Employe.TechnicalLevel) && self.Employe.TechnicalLevel != '') {
                        $('#EmployeModal_Input_TechnicalLevel').val(self.Employe.TechnicalLevel);
                    }

                    if (isDefined(self.Employe.SocialSercurityFee) && self.Employe.SocialSercurityFee != '') {
                        $('#EmployeModal_Input_SocialSercurityFee').val(self.Employe.SocialSercurityFee);
                    }

                    if (isDefined(self.Employe.SelfPaySocialSercurityFee) && self.Employe.SelfPaySocialSercurityFee != '') {
                        $('#EmployeModal_Input_SelfPaySocialSercurity').val(self.Employe.SelfPaySocialSercurityFee);
                    }

                    if (isDefined(self.Employe.HousingReservesFee) && self.Employe.HousingReservesFee != '') {
                        $('#EmployeModal_Input_SelfPayHousingReserves').val(self.Employe.HousingReservesFee);
                    }

                    if (isDefined(self.Employe.PositionPay) && self.Employe.PositionPay != '') {
                        $('#EmployeModal_Input_PositionPay').val(self.Employe.PositionPay);
                    }

                    if (isDefined(self.Employe.SeniorityPay) && self.Employe.SeniorityPay != '') {
                        $('#EmployeModal_Input_SeniorityPay').val(self.Employe.SeniorityPay);
                    }


                    if (isDefined(self.Employe.DormFee) ) {
                        $('#EmployeModal_Input_DormFee').val(self.Employe.DormFee);
                    }

                    if (isDefined(self.Employe.HasTransportFee) && self.Employe.HasTransportFee == true) {
                        $('#EmployeModal_CheckBox_HasTransportFee').prop('checked', true);
                    }


                    if (isDefined(self.Employe.IsChefOfGroup) && self.Employe.IsChefOfGroup == true) {
                        $('#EmployeModal_CheckBox_IsChefOfGroup').prop('checked', true);
                    }


                    if (isDefined(self.Employe.IsTemporaryEmploye) && self.Employe.IsTemporaryEmploye == true) {
                        $('#EmployeModal_CheckBox_IsTemporaryEmploye').prop('checked', true);
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


                self.refreshEmployeModalProductionValueConfigurationTable();

                $('button#EmployeModal_Button_Close').on("click", function (e) { $("#" + self.modalEmployeeTicks).modal('hide'); });

                $('body').localize();
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

                            $('[data-specific="TechnicalLevel"]').addClass('d-none');

                            self.Employe.TechnicalLevel = null;
                        }
                        else if (isDefined(targetGroup.SharePropotion) && isDefined(targetGroup.ProductionValueTypeId) ) {

                            $('[data-specific="FixSalary"]').addClass('d-none');

                            $('[data-specific="TechnicalLevel"]').removeClass('d-none');

                            self.Employe.FixSalary = null;
                            self.Employe.EmployeDeductionConfigurations = null;
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
                self.Employe.SelfPaySocialSercurityFee = value;
                break;

            case 'EmployeModal_Input_SocialSercurityFee':
                self.Employe.SocialSercurityFee = value;
                break;


            case 'EmployeModal_Input_SelfPayHousingReserves':
                self.Employe.HousingReservesFee = value;
                break;

            case 'EmployeModal_Input_DormFee':
                self.Employe.DormFee = value;
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

            case 'EmployeModal_CheckBox_HasTransportFee':
                var checked = $("#" + event.currentTarget.id).is(":checked");
                checked = isDefined(checked) ? checked : false;
                self.Employe.HasTransportFee = checked;
                break;

            case 'EmployeModal_Input_SeniorityPay':
                self.Employe.SeniorityPay = value;
                break;

            case 'EmployeModal_Input_FixSalary':
                self.Employe.FixSalary = value;
                break;

            case 'EmployeModal_Input_IsTemporaryEmploye':
                var checked = $("#" + event.currentTarget.id).is(":checked");
                checked = isDefined(checked) ? checked : false;
                self.Employe.IsTemporaryEmploye = checked;
                break;

            case 'EmployeModal_Input_DepartDate':
                self.Employe.DepartDate = value;
                break;

            // ProductionValue configuration modal 
            case 'EmployeProductionValueModal_Select_TypeId':
                self.EmployeDeductionConfiguration.LinkedProductionValueTypeId = value;
                break;

            case 'EmployeProductionValueModal_Input_Value':
                self.EmployeDeductionConfiguration.DeductionSharePropotion = value;
                break;

            default:
                break;
        }
    };

    self.refreshEmployeModalProductionValueConfigurationTable = function () {
        var EmployeDeductionConfigurationTypeIds = []
        if (isDefined(self.Employe) && isDefined(self.Employe.EmployeDeductionConfigurations) && self.Employe.EmployeDeductionConfigurations.length > 0) {

            EmployeDeductionConfigurationTypeIds =  self.Employe.EmployeDeductionConfigurations.map(p => parseInt(p.LinkedProductionValueTypeId));
            var ConfigurationList = self.Employe.EmployeDeductionConfigurations.map((p,index) => {
                p.Ticks = index + 1;

                p.EmployeDeductionConfigurationEditEvent = "Application.ViewsScript['EmployeSearch'].buildEmployeProductionValueConfigurationModal(" + p.Ticks +")";
                p.EmployeDeductionConfigurationDeleteEvent = "Application.ViewsScript['EmployeSearch'].deleteEmployeProductionValueConfigurationModal(" + p.Ticks + ")";

                var TypeLabel = '';
                if (isDefined(self.productionValueTypeList) && self.productionValueTypeList.length >0) {
                    var Type = self.productionValueTypeList.find(x => x.Value == p.LinkedProductionValueTypeId);
                    if (isDefined(Type) && isDefined(Type.Type)) {
                        TypeLabel = Type.Type;
                    }
                }
                p.DeductionSharePropotionlLabel = isDefined(p.DeductionSharePropotion) ?  p.DeductionSharePropotion * 100 + '%' : '';
                p.TypeLabel = TypeLabel;
                return p;
            });
            $('#EmployeModal_EmployeDeductionConfiguration_Table tbody').loadTemplate($('#TP1_EmployeModal_EmployeDeductionConfiguration_Table'), ConfigurationList);
        }
        else { 
            var numberOfColum = $('#EmployeModal_EmployeDeductionConfiguration_Table thead tr th').length;
            $('#EmployeModal_EmployeDeductionConfiguration_Table tbody').html('<tr><td colspan="' + numberOfColum + '">'+ i18next.t('NoData') + '</td></tr>');
        }


        self.Employe.EmployeDeductionConfigurationTypeIds = EmployeDeductionConfigurationTypeIds;
    };

    self.buildEmployeProductionValueConfigurationModal = function (ProductionValueConfigurationTick) {

        self.modalEmployeeProductionValueConfigurationTicks = "Modal_" + getTicks();

        if (isDefined(ProductionValueConfigurationTick) && ProductionValueConfigurationTick && isDefined(self.Employe)
            && isDefined(self.Employe.EmployeDeductionConfigurations) && self.Employe.EmployeDeductionConfigurations.length >0 && self.Employe.EmployeDeductionConfigurations.findIndex(p => p.Ticks == ProductionValueConfigurationTick ) >=0 ) {
            var EmployeDeductionConfiguration = self.Employe.EmployeDeductionConfigurations.find(p => p.Ticks == ProductionValueConfigurationTick)
            if (isDefined(EmployeDeductionConfiguration)) {
                self.EmployeDeductionConfiguration = EmployeDeductionConfiguration;
            }
        }
        else {
            self.EmployeDeductionConfiguration = {
                LinkedProductionValueTypeId: null,
                DeductionSharePropotion: null,
                Ticks: getTicks()
            };
        }
      
        Application.Main.initModal(
            self.modalEmployeeProductionValueConfigurationTicks,
            'Tp1_EmployeProductionValueModal',
            { title: isDefined(ProductionValueConfigurationTick) ? i18next.t('Edit') : i18next.t('Create') },
            function () { $('#' + self.modalEmployeeProductionValueConfigurationTicks).remove(); },
            function () {

                // Step 0: Only number input 
                $('#EmployeProductionValueModal_Input_Value').numeric({ negative: false, decimal: '.' });
                // Step 1: Refresh select
                $('.selectpicker').selectpicker('refresh');
                // Step 2: Build ProductionValue type select options
                if (isDefined(self.productionValueTypeList) && self.productionValueTypeList.length > 0) {

                    var targetedProductionValueTypeList = self.productionValueTypeList.filter(p => {
                        return self.Employe.EmployeDeductionConfigurationTypeIds.indexOf(p.Value) == -1 || (isDefined(self.EmployeDeductionConfiguration.LinkedProductionValueTypeId) && p.Value == self.EmployeDeductionConfiguration.LinkedProductionValueTypeId);
                    });
                    self.buildSelectOption($('#EmployeProductionValueModal_Select_TypeId'), targetedProductionValueTypeList, 'Value', 'Type');
                }
                // Step 3: Bind data 
                if (isDefined(self.EmployeDeductionConfiguration)) {

                    if (isDefined(self.EmployeDeductionConfiguration.LinkedProductionValueTypeId)) {
                        $('#EmployeProductionValueModal_Select_TypeId').val(self.EmployeDeductionConfiguration.LinkedProductionValueTypeId);
                        $('.selectpicker').selectpicker('refresh');
                    }

                    if (isDefined(self.EmployeDeductionConfiguration.DeductionSharePropotion) && self.EmployeDeductionConfiguration.DeductionSharePropotion != '') {
                        $('#EmployeProductionValueModal_Input_Value').val(self.EmployeDeductionConfiguration.DeductionSharePropotion *100);
                    }
                }
                $('button#EmployeProductionValueModal_Button_Close').on("click", function (e) { $("#" + self.modalEmployeeProductionValueConfigurationTicks).modal('hide'); });

                $('body').localize();
                $("#" + self.modalEmployeeProductionValueConfigurationTicks).modal();
            });
        };

    self.saveEmployeProductionValue = function () {
        $('#EmployeProductionValueModal_Select_TypeId').removeClass('is-invalid');
        if (isDefined(self.EmployeDeductionConfiguration) && isDefined(self.EmployeDeductionConfiguration.LinkedProductionValueTypeId)
            && isDefined(self.EmployeDeductionConfiguration.DeductionSharePropotion) && self.EmployeDeductionConfiguration.DeductionSharePropotion !='') {
            var otherDeductionConfiguration = [];
            if (isDefined(self.Employe) && isDefined(self.Employe.EmployeDeductionConfigurations) && self.Employe.EmployeDeductionConfigurations.length > 0) {
                otherDeductionConfiguration  = self.Employe.EmployeDeductionConfigurations.filter(p => p.Ticks != self.EmployeDeductionConfiguration.Ticks);
            }
            self.EmployeDeductionConfiguration.DeductionSharePropotion = self.EmployeDeductionConfiguration.DeductionSharePropotion / 100;
            otherDeductionConfiguration.push(self.EmployeDeductionConfiguration);
            self.Employe.EmployeDeductionConfigurations = otherDeductionConfiguration;

            // Refresh productionValue table 
            self.refreshEmployeModalProductionValueConfigurationTable();
            $("#" + self.modalEmployeeProductionValueConfigurationTicks).modal('hide');
        }
        else {
            if (!isDefined(self.EmployeDeductionConfiguration.LinkedProductionValueTypeId)) {
                $('#EmployeProductionValueModal_Select_TypeId').addClass('is-invalid');
            }
            if (!isDefined(self.EmployeDeductionConfiguration.DeductionSharePropotion) || self.EmployeDeductionConfiguration.DeductionSharePropotion == '') {
                $('#EmployeProductionValueModal_Input_Value').addClass('is-invalid');
            }
        }

        $('.selectpicker').selectpicker('refresh');
    };

    self.deleteEmployeProductionValueConfigurationModal = function (ProductionValueConfigurationTick) {
        if (isDefined(ProductionValueConfigurationTick)) {
            var otherDeductionConfiguration = [];
            if (isDefined(self.Employe) && isDefined(self.Employe.EmployeDeductionConfigurations) && self.Employe.EmployeDeductionConfigurations.length > 0) {
                otherDeductionConfiguration = self.Employe.EmployeDeductionConfigurations.filter(p => p.Ticks != ProductionValueConfigurationTick);
            }
            self.Employe.EmployeDeductionConfigurations = otherDeductionConfiguration;
            self.refreshEmployeModalProductionValueConfigurationTable();
        }
    }

    self.saveEmployee = function () {
        if (self.checkEmployeModalValidity() == true) {
            $('#' + self.modalEmployeeTicks).mask();

            Application.Services.CommonService.CheckExternalIdNotExists({
                EmployeId: self.Employe.Id,
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
                        $('#EmployeModal_ErrorMessage').removeClass('d-none');
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