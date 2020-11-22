

$(document).ready(function () {
    $('#DeleteCycle').on('click', function (event) {
        var cycleId = $(event.currentTarget).attr('data-cycle-id');
        $.ajax({
            url: '/Cycles/Delete',
            type:'POST',
            context: document.body,
            data: {
                id: cycleId
            },
            success: {

            }
        });
    });
});