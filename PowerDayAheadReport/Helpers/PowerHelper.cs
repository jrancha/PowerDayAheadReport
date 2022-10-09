namespace PowerDayAheadReport.Helpers;

public static class PowerHelper
{
    public static DateTime GetPowerDayAhead(DateTime calendarDateTime)
    {
        return calendarDateTime.Date.AddDays(1 + (calendarDateTime.Hour == 23 ? 1 : 0));
    }
}
