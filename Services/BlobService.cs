using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace GamifyBackEnd.Services
{

    public class BlobService
    {
        private readonly string _storageAccountName;
        private readonly string _storageAccountKey;
        private readonly string _containerName;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(IConfiguration config)
        {
            _storageAccountName = Environment.GetEnvironmentVariable("AZUREBLOB_ACCOUNTNAME");//found in the .env
            _storageAccountKey = Environment.GetEnvironmentVariable("AZUREBLOB_ACCOUNTKEY");//found in the .env
            _containerName = Environment.GetEnvironmentVariable("AZUREBLOB_CONTAINERNAME"); //found in the .env
            var connectionString = Environment.GetEnvironmentVariable("AZUREBLOB_CONNECTIONSTRING"); //found in the .env
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        //For allowing test variables during unit tests.
        public BlobService(BlobServiceClient blobServiceClient, string containerName)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = containerName;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string contentEncoding = null)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.None);

            var blobClient = containerClient.GetBlobClient(fileName);

            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = contentType
            };

            if (!string.IsNullOrEmpty(contentEncoding))
            {
                blobHttpHeaders.ContentEncoding = contentEncoding;
            }

            await blobClient.UploadAsync(fileStream, new Azure.Storage.Blobs.Models.BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders,
                TransferOptions = new StorageTransferOptions
                {
                    // This much should be enough for most webGL builds
                    MaximumTransferSize = 4 * 1024 * 1024,
                    InitialTransferSize = 4 * 1024 * 1024
                }
            });

            return blobClient.Uri.ToString();
        }

    }
}