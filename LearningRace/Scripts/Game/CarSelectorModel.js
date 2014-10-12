function carSelector(colors, cars, raceId, currentCarId) {
    var self = this;

    self.RaceId = raceId;
    self.carColors = colors;
    self.carsCollection = cars;
    self.currentCarId = ko.observable(currentCarId);
    self.currentCar = function () {
        for (var index = 0; index < self.carsCollection.length; index++) {
            if (self.carsCollection[index].Id === currentCarId) {
                return self.carsCollection[index];
            }
        }
    }
    self.currentColor = ko.observable(self.currentCar().StringColor);

    self.setColor = function (color) {
        self.currentColor(color);
    }
    self.setCar = function (carId) {
        self.currentCarId(carId);
    }

    self.imagePath = function (name) {
        return ko.computed(function () {
            return utils.getPathToCarImage(name, self.currentColor());
        }, this)
    };
}