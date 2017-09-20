

namespace FridgeCoreWeb.Models
{
    public class UpdatedPickModel
    {
        public int GameId { get; set; }
        public int UserId { get; set; }

        public int? PickId { get; set; }

        public string EvenUp { get; set; }
        public string VersusSpread { get; set; }

        public int? Confidence { get; set; }
    }
}
