CommonService = function () {
    var self = this;
  
    self.FindGroupList = function (criteira, cb) {
        $.ajax({
            url: 'Groups/FindGroupList', // Add into config 
            type: "get",
            data: criteira,
            dataType: "json",
            success: cb 
            });
    };
}
Application.Services.CommonService = new CommonService();