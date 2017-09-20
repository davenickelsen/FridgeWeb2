using System;
using System.Collections.Generic;

namespace FridgeData.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int Season { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime GameTime { get; set; }
        public int Week { get; set; }
        public decimal? Spread { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public string PickWinner { get; set; }
        public string VersusSpreadWinner { get; set; }

        public ICollection<Pick> Picks { get; set; }
    }
}
