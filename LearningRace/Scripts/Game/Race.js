var raceDetails = {
    raceObj: null,
    racerName:null,
    isStarted:false,
    timer: null,
    imagesToLoad: [],
    images: [],
    roadShape: null,
    carShapes: [],
    radarShapes: [],
    loadedContentCount: 0,
    callback: null,
    canvas: null,
    stage: null,
    screen_width: 0,
    screen_height: 0,
    racerOffset: 340,

    currentRacer: null,

    radar: {
        pos: { x: 250, y: 60 },
        height: 370,
        width: 80
    },

    labels: {
        speed: null,
        position: null,
        length: null
    },

    getCurrentRacer: function () {
        for (var i = 0; i < this.raceObj.Racers.length; i++) {
            if (this.raceObj.Racers[i].Racer.UserName == raceDetails.racerName) {
                return this.raceObj.Racers[i];
            }
        }
        return null;
    },

    percentLoaded: function () {
        return raceDetails.imagesToLoad.length == 0 ? 100 : (100 / raceDetails.imagesToLoad.length) * loadedContentCount;
    },

    loadContent: function (callback, images) {
        if (images) {
            raceDetails.imagesToLoad = images;
            raceDetails.callback = callback;
        }
        else {
            callback();
        }
        raceDetails.loadedContentCount = 0;
        for (var i = 0; i < raceDetails.imagesToLoad.length; i++) {
            raceDetails.loadImage(raceDetails.imagesToLoad[i]);
        }
    },

    onLoadContent: function () {
        raceDetails.loadedContentCount++;
        if (raceDetails.loadedContentCount == raceDetails.imagesToLoad.length && raceDetails.callback) {
            raceDetails.callback();
        }
    },

    loadImage: function (path) {
        var objToLoad;
        objToLoad = new Image();
        objToLoad.onload = raceDetails.onLoadContent;
        objToLoad.src = path;
        raceDetails.images.push(objToLoad);
    }
}

function startRace() {
    raceDetails.stage = new createjs.Stage(raceDetails.canvas);
    raceDetails.screen_width = raceDetails.canvas.width;
    raceDetails.screen_height = raceDetails.canvas.height;

    raceDetails.timer = setInterval(function () {
        changePosition();
    }, 1000);
    raceDetails.stage.removeAllChildren();
    drwaUI();
    drawCars();
}

function changePosition() {
    for (var i = 0; i < raceDetails.raceObj.Racers.length; i++) {
        var racer = raceDetails.raceObj.Racers[i];
        if (racer.Speed > racer.AvrSpeed) {
            racer.Speed -= raceDetails.raceObj.DSpeed;
        }
        if (racer.Speed < racer.AvrSpeed) {
            racer.Speed += raceDetails.raceObj.DSpeed;
        }
        racer.Length += racer.Speed;
        if (racer.Length >= raceDetails.raceObj.Length) {
            racer.Speed = 0;
        }
        if (raceDetails.raceObj.IsFinished) {
            clearInterval(raceDetails.timer);
        }
    }
    updateRaceUI();
    updateRaceInfo();
    updateCars();
}

function drawCars() {
    drawRoad();
    for (var index = 0; index < raceDetails.raceObj.Racers.length; index++) {
        var rGraph = drawRadarCar(index);
        raceDetails.radarShapes[index] = new createjs.Shape(rGraph);
        raceDetails.stage.addChild(raceDetails.radarShapes[index]);

        var y = raceDetails.screen_height - 100;
        var x = 35 + (index * 40);
        graph = drawCar(index);
        raceDetails.carShapes[index] = new createjs.Shape(graph);
        raceDetails.carShapes[index].x = x;
        raceDetails.carShapes[index].y = raceDetails.racerOffset;
        raceDetails.stage.addChild(raceDetails.carShapes[index]);
    }
    raceDetails.stage.update();
}

function drawCar(index) {
    var racer = raceDetails.raceObj.Racers[index];
    var g = new createjs.Graphics();
    g.beginBitmapFill(raceDetails.images[1], 'repeat');
    g.drawRect(0, 0, 30, 50);
    return g;
}

function drawRoad() {
    var graph = new createjs.Graphics();
    graph.beginFill('#777');
    //road
    var roadEndY = -raceDetails.raceObj.Length;
    var finishY = roadEndY + raceDetails.racerOffset + 50;
    var roadLength = raceDetails.raceObj.Length + raceDetails.screen_height;
    graph.drawRect(30, roadEndY, 180, roadLength);
    graph.beginFill('#0A0');
    graph.drawRect(0, roadEndY, 30, roadLength);
    graph.drawRect(210, roadEndY, 30, roadLength);
    //lines
    graph.beginStroke('#FFF');
    for (var i = roadEndY; i < raceDetails.screen_height; i = i + 15) {
        graph.moveTo(75, i);
        graph.lineTo(75, i-5);
        graph.moveTo(120, i);
        graph.lineTo(120, i - 5);
        graph.moveTo(165, i);
        graph.lineTo(165, i - 5);
    }

    //finish
    graph.beginFill('#FFF');
    graph.drawRect(30, finishY - 20, 180, 20);
    graph.beginFill('#000');
    for (var i = 0; i < 180; i += 20) {
        graph.drawRect(30 + i, finishY - 20, 10, 10);
        graph.drawRect(40 + i, finishY - 10, 10, 10);
    }

    raceDetails.roadShape = new createjs.Shape(graph);
    raceDetails.stage.addChild(raceDetails.roadShape);
}

