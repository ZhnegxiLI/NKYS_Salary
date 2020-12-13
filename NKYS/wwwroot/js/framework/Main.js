Application.Main = {

    PileInstantiatedModal: [],

    onload: function () {
        try {
            if (Application.Main.CurrentView != null && Application.ViewsScript[Application.Main.CurrentView]!=null) {
                Application.ViewsScript[Application.Main.CurrentView].init && Application.ViewsScript[Application.Main.CurrentView].init();
            }
        } catch (e) {

        }
    },

    addModalInPile: function (modalId) {
        var modalCanBeAdded = false;

        if (!$('#' + modalId).is(':visible')) {
            var ticks = getTicks();
            var alreadyExist = getElementOfArrayByPropertyValue(Application.Main.PileInstantiatedModal, 'ID', modalId);

            if (alreadyExist.length == 1) {

                if ((ticks - alreadyExist[0].Ticks) > 30000) { // Instantiated more than 30 seconds ago

                    modalCanBeAdded = true;
                    for (var i = 0; i < Application.Main.PileInstantiatedModal.length; i++) {

                        if (Application.Main.PileInstantiatedModal[i].ID == modalId) {

                            delete Application.Main.PileInstantiatedModal[i];
                            Application.Main.PileInstantiatedModal.splice(i, 1);
                        }
                    }
                    $('#' + modalId).remove();

                    Application.Main.PileInstantiatedModal.push({
                        'ID': modalId,
                        'Ticks': ticks
                    });

                    console.log("Crystal => addModalInPile : a modal was instantiated in the pile " + (ticks - alreadyExist[0].Ticks) + " milliseconds ago (> 30000), it has now been removed.");
                }
                else {
                    console.log("Crystal => addModalInPile : a modal is instantiated in the pile " + (ticks - alreadyExist[0].Ticks) + " milliseconds ago (< 30000) !");
                }
            }
            else if (alreadyExist.length == 0) {
                modalCanBeAdded = true;
                Application.Main.PileInstantiatedModal.push({
                    'ID': modalId,
                    'Ticks': ticks
                });
            }
        }
        else {
            console.log("Crystal => addModalInPile : a modal with same id is currently visible");
        }

        return modalCanBeAdded;
    },

    initModal: function (modalId, templateModalId, templateModalObject, cbOnHidden, cbOnInit) {

        var isCorrect = Application.Main.addModalInPile(modalId);

        if (isCorrect) {

            //aria-hidden='true' empêche le clavier de s'ouvrir sur EDGE (app windows10)
            $('body').append("<div id='" + modalId + "' data-backdrop='static' class='modal' tabindex='-1' role='dialog' aria-hidden='false'></div>");
            $('#' + modalId).loadTemplate($('#' + templateModalId), templateModalObject);

            $('#' + modalId).on('hidden.bs.modal', function () {
                Application.Main.forceCloseModal(modalId);
                cbOnHidden && cbOnHidden();
            });

            $('#' + modalId).on('shown.bs.modal', function () {
            });

            cbOnInit && cbOnInit();
        }
    },

    forceCloseModal: function (modalId) {
        $('#' + modalId).modal('hide');
        $('#' + modalId).html("");
        $('#' + modalId).remove();
        $('[data-renderto="' + modalId + '"]').remove();
        Application.Main.removeModalFromPile(modalId);
    },

    removeModalFromPile: function (modalId) {
        if ($('#' + modalId).length == 0) {
            for (var i = 0; i < Application.Main.PileInstantiatedModal.length; i++) {

                if (Application.Main.PileInstantiatedModal[i].ID == modalId) {

                    delete Application.Main.PileInstantiatedModal[i];
                    Application.Main.PileInstantiatedModal.splice(i, 1);
                    break;
                }
            }
        }
    },
}

window.onload = Application.Main.onload;