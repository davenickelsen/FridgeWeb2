namespace FridgeData.Models
{
    public class WeeklyPickTotal
    {
        public int UserId { get; set; }

        public int Season { get; set; }
        public int Week { get; set; }

        public int? PicksCorrect { get; set; }
        public int? VersusSpreadCorrect { get; set; }
        public int? ConfidenceAwarded { get; set; }

        public int GameCount { get; set; }
    }
}