function updateCars() {
    var currentOffset = raceDetails.getCurrentRacer().Length;
    //road update
    raceDetails.roadShape.y = currentOffset;
    var fullLen = raceDetails.screen_height - raceDetails.radar.pos.y - 40;
    for (var index = 0; index < raceDetails.raceObj.Racers.length; index++) {
        var racer = raceDetails.raceObj.Racers[index];
        //car update
        var currentLen = racer.Length > raceDetails.raceObj.Length ? raceDetails.raceObj.Length : racer.Length;
        raceDetails.carShapes[index].y = currentOffset - currentLen + raceDetails.racerOffset;
        //radar update
        var progress = ((raceDetails.radar.height - 20) / raceDetails.raceObj.Length) * currentLen;
        raceDetails.radarShapes[index].y = -progress;

        raceDetails.stage.update();
    }
}

function drwaUI() {
    var g = new createjs.Graphics();
    drawRadar(g);
    var inter = new createjs.Shape(g);
    raceDetails.stage.addChild(inter);
    infoUI(raceDetails.stage);
}

//--radar---
function drawRadar(g){
    g.beginStroke('#00F300');
    g.beginFill('#007000');
    g.drawRect(raceDetails.radar.pos.x, raceDetails.radar.pos.y, raceDetails.radar.width, raceDetails.radar.height);
    g.moveTo(raceDetails.radar.pos.x + 10, raceDetails.radar.pos.y + 20);
    g.lineTo(raceDetails.radar.pos.x + 10, raceDetails.screen_height - 80);
    g.moveTo(raceDetails.radar.pos.x + 30, raceDetails.radar.pos.y + 20);
    g.lineTo(raceDetails.radar.pos.x + 30, raceDetails.screen_height - 80);
    g.moveTo(raceDetails.radar.pos.x + 50, raceDetails.radar.pos.y + 20);
    g.lineTo(raceDetails.radar.pos.x + 50, raceDetails.screen_height - 80);
    g.moveTo(raceDetails.radar.pos.x + 70, raceDetails.radar.pos.y + 20);
    g.lineTo(raceDetails.radar.pos.x + 70, raceDetails.screen_height - 80);
}

function drawRadarCar(index) {
    var racer = raceDetails.raceObj.Racers[index];
    var g = new createjs.Graphics();
    var x = raceDetails.radar.pos.x + index * 20 + 10;
    var y = raceDetails.radar.height + raceDetails.radar.pos.y;
    g.beginFill('#' + Math.floor((Math.random() * 999) + 1));
    if (racer == raceDetails.getCurrentRacer()) {
        g.drawCircle(x - 5, y - 5, 10);
    }
    else {
        g.drawRect(x - 5, y - 5, 10, 10);
    }
    return g;
}

//--race UI---
function infoUI(stage) {
    var states = '';
    for(i in raceDetails.raceObj.Racers){
        states += raceDetails.raceObj.Racers[i].Racer.UserName + "\n";
    }
    raceDetails.labels.position = new createjs.Text(states, "16px Arial");
    raceDetails.labels.position.x = 250;
    raceDetails.labels.speed = new createjs.Text("0 km/h", "22px Arial");
    raceDetails.labels.speed.x = 250;
    raceDetails.labels.speed.y = 450;
    raceDetails.labels.length = new createjs.Text("0/" + raceDetails.raceObj.Length, "16px Arial", "#090");
    raceDetails.labels.length.x = 250;
    raceDetails.labels.length.y = 475;

    stage.addChild(raceDetails.labels.position);
    stage.addChild(raceDetails.labels.speed);
    stage.addChild(raceDetails.labels.length);
}

function updateRaceUI() {
    raceDetails.labels.speed.text = raceDetails.getCurrentRacer().Speed + ' km/h';
    raceDetails.labels.length.text = raceDetails.getCurrentRacer().Length + '/' + raceDetails.raceObj.Length;
    var sortedRacers = utils.getSortedRacers(raceDetails.raceObj.Racers);
    var position = '';
    $(sortedRacers).each(function () {
        position += this.RacerName + '\n';
    });
    raceDetails.labels.position.text = position;
}