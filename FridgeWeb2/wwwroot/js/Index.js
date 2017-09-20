function onSaveComplete(data) {
    alertify.set('notifier', 'position', 'top-right');
    if (data.updated === data.sent) {
        alertify.success("All picks saved.");
    }
    else {
        alertify.warning(data.updated + " / " + data.sent + " picks saved.");
    }
    $("#savePicks").prop("disabled", false);
}

