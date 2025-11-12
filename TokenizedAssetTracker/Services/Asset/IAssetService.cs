using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Services.Asset;

public interface IAssetService
{
    Task ProcessAssetTransferAsync(BlockchainEventModel eventData);
}
