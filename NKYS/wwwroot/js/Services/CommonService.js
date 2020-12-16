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
    
    
}
Application.Services.CommonService = new CommonService();