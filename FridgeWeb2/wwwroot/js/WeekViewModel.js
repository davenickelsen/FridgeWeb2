function WeekViewModel(currentWeek, userId, editable) {
    var self = this;
    self.originalUserId = userId;
    self.originalWeek = currentWeek;
    self.originalEditable = editable;
    self.week = ko.observable(currentWeek);
    self.userId = ko.observable(userId);
    self.canEdit = ko.observable(editable);

    function updateRankings() {
        $.post("/Standings/CurrentRanks?userId=" + self.userId(), function (html) {
            $(".rankings").text(html);
        });
    }

    function updateTotals() {
        $.post("/Standings/WeeklyTotals?userId=" + self.userId() + "&week=" + self.week(), function (summary) {
            $(".week-totals").text(summary);
        });
    }

    function updateData() {
        $.post("/Home/Week?id=" + self.week() + "&userId=" + self.userId(), function (html) {
            $("#weekContainer").html(html);
        });
        self.canEdit(self.originalEditable && parseInt(self.userId()) === self.originalUserId && parseInt(self.week()) === self.originalWeek);
    }

    self.onWeekChanged = function () {
        updateData();
        updateTotals();
        updateRankings();
    };

    self.onUserChanged = function () {
        updateData();
        updateTotals();
        updateRankings();        
    };


}