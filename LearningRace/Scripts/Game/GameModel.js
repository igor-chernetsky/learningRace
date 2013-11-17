/// <reference path="../jquery-1.7.1.min.js" />
/// <reference path="../knockout-2.3.0.js" />

function question(id, questionText, raceId) {
    var self = this;

    self.Id = id;
    self.QuestionText = questionText;
    self.RaceId = raceId;
    self.variants = ko.observableArray([]);
    self.gotVariant = false;

    self.addVariant = function (v) {
        self.variants.push(v);
    }

    self.sendAnswer = function (variant, callback, questionIndex) {
        if (self.gotVariant) return;
        self.gotVariant = true;
        $.ajax({
            url: "/Home/SendAnswer",
            data: { questionId: self.Id, variantId: variant.Id, raceId: self.RaceId },
            contentType: 'application/json',
            dataType: "json",
            success: function (data) {
                if (callback) callback(data.race.Racers);
                if (data.result) variant.right(true);
                else {
                    variant.wrong(true);
                    $.each(self.variants(), function (index, item) {
                        if (item.Id === data.rightId) {
                            item.right(true);
                            return false;
                        }
                    });
                }
            }
        });
        setTimeout(function () {
            questionIndex(questionIndex() + 1);
        }, 1000);
    }
}

function variant(id, text) {
    var self = this;

    self.Id = id;
    self.Text = text;
    self.right = ko.observable(false);
    self.wrong = ko.observable(false);
}

function racer(id, name, length, speed, dspeed, avrspeed, isReady) {
    var self = this;

    self.Id = id;
    self.Name = name;
    self.Length = ko.observable(length);
    self.Speed = ko.observable(speed);
    self.Dspeed = dspeed;
    self.AvrSpeed = avrspeed;
    self.IsReady = ko.observable(isReady);
}

function gameModel(userId, qs, rs) {
    self.questions = ko.observableArray([]);

    self.UserId = userId;
    self.racers = ko.observableArray([]);
    self.RaceId = rs.id;
    self.Length = rs.length;
    self.IsStarted = ko.observable(rs.isStarted);
    self.Version = 0;
    self.QuestionIndex = ko.observable(0);

    var intervalUpdate = null;

    function updateModel() {
        ko.utils.arrayFirst(self.racers(), function (item) {
            if (item().Speed() > item().AvrSpeed)
                item().Speed(item().Speed() - item().Dspeed);
            else if (item().Speed() < item().AvrSpeed)
                item().Speed(item().Speed() + item().Dspeed);
            if (item().Length() < self.Length) {
                item().Length(item().Length() + item().Speed());
                raceController.updateRace(self);
            }
        });
        if (!intervalUpdate && self.IsStarted()) {
            intervalUpdate = setInterval(updateModel, 1000);
        }
    }

    $(rs.racers).each(function (index, item) {
        self.racers.push(ko.observable(item));
    });

    $(qs).each(function (index, item) { self.questions.push(item); });

    self.addQuestion = function (q) {
        self.questions.push(q);
    }

    self.updateRacers = function (racers) {
        if (self.racers().length < racers.length) {
            $(racers).each(function (index, rsr) {
                var isExists = false;
                ko.utils.arrayFirst(self.racers(), function (item) {
                    if (item().Id === rsr.Racer.UserId) isExists = true;
                });
                if(!isExists){
                    var newRacer = new racer(rsr.Racer.UserId, rsr.Racer.UserName, rsr.Length, rsr.Speed, rsr.DSpeed, rsr.AvrSpeed, rsr.IsReady);
                    self.racers.push(ko.observable(newRacer));
                }
            });
        }
                
        ko.utils.arrayFirst(self.racers(), function (item) {
            $(racers).each(function (index, rsr) {
                if (item().Id === rsr.Racer.UserId) {
                    item().Length(rsr.Length);
                    item().Speed(rsr.Speed);
                    item().IsReady(rsr.IsReady);
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
        $.ajax({
            url: "/Home/RacerReady",
            data: { raceId: self.RaceId, isReady: !this.IsReady() },
            contentType: "application/json;",
            dataType: "json",
            success: function (data) {
                self.updateRacers(data.Racers);
                if (!data.IsFinished) {
                    self.updateRaceInfo();
                } else {
                    clearInterval(intervalUpdate);
                }
                raceController.updateRace(self);
            }
        });
    }

    self.updateRaceInfo();
}

$(document).ready(function () {
    ko.applyBindings(new gameModel(userId, getQuestions(), getRaceData()));
});