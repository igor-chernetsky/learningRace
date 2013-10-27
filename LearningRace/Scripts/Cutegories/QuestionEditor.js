function addVariant() {
    variantNum = $('#variantNumber').val();
    $newVariant = $('<div class="variantBlock">' +
                '<input type="hidden" name="id" value="@variant.Id" />' +
                '<input name="variantValue' + variantNum + '" />' +
                '<input type="radio" name="correctVariant" value="variantValue' + variantNum + '" />' +
                '<input type="button" onclick="removeVariant(this)" value="Delete"></div>');
    $('#variantsContainer').append($newVariant);
    if (variantNum == 0) {
        $newVariant.find('input[name="correctVariant"]').attr('checked', 'checked');
    }
    $('#variantNumber').val(parseInt(variantNum) + 1);
}

function removeVariant(sender) {
    $(sender).parents('.variantBlock').remove();
    if ($('input[name="correctVariant"][checked]').length == 0) {
        $('input[name="correctVariant"]').first().attr('checked', 'checked');
    }
}