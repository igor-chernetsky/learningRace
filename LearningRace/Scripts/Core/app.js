$(document).ready(function() {
    authController.initLoginDropdowns();

    authController.initAuthForm($('.login-menu form'), function () {
        location.reload();
    });

    authController.initAuthForm($('.register-menu form'), function () {
        location.href = '/';
    });

    social.addVKShareButton($('#footer .container'));
});

appData = {};