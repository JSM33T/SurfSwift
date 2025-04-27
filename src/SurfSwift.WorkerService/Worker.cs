using Microsoft.Extensions.DependencyInjection;
using SurfSwift.Engine;
using SurfSwift.Entities;
using SurfSwift.DBEngine.Context;

namespace SurfSwift.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger; 
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await AutomationExecutor.RunAllAutomationsAsync(_serviceProvider, stoppingToken);
                _logger.LogInformation("Worker automation cycle complete at: {time}", DateTimeOffset.Now);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
