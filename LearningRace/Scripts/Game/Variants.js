$(document).ready(function () {
    raceDetails.canvas = document.getElementById("cnvRace");
    raceDetails.loadContent(mainInitialization, ['../Images/Cars/BasicCar.png', '../Images/Cars/redSedan.png', '../Images/Cars/pinkUnversal.png']);
});

function mainInitialization() {
    addRacer();
    initButtons();
    initContainers();
}

function initButtons() {
    $('.variantButton').click(function () {
        var questionContainer = $(this).parents('.questionContainer');
        questionContainer.find('.variantButton').unbind();

        sendAnswer($(this), questionContainer)

        setTimeout(function () {
            gameInfo.questionIndex++;
            questionContainer.hide();
            questionContainer.addClass('result');
            initContainers();
        }, 1000);
    });
}

function initContainers() {
    if (gameInfo.questionIndex >= gameInfo.questionCount) {
        $('#messageContainer').text('Good job racer, there no more question for you, just wait the end of the race.');
        $('#messageContainer').show();
        $('.result').each(function(){$(this).show()});
    }
    else {
        $(".questionContainer[index='" + gameInfo.questionIndex + "']").show();
    }
}

function sendAnswer(button, questionContainer) {
    $.ajax({
        url: SendAnswerURL,
        data: { questionId: questionContainer.attr("id"), variantId: button.attr("id"), raceId: raceDetails.raceObj.RaceId },
        success: function (data) {
            if (data.result) {
                button.addClass('right');
            }
            else {
                button.addClass('wrong');
                questionContainer.find('.variantButton[id="' + data.rightId + '"]').addClass('right');
            }
            raceDetails.raceObj = data.race;
            updateRaceInfo();
        },
        type: "GET",
        contentType: "application/json;",
        dataType: "json"
    });
}

function addRacer(){
    $.ajax({
        url: AddRacerURL,
        contentType: "application/json;",
        data: { categoryId: categoryId },
        dataType: "json",
        success: function (data) {
            raceDetails.raceObj = data.race;
            $(data.race.Racers).each(function (index, item) {
                if (data.racerId == item.Racer.UserId) {
                    raceDetails.currentRacer = item;
                    return;
                }
            });
            updateRaceInfo();
            startRaceUpdater();
        }
    });
}

function startRaceUpdater(){
    $.ajax({
        data: { raceId: raceDetails.raceObj.RaceId, version: raceDetails.raceObj.Version },
        url: GetRaceInfoURL,
        contentType: "application/json;",
        dataType: "json",
        success: function (data) {
            raceDetails.raceObj = data;
            updateRaceInfo();
            if (!data.IsFinished) {
                startRaceUpdater();
            }
        }
    });
}

function updateRaceInfo() {
    $('#raceRating').html('');
    var raceInfo = raceDetails.raceObj;

    var readyButton = '';
    if (!raceInfo.IsStarted) {
        readyButton = !raceInfo.currentRacer.IsReady ? '<input type="button" value="Ready!" onclick="racerReady(true);"/>'
           : '<input type="button" value="Not Ready!" onclick="racerReady(false);"/>';
    }
    else {
        readyButton = raceInfo.Racers[i].Length >= raceInfo.Length ? '<a href="/UserDetails/UserRaces?userId=' + userId + '">Show Comleted Races</a>' : 'Race In Process';
    }

    for(var i =0;i< raceInfo.Racers.length;i++){
         if(raceInfo.Racers[i].Racer.UserName == raceDetails.racerName){
             if(!raceInfo.IsStarted){
                 readyButton = !raceInfo.Racers[i].IsReady ? '<input type="button" value="Ready!" onclick="racerReady(true);"/>'
                    : '<input type="button" value="Not Ready!" onclick="racerReady(false);"/>';
             }
             else{
                 readyButton = raceInfo.Racers[i].Length >= raceInfo.Length ? '<a href="/UserDetails/UserRaces?userId=' + userId + '">Show Comleted Races</a>' : 'Race In Process';
             }
         }
         $('#raceRating').append('<tr><td>'+raceInfo.Racers[i].Racer.UserName+'</td><td>'+
                                  raceInfo.Racers[i].Length+'</td><td>'+raceInfo.Racers[i].Speed+
                                  '<td>'+readyButton+'</td>'+'</td></tr>');
    }

     if (!raceDetails.isStarted && raceDetails.raceObj.IsStarted) {
         startRace();
         raceDetails.isStarted = true;
    }
}

function racerReady(isReady){
    $.ajax({
        url: RacerReadyURL,
        data: { raceId: raceDetails.raceObj.RaceId, isReady: isReady },
        contentType: "application/json;",
        dataType: "json",
        success: function (data) {
            raceDetails.raceObj = data;
            updateRaceInfo();
        }
    });
}

var gameInfo = {
    questionIndex: 0,
    questionCount: 10
}