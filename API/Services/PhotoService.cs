using API.DTO;
using API.Helpers;
using API.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly BlobContainerClient _containerClient;

        public PhotoService(IOptions<AzureStorageSettings> azureStorageSettings)
        {
            _azureStorageSettings = azureStorageSettings.Value;            

            var blobServiceClient = new BlobServiceClient(_azureStorageSettings.ConnectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(_azureStorageSettings.ContainerName);
        }

        public async Task<bool> DeletePhotoAsync(string publicId)
        {
            var blobClient = _containerClient.GetBlobClient(publicId);

            var response = await blobClient.DeleteIfExistsAsync();

            return response.Value;
        }

        public async Task<PhotoResponseDto> UploadPhotoAsync(IFormFile file)
        {
            if (file is null || file.Length == 0)
                throw new Exception("Invalid file");

            // Generate unique name
            var fileName = $"{Guid.NewGuid()}-{file.FileName}";

            var blobClient = _containerClient.GetBlobClient(fileName);

            using var stream = file.OpenReadStream();

            var result = await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                }
            });

            return new PhotoResponseDto { Url = blobClient.Uri.ToString(), FileName = fileName };
        }
    }
}
