using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TokenizedAssetTracker.Services.Asset;
using TokenizedAssetTracker.Services.EventPublisher;

var builder = FunctionsApplication.CreateBuilder(args);

var unusedTypeReference = typeof(IAssetService);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    // Register the core Azure Service Client
    .AddAzureClients(clientBuilder =>
    {
        // The configuration system automatically retrieves the value "UseDevelopmentStorage=true"
        clientBuilder.AddQueueServiceClient(
            builder.Configuration["AzureWebJobsStorage"]
        );
    });

builder.Services
    .AddSingleton<IAssetService, AssetDataService>()
    .AddSingleton<IEventPublisherService, AzureQueueEventPublisherService>();

builder.Build().Run();
