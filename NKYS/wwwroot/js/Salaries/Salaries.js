Salaries = function () {
    var self = this;

    self.init = function () {
        console.log(Application.Main.CurrentView);
    }
}

Application.ViewsScript["Salaries"] = new Salaries();