using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TokenizedAssetTracker.Configurations.Options;
using TokenizedAssetTracker.Data.DbInitializer;
using TokenizedAssetTracker.Data.Repositories.FailedTransaction;
using TokenizedAssetTracker.Data.Repositories.Transaction;
using TokenizedAssetTracker.Services.Asset;
using TokenizedAssetTracker.Services.DlqHandling;
using TokenizedAssetTracker.Services.EventPublisher;

var builder = FunctionsApplication.CreateBuilder(args);

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

builder.Services.Configure<CosmosDbOptions>(builder.Configuration.GetSection(CosmosDbOptions.CosmosDb));

builder.Services
    .AddSingleton(new CosmosClient(builder.Configuration["CosmosDbConnectionString"]))
    .AddSingleton<ICosmosDbProvisioner, CosmosDbProvisioner>()
    .AddSingleton<ITransactionRepository, TransactionRepository>()
    .AddSingleton<IFailedTransactionRepository, FailedTransactionRepository>();

builder.Services
    .AddSingleton<IAssetService, AssetDataService>()
    .AddSingleton<IEventPublisherService, AzureQueueEventPublisherService>()
    .AddSingleton<IDlqHandlingService, DlqHandlingService>();

var host = builder.Build();

using var scope = host.Services.CreateScope();
// Ensure Cosmos DB and Container are created
await scope.ServiceProvider.GetRequiredService<ICosmosDbProvisioner>()
                     .EnsureDbAndContainerCreatedAsync();

host.Run();
