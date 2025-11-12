using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TokenizedAssetTracker.Services.EventPublisher;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    // Register the core Azure Service Client
    .AddAzureClients(clientBuilder =>
    {
        // The key here is the full path "Values:AzureWebJobsStorage"
        // The configuration system automatically retrieves the value "UseDevelopmentStorage=true"
        clientBuilder.AddQueueServiceClient(
            builder.Configuration["AzureWebJobsStorage"]
        );
    });

builder.Services.AddSingleton<IEventPublisherService, AzureQueueEventPublisherService>();

builder.Build().Run();
