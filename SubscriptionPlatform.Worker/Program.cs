using SubscriptionPlatform.Infrastructure; // Extension metodun için
using SubscriptionPlatform.Application.Features.Inventory.Commands.CheckStock; 
using SubscriptionPlatform.Worker.BackgroundServices;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAutoMapper(typeof(CheckLowStockCommand).Assembly);

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CheckLowStockCommand).Assembly);
});
builder.Services.AddHostedService<StockWatcherWorker>();

var host = builder.Build();
host.Run();

