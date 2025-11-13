using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Services.DlqHandling;

public interface IDlqHandlingService
{
    Task ArchivePoisonedBlockchainEventAsync(BlockchainEventModel eventData);
    Task SendFailureAlertAsync(BlockchainEventModel eventData);
}
