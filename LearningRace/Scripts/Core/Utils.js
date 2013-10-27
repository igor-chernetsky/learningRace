var utils = {
    sortRacesrs: function (a, b) {
        if (a.Length > b.Length) return 1;
        if (a.Length < b.Length) return -1;
        return 0;
    },

    getSortedRacers: function (racers) {
        var result = [];
        $(racers).each(function () {
            result.push({ RacerName: this.RacerName, Length: this.Length });
        });
        result.sort(this.sortRacesrs);
        return result;
    }
}