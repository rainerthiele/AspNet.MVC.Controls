var initScriptDropDownList = function (listId, data) {
    var value = $('#' + listId).data('value');
    var optionValue = $('#' + listId).data('option-value');
    var optionText = $('#' + listId).data('option-text');
    var items = [];
    var optionStart = "<option value='";
    var optionEnd = "</option>";
    if (typeof optionText !== typeof undefined && optionText !== false) {
        items.push("<option value='" + optionValue + "'> " + optionText + optionEnd);
    };
    $.each(data, function (i, item) {
        if (item[0] == value) {
            items.push(optionStart + item[0] + "' selected='selected'> " + item[1] + optionEnd);
        } else {
            items.push(optionStart + item[0] + "'>" + item[1] + optionEnd);
        }
    });
    $('#' + listId).html(items.join(""));
};
