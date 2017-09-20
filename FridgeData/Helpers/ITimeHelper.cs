using System;

namespace FridgeData.Helpers
{
    public interface ITimeHelper
    {
        DateTime GetCurrentTime();

        int GetCurrentWeek();

        int GetCurrentSeason();
    }
}