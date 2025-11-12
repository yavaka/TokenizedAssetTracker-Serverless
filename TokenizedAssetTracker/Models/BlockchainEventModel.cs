namespace TokenizedAssetTracker.Models;

public class BlockchainEventModel
{
    public Guid AssetId { get; set; }
    public string TxHash { get; set; } = default!;
    public string NewOwnerAddress { get; set; } = default!;
    public string EventType { get; set; } = default!;
}
