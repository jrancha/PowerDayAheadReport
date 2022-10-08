using Microsoft.Extensions.Configuration;
using PowerDayAheadReport;
using Serilog;
using PowerDayAheadReport.Models;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            IConfiguration configuration = hostContext.Configuration;
            services.Configure<ServiceConfig>(configuration.GetSection(nameof(ServiceConfig)));
            services.AddHostedService<Worker>();
        })
        .UseWindowsService()
        .UseSerilog()
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fatal application error while running power day ahead report service");
}
finally
{
    Log.CloseAndFlush();
}
