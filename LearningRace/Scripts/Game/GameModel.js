/// <reference path="../jquery-1.7.1.min.js" />
/// <reference path="../knockout-2.3.0.js" />

function racer(id, name, length, speed, isReady, car) {
    var self = this;

    self.Id = id;
    self.Name = name;
    self.Length = ko.observable(length);
    self.Speed = ko.observable(speed);
    self.IsReady = ko.observable(isReady);
    self.Car = ko.observable(car);
}

function gameModel(userId, qs, rs, selector) {
    self.questions = ko.observableArray([]);

    self.UserId = userId;
    self.racers = ko.observableArray([]);
    self.RaceId = rs.id;
    self.TotalLength = rs.length;
    self.IsStarted = ko.observable(rs.isStarted);
    self.IsFinished = ko.observable(rs.isFinished);
    self.IsCurrentUserFinished = ko.observable(rs.isFinished);
    self.Version = 0;
    self.QuestionIndex = ko.observable(0);
    self.RaceMessage = ko.observable('');
    self.CurrentRacer = null;

    self.ReadyTimeout = ko.observable(0);
    self.ReadyTimer = null;

    $(rs.racers).each(function (index, item) {
        var racer = ko.observable(item);
        self.racers.push(racer);
        if (item.Id === self.UserId) self.CurrentRacer = racer;
    });

    $(qs).each(function (index, item) { self.questions.push(item); });

    self.carSelector = selector;

    var intervalUpdate = null;

    function updateModel() {
        if (self.IsStarted()) {
            ko.utils.arrayFirst(self.racers(), function (item) {
                var racer = item(), raceCar = racer.Car();

                if (racer.Speed() > raceCar.AvrSpeed) {
                    racer.Speed(racer.Speed() - raceCar.DSpeed);
                }
                else if (racer.Speed() < raceCar.AvrSpeed) {
                    racer.Speed(racer.Speed() + raceCar.DSpeed);
                }
                if (racer.Length() < self.TotalLength) {
                    racer.Length(racer.Length() + racer.Speed());
                }
            });
            raceController.updateRace(self);
            if (!intervalUpdate) {
                intervalUpdate = setInterval(updateModel, 1000);
            }
        }
    }

    self.addQuestion = function (q) {
        self.questions.push(q);
    }

    self.isCurrentUserFinished = function () {

    }

    self.updateRacers = function (racers) {
        //add new racer
        if (self.racers().length < racers.length) {
            $(racers).each(function (index, rsr) {
                var isExists = false;
                ko.utils.arrayFirst(self.racers(), function (item) {
                    if (item().Id === rsr.Racer.UserId) isExists = true;
                });
                if (!isExists) {
                    var newRacer = new racer(rsr.Racer.UserId, rsr.Racer.UserName, rsr.Length, rsr.Speed, rsr.IsReady, rsr.RaceCar);
                    self.racers.push(ko.observable(newRacer));
                }
            });
        }

        ko.utils.arrayFirst(self.racers(), function (item) {
            var racer = item();
            $(racers).each(function (index, rsr) {
                if (racer.Id === rsr.Racer.UserId) {
                    if (self.IsStarted()) {
                        racer.Length(rsr.Length);
                        racer.Speed(rsr.Speed);
                    } else if (!self.IsFinished() && !(self.ReadyTimer && self.UserId === rsr.Racer.UserId)) {
                        racer.IsReady(rsr.IsReady);
                    }
                    racer.Car(rsr.RaceCar);

                    if (rsr.Racer.UserId === self.UserId && rsr.RaceResult && rsr.RaceResult.RacerPlace) {
                        var message = "You finished the race " + rsr.RaceResult.RacerPlace + "/" +
                          rsr.RaceResult.RacersCount + ", with " + rsr.Score + " points";
                        self.RaceMessage(message);
                    }
                    return;
                }
            });
        });
    }

    self.updateRaceInfo = function () {
        $.ajax({
            data: { raceId: self.RaceId, version: self.Version },
            url: "/Home/GetRaceInfo",
            contentType: "application/json;",
            dataType: "json",
            success: function (data) {
                self.Version = data.Version;
                self.updateRacers(data.Racers);
                self.IsStarted(data.IsStarted);
                self.IsFinished(data.IsFinished);
                if (!data.IsFinished) {
                    self.updateRaceInfo();
                } else {
                    clearInterval(intervalUpdate);
                }
                updateModel();
                raceController.updateRace(self);
            }
        });
    }

    self.changeStatus = function () {
        var isReady = !this.IsReady();
        this.IsReady(isReady);
        clearInterval(self.ReadyTimer);
        if (isReady) {
            self.ReadyTimeout(4);
            self.ReadyTimer = setInterval(function () {
                self.ReadyTimeout(self.ReadyTimeout() - 1);
                if (self.ReadyTimeout() <= 0) {
                    sendReadyState(isReady);
                    clearInterval(self.ReadyTimer);
                }
            }, 1000);
        } else {
            sendReadyState(isReady);
        }
    }

    function sendReadyState(isReady) {
        $.ajax({
            url: "/Home/RacerReady",
            data: {
                raceId: self.RaceId,
                isReady: isReady,
                carId: self.carSelector.currentCarId(),
                color: self.carSelector.currentColor()
            },
            contentType: "application/json;",
            dataType: "json",
            success: function (data) {
                self.updateRacers(data.Racers);
            }
        });
    }

    self.updateRaceInfo();
}

$(document).ready(function () {
    ko.applyBindings(new gameModel(userId, getQuestions(), getRaceData(), getCarSelector()));
});