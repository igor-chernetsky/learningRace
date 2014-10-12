var social = function () {
    var _facebookShare = function () {
        if (FB) {
            FB.ui(
            {
                method: 'share',
                href: window.location,
            },
            function (response) {
                if (response && !response.error_code) {
                    alert('Posting completed.');
                } else {
                    alert('Error while posting.');
                }
            }
          );
        }
    }

    var _addVKShareButton = function ($container, shareData) {
        if ($container.length) {
            var $vkShare = $(VK.Share.button(shareData, { type: "link", text: "Поделиться" }));
            $vkShare.addClass('vkshare');
            $container.append($vkShare);
        }
    }

    return {
        facebookShare: _facebookShare,
        addVKShareButton: _addVKShareButton
    }
}();