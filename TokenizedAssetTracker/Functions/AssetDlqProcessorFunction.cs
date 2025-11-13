using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TokenizedAssetTracker.Models;
using TokenizedAssetTracker.Services.DlqHandling;

namespace TokenizedAssetTracker.Functions;

public class AssetDlqProcessorFunction(
    ILogger<AssetDlqProcessorFunction> logger,
    IDlqHandlingService dlqHandlingService)
{
    private readonly ILogger<AssetDlqProcessorFunction> _logger = logger;
    private readonly IDlqHandlingService _dlqHandlingService = dlqHandlingService;

    [Function(nameof(AssetDlqProcessorFunction))]
    public async Task RunAsync(
        [QueueTrigger("blockchain-events-queue-poison", Connection = "AzureWebJobsStorage")] BlockchainEventModel eventData)
    {
        try
        {
            this._logger.LogWarning("Processing message from dead-letter queue for Asset ID: {AssetId}", eventData.AssetId);

            await this._dlqHandlingService.ArchivePoisonedBlockchainEventAsync(eventData);

            await this._dlqHandlingService.SendFailureAlertAsync(eventData);

            this._logger.LogInformation("DLQ message successfully archived and reported for Asset ID: {AssetId}. Message deleted from DLQ.", eventData.AssetId);
        }
        catch (Exception e)
        {
            this._logger.LogCritical(
                e, 
                "CRITICAL: Failed to archive or alert on DLQ message for Asset ID: {AssetId}. The message will REMAIN in the poison queue.",
                eventData?.AssetId);

            throw;
        }
    }
}