SalaryCalculModule = function () {
    var self = this;

    this.show = function (config) {
        self.config = config;
      
        self.buildSalaryCalculModal();
    };
    self.buildSalaryCalculModal = async function () {

        if (isDefined(self.config) && isDefined(self.config.CycleId)) {

            var checkResult = await self.CheckSalaryCalculValidity();

            if (isDefined(checkResult) && !(isDefined(checkResult.EmployeeList) && checkResult.EmployeeList.length > 0)
                && !(isDefined(checkResult.ProductionValueList) && checkResult.ProductionValueList.length > 0)) {

                self.InitCalculSalary(false, function (salariesResult) {
                    self.BuildSalaryTableModal(salariesResult);
                })
            }
            else {
                var errorMessage = '';
                if (isDefined(checkResult.ProductionValueList) && checkResult.ProductionValueList.length > 0) {
                    errorMessage += 'Production value not inputed: <br>'
                    checkResult.ProductionValueList.map(p => errorMessage += p + '<br>');
                }
                if (isDefined(checkResult.EmployeeList) && checkResult.EmployeeList.length > 0) {
                    errorMessage += 'Salary is not inputed: <br>'
                    checkResult.EmployeeList.map(p => errorMessage += p.Name + ' ' + '<br>');
                }


                if (errorMessage != '') {
                    Application.Main.showError(errorMessage);
                }
            }
        }
    };

    self.BuildSalaryTableModal = function (salariesResult) {
        $('body').mask();
        self.modalSalaryCalculTicks = "Modal_" + getTicks();
        Application.Main.initModal(
            self.modalSalaryCalculTicks,
            'Tp1_SalaryCalculModal',
            null,
            function () { $('#' + self.modalSalaryCalculTicks).remove(); },
            function () {
                if (isDefined(salariesResult) && salariesResult.length > 0) {
                    $('#EmployeProductionValueModal_SalaryTable tbody').loadTemplate($('#Tp1_SalaryCalcul_Table'), salariesResult);
                }
                else {
                    var numberOfColum = $('#EmployeProductionValueModal_SalaryTable thead tr th').length;
                    $('#EmployeProductionValueModal_SalaryTable tbody').html('<tr><td colspan="' + numberOfColum + '">No data to display</td></tr>');
                }
                $('body').unmask();
                $('button#SalaryCalculModal_Button_Close').on("click", function (e) { $("#" + self.modalSalaryCalculTicks).modal('hide'); });

                $('body').localize();
                $("#" + self.modalSalaryCalculTicks).modal();
            });
    };


    self.InitSalaryValidation = function () {
        return Application.Services.CommonService.GetSalariesValidationList({
            CycleId: self.config.SalariesSearchCriteria.CycleId
        });  
    };

    self.InitCalculSalary = function (IsUpdate, cb) {
        Application.Services.CommonService.CalculSalaries({
            CycleId: self.config.CycleId,
            IsUpdate: isDefined(IsUpdate) ? IsUpdate: false
        }, function (result) {
                cb && cb(result);
        });
    }

    self.updateSalary = function () {
        $('#' + self.modalSalaryCalculTicks).mask();
        self.InitCalculSalary(true, function () {
            $('#' + self.modalSalaryCalculTicks).unmask();
            $('#EmployeProductionValueModal_SuccessMessage').removeClass('d-none');
        });
    }

    self.CheckSalaryCalculValidity = function () {
       return  Application.Services.CommonService.CheckSalaryCalculValidity({
            CycleId: self.config.CycleId
        });  
    }
}
Application.Main.Controls.SalaryCalculModule = new SalaryCalculModule();