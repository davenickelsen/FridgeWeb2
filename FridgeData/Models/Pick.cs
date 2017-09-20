namespace FridgeData.Models
{
    public class Pick
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string EvenUp { get; set; }
        public string VersusSpread { get; set; }
        public int? Confidence { get; set; }
        public int Id { get; set; }

        public Game Game { get; set; }
        public User User { get; set; }
    }
}
