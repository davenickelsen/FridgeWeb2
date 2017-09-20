using System;
using System.Collections.Generic;
using System.Linq;
using FridgeCoreWeb.Models;
using FridgeData;
using FridgeData.Helpers;
using FridgeData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FridgeWeb2.Api
{
    [Route("api/Scores")]
    public class ScoresController
    {
        private IFridgeContext _context;
        private ITimeHelper _helper;

        public ScoresController(ITimeHelper helper, IFridgeContext context)
        {
            _helper = helper;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public int Post([FromBody] IEnumerable<FinalScoreViewModel> scores, string token, int week)
        {
            int count = 0;
            if (token != Environment.GetEnvironmentVariable("FRIDGE_API_TOKEN"))
            {
                throw new UnauthorizedAccessException("Bad key");
            }
            if (_helper.GetCurrentWeek() != week)
            {
                throw new Exception("Incorrect week");
            }
            var games = _context.Games.Where(g => g.Season == _helper.GetCurrentSeason() && g.Week == week).ToList();
            foreach (var score in scores)
            {
                var game = games.Single(g => g.HomeTeam.ToUpper() == score.HomeTeam.ToUpper());
                UpdateGame(score, game);
                count++;
            }
            _context.SaveChanges();

            return count;
        }

        private void UpdateGame(FinalScoreViewModel score, Game game)
        {
            game.HomeTeamScore = score.HomeTeamScore;
            game.AwayTeamScore = score.AwayTeamScore;
            game.PickWinner = DetermineEvenUpWinner(score, game);
            game.VersusSpreadWinner = DetermineVersusSpreadWinner(score, game);
        }

        private string DetermineVersusSpreadWinner(FinalScoreViewModel score, Game game)
        {
            if (score.AwayTeamScore == score.HomeTeamScore)
            {
                return "TIE";
            }
            return score.HomeTeamScore > score.AwayTeamScore ? game.HomeTeam : game.AwayTeam;
        }

        private string DetermineEvenUpWinner(FinalScoreViewModel score, Game game)
        {
            var net = score.HomeTeamScore - score.AwayTeamScore + game.Spread.GetValueOrDefault(0);
            if (net == 0)
            {
                return "TIE";
            }
            return net > 0 ? game.HomeTeam : game.AwayTeam;
        }
    }
}
