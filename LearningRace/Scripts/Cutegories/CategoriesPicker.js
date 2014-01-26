var categoryPicker = function () {
  var _showChildCategories = function (button) {
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

  var _deleteItem = function (button, id) {
    utils.showConfirmation('Are you sure want to remove this category?', function () {
      location.href = '/Category/Delete/' + id;
    });
  }

  return {
    showChildCategories: _showChildCategories,
    deleteItem: _deleteItem
  }
}();