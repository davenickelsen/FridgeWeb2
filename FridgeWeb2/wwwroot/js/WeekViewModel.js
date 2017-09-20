function WeekViewModel(currentWeek, userId, editable) {
    var self = this;
    self.orginalUserId = userId;
    self.originalWeek = currentWeek;
    self.originalEditable = editable;
    self.week = ko.observable(currentWeek);
    self.userId = ko.observable(userId);
    self.editable = ko.observable(editable);

    function updateRankings() {
        $.post("/Standings/CurrentRanks?userId=" + self.userId(), function (html) {
            $(".rankings").html(html);
        });
    }

    function updateData() {
        $.post("/Home/Week?id=" + self.week() + "&userId=" + self.userId(), function (html) {
            $("#weekContainer").html(html);
        });
        self.editable(self.originalEditable && (self.userId() === self.orginalUserId) && self.week() === self.originalWeek);
    }

    self.onWeekChanged = function () {
        updateData();
    };

    self.onUserChanged = function () {
        updateRankings();
        updateData();
        
    };


}