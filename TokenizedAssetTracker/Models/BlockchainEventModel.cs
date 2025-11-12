namespace TokenizedAssetTracker.Models;

public class BlockchainEventModel
{
    public string AssetId { get; set; } = default!;
    public string TxHash { get; set; } = default!;
    public string NewOwnerAddress { get; set; } = default!;
    public string EventType { get; set; } = default!;
}
