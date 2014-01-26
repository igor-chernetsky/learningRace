function addVariant() {
    variantNum = $('#variantNumber').val();
    $newVariant = $('<div class="variantBlock">' +
                '<input type="hidden" name="id" value="@variant.Id" />' +
                '<input name="variantValue' + variantNum + '" required />' +
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

$(document).ready(function () {
    editor = new TINY.editor.edit('editor', {
        id: 'QuestionText',
        width: 584,
        height: 175,
        cssclass: 'tinyeditor',
        controlclass: 'tinyeditor-control',
        rowclass: 'tinyeditor-header',
        dividerclass: 'tinyeditor-divider',
        controls: ['bold', 'italic', 'underline', 'strikethrough', '|', 'subscript', 'superscript', '|', 'orderedlist', 'unorderedlist', '|', 'outdent', 'indent', '|', 'leftalign', 'centeralign', 'rightalign', 'blockjustify', '|', 'unformat', '|', 'undo', 'redo', 'n', 'font', 'size', 'style', '|', 'image', 'hr', 'link', 'unlink', '|', 'print'], // (required) options you want available, a '|' represents a divider and an 'n' represents a new row
        footer: true,
        fonts: ['Verdana', 'Arial', 'Georgia', 'Trebuchet MS'],
        xhtml: true,
        cssfile: '../Content/stylesheets/tinyeditor.css',
        bodyid: 'tinyeditor', // (optional) attach an ID to the editor body
        footerclass: 'tinyeditor-footer',
        toggle: { text: 'source', activetext: 'wysiwyg', cssclass: 'toggle' } // (optional) toggle to markup view options
    });

    $('#saveQuestionBtn').click(function () {
        editor.post();
    });
});