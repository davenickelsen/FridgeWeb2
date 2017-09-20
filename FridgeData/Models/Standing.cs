namespace FridgeData.Models
{
    public class Standing
    {
        public int ConfidenceCorrect { get; set; }
        public int VersusSpreadCorrect { get; set; }
        public int PicksCorrect { get; set; }
        public int UserId { get; set; }

        public string FullName { get; set; }
    }
}