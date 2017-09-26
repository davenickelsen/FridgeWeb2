using System.Collections.Generic;
using System.Linq;
using FridgeData.Authorization;
using FridgeData.Helpers;
using FridgeData.Models;
using FridgeData.Standings;
using FridgeCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FridgeCoreWeb.Controllers
{
    public class StandingsController : Controller
    {
        private ITimeHelper _timeHelper;
        private IStandingsProvider _standingsProvider;
        private IStandingsSorter _standingsSorter;
        private IUserService _userService;
        private IWeeklyPickTotalProvider _wptProvider;

        public StandingsController(IStandingsProvider provider, ITimeHelper timeHelper, IStandingsSorter sorter, IWeeklyPickTotalProvider wptProvider, IUserService userService)
        {
            _wptProvider = wptProvider;
            _userService = userService;
            _standingsSorter = sorter;
            _standingsProvider = provider;
            _timeHelper = timeHelper;
        }
        [Authorize]
        public IActionResult Index(string sortDirection = "picks-desc")
        {
            var currentUser = _userService.GetCurrentUser();

            if (_timeHelper.GetCurrentWeek() == 1)
            {
                return View( new StandingsViewModel{ Standings = new List<Standing>()});
            }
            var standings = _standingsProvider.GetStandings(_timeHelper.GetCurrentSeason());
            _standingsSorter.Sort(standings, sortDirection);
            SetSelectableSortDirections(sortDirection);
            ViewBag.SortDirection = sortDirection;
            return View(new StandingsViewModel{ Standings = standings, CurrentUserId = currentUser.Id});
        }

        [Authorize]
        public string CurrentRanks(int? userId)
        {           
            if (_timeHelper.GetCurrentWeek() == 1)
            {
                return $"Opening week";
            }
            var standings = _standingsProvider.GetStandings(_timeHelper.GetCurrentSeason());
            var currentUserId = _userService.GetCurrentUser().Id;
            return ComputeRanks(standings, userId.GetValueOrDefault(currentUserId));

        }

        [Authorize]
        public string WeeklyTotals(int? userId, int? week)
        {
            userId = userId ?? _userService.GetCurrentUser().Id;
            week = week ?? _timeHelper.GetCurrentWeek();
            return _wptProvider.GetWeekSummary(userId.Value, week.Value);
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

        private void SetSelectableSortDirections(string currentSortOrder)
        {
            ViewBag.PicksSort = $"picks-{GetNextDirection("picks", currentSortOrder)}";
            ViewBag.SpreadSort = $"spread-{GetNextDirection("spread", currentSortOrder)}";
            ViewBag.ConfidenceSort = $"confidence-{GetNextDirection("confidence", currentSortOrder)}";
        }

        private string GetNextDirection(string columnName, string previousSortOrder)
        {
            var pieces = previousSortOrder.Split('-');
            if (columnName == pieces[0])
            {
                return pieces[1] == "asc" ? "desc" : "asc";
            }
            return "asc";

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
    }
}
