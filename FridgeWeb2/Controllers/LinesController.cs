using System.Linq;
using FridgeData.Helpers;
using FridgeCoreWeb.Models;
using FridgeCoreWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FridgeCoreWeb.Controllers
{
    public class LinesController : Controller
    {
        private ILinesRepository _repository;
        private ITimeHelper _helper;

        public LinesController(ITimeHelper helper, ILinesRepository repository)
        {
            _helper = helper;
            _repository = repository;
        }

        [Authorize(Policy = "Administrators")]
        public IActionResult Index()
        {
            var games = _repository.GetGames(_helper.GetCurrentWeek(), _helper.GetCurrentSeason());
            var lineModels = games.Select(g => new LineViewModel
            {
                GameId = g.Id,
                AwayTeam = g.AwayTeam,
                GameTime = g.GameTime,
                HomeTeam = g.HomeTeam,
                Spread = g.Spread
            }).ToList();
            return View(lineModels);
        }
    }
}
