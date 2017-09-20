using System.Collections.Generic;
using FridgeData.Models;

namespace FridgeCoreWeb.Models
{
    public class StandingsViewModel
    {
        public List<Standing> Standings { get; set; }
        public int CurrentUserId { get; set; }
    }
}
