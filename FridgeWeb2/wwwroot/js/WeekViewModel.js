function WeekViewModel(currentWeek, userId, editable) {
    console.log("Model built");
    var self = this;
    self.originalUserId = userId;
    self.originalWeek = currentWeek;
    self.originalEditable = editable;
    self.week = ko.observable(currentWeek);
    self.userId = ko.observable(userId);
    self.canEdit = ko.observable(editable);

    function updateRankings() {
        $.post("/Standings/CurrentRanks?userId=" + self.userId(), function (html) {
            $(".rankings").html(html);
        });
    }

    function updateData() {
        $.post("/Home/Week?id=" + self.week() + "&userId=" + self.userId(), function (html) {
            $("#weekContainer").html(html);
        });
        self.canEdit(self.originalEditable && (parseInt(self.userId()) === self.originalUserId) && (parseInt(self.week()) === self.originalWeek));
    }

    self.onWeekChanged = function () {
        updateData();
    };

    self.onUserChanged = function () {
        updateRankings();
        updateData();
        
    };


}