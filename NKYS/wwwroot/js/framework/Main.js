Application.Main = {

    PileInstantiatedModal: [],

    Controls: {},

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

    showError: function (message, cb) {
        Application.Main.showMessage(message, cb, 'Error');
    },
    showConfirm: function (message, cb, isImportant, pModalId) {
        Application.Main.showMessage(message, cb, 'Confirmation', isImportant, pModalId);
    },

    showMessage: function (message, cb, title, isImportant, pModalId, pCbOnInit) {

        title = isDefined(title) ? title : translate('Confirmation');

        var modalMessageId = "Modal_" + getTicks();
        if (isDefined(pModalId)) {
            modalMessageId = pModalId;
        }
        Application.Main.callBackDynamicMessageModal[modalMessageId] = function () {
            cb && cb();
        };

        var objectModal = {
            titleModal: title,
            message: message,
            labelClose: "<span aria-hidden='true'>×</span><span class='sr-only'>Close</span>",
            labelCancel: translate("Cancel"),
            labelYes: translate("Yes"),
            onclick: "Application.Main.callBackDynamicMessageModal['" + modalMessageId + "']();",
            cssClass: "modal-header"
        };

        if (title == translate('Error')) {
            objectModal.cssClass += " error";
            objectModal.labelYes = translate("OK");
            delete objectModal.labelCancel;
            delete objectModal.labelClose;
        }
        else if (title == translate('Information')) {
            //objectModal.labelClose = "<span aria-hidden='true'>×</span><span class='sr-only'>Close</span>";
            objectModal.labelYes = translate("OK");
            delete objectModal.labelCancel;
        }
        else if (title == translate('Warning')) {
            objectModal.cssClass += " warning";
            delete objectModal.labelCancel;
        }
        else if (title == translate('Success')) {
            objectModal.labelYes = translate("OK");
            delete objectModal.labelCancel;
        }

        if (isDefined(isImportant) && isImportant == true) {
            objectModal.labelYes = translate("Yes");
            objectModal.labelCancel = translate("No");
            objectModal.cssClass += " error";
            delete objectModal.labelClose;
        }

        Application.Main.initModal(
            modalMessageId,
            'tplConfirmDelete',
            objectModal,
            function () {
                delete Application.Main.callBackDynamicMessageModal[modalMessageId];
            },
            function () {
                $("#" + modalMessageId).modal();
                pCbOnInit && pCbOnInit(modalMessageId);
            }
        );
        /*$("body").append("<div id='" + modalMessageId + "' class='modal' tabindex='-1' role='dialog' aria-hidden='true'></div>");
        $("#" + modalMessageId).loadTemplate($('#tplConfirmDelete'), objectModal);*/
        /*$("#" + modalMessageId).on("hidden.bs.modal", function () {
            $("#" + modalMessageId).modal('hide');
            $("#" + modalMessageId).html("");
            $("#" + modalMessageId).remove();
            delete Application.SharedControls.callBackDynamicMessageModal[modalMessageId];
        });*/
    },

    callBackDynamicMessageModal: {

    },
}

window.onload = Application.Main.onload;