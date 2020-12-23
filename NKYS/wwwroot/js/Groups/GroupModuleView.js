GroupModuleView = function () {

    var self = this;

    self.init = function () {
        $('#GroupModuleView_Checkbox_IsFixSalary').trigger('change');
    }

    self.onChangeGroupsField = function(event) {
        var Id = event.target.id;
        var value = event.target.value;
        switch (Id) {
            case 'GroupModuleView_Checkbox_IsFixSalary':
                var checked = $("#" + event.target.id).is(":checked");
                checked = isDefined(checked) ? checked : false;
                if (checked == true) {
                    $('#GroupModuleView_Input_SharePropotion').val('');
                    $('#GroupModuleView_Input_GroupVariableSharePropotion').val('');
                    $('#GroupModuleView_Select_ProductionValueTypeId').val('');

                    $('[data-specific="ProductionValueTypeId"],[data-specific="GroupVariableSharePropotion"],[data-specific="SharePropotion"]').addClass('d-none');
                }
                else {

                    $('[data-specific="ProductionValueTypeId"],[data-specific="GroupVariableSharePropotion"],[data-specific="SharePropotion"]').removeClass('d-none');
                }
                break;
            default:
                break;

        }
       
    }
}

Application.ViewsScript["GroupModuleView"] = new GroupModuleView();