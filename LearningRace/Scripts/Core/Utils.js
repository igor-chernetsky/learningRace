var utils = function () {

  var _showMessage = function (message, title) {
    alert(message);
  }

  var _showConfirmation = function (message, callbackYes, callbackNo) {
    if (confirm(message)) {
      if (callbackYes && typeof callbackYes === 'function') {
        callbackYes();
      }
    } else {
      if (callbackNo && typeof callbackNo === 'function') {
        callbackNo();
      }
    }
  }

  return {
    showMessage: _showMessage,
    showConfirmation: _showConfirmation
  }

}();