namespace TokenizedAssetTracker.Configurations.Options;

public class CosmosDbOptions
{
    // Must match the key name in your configuration settings (local.settings.json)
    public const string CosmosDb = "CosmosDbSettings";

    // These property names must match the keys inside the "CosmosDbSettings" section in your configuration.
    public string DatabaseName { get; set; } = string.Empty;
    public string TxContainerName { get; set; } = string.Empty;
    public string FailedTxContainerName { get; set; } = string.Empty;
}
