using Microsoft.Extensions.Logging;
using TokenizedAssetTracker.Data.Repositories.Transaction;
using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Services.Asset;

public class AssetDataService(
    ILogger<AssetDataService> logger,
    ITransactionRepository transactionRepository) : IAssetService
{
    private readonly ILogger<AssetDataService> _logger = logger;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;

    public async Task ProcessAssetTransferAsync(BlockchainEventModel eventData)
    {
        // Log a message indicating the start of the heavy workload
        _logger.LogInformation("Starting complex ledger update for Asset ID: {AssetId}", eventData.AssetId);

        await _transactionRepository.SaveTransactionAsync(eventData);

        // Simulate a heavy workload with a delay
        await Task.Delay(5000);
       

        // Log a message indicating the completion of the heavy workload
        _logger.LogWarning("Asset ID: {AssetId} successfully processed. New owner: {NewOwnerAddress}", eventData.AssetId, eventData.NewOwnerAddress);
    }
}
