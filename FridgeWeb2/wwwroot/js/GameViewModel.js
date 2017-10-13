function GameViewModel(availableConfidences, context, bus) {
    var self = this;
    self.confidence = ko.observable(context.confidence);
    self.confidences = ko.observableArray(createConfidences(self.confidence(), availableConfidences));
    self.previousConfidence = self.confidence();
    self.context = context;
    self.winner = ko.observable(context.evenUpSelection);
    self.vsSpread = ko.observable(context.vsSpreadSelection);
    self.editable = ko.observable(context.editable);
    self.winnerCss = ko.computed(function () {
        return "team-selection " + (self.winner() || "").toLowerCase();
    });
    self.vsSpreadCss = ko.computed(function () {
        return "team-selection " + (self.vsSpread() || "").toLowerCase();
    });
    self.evenUpStatus = ko.observable(context.pending ? "pending" : context.evenUpCorrect ? "correct" : "wrong");
    self.vsSpreadStatus = ko.observable(context.pending ? "pending" : context.vsSpreadCorrect ? "correct" : "wrong");
    self.confidenceStatus = ko.observable(context.pending ? "pending" : context.evenUpCorrect ? "correct" : "wrong");
    self.noLine = context.noLine;
    self.pickId = context.pickId;
    self.gameId = context.gameId;

    bus.subscribe(function(eventData) {
        if (eventData.homeTeam !== context.homeTeam) {
            if (eventData.claimed) {
                var index = self.confidences.indexOf(eventData.claimed);
                
                self.confidences.splice(index, 1);
            }
            if (eventData.released) {
                var newlyAvailableNumber = parseInt(eventData.released);
                self.confidences.push(newlyAvailableNumber);
                self.confidences.sort(numericSort);
            }
        }
    });

    self.onConfidenceChanged = function (model) {
        var newConfidence = model.confidence();         
        var previousConfidence = model.previousConfidence;
        model.previousConfidence = newConfidence;
        var changeEvent = { homeTeam: model.context.homeTeam, claimed: newConfidence, released: previousConfidence };
        bus(changeEvent); 
    };

    self.changeWinner = function () {
        if (self.editable()) {
            if (self.winner() === self.context.homeTeam) {
                self.winner(self.context.awayTeam);
            }
            else if (self.winner() === self.context.awayTeam) {
                self.winner(null);
            }
            else {
                self.winner(self.context.homeTeam);
            }
            
        }
    };

    self.changeVersusSpread = function() {
        if (self.editable()) {
            if (self.vsSpread() === self.context.homeTeam) {
                self.vsSpread(self.context.awayTeam);
            }
            else if (self.vsSpread() === self.context.awayTeam) {
                self.vsSpread(null);
            }
            else {
                self.vsSpread(self.context.homeTeam);
            }
        }
    };

    function createConfidences(selected, available) {
        var confidences = available.map(function (i) { return i; });
        if (selected)
            confidences.push(selected);
        confidences.unshift("");
        confidences.sort(numericSort);
        return confidences;
    }

    function numericSort(a, b) {
        if (a === b)
            return 0;
        if (!a) {
            return -1;
        }
        if (!b) {
            return 1;
        }

        return parseInt(a) > parseInt(b) ? 1 : -1;
    }
}