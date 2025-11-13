using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TokenizedAssetTracker.Configurations.Options;
using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Data.Repositories.FailedTransaction;

internal class FailedTransactionRepository(
    ILogger<FailedTransactionRepository> logger,
    IOptions<CosmosDbOptions> cosmoDbOptions,
    CosmosClient cosmosClient) : IFailedTransactionRepository
{
    private readonly ILogger<FailedTransactionRepository> _logger = logger;
    private readonly CosmosDbOptions _cosmoDbOptions = cosmoDbOptions.Value;
    private readonly CosmosClient _cosmosClient = cosmosClient;

    public async Task SaveAsync(BlockchainEventModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var container = this._cosmosClient
            .GetContainer(this._cosmoDbOptions.DatabaseName, this._cosmoDbOptions.FailedTxContainerName);

        var itemResponse = await container.CreateItemAsync(model, new PartitionKey(model.AssetId));

        // Log key details from the Cosmos DB response for diagnostics
        _logger.LogInformation(
            "Saved failed TX to Cosmos DB. Id: {Id}, Container: {Container}, StatusCode: {StatusCode}, ActivityId: {ActivityId}, RequestCharge: {RequestCharge}, ETag: {ETag}",
            model.Id,
            this._cosmoDbOptions.TxContainerName,
            itemResponse.StatusCode,
            itemResponse.ActivityId,
            itemResponse.RequestCharge,
            itemResponse.ETag);
    }
}
