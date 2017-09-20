using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeCoreWeb.Models
{
    public class LineViewModel
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int GameId { get; set; }
        public DateTime GameTime { get; set; }

        public decimal? Spread { get; set; }
    }
}
