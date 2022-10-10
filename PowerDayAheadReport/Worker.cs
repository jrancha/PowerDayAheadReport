using Microsoft.Extensions.Options;
using Models = PowerDayAheadReport.Models;
using BusinessLogic = PowerDayAheadReport.BusinessLogic;

namespace PowerDayAheadReport;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IOptions<Models.ServiceConfig> _config;

    public Worker(ILogger<Worker> logger, IOptions<Models.ServiceConfig> config)
    {
        _logger = logger;
        _config = config;

        if (_config.Value.Interval == 0)
            throw new ArgumentNullException("Power day ahead report service run interval");
        else if (string.IsNullOrEmpty(_config.Value.OutputFilePath))
            throw new ArgumentNullException("Power day ahead report service output file path");

        if (!Directory.Exists(_config.Value.OutputFilePath))
            Directory.CreateDirectory(_config.Value.OutputFilePath);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                BusinessLogic.Report.Create(_config.Value.OutputFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while creating the report");
            }

            await Task.Delay(_config.Value.Interval * 60 * 1000, stoppingToken);
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