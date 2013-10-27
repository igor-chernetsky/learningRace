function showChildCategories(button) {
    var id = $(button).attr('categoryId');
    var childContainer = $("div [parentId=" + id + "]");
    if (childContainer.css('display') == 'none') {
        $(button).val('Hide Child Categories');
        childContainer.show();
    }
    else {
        $(button).val('Show Child Categories');
        childContainer.hide();
    }
}