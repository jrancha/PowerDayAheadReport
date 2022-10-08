namespace PowerDayAheadReport.Helpers;

internal static class PowerHelper
{
    public static DateTime GetPowerDayAhead(DateTime calendarDateTime)
    {
        return calendarDateTime.Date.AddDays(1 + (DateTime.Today.Hour == 23 ? 1 : 0));
    }
}
