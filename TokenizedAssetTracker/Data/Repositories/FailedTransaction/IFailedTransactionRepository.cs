using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Data.Repositories.FailedTransaction;

public interface IFailedTransactionRepository
{
    Task SaveAsync(BlockchainEventModel model);
}
