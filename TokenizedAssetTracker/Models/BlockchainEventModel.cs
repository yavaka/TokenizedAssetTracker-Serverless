using Newtonsoft.Json;

namespace TokenizedAssetTracker.Models;

public class BlockchainEventModel
{
    // Cosmos DB requires a JSON property named "id". Map the C# Id to "id".
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string AssetId { get; set; } = default!;
    public string TxHash { get; set; } = default!;
    public string NewOwnerAddress { get; set; } = default!;
    public string EventType { get; set; } = default!;
}
