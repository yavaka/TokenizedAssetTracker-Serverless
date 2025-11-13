using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Functions.Durable;

public class AssetTransferOrchestrator
{
    [Function(nameof(AssetTransferOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context, BlockchainEventModel eventData) 
        => await context.CallActivityAsync(nameof(PersistAssetActivity), eventData);
}
