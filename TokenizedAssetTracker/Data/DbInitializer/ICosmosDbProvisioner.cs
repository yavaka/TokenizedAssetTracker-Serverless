namespace TokenizedAssetTracker.Data.DbInitializer;

public interface ICosmosDbProvisioner
{
    Task EnsureDbAndContainerCreatedAsync();
}
