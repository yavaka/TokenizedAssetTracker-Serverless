using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Data.Repositories.Transaction;

public interface ITransactionRepository
{
    Task SaveTransactionAsync(BlockchainEventModel model);
}
