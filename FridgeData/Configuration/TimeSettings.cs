using System;

namespace FridgeData.Configuration
{
    public class TimeSettings
    {
        public bool OverrideOn { get; set; }
        public DateTime OverrideValue { get; set; }

        public int Season { get; set; }
        public string EasternTimeZoneId { get; set; }
    }
}
