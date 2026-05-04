namespace API.Helpers
{
    public class AzureStorageSettings
    {
        public required string ConnectionString { get; set; }
        public required string ContainerName { get; set; }
    }
}
