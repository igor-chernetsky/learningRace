if (typeof controller === 'undefined') var controller = {};

controller.hallOfFave = function () {
    var _init = function () {
        $('.icons-fbshare-icon').on('click', function () {
            FB.ui({
                    method: 'feed',
                    name: 'Learning Race',
                    link: 'http://tut.by',
                    caption: 'I\'ve got ' + appData.score + ' points in the racing!',
                },  function(response) {
                    if (response && response.post_id) {

                    }
                }
            );
        });

        var vkShareData = {
            url: window.location,
            description: 'Я наездил на ' + appData.score + ' очков, догоняй!',
            title: 'тестовый заголовок'
        };
        social.addVKShareButton($('.shareContainer'), vkShareData);
    };

    return {
        init: _init
    };
}();

$(document).ready(function () {
    controller.hallOfFave.init();
});