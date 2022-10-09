using PowerDayAheadReport.Helpers;
using PowerDayAheadReport.BusinessLogic;
using System.Reflection;

namespace PowerDayAheadReportTests;

public class PowerHelperTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test_GetPowerDayAhead_Before2300()
    {
        var calendarDateTime = new DateTime(2022, 10, 08, 22, 0, 0);

        var expect = new DateTime(2022, 10, 09);
        var actual = PowerHelper.GetPowerDayAhead(calendarDateTime);

        Assert.That(actual, Is.EqualTo(expect));
    }

    [Test]
    public void Test_GetPowerDayAhead_After2300()
    {
        var calendarDateTime = new DateTime(2022, 10, 08, 23, 30, 0);

        var expect = new DateTime(2022, 10, 10);
        var actual = PowerHelper.GetPowerDayAhead(calendarDateTime);

        Assert.That(actual, Is.EqualTo(expect));
    }

    [Test]
    public void Test_AggregatedVolumes()
    {
        var volumes = new List<Services.PowerPeriod>();
        volumes.AddRange(new Services.PowerPeriod[]
        {
            new Services.PowerPeriod() { Period = 1, Volume = 11 },
            new Services.PowerPeriod() { Period = 2, Volume = 22 },
            new Services.PowerPeriod() { Period = 3, Volume = 33 },
            new Services.PowerPeriod() { Period = 1, Volume = 12 }, 
            new Services.PowerPeriod() { Period = 2, Volume = 23 }, 
            new Services.PowerPeriod() { Period = 3, Volume = 34 }
        });

        MethodInfo? method = typeof(Report).GetMethod("GetAggregatedVolumes", BindingFlags.Static | BindingFlags.NonPublic);
        var parms = new object[] { volumes };

        var actual = (Dictionary<int, double>) method?.Invoke(null, parms);

        Assert.That(actual.Count(), Is.EqualTo(3));
        Assert.That(actual[1], Is.EqualTo(23));
        Assert.That(actual[2], Is.EqualTo(45));
        Assert.That(actual[3], Is.EqualTo(67));
    }

}