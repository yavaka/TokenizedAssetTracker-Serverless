using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Data.Repositories.Transaction;

public interface ITransactionRepository
{
    Task SaveAsync(BlockchainEventModel model);
}
