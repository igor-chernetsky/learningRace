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