using System;
using System.Collections.Generic;
using System.Linq;
using FridgeData;
using FridgeData.Models;
using FridgeCoreWeb.Models;

namespace FridgeCoreWeb.Repositories
{
    public class GameViewRepository : IGameViewRepository
    {
        private IFridgeContext _context;
        public GameViewRepository(IFridgeContext context)
        {
            _context = context;

        }
        public List<GameViewGroupModel> GetGameViews(int week, int season, int userId, DateTime currentTime, bool restricted = false)
        {
            var games = (from game in _context.Games.Where(g => g.Season == season && g.Week == week)
                        from pick in _context.Picks.Where(p => p.UserId == userId && p.GameId == game.Id).DefaultIfEmpty()
                        select new GameViewModel
                        {
                            AwayTeam = game.AwayTeam,
                            Spread = game.Spread.GetValueOrDefault(),
                            Confidence = pick == null ? null : pick.Confidence,
                            HomeTeam = game.HomeTeam,
                            EvenUpSelection = pick == null ? null : pick.EvenUp,
                            VsSpreadSelection = pick == null ? null : pick.VersusSpread,
                            GameTime = game.GameTime,
                            EvenUpWinner = game.PickWinner,
                            VsSpreadWinner = game.VersusSpreadWinner,
                            GameId = game.Id,
                            PickId = pick == null ? -1 : pick.Id,
                            Editable = game.GameTime >= currentTime
                        }).ToList();
            if (restricted)
            {
                games = MaskUncommittedGames(games);
            }
            return games.GroupBy(g => g.GameTime).Select((group) => new GameViewGroupModel
            {
                GameTime = group.Key,
                GameViewModels = group.ToList(),
            }).ToList();
        }

        private List<GameViewModel> MaskUncommittedGames(IEnumerable<GameViewModel> games)
        {
            var uncommittedGames = games.Where(g => g.Editable).ToList();
            var count = uncommittedGames.Count;
            for (var i = 0; i < count; i++)
            {
                uncommittedGames[i].Confidence = null;
                uncommittedGames[i].VsSpreadSelection = null;
                uncommittedGames[i].EvenUpSelection = null;
            }
            return games.Where(g => !g.Editable).Union(uncommittedGames).ToList();

        }
    }
}
