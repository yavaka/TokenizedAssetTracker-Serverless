using Microsoft.Extensions.Logging;
using TokenizedAssetTracker.Data.Repositories.FailedTransaction;
using TokenizedAssetTracker.Models;

namespace TokenizedAssetTracker.Services.DlqHandling;

internal class DlqHandlingService(
    ILogger<DlqHandlingService> logger,
    IFailedTransactionRepository failedTransactionRepository) : IDlqHandlingService
{
    private readonly ILogger<DlqHandlingService> _logger = logger;
    private readonly IFailedTransactionRepository _failedTransactionRepository = failedTransactionRepository;

    public async Task ArchivePoisonedBlockchainEventAsync(BlockchainEventModel eventData)
        => await this._failedTransactionRepository.SaveAsync(eventData);

    public async Task SendFailureAlertAsync(BlockchainEventModel eventData)
    {
        this._logger.LogInformation("Sending Alert to the operations team");

        await Task.Delay(5000);

        this._logger.LogInformation("Alert sent to the operations team");
    }
}
