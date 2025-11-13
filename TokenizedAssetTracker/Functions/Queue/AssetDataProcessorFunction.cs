using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using TokenizedAssetTracker.Functions.Durable;
using TokenizedAssetTracker.Models;
using TokenizedAssetTracker.Services.Asset;

namespace TokenizedAssetTracker.Functions.Queue;

public class AssetDataProcessorFunction(
    ILogger<AssetDataProcessorFunction> logger,
    IAssetService assetService)
{
    private readonly ILogger<AssetDataProcessorFunction> _logger = logger;
    private readonly IAssetService _assetService = assetService;

    [Function(nameof(AssetDataProcessorFunction))]
    public async Task Run(
        [QueueTrigger("blockchain-events-queue", Connection = "AzureWebJobsStorage")] BlockchainEventModel eventData, 
        [DurableClient] DurableTaskClient durableTaskClient)
    {
        if (eventData == null)
        {
            this._logger.LogError("Failed to deserialize queue message into BlockchainEventModel.");
            return; 
        }

        this._logger.LogInformation("Queue trigger received event for Asset ID: {AssetId}", eventData.AssetId);
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(
            nameof(AssetTransferOrchestrator), 
            eventData,
            new StartOrchestrationOptions(eventData.AssetId));
    }
}