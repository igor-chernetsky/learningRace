var core = {
    imagesToLoad: [],
    loadedContentCount: 0,
    callback: null,

    percentLoaded: function () {
        return core.imagesToLoad.length == 0 ? 100 : (100 / core.imagesToLoad.length) * loadedContentCount;
    },

    loadContent: function (callback, images) {
        if (images) {
            core.imagesToLoad = images;
            core.callback = callback;
        }
        else {
            callback();
        }
        core.loadedContentCount = 0;
        for (var i = 0; i < core.imagesToLoad.length; i++) {
            loadImage(core.imagesToLoad[i]);
        }
    },

    onLoadContent: function () {
        gameCore.loadedContentCount++;
        if (core.loadedContentCount == core.imagesToLoad.length && core.callback) {
            core.callback();
        }
    },

    loadImage: function (path) {
        var objToLoad;
        objToLoad = new Image();
        objToLoad.onload = onLoadContent;
        objToLoad.src = path;
    }
}