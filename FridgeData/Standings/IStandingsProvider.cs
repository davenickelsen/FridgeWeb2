using System.Collections.Generic;
using FridgeData.Models;

namespace FridgeData.Standings
{
    public interface IStandingsProvider
    {
        List<Standing> GetStandings(int season);

    }
}