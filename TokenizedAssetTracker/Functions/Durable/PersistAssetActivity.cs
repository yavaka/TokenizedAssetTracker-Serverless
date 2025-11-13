using Microsoft.Azure.Functions.Worker;
using TokenizedAssetTracker.Models;
using TokenizedAssetTracker.Services.Asset;

namespace TokenizedAssetTracker.Functions.Durable;

public class PersistAssetActivity(IAssetService assetService)
{
    private readonly IAssetService _assetService = assetService;

    [Function(nameof(PersistAssetActivity))]
    public async Task RunActivityAsync([ActivityTrigger] BlockchainEventModel eventData)
        => await _assetService.ProcessAssetTransferAsync(eventData);
}