using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Services.EventPublisher;

public interface IEventPublisherService
{
    Task<string> PublishEventAsync(BlockchainEventModel eventData);
}
