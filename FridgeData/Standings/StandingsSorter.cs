using System.Collections.Generic;
using FridgeData.Models;

namespace FridgeData.Standings
{
    public class StandingsSorter : IStandingsSorter
    {
        public void Sort(List<Standing> standings, string sortDirection)
        {
            var pieces = sortDirection.Split('-');
            switch (pieces[0])
            {

                case "spread":
                    PerformVersusSpreadSort(standings);
                    break;
                case "confidence":
                    PerformConfidenceSort(standings);
                    break;
                default:
                    PerformPicksSort(standings);
                    break;
            }
            if (pieces[1] == "desc")
                standings.Reverse();
        }

        private void PerformPicksSort(List<Standing> standings)
        {
            standings.Sort((a, b) =>
            {
                if (a.PicksCorrect == b.PicksCorrect)
                    return 0;
                return a.PicksCorrect > b.PicksCorrect ? 1 : -1;
            });
        }

        private void PerformConfidenceSort(List<Standing> standings)
        {
            standings.Sort((a, b) =>
            {
                if (a.ConfidenceCorrect == b.ConfidenceCorrect)
                    return 0;
                return a.ConfidenceCorrect > b.ConfidenceCorrect ? 1 : -1;
            });
        }

        private void PerformVersusSpreadSort(List<Standing> standings)
        {
            standings.Sort((a, b) =>
            {
                if (a.VersusSpreadCorrect == b.VersusSpreadCorrect)
                    return 0;
                return a.VersusSpreadCorrect > b.VersusSpreadCorrect ? 1 : -1;
            });
        }
    }
}
