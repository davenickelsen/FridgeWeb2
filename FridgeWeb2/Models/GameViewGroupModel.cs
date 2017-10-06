using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeCoreWeb.Models
{
    public class GameViewGroupModel
    {
        public List<GameViewModel> GameViewModels { get; set; }
        public DateTime GameTime { get; set; }
        public bool IsLondonGame => GameTime.Hour == 9;
    }
}
