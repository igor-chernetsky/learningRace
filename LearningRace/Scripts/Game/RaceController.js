/// <reference path="../kinetic-v4.4.3.min.js" />

raceController = (function () {
    var raceConst = {
        mainHeight: 500,
        mainWidth: 400,
        racerPosY: 400,
        imagesToLoad: ['../Images/Race/finish.png']
    }

    var raceModel = {
        timer: null,
        isStarted: false,
        carShapes: [],
        radarShapes: [],
        images: [],
        gui: {
            speed: null
        },

        currentRacer: null,

        canvas: null,
        layer: null,
        roadlayer: null,
    }

    function drawGui(length) {
        var speedText = new Kinetic.Text({
            x: 280,
            y: raceConst.mainHeight - 80,
            text: 0 + ' km/h',
            fontSize: 35,
            fontFamily: 'Calculator',
            fill: '#b1af00'
        });
        var radarRoad = new Kinetic.Rect({
            x: 260,
            y: 27,
            width: 200,
            height: raceConst.mainHeight - 122,
            fill: '#a0a0a0'
        });
        var lengthText = new Kinetic.Text({
            x: 280,
            y: raceConst.mainHeight - 30,
            text: '0 / ' + length,
            fontSize: 25,
            fontFamily: 'Calculator',
            fill: '#0fa403'
        });
        var uiBack = new Kinetic.Rect({
            x: 260,
            y: raceConst.mainHeight - 95,
            width: 150,
            height: 100,
            fill: '#585858'
        });
        raceModel.layer.add(uiBack);
        raceModel.layer.add(radarRoad);
        raceModel.layer.add(new Kinetic.Text({ x: 270, y: raceConst.mainHeight - 90, text: 'Speed', fill: '#0fa403' }));
        raceModel.layer.add(new Kinetic.Text({ x: 270, y: raceConst.mainHeight - 40, text: 'Distance', fill: '#0fa403' }));
        raceModel.layer.add(speedText);
        raceModel.layer.add(lengthText);
        raceModel.gui.speed = speedText;
        raceModel.gui.length = lengthText;

    }

    function getCarImage(raceCar) {
        var imageObj = new Image();
        imageObj.src = utils.getPathToCarImage(raceCar.Name, raceCar.StringColor);
        return imageObj;
    }

    function drawCars(racers, id, length) {
        ko.utils.arrayFirst(racers, function (item) {
            //radar
            var racer = item();
            var radarItem = new Kinetic.Circle({
                x: 280 + (raceModel.radarShapes.length * 20),
                y: raceConst.mainHeight - 95,
                radius: racer.Id === id ? 7 : 5,
                fill: racer.Car().StringColor,
                stroke: 'black',
                strokeWidth: racer.Id === id ? 2 : 1,
            });
            var radarLine = new Kinetic.Line({
                points: [280 + (raceModel.radarShapes.length * 20), 28, 280 + (raceModel.radarShapes.length * 20), raceConst.mainHeight - 100],
                stroke: '#0EC427'
            });
            raceModel.radarShapes.push(radarItem);
            raceModel.layer.add(radarLine);
            raceModel.layer.add(radarItem);

            //cars
            var imageObj = new Image();

            var car = new Kinetic.Image({
                x: 40 + (raceModel.carShapes.length * 50),
                y: raceConst.racerPosY,
                image: getCarImage(racer.Car()),
                width: 28,
                height: 49
            });
            raceModel.carShapes.push(car);
            raceModel.roadlayer.add(car);
        });
    }

    function drawRoad(length, speed) {
        var road = new Kinetic.Rect({
            x: 30,
            y: -length - 50,
            width: 200,
            height: raceConst.mainHeight + length + 50,
            fill: 'gray',
            strokeWidth: 0
        });
        var imageObj = new Image();
        imageObj.src = raceConst.imagesToLoad[0];
        var finishLine = new Kinetic.Image({
            x: 30,
            y: -length + 400,
            width: 200,
            height: 27,
            fillPatternImage: imageObj,
            fillPatternRepeat: 'repeat-x'
        });
        var grass = new Kinetic.Rect({
            x: 0,
            y: -length - 50,
            width: 260,
            height: raceConst.mainHeight + length + 50,
            fill: '#088A1A'
        });
        raceModel.roadlayer.add(grass);
        raceModel.roadlayer.add(road);

        for (var i = 0 ; i < 3; i++) {
            var whiteLine = new Kinetic.Line({
                points: [80 + i * 50, -length, 80 + i * 50, raceConst.mainHeight],
                stroke: '#fff',
                strokeWidth: 4,
                dash: [50, 5]
            });
            raceModel.roadlayer.add(whiteLine);
        }
        raceModel.roadlayer.add(finishLine);
    }

    function drawInterface() {
        var imageObj = new Image();
        imageObj.src = raceConst.imagesToLoad[0];
        var finishLine = new Kinetic.Image({
            x: 260,
            y: 0,
            width: 150,
            height: 27,
            fillPatternImage: imageObj,
            fillPatternRepeat: 'repeat-x'
        });
        raceModel.layer.add(finishLine);
    }

    function updateCars(racers, id, length) {
        var index = 0
        if (typeof (raceModel.currentRacer) === 'function') {
            var racer = raceModel.currentRacer(), raceCar = racer.Car();
            ko.utils.arrayFirst(racers, function (item) {
                var progress = (raceConst.mainHeight - 125) * item().Length() / length;
                raceModel.radarShapes[index].setY(raceConst.mainHeight - 95 - progress);
                raceModel.carShapes[index].setY(-item().Length() + raceConst.racerPosY);
                index++;
            });
            raceModel.gui.speed.setText(racer.Speed() + ' km/h');
            raceModel.gui.speed.setFill(racer.Speed() > raceCar.AvrSpeed + 10 ? '#0fa403' :
                racer.Speed() < raceCar.AvrSpeed - 10 ? '#b10000' : '#b1af00');
            raceModel.gui.length.setText(racer.Length() + ' / ' + length);
            raceModel.roadlayer.setY(racer.Length());
            raceModel.roadlayer.draw();
            raceModel.layer.draw();
        }
    }

    function loadContent (callback) {
        var loadedImages = 0;
        for (i in raceConst.imagesToLoad) {
            var imageObj = new Image();
            imageObj.onload = function () {
                loadedImages++;
                if (loadedImages == raceConst.imagesToLoad.length) {
                    callback();
                }
            }
            imageObj.src = raceConst.imagesToLoad[i];
        }
    }

    function startRace(raceObj) {
        raceModel.isStarted = true;
        loadContent(function () {
            var stage = new Kinetic.Stage({
                container: 'cnvRace',
                width: raceConst.mainWidth,
                height: raceConst.mainHeight
            });

            raceModel.layer = new Kinetic.Layer();
            raceModel.roadlayer = new Kinetic.Layer();
            raceModel.currentRacer = raceObj.CurrentRacer;

            drawInterface();
            drawGui(raceObj.TotalLength);
            drawRoad(raceObj.TotalLength, raceObj.racers()[0]().Speed());
            drawCars(raceObj.racers(), raceObj.UserId, raceObj.TotalLength);

            stage.add(raceModel.roadlayer);
            stage.add(raceModel.layer);
        });
    }

    return {
        updateRace: function (raceObj) {
            if (raceObj.IsStarted()) {
                if (!raceModel.isStarted) {
                    startRace(raceObj);
                } else {
                    updateCars(raceObj.racers(), raceObj.UserId, raceObj.TotalLength);
                }
            }
        }
    }
})();