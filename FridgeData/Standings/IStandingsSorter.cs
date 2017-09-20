using System.Collections.Generic;
using FridgeData.Models;

namespace FridgeData.Standings
{
    public interface IStandingsSorter
    {
        void Sort(List<Standing> standings, string sortDirection);
    }
}