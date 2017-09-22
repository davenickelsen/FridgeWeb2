using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FridgeData.Authorization;
using FridgeCoreWeb.Models;
using FridgeData.Helpers;
using FridgeData.Models;
using FridgeData.Standings;
using FridgeWeb2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FridgeCoreWeb.ViewComponents
{
    public class WeekHeaderViewComponent : ViewComponent
    {
        private ITimeHelper _timeHelper;
        private IStandingsProvider _standingsProvider;
        private IUserService _userService;

        public WeekHeaderViewComponent(IStandingsProvider provider, ITimeHelper timeHelper, IUserService userService)
        {
            _userService = userService;
            _standingsProvider = provider;
            _timeHelper = timeHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId, int week)
        {
            var rankings = "";
            if (_timeHelper.GetCurrentWeek() == 1)
            {
                rankings = $"Opening week";
            }

            var standings = _standingsProvider.GetStandings(_timeHelper.GetCurrentSeason());
            rankings = ComputeRanks(standings, userId);
            var model = new WeekHeaderViewModel
            {
                Rankings = rankings,
                SelectedUserName = _userService.GetUser(userId).FullName,
                Users = _userService.GetAllUsers().Select(u => new SelectListItem{Text = u.FullName, Value = u.Id.ToString()}),
                SelectedWeek = week,
                Weeks = Enumerable.Range(1, _timeHelper.GetCurrentWeek()).Select(w => new SelectListItem {Text = $"Week {w}", Value = w.ToString()})
            };
            return View("Default", model);
        }

        private string ComputeRanks(List<Standing> standings, int userId)
        {
            if (userId == 28)
            {
                return " ";
            }
            var userStanding = standings.Single(s => s.UserId == userId);
            var evenUpRank = GetEvenUpRank(userStanding, standings);
            var spreadRank = GetSpreadRank(userStanding, standings);
            var confidenceRank = GetConfidenceRank(userStanding, standings);
            return $"{AppendSuffix(evenUpRank)} / {AppendSuffix(spreadRank)} / {AppendSuffix(confidenceRank)} of {standings.Count}";
        }

        private int GetEvenUpRank(Standing target, List<Standing> standings)
        {
            var q = from s in standings
                orderby s.PicksCorrect descending
                select new
                {
                    s.UserId,
                    Rank = (from o in standings
                               where o.PicksCorrect > target.PicksCorrect
                               select o).Count() + 1
                };
            return q.Single(s => s.UserId == target.UserId).Rank;
        }

        private int GetSpreadRank(Standing target, List<Standing> standings)
        {
            var q = from s in standings
                orderby s.VersusSpreadCorrect descending
                select new
                {
                    s.UserId,
                    Rank = (from o in standings
                               where o.VersusSpreadCorrect > target.VersusSpreadCorrect
                               select o).Count() + 1
                };
            return q.Single(s => s.UserId == target.UserId).Rank;
        }

        private int GetConfidenceRank(Standing target, List<Standing> standings)
        {
            var q = from s in standings
                orderby s.ConfidenceCorrect descending
                select new
                {
                    s.UserId,
                    Rank = (from o in standings
                               where o.ConfidenceCorrect > target.ConfidenceCorrect
                               select o).Count() + 1
                };
            return q.Single(s => s.UserId == target.UserId).Rank;
        }

        private string AppendSuffix(int index)
        {
            var suffix = "th";
            switch (index)
            {
                case 1:
                    suffix = "st";
                    break;
                case 2:
                    suffix = "nd";
                    break;
                case 3:
                    suffix = "rd";
                    break;
            }
            return $"{index}{suffix}";
        }
    }
}
