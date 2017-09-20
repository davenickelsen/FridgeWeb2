namespace FridgeData.Models
{
    public class SeasonTotal
    {
        public int UserId { get; set; }
        public int Season { get; set; }
        public decimal? PicksCorrect { get; set; }
        public decimal? VersusSpreadCorrect { get; set; }
        public decimal? Confidence { get; set; }
    }
}
