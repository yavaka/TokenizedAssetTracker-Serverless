using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Services.EventPublisher;

public class AzureQueueEventPublisherService(
    ILogger<AzureQueueEventPublisherService> logger,
    QueueServiceClient queueServiceClient) : IEventPublisherService
{
    private readonly ILogger<AzureQueueEventPublisherService> _logger = logger;
    private readonly QueueServiceClient _queueServiceClient = queueServiceClient;

    private const string QUEUE_NAME = "blockchain-events-queue";

    public async Task<string> PublishEventAsync(BlockchainEventModel eventData)
    {
        try
        {
            var payload = JsonSerializer.Serialize(eventData);

            var client = await this._queueServiceClient.CreateQueueAsync(QUEUE_NAME);

            await client.Value.SendMessageAsync(payload);

            _logger.LogInformation("Event published to queue for AssetId: {AssetId}", eventData.AssetId);
            return "Event published successfully!";
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Creating Queue failed for {AssetId}", eventData.AssetId);
        }
        return "Creating Queue failed!";
    }
}
