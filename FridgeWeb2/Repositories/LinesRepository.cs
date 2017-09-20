using System.Collections.Generic;
using System.Linq;
using FridgeData;
using FridgeData.Models;
using FridgeCoreWeb.Models;

namespace FridgeCoreWeb.Repositories
{
    public class LinesRepository : ILinesRepository
    {
        private IFridgeContext _context;

        public LinesRepository(IFridgeContext context)
        {
            _context = context;
        }

        public List<Game> GetGames(int week, int season)
        {
            return _context.Games.Where(g => g.Season == season && g.Week == week).ToList();
        }

        public void SaveLines(List<LineUpdateModel> updates)
        {
            foreach (var update in updates)
            {
                var game = _context.Games.Single(g => g.Id == update.GameId);
                game.Spread = update.Spread;

            }
            _context.SaveChanges();
        }
    }
}