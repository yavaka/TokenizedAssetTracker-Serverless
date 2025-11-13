using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using TokenizedAssetTracker.Models;
using TokenizedAssetTracker.Services.EventPublisher;

namespace TokenizedAssetTracker.Functions.Http;

public class BlockchainEventIngestorFunction(
    ILogger<BlockchainEventIngestorFunction> logger,
    IEventPublisherService eventPublisherService)
{
    private readonly ILogger<BlockchainEventIngestorFunction> _logger = logger;
    private readonly IEventPublisherService _eventPublisherService = eventPublisherService;

    [Function(nameof(BlockchainEventIngestorFunction))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        var eventDataModel = await req.ReadFromJsonAsync<BlockchainEventModel>();

        if (string.IsNullOrEmpty(eventDataModel!.AssetId))
        {
            return new BadRequestObjectResult("AssetId is required");
        }

        var result = await _eventPublisherService.PublishEventAsync(eventDataModel);

        _logger.LogInformation("Blockchain Event Ingestor function processed a request.");
        return new AcceptedResult("blockchain-events-queue", result);
    }
}
