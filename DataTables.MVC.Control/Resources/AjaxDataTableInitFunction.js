var initAjaxDataTable = function (tableId, colConfig, cbf) {
    var languageFile = $('#' + tableId).data("languagefile");
    var ajaxUrl = $('#' + tableId).data("ajaxurl");
    var ajaxType = "GET";
    if ($('#' + tableId).data("server-side") === true)
        ajaxType = "POST";
    var dt = $('#' + tableId).DataTable({
        ajax: { "url": ajaxUrl, "type": ajaxType },
        language: { "url": languageFile },
        columnDefs: colConfig
    });
    if (cbf !== undefined) {
        dt.on('draw', cbf);
    }
};