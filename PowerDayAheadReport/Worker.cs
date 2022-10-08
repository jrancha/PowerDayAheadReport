using Microsoft.Extensions.Options;
using PowerDayAheadReport.Models;

namespace PowerDayAheadReport;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IOptions<ServiceConfig> _config;

    public Worker(ILogger<Worker> logger, IOptions<ServiceConfig> config)
    {
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Power day ahead report service starting");

        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Power day ahead report service stopping");

        return base.StopAsync(cancellationToken);
    }

}