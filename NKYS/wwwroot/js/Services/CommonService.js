CommonService = function () {
    var self = this;
  
    self.FindGroupList = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl + 'Groups/FindGroupList', // Add into config 
            type: "get",
            data: criteira,
            dataType: "json",
            success: cb 
            });
    };

    self.GetEmployeList = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl +'Employes/GetEmployeList', // Add into config 
            type: "get",
            data: criteira,
            dataType: "json",
            success: cb
        });
    };

    self.SalariesSearch = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl + 'Salaries/SalariesSearch', // Add into config 
            type: "get",
            data: criteira,
            dataType: "json",
            success: cb
        });
    };

    
    self.ExportSalariesWorkingHours = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl + 'Export/ExportSalariesWorkingHours', // Add into config 
            type: "get",
            data: criteira,
          //  dataType: "json",
            success: cb
        });
    };

    self.SaveSalaries = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl + 'Salaries/SaveSalaries', // Add into config 
            type: "post",
            data: criteira,
            dataType: "json",
            success: cb
        });
    };

    self.SalariesValidation = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl + 'Salaries/SalariesValidation', // Add into config 
            type: "post",
            data: criteira,
            dataType: "json",
            success: cb
        });
    };

    self.CalculSalaries = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl + 'Salaries/CalculSalaries', // Add into config 
            type: "post",
            data: criteira,
            dataType: "json",
            success: cb
        });
    };

    self.CheckSalaryCalculValidity = function (criteira, cb) {
        return  $.ajax({
            url: Application.Configuration.baseUrl + 'Salaries/CheckSalaryCalculValidity', // Add into config 
            type: "get",
            data: criteira,
            dataType: "json"
        });
    };
    
    
    

    self.FindDepartmentList = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl +'Departments/FindDepartmentList', // Add into config 
            type: "get",
            data: criteira,
            dataType: "json",
            success: cb
        });
    };

    self.InsertOrUpdateEmploye = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl + 'Employes/InsertOrUpdateEmploye', // Add into config 
            type: "post",
            data: criteira,
            dataType: "json",
            success: cb
        });
    };

    self.CheckExternalIdNotExists = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl + 'Employes/CheckExternalIdNotExists', // Add into config 
            type: "get",
            data: criteira,
            dataType: "json",
            success: cb
        });
    };

    self.GetProductionValueTypeList = function (criteira, cb) {
        $.ajax({
            url: Application.Configuration.baseUrl + 'ProductionValues/GetProductionValueTypeList', // Add into config 
            type: "get",
            data: criteira,
            dataType: "json",
            success: cb
        });
    };
    
}
Application.Services.CommonService = new CommonService();