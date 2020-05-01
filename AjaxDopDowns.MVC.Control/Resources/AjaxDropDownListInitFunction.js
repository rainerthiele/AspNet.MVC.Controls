var initAjaxDropDownList = function (listId) {
    var url = $('#' + listId).data('url');
    var value = $('#' + listId).data('value');
    var optionValue = $('#' + listId).data('option-value');
    var optionText = $('#' + listId).data('option-text');
    $.ajax({
        cache: false,
        url: url,
        dataType: "json",
        success: function (data) {
            var items = [];
            var optionStart = "<option value='";
            var optionEnd = "</option>";
            if (typeof optionText !== typeof undefined && optionText !== false) {
                items.push(optionStart + optionValue + "'> " + optionText + optionEnd);
            };
            $.each(data, function (i, item) {
                if (item.Value == value) {
                    items.push(optionStart + item.Value + "' selected='selected'> " + item.Text + optionEnd);
                } else {
                    items.push(optionStart + item.Value + "'>" + item.Text + optionEnd);
                }
            });
            $('#' + listId).html(items.join(""));
        }
    });
};