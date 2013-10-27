$(document).ready(function () { 
    $('html').click(function () {
        $('.dropdown').removeClass('open');
    });

    $('.dropdown-menu').click(function (event) {
        event.stopPropagation();
    });

    $('.dropdown-toggle').click(function (event) {
        var $dropdown = $(this).parents('.dropdown');
        $dropdown.toggleClass('open');
        event.stopPropagation();
    });
});