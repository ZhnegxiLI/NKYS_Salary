Application.Main = {
    onload: function () {
        try {
            if (Application.Main.CurrentView != null && Application.ViewsScript[Application.Main.CurrentView]!=null) {
                Application.ViewsScript[Application.Main.CurrentView].init && Application.ViewsScript[Application.Main.CurrentView].init();
            }
        } catch (e) {

        }
    }
}

window.onload = Application.Main.onload;