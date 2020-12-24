using System;
using System.Linq;
using FridgeData.Configuration;
using Microsoft.Extensions.Options;

namespace FridgeData.Helpers
{
    public class TimeHelper : ITimeHelper
    {
        private TimeSettings _overrideSettings;

        public TimeHelper(IOptions<TimeSettings> overrideOptions)
        {
            _overrideSettings = overrideOptions.Value;
        }
        public DateTime GetCurrentTime()
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(_overrideSettings.EasternTimeZoneId);
            return _overrideSettings.OverrideOn ? _overrideSettings.OverrideValue : TimeZoneInfo.ConvertTime(DateTime.UtcNow, easternZone);
        }

        public int GetCurrentSeason()
        {
            return _overrideSettings.Season;
        }

        public int GetCurrentWeek()
        {
            var now = GetCurrentTime();
            var laborDay = Enumerable.Range(1, 7).Select(i => new DateTime(GetCurrentSeason(), 9, i))
                .Single(d => d.DayOfWeek == DayOfWeek.Monday);
            var weekStart = laborDay.AddDays(2);

            var week = 1;
            while (now > weekStart && week < 18)
            {
                week++;
                weekStart = weekStart.AddDays(7);
            }
            return week == 1 ? 1 : week - 1;

        }

        public static bool IsChristmas
        {
            get { return DateTime.Now.Month == 12 && DateTime.Now.Day > 19; }
        }

    }
}
