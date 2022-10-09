using Serilog;
using PowerDayAheadReport.Helpers;

namespace PowerDayAheadReport.BusinessLogic;

public static class Report
{
    public static void Create(string path)
    {
        var powerDay = PowerHelper.GetPowerDayAhead(DateTime.Now);

        var trades = GetTrades(powerDay);
        var volumes = trades.Result.SelectMany(x => x.Periods);

        var aggregatedVolumes = GetAggregatedVolumes(volumes);

        Save(aggregatedVolumes, path);
    }

    private static async Task<IEnumerable<Services.PowerTrade>> GetTrades(DateTime powerDay)
    {
        Log.Information($"Retrieving trades for {powerDay:yyyy/MM/dd}");

        var ps = new Services.PowerService();
        var trades = await ps.GetTradesAsync(powerDay);

        Log.Information($"{trades.Count()} trades retrieved");

        foreach (var trade in trades)
        {
            Log.Information($"{powerDay:yyyyMMdd}=>{string.Join(',', trade.Periods.Select(x => x.Volume))}");
        }

        return trades;
    }

    private static Dictionary<int, double> GetAggregatedVolumes(IEnumerable<Services.PowerPeriod> volumes)
    {
        return volumes
            .GroupBy(x => x.Period)
            .ToDictionary(x => x.Key, x => x.Sum(y => y.Volume));
    }

    private static async void Save(Dictionary<int, double> data, string path)
    {
        var sortedData = new SortedDictionary<int, double>(data);

        var lines = new List<string>() { "Local Time,Volume" };
        lines.AddRange(sortedData.Select(x => string.Join(",", $"{(x.Key == 1 ? 23 : x.Key - 2):00}:00", x.Value)));

        string filePath = Path.Combine(path, $"PowerPosition_{DateTime.Now:yyyyMMdd_HHmm}.csv");

        Log.Information($"Saving the report at {filePath}");

        await File.WriteAllLinesAsync(filePath, lines.ToArray());
    }
}
