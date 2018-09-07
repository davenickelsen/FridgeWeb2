using System;
using System.Collections.Generic;
using System.Linq;
using FridgeData.Models;

namespace FridgeData.Standings
{
    public class StandingsProvider : IStandingsProvider
    {
        private IFridgeContext _context;

        public StandingsProvider(IFridgeContext context)
        {
            _context = context;
        }
        public List<Standing> GetStandings(int season)
        {
            var standingEntities = (from s in _context.SeasonTotals
                join u in _context.Users
                on s.UserId equals u.Id
                where s.Season == season && u.Login.ToLower() != "chase" && u.Login.ToLower() != "cole"
                select new { Standing = s, User = u }).ToList();
            var standings = standingEntities.Select(s => new Standing
            {
                ConfidenceCorrect = Convert.ToInt32(s.Standing.Confidence),
                PicksCorrect = Convert.ToInt32(s.Standing.PicksCorrect),
                VersusSpreadCorrect = Convert.ToInt32(s.Standing.VersusSpreadCorrect),
                UserId = s.User.Id,
                FullName = s.User.FirstName + " " + s.User.LastName
            }).ToList();

            standings.Sort((a, b) =>
            {
                if (a.PicksCorrect > b.PicksCorrect)
                {
                    return -1;
                }
                if (a.PicksCorrect < b.PicksCorrect)
                {
                    return 1;
                }
                return 0;
            });
            return standings;
        }


    }
}
