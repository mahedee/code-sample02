using Azure.Storage.Blobs;

namespace DMS.Api.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _container;

        public BlobStorageService(IConfiguration config)
        {
            _container = new BlobContainerClient(
                config["AzureBlob:ConnectionString"],
                config["AzureBlob:ContainerName"]);

            _container.CreateIfNotExists();
        }

        public async Task UploadAsync(string fileName, Stream stream)
        {
            var blob = _container.GetBlobClient(fileName);
            await blob.UploadAsync(stream, overwrite: true);
        }

        public async Task<Stream> DownloadAsync(string fileName)
        {
            var blob = _container.GetBlobClient(fileName);
            var result = await blob.DownloadAsync();
            return result.Value.Content;
        }

        public async Task DeleteAsync(string fileName)
        {
            var blob = _container.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }
    }
}