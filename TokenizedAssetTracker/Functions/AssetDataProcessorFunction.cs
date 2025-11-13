using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TokenizedAssetTracker.Models;
using TokenizedAssetTracker.Services.Asset;

namespace TokenizedAssetTracker.Functions;

public class AssetDataProcessorFunction(
    ILogger<AssetDataProcessorFunction> logger,
    IAssetService assetService)
{
    private readonly ILogger<AssetDataProcessorFunction> _logger = logger;
    private readonly IAssetService _assetService = assetService;

    [Function(nameof(AssetDataProcessorFunction))]
    public async Task Run([QueueTrigger("blockchain-events-queue", Connection = "AzureWebJobsStorage")] BlockchainEventModel eventData)
    {
        if (eventData == null)
        {
            _logger.LogError("Failed to deserialize queue message into BlockchainEventModel.");
            return; // Optionally re-throw or handle as a logic failure
        }

        _logger.LogInformation("Queue trigger received event for Asset ID: {AssetId}", eventData.AssetId);
        await this._assetService.ProcessAssetTransferAsync(eventData);
    }
}