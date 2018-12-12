using System.Linq;
using FridgeData.Helpers;

namespace FridgeData.Standings
{
    public interface IWeeklyPickTotalProvider
    {
        string GetWeekSummary(int userId, int week);
    }

    public class WeeklyPickTotalProvider : IWeeklyPickTotalProvider
    {
        private ITimeHelper _timeHelper;
        private IFridgeContext _context;

        public WeeklyPickTotalProvider(IFridgeContext context, ITimeHelper helper)
        {
            _context = context;
            _timeHelper = helper;
        }

        public string GetWeekSummary(int userId, int week)
        {
            var total = _context.WeeklyTotals.SingleOrDefault(wpt => wpt.UserId == userId &&
                                                                     wpt.Season == _timeHelper.GetCurrentSeason() &&
                                                                     wpt.Week == week);
            if (total == null)
            {
                return " ";
            }

            if (!total.PicksCorrect.HasValue)
            {
                return $"{total.GameCount} games";
            }

            return $"{total.PicksCorrect} / {total.VersusSpreadCorrect} / {total.ConfidenceAwarded} over {total.GameCount} games";

        }
    }
}
