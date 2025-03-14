using SurfSwift.Engine;
using SurfSwift.Entities;
using SurfSwift.Infra;

namespace SurfSwift.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;


        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var automationEngine = scope.ServiceProvider.GetRequiredService<IAutomationEngine>();
                    var dbContext = scope.ServiceProvider.GetRequiredService<SurfSwiftDbContext>();

                    // Sample automation engine run
                    await automationEngine.Initialize(new AutomationConfig
                    {
                        ActionScript = @"C:\Users\jskai\OneDrive\Desktop\NET_AUTOMATION\NET_AUTOMATION.Logic\BANDI_actions.json"
                    });

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
