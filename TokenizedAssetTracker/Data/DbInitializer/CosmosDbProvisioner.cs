using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TokenizedAssetTracker.Configurations.Options;

namespace TokenizedAssetTracker.Data.DbInitializer;

internal class CosmosDbProvisioner(
    ILogger<CosmosDbProvisioner> logger, 
    IOptions<CosmosDbOptions> cosmosDbOptions, 
    CosmosClient cosmosClient) : ICosmosDbProvisioner
{
    private readonly ILogger<CosmosDbProvisioner> _logger = logger;
    private readonly CosmosDbOptions _cosmosDbOptions = cosmosDbOptions.Value;
    private readonly CosmosClient _cosmosClient = cosmosClient;

    public async Task EnsureDbAndContainerCreatedAsync()
    {
        var dbResponse = await this._cosmosClient.CreateDatabaseIfNotExistsAsync(this._cosmosDbOptions.DatabaseName);

        this._logger.LogInformation(
            "Ensure database '{DatabaseName}' - StatusCode: {StatusCode}, ActivityId: {ActivityId}, RequestCharge: {RequestCharge}",
            this._cosmosDbOptions.DatabaseName,
            dbResponse.StatusCode,
            dbResponse.ActivityId,
            dbResponse.RequestCharge);

        var containerResponse = await this._cosmosClient
            .GetDatabase(this._cosmosDbOptions.DatabaseName)
            .CreateContainerIfNotExistsAsync(
                new ContainerProperties
                {
                    Id = this._cosmosDbOptions.ContainerName,
                    PartitionKeyPath = "/AssetId"
                });

        this._logger.LogInformation(
            "Ensure container '{ContainerName}' in database '{DatabaseName}' - StatusCode: {StatusCode}, ActivityId: {ActivityId}, RequestCharge: {RequestCharge}, PartitionKeyPath: {PartitionKeyPath}",
            this._cosmosDbOptions.ContainerName,
            this._cosmosDbOptions.DatabaseName,
            containerResponse.StatusCode,
            containerResponse.ActivityId,
            containerResponse.RequestCharge,
            containerResponse.Resource?.PartitionKeyPath);
    }
}
