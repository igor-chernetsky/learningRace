var authController = (function () {
    var _initAuthForm = function ($form, success) {
        $form.submit(function () {
            var url = $form.attr('action');
            var data = $form.serialize();
            var $message = $form.find('.alert-message');

            $.ajax({
                type: "POST",
                url: url,
                data: data, // serializes the form's elements.
                success: function (data) {
                    if (data.status === 0) {
                        location.reload();
                    } else {
                        $message.text(data.status).show();
                        setTimeout(function () {
                            $message.fadeOut(function () { $message.text(''); });
                        }, 3000);
                    }
                }
            });

            return false;
        });
    };

    var _initLoginDropdowns = function () {
        $('html').click(function () {
            $('.dropdown').removeClass('open');
        });

        $('.dropdown-menu').click(function (event) {
            event.stopPropagation();
        });

        $('.dropdown-toggle').click(function (event) {
            $('.dropdown').removeClass('open');
            var $dropdown = $(this).parents('.dropdown');
            $dropdown.toggleClass('open');
            event.stopPropagation();
        });
    }

    return {
        initAuthForm: _initAuthForm,
        initLoginDropdowns: _initLoginDropdowns
    };
})();