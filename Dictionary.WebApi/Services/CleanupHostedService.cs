using Dictionary.WebApi.Interfaces;

namespace Dictionary.WebApi.Services
{
    public class CleanupHostedService : BackgroundService
    {
        private readonly ILogger<CleanupHostedService> _logger;
        public IServiceProvider Services { get; }

        public CleanupHostedService(IServiceProvider services, ILogger<CleanupHostedService> logger)
        {
            Services = services;
            _logger = logger;
        }

        int cleanupRepeatTime = 5;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            // When the timer should have no due-time, then do the work once now.
            await CleanupAsync();

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(cleanupRepeatTime));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await CleanupAsync();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        private async Task CleanupAsync()
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedItemService =
                    scope.ServiceProvider
                        .GetRequiredService<IItemService>();

                await scopedItemService.CleanupAsync();
            }
        }
    }
}