using MediatR;
using SubscriptionPlatform.Application.Features.Inventory.Commands.CheckStock;

namespace SubscriptionPlatform.Worker.BackgroundServices
{
    public class StockWatcherWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StockWatcherWorker> _logger;

        public StockWatcherWorker(IServiceProvider serviceProvider, ILogger<StockWatcherWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Stok kontrolü tetikleniyor...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    
                    await mediator.Send(new CheckLowStockCommand(), stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}