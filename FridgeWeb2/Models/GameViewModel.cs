using System;

namespace FridgeCoreWeb.Models
{
    public class GameViewModel
    {
        public string HomeTeam { get; set; }
        public string HomeImageUrl => $"images/{HomeTeam}.png";
        public string AwayTeam { get; set; }

        public long GameId { get; set; }
        public long? PickId { get; set; }

        public string AwayImageUrl => $"images/{AwayTeam}.png";
        public decimal Spread { get; set; }
        public DateTime GameTime { get; set; }

        public string DisplaySpread => $"{(Spread >= 0 ? "+" : "-")}{SpreadMagnitude}";

        public string SpreadMagnitude => String.Format("{0:0.##}", Math.Abs((decimal)Spread));

        public int? Confidence { get; set; }

        public string EvenUpSelection { get; set; }

        public string VsSpreadSelection { get; set; }

        public string VsSpreadImageUrl => EvenUpWinner == null ? "/images/choose.gif" : $"images/{VsSpreadSelection}.png";

        public string EvenUpWinner { get; set; }

        public string EvenUpImageUrl => EvenUpWinner == null ? "/images/choose.gif" : $"images/{EvenUpSelection}.png";

        public string VsSpreadWinner { get; set; }
        public bool? EvenUpCorrect => EvenUpWinner != null && EvenUpWinner == EvenUpSelection;

        public bool? VsSpreadCorrect => VsSpreadWinner != null && VsSpreadWinner == VsSpreadSelection;
        public bool Editable { get; set; }

        public bool Pending => !Editable && EvenUpWinner == null;
    }
}
