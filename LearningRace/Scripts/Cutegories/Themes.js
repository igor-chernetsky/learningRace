$(window).ready(function () {
    ThemeViewer.initFilter($('#languagePicker'), 'len');
});

var ThemeViewer = function () {
    var filterOptions = {};

    var _useFilter = function () {
        $('.themeFrame').each(function () {
            var $this = $(this);
            $this.show();

            for (prefix in filterOptions) {
                if (filterOptions[prefix] && !$this.hasClass(filterOptions[prefix])) $this.hide();
            }
        });
    }

    var _initFilter = function ($control, prefix) {
        $control.change(function (e) {
            var selectedOption = $(this).val();
            if (selectedOption === 'all') {
                filterOptions[prefix] = null;
            } else {
                filterOptions[prefix] = selectedOption;
            }
            _useFilter();
        });
    }

    return {
        initFilter: _initFilter
    }
}();